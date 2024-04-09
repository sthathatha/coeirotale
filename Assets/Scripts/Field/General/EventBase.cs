using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �t�B�[���h�C�x���g��{����
/// </summary>
public abstract class EventBase : MonoBehaviour
{
    /// <summary>�t�B�[���h</summary>
    public MainScriptBase field;

    public virtual void Start() { }

    /// <summary>
    /// �C�x���g���s
    /// </summary>
    /// <returns></returns>
    public void ExecEvent()
    {
        field.FieldState = MainScriptBase.State.Event;
        field.StartCoroutine(ExecEventCoroutine());
    }

    /// <summary>
    /// �C�x���g���s�R���[�`���@MainScript����Ăяo���Ă��炤
    /// </summary>
    /// <returns></returns>
    public IEnumerator ExecEventCoroutine()
    {
        yield return null; //����A�{�^�����c���Ă�̂łP�t���҂�

        yield return Exec();

        yield return null;
        field.FieldState = MainScriptBase.State.Idle;
    }

    /// <summary>
    /// ���ۂ̃C�x���g����
    /// </summary>
    /// <returns></returns>
    protected abstract IEnumerator Exec();
}
