using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class BossGameBCarnageLine : MonoBehaviour
{
    private const float LINE_SPEED = 5000f;
    private const float TAIL_TIME = 0.2f;

    private Vector3 pos1;
    private Vector3 pos2;
    private DeltaVector3 head;
    private DeltaVector3 tail;

    private bool headMoving = false;
    private bool tailMoving = false;

    /// <summary>
    /// 表示開始
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    public void StartLine(Vector3 p1, Vector3 p2)
    {
        headMoving = true;
        tailMoving = true;

        pos1 = p1;
        pos2 = p2;
        head = new DeltaVector3();
        tail = new DeltaVector3();
        head.Set(p1);
        tail.Set(p1);
        DisplayModel(head.Get(), tail.Get());
        gameObject.SetActive(true);

        StartCoroutine(MoveHeadCoroutine());
        StartCoroutine(MoveTailCoroutine());
        StartCoroutine(LineUpdateCoroutine());
    }
    public bool IsHeadMoving() { return headMoving; }
    public bool IsTailMoving() { return tailMoving; }

    /// <summary>
    /// 頭の動きコルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator MoveHeadCoroutine()
    {
        var time = (pos2 - pos1).magnitude / LINE_SPEED;
        head.MoveTo(pos2, time, DeltaFloat.MoveType.LINE);
        while (head.IsActive())
        {
            yield return null;
            head.Update(Time.deltaTime);
        }
        headMoving = false;
    }

    /// <summary>
    /// 尻尾の動きコルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator MoveTailCoroutine()
    {
        var time = (pos2 - pos1).magnitude / LINE_SPEED;
        yield return new WaitForSeconds(TAIL_TIME);
        tail.MoveTo(pos2, time, DeltaFloat.MoveType.LINE);
        while (tail.IsActive())
        {
            yield return null;
            tail.Update(Time.deltaTime);
        }
        tailMoving = false;
    }

    /// <summary>
    /// 描画更新コルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator LineUpdateCoroutine()
    {
        while (true)
        {
            DisplayModel(head.Get(), tail.Get());
            yield return null;
        }
    }

    /// <summary>
    /// ラインの表示
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    private void DisplayModel(Vector3 p1, Vector3 p2)
    {
        transform.localScale = new Vector3((p2 - p1).magnitude, 8f, 1f);
        transform.localPosition = (p1 + p2) / 2f;
        transform.localRotation = Util.GetRotateQuaternion(Util.GetRadianFromVector(p2 - p1));
    }
}
