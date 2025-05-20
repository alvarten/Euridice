using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public GameObject player; // Asigna el jugador desde el Inspector
    public PuzzlePiece[] puzzlePieces; // Lista de todas las piezas del puzzle
    public GameObject puzzleCompletedPanel; // Panel que se muestra cuando el puzzle es completado
    public GameObject resolvingPuzzle; // Cuadro del puzzle resolviendo (vacío)
    public GameObject solvedPuzzle; // Cuadro del puzzle resuelto (vacío)

    void Start()
    {
        // Solo randomizar si hay piezas
        if (puzzlePieces != null && puzzlePieces.Length > 0)
        {
            RandomizePuzzle();
        }
    }

    // Método para comprobar si el puzzle está completado
    public void CheckIfSolved()
    {
        if (puzzlePieces == null || puzzlePieces.Length == 0)
        {
            Debug.LogWarning("No hay piezas de puzzle asignadas.");
            return;
        }

        bool isSolved = true;
        foreach (PuzzlePiece piece in puzzlePieces)
        {
            if (piece == null || !piece.IsInCorrectPosition())
            {
                isSolved = false;
                break;
            }
        }

        if (isSolved)
        {
            Debug.Log("Puzzle completado!");

            if (puzzleCompletedPanel != null)
                puzzleCompletedPanel.SetActive(true);

            if (resolvingPuzzle != null)
                resolvingPuzzle.SetActive(false);

            if (solvedPuzzle != null)
                solvedPuzzle.SetActive(true);
        }
        else
        {
            Debug.Log("Puzzle aún no resuelto.");
        }
    }

    // Método para randomizar las piezas al inicio de la escena
    public void RandomizePuzzle()
    {
        if (puzzlePieces == null || puzzlePieces.Length == 0)
            return;

        for (int i = 0; i < puzzlePieces.Length; i++)
        {
            int randomIndex = Random.Range(0, puzzlePieces.Length);
            if (puzzlePieces[i] != null && puzzlePieces[randomIndex] != null)
            {
                SwapPieces(puzzlePieces[i], puzzlePieces[randomIndex]);
            }
        }

        CheckIfSolved();
    }

    // Método para intercambiar dos piezas sin modificar el `correctId`
    private void SwapPieces(PuzzlePiece pieceA, PuzzlePiece pieceB)
    {
        string tempId = pieceA.correctId;
        pieceA.correctId = pieceB.correctId;
        pieceB.correctId = tempId;

        int tempIndex = pieceA.transform.GetSiblingIndex();
        pieceA.transform.SetSiblingIndex(pieceB.transform.GetSiblingIndex());
        pieceB.transform.SetSiblingIndex(tempIndex);
    }

    public void TogglePuzzlePanel(GameObject panel)
    {
        if (panel == null) return;

        bool isActive = panel.activeSelf;
        panel.SetActive(!isActive);

        if (player != null)
        {
            var controller = player.GetComponent<PlayerController>();
            if (controller != null)
            {
                controller.canMove = !panel.activeSelf;
            }
        }

        //Cursor.visible = panel.activeSelf;
        //Cursor.lockState = panel.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void ActivarPanelResuelto(GameObject panelAntes, GameObject panelDespues)
    {
        if (panelAntes != null) panelAntes.SetActive(false);
        if (panelDespues != null) panelDespues.SetActive(true);
    }
}
