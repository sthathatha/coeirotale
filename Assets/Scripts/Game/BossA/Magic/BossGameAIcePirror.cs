using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���������@�u���U�K�̒�
/// </summary>
public class BossGameAIcePirror : MonoBehaviour
{
    private Vector3 endPos;
    private DeltaVector3 updPos;

    private bool destroying = false;

    /// <summary>
    /// �J�n���ɒ��e�ʒu��ۑ�
    /// </summary>
    void Start()
    {
        endPos = transform.localPosition;

        updPos = new DeltaVector3();
        updPos.Set(endPos + new Vector3(0, 400));
        updPos.MoveTo(endPos, 0.2f, DeltaFloat.MoveType.LINE);
        transform.localPosition = updPos.Get();
    }

    /// <summary>
    /// �X�V
    /// </summary>
    void Update()
    {
        if (destroying) return;

        updPos.Update(Time.deltaTime);
        transform.localPosition = updPos.Get();

        if (!updPos.IsActive())
        {
            Destroy(gameObject, 0.3f);
            destroying = true;
        }
    }
}
