using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public int monstersAmountMax = 10;      //максимальное число монстров
    public Tank tankObject;     //танк
    public GameObject[] monsterPref;        //префабы монстров
    public Transform[] monsterSpawnPoints;      //позиции спавна монстров 
    public Transform[] cameraPositions;     //позиции камер
    public GameObject medicinePref;         //префаб аптечки
    public GameObject UIGameOver;       //интерфейс конца игры
    public GameObject UITextInfo;       //интерфейс тестового сообщения
    public GameObject UIPauseMenu;      //интерфейс паузы   
    public Text UIScore;        //интерфейс очков
    public Vector2 arenaSize;       //размер арены

    int currentCameraPosition;      //текущая позиция камеры
    int score;      //число очков
    Camera mainCamera;  //главная камера

    void Start()
    {
        mainCamera = Camera.main;
        SpawnMonsters(monstersAmountMax);
        DisableUIElements();
        SwitchCameraPosition(0);
        SpawnMedicine();
    }

    void Update()
    {
        if (Input.GetButtonDown("SwitchCamera"))
            SwitchCameraPosition(++currentCameraPosition);

        if (Input.GetKeyDown(KeyCode.Escape))
            SetPause(!UIPauseMenu.activeSelf);
    }

    public void SpawnMonsters(int amount)       //спавн монстров
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject monsterInstance = Instantiate(monsterPref[Random.Range(0, monsterPref.Length)]);
            monsterInstance.transform.position = monsterSpawnPoints[Random.Range(0, monsterSpawnPoints.Length)].position;
            monsterInstance.GetComponent<Monster>().targetTransform = tankObject.transform;
        }
    }

    public void GameOver()      //конец игры
    {
        if (UIGameOver.activeSelf)
            return;
        UIGameOver.SetActive(true);
        UIGameOver.transform.Find("YourScore").GetComponent<Text>().text += score;
        int bestScore = PlayerPrefs.GetInt("BestScore");
        if (score < bestScore)
            UIGameOver.transform.Find("BestScore").GetComponent<Text>().text += PlayerPrefs.GetInt("BestScore");
        else
        {
            UIGameOver.transform.Find("BestScore").GetComponent<Text>().text = "Новый рекорд!";
            PlayerPrefs.SetInt("BestScore", score);
        }
        SwitchCameraPosition(2);
        Destroy(tankObject.gameObject);
    }

    public void RestartGame()       //начать игру заново
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }    

    public void SwitchCameraPosition(int positionIndex)     //смена камеры по индексу
    {
        currentCameraPosition = positionIndex;
        if (currentCameraPosition > cameraPositions.Length - 1)
            currentCameraPosition = 0;
        mainCamera.transform.SetPositionAndRotation(cameraPositions[currentCameraPosition].position, cameraPositions[currentCameraPosition].rotation);
        mainCamera.transform.SetParent(cameraPositions[currentCameraPosition].parent);
        if (!mainCamera.gameObject.activeSelf)
            mainCamera.gameObject.SetActive(true);
    }

    public void SwitchCameraPosition(Transform transform)       //смена камеры по позиции
    {
        mainCamera.transform.SetPositionAndRotation(transform.position, transform.rotation);
        mainCamera.transform.SetParent(transform.parent);
        if (!mainCamera.gameObject.activeSelf)
            mainCamera.gameObject.SetActive(true);
    }

    public void ResetCameraPosition()       //возвращение камеры на место
    {
        mainCamera.transform.SetPositionAndRotation(cameraPositions[currentCameraPosition].position, cameraPositions[currentCameraPosition].rotation);
        mainCamera.transform.SetParent(cameraPositions[currentCameraPosition].parent);
        if (!mainCamera.gameObject.activeSelf)
            mainCamera.gameObject.SetActive(true);
    }

    public void SpawnMedicine()     //спавн аптечек
    {
        GameObject medicineInstance = Instantiate(medicinePref);
        medicineInstance.transform.position = new Vector3(Random.Range(-arenaSize.x / 2, arenaSize.x / 2), medicineInstance.transform.position.y, Random.Range(-arenaSize.y / 2, arenaSize.y / 2));
    }

    public void SetUITextInfo(string text, Color color = default(Color))        //вывод сообщения на экран c цветом
    {
        UITextInfo.GetComponent<Text>().text = text;
        UITextInfo.GetComponent<Text>().color = color;

        if (UITextInfo.activeSelf)
            UITextInfo.SetActive(false);
        UITextInfo.SetActive(true);
    }

    public void SetUITextInfo(string text)      //вывод сообщения на экран
    {
        UITextInfo.GetComponent<Text>().text = text;
        UITextInfo.GetComponent<Text>().color = Color.white;

        if (UITextInfo.activeSelf)
            UITextInfo.SetActive(false);
        UITextInfo.SetActive(true);
    }

    public void SetPause(bool isPause)       //поставить игру на паузу
    {
        UIPauseMenu.SetActive(isPause);
        if (isPause)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    public void AddScore(int value)     //прибавить очки
    {
        score += value;
        SetUITextInfo("+" + value, Color.blue);
        UIScore.text = score.ToString();
    }

    void DisableUIElements()        //отключить интерфейс
    {
        UIGameOver.SetActive(false);
        UITextInfo.SetActive(false);
        UIPauseMenu.SetActive(false);
    }

    public void TakeControl()       //забрать управление у игрока
    {
        tankObject.canMove = false;
        foreach (Transform weapon in tankObject.weapons.transform)
        {
            weapon.GetComponent<Weapon>().canShoot = false;
        }
    }
}
