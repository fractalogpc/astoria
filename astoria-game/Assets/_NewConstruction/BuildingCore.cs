using System.Collections.Generic;
using UnityEngine;

public class BuildingCore
{
    Dictionary<Vector2Int, FoundationTile> placedTiles;

    public bool CanPlaceTile(Vector2Int position, FoundationTile tile) {

        return false;
    }

    public void PlaceTile(Vector2Int position, FoundationTile tile) {

    }

    private bool IsConnectedToCore(Vector2Int position) {
        return false;
    }

    private void SyncTileEdges(Vector2Int position, FoundationTile tile) {

    }

}