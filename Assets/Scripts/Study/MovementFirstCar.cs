using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementFirstCar : MonoBehaviour
{
    [SerializeField] private GameObject _canvasFirst, secondCar, _canvasSecond;

    private bool _isFirst;
    private CarController _controller;

    private void Start()
    {
        _controller = GetComponent<CarController>();
    }
    private void Update()
    {
        if (transform.position.x < 8f && !_isFirst)
        {
            _isFirst = true;
            GetComponent<CarController>()._speed = 0;
            _canvasFirst.SetActive(true);
        }
    }

    private void OnMouseDown()
    {
        if (!_isFirst || transform.position.x > 9f) return;

        _controller._speed = 15f;
        _canvasFirst.SetActive(false);
        _canvasSecond.SetActive(true);
        secondCar.GetComponent<CarController>()._speed = 15f;
    }
}
