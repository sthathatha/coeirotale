using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �G�t�F�N�g��{�N���X
/// </summary>
public abstract class BossGameBEffectBase : MonoBehaviour
{
    public BossGameSystemB system;
    public SpriteRenderer model;
    protected Vector3 basePosition;

    /// <summary>
    /// ���s�㎩����Destroy
    /// </summary>
    /// <returns></returns>
    public void PlayAndDestroy(Vector3 pos)
    {
        basePosition = pos;
        transform.localPosition = basePosition;

        model.sortingLayerName = "FieldObject";
        model.sortingOrder = 20000;
        model.gameObject.SetActive(false);
        gameObject.SetActive(true);
        StartCoroutine(BasePlayCoroutine());
    }

    /// <summary>
    /// ���s�R���[�`��
    /// </summary>
    /// <returns></returns>
    private IEnumerator BasePlayCoroutine()
    {
        model.gameObject.SetActive(true);
        yield return Play();
        // �Đ����I������狏�Ȃ��Ȃ�
        Destroy(gameObject);
    }

    /// <summary>
    /// ���s���e�@�h����Ŏ���
    /// </summary>
    /// <returns></returns>
    protected abstract IEnumerator Play();
}
