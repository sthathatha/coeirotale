using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エンディング用画像表示
/// </summary>
public class EndingPicture : MonoBehaviour
{
    private const float FADE_TIME = 1f;

    private readonly Vector3 POS_AMI = new Vector3(-548, 4, 0);
    private readonly Vector3 POS_MANA = new Vector3(-591, 87, 0);
    private readonly Vector3 POS_MATI = new Vector3(-586, 145, 0);
    private readonly Vector3 POS_MENDERU = new Vector3(-23, 150, 0);
    private readonly Vector3 POS_MATUKA = new Vector3(-325, 66, 0);
    private readonly Vector3 POS_PIERRE = new Vector3(64, 114, 0);
    private readonly Vector3 POS_TUKUYOMI = new Vector3(-313, 52, 0);

    /// <summary>
    /// 写真
    /// </summary>
    public enum Pic_Type : int
    {
        Ami = 0,
        Pierre,
        Menderu,
        Tukuyomi,
        Matuka,
        Mana,
        Mati,
    }

    public ModelUtil syugoPic;
    public ModelUtil pic1;
    public ModelUtil pic2;
    public ModelUtil pic3;
    public ModelUtil pic4;

    private ModelUtil nowPic = null;

    /// <summary>
    /// 開始時
    /// </summary>
    private void Start()
    {
        if (syugoPic != null) syugoPic.FadeOutImmediate();
        if (pic1 != null) pic1.FadeOutImmediate();
        if (pic2 != null) pic2.FadeOutImmediate();
        if (pic3 != null) pic3.FadeOutImmediate();
        if (pic4 != null) pic4.FadeOutImmediate();
    }

    /// <summary>
    /// 写真決定
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private ModelUtil GetPic(Pic_Type type)
    {
        return type switch
        {
            Pic_Type.Ami => pic1,
            Pic_Type.Pierre => pic1,
            Pic_Type.Mana => pic3,
            Pic_Type.Matuka => pic3,
            Pic_Type.Tukuyomi => pic2,
            Pic_Type.Menderu => pic2,
            _ => pic4,
        };
    }

    /// <summary>
    /// 集合写真フェードイン
    /// </summary>
    public void ShowSyugoPic()
    {
        syugoPic.FadeIn(FADE_TIME);
    }

    /// <summary>
    /// 集合写真フェードアウト
    /// </summary>
    public void HideSyugoPic()
    {
        syugoPic.FadeOut(FADE_TIME);
    }

    /// <summary>
    /// 集合写真から個人に変更
    /// </summary>
    /// <param name="pic"></param>
    public void ChangeSyugoToPic(Pic_Type pic)
    {
        var p = GetPic(pic);
        p.FadeInImmediate();
        syugoPic.FadeOut(FADE_TIME);

        nowPic = p;
    }

    /// <summary>
    /// 写真表示
    /// </summary>
    /// <param name="pic"></param>
    public void ShowPic(Pic_Type pic)
    {
        var idx = (int)pic;
        var p = GetPic(pic);

        p.FadeIn(FADE_TIME);
        nowPic = p;
    }

    /// <summary>
    /// 表示中の写真をフェードアウト
    /// </summary>
    public void HidePic()
    {
        if (nowPic != null)
        {
            nowPic.FadeOut(FADE_TIME);
        }

        nowPic = null;
    }

    /// <summary>
    /// 個人写真の表示場所に移動
    /// </summary>
    /// <param name="type"></param>
    public void SetPos(Pic_Type type, float time = -1f)
    {
        var vec = type switch
        {
            Pic_Type.Ami => POS_AMI,
            Pic_Type.Pierre => POS_PIERRE,
            Pic_Type.Mana => POS_MANA,
            Pic_Type.Matuka => POS_MATUKA,
            Pic_Type.Tukuyomi => POS_TUKUYOMI,
            Pic_Type.Menderu => POS_MENDERU,
            _ => POS_MATI,
        };

        if (time <= 0f)
        {
            transform.localPosition = vec;
        }
        else
        {
            StartCoroutine(SetPosCoroutine(vec, time));
        }
    }

    /// <summary>
    /// 移動
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    private IEnumerator SetPosCoroutine(Vector3 pos, float time)
    {
        var p = new DeltaVector3();
        p.Set(transform.localPosition);
        p.MoveTo(pos, time, DeltaFloat.MoveType.LINE);
        while (p.IsActive())
        {
            yield return null;
            p.Update(Time.deltaTime);
            transform.localPosition = p.Get();
        }
    }
}
