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
    public GameObject downObj;

    /// <summary>
    /// グラフィック表示
    /// </summary>
    /// <param name="wait">待機画像</param>
    /// <param name="pose">ポーズ画像</param>
    /// <param name="face">顔</param>
    /// <param name="down">ダウン</param>
    public void ShowObject(bool wait, bool pose, bool face, bool down = false)
    {
        waitObj.SetActive(wait);
        poseObj.SetActive(pose);
        faceObj.SetActive(face);
        if (downObj != null)
        {
            downObj.SetActive(down);
        }
    }

    /// <summary>
    /// 描画順設定
    /// </summary>
    /// <param name="priority"></param>
    public void SetRenderPriority(int priority)
    {
        waitObj.GetComponent<SpriteRenderer>().sortingOrder = priority;
        poseObj.GetComponent<SpriteRenderer>().sortingOrder = priority;
        faceObj.GetComponent<SpriteRenderer>().sortingOrder = priority + 1;
        if (downObj != null)
        {
            downObj.GetComponent<SpriteRenderer>().sortingOrder = priority;
        }
    }
}
