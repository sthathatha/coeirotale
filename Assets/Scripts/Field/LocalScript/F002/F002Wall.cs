using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F002��
/// </summary>
public class F002Wall : ObjectBase
{
    /// <summary>���Y���W</summary>
    public float UpY;
    /// <summary>����Y���W</summary>
    public float DownY;

    /// <summary>���݈ʒutrue:��</summary>
    public bool isDown;

    /// <summary>���W</summary>
    private DeltaVector3 pos = new DeltaVector3();

    /// <summary>
    /// ������
    /// </summary>
    protected override void Start()
    {
        base.Start();

        // �����l�ݒ�
        pos.Set(new Vector3(transform.localPosition.x, isDown ? DownY : UpY, 0));
        transform.localPosition = pos.Get();
    }

    /// <summary>
    /// �ʒu�ύX
    /// </summary>
    public void Toggle()
    {
        StartCoroutine(ToggleCoroutine());
    }

    /// <summary>
    /// �ʒu�ύX�R���[�`��
    /// </summary>
    /// <returns></returns>
    public IEnumerator ToggleCoroutine()
    {
        isDown = !isDown;

        pos.MoveTo(new Vector3(transform.localPosition.x, isDown ? DownY : UpY, 0), 2f, DeltaFloat.MoveType.LINE);
        while (pos.IsActive())
        {
            yield return null;

            pos.Update(Time.deltaTime);
            transform.localPosition = pos.Get();
        }
    }

    /// <summary>
    /// �ړ���
    /// </summary>
    /// <returns></returns>
    public bool IsMoving()
    {
        return pos.IsActive();
    }
}
