using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private GameObject _turnLeftSignal, _turnRightSignal, _explosion, _exhaust;
    [SerializeField] private AudioClip[] _accelerates;
    [SerializeField] private AudioClip _crash;

    private Rigidbody carRb;
    private Camera _mainCam;
    private float _originRotationY;
    private float _rotateMultRight = 6f, _rotateMultLeft = 4.5f; //скорость вращения автомобиля
    private bool _isMovingFast, _carCrash;

    [NonSerialized] public static int _countCars;
    [NonSerialized] public bool _carPassed;
    [SerializeField] public bool _rightTurn, _leftTurn;
    [NonSerialized] public static bool isLose;

    public float _speed = 15f, _carForce = 50f; // указываем скорость машины и силу удара
    

    private void Start()
    {
        _originRotationY = transform.eulerAngles.y;
        carRb = GetComponent<Rigidbody>();
        _mainCam = Camera.main;

        if (_rightTurn)
            StartCoroutine(TurnSignals(_turnRightSignal));
        else if (_leftTurn)
            StartCoroutine(TurnSignals(_turnLeftSignal));
    }

    private void Update()
    {
#if UNITY_EDITOR
        Ray ray = _mainCam.ScreenPointToRay(Input.mousePosition);
#else
        if (Input.touchCount == 0)
            return;

        Ray ray = _mainCam.ScreenPointToRay(Input.GetTouch(0).position);
#endif

        RaycastHit hit;
        
        //увеличиваем скорость машины на которую нажали мишкой
        if (Physics.Raycast(ray, out hit, 100f, _layerMask))
        {
            string carName = hit.transform.gameObject.name; // получаем имя объекта на который навели мышкой
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0) && !_isMovingFast && gameObject.name == carName) // проверям этот объект
            {
#else
            if (Input.GetTouch(0).phase == TouchPhase.Began && !_isMovingFast && gameObject.name == carName) // проверям этот объект
            {
#endif
                GameObject vfx = Instantiate(_exhaust, new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), Quaternion.Euler(90, 0, 0));
                Destroy(vfx, 2f);
                _speed *= 2f; // увеличиваем скорость
                _isMovingFast = true; //меняем переключятель 

                if (PlayerPrefs.GetString("music") != "No")
                {
                    //пригрываем рандомный звук из массива
                    GetComponent<AudioSource>().clip = _accelerates[Random.Range(0, _accelerates.Length)];
                    GetComponent<AudioSource>().Play();
                }
            }
        }
    }

    private void FixedUpdate() // FixedUpdate подходит для взаимодействия с физикой чем Update
    {
        // двигаем машины вперед по напралениию y
        carRb.MovePosition(transform.position - transform.forward * _speed * Time.fixedDeltaTime);
    }

    // обртаботка сталькновения 
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Car") && !_carCrash)
        {
            _carCrash = true;
            isLose = true;
            _speed = 0f; // останавливаем первый автомобиль
            collision.gameObject.GetComponent<CarController>()._speed = 0f; // останавливаем второй автомобиль
            GameObject vfx = Instantiate(_explosion, transform.position, Quaternion.identity);
            Destroy(vfx, 5f);
            if (_isMovingFast)
                _carForce *= 1.2f;

            // добовляем силу относительно кооринаты машины 
            carRb.AddRelativeForce(Vector3.back * _carForce);

            //пригрываем звук столькновения
            if (PlayerPrefs.GetString("music") != "No")
            {
                GetComponent<AudioSource>().clip = _crash;
                GetComponent<AudioSource>().Play();
            }

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_carCrash)
            return;
        // при входе в колайдер с нужным тегом и булевым значением совершаем поворот
        if (other.transform.CompareTag("TurnBlock Right") && _rightTurn) 
        {
            RotateCar(_rotateMultRight);
        }
        else if (other.transform.CompareTag("TurnBlock Left") && _leftTurn)
        {
            RotateCar(_rotateMultLeft, -1);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // если у объекта который столькнулся тег Car и у него есть так Trigger pass
        if (other.gameObject.CompareTag("Car") && other.GetComponent<CarController>()._carPassed)
            other.GetComponent<CarController>()._speed = _speed + 5f; // устанавливаем одинаковую скорость 
    }

    private void OnTriggerExit(Collider other)
    {
        if (_carCrash)
            return;
        if (other.transform.CompareTag("Trigger Pass"))
        {
            if (_carPassed)
                return;
            _carPassed = true;
            Collider[] colliders = GetComponents<BoxCollider>();
            foreach (Collider collider in colliders)
                collider.enabled = true;
            _countCars++; // добавление машины в счет
        }
        // при выходе из колайдера с нужным тегом и булевым значением устанавливаем точное значение поворота 90 градусов
        if (other.transform.CompareTag("TurnBlock Right") && _rightTurn)
        {
            carRb.rotation = Quaternion.Euler(0, _originRotationY + 90f, 0);
        }
        else if (other.transform.CompareTag("TurnBlock Left") && _leftTurn)
        {
            carRb.rotation = Quaternion.Euler(0, _originRotationY - 90f, 0);
        }
        else if (other.transform.CompareTag("Delete Car")) // удаление машины при выходе из нужного тригера
        {
            Destroy(gameObject);
        }

        
      
    }


    // метод для поворота машин
    private void RotateCar(float speedRotate, int dir = 1)
    {
        if (_carCrash)
            return;
        //если текущее вращение машины меньше чем (первоначальное - 90 градусов) то мы выходим из функции
        if (dir == -1 && transform.localRotation.eulerAngles.y < _originRotationY - 90f ) 
        {
            return;
        }
        //if (dir == -1 && transform.localRotation.eulerAngles.y > 250f && transform.localRotation.eulerAngles.y < 270f)
        //{
        //    return;
        //}

            //совершаем поворот машин
            float rotateSoeed = _speed * speedRotate * dir; // при повороте что было фиксированя скорость 
        Quaternion deltaRotation = Quaternion.Euler(new Vector3(0, rotateSoeed, 0) * Time.fixedDeltaTime);
        carRb.MoveRotation(carRb.rotation * deltaRotation);
    }

    // метод отображения поворота имитация поворотчиков
    IEnumerator TurnSignals(GameObject turnSignal)
    {
        while (!_carPassed)
        {
            turnSignal.SetActive(!turnSignal.activeSelf);
            yield return new WaitForSeconds(0.3f);

        }
    }
}
