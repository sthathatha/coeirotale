using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

/// <summary>
/// �܂��肷���⋩�Q�[��
/// </summary>
public class MatukaGameSystemB : GameSceneScriptBase
{
    #region �萔

    /// <summary>����������</summary>
    private const int PRESS_LIMIT = 25;

    /// <summary>�G�̘A�ő��x</summary>
    private const float ENEMY_PRESS_INTERVAL = 0.16f;

    /// <summary>�J�n���E�����ꂽ�畉���̍��W</summary>
    private const float X_LIMIT = 230f;
    /// <summary>���[�U�[�I�u�W�F�N�g�̍��{</summary>
    private const float LASER_BASE = 376f;

    /// <summary>�������o�̃w�b�h�ʒu</summary>
    private const float X_EXIT = 1000f;
    /// <summary>�������o�̃w�b�h�ړ�����</summary>
    private const float WIN_HEAD_TIME = 0.4f;
    /// <summary>�������o�̑��x</summary>
    private const float WIN_SPEED =(X_EXIT - X_LIMIT) / WIN_HEAD_TIME;
    /// <summary>�������o�̍��{�ړ�����</summary>
    private const float WIN_BODY_TIME = (X_EXIT + LASER_BASE) / WIN_SPEED;

    #endregion

    #region �����o�[

    /// <summary>�܂��肷��A</summary>
    public MatukaGameCharacter matukaPlayer;
    /// <summary>�ɂ��܂��肷��</summary>
    public MatukaGameCharacter matukaEnemy;
    /// <summary>�e�L�X�g</summary>
    public TMP_Text messageText;

    public MatukaGameBLaser laserP;
    public MatukaGameBLaser laserE;

    /// <summary>���у{�C�X</summary>
    public AudioClip shoutVoice1;
    /// <summary>���у{�C�X</summary>
    public AudioClip shoutVoice2;
    /// <summary>���у{�C�X</summary>
    public AudioClip shoutVoice3;
    /// <summary>���у{�C�X</summary>
    public AudioClip shoutVoice4;

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
        laserP.Show(false);
        laserE.Show(false);

        matukaPlayer.ShowObject(true, false, false, false);
        matukaEnemy.ShowObject(true, false, false, false);
        messageText.SetText("");

        yield return base.Start();
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
        var shoutCoroutine = ShoutCoroutine();

        // �܂��V���E�g
        matukaPlayer.ShowObject(false, true, true, false);
        matukaEnemy.ShowObject(false, true, true, false);
        sound.PlaySE(shoutVoice1);
        cam.PlayShake(Shaker.ShakeSize.Weak);
        // ���[�U�[�\��
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
        // �`���[�g���A���\��
        tutorial.SetTitle(StringMinigameMessage.MatukaA_Title);
        tutorial.SetText(StringMinigameMessage.MatukaA_Tutorial);
        yield return tutorial.Open();
        yield return new WaitUntil(() => input.GetKeyPress(InputManager.Keys.South));
        yield return tutorial.Close();

        StartCoroutine(shoutCoroutine);
        yield return new WaitForSeconds(2f);
        // ����J�n
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

        // ���s
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

    #region �v���C�x�[�g

    /// <summary>
    /// ���[�U�[�\���X�V
    /// </summary>
    private void UpdateLaser()
    {
        // �ʒu�v�Z
        var x = -pressCount * X_LIMIT / PRESS_LIMIT;
        laserP.SetPos(x);
        laserE.SetPos(x);
    }

    /// <summary>
    /// ���эĐ��R���[�`��
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
    /// �������o
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
    /// �s�k���o
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
