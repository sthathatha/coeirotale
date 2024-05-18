using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �t�B�[���h�C�x���g��{����
/// </summary>
public abstract class EventBase : MonoBehaviour
{
    /// <summary>�t�B�[���h</summary>
    public MainScriptBase fieldScript;

    public virtual void Start()
    {
        if (fieldScript == null)
        {
            // �ݒ肪�ʓ|�Ȃ̂Ŋ�{�I�ȍ\���Ȃ�擾
            var objects = gameObject.scene.GetRootGameObjects();
            foreach (var obj in objects)
            {
                var sys = obj.GetComponent<MainScriptBase>();
                if (sys != null)
                {
                    fieldScript = sys;
                    break;
                }
            }
        }
    }

    /// <summary>
    /// �C�x���g���s
    /// </summary>
    /// <returns></returns>
    public void ExecEvent()
    {
        fieldScript.FieldState = MainScriptBase.State.Event;
        fieldScript.StartCoroutine(ExecEventCoroutine());
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
        fieldScript.FieldState = MainScriptBase.State.Idle;
    }

    /// <summary>
    /// ���ۂ̃C�x���g����
    /// </summary>
    /// <returns></returns>
    protected abstract IEnumerator Exec();
}
