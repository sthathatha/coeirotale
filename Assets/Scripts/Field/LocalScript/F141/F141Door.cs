using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F141�h�A
/// </summary>
public class F141Door : ObjectBase
{
    /// <summary>���f���̃A�j���[�V����</summary>
    public Animator doorAnim;
    /// <summary>���܂��Ă���R���W����</summary>
    public GameObject closeCollision;

    /// <summary>�h�A���</summary>
    public enum DoorType : int
    {
        Success = 0,
        Failed,
    }
    protected DoorType doorType;

    /// <summary>
    /// �����E�n�Y���ݒ�
    /// </summary>
    /// <param name="type"></param>
    public void SetDoorType(DoorType type) { doorType = type; }
    /// <summary>
    /// �����E�n�Y��
    /// </summary>
    /// <returns></returns>
    public DoorType GetDoorType() { return doorType; }

    /// <summary>
    /// ���E���]�\��
    /// </summary>
    /// <param name="mirror"></param>
    public void SetMirror(bool mirror)
    {
        model.GetComponent<SpriteRenderer>().flipX = mirror;
    }

    /// <summary>
    /// �J��
    /// </summary>
    /// <param name="immediate">true:��u</param>
    public void Open(bool immediate = false)
    {
        closeCollision.SetActive(false);

        if (immediate)
        {
            doorAnim.Play("open");
            return;
        }

        if (doorType == DoorType.Success)
        {
            doorAnim.Play("openInverse");
        }
        else
        {
            doorAnim.Play("openNormal");
        }
    }
}
