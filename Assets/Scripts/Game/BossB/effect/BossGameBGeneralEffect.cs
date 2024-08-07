using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 単純なスプライト切り替えエフェクト
/// </summary>
public class BossGameBGeneralEffect : BossGameBEffectBase
{
    private List<Sprite> spriteList;
    private float changeInterval;

    /// <summary>
    /// パラメータ設定
    /// </summary>
    /// <param name="sprites"></param>
    /// <param name="interval"></param>
    public void SetParam(Sprite[] sprites, float interval = 0.06f)
    {
        spriteList = new List<Sprite>();
        spriteList.AddRange(sprites);
        changeInterval = interval;
    }

    /// <summary>
    /// 再生
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Play()
    {
        foreach (var sprite in spriteList)
        {
            model.sprite = sprite;
            yield return new WaitForSeconds(changeInterval);
        }
    }
}
