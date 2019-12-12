using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public void BombExplode()
    {
        //Manager.Instance.Bombar(transform.position);
        Destroy(gameObject);
    }
}
