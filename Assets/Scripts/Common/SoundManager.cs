using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region �萔
    /// <summary>�t�B�[���hBGM�^�C�v</summary>
    public enum FieldBgmType : int
    {
        None = 0,
        Common1,
        Common2,
        Common3,
        Clip,
    }

    /// <summary>BGM�t�F�[�h����</summary>
    private const float BGM_FADE_TIME = 0.5f;
    #endregion

    #region �����o�[
    /// <summary>SE�Đ������p</summary>
    public GameObject seDummy = null;
    /// <summary>�t�B�[���hBGM�Đ��p</summary>
    public AudioSource fieldBgmSource = null;
    /// <summary>�Q�[��BGM�Đ��p</summary>
    public AudioSource gameBgmSource = null;
    /// <summary>�{�C�X�Đ��p</summary>
    public AudioSource voiceSource = null;

    /// <summary>�t�B�[���h����BGM1</summary>
    public AudioClip commonBgm1 = null;
    /// <summary>�t�B�[���h����BGM2</summary>
    public AudioClip commonBgm2 = null;
    /// <summary>�t�B�[���h����BGM3</summary>
    public AudioClip commonBgm3 = null;

    /// <summary>�ėp�I��SE</summary>
    public AudioClip commonSeSelect = null;
    /// <summary>�ėp�J�[�\���ړ�SE</summary>
    public AudioClip commonSeMove = null;
    /// <summary>�ėp�E�B���h�E�J��SE</summary>
    public AudioClip commonSeWindowOpen = null;
    #endregion

    #region �v���C�x�[�g�ϐ�
    /// <summary>�Đ����̃t�B�[���hBGM</summary>
    private FieldBgmType playingFieldBgm;
    #endregion

    #region ���菈��
    /// <summary>
    /// ������
    /// </summary>
    void Start()
    {
        playingFieldBgm = FieldBgmType.None;
    }
    #endregion

    #region SE�Ǘ�
    /// <summary>
    /// SE�{�����[���ݒ�
    /// </summary>
    /// <param name="v"></param>
    public void UpdateSeVolume()
    {
        seDummy.GetComponent<AudioSource>().volume = CalcSeVolume();
    }

    /// <summary>
    /// SE�Đ�
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
    /// SE�Đ��I����҂��č폜
    /// </summary>
    /// <param name="se"></param>
    /// <returns></returns>
    private IEnumerator DestroyWaitCoroutine(AudioSource se)
    {
        yield return new WaitWhile(() => se.isPlaying);
        Destroy(se.gameObject);
    }
    #endregion

    #region BGM�Ǘ�
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
    /// �t�B�[���h�ǂݍ��ݎ���BGM�Đ�
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
    /// �Q�[���J�n����BGM�J�n
    /// </summary>
    /// <param name="source"></param>
    public void StartGameBgm(AudioClip source = null)
    {
        StartCoroutine(FadeOutGamePlayCoroutine(source));
    }

    /// <summary>
    /// �t�B�[���hBGM���t�F�[�h�A�E�g���ăQ�[��BGM�J�n
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
    /// �Q�[���I�����̃t�B�[���hBGM���A
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
    /// �Q�[���؂�ւ�����BGM�ύX
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

    #region �{�C�X�Ǘ�
    /// <summary>
    /// �{�C�X�{�����[���ݒ�
    /// </summary>
    public void UpdateVoiceVolume()
    {
        voiceSource.volume = CalcVoiceVolume();
    }

    /// <summary>
    /// �{�C�X�Đ�
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
    /// �Đ����̃{�C�X���~
    /// </summary>
    public void StopVoice()
    {
        if (voiceSource.isPlaying) { voiceSource.Stop(); }
    }
    #endregion

    #region �v���C�x�[�g
    /// <summary>
    /// BGMSource�̍Đ��{�����[��
    /// </summary>
    /// <returns></returns>
    private float CalcBgmVolume()
    {
        var optionVol = Global.GetSaveData().option.bgmVolume;
        return 0.2f * optionVol / 10;
    }
    /// <summary>
    /// SESource�̍Đ��{�����[��
    /// </summary>
    /// <returns></returns>
    private float CalcSeVolume()
    {
        var optionVol = Global.GetSaveData().option.seVolume;
        return 1f * optionVol / 10;
    }
    /// <summary>
    /// VoiceSource�̍Đ��{�����[��
    /// </summary>
    /// <returns></returns>
    private float CalcVoiceVolume()
    {
        var optionVol = Global.GetSaveData().option.voiceVolume;
        return 1f * optionVol / 10;
    }
    #endregion
}
