using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleActionEvent : ActionEventBase
{
    /// <summary>パラメータ</summary>
    public int param1;

    /// <summary>ボイス</summary>
    public AudioClip voiceClip;
    /// <summary>ボイス2</summary>
    public AudioClip voiceClip2;

    private int loseCount;
    private int winCount;

    public override void Start()
    {
        base.Start();
        loseCount = 0;

        winCount = 0;
    }

    // テスト
    protected override IEnumerator Exec()
    {
        var manager = ManagerSceneScript.GetInstance();
        var msg = manager.GetMessageWindow();
        var dialog = manager.GetDialogWindow();
        var tmpData = Global.GetTemporaryData();

        msg.Open();
        switch (param1)
        {
            case 0:
                msg.StartMessage(MessageWindow.Face.Menderu0, StringFieldMessage.DebugMap_Menderu, voiceClip);
                break;
            case 1:
                msg.StartMessage(MessageWindow.Face.Pierre0, StringFieldMessage.DebugMap_Pierre, voiceClip);
                break;
            case 2:
                msg.StartMessage(MessageWindow.Face.Mati0, StringFieldMessage.DebugMap_Mati, voiceClip);
                break;
            case 3:
                msg.StartMessage(MessageWindow.Face.Matuka0, StringFieldMessage.DebugMap_Matuka, voiceClip);
                break;
            case 4:
                msg.StartMessage(MessageWindow.Face.Mana0, StringFieldMessage.DebugMap_Mana, voiceClip);
                break;
            case 5:
                msg.StartMessage(MessageWindow.Face.Ami0, StringFieldMessage.DebugMap_Ami, voiceClip);
                break;
        }
        yield return msg.WaitForMessageEnd();
        yield return dialog.OpenDialog();
        msg.Close();

        if (dialog.GetResult() == DialogWindow.Result.Yes)
        {
            //ゲームシーン呼び出し
            var sceneName = param1 switch
            {
                0 => "GameSceneMenderuA",
                1 => "GameScenePierreA",
                2 => "GameSceneIkusautaA",
                3 => "GameSceneMatukaA",
                4 => "GameSceneManaA",
                5 => "GameSceneAmiA",
                _ => "",
            };

            Global.GetTemporaryData().bossRush = false;
            if (param1 == 5 && winCount == 1)
            {
                msg.Open();
                msg.StartMessage(MessageWindow.Face.Ami0, StringFieldMessage.DebugMap_Ami2, voiceClip2);
                yield return msg.WaitForMessageEnd();
                yield return dialog.OpenDialog();
                msg.Close();

                if (dialog.GetResult() == DialogWindow.Result.Yes)
                {
                    sceneName = "GameSceneAmiB";
                    Global.GetTemporaryData().bossRush = true;
                }
            }

            tmpData.loseCount = loseCount;
            manager.StartGame(sceneName);
            yield return new WaitWhile(() => manager.SceneState != ManagerSceneScript.State.Main);

            if (tmpData.gameWon)
            {
                winCount = 1;
            }
            else
            {
                loseCount = Mathf.Clamp(loseCount + 1, 0, 100);
            }
        }
    }
}
