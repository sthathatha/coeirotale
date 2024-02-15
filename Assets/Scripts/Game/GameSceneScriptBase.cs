using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneScriptBase : MonoBehaviour
{
    // Start is called before the first frame update
    virtual public void Start()
    {
        ManagerSceneScript.GetInstance().SetGameScript(this);
    }

    // Update is called once per frame
    virtual public void Update()
    {
        
    }
}
