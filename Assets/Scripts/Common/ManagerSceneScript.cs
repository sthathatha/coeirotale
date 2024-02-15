using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerSceneScript : MonoBehaviour
{
    public static Boolean isDebugLoad = false;

    #region �萔
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
    private static ManagerSceneScript _instance = null;
    public static ManagerSceneScript GetInstance() { return _instance; }

    public ManagerSceneScript()
    {
        _instance = this;
    }
    #endregion

    #region �����o�[
    /// <summary>���b�Z�[�W�E�B���h�E</summary>
    public GameObject messageWindow = null;

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
    #endregion

    #region ������
    IEnumerator Start()
    {
        fader.gameObject.SetActive(true);
        fader.alpha = 1f;

        if (!isDebugLoad)
        {
            //todo:�����V�[�����[�h
            SceneManager.LoadSceneAsync("SampleScene", LoadSceneMode.Additive);
        }

        SceneState = State.Loading;
        yield return new WaitWhile(() => mainScript == null);
        yield return FadeIn();
        SceneState = State.Main;
    }
    #endregion

    void Update()
    {

    }


    #region �O���[�o���v�f�擾
    /// <summary>���b�Z�[�W�E�B���h�E</summary>
    /// <returns></returns>
    public MessageWindow GetMessageWindow()
    {
        return messageWindow.GetComponent<MessageWindow>();
    }
    #endregion

    #region �t�F�[�h�Ǘ�
    /// <summary>
    /// �t�F�[�h�A�E�g
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeOut()
    {
        fader.gameObject.SetActive(true);
        var alpha = new DeltaFloat();
        alpha.Set(0);
        alpha.MoveTo(1f, FADE_TIME, DeltaFloat.MoveType.LINE);
        while (alpha.IsActive())
        {
            fader.alpha = alpha.Get();
            yield return null;
            alpha.Update(Time.deltaTime);
        }
    }

    /// <summary>
    /// �t�F�[�h�C��
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeIn()
    {
        var alpha = new DeltaFloat();
        alpha.Set(1f);
        alpha.MoveTo(0f, FADE_TIME, DeltaFloat.MoveType.LINE);
        while (alpha.IsActive())
        {
            fader.alpha = alpha.Get();
            yield return null;
            alpha.Update(Time.deltaTime);
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

    private IEnumerator LoadMainSceneCoroutine(string sceneName, int id)
    {
        SceneState = State.Loading;
        yield return FadeOut();

        if (!mainScript)
        {
            SceneManager.UnloadSceneAsync(mainScript.GetSceneName());
            mainScript = null;
        }
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        yield return FadeIn();
        SceneState = State.Game;
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

    private IEnumerator StartGameCoroutine(string sceneName)
    {
        SceneState = State.Loading;
        yield return FadeOut();

        //���C���V�[�����X���[�v
        mainScript.Sleep();

        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        yield return new WaitWhile(() => gameScript == null);
        yield return FadeIn();

        SceneState = State.Game;
    }

    /// <summary>
    /// �Q�[���V�[���I��
    /// </summary>
    public void ExitGame()
    {
        StartCoroutine (ExitGameCoroutine());
    }

    private IEnumerator ExitGameCoroutine()
    {
        SceneState = State.Loading;
        yield return FadeOut();

        //�A�����[�h
        var unloadSync = SceneManager.UnloadSceneAsync(gameSceneName);
        yield return unloadSync;

        //���C���V�[���𕜋A
        mainScript.Awake();

        yield return FadeIn();

        SceneState = State.Main;
    }
    #endregion
}
