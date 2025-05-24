using UnityEngine;

public abstract class BaseLockManager : MonoBehaviour
{
    public static BaseLockManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public abstract void CheckCombination();
}
