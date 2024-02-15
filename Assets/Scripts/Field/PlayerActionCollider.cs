using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionCollider : MonoBehaviour
{
    private List<Collider2D> hitColliders;

    void Start()
    {
        hitColliders = new List<Collider2D> ();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hitColliders.Add (collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        hitColliders.Remove (collision);
    }

    /// <summary>
    /// 当たっているコリジョンのリスト
    /// </summary>
    /// <returns></returns>
    public List<Collider2D> GetHitList()
    {
        return hitColliders;
    }
}
