using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ���X�{�X�{��@�L�����N�^�[���ʏ���
/// </summary>
public class BossGameBCharacterBase : MonoBehaviour
{
    #region �萔

    /// <summary>�L�����̌���</summary>
    public enum CharaDirection : int
    {
        LeftUp = 0,
        RightUp,
        LeftDown,
        RightDown,
    }

    #endregion

    #region �����o�[

    /// <summary>�Q�[���V�X�e��</summary>
    public BossGameSystemB system;

    /// <summary>���f��</summary>
    public SpriteRenderer model;

    /// <summary>�E��摜</summary>
    public Sprite sp_rightup = null;
    /// <summary>����摜</summary>
    public Sprite sp_leftup = null;
    /// <summary>�E���摜</summary>
    public Sprite sp_rightdown = null;
    /// <summary>�����摜</summary>
    public Sprite sp_leftdown = null;

    /// <summary>���}�X��</summary>
    public int body_width = 1;
    /// <summary>�c�}�X��</summary>
    public int body_height = 1;

    /// <summary>�������W</summary>
    public int init_locx = 0;
    /// <summary>�������W</summary>
    public int init_locy = 0;
    /// <summary>��������</summary>
    public int init_dir_x = 1;
    /// <summary>��������</summary>
    public int init_dir_y = 1;

    #endregion

    #region �ϐ�

    /// <summary>���݂̌���</summary>
    protected CharaDirection nowDirection;

    /// <summary>���ݍ��W</summary>
    protected Vector2Int location;

    /// <summary>�ő�HP</summary>
    public int param_HP_max;
    /// <summary>����HP</summary>
    protected int param_HP;
    /// <summary>��{�U����</summary>
    public int param_ATK_base;
    /// <summary>�U���͕ϓ��l</summary>
    protected float param_ATK_rate;
    /// <summary>��{���x</summary>
    public int param_SPD_base;
    /// <summary>���x�ϓ��l</summary>
    protected float param_SPD_rate;
    /// <summary>�s���҂�����</summary>
    protected int param_wait_time;

    #endregion

    #region ������

    /// <summary>
    /// ������
    /// </summary>
    protected virtual void Start()
    {
        SetDirection(init_dir_x, init_dir_y);
        location = new Vector2Int(init_locx, init_locy);
        transform.localPosition = BossGameSystemB.GetCellLocation(init_locx, init_locy);
    }

    #endregion

    #region �@�\

    /// <summary>
    /// �L����ID�@�h����Ŏ���
    /// </summary>
    /// <returns></returns>
    public virtual BossGameSystemB.CharacterID GetCharacterID() { return BossGameSystemB.CharacterID.Player; }

    /// <summary>
    /// Vector���������ݒ�
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void SetDirection(int x, int y)
    {
        // �㉺�E���E���t���O����
        var isRight = false;
        var isUp = false;

        if (x > 0) isRight = true;
        else if (x < 0) isRight = false;
        else isRight = nowDirection == CharaDirection.RightUp || nowDirection == CharaDirection.RightDown;

        if (y > 0) isUp = true;
        else if (y < 0) isUp = false;
        else isUp = nowDirection == CharaDirection.RightUp || nowDirection == CharaDirection.LeftUp;


        // �E��
        if (isRight && isUp) SetDirection(CharaDirection.RightUp);
        // �E��
        else if (isRight && !isUp) SetDirection(CharaDirection.RightDown);
        // ����
        else if (!isRight && isUp) SetDirection(CharaDirection.LeftUp);
        // ����
        else SetDirection(CharaDirection.LeftDown);
    }

    /// <summary>
    /// ������ݒ�
    /// </summary>
    /// <param name="dir"></param>
    public void SetDirection(CharaDirection dir)
    {
        // �摜�ύX
        model.sprite = dir switch
        {
            CharaDirection.LeftUp => sp_leftup,
            CharaDirection.RightUp => sp_rightup,
            CharaDirection.RightDown => sp_rightdown,
            _ => sp_leftdown,
        };

        nowDirection = dir;
    }

    /// <summary>
    /// ���݂̌���
    /// </summary>
    /// <returns></returns>
    public CharaDirection GetDirection() { return nowDirection; }

    /// <summary>
    /// ���݈ʒu
    /// </summary>
    /// <returns></returns>
    public Vector2Int GetLocation() { return location; }

    #region �p�����[�^�Ǘ�

    /// <summary>
    /// �p�����[�^������
    /// </summary>
    public void InitParameter()
    {
        param_ATK_rate = 1f;
        param_SPD_rate = 1f;
        param_HP = param_HP_max;

        param_wait_time = CalcWaitTime(param_SPD_base);
    }

    /// <summary>
    /// ��{�̍s���ҋ@����
    /// </summary>
    /// <returns></returns>
    public int GetMaxWaitTime() { return CalcWaitTime(Mathf.FloorToInt(param_SPD_base * param_SPD_rate)); }
    /// <summary>
    /// ���݂̑҂�����
    /// </summary>
    /// <returns></returns>
    public int GetWaitTime() { return param_wait_time; }
    /// <summary>
    /// �҂����Ԃ����炷
    /// </summary>
    /// <param name="t"></param>
    public void DecreaseTime(int t) { param_wait_time -= t; }
    /// <summary>
    /// �҂����Ԃ��Đݒ�
    /// </summary>
    /// <param name="init"></param>
    public virtual void ResetTime(bool init = false) { param_wait_time = GetMaxWaitTime(); }
    /// <summary>
    /// ���x�ω�
    /// </summary>
    /// <param name="mul"></param>
    public void ChangeSpeed(float mul)
    {
        //todo:�҂����Ԃ�ϓ�

        param_SPD_rate *= mul;
    }

    /// <summary>
    /// �o�t�E�f�o�t�����Z�b�g
    /// </summary>
    public void ResetParam()
    {
        //todo:�҂����Ԃ�ϓ�

        param_ATK_rate = 1f;
        param_SPD_rate = 1f;
    }

    #endregion

    #endregion

    #region

    /// <summary>
    /// SPD�ɂ�肩����W���ҋ@����
    /// </summary>
    /// <param name="spd"></param>
    /// <returns></returns>
    protected static int CalcWaitTime(int spd)
    {
        var tmp = 100 - spd;
        if (tmp < 10) tmp = 10;
        return tmp;
    }

    #endregion

    #region �R���[�`��

    /// <summary>
    /// �����ړ�
    /// </summary>
    /// <param name="x">�ړ�����</param>
    /// <param name="y">�ړ�����</param>
    /// <returns></returns>
    public IEnumerator Walk(int x, int y)
    {
        /// �摜�ύX
        SetDirection(x, y);

        location.x += x;
        location.y += y;

        var deltaPos = new DeltaVector3();
        deltaPos.Set(transform.localPosition);
        deltaPos.MoveTo(BossGameSystemB.GetCellLocation(location.x, location.y), 0.2f, DeltaFloat.MoveType.LINE);
        while (deltaPos.IsActive())
        {
            yield return null;
            deltaPos.Update(Time.deltaTime);
            transform.localPosition = deltaPos.Get();
        }
    }

    /// <summary>
    /// �ӂ��Ƃ΂���
    /// </summary>
    /// <param name="x">�ړ�����</param>
    /// <param name="y">�ړ�����</param>
    /// <returns></returns>
    public IEnumerator Slide(int x, int y)
    {
        location.x += x;
        location.y += y;

        var deltaPos = new DeltaVector3();
        deltaPos.Set(transform.localPosition);
        deltaPos.MoveTo(BossGameSystemB.GetCellLocation(location.x, location.y), 0.05f, DeltaFloat.MoveType.LINE);
        while (deltaPos.IsActive())
        {
            yield return null;
            deltaPos.Update(Time.deltaTime);
            transform.localPosition = deltaPos.Get();
        }
    }

    /// <summary>
    /// �^�[���s��
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerator TurnProcess()
    {
        // �h����Ŏ���
        yield return new WaitForSeconds(1f);
    }

    #endregion
}
