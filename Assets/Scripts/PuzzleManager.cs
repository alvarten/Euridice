using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public GameObject player; 
    public PuzzlePiece[] puzzlePieces; // Lista de todas las piezas del puzzle del cuadro
    public GameObject puzzleCompletedPanel; // Panel que se muestra cuando el puzzle del cuadro es completado
    public GameObject resolvingPuzzle; 
    public GameObject solvedPuzzle;
    public List<GameObject> puzzlePanels;
    public List<string> panelsExcluidos;

    public bool isPuzzleOpen = false;
    private GameObject currentActivePanel;

    void Start()
    {
        // Se detectan las piezas y se randomizan
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

    // Método para intercambiar dos piezas
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

        isPuzzleOpen = panel.activeSelf;
        currentActivePanel = isPuzzleOpen ? panel : null;

        if (isPuzzleOpen)
        {
            CanvasGroup cg = panel.GetComponent<CanvasGroup>();
            if (cg == null)
            {
                cg = panel.AddComponent<CanvasGroup>();
            }

            cg.alpha = 0f; // Empezamos transparente
            cg.interactable = true;
            cg.blocksRaycasts = true;

            StopAllCoroutines();
            StartCoroutine(FadeCanvasGroup(cg, 1f, 0.6f));
        }
    }
    public void CerrarPanelesParaAnimacion()
    {
        foreach (GameObject panel in puzzlePanels)
        {
            if (panel.activeSelf && !panelsExcluidos.Contains(panel.name))
            {
                TogglePuzzlePanel(panel);
            }

            isPuzzleOpen = false;
            currentActivePanel = null;
        }
    }
    public void CloseCurrentPuzzle()
    {
        if (currentActivePanel != null && currentActivePanel.activeSelf)
        {
            // Desactivamos directamente el panel (no usamos Toggle para evitar inconsistencias)
            currentActivePanel.SetActive(false);

            // Restauramos movimiento del player
            if (player != null)
            {
                var controller = player.GetComponent<PlayerController>();
                if (controller != null)
                    controller.canMove = true;
            }

            // Reseteamos estado interno
            isPuzzleOpen = false;
            currentActivePanel = null;
        }
    }
    public void ActivarPanelResuelto(GameObject panelAntes, GameObject panelDespues)
    {
        if (panelAntes != null) panelAntes.SetActive(false);
        if (panelDespues != null) panelDespues.SetActive(true);
    }


    // Corrutina para animar el alpha de un CanvasGroup
    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float targetAlpha, float duration)
    {
        float startAlpha = cg.alpha;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cg.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);
            yield return null;
        }

        cg.alpha = targetAlpha;
    }
}
