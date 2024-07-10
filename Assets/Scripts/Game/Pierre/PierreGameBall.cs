using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// �]����{�[��
/// </summary>
public class PierreGameBall : PierreGameRoadObject
{
    #region �萔
    public enum BallType : int
    {
        /// <summary>�ʏ�</summary>
        Normal = 0,
        /// <summary>���E�ɓ���</summary>
        Drift,
        /// <summary>�ϑ�</summary>
        SpeedGear,
    }
    /// <summary>�^�C�v</summary>
    private BallType type;

    /// <summary>��{�X�s�[�h</summary>
    private const float SPEEDX = 400f;
    #endregion

    #region �����o�[
    /// <summary>�{�[��</summary>
    public SpriteRenderer image = null;
    #endregion

    #region �ϐ�
    /// <summary>�q�b�g��O�ɏo��</summary>
    private bool goOut = false;

    /// <summary>���x</summary>
    private Vector2 velocity;
    /// <summary>�ϑ�</summary>
    private DeltaFloat gear = null;
    /// <summary>������</summary>
    private bool accel = true;
    #endregion

    /// <summary>
    /// �t���[������
    /// </summary>
    override public void Update()
    {
        base.Update();

        // �ϑ��{�[���̔���
        if (type == BallType.SpeedGear)
        {
            gear.Update(Time.deltaTime);
            velocity.x = gear.Get();

            if (!gear.IsActive())
            {
                if (accel)
                {
                    gear.MoveTo(SPEEDX * 0.5f, 0.3f, DeltaFloat.MoveType.BOTH);
                    accel = false;
                }
                else
                {
                    gear.MoveTo(SPEEDX * 1.5f, 0.3f, DeltaFloat.MoveType.BOTH);
                    accel = true;
                }
            }
        }

        // �ʒu�X�V
        transform.Translate(new Vector3(velocity.x, 0, 0) * Time.deltaTime);

        if (goOut)
        {
            // �ޏ�t�F�[�Y
            transform.Translate(new Vector3(0, -SPEEDX / 2 * Time.deltaTime, 0));
        }
        else if (type == BallType.Drift)
        {
            // �ޏ�ȊO�ō��E�ړ��{�[���̌v�Z
            SetFarPosition(GetFarPosition() + velocity.y * Time.deltaTime);
            if (GetFarPosition() >= ROAD_FAR_MAX || GetFarPosition() <= -ROAD_FAR_MAX)
            {
                velocity.y *= -1f;
            }
        }

        // �E�܂ōs�����������
        if (transform.localPosition.x > Constant.SCREEN_WIDTH * 0.6f)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// �q�b�g��O�ɏo��
    /// </summary>
    public void GoOut()
    {
        goOut = true;
        GetComponent<Collider2D>().enabled = false;
    }

    /// <summary>
    /// �^�C�v�w��
    /// </summary>
    /// <param name="_type"></param>
    public void SetBallType(BallType _type)
    {
        type = _type;

        switch (type)
        {
            case BallType.Normal: // �ʏ�
                velocity = new Vector2(SPEEDX, 0);
                break;
            case BallType.Drift: // ���E�ړ�
                var pm = Util.RandomInt(0, 1) == 0 ? 1 : -1;
                velocity = new Vector2(SPEEDX * 0.8f, SPEEDX * Util.RandomFloat(0.3f, 0.5f) * pm);
                break;
            default: // �ϑ�
                velocity = new Vector2(SPEEDX, 0);
                gear = new DeltaFloat();
                gear.Set(SPEEDX * 1f);
                accel = false;
                break;
        }
        image.color = CalcBallColor(type);
    }

    /// <summary>
    /// �{�[���̐F
    /// </summary>
    /// <param name="_type"></param>
    /// <returns></returns>
    public static Color CalcBallColor(BallType _type)
    {
        return _type switch
        {
            BallType.Normal => Color.cyan,
            BallType.Drift => Color.yellow,
            _ => Color.magenta,
        };
    }
}
