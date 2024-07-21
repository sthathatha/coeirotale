using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

/// <summary>
/// MANA�ΐ�X�s�[�h
/// </summary>
public class MANAGameSystemA : GameSceneScriptBase
{
    #region �萔

    /// <summary>���������@��ɏo�����x</summary>
    private const float MOVE_TIME_START = 1f;
    /// <summary>�v���C���@��ɏo�����x</summary>
    private const float MOVE_TIME_PLAY = 0.2f;

    /// <summary>�J�[�h�\���D�揇��</summary>
    private const int BASE_PRIORITY = 100;

    /// <summary>CPU�v���C�Ԋu</summary>
    private const float CPU_PLAY_TIME = 1f;

    /// <summary>B�Q�[�����̊Ԋu�Ɋ|���Z</summary>
    private const float CPU_TIME_RATE_B = 0.8f;

    #endregion

    #region �����o�[

    public GameObject modeA;

    public Transform posP1;
    public Transform posP2;
    public Transform posP3;
    public Transform posP4;
    public Transform posYamaP;

    public Transform posE1;
    public Transform posE2;
    public Transform posE3;
    public Transform posE4;
    public Transform posYamaE;

    public Transform posField1;
    public Transform posField2;

    public Transform cursor;

    public GameObject dummyCard;
    public Transform cardParent;

    public MANAGameMessage message;

    public AudioClip cardMoveSE;

    #endregion

    #region �v���C�x�[�g

    /// <summary>�J�[�h���</summary>
    private struct CardParam
    {
        public MANAGameCard.Suit suit;
        public int num;
        public CardParam(MANAGameCard.Suit s, int n) { suit = s; num = n; }
    }
    /// <summary>�v���C���[�R�D���</summary>
    private List<CardParam> playerYamaList = new List<CardParam>();
    /// <summary>�G�R�D���</summary>
    private List<CardParam> enemyYamaList = new List<CardParam>();

    /// <summary>�v���C���[�R�D��ԉ��̃J�[�h</summary>
    private MANAGameCard pYamaLastCard;
    /// <summary>�v���C���[��D�P</summary>
    private MANAGameCard pHandCard1;
    /// <summary>�v���C���[��D�Q</summary>
    private MANAGameCard pHandCard2;
    /// <summary>�v���C���[��D�R</summary>
    private MANAGameCard pHandCard3;
    /// <summary>�v���C���[��D�S</summary>
    private MANAGameCard pHandCard4;
    /// <summary>�G�R�D��ԉ��̃J�[�h</summary>
    private MANAGameCard eYamaLastCard;
    /// <summary>�G��D�P</summary>
    private MANAGameCard eHandCard1;
    /// <summary>�G��D�Q</summary>
    private MANAGameCard eHandCard2;
    /// <summary>�G��D�R</summary>
    private MANAGameCard eHandCard3;
    /// <summary>�G��D�S</summary>
    private MANAGameCard eHandCard4;
    /// <summary>��̎D�P</summary>
    private MANAGameCard field1Card;
    /// <summary>��̎D�Q</summary>
    private MANAGameCard field2Card;

    private MANAGameCard field1CardTmp = null;
    private MANAGameCard field2CardTmp = null;

    private enum MANAState : int
    {
        Initialize = 0,
        Start,
        StartCheck,
        Play,
        End,
    }
    /// <summary>MANA�Q�[���̏��</summary>
    private MANAState state = MANAState.Initialize;

    /// <summary>�J�[�\���ʒu</summary>
    private int playerCursor = 1;

    /// <summary>CPU�̓���Ԋu</summary>
    private float cpuPlayWait = 0f;

    /// <summary>�r������p�I�u�W�F�N�g</summary>
    private object lockObj = new object();

    #endregion

    #region ���

    /// <summary>
    /// ������
    /// </summary>
    /// <returns></returns>
    override public IEnumerator Start()
    {
        state = MANAState.Initialize;
        message.SetText("");

        UpdateCursor();

        modeA.SetActive(true);
        cpuPlayWait = CPU_PLAY_TIME;
        if (GetLoseCount() > 3)
        {
            cpuPlayWait += (GetLoseCount() - 3) * 0.1f;
        }
        if (IsBossRush())
        {
            cpuPlayWait *= CPU_TIME_RATE_B;
        }

        // �R�D�����_���쐬
        var pRandom = Util.RandomUniqueIntList(1, 26, 26);
        foreach (var c in pRandom)
        {
            var suit = c > 13 ? MANAGameCard.Suit.Spade : MANAGameCard.Suit.Crub;
            var num = c > 13 ? c - 13 : c;
            if (IsBossRush())
                enemyYamaList.Add(new CardParam(suit, num));
            else
                playerYamaList.Add(new CardParam(suit, num));
        }

        var eRandom = Util.RandomUniqueIntList(1, 26, 26);
        foreach (var c in eRandom)
        {
            var suit = c > 13 ? MANAGameCard.Suit.Heart : MANAGameCard.Suit.Dia;
            var num = c > 13 ? c - 13 : c;
            if (IsBossRush())
                playerYamaList.Add(new CardParam(suit, num));
            else
                enemyYamaList.Add(new CardParam(suit, num));
        }

        // �R�DLast�\��
        var pLast = GameObject.Instantiate(dummyCard, cardParent);
        var eLast = GameObject.Instantiate(dummyCard, cardParent);
        pLast.SetActive(true);
        eLast.SetActive(true);
        pYamaLastCard = pLast.GetComponent<MANAGameCard>();
        eYamaLastCard = eLast.GetComponent<MANAGameCard>();
        pYamaLastCard.MoveTo(posYamaP.localPosition);
        eYamaLastCard.MoveTo(posYamaE.localPosition);
        pYamaLastCard.ShowCard(false);
        eYamaLastCard.ShowCard(false);
        pYamaLastCard.SetPriority(BASE_PRIORITY);
        eYamaLastCard.SetPriority(BASE_PRIORITY);

        yield return base.Start();
    }

    /// <summary>
    /// �t�F�[�h�C����
    /// </summary>
    /// <returns></returns>
    public override IEnumerator AfterFadeIn()
    {
        yield return base.AfterFadeIn();
        yield return new WaitForSeconds(0.5f);

        // ��D�S���z��
        for (var i = 4; i > 0; i--)
        {
            StartCoroutine(GetHandCoroutine(true, i, false));
            StartCoroutine(GetHandCoroutine(false, i, false));
            yield return new WaitForSeconds(MOVE_TIME_PLAY);
        }
        yield return new WaitForSeconds(1f);

        // �`���[�g���A���\��
        var tutorial = ManagerSceneScript.GetInstance().GetMinigameTutorialWindow();
        var input = InputManager.GetInstance();
        if (IsBossRush())
        {
            tutorial.SetTitle(StringMinigameMessage.ManaB_Title);
            tutorial.SetText(StringMinigameMessage.ManaB_Tutorial);
        }
        else
        {
            tutorial.SetTitle(StringMinigameMessage.ManaA_Title);
            tutorial.SetText(StringMinigameMessage.ManaA_Tutorial);
        }
        yield return tutorial.Open();
        yield return new WaitUntil(() => input.GetKeyPress(InputManager.Keys.South));
        yield return tutorial.Close();

        StartCoroutine(InitFieldCoroutine());
        StartCoroutine(CpuPlayCoroutine());
    }

    /// <summary>
    /// �t���[������
    /// </summary>
    override public void Update()
    {
        base.Update();

        if (ManagerSceneScript.GetInstance().SceneState != ManagerSceneScript.State.Game) { return; }
        if (state == MANAState.End) { return; }

        var input = InputManager.GetInstance();

        // ���E�J�[�\���ړ��͂��ł�
        if (input.GetKeyPress(InputManager.Keys.Right) || input.GetKeyPress(InputManager.Keys.Left))
        {
            var move = input.GetKeyPress(InputManager.Keys.Left) ? 1 : -1;
            playerCursor += move;
            if (playerCursor <= 0) playerCursor = 4;
            else if (playerCursor > 4) playerCursor = 1;

            UpdateCursor();
        }

        // �J�[�h�o�������Play��
        if (state == MANAState.Play)
        {
            if (input.GetKeyPress(InputManager.Keys.South))
            {
                StartCoroutine(CardPlayCoroutine(true, playerCursor));
            }
        }
    }

    #endregion

    #region �R���[�`��

    /// <summary>
    /// CPU�v���C
    /// </summary>
    /// <returns></returns>
    private IEnumerator CpuPlayCoroutine()
    {
        var timer = new DeltaFloat();
        timer.Set(0);
        while (true)
        {
            // �v���C���ȊO�̓v���C�܂ő҂�
            if (state != MANAState.Play)
            {
                yield return new WaitUntil(() => state == MANAState.Play);
                timer.MoveTo(0, cpuPlayWait, DeltaFloat.MoveType.LINE);
            }

            if (timer.IsActive())
            {
                timer.Update(Time.deltaTime);
                yield return null;
                continue;
            }

            // �o���J�[�h�I��
            var playIndex = -1;
            if (CanPlayCard(eHandCard1))
            {
                playIndex = 1;
            }
            else if (CanPlayCard(eHandCard2))
            {
                playIndex = 2;
            }
            else if (CanPlayCard(eHandCard3))
            {
                playIndex = 3;
            }
            else if (CanPlayCard(eHandCard4))
            {
                playIndex = 4;
            }

            if (playIndex > 0)
            {
                // �o��
                yield return CardPlayCoroutine(false, playIndex);
                timer.MoveTo(0, cpuPlayWait, DeltaFloat.MoveType.LINE);
            }
            else
            {
                // �o����̂��������炿����Ƒ҂�
                timer.MoveTo(0, cpuPlayWait / 2f, DeltaFloat.MoveType.LINE);
            }
        }
    }

    /// <summary>
    /// ��ɂQ���o��
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>
    private IEnumerator InitFieldCoroutine(float delay = -1f)
    {
        lock (lockObj)
        {
            if (state == MANAState.Start) { yield break; }
            state = MANAState.Start;
        }

        if (delay > 0f)
        {
            yield return new WaitForSeconds(delay);
        }

        // �R�D�܂��͎�D��I��
        MANAGameCard pCard = null;
        if (playerYamaList.Count > 0)
        {
            pCard = PickUpYamaCard(true);
        }
        else if (pHandCard1 != null)
        {
            pCard = pHandCard1;
            pHandCard1 = null;
        }
        else if (pHandCard2 != null)
        {
            pCard = pHandCard2;
            pHandCard2 = null;
        }
        else if (pHandCard3 != null)
        {
            pCard = pHandCard3;
            pHandCard3 = null;
        }
        else if (pHandCard4 != null)
        {
            pCard = pHandCard4;
            pHandCard4 = null;
        }
        MANAGameCard eCard = null;
        if (enemyYamaList.Count > 0)
        {
            eCard = PickUpYamaCard(false);
        }
        else if (eHandCard1 != null)
        {
            eCard = eHandCard1;
            eHandCard1 = null;
        }
        else if (eHandCard2 != null)
        {
            eCard = eHandCard2;
            eHandCard2 = null;
        }
        else if (eHandCard3 != null)
        {
            eCard = eHandCard3;
            eHandCard3 = null;
        }
        else if (eHandCard4 != null)
        {
            eCard = eHandCard4;
            eHandCard4 = null;
        }

        // �t�B�[���h�P�ƂQ�Ɉړ�
        pCard.MoveTo(posField1.localPosition, MOVE_TIME_START);
        eCard.MoveTo(posField2.localPosition, MOVE_TIME_START);
        pCard.SetPriority(BASE_PRIORITY + 10);
        eCard.SetPriority(BASE_PRIORITY + 20);
        ManagerSceneScript.GetInstance().soundMan.PlaySE(cardMoveSE);
        yield return new WaitWhile(() => pCard.IsMoving() || eCard.IsMoving());
        pCard.SetPriority(BASE_PRIORITY);
        eCard.SetPriority(BASE_PRIORITY);

        // �t�B�[���h�ɑO�̃J�[�h�������������
        if (field1Card != null)
        {
            Destroy(field1Card.gameObject);
        }
        if (field2Card != null)
        {
            Destroy(field2Card.gameObject);
        }
        field1Card = pCard;
        field2Card = eCard;

        // ���b�Z�[�W�\��
        message.SetText(StringMinigameMessage.ManaA_Ready);
        message.SetAlpha(1);
        yield return new WaitForSeconds(1f);
        message.SetAlpha(0, 1f);
        message.SetText(StringMinigameMessage.ManaA_Go);

        field1Card.ShowCard(true);
        field2Card.ShowCard(true);

        state = MANAState.StartCheck;
        CheckGame();
    }

    /// <summary>
    /// �R�D�����ɂP���ړ�
    /// </summary>
    /// <param name="isPlayer"></param>
    /// <param name="index"></param>
    /// <param name="check"></param>
    /// <returns></returns>
    private IEnumerator GetHandCoroutine(bool isPlayer, int index = -1, bool check = true)
    {
        var card = PickUpYamaCard(isPlayer);
        // �R������΂Ȃɂ����Ȃ�
        if (card == null)
        {
            yield break;
        }

        // �ꏊ�I��
        Vector3 newPos;
        if (isPlayer)
        {
            if (index < 0)
            {
                if (pHandCard1 == null) index = 1;
                else if (pHandCard2 == null) index = 2;
                else if (pHandCard3 == null) index = 3;
                else if (pHandCard4 == null) index = 4;
            }

            newPos = index switch
            {
                1 => posP1.localPosition,
                2 => posP2.localPosition,
                3 => posP3.localPosition,
                _ => posP4.localPosition
            };
        }
        else
        {
            if (index < 0)
            {
                if (eHandCard1 == null) index = 1;
                else if (eHandCard2 == null) index = 2;
                else if (eHandCard3 == null) index = 3;
                else if (eHandCard4 == null) index = 4;
            }

            newPos = index switch
            {
                1 => posE1.localPosition,
                2 => posE2.localPosition,
                3 => posE3.localPosition,
                _ => posE4.localPosition
            };
        }

        // �����CheckGame��null�����ɂȂ��Ă��܂����߁A���n�߂̏u�Ԃ��玝���Ă邱�Ƃɂ���
        switch (isPlayer, index)
        {
            case (true, 1): pHandCard1 = card; break;
            case (true, 2): pHandCard2 = card; break;
            case (true, 3): pHandCard3 = card; break;
            case (true, 4): pHandCard4 = card; break;
            case (false, 1): eHandCard1 = card; break;
            case (false, 2): eHandCard2 = card; break;
            case (false, 3): eHandCard3 = card; break;
            case (false, 4): eHandCard4 = card; break;
        }

        // �ړ�
        card.MoveTo(newPos, MOVE_TIME_PLAY);
        ManagerSceneScript.GetInstance().soundMan.PlaySE(cardMoveSE);
        yield return new WaitWhile(() => card.IsMoving());

        // �\�ɂ��Ď�D�ɐݒ�
        card.ShowCard(true);
        card.SetPriority(BASE_PRIORITY);

        if (check)
        {
            CheckGame();
        }
    }

    /// <summary>
    /// �J�[�h���o������
    /// </summary>
    /// <param name="isPlayer"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    private IEnumerator CardPlayCoroutine(bool isPlayer, int index)
    {
        var sound = ManagerSceneScript.GetInstance().soundMan;

        // �o���J�[�h�̃N���X
        MANAGameCard playCard;
        playCard = (isPlayer, index) switch
        {
            (true, 1) => pHandCard1,
            (true, 2) => pHandCard2,
            (true, 3) => pHandCard3,
            (true, 4) => pHandCard4,
            (false, 1) => eHandCard1,
            (false, 2) => eHandCard2,
            (false, 3) => eHandCard3,
            _ => eHandCard4,
        };

        // �o���J�[�h���������E�������̎��̓G���[
        if (playCard == null || playCard.IsMoving())
        {
            if (isPlayer)
            {
                sound.PlaySE(sound.commonSeError);
            }
            yield break;
        }

        // �o�����I��
        var target = 0;
        var f1TmpNum = field1CardTmp?.GetNum() ?? 0;
        var f2TmpNum = field2CardTmp?.GetNum() ?? 0;
        var f1Num = field1Card.GetNum();
        var f2Num = field2Card.GetNum();
        var playNum = playCard.GetNum();
        if (f1TmpNum == 0 && CalcCardDiff(f1Num, playNum) == 1)
        {
            // �P���t���[�ŏo����
            target = 1;
        }
        else if (f2TmpNum == 0 && CalcCardDiff(f2Num, playNum) == 1)
        {
            // �Q���t���[�ŏo����
            target = 2;
        }
        else if (f1TmpNum != 0 && CalcCardDiff(f1TmpNum, playNum) == 1)
        {
            // �P�ɏo�Ă��Ă鏊�ɏo����
            target = 1;
        }
        else if (f2TmpNum != 0 && CalcCardDiff(f2TmpNum, playNum) == 1)
        {
            // �Q�ɏo�Ă��Ă鏊�ɏo����
            target = 2;
        }

        // �o���Ȃ��ꍇ
        if (target == 0)
        {
            if (f1TmpNum != 0 && CalcCardDiff(f1Num, playNum) == 1)
            {
                // �P�ɐ�ɏo���ꂻ���i���������j
                target = 1;
            }
            else if (f2TmpNum != 0 && CalcCardDiff(f2Num, playNum) == 1)
            {
                // �Q�ɐ�ɏo���ꂻ��
                target = 2;
            }
        }

        // ����ȊO�͋߂��ꏊ��
        if (target == 0)
        {
            target = index > 2 ? 2 : 1;
        }

        // �J�[�h����ɓ�����
        var tmp = target == 1 ? field1CardTmp : field2CardTmp;
        playCard.SetPriority(tmp == null ? BASE_PRIORITY + 10 : tmp.GetPriority() + 2);
        playCard.MoveTo(target == 1 ? posField1.localPosition : posField2.localPosition, MOVE_TIME_PLAY);
        if (target == 1)
        {
            field1CardTmp = playCard;
        }
        else
        {
            field2CardTmp = playCard;
        }
        sound.PlaySE(cardMoveSE);
        yield return new WaitWhile(() => playCard.IsMoving());

        // tmp���N���A
        if (target == 1 && field1CardTmp == playCard)
        {
            field1CardTmp = null;
        }
        else if (target == 2 && field2CardTmp == playCard)
        {
            field2CardTmp = null;
        }

        // 
        var targetCard = target == 1 ? field1Card : field2Card;
        var nowDiff = CalcCardDiff(targetCard.GetNum(), playCard.GetNum());
        if (nowDiff == 1)
        {
            // ���̃J�[�h������
            Destroy(targetCard.gameObject);
            if (target == 1)
            {
                field1Card = playCard;
            }
            else
            {
                field2Card = playCard;
            }
            playCard.SetPriority(BASE_PRIORITY);

            // �o������D����ɂ��ĎR�������
            switch (isPlayer, index)
            {
                case (true, 1): pHandCard1 = null; break;
                case (true, 2): pHandCard2 = null; break;
                case (true, 3): pHandCard3 = null; break;
                case (true, 4): pHandCard4 = null; break;
                case (false, 1): eHandCard1 = null; break;
                case (false, 2): eHandCard2 = null; break;
                case (false, 3): eHandCard3 = null; break;
                case (false, 4): eHandCard4 = null; break;
            }

            if (isPlayer && playerYamaList.Count == 0 ||
                isPlayer == false && enemyYamaList.Count == 0)
            {
                CheckGame();
            }
            else
            {
                StartCoroutine(GetHandCoroutine(isPlayer, index));
            }
        }
        else
        {
            // ���Ă��Ō��̈ʒu�ɖ߂�
            var backPos = (isPlayer, index) switch
            {
                (true, 1) => posP1,
                (true, 2) => posP2,
                (true, 3) => posP3,
                (true, 4) => posP4,
                (false, 1) => posE1,
                (false, 2) => posE2,
                (false, 3) => posE3,
                _ => posE4,
            };
            playCard.SetPriority(BASE_PRIORITY + 2 * index);
            playCard.MoveTo(backPos.localPosition, MOVE_TIME_PLAY * 5);
            yield return new WaitWhile(() => playCard.IsMoving());
            playCard.SetPriority(BASE_PRIORITY);

            CheckGame();
        }
    }

    /// <summary>
    /// �����E�s�k�ŏI������
    /// </summary>
    /// <returns></returns>
    private IEnumerator GameEndCoroutine()
    {
        var end = false;
        if (pHandCard1 == null &&
            pHandCard2 == null &&
            pHandCard3 == null &&
            pHandCard4 == null)
        {
            // ����
            SetGameResult(true);
            if (IsBossRush())
            {
                Global.GetTemporaryData().bossRushManaWon = true;
            }
            yield return new WaitForSeconds(1f);

            message.SetText(StringMinigameMessage.ManaA_Win);
            message.SetAlpha(1f);
            end = true;
        }
        else if (eHandCard1 == null &&
            eHandCard2 == null &&
            eHandCard3 == null &&
            eHandCard4 == null)
        {
            // ����
            SetGameResult(false);
            if (IsBossRush())
            {
                Global.GetTemporaryData().bossRushManaWon = false;
            }
            yield return new WaitForSeconds(1f);

            message.SetText(StringMinigameMessage.ManaA_Lose);
            message.SetAlpha(1f);
            end = true;
        }

        if (end)
        {
            yield return new WaitForSeconds(2f);

            if (IsBossRush())
            {
                ManagerSceneScript.GetInstance().NextGame("GameSceneMenderuB");
            }
            else
            {
                ManagerSceneScript.GetInstance().ExitGame();
            }
        }
    }

    #endregion

    #region ���\�b�h

    /// <summary>
    /// �R�D�̈�ԏ�̃J�[�h�����
    /// </summary>
    /// <param name="isPlayer"></param>
    /// <returns></returns>
    private MANAGameCard PickUpYamaCard(bool isPlayer)
    {
        MANAGameCard ret;
        if (isPlayer)
        {
            if (playerYamaList.Count == 0) { return null; }

            else if (playerYamaList.Count == 1)
            {
                ret = pYamaLastCard;
            }
            else
            {
                var newCard = GameObject.Instantiate(dummyCard, cardParent);
                newCard.SetActive(true);
                ret = newCard.GetComponent<MANAGameCard>();
            }
            ret.SetCard(playerYamaList.First().suit, playerYamaList.First().num);
            ret.ShowCard(false);
            ret.MoveTo(posYamaP.localPosition);

            playerYamaList.RemoveAt(0);
        }
        else
        {
            if (enemyYamaList.Count == 1)
            {
                ret = eYamaLastCard;
            }
            else
            {
                var newCard = GameObject.Instantiate(dummyCard, cardParent);
                newCard.SetActive(true);
                ret = newCard.GetComponent<MANAGameCard>();
            }
            ret.SetCard(enemyYamaList.First().suit, enemyYamaList.First().num);
            ret.ShowCard(false);
            ret.MoveTo(posYamaE.localPosition);

            enemyYamaList.RemoveAt(0);
        }

        ret.SetPriority(BASE_PRIORITY + 20);
        return ret;
    }

    /// <summary>
    /// �J�[�\���ʒu�X�V
    /// </summary>
    private void UpdateCursor()
    {
        var x = playerCursor switch
        {
            1 => posP1.localPosition.x,
            2 => posP2.localPosition.x,
            3 => posP3.localPosition.x,
            _ => posP4.localPosition.x,
        };

        var pos = cursor.localPosition;
        pos.x = x;
        cursor.localPosition = pos;
    }

    /// <summary>
    /// �J�[�h�̍����v�Z
    /// </summary>
    /// <param name="num1"></param>
    /// <param name="num2"></param>
    /// <returns></returns>
    private int CalcCardDiff(int num1, int num2)
    {
        var diff = Mathf.Abs(num1 - num2);
        if (diff >= 7) diff = 13 - diff;

        return diff;
    }

    /// <summary>
    /// �J�[�h���o���邩�ǂ�������
    /// �����Ă���͔̂F�����Ȃ�
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    private bool CanPlayCard(MANAGameCard c)
    {
        if (c == null || c.IsMoving()) return false;

        var f1Num = field1Card?.GetNum();
        var f2Num = field2Card?.GetNum();

        if (f1Num.HasValue)
        {
            var diff = CalcCardDiff(f1Num.Value, c.GetNum());
            if (diff == 1) return true;
        }
        if (f2Num.HasValue)
        {
            var diff = CalcCardDiff(f2Num.Value, c.GetNum());
            if (diff == 1) return true;
        }

        return false;
    }

    /// <summary>
    /// �Q�[����Ԕ���
    /// </summary>
    /// <returns></returns>
    private bool CheckGame()
    {
        // �I�����Ȃ��������
        if (state == MANAState.End) return false;

        // ��D����Ȃ�I��
        if (pHandCard1 == null &&
            pHandCard2 == null &&
            pHandCard3 == null &&
            pHandCard4 == null ||
            eHandCard1 == null &&
            eHandCard2 == null &&
            eHandCard3 == null &&
            eHandCard4 == null)
        {
            state = MANAState.End;
            StartCoroutine(GameEndCoroutine());
            return false;
        }

        // �c���Ă�ꍇ�����̎�D���`�F�b�N
        MANAGameCard[] cards =
        {
            pHandCard1, pHandCard2, pHandCard3, pHandCard4,
            eHandCard1, eHandCard2, eHandCard3, eHandCard4,
        };

        foreach (var card in cards)
        {
            // �����Ă��牽�����Ȃ�
            if (card?.IsMoving() == true)
            {
                state = MANAState.Play;
                return true;
            }
        }

        // �o���邩����
        foreach (var card in cards)
        {
            if (CanPlayCard(card))
            {
                state = MANAState.Play;
                return true;
            }
        }

        // �o����̂��������͎R�D����o��
        StartCoroutine(InitFieldCoroutine(1f));
        return false;
    }
    #endregion
}
