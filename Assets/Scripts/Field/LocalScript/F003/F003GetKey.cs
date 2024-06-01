using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �T�[�J�X�����̌����E���I�u�W�F�N�g
/// </summary>
public class F003GetKey : AreaActionEventBase
{
    /// <summary>
    /// �J�n��
    /// </summary>
    public override void Start()
    {
        base.Start();

        // ����ς݂Ȃ玩���ŏ���
        if (Global.GetSaveData().GetGameDataInt(F121KeyBlock.KEY_FLG) >= 1)
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// ���s
    /// </summary>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    protected override IEnumerator Exec()
    {
        // �������
        Global.GetSaveData().SetGameData(F121KeyBlock.KEY_FLG, 1);
        var msg = ManagerSceneScript.GetInstance().GetMessageWindow();

        msg.Open();
        msg.StartMessage(MessageWindow.Face.None2, StringFieldMessage.F003_GetKey);
        yield return msg.WaitForMessageEnd();

        msg.Close();

        gameObject.SetActive(false);
    }
}
