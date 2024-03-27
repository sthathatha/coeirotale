using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PierreGamePlayerA : PierreGameRoadObject
{
    private const int PLAYER_INIT_HP = 3;

    /// <summary>アニメーション操作用</summary>
    public Animator model = null;

    /// <summary>ピエール</summary>
    public PierreGamePierreA pierre = null;

    /// <summary>ジャンプSE</summary>
    public AudioClip se_jump = null;
    /// <summary>転ぶSE</summary>
    public AudioClip se_down = null;
    /// <summary>ボール踏むSE</summary>
    public AudioClip se_jump_humi = null;
    /// <summary>ピエールタッチSE</summary>
    public AudioClip se_touch = null;

    /// <summary>ピエール接触中フラグ</summary>
    private bool pierreHitting = false;

    /// <summary>当たり保留ボール</summary>
    private PierreGameBall hitWaitBall = null;

    private enum PlayerAction : int
    {
        Run = 0,
        Down,
    }
    /// <summary>動作中</summary>
    private PlayerAction action;

    /// <summary>体力</summary>
    private int hp = 1;

    /// <summary>
    /// 初期化
    /// </summary>
    override public void Start()
    {
        base.Start();

        SetFarPosition(0f);

        hp = PLAYER_INIT_HP;
    }

    /// <summary>
    /// フレーム処理
    /// </summary>
    override public void Update()
    {
        base.Update();

        var sound = ManagerSceneScript.GetInstance().soundMan;
        var input = InputManager.GetInstance();
        var rigid = GetComponent<Rigidbody2D>();

        try
        {
            if (ManagerSceneScript.GetInstance().SceneState != ManagerSceneScript.State.Game) { return; }
            if (system.state != PierreGameSystemA.GameState.Main) { return; }
            if (action != PlayerAction.Run) { return; }

            // 上下移動
            if (input.GetKey(InputManager.Keys.Up))
            {
                SetFarPosition(GetFarPosition() + ROAD_FAR_MAX * 1.5f * Time.deltaTime);
            }
            else if (input.GetKey(InputManager.Keys.Down))
            {
                SetFarPosition(GetFarPosition() - ROAD_FAR_MAX * 1.5f * Time.deltaTime);
            }

            // ジャンプ
            if (input.GetKeyPress(InputManager.Keys.South))
            {
                if (gameObject.transform.localPosition.y <= 1f)
                {
                    sound.PlaySE(se_jump);
                    var v = rigid.velocity;
                    v.y = 800f;
                    rigid.velocity = v;
                }
            }

            // ピエールに触る
            if (pierreHitting && pierre.IsHit(this))
            {
                sound.PlaySE(se_touch);
                system.TatchPierre();
            }

            // 横からボールに触る
            if (hitWaitBall && hitWaitBall.IsHit(this))
            {
                CheckBallHit(hitWaitBall);
                hitWaitBall = null;
            }
        }
        finally
        {
            model.SetFloat("x", rigid.velocity.x);
            model.SetFloat("y", rigid.velocity.y);
        }
    }

    /// <summary>
    /// 物に当たる
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var obj = collision.gameObject;
        var ballScript = obj.GetComponent<PierreGameBall>();
        var pierreScript = obj.GetComponent<PierreGamePierreA>();

        if (ballScript != null)
        {
            // ボール
            if (action == PlayerAction.Down) { return; }
            if (!ballScript.IsHit(this))
            {
                hitWaitBall = ballScript;
                return;
            }

            CheckBallHit(ballScript);
        }
        else if (pierreScript != null)
        {
            // ピエールタッチ
            pierreHitting = true;
        }
    }

    /// <summary>
    /// ボールに当たった時の挙動
    /// </summary>
    /// <param name="ballScript"></param>
    private void CheckBallHit(PierreGameBall ballScript)
    {
        var sound = ManagerSceneScript.GetInstance().soundMan;
        var rigid = GetComponent<Rigidbody2D>();
        var collision = ballScript.gameObject.GetComponent<Collider2D>();

        var closest = collision.ClosestPoint(new Vector2(transform.position.x, transform.position.y));
        if (closest.y > collision.transform.position.y + collision.offset.y)
        {
            // 踏んだ処理
            sound.PlaySE(se_jump_humi);
            var v = rigid.velocity;
            v.y = 800f;
            v.x = -300f;
            rigid.velocity = v;
        }
        else
        {
            // やられる処理
            sound.PlaySE(se_down);
            action = PlayerAction.Down;
            StartCoroutine(DownAction());
        }

        ballScript.GoOut();
    }

    /// <summary>
    /// やられる動きコルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator DownAction()
    {
        var rigid = GetComponent<Rigidbody2D>();

        var v = rigid.velocity;
        v.x = PierreGameSystemA.SCROLL_SPEED * 1.5f;
        rigid.velocity = v;

        yield return new WaitForSeconds(0.8f);
        v.x = 0f;
        rigid.velocity = v;

        while (transform.localPosition.x > PierreGameSystemA.PLAYER_INIT_X)
        {
            v = rigid.velocity;
            v.x = -200f;
            rigid.velocity = v;
            yield return null;
        }

        hp--;
        if (hp <= 0)
        {
            system.GameOver();
        }
        else
        {
            action = PlayerAction.Run;
        }
    }

    /// <summary>
    /// 物に当たらなくなる
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        var obj = collision.gameObject;
        var pierreScript = obj.GetComponent<PierreGamePierreA>();
        if (pierreScript != null)
        {
            // ピエールタッチ
            pierreHitting = false;
            return;
        }

        var ballScript = obj.GetComponent<PierreGameBall>();
        if (ballScript == hitWaitBall)
        {
            hitWaitBall = null;
        }
    }
}
