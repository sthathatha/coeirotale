using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ėp���W
/// </summary>
public class GeneralPosition : MonoBehaviour
{
    public int id = 0;

    /// <summary>
    /// ���W�擾
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPosition()
    {
        return gameObject.transform.position;
    }
}
