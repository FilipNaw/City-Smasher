using UnityEngine;

public class StaticCamera : MonoBehaviour
{
    public Transform player; // Obiekt gracza

    void Update()
    {
        if (player != null)
        {
            // Skieruj kamer� w stron� gracza
            transform.LookAt(player);
        }
    }
}