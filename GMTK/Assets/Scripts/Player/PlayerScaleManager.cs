using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScaleManager : MonoBehaviour
{

    public bool toggleDamage = false;
    private void Update()
    {
        if (toggleDamage)
        {
            toggleDamage = false;
            TakeScaleDamage();
        }
    }
    public void TakeScaleDamage()
    {
        transform.localScale = new Vector3(transform.localScale.x -0.2f, transform.localScale.y -0.2f, transform.localScale.z);  
    }
}
