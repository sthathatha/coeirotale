using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �t�B�[���h�C�x���g��{����
/// </summary>
public abstract class EventBase : MonoBehaviour
{
    /// <summary>�����񐔃J�E���g���Z�[�u����</summary>
    public bool SaveViewFlag = false;

    /// <summary>�����񐔕ۑ���</summary>
    public string saveName = string.Empty;

    /// <summary>�t�B�[���h</summary>
    protected MainScriptBase fieldScript;

    /// <summary>������</summary>
    protected int viewCount = 0;

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

        if (SaveViewFlag)
        {
            viewCount = Global.GetSaveData().GetGameDataInt(saveName);
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

        if (SaveViewFlag)
        {
            // �C�x���g�����񐔂�ۑ�
            if (viewCount < int.MaxValue)
            {
                viewCount++;
            }

            Global.GetSaveData().SetGameData(saveName, viewCount);
        }

        if (ManagerSceneScript.GetInstance().SceneState != ManagerSceneScript.State.Loading)
        {
            fieldScript.FieldState = MainScriptBase.State.Idle;
        }
    }

    /// <summary>
    /// ���ۂ̃C�x���g����
    /// </summary>
    /// <returns></returns>
    protected abstract IEnumerator Exec();
}
