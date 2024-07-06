using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// ＊＊＊＊戦　ダメージ数字
/// </summary>
public class BossGameADamageUI : MonoBehaviour
{
    public BossGameADamageUIOne num4;
    public BossGameADamageUIOne num3;
    public BossGameADamageUIOne num2;
    public BossGameADamageUIOne num1;

    /// <summary>
    /// ダメージ表示コルーチン
    /// </summary>
    /// <param name="dmg"></param>
    /// <returns></returns>
    public IEnumerator ShowDamageCoroutine(int dmg)
    {
        if (dmg >= 1000)
        {
            num4.StartNum(dmg / 1000);
            yield return new WaitForSeconds(0.07f);
        }
        if (dmg >= 100)
        {
            num3.StartNum(dmg / 100 % 10);
            yield return new WaitForSeconds(0.07f);
        }
        if (dmg >= 10)
        {
            num2.StartNum(dmg / 10 % 10);
            yield return new WaitForSeconds(0.07f);
        }
        num1.StartNum(dmg % 10);
        yield return new WaitForSeconds(0.8f);

        Hide();
    }

    /// <summary>
    /// 消す
    /// </summary>
    public void Hide()
    {
        num4.Delete();
        num3.Delete();
        num2.Delete();
        num1.Delete();
    }
}
