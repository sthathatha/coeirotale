using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �t�B�[���h�C�x���g��{����
/// </summary>
public abstract class EventBase : MonoBehaviour
{
    /// <summary>�t�B�[���h</summary>
    protected MainScriptBase fieldScript;

    /// <summary>�C�x���g�����t���O�̕ۑ� "Event"�ŏI�����̂̂�</summary>
    private string saveName = string.Empty;

    /// <summary>
    /// �J�n��
    /// </summary>
    public virtual void Start()
    {
        // �t�B�[���h�X�N���v�g�@�ݒ肪�ʓ|�Ȃ̂�protected�ɂ��Ċ�{�I�ȍ\���Ȃ�擾
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

        // �N���X���������t���O�ۑ��p�ɂ���
        var name = GetType().Name;
        if (name.EndsWith("Event"))
        {
            saveName = name.Replace("Event", "");
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

        if (string.IsNullOrEmpty(saveName) == false)
        {
            // �C�x���g�t���O��ۑ�
            Global.GetSaveData().SetGameData(saveName, 1);
        }

        if (ManagerSceneScript.GetInstance().SceneState != ManagerSceneScript.State.Loading)
        {
            fieldScript.FieldState = MainScriptBase.State.Idle;
        }
    }

    /// <summary>
    /// �������Ƃ��邩
    /// </summary>
    /// <returns></returns>
    public bool IsShowed()
    {
        if (string.IsNullOrEmpty(saveName)) return false;

        return Global.GetSaveData().GetGameDataString(saveName) == "1";
    }

    /// <summary>
    /// ���ۂ̃C�x���g����
    /// </summary>
    /// <returns></returns>
    protected abstract IEnumerator Exec();
}
