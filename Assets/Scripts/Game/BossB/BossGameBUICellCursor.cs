using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���X�{�X�{��@�Z���I���J�[�\��
/// </summary>
public class BossGameBUICellCursor : MonoBehaviour
{
    private const float PARTS_X = 50f;
    private const float PARTS_Y = 28f;

    #region �����o�[

    public Transform parts_ld;
    public Transform parts_rd;
    public Transform parts_ru;
    public Transform parts_lu;
    public GameObject star;

    #endregion

    /// <summary>
    /// �\�� ���ƍ����͎d�l�ォ�Ȃ炸�
    /// </summary>
    /// <param name="centerX"></param>
    /// <param name="centerY"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="showStar"></param>
    public void Show(int centerX, int centerY, int width, int height, bool showStar)
    {
        // ���S���W
        var centerPos = BossGameSystemB.GetCellLocation(centerX, centerY);
        transform.localPosition = centerPos;

        // ���̕\��
        star.SetActive(showStar);

        // �㉺���E�̈ʒu�ݒ�
        var addX = (width / 2) * BossGameSystemB.CELL_WIDTH + PARTS_X;
        var addY = (height / 2) * BossGameSystemB.CELL_HEIGHT + PARTS_Y;

        parts_ld.transform.localPosition = new Vector3(-addX, -addY);
        parts_rd.transform.localPosition = new Vector3(addX, -addY);
        parts_ru.transform.localPosition = new Vector3(addX, addY);
        parts_lu.transform.localPosition = new Vector3(-addX, addY);
    }
}
