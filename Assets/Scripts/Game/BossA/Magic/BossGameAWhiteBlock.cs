using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ＊＊＊＊戦　白魔法エフェクト
/// </summary>
public class BossGameAWhiteBlock : MonoBehaviour
{
    public Sprite block1;
    public Sprite block2;
    public Sprite block3;
    public Sprite block4;
    public Sprite block5;

    private DeltaFloat distance = new DeltaFloat();
    private float rotate = 0;

    /// <summary>
    /// 開始
    /// </summary>
    /// <param name="startRot"></param>
    public void Show(float startRot)
    {
        GetComponent<SpriteRenderer>().sprite = block1;
        rotate = startRot;
        distance.Set(0);
        StartCoroutine(BlockCoroutine());
        StartCoroutine(PositionCoroutine());
    }

    /// <summary>
    /// 画像切替コルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator BlockCoroutine()
    {
        var renderer = GetComponent<SpriteRenderer>();

        renderer.sprite = block1;
        yield return new WaitForSeconds(0.18f);
        renderer.sprite = block2;
        yield return new WaitForSeconds(0.18f);
        renderer.sprite = block3;
        yield return new WaitForSeconds(0.18f);
        renderer.sprite = block4;
        yield return new WaitForSeconds(0.18f);
        renderer.sprite = block5;
        yield return new WaitForSeconds(0.18f);
        Destroy(gameObject);
    }

    /// <summary>
    /// 位置制御コルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator PositionCoroutine()
    {
        distance.MoveTo(64f, 0.6f, DeltaFloat.MoveType.DECEL);
        while (true)
        {
            distance.Update(Time.deltaTime);
            rotate -= Time.deltaTime * Mathf.PI * 1.8f;
            if (rotate < 0) rotate += Mathf.PI * 2f;

            transform.localPosition = new Vector3(Mathf.Cos(rotate), Mathf.Sin(rotate)) * distance.Get();

            yield return null;
        }
    }
}
