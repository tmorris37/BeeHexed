using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GridSystem;

namespace EnemyAndTowers
{
    public class MovementAlgorithms : MonoBehaviour
    {
        public bool DEBUG = false;
        [SerializeField] private GridManager GridManager;
        
        // Debug function letting us know the move was successful or not
        public void moveFailure()
        {
            if (DEBUG)
                Debug.Log("Move failed");
        }

        // Rotate the enemy to face the target position
        public void RotateTowards(Enemy enemy, Vector3 targetPosition)
        {
            // Calculate the direction vector
            Vector3 direction = targetPosition - enemy.transform.position;

            // Rotate to face direction
            if (direction != Vector3.zero)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                enemy.transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }

        // Move the enemy to the northwest
        // Returns true if the move was successful, false otherwise
        public bool MoveNW(Enemy enemy, int q, int r, int s)
        {
            enemy.targetPosition = new Vector3(q,r-1,s+1);
            (float a, float b) = GridManager.QRStoXY(q,r-1,s+1);
            Vector3 targetPositionXY = new Vector3(a, b, 0);
            if (enemy.Move("Northwest") == 1)
            {
                RotateTowards(enemy, targetPositionXY);
                enemy.MoveToPosition(targetPositionXY);
                return true;
            }
            if (BlockedByTower(enemy))
            {
                RotateTowards(enemy, targetPositionXY);
            }
            return false;
        }

        // Move the enemy to the northeast
        // Returns true if the move was successful, false otherwise
        public bool MoveNE(Enemy enemy, int q, int r, int s)
        {
            enemy.targetPosition = new Vector3(q+1,r-1,s);
            (float a, float b) = GridManager.QRStoXY(q+1,r-1,s);
            Vector3 targetPositionXY = new Vector3(a, b, 0);
            if (enemy.Move("Northeast") == 1)
            {
                RotateTowards(enemy, targetPositionXY);
                enemy.MoveToPosition(targetPositionXY);
                return true;
            }
            if (BlockedByTower(enemy))
            {
                RotateTowards(enemy, targetPositionXY);
            }
            return false;
        }

        // Move the enemy to the east
        // Returns true if the move was successful, false otherwise
        public bool MoveE(Enemy enemy, int q, int r, int s)
        {
            enemy.targetPosition = new Vector3(q+1,r,s-1);
            (float a, float b) = GridManager.QRStoXY(q+1,r,s-1);
            Vector3 targetPositionXY = new Vector3(a, b, 0);
            if (enemy.Move("East") == 1)
            {
                RotateTowards(enemy, targetPositionXY);
                enemy.MoveToPosition(targetPositionXY);
                return true;
            }
            if (BlockedByTower(enemy))
            {
                RotateTowards(enemy, targetPositionXY);
            }
            return false;
        }

        // Move the enemy to the southeast
        // Returns true if the move was successful, false otherwise
        public bool MoveSE(Enemy enemy, int q, int r, int s)
        {
            enemy.targetPosition = new Vector3(q,r+1,s-1);
            (float a, float b) = GridManager.QRStoXY(q,r+1,s-1);
            Vector3 targetPositionXY = new Vector3(a, b, 0);
            if (enemy.Move("Southeast") == 1)
            {
                RotateTowards(enemy, targetPositionXY);
                enemy.MoveToPosition(targetPositionXY);
                return true;
            }
            if (BlockedByTower(enemy))
            {
                RotateTowards(enemy, targetPositionXY);
            }
            return false;
        }

        // Move the enemy to the southwest
        // Returns true if the move was successful, false otherwise
        public bool MoveSW(Enemy enemy, int q, int r, int s)
        {
            enemy.targetPosition = new Vector3(q-1,r+1,s);
            (float a, float b) = GridManager.QRStoXY(q-1,r+1,s);
            Vector3 targetPositionXY = new Vector3(a, b, 0);
            if (enemy.Move("Southwest") == 1)
            {
                RotateTowards(enemy, targetPositionXY);
                enemy.MoveToPosition(targetPositionXY);
                return true;
            }
            if (BlockedByTower(enemy))
            {
                RotateTowards(enemy, targetPositionXY);
            }
            return false;
        }

        // Move the enemy to the west
        // Returns true if the move was successful, false otherwise
        public bool MoveW(Enemy enemy, int q, int r, int s)
        {
            enemy.targetPosition = new Vector3(q-1,r,s+1);
            (float a, float b) = GridManager.QRStoXY(q-1,r,s+1);
            Vector3 targetPositionXY = new Vector3(a, b, 0);
            if (enemy.Move("West") == 1)
            {
                RotateTowards(enemy, targetPositionXY);
                enemy.MoveToPosition(targetPositionXY);
                return true;
            }
            if (BlockedByTower(enemy))
            {
                RotateTowards(enemy, targetPositionXY);
            }
            return false;
        }

        // Move the enemy to the nearest spoke and then along the spoke
        // Returns true if the move was successful, false otherwise
        public bool SimpleMove(Enemy enemy)
        {
            // Get the current position of the enemy
            int q = enemy.q;
            int r = enemy.r;
            int s = enemy.s;
            
            // If enemy is at the origin, do nothing
            if (q == 0 && r == 0 && s == 0) {
                return false;
            }
            // If the enemy is on a spoke
            if (q == 0 || r == 0 || s == 0)
            {
                //Try to move towards the center
                if (moveInOnSpokes(enemy)) {
                    enemy.inertia = Enemy.InertiaDirection.i;
                    return true;
                }
                // Failed, check if the blocker was a tower
                if (BlockedByTower(enemy)) {
                    enemy.inertia = Enemy.InertiaDirection.i;
                    return false;
                }
                if (enemy.inertia == Enemy.InertiaDirection.i) {
                    enemy.inertia = Enemy.InertiaDirection.cw;
                }
                if (enemy.inertia == Enemy.InertiaDirection.cw) {
                    // Try to move counterclockwise
                    if (moveClockwise(enemy)) {
                        enemy.inertia = Enemy.InertiaDirection.cw;
                        return true;
                    }
                    // Failed, check if the blocker was a tower
                    if (BlockedByTower(enemy)) {
                        enemy.inertia = Enemy.InertiaDirection.cw;
                        return false;
                    }
                    enemy.inertia = Enemy.InertiaDirection.ccw;
                }
                if (enemy.inertia == Enemy.InertiaDirection.ccw) {
                    // Try to move clockwise
                    if (moveCounterclockwise(enemy)) {
                        enemy.inertia = Enemy.InertiaDirection.cw;
                        return true;
                    }
                    // Failed, check if the blocker was a tower
                    if (BlockedByTower(enemy)) {
                        enemy.inertia = Enemy.InertiaDirection.ccw;
                        return false;
                    }
                    enemy.inertia = Enemy.InertiaDirection.cw;
                }
            }

            // If the enemy is not on a spoke
            if (enemy.inertia == Enemy.InertiaDirection.cw)
            {
                if (moveClockwise(enemy)) {
                    return true;
                }
                // Failed, check if the blocker was a tower
                if (BlockedByTower(enemy)) {
                    return false;
                }
                enemy.inertia = Enemy.InertiaDirection.ccw;
            }

            if (enemy.inertia == Enemy.InertiaDirection.ccw)
            {
                if (moveCounterclockwise(enemy)) {
                    return true;
                }
                // Failed, check if the blocker was a tower
                if (BlockedByTower(enemy)) {
                    return false;
                }
                enemy.inertia = Enemy.InertiaDirection.cw;
            }
            return false;
        }

        // Checks to see if the target position is blocked by an obstacle
        public bool BlockedByTower(Enemy enemy)
        {
            int tq = (int) enemy.targetPosition.x;
            int tr = (int) enemy.targetPosition.y;
            int ts = (int) enemy.targetPosition.z;
            if (enemy.isBlockedBy(tq, tr, ts) == 1)
            {
                return true;
            }
            return false;
        }

        // Move the enemy counterclockwise along the hex grid
        // Returns true if the move was successful, false otherwise
        public bool moveCounterclockwise(Enemy enemy)
        {
            // Get the current position of the enemy
            int q = enemy.q;
            int r = enemy.r;
            int s = enemy.s;

            // If enemy is at the origin, do nothing
            if (q == 0 && r == 0 && s == 0) {
                return false;
            }

            // If the enemy is not on a spoke
            if (q != 0 && r != 0 && s != 0)
            {
                int absq = Math.Abs(q);
                int absr = Math.Abs(r);
                int abss = Math.Abs(s);

                if (q > r && q > s && absq > absr && absq > abss) {
                return MoveNW(enemy,q,r,s);
                } else if (r > q && r > s && absr > absq && absr > abss) {
                return MoveE(enemy,q,r,s);
                } else if (s > q && s > r && abss > absq && abss > absr) {
                return MoveSW(enemy,q,r,s);
                } else if (q < r && q < s && absq > absr && absq > abss) {
                return MoveSE(enemy,q,r,s);
                } else if (r < q && r < s && absr > absq && absr > abss) {
                return MoveW(enemy,q,r,s);
                } else if (s < q && s < r && abss > absq && abss > absr) {
                return MoveNE(enemy,q,r,s);
                }
            }

            // Move the enemy off the spoke
            if (Math.Abs(q) == Math.Abs(r) && s == 0) {
                if (q > r) {
                return MoveW(enemy,q,r,s);
                } else {
                return MoveE(enemy,q,r,s);
                }
            } else if (Math.Abs(q) == Math.Abs(s) && r == 0) {
                if (q > s) {
                return MoveNW(enemy,q,r,s);
                } else {
                return MoveSE(enemy,q,r,s);
                }
            } else if (Math.Abs(r) == Math.Abs(s) && q == 0) {
                if (r > s) {
                return MoveNE(enemy,q,r,s);
                } else {
                return MoveSW(enemy,q,r,s);
                }
            }
            return false;
        }

        // Move the enemy clockwise along the hex grid
        // Returns true if the move was successful, false otherwise
        public bool moveClockwise(Enemy enemy)
        {
            // Get the current position of the enemy
            int q = enemy.q;
            int r = enemy.r;
            int s = enemy.s;

            // If enemy is at the origin, do nothing
            if (q == 0 && r == 0 && s == 0) {
                return false;
            }

            // If the enemy is not on a spoke
            if (q != 0 && r != 0 && s != 0)
            {
                int absq = Math.Abs(q);
                int absr = Math.Abs(r);
                int abss = Math.Abs(s);

                if (q > r && q > s && absq > absr && absq > abss) {
                return MoveSE(enemy,q,r,s);
                } else if (r > q && r > s && absr > absq && absr > abss) {
                return MoveW(enemy,q,r,s);
                } else if (s > q && s > r && abss > absq && abss > absr) {
                return MoveNE(enemy,q,r,s);
                } else if (q < r && q < s && absq > absr && absq > abss) {
                return MoveNW(enemy,q,r,s);
                } else if (r < q && r < s && absr > absq && absr > abss) {
                return MoveE(enemy,q,r,s);
                } else if (s < q && s < r && abss > absq && abss > absr) {
                return MoveSW(enemy,q,r,s);
                }
            }

            // Move the enemy off the spoke
            if (Math.Abs(q) == Math.Abs(r) && s == 0) {
                if (q > r) {
                return MoveSE(enemy,q,r,s);
                } else {
                return MoveNW(enemy,q,r,s);
                }
            } else if (Math.Abs(q) == Math.Abs(s) && r == 0) {
                if (q > s) {
                return MoveSW(enemy,q,r,s);
                } else {
                return MoveNE(enemy,q,r,s);
                }
            } else if (Math.Abs(r) == Math.Abs(s) && q == 0) {
                if (r > s) {
                return MoveW(enemy,q,r,s);
                } else {
                return MoveE(enemy,q,r,s);
                }
            }
            return false;
        }

        // If on a spoke, move the enemy towards the center of the hex grid
        // Returns true if the move was successful, false otherwise
        public bool moveInOnSpokes(Enemy enemy)
        {
            // Get the current position of the enemy
            int q = enemy.q;
            int r = enemy.r;
            int s = enemy.s;

            // If enemy is at the origin, do nothing
            if (q == 0 && r == 0 && s == 0) {
                return false;
            }

            // Move the enemy along the spokes of the hex grid
            if (Math.Abs(q) == Math.Abs(r) && s == 0) {
                if (q > r) {
                return MoveSW(enemy,q,r,s);
                } else {
                return MoveNE(enemy,q,r,s);
                }
            } else if (Math.Abs(q) == Math.Abs(s) && r == 0) {
                if (q > s) {
                return MoveW(enemy,q,r,s);
                } else {
                return MoveE(enemy,q,r,s);
                }
            } else if (Math.Abs(r) == Math.Abs(s) && q == 0) {
                if (r > s) {
                return MoveNW(enemy,q,r,s);
                } else {
                return MoveSE(enemy,q,r,s);
                }
            }
            return false;
        }

        // If on a spoke, move the enemy away from the center of the hex grid
        // Returns true if the move was successful, false otherwise
        public bool moveOutOnSpokes(Enemy enemy)
        {
            // Get the current position of the enemy
            int q = enemy.q;
            int r = enemy.r;
            int s = enemy.s;

            // If enemy is at the origin, do nothing
            if (q == 0 && r == 0 && s == 0) {
                return false;
            }

            // Move the enemy along the spokes of the hex grid
            if (Math.Abs(q) == Math.Abs(r) && s == 0) {
                if (q > r) {
                return MoveNE(enemy,q,r,s);
                } else {
                return MoveSW(enemy,q,r,s);
                }
            } else if (Math.Abs(q) == Math.Abs(s) && r == 0) {
                if (q > s) {
                return MoveE(enemy,q,r,s);
                } else {
                return MoveW(enemy,q,r,s);
                }
            } else if (Math.Abs(r) == Math.Abs(s) && q == 0) {
                if (r > s) {
                return MoveSE(enemy,q,r,s);
                } else {
                return MoveNW(enemy,q,r,s);
                }
            }
            return false;
        }
    }
}