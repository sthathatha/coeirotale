using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ＊＊＊＊　ボス
/// </summary>
public class BossGameAEnemy : MonoBehaviour
{
    private const float MASK_INIT_Y = 400f;
    private const float MASK_END_Y = -20f;

    private const float DEFEAT_TIME = 18f;

    public Transform model;
    public Transform defeatMask;

    public AudioClip defeatSe1;
    public AudioClip defeatSe2;

    /// <summary>
    /// 初期化
    /// </summary>
    void Start()
    {
        defeatMask.transform.localPosition = new Vector3(0, MASK_INIT_Y);
    }

    /// <summary>
    /// 撃破演出
    /// </summary>
    /// <returns></returns>
    public IEnumerator DefeatAnim()
    {
        StartCoroutine(SeCoroutine());
        var maskY = new DeltaFloat();
        maskY.Set(MASK_INIT_Y);
        maskY.MoveTo(MASK_END_Y, DEFEAT_TIME, DeltaFloat.MoveType.LINE);

        var rotate = Mathf.PI / 2f;
        while (maskY.IsActive())
        {
            yield return null;
            maskY.Update(Time.deltaTime);
            defeatMask.transform.localPosition = new Vector3(0, maskY.Get());

            rotate += Mathf.PI * 50f * Time.deltaTime;
            if (rotate > Mathf.PI * 2f) rotate -= Mathf.PI * 2f;
            var x = Mathf.Cos(rotate) * 4f;
            model.transform.localPosition = new Vector3(x, 0, 0);
        }
    }

    /// <summary>
    /// 撃破SEコルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator SeCoroutine()
    {
        var sound = ManagerSceneScript.GetInstance().soundMan;
        var time_se1 = 0.08f;
        var time_se2 = 0.3f;
        var cnt = Mathf.FloorToInt(DEFEAT_TIME / (time_se1 + time_se2));

        for(var i =0; i<cnt; ++i)
        {
            sound.PlaySE(defeatSe1);
            yield return new WaitForSeconds(time_se1);
            sound.PlaySE(defeatSe2);
            yield return new WaitForSeconds(time_se2);
        }
    }
}
