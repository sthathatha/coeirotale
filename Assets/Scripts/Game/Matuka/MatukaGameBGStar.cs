using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �w�i�̐�
/// </summary>
public class MatukaGameBGStar : MonoBehaviour
{
    /// <summary>
    /// �X�V
    /// </summary>
    void Update()
    {
        var camp = ManagerSceneScript.GetInstance().mainCam.transform.position;
        transform.localPosition = new Vector3(camp.x, camp.y);
    }
}
