using UnityEngine;

public class CustomGridLayout2D : MonoBehaviour
{
    public GameObject[] gameObjects; // Array of game objects to be arranged
    public float spacing = 10f; // Space between objects

    void Start()
    {
        ArrangeObjectsInGrid();
    }

    void ArrangeObjectsInGrid()
    {
        int totalObjects = gameObjects.Length;
        int columns = Mathf.CeilToInt(totalObjects / 2f); // Calculate the number of columns
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Calculate the width and height of each cell
        float cellWidth = (screenWidth - (spacing * (columns - 1))) / columns;
        float cellHeight = (screenHeight / 2f) - (spacing / 2f);

        for (int i = 0; i < totalObjects; i++)
        {
            int row = i / columns;
            int column = i % columns;

            float xPos = (cellWidth + spacing) * column;
            float yPos = -((cellHeight + spacing) * row);

            Vector3 position = new Vector3(xPos - screenWidth / 2 + cellWidth / 2, yPos + screenHeight / 4, 0);

            gameObjects[i].transform.position = Camera.main.ScreenToWorldPoint(position);
            gameObjects[i].transform.position = new Vector3(gameObjects[i].transform.position.x, gameObjects[i].transform.position.y, 0);
        }
    }
}
