using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerSceneScript : MonoBehaviour
{
    public static Boolean isDebugLoad = false;

    #region 定数
    const float FADE_TIME = 0.5f;

    /// <summary>
    /// シーン状態
    /// </summary>
    public enum State : int
    {
        Loading = 0,
        Main,
        Game,
    }

    /// <summary>シーン状態</summary>
    public State SceneState { get; private set; }
    #endregion

    #region インスタンス
    private static ManagerSceneScript _instance = null;
    public static ManagerSceneScript GetInstance() { return _instance; }

    public ManagerSceneScript()
    {
        _instance = this;
    }
    #endregion

    #region メンバー
    /// <summary>メッセージウィンドウ</summary>
    public GameObject messageWindow = null;

    /// <summary>フェーダ</summary>
    public CanvasGroup fader = null;

    /// <summary>基本シーン</summary>
    public void SetMainScript(MainScriptBase script) { mainScript = script; }
    private MainScriptBase mainScript = null;

    /// <summary>ゲームシーン</summary>
    public void SetGameScript(GameSceneScriptBase script) { gameScript = script; }
    private GameSceneScriptBase gameScript = null;
    /// <summary>ゲームシーン名</summary>
    private string gameSceneName = null;
    #endregion

    #region 初期化
    IEnumerator Start()
    {
        fader.gameObject.SetActive(true);
        fader.alpha = 1f;

        if (!isDebugLoad)
        {
            //todo:初期シーンロード
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


    #region グローバル要素取得
    /// <summary>メッセージウィンドウ</summary>
    /// <returns></returns>
    public MessageWindow GetMessageWindow()
    {
        return messageWindow.GetComponent<MessageWindow>();
    }
    #endregion

    #region フェード管理
    /// <summary>
    /// フェードアウト
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
    /// フェードイン
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

    #region シーン管理
    /// <summary>
    /// メインシーン切り替え
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="id">プレイヤー位置</param>
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
    /// ゲームシーン呼び出し
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

        //メインシーンをスリープ
        mainScript.Sleep();

        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        yield return new WaitWhile(() => gameScript == null);
        yield return FadeIn();

        SceneState = State.Game;
    }

    /// <summary>
    /// ゲームシーン終了
    /// </summary>
    public void ExitGame()
    {
        StartCoroutine (ExitGameCoroutine());
    }

    private IEnumerator ExitGameCoroutine()
    {
        SceneState = State.Loading;
        yield return FadeOut();

        //アンロード
        var unloadSync = SceneManager.UnloadSceneAsync(gameSceneName);
        yield return unloadSync;

        //メインシーンを復帰
        mainScript.Awake();

        yield return FadeIn();

        SceneState = State.Main;
    }
    #endregion
}
