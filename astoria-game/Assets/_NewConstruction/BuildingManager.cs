using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    BuildingCore core;

    public void PlayerPlaceTile(Vector2Int position) {
        if (!core.CanPlaceTile(position, new FoundationTile())) {
            return;
        }
    }

    public void PlayerRemoveTile(Vector2Int position) {
        if (!core.CanRemoveTile(position)) {
            return;
        }
    }

    public void PlayerPlaceWall(Vector2Int position) {
        
    }

}
