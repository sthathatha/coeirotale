using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PierreGameBGObject : MonoBehaviour
{
    public float speed = 1f;

    public const float OBJECT_WIDTH_MAX = 256f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var pos = transform.localPosition;
        pos.x += PierreGameSystemA.SCROLL_SPEED * speed * Time.deltaTime;
        transform.localPosition = pos;
       
        if (pos.x > (Constant.SCREEN_WIDTH + OBJECT_WIDTH_MAX) / 2f)
        {
            Destroy(gameObject);
        }
    }
}
