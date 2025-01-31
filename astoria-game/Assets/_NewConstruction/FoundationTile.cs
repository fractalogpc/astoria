using UnityEngine;

public class FoundationTile : MonoBehaviour
{
    Vector2Int gridPosition;
    bool[,] points = new bool[3,3];

    public void TogglePoint(int x, int y, bool enabled) {

    }

    private bool IsLegalConfiguration(int x, int y, bool enabled) {
        return false;
    }

    private void SyncWithNeighbors() {

    }
}
