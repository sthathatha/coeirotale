using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �A�N�V�����C�x���g�p
/// </summary>
public abstract class ActionEventBase : EventBase
{
    /// <summary>
    /// �v���C���[����̋������擾
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPlayerDistance()
    {
        if (fieldScript?.playerObj == null) return Vector3.zero;

        var ownCol = GetComponent<Collider2D>();
        if (ownCol == null) ownCol = GetComponentInChildren<Collider2D>();

        var pPos = fieldScript.playerObj.transform.position;
        var closest = ownCol.ClosestPoint(new Vector2(pPos.x, pPos.y));

        var dist = new Vector3(closest.x - pPos.x, closest.y - pPos.y, 0);

        return dist;
    }

}
