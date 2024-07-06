using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ＊＊＊＊戦　ファイガのしぶき
/// </summary>
public class BossGameAFireSplash : MonoBehaviour
{
    private Vector3 speed;

    /// <summary>
    /// 実行して消える
    /// </summary>
    /// <param name="start"></param>
    /// <param name="initSpeed"></param>
    public void PlayAndDestroy(Vector3 start, Vector3 initSpeed)
    {
        gameObject.SetActive(true);
        transform.localPosition = start;
        speed = initSpeed;

        UpdateRot();
        StartCoroutine(PlayCoroutine());
    }

    /// <summary>
    /// 実行して消えるコルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayCoroutine()
    {
        var tmp = new DeltaFloat();
        tmp.Set(speed.y);
        tmp.MoveTo(speed.y - 800f, 0.7f, DeltaFloat.MoveType.LINE);
        while (tmp.IsActive())
        {
            yield return null;
            tmp.Update(Time.deltaTime);
            speed.y = tmp.Get();
            transform.localPosition += speed * Time.deltaTime;
            UpdateRot();
        }

        Destroy(gameObject);
        yield break;
    }

    /// <summary>
    /// 現在のスピードにあわせて角度を更新
    /// </summary>
    private void UpdateRot()
    {
        transform.rotation = Quaternion.FromToRotation(new Vector3(1, 0, 0), speed);
    }
}
