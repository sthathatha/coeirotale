using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// プレイヤー★
/// </summary>
public class PlayerScript : CharacterScript
{
    #region 定数

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
    }

    /// <summary>
    /// 毎フレーム処理
    /// </summary>
    override protected void Update()
    {
        if (ManagerSceneScript.GetInstance()?.SceneState != ManagerSceneScript.State.Main) { return; }
        if (fieldScript.FieldState != MainScriptBase.State.Idle)
        {
            UpdateCamera();
            return;
        }

        var optionWindow = ManagerSceneScript.GetInstance().GetOptionWindow();
        if (optionWindow.gameObject.activeSelf) { return; }

        var input = InputManager.GetInstance();

        if (input.GetKeyPress(InputManager.Keys.South))
        {
            // アクション実行
            var list = actionCollide.GetComponent<PlayerActionCollider>().GetHitList();
            foreach (var coll in list)
            {
                var ev = coll.GetComponent<ActionEventBase>();
                // parent
                if (ev == null)
                {
                    ev = coll.GetComponentInParent<ActionEventBase>();
                }

                if (ev == null) continue;

                // イベント発生
                StopAnim();
                ev.ExecEvent();
                break;
            }

            return;
        }
        else if (input.GetKeyPress(InputManager.Keys.North))
        {
            // オプション開く
            ManagerSceneScript.GetInstance().StartCoroutine(optionWindow.OpenDialog());
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

        UpdateCamera();

        base.Update();
    }

    /// <summary>
    /// カメラ更新
    /// </summary>
    private void UpdateCamera()
    {
        var cam = ManagerSceneScript.GetInstance().mainCam;
        cam.SetTargetPos(gameObject);
    }

    /// <summary>
    /// 立ち止まる
    /// </summary>
    private void StopAnim()
    {
        rigid.velocity = new Vector3(0, 0, 0);
        modelAnim.SetFloat("speedX", 0);
        modelAnim.SetFloat("speedY", 0);
    }

    /// <summary>
    /// ボリューム型イベントに入ったら実行
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ManagerSceneScript.GetInstance()?.SceneState != ManagerSceneScript.State.Main) return;
        if (fieldScript?.FieldState != MainScriptBase.State.Idle) return;

        var evt = collision.GetComponent<AreaEventBase>();
        if (evt == null) return;

        StopAnim();
        evt.ExecEvent();
    }
}
