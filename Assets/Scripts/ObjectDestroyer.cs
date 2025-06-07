using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    public void DestroyObject(GameObject obj)
    {
        if (obj != null)
        {
            Destroy(obj);
        }
        else
        {
            Debug.LogWarning("Intentaste destruir un objeto nulo.");
        }
    }
}
