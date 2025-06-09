using UnityEngine;

public class SetGame : MonoBehaviour
{
    public void Restart()
    {
        PlayerPrefs.SetInt("LastScene", 1);
        PlayerPrefs.Save();
        Debug.Log("LastScene seteado a 1.");
    }
}
