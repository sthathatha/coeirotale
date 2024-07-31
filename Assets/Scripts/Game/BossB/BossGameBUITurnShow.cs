using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ���X�{�X�{��@�^�[���\��UI
/// </summary>
public class BossGameBUITurnShow : MonoBehaviour
{
    /// <summary>�_�~�[</summary>
    public BossGameBUITurnShowOne dummy;

    /// <summary></summary>
    private List<BossGameBUITurnShowOne> itemList;

    /// <summary>
    /// �R���X�g���N�^
    /// </summary>
    public BossGameBUITurnShow()
    {
        itemList = new List<BossGameBUITurnShowOne>();
    }

    /// <summary>
    /// �\��
    /// </summary>
    /// <param name="idList"></param>
    public void Show(List<BossGameSystemB.CharacterID> idList, bool deleteTop = true)
    {
        var newItemList = new List<BossGameBUITurnShowOne>();

        // �擪�͎g���񂳂Ȃ�
        if (deleteTop && itemList.Count > 0)
        {
            itemList[0].Disappear();
            itemList.RemoveAt(0);
        }

        foreach (var idItem in idList.Select((id, idx) => new { id, idx }))
        {
            // ���ɂ���A�C�e����T��
            var usedItem = itemList.Find(i => i.GetCharacterID() == idItem.id);
            if (usedItem != null)
            {
                // ��������g��
                itemList.Remove(usedItem);
            }
            else
            {
                // ������΍쐬
                usedItem = Instantiate(dummy);
                usedItem.transform.SetParent(transform);
                //���v���C���[�͈�l�����Ȃ̂�
                usedItem.SetFace(idItem.id, idItem.id == BossGameSystemB.CharacterID.Player);
                usedItem.gameObject.SetActive(true);
            }

            usedItem.MoveTo(idItem.idx);
            newItemList.Add(usedItem);
        }

        // �g��Ȃ������A�C�e��������
        foreach (var item in itemList)
        {
            item.Disappear();
        }
        itemList.Clear();
        // �V�A�C�e�����X�g
        itemList = newItemList;
    }
}
