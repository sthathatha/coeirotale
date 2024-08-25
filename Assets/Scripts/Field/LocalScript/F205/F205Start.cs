using System.Collections;
using UnityEngine;

/// <summary>
/// F205　入った時からイベント
/// </summary>
public class F205Start : EventBase
{
    #region メンバー

    public GameObject bg_far;
    public GameObject bg_near;
    public GameObject bg_base;
    public GameObject blackScreen;

    public PlayerScript reko;
    public TukuyomiScript tukuyomi;
    public CharacterScript ami;
    public CharacterScript mana;
    public CharacterScript mati;
    public CharacterScript matuka;
    public CharacterScript menderu;
    public CharacterScript pierre;
    public GameObject sirowaniShadow;
    public GameObject sirowani0;
    public GameObject sirowani1;

    public ObjectBase boss_phantom;
    public ObjectBase boss_ai1;
    public ObjectBase boss_ai2;
    public GameObject boss_canon;
    public GameObject fakeCharacters;
    public GameObject obj_knife;

    public AudioClip helpBgm;

    #region セリフ音声

    public AudioClip voice_ev1_1_Tukuyomi;
    public AudioClip voice_ev1_2_Tukuyomi;
    public AudioClip voice_ev1_3_Tukuyomi;
    public AudioClip voice_ev1_4_Tukuyomi;
    public AudioClip voice_ev1_5_Tukuyomi;
    public AudioClip voice_ev1_6_Tukuyomi;
    public AudioClip voice_ev1_7_Tukuyomi;
    public AudioClip voice_ev1_8_Tukuyomi;
    public AudioClip voice_ev1_9_Mati;
    public AudioClip voice_ev1_10_Matuka;
    public AudioClip voice_ev1_11_Pierre;
    public AudioClip voice_ev1_12_Menderu;
    public AudioClip voice_ev1_13_Mana;
    public AudioClip voice_ev1_14_Ami;
    public AudioClip voice_ev1_15_Mati;

    public AudioClip voice_ev2_1_Mana;
    public AudioClip voice_ev2_2_Matuka;
    public AudioClip voice_ev2_3_Menderu;
    public AudioClip voice_ev2_4_Pierre;
    public AudioClip voice_ev2_5_Ami;
    public AudioClip voice_ev2_6_Mati;
    public AudioClip voice_ev2_7_Ami;
    public AudioClip voice_ev2_8_Mana;
    public AudioClip voice_ev2_9_Matuka;
    public AudioClip voice_ev2_10_Mati;
    public AudioClip voice_ev2_11_Pierre;
    public AudioClip voice_ev2_12_Menderu;
    public AudioClip voice_ev2_13_Tukuyomi;
    public AudioClip voice_ev2_14_Ami;
    public AudioClip voice_ev2_15_Tukuyomi;
    public AudioClip voice_ev2_16_Matuka;
    public AudioClip voice_ev2_17_Menderu;
    public AudioClip voice_ev2_18_Pierre;

    #endregion

    #region SE

    public AudioClip se_canon_charge;
    public AudioClip se_canon_shoot;
    public AudioClip se_knife_throw;
    public AudioClip se_knife_hit;
    public AudioClip se_grass_break;
    public AudioClip se_sirowani_appear;
    public AudioClip se_sirowani_eat;
    public AudioClip se_bomb;

    #endregion

    #endregion

    private bool isSkip = false;
    private bool playingBossRush = false;

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="skip"></param>
    public void InitScene(bool skip)
    {
        isSkip = skip;

        bg_base.SetActive(true);
        bg_far.SetActive(true);
        bg_near.SetActive(true);

        sirowani0.SetActive(false);
        sirowani1.SetActive(false);
        sirowaniShadow.SetActive(false);
        StartCoroutine(sirowaniShadow.GetComponent<ModelUtil>().FadeOutCoroutine(0f));

        boss_canon.SetActive(false);
        boss_ai1.gameObject.SetActive(false);
        obj_knife.SetActive(false);
        if (isSkip)
        {
            // スキップの場合ボス前の状態
            ami.gameObject.SetActive(true);
            mana.gameObject.SetActive(true);
            mati.gameObject.SetActive(true);
            matuka.gameObject.SetActive(true);
            menderu.gameObject.SetActive(true);
            pierre.gameObject.SetActive(true);
            boss_phantom.gameObject.SetActive(false);
            boss_ai2.gameObject.SetActive(true);
            fakeCharacters.SetActive(true);

            var p5 = fieldScript.SearchGeneralPosition(5);
            reko.SetPosition(p5.GetPosition());
            ami.SetPosition(new Vector3(ami.transform.localPosition.x, p5.GetPosition().y));
            mana.SetPosition(new Vector3(mana.transform.localPosition.x, p5.GetPosition().y));
            matuka.SetPosition(new Vector3(matuka.transform.localPosition.x, p5.GetPosition().y));
            mati.SetPosition(new Vector3(mati.transform.localPosition.x, p5.GetPosition().y));
            menderu.SetPosition(new Vector3(menderu.transform.localPosition.x, p5.GetPosition().y));
            pierre.SetPosition(new Vector3(pierre.transform.localPosition.x, p5.GetPosition().y));
            ami.SetDirection(Constant.Direction.Up);
            mana.SetDirection(Constant.Direction.Up);
            matuka.SetDirection(Constant.Direction.Up);
            mati.SetDirection(Constant.Direction.Up);
            menderu.SetDirection(Constant.Direction.Up);
            pierre.SetDirection(Constant.Direction.Up);

            ManagerSceneScript.GetInstance().soundMan.PlayFieldBgm(SoundManager.FieldBgmType.None, helpBgm);

            var cam = ManagerSceneScript.GetInstance().mainCam;
            cam.SetTargetPos(boss_ai1.gameObject);
            cam.Immediate();
        }
        else
        {
            // スキップなしの場合
            ami.gameObject.SetActive(false);
            mana.gameObject.SetActive(false);
            mati.gameObject.SetActive(false);
            matuka.gameObject.SetActive(false);
            menderu.gameObject.SetActive(false);
            pierre.gameObject.SetActive(false);
            boss_phantom.gameObject.SetActive(true);
            boss_ai2.gameObject.SetActive(false);

            StartCoroutine(fakeCharacters.GetComponent<ModelUtil>().FadeOutCoroutine(0f));
            fakeCharacters.SetActive(false);
        }
    }

    /// <summary>
    /// ゲームから戻ったときフェードイン前
    /// </summary>
    public void BackFromGame()
    {
        var cam = ManagerSceneScript.GetInstance().mainCam;

        if (playingBossRush)
        {
            cam.SetTargetPos(boss_ai1.gameObject);
            cam.Immediate();
            fakeCharacters.SetActive(false);
            reko.SetCameraEnable(false);
        }
    }

    /// <summary>
    /// 実行
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Exec()
    {
        var manager = ManagerSceneScript.GetInstance();
        var cam = manager.mainCam;
        var sound = manager.soundMan;
        var msg = manager.GetMessageWindow();
        var tmpData = Global.GetTemporaryData();

        var p0 = fieldScript.SearchGeneralPosition(0);
        var p1 = fieldScript.SearchGeneralPosition(1);
        var p2 = fieldScript.SearchGeneralPosition(2);
        var p3 = fieldScript.SearchGeneralPosition(3);
        var p4 = fieldScript.SearchGeneralPosition(4);
        var p5 = fieldScript.SearchGeneralPosition(5);
        var tmp = Vector3.zero;
        var tmpStr = "";
        AudioSource tmpSe = null;

        if (!isSkip)
        {
            #region 前座イベント

            // ボス前まで歩く
            reko.WalkTo(p1.GetPosition());
            tukuyomi.SetPosition(p2.GetPosition());
            yield return new WaitForSeconds(2f);
            // 会話
            reko.SetCameraEnable(false);
            cam.SetTargetPos(boss_ai1.gameObject);
            msg.Open();
            msg.StartMessage(MessageWindow.Face.None, StringFieldMessage.F205_S1_1_Boss);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F205_S1_2_Reko);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.None, StringFieldMessage.F205_S1_3_Boss);
            yield return msg.WaitForMessageEnd();
            cam.PlayShakeOne(Shaker.ShakeSize.Weak, 1f);
            msg.StartMessage(MessageWindow.Face.None, StringFieldMessage.F205_S1_4_Boss);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F205_S1_5_Reko);
            yield return msg.WaitForMessageEnd();
            // つくよみちゃんが駆けつける
            tukuyomi.SetMode(TukuyomiScript.TukuyomiMode.NPC);
            tukuyomi.gameObject.SetActive(true);
            tmp = p1.GetPosition();
            tmp.x = p2.GetPosition().x;
            tukuyomi.WalkTo(tmp);
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F205_S1_6_Tukuyomi, voice_ev1_1_Tukuyomi);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F205_S1_7_Reko);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F205_S1_8_Tukuyomi, voice_ev1_2_Tukuyomi);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F205_S1_9_Reko);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.None, StringFieldMessage.F205_S1_10_Boss);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.None, StringFieldMessage.F205_S1_11_Boss);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F205_S1_12_Tukuyomi, voice_ev1_3_Tukuyomi);
            yield return msg.WaitForMessageEnd();
            msg.Close();

            // 自動戦闘
            manager.StartGame("GameSceneBossA");
            yield return new WaitUntil(() => manager.SceneState == ManagerSceneScript.State.Main);
            // 連打だと下の方に居るかもしれないので
            if (tukuyomi.transform.localPosition.y < tmp.y) tukuyomi.WalkTo(tmp);

            // 幻影消える
            boss_phantom.GetComponent<ModelUtil>().FadeOut(3f);
            boss_ai1.gameObject.SetActive(true);
            msg.Open();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F205_S2_1_Reko);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F205_S2_2_Tukuyomi, voice_ev1_4_Tukuyomi);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F205_S2_3_Reko);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F205_S2_4_Tukuyomi, voice_ev1_5_Tukuyomi);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.None2, StringFieldMessage.F205_S2_5_Boss);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F205_S2_6_Reko);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F205_S2_7_Tukuyomi, voice_ev1_6_Tukuyomi);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.None2, StringFieldMessage.F205_S2_8_Boss);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F205_S2_9_Reko);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.None2, StringFieldMessage.F205_S2_10_Boss);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.None2, StringFieldMessage.F205_S2_11_Boss);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F205_S2_12_Reko);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F205_S2_13_Tukuyomi, voice_ev1_7_Tukuyomi);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.None2, StringFieldMessage.F205_S2_14_Boss);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.None2, StringFieldMessage.F205_S2_15_Boss);
            yield return msg.WaitForMessageEnd();

            // チャージ
            tmpSe = sound.PlaySELoop(se_canon_charge);
            msg.StartMessage(MessageWindow.Face.None2, StringFieldMessage.F205_S2_16_Boss);
            yield return msg.WaitForMessageEnd();
            // つくよみちゃん押す
            tukuyomi.WalkTo(p1.GetPosition(), 3f);
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F205_S2_17_Tukuyomi, voice_ev1_8_Tukuyomi);
            yield return new WaitForSeconds(0.2f);
            reko.SlideTo(p3.GetPosition(), 4f, moveType: DeltaFloat.MoveType.DECEL);
            yield return new WaitForSeconds(0.2f);
            // レーザー
            sound.StopLoopSE(tmpSe, 0.5f);
            sound.PlaySE(se_canon_shoot);
            cam.PlayShakeOne(Shaker.ShakeSize.Weak, 0f);
            boss_canon.SetActive(true);
            tukuyomi.gameObject.SetActive(false);
            tukuyomi.SetPosition(p4.GetPosition());
            msg.Close();
            yield return new WaitForSeconds(1f);

            boss_canon.GetComponent<ModelUtil>().FadeOut(2f);
            msg.Open();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F205_S3_1_Reko);
            yield return msg.WaitForMessageEnd();
            msg.Close();

            yield return new WaitForSeconds(1f);

            // 中央に移動
            reko.WalkTo(p1.GetPosition());
            msg.Open();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F205_S3_2_Reko);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.None2, StringFieldMessage.F205_S3_3_Boss);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.None2, StringFieldMessage.F205_S3_4_Boss);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.None2, StringFieldMessage.F205_S3_5_Boss);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.None2, StringFieldMessage.F205_S3_6_Boss);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F205_S3_7_Reko);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.None2, StringFieldMessage.F205_S3_8_Boss);
            yield return msg.WaitForMessageEnd();

            // ニセモノ出現
            fakeCharacters.SetActive(true);
            fakeCharacters.GetComponent<ModelUtil>().FadeIn(1f);
            msg.StartMessage(MessageWindow.Face.None2, StringFieldMessage.F205_S3_9_Boss);
            yield return msg.WaitForMessageEnd();
            reko.SlideTo(p5.GetPosition(), 1.5f);
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F205_S3_10_Reko);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.None2, StringFieldMessage.F205_S3_11_Boss);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.None2, StringFieldMessage.F205_S3_12_Boss);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.None2, StringFieldMessage.F205_S3_13_Boss);
            yield return msg.WaitForMessageEnd();
            // チャージ
            sound.PlaySE(se_canon_charge);
            msg.StartMessage(MessageWindow.Face.None2, StringFieldMessage.F205_S3_14_Boss);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F205_S3_15_Reko);
            yield return msg.WaitForMessageEnd();
            msg.Close();

            // ナイフ飛んでくる
            sound.PlaySE(se_knife_throw);
            sound.PlayFieldBgm(SoundManager.FieldBgmType.None, null);
            obj_knife.SetActive(true);
            var knifePos = new DeltaVector3();
            knifePos.Set(obj_knife.transform.localPosition);
            knifePos.MoveTo(boss_ai1.transform.localPosition + new Vector3(0, 100f), 0.4f, DeltaFloat.MoveType.LINE);
            while (knifePos.IsActive())
            {
                yield return null;
                knifePos.Update(Time.deltaTime);
                obj_knife.transform.localPosition = knifePos.Get();
            }
            sound.PlaySE(se_knife_hit);
            obj_knife.SetActive(false);
            boss_ai1.gameObject.SetActive(false);
            boss_ai2.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);

            // 6人入場
            sound.PlayFieldBgm(SoundManager.FieldBgmType.None, helpBgm);
            mati.gameObject.SetActive(true);
            tmp = mati.transform.localPosition;
            tmp.y = p5.GetPosition().y;
            mati.WalkTo(tmp);
            msg.Open();
            msg.StartMessage(MessageWindow.Face.Mati0, StringFieldMessage.F205_S4_1_Mati, voice_ev1_9_Mati);
            yield return msg.WaitForMessageEnd();
            matuka.gameObject.SetActive(true);
            tmp = matuka.transform.localPosition;
            tmp.y = p5.GetPosition().y;
            matuka.WalkTo(tmp);
            msg.StartMessage(MessageWindow.Face.Matuka0, StringFieldMessage.F205_S4_2_Matuka, voice_ev1_10_Matuka);
            yield return msg.WaitForMessageEnd();
            pierre.gameObject.SetActive(true);
            tmp = pierre.transform.localPosition;
            tmp.y = p5.GetPosition().y;
            pierre.WalkTo(tmp);
            msg.StartMessage(MessageWindow.Face.Pierre0, StringFieldMessage.F205_S4_3_Pierre, voice_ev1_11_Pierre);
            yield return msg.WaitForMessageEnd();
            menderu.gameObject.SetActive(true);
            tmp = menderu.transform.localPosition;
            tmp.y = p5.GetPosition().y;
            menderu.WalkTo(tmp);
            msg.StartMessage(MessageWindow.Face.Menderu0, StringFieldMessage.F205_S4_4_Menderu, voice_ev1_12_Menderu);
            yield return msg.WaitForMessageEnd();
            mana.gameObject.SetActive(true);
            tmp = mana.transform.localPosition;
            tmp.y = p5.GetPosition().y;
            mana.WalkTo(tmp);
            msg.StartMessage(MessageWindow.Face.Mana0, StringFieldMessage.F205_S4_5_Mana, voice_ev1_13_Mana);
            yield return msg.WaitForMessageEnd();
            ami.gameObject.SetActive(true);
            tmp = ami.transform.localPosition;
            tmp.y = p5.GetPosition().y;
            ami.WalkTo(tmp);
            msg.StartMessage(MessageWindow.Face.Ami0, StringFieldMessage.F205_S4_6_Ami, voice_ev1_14_Ami);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F205_S4_7_Reko);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.None2, StringFieldMessage.F205_S4_8_Boss);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.None2, StringFieldMessage.F205_S4_9_Boss);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Mati0, StringFieldMessage.F205_S4_10_Mati, voice_ev1_15_Mati);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F205_S4_11_Reko);
            yield return msg.WaitForMessageEnd();

            #endregion
        }
        else
        {
            msg.Open();
        }

        // 1歩ずつ
        tmp = reko.transform.localPosition;
        tmp.y += 30f;
        reko.WalkTo(tmp);
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F205_S4_12_Reko);
        yield return msg.WaitForMessageEnd();
        tmp.y += 30f;
        reko.WalkTo(tmp);
        ami.WalkTo(new Vector3(ami.transform.localPosition.x, tmp.y));
        mana.WalkTo(new Vector3(mana.transform.localPosition.x, tmp.y));
        mati.WalkTo(new Vector3(mati.transform.localPosition.x, tmp.y));
        menderu.WalkTo(new Vector3(menderu.transform.localPosition.x, tmp.y));
        matuka.WalkTo(new Vector3(matuka.transform.localPosition.x, tmp.y));
        pierre.WalkTo(new Vector3(pierre.transform.localPosition.x, tmp.y));
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F205_S4_13_Reko);
        yield return msg.WaitForMessageEnd();
        // 消えるセリフ
        tmpStr = StringFieldMessage.F205_S4_14_Boss;
        msg.StartMessage(MessageWindow.Face.None2, tmpStr);
        yield return new WaitForSeconds(1.5f);
        while (tmpStr.Length > 0)
        {
            tmpStr = tmpStr.Substring(0, tmpStr.Length - 1);
            msg.StartMessage(MessageWindow.Face.None2, tmpStr);
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(0.5f);

        // 現れるセリフ
        tmpStr = StringFieldMessage.F205_S4_15_Boss;
        var strBossLen = 0;
        while (strBossLen < tmpStr.Length)
        {
            strBossLen++;
            msg.StartMessage(MessageWindow.Face.None2, tmpStr.Substring(0, strBossLen));
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(1.5f);
        msg.Close();

        // 7連戦
        playingBossRush = true;
        Global.GetSaveData().SetGameData(F205System.LAST_BATTLE_SHOWN, 1);
        tmpData.bossRush = true;
        tmpData.loseCount = 0;
        tmpData.bossRushAmiWon = false;
        tmpData.bossRushManaWon = false;
        tmpData.bossRushMatukaWon = false;
        tmpData.bossRushMatiWon = false;
        tmpData.bossRushMenderuWon = false;
        tmpData.bossRushPierreWon = false;
        manager.StartGame("GameSceneAmiB");
        yield return new WaitUntil(() => manager.SceneState == ManagerSceneScript.State.Main);
        playingBossRush = false;
        // 負けた場合はF204に戻るためここには来ない

        // 勝った後
        yield return new WaitForSeconds(0.5f);
        msg.Open();
        msg.StartMessage(MessageWindow.Face.None2, StringFieldMessage.F205_S5_1_Boss);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.None2, StringFieldMessage.F205_S5_2_Boss);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F205_S5_3_Reko);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F205_S5_4_Reko);
        yield return msg.WaitForMessageEnd();
        // 揺れ始める
        cam.PlayShake(Shaker.ShakeSize.Middle);
        msg.StartMessage(MessageWindow.Face.None2, StringFieldMessage.F205_S5_5_Boss);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Mana0, StringFieldMessage.F205_S5_6_Mana, voice_ev2_1_Mana);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Matuka0, StringFieldMessage.F205_S5_7_Matuka, voice_ev2_2_Matuka);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Menderu0, StringFieldMessage.F205_S5_8_Menderu, voice_ev2_3_Menderu);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Pierre0, StringFieldMessage.F205_S5_9_Pierre, voice_ev2_4_Pierre);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Ami0, StringFieldMessage.F205_S5_10_Ami, voice_ev2_5_Ami);
        yield return msg.WaitForMessageEnd();
        // シロワニさんの影
        sirowaniShadow.gameObject.SetActive(true);
        sirowaniShadow.GetComponent<ModelUtil>().FadeIn(1f, alpha: 0.5f);
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F205_S5_11_Reko);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Mati0, StringFieldMessage.F205_S5_12_Mati);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Mati0, StringFieldMessage.F205_S5_13_Mati, voice_ev2_6_Mati);
        yield return msg.WaitForMessageEnd();
        msg.Close();
        ami.SlideTo(new Vector3(ami.transform.localPosition.x, p5.GetPosition().y - 20f));
        mana.SlideTo(new Vector3(mana.transform.localPosition.x, p5.GetPosition().y - 20f));
        matuka.SlideTo(new Vector3(matuka.transform.localPosition.x, p5.GetPosition().y - 20f));
        mati.SlideTo(new Vector3(mati.transform.localPosition.x, p5.GetPosition().y - 20f));
        menderu.SlideTo(new Vector3(menderu.transform.localPosition.x, p5.GetPosition().y - 20f));
        pierre.SlideTo(new Vector3(pierre.transform.localPosition.x, p5.GetPosition().y - 20f));
        reko.SlideTo(new Vector3(reko.transform.localPosition.x, p5.GetPosition().y - 20f));

        cam.StopShake();
        yield return new WaitForSeconds(0.5f);
        // 割れる
        sound.PlaySE(se_grass_break);
        blackScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        blackScreen.SetActive(false);
        bg_base.SetActive(false);
        bg_far.SetActive(true);
        bg_near.SetActive(true);
        yield return new WaitForSeconds(1.2f);
        // シロワニさん
        blackScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        tmpSe = sound.PlaySELoop(se_sirowani_appear);
        sound.StopLoopSE(tmpSe, 1.2f);
        blackScreen.SetActive(false);
        sirowaniShadow.gameObject.SetActive(false);
        boss_ai2.gameObject.SetActive(false);
        sirowani0.SetActive(true);
        yield return new WaitForSeconds(1.2f);
        blackScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        sound.PlaySE(se_sirowani_eat);
        blackScreen.SetActive(false);
        sirowani0.SetActive(false);
        sirowani1.SetActive(true);
        yield return new WaitForSeconds(1.2f);
        // 爆発
        sound.PlaySE(se_bomb);
        cam.PlayShakeOne(Shaker.ShakeSize.Weak, 0.1f);
        yield return new WaitForSeconds(0.5f);
        yield return manager.FadeOut();
        tmpSe = sound.PlaySELoop(se_sirowani_appear);
        sound.StopLoopSE(tmpSe, 1.2f);
        sirowani1.SetActive(false);
        yield return new WaitForSeconds(1f);
        yield return manager.FadeIn();
        yield return new WaitForSeconds(1f);

        msg.Open();
        msg.StartMessage(MessageWindow.Face.Ami0, StringFieldMessage.F205_S6_1_Ami, voice_ev2_7_Ami);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Mana0, StringFieldMessage.F205_S6_2_Mana, voice_ev2_8_Mana);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Matuka0, StringFieldMessage.F205_S6_3_Matuka, voice_ev2_9_Matuka);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Mati0, StringFieldMessage.F205_S6_4_Mati, voice_ev2_10_Mati);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F205_S6_5_Reko);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Pierre0, StringFieldMessage.F205_S6_6_Pierre, voice_ev2_11_Pierre);
        yield return msg.WaitForMessageEnd();
        // つくよみちゃん復活
        tukuyomi.SetPosition(p4.GetPosition());
        tukuyomi.SetMode(TukuyomiScript.TukuyomiMode.Trace);
        tukuyomi.gameObject.SetActive(true);
        msg.StartMessage(MessageWindow.Face.Menderu0, StringFieldMessage.F205_S6_7_Menderu, voice_ev2_12_Menderu);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F205_S6_8_Reko);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F205_S6_9_Tukuyomi, voice_ev2_13_Tukuyomi);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F205_S6_10_Reko);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Ami0, StringFieldMessage.F205_S6_11_Ami, voice_ev2_14_Ami);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F205_S6_12_Tukuyomi, voice_ev2_15_Tukuyomi);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F205_S6_13_Reko);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Matuka0, StringFieldMessage.F205_S6_14_Matuka, voice_ev2_16_Matuka);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Menderu0, StringFieldMessage.F205_S6_15_Menderu, voice_ev2_17_Menderu);
        yield return msg.WaitForMessageEnd();
        msg.StartMessage(MessageWindow.Face.Pierre0, StringFieldMessage.F205_S6_16_Pierre, voice_ev2_18_Pierre);
        yield return msg.WaitForMessageEnd();
        msg.Close();

        // 最後の壁によるつくよみちゃん消去フラグをおろす
        Global.GetSaveData().SetGameData(F204System.WALL_OPEN_FLG, 2);
        manager.LoadMainScene("Field210", 0);
    }
}
