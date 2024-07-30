using DG.Tweening;
using UnityEngine;

public class PathAnimation : MonoBehaviour
{
    public Transform[] waypoints;
    public bool isfade;
    void Start()
    {
        if(!isfade)
        {
            // Create a path from the array of waypoints
            Vector3[] path = new Vector3[waypoints.Length];
            for (int i = 0; i < waypoints.Length; i++)
            {
                path[i] = waypoints[i].position;
            }

            // Animate the object along the path
            transform.DOPath(path, 1f, PathType.Linear)
                .SetEase(Ease.Linear);
            //.SetLoops(-1, LoopType.Yoyo); // Loop back and forth indefinitely

        }
        else
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.DOFade(0f, 1f)
                .SetEase(Ease.InOutQuad) // Use a smooth ease for a gradual effect
                .SetLoops(-1, LoopType.Yoyo); // Loop back and forth indefinitely

            // Create a path from the array of waypoints
            Vector3[] path = new Vector3[waypoints.Length];
            for (int i = 0; i < waypoints.Length; i++)
            {
                path[i] = waypoints[i].position;
            }

            // Animate the object along the path
            transform.DOPath(path, 1f, PathType.Linear)
                .SetEase(Ease.Linear);
            //.SetLoops(-1, LoopType.Yoyo); // Loop back and forth indefinitely
        }

    }
}
