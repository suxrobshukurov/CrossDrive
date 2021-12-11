using System;
using System.Collections;
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
    [SerializeField] private GameObject _canvasLosePanel, _horn, _adsManager;
    [SerializeField] private AudioSource _turnSignal;

    private int _countCars;
    private Coroutine _bottomCars, _leftCars, _rightCars, _upCars;
    private bool _isLoseOnce;

    [NonSerialized] public static int countLoses; // ���������� ��������� ������ 
    [NonSerialized] private static bool _isAdd;




    private void Start()
    {
        if (!_isAdd && PlayerPrefs.GetString("NoAds") != "yes") // ��������� ������� ������ ���� ��� �� �����
        {
            Instantiate(_adsManager, Vector3.zero, Quaternion.identity);
            _isAdd = true;
        }

        // ����� ����� ����������� � ����
        if (PlayerPrefs.GetInt("NowMap") == 2)
        {
            Destroy(maps[0]); // ���������� �������� ����� 
            maps[1].SetActive(true); // ���������� ������ �����
            Destroy(maps[2]);
        }
        else if (PlayerPrefs.GetInt("NowMap") == 3)
        {
            Destroy(maps[0]);
            Destroy(maps[1]);
            maps[2].SetActive(true);
        }
        else
        {
            maps[0].SetActive(true);
            Destroy(maps[1]);
            Destroy(maps[2]);
        }

        CarController.isLose = false; // �������� canvas ���������
        CarController._countCars = 0; // �������� ������� ����

        if (_isMainScene)
        {
            _timetToSpawnFrom = 4f;
            _timeToSpawnTo = 6f;
        }
        _bottomCars = StartCoroutine(BottomCars());
        _leftCars = StartCoroutine(LeftCars());
        _rightCars = StartCoroutine(RightCars());
        _upCars = StartCoroutine(UpCars());

        StartCoroutine(CreateHorn());
        
    }
    private void Update()
    {
        if (CarController.isLose && !_isLoseOnce)    // ��� ��������� ������
        {
            countLoses++; // ������������ �������� ������
            StopCoroutine(_bottomCars);
            StopCoroutine(_leftCars);
            StopCoroutine(_rightCars);
            StopCoroutine(_upCars);
            // ����� �������� ����������
            nowScore.text = $"<color='#f65757'>Score:</color> {CarController._countCars}";
            // ������ ������� ����������
            if(PlayerPrefs.GetInt("Score") < CarController._countCars)
            {
                PlayerPrefs.SetInt("Score", CarController._countCars);
            }
            // ����� ������� ����������
            topScore.text = "<color='#f65757'>Top:</color>" + PlayerPrefs.GetInt("Score");

            // ����� ����� ������� Coins
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
    IEnumerator CreateHorn()
    {   
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(4, 9));
            if (PlayerPrefs.GetString("music") != "No")
            {
                Instantiate(_horn, Vector3.zero, Quaternion.identity);
            }
        }
    }

    //  ����� ��� ���������� �����
    private void StopSound()
    {
        _turnSignal.Stop();
    }

    private void SpawnCar(Vector3 pos, float rotationY)
    {
        GameObject newCar = Instantiate(_carsPrefab[Random.Range(0, _carsPrefab.Length)], pos, Quaternion.Euler(0, rotationY, 0)) as GameObject;
        _countCars++;
        newCar.name = "Car - " + _countCars;
        // ����� ���������� ����������� ��������
        int random = _isMainScene ? 1 : Random.Range(1, 6);
        if (_isMainScene)
        {
            newCar.GetComponent<CarController>()._speed = 10f;
        }
        switch (random)
        {
            case 1:
            case 2:
                newCar.GetComponent<CarController>()._rightTurn = true;  // Move right
                if (PlayerPrefs.GetString("music") != "No" && !_turnSignal.isPlaying)
                {
                    _turnSignal.Play(); // Turn signal
                    Invoke("StopSound", 3.5f); // turn of
                }
                break;
            case 3:
            case 4:
                newCar.GetComponent<CarController>()._leftTurn = true; // Move left
                if (PlayerPrefs.GetString("music") != "No" && !_turnSignal.isPlaying)
                {
                    _turnSignal.Play(); // Turn signal
                    Invoke("StopSound", 3.5f); // turn of
                }

                break;
            default:
                // Move forward
                break;
        }
       
    }
}
