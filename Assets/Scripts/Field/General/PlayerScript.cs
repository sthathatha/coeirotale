using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : CharacterScript
{
    #region �萔
    /// <summary>�ړ����x</summary>
    const float WALK_VELOCITY = 200f;

    /// <summary>A���������̔��苗��</summary>
    const float ACTION_DISTANCE = 30f;
    #endregion

    /// <summary>�A�N�V���������p�����蔻��</summary>
    public GameObject actionCollide = null;

    /// <summary>����</summary>
    private Rigidbody2D rigid;

    /// <summary>
    /// ������
    /// </summary>
    override protected void Start()
    {
        base.Start();

        rigid = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// ���t���[������
    /// </summary>
    override protected void Update()
    {
        if (ManagerSceneScript.GetInstance()?.SceneState != ManagerSceneScript.State.Main) { return; }
        if (field.FieldState != MainScriptBase.State.Idle) { return; }
        var input = InputManager.GetInstance();

        if (input.GetKeyPress(InputManager.Keys.South))
        {
            var list = actionCollide.GetComponent<PlayerActionCollider>().GetHitList();
            foreach (var coll in list)
            {
                var ev = coll.gameObject.GetComponent<ActionEventBase>();
                if (!ev) continue;

                // �C�x���g����
                StopAnim();
                ev.ExecEvent();
                break;
            }

            return;
        }

        var v = new Vector3(0, 0, 0);
        var actionV = new Vector3(0, 0, 0);
        var moving = false;
        if (input.GetKey(InputManager.Keys.Up))
        {
            v.y = WALK_VELOCITY;
            actionV.y = ACTION_DISTANCE;
            moving = true;
        }
        else if (input.GetKey(InputManager.Keys.Down))
        {
            v.y = -WALK_VELOCITY;
            actionV.y = -ACTION_DISTANCE;
            moving = true;
        }

        if (input.GetKey(InputManager.Keys.Left))
        {
            v.x = -WALK_VELOCITY;
            actionV.x = -ACTION_DISTANCE;
            moving = true;
        }
        else if (input.GetKey(InputManager.Keys.Right))
        {
            v.x = WALK_VELOCITY;
            actionV.x = ACTION_DISTANCE;
            moving = true;
        }

        rigid.velocity = v;
        modelAnim.SetFloat("speedX", v.x);
        modelAnim.SetFloat("speedY", v.y);

        if (moving)
        {
            actionCollide.transform.position = transform.position + actionV;
        }

        // �J�����X�V
        var cam = ManagerSceneScript.GetInstance().mainCam;
        cam.SetTargetPos(gameObject);

        base.Update();
    }

    /// <summary>
    /// �����~�܂�
    /// </summary>
    private void StopAnim()
    {
        rigid.velocity = new Vector3(0, 0, 0);
        modelAnim.SetFloat("speedX", 0);
        modelAnim.SetFloat("speedY", 0);
    }

    /// <summary>
    /// �{�����[���^�C�x���g�ɓ���������s
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (field.FieldState != MainScriptBase.State.Idle) return;

        var evt = collision.gameObject.GetComponent<AreaEventBase>();
        if (evt == null) return;

        StopAnim();
        evt.ExecEvent();
    }
}
