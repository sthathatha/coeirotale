using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ラスボス本戦　チャージエフェクト
/// </summary>
public class BossGameBChargeEffect : BossGameBEffectBase
{
    public const float CHARGE_TIME = 1f;

    /// <summary>
    /// 再生
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Play()
    {
        var colorBase = model.color;
        var alpha = new DeltaFloat();
        alpha.Set(0);
        alpha.MoveTo(0.5f, CHARGE_TIME, DeltaFloat.MoveType.DECEL);
        var size = new DeltaFloat();
        size.Set(800f);
        size.MoveTo(10f, CHARGE_TIME, DeltaFloat.MoveType.DECEL);

        while (alpha.IsActive())
        {
            model.transform.localScale = new Vector3(size.Get(), size.Get(), 1);
            model.color = new Color(colorBase.r, colorBase.g, colorBase.b, alpha.Get());
            yield return null;
            alpha.Update(Time.deltaTime);
            size.Update(Time.deltaTime);

        }
    }
}
