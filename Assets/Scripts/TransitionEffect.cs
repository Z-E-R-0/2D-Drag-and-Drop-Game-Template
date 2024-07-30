using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TransitionEffect : MonoBehaviour
{
    public int rows = 5;
    public int columns = 5;
    public GridLayoutGroup gridLayout;
    public List<Sprite> spriteList;
    public Vector2 scaleRange = new Vector2(0.5f, 2f);
    public Vector3 rotationRange = new Vector3(0f, 0f, 360f);
    public float fadeInDuration = 0.5f;
    public float fadeOutDuration = 0.5f;
    public float scaleOutDuration = 0.5f;
    public float masterTransitionDuration = 5f;

    private List<GameObject> pooledSprites = new List<GameObject>();
    private List<GameObject> activeSprites = new List<GameObject>();

    private void Start()
    {
        // Populate the object pool
        PopulateObjectPool();
    }

    private void PopulateObjectPool()
    {
        foreach (Sprite sprite in spriteList)
        {
            GameObject spriteObject = new GameObject("Sprite");
            Image image = spriteObject.AddComponent<Image>();
            image.sprite = sprite;
            spriteObject.SetActive(false); // Initially deactivate the sprite
            pooledSprites.Add(spriteObject);
        }
    }

    public void StartTransition()
    {
        StartCoroutine(Transition());
    }

    private IEnumerator Transition()
    {
        float spriteInterval = masterTransitionDuration / (rows * columns);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                GameObject spriteObject = GetPooledSprite();

                spriteObject.transform.SetParent(transform);
                RectTransform rectTransform = spriteObject.GetComponent<RectTransform>();
                rectTransform.localScale = Vector3.zero;
                rectTransform.localEulerAngles = new Vector3(0f, 0f, Random.Range(rotationRange.x, rotationRange.y));

                LeanTween.scale(spriteObject, Vector3.one * Random.Range(scaleRange.x, scaleRange.y), fadeInDuration).setEase(LeanTweenType.easeOutBack);
                LeanTween.alpha(spriteObject, 1f, fadeInDuration);

                activeSprites.Add(spriteObject); // Add to active sprites list

                yield return new WaitForSeconds(spriteInterval);
            }
        }

        yield return new WaitForSeconds(masterTransitionDuration);

        // Reverse the hiding process
        foreach (GameObject spriteObject in activeSprites)
        {
            LeanTween.alpha(spriteObject, 0f, fadeOutDuration);
            LeanTween.scale(spriteObject, Vector3.zero, scaleOutDuration).setEase(LeanTweenType.easeInBack).setOnComplete(() =>
            {
                // Instead of destroying, just deactivate the sprite
                spriteObject.SetActive(false);
            });
        }

        activeSprites.Clear(); // Clear the active sprites list
    }

    private GameObject GetPooledSprite()
    {
        // Retrieve a sprite from the pool and activate it
        foreach (GameObject spriteObject in pooledSprites)
        {
            if (!spriteObject.activeSelf)
            {
                spriteObject.SetActive(true);
                return spriteObject;
            }
        }

        // If all pooled sprites are active, create a new one
        GameObject newSprite = Instantiate(pooledSprites[0], transform);
        newSprite.SetActive(true);
        pooledSprites.Add(newSprite);
        return newSprite;
    }
}
