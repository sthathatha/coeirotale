using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �㉺�̊�摜
/// </summary>
public class IkusautaGameZoomFace : MonoBehaviour
{
    /// <summary>�\���ꏊ</summary>
    private enum Location : int
    {
        /// <summary>��</summary>
        Top = 0,
        /// <summary>��</summary>
        Bottom,
    }

    /// <summary>�\���ꏊ</summary>
    public int location;

    /// <summary>���s��</summary>
    private bool _active = false;

    /// <summary>
    /// �J�n
    /// </summary>
    /// <returns></returns>
    public IEnumerator PlayAnim()
    {
        _active = true;
        var loc = (Location)location;
        var x = new DeltaFloat();
        var locSign = loc == Location.Top ? 1 : -1;
        var y = locSign * 260;

        // �����ʒu
        x.Set(locSign * -640);
        x.MoveTo(locSign * 640, 1f, DeltaFloat.MoveType.LINE);

        // �ړ�
        while (x.IsActive())
        {
            x.Update(Time.deltaTime);
            transform.localPosition = new Vector3(x.Get(), y, 0);

            yield return null;
        }
        transform.localPosition = new Vector3(x.Get(), y, 0);

        // �����
        _active = false;
    }

    /// <summary>
    /// �A�j���[�V������
    /// </summary>
    /// <returns></returns>
    public bool IsActive()
    {
        return _active;
    }
}
