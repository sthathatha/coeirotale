using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �G��ƃ}�b�v�ړ�����
/// </summary>
public class MapMoveArea : AreaEventBase
{
    /// <summary>�ړ����Scene��</summary>
    public string targetMap;

    /// <summary>�ړ���Scene��GeneralPosition</summary>
    public int targetPos;

    /// <summary>
    /// �ړ����s
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Exec()
    {
        ManagerSceneScript.GetInstance().LoadMainScene(targetMap, targetPos);
        yield break;
    }
}
