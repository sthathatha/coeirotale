using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

/// <summary>
/// まつかりすく絶叫ゲーム
/// </summary>
public class MatukaGameSystemB : GameSceneScriptBase
{
    #region 定数

    /// <summary>勝負がつく差</summary>
    private const int PRESS_LIMIT = 25;

    /// <summary>敵の連打速度</summary>
    private const float ENEMY_PRESS_INTERVAL = 0.16f;

    /// <summary>開始時・押されたら負けの座標</summary>
    private const float X_LIMIT = 230f;
    /// <summary>レーザーオブジェクトの根本</summary>
    private const float LASER_BASE = 376f;

    /// <summary>勝利演出のヘッド位置</summary>
    private const float X_EXIT = 1000f;
    /// <summary>勝利演出のヘッド移動時間</summary>
    private const float WIN_HEAD_TIME = 0.4f;
    /// <summary>勝利演出の速度</summary>
    private const float WIN_SPEED =(X_EXIT - X_LIMIT) / WIN_HEAD_TIME;
    /// <summary>勝利演出の根本移動時間</summary>
    private const float WIN_BODY_TIME = (X_EXIT + LASER_BASE) / WIN_SPEED;

    #endregion

    #region メンバー

    /// <summary>まつかりすくA</summary>
    public MatukaGameCharacter matukaPlayer;
    /// <summary>にせまつかりすく</summary>
    public MatukaGameCharacter matukaEnemy;
    /// <summary>テキスト</summary>
    public TMP_Text messageText;

    public MatukaGameBLaser laserP;
    public MatukaGameBLaser laserE;

    /// <summary>叫びボイス</summary>
    public AudioClip shoutVoice1;
    /// <summary>叫びボイス</summary>
    public AudioClip shoutVoice2;
    /// <summary>叫びボイス</summary>
    public AudioClip shoutVoice3;
    /// <summary>叫びボイス</summary>
    public AudioClip shoutVoice4;

    #endregion

    #region 変数

    /// <summary>連打回数</summary>
    private int pressCount = 0;

    #endregion

    #region 基底処理
    /// <summary>
    /// 初期化
    /// </summary>
    /// <returns></returns>
    override public IEnumerator Start()
    {
        laserP.Show(false);
        laserE.Show(false);

        matukaPlayer.ShowObject(true, false, false, false);
        matukaEnemy.ShowObject(true, false, false, false);
        messageText.SetText("");

        yield return base.Start();
    }

    /// <summary>
    /// フェードイン後
    /// </summary>
    /// <returns></returns>
    public override IEnumerator AfterFadeIn()
    {
        yield return base.AfterFadeIn();

        StartCoroutine(GameCoroutine());
    }
    #endregion

    #region コルーチン

    /// <summary>
    /// メイン
    /// </summary>
    /// <returns></returns>
    private IEnumerator GameCoroutine()
    {
        var sound = ManagerSceneScript.GetInstance().soundMan;
        var tutorial = ManagerSceneScript.GetInstance().GetMinigameTutorialWindow();
        var input = InputManager.GetInstance();
        var cam = ManagerSceneScript.GetInstance().mainCam;
        var shoutCoroutine = ShoutCoroutine();

        // まつかシャウト
        matukaPlayer.ShowObject(false, true, true, false);
        matukaEnemy.ShowObject(false, true, true, false);
        sound.PlaySE(shoutVoice1);
        cam.PlayShake(Shaker.ShakeSize.Weak);
        // レーザー表示
        laserP.Show(true);
        laserE.Show(true);
        var tmp = new DeltaFloat();
        tmp.Set(X_LIMIT);
        tmp.MoveTo(0f, 0.4f, DeltaFloat.MoveType.LINE);
        while (tmp.IsActive())
        {
            yield return null;
            tmp.Update(Time.deltaTime);
            laserP.SetPos(tmp.Get());
            laserE.SetPos(-tmp.Get());
        }

        yield return new WaitForSeconds(1.5f);
        // チュートリアル表示
        tutorial.SetTitle(StringMinigameMessage.MatukaA_Title);
        tutorial.SetText(StringMinigameMessage.MatukaA_Tutorial);
        yield return tutorial.Open();
        yield return new WaitUntil(() => input.GetKeyPress(InputManager.Keys.South));
        yield return tutorial.Close();

        StartCoroutine(shoutCoroutine);
        yield return new WaitForSeconds(2f);
        // 判定開始
        var isWin = false;
        var enemyInt = ENEMY_PRESS_INTERVAL;
        while (true)
        {
            yield return null;
            if (input.GetKeyPress(InputManager.Keys.South))
            {
                pressCount++;
                if (pressCount >= PRESS_LIMIT)
                {
                    isWin = true;
                    break;
                }
            }

            enemyInt -= Time.deltaTime;
            if (enemyInt < 0f)
            {
                enemyInt += ENEMY_PRESS_INTERVAL;
                pressCount--;
                if (pressCount <= -PRESS_LIMIT)
                {
                    isWin = false;
                    break;
                }
            }

            UpdateLaser();
        }
        StopCoroutine(shoutCoroutine);
        cam.StopShake();

        // 勝敗
        if (isWin)
        {
            yield return WinCoroutine();
        }
        else
        {
            yield return LoseCoroutine();
        }
        yield return new WaitForSeconds(1f);
        ManagerSceneScript.GetInstance().NextGame("GameSceneIkusautaB");
    }

    #endregion

    #region プライベート

    /// <summary>
    /// レーザー表示更新
    /// </summary>
    private void UpdateLaser()
    {
        // 位置計算
        var x = -pressCount * X_LIMIT / PRESS_LIMIT;
        laserP.SetPos(x);
        laserE.SetPos(x);
    }

    /// <summary>
    /// 叫び再生コルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShoutCoroutine()
    {
        var sound = ManagerSceneScript.GetInstance().soundMan;

        while (true)
        {
            yield return new WaitForSeconds(1.5f);

            sound.PlaySE(Util.RandomInt(0, 3) switch
            {
                0 => shoutVoice1,
                1 => shoutVoice2,
                2 => shoutVoice3,
                _ => shoutVoice4,
            });
        }
    }

    /// <summary>
    /// 勝利演出
    /// </summary>
    /// <returns></returns>
    private IEnumerator WinCoroutine()
    {
        laserE.Show(false);
        matukaEnemy.SetRenderPriority(-100);
        var x = new DeltaFloat();
        x.Set(-X_LIMIT);
        x.MoveTo(-X_EXIT, WIN_HEAD_TIME, DeltaFloat.MoveType.LINE);
        while (x.IsActive())
        {
            yield return null;
            x.Update(Time.deltaTime);
            laserP.SetPos(x.Get());
        }
        yield return new WaitForSeconds(0.5f);
        matukaEnemy.ShowObject(false, false, false, true);

        x.Set(LASER_BASE);
        x.MoveTo(-X_EXIT, WIN_BODY_TIME, DeltaFloat.MoveType.LINE);
        var y = laserP.transform.localPosition.y;
        while (x.IsActive())
        {
            yield return null;
            x.Update(Time.deltaTime);
            laserP.transform.localPosition = new Vector3(x.Get(), y);
        }
        yield return new WaitForSeconds(1f);

        Global.GetTemporaryData().bossRushMatukaWon = true;
        messageText.SetText(StringMinigameMessage.MatukaB_Win);
        matukaPlayer.ShowObject(false, true, false, false);
    }

    /// <summary>
    /// 敗北演出
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoseCoroutine()
    {
        laserP.Show(false);
        matukaPlayer.SetRenderPriority(-100);
        var x = new DeltaFloat();
        x.Set(X_LIMIT);
        x.MoveTo(X_EXIT, WIN_HEAD_TIME, DeltaFloat.MoveType.LINE);
        while (x.IsActive())
        {
            yield return null;
            x.Update(Time.deltaTime);
            laserE.SetPos(x.Get());
        }
        yield return new WaitForSeconds(0.5f);
        matukaPlayer.ShowObject(false, false, false, true);

        x.Set(-LASER_BASE);
        x.MoveTo(X_EXIT, WIN_BODY_TIME, DeltaFloat.MoveType.LINE);
        var y = laserE.transform.localPosition.y;
        while (x.IsActive())
        {
            yield return null;
            x.Update(Time.deltaTime);
            laserE.transform.localPosition = new Vector3(x.Get(), y);
        }
        yield return new WaitForSeconds(1f);

        Global.GetTemporaryData().bossRushMatukaWon = false;
        messageText.SetText(StringMinigameMessage.MatukaB_Lose);
        matukaEnemy.ShowObject(false, true, false, false);
    }

    #endregion
}
