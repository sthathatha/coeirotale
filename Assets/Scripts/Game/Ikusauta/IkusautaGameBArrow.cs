using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���\��
/// </summary>
public class IkusautaGameBArrow : MonoBehaviour
{
    public Sprite spr_arrow;
    public Sprite spr_button;

    private IkusautaGameSystemB.ArrowDir commandDirection;

    /// <summary>
    /// �摜�\��
    /// </summary>
    /// <param name="dir"></param>
    public void SetDirection(IkusautaGameSystemB.ArrowDir dir)
    {
        commandDirection = dir;

        // �摜�ݒ�
        var render = GetComponent<SpriteRenderer>();
        render.sprite = dir switch
        {
            IkusautaGameSystemB.ArrowDir.Button => spr_button,
            _ => spr_arrow,
        };

        // �����ݒ�
        transform.localRotation = dir switch
        {
            IkusautaGameSystemB.ArrowDir.Up => Util.GetRotateQuaternion(Mathf.PI * 0.5f),
            IkusautaGameSystemB.ArrowDir.Left => Util.GetRotateQuaternion(Mathf.PI),
            IkusautaGameSystemB.ArrowDir.Down => Util.GetRotateQuaternion(Mathf.PI * 1.5f),
            _ => Quaternion.identity,
        };
    }

    /// <summary>
    /// �ݒ�����擾
    /// </summary>
    /// <returns></returns>
    public IkusautaGameSystemB.ArrowDir GetDirection()
    {
        return commandDirection;
    }
}
