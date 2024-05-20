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
    #endregion

    #region 基底
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
    /// ゲーム開始用にスリープ
    /// </summary>
    virtual public void Sleep()
    {
        objectParent?.SetActive(false);
    }

    /// <summary>
    /// ゲーム終了時に再開
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
    /// フェードイン直前にやること
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
    /// フェードイン終わったらやること
    /// </summary>
    /// <returns></returns>
    virtual public IEnumerator AfterFadeIn()
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
    /// プレイヤー位置設定・つくよみちゃんも追従
    /// </summary>
    /// <param name="gp"></param>
    protected void SetPlayerPos(GeneralPosition gp)
    {
        // プレイヤー
        if (playerObj != null)
        {
            playerObj.transform.position = gp.GetPosition();
        }

        // つくよみちゃん
        if (tukuyomiObj != null)
        {
            tukuyomiObj.GetComponent<TukuyomiScript>().InitTrace();
        }
    }
}
