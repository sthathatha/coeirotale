using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���b�Z�[�W�݂̂̃A�N�V�����C�x���g
/// </summary>
abstract public class SimpleMessageEvent : ActionEventBase
{
    /// <summary>
    /// ���s
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Exec()
    {
        var msg = ManagerSceneScript.GetInstance().GetMessageWindow();

        msg.Open();
        var idx = 0;
        while (true)
        {
            if (ShowMessage(msg, idx) == false) break;

            yield return msg.WaitForMessageEnd();
            ++idx;
        }

        msg.Close();
    }

    /// <summary>
    /// ���b�Z�[�W�\��
    /// </summary>
    /// <param name="msg">���b�Z�[�W�E�B���h�E</param>
    /// <param name="index">0���珇�ɌĂ΂��</param>
    /// <returns>false��Ԃ��ƏI��</returns>
    abstract protected bool ShowMessage(MessageWindow msg, int index);
}
