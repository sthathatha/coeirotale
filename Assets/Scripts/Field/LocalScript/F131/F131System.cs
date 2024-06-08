using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// F131�@�X�̃u���b�N�p�Y��
/// </summary>
public class F131System : MainScriptBase
{
    #region �萔

    /// <summary>�u���b�N�N���A</summary>
    public const string ICE_BLOCK_FLG = "F131IceBlock";

    /// <summary>�I�߂܂���t���O�@0:���m�F�@1:�m�F�@2:�E�[���A��ė���@3:�ߊl�ς�</summary>
    public const string ICE_YOU_FLG = "F131YouCatch";

    /// <summary>�ő�X</summary>
    protected const int LOC_MAX_X = 10;
    /// <summary>�ő�Y</summary>
    protected const int LOC_MAX_Y = 9;
    /// <summary>�N���A����Y</summary>
    public const int LOC_GOAL_Y = 4;

    #endregion

    #region �����o�[

    /// <summary>�I</summary>
    public GameObject you;
    /// <summary>�E�[��</summary>
    public GameObject worra;

    /// <summary>�u���b�N</summary>
    public GameObject blockParent;

    /// <summary>��</summary>
    public GameObject holeObj;

    /// <summary>����SE</summary>
    public AudioClip slipSe;
    /// <summary>�~�܂�SE</summary>
    public AudioClip stopSe;

    #endregion

    #region �ϐ�

    /// <summary>�u���b�N���X�g</summary>
    private List<F131Block> blocks = new List<F131Block>();

    #endregion

    #region ���

    /// <summary>
    /// �J�n��
    /// </summary>
    /// <returns></returns>
    public override IEnumerator BeforeInitFadeIn()
    {
        yield return base.BeforeInitFadeIn();

        blocks.AddRange(blockParent.GetComponentsInChildren<F131Block>());

        you.SetActive(false);
        var clearSyori = false;
        var youFlg = Global.GetSaveData().GetGameDataInt(ICE_YOU_FLG);
        if (Global.GetSaveData().GetGameDataInt(ICE_BLOCK_FLG)  >= 1)
        {
            // �I�ߊl�O�{�E����������ꍇ�͏������
            if (youFlg >= 3 || ManagerSceneScript.GetInstance().GetInitId() != 0)
            {
                clearSyori = true;
            }
            else
            {
                // �E�[���A��Ă����ꍇ�̂ݔz�u
                if (youFlg != 2)
                {
                    worra.SetActive(false);
                }
            }
        }
        else
        {
            worra.SetActive(false);
        }

        if (clearSyori)
        {
            worra.SetActive(false);
            foreach (var b in blocks)
            {
                var clear = b as F131ClearBlock;
                if (clear != null)
                {
                    clear.SetClearHole();
                    break;
                }
            }
            holeObj.SetActive(false);
        }
    }

    /// <summary>
    /// �t�F�[�h�C����
    /// </summary>
    /// <returns></returns>
    public override IEnumerator AfterFadeIn()
    {
        yield return base.AfterFadeIn();

        if (Global.GetSaveData().GetGameDataInt(ICE_BLOCK_FLG) >= 1)
        {
            if (Global.GetSaveData().GetGameDataInt(ICE_YOU_FLG) < 3 &&
                ManagerSceneScript.GetInstance().GetInitId() == 0)
            {
                GetComponent<F131Start>().ExecEvent();
            }
        }
    }

    /// <summary>
    /// BGM
    /// </summary>
    /// <returns></returns>
    public override Tuple<SoundManager.FieldBgmType, AudioClip> GetBgm()
    {
        return new Tuple<SoundManager.FieldBgmType, AudioClip>(SoundManager.FieldBgmType.Common1, null);
    }

    #endregion

    #region �p�u���b�N

    /// <summary>
    /// �����̍��W�𔻒�
    /// </summary>
    /// <param name="before"></param>
    /// <param name="dir"></param>
    /// <returns></returns>
    public Vector2Int CheckSlipLocation(Vector2Int before, Constant.Direction dir)
    {
        // �Ԃ���΂�����
        F131Block hitBlock = null;
        foreach (var block in blocks)
        {
            var l = block.GetLocation();
            switch (dir)
            {
                case Constant.Direction.Up:
                    if (l.x == before.x && l.y > before.y &&
                        (hitBlock == null || hitBlock.GetLocation().y > l.y))
                    {
                        hitBlock = block;
                    }
                    break;
                case Constant.Direction.Down:
                    if (l.x == before.x && l.y < before.y &&
                        (hitBlock == null || hitBlock.GetLocation().y < l.y))
                    {
                        hitBlock = block;
                    }
                    break;
                case Constant.Direction.Right:
                    if (l.y == before.y && l.x > before.x &&
                        (hitBlock == null || hitBlock.GetLocation().x > l.x))
                    {
                        hitBlock = block;
                    }
                    break;
                case Constant.Direction.Left:
                    if (l.y == before.y && l.x < before.x &&
                        (hitBlock == null || hitBlock.GetLocation().x < l.x))
                    {
                        hitBlock = block;
                    }
                    break;
            }
        }

        var after = before;
        if (hitBlock == null)
        {
            // �Ԃ���΂������ꍇ�[�܂�
            if (dir == Constant.Direction.Left && before.y == LOC_GOAL_Y)
            {
                after.x = -2;
            }
            else
            {
                switch (dir)
                {
                    case Constant.Direction.Up: after.y = LOC_MAX_Y; break;
                    case Constant.Direction.Down: after.y = 0; break;
                    case Constant.Direction.Right: after.x = LOC_MAX_X; break;
                    case Constant.Direction.Left: after.x = 0; break;
                }
            }
        }
        else
        {
            // �Ԃ���΂�1�O
            var l = hitBlock.GetLocation();
            switch (dir)
            {
                case Constant.Direction.Up: after.y = l.y - 1; break;
                case Constant.Direction.Down: after.y = l.y + 1; break;
                case Constant.Direction.Right: after.x = l.x - 1; break;
                case Constant.Direction.Left: after.x = l.x + 1; break;
            }
        }

        return after;
    }

    #endregion

    #region �v���C�x�[�g



    #endregion
}
