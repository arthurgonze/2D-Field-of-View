using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{

    [SerializeField] private int _sceneIndex;

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(_sceneIndex >= 0)
            SceneManager.LoadScene(_sceneIndex);
        else
            other.gameObject.transform.position = new Vector3(0,0,0);
    }
}
