using UnityEngine;
using System;
using UnityEngine.VFX;

public class Tornado : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private float alpha = 0.4f;
    [SerializeField] private AnimationCurve lifetimeAlphaCurve;
    [SerializeField] private VisualEffect visualEffect;
    [SerializeField] private LayerMask groundLayerMask;

    private float spawnTime;

    private void Start()
    {
        spawnTime = Time.time;
    }

    private void Update()
    {
        if (Time.time - spawnTime > lifetime)
        {
            Destroy(gameObject);
            return;
        }

        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        float elapsedTime = Time.time - spawnTime;
        float alphaValue = lifetimeAlphaCurve.Evaluate(elapsedTime / lifetime) * alpha;
        visualEffect.SetFloat("alpha", alphaValue);

        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 1000, Vector3.down, out hit, Mathf.Infinity, groundLayerMask))
        {
            transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
        }
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void SetLifetime(float newLifetime)
    {
        lifetime = newLifetime;
        spawnTime = Time.time;
    }

}