using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F143�@�������
/// </summary>
public class F143System : MainScriptBase
{
    /// <summary>�����f���ɂ������t���O</summary>
    public const string MENDERU_MEET_FLG = "BossMenderuMeet";
    /// <summary>�����f���ɏ������t���O</summary>
    public const string MENDERU_WIN_FLG = "BossMenderuWin";

    /// <summary>�J�n�C�x���g�����t���O</summary>
    public const string F143_SHOW_FLG = "F143Show";

    public CharacterScript koob;
    public CharacterScript menderu;

    /// <summary>
    /// �J�n��
    /// </summary>
    /// <returns></returns>
    public override IEnumerator BeforeInitFadeIn()
    {
        yield return base.BeforeInitFadeIn();

        if (Global.GetSaveData().GetGameDataInt(MENDERU_WIN_FLG) >= 1)
        {
            menderu.gameObject.SetActive(false);
        }

        // 3��ȏ�Ȃ�N�[���Ȃ�
        if (Global.GetSaveData().GetGameDataInt(F143_SHOW_FLG) >= 1 ||
            Global.GetSaveData().GetGameDataInt(F141System.FAIL_COUNT_SAVE) >= 3)
        {
            Global.GetSaveData().SetGameData(F143_SHOW_FLG, 1);

            koob.gameObject.SetActive(false);
        }
        else
        {
            InitPlayerPos(1);
        }
    }

    /// <summary>
    /// �t�F�[�h�C����
    /// </summary>
    /// <returns></returns>
    public override IEnumerator AfterFadeIn(bool init)
    {
        yield return base.AfterFadeIn(init);

        // 3��ȉ��Ȃ�N�[�C�x���g�J�n
        if (Global.GetSaveData().GetGameDataInt(F143_SHOW_FLG) <= 0 &&
            Global.GetSaveData().GetGameDataInt(F141System.FAIL_COUNT_SAVE) < 3)
        {
            GetComponent<F143Start>().ExecEvent();
        }
    }
}
