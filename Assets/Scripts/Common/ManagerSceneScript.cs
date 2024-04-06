using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerSceneScript : MonoBehaviour
{
    /// <summary>�f�o�b�O�p</summary>
    public static Boolean isDebugLoad = false;

    #region �萔
    /// <summary>�t�F�[�h����</summary>
    const float FADE_TIME = 0.5f;

    /// <summary>
    /// �V�[�����
    /// </summary>
    public enum State : int
    {
        Loading = 0,
        Main,
        Game,
    }

    /// <summary>�V�[�����</summary>
    public State SceneState { get; private set; }
    #endregion

    #region �C���X�^���X
    /// <summary>�C���X�^���X</summary>
    private static ManagerSceneScript _instance = null;
    /// <summary>�C���X�^���X</summary>
    /// <returns></returns>
    public static ManagerSceneScript GetInstance() { return _instance; }

    /// <summary>
    /// �R���X�g���N�^
    /// </summary>
    public ManagerSceneScript()
    {
        _instance = this;
    }
    #endregion

    #region �����o�[
    /// <summary>���b�Z�[�W�E�B���h�E</summary>
    public GameObject messageWindow = null;

    /// <summary>�_�C�A���O�E�B���h�E</summary>
    public GameObject dialogWindow = null;

    /// <summary>�~�j�Q�[�������E�B���h�E</summary>
    public GameObject minigameTutorialWindow = null;

    /// <summary>�t�F�[�_</summary>
    public CanvasGroup fader = null;

    /// <summary>��{�V�[��</summary>
    public void SetMainScript(MainScriptBase script) { mainScript = script; }
    private MainScriptBase mainScript = null;

    /// <summary>�Q�[���V�[��</summary>
    public void SetGameScript(GameSceneScriptBase script) { gameScript = script; }
    private GameSceneScriptBase gameScript = null;
    /// <summary>�Q�[���V�[����</summary>
    private string gameSceneName = null;

    /// <summary>�T�E���h�Ǘ�</summary>
    public SoundManager soundMan = null;

    /// <summary>�J����</summary>
    public MainCamera mainCam = null;

    #endregion

    #region ������
    /// <summary>
    /// ������
    /// </summary>
    /// <returns></returns>
    IEnumerator Start()
    {
        Global.GetSaveData().Load();
        soundMan.UpdateBgmVolume();
        soundMan.UpdateSeVolume();
        soundMan.UpdateVoiceVolume();

        messageWindow.SetActive(false);
        dialogWindow.gameObject.SetActive(false);
        fader.gameObject.SetActive(true);
        fader.alpha = 1f;

        if (!isDebugLoad)
        {
            //todo:�����V�[�����[�h
            SceneManager.LoadSceneAsync("SampleScene", LoadSceneMode.Additive);
        }

        SceneState = State.Loading;
        yield return new WaitWhile(() => mainScript == null);

        // �Q�[���N�����ŏ��̏���
        InitMainScene();

        yield return FadeIn();
        SceneState = State.Main;
    }
    #endregion

    #region �O���[�o���v�f�擾
    /// <summary>���b�Z�[�W�E�B���h�E</summary>
    /// <returns></returns>
    public MessageWindow GetMessageWindow()
    {
        return messageWindow.GetComponent<MessageWindow>();
    }

    /// <summary>�~�j�Q�[�������E�B���h�E</summary>
    /// <returns></returns>
    public MinigameTutorialWindow GetMinigameTutorialWindow()
    {
        return minigameTutorialWindow.GetComponent<MinigameTutorialWindow>();
    }

    /// <summary>�_�C�A���O�E�B���h�E</summary>
    /// <returns></returns>
    public DialogWindow GetDialogWindow()
    {
        return dialogWindow.GetComponent<DialogWindow>();
    }
    #endregion

    #region �t�F�[�h�Ǘ�
    /// <summary>
    /// �t�F�[�h�A�E�g
    /// </summary>
    /// <returns></returns>
    public IEnumerator FadeOut()
    {
        fader.gameObject.SetActive(true);
        var alpha = new DeltaFloat();
        alpha.Set(0);
        alpha.MoveTo(1f, FADE_TIME, DeltaFloat.MoveType.LINE);
        while (alpha.IsActive())
        {
            alpha.Update(Time.deltaTime);
            fader.alpha = alpha.Get();
            yield return null;
        }
    }

    /// <summary>
    /// �t�F�[�h�C��
    /// </summary>
    /// <returns></returns>
    public IEnumerator FadeIn()
    {
        var alpha = new DeltaFloat();
        alpha.Set(1f);
        alpha.MoveTo(0f, FADE_TIME, DeltaFloat.MoveType.LINE);

        // Start�ŕ\�����������Ă��邪�A����t�F�[�h�C���n�܂�ƈ�u������̂�1�t���҂�
        fader.alpha = alpha.Get();
        yield return null;

        while (alpha.IsActive())
        {
            alpha.Update(Time.deltaTime);
            fader.alpha = alpha.Get();
            yield return null;
        }
        fader.alpha = 0f;
        fader.gameObject.SetActive(false);
    }
    #endregion

    #region �V�[���Ǘ�
    /// <summary>
    /// ���C���V�[���؂�ւ�
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="id">�v���C���[�ʒu</param>
    public void LoadMainScene(string sceneName, int id)
    {
        StartCoroutine(LoadMainSceneCoroutine(sceneName, id));
    }

    /// <summary>
    /// ���C���V�[���؂�ւ��R���[�`��
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    private IEnumerator LoadMainSceneCoroutine(string sceneName, int id)
    {
        // �t�F�[�h�A�E�g
        SceneState = State.Loading;
        yield return FadeOut();

        // ���V�[����ێ����ă��[�h�J�n
        var oldScript = mainScript;
        mainScript = null;
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        yield return new WaitUntil(() => mainScript != null);

        // BGM�f�[�^�擾
        var bgmData = mainScript.GetBgm();
        // �ύX�̕K�v������ꍇ�t�F�[�h�A�E�g��҂�
        yield return soundMan.FadeOutFieldBgm(false);

        // ���V�[��������ꍇ�A�����[�h
        if (!oldScript)
        {
            SceneManager.UnloadSceneAsync(oldScript.GetSceneName());
        }

        // ��������
        InitMainScene();
        // �t�F�[�h�C��
        yield return FadeIn();

        // �t�F�[�h�C����̏���
        yield return mainScript.AfterFadeIn();

        SceneState = State.Main;
    }

    /// <summary>
    /// ���C���V�[���ǂݍ��݌�A�t�F�[�h�A�E�g���O�ɂ��ׂ�����
    /// </summary>
    private void InitMainScene()
    {
        // BGM�J�n
        var bgmData = mainScript.GetBgm();
        soundMan.PlayFieldBgm(bgmData.Item1, bgmData.Item2);
    }

    /// <summary>
    /// �Q�[���V�[���Ăяo��
    /// </summary>
    /// <param name="sceneName"></param>
    public void StartGame(string sceneName)
    {
        gameSceneName = sceneName;
        StartCoroutine(StartGameCoroutine(sceneName));
    }

    /// <summary>
    /// �Q�[���V�[���Ăяo���R���[�`��
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    private IEnumerator StartGameCoroutine(string sceneName)
    {
        // �t�F�[�h�A�E�g���Ȃ���BGM���|�[�Y
        SceneState = State.Loading;
        StartCoroutine(soundMan.FadeOutFieldBgm(true));
        yield return FadeOut();

        // ���C���V�[�����X���[�v
        mainScript.Sleep();

        // �Q�[���V�[�������[�h
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        yield return new WaitWhile(() => gameScript == null);

        // �t�F�[�h�A�E�g���ꉞ�m�F���ăQ�[���pBGM�Đ�
        yield return new WaitWhile(() => soundMan.IsFieldBgmPlaying());
        soundMan.StartGameBgm(gameScript.bgmClip);

        // �t�F�[�h�C��
        yield return FadeIn();

        // �t�F�[�h�C����̏���
        yield return gameScript.AfterFadeIn();

        SceneState = State.Game;
    }

    /// <summary>
    /// �Q�[���V�[���I��
    /// </summary>
    public void ExitGame()
    {
        StartCoroutine(ExitGameCoroutine());
    }

    /// <summary>
    /// �Q�[���I���R���[�`��
    /// </summary>
    /// <returns></returns>
    private IEnumerator ExitGameCoroutine()
    {
        // �t�F�[�h�A�E�g���Ȃ���Q�[��BGM���~
        SceneState = State.Loading;
        StartCoroutine(soundMan.FadeOutGameBgm());
        yield return FadeOut();

        // �T�E���h�̃t�F�[�h�A�E�g���m�F���ăA�����[�h
        yield return new WaitWhile(() => soundMan.IsGameBgmPlaying());
        var unloadSync = SceneManager.UnloadSceneAsync(gameSceneName);
        yield return unloadSync;

        //���C���V�[���𕜋A
        StartCoroutine(soundMan.ResumeFieldBgm());
        mainScript.Awake();

        yield return mainScript.BeforeFadeIn();
        yield return FadeIn();
        yield return mainScript.AfterFadeIn();
        SceneState = State.Main;
    }
    #endregion
}
