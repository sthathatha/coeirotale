using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 204中心核　背景コア
/// </summary>
public class F204BGCore : MonoBehaviour
{
    public GameObject playerObj;

    /// <summary>
    /// 更新
    /// </summary>
    private void Update()
    {
        transform.position = playerObj.transform.position * 0.6f;
    }
}
