using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// �t�B�[���h�L�����N�^�[���ʏ���
/// </summary>
public class CharacterScript : ObjectBase
{
    /// <summary>���f���A�j���[�V����</summary>
    protected Animator modelAnim;

    /// <summary>
    /// ������
    /// </summary>
    protected override void Start()
    {
        base.Start();

        modelAnim = model.GetComponent<Animator>();
    }

    /// <summary>
    /// �t���[������
    /// </summary>
    protected override void Update()
    {
        base.Update();
    }
}
