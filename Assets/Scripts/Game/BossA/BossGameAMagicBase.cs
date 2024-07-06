using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ＊＊＊＊イベント戦　エフェクト管理
/// </summary>
abstract public class BossGameAMagicBase : MonoBehaviour
{
    public ModelUtil fader;

    abstract public IEnumerator Play();
}
