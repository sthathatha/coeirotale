using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGameBEnemyMati : BossGameBEnemy
{
    public override BossGameSystemB.CharacterID GetCharacterID()
    {
        return BossGameSystemB.CharacterID.Mati;
    }
}
