using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public BossGameBJuggleEffect juggleEffectDummy;
    public BossGameBCardEffect cardEffectDummy;
    public BossGameBPlasmaEffect plasmaEffectDummy;
    public BossGameBChargeEffect chargeEffectDummy;
    public BossGameBCanonEffect canonEffectDummy;
    public BossGameBTranquiEffect tranquiEffectDummy;
    public BossGameBNoFuruteEffect nofutureEffectDummy;
    public BossGameBCarnageEffect carnageEffectDummy;

    public GameObject fieldEffectDummy;

    public AudioClip se_turnStart;
    public AudioClip se_statusUp;
    public AudioClip se_statusDown;

    #endregion

    #region �ϐ�

    /// <summary>�����Ă�L�������X�g</summary>
    private List<BossGameBCharacterBase> characterList = new List<BossGameBCharacterBase>();

    /// <summary>�n�`���ʃ��X�g</summary>
    private List<FieldEffectCell> fieldEffectList = new List<FieldEffectCell>();

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
        juggleEffectDummy.gameObject.SetActive(false);
        cardEffectDummy.gameObject.SetActive(false);
        plasmaEffectDummy.gameObject.SetActive(false);
        chargeEffectDummy.gameObject.SetActive(false);
        canonEffectDummy.gameObject.SetActive(false);
        tranquiEffectDummy.gameObject.SetActive(false);
        nofutureEffectDummy.gameObject.SetActive(false);
        carnageEffectDummy.gameObject.SetActive(false);

        fieldEffectDummy.SetActive(false);
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
        var won = false;

        while (true)
        {
            yield return null;

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

            // �n�`���Ԍ��炷
            DecreaseFieldEffectTime(nextTime);

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
                won = true;
                break;
            }

            // �v���C���[�����S���Ă�����I������
            if (!characterList.Contains(player))
            {
                skillNameUI.Show(StringMinigameMessage.BossB_Lose);
                break;
            }

            // �N�����s��������̓v���C���[�̕������Z�b�g
            if (playerWalkReset)
            {
                player.ResetWalkCount();
            }

            // �s�������L�����̑ҋ@���Ԃ��Đݒ�
            turnChara.ResetTime();
        }

        yield return new WaitForSeconds(2f);
        Global.GetTemporaryData().lastBossLost = !won;
        Global.GetTemporaryData().bossRush = false;
        if (won)
        {
            ManagerSceneScript.GetInstance().ExitGame();
        }
        else
        {
            // ��������O�̃}�b�v�ɖ߂�
            ManagerSceneScript.GetInstance().ExitGame("Field204", 2);
        }
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

        // �n�`�����Z
        DecreaseFieldEffectTime(time);
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
            // 0�͎����̈ʒu
            if (r == 0)
            {
                for (var x = -outW; x <= outW; ++x)
                    for (var y = -outH; y <= outH; ++y)
                    {
                        offsetList.Add(new Vector2Int(x, y));
                    }
                continue;
            }

            switch (rangeType)
            {
                // �S����
                case BossGameBDataBase.RangeTypeEnum.All:
                    for (var x = -r - outW; x <= r + outW; ++x)
                    {
                        // ��Ɖ���
                        offsetList.Add(new Vector2Int(x, r + outH));
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
    /// �X�L�����ʔ͈͂��쐬
    /// </summary>
    /// <param name="skillId"></param>
    /// <param name="center"></param>
    /// <returns></returns>
    public static List<Vector2Int> CreateSkillEffectCellList(BossGameBDataBase.SkillID skillId, Vector2Int center)
    {
        var skill = BossGameBDataBase.SkillList[skillId];
        var list = new List<Vector2Int>();

        for (var x = center.x - skill.EffectRange; x <= center.x + skill.EffectRange; ++x)
            for (var y = center.y - skill.EffectRange; y <= center.y + skill.EffectRange; ++y)
            {
                if (x < 0 || x >= CELL_X_COUNT) continue;
                if (y < 0 || y >= CELL_Y_COUNT) continue;

                list.Add(new Vector2Int(x, y));
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
    /// <param name="isPlayer"></param>
    /// <returns></returns>
    public bool CanWalk(Vector2Int loc, bool isPlayer = false)
    {
        if (loc.x < 0 || loc.y < 0 || loc.x >= CELL_X_COUNT || loc.y >= CELL_Y_COUNT) return false;
        if (GetCellCharacter(loc) != null) return false;
        if (!isPlayer)
        {
            // �v���C���[�ӊO�̓}���g���b�v�ɐN���s��
            if (GetCellFieldEffect(loc) == BossGameBDataObject.FieldEffect.Mantrap) return false;
        }

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
        characterList = characterList.Where(c => c.GetHp() > 0).ToList();
        if (!characterList.Any()) yield break;

        var isUp = rate > 1f;
        foreach (var character in characterList)
        {
            if (!isUp && character.IsInvincible()) continue;

            character.ChangeSpeed(rate);
            var buff = Instantiate(buffEffectDummy, cellEffectParent);
            buff.SetParam(isUp, BossGameBStatusBuffEffect.Type.Speed);
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
        characterList = characterList.Where(c => c.GetHp() > 0).ToList();
        if (!characterList.Any()) yield break;

        var isUp = rate > 1f;
        foreach (var character in characterList)
        {
            if (!isUp && character.IsInvincible()) continue;

            character.ChangeAttackRate(rate);
            var buff = Instantiate(buffEffectDummy, cellEffectParent);
            buff.SetParam(isUp, BossGameBStatusBuffEffect.Type.Strength);
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
        var eff = Instantiate(generalEffectDummy, cellEffectParent);
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

        for (var i = 0; i < 3; ++i)
        {
            var rand = new Vector3(Util.RandomFloat(-ofsX, ofsX), Util.RandomFloat(-ofsY, ofsY));
            CreateGeneralEffect(base1 + rand, BossGameBDataObject.EffectKind.Heal);
            yield return new WaitForSeconds(0.05f);
            rand = new Vector3(Util.RandomFloat(-ofsX, ofsX), Util.RandomFloat(-ofsY, ofsY));
            CreateGeneralEffect(base2 + rand, BossGameBDataObject.EffectKind.Heal);
            yield return new WaitForSeconds(0.05f);
            rand = new Vector3(Util.RandomFloat(-ofsX, ofsX), Util.RandomFloat(-ofsY, ofsY));
            CreateGeneralEffect(base3 + rand, BossGameBDataObject.EffectKind.Heal);
            yield return new WaitForSeconds(0.05f);
            rand = new Vector3(Util.RandomFloat(-ofsX, ofsX), Util.RandomFloat(-ofsY, ofsY));
            CreateGeneralEffect(base4 + rand, BossGameBDataObject.EffectKind.Heal);
            yield return new WaitForSeconds(0.05f);
        }
    }

    /// <summary>
    /// �z���[�G�t�F�N�g�Đ�
    /// </summary>
    /// <param name="center"></param>
    public void PlayHorrorEffect(Vector2Int center)
    {
        var eff = Instantiate(horrorEffectDummy, cellEffectParent);
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
        var eff = Instantiate(generalEffectDummy, cellEffectParent);
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
        var eff = Instantiate(originEffectDummy, cellEffectParent);
        eff.PlayAndDestroy(GetCellPosition(center));
    }

    /// <summary>
    /// �W���O�����O�G�t�F�N�g
    /// </summary>
    /// <param name="center"></param>
    public void CreateJuggleEffect(Vector2Int center, Vector3 p1, Vector3 p2, Vector2Int target, int type)
    {
        var ctp = GetCellPosition(center);
        var eff = Instantiate(juggleEffectDummy, cellEffectParent);
        eff.SetParam(ctp + p1, ctp + p2, GetCellPosition(target), type);
        eff.PlayAndDestroy(GetCellPosition(center));
    }

    /// <summary>
    /// �J�[�h�쐬
    /// </summary>
    /// <param name="center"></param>
    /// <param name="param"></param>
    public void CreateCardEffect(Vector2Int center, BossGameBCardEffect.CardParam param, int index)
    {
        var eff = Instantiate(cardEffectDummy, cellEffectParent);
        eff.SetParam(index, param.num, param.suit, GetCellPosition(center));
        eff.PlayAndDestroy(GetCellPosition(center));
    }

    /// <summary>
    /// �v���Y�}�t�B�[���h�G�t�F�N�g
    /// </summary>
    /// <param name="cellPosition"></param>
    public void CreatePlasmaEffect(Vector3 cellPosition)
    {
        var eff = Instantiate(plasmaEffectDummy, cellEffectParent);
        eff.PlayAndDestroy(cellPosition);
    }

    /// <summary>
    /// �`���[�W�G�t�F�N�g
    /// </summary>
    /// <param name="center"></param>
    public void CreateChargeEffect(Vector2Int center)
    {
        var eff = Instantiate(chargeEffectDummy, cellEffectParent);
        eff.PlayAndDestroy(GetCellPosition(center));
    }

    /// <summary>
    /// �A�[���X�g�����O�C�G�t�F�N�g
    /// </summary>
    /// <param name="dist"></param>
    public void CreateCanonEffect(Vector2Int dist)
    {
        var eff = Instantiate(canonEffectDummy, effectParent);
        eff.SetParam(dist);
        eff.PlayAndDestroy(new Vector3());
    }

    /// <summary>
    /// �g�����L�[���C�U�[�G�t�F�N�g
    /// </summary>
    /// <param name="pos1"></param>
    /// <param name="pos2"></param>
    public void CreateTranquiEffect(Vector3 pos1, Vector3 pos2)
    {
        var eff = Instantiate(tranquiEffectDummy, cellEffectParent);
        eff.SetParam(pos1, pos2);
        eff.PlayAndDestroy(pos1);
    }

    /// <summary>
    /// NO FUTURE
    /// </summary>
    public void CreateNoFutureEffect()
    {
        var eff = Instantiate(nofutureEffectDummy, effectParent);
        eff.PlayAndDestroy(Vector3.zero);
    }

    /// <summary>
    /// �J�[�l�C�W�G�t�F�N�g
    /// </summary>
    public void CreateCarnageEffect(Vector2Int center)
    {
        var eff = Instantiate(carnageEffectDummy, cellEffectParent);
        eff.PlayAndDestroy(GetCellPosition(center));
    }

    #endregion

    #region �n�`�Ǘ�

    /// <summary>
    /// �n�`���ʏ��N���X
    /// </summary>
    private class FieldEffectCell
    {
        /// <summary></summary>
        public Vector2Int loc;
        /// <summary>���</summary>
        public BossGameBDataObject.FieldEffect effect;
        /// <summary>�c�莞��</summary>
        public int time;

        /// <summary>��ʕ\��</summary>
        public GameObject obj = null;
    }

    /// <summary>
    /// �Z���̒n�`���ʂ��擾
    /// </summary>
    /// <param name="loc"></param>
    /// <returns></returns>
    public BossGameBDataObject.FieldEffect GetCellFieldEffect(Vector2Int loc)
    {
        foreach (var eff in fieldEffectList)
        {
            if (eff.loc == loc) return eff.effect;
        }

        return BossGameBDataObject.FieldEffect.None;
    }

    /// <summary>
    /// �n�`���ʂ��Z�b�g
    /// </summary>
    /// <param name="loc"></param>
    /// <param name="eff"></param>
    /// <param name="time"></param>
    public void SetFieldEffect(Vector2Int loc, BossGameBDataObject.FieldEffect eff, int time)
    {
        // �L����������ꏊ�ɂ͍��Ȃ�
        if (GetCellCharacter(loc) != null) return;

        var old = GetCellFieldEffect(loc);
        //�}���g���b�v�D��
        if (old == BossGameBDataObject.FieldEffect.Mantrap && eff != BossGameBDataObject.FieldEffect.Mantrap) return;

        var eInfo = fieldEffectList.Find(eff => eff.loc == loc);
        if (eInfo == null)
        {
            // ��Ȃ�V�K�쐬
            eInfo = new FieldEffectCell();
            eInfo.loc = loc;
            eInfo.obj = Instantiate(fieldEffectDummy);
            eInfo.obj.SetActive(true);
            eInfo.obj.transform.SetParent(cellEffectParent);
            eInfo.obj.transform.localPosition = GetCellPosition(loc);
            fieldEffectList.Add(eInfo);
        }
        eInfo.effect = eff;
        eInfo.time = time;
        eInfo.obj.GetComponentInChildren<SpriteRenderer>().sprite = eff switch
        {
            BossGameBDataObject.FieldEffect.Mantrap => dataObj.sp_field_mantrap,
            _ => dataObj.sp_field_plasma,
        };
    }

    /// <summary>
    /// �P�ӏ��̃t�B�[���h�G�t�F�N�g���폜
    /// </summary>
    /// <param name="loc"></param>
    public void ClearFieldEffect(Vector2Int loc)
    {
        var ei = fieldEffectList.FindIndex(eff => eff.loc == loc);
        if (ei < 0) return;

        Destroy(fieldEffectList[ei].obj);
        fieldEffectList.RemoveAt(ei);
    }

    /// <summary>
    /// �t�B�[���h�G�t�F�N�g�S�폜
    /// </summary>
    public void ClearFieldEffect()
    {
        foreach (var eff in fieldEffectList)
        {
            Destroy(eff.obj);
        }
        fieldEffectList.Clear();
    }

    /// <summary>
    /// �n�`���玞�Ԍ��Z
    /// </summary>
    /// <param name="time"></param>
    public void DecreaseFieldEffectTime(int time)
    {
        foreach (var feff in fieldEffectList)
        {
            feff.time -= time;
            if (feff.time <= 0)
            {
                Destroy(feff.obj);
            }
        }
        fieldEffectList.RemoveAll(e => e.time <= 0);
    }

    #endregion
}
