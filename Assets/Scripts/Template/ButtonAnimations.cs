using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnimations : MonoBehaviour
{
    private Animator animator;
    public bool Ispressed;
    private GameController gC;

    // Start is called before the first frame update
    private void Awake()
    {
        gC = FindObjectOfType<GameController>();
    }

    public void PlayButtonPressedAnimation()
    {
        Ispressed = true;
        animator = transform.gameObject.GetComponent<Animator>();
        animator.SetBool("Ispressed", true);

        StartCoroutine(ResetButtonPress());
    }

    private IEnumerator ResetButtonPress()
    {
        yield return new WaitForSeconds(0.25f);
        Ispressed = false;
        animator.SetBool("Ispressed", false);
    }
}
