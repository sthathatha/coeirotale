using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Util
{
    /// <summary>
    /// �T�C���J�[�u��float�ɕϊ�
    /// </summary>
    /// <param name="_val">0�`1</param>
    /// <param name="_type">�������I��</param>
    /// <returns>0�`1</returns>
    public static float SinCurve(float _val, Constant.SinCurveType _type)
    {
        float theta;
        switch (_type)
        {
            case Constant.SinCurveType.Accel:
                theta = Mathf.PI * (_val / 2f - 0.5f);
                return Mathf.Sin(theta) + 1f;
            case Constant.SinCurveType.Decel:
                theta = Mathf.PI * (_val / 2f);
                return Mathf.Sin(theta);
            case Constant.SinCurveType.Both:
                theta = Mathf.PI * (_val - 0.5f);
                return (Mathf.Sin(theta) + 1f) / 2f;
        }

        return 0f;
    }

    /// <summary>
    /// ��Ԓl
    /// </summary>
    /// <param name="_rate"></param>
    /// <param name="_val1"></param>
    /// <param name="_val2"></param>
    /// <returns></returns>
    public static float CalcBetweenFloat(float _rate, float _val1, float _val2)
    {
        return _val1 + (_val2 - _val1) * _rate;
    }

    /// <summary>
    /// �����_������ max�������̉\������
    /// </summary>
    /// <param name="min">�Œ�l</param>
    /// <param name="max">�ő�l</param>
    /// <returns></returns>
    public static int RandomInt(int min, int max)
    {
        return Random.Range(min, max + 1);
    }

    /// <summary>
    /// �����_������
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static float RandomFloat(float min, float max)
    {
        return Random.Range(min, max);
    }

    /// <summary>
    /// �d�����Ȃ������_�������i���т������_���j
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <param name="count">�� ��������Ƒ���Ȃ��Ȃ�̂ŋ֎~</param>
    /// <returns></returns>
    public static List<int> RandomUniqueIntList(int min, int max, int count)
    {
        if (max - min + 1 < count) { throw new System.Exception("RandomUniqueIntList��count��������"); }

        var list = new List<int>();
        for (int i = min; i <= max; ++i)
        {
            list.Add(i);
        }

        var ret = new List<int>();
        for (int i = 0; i < count; ++i)
        {
            var rand = RandomInt(0, list.Count - 1);
            ret.Add(list[rand]);
            list.RemoveAt(rand);
        }

        return ret;
    }

    /// <summary>
    /// �x�N�g����Direction�ɕϊ�
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public static Constant.Direction GetDirectionFromVec(Vector3 pos)
    {
        if (Mathf.Abs(pos.x) > Mathf.Abs(pos.y))
        {
            if (pos.x < 0)
                return Constant.Direction.Left;
            else
                return Constant.Direction.Right;
        }
        else
        {
            if (pos.y < 0)
                return Constant.Direction.Down;
            else
                return Constant.Direction.Up;
        }
    }

    /// <summary>
    /// Direction�̒P�ʃx�N�g��
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    public static Vector3 GetVector3FromDirection(Constant.Direction dir)
    {
        return dir switch
        {
            Constant.Direction.Right => new Vector3(1, 0),
            Constant.Direction.Up => new Vector3(0, 1),
            Constant.Direction.Down => new Vector3(0, -1),
            _ => new Vector3(-1, 0),
        };
    }

    /// <summary>
    /// Z����]�N�I�[�^�j�I���쐬�i�����v���j
    /// </summary>
    /// <param name="radian"></param>
    /// <returns></returns>
    public static Quaternion GetRotateQuaternion(float radian)
    {
        return Quaternion.Euler(0, 0, Mathf.Rad2Deg * radian);
    }
}
