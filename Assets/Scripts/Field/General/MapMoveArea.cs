using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 触るとマップ移動する
/// </summary>
public class MapMoveArea : AreaEventBase
{
    /// <summary>移動先のScene名</summary>
    public string targetMap;

    /// <summary>移動先SceneのGeneralPosition</summary>
    public int targetPos;

    /// <summary>
    /// 移動実行
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Exec()
    {
        ManagerSceneScript.GetInstance().LoadMainScene(targetMap, targetPos);
        yield break;
    }

    public override void Start()
    {
        base.Start();

        // エディタ用のSpriteは消す
        var sprite = GetComponent<SpriteRenderer>();
        if (sprite != null)
        {
            sprite.enabled = false;
        }
    }
}
