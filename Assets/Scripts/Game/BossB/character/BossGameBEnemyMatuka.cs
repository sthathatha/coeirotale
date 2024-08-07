using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGameBEnemyMatuka : BossGameBEnemy
{
    public override BossGameSystemB.CharacterID GetCharacterID()
    {
        return BossGameSystemB.CharacterID.Matuka;
    }
}
