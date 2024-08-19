using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpParticles : MonoBehaviour
{
    public PlayerManager player;
    public GameObject jumpUpParticle;
    public GameObject fallDownParticle;
    public GameObject legs;
    public GameObject childCombatManager;
    public bool activated = false;
    public bool inAir = false;
    private void Awake()
    {
        player = GetComponentInParent<PlayerManager>();
    }

    private void Update()
    {
        if (player.isGrounded == true && player.playerInputManager.isJumping == true && !activated)
        {
            if (childCombatManager.transform.localScale.x < 0)
            {
                Instantiate(jumpUpParticle, legs.transform.position, Quaternion.Euler(jumpUpParticle.transform.rotation.x, 180f, jumpUpParticle.transform.rotation.z));
            }
            else
            {
                Instantiate(jumpUpParticle, legs.transform.position, Quaternion.identity);
            }
            activated = true;
            StartCoroutine(detectInAir());
        }
        else if (player.isGrounded == false && activated)
        {
            activated = false;
        }

        if (inAir && player.isGrounded)
        {

            if (childCombatManager.transform.localScale.x < 0)
            {
                Instantiate(fallDownParticle, legs.transform.position, Quaternion.Euler(jumpUpParticle.transform.rotation.x, 180f, jumpUpParticle.transform.rotation.z));
            }
            else
            {
                Instantiate(fallDownParticle, legs.transform.position, Quaternion.identity);
            }
            inAir = false;
        }
                
    }

    public IEnumerator detectInAir()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        inAir = true;
    }
}
