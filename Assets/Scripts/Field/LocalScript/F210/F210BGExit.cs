using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// F205 �o���̌�����
/// </summary>
public class F210BGExit : MonoBehaviour
{
    public SpriteRenderer bg0;
    public SpriteRenderer bg1;
    public SpriteRenderer bg2;
    public SpriteRenderer bg3;
    public SpriteRenderer bg4;
    public SpriteRenderer bg5;

    private List<SpriteRenderer> sprites = new List<SpriteRenderer>();

    private const float UPD_INTERVAL = 0.4f;
    private float updTime = UPD_INTERVAL;

    private int color_Idx = 0;

    /// <summary>
    /// �J�n��
    /// </summary>
    private void Start()
    {
        sprites.Add(bg0);
        sprites.Add(bg1);
        sprites.Add(bg2);
        sprites.Add(bg3);
        sprites.Add(bg4);
        sprites.Add(bg5);

        for (var i = sprites.Count - 1; i >= 0; i--)
        {
            sprites[i].color = GetNextColor();
        }
    }

    /// <summary>
    /// �F��V�K�쐬
    /// </summary>
    /// <returns></returns>
    private Color GetNextColor()
    {
        var colSeed = Util.RandomFloat(0.05f, 0.15f) + (color_Idx * 0.25f);

        color_Idx--;
        if (color_Idx < 0) color_Idx = 3;

        return new Color(colSeed, colSeed, colSeed);
    }

    /// <summary>
    /// �X�V
    /// </summary>
    private void Update()
    {
        updTime -= Time.deltaTime;
        if (updTime > 0f) return;

        updTime += UPD_INTERVAL;

        // �F�X�V
        for (var i = sprites.Count - 1; i > 0; i--)
        {
            sprites[i].color = sprites[i - 1].color;
        }
        sprites[0].color = GetNextColor();
    }
}
