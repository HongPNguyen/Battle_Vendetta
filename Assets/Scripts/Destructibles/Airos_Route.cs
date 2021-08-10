using UnityEngine;

public class Airos_Route : MonoBehaviour
{
    [SerializeField]
    private Transform[] controlSpots;
    private Vector2 TracPosition;

    private void OnDrawGizmos()
    {
        for (float t = 0f; t <= 1f; t += .05f)
        {
            TracPosition = Mathf.Pow(1 - t, 3) * controlSpots[0].position +
                       3 * Mathf.Pow(1 - t, 2) * t * controlSpots[1].position +
                       3 * (1 - t) * Mathf.Pow(t, 2) * controlSpots[2].position +
                        Mathf.Pow(t, 3) * controlSpots[3].position;

            Gizmos.color = Color.black;   // dotted line color
            Gizmos.DrawSphere(TracPosition, 0.25f);
        }

        Gizmos.DrawLine(new Vector2(controlSpots[0].position.x, controlSpots[0].position.y),
            new Vector2(controlSpots[1].position.x, controlSpots[1].position.y));

        Gizmos.DrawLine(new Vector2(controlSpots[2].position.x, controlSpots[2].position.y),
            new Vector2(controlSpots[3].position.x, controlSpots[3].position.y));
    }
}