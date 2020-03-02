using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float lifeTime;      //время жизни

    // Use this for initialization
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
