using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
    public int Lap;
    public int MaxLap;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Lap++;
            if (Lap >= MaxLap)
            {
                Time.timeScale = 0.0f;
                Debug.Log("완주");
            }
        }
    }
}
