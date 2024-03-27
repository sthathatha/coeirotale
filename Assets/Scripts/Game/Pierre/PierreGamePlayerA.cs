using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PierreGamePlayerA : PierreGameRoadObject
{
    private const int PLAYER_INIT_HP = 3;

    /// <summary>�A�j���[�V��������p</summary>
    public Animator model = null;

    /// <summary>�s�G�[��</summary>
    public PierreGamePierreA pierre = null;

    /// <summary>�W�����vSE</summary>
    public AudioClip se_jump = null;
    /// <summary>�]��SE</summary>
    public AudioClip se_down = null;
    /// <summary>�{�[������SE</summary>
    public AudioClip se_jump_humi = null;
    /// <summary>�s�G�[���^�b�`SE</summary>
    public AudioClip se_touch = null;

    /// <summary>�s�G�[���ڐG���t���O</summary>
    private bool pierreHitting = false;

    /// <summary>������ۗ��{�[��</summary>
    private PierreGameBall hitWaitBall = null;

    private enum PlayerAction : int
    {
        Run = 0,
        Down,
    }
    /// <summary>���쒆</summary>
    private PlayerAction action;

    /// <summary>�̗�</summary>
    private int hp = 1;

    /// <summary>
    /// ������
    /// </summary>
    override public void Start()
    {
        base.Start();

        SetFarPosition(0f);

        hp = PLAYER_INIT_HP;
    }

    /// <summary>
    /// �t���[������
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

            // �㉺�ړ�
            if (input.GetKey(InputManager.Keys.Up))
            {
                SetFarPosition(GetFarPosition() + ROAD_FAR_MAX * 1.5f * Time.deltaTime);
            }
            else if (input.GetKey(InputManager.Keys.Down))
            {
                SetFarPosition(GetFarPosition() - ROAD_FAR_MAX * 1.5f * Time.deltaTime);
            }

            // �W�����v
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

            // �s�G�[���ɐG��
            if (pierreHitting && pierre.IsHit(this))
            {
                sound.PlaySE(se_touch);
                system.TatchPierre();
            }

            // ������{�[���ɐG��
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
    /// ���ɓ�����
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var obj = collision.gameObject;
        var ballScript = obj.GetComponent<PierreGameBall>();
        var pierreScript = obj.GetComponent<PierreGamePierreA>();

        if (ballScript != null)
        {
            // �{�[��
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
            // �s�G�[���^�b�`
            pierreHitting = true;
        }
    }

    /// <summary>
    /// �{�[���ɓ����������̋���
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
            // ���񂾏���
            sound.PlaySE(se_jump_humi);
            var v = rigid.velocity;
            v.y = 800f;
            v.x = -300f;
            rigid.velocity = v;
        }
        else
        {
            // ����鏈��
            sound.PlaySE(se_down);
            action = PlayerAction.Down;
            StartCoroutine(DownAction());
        }

        ballScript.GoOut();
    }

    /// <summary>
    /// ����铮���R���[�`��
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
    /// ���ɓ�����Ȃ��Ȃ�
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        var obj = collision.gameObject;
        var pierreScript = obj.GetComponent<PierreGamePierreA>();
        if (pierreScript != null)
        {
            // �s�G�[���^�b�`
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
