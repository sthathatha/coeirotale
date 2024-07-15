using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// マチ　ボスラッシュ用　剣の軌跡
/// </summary>
public class IkusautaGameBSlash : MonoBehaviour
{
    private const float SLASH_TIME = 0.4f;

    /// <summary>
    /// 軌跡タイプ
    /// </summary>

    public enum Type : int
    {
        /// <summary>曲線</summary>
        Curve = 0,
        /// <summary>直線</summary>
        Line,
    }

    /// <summary>曲線テクスチャ</summary>
    public Sprite spr_curve;
    /// <summary>直線テクスチャ</summary>
    public Sprite spr_line;

    /// <summary>
    /// 表示
    /// </summary>
    /// <param name="type"></param>
    /// <param name="dir"></param>
    /// <param name="color"></param>
    public void Show(Type type, float dir, Color? color = null)
    {
        // 色
        var render = GetComponent<SpriteRenderer>();
        render.color = color ?? Color.white;

        // 画像と向き
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
    /// フェードして自動で消えるコルーチン
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
