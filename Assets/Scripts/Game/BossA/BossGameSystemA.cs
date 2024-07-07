using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

/// <summary>
/// ���X�{�X��P
/// </summary>
public class BossGameSystemA : GameSceneScriptBase
{
    #region �����o�[

    public BossGameATextUI messageWindow;
    public BossGameATextUI skillWindow;
    public BossGameADamageUI damage;

    public BossGameATukuyomi tukuyomi;
    public BossGameAReko reko;
    public BossGameAEnemy enemy;

    public BossGameAFireEffect fire;
    public BossGameAIceEffect ice;
    public BossGameAThunderEffect thunder;

    public BossGameASlowEffect slow;
    public BossGameAHolyEffect holy;
    public BossGameAMeteorEffect meteor;

    #endregion

    /// <summary>
    /// �t�F�[�h�C����
    /// </summary>
    /// <returns></returns>
    public override IEnumerator AfterFadeIn()
    {
        yield return base.AfterFadeIn();
        yield return new WaitForSeconds(2f);

        StartCoroutine(PlayEvent());
    }

    /// <summary>
    /// �C�x���g
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayEvent()
    {
        skillWindow.Show("�t�@�C�K");
        yield return tukuyomi.ToLeft();
        yield return tukuyomi.PlayMagicEffect();
        yield return new WaitForSeconds(0.2f);
        yield return fire.Play();
        yield return damage.ShowDamageCoroutine(Util.RandomInt(800, 2400));
        yield return tukuyomi.ToRight();
        skillWindow.Hide();

        yield return new WaitForSeconds(1f);

        skillWindow.Show("�X���E");
        yield return reko.ToLeft();
        yield return reko.PlayMagicEffect();
        yield return new WaitForSeconds(0.2f);
        yield return slow.Play();
        yield return new WaitForSeconds(0.2f);
        yield return reko.ToRight();
        skillWindow.Hide();

        yield return new WaitForSeconds(1f);

        skillWindow.Show("�u���U�K");
        yield return tukuyomi.ToLeft();
        yield return tukuyomi.PlayMagicEffect();
        yield return new WaitForSeconds(0.2f);
        yield return ice.Play();
        yield return damage.ShowDamageCoroutine(Util.RandomInt(800, 2400));
        yield return tukuyomi.ToRight();
        skillWindow.Hide();

        yield return new WaitForSeconds(1f);

        skillWindow.Show("�z�[���h");
        yield return reko.ToLeft();
        yield return reko.PlayMagicEffect();
        yield return new WaitForSeconds(0.5f);
        yield return reko.ToRight();
        skillWindow.Hide();

        yield return new WaitForSeconds(1f);

        skillWindow.Show("�T���_�K");
        yield return tukuyomi.ToLeft();
        yield return tukuyomi.PlayMagicEffect();
        yield return new WaitForSeconds(0.2f);
        yield return thunder.Play();
        yield return damage.ShowDamageCoroutine(Util.RandomInt(800, 2400));
        yield return tukuyomi.ToRight();
        skillWindow.Hide();

        yield return new WaitForSeconds(1f);

        skillWindow.Show("�z�[���[");
        yield return reko.ToLeft();
        yield return reko.PlayMagicEffect();
        yield return new WaitForSeconds(0.2f);
        yield return holy.Play();
        yield return damage.ShowDamageCoroutine(Util.RandomInt(2000, 3700));
        yield return reko.ToRight();
        skillWindow.Hide();

        yield return new WaitForSeconds(1f);

        messageWindow.Show("���u�����ЂƂ������@�p���[�����e�I��");
        yield return new WaitForSeconds(2f);
        messageWindow.Hide();
        yield return new WaitForSeconds(0.1f);
        messageWindow.Show("����݂����u�����ł��Ƃ��I");
        yield return new WaitForSeconds(2f);
        messageWindow.Hide();
        tukuyomi.KamaePose();
        yield return new WaitForSeconds(2.5f);

        skillWindow.Show("�v���e�I");
        StartCoroutine(tukuyomi.ToLeft());
        yield return reko.ToLeft();
        yield return new WaitForSeconds(0.2f);
        yield return meteor.Play();
        yield return damage.ShowDamageCoroutine(9999);
        StartCoroutine(tukuyomi.ToRight());
        yield return reko.ToRight();
        skillWindow.Hide();

        yield return new WaitForSeconds(1f);

        //�[���X���S
        yield return enemy.DefeatAnim();
        yield return new WaitForSeconds(0.3f);

        ManagerSceneScript.GetInstance().ExitGame();
    }
}
