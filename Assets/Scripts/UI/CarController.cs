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
    private float _rotateMultRight = 6f, _rotateMultLeft = 4.5f; //�������� �������� ����������
    private bool _isMovingFast, _carCrash;

    [NonSerialized] public static int _countCars;
    [NonSerialized] public bool _carPassed;
    [SerializeField] public bool _rightTurn, _leftTurn;
    [NonSerialized] public static bool isLose;

    public float _speed = 15f, _carForce = 50f; // ��������� �������� ������ � ���� �����
    

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
        
        //����������� �������� ������ �� ������� ������ ������
        if (Physics.Raycast(ray, out hit, 100f, _layerMask))
        {
            string carName = hit.transform.gameObject.name; // �������� ��� ������� �� ������� ������ ������
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0) && !_isMovingFast && gameObject.name == carName) // �������� ���� ������
            {
#else
            if (Input.GetTouch(0).phase == TouchPhase.Began && !_isMovingFast && gameObject.name == carName) // �������� ���� ������
            {
#endif
                GameObject vfx = Instantiate(_exhaust, new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), Quaternion.Euler(90, 0, 0));
                Destroy(vfx, 2f);
                _speed *= 2f; // ����������� ��������
                _isMovingFast = true; //������ ������������� 

                if (PlayerPrefs.GetString("music") != "No")
                {
                    //���������� ��������� ���� �� �������
                    GetComponent<AudioSource>().clip = _accelerates[Random.Range(0, _accelerates.Length)];
                    GetComponent<AudioSource>().Play();
                }
            }
        }
    }

    private void FixedUpdate() // FixedUpdate �������� ��� �������������� � ������� ��� Update
    {
        // ������� ������ ������ �� ����������� y
        carRb.MovePosition(transform.position - transform.forward * _speed * Time.fixedDeltaTime);
    }

    // ���������� ������������� 
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Car") && !_carCrash)
        {
            _carCrash = true;
            isLose = true;
            _speed = 0f; // ������������� ������ ����������
            collision.gameObject.GetComponent<CarController>()._speed = 0f; // ������������� ������ ����������
            GameObject vfx = Instantiate(_explosion, transform.position, Quaternion.identity);
            Destroy(vfx, 5f);
            if (_isMovingFast)
                _carForce *= 1.2f;

            // ��������� ���� ������������ ��������� ������ 
            carRb.AddRelativeForce(Vector3.back * _carForce);

            //���������� ���� �������������
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
        // ��� ����� � �������� � ������ ����� � ������� ��������� ��������� �������
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
        // ���� � ������� ������� ����������� ��� Car � � ���� ���� ��� Trigger pass
        if (other.gameObject.CompareTag("Car") && other.GetComponent<CarController>()._carPassed)
            other.GetComponent<CarController>()._speed = _speed + 5f; // ������������� ���������� �������� 
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
            _countCars++; // ���������� ������ � ����
        }
        // ��� ������ �� ��������� � ������ ����� � ������� ��������� ������������� ������ �������� �������� 90 ��������
        if (other.transform.CompareTag("TurnBlock Right") && _rightTurn)
        {
            carRb.rotation = Quaternion.Euler(0, _originRotationY + 90f, 0);
        }
        else if (other.transform.CompareTag("TurnBlock Left") && _leftTurn)
        {
            carRb.rotation = Quaternion.Euler(0, _originRotationY - 90f, 0);
        }
        else if (other.transform.CompareTag("Delete Car")) // �������� ������ ��� ������ �� ������� �������
        {
            Destroy(gameObject);
        }

        
      
    }


    // ����� ��� �������� �����
    private void RotateCar(float speedRotate, int dir = 1)
    {
        if (_carCrash)
            return;
        //���� ������� �������� ������ ������ ��� (�������������� - 90 ��������) �� �� ������� �� �������
        if (dir == -1 && transform.localRotation.eulerAngles.y < _originRotationY - 90f ) 
        {
            return;
        }
        //if (dir == -1 && transform.localRotation.eulerAngles.y > 250f && transform.localRotation.eulerAngles.y < 270f)
        //{
        //    return;
        //}

            //��������� ������� �����
            float rotateSoeed = _speed * speedRotate * dir; // ��� �������� ��� ���� ����������� �������� 
        Quaternion deltaRotation = Quaternion.Euler(new Vector3(0, rotateSoeed, 0) * Time.fixedDeltaTime);
        carRb.MoveRotation(carRb.rotation * deltaRotation);
    }

    // ����� ����������� �������� �������� ������������
    IEnumerator TurnSignals(GameObject turnSignal)
    {
        while (!_carPassed)
        {
            turnSignal.SetActive(!turnSignal.activeSelf);
            yield return new WaitForSeconds(0.3f);

        }
    }
}
