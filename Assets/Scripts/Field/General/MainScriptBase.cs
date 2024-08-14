using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScriptBase : MonoBehaviour
{
    #region メンバー

    /// <summary>スリープ時にActive=falseする親オブジェクト</summary>
    public GameObject objectParent = null;

    /// <summary>BGM</summary>
    public AudioClip bgmClip = null;

    /// <summary>プレイヤー</summary>
    public GameObject playerObj = null;

    /// <summary>追尾つくよみちゃん</summary>
    public GameObject tukuyomiObj = null;

    #endregion

    #region 変数
    /// <summary>
    /// フィールドの状態
    /// </summary>
    public enum State : int
    {
        Idle = 0,
        Event,
    }

    /// <summary>
    /// フィールドの状態
    /// </summary>
    public State FieldState
    {
        get; set;
    }

    /// <summary>
    /// 実行中イベント
    /// </summary>
    protected List<EventBase> activeEvent;
    #endregion

    #region 基底

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public MainScriptBase()
    {
        activeEvent = new List<EventBase>();
    }

    /// <summary>
    /// 開始時
    /// </summary>
    /// <returns></returns>
    virtual protected IEnumerator Start()
    {
        // 直接起動時にマネージャ起動
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

    #region パブリックメソッド

    /// <summary>
    /// ゲーム開始用にスリープ
    /// </summary>
    virtual public void Sleep()
    {
        objectParent?.SetActive(false);
    }

    /// <summary>
    /// ゲーム終了時に再開
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
    /// ロード後最初の１回
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
    /// フェードイン直前にやること
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
    /// フェードイン終わったらやること
    /// </summary>
    /// <returns></returns>
    virtual public IEnumerator AfterFadeIn(bool init)
    {
        FieldState = State.Idle;
        yield break;
    }

    /// <summary>
    /// BGM設定　特殊処理の場合はオーバーライド
    /// </summary>
    /// <returns></returns>
    virtual public Tuple<SoundManager.FieldBgmType, AudioClip> GetBgm()
    {
        return new Tuple<SoundManager.FieldBgmType, AudioClip>(SoundManager.FieldBgmType.Clip, bgmClip);
    }

    /// <summary>
    /// 汎用座標を取得
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
    /// プレイヤー位置設定・つくよみちゃんも追従
    /// </summary>
    /// <param name="id">GeneralPosition</param>
    public void InitPlayerPos(int id)
    {
        var pos = SearchGeneralPosition(id);
        if (pos == null) return;

        SetPlayerPos(pos);
    }

    /// <summary>
    /// イベント開始
    /// </summary>
    /// <param name="ev"></param>
    public void ActivateEvent(EventBase ev)
    {
        activeEvent.Add(ev);
    }

    /// <summary>
    /// イベント終了
    /// </summary>
    /// <param name="ev"></param>
    public void EndEvent(EventBase ev)
    {
        activeEvent.Remove(ev);
    }

    /// <summary>
    /// イベント実行中か否か
    /// </summary>
    /// <returns></returns>
    public bool IsEventPlaying()
    {
        return activeEvent.Count > 0;
    }

    #endregion

    #region プロテクトメソッド

    /// <summary>
    /// プレイヤー位置設定・つくよみちゃんも追従
    /// </summary>
    /// <param name="gp"></param>
    protected void SetPlayerPos(GeneralPosition gp)
    {
        // プレイヤー
        if (playerObj != null)
        {
            playerObj.transform.position = gp.GetPosition();

            var cam = ManagerSceneScript.GetInstance().mainCam;
            cam.SetTargetPos(playerObj);
            cam.Immediate();
        }

        // つくよみちゃん
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
