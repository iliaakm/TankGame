using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tank : MonoBehaviour
{
    public float movementSpeed;     //величина скорости передвижения
    public float turnSpeed;     //величина скорости поворота
    [Range(0, 1)]
    public float protection;        //величина защиты
    public Slider healthSider;      //полоска здоровья
    public int healthMax;       //величина здоровья
    public Transform weapons;       //оружия
    public AudioClip staySound;     //звук двигателя на месте
    public AudioClip moveSound;     //звук двигателя в движении
    public ParticleSystem deadParticle;     //партикл смерти
    public bool canMove;        //можно ли двигаться

    new Rigidbody rigidbody;        //rigbody танка
    float turnValue;        //величина поворота
    int health = 100;       //величина здоровья
    Vector3 movementValue;      //величина движения
    GameController gameController;      //контроллер
    AudioSource audioSource;        //источник звука

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        gameController = FindObjectOfType<GameController>();
        audioSource = GetComponent<AudioSource>();
        healthSider.maxValue = healthMax;
        HealthBar(health);
        canMove = true;
    }

    void Update()
    {
        #region controls
        if (canMove)
        {
            movementValue = transform.forward * Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.LeftShift))
                movementValue *= 1.5f;
            rigidbody.MovePosition(rigidbody.position + movementValue);
            SetEngineSound(Input.GetAxis("Vertical"));

            turnValue = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;
            rigidbody.MoveRotation(rigidbody.rotation * Quaternion.Euler(0f, turnValue, 0f));
        }

        if (Input.GetButtonDown("WeaponsForward"))
            ChangeWeapons(1);
        if (Input.GetButtonDown("WeaponsBack"))
            ChangeWeapons(-1);

        #endregion
    }

    public void SetDamage(int hp)  //получить урон
    {
        health -= (int)(hp * protection);
        HealthBar(health);

        if (health <= 0)
        {
            gameController.TakeControl();
            gameController.GameOver();
            ParticleSystem deadParticleInstance = Instantiate(deadParticle, transform.position, transform.rotation);
            deadParticleInstance.Play();
        }
    }

    public void SetHeal(int hp)     //пополнить здоровье
    {
        health += hp;
        if (health > healthMax)
            health = healthMax;
        HealthBar(health);
    }

    void ChangeWeapons(int offset)      //сменить урон
    {
        int currentWeaponsIndex = -1;
        foreach (Transform child in weapons)
            if (child.gameObject.activeSelf)
            {
                currentWeaponsIndex = child.GetSiblingIndex();
                currentWeaponsIndex += offset;
                if (currentWeaponsIndex > weapons.childCount - 1)
                    currentWeaponsIndex = 0;
                if (currentWeaponsIndex < 0)
                    currentWeaponsIndex = weapons.childCount - 1;

                child.gameObject.SetActive(false);
                weapons.GetChild(currentWeaponsIndex).gameObject.SetActive(true);
                break;
            }
    }

    void HealthBar(int hp)      //обновить полоску здоровья
    {
        healthSider.value = hp;

        if (healthSider.value >= 75)
            healthSider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.green;

        if (healthSider.value < 75)
            healthSider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.yellow;

        if (healthSider.value < 25)
            healthSider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.red;
    }

    void SetEngineSound(float axisValue)        //обновить звук двигателя
    {
        if (axisValue == 0)
        {
            if (audioSource.clip != staySound)
            {
                audioSource.clip = staySound;
                audioSource.Play();
            }
        }
        else
            if (audioSource.clip != moveSound)
            {
                audioSource.clip = moveSound;
                audioSource.Play();
            }
    }
}
