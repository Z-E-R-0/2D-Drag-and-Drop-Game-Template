using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playsound : MonoBehaviour
{
    public AudioSource audioSource;
   
    // Start is called before the first frame update
   public void PlayAudio()
    {
        //popsound.Play();
        audioSource.Play();


    }
}
