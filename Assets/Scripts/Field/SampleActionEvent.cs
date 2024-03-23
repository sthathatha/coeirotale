using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleActionEvent : ActionEventBase
{
    /// <summary>�p�����[�^</summary>
    public int param1;

    /// <summary>�{�C�X</summary>
    public AudioClip voiceClip;

    private int loseCount;

    public override void Start()
    {
        base.Start();
        loseCount = 0;
    }

    // �e�X�g
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
                msg.StartMessage(MessageWindow.Face.Menderu0, "���ƗV�ԁH", voiceClip);
                break;
            case 1:
                msg.StartMessage(MessageWindow.Face.Pierre0, "�l�ƗV�Ԃ����H", voiceClip);
                break;
            case 2:
                msg.StartMessage(MessageWindow.Face.Mati0, "���ƗV�Ԃ̂��H", voiceClip);
                break;
        }
        yield return msg.WaitForMessageEnd();
        yield return dialog.OpenDialog();
        msg.Close();

        if (dialog.GetResult() == DialogWindow.Result.Yes)
        {
            //�Q�[���V�[���Ăяo��
            var sceneName = param1 switch
            {
                0 => "GameSceneMenderuA",
                1 => "GameScenePierreA",
                2 => "GameSceneIkusautaA",
                _ => "",
            };

            tmpData.loseCount = loseCount;
            manager.StartGame(sceneName);
            yield return new WaitWhile(() => manager.SceneState != ManagerSceneScript.State.Main);

            if (!tmpData.gameWon)
            {
                if (loseCount < 100) loseCount++;
            }
        }
    }
}
