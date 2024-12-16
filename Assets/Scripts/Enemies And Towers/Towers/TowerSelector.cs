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

    private Vector3Int lastHovPositionQRS;
    private TileBase originalTile;
    private Vector3Int origTilePos;
    private bool hasHoveredTile;
    private Vector3Int HovPositionQRS;
    public bool DEBUG_MODE;
    public bool highlightTileMode;
    public bool hovTileBlocked;

    void Start() {
        tiles = GameObject.Find("TileSpawner").GetComponent<TileSpawner>();
        hasHoveredTile = false;
    }

    void Update() {
        // Get the mouse position in world space
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        // Convert the mouse position to a cell position in the tilemap
        Vector3Int tileMapXY = hexTilemap.WorldToCell(mouseWorldPos);

        (int q, int r, int s) = gridManager.TileMapXYtoQRS(tileMapXY.x, tileMapXY.y);

        // Set the current hovered position in QRS coordinates
        HovPositionQRS = new Vector3Int(q, r, s);

        HexTile candidateTile = gridManager.FetchTile(q, r, s);
        if (candidateTile != null && (candidateTile.getOccupiedByObstacle() || candidateTile.getOccupiedByTower())) {
            hovTileBlocked = true;
        } else {
            hovTileBlocked = false;
        }
        if (DEBUG_MODE) Debug.Log("QRS Position: " + HovPositionQRS);
        // If the mouse is over a new cell
        if (HovPositionQRS != lastHovPositionQRS) {
            // Reset the last hovered tile if there was one
            if (hasHoveredTile) {
                tiles.ColorTile(lastHovPositionQRS, Color.white); // Restore the original tile
                hasHoveredTile = false;
            }

            // Set the new tile to the highlight tile
            if (tiles.HasTile(HovPositionQRS)) {
                origTilePos = HovPositionQRS;
                lastHovPositionQRS = HovPositionQRS;
                hasHoveredTile = true;
                if (highlightTileMode && !hovTileBlocked) {
                    tiles.ColorTile(HovPositionQRS, Color.grey); // Highlight the new tile
                }
            }
        }
    }

    public bool spawnTower(GameObject tower) {
        if (hasHoveredTile) {
            (int q, int r, int s) = (HovPositionQRS.x, HovPositionQRS.y, HovPositionQRS.z);
            HexTile spot = gridManager.FetchTile(q, r, s);
            HexPosition towerComponent;
            GameObject t;
            if (!spot.getOccupied()) {
                if (SFXManager.Instance != null) SFXManager.Instance.PlayTowerPlace();
                (int x, int y) = gridManager.QRStoTileMapXY(q, r, s);
                Vector3Int tileMapXY = new Vector3Int(x, y, 0);
                Vector3 towerPosition = hexTilemap.CellToWorld(tileMapXY); // Adjust for tile center
                t = Instantiate(tower, towerPosition, Quaternion.identity); // Spawn the tower at the tile position
                towerComponent = t.GetComponent<HexPosition>();
                towerComponent.gridManager = this.gridManager;
                towerComponent.SetQRS(q, r, s);
                spot.EnterTile(t);
                if (DEBUG_MODE) Debug.Log("Tower cast at: " + q + r + s);
                return true;
            }
            return false;
        }
        return false;
    }

    public bool CastSpell(GameObject spell) {
        Vector3 spellPosition = Vector3.zero;
        Instantiate(spell, spellPosition, Quaternion.identity); // Spawn the 'spell' at 0,0,0
        return true;
    }
}
