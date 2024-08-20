using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionStart : MonoBehaviour
{
    // Start is called before the first frame update
    AudioSource src;
    public bool hardMode = false;
    private void Start()
    {
        // Ensure that the AudioSource is assigned. You can also assign it via the inspector.
        if (src == null)
        {
            src = GameObject.Find("Menu").GetComponent<AudioSource>();
        }
    }

    public void LowerMusic()
    {
        StartCoroutine(LowerVolumeCoroutine());
    }

    private IEnumerator LowerVolumeCoroutine()
    {
        while (src.volume > 0)
        {
            src.volume = Mathf.Lerp(src.volume, 0, 4 * Time.deltaTime);
            yield return null; // Wait until the next frame
        }
        src.volume = 0; // Ensure the volume is set to 0 at the end
    }
    private void StartGame()
    {
        if (!hardMode)
        {
            SceneManager.LoadScene("Game");
        }
        else
        {
            SceneManager.LoadScene("Hard Game");
        }

    }

    public void setHardMode(bool value)
    {
        hardMode = value;
    }
}
