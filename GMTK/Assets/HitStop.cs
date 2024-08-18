using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    public static HitStop instance;
    public bool waiting;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void Stop(float duration)
    {
        if (waiting)
        {
            return;
        }
        Time.timeScale = 0.0f;
        StartCoroutine(Wait(duration));
    }

    public IEnumerator Wait(float duration)
    {
        waiting = true;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1.0f;
        waiting = false;
    }
}
