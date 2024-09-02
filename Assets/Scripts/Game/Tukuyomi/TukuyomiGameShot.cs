using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����݂�����@�e
/// </summary>
public class TukuyomiGameShot : MonoBehaviour
{
    public enum ShotType
    {
        /// <summary>�v���C���[�̒e�@�U�����舵��</summary>
        Player = 0,
        /// <summary>���Ԃ̒e�@���炢���舵��</summary>
        Enemy,
    }

    /// <summary>�폜�\��</summary>
    public bool DestroyWait { get; set; } = false;

    /// <summary>���ł�������</summary>
    private Vector3 shotVec;

    /// <summary>�^�C�v</summary>
    private ShotType shotType;

    /// <summary>�V�X�e���A�N�Z�X�p</summary>
    public TukuyomiGameSystem system;

    /// <summary>
    ///  ����
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="vec"></param>
    /// <param name="enemy"></param>
    public void Shoot(Vector3 pos, Vector3 vec, ShotType type)
    {
        shotType = type;
        gameObject.SetActive(true);

        // �^�C�v�ݒ�
        if (type == ShotType.Player)
        {
            // �v���C���[�̒e��isTrigger�U������
            GetComponent<Collider2D>().isTrigger = true;
        }
        else
        {
            // �G�̒e�͒ʏ�R���W����
            GetComponent<Collider2D>().isTrigger = false;
        }

        // �p�x
        transform.localRotation = Util.GetRotateQuaternion(Util.GetRadianFromVector(vec));
        shotVec = vec;
        // �ʒu
        transform.localPosition = pos;
        StartCoroutine(ShootCoroutine());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShootCoroutine()
    {
        const float MaxX = Constant.SCREEN_WIDTH / 2f + 64f;
        const float MinX = -MaxX;
        const float MaxY = Constant.SCREEN_HEIGHT / 2f + 64f;
        const float MinY = -MaxY;

        while (true)
        {
            yield return null;
            var p = transform.localPosition + shotVec * Time.deltaTime;
            transform.localPosition = p;

            var wp = transform.position;
            if (wp.x > MaxX || wp.x < MinX || wp.y > MaxY || wp.y < MinY || DestroyWait) break;

            if (shotType == ShotType.Enemy)
            {
                // ��l���̈ʒu�ɓ��B
                if (system.reko.gameObject.activeSelf && p.y < 20f)
                {
                    system.PlayerDamage();
                    DestroyWait = true;
                }
            }
        }

        system.ShotRemove(this);
        // �O�ɍs�����������
        Destroy(gameObject);
    }

    /// <summary>
    /// �U���������炨�݂�������
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var shotParam = collision.GetComponent<TukuyomiGameShot>();
        if (shotParam == null) return; // �V���b�g�ȊO�i�����Ǝv���j
        shotParam.DestroyWait = true;

        DestroyWait = true;
    }
}
