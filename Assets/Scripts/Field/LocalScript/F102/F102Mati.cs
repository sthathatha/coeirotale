using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F102　マチ
/// </summary>
public class F102Mati : ActionEventBase
{
    #region メンバー

    public AudioClip voice_new1;
    public AudioClip voice_new3;
    public AudioClip voice_new4;
    public AudioClip voice_new6;
    public AudioClip voice_new7;
    public AudioClip voice_new9;
    public AudioClip voice_new10;
    public AudioClip voice_new12;
    public AudioClip voice_new13;

    public AudioClip voice_lose1;
    public AudioClip voice_lose3;

    public AudioClip voice_retry1;

    public AudioClip voice_win1;
    public AudioClip voice_win2;
    public AudioClip voice_win3;
    public AudioClip voice_win4;

    #endregion

    /// <summary>負けカウント</summary>
    private int loseCount = 0;

    /// <summary>
    /// 開始時
    /// </summary>
    public override void Start()
    {
        base.Start();

        if (Global.GetSaveData().GetGameDataInt(F102System.MATI_WIN_FLG) == 1)
        {
            // クリア済みで居なくなる
            gameObject.SetActive(false);
        }
        else if (Global.GetSaveData().GetGameDataInt(F102System.MATI_MEET_FLG) == 1)
        {
            GetComponent<CharacterScript>().PlayAnim("down");
        }
        else
        {
            GetComponent<CharacterScript>().PlayAnim("up");
        }
    }

    /// <summary>
    /// 実行
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Exec()
    {
        var manager = ManagerSceneScript.GetInstance();
        var msg = manager.GetMessageWindow();
        var save = Global.GetSaveData();

        msg.Open();
        // 開始会話
        if (save.GetGameDataInt(F102System.MATI_MEET_FLG) == 1)
        {
            msg.StartMessage(MessageWindow.Face.Mati0, StringFieldMessage.F102_Retry1_Mati, voice_retry1);
            yield return msg.WaitForMessageEnd();
        }
        else
        {
            GetComponent<CharacterScript>().PlayAnim("down");
            save.SetGameData(F102System.MATI_MEET_FLG, 1);
            msg.StartMessage(MessageWindow.Face.Mati0, StringFieldMessage.F102_New1_Mati, voice_new1);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F102_New2_Reko, null);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Mati0, StringFieldMessage.F102_New3_Mati, voice_new3);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F102_New4_Tukuyomi, voice_new4);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F102_New5_Reko, null);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F102_New6_Tukuyomi, voice_new6);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Mati0, StringFieldMessage.F102_New7_Mati, voice_new7);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F102_New8_Reko, null);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Mati0, StringFieldMessage.F102_New9_Mati, voice_new9);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Mati0, StringFieldMessage.F102_New10_Mati, voice_new10);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F102_New11_Reko, null);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Mati0, StringFieldMessage.F102_New12_Mati, voice_new12);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F102_New13_Tukuyomi, voice_new13);
            yield return msg.WaitForMessageEnd();
        }
        msg.Close();

        // 戦闘
        Global.GetTemporaryData().loseCount = loseCount;
        manager.StartGame("GameSceneIkusautaA");
        yield return new WaitUntil(() => manager.SceneState == ManagerSceneScript.State.Main);

        msg.Open();
        if (Global.GetTemporaryData().gameWon)
        {
            save.SetGameData(F102System.MATI_WIN_FLG, 1);
            //勝利
            msg.StartMessage(MessageWindow.Face.Mati0, StringFieldMessage.F102_Win1_Mati, voice_win1);
            yield return msg.WaitForMessageEnd();

            // まだ残っている
            if (save.GetGameDataInt(F112System.MATUKA_WIN_FLG) < 1 ||
                save.GetGameDataInt(F122System.F122_PIERRE_PHASE) < 2 ||
                save.GetGameDataInt(F132System.MANA_WIN_FLG) < 1 ||
                save.GetGameDataInt(F143System.MENDERU_WIN_FLG) < 1 ||
                save.GetGameDataInt(F153System.AMI_WIN_FLG) < 1)
            {
                msg.StartMessage(MessageWindow.Face.Mati0, StringFieldMessage.F102_Win2_Mati, voice_win2);
                yield return msg.WaitForMessageEnd();
            }
            msg.StartMessage(MessageWindow.Face.Mati0, StringFieldMessage.F102_Win3_Mati, voice_win3);
            yield return msg.WaitForMessageEnd();
            msg.Close();

            yield return manager.FadeOut();
            GetComponent<ObjectBase>().model.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            yield return manager.FadeIn();

            msg.Open();
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F102_Win4_Tukuyomi, voice_win4);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F102_Win5_Reko, null);
            yield return msg.WaitForMessageEnd();
            msg.Close();

            manager.LoadMainScene("Field004", 4);
        }
        else
        {
            //負け
            if (loseCount < 100) loseCount++;

            msg.StartMessage(MessageWindow.Face.Mati0, StringFieldMessage.F102_Lose1_Mati, voice_lose1);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F102_Lose2_Reko, null);
            yield return msg.WaitForMessageEnd();
            msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F102_Lose3_Tukuyomi, voice_lose3);
            yield return msg.WaitForMessageEnd();
            msg.Close();
        }
    }
}
