using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// ＊＊＊＊戦　ダメージ数字１桁分
/// </summary>
public class BossGameADamageUIOne : MonoBehaviour
{
    private const float Y_APPEAR = 30f;
    private const float Y_START_SPEED = 1200f;
    private const float Y_ACCEL = -9000f;

    private float speed;
    private bool isActive;

    /// <summary>
    /// 数字表示
    /// </summary>
    /// <param name="num"></param>
    public void StartNum(int num)
    {
        GetComponent<TMP_Text>().SetText(num.ToString());
        transform.localPosition = new Vector3(transform.localPosition.x, Y_APPEAR);
        gameObject.SetActive(true);
        StartCoroutine(ShowNumCoroutine());
    }

    /// <summary>
    /// 数字表示コルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShowNumCoroutine()
    {
        isActive = true;
        speed = Y_START_SPEED;
        while (transform.localPosition.y > 0)
        {
            if (!isActive) yield break;
            speed += Y_ACCEL * Time.deltaTime;
            var y = transform.localPosition.y + speed * Time.deltaTime;
            if (y < 0) y = 0f;
            transform.localPosition = new Vector3(transform.localPosition.x, y);
            yield return null;
        }
        speed = -speed / 3f;
        transform.localPosition = new Vector3(transform.localPosition.x, 1);
        while (transform.localPosition.y > 0)
        {
            if (!isActive) yield break;
            speed += Y_ACCEL * Time.deltaTime;
            var y = transform.localPosition.y + speed * Time.deltaTime;
            if (y < 0) y = 0f;
            transform.localPosition = new Vector3(transform.localPosition.x, y);
            yield return null;
        }
        transform.localPosition = new Vector3(transform.localPosition.x, 0);
    }

    /// <summary>
    /// 数字消す
    /// </summary>
    public void Delete()
    {
        isActive = false;
        gameObject.SetActive(false);
    }
}
