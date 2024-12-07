using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class WorldStreamingEditor : EditorWindow {
  public delegate void GridSquareClicked(int x, int y);  // Delegate for callback

  private int gridWidth = 8;
  private int gridHeight = 8;
  private float squareSize = 40f;  // Size of each square in the grid
  private Color defaultColor = Color.red;
  private Color clickColor = Color.green;

  public static bool[] clickedSquares = null;  // Array to store which squares have been clicked

  private GridSquareClicked onSquareClicked;

  private WorldStreamingEditor() {
    // Set the default callback function
    onSquareClicked = (x, y) => ClickCallback(x, y);
  }

  private void ClickCallback(int x, int y) {
    clickedSquares[y * gridWidth + x] = !clickedSquares[y * gridWidth + x];
    Debug.Log("Clicked on square: " + x + ", " + y);
  }

  // Method to initialize the editor window
  [MenuItem("Window/World Streaming Editor")]
  public static void ShowWindow() {
    GetWindow<WorldStreamingEditor>("World Streaming Editor");
  }

  private void OnGUI() {
    // Initialize the clickedSquares array if it hasn't been created yet
    if (clickedSquares == null) {
      clickedSquares = new bool[gridWidth * gridHeight];
    }

    DrawGrid();
  }

  // Draws a grid of clickable squares
  private void DrawGrid() {
    // Title label
    GUILayout.Label("World Streaming Grid", EditorStyles.boldLabel);

    Rect gridRect = new Rect(10, 20, (gridWidth + 0.5f) * squareSize, (gridHeight + 0.5f) * squareSize);
    GUILayout.BeginArea(gridRect);

    for (int y = 0; y < gridHeight; y++) {
      GUILayout.BeginHorizontal();
      for (int x = 0; x < gridWidth; x++) {
        // Set color based on whether the square has been clicked
        GUI.backgroundColor = clickedSquares[y * gridWidth + x] ? clickColor : defaultColor;
        if (GUILayout.Button(x + ", " + y, GUILayout.Width(squareSize), GUILayout.Height(squareSize))) {
          // Trigger callback when a square is clicked
          onSquareClicked?.Invoke(x, y);
        }
      }
      GUILayout.EndHorizontal();
    }

    GUILayout.EndArea();
  }

  public static bool[] GetClickedSquares() {
    return clickedSquares;
  }

  // Method to set the callback function
  public void SetOnSquareClickedCallback(GridSquareClicked callback) {
    onSquareClicked = callback;
  }
}