using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F141ドア
/// </summary>
public class F141Door : ObjectBase
{
    /// <summary>モデルのアニメーション</summary>
    public Animator doorAnim;
    /// <summary>しまっているコリジョン</summary>
    public GameObject closeCollision;

    /// <summary>ドア種類</summary>
    public enum DoorType : int
    {
        Success = 0,
        Failed,
    }
    protected DoorType doorType;

    /// <summary>
    /// 正解・ハズレ設定
    /// </summary>
    /// <param name="type"></param>
    public void SetDoorType(DoorType type) { doorType = type; }
    /// <summary>
    /// 正解・ハズレ
    /// </summary>
    /// <returns></returns>
    public DoorType GetDoorType() { return doorType; }

    /// <summary>
    /// 左右反転表示
    /// </summary>
    /// <param name="mirror"></param>
    public void SetMirror(bool mirror)
    {
        model.GetComponent<SpriteRenderer>().flipX = mirror;
    }

    /// <summary>
    /// 開く
    /// </summary>
    /// <param name="immediate">true:一瞬</param>
    public void Open(bool immediate = false)
    {
        closeCollision.SetActive(false);

        if (immediate)
        {
            doorAnim.Play("open");
            return;
        }

        if (doorType == DoorType.Success)
        {
            doorAnim.Play("openInverse");
        }
        else
        {
            doorAnim.Play("openNormal");
        }
    }
}
