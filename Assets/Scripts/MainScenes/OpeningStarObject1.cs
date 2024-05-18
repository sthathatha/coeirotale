using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningStarObject1 : MonoBehaviour
{
    private float rotSpeed;
    private float rotation;
    private DeltaVector3 position;

    // Start is called before the first frame update
    void Start()
    {
        var scale = Util.RandomFloat(30f, 60f);
        transform.localScale = new Vector3(scale, scale, 1);

        rotation = 0;
        rotSpeed = Util.RandomFloat(360, 540);
        position = new DeltaVector3();

        var startY = Util.RandomFloat(-450f, 450f);
        position.Set(new Vector3(700f, startY, 0));
        position.MoveTo(new Vector3(-700f, startY + Util.RandomFloat(-100f, 100f), 0), Util.RandomFloat(0.8f, 1.3f), DeltaFloat.MoveType.LINE);
    }

    // Update is called once per frame
    void Update()
    {
        // ‰ñ“]
        var p2 = 360;
        rotation += Time.deltaTime * rotSpeed;
        var over = Mathf.Floor(rotation / p2);
        if (over > 0f)
        {
            rotation -= p2 * over;
        }

        position.Update(Time.deltaTime);
        transform.localPosition = position.Get();
        transform.localRotation = Quaternion.AngleAxis(rotation, new Vector3(0, 0, 1));

        if (position.IsActive() == false)
        {
            Destroy(gameObject);
        }
    }
}
