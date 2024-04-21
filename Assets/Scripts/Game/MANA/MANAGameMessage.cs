using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MANAGameMessage : MonoBehaviour
{
    private DeltaFloat _alpha = new DeltaFloat();

    void Start()
    {
        _alpha.Set(1);
    }

    void Update()
    {
        if (_alpha.IsActive())
        {
            _alpha.Update(Time.deltaTime);
            GetComponent<CanvasGroup>().alpha = _alpha.Get();
        }
    }

    public void SetAlpha(float a, float time = -1)
    {
        if (time > 0f)
        {
            _alpha.MoveTo(a, time, DeltaFloat.MoveType.LINE);
        }
        else
        {
            _alpha.Set(a);
            GetComponent<CanvasGroup>().alpha = a;
        }
    }

    public void SetText(string t)
    {
        GetComponent<TMP_Text>().SetText(t);
    }
}
