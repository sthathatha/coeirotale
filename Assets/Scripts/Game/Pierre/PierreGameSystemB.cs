using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

/// <summary>
/// �{�X���b�V���s�G�[��
/// </summary>
public class PierreGameSystemB : GameSceneScriptBase
{
    #region �萔

    /// <summary>���I�u�W�F�N�g�����ʒu</summary>
    private const float OBJ_INIT_X = (Constant.SCREEN_WIDTH + PierreGameBGObject.OBJECT_WIDTH_MAX) / (-2f);

    /// <summary>��[�̍��W</summary>
    public const float FIELD_MAX_Y = 210f;
    /// <summary>���[�̍��W</summary>
    public const float FIELD_MIN_Y = -365f;
    /// <summary>�t�B�[���h����</summary>
    public const float FIELD_CENTER_Y = (FIELD_MAX_Y + FIELD_MIN_Y) / 2f;

    /// <summary>HP�ő�̃}�X�N�ʒu</summary>
    private const float GAUGE_MAX_X = 1000f;
    /// <summary>HP0�̃}�X�N�ʒu</summary>
    private const float GAUGE_MIN_X = 0f;

    /// <summary>�Q�[���i�s</summary>
    public enum GameState : int
    {
        LOADING = 0,
        PLAY,
        ENDING,
    }

    #endregion

    #region �����o�[

    /// <summary>�I�u�W�F�N�g�e</summary>
    public GameObject objectParent = null;
    /// <summary>�{�[���ǉ��p�e</summary>
    public Transform ballParent = null;
    /// <summary>�n�ʃe���v��</summary>
    public GameObject ground_dummy = null;
    /// <summary>�{�[��0�̃e���v���[�g</summary>
    public PierreGameBallB ball_dummy = null;

    /// <summary>�s�G�[��A</summary>
    public PierreGameBPlayer pierreA = null;
    /// <summary>�s�G�[��B</summary>
    public PierreGameBEnemy pierreB = null;

    /// <summary>�{�XHP�Q�[�W�̃}�X�N</summary>
    public Transform bossHPGauge;

    /// <summary>�Ȗ�</summary>
    public PierreGameBFadeUI musicText;
    /// <summary>�J�[�h��</summary>
    public PierreGameBFadeUI cardNameText;

    /// <summary>�X�y��������SE</summary>
    public AudioClip se_phaseChange;
    /// <summary>�G���S����SE</summary>
    public AudioClip se_enemy_death;

    #endregion

    #region �v���p�e�B

    /// <summary>�Q�[�����</summary>
    public GameState State { get; private set; } = GameState.LOADING;

    #endregion

    #region �ϐ�

    /// <summary>�ŐV�̒n��</summary>
    private GameObject newBG = null;

    #endregion

    #region ���

    /// <summary>
    /// ������
    /// </summary>
    /// <returns></returns>
    override public IEnumerator Start()
    {
        DisplayHP(1);
        GenerateInitObjects();
        StartCoroutine(GenerateGroundCoroutine());
        yield return musicText.Hide();
        yield return cardNameText.Hide();

        yield return base.Start();
    }

    /// <summary>
    /// �t�F�[�h�C����Q�[���J�n
    /// </summary>
    /// <returns></returns>
    public override IEnumerator AfterFadeIn()
    {
        var tutorial = ManagerSceneScript.GetInstance().GetMinigameTutorialWindow();
        var input = InputManager.GetInstance();

        yield return base.AfterFadeIn();

        yield return musicText.Show(StringMinigameMessage.PierreB_Music, 0.5f);
        tutorial.SetTitle(StringMinigameMessage.PierreB_Title);
        tutorial.SetText(StringMinigameMessage.PierreB_Tutorial);
        yield return tutorial.Open();
        yield return new WaitUntil(() => input.GetKeyPress(InputManager.Keys.South));
        StartCoroutine(musicText.Hide(0.5f));
        yield return tutorial.Close();

        State = GameState.PLAY;
    }

    #endregion

    #region �@�\�Ăяo��

    /// <summary>
    /// �{�[������
    /// </summary>
    /// <param name="startPos">�����ʒu</param>
    /// <param name="direction">���x</param>
    /// <param name="type">�G����</param>
    /// <param name="color">�F</param>
    /// <param name="power">�U����</param>
    public void GenerateBall(Vector3 startPos, Vector3 direction, PierreGameBallB.AttackType type, Color color, int power = 1)
    {
        var ball = Instantiate(ball_dummy);
        ball.transform.SetParent(ballParent, false);
        ball.SetParam(type, startPos, direction, color, power);
        ball.gameObject.SetActive(true);
    }

    /// <summary>
    /// �{�[���S����
    /// </summary>
    public void DeleteAllBall(PierreGameBallB.AttackType deleteType)
    {
        var balls = ballParent.GetComponentsInChildren<PierreGameBallB>();
        foreach (var ball in balls)
        {
            if (ball.attacktype == deleteType)
            {
                ball.DestroyWait();
            }
        }
    }

    /// <summary>
    /// HP�Q�[�W�\��
    /// </summary>
    /// <param name="rate"></param>
    public void DisplayHP(float rate)
    {
        var x = Util.CalcBetweenFloat(rate, GAUGE_MIN_X, GAUGE_MAX_X);
        bossHPGauge.localPosition = new Vector3(x, 0, 0);
    }

    /// <summary>
    /// �t�F�[�Y���b�Z�[�W�\��
    /// </summary>
    /// <param name="phase"></param>
    public void ShowPhaseMessage(int phase)
    {
        StartCoroutine(ShowPhaseCoroutine(phase));
    }

    /// <summary>
    /// �t�F�[�Y���b�Z�[�W�\��
    /// </summary>
    /// <param name="phase"></param>
    /// <returns></returns>
    private IEnumerator ShowPhaseCoroutine(int phase)
    {
        var sound = ManagerSceneScript.GetInstance().soundMan;
        sound.PlaySE(se_phaseChange);

        if (phase == 1)
        {
            yield return cardNameText.Show(StringMinigameMessage.PierreB_Spell1, 0.5f);
        }
        else
        {
            yield return cardNameText.Show(StringMinigameMessage.PierreB_Spell2, 0.5f);
        }
        yield return new WaitForSeconds(2f);
        yield return cardNameText.Hide(0.5f);
    }

    #endregion

    #region �Q�[�����ꏈ��

    /// <summary>
    /// �I������
    /// </summary>
    /// <param name="isWin"></param>
    public void EndGame(bool isWin)
    {
        if (State != GameState.PLAY) return;
        State = GameState.ENDING;

        Global.GetTemporaryData().bossRushPierreWon = isWin;

        StartCoroutine(EndGameCoroutine(isWin));
    }

    /// <summary>
    /// �I�������R���[�`��
    /// </summary>
    /// <param name="isWin"></param>
    /// <returns></returns>
    private IEnumerator EndGameCoroutine(bool isWin)
    {
        var sound = ManagerSceneScript.GetInstance().soundMan;
        if (isWin)
        {
            for (var i = 0; i < 10; ++i)
            {
                sound.PlaySE(se_enemy_death);
                yield return new WaitForSeconds(0.25f);
            }
        }

        yield return new WaitForSeconds(2f);
        ManagerSceneScript.GetInstance().ExitGame();
        //ManagerSceneScript.GetInstance().NextGame("GameSceneBossB");
    }

    #endregion

    #region �w�i�I�u�W�F�N�g�܂��

    /// <summary>
    /// �����I�u�W�F�N�g�쐬
    /// </summary>
    private void GenerateInitObjects()
    {
        // BG�@�E�[�������΂��������
        newBG = Instantiate(ground_dummy);
        newBG.transform.SetParent(objectParent.transform);
        newBG.transform.localPosition = new Vector3(0, 0);
    }

    /// <summary>
    /// �n�ʂ���ɐ���
    /// </summary>
    /// <returns></returns>
    private IEnumerator GenerateGroundCoroutine()
    {
        while (true)
        {
            yield return new WaitUntil(() => newBG.transform.localPosition.x > 0f);

            var bg = Instantiate(ground_dummy);
            bg.transform.SetParent(objectParent.transform, false);
            bg.transform.localPosition = new Vector3(newBG.transform.localPosition.x - Constant.SCREEN_WIDTH, 0);

            newBG = bg;
        }
    }
    #endregion
}
