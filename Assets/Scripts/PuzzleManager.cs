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
        // Al inicio de la escena, randomizamos las piezas
        RandomizePuzzle();
    }

    // Método para comprobar si el puzzle está completado
    public void CheckIfSolved()
    {
        bool isSolved = true;
        foreach (PuzzlePiece piece in puzzlePieces)
        {
            if (!piece.IsInCorrectPosition()) // Si alguna pieza no está en su lugar
            {
                isSolved = false;
                break;
            }
        }

        if (isSolved)
        {
            // Si todas las piezas están en su lugar, mostramos el panel de "completado"
            Debug.Log("Puzzle completado!");

            // Mostrar el panel de completado
            //puzzleCompletedPanel.SetActive(true);

            // Desactivar el cuadro de "resolviendo" y activar el cuadro de "resuelto"
            resolvingPuzzle.SetActive(false);
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
        // Asegúrate de que todas las piezas del puzzle estén desordenadas al inicio
        for (int i = 0; i < puzzlePieces.Length; i++)
        {
            int randomIndex = Random.Range(0, puzzlePieces.Length);
            SwapPieces(puzzlePieces[i], puzzlePieces[randomIndex]);
        }

        // Llamar a CheckIfSolved para asegurarse de que el puzzle no esté completado por error
        CheckIfSolved();
    }

    // Método para intercambiar dos piezas sin modificar el `correctId`
    private void SwapPieces(PuzzlePiece pieceA, PuzzlePiece pieceB)
    {
        // Guardamos el correctId de cada pieza antes de intercambiar
        string tempId = pieceA.correctId;
        pieceA.correctId = pieceB.correctId;
        pieceB.correctId = tempId;

        // Intercambiar los índices en el GridLayout
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
                controller.enabled = !panel.activeSelf;
            }
        }

        // También puede desbloquear o bloquear el cursor si es necesario:
        //Cursor.visible = panel.activeSelf;
        //Cursor.lockState = panel.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
