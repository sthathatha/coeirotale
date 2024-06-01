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

    /// <summary>�`�揇��ݒ肷�郊�X�g</summary>
    protected List<SpriteRenderer> priorityTargetList;

    /// <summary>
    /// �J�n��
    /// </summary>
    virtual protected void Start()
    {
        priorityTargetList = new List<SpriteRenderer>();

        //�X�v���C�g�擾
        var sprite = model?.GetComponent<SpriteRenderer>();
        if (sprite != null)
        {
            AddPriorityList(sprite);
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
        if (priorityTargetList.Count > 0)
        {
            var sortingOrder = Mathf.CeilToInt(-transform.position.y);
            foreach (var s in priorityTargetList)
            {
                if (s.sortingOrder != sortingOrder)
                {
                    s.sortingOrder = sortingOrder;
                }
            }
        }
    }

    /// <summary>
    /// �`�揇�ݒ�Sprite�̒ǉ�
    /// </summary>
    /// <param name="sprite"></param>
    protected void AddPriorityList(SpriteRenderer sprite)
    {
        sprite.sortingLayerName = "FieldObject";
        priorityTargetList.Add(sprite);
    }
}
