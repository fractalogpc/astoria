using UnityEngine;

public class Animal : BasicAI
{
    [SerializeField] private float waterMax;
    [SerializeField] private float waterLimit;
    [SerializeField] private float foodMax;
    [SerializeField] private float foodLimit;
    private float currentWater;
    private float currentFood;
}