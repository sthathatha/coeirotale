using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ラスボス本戦　アームストロング砲エフェクト
/// </summary>
public class BossGameBCanonEffect : BossGameBEffectBase
{
    private const float P_RIGHT = 584;
    private const float P_LEFT = -220;
    private const float P_UP = 290;
    private const float P_DOWN = -222;

    public Sprite sp_canon1;
    public Sprite sp_canon2;
    public Sprite sp_canon3;

    private Vector3 paramPos;

    /// <summary>
    /// 設定
    /// </summary>
    /// <param name="dist"></param>
    public void SetParam(Vector2Int dist)
    {
        paramPos = new Vector3();
        if (dist.x > 0)
        {
            paramPos.x = P_RIGHT;
            model.flipX = false;
        }
        else
        {
            paramPos.x = P_LEFT;
            model.flipX = true;
        }

        if (dist.y > 0)
        {
            paramPos.y = P_UP;
            model.flipY = true;
        }
        else
        {
            paramPos.y = P_DOWN;
            model.flipY = false;
        }
    }

    /// <summary>
    /// 再生
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Play()
    {
        transform.localPosition = paramPos;
        for (var i = 0; i < 15; ++i)
        {
            model.sprite = sp_canon1;
            yield return new WaitForSeconds(0.05f);
            model.sprite = sp_canon2;
            yield return new WaitForSeconds(0.05f);
            model.sprite = sp_canon3;
            yield return new WaitForSeconds(0.05f);
        }
    }

}
