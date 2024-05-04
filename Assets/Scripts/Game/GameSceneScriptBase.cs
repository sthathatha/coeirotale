using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneScriptBase : MonoBehaviour
{
    /// <summary>�炷BGM</summary>
    public AudioClip bgmClip = null;

    /// <summary>�Q�[�����[�h0�F�Q�[�����@1�F���X�{�X�O</summary>
    public int gameMode;

    /// <summary>
    /// �t�F�[�h�C�����O
    /// </summary>
    /// <returns></returns>
    virtual public IEnumerator Start()
    {
        SetGameResult(false);
        ManagerSceneScript.GetInstance().SetGameScript(this);

        var cam = ManagerSceneScript.GetInstance().mainCam;
        cam.SetTargetPos(new Vector2(0, 0));
        cam.Immediate();

        yield break;
    }

    /// <summary>
    /// �t���[������
    /// </summary>
    virtual public void Update()
    {

    }

    /// <summary>
    /// �t�F�[�h�C���I��������邱��
    /// </summary>
    /// <returns></returns>
    virtual public IEnumerator AfterFadeIn()
    {
        yield break;
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <returns></returns>
    protected int GetLoseCount()
    {
        return Global.GetTemporaryData().loseCount;
    }

    /// <summary>
    /// BGMClip
    /// </summary>
    /// <returns></returns>
    virtual public AudioClip GetBgmClip() { return  bgmClip; }

    /// <summary>
    /// ���ʐݒ�
    /// </summary>
    /// <param name="_win"></param>
    protected void SetGameResult(bool _win)
    {
        Global.GetTemporaryData().gameWon = _win;
    }
}
