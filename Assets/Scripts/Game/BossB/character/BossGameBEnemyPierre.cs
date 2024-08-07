using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGameBEnemyPierre : BossGameBEnemy
{
    public override BossGameSystemB.CharacterID GetCharacterID()
    {
        return BossGameSystemB.CharacterID.Pierre;
    }
}
