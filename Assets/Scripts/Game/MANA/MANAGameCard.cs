using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カード１枚管理クラス
/// </summary>
public class MANAGameCard : MonoBehaviour
{
    #region 定数
    /// <summary>
    /// マーク
    /// </summary>
    public enum Suit : int
    {
        Spade = 0,
        Heart,
        Dia,
        Crub,
    }
    #endregion

    #region メンバー

    public SpriteRenderer obj_card_base;
    public SpriteRenderer obj_card_num;
    public SpriteRenderer obj_card_suit;
    public SpriteRenderer obj_card_pic;

    public Sprite card_base_image;
    public Sprite card_back_image;
    public Sprite card_num_1;
    public Sprite card_num_2;
    public Sprite card_num_3;
    public Sprite card_num_4;
    public Sprite card_num_5;
    public Sprite card_num_6;
    public Sprite card_num_7;
    public Sprite card_num_8;
    public Sprite card_num_9;
    public Sprite card_num_10;
    public Sprite card_num_11;
    public Sprite card_num_12;
    public Sprite card_num_13;
    public Sprite card_suit_sp;
    public Sprite card_suit_he;
    public Sprite card_suit_di;
    public Sprite card_suit_cr;
    public Sprite card_suit_sp2;
    public Sprite card_suit_he2;
    public Sprite card_suit_di2;
    public Sprite card_suit_cr2;
    public Sprite card_pic_11_sp;
    public Sprite card_pic_12_sp;
    public Sprite card_pic_13_sp;
    public Sprite card_pic_11_he;
    public Sprite card_pic_12_he;
    public Sprite card_pic_13_he;
    public Sprite card_pic_11_di;
    public Sprite card_pic_12_di;
    public Sprite card_pic_13_di;
    public Sprite card_pic_11_cr;
    public Sprite card_pic_12_cr;
    public Sprite card_pic_13_cr;

    #endregion

    #region プライベート

    /// <summary>マーク</summary>
    private Suit _suit;
    /// <summary>数字</summary>
    private int _num;
    /// <summary>位置管理</summary>
    private DeltaVector3 _movePos = new DeltaVector3();

    #endregion

    #region パブリックメソッド
    /// <summary>
    /// 移動
    /// </summary>
    /// <param name="target">移動先</param>
    /// <param name="time">移動時間</param>
    public void MoveTo(Vector3 target, float time = -1f)
    {
        if (time <= 0f)
        {
            transform.localPosition = target;
        }
        else
        {
            _movePos.Set(transform.localPosition);
            _movePos.MoveTo(target, time, DeltaFloat.MoveType.LINE);
        }
    }

    /// <summary>
    /// 動作中
    /// </summary>
    /// <returns></returns>
    public bool IsMoving() { return _movePos.IsActive(); }

    /// <summary>
    /// マーク
    /// </summary>
    /// <returns></returns>
    public Suit GetSuit() { return _suit; }
    /// <summary>
    /// 数値
    /// </summary>
    /// <returns></returns>
    public int GetNum() { return _num; }

    /// <summary>
    /// フレーム処理
    /// </summary>
    public void Update()
    {
        if (_movePos.IsActive())
        {
            _movePos.Update(Time.deltaTime);
            transform.localPosition = _movePos.Get();
        }
    }

    /// <summary>
    /// カード内容設定
    /// </summary>
    /// <param name="s"></param>
    /// <param name="n"></param>
    public void SetCard(Suit s, int n)
    {
        _suit = s;
        _num = n;

        UpdateCardImages();
    }

    /// <summary>
    /// カード表裏表示
    /// </summary>
    /// <param name="fore">true:表</param>
    public void ShowCard(bool fore)
    {
        if (fore)
        {
            obj_card_base.sprite = card_base_image;
            obj_card_num.gameObject.SetActive(true);
            obj_card_suit.gameObject.SetActive(true);
            obj_card_pic.gameObject.SetActive(_num > 10);
        }
        else
        {
            obj_card_base.sprite = card_back_image;
            obj_card_num.gameObject.SetActive(false);
            obj_card_suit.gameObject.SetActive(false);
            obj_card_pic.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 表示優先度設定
    /// </summary>
    /// <param name="priority"></param>
    public void SetPriority(int priority)
    {
        obj_card_base.sortingOrder = priority;
        obj_card_num.sortingOrder = priority + 1;
        obj_card_suit.sortingOrder = priority + 1;
        obj_card_pic.sortingOrder = priority + 1;
    }

    /// <summary>
    /// 表示優先度取得
    /// </summary>
    /// <returns></returns>
    public int GetPriority()
    {
        return obj_card_base.sortingOrder;
    }

    #endregion

    #region プライベートメソッド

    /// <summary>
    /// カード画像更新
    /// </summary>
    private void UpdateCardImages()
    {
        obj_card_num.sprite = _num switch
        {
            1 => card_num_1,
            2 => card_num_2,
            3 => card_num_3,
            4 => card_num_4,
            5 => card_num_5,
            6 => card_num_6,
            7 => card_num_7,
            8 => card_num_8,
            9 => card_num_9,
            10 => card_num_10,
            11 => card_num_11,
            12 => card_num_12,
            _ => card_num_13,
        };

        if (_num > 10)
        {
            obj_card_suit.sprite = _suit switch
            {
                Suit.Spade => card_suit_sp2,
                Suit.Heart => card_suit_he2,
                Suit.Dia => card_suit_di2,
                _ => card_suit_cr2,
            };

            obj_card_pic.sprite = (_suit, _num) switch
            {
                (Suit.Spade, 11) => card_pic_11_sp,
                (Suit.Spade, 12) => card_pic_12_sp,
                (Suit.Spade, 13) => card_pic_13_sp,
                (Suit.Heart, 11) => card_pic_11_he,
                (Suit.Heart, 12) => card_pic_12_he,
                (Suit.Heart, 13) => card_pic_13_he,
                (Suit.Dia, 11) => card_pic_11_di,
                (Suit.Dia, 12) => card_pic_12_di,
                (Suit.Dia, 13) => card_pic_13_di,
                (Suit.Crub, 11) => card_pic_11_cr,
                (Suit.Crub, 12) => card_pic_12_cr,
                _ => card_pic_13_cr,
            };
        }
        else
        {
            obj_card_suit.sprite = _suit switch
            {
                Suit.Spade => card_suit_sp,
                Suit.Heart => card_suit_he,
                Suit.Dia => card_suit_di,
                _ => card_suit_cr,
            };
        }

        if (_suit == Suit.Spade || _suit == Suit.Crub)
        {
            obj_card_num.color = Color.black;
            obj_card_suit.color = Color.black;
        }
        else
        {
            obj_card_num.color = Color.red;
            obj_card_suit.color = Color.red;
        }
    }

    #endregion
}
