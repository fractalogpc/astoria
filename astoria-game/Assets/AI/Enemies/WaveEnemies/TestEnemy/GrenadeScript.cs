using UnityEngine;

public class GrenadeScript : MonoBehaviour
{
    [SerializeField] private float timeBeforeExplode;
    private float timeSinceInstantiate;
    public void Update() {
      timeSinceInstantiate += Time.deltaTime;
      if (timeSinceInstantiate > timeBeforeExplode) {
        Explode();
      }
    }

    private void Explode() {
      Destroy(this.gameObject);
    }
}
