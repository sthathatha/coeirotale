using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 205�@���X�{�X����
/// </summary>
public class F205System : MainScriptBase
{
    public const string LAST_BATTLE_SHOWN = "F205Shown";

    public AudioClip bgm_help;

    #region ���

    /// <summary>
    /// ��������
    /// </summary>
    /// <returns></returns>
    public override IEnumerator BeforeInitFadeIn()
    {
        yield return base.BeforeInitFadeIn();

        var ev = GetComponent<F205Start>();
        var skip = false;
        // ������̓X�L�b�v�`�F�b�N
        if (Global.GetSaveData().GetGameDataInt(LAST_BATTLE_SHOWN) == 1)
        {
            var msg = ManagerSceneScript.GetInstance().GetMessageWindow();
            var dialog = ManagerSceneScript.GetInstance().GetDialogWindow();

            msg.Open();
            msg.StartMessage(MessageWindow.Face.None, StringFieldMessage.F205_Lose_Skip_Dialog);
            yield return msg.WaitForMessageEnd();
            yield return dialog.OpenDialog();
            skip = dialog.GetResult() == DialogWindow.Result.Yes;
            msg.Close();
        }

        ev.InitScene(skip);
    }

    /// <summary>
    /// �t�F�[�h�C���O
    /// </summary>
    /// <returns></returns>
    public override IEnumerator BeforeFadeIn()
    {
        yield return base.BeforeFadeIn();

        var ev = GetComponent<F205Start>();
        ev.BackFromGame();
    }

    /// <summary>
    /// �t�F�[�h�C����
    /// </summary>
    /// <returns></returns>
    public override IEnumerator AfterFadeIn(bool init)
    {
        yield return base.AfterFadeIn(init);

        if (init)
        {
            // �J�n���C�x���g�J�n
            var ev = GetComponent<F205Start>();
            ev.ExecEvent();
        }
    }

    #endregion
}
