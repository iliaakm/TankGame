using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medicine : MonoBehaviour
{
    public int healValue;       //значение излечения
    // Use this for initialization

    private void OnCollisionEnter(Collision collision)      //при столкновении с игроком, добавить ему звдоровье
    {
        if (collision.transform.tag == "Player")
        {
            collision.transform.GetComponent<Tank>().SetHeal(healValue);
            GameObject.FindObjectOfType<GameController>().SetUITextInfo("+" + healValue + " HP", Color.green);
            GameObject.FindObjectOfType<GameController>().SpawnMedicine();
            Destroy(gameObject);
        }
    }
}
