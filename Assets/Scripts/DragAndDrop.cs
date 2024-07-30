using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private GridLayoutGroup gridLayoutGroup;
    private Vector3 originalPosition;
    private GameObject parentObject;
    public string value;
    public float shakeDuration = 0.5f;
    public float shakeStrength = 10;
    public int shakeVibrato = 10;
    public float shakeRandomness = 90f;
    public float delayBeforeShake = 0.25f;
    private Vector3 defaultPos;
    private bool isDropped = false;
    public wincondition winCondition;
    public GameController gameController;
    public List<GameObject> spawnedObjects = new List<GameObject>();
    public AudioClip[] Audios;
    private AudioSource audioSource;
    public string[] tryAgainMessages;
    public TMP_Text tryAgainText;
    // Static list to hold all objects with this script attached
    private static List<DragAndDrop> draggableObjects = new List<DragAndDrop>();
    public AudioClip[] wrongAudios;
    public AudioSource wrongAudiosSource;

    private void Awake()
    {
        
    }
    private void Start()
    {

        tryAgainText.gameObject.SetActive(false);
        parentObject = this.gameObject.transform.parent.gameObject;
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        gridLayoutGroup = GetComponentInParent<GridLayoutGroup>();
        defaultPos = rectTransform.localPosition;
        audioSource = GetComponent<AudioSource>();

        // Add this object to the list of draggable objects
        draggableObjects.Add(this);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        PlayAudioClip(0);
        originalPosition = rectTransform.localPosition;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        GameObject dropZone = eventData.pointerEnter?.gameObject;
        if (dropZone != null && dropZone.CompareTag(value) && !isDropped)
        {
            PlayAudioClip(1);
            InstantiateAtDefaultPosition();
            isDropped = true;
            rectTransform.SetParent(dropZone.transform);
            rectTransform.localPosition = Vector3.zero;

            winCondition = FindObjectOfType<wincondition>();
            winCondition.CheckWinCondition();

            gameController = FindObjectOfType<GameController>();
            gameController.CheckWinCondition();
        }
        else
        {
            if(!wrongAudiosSource.isPlaying)
            {
                PlayRandomAudio();
                ShowTryAgainMessage();


            }
           
            PlayAudioClip(2);
            Shake();
           
            
            
        }
    }

    private void InstantiateAtDefaultPosition()
    {
       
       
            GameObject newInstance = Instantiate(gameObject, parentObject.transform);
            newInstance.transform.localPosition = defaultPos;
            newInstance.transform.SetAsLastSibling();
            spawnedObjects.Add(newInstance);
        
    }

    public void ResetToOriginalPosition()
    {
        
        rectTransform.SetParent(parentObject.transform);
        rectTransform.localPosition = defaultPos;
        isDropped = false;

        
        
    }

    private void PlayAudioClip(int index)
    {
        if (Audios != null && index >= 0 && index < Audios.Length)
        {
            audioSource.clip = Audios[index];
            audioSource.Play();
        }
    }

      void PlayRandomAudio()
    {
        // Check if the audio clips array is not empty
        if (wrongAudios.Length > 0)
        {
            // Select a random audio clip from the array
            wrongAudiosSource.clip = wrongAudios[Random.Range(0, wrongAudios.Length)];
            // Play the selected audio clip
            wrongAudiosSource.Play();
        }
        else
        {
            Debug.LogWarning("No audio clips available to play.");
        }
    }

    public void Shake()
    {
        transform.DOShakePosition(shakeDuration, shakeStrength, shakeVibrato, shakeRandomness, false)
            .OnComplete(() => ResetToOriginalPosition());
        
    }

    // Static function to reset all draggable objects to their original positions
    public static void ResetAllToOriginalPositions()
    {

        foreach (DragAndDrop draggableObject in draggableObjects)
        {
            draggableObject.ResetToOriginalPosition();
            
        }
        
       
        
    }
    public void ShowTryAgainMessage()
    {
        tryAgainText.gameObject.SetActive(true);
        tryAgainText.text = tryAgainMessages[Random.Range(0, tryAgainMessages.Length)];
        StartCoroutine(WaitAndHide());
    }

    private IEnumerator WaitAndHide()
    {
        yield return new WaitForSeconds(2); // Replace 2 with the number of seconds you want to wait
        tryAgainText.gameObject.SetActive(false);
    }

    public static void clearlist()
    {
        draggableObjects.Clear();

    }
    // Static function to find a DragAndDrop object by its value
    public static DragAndDrop FindDragAndDropByValue(string searchValue)
    {
        foreach (DragAndDrop draggableObject in draggableObjects)
        {
            if (draggableObject.value == searchValue)
            {
                return draggableObject;
            }
        }
        return null;
    }
}
