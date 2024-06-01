using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// フィールドオブジェクト
/// </summary>
public class ObjectBase : MonoBehaviour
{
    /// <summary>モデル</summary>
    public GameObject model;

    /// <summary>フィールド</summary>
    protected MainScriptBase fieldScript;

    /// <summary>描画順を設定するリスト</summary>
    protected List<SpriteRenderer> priorityTargetList;

    /// <summary>
    /// 開始時
    /// </summary>
    virtual protected void Start()
    {
        priorityTargetList = new List<SpriteRenderer>();

        //スプライト取得
        var sprite = model?.GetComponent<SpriteRenderer>();
        if (sprite != null)
        {
            AddPriorityList(sprite);
        }

        // フィールドスクリプト　設定が面倒なので基本的な構造なら取得
        var objects = gameObject.scene.GetRootGameObjects();
        foreach (var obj in objects)
        {
            var sys = obj.GetComponent<MainScriptBase>();
            if (sys != null)
            {
                fieldScript = sys;
                break;
            }
        }
    }

    /// <summary>
    /// 更新
    /// </summary>
    virtual protected void Update()
    {
        //スプライトの表示オーダーを位置にあわせて更新
        if (priorityTargetList.Count > 0)
        {
            var sortingOrder = Mathf.CeilToInt(-transform.position.y);
            foreach (var s in priorityTargetList)
            {
                if (s.sortingOrder != sortingOrder)
                {
                    s.sortingOrder = sortingOrder;
                }
            }
        }
    }

    /// <summary>
    /// 描画順設定Spriteの追加
    /// </summary>
    /// <param name="sprite"></param>
    protected void AddPriorityList(SpriteRenderer sprite)
    {
        sprite.sortingLayerName = "FieldObject";
        priorityTargetList.Add(sprite);
    }
}
