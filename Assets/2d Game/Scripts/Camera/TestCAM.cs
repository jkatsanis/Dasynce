using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TestCAM : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _cam;

    [SerializeField] private int[] _xPositionWhereThePlayerShouldStop;

    [SerializeField] private Transform _playerTransform;

    private void Start()
    {
        _cam = GetComponent<CinemachineVirtualCamera>();  
    }
        
    private void Update()
    {
        if (IsTransFormSmaller())
        {
            _cam.enabled = false;
        }
        if (IsTransFormBigger())
        {
            _cam.enabled = true;

            _cam.transform.position = _playerTransform.position;
        }
    }

    private bool IsTransFormBigger()
    {
        for (int i = 0; i < _xPositionWhereThePlayerShouldStop.Length; i++)
        {
            if (_playerTransform.transform.position.x > _xPositionWhereThePlayerShouldStop[i])
            {
                return true;
            }
        }
        return false;
    }

    private bool IsTransFormSmaller()
    {
        for (int i = 0; i < _xPositionWhereThePlayerShouldStop.Length; i++)
        {
            if (_playerTransform.transform.position.x < _xPositionWhereThePlayerShouldStop[i])
            {
                return true;
            }
        }
        return false;   
    }
}
