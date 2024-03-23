using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region 定数
    /// <summary>フィールドBGMタイプ</summary>
    public enum FieldBgmType : int
    {
        None = 0,
        Common1,
        Common2,
        Common3,
        Clip,
    }

    /// <summary>BGMフェード時間</summary>
    private const float BGM_FADE_TIME = 0.5f;
    #endregion

    #region メンバー
    /// <summary>SE再生複製用</summary>
    public GameObject seDummy = null;
    /// <summary>フィールドBGM再生用</summary>
    public AudioSource fieldBgmSource = null;
    /// <summary>ゲームBGM再生用</summary>
    public AudioSource gameBgmSource = null;
    /// <summary>ボイス再生用</summary>
    public AudioSource voiceSource = null;

    /// <summary>フィールド共通BGM1</summary>
    public AudioClip commonBgm1 = null;
    /// <summary>フィールド共通BGM2</summary>
    public AudioClip commonBgm2 = null;
    /// <summary>フィールド共通BGM3</summary>
    public AudioClip commonBgm3 = null;

    /// <summary>汎用選択SE</summary>
    public AudioClip commonSeSelect = null;
    /// <summary>汎用カーソル移動SE</summary>
    public AudioClip commonSeMove = null;
    /// <summary>汎用ウィンドウ開くSE</summary>
    public AudioClip commonSeWindowOpen = null;
    #endregion

    #region プライベート変数
    /// <summary>再生中のフィールドBGM</summary>
    private FieldBgmType playingFieldBgm;
    #endregion

    #region 既定処理
    /// <summary>
    /// 初期化
    /// </summary>
    void Start()
    {
        playingFieldBgm = FieldBgmType.None;
    }
    #endregion

    #region SE管理
    /// <summary>
    /// SEボリューム設定
    /// </summary>
    /// <param name="v"></param>
    public void UpdateSeVolume()
    {
        seDummy.GetComponent<AudioSource>().volume = CalcSeVolume();
    }

    /// <summary>
    /// SE再生
    /// </summary>
    /// <param name="se"></param>
    public void PlaySE(AudioClip se)
    {
        var seObj = GameObject.Instantiate(seDummy);
        seObj.transform.SetParent(this.transform);

        var seSource = seObj.GetComponent<AudioSource>();
        seSource.clip = se;
        seSource.Play();

        StartCoroutine(DestroyWaitCoroutine(seSource));
    }

    /// <summary>
    /// SE再生終了を待って削除
    /// </summary>
    /// <param name="se"></param>
    /// <returns></returns>
    private IEnumerator DestroyWaitCoroutine(AudioSource se)
    {
        yield return new WaitWhile(() => se.isPlaying);
        Destroy(se.gameObject);
    }
    #endregion

    #region BGM管理
    /// <summary>
    /// 
    /// </summary>
    public void UpdateBgmVolume()
    {
        var vol = CalcBgmVolume();
        fieldBgmSource.GetComponent<AudioSource>().volume = vol;
        gameBgmSource.GetComponent<AudioSource>().volume = vol;
    }

    /// <summary>
    /// フィールド読み込み時のBGM再生
    /// </summary>
    /// <param name="fieldBgmType"></param>
    /// <param name="source"></param>
    public IEnumerator PlayFieldBgm(FieldBgmType fieldBgmType, AudioClip source = null)
    {
        if (fieldBgmType == FieldBgmType.Clip || fieldBgmType != playingFieldBgm)
        {
            if (fieldBgmSource.isPlaying)
            {
                var vol = new DeltaFloat();
                vol.Set(fieldBgmSource.volume);
                vol.MoveTo(0, BGM_FADE_TIME, DeltaFloat.MoveType.LINE);
                while (vol.IsActive())
                {
                    vol.Update(Time.deltaTime);
                    fieldBgmSource.volume = vol.Get();
                    yield return null;
                }
                fieldBgmSource.Stop();
            }

            var clip = fieldBgmType switch
            {
                FieldBgmType.Common1 => commonBgm1,
                FieldBgmType.Common2 => commonBgm2,
                FieldBgmType.Common3 => commonBgm3,
                _ => source,
            };
            if (clip != null)
            {
                fieldBgmSource.volume = CalcBgmVolume();
                fieldBgmSource.clip = clip;
                fieldBgmSource.Play();
            }
        }

        playingFieldBgm = fieldBgmType;
    }

    /// <summary>
    /// ゲーム開始時のBGM開始
    /// </summary>
    /// <param name="source"></param>
    public void StartGameBgm(AudioClip source = null)
    {
        StartCoroutine(FadeOutGamePlayCoroutine(source));
    }

    /// <summary>
    /// フィールドBGMをフェードアウトしてゲームBGM開始
    /// </summary>
    /// <param name="clip"></param>
    /// <returns></returns>
    private IEnumerator FadeOutGamePlayCoroutine(AudioClip clip = null)
    {
        var vol = new DeltaFloat();
        vol.Set(fieldBgmSource.volume);
        vol.MoveTo(0, BGM_FADE_TIME, DeltaFloat.MoveType.LINE);
        while (vol.IsActive())
        {
            vol.Update(Time.deltaTime);
            fieldBgmSource.volume = vol.Get();
            yield return null;
        }
        fieldBgmSource.Pause();

        if (clip)
        {
            gameBgmSource.volume = CalcBgmVolume();
            gameBgmSource.clip = clip;
            gameBgmSource.Play();
        }
    }

    /// <summary>
    /// ゲーム終了時のフィールドBGM復帰
    /// </summary>
    /// <returns></returns>
    public IEnumerator ResumeBgmFromGame()
    {
        if (gameBgmSource.isPlaying)
        {
            var vol = new DeltaFloat();
            vol.Set(gameBgmSource.volume);
            vol.MoveTo(0, BGM_FADE_TIME, DeltaFloat.MoveType.LINE);
            while (vol.IsActive())
            {
                vol.Update(Time.deltaTime);
                gameBgmSource.volume = vol.Get();
                yield return null;
            }
            gameBgmSource.Stop();
        }

        fieldBgmSource.UnPause();
        var newVol = new DeltaFloat();
        newVol.Set(0);
        newVol.MoveTo(CalcBgmVolume(), BGM_FADE_TIME, DeltaFloat.MoveType.LINE);
        while (newVol.IsActive())
        {
            newVol.Update(Time.deltaTime);
            fieldBgmSource.volume = newVol.Get();
            yield return null;
        }
    }

    /// <summary>
    /// ゲーム切り替え時のBGM変更
    /// </summary>
    /// <param name="clip"></param>
    /// <returns></returns>
    public IEnumerator ChangeGameBgm(AudioClip clip)
    {
        if (gameBgmSource.isPlaying)
        {
            var vol = new DeltaFloat();
            vol.Set(gameBgmSource.volume);
            vol.MoveTo(0, BGM_FADE_TIME, DeltaFloat.MoveType.LINE);
            while (vol.IsActive())
            {
                vol.Update(Time.deltaTime);
                gameBgmSource.volume = vol.Get();
                yield return null;
            }
            gameBgmSource.Stop();
        }

        if (clip)
        {
            gameBgmSource.volume = CalcBgmVolume();
            gameBgmSource.clip = clip;
            gameBgmSource.Play();
        }
    }
    #endregion

    #region ボイス管理
    /// <summary>
    /// ボイスボリューム設定
    /// </summary>
    public void UpdateVoiceVolume()
    {
        voiceSource.volume = CalcVoiceVolume();
    }

    /// <summary>
    /// ボイス再生
    /// </summary>
    /// <param name="voice"></param>
    public void PlayVoice(AudioClip voice)
    {
        StopVoice();
        if (voice)
        {
            voiceSource.clip = voice;
            voiceSource.Play();
        }
    }

    /// <summary>
    /// 再生中のボイスを停止
    /// </summary>
    public void StopVoice()
    {
        if (voiceSource.isPlaying) { voiceSource.Stop(); }
    }
    #endregion

    #region プライベート
    /// <summary>
    /// BGMSourceの再生ボリューム
    /// </summary>
    /// <returns></returns>
    private float CalcBgmVolume()
    {
        var optionVol = Global.GetSaveData().option.bgmVolume;
        return 0.2f * optionVol / 10;
    }
    /// <summary>
    /// SESourceの再生ボリューム
    /// </summary>
    /// <returns></returns>
    private float CalcSeVolume()
    {
        var optionVol = Global.GetSaveData().option.seVolume;
        return 1f * optionVol / 10;
    }
    /// <summary>
    /// VoiceSourceの再生ボリューム
    /// </summary>
    /// <returns></returns>
    private float CalcVoiceVolume()
    {
        var optionVol = Global.GetSaveData().option.voiceVolume;
        return 1f * optionVol / 10;
    }
    #endregion
}
