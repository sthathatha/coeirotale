using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IkusautaGameHudaMask : MonoBehaviour
{
    /// <summary>���쒆</summary>
    private bool _active = false;

    /// <summary>
    /// �}�X�N�ʒu�ݒ�
    /// </summary>
    /// <param name="fixedPos">0:�S�B��@�`�@1:�S�\��</param>
    private void SetPos(float fixedPos)
    {
        transform.localPosition = new Vector3(0, fixedPos * -320, 0);
    }

    /// <summary>
    /// �D�\���A�j���[�V����
    /// </summary>
    /// <param name="_enable">true:�����@false:�B���</param>
    /// <returns></returns>
    public IEnumerator ShowHuda(bool _enable = true)
    {
        _active = true;
        var pos = new DeltaFloat();
        pos.Set(_enable ? 0 : 1);
        pos.MoveTo(_enable ? 1 : 0, 0.3f, DeltaFloat.MoveType.LINE);

        while (pos.IsActive())
        {
            SetPos(pos.Get());
            pos.Update(Time.deltaTime);

            yield return null;
        }
        SetPos(pos.Get());

        _active = false;
    }

    /// <summary>
    /// ���쒆
    /// </summary>
    /// <returns></returns>
    public bool IsActive() { return _active; }
}
