using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ＊＊＊＊戦　プレイヤーキャラ共通処理
/// </summary>
public class BossGameAPlayers : MonoBehaviour
{
    protected const float LEFT_X = -80;

    public SpriteRenderer model;
    public SpriteRenderer magicEff;

    /// <summary>魔法エフェクトの音</summary>
    public AudioClip magicSe;

    /// <summary>立ちポーズ</summary>
    public Sprite image_stand;
    /// <summary>歩きポーズ</summary>
    public Sprite image_walk;
    /// <summary>魔法ポーズ</summary>
    public Sprite image_magic;

    /// <summary>魔法エフェクト閉じ</summary>
    public Sprite image_spell0;
    /// <summary>魔法エフェクト開</summary>
    public Sprite image_spell1;

    /// <summary>X位置</summary>
    private DeltaFloat walkX = new DeltaFloat();

    /// <summary>
    /// 左に出て魔法ポーズ
    /// </summary>
    /// <returns></returns>
    public IEnumerator ToLeft()
    {
        walkX.Set(model.transform.localPosition.x);
        walkX.MoveTo(LEFT_X, 0.2f, DeltaFloat.MoveType.LINE);

        model.flipX = false;
        model.sprite = image_walk;
        while (walkX.IsActive())
        {
            yield return null;
            walkX.Update(Time.deltaTime);
            model.transform.localPosition = new Vector3(walkX.Get(), 0);
        }
        model.sprite = image_magic;
    }

    /// <summary>
    /// 右に戻って通常ポーズ
    /// </summary>
    /// <returns></returns>
    public IEnumerator ToRight()
    {
        walkX.Set(model.transform.localPosition.x);
        walkX.MoveTo(0, 0.2f, DeltaFloat.MoveType.LINE);

        model.flipX = true;
        model.sprite = image_walk;
        while (walkX.IsActive())
        {
            yield return null;
            walkX.Update(Time.deltaTime);
            model.transform.localPosition = new Vector3(walkX.Get(), 0);
        }
        model.flipX = false;
        model.sprite = image_stand;
    }
}
