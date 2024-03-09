using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PierreGameRoadObject : MonoBehaviour
{
    /// <summary>�����v���X�}�C�i�X</summary>
    public const float ROAD_FAR_MAX = 80f;
    /// <summary>�I�u�W�F�N�g�������鋗��</summary>
    public const float ROAD_HIT_DISTANCE = 20f;

    public PierreGameSystemA system = null;

    /// <summary>�����ړ����ɕ\�����㉺������e�I�u�W�F�N�g</summary>
    public GameObject renderParent = null;
    /// <summary>�����ړ����ɕ`�揇��ݒ肷��Sprite</summary>
    public SpriteRenderer priorityRender = null;

    /// <summary>�����̈ʒu</summary>
    private float farPosition = 0f;
    /// <summary>�`�揇�ݒ�Sprite</summary>
    private List<SpriteRenderer> priorityRenderList = null;

    /// <summary>
    /// ������
    /// </summary>
    virtual public void Start()
    {
        priorityRenderList = new List<SpriteRenderer>();
        if (priorityRender != null)
        {
            AddRenderList(priorityRender);
        }

        SetFarPosition(farPosition);
    }

    /// <summary>
    /// �`�揇���샊�X�g�ǉ�
    /// </summary>
    /// <param name="sprite"></param>
    protected void AddRenderList(SpriteRenderer sprite)
    {
        priorityRenderList.Add(sprite);
    }

    /// <summary>
    /// �t���[������
    /// </summary>
    virtual public void Update()
    {
    }

    /// <summary>
    /// ���̈ʒu�擾
    /// </summary>
    /// <returns></returns>
    public float GetFarPosition() { return farPosition; }
    /// <summary>
    /// ���̈ʒu��ݒ�
    /// </summary>
    /// <param name="_far"></param>
    public virtual void SetFarPosition(float _far)
    {
        farPosition = _far;
        farPosition = Mathf.Clamp(farPosition, -ROAD_FAR_MAX, ROAD_FAR_MAX);

        var pos = renderParent.transform.localPosition;
        pos.y = farPosition;
        renderParent.transform.localPosition = pos;

        if (priorityRenderList != null)
        {
            var order = CalcBaseSortingOrder();
            foreach (var sprite in priorityRenderList)
            {
                sprite.sortingOrder = order;
            }
        }
    }

    /// <summary>
    /// �ʒu�ɂ��`�揇�̌���
    /// </summary>
    /// <returns>���悻0�`1600</returns>
    protected int CalcBaseSortingOrder()
    {
        return Mathf.FloorToInt((farPosition - ROAD_FAR_MAX) * -10);
    }

    /// <summary>
    /// �ʒu�ɂ���ē������Ă��邩�ǂ���
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool IsHit(PierreGameRoadObject other)
    {
        var abs = Mathf.Abs(GetFarPosition() - other.GetFarPosition());
        return abs <= ROAD_HIT_DISTANCE;
    }
}
