using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ”Ä—pÀ•W
/// </summary>
public class GeneralPosition : MonoBehaviour
{
    public int id = 0;

    /// <summary>
    /// À•Wæ“¾
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPosition()
    {
        return gameObject.transform.position;
    }
}
