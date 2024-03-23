using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerSceneScript : MonoBehaviour
{
    /// <summary>デバッグ用</summary>
    public static Boolean isDebugLoad = false;

    #region 定数
    /// <summary>フェード時間</summary>
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
    /// <summary>インスタンス</summary>
    private static ManagerSceneScript _instance = null;
    /// <summary>インスタンス</summary>
    /// <returns></returns>
    public static ManagerSceneScript GetInstance() { return _instance; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public ManagerSceneScript()
    {
        _instance = this;
    }
    #endregion

    #region メンバー
    /// <summary>メッセージウィンドウ</summary>
    public GameObject messageWindow = null;

    /// <summary>ダイアログウィンドウ</summary>
    public GameObject dialogWindow = null;

    /// <summary>ミニゲーム説明ウィンドウ</summary>
    public GameObject minigameTutorialWindow = null;

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

    /// <summary>サウンド管理</summary>
    public SoundManager soundManager = null;
    #endregion

    #region 初期化
    /// <summary>
    /// 初期化
    /// </summary>
    /// <returns></returns>
    IEnumerator Start()
    {
        Global.GetSaveData().Load();
        soundManager.UpdateBgmVolume();
        soundManager.UpdateSeVolume();
        soundManager.UpdateVoiceVolume();

        messageWindow.SetActive(false);
        dialogWindow.gameObject.SetActive(false);
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

    #region グローバル要素取得
    /// <summary>メッセージウィンドウ</summary>
    /// <returns></returns>
    public MessageWindow GetMessageWindow()
    {
        return messageWindow.GetComponent<MessageWindow>();
    }

    /// <summary>ミニゲーム説明ウィンドウ</summary>
    /// <returns></returns>
    public MinigameTutorialWindow GetMinigameTutorialWindow()
    {
        return minigameTutorialWindow.GetComponent<MinigameTutorialWindow>();
    }

    /// <summary>ダイアログウィンドウ</summary>
    /// <returns></returns>
    public DialogWindow GetDialogWindow()
    {
        return dialogWindow.GetComponent<DialogWindow>();
    }
    #endregion

    #region フェード管理
    /// <summary>
    /// フェードアウト
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
    /// フェードイン
    /// </summary>
    /// <returns></returns>
    public IEnumerator FadeIn()
    {
        var alpha = new DeltaFloat();
        alpha.Set(1f);
        alpha.MoveTo(0f, FADE_TIME, DeltaFloat.MoveType.LINE);

        // Startで表示初期化しているが、直後フェードイン始まると一瞬見えるので1フレ待つ
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

    /// <summary>
    /// メインシーン切り替えコルーチン
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    private IEnumerator LoadMainSceneCoroutine(string sceneName, int id)
    {
        // フェードアウト
        SceneState = State.Loading;
        yield return FadeOut();

        // 旧シーンを保持してロード
        var oldScript = mainScript;
        mainScript = null;
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        yield return new WaitUntil(() => mainScript != null);

        // 旧シーンがある場合閉じる
        if (!oldScript)
        {
            SceneManager.UnloadSceneAsync(oldScript.GetSceneName());
        }

        // フェードイン
        yield return FadeIn();

        // フェードイン後の処理
        yield return mainScript.AfterFadeIn();

        SceneState = State.Main;
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

    /// <summary>
    /// ゲームシーン呼び出しコルーチン
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    private IEnumerator StartGameCoroutine(string sceneName)
    {
        // フェードアウト
        SceneState = State.Loading;
        yield return FadeOut();

        // メインシーンをスリープ
        mainScript.Sleep();

        // ゲームシーンをロード
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        yield return new WaitWhile(() => gameScript == null);

        // ゲーム用BGM再生
        soundManager.StartGameBgm(gameScript.bgmClip);

        // フェードイン
        yield return FadeIn();

        // フェードイン後の処理
        yield return gameScript.AfterFadeIn();

        SceneState = State.Game;
    }

    /// <summary>
    /// ゲームシーン終了
    /// </summary>
    public void ExitGame()
    {
        StartCoroutine(ExitGameCoroutine());
    }

    /// <summary>
    /// ゲーム終了コルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator ExitGameCoroutine()
    {
        SceneState = State.Loading;
        yield return FadeOut();

        // ゲームのをフェードアウトしてフィールドのBGMを復帰
        yield return soundManager.ResumeBgmFromGame();

        // アンロード
        var unloadSync = SceneManager.UnloadSceneAsync(gameSceneName);
        yield return unloadSync;

        //メインシーンを復帰
        mainScript.Awake();

        yield return mainScript.BeforeFadeIn();
        yield return FadeIn();
        yield return mainScript.AfterFadeIn();
        SceneState = State.Main;
    }
    #endregion
}
