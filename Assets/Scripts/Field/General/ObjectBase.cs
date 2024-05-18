using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// フィールドオブジェクト
/// </summary>
public class ObjectBase : MonoBehaviour
{
    /// <summary>フィールド</summary>
    public MainScriptBase fieldScript;
    /// <summary>モデル</summary>
    public GameObject model;

    /// <summary>モデルSpriteRenderer</summary>
    protected SpriteRenderer spriteRenderer;

    /// <summary>
    /// 開始時
    /// </summary>
    virtual protected void Start()
    {
        //スプライト取得
        spriteRenderer = model?.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingLayerName = "FieldObject";
        }

        if (fieldScript == null)
        {
            // 設定が面倒なので基本的な構造なら取得
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
    }

    /// <summary>
    /// 更新
    /// </summary>
    virtual protected void Update()
    {
        //スプライトの表示オーダーを位置にあわせて更新
        if (spriteRenderer != null)
        {
            var sortingOrder = Mathf.CeilToInt(-transform.position.y);
            if (spriteRenderer.sortingOrder != sortingOrder)
            {
                spriteRenderer.sortingOrder = sortingOrder;
            }
        }
    }
}
