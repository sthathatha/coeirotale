using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// �����f���Q�[���P
/// </summary>
public class MenderuGameSystem1 : GameSceneScriptBase
{
    #region �萔
    /// <summary>1�^�[���Ɏ��鐔</summary>
    private const int TURN_PICK_LIMIT = 3;

    /// <summary>�v���C���[�J�[�\���F</summary>
    private readonly Color COLOR_PLAYER = Color.cyan;
    /// <summary>�G�J�[�\���F</summary>
    private readonly Color COLOR_ENEMY = Color.magenta;

    /// <summary>
    /// �Z���t����҂��^�C�v
    /// </summary>
    private enum TalkWaitType : int
    {
        None = 0,
        Button,
        Time,
    }
    #endregion

    #region �����o�[
    /// <summary>�P��ڐe�I�u�W�F�N�g</summary>
    public GameObject battle1Parent = null;
    /// <summary>�Q��ڐe�I�u�W�F�N�g</summary>
    public GameObject battle2Parent = null;

    /// <summary>�����o��</summary>
    public GameObject talkWindow = null;
    /// <summary>�Z���t</summary>
    public TMP_Text talkMessage = null;

    /// <summary>�����f����</summary>
    public Animator menderuMouth = null;

    /// <summary>���_��</summary>
    public TMP_Text pointL = null;
    /// <summary>���_�E</summary>
    public TMP_Text pointR = null;

    /// <summary>�J�[�\��</summary>
    public GameObject cursorMain = null;
    /// <summary>�I���{�^���J�[�\��</summary>
    public GameObject cursorEnd = null;

    /// <summary>��I�u�W�F�N�g�̐e</summary>
    public GameObject seeds = null;

    /// <summary>�J�[�\���ړ�SE</summary>
    public AudioClip se_cursor_move = null;
    /// <summary>��擾SE</summary>
    public AudioClip se_get = null;
    /// <summary>�^�[���G���hSE</summary>
    public AudioClip se_end = null;
    #endregion

    #region �v���C�x�[�g
    private Dictionary<int, Dictionary<int, SeedScript>> seedScripts;

    /// <summary>�Q�[���i�s�R���[�`��</summary>
    private Coroutine gameCoroutine = null;

    /// <summary>�J�[�\���ʒuX</summary>
    private int cursorRow = 2;
    /// <summary>�J�[�\���ʒuY</summary>
    private int cursorCol = 2;

    /// <summary>�G�|�C���g</summary>
    private int pointLNum = 0;
    /// <summary>�����|�C���g</summary>
    private int pointRNum = 0;

    /// <summary>���̃^�[���Ɏ������</summary>
    private int pickCount = 0;
    /// <summary>�G����郊�X�g</summary>
    private List<Vector2Int> enemyPickList = new List<Vector2Int>();

    /// <summary>���̓����~�߂�^�C�}�[�p</summary>
    private DeltaFloat mouthTimer = new DeltaFloat();
    #endregion

    #region ��ꏈ��
    /// <summary>
    /// ������
    /// </summary>
    override public IEnumerator Start()
    {
        yield return base.Start();

        seedScripts = new Dictionary<int, Dictionary<int, SeedScript>>();
        for (int row = 0; row < 5; ++row)
        {
            var rowDictionary = new Dictionary<int, SeedScript>();
            for (int col = 0; col < 5; ++col)
            {
                var s = seeds.transform.Find($"seed{row}_{col}");
                rowDictionary.Add(col, s.GetComponent<SeedScript>());
            }
            seedScripts[row] = rowDictionary;
        }

        //���b�Z�[�W��\��
        talkWindow.SetActive(false);
        talkMessage.SetText("");

        //todo:�t���O�ɂ��
        if (true)
        {
            battle1Parent.SetActive(true);
            battle2Parent.SetActive(false);
        }
        else
        {
            //battle1Parent.SetActive(false);
            //battle2Parent.SetActive(true);
        }

        // 
        cursorMain.GetComponent<SpriteRenderer>().color = COLOR_PLAYER;
        cursorEnd.GetComponent<SpriteRenderer>().color = COLOR_PLAYER;
        UpdateCursorLocation();
    }

    /// <summary>
    /// �t�F�[�h�C����̏�������
    /// </summary>
    /// <returns></returns>
    public override IEnumerator AfterFadeIn()
    {
        var input = InputManager.GetInstance();
        var tutorial = ManagerSceneScript.GetInstance().GetMinigameTutorialWindow();

        yield return base.AfterFadeIn();

        yield return MenderuTalk("�u�����f���̎�v�ŏ�����I");
        MenderuTalkStop();
        yield return null;

        // �`���[�g���A���\��
        tutorial.SetTitle("�����f���̎�");
        tutorial.SetText($"25�̎�����݂�1�`{TURN_PICK_LIMIT}��荇���܂��B\n" +
            $"��̓^�[�����ڂ鎞�ɏ㉺���E�ɂ���󗓂̐������������A{SeedScript.SEED_MAX_NUM}�܂Ő�������Ǝ��Ȃ��Ȃ�܂��B\n" +
            $"�����̃^�[����1�����Ȃ������ꍇ�A�����ƂȂ�܂��B");
        yield return tutorial.Open();
        yield return new WaitUntil(() => input.GetKeyPress(InputManager.Keys.South));
        yield return tutorial.Close();

        // ��U
        yield return PlayerTurnStartCoroutine(true);
    }

    /// <summary>
    /// ����
    /// </summary>
    override public void Update()
    {
        base.Update();
        mouthTimer.Update(Time.deltaTime);

        if (ManagerSceneScript.GetInstance().SceneState != ManagerSceneScript.State.Game) { return; }
        if (gameCoroutine != null) { return; }

        if (!mouthTimer.IsActive())
        {
            menderuMouth.SetBool("talking", false);
        }

        var sound = ManagerSceneScript.GetInstance().SoundManager;
        var input = InputManager.GetInstance();

        if (input.GetKeyPress(InputManager.Keys.Up))
        {
            if (cursorCol > 4) { return; }
            // ��
            if (cursorRow <= 0)
            {
                cursorRow = 4;
            }
            else
            {
                --cursorRow;
            }
            sound.PlaySE(se_cursor_move);
            UpdateCursorLocation();
        }
        else if (input.GetKeyPress(InputManager.Keys.Down))
        {
            if (cursorCol > 4) { return; }
            // ��
            if (cursorRow >= 4)
            {
                cursorRow = 0;
            }
            else
            {
                ++cursorRow;
            }
            sound.PlaySE(se_cursor_move);
            UpdateCursorLocation();
        }
        else if (input.GetKeyPress(InputManager.Keys.Right))
        {
            // �E
            if (cursorCol >= 5)
            {
                cursorCol = 0;
                cursorRow = 2;
            }
            else
            {
                ++cursorCol;
            }
            sound.PlaySE(se_cursor_move);
            UpdateCursorLocation();
        }
        else if (input.GetKeyPress(InputManager.Keys.Left))
        {
            // ��
            if (cursorCol >= 5)
            {
                cursorCol = 4;
                cursorRow = 2;
            }
            else if (cursorCol <= 0)
            {
                cursorCol = 5;
            }
            else
            {
                --cursorCol;
            }
            sound.PlaySE(se_cursor_move);
            UpdateCursorLocation();
        }
        else if (input.GetKeyPress(InputManager.Keys.East))
        {
            // �I���{�^���Ɉړ�
            cursorRow = 2;
            cursorCol = 5;
            sound.PlaySE(se_cursor_move);
            UpdateCursorLocation();
        }
        else if (input.GetKeyPress(InputManager.Keys.South))
        {
            // ���݈ʒu�I���̃R���[�`��
            gameCoroutine = StartCoroutine(SelectCoroutine());
        }
    }
    #endregion

    /// <summary>
    /// �J�[�\���\���X�V
    /// </summary>
    private void UpdateCursorLocation()
    {
        if (cursorCol >= 5)
        {
            cursorEnd.SetActive(true);
            cursorMain.SetActive(false);
        }
        else
        {
            cursorEnd.SetActive(false);
            cursorMain.SetActive(true);

            cursorMain.transform.localPosition = new Vector3(
                (cursorCol - 2) * 100,
                (cursorRow - 2) * (-100),
                0);
        }
    }

    /// <summary>
    /// �J�[�\���ʒu�̑I�����鏈��
    /// </summary>
    /// <returns></returns>
    private IEnumerator SelectCoroutine()
    {
        var manager = ManagerSceneScript.GetInstance();
        var sound = manager.SoundManager;

        if (cursorCol >= 5)
        {
            // �^�[���I���{�^��
            if (pickCount == 0)
            {
                //todo: 1������ĂȂ��ꍇ�u�u�[
            }
            else
            {
                sound.PlaySE(se_end);
                yield return EnemyTurnCoroutine();
                if (manager.SceneState != ManagerSceneScript.State.Game) yield break;
            }
        }
        else
        {
            // �I���ʒu�̎���Ƃ�
            var seed = seedScripts[cursorRow][cursorCol];
            if (seed.IsEnable())
            {
                sound.PlaySE(se_get);
                pointRNum += seed.GetNum();
                pointR.SetText(pointRNum.ToString());
                seed.Pick();

                pickCount++;
                if (pickCount >= TURN_PICK_LIMIT)
                {
                    // ����������^�[���I��
                    yield return EnemyTurnCoroutine();
                    if (manager.SceneState != ManagerSceneScript.State.Game) yield break;
                }
            }
            else
            {
                //todo: ���Ȃ��ꍇ�u�u�[
            }
        }

        yield return null;
        gameCoroutine = null;
    }

    /// <summary>
    /// �����f�����鏈��
    /// </summary>
    /// <param name="text"></param>
    /// <param name="waitType"></param>
    /// <param name="waitTime"></param>
    private IEnumerator MenderuTalk(string text, TalkWaitType waitType = TalkWaitType.Button, float waitTime = 1f)
    {
        talkWindow.SetActive(true);
        talkMessage.SetText(text);
        menderuMouth.SetBool("talking", true);

        if (waitType == TalkWaitType.Button)
        {
            yield return new WaitUntil(() => InputManager.GetInstance().GetKeyPress(InputManager.Keys.South));
            yield return null;
        }
        else if (waitType == TalkWaitType.Time)
        {
            yield return new WaitForSeconds(waitTime);
        }
    }

    /// <summary>
    /// �����߂鏈��
    /// </summary>
    private void MenderuTalkStop()
    {
        talkWindow.SetActive(false);
        menderuMouth.SetBool("talking", false);
    }

    /// <summary>
    /// �G�^�[���̃R���[�`��
    /// </summary>
    /// <returns></returns>
    private IEnumerator EnemyTurnCoroutine()
    {
        var sound = ManagerSceneScript.GetInstance().SoundManager;

        GrowUp();
        yield return new WaitForSeconds(1f);

        cursorMain.GetComponent<SpriteRenderer>().color = COLOR_ENEMY;
        cursorEnd.GetComponent<SpriteRenderer>().color = COLOR_ENEMY;
        CreateEnemyPickList();
        yield return MenderuTalk("���ꂶ�Ⴀ�@���́c", TalkWaitType.Time, 2f);

        if (enemyPickList.Count == 0)
        {
            // ���Ȃ��̂ŏ���
            yield return MenderuTalk("�ȁA�Ȃ�ł����āI", TalkWaitType.Time);
            yield return MenderuTalk("����킪�ЂƂ���������Ȃ��I", TalkWaitType.Time);
            yield return MenderuTalk("���̕����ˁc����������I", TalkWaitType.Time);
            ManagerSceneScript.GetInstance().ExitGame();
            yield break;
        }

        foreach (var pickLoc in enemyPickList)
        {
            cursorRow = pickLoc.x;
            cursorCol = pickLoc.y;
            UpdateCursorLocation();

            var pickScr = seedScripts[pickLoc.x][pickLoc.y];
            pointLNum += pickScr.GetNum();
            pointL.SetText(pointLNum.ToString());
            pickScr.Pick();

            sound.PlaySE(se_get);
            if (pickLoc == enemyPickList.Last())
            {
                yield return MenderuTalk("����ˁI", TalkWaitType.Time);
            }
            else
            {
                yield return MenderuTalk("����Ɓc", TalkWaitType.Time);
            }
        }

        yield return PlayerTurnStartCoroutine();
    }

    /// <summary>
    /// �v���C���[�^�[���J�n���R���[�`��
    /// </summary>
    /// <param name="init">��������</param>
    /// <returns></returns>
    private IEnumerator PlayerTurnStartCoroutine(bool init = false)
    {
        var input = InputManager.GetInstance();
        if (!init)
        {
            GrowUp();
            yield return new WaitForSeconds(1f);

            var enableList = GetEnableList();
            if (enableList.Count == 0)
            {
                //���Ȃ��̂Ŕs�k
                yield return MenderuTalk("����A��������킪������", TalkWaitType.Time);
                yield return MenderuTalk("���̏����ˁA�܂��V�т܂��傤", TalkWaitType.Time);
                ManagerSceneScript.GetInstance().ExitGame();
                yield break;
            }
        }

        cursorMain.GetComponent<SpriteRenderer>().color = COLOR_PLAYER;
        cursorEnd.GetComponent<SpriteRenderer>().color = COLOR_PLAYER;
        pickCount = 0;
        yield return MenderuTalk("�����A���Ȃ��̔Ԃ�", TalkWaitType.None);
        mouthTimer.Set(0);
        mouthTimer.MoveTo(1f, 2f, DeltaFloat.MoveType.LINE);
    }

    #region �ʏ탁�\�b�h
    /// <summary>
    /// �G������𔻒�
    /// </summary>
    private void CreateEnemyPickList()
    {
        enemyPickList.Clear();
        // �L���Ȏ�s�b�N�A�b�v
        var enableSeeds = GetEnableList();

        // 3�ȉ��Ȃ�S���Ƃ�
        if (enableSeeds.Count <= TURN_PICK_LIMIT)
        {
            enemyPickList.AddRange(enableSeeds);
            return;
        }

        // 4�ȏ�̏ꍇ��AI

        // �m�����X�g
        var out0List = new List<Vector2Int>();
        // ����1�ŕm�����X�g
        var out1List = new Dictionary<Vector2Int, List<Vector2Int>>();
        // ����2�ŕm�����X�g
        var out2List = new Dictionary<Vector2Int, List<Vector2Int>>();
        // ����3�ŕm�����X�g
        var out3List = new Dictionary<Vector2Int, List<Vector2Int>>();
        // ����ȊO�̃��X�g
        var elseList = new List<Vector2Int>();
        // ��ԑ�������
        var maxNum = -1;
        var maxNumList = new List<Vector2Int>();
        // �`�F�b�N
        foreach (var loc in enableSeeds)
        {
            var scr = GetLocationSeedScript(loc.x, loc.y);
            var yuuyo = SeedScript.SEED_MAX_NUM - scr.GetNum();
            var growNum = CalcGrowNum(loc.x, loc.y);

            if (yuuyo <= growNum)
            {
                // �m���ł���
                out0List.Add(loc);
            }
            else
            {
                var crossList = GetCrossVector(loc);
                if (crossList.Count >= yuuyo - growNum)
                {
                    // �m���ɂł���\��������
                    switch (yuuyo - growNum)
                    {
                        case 1: out1List.Add(loc, crossList); break;
                        case 2: out2List.Add(loc, crossList); break;
                        default: out3List.Add(loc, crossList); break;
                    }
                }
                else
                {
                    // �܂��܂�����
                    elseList.Add(loc);
                }
            }

            // �ő�̐�����O�̂��߃��X�g�A�b�v
            if (scr.GetNum() > maxNum)
            {
                maxNum = scr.GetNum();
                maxNumList.Clear();
                maxNumList.Add(loc);
            }
            else if (scr.GetNum() == maxNum)
            {
                maxNumList.Add(loc);
            }
        }

        // �s�̏ꍇ�p�@����3��2��1��0��maxList�̃��X�g���珇�ɂ����������_���ɑI��
        Action Random3Pick = () =>
        {
            var count = Util.RandomInt(1, TURN_PICK_LIMIT);

            for(int i=0; i<count; ++i)
            {
                if (out3List.Count > 0)
                {
                    var idx = Util.RandomInt(0, out3List.Count - 1);
                    var v = out3List.Select(p => p.Key).ElementAt(idx);
                    enemyPickList.Add(v);
                    out3List.Remove(v);
                }else if(out2List.Count > 0)
                {
                    var idx = Util.RandomInt(0, out2List.Count - 1);
                    var v = out2List.Select(p => p.Key).ElementAt(idx);
                    enemyPickList.Add(v);
                    out2List.Remove(v);
                }
                else if(out1List.Count > 0)
                {
                    var idx = Util.RandomInt(0, out1List.Count - 1);
                    var v = out1List.Select(p => p.Key).ElementAt(idx);
                    enemyPickList.Add(v);
                    out1List.Remove(v);
                }
                else if (out0List.Count > 0)
                {
                    var idx = Util.RandomInt(0, out0List.Count - 1);
                    var v = out0List[idx];
                    enemyPickList.Add(v);
                    out0List.Remove(v);
                }
                else
                {
                    if (enemyPickList.Count > 0) break;
                    // �ǂ����Ă������Ȃ�ő�̂�1��
                    enemyPickList.Add(maxNumList[Util.RandomInt(0, maxNumList.Count-1)]);
                }
            }
        };

        // �s�̏ꍇ�pAI
        Action NormalPick = () =>
        {
            // �]�T���镨��4�̔{���c���悤�ɂ���
            var elsePickCount = elseList.Count % (TURN_PICK_LIMIT + 1);
            // ���ł�4�̔{���Ȃ�
            if (elsePickCount == 0) { Random3Pick(); }

            List<int> pickList = Util.RandomUniqueIntList(0, elseList.Count - 1, elsePickCount);
            foreach(var pindex in pickList)
            {
                enemyPickList.Add(elseList[pindex]);
            }
        };

        if (elseList.Count > TURN_PICK_LIMIT)
        {
            // �m���ɂł��Ȃ����̂���������
            NormalPick();
            return;
        }

        var wouldPickList = new List<Vector2Int>();
        var tmp1List = out1List;
        var tmp2List = out2List;
        var tmp3List = out3List;
        // elseList�͊m���Ɏ��K�v����
        foreach (var pick in elseList)
        {
            wouldPickList.Add(pick);
            PickRemoveFromList(ref tmp1List, ref tmp2List, ref tmp3List, pick);
        }

        // ���̎��_�Ŋ������Ă�
        if (tmp1List.Count == 0 && tmp2List.Count == 0 && tmp3List.Count == 0)
        {
            enemyPickList.AddRange(wouldPickList);
            if (enemyPickList.Count == 0)
            {
                NormalPick();
            }
            return;
        }

        // �c���Ă鎞�_�łR�g���Ďc���Ă��疳���Ƃ킩��
        if (wouldPickList.Count == 3)
        {
            NormalPick();
            return;
        }

        // �d�݂����X�g���쐬
        var kohoList = new Dictionary<Vector2Int, int>();
        Action<Vector2Int, int> KohoAdd = (v, add) =>
        {
            if (kohoList.ContainsKey(v)) kohoList[v] += add;
            else kohoList[v] = add;
        };

        // tmp3�͍ő�1���������͂������T�O�̂���foreach�ŋL�q����
        foreach (var tmp3 in tmp3List)
        {
            foreach (var t in tmp3.Value)
            {
                KohoAdd(t, 1);
            }
            KohoAdd(tmp3.Key, 3);
        }
        foreach (var tmp2 in tmp2List)
        {
            foreach (var t in tmp2.Value)
            {
                KohoAdd(t, 1);
            }
            KohoAdd(tmp2.Key, 2);
        }
        foreach (var tmp1 in tmp1List)
        {
            foreach (var t in tmp1.Value)
            {
                KohoAdd(t, 1);
            }
            KohoAdd(tmp1.Key, 1);
        }
        // �d�݂̑傫����
        var sortedKoho = kohoList.OrderBy(k => k.Value).Reverse().Select(k => k.Key).ToList();
        // �ǉ��őI�ׂ鐔
        var pickableCount = TURN_PICK_LIMIT - wouldPickList.Count;

        // �\�Ȃ�����T��
        var coveringList = CalcCoveringList(sortedKoho, 0, pickableCount, tmp1List, tmp2List, tmp3List);
        if (coveringList == null)
        {
            // ������Ȃ��Ȃ疳��
            NormalPick();
            return;
        }

        // ����������Z�b�g���Ċ���
        enemyPickList.AddRange(wouldPickList);
        enemyPickList.AddRange(coveringList);
    }

    /// <summary>
    /// out1�`out3��ԗ��ł��郊�X�g���擾
    /// </summary>
    /// <param name="list">��⃊�X�g</param>
    /// <param name="startIndex">�����J�n�C���f�b�N�X</param>
    /// <param name="canSelectNum">��₩��I���ł��鐔</param>
    /// <param name="out1List">����1��</param>
    /// <param name="out2List">����2��</param>
    /// <param name="out3List">����3��</param>
    /// <returns></returns>
    private List<Vector2Int> CalcCoveringList(List<Vector2Int> list, int startIndex, int canSelectNum,
        Dictionary<Vector2Int, List<Vector2Int>> out1List,
        Dictionary<Vector2Int, List<Vector2Int>> out2List,
        Dictionary<Vector2Int, List<Vector2Int>> out3List)
    {
        // �S���ԗ��ł��Ă��琬��
        if (out1List.Count == 0 && out2List.Count == 0 && out3List.Count == 0) return new List<Vector2Int>();
        // �����I�ׂȂ��Ȃ玸�s
        if (canSelectNum == 0) return null;

        // 1�ǂꂩ�I��ōċA
        for (int i = startIndex; i < list.Count; ++i)
        {
            var v = list[i];
            var tmp1List = out1List;
            var tmp2List = out2List;
            var tmp3List = out3List;

            // 1�������ꍇ�̎c�胊�X�g
            PickRemoveFromList(ref tmp1List, ref tmp2List, ref tmp3List, v);

            // �ċA
            var recursive = CalcCoveringList(list, i + 1, canSelectNum - 1, tmp1List, tmp2List, tmp3List);
            if (recursive != null)
            {
                var ret = new List<Vector2Int> { v };
                ret.AddRange(recursive);

                return ret;
            }
        }

        // ������Ȃ������玸�s
        return null;
    }

    /// <summary>
    /// �K�v�Ȑ��̃��X�g������W��1��菜��
    /// </summary>
    /// <param name="list1"></param>
    /// <param name="list2"></param>
    /// <param name="list3"></param>
    /// <param name="v"></param>
    private void PickRemoveFromList(ref Dictionary<Vector2Int, List<Vector2Int>> list1,
        ref Dictionary<Vector2Int, List<Vector2Int>> list2,
        ref Dictionary<Vector2Int, List<Vector2Int>> list3,
        Vector2Int v)
    {
        list1.Remove(v);
        list2.Remove(v);
        list3.Remove(v);

        var subList = list1.Where(p => p.Value.Contains(v)).Select(p => p.Key).ToList();
        foreach (var k in subList)
        {
            list1.Remove(k);
        }

        subList = list2.Where(p => p.Value.Contains(v)).Select(p => p.Key).ToList();
        foreach (var k in subList)
        {
            var val = list2[k];
            val.Remove(v);
            list1.Add(k, val);
            list2.Remove(k);
        }

        subList = list3.Where(p => p.Value.Contains(v)).Select(p => p.Key).ToList();
        foreach (var k in subList)
        {
            var val = list3[k];
            val.Remove(v);
            list2.Add(k, val);
            list3.Remove(k);
        }
    }

    /// <summary>
    /// �L���Ȏ탊�X�g
    /// </summary>
    /// <returns></returns>
    private List<Vector2Int> GetEnableList()
    {
        var enableSeeds = new List<Vector2Int>();
        foreach (var seedRow in seedScripts)
        {
            foreach (var seed in seedRow.Value)
            {
                if (seed.Value.IsEnable())
                {
                    enableSeeds.Add(new Vector2Int(seedRow.Key, seed.Key));
                }
            }
        }
        return enableSeeds;
    }

    /// <summary>
    /// �킪���
    /// </summary>
    private void GrowUp()
    {
        foreach (var seedRow in seedScripts)
            foreach (var seed in seedRow.Value)
            {
                var scr = seed.Value;
                if (!scr.IsEnable()) continue;

                int growNum = CalcGrowNum(seedRow.Key, seed.Key);
                scr.GrowNum(growNum);
            }
    }

    /// <summary>
    /// �㉺���E��4�x�N�g�����X�g
    /// </summary>
    /// <param name="center">���S���W</param>
    /// <param name="beingOnly">�킪���݂�����W�̂�</param>
    /// <param name="enableOnly">�擾�ł�����W�̂�</param>
    /// <returns></returns>
    private List<Vector2Int> GetCrossVector(Vector2Int center, bool beingOnly = true, bool enableOnly = true)
    {
        var koho = new List<Vector2Int>
        {
            center + new Vector2Int(-1, 0),
            center + new Vector2Int(1, 0),
            center + new Vector2Int(0, 1),
            center + new Vector2Int(0, -1),
        };

        var ret = new List<Vector2Int>();
        foreach (var v in koho)
        {
            if (beingOnly)
            {
                var scr = GetLocationSeedScript(v.x, v.y);
                if (scr == null) continue;
                if (scr.GetNum() < 0) continue;
                if (enableOnly)
                {
                    if (!scr.IsEnable()) continue;
                }
            }

            ret.Add(v);
        }

        return ret;
    }

    /// <summary>
    /// �������鐔���v�Z
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <returns></returns>
    private int CalcGrowNum(int row, int col)
    {
        // �㉺���E�J�E���g
        var crossList = GetCrossVector(new Vector2Int(row, col), true, false);
        return 4 - crossList.Count;
    }

    /// <summary>
    /// ��X�N���v�g�擾
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <returns></returns>
    private SeedScript GetLocationSeedScript(int row, int col)
    {
        if (!seedScripts.ContainsKey(row)) return null;
        var seedRow = seedScripts[row];

        if (!seedRow.ContainsKey(col)) return null;

        return seedRow[col];
    }
    #endregion
}
