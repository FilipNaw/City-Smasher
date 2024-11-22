using UnityEngine;

public class StaticCamera : MonoBehaviour
{
    public Transform player; // Obiekt gracza

    void Update()
    {
        if (player != null)
        {
            // Skieruj kamerê w stronê gracza
            transform.LookAt(player);
        }
    }
}