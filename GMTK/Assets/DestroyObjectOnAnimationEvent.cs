using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjectOnAnimationEvent : MonoBehaviour
{
    private void DestroyBlood()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }
}
