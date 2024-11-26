using System.Collections.Generic;
using UnityEngine;
public class ProjectileManager : Singleton<ProjectileManager>
{
    public int SamplesPerFixedUpdate = 5;
    public float ProjectileLifetime = 20f;
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
        
        public ProjectileHitHandler Callback;
	
        public Projectile(float timeToLive, ProjectileHitHandler callback, float damage, float mass, Vector3 startPosition, Vector3 startVelocity, Aerodynamics aerodynamics) {
            Damage = damage;
            Mass = mass;
            Position = startPosition;
            Velocity = startVelocity;
            Aerodynamics = aerodynamics;
            TimeToLive = timeToLive;
            Callback = callback;    
        }
        public RaycastHit TickProjectile(float deltaTime) {
            // Calculate drag force direction
            Vector3 dragForceDirection = -Velocity.normalized;
            // Calculate drag force magnitude
            float dragForceMagnitude = 0.5f * Aerodynamics.DragCoefficient * Aerodynamics.CrossSectionalArea * Aerodynamics._airDensity * Velocity.sqrMagnitude;
            // Compute drag acceleration
            Vector3 dragAcceleration = dragForceDirection * (dragForceMagnitude / Mass);

            // Update velocity considering both drag and gravity
            Velocity += (dragAcceleration + Physics.gravity) * deltaTime;

            // Update position based on new velocity
            Position += Velocity * deltaTime;
            
            // Handle collision
            Physics.Raycast(Position, Velocity.normalized, out RaycastHit hit, Velocity.magnitude * deltaTime);
            Debug.DrawLine(Position, Position + Velocity * deltaTime, Color.red, 0.1f);
            
            TimeToLive -= deltaTime;
            
            return hit;
        }
    }
    public void FireProjectile(float damage, float mass, Vector3 startPosition, Vector3 startVelocity, Aerodynamics aerodynamics, ProjectileHitHandler hitCallback) {
        _projectilesToTick.Add(new Projectile(ProjectileLifetime, hitCallback, damage, mass, startPosition, startVelocity, aerodynamics));
        if (_projectilesToTick.Count > 800) {
            Debug.Log($"GunLogic: Holds {_projectilesToTick.Count} projectiles. This could be a performance issue. Remind matthew to multithread this.");
        }
    }
    private void FixedUpdate() {
        for (int i = _projectilesToTick.Count - 1; i >= 0; i--) {
            Projectile projectile = _projectilesToTick[i];
            for (int j = 0; j < SamplesPerFixedUpdate; j++) {
                RaycastHit hit = projectile.TickProjectile(Time.fixedDeltaTime / SamplesPerFixedUpdate);
                if (hit.collider != null) {
                    projectile.Callback(hit);
                    _projectilesToTick.Remove(projectile);
                    break;
                }
                if (projectile.TimeToLive <= 0) {
                    Debug.Log("Projectile expired");
                    _projectilesToTick.Remove(projectile);
                    break;
                }
            }
        }
    }
}
