using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScriptBase : MonoBehaviour
{
    #region �����o�[

    /// <summary>�X���[�v����Active=false����e�I�u�W�F�N�g</summary>
    public GameObject objectParent = null;

    /// <summary>BGM</summary>
    public AudioClip bgmClip = null;

    /// <summary>�v���C���[</summary>
    public GameObject playerObj = null;

    /// <summary>�ǔ�����݂����</summary>
    public GameObject tukuyomiObj = null;

    #endregion

    #region �ϐ�
    /// <summary>
    /// �t�B�[���h�̏��
    /// </summary>
    public enum State : int
    {
        Idle = 0,
        Event,
    }

    /// <summary>
    /// �t�B�[���h�̏��
    /// </summary>
    public State FieldState
    {
        get; set;
    }
    #endregion

    #region ���
    /// <summary>
    /// �J�n��
    /// </summary>
    /// <returns></returns>
    virtual protected IEnumerator Start()
    {
        // ���ڋN�����Ƀ}�l�[�W���N��
        if (!ManagerSceneScript.GetInstance())
        {
            ManagerSceneScript.isDebugLoad = true;
            SceneManager.LoadScene("_ManagerScene", LoadSceneMode.Additive);
            yield return null;
        }

        ManagerSceneScript.GetInstance().SetMainScript(this);
    }

    virtual protected void Update()
    {
    }
    #endregion

    /// <summary>
    /// �Q�[���J�n�p�ɃX���[�v
    /// </summary>
    virtual public void Sleep()
    {
        objectParent?.SetActive(false);
    }

    /// <summary>
    /// �Q�[���I�����ɍĊJ
    /// </summary>
    virtual public void Awake()
    {
        objectParent?.SetActive(true);

        if (!ManagerSceneScript.GetInstance()) return;

        if (playerObj != null)
        {
            var cam = ManagerSceneScript.GetInstance().mainCam;
            cam.SetTargetPos(playerObj);
            cam.Immediate();
        }
    }

    /// <summary>
    /// �t�F�[�h�C�����O�ɂ�邱��
    /// </summary>
    /// <returns></returns>
    virtual public IEnumerator BeforeFadeIn()
    {
        if (playerObj != null)
        {
            var cam = ManagerSceneScript.GetInstance().mainCam;
            cam.SetTargetPos(playerObj);
            cam.Immediate();
        }

        yield break;
    }

    /// <summary>
    /// �t�F�[�h�C���I��������邱��
    /// </summary>
    /// <returns></returns>
    virtual public IEnumerator AfterFadeIn()
    {
        FieldState = State.Idle;
        yield break;
    }

    /// <summary>
    /// BGM�ݒ�@���ꏈ���̏ꍇ�̓I�[�o�[���C�h
    /// </summary>
    /// <returns></returns>
    virtual public Tuple<SoundManager.FieldBgmType, AudioClip> GetBgm()
    {
        return new Tuple<SoundManager.FieldBgmType, AudioClip>(SoundManager.FieldBgmType.Clip, bgmClip);
    }

    /// <summary>
    /// �ėp���W���擾
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public GeneralPosition SearchGeneralPosition(int id)
    {
        var poses = objectParent.transform.GetComponentsInChildren<GeneralPosition>();
        if (poses.Length == 0) return null;

        if (id < 0)
        {
            return poses[0];
        }

        return poses.FirstOrDefault(p => p.id == id);
    }

    /// <summary>
    /// �v���C���[�ʒu�ݒ�E����݂������Ǐ]
    /// </summary>
    /// <param name="id">GeneralPosition</param>
    public void InitPlayerPos(int id)
    {
        var pos = SearchGeneralPosition(id);
        if (pos == null) return;

        SetPlayerPos(pos);
    }

    /// <summary>
    /// �v���C���[�ʒu�ݒ�E����݂������Ǐ]
    /// </summary>
    /// <param name="gp"></param>
    protected void SetPlayerPos(GeneralPosition gp)
    {
        // �v���C���[
        if (playerObj != null)
        {
            playerObj.transform.position = gp.GetPosition();
        }

        // ����݂����
        if (tukuyomiObj != null)
        {
            tukuyomiObj.GetComponent<TukuyomiScript>().InitTrace();
        }
    }
}
