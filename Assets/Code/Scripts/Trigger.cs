using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneTransiton : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Hi");

        if (collision.gameObject.tag == "Player")
        {
            SceneManager.LoadSceneAsync(3);
        }
    }
}
