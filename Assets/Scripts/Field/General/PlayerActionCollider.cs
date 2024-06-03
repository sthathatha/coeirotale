using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionCollider : MonoBehaviour
{
    private List<ActionEventBase> hitColliders;

    void Start()
    {
        hitColliders = new List<ActionEventBase>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var ae = collision.GetComponent<ActionEventBase>();
        if (ae == null) ae = collision.GetComponentInParent<ActionEventBase>();

        if (ae != null)
        {
            hitColliders.Add(ae);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var ae = collision.GetComponent<ActionEventBase>();
        if (ae == null) ae = collision.GetComponentInParent<ActionEventBase>();

        if (ae != null)
        {
            RemoveActionEventList(ae);
        }
    }

    /// <summary>
    /// �����ŏ�����Ƃ���Exit���Ă΂�Ȃ��̂Ŏ蓮�ō폜�Ă�
    /// </summary>
    /// <param name="collision"></param>
    public void RemoveActionEventList(ActionEventBase ae)
    {
        hitColliders.Remove(ae);
    }

    /// <summary>
    /// �������Ă���R���W�����̃��X�g
    /// </summary>
    /// <returns></returns>
    public List<ActionEventBase> GetHitList()
    {
        return hitColliders;
    }
}
