using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class GameController : MonoBehaviour
{
    [SerializeField] public Text nowScore, topScore, coinsCount;
    [SerializeField] public GameObject[] maps;
    [SerializeField] private float _timetToSpawnFrom = 2f, _timeToSpawnTo = 4.5f;
    [SerializeField] private GameObject[] _carsPrefab;
    [SerializeField] private bool _isMainScene;
    [SerializeField] private GameObject _canvasLosePanel;

    private int _countCars;
    private Coroutine _bottomCars, _leftCars, _rightCars, _upCars;
    private bool _isLoseOnce;

    private void Start()
    {
        // выбор карты отображение в игре
        if (PlayerPrefs.GetInt("NowMap") == 2)
        {
            Destroy(maps[0]); // уничтожаем ненужыне карты 
            maps[1].SetActive(true); // отображаем нужную карту
            Destroy(maps[2]);
        }
        else if (PlayerPrefs.GetInt("NowMap") == 3)
        {
            Destroy(maps[0]);
            Destroy(maps[1]);
            maps[3].SetActive(true);
        }
        else
        {
            maps[0].SetActive(true);
            Destroy(maps[1]);
            Destroy(maps[2]);
        }

        CarController.isLose = false; // скрываем canvas проигрыша
        CarController._countCars = 0; // обнуляем текуший счёт

        if (_isMainScene)
        {
            _timetToSpawnFrom = 4f;
            _timeToSpawnTo = 6f;
        }
        _bottomCars = StartCoroutine(BottomCars());
        _leftCars = StartCoroutine(LeftCars());
        _rightCars = StartCoroutine(RightCars());
        _upCars = StartCoroutine(UpCars());
        
    }
    private void Update()
    {
        if (CarController.isLose && !_isLoseOnce)    // при проигрыше игрока
        {
            StopCoroutine(_bottomCars);
            StopCoroutine(_leftCars);
            StopCoroutine(_rightCars);
            StopCoroutine(_upCars);
            // Вывод текушего результата
            nowScore.text = $"<color='#f65757'>Score:</color> {CarController._countCars}";
            // Запись лучшего результата
            if(PlayerPrefs.GetInt("Score") < CarController._countCars)
            {
                PlayerPrefs.SetInt("Score", CarController._countCars);
            }
            // Вывод лучшего результата
            topScore.text = "<color='#f65757'>Top:</color>" + PlayerPrefs.GetInt("Score");

            // Вывод счета монеток Coins
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + CarController._countCars);
            coinsCount.text = PlayerPrefs.GetInt("Coins").ToString();


            _canvasLosePanel.SetActive(true);
            _isLoseOnce = true;
        }

    }

    IEnumerator BottomCars()
    {
        while (true)
        {
            float _timetToSpawn = Random.Range(_timetToSpawnFrom, _timeToSpawnTo);
            SpawnCar(new Vector3(-1.3f, 0f, -26.13f), 180f);
            yield return new WaitForSeconds(_timetToSpawn);
        }
    }
    IEnumerator LeftCars()
    {
        while (true)
        {
            SpawnCar(new Vector3(-85.8f, 0f, 3.2f), 270f);
            float _timetToSpawn = Random.Range(_timetToSpawnFrom, _timeToSpawnTo);
            yield return new WaitForSeconds(_timetToSpawn);
        }
    }
    IEnumerator RightCars()
    {
        while (true)
        {
            SpawnCar(new Vector3(25.28f, 0f, 10.5f), 90f);
            float _timetToSpawn = Random.Range(_timetToSpawnFrom, _timeToSpawnTo);
            yield return new WaitForSeconds(_timetToSpawn);
        }
    }
    IEnumerator UpCars()
    {
        while (true)
        {
            SpawnCar(new Vector3(-7.7f, 0f, 69.4f), 0f);
            float _timetToSpawn = Random.Range(_timetToSpawnFrom, _timeToSpawnTo);
            yield return new WaitForSeconds(_timetToSpawn);
        }
    }

    private void SpawnCar(Vector3 pos, float rotationY)
    {
        GameObject newCar = Instantiate(_carsPrefab[Random.Range(0, _carsPrefab.Length)], pos, Quaternion.Euler(0, rotationY, 0)) as GameObject;
        _countCars++;
        newCar.name = "Car - " + _countCars;
        // выбор рандомного направления поворота
        int random = _isMainScene ? 1 : Random.Range(1, 6);
        if (_isMainScene)
        {
            newCar.GetComponent<CarController>()._speed = 10f;
        }
        switch (random)
        {
            case 1:
            case 2:
                // Move right
                newCar.GetComponent<CarController>()._rightTurn = true;
                break;
            case 3:
            case 4:
                newCar.GetComponent<CarController>()._leftTurn = true;
                // Move left
                break;
            default:
                // Move forward
                break;
        }
       
    }


}
