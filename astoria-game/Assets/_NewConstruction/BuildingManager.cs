using System;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    BuildingCore core;

    public float tileSize = 1.5f;

    public void PlayerPlaceTile(Vector2Int position) {
        if (!core.CanPlaceTile(position, new FoundationTile())) {
            core.PlaceTile(position, new FoundationTile());
            return;
        }
    }

    public void PlayerRemoveTile(Vector2Int position) {
        if (!core.CanRemoveTile(position)) {
            core.RemoveTile(position);
            return;
        }
    }

    public void PlayerPlaceWall(Vector2Int position) {

    }

}
