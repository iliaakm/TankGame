using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{   
    int _damage;        //величина урона
    public int damage
    {
        get { return _damage; }
        set { _damage = value; }
    }

    float _lifeTime;        //время жизни снаряда
    public float lifeTime
    {
        get { return _lifeTime; }
        set { _lifeTime = value; }
    }

    float spawnTime;        //время спавна

    private void FixedUpdate()
    {
        if (spawnTime > lifeTime)
            Destroy(gameObject);
        spawnTime += Time.fixedDeltaTime;
    }

    private void OnCollisionEnter(Collision collision)      //при попадании в монстра, нанести ему урон
    {
        if (collision.transform.tag == "Enemy")
        {
            collision.transform.GetComponent<Monster>().SetDamage(damage);            
        }
        Destroy(gameObject);
    }
}
