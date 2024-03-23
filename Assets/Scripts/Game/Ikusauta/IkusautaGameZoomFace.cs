using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 上下の顔画像
/// </summary>
public class IkusautaGameZoomFace : MonoBehaviour
{
    /// <summary>表示場所</summary>
    private enum Location : int
    {
        /// <summary>上</summary>
        Top = 0,
        /// <summary>下</summary>
        Bottom,
    }

    /// <summary>表示場所</summary>
    public int location;

    /// <summary>実行中</summary>
    private bool _active = false;

    /// <summary>
    /// 開始
    /// </summary>
    /// <returns></returns>
    public IEnumerator PlayAnim()
    {
        _active = true;
        var loc = (Location)location;
        var x = new DeltaFloat();
        var locSign = loc == Location.Top ? 1 : -1;
        var y = locSign * 260;

        // 初期位置
        x.Set(locSign * -640);
        x.MoveTo(locSign * 640, 1f, DeltaFloat.MoveType.LINE);

        // 移動
        while (x.IsActive())
        {
            x.Update(Time.deltaTime);
            transform.localPosition = new Vector3(x.Get(), y, 0);

            yield return null;
        }
        transform.localPosition = new Vector3(x.Get(), y, 0);

        // おわり
        _active = false;
    }

    /// <summary>
    /// アニメーション中
    /// </summary>
    /// <returns></returns>
    public bool IsActive()
    {
        return _active;
    }
}
