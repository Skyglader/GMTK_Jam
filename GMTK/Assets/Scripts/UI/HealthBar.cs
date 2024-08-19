using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public GameObject player;
    public PlayerManager playerManager;
    public float health;
    public float maxHealth;
    public float pastValue;
    public Image image;
    public Color originalColor;
    void Start()
    {
        player = WorldGameObjectStorage.instance.player;
        playerManager = player.GetComponent<PlayerManager>();
        slider.maxValue = playerManager.maxScale;
        slider.minValue = playerManager.deathThreshold;
        image = slider.fillRect.GetComponent<Image>();
        originalColor = image.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerManager.isDead)
            slider.value = 0;
        else
            slider.value = player.transform.localScale.x;

        if (slider.value < pastValue)
        {
            StartCoroutine(FlashRed());
        }

        pastValue = slider.value;   
    }

    public IEnumerator FlashRed()
    {
        Color flashColor = Color.red;
        float flashDuration = 0.1f; // Duration of the flash

        image.color = flashColor;
        yield return new WaitForSecondsRealtime(flashDuration);

        // Ensure the color returns to the original color
        image.color = originalColor;
    }
}
