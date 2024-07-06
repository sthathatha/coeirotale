using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// �v���C���[��
/// </summary>
public class PlayerScript : CharacterScript
{
    #region �萔

    /// <summary>A���������̔��苗��</summary>
    const float ACTION_DISTANCE = 30f;

    #endregion

    #region �����o�[

    /// <summary>�A�N�V���������p�����蔻��</summary>
    public GameObject actionCollide = null;

    #endregion

    #region �ϐ�

    /// <summary>����</summary>
    private Rigidbody2D rigid;

    /// <summary>�G���A�A�N�V�����ێ�</summary>
    private List<AreaActionEventBase> areaActionList = new List<AreaActionEventBase>();

    #endregion

    #region ���
    /// <summary>
    /// ������
    /// </summary>
    override protected void Start()
    {
        base.Start();

        rigid = GetComponent<Rigidbody2D>();
        enableCamera = true;
    }

    /// <summary>
    /// ���t���[������
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
            // �A�N�V�������s
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
                // �C�x���g����
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
            // �I�v�V�����J��
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

    #region �p�u���b�N

    /// <summary>
    /// �G���A�A�N�V�������X�g����폜
    /// �@�ڐG����SetActive�ŏ������OnTriggerExit���Ă΂�Ȃ�����
    /// </summary>
    /// <param name="aa"></param>
    public void RemoveAreaActionList(AreaActionEventBase aa)
    {
        areaActionList.Remove(aa);
    }

    /// <summary>
    /// �A�N�V�������X�g����폜
    /// �@�ڐG����SetActive�ŏ�����Ƃ��p
    /// </summary>
    /// <param name="ae"></param>
    public void RemoveActionEvent(ActionEventBase ae)
    {
        actionCollide.GetComponent<PlayerActionCollider>().RemoveActionEventList(ae);
    }
    #endregion

    #region �v���C�x�[�g

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
    /// �{�����[������o��
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
