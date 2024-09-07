using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class AmiGameSystemA : GameSceneScriptBase
{
    #region �萔

    /// <summary>GREAT�̖�󋗗�</summary>
    private const float GREAT_DIST = 30f;
    /// <summary>GOOD�̖�󋗗�</summary>
    private const float GOOD_DIST = 50f;
    /// <summary>BAD�̖�󋗗�</summary>
    private const float BAD_DIST = 70f;

    /// <summary>�Q�[�W�̍ő�ړ�</summary>
    private const float POINT_GAUGE_MAX = 200;

    #endregion

    #region �����o�[

    /// <summary>��</summary>
    public AudioClip playMusic;
    /// <summary></summary>
    public int bpm;
    /// <summary></summary>
    public float swingNotes;

    /// <summary>�G�L����</summary>
    public Animator enemy;
    /// <summary>�v���C���[</summary>
    public Animator player;

    /// <summary>��󐶐��e</summary>
    public GameObject playArrowParent;
    /// <summary>�E���_�~�[</summary>
    public GameObject dummyRight;
    /// <summary>����_�~�[</summary>
    public GameObject dummyUp;
    /// <summary>�����_�~�[</summary>
    public GameObject dummyDown;
    /// <summary>�����_�~�[</summary>
    public GameObject dummyLeft;

    /// <summary>�E�������\��</summary>
    public Animator pressRight;
    /// <summary>�㉟�����\��</summary>
    public Animator pressUp;
    /// <summary>���������\��</summary>
    public Animator pressDown;
    /// <summary>���������\��</summary>
    public Animator pressLeft;

    /// <summary>�����������\��</summary>
    public TMP_Text pressText;

    /// <summary>���T�\���Q�[�W�̃}�X�N</summary>
    public GameObject pointMask;

    #endregion

    #region �ϐ�

    /// <summary>�E��󃊃X�g</summary>
    private List<AmiGameArrow> listRight;
    /// <summary>���󃊃X�g</summary>
    private List<AmiGameArrow> listUp;
    /// <summary>����󃊃X�g</summary>
    private List<AmiGameArrow> listDown;
    /// <summary>����󃊃X�g</summary>
    private List<AmiGameArrow> listLeft;

    /// <summary>������</summary>
    private enum ArrowDir : int
    {
        Right = 0,
        Up,
        Down,
        Left,
    }

    /// <summary>�Q�[�����</summary>
    private enum GameState : int
    {
        /// <summary>�J�n������</summary>
        Starting = 0,
        /// <summary>�v���C��</summary>
        Game,
    }
    /// <summary>�Q�[�����</summary>
    private GameState state;

    /// <summary>�y���f�[�^</summary>
    private struct Note
    {
        public float startBeat;
        public float weightBeat;
        public ArrowDir dir;

        public Note(float s, float w, ArrowDir d) { startBeat = s; weightBeat = w; dir = d; }
    };
    /// <summary>�y�����X�g</summary>
    private List<Note> notes;
    /// <summary>BGM�J�n�^�C�~���O</summary>
    private float headTime;

    /// <summary>�G�p�y�����X�g</summary>
    private List<Note> cpuNotes;

    /// <summary>�ő�_��</summary>
    private int maxPoint = 1;
    /// <summary>�_��</summary>
    private int point = 0;

    #endregion

    #region ���

    /// <summary>
    /// �R���X�g���N�^
    /// </summary>
    public AmiGameSystemA()
    {
        listRight = new List<AmiGameArrow>();
        listUp = new List<AmiGameArrow>();
        listDown = new List<AmiGameArrow>();
        listLeft = new List<AmiGameArrow>();
        notes = new List<Note>();
        cpuNotes = new List<Note>();
    }

    /// <summary>
    /// ������
    /// </summary>
    /// <returns></returns>
    public override IEnumerator Start()
    {
        state = GameState.Starting;
        InitializeNote();

        maxPoint = notes.Count * 2;

        yield return base.Start();
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
        if (IsBossRush())
        {
            tutorial.SetTitle(StringMinigameMessage.AmiB_Title);
            tutorial.SetText(StringMinigameMessage.AmiB_Tutorial);
        }
        else
        {
            tutorial.SetTitle(StringMinigameMessage.AmiA_Title);
            tutorial.SetText(StringMinigameMessage.AmiA_Tutorial);
        }
        yield return tutorial.Open();
        yield return new WaitUntil(() => input.GetKeyPress(InputManager.Keys.South));
        yield return tutorial.Close();

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(ExecuteNotesCoroutine());
        state = GameState.Game;
    }

    /// <summary>
    /// �t���[������
    /// </summary>
    override public void Update()
    {
        base.Update();

        if (ManagerSceneScript.GetInstance().SceneState != ManagerSceneScript.State.Game) { return; }
        if (state != GameState.Game) { return; }

        var input = InputManager.GetInstance();

        if (input.GetKeyPress(InputManager.Keys.AmiRight))
        {
            PressButton(ArrowDir.Right);
        }
        else if (input.GetKeyPress(InputManager.Keys.AmiUp))
        {
            PressButton(ArrowDir.Up);
        }
        else if (input.GetKeyPress(InputManager.Keys.AmiDown))
        {
            PressButton(ArrowDir.Down);
        }
        else if (input.GetKeyPress(InputManager.Keys.AmiLeft))
        {
            PressButton(ArrowDir.Left);
        }

        CheckBadArrow();
    }

    #endregion

    #region �v���C�x�[�g���\�b�h

    #region �{�^������

    /// <summary>
    /// �{�^������
    /// </summary>
    /// <param name="dir"></param>
    private void PressButton(ArrowDir dir)
    {
        var animName = GetAnimName(dir);
        switch (dir)
        {
            case ArrowDir.Right:
                pressRight.PlayInFixedTime("press", 0, 0);
                StartCoroutine(PressButtonCoroutine(animName, animName, listRight));
                break;
            case ArrowDir.Up:
                pressUp.PlayInFixedTime("press", 0, 0);
                StartCoroutine(PressButtonCoroutine(animName, animName, listUp));
                break;
            case ArrowDir.Down:
                pressDown.PlayInFixedTime("press", 0, 0);
                StartCoroutine(PressButtonCoroutine(animName, animName, listDown));
                break;
            case ArrowDir.Left:
                pressLeft.PlayInFixedTime("press", 0, 0);
                StartCoroutine(PressButtonCoroutine(animName, animName, listLeft));
                break;
        }
    }

    /// <summary>
    /// �{�^�������R���[�`��
    /// </summary>
    /// <param name="animName"></param>
    /// <param name="boolName"></param>
    /// <param name="list"></param>
    /// <returns></returns>
    private IEnumerator PressButtonCoroutine(string animName, string boolName, List<AmiGameArrow> list)
    {
        // ��ԋ߂������擾
        AmiGameArrow nearest = list.OrderBy(a => a.GetNowPositionAbs()).FirstOrDefault();
        if (nearest != null && nearest.GetNowPositionAbs() > BAD_DIST)
        {
            // ���ꂷ���Ă��疳����
            nearest = null;
        }
        // �|�[�Y��鎞��
        var time = nearest != null ? nearest.GetWeight() : -1f;

        if (nearest != null)
        {
            // �����ɂ���Ĕ���
            var abs = nearest.GetNowPositionAbs();
            if (abs <= GREAT_DIST)
            {
                pressText.SetText(StringMinigameMessage.AmiA_Great);
                AddPoint(2);
            }
            else if (abs <= GOOD_DIST)
            {
                pressText.SetText(StringMinigameMessage.AmiA_Good);
                AddPoint(1);
            }
            else
            {
                pressText.SetText(StringMinigameMessage.AmiA_Bad);
                AddPoint(-2);
            }
            pressText.GetComponent<Animator>().PlayInFixedTime("display", 0, 0);

            // �Ώۖ�������
            list.Remove(nearest);
            Destroy(nearest.gameObject);
        }

        // �A�j���[�V�����Đ�
        yield return PlayAnim(player, animName, boolName, time);
    }

    /// <summary>
    /// �A�j���[�V������
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    private string GetAnimName(ArrowDir dir)
    {
        return dir switch
        {
            ArrowDir.Up => "up",
            ArrowDir.Down => "down",
            ArrowDir.Right => "right",
            _ => "left"
        };
    }

    /// <summary>
    /// �L�����A�j���[�V�����Đ�
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="animName"></param>
    /// <param name="boolName"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    private IEnumerator PlayAnim(Animator animator, string animName, string boolName, float time)
    {
        animator.PlayInFixedTime(animName, 0, 0);
        if (time <= 0f)
        {
            yield break;
        }

        animator.SetBool(boolName, true);
        yield return new WaitForSeconds(time);
        animator.SetBool(boolName, false);
    }

    /// <summary>
    /// Swing�A�j���[�V�����Đ�
    /// </summary>
    /// <param name="anim"></param>
    private void PlaySwing(Animator anim)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            anim.Play("swing");
        }
    }

    #endregion

    #region ���Ǘ�

    /// <summary>
    /// ���쐬
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="offset"></param>
    /// <param name="weight"></param>
    private void CreateArrow(ArrowDir dir, float offset, float weight = -1f)
    {
        AmiGameArrow ar = null;
        switch (dir)
        {
            case ArrowDir.Right:
                ar = GameObject.Instantiate(dummyRight).GetComponent<AmiGameArrow>();
                listRight.Add(ar);
                break;
            case ArrowDir.Up:
                ar = GameObject.Instantiate(dummyUp).GetComponent<AmiGameArrow>();
                listUp.Add(ar);
                break;
            case ArrowDir.Down:
                ar = GameObject.Instantiate(dummyDown).GetComponent<AmiGameArrow>();
                listDown.Add(ar);
                break;
            case ArrowDir.Left:
                ar = GameObject.Instantiate(dummyLeft).GetComponent<AmiGameArrow>();
                listLeft.Add(ar);
                break;
        }

        if (ar != null)
        {
            ar.transform.SetParent(playArrowParent.transform, true);
            ar.ForceStart();
            ar.AddOffset(offset);
            ar.gameObject.SetActive(true);
            ar.SetWeight(weight);
        }
    }

    /// <summary>
    /// ���ɒʂ�߂�����������
    /// </summary>
    private void CheckBadArrow()
    {
        var lists = new List<List<AmiGameArrow>>() { listRight, listUp, listDown, listLeft };

        foreach (var list in lists)
        {
            var bads = list.Where(a => a.IsActive() == false).ToList();

            foreach (var bad in bads)
            {
                list.Remove(bad);
                Destroy(bad.gameObject);
                AddPoint(-4);
            }
        }

    }

    #endregion

    #region �_��

    /// <summary>
    /// �_�����Z
    /// </summary>
    /// <param name="p"></param>
    private void AddPoint(int p)
    {
        point += p;
        if (point < -maxPoint) point = -maxPoint;

        // �Q�[�W�\���X�V
        var maskX = ((float)point / maxPoint) * -200f;
        pointMask.transform.localPosition = new Vector3(maskX, 0, 0);
    }

    #endregion

    #region �y������

    /// <summary>
    /// Note���s�R���[�`��
    /// </summary>
    /// <returns></returns>
    private IEnumerator ExecuteNotesCoroutine()
    {
        // �b�ɏ�Z���Ďl�������ɂ���l
        var timeRate = bpm / 60f;
        // ���쐬���画��܂ł̎���
        var delayNote = AmiGameArrow.DELAY_TIME * timeRate;
        // ���݈ʒu������
        var beforeTime = Time.realtimeSinceStartup + AmiGameArrow.DELAY_TIME;
        // ���݂�Note
        var index = 0;
        var cpuIndex = 0;
        // BGM�J�n�t���O
        var startBgm = false;
        // Swing�̎��Ԕ���
        var swingMod = -1;

        // �덷
        var gosa = 0.25f;

        while (index < notes.Count ||
            listRight.Count > 0 ||
            listUp.Count > 0 ||
            listDown.Count > 0 ||
            listLeft.Count > 0)
        {
            yield return null;
            var nowSeconds = Time.realtimeSinceStartup - beforeTime - headTime;

            // BGM�J�n����
            if (startBgm == false && nowSeconds >= -headTime)
            {
                ManagerSceneScript.GetInstance().soundMan.StartGameBgm(playMusic);
                startBgm = true;
            }

            // Swing����
            var swingModNew = Mathf.FloorToInt(nowSeconds * timeRate / swingNotes);
            if (swingMod != swingModNew && swingModNew >= 0)
            {
                PlaySwing(player);
                PlaySwing(enemy);

                swingMod = swingModNew;
            }

            // ���쐬����
            var nowNote = nowSeconds * timeRate - gosa;
            while (index < notes.Count && notes[index].startBeat <= nowNote + delayNote)
            {
                var offset = (nowNote + delayNote - notes[index].startBeat) / timeRate;
                CreateArrow(notes[index].dir, offset, notes[index].weightBeat / timeRate);
                index++;
            }

            // CPU
            while (cpuIndex < cpuNotes.Count && cpuNotes[cpuIndex].startBeat <= nowNote + gosa / 2f)
            {
                var animName = GetAnimName(cpuNotes[cpuIndex].dir);
                var time = cpuNotes[cpuIndex].weightBeat / timeRate;
                StartCoroutine(PlayAnim(enemy, animName, animName, time));
                cpuIndex++;
            }
        }

        yield return new WaitForSeconds(2f);

        // ���s����
        if (point > 0)
        {
            SetGameResult(true);
            if (IsBossRush())
            {
                Global.GetTemporaryData().bossRushAmiWon = true;
            }
        }
        else
        {
            SetGameResult(false);
            if (IsBossRush())
            {
                Global.GetTemporaryData().bossRushAmiWon = false;
            }
        }

        if (IsBossRush())
        {
            ManagerSceneScript.GetInstance().NextGame("GameSceneManaB");
        }
        else
        {
            ManagerSceneScript.GetInstance().ExitGame();
        }
    }

    /// <summary>
    /// �y��������
    /// </summary>
    private void InitializeNote()
    {
        if (IsBossRush() == false)
        {
            // Overworld Folk
            headTime = 0f;

            #region Overworld Folk�@�v���C���[
            notes.AddRange(new Note[]{
            new Note( 8.0f, 0.5f, ArrowDir.Left ),
            new Note( 8.75f, 0.5f, ArrowDir.Up ),
            new Note( 9.5f, 0.3f, ArrowDir.Down ),
            new Note( 10.0f, -1f, ArrowDir.Right ),

            new Note( 11.0f, 0.3f, ArrowDir.Down ),
            new Note( 11.5f, 0.3f, ArrowDir.Up ),
            new Note( 12.0f, 0.5f, ArrowDir.Down ),
            new Note( 12.75f, 0.5f, ArrowDir.Up ),
            new Note( 13.5f, 0.3f, ArrowDir.Right ),
            new Note( 14.0f, -1f, ArrowDir.Left ),

            new Note( 15.0f, 0.3f, ArrowDir.Down ),
            new Note( 15.5f, 0.3f, ArrowDir.Left ),
            new Note( 16.0f, 0.5f, ArrowDir.Down ),
            new Note( 16.75f, 0.5f, ArrowDir.Down ),
            new Note( 17.5f, 0.3f, ArrowDir.Up ),
            new Note( 18.0f, 0.3f, ArrowDir.Right ),
            new Note( 18.5f, 0.3f, ArrowDir.Up ),
            new Note( 19.0f, 0.3f, ArrowDir.Down ),
            new Note( 19.5f, 0.3f, ArrowDir.Left ),

            new Note( 20.0f, 0.5f, ArrowDir.Right ),
            new Note( 20.75f, 0.5f, ArrowDir.Left ),
            new Note( 21.5f, -1f, ArrowDir.Down ),
            new Note( 21.75f, -1f, ArrowDir.Up ),
            new Note( 22.0f, -1f, ArrowDir.Right ),

            new Note( 23.5f, 0.3f, ArrowDir.Left ),
            new Note( 24.0f, 0.5f, ArrowDir.Left ),
            new Note( 24.75f, 0.5f, ArrowDir.Up ),
            new Note( 25.5f, 0.3f, ArrowDir.Down ),
            new Note( 26.0f, -1f, ArrowDir.Right ),

            new Note( 27.0f, 0.3f, ArrowDir.Down ),
            new Note( 27.5f, 0.3f, ArrowDir.Up ),
            new Note( 28.0f, 0.5f, ArrowDir.Down ),
            new Note( 28.75f, 0.5f, ArrowDir.Up ),
            new Note( 29.5f, 0.3f, ArrowDir.Right ),
            new Note( 30.0f, -1f, ArrowDir.Left ),

            new Note( 31.0f, 0.3f, ArrowDir.Right ),
            new Note( 31.5f, 0.3f, ArrowDir.Down ),
            new Note( 32.0f, 0.5f, ArrowDir.Up ),
            new Note( 32.75f, 0.5f, ArrowDir.Down ),
            new Note( 33.5f, 0.3f, ArrowDir.Left ),
            new Note( 34.0f, 0.3f, ArrowDir.Right ),
            new Note( 34.5f, 0.3f, ArrowDir.Right ),
            new Note( 35.0f, 0.3f, ArrowDir.Left ),
            new Note( 35.5f, 0.3f, ArrowDir.Right ),
            new Note( 36.0f, -1f, ArrowDir.Down ),
            new Note( 36.75f, -1f, ArrowDir.Down ),
            new Note( 37.5f, -1f, ArrowDir.Down ),
            new Note( 37.75f, -1f, ArrowDir.Left ),
            new Note( 38.0f, -1f, ArrowDir.Down ),

            new Note( 39.5f, -1f, ArrowDir.Left ),
            new Note( 39.75f, -1f, ArrowDir.Down ),
            new Note( 40.0f, 0.5f, ArrowDir.Up ),
            new Note( 40.75f, 0.5f, ArrowDir.Down ),
            new Note( 41.5f, 0.3f, ArrowDir.Up ),
            new Note( 42.0f, 0.5f, ArrowDir.Right ),
            new Note( 42.75f, 0.5f, ArrowDir.Up ),
            new Note( 43.5f, 0.3f, ArrowDir.Right ),
            new Note( 44.0f, 0.5f, ArrowDir.Up ),
            new Note( 44.75f, 0.5f, ArrowDir.Down ),
            new Note( 45.5f, -1f, ArrowDir.Down ),
            new Note( 45.75f, -1f, ArrowDir.Right ),
            new Note( 46.0f, -1f, ArrowDir.Left ),

            new Note( 47.0f, 0.3f, ArrowDir.Left ),
            new Note( 47.5f, 0.3f, ArrowDir.Up ),
            new Note( 48.0f, 0.5f, ArrowDir.Right ),
            new Note( 48.75f, 0.5f, ArrowDir.Down ),
            new Note( 49.5f, 0.3f, ArrowDir.Left ),
            new Note( 50.0f, 0.5f, ArrowDir.Down ),
            new Note( 50.75f, 0.5f, ArrowDir.Left ),
            new Note( 51.5f, 0.3f, ArrowDir.Up ),
            new Note( 52.0f, 2f, ArrowDir.Right ),

            new Note( 55.5f, -1f, ArrowDir.Down ),
            new Note( 55.75f, -1f, ArrowDir.Up ),
            new Note( 56.0f, 0.5f, ArrowDir.Right ),
            new Note( 56.75f, 0.5f, ArrowDir.Up ),
            new Note( 57.5f, 0.3f, ArrowDir.Left ),
            new Note( 58.0f, -1f, ArrowDir.Down ),
            new Note( 59.0f, 0.3f, ArrowDir.Left ),
            new Note( 59.5f, 0.3f, ArrowDir.Down ),
            new Note( 60.0f, 0.5f, ArrowDir.Right ),
            new Note( 60.75f, 0.5f, ArrowDir.Up ),
            new Note( 61.5f, 0.3f, ArrowDir.Down ),
            new Note( 62.0f, -1f, ArrowDir.Left ),

            new Note( 63.5f, 0.3f, ArrowDir.Down ),
            new Note( 64.0f, 1f, ArrowDir.Down ),
            new Note( 65.5f, 0.3f, ArrowDir.Up ),
            new Note( 66.0f, 0.5f, ArrowDir.Down ),
            new Note( 66.75f, 0.5f, ArrowDir.Left ),
            new Note( 67.5f, 0.3f, ArrowDir.Up ),
            new Note( 68.0f, 1f, ArrowDir.Right ),

            new Note( 69.0f, -1f, ArrowDir.Left ),
            new Note( 69.25f, -1f, ArrowDir.Down ),
            new Note( 69.5f, -1f, ArrowDir.Right ),
            new Note( 69.75f, -1f, ArrowDir.Up ),
            new Note( 70.0f, -1f, ArrowDir.Right ),
            new Note( 70.25f, -1f, ArrowDir.Up ),
            new Note( 70.5f, -1f, ArrowDir.Left ),
            new Note( 70.75f, -1f, ArrowDir.Down ),
            new Note( 71.0f, -1f, ArrowDir.Right ),
            new Note( 71.25f, -1f, ArrowDir.Up ),
            new Note( 71.5f, -1f, ArrowDir.Left ),
            new Note( 71.75f, -1f, ArrowDir.Down ),
            new Note( 72.0f, -1f, ArrowDir.Up ),
            new Note( 72.75f, -1f, ArrowDir.Up ),
            new Note( 73.5f, -1f, ArrowDir.Right ),
            new Note( 74.0f, -1f, ArrowDir.Right ),

            new Note( 75.0f, 0.3f, ArrowDir.Down ),
            new Note( 75.5f, 0.3f, ArrowDir.Down ),
            new Note( 76.0f, 0.5f, ArrowDir.Up ),
            new Note( 76.75f, 0.5f, ArrowDir.Up ),
            new Note( 77.25f, -1f, ArrowDir.Right ),
            new Note( 77.5f, -1f, ArrowDir.Up ),
            new Note( 77.75f, -1f, ArrowDir.Down ),
            new Note( 78.0f, -1f, ArrowDir.Left ),

            new Note( 79.0f, 0.3f, ArrowDir.Down ),
            new Note( 79.5f, 0.3f, ArrowDir.Down ),
            new Note( 80.0f, 0.5f, ArrowDir.Up ),
            new Note( 80.75f, 0.5f, ArrowDir.Up ),
            new Note( 81.25f, -1f, ArrowDir.Right ),
            new Note( 81.5f, -1f, ArrowDir.Up ),
            new Note( 81.75f, -1f, ArrowDir.Down ),
            new Note( 82.0f, -1f, ArrowDir.Left ),

            new Note( 83.0f, 0.3f, ArrowDir.Right ),
            new Note( 83.5f, 0.3f, ArrowDir.Left ),
            new Note( 84.0f, 0.5f, ArrowDir.Left ),
            new Note( 84.75f, 0.5f, ArrowDir.Down ),
            new Note( 85.5f, 0.3f, ArrowDir.Up ),
            new Note( 86.0f, 0.3f, ArrowDir.Down ),
            new Note( 86.5f, 0.3f, ArrowDir.Down ),
            new Note( 87.0f, 0.3f, ArrowDir.Left ),
            new Note( 87.5f, 0.3f, ArrowDir.Down ),
            new Note( 88.0f, 0.5f, ArrowDir.Up ),
            new Note( 88.75f, 0.5f, ArrowDir.Right ),
            new Note( 89.5f, 0.3f, ArrowDir.Up ),
            new Note( 90.0f, -1f, ArrowDir.Right ),

            new Note( 91.0f, 0.3f, ArrowDir.Down ),
            new Note( 91.5f, 0.3f, ArrowDir.Down ),
            new Note( 92.0f, 0.5f, ArrowDir.Up ),
            new Note( 92.75f, 0.5f, ArrowDir.Up ),
            new Note( 93.25f, -1f, ArrowDir.Right ),
            new Note( 93.5f, -1f, ArrowDir.Up ),
            new Note( 93.75f, -1f, ArrowDir.Left ),
            new Note( 94.0f, -1f, ArrowDir.Down ),

            new Note( 95.0f, 0.3f, ArrowDir.Down ),
            new Note( 95.5f, 0.3f, ArrowDir.Up ),
            new Note( 96.0f, 0.5f, ArrowDir.Right ),
            new Note( 96.75f, 0.5f, ArrowDir.Right ),
            new Note( 97.25f, -1f, ArrowDir.Up ),
            new Note( 97.5f, -1f, ArrowDir.Down ),
            new Note( 97.75f, -1f, ArrowDir.Left ),
            new Note( 98.0f, -1f, ArrowDir.Up ),

            new Note( 99.0f, 0.3f, ArrowDir.Down ),
            new Note( 99.5f, 0.3f, ArrowDir.Down ),
            new Note( 100.0f, 0.5f, ArrowDir.Up ),
            new Note( 100.75f, 0.5f, ArrowDir.Right ),
            new Note( 101.5f, 0.3f, ArrowDir.Down ),
            new Note( 102.0f, 0.3f, ArrowDir.Up ),
            new Note( 102.5f, 0.3f, ArrowDir.Up ),
            new Note( 103.0f, 0.3f, ArrowDir.Left ),
            new Note( 103.5f, 0.3f, ArrowDir.Right ),
            new Note( 104.0f, -1f, ArrowDir.Up ),
            new Note( 104.75f, -1f, ArrowDir.Up),
            new Note( 105.5f, -1f, ArrowDir.Up ),
            new Note( 105.75f, -1f, ArrowDir.Down ),
            new Note( 106.0f, -1f, ArrowDir.Up ),

            new Note( 107.0f, -1f, ArrowDir.Down ),
            new Note( 107.25f, -1f, ArrowDir.Left ),
            new Note( 107.5f, -1f, ArrowDir.Up ),
            new Note( 107.75f, -1f, ArrowDir.Right ),
            new Note( 108.0f, 0.3f, ArrowDir.Right ),
            new Note( 108.75f, -1f, ArrowDir.Down ),
            new Note( 109.0f, -1f, ArrowDir.Up ),
            new Note( 109.5f, -1f, ArrowDir.Right ),
            new Note( 110.0f, 0.3f, ArrowDir.Up ),
            new Note( 110.75f, -1f, ArrowDir.Left ),
            new Note( 111.0f, -1f, ArrowDir.Down ),
            new Note( 111.5f, -1f, ArrowDir.Up ),

            new Note( 112.0f, 0.3f, ArrowDir.Up ),
            new Note( 112.75f, -1f, ArrowDir.Right ),
            new Note( 113.0f, -1f, ArrowDir.Up ),
            new Note( 113.25f, -1f, ArrowDir.Down ),
            new Note( 113.5f, -1f, ArrowDir.Left ),
            new Note( 114.0f, 0.3f, ArrowDir.Up ),
            new Note( 114.5f, -1f, ArrowDir.Left ),
            new Note( 114.75f, -1f, ArrowDir.Down ),
            new Note( 115.0f, -1f, ArrowDir.Right ),

            new Note( 115.5f, -1f, ArrowDir.Left ),
            new Note( 115.75f, -1f, ArrowDir.Up ),
            new Note( 116.0f, 0.3f, ArrowDir.Right ),
            new Note( 116.75f, -1f, ArrowDir.Down ),
            new Note( 117.0f, -1f, ArrowDir.Up ),
            new Note( 117.5f, -1f, ArrowDir.Right ),
            new Note( 118.0f, 0.3f, ArrowDir.Up ),
            new Note( 118.75f, -1f, ArrowDir.Left ),
            new Note( 119.0f, -1f, ArrowDir.Down ),
            new Note( 119.5f, -1f, ArrowDir.Up ),

            new Note( 120.0f, -1f, ArrowDir.Down ),
            new Note( 120.75f, -1f, ArrowDir.Up ),
            new Note( 121.0f, -1f, ArrowDir.Right ),
            new Note( 121.25f, -1f, ArrowDir.Up ),
            new Note( 121.5f, -1f, ArrowDir.Down ),
            new Note( 121.75f, -1f, ArrowDir.Left ),
            new Note( 122.0f, 0.3f, ArrowDir.Up ),
            new Note( 122.5f, -1f, ArrowDir.Up ),
            new Note( 122.75f, -1f, ArrowDir.Left ),
            new Note( 123.0f, -1f, ArrowDir.Up ),
            new Note( 123.25f, -1f, ArrowDir.Down ),
            new Note( 123.5f, -1f, ArrowDir.Left ),
            new Note( 123.75f, -1f, ArrowDir.Right ),

            new Note( 124.0f, 0.3f, ArrowDir.Up ),
            new Note( 124.5f, -1f, ArrowDir.Down ),
            new Note( 124.75f, -1f, ArrowDir.Left ),
            new Note( 125.0f, 0.3f, ArrowDir.Left ),
            new Note( 125.5f, -1f, ArrowDir.Up ),
            new Note( 125.75f, -1f, ArrowDir.Right ),
            new Note( 126.0f, -1f, ArrowDir.Up ),
            new Note( 126.25f, -1f, ArrowDir.Down ),
            new Note( 126.5f, -1f, ArrowDir.Left ),
            new Note( 126.75f, -1f, ArrowDir.Down ),
            new Note( 127.0f, -1f, ArrowDir.Up ),
            new Note( 127.25f, -1f, ArrowDir.Down ),
            new Note( 127.5f, 0.5f, ArrowDir.Left ),

            new Note( 128.0f, 0.5f, ArrowDir.Down ),
            new Note( 128.5f, -1f, ArrowDir.Left ),
            new Note( 128.75f, -1f, ArrowDir.Down ),
            new Note( 129.0f, -1f, ArrowDir.Up ),
            new Note( 129.25f, -1f, ArrowDir.Up ),
            new Note( 129.5f, -1f, ArrowDir.Right ),
            new Note( 129.75f, -1f, ArrowDir.Left ),
            new Note( 130.0f, -1f, ArrowDir.Up ),
            new Note( 130.25f, -1f, ArrowDir.Up ),
            new Note( 130.5f, -1f, ArrowDir.Right ),
            new Note( 130.75f, -1f, ArrowDir.Left ),
            new Note( 131.0f, -1f, ArrowDir.Down ),

            new Note( 131.5f, -1f, ArrowDir.Down),
            new Note( 131.75f, -1f, ArrowDir.Right ),
            new Note( 132.0f, 0.5f, ArrowDir.Up ),
            new Note( 132.5f, -1f, ArrowDir.Left ),
            new Note( 132.75f, -1f, ArrowDir.Up ),
            new Note( 133.0f, -1f, ArrowDir.Left ),
            new Note( 133.25f, -1f, ArrowDir.Down ),
            new Note( 133.5f, -1f, ArrowDir.Up ),
            new Note( 133.75f, -1f, ArrowDir.Right ),
            new Note( 134.0f, -1f, ArrowDir.Up ),
            new Note( 134.25f, -1f, ArrowDir.Right ),
            new Note( 134.5f, -1f, ArrowDir.Up ),
            new Note( 134.75f, -1f, ArrowDir.Down ),
            new Note( 135.0f, -1f, ArrowDir.Left ),
            new Note( 135.25f, -1f, ArrowDir.Down ),
            new Note( 135.5f, -1f, ArrowDir.Up ),
            new Note( 135.75f, -1f, ArrowDir.Right ),

            new Note( 136.0f, 0.5f, ArrowDir.Right ),
            new Note( 136.5f, -1f, ArrowDir.Up ),
            new Note( 136.75f, -1f, ArrowDir.Down ),
            new Note( 137.0f, -1f, ArrowDir.Left ),
            new Note( 137.25f, -1f, ArrowDir.Down ),
            new Note( 137.5f, -1f, ArrowDir.Up ),
            new Note( 138.0f, -1f, ArrowDir.Right ),
        });
            #endregion

            #region Overworld Folk�@CPU
            cpuNotes.AddRange(new Note[]{
            new Note( 8.0f, 0.5f, ArrowDir.Right ),
            new Note( 8.75f, 0.5f, ArrowDir.Up ),
            new Note( 9.5f, 0.3f, ArrowDir.Down ),
            new Note( 10.0f, -1f, ArrowDir.Left ),

            new Note( 11.0f, 0.3f, ArrowDir.Down ),
            new Note( 11.5f, 0.3f, ArrowDir.Up ),
            new Note( 12.0f, 0.5f, ArrowDir.Down ),
            new Note( 12.75f, 0.5f, ArrowDir.Up ),
            new Note( 13.5f, 0.3f, ArrowDir.Left ),
            new Note( 14.0f, -1f, ArrowDir.Right ),

            new Note( 15.0f, 0.3f, ArrowDir.Down ),
            new Note( 15.5f, 0.3f, ArrowDir.Right ),
            new Note( 16.0f, 0.5f, ArrowDir.Down ),
            new Note( 16.75f, 0.5f, ArrowDir.Down ),
            new Note( 17.5f, 0.3f, ArrowDir.Up ),
            new Note( 18.0f, 0.3f, ArrowDir.Left ),
            new Note( 18.5f, 0.3f, ArrowDir.Up ),
            new Note( 19.0f, 0.3f, ArrowDir.Down ),
            new Note( 19.5f, 0.3f, ArrowDir.Right ),

            new Note( 20.0f, 0.5f, ArrowDir.Left ),
            new Note( 20.75f, 0.5f, ArrowDir.Right ),
            new Note( 21.5f, -1f, ArrowDir.Down ),
            new Note( 21.75f, -1f, ArrowDir.Up ),
            new Note( 22.0f, -1f, ArrowDir.Left ),

            new Note( 23.5f, 0.3f, ArrowDir.Right ),
            new Note( 24.0f, 0.5f, ArrowDir.Right ),
            new Note( 24.75f, 0.5f, ArrowDir.Up ),
            new Note( 25.5f, 0.3f, ArrowDir.Down ),
            new Note( 26.0f, -1f, ArrowDir.Left ),

            new Note( 27.0f, 0.3f, ArrowDir.Down ),
            new Note( 27.5f, 0.3f, ArrowDir.Up ),
            new Note( 28.0f, 0.5f, ArrowDir.Down ),
            new Note( 28.75f, 0.5f, ArrowDir.Up ),
            new Note( 29.5f, 0.3f, ArrowDir.Left ),
            new Note( 30.0f, -1f, ArrowDir.Right ),

            new Note( 31.0f, 0.3f, ArrowDir.Left ),
            new Note( 31.5f, 0.3f, ArrowDir.Down ),
            new Note( 32.0f, 0.5f, ArrowDir.Up ),
            new Note( 32.75f, 0.5f, ArrowDir.Down ),
            new Note( 33.5f, 0.3f, ArrowDir.Right ),
            new Note( 34.0f, 0.3f, ArrowDir.Left ),
            new Note( 34.5f, 0.3f, ArrowDir.Left ),
            new Note( 35.0f, 0.3f, ArrowDir.Right ),
            new Note( 35.5f, 0.3f, ArrowDir.Left ),
            new Note( 36.0f, -1f, ArrowDir.Down ),
            new Note( 36.75f, -1f, ArrowDir.Down ),
            new Note( 37.5f, -1f, ArrowDir.Down ),
            new Note( 37.75f, -1f, ArrowDir.Right ),
            new Note( 38.0f, -1f, ArrowDir.Down ),

            new Note( 39.5f, -1f, ArrowDir.Right ),
            new Note( 39.75f, -1f, ArrowDir.Down ),
            new Note( 40.0f, 0.5f, ArrowDir.Up ),
            new Note( 40.75f, 0.5f, ArrowDir.Down ),
            new Note( 41.5f, 0.3f, ArrowDir.Up ),
            new Note( 42.0f, 0.5f, ArrowDir.Left ),
            new Note( 42.75f, 0.5f, ArrowDir.Up ),
            new Note( 43.5f, 0.3f, ArrowDir.Left ),
            new Note( 44.0f, 0.5f, ArrowDir.Up ),
            new Note( 44.75f, 0.5f, ArrowDir.Down ),
            new Note( 45.5f, -1f, ArrowDir.Down ),
            new Note( 45.75f, -1f, ArrowDir.Left ),

            new Note( 46.0f, 0.5f, ArrowDir.Up ),
            new Note( 46.75f, 0.5f, ArrowDir.Left ),
            new Note( 47.5f, 0.3f, ArrowDir.Down ),
            new Note( 48.0f, 1.5f, ArrowDir.Right ),
            new Note( 49.5f, 0.3f, ArrowDir.Down ),
            new Note( 50.0f, 0.5f, ArrowDir.Left ),
            new Note( 50.75f, 0.5f, ArrowDir.Down ),
            new Note( 51.5f, 0.5f, ArrowDir.Up ),
            new Note( 52.0f, 0.5f, ArrowDir.Right ),
            new Note( 52.75f, 2f, ArrowDir.Down ),

            new Note( 56.0f, 2f, ArrowDir.Up ),
            new Note( 58.0f, 1f, ArrowDir.Down ),
            new Note( 59.0f, 1f, ArrowDir.Right ),
            new Note( 60.0f, 1f, ArrowDir.Down ),
            new Note( 61.0f, 1f, ArrowDir.Right ),
            new Note( 62.0f, 1f, ArrowDir.Up ),

            new Note( 63.5f, 0.3f, ArrowDir.Down ),
            new Note( 64.0f, 1f, ArrowDir.Down ),
            new Note( 65.5f, 0.3f, ArrowDir.Up ),
            new Note( 66.0f, 0.5f, ArrowDir.Down ),
            new Note( 66.75f, 0.5f, ArrowDir.Right ),
            new Note( 67.5f, 0.3f, ArrowDir.Up ),
            new Note( 68.0f, 1f, ArrowDir.Left ),

            new Note( 69.0f, -1f, ArrowDir.Right ),
            new Note( 69.25f, -1f, ArrowDir.Down ),
            new Note( 69.5f, -1f, ArrowDir.Left ),
            new Note( 69.75f, -1f, ArrowDir.Up ),
            new Note( 70.0f, -1f, ArrowDir.Left ),
            new Note( 70.25f, -1f, ArrowDir.Up ),
            new Note( 70.5f, -1f, ArrowDir.Right ),
            new Note( 70.75f, -1f, ArrowDir.Down ),
            new Note( 71.0f, -1f, ArrowDir.Left ),
            new Note( 71.25f, -1f, ArrowDir.Up ),
            new Note( 71.5f, -1f, ArrowDir.Right ),
            new Note( 71.75f, -1f, ArrowDir.Down ),
            new Note( 72.0f, -1f, ArrowDir.Up ),
            new Note( 72.75f, -1f, ArrowDir.Up ),
            new Note( 73.5f, -1f, ArrowDir.Left ),
            new Note( 74.0f, -1f, ArrowDir.Left ),

            new Note( 78.0f, 0.5f, ArrowDir.Up ),
            new Note( 78.75f, 0.5f, ArrowDir.Up ),
            new Note( 79.25f, -1f, ArrowDir.Left ),
            new Note( 79.5f, -1f, ArrowDir.Up ),
            new Note( 79.75f, -1f, ArrowDir.Down ),
            new Note( 80.0f, -1f, ArrowDir.Right ),

            new Note( 82.0f, 0.5f, ArrowDir.Up ),
            new Note( 82.75f, 0.5f, ArrowDir.Up ),
            new Note( 83.25f, -1f, ArrowDir.Left ),
            new Note( 83.5f, -1f, ArrowDir.Up ),
            new Note( 83.75f, -1f, ArrowDir.Down ),

            new Note( 84.0f, -1f, ArrowDir.Right ),
            new Note( 84.75f, 0.5f, ArrowDir.Down ),
            new Note( 85.5f, 0.3f, ArrowDir.Up ),
            new Note( 86.0f, 0.3f, ArrowDir.Down ),
            new Note( 86.5f, 0.3f, ArrowDir.Down ),
            new Note( 87.0f, 0.3f, ArrowDir.Right ),
            new Note( 87.5f, 0.3f, ArrowDir.Down ),
            new Note( 88.0f, 0.5f, ArrowDir.Up ),
            new Note( 88.75f, 0.5f, ArrowDir.Left ),
            new Note( 89.5f, 0.3f, ArrowDir.Up ),
            new Note( 90.0f, -1f, ArrowDir.Left ),

            new Note( 94.0f, 0.5f, ArrowDir.Up ),
            new Note( 94.75f, 0.5f, ArrowDir.Up ),
            new Note( 95.25f, -1f, ArrowDir.Left ),
            new Note( 95.5f, -1f, ArrowDir.Up ),
            new Note( 95.75f, -1f, ArrowDir.Right ),
            new Note( 96.0f, -1f, ArrowDir.Down ),

            new Note( 98.0f, 0.5f, ArrowDir.Left ),
            new Note( 98.75f, 0.5f, ArrowDir.Left ),
            new Note( 99.25f, -1f, ArrowDir.Up ),
            new Note( 99.5f, -1f, ArrowDir.Down ),
            new Note( 99.75f, -1f, ArrowDir.Right ),
            new Note( 100.0f, 0.5f, ArrowDir.Up ),

            new Note( 100.75f, 0.5f, ArrowDir.Left ),
            new Note( 101.5f, 0.3f, ArrowDir.Down ),
            new Note( 102.0f, 0.3f, ArrowDir.Up ),
            new Note( 102.5f, 0.3f, ArrowDir.Up ),
            new Note( 103.0f, 0.3f, ArrowDir.Right ),
            new Note( 103.5f, 0.3f, ArrowDir.Left ),
            new Note( 104.0f, -1f, ArrowDir.Up ),
            new Note( 104.75f, -1f, ArrowDir.Up),
            new Note( 105.5f, -1f, ArrowDir.Up ),
            new Note( 105.75f, -1f, ArrowDir.Down ),
            new Note( 106.0f, -1f, ArrowDir.Up ),

            new Note( 107.0f, -1f, ArrowDir.Down ),
            new Note( 107.25f, -1f, ArrowDir.Right ),
            new Note( 107.5f, -1f, ArrowDir.Up ),
            new Note( 107.75f, -1f, ArrowDir.Left ),
            new Note( 108.0f, 0.3f, ArrowDir.Left ),
            new Note( 108.75f, -1f, ArrowDir.Down ),
            new Note( 109.0f, -1f, ArrowDir.Up ),
            new Note( 109.5f, -1f, ArrowDir.Left ),
            new Note( 110.0f, 0.3f, ArrowDir.Up ),
            new Note( 110.75f, -1f, ArrowDir.Right ),
            new Note( 111.0f, -1f, ArrowDir.Down ),
            new Note( 111.5f, -1f, ArrowDir.Up ),

            new Note( 112.0f, 0.3f, ArrowDir.Up ),
            new Note( 112.75f, -1f, ArrowDir.Left ),
            new Note( 113.0f, -1f, ArrowDir.Up ),
            new Note( 113.25f, -1f, ArrowDir.Down ),
            new Note( 113.5f, -1f, ArrowDir.Right ),
            new Note( 114.0f, 0.3f, ArrowDir.Up ),
            new Note( 114.5f, -1f, ArrowDir.Right ),
            new Note( 114.75f, -1f, ArrowDir.Down ),
            new Note( 115.0f, -1f, ArrowDir.Left ),

            new Note( 115.5f, -1f, ArrowDir.Right ),
            new Note( 115.75f, -1f, ArrowDir.Up ),
            new Note( 116.0f, 0.3f, ArrowDir.Left ),
            new Note( 116.75f, -1f, ArrowDir.Down ),
            new Note( 117.0f, -1f, ArrowDir.Up ),
            new Note( 117.5f, -1f, ArrowDir.Left ),
            new Note( 118.0f, 0.3f, ArrowDir.Up ),
            new Note( 118.75f, -1f, ArrowDir.Right ),
            new Note( 119.0f, -1f, ArrowDir.Down ),
            new Note( 119.5f, -1f, ArrowDir.Up ),

            new Note( 120.0f, -1f, ArrowDir.Down ),
            new Note( 120.75f, -1f, ArrowDir.Up ),
            new Note( 121.0f, -1f, ArrowDir.Left ),
            new Note( 121.25f, -1f, ArrowDir.Up ),
            new Note( 121.5f, -1f, ArrowDir.Down ),
            new Note( 121.75f, -1f, ArrowDir.Right ),
            new Note( 122.0f, 0.3f, ArrowDir.Up ),
            new Note( 122.5f, -1f, ArrowDir.Up ),
            new Note( 122.75f, -1f, ArrowDir.Right ),
            new Note( 123.0f, -1f, ArrowDir.Up ),
            new Note( 123.25f, -1f, ArrowDir.Down ),
            new Note( 123.5f, -1f, ArrowDir.Right ),
            new Note( 123.75f, -1f, ArrowDir.Left ),

            new Note( 124.0f, 0.3f, ArrowDir.Up ),
            new Note( 124.5f, -1f, ArrowDir.Down ),
            new Note( 124.75f, -1f, ArrowDir.Right ),
            new Note( 125.0f, 0.3f, ArrowDir.Right ),
            new Note( 125.5f, -1f, ArrowDir.Up ),
            new Note( 125.75f, -1f, ArrowDir.Left ),
            new Note( 126.0f, -1f, ArrowDir.Up ),
            new Note( 126.25f, -1f, ArrowDir.Down ),
            new Note( 126.5f, -1f, ArrowDir.Right ),
            new Note( 126.75f, -1f, ArrowDir.Down ),
            new Note( 127.0f, -1f, ArrowDir.Up ),
            new Note( 127.25f, -1f, ArrowDir.Down ),
            new Note( 127.5f, 0.5f, ArrowDir.Right ),

            new Note( 128.0f, 0.5f, ArrowDir.Down ),
            new Note( 128.5f, -1f, ArrowDir.Right ),
            new Note( 128.75f, -1f, ArrowDir.Down ),
            new Note( 129.0f, -1f, ArrowDir.Up ),
            new Note( 129.25f, -1f, ArrowDir.Up ),
            new Note( 129.5f, -1f, ArrowDir.Left ),
            new Note( 129.75f, -1f, ArrowDir.Right ),
            new Note( 130.0f, -1f, ArrowDir.Up ),
            new Note( 130.25f, -1f, ArrowDir.Up ),
            new Note( 130.5f, -1f, ArrowDir.Left ),
            new Note( 130.75f, -1f, ArrowDir.Right ),
            new Note( 131.0f, -1f, ArrowDir.Down ),

            new Note( 131.5f, -1f, ArrowDir.Down),
            new Note( 131.75f, -1f, ArrowDir.Left ),
            new Note( 132.0f, 0.5f, ArrowDir.Up ),
            new Note( 132.5f, -1f, ArrowDir.Right ),
            new Note( 132.75f, -1f, ArrowDir.Up ),
            new Note( 133.0f, -1f, ArrowDir.Right ),
            new Note( 133.25f, -1f, ArrowDir.Down ),
            new Note( 133.5f, -1f, ArrowDir.Up ),
            new Note( 133.75f, -1f, ArrowDir.Left ),
            new Note( 134.0f, -1f, ArrowDir.Up ),
            new Note( 134.25f, -1f, ArrowDir.Left ),
            new Note( 134.5f, -1f, ArrowDir.Up ),
            new Note( 134.75f, -1f, ArrowDir.Down ),
            new Note( 135.0f, -1f, ArrowDir.Right ),
            new Note( 135.25f, -1f, ArrowDir.Down ),
            new Note( 135.5f, -1f, ArrowDir.Up ),
            new Note( 135.75f, -1f, ArrowDir.Left ),

            new Note( 136.0f, 0.5f, ArrowDir.Left ),
            new Note( 136.5f, -1f, ArrowDir.Up ),
            new Note( 136.75f, -1f, ArrowDir.Down ),
            new Note( 137.0f, -1f, ArrowDir.Right ),
            new Note( 137.25f, -1f, ArrowDir.Down ),
            new Note( 137.5f, -1f, ArrowDir.Up ),
            new Note( 138.0f, -1f, ArrowDir.Left ),
        });
            #endregion
        }
        else
        {
            // Prairie4
            headTime = 0f;

            #region Prairie4�@�v���C���[
            notes.AddRange(new Note[]{
                new Note( 0.0f, 0.5f, ArrowDir.Up ),
                new Note( 0.75f, -1f, ArrowDir.Up ),
                new Note( 1.0f, -1f, ArrowDir.Right ),
                new Note( 1.25f, -1f, ArrowDir.Left ),
                new Note( 1.5f, -1f, ArrowDir.Down ),
                new Note( 2.25f, -1f, ArrowDir.Left ),
                new Note( 2.75f, -1f, ArrowDir.Left ),
                new Note( 3.0f, -1f, ArrowDir.Down ),
                new Note( 3.25f, -1f, ArrowDir.Left ),
                new Note( 3.5f, -1f, ArrowDir.Up ),
                new Note( 3.75f, -1f, ArrowDir.Right ),

                new Note( 4.0f, 0.5f, ArrowDir.Left ),
                new Note( 4.75f, -1f, ArrowDir.Down ),
                new Note( 5.0f, -1f, ArrowDir.Left ),
                new Note( 5.25f, -1f, ArrowDir.Up ),
                new Note( 5.5f, -1f, ArrowDir.Right ),
                new Note( 6.25f, -1f, ArrowDir.Right ),
                new Note( 6.75f, -1f, ArrowDir.Right ),
                new Note( 7.0f, -1f, ArrowDir.Left ),
                new Note( 7.25f, -1f, ArrowDir.Down ),
                new Note( 7.5f, -1f, ArrowDir.Up ),
                new Note( 7.75f, -1f, ArrowDir.Right ),

                new Note( 8.0f, 0.5f, ArrowDir.Up ),
                new Note( 8.75f, -1f, ArrowDir.Up ),
                new Note( 9.0f, -1f, ArrowDir.Right ),
                new Note( 9.25f, -1f, ArrowDir.Down ),
                new Note( 9.5f, -1f, ArrowDir.Up ),
                new Note( 9.75f, -1f, ArrowDir.Right ),
                new Note( 10.25f, -1f, ArrowDir.Right ),
                new Note( 10.75f, -1f, ArrowDir.Right ),
                new Note( 11.0f, -1f, ArrowDir.Up ),
                new Note( 11.25f, -1f, ArrowDir.Down ),
                new Note( 11.5f, -1f, ArrowDir.Left ),
                new Note( 11.75f, -1f, ArrowDir.Down ),
                new Note( 12.0f, -1f, ArrowDir.Left ),
                new Note( 12.5f, -1f, ArrowDir.Right ),
                new Note( 12.75f, -1f, ArrowDir.Up ),
                new Note( 13.25f, -1f, ArrowDir.Down ),
                new Note( 13.5f, -1f, ArrowDir.Up ),

                new Note( 15.0f, -1f, ArrowDir.Left ),
                new Note( 15.25f, -1f, ArrowDir.Down ),
                new Note( 15.5f, -1f, ArrowDir.Up ),
                new Note( 15.75f, -1f, ArrowDir.Right ),
                new Note( 16.0f, -1f, ArrowDir.Down ),
                new Note( 16.75f, -1f, ArrowDir.Down ),
                new Note( 17.25f, -1f, ArrowDir.Down ),
                new Note( 17.5f, -1f, ArrowDir.Up ),
                new Note( 17.75f, -1f, ArrowDir.Down ),
                new Note( 18.0f, -1f, ArrowDir.Left ),
                new Note( 18.25f, -1f, ArrowDir.Right ),
                new Note( 18.5f, -1f, ArrowDir.Up ),
                new Note( 18.75f, -1f, ArrowDir.Down ),
                new Note( 19.25f, -1f, ArrowDir.Up ),
                new Note( 19.5f, -1f, ArrowDir.Left ),
                new Note( 19.75f, -1f, ArrowDir.Up ),

                new Note( 20.0f, -1f, ArrowDir.Right ),
                new Note( 20.75f, -1f, ArrowDir.Right ),
                new Note( 21.25f, -1f, ArrowDir.Right ),
                new Note( 21.5f, -1f, ArrowDir.Up ),
                new Note( 21.75f, -1f, ArrowDir.Down ),
                new Note( 22.0f, -1f, ArrowDir.Left ),
                new Note( 22.25f, -1f, ArrowDir.Down ),
                new Note( 22.5f, -1f, ArrowDir.Up ),
                new Note( 22.75f, -1f, ArrowDir.Right ),
                new Note( 23.25f, -1f, ArrowDir.Down ),
                new Note( 23.5f, -1f, ArrowDir.Left ),
                new Note( 23.75f, -1f, ArrowDir.Up ),

                new Note( 24.0f, -1f, ArrowDir.Right ),
                new Note( 24.75f, -1f, ArrowDir.Right ),
                new Note( 25.25f, -1f, ArrowDir.Right ),
                new Note( 25.5f, -1f, ArrowDir.Down ),
                new Note( 25.75f, -1f, ArrowDir.Up ),
                new Note( 26.0f, -1f, ArrowDir.Down ),
                new Note( 26.25f, -1f, ArrowDir.Left ),
                new Note( 26.5f, -1f, ArrowDir.Up ),
                new Note( 26.75f, -1f, ArrowDir.Right ),
                new Note( 27.0f, -1f, ArrowDir.Up ),
                new Note( 27.25f, -1f, ArrowDir.Down ),
                new Note( 27.5f, -1f, ArrowDir.Right ),
                new Note( 27.75f, -1f, ArrowDir.Left ),
                new Note( 28.0f, -1f, ArrowDir.Up ),
                new Note( 28.25f, -1f, ArrowDir.Right ),
                new Note( 28.5f, -1f, ArrowDir.Left ),
                new Note( 28.75f, -1f, ArrowDir.Down ),
                new Note( 29.25f, -1f, ArrowDir.Right ),
                new Note( 29.5f, -1f, ArrowDir.Down ),
                new Note( 29.75f, -1f, ArrowDir.Left ),
                new Note( 30.0f, -1f, ArrowDir.Up ),
                new Note( 30.25f, -1f, ArrowDir.Right ),
                new Note( 30.5f, -1f, ArrowDir.Down ),
                new Note( 30.75f, -1f, ArrowDir.Up ),

                new Note( 31.0f, -1f, ArrowDir.Left ),
                new Note( 31.25f, -1f, ArrowDir.Down ),
                new Note( 31.5f, -1f, ArrowDir.Up ),
                new Note( 31.75f, -1f, ArrowDir.Right ),
                new Note( 32.0f, -1f, ArrowDir.Up ),
                new Note( 32.75f, -1f, ArrowDir.Up ),
                new Note( 33.25f, -1f, ArrowDir.Up ),
                new Note( 33.5f, -1f, ArrowDir.Right ),
                new Note( 33.75f, -1f, ArrowDir.Down ),
                new Note( 34.0f, -1f, ArrowDir.Left ),
                new Note( 34.25f, -1f, ArrowDir.Right ),
                new Note( 34.5f, -1f, ArrowDir.Up ),
                new Note( 34.75f, -1f, ArrowDir.Down ),
                new Note( 35.25f, -1f, ArrowDir.Up ),
                new Note( 35.5f, -1f, ArrowDir.Left ),
                new Note( 35.75f, -1f, ArrowDir.Up ),

                new Note( 36.0f, -1f, ArrowDir.Right ),
                new Note( 36.75f, -1f, ArrowDir.Right ),
                new Note( 37.25f, -1f, ArrowDir.Right ),
                new Note( 37.5f, -1f, ArrowDir.Down ),
                new Note( 37.75f, -1f, ArrowDir.Left ),
                new Note( 38.0f, -1f, ArrowDir.Down ),
                new Note( 38.25f, -1f, ArrowDir.Left ),
                new Note( 38.5f, -1f, ArrowDir.Up ),
                new Note( 38.75f, -1f, ArrowDir.Right ),
                new Note( 39.25f, -1f, ArrowDir.Up ),
                new Note( 39.5f, -1f, ArrowDir.Left ),
                new Note( 39.75f, -1f, ArrowDir.Down ),

                new Note( 40.0f, -1f, ArrowDir.Down ),
                new Note( 40.75f, -1f, ArrowDir.Down ),
                new Note( 41.25f, -1f, ArrowDir.Down ),
                new Note( 41.5f, -1f, ArrowDir.Up ),
                new Note( 41.75f, -1f, ArrowDir.Down ),
                new Note( 42.0f, -1f, ArrowDir.Left ),
                new Note( 42.25f, -1f, ArrowDir.Down ),
                new Note( 42.5f, -1f, ArrowDir.Up ),
                new Note( 42.75f, -1f, ArrowDir.Right ),
                new Note( 43.0f, -1f, ArrowDir.Down ),
                new Note( 43.25f, -1f, ArrowDir.Up ),
                new Note( 43.5f, -1f, ArrowDir.Down ),
                new Note( 43.75f, -1f, ArrowDir.Left ),
                new Note( 44.0f, -1f, ArrowDir.Down ),
                new Note( 44.25f, -1f, ArrowDir.Up ),
                new Note( 44.5f, -1f, ArrowDir.Left ),
                new Note( 44.75f, -1f, ArrowDir.Right ),
                new Note( 45.25f, -1f, ArrowDir.Up ),
                new Note( 45.5f, -1f, ArrowDir.Down ),
                new Note( 45.75f, -1f, ArrowDir.Left ),
                new Note( 46.0f, -1f, ArrowDir.Up ),
                new Note( 46.25f, -1f, ArrowDir.Right ),
                new Note( 46.5f, -1f, ArrowDir.Left ),
                new Note( 46.75f, -1f, ArrowDir.Down ),

                new Note( 48.0f, 0.5f, ArrowDir.Right ),
                new Note( 48.75f, -1f, ArrowDir.Right ),
                new Note( 49.0f, -1f, ArrowDir.Up ),
                new Note( 49.25f, -1f, ArrowDir.Down ),
                new Note( 49.5f, 0.5f, ArrowDir.Left ),
                new Note( 50.25f, -1f, ArrowDir.Left ),
                new Note( 50.75f, -1f, ArrowDir.Left ),
                new Note( 51.0f, -1f, ArrowDir.Down ),
                new Note( 51.25f, -1f, ArrowDir.Right ),
                new Note( 51.5f, 0.5f, ArrowDir.Up ),
                new Note( 52.0f, 0.5f, ArrowDir.Down ),
                new Note( 52.75f, -1f, ArrowDir.Left ),
                new Note( 53.0f, -1f, ArrowDir.Down ),
                new Note( 53.25f, -1f, ArrowDir.Up ),
                new Note( 53.5f, 0.5f, ArrowDir.Right ),
                new Note( 54.25f, -1f, ArrowDir.Right ),
                new Note( 54.75f, -1f, ArrowDir.Right ),
                new Note( 55.0f, -1f, ArrowDir.Left ),
                new Note( 55.25f, -1f, ArrowDir.Down ),
                new Note( 55.5f, -1f, ArrowDir.Up ),
                new Note( 55.75f, -1f, ArrowDir.Right ),

                new Note( 56.0f, 0.5f, ArrowDir.Up ),
                new Note( 56.75f, -1f, ArrowDir.Up ),
                new Note( 57.0f, -1f, ArrowDir.Right ),
                new Note( 57.25f, -1f, ArrowDir.Up ),
                new Note( 57.5f, -1f, ArrowDir.Right ),
                new Note( 57.75f, 0.5f, ArrowDir.Down ),
                new Note( 58.25f, 0.5f, ArrowDir.Left ),
                new Note( 58.75f, -1f, ArrowDir.Up ),
                new Note( 59.0f, -1f, ArrowDir.Down ),
                new Note( 59.25f, -1f, ArrowDir.Left ),
                new Note( 59.5f, 0.5f, ArrowDir.Down ),
                new Note( 60.0f, 0.5f, ArrowDir.Up ),
                new Note( 60.5f, -1f, ArrowDir.Right ),
                new Note( 60.75f, 0.5f, ArrowDir.Up ),
                new Note( 61.25f, -1f, ArrowDir.Left ),
                new Note( 61.5f, 0.5f, ArrowDir.Down ),

                new Note( 62.25f, -1f, ArrowDir.Left ),
                new Note( 62.75f, -1f, ArrowDir.Right ),
                new Note( 63.0f, -1f, ArrowDir.Up ),
                new Note( 63.25f, -1f, ArrowDir.Left ),
                new Note( 63.5f, -1f, ArrowDir.Down ),
                new Note( 63.75f, -1f, ArrowDir.Right ),

                new Note( 64.0f, 0.5f, ArrowDir.Right ),
                new Note( 64.75f, -1f, ArrowDir.Right ),
                new Note( 65.0f, -1f, ArrowDir.Up ),
                new Note( 65.25f, -1f, ArrowDir.Down ),
                new Note( 65.5f, 0.5f, ArrowDir.Left ),
                new Note( 66.25f, -1f, ArrowDir.Left ),
                new Note( 66.75f, -1f, ArrowDir.Left ),
                new Note( 67.0f, -1f, ArrowDir.Down ),
                new Note( 67.25f, -1f, ArrowDir.Right ),
                new Note( 67.5f, 0.5f, ArrowDir.Up ),
                new Note( 68.0f, 0.5f, ArrowDir.Down ),
                new Note( 68.75f, -1f, ArrowDir.Left ),
                new Note( 69.0f, -1f, ArrowDir.Down ),
                new Note( 69.25f, -1f, ArrowDir.Up ),
                new Note( 69.5f, 0.5f, ArrowDir.Right ),
                new Note( 70.25f, -1f, ArrowDir.Right ),
                new Note( 70.75f, -1f, ArrowDir.Right ),
                new Note( 71.0f, -1f, ArrowDir.Left ),
                new Note( 71.25f, -1f, ArrowDir.Down ),
                new Note( 71.5f, -1f, ArrowDir.Up ),
                new Note( 71.75f, -1f, ArrowDir.Right ),

                new Note( 72.0f, 0.5f, ArrowDir.Up ),
                new Note( 72.75f, -1f, ArrowDir.Up ),
                new Note( 73.0f, -1f, ArrowDir.Right ),
                new Note( 73.25f, -1f, ArrowDir.Up ),
                new Note( 73.5f, -1f, ArrowDir.Right ),
                new Note( 73.75f, 0.5f, ArrowDir.Down ),
                new Note( 74.25f, 0.5f, ArrowDir.Left ),
                new Note( 74.75f, -1f, ArrowDir.Up ),
                new Note( 75.0f, -1f, ArrowDir.Down ),
                new Note( 75.25f, -1f, ArrowDir.Left ),
                new Note( 75.5f, 0.5f, ArrowDir.Down ),
                new Note( 76.0f, 0.5f, ArrowDir.Up ),
                new Note( 76.5f, -1f, ArrowDir.Right ),
                new Note( 76.75f, 0.5f, ArrowDir.Up ),
                new Note( 77.25f, 0.5f, ArrowDir.Left ),
                new Note( 77.75f, 0.5f, ArrowDir.Down ),
                new Note( 78.25f, 0.5f, ArrowDir.Up ),
                new Note( 78.75f, -1f, ArrowDir.Right ),
                new Note( 79.0f, 0.5f, ArrowDir.Up ),
                new Note( 79.5f, 0.5f, ArrowDir.Down ),

                new Note( 80.0f, -1f, ArrowDir.Left ),
                new Note( 80.75f, -1f, ArrowDir.Left ),
                new Note( 81.5f, -1f, ArrowDir.Right ),
                new Note( 82.0f, -1f, ArrowDir.Right ),

                new Note( 83.0f, -1f, ArrowDir.Left ),
                new Note( 83.25f, -1f, ArrowDir.Down ),
                new Note( 83.5f, -1f, ArrowDir.Right ),
                new Note( 83.75f, -1f, ArrowDir.Up ),

                new Note( 84.0f, -1f, ArrowDir.Right ),
                new Note( 84.25f, -1f, ArrowDir.Up ),
                new Note( 84.5f, -1f, ArrowDir.Down ),
                new Note( 84.75f, -1f, ArrowDir.Left ),
                new Note( 85.0f, 0.5f, ArrowDir.Down ),
                new Note( 85.5f, -1f, ArrowDir.Up ),
                new Note( 85.75f, 0.5f, ArrowDir.Right ),
                new Note( 86.25f, 0.5f, ArrowDir.Left ),
                new Note( 86.75f, -1f, ArrowDir.Down ),
                new Note( 87.0f, -1f, ArrowDir.Left ),
                new Note( 87.25f, -1f, ArrowDir.Down ),
                new Note( 87.5f, -1f, ArrowDir.Up ),
                new Note( 87.75f, -1f, ArrowDir.Right ),
                new Note( 88.0f, 0.5f, ArrowDir.Up ),
                new Note( 88.5f, -1f, ArrowDir.Down ),
                new Note( 88.75f, 0.5f, ArrowDir.Up ),
                new Note( 89.25f, -1f, ArrowDir.Down ),
                new Note( 89.5f, -1f, ArrowDir.Left ),
                new Note( 89.75f, 0.5f, ArrowDir.Up ),
                new Note( 90.25f, -1f, ArrowDir.Right ),
                new Note( 90.5f, -1f, ArrowDir.Down ),
                new Note( 90.75f, -1f, ArrowDir.Left ),
                new Note( 91.0f, -1f, ArrowDir.Down ),

                new Note( 91.5f, -1f, ArrowDir.Down ),
                new Note( 91.75f, -1f, ArrowDir.Left ),
                new Note( 92.0f, -1f, ArrowDir.Up ),
                new Note( 92.25f, -1f, ArrowDir.Right ),
                new Note( 92.5f, -1f, ArrowDir.Up ),
                new Note( 92.75f, -1f, ArrowDir.Down ),
                new Note( 93.0f, -1f, ArrowDir.Left ),

                new Note( 93.5f, -1f, ArrowDir.Up ),
                new Note( 93.75f, -1f, ArrowDir.Right ),
                new Note( 94.0f, -1f, ArrowDir.Up ),
                new Note( 94.25f, -1f, ArrowDir.Down ),
                new Note( 94.5f, -1f, ArrowDir.Left ),

                new Note( 95.0f, -1f, ArrowDir.Down ),
                new Note( 95.25f, -1f, ArrowDir.Left ),
                new Note( 95.5f, -1f, ArrowDir.Up ),
                new Note( 95.75f, -1f, ArrowDir.Right ),
                new Note( 96.0f, -1f, ArrowDir.Down ),
                new Note( 96.75f, -1f, ArrowDir.Left ),
                new Note( 97.5f, -1f, ArrowDir.Up ),
                new Note( 98.0f, -1f, ArrowDir.Down ),
                new Note( 98.75f, -1f, ArrowDir.Right ),
                new Note( 99.5f, -1f, ArrowDir.Up ),

                new Note( 100.0f, -1f, ArrowDir.Right ),
                new Note( 100.25f, -1f, ArrowDir.Up ),
                new Note( 100.5f, -1f, ArrowDir.Down ),
                new Note( 100.75f, -1f, ArrowDir.Left ),
                new Note( 101.0f, 0.5f, ArrowDir.Down ),
                new Note( 101.5f, -1f, ArrowDir.Up ),
                new Note( 101.75f, 0.5f, ArrowDir.Right ),
                new Note( 102.25f, 0.5f, ArrowDir.Left ),
                new Note( 102.75f, -1f, ArrowDir.Down ),
                new Note( 103.0f, -1f, ArrowDir.Left ),
                new Note( 103.25f, -1f, ArrowDir.Down ),
                new Note( 103.5f, -1f, ArrowDir.Up ),
                new Note( 103.75f, -1f, ArrowDir.Right ),
                new Note( 104.0f, 0.5f, ArrowDir.Left ),
                new Note( 104.75f, 0.5f, ArrowDir.Down ),
                new Note( 105.5f, 0.5f, ArrowDir.Right ),
                new Note( 106.0f, 0.5f, ArrowDir.Up ),
                new Note( 106.75f, 0.5f, ArrowDir.Down ),
                new Note( 107.5f, -1f, ArrowDir.Up ),
                new Note( 107.75f, -1f, ArrowDir.Down ),

                new Note( 108.0f, -1f, ArrowDir.Right ),
                new Note( 108.25f, -1f, ArrowDir.Up ),
                new Note( 108.5f, -1f, ArrowDir.Down ),
                new Note( 108.75f, -1f, ArrowDir.Left ),
                new Note( 109.0f, -1f, ArrowDir.Down ),
                new Note( 109.25f, -1f, ArrowDir.Left ),
                new Note( 109.5f, -1f, ArrowDir.Down ),
                new Note( 109.75f, -1f, ArrowDir.Up ),
                new Note( 110.0f, -1f, ArrowDir.Right ),
                new Note( 110.25f, -1f, ArrowDir.Up ),
                new Note( 110.5f, -1f, ArrowDir.Down ),
                new Note( 110.75f, -1f, ArrowDir.Left ),
                new Note( 111.0f, -1f, ArrowDir.Up ),
                new Note( 111.25f, -1f, ArrowDir.Down ),
                new Note( 111.5f, -1f, ArrowDir.Up ),
                new Note( 111.75f, -1f, ArrowDir.Right ),

                new Note( 112.0f, -1f, ArrowDir.Down ),
                new Note( 112.75f, -1f, ArrowDir.Up ),
                new Note( 113.5f, -1f, ArrowDir.Right ),
                new Note( 114.0f, -1f, ArrowDir.Left ),
                new Note( 115.0f, -1f, ArrowDir.Down ),
                new Note( 115.25f, -1f, ArrowDir.Up ),
                new Note( 115.5f, -1f, ArrowDir.Right ),
                new Note( 115.75f, -1f, ArrowDir.Up ),

                new Note( 116.0f, 0.5f, ArrowDir.Right ),
                new Note( 116.75f, 0.5f, ArrowDir.Down ),
                new Note( 117.5f, 0.5f, ArrowDir.Up ),
                new Note( 118.25f, 0.5f, ArrowDir.Left ),
                new Note( 119.0f, -1f, ArrowDir.Down ),
                new Note( 119.5f, -1f, ArrowDir.Right ),
                new Note( 120.0f, 0.5f, ArrowDir.Up ),
                new Note( 120.75f, 0.5f, ArrowDir.Left ),
                new Note( 121.5f, 0.5f, ArrowDir.Up ),
                new Note( 122.25f, 0.5f, ArrowDir.Down ),
                new Note( 123.0f, -1f, ArrowDir.Left ),
                new Note( 123.5f, -1f, ArrowDir.Down ),

                new Note( 124.0f, 0.5f, ArrowDir.Left ),
                new Note( 124.75f, 0.5f, ArrowDir.Right ),
                new Note( 125.5f, 0.5f, ArrowDir.Down ),
                new Note( 126.25f, 0.5f, ArrowDir.Up ),
                new Note( 127.0f, -1f, ArrowDir.Right ),
                new Note( 127.25f, -1f, ArrowDir.Up ),
                new Note( 127.5f, -1f, ArrowDir.Left ),
                new Note( 127.75f, -1f, ArrowDir.Down ),
                new Note( 128.0f, 2f, ArrowDir.Right ),

                new Note( 130.0f, -1f, ArrowDir.Up ),
                new Note( 130.75f, -1f, ArrowDir.Up ),
            });
            #endregion

            #region Prairie4�@CPU
            cpuNotes.AddRange(new Note[]{
                new Note( 0.0f, 0.5f, ArrowDir.Up ),
                new Note( 0.75f, -1f, ArrowDir.Up ),
                new Note( 1.0f, -1f, ArrowDir.Left ),
                new Note( 1.25f, -1f, ArrowDir.Right ),
                new Note( 1.5f, -1f, ArrowDir.Down ),
                new Note( 2.25f, -1f, ArrowDir.Right ),
                new Note( 2.75f, -1f, ArrowDir.Right ),
                new Note( 3.0f, -1f, ArrowDir.Down ),
                new Note( 3.25f, -1f, ArrowDir.Right ),
                new Note( 3.5f, -1f, ArrowDir.Up ),
                new Note( 3.75f, -1f, ArrowDir.Left ),

                new Note( 4.0f, 0.5f, ArrowDir.Right ),
                new Note( 4.75f, -1f, ArrowDir.Down ),
                new Note( 5.0f, -1f, ArrowDir.Right ),
                new Note( 5.25f, -1f, ArrowDir.Up ),
                new Note( 5.5f, -1f, ArrowDir.Left ),
                new Note( 6.25f, -1f, ArrowDir.Left ),
                new Note( 6.75f, -1f, ArrowDir.Left ),
                new Note( 7.0f, -1f, ArrowDir.Right ),
                new Note( 7.25f, -1f, ArrowDir.Down ),
                new Note( 7.5f, -1f, ArrowDir.Up ),
                new Note( 7.75f, -1f, ArrowDir.Left ),

                new Note( 8.0f, 0.5f, ArrowDir.Up ),
                new Note( 8.75f, -1f, ArrowDir.Up ),
                new Note( 9.0f, -1f, ArrowDir.Left ),
                new Note( 9.25f, -1f, ArrowDir.Down ),
                new Note( 9.5f, -1f, ArrowDir.Up ),
                new Note( 9.75f, -1f, ArrowDir.Left ),
                new Note( 10.25f, -1f, ArrowDir.Left ),
                new Note( 10.75f, -1f, ArrowDir.Left ),
                new Note( 11.0f, -1f, ArrowDir.Up ),
                new Note( 11.25f, -1f, ArrowDir.Down ),
                new Note( 11.5f, -1f, ArrowDir.Right ),
                new Note( 11.75f, -1f, ArrowDir.Down ),
                new Note( 12.0f, -1f, ArrowDir.Right ),
                new Note( 12.5f, -1f, ArrowDir.Left ),
                new Note( 12.75f, -1f, ArrowDir.Up ),
                new Note( 13.25f, -1f, ArrowDir.Down ),
                new Note( 13.5f, -1f, ArrowDir.Up ),

                new Note( 15.0f, -1f, ArrowDir.Right ),
                new Note( 15.25f, -1f, ArrowDir.Down ),
                new Note( 15.5f, -1f, ArrowDir.Up ),
                new Note( 15.75f, -1f, ArrowDir.Left ),
                new Note( 16.0f, -1f, ArrowDir.Down ),
                new Note( 16.75f, -1f, ArrowDir.Down ),
                new Note( 17.25f, -1f, ArrowDir.Down ),
                new Note( 17.5f, -1f, ArrowDir.Up ),
                new Note( 17.75f, -1f, ArrowDir.Down ),
                new Note( 18.0f, -1f, ArrowDir.Right ),
                new Note( 18.25f, -1f, ArrowDir.Left ),
                new Note( 18.5f, -1f, ArrowDir.Up ),
                new Note( 18.75f, -1f, ArrowDir.Down ),
                new Note( 19.25f, -1f, ArrowDir.Up ),
                new Note( 19.5f, -1f, ArrowDir.Right ),
                new Note( 19.75f, -1f, ArrowDir.Up ),

                new Note( 20.0f, -1f, ArrowDir.Left ),
                new Note( 20.75f, -1f, ArrowDir.Left ),
                new Note( 21.25f, -1f, ArrowDir.Left ),
                new Note( 21.5f, -1f, ArrowDir.Up ),
                new Note( 21.75f, -1f, ArrowDir.Down ),
                new Note( 22.0f, -1f, ArrowDir.Right ),
                new Note( 22.25f, -1f, ArrowDir.Down ),
                new Note( 22.5f, -1f, ArrowDir.Up ),
                new Note( 22.75f, -1f, ArrowDir.Left ),
                new Note( 23.25f, -1f, ArrowDir.Down ),
                new Note( 23.5f, -1f, ArrowDir.Right ),
                new Note( 23.75f, -1f, ArrowDir.Up ),

                new Note( 24.0f, -1f, ArrowDir.Left ),
                new Note( 24.75f, -1f, ArrowDir.Left ),
                new Note( 25.25f, -1f, ArrowDir.Left ),
                new Note( 25.5f, -1f, ArrowDir.Down ),
                new Note( 25.75f, -1f, ArrowDir.Up ),
                new Note( 26.0f, -1f, ArrowDir.Down ),
                new Note( 26.25f, -1f, ArrowDir.Right ),
                new Note( 26.5f, -1f, ArrowDir.Up ),
                new Note( 26.75f, -1f, ArrowDir.Left ),
                new Note( 27.0f, -1f, ArrowDir.Up ),
                new Note( 27.25f, -1f, ArrowDir.Down ),
                new Note( 27.5f, -1f, ArrowDir.Left ),
                new Note( 27.75f, -1f, ArrowDir.Right ),
                new Note( 28.0f, -1f, ArrowDir.Up ),
                new Note( 28.25f, -1f, ArrowDir.Left ),
                new Note( 28.5f, -1f, ArrowDir.Right ),
                new Note( 28.75f, -1f, ArrowDir.Down ),
                new Note( 29.25f, -1f, ArrowDir.Left ),
                new Note( 29.5f, -1f, ArrowDir.Down ),
                new Note( 29.75f, -1f, ArrowDir.Right ),
                new Note( 30.0f, -1f, ArrowDir.Up ),
                new Note( 30.25f, -1f, ArrowDir.Left ),
                new Note( 30.5f, -1f, ArrowDir.Down ),
                new Note( 30.75f, -1f, ArrowDir.Up ),

                new Note( 31.0f, -1f, ArrowDir.Right ),
                new Note( 31.25f, -1f, ArrowDir.Down ),
                new Note( 31.5f, -1f, ArrowDir.Up ),
                new Note( 31.75f, -1f, ArrowDir.Left ),
                new Note( 32.0f, -1f, ArrowDir.Up ),
                new Note( 32.75f, -1f, ArrowDir.Up ),
                new Note( 33.25f, -1f, ArrowDir.Up ),
                new Note( 33.5f, -1f, ArrowDir.Left ),
                new Note( 33.75f, -1f, ArrowDir.Down ),
                new Note( 34.0f, -1f, ArrowDir.Right ),
                new Note( 34.25f, -1f, ArrowDir.Left ),
                new Note( 34.5f, -1f, ArrowDir.Up ),
                new Note( 34.75f, -1f, ArrowDir.Down ),
                new Note( 35.25f, -1f, ArrowDir.Up ),
                new Note( 35.5f, -1f, ArrowDir.Right ),
                new Note( 35.75f, -1f, ArrowDir.Up ),

                new Note( 36.0f, -1f, ArrowDir.Left ),
                new Note( 36.75f, -1f, ArrowDir.Left ),
                new Note( 37.25f, -1f, ArrowDir.Left ),
                new Note( 37.5f, -1f, ArrowDir.Down ),
                new Note( 37.75f, -1f, ArrowDir.Right ),
                new Note( 38.0f, -1f, ArrowDir.Down ),
                new Note( 38.25f, -1f, ArrowDir.Right ),
                new Note( 38.5f, -1f, ArrowDir.Up ),
                new Note( 38.75f, -1f, ArrowDir.Left ),
                new Note( 39.25f, -1f, ArrowDir.Up ),
                new Note( 39.5f, -1f, ArrowDir.Right ),
                new Note( 39.75f, -1f, ArrowDir.Down ),

                new Note( 40.0f, -1f, ArrowDir.Down ),
                new Note( 40.75f, -1f, ArrowDir.Down ),
                new Note( 41.25f, -1f, ArrowDir.Down ),
                new Note( 41.5f, -1f, ArrowDir.Up ),
                new Note( 41.75f, -1f, ArrowDir.Down ),
                new Note( 42.0f, -1f, ArrowDir.Right ),
                new Note( 42.25f, -1f, ArrowDir.Down ),
                new Note( 42.5f, -1f, ArrowDir.Up ),
                new Note( 42.75f, -1f, ArrowDir.Left ),
                new Note( 43.0f, -1f, ArrowDir.Down ),
                new Note( 43.25f, -1f, ArrowDir.Up ),
                new Note( 43.5f, -1f, ArrowDir.Down ),
                new Note( 43.75f, -1f, ArrowDir.Right ),
                new Note( 44.0f, -1f, ArrowDir.Down ),
                new Note( 44.25f, -1f, ArrowDir.Up ),
                new Note( 44.5f, -1f, ArrowDir.Right ),
                new Note( 44.75f, -1f, ArrowDir.Left ),
                new Note( 45.25f, -1f, ArrowDir.Up ),
                new Note( 45.5f, -1f, ArrowDir.Down ),
                new Note( 45.75f, -1f, ArrowDir.Right ),
                new Note( 46.0f, -1f, ArrowDir.Up ),
                new Note( 46.25f, -1f, ArrowDir.Left ),
                new Note( 46.5f, -1f, ArrowDir.Right ),
                new Note( 46.75f, -1f, ArrowDir.Down ),

                new Note( 48.0f, 0.5f, ArrowDir.Left ),
                new Note( 48.75f, -1f, ArrowDir.Left ),
                new Note( 49.0f, -1f, ArrowDir.Up ),
                new Note( 49.25f, -1f, ArrowDir.Down ),
                new Note( 49.5f, 0.5f, ArrowDir.Right ),
                new Note( 50.25f, -1f, ArrowDir.Right ),
                new Note( 50.75f, -1f, ArrowDir.Right ),
                new Note( 51.0f, -1f, ArrowDir.Down ),
                new Note( 51.25f, -1f, ArrowDir.Left ),
                new Note( 51.5f, 0.5f, ArrowDir.Up ),
                new Note( 52.0f, 0.5f, ArrowDir.Down ),
                new Note( 52.75f, -1f, ArrowDir.Right ),
                new Note( 53.0f, -1f, ArrowDir.Down ),
                new Note( 53.25f, -1f, ArrowDir.Up ),
                new Note( 53.5f, 0.5f, ArrowDir.Left ),
                new Note( 54.25f, -1f, ArrowDir.Left ),
                new Note( 54.75f, -1f, ArrowDir.Left ),
                new Note( 55.0f, -1f, ArrowDir.Right ),
                new Note( 55.25f, -1f, ArrowDir.Down ),
                new Note( 55.5f, -1f, ArrowDir.Up ),
                new Note( 55.75f, -1f, ArrowDir.Left ),

                new Note( 56.0f, 0.5f, ArrowDir.Up ),
                new Note( 56.75f, -1f, ArrowDir.Up ),
                new Note( 57.0f, -1f, ArrowDir.Left ),
                new Note( 57.25f, -1f, ArrowDir.Up ),
                new Note( 57.5f, -1f, ArrowDir.Left ),
                new Note( 57.75f, 0.5f, ArrowDir.Down ),
                new Note( 58.25f, 0.5f, ArrowDir.Right ),
                new Note( 58.75f, -1f, ArrowDir.Up ),
                new Note( 59.0f, -1f, ArrowDir.Down ),
                new Note( 59.25f, -1f, ArrowDir.Right ),
                new Note( 59.5f, 0.5f, ArrowDir.Down ),
                new Note( 60.0f, 0.5f, ArrowDir.Up ),
                new Note( 60.5f, -1f, ArrowDir.Left ),
                new Note( 60.75f, 0.5f, ArrowDir.Up ),
                new Note( 61.25f, -1f, ArrowDir.Right ),
                new Note( 61.5f, 0.5f, ArrowDir.Down ),

                new Note( 62.25f, -1f, ArrowDir.Right ),
                new Note( 62.75f, -1f, ArrowDir.Left ),
                new Note( 63.0f, -1f, ArrowDir.Up ),
                new Note( 63.25f, -1f, ArrowDir.Right ),
                new Note( 63.5f, -1f, ArrowDir.Down ),
                new Note( 63.75f, -1f, ArrowDir.Left ),

                new Note( 64.0f, 0.5f, ArrowDir.Left ),
                new Note( 64.75f, -1f, ArrowDir.Left ),
                new Note( 65.0f, -1f, ArrowDir.Up ),
                new Note( 65.25f, -1f, ArrowDir.Down ),
                new Note( 65.5f, 0.5f, ArrowDir.Right ),
                new Note( 66.25f, -1f, ArrowDir.Right ),
                new Note( 66.75f, -1f, ArrowDir.Right ),
                new Note( 67.0f, -1f, ArrowDir.Down ),
                new Note( 67.25f, -1f, ArrowDir.Left ),
                new Note( 67.5f, 0.5f, ArrowDir.Up ),
                new Note( 68.0f, 0.5f, ArrowDir.Down ),
                new Note( 68.75f, -1f, ArrowDir.Right ),
                new Note( 69.0f, -1f, ArrowDir.Down ),
                new Note( 69.25f, -1f, ArrowDir.Up ),
                new Note( 69.5f, 0.5f, ArrowDir.Left ),
                new Note( 70.25f, -1f, ArrowDir.Left ),
                new Note( 70.75f, -1f, ArrowDir.Left ),
                new Note( 71.0f, -1f, ArrowDir.Right ),
                new Note( 71.25f, -1f, ArrowDir.Down ),
                new Note( 71.5f, -1f, ArrowDir.Up ),
                new Note( 71.75f, -1f, ArrowDir.Left ),

                new Note( 72.0f, 0.5f, ArrowDir.Up ),
                new Note( 72.75f, -1f, ArrowDir.Up ),
                new Note( 73.0f, -1f, ArrowDir.Left ),
                new Note( 73.25f, -1f, ArrowDir.Up ),
                new Note( 73.5f, -1f, ArrowDir.Left ),
                new Note( 73.75f, 0.5f, ArrowDir.Down ),
                new Note( 74.25f, 0.5f, ArrowDir.Right ),
                new Note( 74.75f, -1f, ArrowDir.Up ),
                new Note( 75.0f, -1f, ArrowDir.Down ),
                new Note( 75.25f, -1f, ArrowDir.Right ),
                new Note( 75.5f, 0.5f, ArrowDir.Down ),
                new Note( 76.0f, 0.5f, ArrowDir.Up ),
                new Note( 76.5f, -1f, ArrowDir.Left ),
                new Note( 76.75f, 0.5f, ArrowDir.Up ),
                new Note( 77.25f, 0.5f, ArrowDir.Right ),
                new Note( 77.75f, 0.5f, ArrowDir.Down ),
                new Note( 78.25f, 0.5f, ArrowDir.Up ),
                new Note( 78.75f, -1f, ArrowDir.Left ),
                new Note( 79.0f, 0.5f, ArrowDir.Up ),
                new Note( 79.5f, 0.5f, ArrowDir.Down ),

                new Note( 80.0f, -1f, ArrowDir.Right ),
                new Note( 80.75f, -1f, ArrowDir.Right ),
                new Note( 81.5f, -1f, ArrowDir.Left ),
                new Note( 82.0f, -1f, ArrowDir.Left ),

                new Note( 83.0f, -1f, ArrowDir.Right ),
                new Note( 83.25f, -1f, ArrowDir.Down ),
                new Note( 83.5f, -1f, ArrowDir.Left ),
                new Note( 83.75f, -1f, ArrowDir.Up ),

                new Note( 84.0f, -1f, ArrowDir.Left ),
                new Note( 84.25f, -1f, ArrowDir.Up ),
                new Note( 84.5f, -1f, ArrowDir.Down ),
                new Note( 84.75f, -1f, ArrowDir.Right ),
                new Note( 85.0f, 0.5f, ArrowDir.Down ),
                new Note( 85.5f, -1f, ArrowDir.Up ),
                new Note( 85.75f, 0.5f, ArrowDir.Left ),
                new Note( 86.25f, 0.5f, ArrowDir.Right ),
                new Note( 86.75f, -1f, ArrowDir.Down ),
                new Note( 87.0f, -1f, ArrowDir.Right ),
                new Note( 87.25f, -1f, ArrowDir.Down ),
                new Note( 87.5f, -1f, ArrowDir.Up ),
                new Note( 87.75f, -1f, ArrowDir.Left ),
                new Note( 88.0f, 0.5f, ArrowDir.Up ),
                new Note( 88.5f, -1f, ArrowDir.Down ),
                new Note( 88.75f, 0.5f, ArrowDir.Up ),
                new Note( 89.25f, -1f, ArrowDir.Down ),
                new Note( 89.5f, -1f, ArrowDir.Right ),
                new Note( 89.75f, 0.5f, ArrowDir.Up ),
                new Note( 90.25f, -1f, ArrowDir.Left ),
                new Note( 90.5f, -1f, ArrowDir.Down ),
                new Note( 90.75f, -1f, ArrowDir.Right ),
                new Note( 91.0f, -1f, ArrowDir.Down ),

                new Note( 91.5f, -1f, ArrowDir.Down ),
                new Note( 91.75f, -1f, ArrowDir.Right ),
                new Note( 92.0f, -1f, ArrowDir.Up ),
                new Note( 92.25f, -1f, ArrowDir.Left ),
                new Note( 92.5f, -1f, ArrowDir.Up ),
                new Note( 92.75f, -1f, ArrowDir.Down ),
                new Note( 93.0f, -1f, ArrowDir.Right ),

                new Note( 93.5f, -1f, ArrowDir.Up ),
                new Note( 93.75f, -1f, ArrowDir.Left ),
                new Note( 94.0f, -1f, ArrowDir.Up ),
                new Note( 94.25f, -1f, ArrowDir.Down ),
                new Note( 94.5f, -1f, ArrowDir.Right ),

                new Note( 95.0f, -1f, ArrowDir.Down ),
                new Note( 95.25f, -1f, ArrowDir.Right ),
                new Note( 95.5f, -1f, ArrowDir.Up ),
                new Note( 95.75f, -1f, ArrowDir.Left ),
                new Note( 96.0f, -1f, ArrowDir.Down ),
                new Note( 96.75f, -1f, ArrowDir.Right ),
                new Note( 97.5f, -1f, ArrowDir.Up ),
                new Note( 98.0f, -1f, ArrowDir.Down ),
                new Note( 98.75f, -1f, ArrowDir.Left ),
                new Note( 99.5f, -1f, ArrowDir.Up ),

                new Note( 100.0f, -1f, ArrowDir.Left ),
                new Note( 100.25f, -1f, ArrowDir.Up ),
                new Note( 100.5f, -1f, ArrowDir.Down ),
                new Note( 100.75f, -1f, ArrowDir.Right ),
                new Note( 101.0f, 0.5f, ArrowDir.Down ),
                new Note( 101.5f, -1f, ArrowDir.Up ),
                new Note( 101.75f, 0.5f, ArrowDir.Left ),
                new Note( 102.25f, 0.5f, ArrowDir.Right ),
                new Note( 102.75f, -1f, ArrowDir.Down ),
                new Note( 103.0f, -1f, ArrowDir.Right ),
                new Note( 103.25f, -1f, ArrowDir.Down ),
                new Note( 103.5f, -1f, ArrowDir.Up ),
                new Note( 103.75f, -1f, ArrowDir.Left ),
                new Note( 104.0f, 0.5f, ArrowDir.Right ),
                new Note( 104.75f, 0.5f, ArrowDir.Down ),
                new Note( 105.5f, 0.5f, ArrowDir.Left ),
                new Note( 106.0f, 0.5f, ArrowDir.Up ),
                new Note( 106.75f, 0.5f, ArrowDir.Down ),
                new Note( 107.5f, -1f, ArrowDir.Up ),
                new Note( 107.75f, -1f, ArrowDir.Down ),

                new Note( 108.0f, -1f, ArrowDir.Left ),
                new Note( 108.25f, -1f, ArrowDir.Up ),
                new Note( 108.5f, -1f, ArrowDir.Down ),
                new Note( 108.75f, -1f, ArrowDir.Right ),
                new Note( 109.0f, -1f, ArrowDir.Down ),
                new Note( 109.25f, -1f, ArrowDir.Right ),
                new Note( 109.5f, -1f, ArrowDir.Down ),
                new Note( 109.75f, -1f, ArrowDir.Up ),
                new Note( 110.0f, -1f, ArrowDir.Left ),
                new Note( 110.25f, -1f, ArrowDir.Up ),
                new Note( 110.5f, -1f, ArrowDir.Down ),
                new Note( 110.75f, -1f, ArrowDir.Right ),
                new Note( 111.0f, -1f, ArrowDir.Up ),
                new Note( 111.25f, -1f, ArrowDir.Down ),
                new Note( 111.5f, -1f, ArrowDir.Up ),
                new Note( 111.75f, -1f, ArrowDir.Left ),

                new Note( 112.0f, -1f, ArrowDir.Down ),
                new Note( 112.75f, -1f, ArrowDir.Up ),
                new Note( 113.5f, -1f, ArrowDir.Left ),
                new Note( 114.0f, -1f, ArrowDir.Right ),
                new Note( 115.0f, -1f, ArrowDir.Down ),
                new Note( 115.25f, -1f, ArrowDir.Up ),
                new Note( 115.5f, -1f, ArrowDir.Left ),
                new Note( 115.75f, -1f, ArrowDir.Up ),

                new Note( 116.0f, 0.5f, ArrowDir.Left ),
                new Note( 116.75f, 0.5f, ArrowDir.Down ),
                new Note( 117.5f, 0.5f, ArrowDir.Up ),
                new Note( 118.25f, 0.5f, ArrowDir.Right ),
                new Note( 119.0f, -1f, ArrowDir.Down ),
                new Note( 119.5f, -1f, ArrowDir.Left ),
                new Note( 120.0f, 0.5f, ArrowDir.Up ),
                new Note( 120.75f, 0.5f, ArrowDir.Right ),
                new Note( 121.5f, 0.5f, ArrowDir.Up ),
                new Note( 122.25f, 0.5f, ArrowDir.Down ),
                new Note( 123.0f, -1f, ArrowDir.Right ),
                new Note( 123.5f, -1f, ArrowDir.Down ),

                new Note( 124.0f, 0.5f, ArrowDir.Right ),
                new Note( 124.75f, 0.5f, ArrowDir.Left ),
                new Note( 125.5f, 0.5f, ArrowDir.Down ),
                new Note( 126.25f, 0.5f, ArrowDir.Up ),
                new Note( 127.0f, -1f, ArrowDir.Left ),
                new Note( 127.25f, -1f, ArrowDir.Up ),
                new Note( 127.5f, -1f, ArrowDir.Right ),
                new Note( 127.75f, -1f, ArrowDir.Down ),
                new Note( 128.0f, 2f, ArrowDir.Left ),

                new Note( 130.0f, -1f, ArrowDir.Up ),
                new Note( 130.75f, -1f, ArrowDir.Up ),
            });
            #endregion
        }
    }

    #endregion

    #endregion
}
