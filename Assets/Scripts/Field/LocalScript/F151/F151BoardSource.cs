using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 橋の素材とロープを拾う
/// </summary>
public class F151BoardSource : ActionEventBase
{
    /// <summary>板使用フラグ</summary>
    public const string BOARD_USE_FLG = "F151BoardUse";

    /// <summary>橋の板</summary>
    public GameObject bridgeBoard;
    /// <summary>橋部分のコリジョン</summary>
    public GameObject bridgeCollide;
    /// <summary>自分自身のモデル</summary>
    public GameObject selfModel;

    public AudioClip voice1;

    /// <summary>
    /// 開始時
    /// </summary>
    public override void Start()
    {
        base.Start();

        if (Global.GetSaveData().GetGameDataInt(BOARD_USE_FLG) >= 1)
        {
            bridgeBoard.SetActive(true);
            bridgeCollide.SetActive(false);
            gameObject.SetActive(false);
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

        msg.Open();
        msg.StartMessage(MessageWindow.Face.Reko, StringFieldMessage.F151_1_Reko);
        yield return msg.WaitForMessageEnd();
        msg.Close();

        yield return manager.FadeOut();
        bridgeBoard.SetActive(true);
        bridgeCollide.SetActive(false);
        selfModel.SetActive(false);
        yield return new WaitForSeconds(1f);
        yield return manager.FadeIn();

        msg.Open();
        msg.StartMessage(MessageWindow.Face.Tukuyomi0, StringFieldMessage.F151_2_Tukuyomi, voice1);
        yield return msg.WaitForMessageEnd();
        msg.Close();
        Global.GetSaveData().SetGameData(BOARD_USE_FLG, 1);

        gameObject.SetActive(false);
    }
}
