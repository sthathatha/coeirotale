using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ナイトメアエフェクト
/// </summary>
public class BossGameBHorrorEffect : BossGameBEffectBase
{
    public SpriteRenderer model2;
    public AudioClip se_shout;

    /// <summary>
    /// 再生
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Play()
    {
        model.color = new Color(1, 1, 1, 0);
        model2.color = new Color(1, 1, 1, 0);

        var alpha = new DeltaFloat();
        alpha.Set(0f);
        alpha.MoveTo(1f, 0.8f, DeltaFloat.MoveType.LINE);
        while (alpha.IsActive())
        {
            yield return null;
            alpha.Update(Time.deltaTime);
            model.color = new Color(1, 1, 1, alpha.Get());
        }
        yield return new WaitForSeconds(0.5f);

        ManagerSceneScript.GetInstance().soundMan.PlaySE(se_shout);
        alpha.Set(0f);
        alpha.MoveTo(1f, 0.5f, DeltaFloat.MoveType.BOTH);
        while (alpha.IsActive())
        {
            yield return null;
            alpha.Update(Time.deltaTime);
            model2.color = new Color(1, 1, 1, alpha.Get());
        }
        model.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        alpha.Set(1f);
        alpha.MoveTo(0f, 0.3f, DeltaFloat.MoveType.LINE);
        while (alpha.IsActive())
        {
            yield return null;
            alpha.Update(Time.deltaTime);
            model2.color = new Color(1, 1, 1, alpha.Get());
        }
    }
}
