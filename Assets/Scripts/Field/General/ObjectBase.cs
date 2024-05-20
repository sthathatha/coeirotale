using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �t�B�[���h�I�u�W�F�N�g
/// </summary>
public class ObjectBase : MonoBehaviour
{
    /// <summary>���f��</summary>
    public GameObject model;

    /// <summary>�t�B�[���h</summary>
    protected MainScriptBase fieldScript;

    /// <summary>���f��SpriteRenderer</summary>
    protected SpriteRenderer spriteRenderer;

    /// <summary>
    /// �J�n��
    /// </summary>
    virtual protected void Start()
    {
        //�X�v���C�g�擾
        spriteRenderer = model?.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingLayerName = "FieldObject";
        }

        // �t�B�[���h�X�N���v�g�@�ݒ肪�ʓ|�Ȃ̂Ŋ�{�I�ȍ\���Ȃ�擾
        var objects = gameObject.scene.GetRootGameObjects();
        foreach (var obj in objects)
        {
            var sys = obj.GetComponent<MainScriptBase>();
            if (sys != null)
            {
                fieldScript = sys;
                break;
            }
        }
    }

    /// <summary>
    /// �X�V
    /// </summary>
    virtual protected void Update()
    {
        //�X�v���C�g�̕\���I�[�_�[���ʒu�ɂ��킹�čX�V
        if (spriteRenderer != null)
        {
            var sortingOrder = Mathf.CeilToInt(-transform.position.y);
            if (spriteRenderer.sortingOrder != sortingOrder)
            {
                spriteRenderer.sortingOrder = sortingOrder;
            }
        }
    }
}
