using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ラスボス本戦　ターン表示UI
/// </summary>
public class BossGameBUITurnShow : MonoBehaviour
{
    /// <summary>ダミー</summary>
    public BossGameBUITurnShowOne dummy;

    /// <summary></summary>
    private List<BossGameBUITurnShowOne> itemList;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public BossGameBUITurnShow()
    {
        itemList = new List<BossGameBUITurnShowOne>();
    }

    /// <summary>
    /// 表示
    /// </summary>
    /// <param name="idList"></param>
    public void Show(List<BossGameSystemB.CharacterID> idList, bool deleteTop = true)
    {
        var newItemList = new List<BossGameBUITurnShowOne>();

        // 先頭は使い回さない
        if (deleteTop && itemList.Count > 0)
        {
            itemList[0].Disappear();
            itemList.RemoveAt(0);
        }

        foreach (var idItem in idList.Select((id, idx) => new { id, idx }))
        {
            // 既にあるアイテムを探す
            var usedItem = itemList.Find(i => i.GetCharacterID() == idItem.id);
            if (usedItem != null)
            {
                // あったら使う
                itemList.Remove(usedItem);
            }
            else
            {
                // 無ければ作成
                usedItem = Instantiate(dummy);
                usedItem.transform.SetParent(transform);
                //※プレイヤーは一人だけなので
                usedItem.SetFace(idItem.id, idItem.id == BossGameSystemB.CharacterID.Player);
                usedItem.gameObject.SetActive(true);
            }

            usedItem.MoveTo(idItem.idx);
            newItemList.Add(usedItem);
        }

        // 使わなかったアイテムを消す
        foreach (var item in itemList)
        {
            item.Disappear();
        }
        itemList.Clear();
        // 新アイテムリスト
        itemList = newItemList;
    }
}
