using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackColliderScript : MonoBehaviour
{
    [SerializeField] private GameObject _hydra;
    public bool hydraIsLeft { get; private set; }

    void Update()
    {
        if (_hydra.transform.localScale == new Vector3(4, 4, 0))
        {
            hydraIsLeft = true;
        }
        else
        {
            hydraIsLeft = false;
        }
    }
}
