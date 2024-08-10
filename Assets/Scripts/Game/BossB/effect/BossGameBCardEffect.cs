using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ラスボス本戦　トランプエフェクト
/// </summary>
public class BossGameBCardEffect : BossGameBEffectBase
{
    private const float X_INTERVAL = 180f;
    public const float DELAY_ONE = 0.2f;
    public enum Yaku
    {
        /// <summary>ロイヤルストレートフラッシュ</summary>
        Loyal = 0,
        /// <summary>ストレートフラッシュ</summary>
        StraightFlash,
        /// <summary>フォーカード</summary>
        FourCard,
        /// <summary>フルハウス</summary>
        FullHouse,
        /// <summary>フラッシュ</summary>
        Flash,
        /// <summary>ストレート</summary>
        Straight,
        /// <summary>スリーカード</summary>
        ThreeCard,
        /// <summary>ツーペア</summary>
        TwoPair,
        /// <summary>ワンペア</summary>
        OnePair,
        /// <summary>ブタ</summary>
        Boo,
    }

    public SpriteRenderer model_omote;
    public SpriteRenderer model_num;
    public SpriteRenderer model_suit;
    public SpriteRenderer model_pic;

    #region 素材

    public Sprite sp_num_1;
    public Sprite sp_num_2;
    public Sprite sp_num_3;
    public Sprite sp_num_4;
    public Sprite sp_num_5;
    public Sprite sp_num_6;
    public Sprite sp_num_7;
    public Sprite sp_num_8;
    public Sprite sp_num_9;
    public Sprite sp_num_10;
    public Sprite sp_num_11;
    public Sprite sp_num_12;
    public Sprite sp_num_13;
    public Sprite sp_pic_11_sp;
    public Sprite sp_pic_12_sp;
    public Sprite sp_pic_13_sp;
    public Sprite sp_pic_11_he;
    public Sprite sp_pic_12_he;
    public Sprite sp_pic_13_he;
    public Sprite sp_pic_11_di;
    public Sprite sp_pic_12_di;
    public Sprite sp_pic_13_di;
    public Sprite sp_pic_11_cr;
    public Sprite sp_pic_12_cr;
    public Sprite sp_pic_13_cr;

    public Sprite sp_suit_sp1;
    public Sprite sp_suit_sp2;
    public Sprite sp_suit_he1;
    public Sprite sp_suit_he2;
    public Sprite sp_suit_di1;
    public Sprite sp_suit_di2;
    public Sprite sp_suit_cr1;
    public Sprite sp_suit_cr2;

    #endregion

    private Vector3 p1;
    private Vector3 p2;
    private float delayTime;

    /// <summary>
    /// 設定
    /// </summary>
    /// <param name="index"></param>
    /// <param name="num"></param>
    /// <param name="suit"></param>
    /// <param name="pos1"></param>
    public void SetParam(int index, int num, MANAGameCard.Suit suit, Vector3 pos1)
    {
        var basePos = BossGameSystemB.GetCellPosition(new Vector2Int(BossGameSystemB.CELL_X_COUNT, BossGameSystemB.CELL_Y_COUNT) / 2);
        transform.localPosition = pos1;

        p1 = pos1;
        p2 = basePos + new Vector3((index - 2) * X_INTERVAL, 0);
        delayTime = (4 - index) * DELAY_ONE;
        var col = (suit == MANAGameCard.Suit.Spade || suit == MANAGameCard.Suit.Crub) ? Color.black : Color.red;

        var numList = new List<Sprite>
        {
            sp_num_1,
            sp_num_2,
            sp_num_3,
            sp_num_4,
            sp_num_5,
            sp_num_6,
            sp_num_7,
            sp_num_8,
            sp_num_9,
            sp_num_10,
            sp_num_11,
            sp_num_12,
            sp_num_13,
        };
        model_num.sprite = numList[num - 1];
        model_num.color = col;
        model_suit.color = col;

        if (num <= 10)
        {
            model_suit.sprite = suit switch
            {
                MANAGameCard.Suit.Spade => sp_suit_sp1,
                MANAGameCard.Suit.Heart => sp_suit_he1,
                MANAGameCard.Suit.Dia => sp_suit_di1,
                _ => sp_suit_cr1,
            };
            model_pic.color = new Color(0, 0, 0, 0);
        }
        else
        {
            model_suit.sprite = suit switch
            {
                MANAGameCard.Suit.Spade => sp_suit_sp2,
                MANAGameCard.Suit.Heart => sp_suit_he2,
                MANAGameCard.Suit.Dia => sp_suit_di2,
                _ => sp_suit_cr2,
            };
            model_pic.color = Color.white;

            if (num == 11)
            {
                model_pic.sprite = suit switch
                {
                    MANAGameCard.Suit.Spade => sp_pic_11_sp,
                    MANAGameCard.Suit.Heart => sp_pic_11_he,
                    MANAGameCard.Suit.Dia => sp_pic_11_di,
                    _ => sp_pic_11_cr,
                };
            }
            else if (num == 12)
            {
                model_pic.sprite = suit switch
                {
                    MANAGameCard.Suit.Spade => sp_pic_12_sp,
                    MANAGameCard.Suit.Heart => sp_pic_12_he,
                    MANAGameCard.Suit.Dia => sp_pic_12_di,
                    _ => sp_pic_12_cr,
                };
            }
            else
            {
                model_pic.sprite = suit switch
                {
                    MANAGameCard.Suit.Spade => sp_pic_13_sp,
                    MANAGameCard.Suit.Heart => sp_pic_13_he,
                    MANAGameCard.Suit.Dia => sp_pic_13_di,
                    _ => sp_pic_13_cr,
                };
            }
        }
    }

    /// <summary>
    /// 再生
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Play()
    {
        model_num.sortingLayerID = model.sortingLayerID;
        model_omote.sortingLayerID = model.sortingLayerID;
        model_pic.sortingLayerID = model.sortingLayerID;
        model_suit.sortingLayerID = model.sortingLayerID;

        model_omote.sortingOrder = model.sortingOrder;
        model_num.sortingOrder = model.sortingOrder + 1;
        model_pic.sortingOrder = model.sortingOrder + 1;
        model_suit.sortingOrder = model.sortingOrder + 1;

        var pos = new DeltaVector3();
        pos.Set(p1);
        pos.MoveTo(p2, 0.1f, DeltaFloat.MoveType.LINE);
        while (pos.IsActive())
        {
            yield return null;
            pos.Update(Time.deltaTime);
            transform.localPosition = pos.Get();

        }
        yield return new WaitForSeconds(0.4f);
        model.gameObject.SetActive(false);
        model_omote.gameObject.SetActive(true);
        model_num.gameObject.SetActive(true);
        model_suit.gameObject.SetActive(true);
        model_pic.gameObject.SetActive(true);
        yield return new WaitForSeconds(delayTime + 0.5f);
        //pos.MoveTo(p2 + new Vector3(0, -800f), 0.3f, DeltaFloat.MoveType.LINE);
    }

    /// <summary>
    /// 役に必要なカード５枚を決定
    /// </summary>
    /// <param name="yaku"></param>
    /// <returns></returns>
    public static List<CardParam> DecideCard(Yaku yaku)
    {
        var list = new List<CardParam>();
        var randSuitIdx = Util.RandomUniqueIntList(0, 3, 4);
        var s1 = (MANAGameCard.Suit)randSuitIdx[0];
        var s2 = (MANAGameCard.Suit)randSuitIdx[1];
        var s3 = (MANAGameCard.Suit)randSuitIdx[2];
        var s4 = (MANAGameCard.Suit)randSuitIdx[3];

        switch (yaku)
        {
            case Yaku.Loyal:
                list.Add(new CardParam(1, s1));
                list.Add(new CardParam(13, s1));
                list.Add(new CardParam(12, s1));
                list.Add(new CardParam(11, s1));
                list.Add(new CardParam(10, s1));
                break;
            case Yaku.StraightFlash:
                {
                    var head = Util.RandomInt(1, 9);
                    list.Add(new CardParam(head, s1));
                    list.Add(new CardParam(head + 1, s1));
                    list.Add(new CardParam(head + 2, s1));
                    list.Add(new CardParam(head + 3, s1));
                    list.Add(new CardParam(head + 4, s1));
                }
                break;
            case Yaku.FourCard:
                {
                    var head = Util.RandomInt(1, 13);
                    list.Add(new CardParam(head, s1));
                    list.Add(new CardParam(head, s2));
                    list.Add(new CardParam(head, s3));
                    list.Add(new CardParam(head, s4));
                    var ex = head + Util.RandomInt(1, 12);
                    if (ex > 13) ex -= 13;
                    list.Add(new CardParam(ex, s3));
                }
                break;
            case Yaku.FullHouse:
                {
                    var head = Util.RandomInt(1, 13);
                    list.Add(new CardParam(head, s1));
                    list.Add(new CardParam(head, s2));
                    list.Add(new CardParam(head, s4));
                    var ex = head + Util.RandomInt(1, 12);
                    if (ex > 13) ex -= 13;
                    list.Add(new CardParam(ex, s1));
                    list.Add(new CardParam(ex, s3));
                }
                break;
            case Yaku.Flash:
                {
                    var head = Util.RandomInt(1, 13);
                    list.Add(new CardParam(head, s1));
                    head += 12;
                    if (head > 13) head -= 13;
                    list.Add(new CardParam(head, s1));
                    head += 10;
                    if (head > 13) head -= 13;
                    list.Add(new CardParam(head, s1));
                    head += 10;
                    if (head > 13) head -= 13;
                    list.Add(new CardParam(head, s1));
                    head += 9;
                    if (head > 13) head -= 13;
                    list.Add(new CardParam(head, s1));
                }
                break;
            case Yaku.Straight:
                {
                    var head = Util.RandomInt(1, 9);
                    list.Add(new CardParam(head, s3));
                    list.Add(new CardParam(head + 1, s1));
                    list.Add(new CardParam(head + 2, s2));
                    list.Add(new CardParam(head + 3, s4));
                    list.Add(new CardParam(head + 4, s2));
                }
                break;
            case Yaku.ThreeCard:
                {
                    var head = Util.RandomInt(1, 13);
                    list.Add(new CardParam(head, s1));
                    list.Add(new CardParam(head, s2));
                    list.Add(new CardParam(head, s3));
                    head += 2;
                    if (head > 13) head -= 13;
                    list.Add(new CardParam(head, s3));
                    head += 7;
                    if (head > 13) head -= 13;
                    list.Add(new CardParam(head, s1));
                }
                break;
            case Yaku.TwoPair:
                {
                    var head = Util.RandomInt(1, 13);
                    list.Add(new CardParam(head, s1));
                    list.Add(new CardParam(head, s2));
                    head += 6;
                    if (head > 13) head -= 13;
                    list.Add(new CardParam(head, s3));
                    list.Add(new CardParam(head, s1));
                    head += 3;
                    if (head > 13) head -= 13;
                    list.Add(new CardParam(head, s4));
                }
                break;
            case Yaku.OnePair:
                {
                    var head = Util.RandomInt(1, 13);
                    list.Add(new CardParam(head, s1));
                    list.Add(new CardParam(head, s2));
                    head += 12;
                    if (head > 13) head -= 13;
                    list.Add(new CardParam(head, s3));
                    head += 6;
                    if (head > 13) head -= 13;
                    list.Add(new CardParam(head, s3));
                    head += 2;
                    if (head > 13) head -= 13;
                    list.Add(new CardParam(head, s4));
                }
                break;
            default:
                {
                    var head = Util.RandomInt(1, 13);
                    list.Add(new CardParam(head, s1));
                    head += 12;
                    if (head > 13) head -= 13;
                    list.Add(new CardParam(head, s1));
                    head += 12;
                    if (head > 13) head -= 13;
                    list.Add(new CardParam(head, s1));
                    head += 9;
                    if (head > 13) head -= 13;
                    list.Add(new CardParam(head, s3));
                    head += 9;
                    if (head > 13) head -= 13;
                    list.Add(new CardParam(head, s4));
                }
                break;
        }

        return list;
    }

    /// <summary>
    /// カードパラメータ
    /// </summary>
    public class CardParam
    {
        public int num { get; set; } = 1;
        public MANAGameCard.Suit suit { get; set; } = MANAGameCard.Suit.Spade;

        public CardParam(int _num, MANAGameCard.Suit _suit) { num = _num; suit = _suit; }
    }
}
