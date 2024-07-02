using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class F204IceWall : ObjectBase
{
    private const float RIGHT_X = 322f;
    private const float LEFT_X = 0f;

    /// <summary>�~�܂鎞��SE</summary>
    public AudioClip stop_Se;

    private float y;
    private DeltaFloat deltaX = new DeltaFloat();

    private int moveMode = 0;

    /// <summary>
    /// ������
    /// </summary>
    protected override void Start()
    {
        base.Start();

        y = transform.position.y;
        deltaX.Set(transform.position.x);
    }

    /// <summary>
    /// �E�ɍs��
    /// </summary>
    public void SetRight()
    {
        moveMode = 0;
        deltaX.Set(RIGHT_X);
        UpdatePos();
    }

    /// <summary>
    /// �R���[�`���ŉE�ɍs��
    /// </summary>
    /// <returns></returns>
    public IEnumerator MoveRightCoroutine()
    {
        moveMode = 1;
        var dist = RIGHT_X - deltaX.Get();
        var time = dist / (RIGHT_X - LEFT_X) * 0.5f;

        deltaX.MoveTo(RIGHT_X, time, DeltaFloat.MoveType.LINE);
        while (deltaX.IsActive())
        {
            yield return null;
            if (moveMode != 1) yield break;
            deltaX.Update(Time.deltaTime);
            UpdatePos();
        }

        ManagerSceneScript.GetInstance().soundMan.PlaySE(stop_Se);
    }

    /// <summary>
    /// ���ɍs��
    /// </summary>
    public void SetLeft()
    {
        moveMode = 0;
        deltaX.Set(LEFT_X);
        UpdatePos();
    }

    /// <summary>
    /// ���ɍs���R���[�`��
    /// </summary>
    /// <returns></returns>
    public IEnumerator MoveLeftCoroutine()
    {
        moveMode = -1;
        var dist = deltaX.Get() - LEFT_X;
        var time = dist / (RIGHT_X - LEFT_X) * 0.5f;

        deltaX.MoveTo(LEFT_X, time, DeltaFloat.MoveType.LINE);
        while (deltaX.IsActive())
        {
            yield return null;
            if (moveMode != -1) yield break;
            deltaX.Update(Time.deltaTime);
            UpdatePos();
        }

        ManagerSceneScript.GetInstance().soundMan.PlaySE(stop_Se);
    }

    /// <summary>
    /// ���݂�deltaX�ōX�V
    /// </summary>
    private void UpdatePos()
    {
        transform.position = new Vector3(deltaX.Get(), y, 0);
    }
}
