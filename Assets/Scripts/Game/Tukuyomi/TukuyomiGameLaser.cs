using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// つくよみちゃん本戦　レーザー
/// </summary>
public class TukuyomiGameLaser : MonoBehaviour
{
    public SpriteRenderer model;

    /// <summary>
    /// 発射
    /// </summary>
    /// <param name="root">根本</param>
    /// <param name="rot">方向 右が0</param>
    public void Shoot(Vector3 root, float rot)
    {
        gameObject.SetActive(true);
        transform.position = root;
        transform.rotation = Util.GetRotateQuaternion(rot);
        StartCoroutine(ShootCoroutine());
    }

    /// <summary>
    /// 発射コルーチン
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
