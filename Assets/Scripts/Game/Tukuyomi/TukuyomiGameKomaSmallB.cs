using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����݂����㔼��@���R�}
/// </summary>
public class TukuyomiGameKomaSmallB : MonoBehaviour
{
    public const float KOMAB_SIZE = 45f;
    private const float OUT_Y = 400f;

    public TukuyomiGameSystem system;

    private TukuyomiGameSystem.Koma komaKind;
    public bool destroying { get; set; } = false;
    private int health;
    private TukuyomiGameKomaSmallB defenceKing = null;
    public bool working { get; set; }

    /// <summary>
    /// �ʏ�̔z�u�ŊJ�n
    /// </summary>
    /// <param name="root"></param>
    /// <param name="pos"></param>
    /// <param name="kind"></param>
    public void WorkStart(Vector3 root, Vector3 pos, TukuyomiGameSystem.Koma kind)
    {
        komaKind = kind;
        transform.localPosition = root;
        gameObject.SetActive(true);

        // �̗͐ݒ�
        health = kind switch
        {
            TukuyomiGameSystem.Koma.Kin => 15,
            _ => 10,
        };
        // �F�ݒ�
        GetComponent<SpriteRenderer>().color = TukuyomiGameKomaSmall.GetKomaColor(kind);

        StartCoroutine(WorkCoroutine(pos));
    }

    /// <summary>
    /// ���Ώۂ̉���ݒ�
    /// </summary>
    /// <param name="king"></param>
    public void SetDefenceKing(TukuyomiGameKomaSmallB king)
    {
        defenceKing = king;
    }

    /// <summary>
    /// �U�����������Ƃ�
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var shot = collision.GetComponent<TukuyomiGameShot>();
        if (shot == null) return;

        // �V���b�g����������
        shot.DestroyWait = true;
        health--;

        if (health == 0)
        {
            // �_���[�W�󂯂Ď��ʏꍇ�A
            GetComponent<Rigidbody2D>().simulated = false;
            system.resource.PlaySE(system.resource.se_koma_damage);

            // �������ʎ��̓V�X�e���ɒʒm
            if (komaKind == TukuyomiGameSystem.Koma.Ou)
                system.TukuyomiDamage();

            SelfDestroy();
        }
    }

    /// <summary>
    /// ������
    /// </summary>
    private void SelfDestroy()
    {
        if (destroying) return;
        destroying = true;
        StartCoroutine(SelfDestroyCoroutine());
    }

    /// <summary>
    /// �������폜
    /// </summary>
    /// <returns></returns>
    private IEnumerator SelfDestroyCoroutine()
    {
        var model = GetComponent<ModelUtil>();
        model.FadeOut(0.5f);
        yield return new WaitWhile(() => model.IsFading());

        system.BRemoveKoma(this);
        Destroy(gameObject);
    }

    /// <summary>
    /// �d��
    /// </summary>
    /// <returns></returns>
    private IEnumerator WorkCoroutine(Vector3 movePos)
    {
        var p = new DeltaVector3();
        p.Set(transform.localPosition);
        p.MoveTo(movePos, 0.2f, DeltaFloat.MoveType.LINE);
        while (p.IsActive())
        {
            yield return null;
            p.Update(Time.deltaTime);
            transform.localPosition = p.Get();
        }
        GetComponent<Rigidbody2D>().simulated = true;

        // �����͑����̑O�ɏo�ē���
        if (komaKind == TukuyomiGameSystem.Koma.Kin && defenceKing != null)
        {
            yield return new WaitForSeconds(0.2f);
            if (defenceKing.destroying || defenceKing.working)
            {
                defenceKing = null;
                yield return new WaitForSeconds(Util.RandomFloat(0.8f, 2f));
            }
            else
            {
                yield return MoveCoroutine(defenceKing.transform.position - new Vector3(0, KOMAB_SIZE), 150f);
                yield return new WaitUntil(() => defenceKing.destroying || defenceKing.working);
            }
        }
        else //���̓����_���ҋ@
            yield return new WaitForSeconds(Util.RandomFloat(0.8f, 3f));

        working = true;
        // �҂��Ă�ԂɎ��񂾂瓭���Ȃ�
        if (destroying) yield break;

        // �d��������
        switch (komaKind)
        {
            case TukuyomiGameSystem.Koma.Ou: yield return WorkOu(); break;
            case TukuyomiGameSystem.Koma.Kin: yield return WorkKin(); break;
            case TukuyomiGameSystem.Koma.Gin: yield return WorkGin(); break;
            case TukuyomiGameSystem.Koma.Kei: yield return WorkKei(); break;
            case TukuyomiGameSystem.Koma.Kyou: yield return WorkKyou(); break;
            case TukuyomiGameSystem.Koma.Hu: yield return WorkHu(); break;
        }

        // �I������������
        SelfDestroy();
    }

    #region �e�R�}�̎d��

    /// <summary>
    /// ���Ԃ��v�Z
    /// </summary>
    /// <param name="pos1"></param>
    /// <param name="pos2"></param>
    /// <param name="speed"></param>
    /// <returns></returns>
    private float CalcMoveTime(Vector3 pos1, Vector3 pos2, float speed)
    {
        var dist = (pos1 - pos2).magnitude;
        return dist / speed;
    }

    /// <summary>
    /// ��
    /// </summary>
    /// <returns></returns>
    private IEnumerator WorkOu()
    {
        var p = transform.position;
        while (p.y > -OUT_Y)
        {
            if (destroying) yield break;

            // �O�R�}�X���烉���_��
            var moveXRetu = 0;
            if (p.x <= (-KOMAB_SIZE * 3.9f)) moveXRetu = Util.RandomInt(0, 1);
            else if (p.x >= (KOMAB_SIZE * 3.9f)) moveXRetu = Util.RandomInt(-1, 0);
            else moveXRetu = Util.RandomInt(-1, 1);
            p.x += moveXRetu * KOMAB_SIZE;
            p.y -= KOMAB_SIZE;
            yield return MoveCoroutine(p, 120f);
            yield return new WaitForSeconds(0.2f);
        }
    }

    /// <summary>
    /// ��
    /// </summary>
    /// <returns></returns>
    private IEnumerator WorkKin()
    {
        var p = transform.position;
        while (p.y > -OUT_Y)
        {
            if (destroying) yield break;

            if (defenceKing != null)
            {
                // ��������ꍇ�������ē���
                var xLim = 80f * Time.deltaTime;
                p = transform.position;
                p.y -= 130f * Time.deltaTime;
                if (p.x < defenceKing.transform.position.x - xLim) p.x += xLim;
                else if (p.x > defenceKing.transform.position.x + xLim) p.x -= xLim;
                else p.x = defenceKing.transform.position.x;
                transform.position = p;
                yield return null;
                if (defenceKing.destroying) defenceKing = null;
                continue;
            }

            // �O�R�}�X���烉���_��
            var moveXRetu = 0;
            if (p.x <= (-KOMAB_SIZE * 3.9f)) moveXRetu = Util.RandomInt(0, 1);
            else if (p.x >= (KOMAB_SIZE * 3.9f)) moveXRetu = Util.RandomInt(-1, 0);
            else moveXRetu = Util.RandomInt(-1, 1);
            p.x += moveXRetu * KOMAB_SIZE;
            p.y -= KOMAB_SIZE;
            yield return MoveCoroutine(p, 120f);
            yield return new WaitForSeconds(0.2f);
        }
    }

    /// <summary>
    /// ��
    /// </summary>
    /// <returns></returns>
    private IEnumerator WorkGin()
    {
        var endPos = transform.position.y;
        // ���Ɉړ�
        var p = transform.position;
        p.y = -348f;
        transform.position = p;
        yield return new WaitForSeconds(0.5f);

        while (p.y < endPos)
        {
            if (destroying) yield break;

            // ���Q�}�X���烉���_��
            var moveXRetu = 0;
            if (p.x <= (-KOMAB_SIZE * 3.9f)) moveXRetu = 1;
            else if (p.x >= (KOMAB_SIZE * 3.9f)) moveXRetu = -1;
            else moveXRetu = Util.RandomCheck(50) ? 1 : -1;
            p.x += moveXRetu * KOMAB_SIZE;
            p.y += KOMAB_SIZE;
            yield return MoveCoroutine(p, 150f);
            yield return new WaitForSeconds(0.2f);
        }
    }

    /// <summary>
    /// �j�n
    /// </summary>
    /// <returns></returns>
    private IEnumerator WorkKei()
    {
        var p = transform.position;
        while (p.y > -OUT_Y)
        {
            if (destroying) yield break;

            // �j�n��тS�}�X���烉���_�� ������0123
            var moveMasu = 0;
            if (p.x <= (-KOMAB_SIZE * 3.9f)) moveMasu = Util.RandomInt(2, 3);
            else if (p.x <= (-KOMAB_SIZE * 2.9f)) moveMasu = Util.RandomInt(1, 3);
            else if (p.x >= KOMAB_SIZE * 2.9f) moveMasu = Util.RandomInt(0, 2);
            else if (p.x >= (KOMAB_SIZE * 3.9f)) moveMasu = Util.RandomInt(0, 1);
            else moveMasu = Util.RandomInt(0, 3);

            if (moveMasu == 0) p.x -= KOMAB_SIZE * 2f;
            else if (moveMasu == 1) p.x -= KOMAB_SIZE;
            else if (moveMasu == 2) p.x += KOMAB_SIZE;
            else p.x += KOMAB_SIZE * 2f;
            if (moveMasu == 0 || moveMasu == 3) p.y -= KOMAB_SIZE;
            else p.y -= KOMAB_SIZE * 2f;

            yield return MoveCoroutine(p, 180f);
            yield return new WaitForSeconds(0.3f);
        }
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <returns></returns>
    private IEnumerator WorkKyou()
    {
        // ���[�U�[����
        system.resource.PlaySE(system.resource.se_attack_effect_A);
        var laserRot = Mathf.Deg2Rad * transform.rotation.eulerAngles.z + Mathf.PI / 2f;
        system.CreateLaser(transform.position, laserRot);
        // �������ɉ�����
        var backRot = laserRot - Mathf.PI;
        var backPos = transform.position + Util.GetVector3IdentityFromRot(backRot) * 10f;
        var p = new DeltaVector3();
        p.Set(transform.position);
        p.MoveTo(backPos, 0.2f, DeltaFloat.MoveType.DECEL);
        while (p.IsActive())
        {
            yield return null;
            p.Update(Time.deltaTime);
            transform.position = p.Get();
        }

        yield return new WaitForSeconds(0.5f);
    }

    /// <summary>
    /// ��
    /// </summary>
    /// <returns></returns>
    private IEnumerator WorkHu()
    {
        // ��ʂ̉��܂ŕ��ʂɓ���
        var targetPos = new Vector3(transform.position.x, -OUT_Y);
        yield return MoveCoroutine(targetPos, 120f);
    }

    /// <summary>
    /// �����ړ�
    /// </summary>
    /// <param name="pos">�ړ���i���[���h���W�j</param>
    /// <param name="speed">���x</param>
    /// <returns></returns>
    private IEnumerator MoveCoroutine(Vector3 pos, float speed)
    {
        var p = new DeltaVector3();
        p.Set(transform.position);
        p.MoveTo(pos, CalcMoveTime(p.Get(), pos, speed), DeltaFloat.MoveType.LINE);
        while (p.IsActive())
        {
            yield return null;
            if (health == 0) break;
            p.Update(Time.deltaTime);
            transform.position = p.Get();
        }
    }



    #endregion
}
