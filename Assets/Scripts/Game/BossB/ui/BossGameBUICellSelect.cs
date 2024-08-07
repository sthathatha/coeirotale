using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ���X�{�X�{��@�}�X�I��UI
/// </summary>
public class BossGameBUICellSelect : MonoBehaviour
{
    #region �萔

    /// <summary>
    /// �Z���I������
    /// </summary>
    public enum CellSelectResult
    {
        CellSelect = 0,
        Cancel,
    }

    #endregion

    #region �����o�[

    public BossGameSystemB system;

    public GameObject grayMaskParent;
    public GameObject maskListParent;

    public Transform cursorParent;
    public BossGameBUICellCursor cursorDummy;

    #endregion

    #region �ʒu�I���@�\

    /// <summary>�I���ʒu</summary>
    public Vector2Int SelectCell { get; set; }

    /// <summary>�I������</summary>
    public CellSelectResult Result { get; set; }

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
        grayMaskParent.SetActive(true);
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
        foreach (var p in cursorList)
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
                cursor.transform.SetParent(cursorParent);
            }

            // �ʒu�ݒ�
            cursor.Show(p.location, p.width, p.height, p.enable);
            newList.Add(cursor);
        }

        // �g��Ȃ��������폜
        foreach (var old in activeCursorList)
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

    #region �I���@�\

    /// <summary>
    /// �\���ƑI��
    /// </summary>
    /// <param name="skillID"></param>
    /// <param name="chara"></param>
    /// <returns></returns>
    public IEnumerator ShowSelect(BossGameBDataBase.SkillID skillID, BossGameBCharacterBase chara)
    {
        var sound = ManagerSceneScript.GetInstance().soundMan;
        var input = InputManager.GetInstance();
        var skill = BossGameBDataBase.SkillList[skillID];

        // �I���\���X�g�쐬
        var cellList = BossGameSystemB.CreateEnableCellList(skillID, chara);

        // �����ʒu��I��
        SelectCell = cellList.Count > 0 ? cellList[0] : chara.GetLocation();
        // �I��Ώۃ^�C�v
        BossGameBCharacterBase.CharaType targetType = BossGameBCharacterBase.CharaType.Player;
        if (skill.TargetType == BossGameBDataBase.TargetTypeEnum.Fellow &&
            chara.CharacterType == BossGameBCharacterBase.CharaType.Player ||
            skill.TargetType == BossGameBDataBase.TargetTypeEnum.Enemy &&
            chara.CharacterType == BossGameBCharacterBase.CharaType.Enemy)
        {
            targetType = BossGameBCharacterBase.CharaType.Player;
        }
        else
        {
            targetType = BossGameBCharacterBase.CharaType.Enemy;
        }

        // �^�[�Q�b�g���I���ʒu���Ɍ���������I���A������΍ŏ��̈ʒu
        foreach (var cell in cellList)
        {
            var cellChara = system.GetCellCharacter(cell);
            // �L���������Ȃ�
            if (cellChara == null) continue;

            if (skill.TargetType != BossGameBDataBase.TargetTypeEnum.All)
            {
                // �^�[�Q�b�g�łȂ�
                if (cellChara.CharacterType != targetType) continue;
            }

            // �����Ɍ���
            SelectCell = cell;
        }

        // �I���J�[�\���p�����[�^ �����\��
        var cursorParam = new CellUIParam(SelectCell, skill.EffectRange * 2 + 1, skill.EffectRange * 2 + 1, true);
        Show(cellList, new List<CellUIParam>() { cursorParam });

        while (true)
        {
            yield return null;

            if (input.GetKeyPress(InputManager.Keys.South))
            {
                Hide();
                Result = CellSelectResult.CellSelect;
                break;
            }
            else if (input.GetKeyPress(InputManager.Keys.East))
            {
                Hide();
                Result = CellSelectResult.Cancel;
                break;
            }

            if (input.GetKeyPress(InputManager.Keys.Up) ||
                input.GetKeyPress(InputManager.Keys.Down) ||
                input.GetKeyPress(InputManager.Keys.Right) ||
                input.GetKeyPress(InputManager.Keys.Left))
            {
                // �ړ�
                sound.PlaySE(sound.commonSeMove);

                if (input.GetKeyPress(InputManager.Keys.Up))
                    SelectCell = SelectMoveCell(cellList, SelectCell, InputManager.Keys.Up);
                else if (input.GetKeyPress(InputManager.Keys.Down))
                    SelectCell = SelectMoveCell(cellList, SelectCell, InputManager.Keys.Down);
                else if (input.GetKeyPress(InputManager.Keys.Right))
                    SelectCell = SelectMoveCell(cellList, SelectCell, InputManager.Keys.Right);
                else if (input.GetKeyPress(InputManager.Keys.Left))
                    SelectCell = SelectMoveCell(cellList, SelectCell, InputManager.Keys.Left);

                cursorParam.location = SelectCell;
                ShowCursor(new List<CellUIParam>() { cursorParam });
            }
        }
    }

    #endregion

    #region �\���p�����[�^�N���X

    /// <summary>
    /// �\���w��p�����[�^
    /// </summary>
    public class CellUIParam
    {
        /// <summary>���W</summary>
        public Vector2Int location;
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
        public CellUIParam(Vector2Int _location, int _width, int _height, bool _enable)
        {
            location = _location;
            width = _width;
            height = _height;
            enable = _enable;
        }
    }

    #endregion

    #region �@�\���\�b�h

    /// <summary>
    /// �ړ���Z����I��
    /// </summary>
    /// <param name="cellList"></param>
    /// <param name="now"></param>
    /// <param name="moveDir"></param>
    /// <returns></returns>
    private Vector2Int SelectMoveCell(List<Vector2Int> cellList, Vector2Int now, InputManager.Keys moveDir)
    {
        if (!cellList.Any()) return now;
        var minAbs = 0;
        var targetLoc = 0;

        if (moveDir == InputManager.Keys.Up)
        {
            // �^���T��
            var nextList = cellList.Where(c => c.y > now.y && c.x == now.x);
            if (nextList.Any())
            {
                return new Vector2Int(now.x, nextList.Min(c => c.y));
            }

            // ������ΐ^��
            nextList = cellList.Where(c => c.y < now.y && c.x == now.x);
            if (nextList.Any())
            {
                return new Vector2Int(now.x, nextList.Min(c => c.y));
            }

            // ������Έ�ԋ߂�����
            nextList = cellList.Where(c => c.y > now.y);
            if (nextList.Any())
            {
                targetLoc = nextList.Min(c => c.y);
                nextList = nextList.Where(c => c.y == targetLoc);
                minAbs = nextList.Min(c => Mathf.Abs(c.x - now.x));
                foreach (var c in nextList)
                {
                    if (Mathf.Abs(c.x - now.x) == minAbs) return c;
                }
            }

            // ������Έ�ԉ��ɂ������
            targetLoc = cellList.Min(c => c.y);
            nextList = cellList.Where(c => c.y == targetLoc);
            minAbs = nextList.Min(c => Mathf.Abs(c.x - now.x));
            foreach (var c in nextList)
            {
                if (Mathf.Abs(c.x - now.x) == minAbs) return c;
            }
        }
        else if (moveDir == InputManager.Keys.Down)
        {
            // �^����T��
            var nextList = cellList.Where(c => c.y < now.y && c.x == now.x);
            if (nextList.Any())
            {
                return new Vector2Int(now.x, nextList.Max(c => c.y));
            }

            // ������ΐ^��
            nextList = cellList.Where(c => c.y > now.y && c.x == now.x);
            if (nextList.Any())
            {
                return new Vector2Int(now.x, nextList.Max(c => c.y));
            }

            // ������Έ�ԋ߂�����
            nextList = cellList.Where(c => c.y < now.y);
            if (nextList.Any())
            {
                targetLoc = nextList.Max(c => c.y);
                nextList = nextList.Where(c => c.y == targetLoc);
                minAbs = nextList.Min(c => Mathf.Abs(c.x - now.x));
                foreach (var c in nextList)
                {
                    if (Mathf.Abs(c.x - now.x) == minAbs) return c;
                }
            }

            // ������Έ�ԏ�ɂ������
            targetLoc = cellList.Max(c => c.y);
            nextList = cellList.Where(c => c.y == targetLoc);
            minAbs = nextList.Min(c => Mathf.Abs(c.x - now.x));
            foreach (var c in nextList)
            {
                if (Mathf.Abs(c.x - now.x) == minAbs) return c;
            }
        }
        else if (moveDir == InputManager.Keys.Right)
        {
            // �^�E��T��
            var nextList = cellList.Where(c => c.x > now.x && c.y == now.y);
            if (nextList.Any())
            {
                return new Vector2Int(nextList.Min(c => c.x), now.y);
            }

            // ������ΐ^��
            nextList = cellList.Where(c => c.x < now.x && c.y == now.y);
            if (nextList.Any())
            {
                return new Vector2Int(nextList.Min(c => c.x), now.y);
            }

            // ������Έ�ԋ߂�����
            nextList = cellList.Where(c => c.x > now.x);
            if (nextList.Any())
            {
                targetLoc = nextList.Min(c => c.x);
                nextList = nextList.Where(c => c.x == targetLoc);
                minAbs = nextList.Min(c => Mathf.Abs(c.y - now.y));
                foreach (var c in nextList)
                {
                    if (Mathf.Abs(c.y - now.y) == minAbs) return c;
                }
            }

            // ������Έ�ԍ��ɂ������
            targetLoc = cellList.Min(c => c.x);
            nextList = cellList.Where(c => c.x == targetLoc);
            minAbs = nextList.Min(c => Mathf.Abs(c.y - now.y));
            foreach (var c in nextList)
            {
                if (Mathf.Abs(c.y - now.y) == minAbs) return c;
            }
        }
        else if (moveDir == InputManager.Keys.Left)
        {
            // �^����T��
            var nextList = cellList.Where(c => c.x < now.x && c.y == now.y);
            if (nextList.Any())
            {
                return new Vector2Int(nextList.Max(c => c.x), now.y);
            }

            // ������ΐ^�E
            nextList = cellList.Where(c => c.x > now.x && c.y == now.y);
            if (nextList.Any())
            {
                return new Vector2Int(nextList.Max(c => c.x), now.y);
            }

            // ������Έ�ԋ߂�����
            nextList = cellList.Where(c => c.x < now.x);
            if (nextList.Any())
            {
                targetLoc = nextList.Max(c => c.x);
                nextList = nextList.Where(c => c.x == targetLoc);
                minAbs = nextList.Min(c => Mathf.Abs(c.y - now.y));
                foreach (var c in nextList)
                {
                    if (Mathf.Abs(c.y - now.y) == minAbs) return c;
                }
            }

            // ������Έ�ԍ��ɂ������
            targetLoc = cellList.Max(c => c.x);
            nextList = cellList.Where(c => c.x == targetLoc);
            minAbs = nextList.Min(c => Mathf.Abs(c.y - now.y));
            foreach (var c in nextList)
            {
                if (Mathf.Abs(c.y - now.y) == minAbs) return c;
            }
        }

        return now;
    }

    #endregion
}
