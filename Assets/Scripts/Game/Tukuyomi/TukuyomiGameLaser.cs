using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����݂����{��@���[�U�[
/// </summary>
public class TukuyomiGameLaser : MonoBehaviour
{
    public SpriteRenderer model;

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="root">���{</param>
    /// <param name="rot">���� �E��0</param>
    public void Shoot(Vector3 root, float rot)
    {
        gameObject.SetActive(true);
        transform.position = root;
        transform.rotation = Util.GetRotateQuaternion(rot);
        StartCoroutine(ShootCoroutine());
    }

    /// <summary>
    /// ���˃R���[�`��
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShootCoroutine()
    {
        bool atk = true;
        var alpha = new DeltaFloat();
        var width = new DeltaFloat();
        alpha.Set(1f);
        width.Set(model.transform.localScale.y);
        alpha.MoveTo(0f, 0.3f, DeltaFloat.MoveType.LINE);
        width.MoveTo(1f, 0.3f, DeltaFloat.MoveType.DECEL);

        while (alpha.IsActive())
        {
            yield return null;
            alpha.Update(Time.deltaTime);
            width.Update(Time.deltaTime);

            if (atk && alpha.Get() < 0.8f)
            {
                GetComponent<Collider2D>().enabled = false;
                atk = false;
            }

            model.color = new Color(1, 1, 1, alpha.Get());
            model.transform.localScale = new Vector3(model.transform.localScale.x, width.Get(), 1);
        }

        Destroy(gameObject);
    }
}
