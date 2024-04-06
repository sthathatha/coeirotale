using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : CharacterScript
{
    #region 定数
    /// <summary>移動速度</summary>
    const float WALK_VELOCITY = 200f;

    /// <summary>A押した時の判定距離</summary>
    const float ACTION_DISTANCE = 30f;
    #endregion

    /// <summary>アクション検索用当たり判定</summary>
    public GameObject actionCollide = null;

    /// <summary>物理</summary>
    private Rigidbody2D rigid;

    /// <summary>
    /// 初期化
    /// </summary>
    override protected void Start()
    {
        base.Start();

        rigid = GetComponent<Rigidbody2D>();

        if (!ManagerSceneScript.GetInstance()) return;

        var cam = ManagerSceneScript.GetInstance().mainCam;
        cam.SetTargetPos(gameObject);
        cam.Immediate();
    }

    /// <summary>
    /// 毎フレーム処理
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

                // イベント発生
                rigid.velocity = new Vector3(0, 0, 0);
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

        // カメラ更新
        var cam = ManagerSceneScript.GetInstance().mainCam;
        cam.SetTargetPos(gameObject);

        base.Update();
    }
}
