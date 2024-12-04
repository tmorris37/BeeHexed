using UnityEngine;
using UnityEngine.Tilemaps;
using GridSystem;
using EnemyAndTowers;

public class TowerSelector : MonoBehaviour
{
    public Tilemap hexTilemap;
    public Tile highlightTile; // Assign a tile with a different color/shade in the Inspector
    public GameObject towerPrefab; // Assign your tower prefab in the Inspector

    public GridManager gridManager;

    public GameObject PulserPrefab;

    private Vector3Int lastHoveredTilePosition;
    private TileBase originalTile;
    private bool hasHoveredTile;
    private Vector3Int cellPosition;
    public bool DEBUG_MODE;

    /*void Update()
    {
        // Get the mouse position in world space
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        // Convert the mouse position to a cell position in the tilemap
        cellPosition = hexTilemap.WorldToCell(mouseWorldPos);
        //Debug.Log("Cellposition: " + cellPosition);
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
    }*/

    void Update()
{
    // Get the mouse position in world space
    Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    mouseWorldPos.z = 0;

    // Convert the mouse position to a cell position in the tilemap
    cellPosition = hexTilemap.WorldToCell(mouseWorldPos);

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
            hexTilemap.SetTile(cellPosition, highlightTile); // Highlight the current tile
            lastHoveredTilePosition = cellPosition;
            hasHoveredTile = true;
        }
    }

    // Handle case where mouse moves off the tilemap
    if (!hexTilemap.HasTile(cellPosition) && hasHoveredTile)
    {
        hexTilemap.SetTile(lastHoveredTilePosition, originalTile);
        hasHoveredTile = false;
    }
}


    public bool spawnTower(GameObject tower) {
      // Check for mouse click to place a tower
        if (hasHoveredTile)
        {
            //get the tile from grid manager
            //set it to whatever the tower is
            (int q, int r, int s) = this.gridManager.TileMapXYtoQRS(cellPosition.x, cellPosition.y);
            HexTile spot = this.gridManager.FetchTile(q, r, s);
            HexPosition towerComponent;
            GameObject t;
            if (!spot.getOccupied())
            {
                if (Input.GetMouseButtonUp(0))
                {
                    Vector3 towerPosition = hexTilemap.CellToWorld(lastHoveredTilePosition); // Adjust for tile center
                    t = Instantiate(tower, towerPosition, Quaternion.identity); // Spawn the tower at the tile position
                    towerComponent = t.GetComponent<HexPosition>();
                    towerComponent.gridManager = this.gridManager;
                    towerComponent.SetQRS(q, r, s);
                    spot.EnterTile(t);
                    if (DEBUG_MODE) {
                      Debug.Log("Tower cast at: " + q + r + s);
                    }
                    return true;
                }
            }
            return false;
        } 
        return false;
    }

    public bool castSpell(GameObject spell, SpellType spellType) {
      // Check for mouse click to place a tower
      if (spellType == SpellType.Hex) {
         if (hasHoveredTile) {
            (int q, int r, int s) = this.gridManager.XYtoQRS(cellPosition.x, cellPosition.y);
            HexTile spot = this.gridManager.FetchTile(q, r, s);
            GameObject t;
            if (!spot.getOccupied())
            {
                if (Input.GetMouseButtonUp(0))
                {
                  Vector3 spellPosition = hexTilemap.CellToWorld(lastHoveredTilePosition); // Adjust for tile center
                  t = Instantiate(spell, spellPosition, Quaternion.identity); // Spawn the 'spell' at the tile position
                  if (DEBUG_MODE) {
                    Debug.Log("Spell cast at: " + q + r + s);
                  }
                  return true;
                }
            }
          } 
       return false;
    } else {
      if (Input.GetMouseButtonUp(0)) {
        Vector3 spellPosition = Vector3.zero;
        GameObject t = Instantiate(spell, spellPosition, Quaternion.identity); // Spawn the 'spell' at 0,0,0
        return true;
      }
    }
    return false;
  }
}
