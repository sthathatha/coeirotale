using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

/// <summary>
/// ＊＊＊＊　ホーリー爆発１個
/// </summary>
public class BossGameAHolyBomb : MonoBehaviour
{
    public Sprite bomb1;
    public Sprite bomb2;
    public Sprite bomb3;
    public Sprite bomb4;

    /// <summary>
    /// 表示
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="addPriority"></param>
    public void Show(Vector3 pos, int addPriority)
    {
        // ランダム回転
        transform.localRotation = Quaternion.FromToRotation(new Vector3(1, 0, 0),
            Util.RandomInt(0, 3) switch
            {
                0 => new Vector3(1, 0, 0),
                1 => new Vector3(0, 1, 0),
                2 => new Vector3(-1, 0, 0),
                _ => new Vector3(0, -1, 0),
            }
            );

        transform.localPosition = pos;
        GetComponent<SpriteRenderer>().sprite = bomb1;
        GetComponent<SpriteRenderer>().sortingOrder += addPriority;
        gameObject.SetActive(true);
        StartCoroutine(ShowCoroutine());
    }

    /// <summary>
    /// 爆発して消えるコルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShowCoroutine()
    {
        var render = GetComponent<SpriteRenderer>();

        render.sprite = bomb1;
        yield return new WaitForSeconds(0.06f);
        render.sprite = bomb2;
        yield return new WaitForSeconds(0.06f);
        render.sprite = bomb3;
        yield return new WaitForSeconds(0.06f);
        render.sprite = bomb4;
        yield return new WaitForSeconds(0.06f);

        Destroy(gameObject);
    }
}
