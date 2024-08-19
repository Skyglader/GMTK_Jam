using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{
    public AIManager aiManager;
    public float healthThreshold = 0.375f;
    public float defaultDmg = 0.375f;

    [Header("Damage Effect")]
    Color originalColor;
    private void Awake()
    {
        aiManager = GetComponent<AIManager>();

        
    }

    private void Start()
    {
        originalColor = aiManager.sprite.color;
    }
    public void TakeScaleDamage(float damage = -1f)
    {
        if (damage == -1f)
        {
            damage = defaultDmg;
        }

        StartCoroutine(DamageFlash());
        float currentScale = transform.localScale.x;

        if (currentScale - damage < healthThreshold)
        {
            aiManager.animator.CrossFade("Death", 0.1f);
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x - damage, transform.localScale.y - damage, 1);
        }
    }

    public IEnumerator DamageFlash()
    {
        aiManager.sprite.color = new Color(1f, 0f, 0f, originalColor.a);
        yield return new WaitForSeconds(0.1f);
        aiManager.sprite.color = originalColor;

    }
}
