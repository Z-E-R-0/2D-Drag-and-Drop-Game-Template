using UnityEngine;
using DG.Tweening;

public class wincondition : MonoBehaviour
{
    public GameObject[] childObjects;
    public bool gameWon = false; // Boolean variable to track if we won the game
    public GameObject winScreen;
    public float duration;
    public AnimationCurve animGraph;
    public GameObject inGameUI;
    public bool havechildren;

    private void Awake()


    {
        Invoke("DestroyClones", 1);
       
       
        if(childObjects[0].gameObject.transform.childCount > 0)
        {
            //foreach (GameObject obj in childObjects)
            //{
            //    Destroy(obj.transform.GetChild(0));

            //}
            DragAndDrop.ResetAllToOriginalPositions();

            inGameUI.SetActive(true);
            //  winScreen.transform.localScale = Vector3.zero;
            // winScreen.SetActive(false);
            gameWon = false;
        }
        else
        {
            DragAndDrop.ResetAllToOriginalPositions();

            inGameUI.SetActive(true);
            //  winScreen.transform.localScale = Vector3.zero;
            // winScreen.SetActive(false);
            gameWon = false;


        }
        
       

    }

    public void DestroyClones()
    {
        GameController gc = FindObjectOfType<GameController>();
        gc.DestroyAllClones();

    }
    

    

   

   public void CheckWinCondition()
    {
        // Implement your win condition logic here
        // For demonstration, let's say winning condition is when all child objects have children
        havechildren = true;
        foreach (GameObject childObject in childObjects)
        {
            if (childObject.transform.childCount == 0)
            {
                havechildren = false;
                break; // No need to continue checking if one child object doesn't have children
            }
        }

        if (havechildren)
        {
            gameWon = true; // Set gameWon to true if all child objects have children
            Debug.Log("You won the game!");
        }
    }
    // Delete the Extra Spawned Gameobject (Alphabets)//////
     public void deletechild()
    {
        
            foreach (GameObject childObject in childObjects)
            {
                childObject.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }
       
        
    }
}


