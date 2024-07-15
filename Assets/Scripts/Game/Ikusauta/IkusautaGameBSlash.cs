using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �}�`�@�{�X���b�V���p�@���̋O��
/// </summary>
public class IkusautaGameBSlash : MonoBehaviour
{
    private const float SLASH_TIME = 0.4f;

    /// <summary>
    /// �O�Ճ^�C�v
    /// </summary>

    public enum Type : int
    {
        /// <summary>�Ȑ�</summary>
        Curve = 0,
        /// <summary>����</summary>
        Line,
    }

    /// <summary>�Ȑ��e�N�X�`��</summary>
    public Sprite spr_curve;
    /// <summary>�����e�N�X�`��</summary>
    public Sprite spr_line;

    /// <summary>
    /// �\��
    /// </summary>
    /// <param name="type"></param>
    /// <param name="dir"></param>
    /// <param name="color"></param>
    public void Show(Type type, float dir, Color? color = null)
    {
        // �F
        var render = GetComponent<SpriteRenderer>();
        render.color = color ?? Color.white;

        // �摜�ƌ���
        var dirVec = new Vector3(Mathf.Cos(dir), Mathf.Sin(dir));
        if (type == Type.Line)
        {
            render.sprite = spr_line;
            transform.localRotation = Quaternion.FromToRotation(new Vector3(-0.7f, -0.7f), dirVec);
        }
        else
        {
            render.sprite = spr_curve;
            transform.localRotation = Quaternion.FromToRotation(new Vector3(-1f, 0), dirVec);
        }

        gameObject.SetActive(true);
        StartCoroutine(ShowCoroutine());
    }

    /// <summary>
    /// �t�F�[�h���Ď����ŏ�����R���[�`��
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShowCoroutine()
    {
        var render = GetComponent<SpriteRenderer>();
        var defaultColor = render.color;
        var alpha = new DeltaFloat();
        alpha.Set(defaultColor.a);
        alpha.MoveTo(0f, SLASH_TIME, DeltaFloat.MoveType.ACCEL);
        while (alpha.IsActive())
        {
            yield return null;
            alpha.Update(Time.deltaTime);

            render.color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, alpha.Get());
        }

        Destroy(gameObject);
    }
}
