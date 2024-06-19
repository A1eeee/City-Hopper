using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransiton : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SceneManager.LoadSceneAsync(3);
        }
    }
}
