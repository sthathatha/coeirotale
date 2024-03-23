using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScriptBase : MonoBehaviour
{
    #region メンバー
    /// <summary>スリープ時にActive=falseする親オブジェクト</summary>
    public GameObject objectParent = null;

    /// <summary>BGM</summary>
    public AudioClip bgmClip = null;
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

    #region 既定
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

        // BGM切り替え
        var bgmSetting = GetBgm();
        yield return ManagerSceneScript.GetInstance().soundManager.PlayFieldBgm(bgmSetting.Item1, bgmSetting.Item2);

        ManagerSceneScript.GetInstance().SetMainScript(this);

        FieldState = State.Idle;
    }

    virtual protected void Update()
    {
    }
    #endregion

    /// <summary>
    /// シーン名
    /// </summary>
    /// <returns></returns>
    virtual public string GetSceneName() { return "SampleScene"; }

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
    }

    /// <summary>
    /// フェードイン直前にやること
    /// </summary>
    /// <returns></returns>
    virtual public IEnumerator BeforeFadeIn()
    {
        yield break;
    }

    /// <summary>
    /// フェードイン終わったらやること
    /// </summary>
    /// <returns></returns>
    virtual public IEnumerator AfterFadeIn()
    {
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
}
