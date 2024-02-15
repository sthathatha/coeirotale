using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleActionEvent : ActionEventBase
{
    // �e�X�g
    protected override IEnumerator Exec()
    {
        var manager = ManagerSceneScript.GetInstance();
        var window = manager.GetMessageWindow();

        //���b�Z�[�W��
        window.Open();
        window.StartMessage(MessageWindow.Face.Menderu0, "�e�X�g�����񂠂���������");

        yield return window.WaitForMessageEnd();
        window.Close();

        //�Q�[���V�[���Ăяo��
        manager.StartGame("SampleGameScene");
        yield return new WaitWhile(() => manager.SceneState != ManagerSceneScript.State.Main);

        //�I������烁�b�Z�[�W��
        window.Open();
        window.StartMessage(MessageWindow.Face.Menderu0, "�e�X�g�����񂢂�������");

        yield return window.WaitForMessageEnd();
        window.Close();
    }
}
