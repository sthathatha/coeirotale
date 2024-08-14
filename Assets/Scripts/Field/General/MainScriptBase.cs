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

    /// <summary>
    /// ���s���C�x���g
    /// </summary>
    protected List<EventBase> activeEvent;
    #endregion

    #region ���

    /// <summary>
    /// �R���X�g���N�^
    /// </summary>
    public MainScriptBase()
    {
        activeEvent = new List<EventBase>();
    }

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
            Global.GetSaveData().LoadGameData();
            SceneManager.LoadScene("_ManagerScene", LoadSceneMode.Additive);
            yield return null;
        }

        ManagerSceneScript.GetInstance().SetMainScript(this);
    }

    virtual protected void Update()
    {
    }

    #endregion

    #region �p�u���b�N���\�b�h

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
    virtual public void AwakeFromGame()
    {
        objectParent?.SetActive(true);

        if (!ManagerSceneScript.GetInstance()) return;

        if (playerObj != null)
        {
            var scr = playerObj.GetComponent<PlayerScript>();
            if (scr?.IsCameraEnable() == true)
            {
                var cam = ManagerSceneScript.GetInstance().mainCam;
                cam.SetTargetPos(playerObj);
                cam.Immediate();
            }
        }

        var characters = objectParent.GetComponentsInChildren<CharacterScript>();
        foreach (var chara in characters)
        {
            if (chara.isActiveAndEnabled) chara.AwakeResetDirection();
        }
    }

    /// <summary>
    /// ���[�h��ŏ��̂P��
    /// </summary>
    /// <returns></returns>
    virtual public IEnumerator BeforeInitFadeIn()
    {
        if (Global.GetSaveData().GetGameDataInt(F204System.WALL_OPEN_FLG) == 1)
        {
            tukuyomiObj.SetActive(false);
        }

        var playerScr = playerObj?.GetComponent<PlayerScript>();
        if (playerScr != null)
        {
            playerScr.FieldInit();
        }

        yield break;
    }

    /// <summary>
    /// �t�F�[�h�C�����O�ɂ�邱��
    /// </summary>
    /// <returns></returns>
    virtual public IEnumerator BeforeFadeIn()
    {
        if (playerObj != null)
        {
            var scr = playerObj.GetComponent<PlayerScript>();
            if (scr.IsCameraEnable())
            {
                var cam = ManagerSceneScript.GetInstance().mainCam;
                cam.SetTargetPos(playerObj);
                cam.Immediate();
            }
        }

        yield break;
    }

    /// <summary>
    /// �t�F�[�h�C���I��������邱��
    /// </summary>
    /// <returns></returns>
    virtual public IEnumerator AfterFadeIn(bool init)
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
    /// �C�x���g�J�n
    /// </summary>
    /// <param name="ev"></param>
    public void ActivateEvent(EventBase ev)
    {
        activeEvent.Add(ev);
    }

    /// <summary>
    /// �C�x���g�I��
    /// </summary>
    /// <param name="ev"></param>
    public void EndEvent(EventBase ev)
    {
        activeEvent.Remove(ev);
    }

    /// <summary>
    /// �C�x���g���s�����ۂ�
    /// </summary>
    /// <returns></returns>
    public bool IsEventPlaying()
    {
        return activeEvent.Count > 0;
    }

    #endregion

    #region �v���e�N�g���\�b�h

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

            var cam = ManagerSceneScript.GetInstance().mainCam;
            cam.SetTargetPos(playerObj);
            cam.Immediate();
        }

        // ����݂����
        if (tukuyomiObj != null)
        {
            var dir = gp.direction switch
            {
                "up" => Constant.Direction.Up,
                "right" => Constant.Direction.Right,
                "left" => Constant.Direction.Left,
                _ => Constant.Direction.Down
            };

            tukuyomiObj.GetComponent<TukuyomiScript>().InitTrace(dir);
        }
    }

    #endregion
}
