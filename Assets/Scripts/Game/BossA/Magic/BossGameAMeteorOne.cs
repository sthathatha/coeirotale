using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ＊＊＊＊　メテオ隕石1個
/// </summary>
public class BossGameAMeteorOne : MonoBehaviour
{
    /// <summary>Yの開始終了座標</summary>
    private const float BASE_Y = 400f;
    /// <summary>Xの開始座標</summary>
    private const float BASE_X = 300f;
    /// <summary>Xの開始座標から左右ランダム範囲</summary>
    private const float START_X_RANGE = 600f;
    /// <summary>Xの終了座標から左右ランダム範囲</summary>
    private const float FALL_X_RANGE = 50f;

    /// <summary>
    /// 落下開始
    /// </summary>
    public void Fall()
    {
        gameObject.SetActive(true);
        StartCoroutine(FallCoroutine());
    }

    /// <summary>
    /// 落下制御コルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator FallCoroutine()
    {
        var pos = new DeltaVector3();
        var st = new Vector3(BASE_X + Util.RandomFloat(-START_X_RANGE, START_X_RANGE), BASE_Y);
        pos.Set(st);
        var ed = new Vector3(st.x - BASE_X * 2 + Util.RandomFloat(-FALL_X_RANGE, FALL_X_RANGE), -BASE_Y);
        pos.MoveTo(ed, 0.4f, DeltaFloat.MoveType.LINE);
        while (pos.IsActive())
        {
            transform.localPosition = pos.Get();

            yield return null;
        }

        Destroy(gameObject);
    }
}
