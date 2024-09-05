using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ����݂����{��@�U���݂̂̑��
/// </summary>
public class TukuyomiGameKomaSmallC : MonoBehaviour
{
    /// <summary>
    /// ���
    /// </summary>
    public enum KomaCType : int
    {
        NormalHisya = 0,
        NormalKaku,
        Dmg1Kaku1,
        Dmg1Kaku2,
        Dmg2Hisya1,
        Dmg2Hisya2,
        Dmg2Hisya3,
        Dmg3Hisya1,
        Dmg3Hisya2,
        Dmg3Kaku,
        Dmg4Kaku,
        Dmg4Kyou,
    }

    private const float MISO_RIGHT_X = 250f;
    private const float MISO_LEFT_X = -250f;
    private const float MISO_TOP_Y = 3f;
    private readonly Vector3 FIELD_CENTER = new Vector3(0, -172f);
    private const float FIELD_R = 192f;

    public TukuyomiGameSystem system;

    private KomaCType type;
    private TukuyomiGameSystem.Koma komaKind;
    private List<BlastNode> nodes = new List<BlastNode>();
    private float headRot;

    /// <summary>
    /// ���̂ɂ�铮��p�����[�^���쐬
    /// </summary>
    /// <param name="ctype"></param>
    /// <param name="rootPos">�J�n���W</param>
    /// <param name="rot">�p�x</param>
    public void CreateParams(KomaCType ctype, Vector3 rootPos, float rot = Mathf.PI * 1.5f)
    {
        gameObject.SetActive(true);
        GetComponent<ModelUtil>().FadeOutImmediate();
        type = ctype;
        nodes.Clear();

        var tmp = rootPos;
        var tmpTime = 0f;
        switch (type)
        {
            case KomaCType.NormalHisya:
                komaKind = TukuyomiGameSystem.Koma.Hisya;
                // �ʏ�̔�ԁ@�J�n���W���牺�ɓ������E�ɏՌ��g
                while (tmp.y > -Constant.SCREEN_HEIGHT * 0.5f - 50f)
                {
                    tmpTime += 0.1f;
                    tmp.y -= TukuyomiGameKomaSmallB.KOMAB_SIZE * 2f;
                    nodes.Add(new BlastNode(tmp, tmpTime, true));
                }
                break;
            case KomaCType.NormalKaku:
                komaKind = TukuyomiGameSystem.Koma.Kaku;
                // �ʏ�̊p�@�J�n���W����E���ɓ����E��A�����ɏՌ��g
                var first = true;
                while (tmp.y > -Constant.SCREEN_HEIGHT * 0.5f - 50f)
                {
                    tmpTime += 0.1f;
                    tmp.x += TukuyomiGameKomaSmallB.KOMAB_SIZE * 1.4f;
                    tmp.y -= TukuyomiGameKomaSmallB.KOMAB_SIZE * 1.4f;
                    nodes.Add(new BlastNode(tmp, tmpTime, true, first ? 0 : 1));
                    first = false;
                }
                break;
            case KomaCType.Dmg1Kaku1:
            case KomaCType.Dmg1Kaku2:   // �_���[�W1�̊p1 ���v���E�����v���
                {
                    komaKind = TukuyomiGameSystem.Koma.Kaku;
                    var hugo = (type == KomaCType.Dmg1Kaku1) ? 1 : -1;
                    rootPos = new Vector3(FIELD_CENTER.x - hugo * FIELD_R, FIELD_CENTER.y + FIELD_R * 2f);
                    rootPos.x += Util.RandomFloat(-40f, 40f);
                    tmp = rootPos;
                    // �E����
                    for (var i = 0; i < 3; ++i)
                    {
                        tmpTime += 0.08f;
                        tmp.x += hugo * FIELD_R / 3f;
                        tmp.y -= FIELD_R / 3f;
                        nodes.Add(new BlastNode(tmp, tmpTime, false));
                    }
                    for (var i = 0; i < 3; ++i)
                    {
                        tmpTime += 0.08f;
                        tmp.x += hugo * FIELD_R / 3f;
                        tmp.y -= FIELD_R / 3f;
                        nodes.Add(new BlastNode(tmp, tmpTime, true, hugo));
                    }
                    // ������
                    for (var i = 0; i < 3; ++i)
                    {
                        tmpTime += 0.08f;
                        tmp.x -= hugo * FIELD_R / 3f;
                        tmp.y -= FIELD_R / 3f;
                        nodes.Add(new BlastNode(tmp, tmpTime, true, -hugo));
                    }
                    // �����
                    for (var i = 0; i < 3; ++i)
                    {
                        tmpTime += 0.08f;
                        tmp.x -= hugo * FIELD_R / 3f;
                        tmp.y += FIELD_R / 3f;
                        nodes.Add(new BlastNode(tmp, tmpTime, true, hugo));
                    }
                    // �E���
                    for (var i = 0; i < 3; ++i)
                    {
                        tmpTime += 0.08f;
                        tmp.x += hugo * FIELD_R / 3f;
                        tmp.y += FIELD_R / 3f;
                        nodes.Add(new BlastNode(tmp, tmpTime, true, -hugo));
                    }
                }
                break;
            case KomaCType.Dmg2Hisya1:
            case KomaCType.Dmg2Hisya2:
            case KomaCType.Dmg2Hisya3:  // �_���[�W2�̔�ԁ@���@���@��
                {
                    komaKind = TukuyomiGameSystem.Koma.Hisya;
                    var hugo = (type == KomaCType.Dmg2Hisya2) ? -1 : 1;
                    rootPos.y = MISO_TOP_Y;
                    if (type == KomaCType.Dmg2Hisya2) rootPos.y += 40f;
                    else if (type == KomaCType.Dmg2Hisya3) rootPos.y += 80f;
                    rootPos.x = hugo * MISO_RIGHT_X;
                    tmp = rootPos;
                    for (var i = 0; i < 7; ++i)
                    {
                        tmpTime += 0.1f;
                        tmp.x += -hugo * 50f;
                        nodes.Add(new BlastNode(tmp, tmpTime, true));
                    }
                }
                break;
            case KomaCType.Dmg3Hisya1:
            case KomaCType.Dmg3Hisya2:
                {
                    komaKind = TukuyomiGameSystem.Koma.Hisya;
                    var hugo = (type == KomaCType.Dmg3Hisya1) ? 1 : -1;
                    rootPos.x = hugo * MISO_RIGHT_X + 100f;
                    rootPos.y = FIELD_CENTER.y + Util.RandomFloat(-110f, 110f);
                    tmp = rootPos;
                    tmp.x = hugo * MISO_RIGHT_X;
                    tmpTime += 0.2f;
                    nodes.Add(new BlastNode(tmp, tmpTime, true, hugo));
                    nodes.Add(new BlastNode(tmp, tmpTime + 0.5f, false));
                }
                break;
            case KomaCType.Dmg3Kaku:
                {
                    komaKind = TukuyomiGameSystem.Koma.Kaku;
                    rootPos.x = Util.RandomFloat(-200f, 200f);
                    rootPos.y = MISO_TOP_Y + 100f;
                    tmp = rootPos;
                    tmp.y = MISO_TOP_Y;
                    tmpTime += 0.2f;
                    nodes.Add(new BlastNode(tmp, tmpTime, true));
                    nodes.Add(new BlastNode(tmp, tmpTime + 0.5f, false));
                }
                break;
            case KomaCType.Dmg4Kaku:
                // �ォ�痈�Ē����ɉ�������
                {
                    komaKind = TukuyomiGameSystem.Koma.Kaku;
                    rootPos.x = 0f;
                    rootPos.y = FIELD_CENTER.y + 540f;
                    tmp = rootPos;
                    for (var i = 0; i < 9; ++i)
                    {
                        tmp.y -= 60f;
                        tmpTime += 0.2f;
                        nodes.Add(new BlastNode(tmp, tmpTime, true));
                    }
                }
                break;
            case KomaCType.Dmg4Kyou:
                // �p�x�Ō����1��
                {
                    komaKind = TukuyomiGameSystem.Koma.Kyou;
                    var identity = Util.GetVector3IdentityFromRot(rot);
                    rootPos = FIELD_CENTER - identity * FIELD_R * 1.5f;
                    tmp = FIELD_CENTER - identity * FIELD_R;
                    nodes.Add(new BlastNode(tmp, 0.2f, false));
                    nodes.Add(new BlastNode(tmp, 0.6f, true));
                }
                break;
        }

        transform.rotation = Util.GetRotateQuaternion(rot - Mathf.PI * 0.5f);
        transform.position = rootPos;
    }

    /// <summary>
    /// �I��\��
    /// </summary>
    public IEnumerator ShowWarning()
    {
        var color = TukuyomiGameKomaSmall.GetKomaColor(komaKind);
        var maxTime = 0f;
        foreach (var node in nodes)
        {
            if (!node.blast) continue;

            system.CreateWarning(node.pos, color, node.time);
            if (node.time > maxTime) maxTime = node.time;
        }
        yield return new WaitForSeconds(maxTime + 0.7f);
    }

    /// <summary>
    /// �U�����s
    /// </summary>
    /// <returns></returns>
    public IEnumerator ExecAttackCoroutine()
    {
        var model = GetComponent<ModelUtil>();
        model.FadeIn(nodes[0].time, TukuyomiGameKomaSmall.GetKomaColor(komaKind));
        // �m�[�h���ƂɈړ����Ȃ���U������
        var nowTime = 0f;
        var p = new DeltaVector3();
        p.Set(transform.position);
        foreach (var node in nodes)
        {
            // �ړ�
            p.MoveTo(node.pos, node.time - nowTime, DeltaFloat.MoveType.LINE);
            while (p.IsActive())
            {
                yield return null;
                p.Update(Time.deltaTime);
                transform.position = p.Get();
            }
            nowTime = node.time;

            if (!node.blast) continue;
            // �U��
            NodeAttack(node);
        }

        if (type == KomaCType.Dmg4Kyou)
        {
            // ���Ԃ͏����o�b�N KomaSmallB�̍��ԏ����Ɠ���
            var backRot = headRot - Mathf.PI;
            var backPos = transform.position + Util.GetVector3IdentityFromRot(backRot) * 10f;
            p.Set(transform.position);
            p.MoveTo(backPos, 0.2f, DeltaFloat.MoveType.DECEL);
            while (p.IsActive())
            {
                yield return null;
                p.Update(Time.deltaTime);
                transform.position = p.Get();
            }
        }

        model.FadeOut(0.5f);
        yield return new WaitWhile(() => model.IsFading());
        Destroy(gameObject);
    }

    /// <summary>
    /// �U��
    /// </summary>
    /// <param name="node"></param>
    private void NodeAttack(BlastNode node)
    {
        system.resource.PlaySE(system.resource.se_attack_effect_A);
        switch (type)
        {
            case KomaCType.NormalHisya:
                // ���E
                system.CreateBlast(node.pos, 0f);
                system.CreateBlast(node.pos, Mathf.PI);
                break;
            case KomaCType.NormalKaku:
                // �E��A����
                if (node.generalParam == 0)
                {
                    // �ŏ��ɉE���Ɍ����Ă���
                    system.CreateBlast(node.pos, Mathf.PI * 1.75f);
                }
                else
                {
                    system.CreateBlast(node.pos, Mathf.PI * 0.25f);
                    system.CreateBlast(node.pos, Mathf.PI * 1.25f);
                }
                break;
            case KomaCType.Dmg1Kaku1:
            case KomaCType.Dmg1Kaku2:
                // 1�̏ꍇ�͉E�㍶���A-1�͉E������
                if (node.generalParam == 1)
                {
                    system.CreateBlast(node.pos, Mathf.PI * 0.25f);
                    system.CreateBlast(node.pos, Mathf.PI * 1.25f);
                }
                else
                {
                    system.CreateBlast(node.pos, Mathf.PI * 0.75f);
                    system.CreateBlast(node.pos, Mathf.PI * 1.75f);
                }
                break;
            case KomaCType.Dmg2Hisya1:
            case KomaCType.Dmg2Hisya2:
            case KomaCType.Dmg2Hisya3:
                // �c
                system.CreateBlast(node.pos, Mathf.PI * 1.5f);
                break;
            case KomaCType.Dmg3Hisya1:
                // ����
                system.CreateBlast(node.pos, Mathf.PI);
                break;
            case KomaCType.Dmg3Hisya2:
                // �E��
                system.CreateBlast(node.pos, 0f);
                break;
            case KomaCType.Dmg3Kaku:
            case KomaCType.Dmg4Kaku:
                // ��2����
                system.CreateBlast(node.pos, Mathf.PI * 1.25f);
                system.CreateBlast(node.pos, Mathf.PI * 1.75f);
                break;
            case KomaCType.Dmg4Kyou:
                {
                    var laserRot = Mathf.Deg2Rad * transform.rotation.eulerAngles.z + Mathf.PI / 2f;
                    system.CreateLaser(transform.position, laserRot);
                }
                break;
        }
    }

    /// <summary>
    /// �Ռ��g���o���ꏊ�p�����[�^
    /// </summary>
    private class BlastNode
    {
        public Vector3 pos;
        public float time;
        public bool blast;
        public int generalParam;

        public BlastNode(Vector3 _pos, float _time, bool _blast, int _param = 0)
        {
            pos = _pos;
            time = _time;
            blast = _blast;
            generalParam = _param;
        }
    }
}
