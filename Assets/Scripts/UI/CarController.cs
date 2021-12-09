using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    [SerializeField] private float _speed = 15f; // ��������� �������� ������
    [SerializeField] private bool _rightTurn;
    [SerializeField] private bool _leftTurn;
    private float _rotateMultRight = 6f, _rotateMultLeft = 4.5f; //�������� �������� ����������

    private Rigidbody carRb;
    private float _originRotationY;


    private void Start()
    {
        _originRotationY = transform.eulerAngles.y;
        carRb = GetComponent<Rigidbody>();
    }


    private void FixedUpdate() // FixedUpdate �������� ��� �������������� � ������� ��� Update
    {
        // ������� ������ ������ �� ����������� y
        carRb.MovePosition(transform.position - transform.forward * _speed * Time.fixedDeltaTime);
    }

    private void OnTriggerStay(Collider other)
    {
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
    private void OnTriggerExit(Collider other)
    {
        // ��� ������ �� ��������� � ������ ����� � ������� ��������� ������������� ������ �������� �������� 90 ��������
        if (other.transform.CompareTag("TurnBlock Right") && _rightTurn)
        {
            carRb.rotation = Quaternion.Euler(0, _originRotationY + 90f, 0);
        }
        else if (other.transform.CompareTag("TurnBlock Left") && _leftTurn)
        {
            carRb.rotation = Quaternion.Euler(0, _originRotationY - 90f, 0);
        }
    }


    // ����� ��� �������� �����
    private void RotateCar(float speedRotate, int dir = 1)
    {
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
}
