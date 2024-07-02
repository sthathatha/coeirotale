using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F204ƒLƒƒƒ‰
/// </summary>
public class F204Characters : CharacterScript
{
    /// <summary>
    /// ’èˆÊ’u‚Ü‚Å•à‚­
    /// </summary>
    /// <returns></returns>
    public void StartWalk()
    {
        var defPos = transform.position;
        var pos4 = fieldScript.SearchGeneralPosition(4);
        SetPosition(pos4.GetPosition());
        gameObject.SetActive(true);

        WalkTo(defPos, afterDir: "down");
    }
}
