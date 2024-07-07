using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���������@���e�I覐�1��
/// </summary>
public class BossGameAMeteorOne : MonoBehaviour
{
    /// <summary>Y�̊J�n�I�����W</summary>
    private const float BASE_Y = 400f;
    /// <summary>X�̊J�n���W</summary>
    private const float BASE_X = 300f;
    /// <summary>X�̊J�n���W���獶�E�����_���͈�</summary>
    private const float START_X_RANGE = 600f;
    /// <summary>X�̏I�����W���獶�E�����_���͈�</summary>
    private const float FALL_X_RANGE = 50f;

    /// <summary>
    /// �����J�n
    /// </summary>
    public void Fall()
    {
        gameObject.SetActive(true);
        StartCoroutine(FallCoroutine());
    }

    /// <summary>
    /// ��������R���[�`��
    /// </summary>
    /// <returns></returns>
    private IEnumerator FallCoroutine()
    {
        var pos = new DeltaVector3();
        var st = new Vector3(BASE_X + Util.RandomFloat(-START_X_RANGE, START_X_RANGE), BASE_Y);
        pos.Set(st);
        var ed = new Vector3(st.x - BASE_X * 2 + Util.RandomFloat(-FALL_X_RANGE, FALL_X_RANGE), -BASE_Y);
        pos.MoveTo(ed, 0.4f, DeltaFloat.MoveType.LINE);
        while (pos.IsActive())
        {
            transform.localPosition = pos.Get();

            yield return null;
        }

        Destroy(gameObject);
    }
}
