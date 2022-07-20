using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnsolidObject : MonoBehaviour
{
    [SerializeField] private LayerMask _unsolidObjcets;

    private PlayerControllerLevelSelector _movement;

    private void Start()
    {
        _movement = GetComponent<PlayerControllerLevelSelector>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IsShield(collision);
    }

    private void IsShield(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("LevelSelectorShield"))
        {
            StaticSceneManager.LoadScene(StaticSceneManager.levelOneScene, _movement.inv);
        }
    }
}
