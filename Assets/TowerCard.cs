using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Tower Card", menuName = "Card/Tower Card")]
public class TowerCard : Card
{
  [SerializeField] private int TowerID;
  private int health;
  private int damage;
  private double attackSpeed;
}

