using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGameBEnemyMana : BossGameBEnemy
{
    public override BossGameSystemB.CharacterID GetCharacterID()
    {
        return BossGameSystemB.CharacterID.Mana;
    }
}
