using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// �{�X���b�V���@�j�Z�s�G�[��
/// </summary>
public class PierreGameBEnemy : PierreGameBPierreBase
{
    #region �����o�[

    public AudioClip se_attack;

    #endregion

    #region �萔

    /// <summary>�ő�HP</summary>
    private const int HP_MAX = 150;

    /// <summary>�t�F�[�Y�P�ɂȂ�HP</summary>
    private const int HP_PHASE1 = 80;
    /// <summary>�t�F�[�Y�Q�ɂȂ�HP</summary>
    private const int HP_PHASE2 = 40;

    /// <summary>�t�F�[�Y�P��{�ʒu</summary>
    private readonly Vector3 PHASE1_BASE_POS = new Vector3(300f, PierreGameSystemB.FIELD_CENTER_Y);

    #endregion

    #region �ϐ�

    /// <summary>�̗�</summary>
    private int hp;

    /// <summary>����s��AI</summary>
    private IEnumerator ai = null;
    /// <summary>���@�_��AI</summary>
    private IEnumerator targetAi = null;

    /// <summary>���o���U���Ȃǎ~�߂�</summary>
    private bool effecting = false;

    /// <summary>0:�����@1:���_���[�W�@2:�Ō�</summary>
    private int phase = 0;

    #endregion

    #region ���

    /// <summary>
    /// ������
    /// </summary>
    protected override void Start()
    {
        base.Start();

        hp = HP_MAX;

        ai = AICoroutineA();
        StartCoroutine(ai);
        targetAi = AutoTargetCoroutine();
        StartCoroutine(targetAi);
    }

    #endregion

    #region �C�x���g

    /// <summary>
    /// �{�[�����炢
    /// </summary>
    /// <param name="ball"></param>
    protected override void OnBallHit(PierreGameBallB ball)
    {
        base.OnBallHit(ball);

        if (ball.attacktype != PierreGameBallB.AttackType.Player) return;
        if (effecting) return;

        hp -= ball.GetPower();
        system.DisplayHP(hp <= 0 ? 0 : ((float)hp / HP_MAX));
        if (hp <= 0)
        {
            effecting = true;
            system.DeleteAllBall(PierreGameBallB.AttackType.Enemy);
            GetComponent<Collider2D>().enabled = false;
            //���S����
            StopCoroutine(ai);
            StopCoroutine(targetAi);
            Death();
        }
        else if (hp <= HP_PHASE2 && phase < 2)
        {
            effecting = true;
            system.DeleteAllBall(PierreGameBallB.AttackType.Enemy);
            // �t�F�[�Y�Q�Ɉڍs
            StopCoroutine(ai);
            ai = AICoroutineC();
            StartCoroutine(PhaseChangeEffectCoroutine(2));
        }
        else if (hp <= HP_PHASE1 && phase < 1)
        {
            effecting = true;
            system.DeleteAllBall(PierreGameBallB.AttackType.Enemy);
            // �t�F�[�Y�P�Ɉڍs
            StopCoroutine(ai);
            ai = AICoroutineB();
            StartCoroutine(PhaseChangeEffectCoroutine(1));
        }
        else
        {
        }

        ball.DestroyWait();
    }

    /// <summary>
    /// ���S���o
    /// </summary>
    /// <returns></returns>
    private void Death()
    {
        var util = GetComponent<ModelUtil>();
        util.FadeOut(1f);
        system.EndGame(true);
    }

    #endregion

    #region �R���[�`��

    /// <summary>
    /// ���@�_�����˃R���[�`���@��ɉғ�
    /// </summary>
    /// <returns></returns>
    private IEnumerator AutoTargetCoroutine()
    {
        yield return new WaitUntil(() => system.State == PierreGameSystemB.GameState.PLAY);

        while (true)
        {
            yield return new WaitForSeconds(2.2f);
            if (effecting) continue;

            var targetVec = (system.pierreA.GetPos() - GetPos()).normalized;
            var leftVec = Util.GetRotateQuaternion(Mathf.PI / 8f) * targetVec;
            var rightVec = Util.GetRotateQuaternion(-Mathf.PI / 8f) * targetVec;

            if (phase == 0)
            {
                // ����
                system.GenerateBall(GetPos(), targetVec * 150f, PierreGameBallB.AttackType.Enemy, Color.gray);
                system.GenerateBall(GetPos(), targetVec * 200f, PierreGameBallB.AttackType.Enemy, Color.gray);
                system.GenerateBall(GetPos(), targetVec * 250f, PierreGameBallB.AttackType.Enemy, Color.gray);
            }
            else if (phase == 1)
            {
                // �t�F�[�Y�P
                system.GenerateBall(GetPos(), rightVec * 200f, PierreGameBallB.AttackType.Enemy, Color.gray);
                system.GenerateBall(GetPos(), targetVec * 200f, PierreGameBallB.AttackType.Enemy, Color.gray);
                system.GenerateBall(GetPos(), leftVec * 200f, PierreGameBallB.AttackType.Enemy, Color.gray);
            }
            else
            {
                // �t�F�[�Y�Q
                system.GenerateBall(GetPos(), rightVec * 200f, PierreGameBallB.AttackType.Enemy, Color.gray);
                system.GenerateBall(GetPos(), targetVec * 200f, PierreGameBallB.AttackType.Enemy, Color.gray);
                system.GenerateBall(GetPos(), leftVec * 200f, PierreGameBallB.AttackType.Enemy, Color.gray);
                system.GenerateBall(GetPos(), rightVec * 140f, PierreGameBallB.AttackType.Enemy, Color.gray);
                system.GenerateBall(GetPos(), targetVec * 140f, PierreGameBallB.AttackType.Enemy, Color.gray);
                system.GenerateBall(GetPos(), leftVec * 140f, PierreGameBallB.AttackType.Enemy, Color.gray);
            }
        }
    }

    /// <summary>
    /// �t�F�[�Y�؂�ւ����o�R���[�`��
    /// </summary>
    /// <param name="p"></param>
    /// <returns></returns>
    private IEnumerator PhaseChangeEffectCoroutine(int p)
    {
        phase = p;
        system.ShowPhaseMessage(p);
        var pos = new DeltaVector3();
        pos.Set(transform.localPosition);
        if (p == 1)
        {
            // �t�F�[�Y�P�J�n
            pos.MoveTo(PHASE1_BASE_POS, 1f, DeltaFloat.MoveType.DECEL);
        }
        else if (p == 2)
        {
            // �t�F�[�Y�Q�J�n
            pos.MoveTo(new Vector3(0, PierreGameSystemB.FIELD_CENTER_Y), 1f, DeltaFloat.MoveType.DECEL);
        }

        while (pos.IsActive())
        {
            yield return null;
            pos.Update(Time.deltaTime);
            transform.localPosition = pos.Get();
        }

        yield return new WaitForSeconds(2.5f);
        StartCoroutine(ai);
        effecting = false;
    }

    /// <summary>
    /// �����s��
    /// </summary>
    /// <returns></returns>
    private IEnumerator AICoroutineA()
    {
        var sound = ManagerSceneScript.GetInstance().soundMan;
        const float pi6 = Mathf.PI / 6f;
        var isUp = true;

        yield return new WaitUntil(() => system.State == PierreGameSystemB.GameState.PLAY);

        var deltaPos = new DeltaVector3();
        while (true)
        {
            yield return new WaitForSeconds(1f);
            isUp = !isUp;

            deltaPos.Set(GetPos());
            deltaPos.MoveTo(new Vector3(Util.RandomFloat(200f, 500f), PierreGameSystemB.FIELD_CENTER_Y + (isUp ? 100f : -100f) + Util.RandomFloat(-50f, 50f)),
                0.5f, DeltaFloat.MoveType.DECEL);
            while (deltaPos.IsActive())
            {
                yield return null;
                deltaPos.Update(Time.deltaTime);
                transform.localPosition = deltaPos.Get();
            }

            sound.PlaySE(se_attack);
            var rot = Mathf.PI / 12f;
            for (var i = 0; i < 12; ++i)
            {
                var spd = Util.GetVector3IdentityFromRot(rot) * 150f;
                system.GenerateBall(GetPos(), spd, PierreGameBallB.AttackType.Enemy, Color.gray);

                rot += pi6;
            }
        }
    }

    /// <summary>
    /// �t�F�[�Y�P
    /// </summary>
    /// <returns></returns>
    private IEnumerator AICoroutineB()
    {
        var sound = ManagerSceneScript.GetInstance().soundMan;
        const float _2PI = Mathf.PI * 2f;
        const float ROT_SPD = -Mathf.PI / 2f;
        const float RING_INTERVAL = 4f;

        var length = new DeltaFloat();
        length.Set(0f);
        var len_expand = false;
        var rot = 0f;
        var ringTime = RING_INTERVAL;
        while (true)
        {
            yield return null;
            length.Update(Time.deltaTime);
            if (!length.IsActive())
            {
                if (len_expand)
                {
                    length.MoveTo(0f, 6f, DeltaFloat.MoveType.ACCEL);
                }
                else
                {
                    length.MoveTo(150f, 6f, DeltaFloat.MoveType.DECEL);
                    rot += _2PI;
                }
                len_expand = !len_expand;
            }
            rot += ROT_SPD * Time.deltaTime;
            if (rot < 0f) rot += _2PI;

            // ��]�ړ�
            var addPos = Util.GetVector3IdentityFromRot(rot) * length.Get();
            transform.localPosition = PHASE1_BASE_POS + addPos;

            // ����
            ringTime -= Time.deltaTime;
            if (ringTime < 0f)
            {
                sound.PlaySE(se_attack);
                ringTime += RING_INTERVAL;
                GeneratePhase1Ring();
            }
        }
    }

    /// <summary>
    /// �t�F�[�Y�P�p�ւ����U��
    /// </summary>
    private void GeneratePhase1Ring()
    {
        const int RING_COUNT = 16;
        var rot90Quat = Util.GetRotateQuaternion(Mathf.PI / 2f);
        var rot20Quat = Util.GetRotateQuaternion(Mathf.PI * 8f / 9f);

        for (var i = 0; i < RING_COUNT; ++i)
        {
            var ballPosIdentity = Util.GetVector3IdentityFromRot(Mathf.PI * 2 / RING_COUNT * i);
            var ballVecIdentity = rot20Quat * ballPosIdentity;

            system.GenerateBall(system.pierreA.GetPos() + ballPosIdentity * 300f,
                ballVecIdentity * 200f,
                PierreGameBallB.AttackType.Enemy, Color.gray);
        }
    }

    /// <summary>
    /// �t�F�[�Y�Q
    /// </summary>
    /// <returns></returns>
    private IEnumerator AICoroutineC()
    {
        var sound = ManagerSceneScript.GetInstance().soundMan;
        const float _2PI = Mathf.PI * 2f;
        var rotBase = 0f;
        var seCount = 1;
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            rotBase -= Mathf.PI / 3f / 20f;
            if (rotBase < 0f) rotBase += _2PI;

            if (seCount == 1)
            {
                seCount = 0;
            }
            else
            {
                seCount = 1;
                sound.PlaySE(se_attack);
            }

            // �U���쐬
            var rotAdd = _2PI / 6;
            var rot = rotBase;
            for (var i = 0; i < 6; ++i)
            {
                var vec = Util.GetVector3IdentityFromRot(rot) * 150f;
                system.GenerateBall(GetPos(), vec, PierreGameBallB.AttackType.Enemy, Color.gray);

                rot -= rotAdd;
                if (rot < 0f) rot += _2PI;
            }
        }
    }

    #endregion
}
