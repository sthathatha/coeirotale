using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F001壁
/// </summary>
public class F001Wall : ObjectBase
{
    public AudioClip moveSE;

    /// <summary>開いた時の座標</summary>
    private const float OPEN_Y = 56;
    /// <summary>閉じた時の座標</summary>
    private const float CLOSE_Y = -232;

    /// <summary>開いてる</summary>
    private bool opened = false;
    /// <summary>座標アニメーション用</summary>
    private DeltaVector3 pos;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public F001Wall()
    {
        pos = new DeltaVector3();
    }

    /// <summary>
    /// 開始時
    /// </summary>
    protected override void Start()
    {
        base.Start();

        pos.Set(new Vector3(transform.localPosition.x, CLOSE_Y, 0));
        transform.localPosition = pos.Get();
    }

    /// <summary>
    /// 開いてる
    /// </summary>
    /// <returns></returns>
    public bool IsOpened() { return opened; }

    /// <summary>
    /// 開く
    /// </summary>
    public void Open()
    {
        if (opened) return;
        opened = true;

        StartCoroutine(OpenCoroutine());
    }

    /// <summary>
    /// 開くコルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator OpenCoroutine()
    {
        yield return new WaitForSeconds(0.5f);

        var sound = ManagerSceneScript.GetInstance().soundMan;

        // ループSE再生
        var se = sound.PlaySELoop(moveSE);
        // 移動
        pos.MoveTo(new Vector3(transform.localPosition.x, OPEN_Y, 0), 3f, DeltaFloat.MoveType.LINE);
        while (pos.IsActive())
        {
            yield return null;
            pos.Update(Time.deltaTime);
            transform.localPosition = pos.Get();
        }

        sound.StopLoopSE(se, 0.5f);
    }
}
