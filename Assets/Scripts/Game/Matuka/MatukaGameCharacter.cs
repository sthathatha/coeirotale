using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// まつかりすくゲームキャラ
/// </summary>
public class MatukaGameCharacter : MonoBehaviour
{
    public GameObject faceObj;
    public GameObject waitObj;
    public GameObject poseObj;

    /// <summary>
    /// グラフィック表示
    /// </summary>
    /// <param name="wait">待機画像</param>
    /// <param name="pose">ポーズ画像</param>
    /// <param name="face">顔</param>
    public void ShowObject(bool wait, bool pose, bool face)
    {
        waitObj.SetActive(wait);
        poseObj.SetActive(pose);
        faceObj.SetActive(face);
    }
}
