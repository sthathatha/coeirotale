using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGameBEnemyMenderu : BossGameBEnemy
{
    public override BossGameSystemB.CharacterID GetCharacterID()
    {
        return BossGameSystemB.CharacterID.Menderu;
    }
}
