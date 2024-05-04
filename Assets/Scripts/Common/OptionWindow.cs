using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// オプションウィンドウ
/// </summary>
public class OptionWindow : MonoBehaviour
{
    #region メンバー

    /// <summary>BGMスライダー</summary>
    public Slider bgmSlider;
    /// <summary>SEスライダー</summary>
    public Slider seSlider;
    /// <summary>ボイススライダー</summary>
    public Slider voiceSlider;

    /// <summary>カーソル</summary>
    public GameObject cursor;

    #endregion

    #region 変数

    /// <summary>カーソル</summary>
    private enum Cursor : int
    {
        Bgm = 0,
        Se,
        Voice,
    }
    /// <summary>カーソル</summary>
    private Cursor cursorPos;

    #endregion

    #region 使用メソッド
    /// <summary>
    /// 開くコルーチン
    /// </summary>
    /// <returns></returns>
    public IEnumerator OpenDialog()
    {
        var input = InputManager.GetInstance();
        var sound = ManagerSceneScript.GetInstance().soundMan;

        UpdateCursor();
        UpdateSlider();
        sound.PlaySE(sound.commonSeWindowOpen);
        gameObject.SetActive(true);

        yield return null;
        while (!input.GetKeyPress(InputManager.Keys.South) &&
                !input.GetKeyPress(InputManager.Keys.East))
        {
            yield return null;

            // 上下移動
            if (input.GetKeyPress(InputManager.Keys.Up) || input.GetKeyPress(InputManager.Keys.Down))
            {
                sound.PlaySE(sound.commonSeMove);
                cursorPos += input.GetKeyPress(InputManager.Keys.Up) ? -1 : 1;
                if (cursorPos < 0) cursorPos = Cursor.Voice;
                else if (cursorPos > Cursor.Voice) cursorPos = Cursor.Bgm;
                UpdateCursor();
            }

            // ボリューム変更
            var vol = 0;
            if (input.GetKeyPress(InputManager.Keys.Right)) vol = 1;
            else if (input.GetKeyPress(InputManager.Keys.Left)) vol = -1;

            if (vol != 0)
            {
                switch (cursorPos)
                {
                    case Cursor.Bgm:
                        Global.GetSaveData().system.bgmVolume = Mathf.Clamp(Global.GetSaveData().system.bgmVolume + vol, 0, 10);
                        sound.UpdateBgmVolume();
                        break;
                    case Cursor.Se:
                        Global.GetSaveData().system.seVolume = Mathf.Clamp(Global.GetSaveData().system.seVolume + vol, 0, 10);
                        sound.UpdateSeVolume();
                        break;
                    case Cursor.Voice:
                        Global.GetSaveData().system.voiceVolume = Mathf.Clamp(Global.GetSaveData().system.voiceVolume + vol, 0, 10);
                        sound.UpdateVoiceVolume();
                        break;
                }

                sound.PlaySE(sound.commonSeMove);
                UpdateSlider();
                Global.GetSaveData().SaveSystemData();
            }
        }

        sound.PlaySE(sound.commonSeSelect);
        gameObject.SetActive(false);
    }
    #endregion

    #region プライベート

    /// <summary>
    /// カーソル位置更新
    /// </summary>
    private void UpdateCursor()
    {
        cursor.transform.localPosition = new Vector3(0,
            cursorPos switch
            {
                Cursor.Bgm => 70,
                Cursor.Se => 0,
                _ => -70,
            },
            0); ;
    }

    /// <summary>
    /// スライダー表示更新
    /// </summary>
    private void UpdateSlider()
    {
        var optionData = Global.GetSaveData().system;

        bgmSlider.value = optionData.bgmVolume;
        seSlider.value = optionData.seVolume;
        voiceSlider.value = optionData.voiceVolume;
    }

    #endregion
}
