using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GridSystem;

namespace EnemyAndTowers {
    public class CaveRotator : MonoBehaviour {
        [SerializeField] public CaveGenerator caveGenerator;
        [SerializeField] public GridManager gridManager;

        private List<Vector3> cavePositions;

        public enum direction : int {
            None      = -1,
            West      = 0,
            Southwest = 1,
            Southeast = 2,
            East      = 3,
            Northeast = 4,
            Northwest = 5
        }

        void Start() {
            this.cavePositions = this.caveGenerator.cavePositions;

            ShortestPath pathsFromCave = new ShortestPath();

            foreach (Vector3 cavePosition in cavePositions) {
                (int q, int r, int s) = ((int) cavePosition.x,
                                         (int) cavePosition.y,
                                         (int) cavePosition.z);

                foreach (GameObject cave in GameObject.FindGameObjectsWithTag("Cave")) {
                    if ((cave.transform.position.x, cave.transform.position.y) == this.gridManager.QRStoXY(q, r, s)) {
                        List<(int, int, int)> cavePath = 
                            pathsFromCave.DijkstraSimple(this.gridManager,
                                                         (q, r, s),
                                                         (0, 0, 0),
                                                         DijkstraCallback);
                        
                        (int, int, int) QRSTuple = (cavePath[1].Item1 - cavePath[0].Item1,
                                                    cavePath[1].Item2 - cavePath[0].Item2,
                                                    cavePath[1].Item3 - cavePath[0].Item3);

                        direction dir = QRStoDirection(QRSTuple);

                        int rotation = ((int) dir) * 60;

                        cave.transform.Rotate(0, 0, rotation);

                        Debug.Log("Cave at " + cavePosition);
                    }
                }
            }
        }

        public bool DijkstraCallback((int, int, int) QRSTuple) {
            (int q, int r, int s) = QRSTuple;

            return this.gridManager.FetchTile(q, r, s).getOccupiedByObstacle();
        }

        // Converts the unit QRS Tuple to its direction
        // Returns None (-1) if not a unit QRS Tuple
        public direction QRStoDirection((int, int, int) QRSTuple) {
            if        (QRSTuple == (-1,  0, +1)) {
                return direction.West;
            } else if (QRSTuple == (-1, +1,  0)) {
                return direction.Southwest;
            } else if (QRSTuple == ( 0, +1, -1)) {
                return direction.Southeast;
            } else if (QRSTuple == (+1,  0, -1)) {
                return direction.East;
            } else if (QRSTuple == (+1, -1,  0)) {
                return direction.Northeast;
            } else if (QRSTuple == ( 0, -1, +1)) {
                return direction.Northwest;
            } else {
                return direction.None;
            }
        }
    }
}
