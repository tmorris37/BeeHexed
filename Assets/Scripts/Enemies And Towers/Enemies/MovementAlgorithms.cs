using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GridSystem;

namespace EnemyAndTowers
{
    public class MovementAlgorithms : MonoBehaviour
    {
        private static Enemy.InertiaDirection NULLDIR = Enemy.InertiaDirection.nullDir;
        private static Enemy.InertiaDirection CW = Enemy.InertiaDirection.cw;
        private static Enemy.InertiaDirection CCW = Enemy.InertiaDirection.ccw;
        private static Enemy.InertiaDirection I = Enemy.InertiaDirection.i;
        private static Enemy.InertiaDirection O = Enemy.InertiaDirection.o;

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
            (float a, float b) = GridManager.QRStoXY(q,r-1,s+1);
            Vector3 targetPositionXY = new Vector3(a, b, 0);
            enemy.targetPositionXY = targetPositionXY;
            if (enemy.Move("Northwest") == 1)
            {
                RotateTowards(enemy, targetPositionXY);
                enemy.MoveToPosition();
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
            (float a, float b) = GridManager.QRStoXY(q+1,r-1,s);
            Vector3 targetPositionXY = new Vector3(a, b, 0);
            enemy.targetPositionXY = targetPositionXY;
            if (enemy.Move("Northeast") == 1)
            {
                RotateTowards(enemy, targetPositionXY);
                enemy.MoveToPosition();
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
            (float a, float b) = GridManager.QRStoXY(q+1,r,s-1);
            Vector3 targetPositionXY = new Vector3(a, b, 0);
            enemy.targetPositionXY = targetPositionXY;
            if (enemy.Move("East") == 1)
            {
                RotateTowards(enemy, targetPositionXY);
                enemy.MoveToPosition();
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
            (float a, float b) = GridManager.QRStoXY(q,r+1,s-1);
            Vector3 targetPositionXY = new Vector3(a, b, 0);
            enemy.targetPositionXY = targetPositionXY;
            if (enemy.Move("Southeast") == 1)
            {
                RotateTowards(enemy, targetPositionXY);
                enemy.MoveToPosition();
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
            (float a, float b) = GridManager.QRStoXY(q-1,r+1,s);
            Vector3 targetPositionXY = new Vector3(a, b, 0);
            enemy.targetPositionXY = targetPositionXY;
            if (enemy.Move("Southwest") == 1)
            {
                RotateTowards(enemy, targetPositionXY);
                enemy.MoveToPosition();
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
            (float a, float b) = GridManager.QRStoXY(q-1,r,s+1);
            Vector3 targetPositionXY = new Vector3(a, b, 0);
            enemy.targetPositionXY = targetPositionXY;
            if (enemy.Move("West") == 1)
            {
                RotateTowards(enemy, targetPositionXY);
                enemy.MoveToPosition();
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
                    enemy.inertiaIO = I;
                    enemy.desiredInertiaIO = NULLDIR;
                    enemy.desiredInertiaDir = enemy.inertiaDir;
                    enemy.inertiaDir = NULLDIR;
                    return true;
                }
                // Failed, check if the blocker was a tower
                if (BlockedByTower(enemy)) {
                    enemy.inertiaIO = I;
                    enemy.desiredInertiaIO = NULLDIR;
                    // enemy.inertiaDir = NULLDIR;
                    return false;
                }
                // If the inertiaIO was 'in or out' set it to null
                if (enemy.inertiaIO != NULLDIR) {
                    enemy.desiredInertiaIO = enemy.inertiaIO;
                    enemy.inertiaIO = NULLDIR;
                }
                // If the inertiaDir is not set, desire clockwise
                if (enemy.inertiaDir == NULLDIR && enemy.desiredInertiaDir == NULLDIR) {
                    enemy.desiredInertiaDir = CW;
                }
                // Try to move clockwise
                if (enemy.inertiaDir == CW || enemy.desiredInertiaDir == CW)
                {
                    if (moveClockwise(enemy)) {
                        enemy.inertiaDir = CW;
                        enemy.desiredInertiaDir = NULLDIR;
                        enemy.desiredInertiaIO = NULLDIR;
                        return true;
                    }
                    // Failed, check if the blocker was a tower
                    if (BlockedByTower(enemy)) {
                        enemy.inertiaDir = CW;
                        enemy.desiredInertiaDir = NULLDIR;
                        return false;
                    }
                    // If enemy is actually moving clockwise, try clockwise out
                    if (enemy.inertiaDir == CW) {
                        enemy.desiredInertiaIO = O;
                    } else {  // Set the inertiaDir to 'counterclockwise' to try the other direction
                        enemy.inertiaDir = NULLDIR;
                        enemy.desiredInertiaDir = CCW;
                    }
                }
                // Try to move counterclockwise
                if (enemy.inertiaDir == CCW || enemy.desiredInertiaDir == CCW)
                {
                    if (moveCounterclockwise(enemy)) {
                        enemy.inertiaDir = CCW;
                        enemy.desiredInertiaDir = NULLDIR;
                        enemy.desiredInertiaIO = NULLDIR;
                        return true;
                    }
                    // Failed, check if the blocker was a tower
                    if (BlockedByTower(enemy)) {
                        enemy.inertiaDir = CCW;
                        enemy.desiredInertiaDir = NULLDIR;
                        return false;
                    }
                    if (enemy.inertiaDir == CCW) {
                        enemy.desiredInertiaIO = NULLDIR;
                    } else {
                        enemy.inertiaDir = NULLDIR;
                        enemy.desiredInertiaDir = CW;
                    }
                    // Set the inertiaIO to 'out' to try to get out
                    enemy.desiredInertiaIO = O;
                }
                // Try to move counterclockwise out
                if ((enemy.inertiaDir == CCW || enemy.desiredInertiaDir == CCW)
                && (enemy.desiredInertiaIO == O)) {
                    if (moveCounterclockwiseOut(enemy)) {
                        enemy.inertiaDir = CCW;
                        enemy.desiredInertiaDir = NULLDIR;
                        enemy.inertiaIO = O;
                        return true;
                    }
                    // Failed, check if the blocker was a tower
                    if (BlockedByTower(enemy)) {
                        enemy.inertiaDir = CCW;
                        enemy.desiredInertiaDir = NULLDIR;
                        enemy.inertiaIO = O;
                        return false;
                    }
                    // Set the inertia to 'clockwise' to try the other direction
                    
                    enemy.desiredInertiaDir = CW;
                }
                // Try to move clockwise out
                if ((enemy.inertiaDir == CW || enemy.desiredInertiaDir == CW)
                && (enemy.desiredInertiaIO == O)) {
                    if (moveClockwiseOut(enemy)) {
                        enemy.inertiaDir = CW;
                        enemy.desiredInertiaDir = NULLDIR;
                        enemy.inertiaIO = O;
                        return true;
                    }
                    // Failed, check if the blocker was a tower
                    if (BlockedByTower(enemy)) {
                        enemy.inertiaDir = CCW;
                        enemy.desiredInertiaDir = NULLDIR;
                        enemy.inertiaIO = O;
                        return false;
                    }
                }
                return false;

                // // Try to move out
                // if ((enemy.inertiaDir == NULLDIR && enemy.desiredInertiaDir == NULLDIR)
                // && (enemy.inertiaIO == O || enemy.desiredInertiaIO == O)) {
                //     if (moveOutOnSpokes(enemy)) {
                //         enemy.inertiaIO = O;
                //         return true;
                //     }
                //     // Failed, check if the blocker was a tower
                //     if (BlockedByTower(enemy)) {
                //         enemy.desiredInertiaIO = O;
                //         return false;
                //     }
                // }
            }

            // If the enemy is not on a spoke
            // If there is no set inertiaDir, set it to clockwise
            if (enemy.inertiaDir == NULLDIR && enemy.desiredInertiaDir == NULLDIR) {
                enemy.inertiaDir = CW;
            }

            if (enemy.inertiaIO == O || enemy.inertiaIO == I) {
                enemy.desiredInertiaIO = NULLDIR;
                enemy.inertiaIO = NULLDIR;
            }
            
            // Try to move clockwise
            if ((enemy.inertiaDir == CW || enemy.desiredInertiaDir == CW)
            && enemy.desiredInertiaIO == NULLDIR)
            {
                if (moveClockwise(enemy)) {
                    enemy.inertiaDir = CW;
                    enemy.desiredInertiaDir = NULLDIR;
                    return true;
                }
                // Failed, check if the blocker was a tower
                if (BlockedByTower(enemy)) {
                    enemy.inertiaDir = CW;
                    enemy.desiredInertiaDir = NULLDIR;
                    return false;
                }
                // Set the inertiaIO to 'I' to try to go in
                enemy.desiredInertiaIO = I;
            }
            // Try to move clockwise in
            if ((enemy.inertiaDir == CW || enemy.desiredInertiaDir == CW)
            && (enemy.desiredInertiaIO == I))
            {
                if (moveClockwiseIn(enemy)) {
                    enemy.inertiaDir = CW;
                    enemy.desiredInertiaDir = NULLDIR;
                    enemy.inertiaIO = I;
                    return true;
                }
                // Failed, check if the blocker was a tower
                if (BlockedByTower(enemy)) {
                    enemy.inertiaDir = CW;
                    enemy.desiredInertiaDir = NULLDIR;
                    enemy.inertiaIO = I;
                    return false;
                }
                if (enemy.inertiaDir == CCW) {
                    enemy.inertiaDir = NULLDIR;
                    enemy.desiredInertiaDir = CW;
                    enemy.desiredInertiaIO = NULLDIR;
                } else {
                    enemy.desiredInertiaDir = CCW;
                }
                
                // Set the inertiaDir to 'counterclockwise' to try the other direction
                enemy.inertiaDir = NULLDIR;
                enemy.desiredInertiaDir = CCW;
                enemy.desiredInertiaIO = I;
            }
            // Try to move counterclockwise in
            if ((enemy.inertiaDir == CCW || enemy.desiredInertiaDir == CCW)
            && enemy.desiredInertiaIO == I)
            {
                if (moveCounterclockwiseIn(enemy)) {
                    enemy.inertiaDir = CCW;
                    enemy.desiredInertiaDir = NULLDIR;
                    enemy.inertiaIO = I;
                    return true;
                }
                // Failed, check if the blocker was a tower
                if (BlockedByTower(enemy)) {
                    enemy.inertiaDir = CCW;
                    enemy.desiredInertiaDir = NULLDIR;
                    enemy.inertiaIO = I;
                    return false;
                }
                // enemy.inertiaIO = NULLDIR;
                // enemy.desiredInertiaIO = NULLDIR;
                // enemy.desiredInertiaDir = CW;
                if (enemy.inertiaDir == CCW) {
                    enemy.desiredInertiaDir = CW;
                    enemy.desiredInertiaIO = I;
                } else {
                    enemy.desiredInertiaIO = NULLDIR;
                }
            }
            // Try to move counterclockwise
            if ((enemy.inertiaDir == CCW || enemy.desiredInertiaDir == CCW)
            && enemy.desiredInertiaIO == NULLDIR)
            {
                if (moveCounterclockwise(enemy)) {
                    enemy.inertiaDir = CCW;
                    enemy.desiredInertiaDir = NULLDIR;
                    return true;
                }
                // Failed, check if the blocker was a tower
                if (BlockedByTower(enemy)) {
                    enemy.inertiaDir = CCW;
                    enemy.desiredInertiaDir = NULLDIR;
                    return false;
                }
                // Set to inertiaIO to 'I' to try to go in
                if (enemy.inertiaDir == CCW) {
                    enemy.desiredInertiaIO = I;
                } else {
                    enemy.desiredInertiaDir = CW;
                }
            }
            return false;
        }

        // Checks to see if the target position is blocked by an obstacle
        public bool BlockedByTower(Enemy enemy)
        {
            (int q, int r, int s) = GridManager.XYtoQRS(enemy.targetPositionXY.x, enemy.targetPositionXY.y);
            if (enemy.isBlockedBy(q, r, s) == 1)
            {
                return true;
            }
            return false;
        }

        // Move the enemy counterclockwise and towards the middle
        // Returns true if the move was successful, false otherwise
        public bool moveCounterclockwiseIn(Enemy enemy)
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
                return MoveW(enemy,q,r,s);
                } else if (r > q && r > s && absr > absq && absr > abss) {
                return MoveNE(enemy,q,r,s);
                } else if (s > q && s > r && abss > absq && abss > absr) {
                return MoveSE(enemy,q,r,s);
                } else if (q < r && q < s && absq > absr && absq > abss) {
                return MoveE(enemy,q,r,s);
                } else if (r < q && r < s && absr > absq && absr > abss) {
                return MoveSW(enemy,q,r,s);
                } else if (s < q && s < r && abss > absq && abss > absr) {
                return MoveNW(enemy,q,r,s);
                }
            }
            return false;
        }

        // Move the enemy clockwise and towards from the middle
        // Returns true if the move was successful, false otherwise
        public bool moveClockwiseIn(Enemy enemy)
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
                return MoveSW(enemy,q,r,s);
                } else if (r > q && r > s && absr > absq && absr > abss) {
                return MoveNW(enemy,q,r,s);
                } else if (s > q && s > r && abss > absq && abss > absr) {
                return MoveE(enemy,q,r,s);
                } else if (q < r && q < s && absq > absr && absq > abss) {
                return MoveNE(enemy,q,r,s);
                } else if (r < q && r < s && absr > absq && absr > abss) {
                return MoveSE(enemy,q,r,s);
                } else if (s < q && s < r && abss > absq && abss > absr) {
                return MoveW(enemy,q,r,s);
                }
            }
            return false;
        }

        // Move the enemy counterclockwise and away from the middle
        // Returns true if the move was successful, false otherwise
        public bool moveCounterclockwiseOut(Enemy enemy)
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
                return MoveNE(enemy,q,r,s);
                } else if (r > q && r > s && absr > absq && absr > abss) {
                return MoveSE(enemy,q,r,s);
                } else if (s > q && s > r && abss > absq && abss > absr) {
                return MoveW(enemy,q,r,s);
                } else if (q < r && q < s && absq > absr && absq > abss) {
                return MoveSW(enemy,q,r,s);
                } else if (r < q && r < s && absr > absq && absr > abss) {
                return MoveNW(enemy,q,r,s);
                } else if (s < q && s < r && abss > absq && abss > absr) {
                return MoveE(enemy,q,r,s);
                }
            }

            // Move the enemy off the spoke
            if (Math.Abs(q) == Math.Abs(r) && s == 0) {
                if (q > r) {
                return MoveNW(enemy,q,r,s);
                } else {
                return MoveSE(enemy,q,r,s);
                }
            } else if (Math.Abs(q) == Math.Abs(s) && r == 0) {
                if (q > s) {
                return MoveNE(enemy,q,r,s);
                } else {
                return MoveSW(enemy,q,r,s);
                }
            } else if (Math.Abs(r) == Math.Abs(s) && q == 0) {
                if (r > s) {
                return MoveE(enemy,q,r,s);
                } else {
                return MoveW(enemy,q,r,s);
                }
            }
            return false;
        }

        // Move the enemy clockwise and away from the middle
        // Returns true if the move was successful, false otherwise
        public bool moveClockwiseOut(Enemy enemy)
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
                return MoveE(enemy,q,r,s);
                } else if (r > q && r > s && absr > absq && absr > abss) {
                return MoveSW(enemy,q,r,s);
                } else if (s > q && s > r && abss > absq && abss > absr) {
                return MoveNW(enemy,q,r,s);
                } else if (q < r && q < s && absq > absr && absq > abss) {
                return MoveW(enemy,q,r,s);
                } else if (r < q && r < s && absr > absq && absr > abss) {
                return MoveNE(enemy,q,r,s);
                } else if (s < q && s < r && abss > absq && abss > absr) {
                return MoveSE(enemy,q,r,s);
                }
            }

            // Move the enemy off the spoke
            if (Math.Abs(q) == Math.Abs(r) && s == 0) {
                if (q > r) {
                return MoveE(enemy,q,r,s);
                } else {
                return MoveW(enemy,q,r,s);
                }
            } else if (Math.Abs(q) == Math.Abs(s) && r == 0) {
                if (q > s) {
                return MoveSE(enemy,q,r,s);
                } else {
                return MoveNW(enemy,q,r,s);
                }
            } else if (Math.Abs(r) == Math.Abs(s) && q == 0) {
                if (r > s) {
                return MoveSW(enemy,q,r,s);
                } else {
                return MoveNE(enemy,q,r,s);
                }
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

        // Calculates and returns the Optimal path from the enemy to (0,0,0)
        // Note: List includes Enemy's current position. Adds a delay. To 
        //       resolve, execute DijkstraMoves.RemoveAt(0) before the return
        public List<(int, int, int)> DijkstraInitialize(Enemy enemy)
        {
            (int q, int r, int s) = (enemy.q, enemy.r, enemy.s);

            ShortestPath pathfinder = new ShortestPath();

            List<(int, int, int)> DijkstraMoves = 
                pathfinder.DijkstraSimple(this.GridManager, (q, r, s), (0, 0, 0), DijkstraCallback);
            
            if (DEBUG) {
                string debugStr = "";
                foreach (var tile in DijkstraMoves) {
                    debugStr += tile;
                }
                Debug.Log(debugStr);
            }

            DijkstraMoves.RemoveAt(0);
            
            return DijkstraMoves;
        }

        public bool DijkstraCallback((int, int, int) QRSTuple)
        {
            (int q, int r, int s) = QRSTuple;

            return this.GridManager.FetchTile(q, r, s).getOccupiedByObstacle();
        }

        // Reads the next movement in the provided list. If it is possible, will move (returns true)
        // Otherwise, will not move and does not change the List
        public bool DijkstraMove(Enemy enemy, List<(int, int, int)> DijkstraMoves)
        {
            (int q, int r, int s) = (enemy.q, enemy.r, enemy.s);

            // If enemy is at the origin, do nothing
            if ((q == 0 && r == 0 && s == 0) || DijkstraMoves.Count == 0)
                return false;

            (int targetQ, int targetR, int targetS) = DijkstraMoves[0];

            // Delta q,r,s for update position
            int dq = targetQ - q;
            int dr = targetR - r;
            int ds = targetS - s;

            (float a, float b) = GridManager.QRStoXY(targetQ,targetR,targetS);
            Vector3 targetPositionXY = new Vector3(a, b, 0);
            RotateTowards(enemy, targetPositionXY);

            if (enemy.UpdatePosition(dq, dr, ds) == 1) {
            if (enemy.UpdatePosition(dq, dr, ds) == 1) {
                DijkstraMoves.RemoveAt(0);
                enemy.targetPositionXY = targetPositionXY;
                enemy.MoveToPosition();
                return true;
            }
            if (BlockedByTower(enemy)) {
                RotateTowards(enemy, targetPositionXY);
            }
            return false;
        }
    }
}