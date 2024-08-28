using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// エンディング
/// </summary>
public class EndingSystem : MainScriptBase
{
    public AudioClip endingBgm;
    public AudioClip voice_tukuyomi1;
    public AudioClip voice_tukuyomi2;
    public EndingPicture picture1;
    public EndingPicture picture2;
    public ModelUtil sirowani;

    public ModelUtil logo;
    public GameObject logoMask;
    public EndingText txtCenter;
    public EndingText txtDefault;
    public EndingText txtAmi;
    public EndingText txtMana;
    public EndingText txtMenderu;
    public EndingText txtMatuka;
    public EndingText txtMati;
    public EndingText txtPierre;
    public EndingText txtTukuyomi;

    public OpeningStarObject2 reko;

    public Transform starParent;
    public OpeningStarObject1 starDummy;

    private IEnumerator starCoroutine;

    /// <summary>
    /// 初期化
    /// </summary>
    /// <returns></returns>
    public override IEnumerator BeforeInitFadeIn()
    {
        yield return base.BeforeInitFadeIn();
        var cam = ManagerSceneScript.GetInstance().mainCam;
        cam.SetTargetPos(Vector2.zero);

        logo.FadeOutImmediate();
        picture1.transform.localPosition = Vector3.zero;
        picture2.transform.localPosition = Vector3.zero;
        picture1.gameObject.SetActive(true);
        picture2.gameObject.SetActive(true);
        sirowani.gameObject.SetActive(true);
        sirowani.FadeOutImmediate();
    }

    /// <summary>
    /// フェードイン後
    /// </summary>
    /// <param name="init"></param>
    /// <returns></returns>
    public override IEnumerator AfterFadeIn(bool init)
    {
        yield return base.AfterFadeIn(init);
        Global.GetSaveData().system.clearFlag = 1;
        Global.GetSaveData().SaveSystemData();
        StartCoroutine(EndingCoroutine());
    }

    /// <summary>
    /// エンディング再生
    /// </summary>
    /// <returns></returns>
    private IEnumerator EndingCoroutine()
    {
        var manager = ManagerSceneScript.GetInstance();
        manager.soundMan.PlayFieldBgm(SoundManager.FieldBgmType.None, endingBgm, false);
        var input = InputManager.GetInstance();
        var msg = manager.GetMessageWindow();

        var pos1 = SearchGeneralPosition(1);
        var pos2 = SearchGeneralPosition(2);

        yield return new WaitForSeconds(2f);
        //0:00	いつかの時代で　知らない何処かの地で
        //	    流れ落つ　刻の砂の中
        logo.FadeIn(1f);
        yield return new WaitWhile(() => logo.IsFading());
        yield return new WaitForSeconds(7f);
        logo.FadeOut(1f);
        yield return new WaitWhile(() => logo.IsFading());
        yield return new WaitForSeconds(4f);

        //0:13	軋んだ針の音が　刻む歴史の円環
        //  	君の声　淡い日に霞んだ
        picture1.ShowSyugoPic();
        yield return new WaitForSeconds(10f);
        picture1.HideSyugoPic();
        yield return new WaitForSeconds(4f);

        //0:27	繰り返す遠い夢　どれほど永くいたのか
        //	偽りの楽園から発とう　彼の地へ
        picture1.SetPos(EndingPicture.Pic_Type.Ami);
        picture1.ShowPic(EndingPicture.Pic_Type.Ami);
        yield return new WaitForSeconds(4f);
        txtAmi.FadeIn(StringFieldMessage.End_Ami);
        yield return new WaitForSeconds(8f);
        picture1.HidePic();
        txtAmi.FadeOut();
        yield return new WaitForSeconds(5f);

        //0:43	遠かりし定め　光りの一片連れて
        //	氷の矢にこの翅を　引き裂かれ堕ちゆく
        picture1.SetPos(EndingPicture.Pic_Type.Mati);
        picture1.ShowPic(EndingPicture.Pic_Type.Mati);
        yield return new WaitForSeconds(3f);
        txtMati.FadeIn(StringFieldMessage.End_Mati);
        yield return new WaitForSeconds(7f);
        picture1.HidePic();
        txtMati.FadeOut();
        yield return new WaitForSeconds(3f);

        //0:57	吹荒れる風音　真黒き嵐を越えて
        //	果てに明日が無くとも　共に行けるならば
        picture1.SetPos(EndingPicture.Pic_Type.Menderu);
        picture1.ShowPic(EndingPicture.Pic_Type.Menderu);
        yield return new WaitForSeconds(3f);
        txtMenderu.FadeIn(StringFieldMessage.End_Menderu);
        yield return new WaitForSeconds(7f);
        picture1.HidePic();
        txtMenderu.FadeOut();
        yield return new WaitForSeconds(4f);

        //1:10	間奏（短）
        picture1.SetPos(EndingPicture.Pic_Type.Matuka);
        picture1.ShowPic(EndingPicture.Pic_Type.Matuka);
        yield return new WaitForSeconds(3f);
        txtMatuka.FadeIn(StringFieldMessage.End_Matuka);
        yield return new WaitForSeconds(7f);
        picture1.HidePic();
        txtMatuka.FadeOut();
        yield return new WaitForSeconds(3.5f);

        //1:25	淀んだ日は開けて　流る雲映す水
        //	刻の砂　彼方に煌めく
        picture2.ShowSyugoPic();
        yield return new WaitForSeconds(10.5f);
        picture2.HideSyugoPic();
        yield return new WaitForSeconds(4f);

        //1:38	希望と絶望が　織り成す地を駆け出す
        //	心の中　君の姿を見た
        picture2.SetPos(EndingPicture.Pic_Type.Mana);
        picture2.ShowPic(EndingPicture.Pic_Type.Mana);
        yield return new WaitForSeconds(3f);
        txtMana.FadeIn(StringFieldMessage.End_Mana);
        yield return new WaitForSeconds(7f);
        picture2.HidePic();
        txtMana.FadeOut();
        yield return new WaitForSeconds(3f);

        //1:52	震えてる手に握る　怖れを手放して
        //	誘う風の旋律は　霧を振り払う
        picture2.SetPos(EndingPicture.Pic_Type.Tukuyomi);
        picture2.ShowPic(EndingPicture.Pic_Type.Tukuyomi);
        yield return new WaitForSeconds(3f);
        txtTukuyomi.FadeIn(StringFieldMessage.End_Tukuyomi);
        yield return new WaitForSeconds(8f);
        picture2.HidePic();
        txtTukuyomi.FadeOut();
        yield return new WaitForSeconds(5f);

        //2:08	絶え間なく注ぐ　朱の明り背にして
        //	もう何も囚われない　彩られた世界
        picture2.SetPos(EndingPicture.Pic_Type.Pierre);
        picture2.ShowPic(EndingPicture.Pic_Type.Pierre);
        yield return new WaitForSeconds(3f);
        txtPierre.FadeIn(StringFieldMessage.End_Pierre);
        yield return new WaitForSeconds(7f);
        picture2.HidePic();
        txtPierre.FadeOut();
        yield return new WaitForSeconds(3f);

        //2:21	刻の最先へ　生きる意味を探して
        //	終わらぬ地平の果てを　君と紡ぐ軌跡
        sirowani.FadeIn(1f);
        yield return new WaitForSeconds(3f);
        txtCenter.FadeIn(StringFieldMessage.End_Sirowani);
        yield return new WaitForSeconds(8f);
        sirowani.FadeOut(1f);
        txtCenter.FadeOut();
        yield return new WaitForSeconds(4f);

        //2:36	間奏（長）
        starCoroutine = CreateStarCoroutine();
        StartCoroutine(starCoroutine);
        txtCenter.FadeIn(StringFieldMessage.End_BgmSe);
        yield return new WaitForSeconds(12f);
        txtCenter.FadeOut();
        yield return new WaitForSeconds(3f);

        //2:50	穿たれたこの翅は　羽ばたかないけど
        //	この息吹満ちる世界を　歩んでゆく
        txtCenter.FadeIn(StringFieldMessage.End_Song);
        yield return new WaitForSeconds(14f);
        txtCenter.FadeOut();
        yield return new WaitForSeconds(3f);

        //3:08	絶え間なく注ぐ　朱の明り背にして
        //	もう何も囚われない　彩られた世界
        //3:22	刻の最先へ　生きる意味を探して
        //	終わらぬ地平の果てを　君と紡ぐ軌跡
        reko.MoveTo(pos1.GetPosition(), 3f, DeltaFloat.MoveType.DECEL);
        yield return new WaitForSeconds(6f);
        txtDefault.FadeIn(Global.GetTemporaryData().ending_select_voice switch
        {
            F210System.VOICE_AMI => StringFieldMessage.End_Reko_Ami,
            F210System.VOICE_MANA => StringFieldMessage.End_Reko_Mana,
            F210System.VOICE_MATI => StringFieldMessage.End_Reko_Mati,
            F210System.VOICE_MATUKA => StringFieldMessage.End_Reko_Matuka,
            F210System.VOICE_MENDERU => StringFieldMessage.End_Reko_Menderu,
            F210System.VOICE_PIERRE => StringFieldMessage.End_Reko_Pierre,
            F210System.VOICE_TUKUYOMI => StringFieldMessage.End_Reko_Tukuyomi,
            _ => StringFieldMessage.End_Reko_Mycoe,
        });
        yield return new WaitForSeconds(9.5f);
        txtDefault.FadeOut();

        yield return MoveReko(200, 400, 0, 50f, 2f);
        yield return new WaitForSeconds(2f);
        yield return MoveReko(-100, -300, 100f, 150f, 2f);
        yield return new WaitForSeconds(3f);
        reko.MoveTo(new Vector3(0f, 400f), 2f, DeltaFloat.MoveType.DECEL);
        yield return new WaitForSeconds(3.2f);

        //3:35	君と紡ぐ軌跡
        StopCoroutine(starCoroutine);
        starCoroutine = null;
        logo.FadeIn(2f);
        yield return new WaitForSeconds(2f);
        reko.MoveTo(pos2.GetPosition(), 3f, DeltaFloat.MoveType.DECEL);
        yield return new WaitForSeconds(3f);

        //3:41
        reko.gameObject.SetActive(false);
        logoMask.SetActive(false);

        yield return new WaitForSeconds(6f);
        msg.Open();
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.End_Last_Tuku1, voice_tukuyomi1);
        var timer = new DeltaFloat();
        timer.Set(0);
        timer.MoveTo(1f, 180f, DeltaFloat.MoveType.LINE);
        while (timer.IsActive())
        {
            yield return null;
            timer.Update(Time.deltaTime);

            // 入力したら終了
            if (input.GetKeyPress(InputManager.Keys.South))
            {
                msg.Close();
                manager.LoadMainScene("TitleScene", 0);
                yield break;
            }
        }

        // ３分でセリフ変更
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.End_Last_Tuku2, voice_tukuyomi2);
        yield return new WaitUntil(() => input.GetKeyPress(InputManager.Keys.South));
        msg.Close();
        manager.LoadMainScene("TitleScene", 0);
    }

    /// <summary>
    /// レコランダム移動
    /// </summary>
    /// <param name="x1"></param>
    /// <param name="x2"></param>
    /// <param name="y1"></param>
    /// <param name="y2"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    private IEnumerator MoveReko(float x1, float x2, float y1, float y2, float time)
    {
        if (x1 > x2)
        {
            var t = x1;
            x1 = x2;
            x2 = t;
        }
        if (y1 > y2)
        {
            var t = y1;
            y1 = t;
            y2 = t;
        }
        var rand = new Vector3(Util.RandomFloat(x1, x2), Util.RandomFloat(y1, y2));
        reko.MoveTo(rand, time, DeltaFloat.MoveType.DECEL);
        yield return new WaitWhile(() => reko.IsActive());
    }

    /// <summary>
    /// 背景の星生成コルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator CreateStarCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Util.RandomFloat(0.3f, 0.7f));
            CreateStarOne();
        }
    }

    /// <summary>
    /// ダミー星１個作成
    /// </summary>
    private void CreateStarOne()
    {
        var create = Instantiate(starDummy);
        create.transform.SetParent(starParent, true);
        create.gameObject.SetActive(true);
    }
}
