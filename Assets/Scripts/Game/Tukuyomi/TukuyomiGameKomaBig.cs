using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// つくよみちゃん戦　大駒
/// </summary>
public class TukuyomiGameKomaBig : MonoBehaviour
{
    private const float DISAPPEAR_Y = 270f;

    #region メソッド

    /// <summary>
    /// 上から出てくる
    /// </summary>
    /// <param name="x"></param>
    public void Appear(float x)
    {
        var basePos = new Vector3(x, DISAPPEAR_Y);
        var appPos = new Vector3(x, 0);

        Move(basePos);
        Show(true);
        Move(appPos, 0.5f);
    }

    /// <summary>
    /// 上に消える
    /// </summary>
    public void Disappear()
    {
        var disPos = new Vector3(transform.localPosition.x, DISAPPEAR_Y);
        Move(disPos, 0.5f);
    }

    /// <summary>
    /// 表示切り替え
    /// </summary>
    /// <param name="show"></param>
    public void Show(bool show)
    {
        gameObject.SetActive(show);
    }

    /// <summary>
    /// 移動
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="time"></param>
    public void Move(Vector3 pos, float time = -1f)
    {
        if (time <= 0f)
        {
            transform.localPosition = pos;
        }
        else
        {
            StartCoroutine(MoveCoroutine(pos, time));
        }
    }

    /// <summary>
    /// 移動
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    private IEnumerator MoveCoroutine(Vector3 pos, float time)
    {
        var p = new DeltaVector3();
        p.Set(transform.localPosition);
        p.MoveTo(pos, time, DeltaFloat.MoveType.DECEL);
        while (p.IsActive())
        {
            yield return null;
            p.Update(Time.deltaTime);

            transform.localPosition = p.Get();
        }
    }

    #endregion
}
