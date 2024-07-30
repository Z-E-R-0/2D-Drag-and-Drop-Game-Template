using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public GameObject mainScreenPanel;
    public GameObject winScreenPanel;
    public GameObject levelSelectorPanel;
    public GameObject levels;
    private int currentLevelIndex = 0;
    private GameObject[] levelsObject;
    public GameObject[] childObjects;
    private bool isGameWon;
    public GameObject levelButtonPrefab;
    public GameObject levelSpawnerPoint;
    public float transitionDuration = 1.0f; // Duration of the transition
    public AudioSource sceneSwitch;
    public StartPopupManager startPopupManager;

    private const string LevelKey = "currentLevelIndex";
    private const string MaxUnlockedLevelKey = "maxUnlockedLevel";

    // Start is called before the first frame update
    void Start()
    {
        mainScreenPanel.SetActive(true);
        winScreenPanel.SetActive(false);
        levelsObject = new GameObject[levels.transform.childCount];

        // Set All Levels to False
        for (int i = 0; i < levels.transform.childCount; i++)
        {
            levelsObject[i] = levels.transform.GetChild(i).gameObject;
            levelsObject[i].SetActive(false);
        }

        // Load the highest unlocked level
        int maxUnlockedLevel = PlayerPrefs.GetInt(MaxUnlockedLevelKey, 0);
        if (maxUnlockedLevel >= levelsObject.Length)
        {
            currentLevelIndex = 0;
        }
        else
        {
            currentLevelIndex = maxUnlockedLevel;
        }
    }

    public void PlayLevel()
    {
        levelsObject[currentLevelIndex].SetActive(true);
        mainScreenPanel.SetActive(false);
        SlideIn(levelsObject[currentLevelIndex]);
    }

    public void ShowWinScreen()
    {
        winScreenPanel.SetActive(true);
        startPopupManager.ShowStars();

        // Save the current level index
        PlayerPrefs.SetInt(LevelKey, currentLevelIndex);

        // Unlock the next level
        int maxUnlockedLevel = PlayerPrefs.GetInt(MaxUnlockedLevelKey, 0);
        if (currentLevelIndex >= maxUnlockedLevel)
        {
            PlayerPrefs.SetInt(MaxUnlockedLevelKey, currentLevelIndex + 1);
        }
    }

    public void ReplayLevel()
    {
        winScreenPanel.SetActive(false);
        winreseter();
        levelsObject[currentLevelIndex].SetActive(true);
        SlideIn(levelsObject[currentLevelIndex]);
    }

    public void NextLevel()
    {
        winreseter();

        GameObject currentLevel = levelsObject[currentLevelIndex];
        currentLevelIndex = (currentLevelIndex + 1) % levelsObject.Length;
        GameObject nextLevel = levelsObject[currentLevelIndex];

        SlideOut(currentLevel, () =>
        {
            SlideInNextLevel();
        });
        winScreenPanel.SetActive(false);
    }

    public void PreviousLevel()
    {
        winreseter();
        winScreenPanel.SetActive(false);
        GameObject currentLevel = levelsObject[currentLevelIndex];
        currentLevelIndex = (currentLevelIndex - 1 + levelsObject.Length) % levelsObject.Length;
        GameObject previousLevel = levelsObject[currentLevelIndex];

        SlideOut(currentLevel, () =>
        {
            Invoke(nameof(SlideInPreviousLevel), 0.1f);
        });
    }

    public void GoToMainMenu()
    {
        winScreenPanel.SetActive(false);
        mainScreenPanel.SetActive(true);
        winreseter();
        foreach (GameObject level in levelsObject)
        {
            level.SetActive(false);
        }
    }

    public void winreseter()
    {
        winScreenPanel.SetActive(false);
        DragAndDrop DragAndDrop = FindObjectOfType<DragAndDrop>();
        wincondition winCondition = FindObjectOfType<wincondition>();
        if (winCondition.childObjects[0].transform.childCount < 0)
        {
            DragAndDrop.ResetAllToOriginalPositions();
            DragAndDrop.clearlist();
            winCondition.gameWon = false;
            winCondition.havechildren = false;
            DestroyAllClones();
        }
        else
        {
            DragAndDrop.ResetAllToOriginalPositions();
            winCondition.gameWon = false;
            winCondition.havechildren = false;
            DestroyAllClones();
        }
    }

    public void ToggleAudio()
    {
        AudioListener.pause = !AudioListener.pause;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void CheckWinCondition()
    {
        isGameWon = levelsObject[currentLevelIndex].GetComponent<wincondition>().gameWon;
        if (isGameWon)
        {
            ShowWinScreen();
        }
    }

    public void DestroyAllClones()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.name.EndsWith("(Clone)"))
            {
                obj.SetActive(false);
                Debug.Log($"Destroyed clone: {obj.name}");
            }
        }
    }

    public void OpenLevelSelector()
    {
        levelSelectorPanel.SetActive(true);
        mainScreenPanel.SetActive(false);

        // Clear existing buttons if any
        foreach (Transform child in levelSpawnerPoint.transform)
        {
            Destroy(child.gameObject);
        }

        // Spawn buttons for each level
        int maxUnlockedLevel = PlayerPrefs.GetInt(MaxUnlockedLevelKey, 0);
        for (int i = 0; i < levelsObject.Length; i++)
        {
            GameObject button = Instantiate(levelButtonPrefab, levelSpawnerPoint.transform);
            int levelIndex = i;
            button.GetComponentInChildren<TMP_Text>().text = (i + 1).ToString();
            button.GetComponent<Button>().onClick.AddListener(() => LoadLevel(levelIndex));

            bool isUnlocked = i <= maxUnlockedLevel;
            button.GetComponent<Button>().interactable = isUnlocked; // Lock/unlock levels

            // Set the appropriate indicator
            if (isUnlocked)
            {
                button.transform.GetChild(1).gameObject.SetActive(false);
                button.transform.GetChild(0).gameObject.SetActive(true);
                button.transform.GetChild(3).gameObject.SetActive(false);

            }
            else
            {

                button.transform.GetChild(1).gameObject.SetActive(true);
                button.transform.GetChild(0).gameObject.SetActive(false);
                button.transform.GetChild(3).gameObject.SetActive(true);
                
            }
        }
    
}

    public void LoadLevel(int levelIndex)
    {
        levelSelectorPanel.SetActive(false);
        currentLevelIndex = levelIndex;
        PlayLevel();
    }

    public void SlideIn(GameObject level)
    {
        sceneSwitch.Play();
        level.transform.localPosition = new Vector3(Screen.width, 0, 0);
        LeanTween.moveLocalX(level, 0, transitionDuration).setEase(LeanTweenType.easeInOutQuad);
    }

    public void SlideOut(GameObject level, System.Action onComplete)
    {
        level.transform.localPosition = new Vector3(0, 0, 0);
        LeanTween.moveLocalX(level, -Screen.width, transitionDuration).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() =>
        {
            level.SetActive(false);
            onComplete?.Invoke();
        });
    }

    private void SlideInNextLevel()
    {
        GameObject nextLevel = levelsObject[currentLevelIndex];
        nextLevel.SetActive(true);
        SlideIn(nextLevel);
    }

    private void SlideInPreviousLevel()
    {
        GameObject previousLevel = levelsObject[currentLevelIndex];
        previousLevel.SetActive(true);
        SlideIn(previousLevel);
    }
}
