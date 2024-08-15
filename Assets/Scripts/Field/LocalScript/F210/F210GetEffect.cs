using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 声をもらうエフェクト
/// </summary>
public class F210GetEffect : MonoBehaviour
{
    public AudioClip se_get;

    /// <summary>
    /// 再生
    /// </summary>
    /// <param name="color"></param>
    /// <param name="pos"></param>
    public IEnumerator PlayEffect(Color color, Vector3 pos)
    {
        gameObject.SetActive(true);
        transform.localPosition = pos + new Vector3(0, 64f);
        ManagerSceneScript.GetInstance().soundMan.PlaySE(se_get);

        var alpha = new DeltaFloat();
        alpha.Set(0);
        alpha.MoveTo(0.7f, 1.5f, DeltaFloat.MoveType.LINE);
        var r = new DeltaFloat();
        r.Set(640f);
        r.MoveTo(1f, 1.5f, DeltaFloat.MoveType.LINE);
        while (alpha.IsActive())
        {
            transform.localScale = new Vector3(r.Get(), r.Get(), 1);
            GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, alpha.Get());

            yield return null;
        }

        gameObject.SetActive(false);
    }
}
