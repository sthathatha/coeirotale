using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���X�{�X�{��@�^�[���\��1��
/// </summary>
public class BossGameBUITurnShowOne : MonoBehaviour
{
    #region �萔

    private readonly Color PLAYER_COLOR = Color.cyan;
    private readonly Color ENEMY_COLOR = Color.magenta;

    private const float DISAPPEAR_X = -360f;
    private const float ACTIVE_X = 0f;
    private const float WAIT_X = -64f;
    private const float ACTIVE_Y = 150f;
    private const float WAIT1_Y = 60f;
    private const float MOVE_TIME = 0.1f;

    #endregion

    #region �����o�[

    public Image bg;
    public Image face;

    public Sprite sp_reko;
    public Sprite sp_boss;
    public Sprite sp_ami;
    public Sprite sp_mana;
    public Sprite sp_matuka;
    public Sprite sp_menderu;
    public Sprite sp_mati;
    public Sprite sp_pierre;

    #endregion

    #region �ϐ�

    /// <summary>�\���L����</summary>
    private BossGameSystemB.CharacterID id;
    /// <summary>���݈ʒu</summary>
    private int index = -1;

    /// <summary>���s���R���[�`��</summary>
    private IEnumerator playingCoroutine = null;
    private bool isMoving = false;

    #endregion

    #region �@�\

    /// <summary>
    /// �\�����L����
    /// </summary>
    /// <returns></returns>
    public BossGameSystemB.CharacterID GetCharacterID() { return id; }

    /// <summary>
    /// ���݈ʒu
    /// </summary>
    /// <returns></returns>
    public int GetIndex() { return index; }

    /// <summary>
    /// ��ݒ�
    /// </summary>
    /// <param name="face"></param>
    /// <param name="isPlayer"></param>
    public void SetFace(BossGameSystemB.CharacterID f, bool isPlayer)
    {
        id = f;
        face.sprite = f switch
        {
            BossGameSystemB.CharacterID.Ami => sp_ami,
            BossGameSystemB.CharacterID.Mana => sp_mana,
            BossGameSystemB.CharacterID.Matuka => sp_matuka,
            BossGameSystemB.CharacterID.Menderu => sp_menderu,
            BossGameSystemB.CharacterID.Mati => sp_mati,
            BossGameSystemB.CharacterID.Pierre => sp_pierre,
            BossGameSystemB.CharacterID.Player => sp_reko,
            _ => sp_boss,
        };

        bg.color = isPlayer ? PLAYER_COLOR : ENEMY_COLOR;
    }

    /// <summary>
    /// �ړ�
    /// </summary>
    /// <param name="idx"></param>
    public void MoveTo(int idx)
    {
        if (isMoving) StopCoroutine(playingCoroutine);

        var afterPos = CalcDisplayPos(idx);
        if(index < 0)
        {
            // �ŏ��͍�����o�Ă���
            transform.localPosition = new Vector3(DISAPPEAR_X, afterPos.y, afterPos.z);
        }
        index = idx;

        playingCoroutine = MoveCoroutine(afterPos);
        StartCoroutine(playingCoroutine);
    }

    /// <summary>
    /// ���ɏ����ď���
    /// </summary>
    public void Disappear()
    {
        var afterPos = CalcDisplayPos(index);
        afterPos.x = DISAPPEAR_X;
        if (isMoving) StopCoroutine(playingCoroutine);
        playingCoroutine = MoveCoroutine(afterPos, true);
        StartCoroutine(playingCoroutine);
    }

    #endregion

    #region �R���[�`��

    /// <summary>
    /// �ړ��R���[�`��
    /// </summary>
    /// <param name="pos">�ړ���</param>
    /// <param name="exitDestroy">�I����j��</param>
    /// <returns></returns>
    private IEnumerator MoveCoroutine(Vector3 pos, bool exitDestroy = false)
    {
        isMoving = true;
        var deltaPos = new DeltaVector3();
        deltaPos.Set(transform.localPosition);
        deltaPos.MoveTo(pos, MOVE_TIME, DeltaFloat.MoveType.LINE);
        while (deltaPos.IsActive())
        {
            yield return null;
            deltaPos.Update(Time.deltaTime);
            transform.localPosition = deltaPos.Get();
        }

        isMoving = false;
        if (exitDestroy) Destroy(gameObject);
    }

    #endregion

    #region �v�Z

    /// <summary>
    /// �\���ʒu
    /// </summary>
    /// <param name="idx"></param>
    /// <returns></returns>
    private Vector3 CalcDisplayPos(int idx)
    {
        var x = idx == 0 ? ACTIVE_X : WAIT_X;
        var y = ACTIVE_Y - WAIT1_Y * idx;
        return new Vector3(x, y, 0);
    }

    #endregion
}
