using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// �{�X�@����������
/// </summary>
public class BossGameSystemB : GameSceneScriptBase
{
    #region �萔

    /// <summary>
    /// �L����ID
    /// </summary>
    public enum CharacterID : int
    {
        Player = 0,
        Boss,
        Ami,
        Mana,
        Matuka,
        Menderu,
        Mati,
        Pierre,
    }

    /// <summary>1�}�X�̕�</summary>
    public const float CELL_WIDTH = 130f;
    /// <summary>1�}�X�̍���</summary>
    public const float CELL_HEIGHT = 86f;

    /// <summary>�}�X��X��</summary>
    public const int CELL_X_COUNT = 7;
    /// <summary>�}�X��Y��</summary>
    public const int CELL_Y_COUNT = 7;

    #endregion

    #region �����o�[

    public BossGameBDataObject dataObj;

    public BossGameBPlayer player;
    public BossGameBEnemy ami;
    public BossGameBEnemy mana;
    public BossGameBEnemy matuka;
    public BossGameBEnemy menderu;
    public BossGameBEnemy mati;
    public BossGameBEnemy pierre;
    public BossGameBEnemy boss;

    public BossGameBUICommand commandUI;
    public BossGameBUISkillName skillNameUI;
    public BossGameBUITurnShow turnUI;
    public BossGameBUICellSelect cellUI;

    public Transform hpParent;
    public BossGameBUIHPShow hpDummy;
    public BossGameBUIDamage damageDummy;

    public Transform effectParent;
    public Transform cellEffectParent;
    public BossGameBStatusBuffEffect buffEffectDummy;
    public BossGameBGeneralEffect generalEffectDummy;
    public BossGameBHorrorEffect horrorEffectDummy;
    public BossGameBOriginEffect originEffectDummy;

    public AudioClip se_turnStart;
    public AudioClip se_statusUp;
    public AudioClip se_statusDown;

    #endregion

    #region �ϐ�

    /// <summary>�����Ă�L�������X�g</summary>
    private List<BossGameBCharacterBase> characterList = new List<BossGameBCharacterBase>();

    /// <summary>�v���C���[�̕������Z�b�g����t���O</summary>
    public bool playerWalkReset { get; set; } = true;

    #endregion

    #region ���

    /// <summary>
    /// �J�n�O
    /// </summary>
    /// <returns></returns>
    public override IEnumerator Start()
    {
        var g = Global.GetTemporaryData();
        yield return base.Start();

        characterList.Add(player);
        characterList.Add(boss);
        // �A�팋�ʂɂ���č폜
        if (g.bossRushAmiWon) ami.gameObject.SetActive(false);
        else characterList.Add(ami);
        if (g.bossRushManaWon) mana.gameObject.SetActive(false);
        else characterList.Add(mana);
        if (g.bossRushMatukaWon) matuka.gameObject.SetActive(false);
        else characterList.Add(matuka);
        if (g.bossRushMenderuWon) menderu.gameObject.SetActive(false);
        else characterList.Add(menderu);
        if (g.bossRushMatiWon) mati.gameObject.SetActive(false);
        else characterList.Add(mati);
        if (g.bossRushPierreWon) pierre.gameObject.SetActive(false);
        else characterList.Add(pierre);

        foreach (var chara in characterList)
        {
            chara.InitParameter();
            chara.ResetTime(true);
        }

        // UI��U��\��
        commandUI.Close();
        skillNameUI.Hide();
        turnUI.Show(new List<CharacterID>());

        // �_�~�[��\��
        hpDummy.gameObject.SetActive(false);
        damageDummy.gameObject.SetActive(false);
        generalEffectDummy.gameObject.SetActive(false);
        buffEffectDummy.gameObject.SetActive(false);
        horrorEffectDummy.gameObject.SetActive(false);
        originEffectDummy.gameObject.SetActive(false);
    }

    /// <summary>
    /// �t�F�[�h�C����
    /// </summary>
    /// <returns></returns>
    public override IEnumerator AfterFadeIn()
    {
        yield return base.AfterFadeIn();

        StartCoroutine(MainCoroutine());
    }

    #endregion

    #region ����

    /// <summary>
    /// ���C���R���[�`��
    /// </summary>
    /// <returns></returns>
    private IEnumerator MainCoroutine()
    {
        var sound = ManagerSceneScript.GetInstance().soundMan;
        var input = InputManager.GetInstance();

        while (true)
        {
            yield return null;
            // �N�����s��������̓v���C���[�̕������Z�b�g
            if (playerWalkReset)
            {
                player.ResetWalkCount();
            }

            // �s������L����������
            BossGameBCharacterBase turnChara = null;
            var nextTime = int.MaxValue;
            foreach (var chara in characterList)
            {
                if (chara.GetWaitTime() < nextTime)
                {
                    turnChara = chara;
                    nextTime = chara.GetWaitTime();
                }
            }

            // �S���̑ҋ@���Ԃ����炷
            foreach (var chara in characterList)
            {
                chara.DecreaseTime(nextTime);
            }

            // UI�X�V
            UpdateTurnUI();

            if (turnChara.CharacterType == BossGameBCharacterBase.CharaType.Player)
            {
                if (playerWalkReset)
                {
                    // �v���C���[�^�[���̊J�n��
                    sound.PlaySE(se_turnStart);
                    // HP�\��
                    yield return ShowHp();
                    playerWalkReset = false;
                }
            }

            // �s������
            yield return turnChara.TurnProcessBase();

            // �{�X�����S���Ă�����I������
            if (!characterList.Contains(boss))
            {
                skillNameUI.Show(StringMinigameMessage.BossB_Win);
                break;
            }

            // �v���C���[�����S���Ă�����I������
            if (!characterList.Contains(player))
            {
                skillNameUI.Show(StringMinigameMessage.BossB_Lose);
                break;
            }

            // �s�������L�����̑ҋ@���Ԃ��Đݒ�
            turnChara.ResetTime();
        }

        yield return new WaitForSeconds(2f);
        ManagerSceneScript.GetInstance().ExitGame();
    }

    #endregion

    #region �V�X�e���@�\

    /// <summary>
    /// �Z���̍��W�v�Z�@����(0,0)�@�E��(6,6)
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static Vector3 GetCellPosition(Vector2Int location)
    {
        return new Vector3(location.x * CELL_WIDTH, location.y * CELL_HEIGHT);
    }

    /// <summary>
    /// �^�[���\���X�V
    /// </summary>
    /// <param name="deleteTop">�擪�̃A�C�R�����m���ɏ�����</param>
    private void UpdateTurnUI(bool deleteTop = true)
    {
        var turnList = new List<CharacterID>();
        var tmpWaitTime = new List<int>();
        var tmpMaxWaitTime = new List<int>();
        foreach (var chara in characterList)
        {
            tmpWaitTime.Add(chara.GetWaitTime());
            tmpMaxWaitTime.Add(chara.GetMaxWaitTime());
        }

        for (var i = 0; i < 6; ++i)
        {
            var min = tmpWaitTime.Min();
            var idx = tmpWaitTime.IndexOf(min);
            turnList.Add(characterList[idx].GetCharacterID());

            for (var j = 0; j < tmpWaitTime.Count; ++j)
            {
                tmpWaitTime[j] -= min;
            }
            tmpWaitTime[idx] = tmpMaxWaitTime[idx];
        }

        turnUI.Show(turnList, deleteTop);
    }

    /// <summary>
    /// �����s�����Ԃ̑��s�`�F�b�N
    /// </summary>
    /// <param name="excludeCharacter">���g�𔻒肩�珜��</param>
    /// <param name="time">�o�߂��鎞��</param>
    /// <returns></returns>
    public bool CanWalkWait(BossGameBCharacterBase excludeCharacter, int time)
    {
        foreach (var character in characterList)
        {
            if (character == excludeCharacter) continue;
            if (character.GetWaitTime() < time) return false;
        }

        return true;
    }

    /// <summary>
    /// �S�̂̑҂����Ԃ����炷
    /// </summary>
    /// <param name="excludeCharacter">���g������</param>
    /// <param name="time">�o�ߎ���</param>
    public void DecreaseAllCharacterWait(BossGameBCharacterBase excludeCharacter, int time)
    {
        foreach (var character in characterList)
        {
            if (character == excludeCharacter) continue;
            character.DecreaseTime(time);
        }

        UpdateTurnUI(false);
    }

    /// <summary>
    /// �X�L���g�p�\�ʒu���쐬
    /// </summary>
    /// <param name="skillId">�X�L��</param>
    /// <param name="character">�g�p��</param>
    /// <param name="isPlayer">�v���C���[�̏ꍇ�����Ɋ֌W�Ȃ��S���ʂɎg�p�\</param>
    public static List<Vector2Int> CreateEnableCellList(BossGameBDataBase.SkillID skillId, BossGameBCharacterBase character)
    {
        var basePos = character.GetLocation();
        var baseDir = character.GetDirection();
        var isPlayer = character.CharacterType == BossGameBCharacterBase.CharaType.Player;

        var offsetList = new List<Vector2Int>();
        var skill = BossGameBDataBase.SkillList[skillId];
        var dirCell = GetDirectionCell(baseDir);
        var rangeType = skill.RangeType;
        if (isPlayer)
        {
            if (rangeType == BossGameBDataBase.RangeTypeEnum.ThreeLine)
                rangeType = BossGameBDataBase.RangeTypeEnum.AllLine;
            else if (rangeType == BossGameBDataBase.RangeTypeEnum.FrontLine)
                rangeType = BossGameBDataBase.RangeTypeEnum.AllCrossLine;
        }

        // �T�C�Y�ɂ��ʒu�I�t�Z�b�g�i�����ɖ��Ή��j
        var outW = character.body_width / 2;
        var outH = character.body_height / 2;

        // ��{�ʒu
        foreach (var r in skill.RangeList)
        {
            switch (rangeType)
            {
                // �S����
                case BossGameBDataBase.RangeTypeEnum.All:
                    for (var x = -r - outW; x <= r + outW; ++x)
                    {
                        // ��Ɖ���
                        offsetList.Add(new Vector2Int(x, r + outH));
                        if (r != 0) // 0�̏ꍇ�͏d�Ȃ�̂�OK
                            offsetList.Add(new Vector2Int(x, -r - outH));
                    }
                    for (var y = -r - outH + 1; y <= r + outH - 1; ++y)
                    {
                        // ���ƉE��
                        offsetList.Add(new Vector2Int(r + outW, y));
                        offsetList.Add(new Vector2Int(-r - outW, y));
                    }
                    break;
                case BossGameBDataBase.RangeTypeEnum.AllLine:
                    // 8����
                    offsetList.Add(new Vector2Int(r + outW, r + outH));
                    offsetList.Add(new Vector2Int(r + outW, -r - outH));
                    offsetList.Add(new Vector2Int(-r - outW, r + outH));
                    offsetList.Add(new Vector2Int(-r - outW, -r - outH));
                    for (var x = -outW; x <= outW; ++x)
                    {
                        offsetList.Add(new Vector2Int(x, r + outH));
                        offsetList.Add(new Vector2Int(x, -r - outH));
                    }
                    for (var y = -outH; y <= outH; ++y)
                    {
                        offsetList.Add(new Vector2Int(r + outW, y));
                        offsetList.Add(new Vector2Int(-r - outW, y));
                    }
                    break;
                case BossGameBDataBase.RangeTypeEnum.AllCrossLine:
                    // �΂߂S����
                    offsetList.Add(new Vector2Int(r + outW, r + outH));
                    offsetList.Add(new Vector2Int(r + outW, -r - outH));
                    offsetList.Add(new Vector2Int(-r - outW, r + outH));
                    offsetList.Add(new Vector2Int(-r - outW, -r - outH));
                    break;
                case BossGameBDataBase.RangeTypeEnum.AllPlusLine:
                    // �c��
                    for (var x = -outW; x <= outW; ++x)
                    {
                        offsetList.Add(new Vector2Int(x, r + outH));
                        offsetList.Add(new Vector2Int(x, -r - outH));
                    }
                    for (var y = -outH; y <= outH; ++y)
                    {
                        offsetList.Add(new Vector2Int(r + outW, y));
                        offsetList.Add(new Vector2Int(-r - outW, y));
                    }
                    break;
                case BossGameBDataBase.RangeTypeEnum.ThreeLine:
                    // �����Ă�����̎΂�
                    offsetList.Add(new Vector2Int(dirCell.x * (r + outW), dirCell.y * (r + outH)));
                    // �����Ă�����̉�
                    for (var y = -outH; y <= outH; ++y)
                    {
                        offsetList.Add(new Vector2Int(dirCell.x * (r + outW), y));
                    }
                    // �����Ă�����̏c
                    for (var x = -outW; x <= outW; ++x)
                    {
                        offsetList.Add(new Vector2Int(x, dirCell.y * (r + outH)));
                    }
                    break;
                case BossGameBDataBase.RangeTypeEnum.FrontLine:
                    // �����Ă�����̎΂߂̂�
                    offsetList.Add(new Vector2Int(dirCell.x * (r + outW), dirCell.y * (r + outH)));
                    break;
            }
        }

        // �L�����ʒu�����Z���Ĕ͈͓��̃Z���̂�
        var list = new List<Vector2Int>();
        foreach (var ofs in offsetList)
        {
            var pos = ofs + basePos + new Vector2Int(0, outH); // ��T�C�Y�͒�����ԉ�����Ȃ̂�Y�Ƀv���X
            if (pos.x < 0 || pos.x >= CELL_X_COUNT ||
                pos.y < 0 || pos.y >= CELL_Y_COUNT) continue;

            list.Add(pos);
        }

        return list;
    }

    /// <summary>
    /// �����Ă�����̒P�ʃx�N�g��
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    public static Vector2Int GetDirectionCell(BossGameBCharacterBase.CharaDirection dir)
    {
        return dir switch
        {
            BossGameBCharacterBase.CharaDirection.LeftUp => new Vector2Int(-1, 1),
            BossGameBCharacterBase.CharaDirection.RightUp => new Vector2Int(1, 1),
            BossGameBCharacterBase.CharaDirection.RightDown => new Vector2Int(1, -1),
            _ => new Vector2Int(-1, -1),
        };
    }

    /// <summary>
    /// �ʒu�ɂ���L�����N�^�[���擾
    /// </summary>
    /// <param name="loc"></param>
    /// <returns></returns>
    public BossGameBCharacterBase GetCellCharacter(Vector2Int loc)
    {
        foreach (var chara in characterList)
        {
            var center = chara.GetLocation();
            if (center.x - chara.body_width / 2 > loc.x) continue;
            if (center.x + chara.body_width / 2 < loc.x) continue;
            if (center.y > loc.y) continue;
            if (center.y + chara.body_height - 1 < loc.y) continue;

            return chara;
        }

        return null;
    }

    /// <summary>
    /// ���̏ꏊ�Ɉړ��ł��邩
    /// </summary>
    /// <param name="loc"></param>
    /// <returns></returns>
    public bool CanWalk(Vector2Int loc)
    {
        if (loc.x < 0 || loc.y < 0 || loc.x >= CELL_X_COUNT || loc.y >= CELL_Y_COUNT) return false;
        if (GetCellCharacter(loc) != null) return false;

        return true;
    }

    /// <summary>
    /// �v���C���[�̍��W
    /// </summary>
    /// <returns></returns>
    public Vector2Int GetPlayerLoc() { return player.GetLocation(); }

    /// <summary>
    /// �L�����N�^�[���폜
    /// </summary>
    /// <param name="chara"></param>
    public void RemoveCharacter(BossGameBCharacterBase chara)
    {
        characterList.Remove(chara);
    }

    /// <summary>
    /// HP�\��
    /// </summary>
    /// <returns></returns>
    public IEnumerator ShowHp()
    {
        var input = InputManager.GetInstance();

        var hpList = new List<BossGameBUIHPShow>();
        foreach (var character in characterList)
        {
            var show = Instantiate(hpDummy);
            var p = GetCellPosition(character.GetLocation());
            show.transform.SetParent(hpParent);
            show.Show(character.GetHp(), character.GetHpMax(), p);
            hpList.Add(show);
        }

        yield return null;
        yield return new WaitUntil(() => input.GetKeyPress(InputManager.Keys.South) ||
            input.GetKeyPress(InputManager.Keys.Up) ||
            input.GetKeyPress(InputManager.Keys.Down) ||
            input.GetKeyPress(InputManager.Keys.Left) ||
            input.GetKeyPress(InputManager.Keys.Right) ||
            input.GetKeyPress(InputManager.Keys.East)
            );

        foreach (var show in hpList)
        {
            Destroy(show.gameObject);
        }

        yield return new WaitWhile(() =>
            input.GetKey(InputManager.Keys.Up) ||
            input.GetKey(InputManager.Keys.Down) ||
            input.GetKey(InputManager.Keys.Left) ||
            input.GetKey(InputManager.Keys.Right)
        );
    }

    /// <summary>
    /// �_���[�W�\��
    /// </summary>
    /// <param name="loc"></param>
    /// <param name="damage">�}�C�i�X�ŉ�</param>
    public IEnumerator ShowDamage(Vector2Int loc, int damage)
    {
        var dmg = Instantiate(damageDummy);
        dmg.transform.SetParent(hpParent);
        dmg.transform.localPosition = GetCellPosition(loc);
        yield return dmg.ShowDamage(damage);
        Destroy(dmg.gameObject);
    }

    #endregion

    #region �G�t�F�N�g�Ǘ�

    /// <summary>
    /// ���x�o�t�E�f�o�t���o
    /// </summary>
    /// <param name="characterList"></param>
    /// <param name="rate"></param>
    /// <returns></returns>
    public IEnumerator BuffSpeed(List<BossGameBCharacterBase> characterList, float rate)
    {
        var isUp = rate > 1f;
        foreach (var character in characterList)
        {
            if (!isUp && character.IsInvincible()) continue;

            character.ChangeSpeed(rate);
            var buff = Instantiate(buffEffectDummy);
            buff.SetParam(isUp, BossGameBStatusBuffEffect.Type.Speed);
            buff.transform.SetParent(cellEffectParent);
            buff.PlayAndDestroy(GetCellPosition(character.GetLocation()));
        }

        ManagerSceneScript.GetInstance().soundMan.PlaySE(isUp ? se_statusUp : se_statusDown);

        yield return new WaitForSeconds(0.5f);
    }

    /// <summary>
    /// �U���̓o�t�E�f�o�t���o
    /// </summary>
    /// <param name="characterList"></param>
    /// <param name="rate"></param>
    /// <returns></returns>
    public IEnumerator BuffAttack(List<BossGameBCharacterBase> characterList, float rate)
    {
        var isUp = rate > 1f;
        foreach (var character in characterList)
        {
            if (!isUp && character.IsInvincible()) continue;

            character.ChangeAttackRate(rate);
            var buff = Instantiate(buffEffectDummy);
            buff.SetParam(isUp, BossGameBStatusBuffEffect.Type.Strength);
            buff.transform.SetParent(cellEffectParent);
            buff.PlayAndDestroy(GetCellPosition(character.GetLocation()));
        }

        ManagerSceneScript.GetInstance().soundMan.PlaySE(isUp ? se_statusUp : se_statusDown);

        yield return new WaitForSeconds(0.5f);
    }

    /// <summary>
    /// �ėp�G�t�F�N�g����
    /// </summary>
    /// <param name="cellPosition"></param>
    /// <param name="kind">���</param>
    public void CreateGeneralEffect(Vector3 cellPosition, BossGameBDataObject.EffectKind kind)
    {
        var eff = Instantiate(generalEffectDummy);
        eff.transform.SetParent(cellEffectParent);
        eff.SetParam(dataObj.GetGeneralEffectList(kind));
        eff.PlayAndDestroy(cellPosition);
    }

    /// <summary>
    /// �񕜃G�t�F�N�g
    /// </summary>
    /// <param name="center"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public IEnumerator PlayHealEffect(Vector2Int center, int width, int height)
    {
        var basePos = GetCellPosition(center);
        var ofsX = (width / 2 / 2f + 0.25f) * CELL_WIDTH;
        var ofsY = (height / 2 / 2f + 0.25f) * CELL_HEIGHT;
        var base1 = basePos + new Vector3(ofsX, ofsY);
        var base2 = basePos + new Vector3(-ofsX, -ofsY);
        var base3 = basePos + new Vector3(-ofsX, ofsY);
        var base4 = basePos + new Vector3(ofsX, -ofsY);

        for (var i = 0; i < 5; ++i)
        {
            var rand = new Vector3(Util.RandomFloat(-ofsX, ofsX), Util.RandomFloat(-ofsY, ofsY));
            CreateGeneralEffect(base1 + rand, BossGameBDataObject.EffectKind.Heal);
            yield return new WaitForSeconds(0.03f);
            rand = new Vector3(Util.RandomFloat(-ofsX, ofsX), Util.RandomFloat(-ofsY, ofsY));
            CreateGeneralEffect(base2 + rand, BossGameBDataObject.EffectKind.Heal);
            yield return new WaitForSeconds(0.03f);
            rand = new Vector3(Util.RandomFloat(-ofsX, ofsX), Util.RandomFloat(-ofsY, ofsY));
            CreateGeneralEffect(base3 + rand, BossGameBDataObject.EffectKind.Heal);
            yield return new WaitForSeconds(0.03f);
            rand = new Vector3(Util.RandomFloat(-ofsX, ofsX), Util.RandomFloat(-ofsY, ofsY));
            CreateGeneralEffect(base4 + rand, BossGameBDataObject.EffectKind.Heal);
            yield return new WaitForSeconds(0.03f);
        }
    }

    /// <summary>
    /// �z���[�G�t�F�N�g�Đ�
    /// </summary>
    /// <param name="center"></param>
    public void PlayHorrorEffect(Vector2Int center)
    {
        var eff = Instantiate(horrorEffectDummy);
        eff.transform.SetParent(cellEffectParent);
        eff.PlayAndDestroy(GetCellPosition(center));
    }

    /// <summary>
    /// �a���G�t�F�N�g
    /// </summary>
    /// <param name="center"></param>
    /// <param name="direction"></param>
    public void CreateSlashEffect(Vector2Int center, Vector3 direction)
    {
        var rad = Util.GetRadianFromVector(direction);
        var eff = Instantiate(generalEffectDummy);
        eff.transform.SetParent(cellEffectParent);
        eff.SetParam(dataObj.GetGeneralEffectList(BossGameBDataObject.EffectKind.Slash));
        eff.model.transform.localRotation = Util.GetRotateQuaternion(rad);
        eff.PlayAndDestroy(GetCellPosition(center));
    }

    /// <summary>
    /// �I���W���G�t�F�N�g
    /// </summary>
    /// <param name="center"></param>
    public void CreateOriginEffect(Vector2Int center)
    {
        var eff = Instantiate(originEffectDummy);
        eff.transform.SetParent(cellEffectParent);
        eff.PlayAndDestroy(GetCellPosition(center));
    }

    #endregion
}
