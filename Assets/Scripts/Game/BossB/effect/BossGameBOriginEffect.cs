using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オリジンエフェクト
/// </summary>
public class BossGameBOriginEffect : BossGameBEffectBase
{
    /// <summary>
    /// 再生
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Play()
    {
        model.color = new Color(1, 1, 1, 0);
        var alpha = new DeltaFloat();
        var size = 1f;
        alpha.Set(1f);
        alpha.MoveTo(0f, 1f, DeltaFloat.MoveType.ACCEL);
        while (alpha.IsActive())
        {
            yield return null;
            alpha.Update(Time.deltaTime);

            size += Time.deltaTime * 1600f;
            model.transform.localScale = new Vector3(size, size, 1);
            model.color = new Color(1, 1, 1, alpha.Get());
        }

    }
}
