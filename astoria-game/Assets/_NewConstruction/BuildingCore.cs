using System.Collections.Generic;
using UnityEngine;

public class BuildingCore
{
    Dictionary<Vector2Int, FoundationTile> placedTiles;

    public bool CanPlaceTile(Vector2Int position, FoundationTile tile) {
        // Check if the tile is already placed
        if (placedTiles.ContainsKey(position)) {
            return false;
        }

        // Check if the tile is connected to the core
        if (!IsConnectedToCore(position)) {
            return false;
        }

        return false;
    }

    public void PlaceTile(Vector2Int position, FoundationTile tile) {

    }

    public bool CanRemoveTile(Vector2Int position) {
        return false;
    }

    public void RemoveTile(Vector2Int position) {

    }

    private bool IsConnectedToCore(Vector2Int position) {
        // Assume the core is at (0,0)
        
        return false;
    }

    private void SyncTileEdges(Vector2Int position, FoundationTile tile) {

    }

}