using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleActionEvent : ActionEventBase
{
    /// <summary>�p�����[�^</summary>
    public int param1;

    // �e�X�g
    protected override IEnumerator Exec()
    {
        var manager = ManagerSceneScript.GetInstance();
        var window = manager.GetMessageWindow();

        //���b�Z�[�W��
        //window.Open();
        //window.StartMessage(MessageWindow.Face.Menderu0, "�e�X�g�����񂠂���������");

        //yield return window.WaitForMessageEnd();
        //window.Close();

        //�Q�[���V�[���Ăяo��
        var sceneName = param1 switch
        {
            0 => "GameSceneMenderuA",
            1 => "GameScenePierreA",
            _ => "",
        };
        manager.StartGame(sceneName);
        yield return new WaitWhile(() => manager.SceneState != ManagerSceneScript.State.Main);

        //�I������烁�b�Z�[�W��
        //window.Open();
        //window.StartMessage(MessageWindow.Face.Menderu0, "�e�X�g�����񂢂�������");

        //yield return window.WaitForMessageEnd();
        //window.Close();
    }
}
