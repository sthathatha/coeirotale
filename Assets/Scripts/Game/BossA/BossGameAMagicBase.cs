using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���������C�x���g��@�G�t�F�N�g�Ǘ�
/// </summary>
abstract public class BossGameAMagicBase : MonoBehaviour
{
    public ModelUtil fader;

    abstract public IEnumerator Play();
}
