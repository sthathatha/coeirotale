using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �L�����ݒ�
/// </summary>
public class F143MenderuCharacter : CharacterScript
{
    protected override void Start()
    {
        base.Start();

        SetDirection(Constant.Direction.Up);
    }
}
