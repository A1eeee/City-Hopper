using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    public float elepsedTime;

    // Update is called once per frame
    void Update()
    {
        elepsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elepsedTime / 60);
        int seconds = Mathf.FloorToInt(elepsedTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
