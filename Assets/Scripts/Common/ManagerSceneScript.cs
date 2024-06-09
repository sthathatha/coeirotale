using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 基本マネージャーScene
/// </summary>
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

    /// <summary>オプションウィンドウ</summary>
    public GameObject optionWindow = null;

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

    /// <summary>サウンド管理</summary>
    public SoundManager soundMan = null;

    /// <summary>カメラ</summary>
    public MainCamera mainCam = null;

    #endregion

    #region 変数

    /// <summary>プレイヤー初期化位置</summary>
    protected int initId = 0;

    #endregion

    #region 初期化
    /// <summary>
    /// 初期化
    /// </summary>
    /// <returns></returns>
    IEnumerator Start()
    {
        Global.GetSaveData().LoadSystemData();
        soundMan.UpdateBgmVolume();
        soundMan.UpdateSeVolume();
        soundMan.UpdateVoiceVolume();

        messageWindow.SetActive(false);
        dialogWindow.SetActive(false);
        optionWindow.SetActive(false);
        fader.gameObject.SetActive(true);
        fader.alpha = 1f;

        if (!isDebugLoad)
        {
            //初期シーンロード
            SceneManager.LoadSceneAsync("TitleScene", LoadSceneMode.Additive);
        }
        else
        {
            //デバッグ時はセーブデータをロード
            Global.GetSaveData().LoadGameData();
        }

        SceneState = State.Loading;
        yield return new WaitWhile(() => mainScript == null);

        // ゲーム起動時最初の処理
        InitMainScene();

        yield return mainScript.BeforeInitFadeIn();
        yield return mainScript.BeforeFadeIn();
        yield return FadeIn();
        yield return mainScript.AfterFadeIn();
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

    /// <summary>オプションウィンドウ</summary>
    /// <returns></returns>
    public OptionWindow GetOptionWindow()
    {
        return optionWindow.GetComponent<OptionWindow>();
    }
    #endregion

    #region フェード管理
    /// <summary>
    /// フェードアウト
    /// </summary>
    /// <param name="time">フェード時間　未指定でデフォルト</param>
    /// <returns></returns>
    public IEnumerator FadeOut(float time = -1f)
    {
        var fadeTime = time > 0f ? time : FADE_TIME;
        fader.gameObject.SetActive(true);
        var alpha = new DeltaFloat();
        alpha.Set(0);
        alpha.MoveTo(1f, fadeTime, DeltaFloat.MoveType.LINE);
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
    /// <param name="time">フェード時間　未指定でデフォルト</param>
    /// <returns></returns>
    public IEnumerator FadeIn(float time = -1f)
    {
        var fadeTime = time > 0f ? time : FADE_TIME;
        var alpha = new DeltaFloat();
        alpha.Set(1f);
        alpha.MoveTo(0f, fadeTime, DeltaFloat.MoveType.LINE);

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

    /// <summary>
    /// 瞬時に暗くする
    /// </summary>
    public void FadeOutNoWait()
    {
        fader.alpha = 1f;
        fader.gameObject.SetActive(true);
    }

    /// <summary>
    /// 瞬時に明るくする
    /// </summary>
    public void FadeInNoWait()
    {
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
    /// 初期化位置取得
    /// </summary>
    /// <returns></returns>
    public int GetInitId() { return initId; }

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
        initId = id;

        // 旧シーンを保持してロード開始
        var oldScript = mainScript;
        if (oldScript != null)
        {
            // オブジェクトが重なると誤作動するので停止だけする
            oldScript.Sleep();
        }

        mainScript = null;
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        yield return new WaitUntil(() => mainScript != null);

        // BGMデータ取得
        var bgmData = mainScript.GetBgm();
        // 変更の必要がある場合フェードアウトを待つ
        if (soundMan.IsNeedChangeFieldBgm(bgmData.Item1))
        {
            yield return soundMan.FadeOutFieldBgm(false);
        }

        // 旧シーンがある場合アンロード
        if (oldScript != null)
        {
            SceneManager.UnloadSceneAsync(oldScript.gameObject.scene);
        }

        // 初期処理
        InitMainScene(id);
        // フェードイン前の処理
        yield return mainScript.BeforeInitFadeIn();
        yield return mainScript.BeforeFadeIn();
        // フェードイン
        yield return FadeIn();
        // フェードイン後の処理
        yield return mainScript.AfterFadeIn();

        SceneState = State.Main;
    }

    /// <summary>
    /// メインシーン読み込み後、フェードアウト直前にやるべきこと
    /// </summary>
    private void InitMainScene(int id = -1)
    {
        // BGM開始
        var bgmData = mainScript.GetBgm();
        soundMan.PlayFieldBgm(bgmData.Item1, bgmData.Item2);

        // 汎用座標位置に初期化
        mainScript.InitPlayerPos(id);
    }

    /// <summary>
    /// ゲームシーン呼び出し
    /// </summary>
    /// <param name="sceneName"></param>
    public void StartGame(string sceneName)
    {
        StartCoroutine(StartGameCoroutine(sceneName));
    }

    /// <summary>
    /// ゲームシーン呼び出しコルーチン
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    private IEnumerator StartGameCoroutine(string sceneName)
    {
        // フェードアウトしながらBGMをポーズ
        SceneState = State.Loading;
        StartCoroutine(soundMan.FadeOutFieldBgm(true));
        yield return FadeOut();

        // メインシーンをスリープ
        mainScript.Sleep();

        // ゲームシーンをロード
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        yield return new WaitWhile(() => gameScript == null);

        // フェードアウトを一応確認してゲーム用BGM再生
        yield return new WaitWhile(() => soundMan.IsFieldBgmPlaying());
        soundMan.StartGameBgm(gameScript.bgmClip);

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
        // フェードアウトしながらゲームBGMを停止
        SceneState = State.Loading;
        StartCoroutine(soundMan.FadeOutGameBgm());
        yield return FadeOut();

        // サウンドのフェードアウトを確認してアンロード
        yield return new WaitWhile(() => soundMan.IsGameBgmPlaying());
        var unloadSync = SceneManager.UnloadSceneAsync(gameScript.gameObject.scene);
        yield return unloadSync;

        //メインシーンを復帰
        StartCoroutine(soundMan.ResumeFieldBgm());
        mainScript.Awake();

        yield return mainScript.BeforeFadeIn();
        yield return FadeIn();
        yield return mainScript.AfterFadeIn();
        SceneState = State.Main;
    }
    #endregion
}
