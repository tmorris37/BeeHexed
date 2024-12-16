using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridSystem {
    public class TileSelector {
        // Creates a List of N (q, r, s) values within the Radius given
        // If (N < 0): Returns List with size 0
        // If (N > 6*Radius): Returns List with size 6*Radius
        // Otherwise: Returns List with size N
        // If Callback function is specifed, will only add (q, r, s) values that satisfy the Callback

        public List<(int, int, int)> SelectNRandomTiles(int N, int Radius, Predicate<(int, int, int)> ConditionCallback = null) {
            List<(int, int, int)> AvailableTiles = TilesInRing(Radius);

            List<(int, int, int)> NRandomTiles = new List<(int, int, int)>();
            
            int MaxQuantity = 6 * Radius;

            // Ensures Quantity is not an impossible value
            int Quantity = N;
            Quantity = (Quantity < 0) ? 0 : Quantity;
            Quantity = (Quantity > MaxQuantity) ? MaxQuantity : Quantity;

            int q, r, s;
            int i = 0;

            while (i < Quantity && AvailableTiles.Count != 0) {
                // Checks if Seed is null and if it is creates a new one
                if (Seed.Instance == null) {
                    GameObject seedObject = new GameObject("SeedContainer");
                    Seed seed = seedObject.AddComponent<Seed>();
                    seed.SetSeedFromTime();
                }
                // Selects a random tile from the currently available ones
                int RandomIndex = Seed.Instance.GetRandomInt(0, AvailableTiles.Count);

                // Shifts the (q, r, s) value from AvailableTiles to NRandomTiles
                // Only shifts if there is no Callback or if Callback returns true
                (q, r, s) = AvailableTiles[RandomIndex];
                if (ConditionCallback == null || ConditionCallback((q, r, s))) {
                    NRandomTiles.Add((q, r, s));
                    i++;
                }
                AvailableTiles.RemoveAt(RandomIndex);
            }
            return NRandomTiles;
        }
        // Creates a List of all (q, r, s) values within the Radius without repeats
        public List<(int, int, int)> TilesInRing(int Radius) {
            List<(int, int, int)> NewQRS = new List<(int, int, int)>();
            List<(int, int, int)> ABC;
            int a, b, c;

            // Runs twice, for both halves of the Hex Grid
            for (int i = -1; i < 2; i+=2) {
                ABC = QRSGenerator(i * Radius);
                for (int j = 0; j < ABC.Count; j++) {
                    (a, b, c) = ABC[j];

                    NewQRS.Add((a, b, c));
                    NewQRS.Add((b, c, a));
                    NewQRS.Add((c, a, b));
                }
            }
            return NewQRS;
        }

        // Generates a List of QRS coordinates that represent 1 side of the Hex
        public List<(int, int, int)> QRSGenerator(int Radius) {
            List<(int, int, int)> ABC = new List<(int, int, int)>();
            int a, b, c;

            a = Radius;
            // Covers (Radius, 0, -Radius) to (Radius, 1-Radius, -1)
            for (b = 0; b < SignOf(a)*a; b++) {
                int bAdj = -b * SignOf(a);
                c = -a - bAdj;
                ABC.Add((a, bAdj, c));
            }
            return ABC;
        }

        // If number is negative, return -1. 1 otherwise
        public int SignOf(int number) {
            return (number < 0) ? -1 : 1;
        }
    }
}