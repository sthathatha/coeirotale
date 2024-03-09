using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PierreGameRoadObject : MonoBehaviour
{
    /// <summary>道幅プラスマイナス</summary>
    public const float ROAD_FAR_MAX = 80f;
    /// <summary>オブジェクトが当たる距離</summary>
    public const float ROAD_HIT_DISTANCE = 20f;

    public PierreGameSystemA system = null;

    /// <summary>道幅移動時に表示を上下させる親オブジェクト</summary>
    public GameObject renderParent = null;
    /// <summary>道幅移動時に描画順を設定するSprite</summary>
    public SpriteRenderer priorityRender = null;

    /// <summary>道幅の位置</summary>
    private float farPosition = 0f;
    /// <summary>描画順設定Sprite</summary>
    private List<SpriteRenderer> priorityRenderList = null;

    /// <summary>
    /// 初期化
    /// </summary>
    virtual public void Start()
    {
        priorityRenderList = new List<SpriteRenderer>();
        if (priorityRender != null)
        {
            AddRenderList(priorityRender);
        }

        SetFarPosition(farPosition);
    }

    /// <summary>
    /// 描画順操作リスト追加
    /// </summary>
    /// <param name="sprite"></param>
    protected void AddRenderList(SpriteRenderer sprite)
    {
        priorityRenderList.Add(sprite);
    }

    /// <summary>
    /// フレーム処理
    /// </summary>
    virtual public void Update()
    {
    }

    /// <summary>
    /// 道の位置取得
    /// </summary>
    /// <returns></returns>
    public float GetFarPosition() { return farPosition; }
    /// <summary>
    /// 道の位置を設定
    /// </summary>
    /// <param name="_far"></param>
    public virtual void SetFarPosition(float _far)
    {
        farPosition = _far;
        farPosition = Mathf.Clamp(farPosition, -ROAD_FAR_MAX, ROAD_FAR_MAX);

        var pos = renderParent.transform.localPosition;
        pos.y = farPosition;
        renderParent.transform.localPosition = pos;

        if (priorityRenderList != null)
        {
            var order = CalcBaseSortingOrder();
            foreach (var sprite in priorityRenderList)
            {
                sprite.sortingOrder = order;
            }
        }
    }

    /// <summary>
    /// 位置による描画順の決定
    /// </summary>
    /// <returns>およそ0～1600</returns>
    protected int CalcBaseSortingOrder()
    {
        return Mathf.FloorToInt((farPosition - ROAD_FAR_MAX) * -10);
    }

    /// <summary>
    /// 位置によって当たっているかどうか
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool IsHit(PierreGameRoadObject other)
    {
        var abs = Mathf.Abs(GetFarPosition() - other.GetFarPosition());
        return abs <= ROAD_HIT_DISTANCE;
    }
}
