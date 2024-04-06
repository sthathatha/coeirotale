using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// �܂��肷���⋩�Q�[��
/// </summary>
public class MatukaGameSystemA : GameSceneScriptBase
{
    #region �萔

    /// <summary>���ԃQ�[�W�}�X�N�ő厞</summary>
    private const float TIME_MAX_X = 0f;
    /// <summary>���ԃQ�[�W�}�X�N0��</summary>
    private const float TIME_ZERO_X = -300f;
    /// <summary>�A�Ŏ���</summary>
    private const float TIME_SEC = 5f;

    /// <summary>�|�C���g�ő厞</summary>
    private const float POINT_MAX_Y = 108f;
    /// <summary>�|�C���g0��</summary>
    private const float POINT_ZERO_Y = -118f;
    /// <summary>�|�C���g�M���M��������</summary>
    private const float POINT_SUCCESS_Y = -9f;

    /// <summary>�����|�C���g</summary>
    private const int POINT_SUCCESS_P = 40;
    /// <summary>�����{�C�X�|�C���g</summary>
    private const int POINT_STRONG_P = 55;
    /// <summary>�ő�̃|�C���g</summary>
    private int POINT_MAX_P = Mathf.FloorToInt(POINT_SUCCESS_P * (POINT_MAX_Y - POINT_ZERO_Y) / (POINT_SUCCESS_Y - POINT_ZERO_Y));

    #endregion

    #region �����o�[

    /// <summary>�܂��肷��A</summary>
    public MatukaGameCharacter matukaCharaA;
    /// <summary>����݂����</summary>
    public MatukaGameCharacter tukuyomiCharaA;
    /// <summary>�|�C���g�\���}�X�N</summary>
    public GameObject pointMask;
    /// <summary>�^�C���\���}�X�N</summary>
    public GameObject timeMask;
    /// <summary>�e�L�X�g</summary>
    public TMP_Text messageText;

    /// <summary>�܂��肷������</summary>
    public AudioClip matukaA_Shout;
    /// <summary>����݂�����</summary>
    public AudioClip tukuyomiA_Shout_Weak;
    /// <summary>����݂����</summary>
    public AudioClip tukuyomiA_Shout_Middle;
    /// <summary>����݂����</summary>
    public AudioClip tukuyomiA_Shout_Strong;


    #endregion

    #region �ϐ�

    /// <summary>�A�ŉ�</summary>
    private int pressCount = 0;

    #endregion

    #region ��ꏈ��
    /// <summary>
    /// ������
    /// </summary>
    /// <returns></returns>
    override public IEnumerator Start()
    {
        yield return base.Start();

        matukaCharaA.ShowObject(true, false, false);
        tukuyomiCharaA.ShowObject(true, false, false);
        SetTimeGauge(0f);
        SetPointGauge(0);
        messageText.SetText("");
    }

    /// <summary>
    /// �t�F�[�h�C����
    /// </summary>
    /// <returns></returns>
    public override IEnumerator AfterFadeIn()
    {
        yield return base.AfterFadeIn();

        StartCoroutine(GameCoroutine());
    }
    #endregion

    #region �R���[�`��

    /// <summary>
    /// ���C��
    /// </summary>
    /// <returns></returns>
    private IEnumerator GameCoroutine()
    {
        var sound = ManagerSceneScript.GetInstance().soundMan;
        var tutorial = ManagerSceneScript.GetInstance().GetMinigameTutorialWindow();
        var input = InputManager.GetInstance();
        var cam = ManagerSceneScript.GetInstance().mainCam;

        // �Z���t�\��
        messageText.SetText(StringMinigameMessage.MatukaA_Serif1);
        yield return new WaitForSeconds(1f);
        messageText.SetText("");
        yield return new WaitForSeconds(1f);
        // �܂��V���E�g
        matukaCharaA.ShowObject(false, true, true);
        sound.PlaySE(matukaA_Shout);
        cam.PlayShakeOne(Shaker.ShakeSize.Strong);
        yield return new WaitForSeconds(2f);
        matukaCharaA.ShowObject(true, false, false);
        messageText.SetText(StringMinigameMessage.MatukaA_Serif2);
        yield return new WaitForSeconds(1f);
        messageText.SetText("");
        // �`���[�g���A���\��
        tutorial.SetTitle(StringMinigameMessage.MatukaA_Title);
        tutorial.SetText(StringMinigameMessage.MatukaA_Tutorial);
        yield return tutorial.Open();
        yield return new WaitUntil(() => input.GetKeyPress(InputManager.Keys.South));
        yield return tutorial.Close();
        // ���ԊJ�n
        var timer = TIME_SEC;
        SetTimeGauge(timer);
        messageText.SetText(StringMinigameMessage.MatukaA_Naration1);
        yield return new WaitForSeconds(1f);
        messageText.SetText(StringMinigameMessage.MatukaA_Naration2);
        pressCount = 0;
        // �����n���f
        if (GetLoseCount() >= 3)
        {
            pressCount = (GetLoseCount() - 2) * 3;
        }
        while (timer > 0f)
        {
            if (input.GetKeyPress(InputManager.Keys.South)) { pressCount++; }

            yield return null;
            timer -= Time.deltaTime;
            SetTimeGauge(timer);
        }
        // �I���
        messageText.SetText(StringMinigameMessage.MatukaA_Naration3);
        yield return new WaitForSeconds(1f);
        messageText.SetText("");
        yield return new WaitForSeconds(1f);
        // ����
        tukuyomiCharaA.ShowObject(true, false, true);
        sound.PlaySE(pressCount switch
        {
            >= POINT_STRONG_P => tukuyomiA_Shout_Strong,
            >= POINT_SUCCESS_P => tukuyomiA_Shout_Middle,
            _ => tukuyomiA_Shout_Weak
        });
        cam.PlayShakeOne(Shaker.ShakeSize.Weak);
        yield return new WaitForSeconds(2f);
        tukuyomiCharaA.ShowObject(true, false, false);

        // �|�C���g�\��
        yield return ShowPointCoroutine();

        if (pressCount >= POINT_SUCCESS_P)
        {
            yield return SuccessCoroutine();
        }
        else
        {
            yield return FailCoroutine();
        }
    }

    /// <summary>
    /// ������
    /// </summary>
    /// <returns></returns>
    private IEnumerator SuccessCoroutine()
    {
        SetGameResult(true);
        messageText.SetText(StringMinigameMessage.MatukaA_Win);
        tukuyomiCharaA.ShowObject(false, true, false);
        matukaCharaA.ShowObject(false, true, false);
        yield return new WaitForSeconds(2f);
        ManagerSceneScript.GetInstance().ExitGame();
    }

    /// <summary>
    /// ���s��
    /// </summary>
    /// <returns></returns>
    private IEnumerator FailCoroutine()
    {
        SetGameResult(false);
        messageText.SetText(StringMinigameMessage.MatukaA_Lose);
        matukaCharaA.ShowObject(false, true, false);
        yield return new WaitForSeconds(2f);
        ManagerSceneScript.GetInstance().ExitGame();
    }

    /// <summary>
    /// �|�C���g�\��
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShowPointCoroutine()
    {
        var tmpPoint = new DeltaFloat();
        tmpPoint.Set(0);
        tmpPoint.MoveTo(pressCount, 1.5f, DeltaFloat.MoveType.DECEL);
        SetPointGauge(Mathf.FloorToInt(tmpPoint.Get()));

        while (tmpPoint.IsActive())
        {
            yield return null;
            tmpPoint.Update(Time.deltaTime);

            SetPointGauge(Mathf.FloorToInt(tmpPoint.Get()));
        }
    }

    #endregion

    #region �v���C�x�[�g

    /// <summary>
    /// �|�C���g�Q�[�W�\��
    /// </summary>
    /// <param name="_point"></param>
    private void SetPointGauge(int _point)
    {
        if (_point > POINT_MAX_P) _point = POINT_MAX_P;
        var rate = (float)_point / POINT_MAX_P;

        var y = Util.CalcBetweenFloat(rate, POINT_ZERO_Y, POINT_MAX_Y);
        pointMask.transform.localPosition = new Vector3(0, y, 0);
    }

    /// <summary>
    /// ���ԃQ�[�W�\��
    /// </summary>
    /// <param name="_nokori">�c�莞��</param>
    private void SetTimeGauge(float _nokori)
    {
        if (_nokori > TIME_SEC) _nokori = TIME_SEC;

        var rate = _nokori / TIME_SEC;

        var x = Util.CalcBetweenFloat(rate, TIME_ZERO_X, TIME_MAX_X);
        timeMask.transform.localPosition = new Vector3(x, 0, 0);
    }

    #endregion
}
