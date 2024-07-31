using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// ���X�{�X�{��@�}�X�I��UI
/// </summary>
public class BossGameBUICellSelect : MonoBehaviour
{
    #region �����o�[

    public GameObject grayMaskParent;
    public GameObject maskListParent;

    public Transform cursorParent;
    public BossGameBUICellCursor cursorDummy;

    #endregion

    #region �ϐ�

    /// <summary>�}�X�N</summary>
    private List<GameObject> maskList;

    /// <summary>�\�����J�[�\��</summary>
    private List<BossGameBUICellCursor> activeCursorList;

    #endregion

    #region ���

    /// <summary>
    /// �J�n��
    /// </summary>
    private void Start()
    {
        maskList = new List<GameObject>();
        activeCursorList = new List<BossGameBUICellCursor>();

        // �}�X�N�擾
        for (var i = 0; i < maskListParent.transform.childCount; ++i)
        {
            maskList.Add(maskListParent.transform.GetChild(i).gameObject);
        }

        // ������\��
        cursorDummy.gameObject.SetActive(false);
    }

    #endregion

    #region �@�\

    /// <summary>
    /// �\��
    /// </summary>
    /// <param name="maskEnableList">���邢�ꏊ�̃��X�g</param>
    /// <param name="cursorList">�J�[�\���\�����X�g</param>
    public void Show(List<Vector2Int> maskEnableList, List<CellUIParam> cursorList)
    {
        ShowMask(maskEnableList);
        ShowCursor(cursorList);
    }

    /// <summary>
    /// �}�X�N�\��
    /// </summary>
    /// <param name="maskEnableList"></param>
    public void ShowMask(List<Vector2Int> maskEnableList)
    {
        var enableNameList = maskEnableList.Select(p => $"mask{p.x}_{p.y}").ToList();

        foreach (var mask in maskList)
        {
            mask.SetActive(enableNameList.Contains(mask.name));
        }
    }

    /// <summary>
    /// �J�[�\���\��
    /// </summary>
    /// <param name="cursorList"></param>
    public void ShowCursor(List<CellUIParam> cursorList)
    {
        var newList = new List<BossGameBUICellCursor>();
        foreach(var p in cursorList)
        {
            BossGameBUICellCursor cursor;
            if (activeCursorList.Count > 0)
            {
                // ����Ԃ�ė��p
                cursor = activeCursorList[0];
                activeCursorList.RemoveAt(0);
            }
            else
            {
                // ������ΐV�K�쐬
                cursor = Instantiate(cursorDummy);
            }

            // �ʒu�ݒ�
            cursor.Show(p.x, p.y, p.width, p.height, p.enable);
            newList.Add(cursor);
        }

        // �g��Ȃ��������폜
        foreach(var old in activeCursorList)
        {
            Destroy(old.gameObject);
        }
        activeCursorList.Clear();
        activeCursorList = newList;
    }

    /// <summary>
    /// ��\��
    /// </summary>
    public void Hide()
    {
        grayMaskParent.SetActive(false);
        ShowCursor(new List<CellUIParam>());
    }

    #endregion

    #region �\���p�����[�^

    /// <summary>
    /// �\���w��p�����[�^
    /// </summary>
    public struct CellUIParam
    {
        /// <summary>���W</summary>
        public int x;
        /// <summary>���W</summary>
        public int y;
        /// <summary>���F�J�[�\���̂�</summary>
        public int width;
        /// <summary>�����F�J�[�\���̂�</summary>
        public int height;
        /// <summary>���\���F�J�[�\���̂�</summary>
        public bool enable;

        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        /// <param name="_x"></param>
        /// <param name="_y"></param>
        /// <param name="_width"></param>
        /// <param name="_height"></param>
        /// <param name="_enable"></param>
        public CellUIParam(int _x, int _y, int _width, int _height, bool _enable)
        {
            x = _x;
            y = _y;
            width = _width;
            height = _height;
            enable = _enable;
        }
    }

    #endregion
}
