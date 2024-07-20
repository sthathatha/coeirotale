using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ボスラッシュ　ピエールプレイヤー
/// </summary>
public class PierreGameBPlayer : PierreGameBPierreBase
{
    #region メンバー

    public AudioClip se_death;

    #endregion

    #region 定数

    /// <summary>通常移動</summary>
    private const float SPEED_NORMAL = 300f;
    /// <summary>低速移動</summary>
    private const float SPEED_SLOW = 120f;

    /// <summary>低速移動になるまでの押しっぱなし時間</summary>
    private const float SLOW_PRESS_TIME = 0.3f;

    /// <summary>押しっぱなしで連射</summary>
    private const float SHOOT_INTERVAL = 0.2f;

    #endregion

    #region 変数

    /// <summary>低速モード</summary>
    private bool isSlowMode = false;

    /// <summary>押しっぱなし判定</summary>
    private float pressInterval = 0f;

    /// <summary>ダメージで変化</summary>
    private int life = 3;

    /// <summary>無敵時間</summary>
    private bool invincible = false;

    /// <summary>死亡エフェクト中は操作不能</summary>
    private bool effecting = false;

    #endregion

    #region 基底

    /// <summary>
    /// 開始時
    /// </summary>
    protected override void Start()
    {
        base.Start();

        spr_underBall.color = Color.cyan;
        StartCoroutine(SlowCheckCoroutine());
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    protected override void Update()
    {
        var rigid = GetComponent<Rigidbody2D>();

        if (system.State != PierreGameSystemB.GameState.PLAY)
        {
            rigid.velocity = Vector3.zero;
            base.Update();
            return;
        }

        var input = InputManager.GetInstance();

        if (!effecting)
        {
            #region 移動
            var lr = false;
            var ud = false;
            var spd = Vector3.zero;
            if (input.GetKey(InputManager.Keys.Up))
            {
                spd.y = 1;
                ud = true;
            }
            else if (input.GetKey(InputManager.Keys.Down))
            {
                spd.y = -1;
                ud = true;
            }
            if (input.GetKey(InputManager.Keys.Right))
            {
                spd.x = 1;
                lr = true;
            }
            else if (input.GetKey(InputManager.Keys.Left))
            {
                spd.x = -1;
                lr = true;
            }
            if (lr || ud)
            {
                if (lr && ud)
                {
                    // 斜めは√２
                    spd /= Mathf.Sqrt(2f);
                }

                // 入力によって速度設定
                spd *= isSlowMode ? SPEED_SLOW : SPEED_NORMAL;
            }

            rigid.velocity = spd;
            #endregion

            #region 攻撃
            if (input.GetKeyPress(InputManager.Keys.South))
            {
                Shoot();
                pressInterval = SHOOT_INTERVAL;
            }
            else if (input.GetKey(InputManager.Keys.South))
            {
                pressInterval -= Time.deltaTime;
                if (pressInterval <= 0f)
                {
                    Shoot();
                    pressInterval = SHOOT_INTERVAL;
                }
            }
            #endregion
        }

        base.Update();
    }

    #endregion

    #region 機能メソッド

    /// <summary>
    /// 発射1発
    /// </summary>
    private void Shoot()
    {
        switch (life)
        {
            case 3:
                system.GenerateBall(transform.localPosition, new Vector3(1000f, 0f), PierreGameBallB.AttackType.Player, Color.cyan, 1);
                break;
            case 2:
                system.GenerateBall(transform.localPosition, new Vector3(900f, 180f), PierreGameBallB.AttackType.Player, Color.yellow, 1);
                system.GenerateBall(transform.localPosition, new Vector3(1000f, 0f), PierreGameBallB.AttackType.Player, Color.yellow, 1);
                system.GenerateBall(transform.localPosition, new Vector3(900f, -180f), PierreGameBallB.AttackType.Player, Color.yellow, 1);
                break;
            case 1:
                system.GenerateBall(transform.localPosition, new Vector3(900f, 180f), PierreGameBallB.AttackType.Player, Color.magenta, 2);
                system.GenerateBall(transform.localPosition, new Vector3(1000f, 0f), PierreGameBallB.AttackType.Player, Color.magenta, 2);
                system.GenerateBall(transform.localPosition, new Vector3(900f, -180f), PierreGameBallB.AttackType.Player, Color.magenta, 2);
                break;
        }
    }

    /// <summary>
    /// ボールくらい
    /// </summary>
    /// <param name="ball"></param>
    protected override void OnBallHit(PierreGameBallB ball)
    {
        base.OnBallHit(ball);

        if (ball.attacktype != PierreGameBallB.AttackType.Enemy) return;
        if (invincible) return;

        effecting = true;
        invincible = true;
        StartCoroutine(DamageCoroutine());

        ball.DestroyWait();
    }

    #endregion

    #region コルーチン

    /// <summary>
    /// 低速移動判定コルーチン　常時起動
    /// </summary>
    /// <returns></returns>
    private IEnumerator SlowCheckCoroutine()
    {
        var input = InputManager.GetInstance();
        var pressTime = 0f;

        while (true)
        {
            if (isSlowMode)
            {
                if (!input.GetKey(InputManager.Keys.South))
                {
                    isSlowMode = false;
                }
            }
            else
            {
                if (input.GetKey(InputManager.Keys.South))
                {
                    pressTime += Time.deltaTime;
                    if (pressTime >= SLOW_PRESS_TIME)
                    {
                        isSlowMode = true;
                    }
                }
                else
                {
                    pressTime = 0f;
                }
            }

            yield return null;
        }
    }

    /// <summary>
    /// ダメージうけた処理
    /// </summary>
    /// <returns></returns>
    private IEnumerator DamageCoroutine()
    {
        ManagerSceneScript.GetInstance().soundMan.PlaySE(se_death);
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        life--;
        var util = GetComponent<ModelUtil>();
        util.FadeOut(0.5f);
        yield return new WaitWhile(() => util.IsFading());

        if (life == 0)
        {
            // 死亡で負け
            system.EndGame(false);
            yield break;
        }

        // Utilのフェード機能がColorを使うため個別設定
        spr_underBall.color = life switch
        {
            2 => Color.yellow,
            _ => Color.magenta,
        };

        var pos = new Vector3(-Constant.SCREEN_WIDTH * 0.7f, -120f);
        var deltaPos = new DeltaVector3();
        deltaPos.Set(pos);
        pos.x = -Constant.SCREEN_WIDTH * 0.4f;
        deltaPos.MoveTo(pos, 0.8f, DeltaFloat.MoveType.DECEL);
        transform.localPosition = deltaPos.Get();
        util.FadeIn(0f, alpha: 0.7f);
        while (deltaPos.IsActive())
        {
            yield return null;
            deltaPos.Update(Time.deltaTime);
            transform.localPosition = deltaPos.Get();
        }

        effecting = false;
        yield return new WaitForSeconds(2f);
        invincible = false;
        util.FadeIn(0f);
    }

    #endregion
}
