using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TukuyomiGameResource : MonoBehaviour
{
    public AudioClip voice_start_first_0;
    public AudioClip voice_start_first_1;
    public AudioClip voice_start_first_2;
    public AudioClip voice_start_first_3;
    public AudioClip voice_start_second;

    public AudioClip voice_start2_first_0;
    public AudioClip voice_start2_first_1;
    public AudioClip voice_start2_second;

    public AudioClip voice_game_1_1;
    public AudioClip voice_game_1_2;
    public AudioClip voice_game_2_1;
    public AudioClip voice_game_2_2;
    public AudioClip voice_game_3_1;
    public AudioClip voice_game_3_2;
    public AudioClip voice_game_4_1;
    public AudioClip voice_game_4_2;
    public AudioClip voice_game_5_1;
    public AudioClip voice_game_5_2;

    public AudioClip se_koma_set;
    public AudioClip se_attack_effect_A;
    public AudioClip se_player_damage;
    public AudioClip se_koma_damage;
    public AudioClip se_kyou_battle_loop;





    /// <summary>
    /// SE���s
    /// </summary>
    /// <param name="se"></param>
    public void PlaySE(AudioClip se)
    {
        ManagerSceneScript.GetInstance().soundMan.PlaySE(se);
    }

    /// <summary>
    /// ���[�vSE
    /// </summary>
    /// <param name="se"></param>
    /// <returns></returns>
    public AudioSource PlaySELoop(AudioClip se)
    {
        return ManagerSceneScript.GetInstance().soundMan.PlaySELoop(se);
    }

    /// <summary>
    /// ���[�vSE��~
    /// </summary>
    /// <param name="se"></param>
    public void StopLoopSE(AudioSource se)
    {
        ManagerSceneScript.GetInstance().soundMan.StopLoopSE(se, 0.5f);
    }
}
