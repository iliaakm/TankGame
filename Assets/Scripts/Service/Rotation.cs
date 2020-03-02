using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Rotation : MonoBehaviour
{

    public float Xspeed, Yspeed, Zspeed;

    private void FixedUpdate()
    {
        transform.Rotate(Xspeed, Yspeed, Zspeed);
    }
}
