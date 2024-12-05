using UnityEngine;
using UnityEngine.Tilemaps;
using GridSystem;
using EnemyAndTowers;

public class TowerSelector : MonoBehaviour
{
    public Tilemap hexTilemap;
    public TileSpawner tiles;
    public Tile highlightTile; // Assign a tile with a different color/shade in the Inspector
    public GameObject towerPrefab; // Assign your tower prefab in the Inspector

    public GridManager gridManager;

    public GameObject PulserPrefab;

    private Vector3Int lastHoveredTilePosition;
    private TileBase originalTile;
    private Vector3Int origTilePos;
    private bool hasHoveredTile;
    private Vector3Int cellPosition;
    public bool DEBUG_MODE;

    void Start()
    {
        tiles = GameObject.Find("TileSpawner").GetComponent<TileSpawner>();
        hasHoveredTile = false;
    }

    void Update()
    {
        // Get the mouse position in world space
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        // Convert the mouse position to a cell position in the tilemap
        Vector3Int tileMapXY = hexTilemap.WorldToCell(mouseWorldPos);


        (int q, int r, int s) = gridManager.TileMapXYtoQRS(tileMapXY.x, tileMapXY.y);


        Vector3Int qrsPosition = new Vector3Int(q, r, s);
        cellPosition = qrsPosition;

        if (DEBUG_MODE)
        {
            Debug.Log("QRS Position: " + qrsPosition);
        }
        // If the mouse is over a new cell
        if (cellPosition != lastHoveredTilePosition)
        {
            // Reset the last hovered tile if there was one
            if (hasHoveredTile)
            {
                tiles.ColorTile(lastHoveredTilePosition, Color.white); // Restore the original tile
                hasHoveredTile = false;
            }

            // Set the new tile to the highlight tile
            if (tiles.HasTile(cellPosition))
            {
                origTilePos = cellPosition;
                tiles.ColorTile(cellPosition, Color.grey); // Highlight the new tile
                // originalTile = hexTilemap.GetTile(cellPosition); // Save the original tile
                // hexTilemap.SetTile(cellPosition, highlightTile);
                lastHoveredTilePosition = cellPosition;
                hasHoveredTile = true;
            }
        }
    }


    public bool spawnTower(GameObject tower) {
      // Check for mouse click to place a tower
        if (hasHoveredTile)
        {
            (int q, int r, int s) = (cellPosition.x, cellPosition.y, cellPosition.z);
            HexTile spot = gridManager.FetchTile(q, r, s);
            HexPosition towerComponent;
            GameObject t;
            if (!spot.getOccupied())
            {
                if (Input.GetMouseButtonUp(0))
                {
                    if (DEBUG_MODE) {
                        Debug.Log("Click released at: " + q + r + s);
                    }
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
                (int q, int r, int s) = (cellPosition.x, cellPosition.y, cellPosition.z);
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
