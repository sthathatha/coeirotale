using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// îwåiÇÃêØ
/// </summary>
public class MatukaGameBGStar : MonoBehaviour
{
    /// <summary>
    /// çXêV
    /// </summary>
    void Update()
    {
        var camp = ManagerSceneScript.GetInstance().mainCam.transform.position;
        transform.localPosition = new Vector3(camp.x, camp.y);
    }
}
