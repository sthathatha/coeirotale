using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SeedScript : MonoBehaviour
{
    #region 定数
    /// <summary>取れなくなる数字</summary>
    public const int SEED_MAX_NUM = 10;
    #endregion

    #region メンバー
    /// <summary>画像</summary>
    public SpriteRenderer image;
    /// <summary>成長数字</summary>
    public TMP_Text valueText = null;

    /// <summary>0の時</summary>
    public Sprite image0 = null;
    /// <summary>1の時</summary>
    public Sprite image1 = null;
    /// <summary>2の時</summary>
    public Sprite image2 = null;
    /// <summary>3の時</summary>
    public Sprite image3 = null;
    /// <summary>4の時</summary>
    public Sprite image4 = null;
    /// <summary>5の時</summary>
    public Sprite image5 = null;
    #endregion

    #region プライベート
    /// <summary>現在値</summary>
    private int nowValue = 0;
    #endregion

    /// <summary>
    /// 育つ
    /// </summary>
    /// <param name="num"></param>
    public void GrowNum(int num)
    {
        if (num == 0) { return; }

        var grow = nowValue + num;
        if (grow > SEED_MAX_NUM)
        {
            grow = SEED_MAX_NUM;
        }

        SetNum(grow);
    }

    /// <summary>
    /// 数値設定
    /// </summary>
    /// <param name="num"></param>
    public void SetNum(int num)
    {
        nowValue = num;
        valueText.SetText(num.ToString());

        var rate = num * 5 / SEED_MAX_NUM;
        switch (rate)
        {
            case 0:
                image.sprite = image0;
                break;
            case 1:
                image.sprite = image1;
                break;
            case 2:
                image.sprite = image2;
                break;
            case 3:
                image.sprite = image3;
                break;
            case 4:
                image.sprite = image4;
                break;
            case 5:
                image.sprite = image5;
                break;
        }
    }

    /// <summary>現在値</summary>
    /// <returns></returns>
    public int GetNum() { return nowValue; }

    /// <summary>
    /// 取り除く
    /// </summary>
    public void Pick()
    {
        nowValue = -1;
        image.gameObject.SetActive(false);
        valueText.gameObject.SetActive(false);
    }

    /// <summary>
    /// 選択可能かどうか
    /// </summary>
    public bool IsEnable()
    {
        return nowValue >= 0 && nowValue < SEED_MAX_NUM;
    }
}
