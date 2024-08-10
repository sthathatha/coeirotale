using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �萔
/// </summary>
public class Constant
{
    /// <summary>
    /// �T�C���J�[�u
    /// </summary>
    public enum SinCurveType : int
    {
        /// <summary>����</summary>
        Accel = 0,
        /// <summary>����</summary>
        Decel,
        /// <summary>������</summary>
        Both,
    }

    /// <summary>
    /// ��ʕ�
    /// </summary>
    public const float SCREEN_WIDTH = 1280f;
    /// <summary>
    /// ��ʍ���
    /// </summary>
    public const float SCREEN_HEIGHT = 720f;

    /// <summary>
    /// ����
    /// </summary>
    public enum Direction : int
    {
        Up = 0,
        Down,
        Left,
        Right,
    }
}
