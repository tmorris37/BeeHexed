using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell Card", menuName = "Card/Spell Card")]
public class SpellCard : Card
{
    [SerializeField] private SpellType spellType;
    enum SpellType {
      Blessing,
      Hex
    }
}
