using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ボスラッシュ　ニセピエール
/// </summary>
public class PierreGameBEnemy : PierreGameBPierreBase
{
    #region メンバー

    public AudioClip se_attack;

    #endregion

    #region 定数

    /// <summary>最大HP</summary>
    private const int HP_MAX = 150;

    /// <summary>フェーズ１になるHP</summary>
    private const int HP_PHASE1 = 80;
    /// <summary>フェーズ２になるHP</summary>
    private const int HP_PHASE2 = 40;

    /// <summary>フェーズ１基本位置</summary>
    private readonly Vector3 PHASE1_BASE_POS = new Vector3(300f, PierreGameSystemB.FIELD_CENTER_Y);

    #endregion

    #region 変数

    /// <summary>体力</summary>
    private int hp;

    /// <summary>特殊行動AI</summary>
    private IEnumerator ai = null;
    /// <summary>自機狙いAI</summary>
    private IEnumerator targetAi = null;

    /// <summary>演出中攻撃など止める</summary>
    private bool effecting = false;

    /// <summary>0:初期　1:中ダメージ　2:最後</summary>
    private int phase = 0;

    #endregion

    #region 基底

    /// <summary>
    /// 初期化
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

    #region イベント

    /// <summary>
    /// ボールくらい
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
            //死亡処理
            StopCoroutine(ai);
            StopCoroutine(targetAi);
            Death();
        }
        else if (hp <= HP_PHASE2 && phase < 2)
        {
            effecting = true;
            system.DeleteAllBall(PierreGameBallB.AttackType.Enemy);
            // フェーズ２に移行
            StopCoroutine(ai);
            ai = AICoroutineC();
            StartCoroutine(PhaseChangeEffectCoroutine(2));
        }
        else if (hp <= HP_PHASE1 && phase < 1)
        {
            effecting = true;
            system.DeleteAllBall(PierreGameBallB.AttackType.Enemy);
            // フェーズ１に移行
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
    /// 死亡演出
    /// </summary>
    /// <returns></returns>
    private void Death()
    {
        var util = GetComponent<ModelUtil>();
        util.FadeOut(1f);
        system.EndGame(true);
    }

    #endregion

    #region コルーチン

    /// <summary>
    /// 自機狙い発射コルーチン　常に稼働
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
                // 初期
                system.GenerateBall(GetPos(), targetVec * 150f, PierreGameBallB.AttackType.Enemy, Color.gray);
                system.GenerateBall(GetPos(), targetVec * 200f, PierreGameBallB.AttackType.Enemy, Color.gray);
                system.GenerateBall(GetPos(), targetVec * 250f, PierreGameBallB.AttackType.Enemy, Color.gray);
            }
            else if (phase == 1)
            {
                // フェーズ１
                system.GenerateBall(GetPos(), rightVec * 200f, PierreGameBallB.AttackType.Enemy, Color.gray);
                system.GenerateBall(GetPos(), targetVec * 200f, PierreGameBallB.AttackType.Enemy, Color.gray);
                system.GenerateBall(GetPos(), leftVec * 200f, PierreGameBallB.AttackType.Enemy, Color.gray);
            }
            else
            {
                // フェーズ２
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
    /// フェーズ切り替え演出コルーチン
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
            // フェーズ１開始
            pos.MoveTo(PHASE1_BASE_POS, 1f, DeltaFloat.MoveType.DECEL);
        }
        else if (p == 2)
        {
            // フェーズ２開始
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
    /// 初期行動
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
    /// フェーズ１
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

            // 回転移動
            var addPos = Util.GetVector3IdentityFromRot(rot) * length.Get();
            transform.localPosition = PHASE1_BASE_POS + addPos;

            // 発射
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
    /// フェーズ１用輪っか攻撃
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
    /// フェーズ２
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

            // ６発作成
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
