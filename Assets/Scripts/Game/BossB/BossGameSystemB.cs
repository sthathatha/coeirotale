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


    #endregion

    #region �����o�[

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

    #endregion

    private List<BossGameBCharacterBase> characterList = new List<BossGameBCharacterBase>();

    #region ���

    /// <summary>
    /// �J�n�O
    /// </summary>
    /// <returns></returns>
    public override IEnumerator Start()
    {
        yield return base.Start();

        //todo:
        characterList.Add(player);
        characterList.Add(boss);
        characterList.Add(ami);
        characterList.Add(mana);
        characterList.Add(matuka);
        characterList.Add(menderu);
        characterList.Add(mati);
        characterList.Add(pierre);

        foreach (var chara in characterList)
        {
            chara.InitParameter();
            chara.ResetTime(true);
        }

        commandUI.Close();
        skillNameUI.Hide();
        turnUI.Show(new List<CharacterID>());
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
        var input = InputManager.GetInstance();

        while (true)
        {
            yield return null;
            //
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

            //
            foreach (var chara in characterList)
            {
                chara.DecreaseTime(nextTime);
            }

            UpdateTurnUI();

            yield return turnChara.TurnProcess();

            //todo:
            if (!characterList.Contains(boss))
            {
                break;
            }

            //todo:
            if (!characterList.Contains(player))
            {
                break;
            }

            //todo:
            turnChara.ResetTime();
        }

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
    public static Vector3 GetCellLocation(int x, int y)
    {
        return new Vector3(x * CELL_WIDTH, y * CELL_HEIGHT);
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

    #endregion
}
