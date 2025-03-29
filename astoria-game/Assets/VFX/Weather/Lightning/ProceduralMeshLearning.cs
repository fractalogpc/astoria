using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using Unity.Mathematics;
using System.Threading;

public class ProceduralMeshLearning : MonoBehaviour
{
    [SerializeField] private AnimationCurve anim;
    [SerializeField] private Material mat;
    [SerializeField] private float timer;
    [SerializeField] private float maxTimer;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            timer = maxTimer;
        }

        if (timer >= 0) {
            float norm = 1 - (timer / maxTimer);
            
            norm = anim.Evaluate(norm);

            mat.SetFloat("_Brightness", norm * 1000);

            timer -= Time.deltaTime;
        }
    }
}
