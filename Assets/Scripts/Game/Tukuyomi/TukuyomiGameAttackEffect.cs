using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ç¬Ç≠ÇÊÇ›ÇøÇ·ÇÒêÌçUåÇ
/// </summary>
public class TukuyomiGameAttackEffect : MonoBehaviour
{
    public SpriteRenderer sq1;
    public SpriteRenderer sq2;

    /// <summary>
    /// çUåÇ
    /// </summary>
    public void Attack(Vector3 pos)
    {
        gameObject.SetActive(true);
        GetComponent<Collider2D>().enabled = true;
        transform.localPosition = pos;
        StartCoroutine(AttackCoroutine());
    }

    /// <summary>
    /// çUåÇ
    /// </summary>
    /// <returns></returns>
    private IEnumerator AttackCoroutine()
    {
        bool atk = true;
        var r = 0f;
        var alpha = new DeltaFloat();
        alpha.Set(0.8f);
        alpha.MoveTo(0f, 0.4f, DeltaFloat.MoveType.LINE);
        UpdateAlpha(alpha.Get());
        while (alpha.IsActive())
        {
            yield return null;
            if (atk && alpha.Get() < 0.5f)
            {
                GetComponent<Collider2D>().enabled = false;
                atk = false;
            }

            alpha.Update(Time.deltaTime);
            r += Mathf.PI * Time.deltaTime;
            sq1.transform.localRotation = Util.GetRotateQuaternion(r);
            sq2.transform.localRotation = Util.GetRotateQuaternion(-r);

            UpdateAlpha(alpha.Get());
        }

        Destroy(gameObject);
    }

    /// <summary>
    /// ÉAÉãÉtÉ@ê›íË
    /// </summary>
    /// <param name="a"></param>
    private void UpdateAlpha(float a)
    {
        var col = new Color(1, 1, 1, a);
        sq1.color = col;
        sq2.color = col;
    }
}
