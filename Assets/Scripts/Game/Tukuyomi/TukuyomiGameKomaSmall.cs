using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// つくよみちゃん戦　小コマ
/// </summary>
public class TukuyomiGameKomaSmall : MonoBehaviour
{
    #region 定数

    public readonly Color COLOR_OU = new Color(0.8f, 0.9f, 1);
    public readonly Color COLOR_HISYA = new Color(1, 0.5f, 0.5f);
    public readonly Color COLOR_KAKU = new Color(0.5f, 0.5f, 1);
    public readonly Color COLOR_KIN = new Color(1, 0.85f, 1);
    public readonly Color COLOR_GIN = new Color(0.6f, 0.6f, 0.6f);
    public readonly Color COLOR_KEI = new Color(0.65f, 0.4f, 0.3f);
    public readonly Color COLOR_KYOU = new Color(0.5f, 1, 0.5f);
    public readonly Color COLOR_HU = Color.white;

    #endregion

    /// <summary>システムアクセス用</summary>
    public TukuyomiGameSystem system;

    public Vector2 CellLocation { get; set; }

    /// <summary>コマの種類</summary>
    private TukuyomiGameSystem.Koma komaKind;

    #region メソッド

    /// <summary>
    /// 色設定
    /// </summary>
    /// <param name="koma"></param>
    public void SetKoma(TukuyomiGameSystem.Koma koma)
    {
        komaKind = koma;

        GetComponent<SpriteRenderer>().color = koma switch
        {
            TukuyomiGameSystem.Koma.Ou => COLOR_OU,
            TukuyomiGameSystem.Koma.Hisya => COLOR_HISYA,
            TukuyomiGameSystem.Koma.Kaku => COLOR_KAKU,
            TukuyomiGameSystem.Koma.Kin => COLOR_KIN,
            TukuyomiGameSystem.Koma.Gin => COLOR_GIN,
            TukuyomiGameSystem.Koma.Kei => COLOR_KEI,
            TukuyomiGameSystem.Koma.Kyou => COLOR_KYOU,
            _ => COLOR_HU,
        };
    }

    /// <summary>
    /// ダメージ受ける判定を有効化
    /// </summary>
    public void EnableDamageMode()
    {
        GetComponent<Collider2D>().enabled = true;
        GetComponent<Rigidbody2D>().simulated = true;
    }

    #endregion

    /// <summary>
    /// ダメージうけた場合
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (komaKind)
        {
            case TukuyomiGameSystem.Koma.Kyou:
                // システム通知
                system.resource.PlaySE(system.resource.se_koma_damage);
                gameObject.SetActive(false);
                break;
        }
    }
}
