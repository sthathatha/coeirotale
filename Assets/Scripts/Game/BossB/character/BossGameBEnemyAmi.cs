using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGameBEnemyAmi : BossGameBEnemy
{
    #region ƒƒ“ƒo[

    /// <summary>‚Í‚é‚Ì‚Æ‚È‚è‰ñ•œSE</summary>
    public AudioClip se_harunotonari1;

    #endregion

    public override BossGameSystemB.CharacterID GetCharacterID()
    {
        return BossGameSystemB.CharacterID.Ami;
    }
}
