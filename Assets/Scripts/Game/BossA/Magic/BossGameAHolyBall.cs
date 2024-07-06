using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���������@�z�[���[�̋ʂP��
/// </summary>
public class BossGameAHolyBall : MonoBehaviour
{
    /// <summary>
    /// �\��
    /// </summary>
    /// <param name="pos">�ꏊ</param>
    /// <param name="time">�\������</param>
    /// <param name="priority">�\���D��x���Z</param>
    public void Show(Vector3 pos, float time, int priority)
    {
        transform.localPosition = pos;
        gameObject.SetActive(true);
        GetComponent<SpriteRenderer>().sortingOrder += priority;
        Destroy(gameObject, time);
    }
}
