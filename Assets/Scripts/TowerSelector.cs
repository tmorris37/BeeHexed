using UnityEngine;
using UnityEngine.Tilemaps;

public class TowerSelector : MonoBehaviour
{
    public Tilemap hexTilemap;
    public Tile highlightTile; // Assign a tile with a different color/shade in the Inspector
    public GameObject towerPrefab; // Assign your tower prefab in the Inspector

    private Vector3Int lastHoveredTilePosition;
    private TileBase originalTile;
    private bool hasHoveredTile;

    void Update()
    {
        // Get the mouse position in world space
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        // Convert the mouse position to a cell position in the tilemap
        Vector3Int cellPosition = hexTilemap.WorldToCell(mouseWorldPos);

        // If the mouse is over a new cell
        if (cellPosition != lastHoveredTilePosition)
        {
            // Reset the last hovered tile if there was one
            if (hasHoveredTile)
            {
                hexTilemap.SetTile(lastHoveredTilePosition, originalTile); // Restore the original tile
                hasHoveredTile = false;
            }

            // Set the new tile to the highlight tile
            if (hexTilemap.HasTile(cellPosition))
            {
                originalTile = hexTilemap.GetTile(cellPosition); // Save the original tile
                hexTilemap.SetTile(cellPosition, highlightTile);
                lastHoveredTilePosition = cellPosition;
                hasHoveredTile = true;
            }
        }

        // Check for mouse click to place a tower
        if (Input.GetMouseButtonDown(0) && hasHoveredTile)
        {
            Vector3 towerPosition = hexTilemap.CellToWorld(lastHoveredTilePosition) + new Vector3(0, 0.5f, 0); // Adjust for tile center
            Instantiate(towerPrefab, towerPosition, Quaternion.identity); // Spawn the tower at the tile position
        }
    }
}
