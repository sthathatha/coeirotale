using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F002壁
/// </summary>
public class F002Wall : ObjectBase
{
    /// <summary>上のY座標</summary>
    public float UpY;
    /// <summary>下のY座標</summary>
    public float DownY;

    /// <summary>現在位置true:下</summary>
    public bool isDown;

    /// <summary>座標</summary>
    private DeltaVector3 pos = new DeltaVector3();

    /// <summary>
    /// 初期化
    /// </summary>
    protected override void Start()
    {
        base.Start();

        // 初期値設定
        pos.Set(new Vector3(transform.localPosition.x, isDown ? DownY : UpY, 0));
        transform.localPosition = pos.Get();
    }

    /// <summary>
    /// 位置変更
    /// </summary>
    public void Toggle()
    {
        StartCoroutine(ToggleCoroutine());
    }

    /// <summary>
    /// 位置変更コルーチン
    /// </summary>
    /// <returns></returns>
    public IEnumerator ToggleCoroutine()
    {
        isDown = !isDown;

        pos.MoveTo(new Vector3(transform.localPosition.x, isDown ? DownY : UpY, 0), 2f, DeltaFloat.MoveType.LINE);
        while (pos.IsActive())
        {
            yield return null;

            pos.Update(Time.deltaTime);
            transform.localPosition = pos.Get();
        }
    }

    /// <summary>
    /// 移動中
    /// </summary>
    /// <returns></returns>
    public bool IsMoving()
    {
        return pos.IsActive();
    }
}
