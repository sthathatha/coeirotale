using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 204���S�j�@�w�i�R�A
/// </summary>
public class F204BGCore : MonoBehaviour
{
    public GameObject playerObj;

    /// <summary>
    /// �X�V
    /// </summary>
    private void Update()
    {
        transform.position = playerObj.transform.position * 0.6f;
    }
}
