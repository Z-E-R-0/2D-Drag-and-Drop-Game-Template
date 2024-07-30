using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPopupManager : MonoBehaviour
{
    public GameObject[] stars;
    public AudioSource audioSource;
    public AudioSource audioSourceWin;
    public GameObject winParticle;
    public Vector3 offset;
    // Start is called before the first frame update
    void Awake()
    {
       // ShowStars();
    }

    // Update is called once per frame
    void Update()
    {
        //ShowStars();
    }



    public void ShowStars()
    {
        audioSourceWin.Play();
        for (int i = 0; i < stars.Length; i++)
        {
            // Capture the loop variable in a local variable
            int index = i;

            // Set initial scale to zero
            stars[index].transform.localScale = Vector3.zero;

            // Delay for the particle and sound
            LeanTween.delayedCall(index * 0.5f, () =>
            {
                audioSource.Play();
                //Instantiate(winParticle, stars[index].transform.position + offset, Quaternion.identity);
            });
            LeanTween.delayedCall(index * 0.5f, () =>
            {
               // audioSource.Play();
                Instantiate(winParticle, stars[index].transform.position + offset, Quaternion.identity);
            });

            // Animate scale with LeanTween
            LeanTween.scale(stars[index], Vector3.one, 0.5f)
                .setEaseInBounce()
                .setDelay(index * 0.5f);
        }
    }




}
