using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public int health;      //велечина здоровья
    public int damage;      //величина урона
    public float speed;     //величина скорости
    public ParticleSystem deadParticle;     //партикл смерти
    [Range(0, 1)]
    public float protection;        //величина защиты

    Transform _targetTransform;     //позиция цели
    public Transform targetTransform
    {
        get { return _targetTransform; }
        set { _targetTransform = value; }
    }

    GameController gameController;      //скрипт контроллера

    void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }

    private void FixedUpdate()
    {
        if (targetTransform)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, speed * Time.deltaTime);
            transform.LookAt(targetTransform);
        }
    }

    public void SetDamage(int hp)       //получить урон
    {
        health -= (int)(hp * protection);
        if (health <= 0)
        {
            gameController.SpawnMonsters(1);
            Dead();
            gameController.AddScore(100);
        }
    }

    private void OnCollisionEnter(Collision collision)      //при столкновении с игроком, нанести ему урон
    {
        if (collision.transform.tag == "Player")
        {
            collision.transform.GetComponent<Tank>().SetDamage(damage);
            gameController.SpawnMonsters(1);
            Dead();
        }
    }

    void Dead()     //смерть монстра
    {
        if (deadParticle)
        {
            ParticleSystem particle = Instantiate(deadParticle);
            particle.transform.position = transform.position;
            particle.Play();
        }        
        Destroy(gameObject);
    }
}
