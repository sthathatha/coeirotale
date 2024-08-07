using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// HP�\��
/// </summary>
public class BossGameBUIHPShow : MonoBehaviour
{
    #region �����o�[

    public TMP_Text hp_max_text;

    #endregion

    /// <summary>
    /// HP�\��
    /// </summary>
    /// <param name="hp"></param>
    /// <param name="maxHp"></param>
    /// <param name="pos"></param>
    public void Show(int hp, int maxHp, Vector3 pos)
    {
        transform.localPosition = pos;
        var spaceCnt = maxHp.ToString().Length - hp.ToString().Length;
        var space = new string(' ', spaceCnt);
        hp_max_text.SetText($"{space}{hp}\n{maxHp}");

        gameObject.SetActive(true);
    }
}
