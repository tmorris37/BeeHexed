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

        public bool MoveNW(Enemy enemy, int q, int r, int s)
        {
            (float a, float b) = GridManager.QRStoXY(q,r-1,s+1);
            Vector3 targetPosition = new Vector3(a, b, 0);
            RotateTowards(enemy, targetPosition);
            if (enemy.Move("Northwest"))
            {
                enemy.MoveToPosition(targetPosition);
                return true;
            }
            return false;
        }
        public bool MoveNE(Enemy enemy, int q, int r, int s)
        {
            (float a, float b) = GridManager.QRStoXY(q+1,r-1,s);
            Vector3 targetPosition = new Vector3(a, b, 0);
            RotateTowards(enemy, targetPosition);
            if (enemy.Move("Northeast"))
            {
                enemy.MoveToPosition(targetPosition);
                return true;
            }
            return false;
        }
        public bool MoveE(Enemy enemy, int q, int r, int s)
        {
            (float a, float b) = GridManager.QRStoXY(q+1,r,s-1);
            Vector3 targetPosition = new Vector3(a, b, 0);
            RotateTowards(enemy, targetPosition);
            if (enemy.Move("East"))
            {
                enemy.MoveToPosition(targetPosition);
                return true;
            }
            return false;
        }
        public bool MoveSE(Enemy enemy, int q, int r, int s)
        {
            (float a, float b) = GridManager.QRStoXY(q,r+1,s-1);
            Vector3 targetPosition = new Vector3(a, b, 0);
            RotateTowards(enemy, targetPosition);
            if (enemy.Move("Southeast"))
            {
                enemy.MoveToPosition(targetPosition);
                return true;
            }
            return false;
        }
        public bool MoveSW(Enemy enemy, int q, int r, int s)
        {
            (float a, float b) = GridManager.QRStoXY(q-1,r+1,s);
            Vector3 targetPosition = new Vector3(a, b, 0);
            RotateTowards(enemy, targetPosition);
            if (enemy.Move("Southwest"))
            {
                enemy.MoveToPosition(targetPosition);
                return true;
            }
            return false;
        }
        public bool MoveW(Enemy enemy, int q, int r, int s)
        {
            (float a, float b) = GridManager.QRStoXY(q-1,r,s+1);
            Vector3 targetPosition = new Vector3(a, b, 0);
            RotateTowards(enemy, targetPosition);
            if (enemy.Move("West"))
            {
                enemy.MoveToPosition(targetPosition);
                return true;
            }
            return false;
        }
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

          // Move the enemy along the spokes of the hex grid if possible
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
          // If the enemy is not on a spoke, move it to the nearest spoke
          else {
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
          return false;
        }
    }
}