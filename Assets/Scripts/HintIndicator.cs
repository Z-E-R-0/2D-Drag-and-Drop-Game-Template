using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
using System;

public class HintIndicator : MonoBehaviour
{
    // Effect to show when a target is hit
    public float effectDuration = 1f; // Duration for the hit effect
    public GameObject inGameUI; // In-game UI object
    public float noClickDuration = 5f; // Editable duration for no-click condition
    public float shakeDuration = 0.5f; // Duration of the shake effect
    public float shakeStrength = 1f; // Strength of the shake effect
    public int shakeVibrato = 10; // Vibrato of the shake effect
    public float shakeRandomness = 90f; // Randomness of the shake effect
    public wincondition wc;
    private GameObject[] targets; // Array to hold targets with the specified tag
    private bool[] targetHitStatus; // Array to track hit status of each target
    private float lastInteractionTime; // Time of the last interaction (hit or click)
    public string tagValuToFind; // Value of the tag to find
    public DragAndDrop dad;

    private void Awake()
    {
        // Initialize last interaction time
        lastInteractionTime = Time.time;
    }

    private void Update()
    {
        // Check if no interactions have occurred for the specified duration
        if (Time.time - lastInteractionTime >= noClickDuration)
        {
            TriggerAutoHitCondition();
            // Reset the timer
            lastInteractionTime = Time.time;
        }
    }

    public void OnUserInteraction()
    {
        // Call this method whenever there is a user interaction
        lastInteractionTime = Time.time;
    }

    private void TriggerAutoHitCondition()
    {
        wc = FindObjectOfType<wincondition>();
        tagValuToFind = wc.childObjects[0].tag;
        dad = DragAndDrop.FindDragAndDropByValue(tagValuToFind);

        // Check if the DragAndDrop object is found
        if (dad != null)
        {
            dad.transform.DOShakePosition(shakeDuration, shakeStrength, shakeVibrato, shakeRandomness);
            
            // Add any additional effects or actions you want to trigger
        }
        else
        {
            Debug.Log("No DragAndDrop object found with the specified value.");
        }
    }

    public void shakeobject()
    {
        wc = FindObjectOfType<wincondition>();
        tagValuToFind = wc.childObjects[0].tag;
        dad = DragAndDrop.FindDragAndDropByValue(tagValuToFind);

        // Check if the DragAndDrop object is found
        if (dad != null)
        {
            dad.transform.DOShakePosition(shakeDuration, shakeStrength, shakeVibrato, shakeRandomness);
        }
        else
        {
            Debug.Log("No DragAndDrop object found with the specified value.");
        }
    }
}