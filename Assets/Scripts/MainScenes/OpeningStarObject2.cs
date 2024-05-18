using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningStarObject2 : MonoBehaviour
{
    private float rotSpeed;
    private float rotation;
    private DeltaVector3 position = new DeltaVector3();

    // Start is called before the first frame update
    void Start()
    {
        rotation = 0;
        rotSpeed = 360;

        position.Set(transform.localPosition);
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
    }

    public void SetPosition(Vector3 pos)
    {
        position.Set(pos);
    }

    public void MoveTo(Vector3 pos, float time, DeltaFloat.MoveType moveType)
    {
        position.MoveTo(pos, time, moveType);
    }

    public bool IsActive()
    {
        return position.IsActive();
    }
}
