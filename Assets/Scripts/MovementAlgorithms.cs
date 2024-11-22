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
        private GridManager GridManager;

        public MovementAlgorithms(GridManager GridManager, bool DEBUG)
        {
            this.GridManager = GridManager;
            this.DEBUG = DEBUG;

        }
        public void moveFailure()
        {
            if (DEBUG)
                Debug.Log("Move failed");
        }

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

        public void MoveNW(Enemy enemy, int q, int r, int s)
        {
            if (enemy.Move("Northwest"))
            {
                (float a, float b) = GridManager.QRStoXY(q,r-1,s+1);
                Vector3 targetPosition = new Vector3(a, b, 0);
                RotateTowards(enemy, targetPosition);
                enemy.MoveToPosition(targetPosition);
            }
            else
              moveFailure();
        }
        public void MoveNE(Enemy enemy, int q, int r, int s)
        {
            if (enemy.Move("Northeast"))
            {
                (float a, float b) = GridManager.QRStoXY(q+1,r-1,s);
                Vector3 targetPosition = new Vector3(a, b, 0);
                RotateTowards(enemy, targetPosition);
                enemy.MoveToPosition(targetPosition);
            }
            else
              moveFailure();
        }
        public void MoveE(Enemy enemy, int q, int r, int s)
        {
            if (enemy.Move("East"))
            {
                (float a, float b) = GridManager.QRStoXY(q+1,r,s-1);
                Vector3 targetPosition = new Vector3(a, b, 0);
                RotateTowards(enemy, targetPosition);
                enemy.MoveToPosition(targetPosition);
            }
            else
              moveFailure();
        }
        public void MoveSE(Enemy enemy, int q, int r, int s)
        {
            if (enemy.Move("Southeast"))
            {
                (float a, float b) = GridManager.QRStoXY(q,r+1,s-1);
                Vector3 targetPosition = new Vector3(a, b, 0);
                RotateTowards(enemy, targetPosition);
                enemy.MoveToPosition(targetPosition);
            }
            else
              moveFailure();
        }
        public void MoveSW(Enemy enemy, int q, int r, int s)
        {
            if (enemy.Move("Southwest"))
            {
                (float a, float b) = GridManager.QRStoXY(q-1,r+1,s);
                Vector3 targetPosition = new Vector3(a, b, 0);
                RotateTowards(enemy, targetPosition);
                enemy.MoveToPosition(targetPosition);
            }
            else
              moveFailure();
        }
        public void MoveW(Enemy enemy, int q, int r, int s)
        {
            if (enemy.Move("West"))
            {
                (float a, float b) = GridManager.QRStoXY(q-1,r,s+1);
                Vector3 targetPosition = new Vector3(a, b, 0);
                RotateTowards(enemy, targetPosition);
                enemy.MoveToPosition(targetPosition);
            }
            else
              moveFailure();
        }
        public void SimpleMove(Enemy enemy)
        {
          // Get the current position of the enemy
          int q = enemy.q;
          int r = enemy.r;
          int s = enemy.s;
          
          // If enemy is at the origin, do nothing
          if (q == 0 && r == 0 && s == 0) {
            return;
          }

          // Move the enemy along the spokes of the hex grid if possible
          if (Math.Abs(q) == Math.Abs(r) && s == 0) {
            if (q > r) {
              MoveSW(enemy,q,r,s);
            } else {
              MoveNE(enemy,q,r,s);
            }
          } else if (Math.Abs(q) == Math.Abs(s) && r == 0) {
            if (q > s) {
              MoveW(enemy,q,r,s);
            } else {
              MoveE(enemy,q,r,s);
            }
          } else if (Math.Abs(r) == Math.Abs(s) && q == 0) {
            if (r > s) {
              MoveNW(enemy,q,r,s);
            } else {
              MoveSE(enemy,q,r,s);
            }
          }
          // If the enemy is not on a spoke, move it to the nearest spoke
          else {
            int absq = Math.Abs(q);
            int absr = Math.Abs(r);
            int abss = Math.Abs(s);

            if (q > r && q > s && absq > absr && absq > abss) {
              MoveNW(enemy,q,r,s);
              if (DEBUG) {
                Debug.Log("Moving NW");
              }
            } else if (r > q && r > s && absr > absq && absr > abss) {
              MoveE(enemy,q,r,s);
            } else if (s > q && s > r && abss > absq && abss > absr) {
              MoveSW(enemy,q,r,s);
            } else if (q < r && q < s && absq > absr && absq > abss) {
              MoveSE(enemy,q,r,s);
            } else if (r < q && r < s && absr > absq && absr > abss) {
              MoveW(enemy,q,r,s);
            } else if (s < q && s < r && abss > absq && abss > absr) {
              MoveNE(enemy,q,r,s);
            }
          }
        }
    }
}