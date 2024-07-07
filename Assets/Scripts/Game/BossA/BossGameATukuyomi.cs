using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ＊＊＊＊イベント つくよみちゃん
/// </summary>
public class BossGameATukuyomi : BossGameAPlayers
{
    /// <summary>
    /// 左に魔法エフェクト出して立ちポーズに戻る
    /// </summary>
    /// <returns></returns>
    public IEnumerator PlayMagicEffect()
    {
        var sound = ManagerSceneScript.GetInstance().soundMan;
        sound.PlaySE(magicSe);

        magicEff.gameObject.SetActive(true);
        for (var i = 0; i < 3; ++i)
        {
            magicEff.sprite = image_spell0;
            yield return new WaitForSeconds(0.07f);
            magicEff.sprite = image_spell1;
            yield return new WaitForSeconds(0.13f);
        }
        magicEff.gameObject.SetActive(false);

        model.sprite = image_stand;
    }

    /// <summary>
    /// 構えるだけ
    /// </summary>
    public void KamaePose()
    {
        model.sprite = image_magic;
    }
}
