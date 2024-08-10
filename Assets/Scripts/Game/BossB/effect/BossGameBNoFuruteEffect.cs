using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NO FUTURE
/// </summary>
public class BossGameBNoFuruteEffect : BossGameBEffectBase
{
    private const float OUT_X = 1280f;
    private const float CENTER_X = 180f;
    public AudioClip se_nofuture;

    /// <summary>
    /// çƒê∂
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Play()
    {
        var p = new DeltaVector3();
        p.Set(new Vector3(OUT_X + CENTER_X, 0, 0));
        p.MoveTo(new Vector3(CENTER_X, 0f), 0.6f, DeltaFloat.MoveType.LINE);
        transform.localPosition = p.Get();

        while (p.IsActive())
        {
            yield return null;
            p.Update(Time.deltaTime);
            transform.localPosition = p.Get();
        }
        ManagerSceneScript.GetInstance().soundMan.PlaySE(se_nofuture);
        yield return new WaitForSeconds(0.6f);

        p.MoveTo(new Vector3(-OUT_X + CENTER_X, 0, 0), 0.6f, DeltaFloat.MoveType.LINE);
        while (p.IsActive())
        {
            yield return null;
            p.Update(Time.deltaTime);
            transform.localPosition = p.Get();
        }
    }
}
