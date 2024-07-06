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

    #region メンバー

    /// <summary>アクション検索用当たり判定</summary>
    public GameObject actionCollide = null;

    #endregion

    #region 変数

    /// <summary>物理</summary>
    private Rigidbody2D rigid;

    /// <summary>エリアアクション保持</summary>
    private List<AreaActionEventBase> areaActionList = new List<AreaActionEventBase>();

    #endregion

    #region 基底
    /// <summary>
    /// 初期化
    /// </summary>
    override protected void Start()
    {
        base.Start();

        rigid = GetComponent<Rigidbody2D>();
        enableCamera = true;
    }

    /// <summary>
    /// 毎フレーム処理
    /// </summary>
    override protected void Update()
    {
        if (ManagerSceneScript.GetInstance()?.SceneState != ManagerSceneScript.State.Main) { return; }
        if (fieldScript.FieldState != MainScriptBase.State.Idle ||
            fieldScript.IsEventPlaying())
        {
            base.Update();
            return;
        }

        var optionWindow = ManagerSceneScript.GetInstance().GetOptionWindow();
        if (optionWindow.gameObject.activeSelf) { return; }

        var input = InputManager.GetInstance();

        if (input.GetKeyPress(InputManager.Keys.South))
        {
            // アクション実行
            var eventPlayed = false;
            var hitList = actionCollide.GetComponent<PlayerActionCollider>().GetHitList();
            ActionEventBase closestEv = null;
            var closestDist = 0f;
            foreach (var coll in hitList)
            {
                var ev = coll.GetComponent<ActionEventBase>() ??
                    coll.GetComponentInParent<ActionEventBase>();
                if (ev == null) continue;

                var dist = ev.GetPlayerDistance();
                if (closestEv == null || dist.sqrMagnitude < closestDist)
                {
                    closestEv = ev;
                    closestDist = dist.sqrMagnitude;
                }
            }
            if (closestEv != null)
            {
                // イベント発生
                StopAnim();
                closestEv.ExecEvent();
                eventPlayed = true;
            }

            if (eventPlayed == false && areaActionList.Count > 0)
            {
                StopAnim();
                areaActionList[0].ExecEvent();
                eventPlayed = true;
            }

            return;
        }
        else if (input.GetKeyPress(InputManager.Keys.North))
        {
            // オプション開く
            StopAnim();
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

        base.Update();
    }

    #endregion

    #region パブリック

    /// <summary>
    /// エリアアクションリストから削除
    /// 　接触中にSetActiveで消えるとOnTriggerExitが呼ばれないため
    /// </summary>
    /// <param name="aa"></param>
    public void RemoveAreaActionList(AreaActionEventBase aa)
    {
        areaActionList.Remove(aa);
    }

    /// <summary>
    /// アクションリストから削除
    /// 　接触中にSetActiveで消えるとき用
    /// </summary>
    /// <param name="ae"></param>
    public void RemoveActionEvent(ActionEventBase ae)
    {
        actionCollide.GetComponent<PlayerActionCollider>().RemoveActionEventList(ae);
    }
    #endregion

    #region プライベート

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
        if (fieldScript?.IsEventPlaying() == true) return;
        if (fieldScript?.FieldState != MainScriptBase.State.Idle) return;

        // 
        var aa = collision.GetComponent<AreaActionEventBase>();
        if (aa == null) aa = collision.GetComponentInParent<AreaActionEventBase>();
        if (aa != null)
        {
            areaActionList.Add(aa);
        }

        //
        var evt = collision.GetComponent<AreaEventBase>();
        if (evt == null)
        {
            evt = collision.GetComponentInParent<AreaEventBase>();
            if (evt == null) return;
        }

        StopAnim();
        evt.ExecEvent();
    }

    /// <summary>
    /// ボリュームから出た
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        var aa = collision.GetComponent<AreaActionEventBase>();
        if (aa == null) aa = collision.GetComponentInParent<AreaActionEventBase>();

        if (aa != null)
        {
            RemoveAreaActionList(aa);
        }
    }

    #endregion

}
