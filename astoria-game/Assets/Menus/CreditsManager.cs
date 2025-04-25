using UnityEngine;

public class CreditsManager : MonoBehaviour
{

    [System.Serializable]
    private struct CreditsData
    {
        public string name; // Name of the person or entity credited
        public string role; // Role or contribution of the person or entity
    }
    
    [SerializeField] private GameObject creditsPrefab; // Prefab for a single credit entry
    [SerializeField] private CreditsData[] creditsData; // Array of credit data

    public void StartCredits()
    {
        
    }
    

}
