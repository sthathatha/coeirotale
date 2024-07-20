using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Windows;

/// <summary>
/// �R�̃}�` �{�X���b�V���p�Ǘ�
/// </summary>
public class IkusautaGameSystemB : GameSceneScriptBase
{
    #region �萔

    /// <summary></summary>
    private const float COMMAND_SIZE = 80f;

    /// <summary>�ҋ@����</summary>
    private const float TIME_MAX = 0.55f;
    /// <summary>�ǉ�����</summary>
    private const float TIME_ADD = 0.18f;

    /// <summary>���������E������</summary>
    private const int WIN_SUCCESS_CNT = 7;
    /// <summary>�s�k�����E���s��</summary>
    private const int LOSE_FAIL_CNT = 3;

    /// <summary>�a�����㌳�̈ʒu�̖߂�W�����v����</summary>
    private const float RESULT_JUMP_TIME = 0.5f;

    #region enum

    /// <summary>�R�}���h����</summary>
    public enum ArrowDir
    {
        Up = 0,
        Down,
        Right,
        Left,
        Button,
    }

    /// <summary>�\���L�����N�^�[</summary>
    private enum CharacterMode
    {
        Waiting = 0,
        Up,
        Down,
        Right,
        Left,
        Result,
    }

    /// <summary>�O�Օ\���V�`���G�[�V����</summary>
    private enum SlashSituation : int
    {
        /// <summary>��</summary>
        Up = 0,
        /// <summary>��</summary>
        Down,
        /// <summary>�E</summary>
        Right,
        /// <summary>��</summary>
        Left,
        /// <summary>������</summary>
        Win,
        /// <summary>���s��</summary>
        Lose,
        /// <summary>��������߂鎞</summary>
        BackWin,
        /// <summary>���s����߂鎞</summary>
        BackLose,
    }

    #endregion

    #endregion

    #region �����o�[

    public GameObject parent_wait;
    public GameObject parent_up;
    public GameObject parent_down;
    public GameObject parent_left;
    public GameObject parent_right;
    public GameObject parent_result;
    public IkusautaGameBResultCharacter matiA;
    public IkusautaGameBResultCharacter matiB;
    public Transform matiA_up;
    public Transform matiB_up;
    public Transform matiA_down;
    public Transform matiB_down;
    public Transform matiA_right;
    public Transform matiB_right;
    public Transform matiA_left;
    public Transform matiB_left;

    public Transform command_bg;
    public Transform time_gauge;
    public Transform arrow_parent;
    public IkusautaGameBArrow arrow_dummy;
    public Transform slash_parent;
    public IkusautaGameBSlash slash_dummy;

    /// <summary>�R�}���h�\��SE</summary>
    public AudioClip se_bikkuri;
    /// <summary>�ł�����SE</summary>
    public AudioClip se_utiai;
    /// <summary>�U������SE</summary>
    public AudioClip se_attack;
    /// <summary>�f�U��SE</summary>
    public AudioClip se_suburi;
    /// <summary>�������܂�SE</summary>
    public AudioClip se_stack;

    #endregion

    #region �ϐ�

    /// <summary>���͑҂��C���f�b�N�X</summary>
    private int waitIndex;
    /// <summary>��󃊃X�g</summary>
    private List<IkusautaGameBArrow> arrowList;

    /// <summary>������</summary>
    private int successCount;
    /// <summary>���s��</summary>
    private int failCount;

    #endregion

    #region ���

    /// <summary>
    /// �J�n��
    /// </summary>
    /// <returns></returns>
    public override IEnumerator Start()
    {
        yield return base.Start();

        arrowList = new List<IkusautaGameBArrow>();
        ShowCharacter(CharacterMode.Waiting);
        DispTimeGauge(-1);
        arrow_dummy.gameObject.SetActive(false);
        slash_dummy.gameObject.SetActive(false);
        command_bg.gameObject.SetActive(false);
    }

    /// <summary>
    /// �t�F�[�h�C����
    /// </summary>
    /// <returns></returns>
    public override IEnumerator AfterFadeIn()
    {
        var tutorial = ManagerSceneScript.GetInstance().GetMinigameTutorialWindow();
        var input = InputManager.GetInstance();

        yield return base.AfterFadeIn();

        yield return new WaitForSeconds(1f);
        // �`���[�g���A���\��
        tutorial.SetTitle(StringMinigameMessage.MatiB_Title);
        tutorial.SetText(StringMinigameMessage.MatiB_Tutorial);
        yield return tutorial.Open();
        yield return new WaitUntil(() => input.GetKeyPress(InputManager.Keys.South));
        yield return tutorial.Close();

        // �J�n
        StartCoroutine(GameCoroutine());
    }

    #endregion

    #region �i�s�R���[�`��

    /// <summary>
    /// �Q�[���J�n�R���[�`��
    /// </summary>
    /// <returns></returns>
    private IEnumerator GameCoroutine()
    {
        ShowCharacter(CharacterMode.Waiting);
        var sound = ManagerSceneScript.GetInstance().soundMan;
        var input = InputManager.GetInstance();

        sound.PlaySE(se_stack);
        command_bg.gameObject.SetActive(true);
        var bgWidth = (CalcCommandCount() + 1.1f) * COMMAND_SIZE;
        command_bg.localScale = new Vector3(bgWidth, COMMAND_SIZE * 1.1f, 1f);

        var time_limit = TIME_MAX;
        DispTimeGauge(time_limit);

        yield return new WaitForSeconds(Util.RandomFloat(2f, 4f));

        // �쐬���ĕ\��
        sound.PlaySE(se_bikkuri);
        CreateArrowList();

        // ���Ԑ����J�n

        // ���͑҂�
        while (true)
        {
            yield return null;

            // ����
            var up = input.GetKeyPress(InputManager.Keys.Up);
            var down = input.GetKeyPress(InputManager.Keys.Down);
            var right = input.GetKeyPress(InputManager.Keys.Right);
            var left = input.GetKeyPress(InputManager.Keys.Left);
            var button = input.GetKeyPress(InputManager.Keys.South);

            if (up || down || right || left || button)
            {
                // ���͑҂��̂��
                var waitDir = arrowList[waitIndex].GetDirection();

                if (up && waitDir == ArrowDir.Up ||
                    down && waitDir == ArrowDir.Down ||
                    right && waitDir == ArrowDir.Right ||
                    left && waitDir == ArrowDir.Left ||
                    button && waitDir == ArrowDir.Button)
                {
                    // ���͐���
                    time_limit += TIME_ADD;
                    if (time_limit > TIME_MAX) time_limit = TIME_MAX;
                    DispTimeGauge(time_limit);

                    if (waitIndex >= arrowList.Count - 1)
                    {
                        // ���X�g�Ȃ琬���R���[�`����
                        StartCoroutine(ResultCoroutine(true));
                        yield break;
                    }

                    // �ł�����
                    sound.PlaySE(se_utiai);

                    // ���1����
                    arrowList[waitIndex].gameObject.SetActive(false);
                    waitIndex++;

                    // �L�����\��
                    ShowCharacter(waitDir switch
                    {
                        ArrowDir.Up => CharacterMode.Up,
                        ArrowDir.Down => CharacterMode.Down,
                        ArrowDir.Right => CharacterMode.Right,
                        _ => CharacterMode.Left,
                    });

                    // �O��
                    CreateSlashToObj(waitDir switch
                    {
                        ArrowDir.Up => SlashSituation.Up,
                        ArrowDir.Down => SlashSituation.Down,
                        ArrowDir.Right => SlashSituation.Right,
                        _ => SlashSituation.Left,
                    });
                }
                else
                {
                    // ���s
                    StartCoroutine(ResultCoroutine(false));
                    yield break;
                }
            }
            else
            {
                // �����ĂȂ��Ǝ��Ԍ���
                time_limit -= Time.deltaTime;
                if (time_limit <= 0f)
                {
                    // ���Ԑ؂�Ŏ��s
                    StartCoroutine(ResultCoroutine(false));
                    yield break;
                }
                else
                {
                    // ����
                    DispTimeGauge(time_limit);
                }
            }
        }
    }

    /// <summary>
    /// ���͐����R���[�`��
    /// </summary>
    /// <returns></returns>
    private IEnumerator ResultCoroutine(bool isSuccess)
    {
        // �Еt��
        DispTimeGauge(-1);
        ReleaseArrows();

        // �U��SE
        var sound = ManagerSceneScript.GetInstance().soundMan;
        sound.PlaySE(se_attack);

        // �\��
        ShowCharacter(CharacterMode.Result, isSuccess);
        CreateSlashToObj(isSuccess ? SlashSituation.Win : SlashSituation.Lose);

        yield return new WaitForSeconds(1.5f);

        // ���ʔ���
        if (isSuccess)
        {
            successCount++;
            if (successCount >= WIN_SUCCESS_CNT)
            {
                yield return new WaitForSeconds(1f);
                // �����ŏI��
                Global.GetTemporaryData().bossRushMatiWon = true;
                ExitGame();
                yield break;
            }
        }
        else
        {
            failCount++;
            if (failCount >= LOSE_FAIL_CNT)
            {
                yield return new WaitForSeconds(1f);
                // �����ŏI��
                Global.GetTemporaryData().bossRushMatiWon = false;
                ExitGame();
                yield break;
            }
        }

        // ���̈ʒu�ɖ߂��ăQ�[���ĊJ
        StartCoroutine(matiA.BackToBase(RESULT_JUMP_TIME));
        StartCoroutine(matiB.BackToBase(RESULT_JUMP_TIME));
        // �������ق��ɋO�Օ\��
        yield return new WaitForSeconds(RESULT_JUMP_TIME * 0.29f);
        sound.PlaySE(se_suburi);
        var slashRot = Util.RandomFloat(0, 2f);
        for (var i = 0; i < 3; ++i)
        {
            CreateSlashToObj(isSuccess ? SlashSituation.BackWin : SlashSituation.BackLose, Mathf.PI * slashRot);
            yield return new WaitForSeconds(RESULT_JUMP_TIME * 0.07f);

            slashRot += 0.67f;
            if (slashRot >= 2f) slashRot -= 2f;
        }
        yield return new WaitForSeconds(RESULT_JUMP_TIME * 0.5f);

        StartCoroutine(GameCoroutine());
    }

    #endregion

    #region �@�\���\�b�h

    /// <summary>
    /// �L�����\��
    /// </summary>
    /// <param name="mode"></param>
    /// <param name="isSuccess"></param>
    private void ShowCharacter(CharacterMode mode, bool isSuccess = true)
    {
        parent_wait.SetActive(mode == CharacterMode.Waiting);
        parent_up.SetActive(mode == CharacterMode.Up);
        parent_down.SetActive(mode == CharacterMode.Down);
        parent_right.SetActive(mode == CharacterMode.Right);
        parent_left.SetActive(mode == CharacterMode.Left);
        parent_result.SetActive(mode == CharacterMode.Result);

        if (mode == CharacterMode.Result)
        {
            matiA.SetResultDisp(isSuccess);
            matiB.SetResultDisp(!isSuccess);
        }
    }

    private int CalcCommandCount()
    {
        return 3 + successCount; //�쐬���@���ۂ͍Ō�Ƀ{�^��������̂Ł{�P
    }

    /// <summary>
    /// �R�}���h�쐬���ĕ\��
    /// </summary>
    private void CreateArrowList()
    {
        var createCnt = CalcCommandCount();
        var createX = -0.5f * (createCnt) * COMMAND_SIZE; // 1�ڂ̕\���ꏊ

        var arrowDir = Util.RandomInt((int)ArrowDir.Up, (int)ArrowDir.Left);
        for (var i = 0; i < createCnt; ++i)
        {
            var arrow = CreateArrow(arrowDir, createX);
            arrowList.Add(arrow);

            createX += COMMAND_SIZE;
            // ���̃R�}���h�@�����̂�A���ł͏o���Ȃ�
            var next = Util.RandomInt((int)ArrowDir.Up, (int)ArrowDir.Left - 1);
            arrowDir = next >= arrowDir ? next + 1 : next;
        }

        // �Ō�Ɍ���{�^��
        var button = CreateArrow((int)ArrowDir.Button, createX);
        arrowList.Add(button);

        waitIndex = 0;
    }

    /// <summary>
    /// ���쐬
    /// </summary>
    /// <param name="arrowDir"></param>
    /// <param name="x"></param>
    /// <returns></returns>
    private IkusautaGameBArrow CreateArrow(int arrowDir, float x)
    {
        var inst = Instantiate(arrow_dummy);
        inst.transform.SetParent(arrow_parent);
        inst.transform.localPosition = new Vector3(x, 0, 0);
        inst.SetDirection((ArrowDir)arrowDir);

        inst.gameObject.SetActive(true);
        return inst;
    }

    /// <summary>
    /// �����폜
    /// </summary>
    private void ReleaseArrows()
    {
        foreach (var a in arrowList)
        {
            Destroy(a.gameObject);
        }

        arrowList.Clear();
        command_bg.gameObject.SetActive(false);
    }

    /// <summary>
    /// �Q�[�W�\��
    /// </summary>
    /// <param name="now"></param>
    /// <param name="max"></param>
    private void DispTimeGauge(float now, float max = TIME_MAX)
    {
        if (now <= 0f)
        {
            time_gauge.gameObject.SetActive(false);
            return;
        }

        time_gauge.gameObject.SetActive(true);

        var width = Constant.SCREEN_WIDTH * now / max;
        var height = time_gauge.localScale.y;
        time_gauge.localScale = new Vector3(width, height, 1);
    }

    /// <summary>
    /// �O�Օ\��
    /// </summary>
    /// <param name="sit"></param>
    /// <param name="rot"></param>
    private void CreateSlashToObj(SlashSituation sit, float rot = 0f)
    {
        var colorA = Color.white;
        var colorB = new Color(0.8f, 0.4f, 0.4f);

        switch (sit)
        {
            case SlashSituation.Up:
                CreateSlash(matiA_up.position, CalcRandomSlash(3), Mathf.PI * Util.RandomFloat(0.25f, 1.5f), colorA);
                CreateSlash(matiB_up.position, CalcRandomSlash(3), Mathf.PI * Util.RandomFloat(-0.5f, 0f), colorB);
                break;
            case SlashSituation.Down:
                CreateSlash(matiA_down.position, CalcRandomSlash(1), Mathf.PI * Util.RandomFloat(1f, 1.5f), colorA);
                CreateSlash(matiB_down.position, CalcRandomSlash(1), Mathf.PI * Util.RandomFloat(-0.5f, 0f), colorB);
                break;
            case SlashSituation.Right:
                CreateSlash(matiA_right.position, IkusautaGameBSlash.Type.Curve, Mathf.PI * Util.RandomFloat(0.25f, 1.75f), colorA);
                CreateSlash(matiB_right.position, IkusautaGameBSlash.Type.Line, 0f, colorB);
                break;
            case SlashSituation.Left:
                CreateSlash(matiA_left.position, CalcRandomSlash(3), Mathf.PI * Util.RandomFloat(0.7f, 1.3f), colorA);
                CreateSlash(matiB_left.position, IkusautaGameBSlash.Type.Curve, Mathf.PI * Util.RandomFloat(-0.3f, 0.5f), colorB);
                break;
            case SlashSituation.Win:
                CreateSlash(matiB.transform.position, IkusautaGameBSlash.Type.Line, Mathf.PI * 0.9f, colorA);
                break;
            case SlashSituation.Lose:
                CreateSlash(matiA.transform.position, IkusautaGameBSlash.Type.Curve, Mathf.PI * -0.2f, colorB);
                break;
            case SlashSituation.BackWin:
                CreateSlash(matiA.transform.position, IkusautaGameBSlash.Type.Curve, rot, colorA);
                break;
            case SlashSituation.BackLose:
                CreateSlash(matiB.transform.position, IkusautaGameBSlash.Type.Curve, -rot, colorB);
                break;
        }
    }

    /// <summary>
    /// ���O�Օ\��
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="type"></param>
    /// <param name="dir">����</param>
    /// <param name="color">�F</param>
    private void CreateSlash(Vector3 pos, IkusautaGameBSlash.Type type, float dir, Color? color = null)
    {
        var inst = Instantiate(slash_dummy);
        inst.transform.SetParent(slash_parent);
        inst.transform.localPosition = pos;
        inst.Show(type, dir, color);
    }

    /// <summary>
    /// �����_���ŋO�Ճ^�C�v�����߂�
    /// </summary>
    /// <param name="curve_rate">�Ȑ��̂ق��̏o�₷���@1:���{�@3:�R�΂P�ŋȐ�</param>
    /// <returns></returns>
    private IkusautaGameBSlash.Type CalcRandomSlash(int curve_rate)
    {
        return Util.RandomInt(0, curve_rate) == 0 ? IkusautaGameBSlash.Type.Line : IkusautaGameBSlash.Type.Curve;
    }

    /// <summary>
    /// �I������
    /// </summary>
    private void ExitGame()
    {
        //ManagerSceneScript.GetInstance().ExitGame();
        ManagerSceneScript.GetInstance().NextGame("GameScenePierreB");
    }

    #endregion
}
