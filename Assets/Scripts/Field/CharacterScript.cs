using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterScript : MonoBehaviour
{
    /// <summary>�t�B�[���h</summary>
    public MainScriptBase field;
    /// <summary>���f��</summary>
    public GameObject model;

    /// <summary>���f���A�j���[�V����</summary>
    protected Animator modelAnim;
    /// <summary>���f��SpriteRenderer</summary>
    protected SpriteRenderer spriteRenderer;

    /// <summary>
    /// ������
    /// </summary>
    virtual protected void Start()
    {
        modelAnim = model.GetComponent<Animator>();
        spriteRenderer = model.GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// �t���[������
    /// </summary>
    virtual protected void Update()
    {
        var sortingOrder = Mathf.CeilToInt(-transform.position.y);
        if (spriteRenderer.sortingOrder != sortingOrder)
        {
            spriteRenderer.sortingOrder = sortingOrder;
        }
    }
}
