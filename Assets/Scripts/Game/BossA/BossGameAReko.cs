using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

/// <summary>
/// ＊＊＊＊イベント レコ
/// </summary>
public class BossGameAReko : BossGameAPlayers
{
    public GameObject magicParent;
    public BossGameAWhiteBlock magicSrc;

    /// <summary>
    /// 左に魔法エフェクト出して立ちポーズに戻る
    /// </summary>
    /// <returns></returns>
    public IEnumerator PlayMagicEffect()
    {
        var sound = ManagerSceneScript.GetInstance().soundMan;
        sound.PlaySE(magicSe);

        for(var i =0; i<8; ++i)
        {
            var rot = i * Mathf.PI / 4f;
            var block = Instantiate(magicSrc, magicParent.transform);
            block.gameObject.SetActive(true);
            block.Show(rot);
        }
        yield return new WaitForSeconds(1f);

        model.sprite = image_stand;
    }

}
