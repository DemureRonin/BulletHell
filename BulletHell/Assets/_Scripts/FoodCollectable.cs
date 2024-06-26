using UnityEngine;

public class FoodCollectable : MonoBehaviour
{
    public delegate void CollectEvent();

    public static event CollectEvent OnCollect;

    public void Collect()
    {
        OnCollect?.Invoke();
    }
}