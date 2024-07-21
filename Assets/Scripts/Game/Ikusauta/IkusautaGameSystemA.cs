using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 軍歌マチミニゲーム管理スクリプト
/// </summary>
public class IkusautaGameSystemA : GameSceneScriptBase
{
    #region 定数
    /// <summary>
    /// キャラ表示パターン
    /// </summary>
    private enum CharacterPattern : int
    {
        Waiting = 0,
        TukuyomiWin,
        MatiWin,
    }

    /// <summary>
    /// 札表示パターン
    /// </summary>
    private enum HudaPattern : int
    {
        None = 0,
        TukuyomiWin,
        MatiWin,
        Sikiri,
    }
    #endregion

    #region メンバー
    /// <summary>待機中表示キャラ</summary>
    public GameObject waitingCharacter = null;
    /// <summary>つくよみちゃん勝利時表示キャラ</summary>
    public GameObject tukuyomiWinCharacter = null;
    /// <summary>マチ勝利時表示キャラ</summary>
    public GameObject matiWinCharacter = null;

    /// <summary>ミニフェーダ</summary>
    public IkusautaGameMiniFader miniFader = null;

    /// <summary>ビックリマーク</summary>
    public GameObject ui_bikkuri = null;

    /// <summary>つくよみちゃん顔</summary>
    public IkusautaGameZoomFace ui_zoom_tukuyomi = null;
    /// <summary>マチ顔</summary>
    public IkusautaGameZoomFace ui_zoom_mati = null;

    /// <summary>勝者</summary>
    public GameObject ui_huda_winner = null;
    /// <summary>仕切り直し</summary>
    public GameObject ui_huda_sikiri = null;
    /// <summary>つくよみちゃん</summary>
    public GameObject ui_huda_tukuyomi = null;
    /// <summary>マチ</summary>
    public GameObject ui_huda_mati = null;
    /// <summary>札マスク</summary>
    public IkusautaGameHudaMask ui_huda_mask = null;

    /// <summary>つくよみちゃんマル</summary>
    public GameObject ui_maru_tukuyomi = null;
    /// <summary>マチマル</summary>
    public GameObject ui_maru_mati = null;
    /// <summary>つくよみちゃんバツ</summary>
    public GameObject ui_batu_tukuyomi = null;
    /// <summary>つくよみちゃんバツ　キャラ上</summary>
    public GameObject ui_batu_tukuyomi_chara = null;

    /// <summary>草</summary>
    public GameObject grasses = null;

    /// <summary>！が出る音</summary>
    public AudioClip se_bikkuri = null;
    /// <summary></summary>
    public AudioClip se_fault = null;
    /// <summary>マチ勝利SE</summary>
    public AudioClip se_mati_win = null;
    /// <summary>つくよみちゃん勝利SE</summary>
    public AudioClip se_tukuyomi_win = null;
    /// <summary>尺八SE</summary>
    public AudioClip se_syakuhati = null;

    #endregion

    #region プライベート変数
    /// <summary>仕切り直し</summary>
    private int faultCount;
    /// <summary>つくよみちゃん勝利点</summary>
    private int tukuyomiWinCount;
    /// <summary>マチ勝利点</summary>
    private int matiWinCount;
    #endregion

    #region 基底処理
    /// <summary>
    /// 初期化
    /// </summary>
    /// <returns></returns>
    override public IEnumerator Start()
    {
        faultCount = 0;
        tukuyomiWinCount = 0;
        matiWinCount = 0;
        UpdateScoreUI();
        ShowCharacter(CharacterPattern.Waiting);
        miniFader.Hide();
        ui_zoom_tukuyomi.gameObject.SetActive(false);
        ui_zoom_mati.gameObject.SetActive(false);
        ui_huda_tukuyomi.SetActive(false);
        ui_huda_mati.SetActive(false);
        ui_huda_winner.SetActive(false);
        ui_huda_sikiri.SetActive(false);
        ui_maru_tukuyomi.SetActive(false);
        ui_maru_mati.SetActive(false);
        ui_batu_tukuyomi.SetActive(false);
        ui_batu_tukuyomi_chara.SetActive(false);

        for (var i = 0; i < grasses.transform.childCount; ++i)
        {
            var grassAnim = grasses.transform.GetChild(i).gameObject.GetComponent<Animator>();

            grassAnim.PlayInFixedTime("obj_grass", 0, Util.RandomFloat(0, 1));
        }

        yield return base.Start();
    }
    #endregion

    #region コルーチン
    /// <summary>
    /// フェードイン後の初期処理
    /// </summary>
    /// <returns></returns>
    public override IEnumerator AfterFadeIn()
    {
        var tutorial = ManagerSceneScript.GetInstance().GetMinigameTutorialWindow();
        var input = InputManager.GetInstance();

        yield return base.AfterFadeIn();
        yield return new WaitForSeconds(1f);

        // チュートリアル表示
        tutorial.SetTitle(StringMinigameMessage.MatiA_Title);
        tutorial.SetText(StringMinigameMessage.MatiA_Tutorial);
        yield return tutorial.Open();
        yield return new WaitUntil(() => input.GetKeyPress(InputManager.Keys.South));
        yield return tutorial.Close();

        yield return new WaitForSeconds(0.5f);

        // 開始
        StartCoroutine(GameStart());
    }

    /// <summary>
    /// 勝負開始の流れ
    /// </summary>
    /// <returns></returns>
    private IEnumerator GameStart()
    {
        var input = InputManager.GetInstance();

        // 暗くする
        PlaySE(se_syakuhati);
        yield return miniFader.FadeOutDark(true);

        // 上下の顔アニメーション
        ui_zoom_tukuyomi.gameObject.SetActive(true);
        ui_zoom_mati.gameObject.SetActive(true);
        StartCoroutine(ui_zoom_tukuyomi.PlayAnim());
        StartCoroutine(ui_zoom_mati.PlayAnim());
        yield return new WaitWhile(() => ui_zoom_tukuyomi.IsActive());
        yield return new WaitForSeconds(1f);
        ui_zoom_tukuyomi.gameObject.SetActive(false);
        ui_zoom_mati.gameObject.SetActive(false);

        // 明るくする
        yield return miniFader.FadeOutDark(false);

        // ！が出る時間
        var waitTime = Util.RandomFloat(2f, 7f);

        while (waitTime > 0f)
        {
            if (input.GetKeyPress(InputManager.Keys.South))
            {
                // 出る前に押したら失敗＋
                StartCoroutine(FaultCoroutine());
                yield break;
            }

            yield return null;
            waitTime -= Time.deltaTime;
        }

        // ！出る
        yield return new WaitUntil(() => se_bikkuri.loadState == AudioDataLoadState.Loaded);
        PlaySE(se_bikkuri, 0.04f);
        ui_bikkuri.SetActive(true);
        // マチの反応速度
        var matiTime = CalcMatiTime();

        while (matiTime > 0f)
        {
            yield return null;
            
            if (input.GetKeyPress(InputManager.Keys.South))
            {
                // マチより早かったら勝ち
                StartCoroutine(TukuyomiWinCoroutine());
                yield break;
            }
            
            matiTime -= Time.deltaTime;
        }

        // マチの勝ち
        StartCoroutine(MatiWinCoroutine());
    }

    /// <summary>
    /// フライングで仕切り直し
    /// </summary>
    /// <returns></returns>
    private IEnumerator FaultCoroutine()
    {
        var manager = ManagerSceneScript.GetInstance();

        PlaySE(se_fault);
        // キャラにバツ表示
        ui_batu_tukuyomi_chara.SetActive(true);

        // 札表示
        ShowWinnerHuda(HudaPattern.Sikiri);
        yield return HudaMaskShowCoroutine();

        // スコア
        AddFault();

        if (matiWinCount >= 2)
        {
            // 終了してフィールドに戻る
            SetGameResult(false);
            ManagerSceneScript.GetInstance().ExitGame();
            yield break;
        }
        else
        {
            // フェードアウトして表示更新、次のゲーム開始
            yield return manager.FadeOut();
            UpdateScoreUI();
            ui_batu_tukuyomi_chara.SetActive(false);
            yield return manager.FadeIn();
            StartCoroutine(GameStart());
        }
    }

    /// <summary>
    /// つくよみちゃん勝利
    /// </summary>
    /// <returns></returns>
    private IEnumerator TukuyomiWinCoroutine()
    {
        var manager = ManagerSceneScript.GetInstance();
        PlaySE(se_tukuyomi_win);

        // ビックリマーク消す
        ui_bikkuri.SetActive(false);

        // 光
        StartCoroutine(miniFader.Flash());

        // キャラと札表示
        ShowCharacter(CharacterPattern.TukuyomiWin);
        ShowWinnerHuda(HudaPattern.TukuyomiWin);
        yield return HudaMaskShowCoroutine();

        // スコア
        AddTukuyomiWin();

        if (tukuyomiWinCount >= 2)
        {
            // 終了してフィールドに戻る
            SetGameResult(true);
            ManagerSceneScript.GetInstance().ExitGame();
            yield break;
        }
        else
        {
            // フェードアウトして表示更新、次のゲーム開始
            yield return manager.FadeOut();
            UpdateScoreUI();
            ShowCharacter(CharacterPattern.Waiting);
            yield return manager.FadeIn();
            StartCoroutine(GameStart());
        }
    }

    /// <summary>
    /// マチ勝利
    /// </summary>
    /// <returns></returns>
    private IEnumerator MatiWinCoroutine()
    {
        var manager = ManagerSceneScript.GetInstance();
        PlaySE(se_mati_win);

        // ビックリマーク消す
        ui_bikkuri.SetActive(false);

        // 光
        StartCoroutine(miniFader.Flash());

        // キャラと札表示
        ShowCharacter(CharacterPattern.MatiWin);
        ShowWinnerHuda(HudaPattern.MatiWin);
        yield return HudaMaskShowCoroutine();

        // スコア
        AddMatiWin();

        if (matiWinCount >= 2)
        {
            // 終了してフィールドに戻る
            SetGameResult(false);
            ManagerSceneScript.GetInstance().ExitGame();
            yield break;
        }
        else
        {
            // フェードアウトして表示更新、次のゲーム開始
            yield return manager.FadeOut();
            UpdateScoreUI();
            ShowCharacter(CharacterPattern.Waiting);
            yield return manager.FadeIn();
            StartCoroutine(GameStart());
        }
    }

    /// <summary>
    /// 札の表示
    /// </summary>
    /// <returns></returns>
    private IEnumerator HudaMaskShowCoroutine()
    {
        yield return ui_huda_mask.ShowHuda(true);
        yield return new WaitForSeconds(2f);
        yield return ui_huda_mask.ShowHuda(false);
        yield return new WaitForSeconds(1f);
    }

    #endregion

    #region プライベート
    /// <summary>
    /// マチの反応速度決定
    /// </summary>
    /// <returns></returns>
    private float CalcMatiTime()
    {
        var rand = Util.RandomFloat(0.2f, 0.25f);
        if (GetLoseCount() >= 3)
        {
            rand += (GetLoseCount() - 2) * 0.02f;
        }

        return rand;
    }

    /// <summary>
    /// フライング回数加算
    /// </summary>
    private void AddFault()
    {
        faultCount++;
        if (faultCount >= 2)
        {
            AddMatiWin();
        }
    }

    /// <summary>
    /// マチ勝利回数加算
    /// </summary>
    private void AddMatiWin()
    {
        matiWinCount++;
        faultCount = 0;
    }

    /// <summary>
    /// つくよみちゃん勝利回数加算
    /// </summary>
    private void AddTukuyomiWin()
    {
        tukuyomiWinCount++;
        faultCount = 0;
    }

    /// <summary>
    /// 勝利回数の表示更新
    /// </summary>
    private void UpdateScoreUI()
    {
        ui_maru_tukuyomi.SetActive(tukuyomiWinCount == 1);
        ui_maru_mati.SetActive(matiWinCount == 1);
        ui_batu_tukuyomi.SetActive(faultCount == 1);
    }

    /// <summary>
    /// キャラクター表示更新
    /// </summary>
    /// <param name="scene"></param>
    private void ShowCharacter(CharacterPattern scene)
    {
        waitingCharacter.SetActive(scene == CharacterPattern.Waiting);
        tukuyomiWinCharacter.SetActive(scene == CharacterPattern.TukuyomiWin);
        matiWinCharacter.SetActive(scene == CharacterPattern.MatiWin);
    }

    /// <summary>
    /// 札表示更新
    /// </summary>
    /// <param name="scene"></param>
    private void ShowWinnerHuda(HudaPattern scene)
    {
        ui_huda_mati.SetActive(scene == HudaPattern.MatiWin);
        ui_huda_tukuyomi.SetActive(scene == HudaPattern.TukuyomiWin);
        ui_huda_sikiri.SetActive(scene == HudaPattern.Sikiri);
        ui_huda_winner.SetActive(scene == HudaPattern.TukuyomiWin || scene == HudaPattern.MatiWin);
    }

    /// <summary>
    /// SE再生
    /// </summary>
    /// <param name="se"></param>
    /// <param name="startTime"></param>
    private void PlaySE(AudioClip se, float startTime = 0f)
    {
        ManagerSceneScript.GetInstance().soundMan.PlaySE(se, startTime);
    }
    #endregion
}
