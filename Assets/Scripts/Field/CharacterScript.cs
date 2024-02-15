using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterScript : MonoBehaviour
{
    /// <summary>フィールド</summary>
    public MainScriptBase field;
    /// <summary>モデル</summary>
    public GameObject model;

    /// <summary>モデルアニメーション</summary>
    protected Animator modelAnim;
    /// <summary>モデルSpriteRenderer</summary>
    protected SpriteRenderer spriteRenderer;

    /// <summary>
    /// 初期化
    /// </summary>
    virtual protected void Start()
    {
        modelAnim = model.GetComponent<Animator>();
        spriteRenderer = model.GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// フレーム処理
    /// </summary>
    virtual protected void Update()
    {
        var sortingOrder = Mathf.CeilToInt(-transform.position.y);
        if (spriteRenderer.sortingOrder != sortingOrder)
        {
            spriteRenderer.sortingOrder = sortingOrder;
        }
    }
}
