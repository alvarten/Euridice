using UnityEngine;

public class PantallaTutorial : MonoBehaviour
{
    public GameObject panelToShow;

    public void ShowPanel()
    {
        if (panelToShow != null)
        {
            panelToShow.SetActive(true);
        }
    }
}
