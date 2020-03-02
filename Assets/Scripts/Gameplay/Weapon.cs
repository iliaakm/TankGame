using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    physical,
    raycast
};

public class Weapon : MonoBehaviour
{
    public WeaponType weaponType;       //тип оружия
    public float firerate;      //скорстрельность в минуту
    public float shotVelocity;      //скорость снаряда
    public float shotLifetime;      //время жизни снаряда
    public int shotDamage;      //величина урона
    public GameObject shotPref;     //префаб снаряда
    public Transform shotOrigin;        //место выстрела
    public AudioClip shotSound;     //звук выстрела
    public ParticleSystem[] shotParticles;      //партиклы выстрела
    public bool canShoot;       //можно ли стрелять

    float shotLast, shotDelay;      //время последнего выстрела и задержка между выстрелами
    AudioSource audioSource;        //источник звука
    GameController gameController;      //констроллер

    // Use this for initialization
    void Start()
    {
        shotDelay = 60 / firerate;
        audioSource = GetComponent<AudioSource>();
        gameController = FindObjectOfType<GameController>();
        canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire") && Time.time - shotLast > shotDelay && canShoot)
        {
            if (weaponType == WeaponType.physical)
            {
                GameObject shellInstance = Instantiate(shotPref, shotOrigin.transform.position, shotOrigin.transform.rotation);
                shellInstance.GetComponent<Rigidbody>().velocity = shotOrigin.transform.forward * shotVelocity;
                shellInstance.GetComponent<Shot>().damage = shotDamage;
                shellInstance.GetComponent<Shot>().lifeTime = shotLifetime;
            }
            else
            {
                Ray ray = new Ray(shotOrigin.transform.position, shotOrigin.transform.forward);
                Debug.DrawRay(ray.origin, ray.direction, Color.yellow, Mathf.Infinity);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.tag == "Enemy")
                    {
                        hit.collider.GetComponent<Monster>().SetDamage(shotDamage);
                    }
                }
            }

            foreach (ParticleSystem shotParticle in shotParticles)
                shotParticle.Play();

            if (shotSound)
                audioSource.PlayOneShot(shotSound, 0.75f);

            shotLast = Time.time;
        }
    }
}
