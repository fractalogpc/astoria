using System.Collections.Generic;
using UnityEngine;
public class ProjectileManager : Singleton<ProjectileManager>
{
    [Tooltip("The amount of times the bullet path is stepped through per fixed update. If this is too low, bullets will follow a stepped arc.")]
    public int SamplesPerFixedUpdate = 5;
    [Tooltip("The lifetime of a projectile in seconds. The projectile will be destroyed after this time.")]
    public float ProjectileLifetime = 20f;
    [Tooltip("The prefab to use for the projectile. This will be lerped to the actual position of the projectile every frame.")]
    public GameObject BulletPrefab;
    [Tooltip("The prefab to use for the hit effect. This will be spawned at the hit point of the projectile.")]
    public GameObject HitEffectPrefab;
    [Tooltip("The transform to use as the start position for the cosmetic bullet. This can be used to make the projectile appear to come from a different position.")]
    public Transform BulletStart;

    public LayerMask ignoreLayers = ~0; // Default to ignore nothing
    
    private List<Projectile> _projectilesToTick = new List<Projectile>();
    public delegate void ProjectileHitHandler(RaycastHit hit);

    public struct Aerodynamics {
        public float DragCoefficient;
        /// <summary>
        /// The cross-sectional area of the projectile in meters squared.
        /// </summary>
        public float CrossSectionalArea;
        /// <summary>
        /// The air density in kg/m^3.
        /// </summary>
        public float _airDensity;
        public Aerodynamics(float dragCoefficient, float crossSectionalAreaMetersSquared, float airDensity) {
            DragCoefficient = dragCoefficient;
            CrossSectionalArea = crossSectionalAreaMetersSquared;
            _airDensity = airDensity;
        }
    }

    public class Projectile
    {
        public float Damage;
        /// <summary>
        /// Mass of the projectile in kilograms.
        /// </summary>
        public float Mass;
        /// <summary>
        /// The current position of the projectile.
        /// </summary>
        public Vector3 Position;
        /// <summary>
        /// The current velocity vector of the projectile.
        /// </summary>
        public Vector3 Velocity;
        /// <summary>
        /// The aerodynamics of the projectile.
        /// </summary>
        public Aerodynamics Aerodynamics;
        /// <summary>
        /// Seconds the projectile has left to live.
        /// </summary>
        public float TimeToLive;

        private LayerMask _ignoreLayers;

        public Transform Renderer;
        
        public ProjectileHitHandler Callback;
	
        public Projectile(float timeToLive, ProjectileHitHandler callback, float damage, float mass, Vector3 startPosition, Vector3 startVelocity, Aerodynamics aerodynamics, LayerMask ignoreLayers) {
            Damage = damage;
            Mass = mass;
            Position = startPosition;
            Velocity = startVelocity;
            Aerodynamics = aerodynamics;
            TimeToLive = timeToLive;
            Callback = callback;
            _ignoreLayers = ignoreLayers;
            Renderer = Instantiate(Instance.BulletPrefab, Instance.BulletStart.position, Quaternion.identity).transform;
        }
        public RaycastHit TickProjectile(float deltaTime) {
            // Handle collision
            Vector3 normVelocity = Vector3.Normalize(Velocity);
            Physics.Raycast(Position, normVelocity, out RaycastHit hit, Velocity.magnitude * deltaTime, _ignoreLayers);
            // Debug.DrawLine(Position, Position + Velocity * deltaTime, Color.red, 0.1f);
            
            // Calculate drag force direction
            Vector3 dragForceDirection = -normVelocity;
            // Calculate drag force magnitude
            float dragForceMagnitude = 0.5f * Aerodynamics.DragCoefficient * Aerodynamics.CrossSectionalArea * Aerodynamics._airDensity * Velocity.sqrMagnitude;
            // Compute drag acceleration
            Vector3 dragAcceleration = dragForceDirection * (dragForceMagnitude / Mass);

            // Update velocity considering both drag and gravity
            Velocity += (dragAcceleration + Physics.gravity) * deltaTime;

            // Update position based on new velocity
            Position += Velocity * deltaTime;

            // Set renderer position
            Renderer.position = Vector3.Lerp(Renderer.position, Position, 0.5f);
            // Set renderer rotation
            Renderer.rotation = Quaternion.LookRotation(normVelocity);
            
            TimeToLive -= deltaTime;
            
            return hit;
        }
    }

    public void FireProjectile(float damage, float mass, Vector3 startPosition, Vector3 startVelocity, Aerodynamics aerodynamics, ProjectileHitHandler hitCallback) {
        _projectilesToTick.Add(new Projectile(ProjectileLifetime, hitCallback, damage, mass, startPosition, startVelocity, aerodynamics, ignoreLayers));
        if (_projectilesToTick.Count > 800) {
            Debug.Log($"ProjectileManager: Holds {_projectilesToTick.Count} projectiles. This could be a performance issue. Remind matthew to multithread this.");
        }
    }

    private void SpawnHitEffect(RaycastHit hit) {
        Vector3 normal = hit.normal;
        GameObject hitEffect = Instantiate(HitEffectPrefab, hit.point, Quaternion.LookRotation(normal));
        Destroy(hitEffect, 5f);
    }

    private void FixedUpdate() {
        for (int i = _projectilesToTick.Count - 1; i >= 0; i--) {
            Projectile projectile = _projectilesToTick[i];
            for (int j = 0; j < SamplesPerFixedUpdate; j++) {
                RaycastHit hit = projectile.TickProjectile(Time.fixedDeltaTime / SamplesPerFixedUpdate);
                if (hit.collider != null) {
                    projectile.Callback(hit);
                    SpawnHitEffect(hit);
                    Destroy(projectile.Renderer.gameObject);
                    _projectilesToTick.Remove(projectile);
                    break;
                }
                if (projectile.TimeToLive <= 0) {
                    Debug.Log("Projectile expired");
                    Destroy(projectile.Renderer.gameObject);
                    _projectilesToTick.Remove(projectile);
                    break;
                }
            }
        }
    }
}
