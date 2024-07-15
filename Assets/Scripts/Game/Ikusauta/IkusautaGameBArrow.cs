using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// –îˆó•\¦
/// </summary>
public class IkusautaGameBArrow : MonoBehaviour
{
    public Sprite spr_arrow;
    public Sprite spr_button;

    private IkusautaGameSystemB.ArrowDir commandDirection;

    /// <summary>
    /// ‰æ‘œ•\¦
    /// </summary>
    /// <param name="dir"></param>
    public void SetDirection(IkusautaGameSystemB.ArrowDir dir)
    {
        commandDirection = dir;

        // ‰æ‘œİ’è
        var render = GetComponent<SpriteRenderer>();
        render.sprite = dir switch
        {
            IkusautaGameSystemB.ArrowDir.Button => spr_button,
            _ => spr_arrow,
        };

        // Œü‚«İ’è
        transform.localRotation = dir switch
        {
            IkusautaGameSystemB.ArrowDir.Up => Util.GetRotateQuaternion(Mathf.PI * 0.5f),
            IkusautaGameSystemB.ArrowDir.Left => Util.GetRotateQuaternion(Mathf.PI),
            IkusautaGameSystemB.ArrowDir.Down => Util.GetRotateQuaternion(Mathf.PI * 1.5f),
            _ => Quaternion.identity,
        };
    }

    /// <summary>
    /// İ’è•ûŒüæ“¾
    /// </summary>
    /// <returns></returns>
    public IkusautaGameSystemB.ArrowDir GetDirection()
    {
        return commandDirection;
    }
}
