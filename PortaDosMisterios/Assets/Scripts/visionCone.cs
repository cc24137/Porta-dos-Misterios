using UnityEngine;

public class VisionCone : MonoBehaviour
{
    public float viewDistance = 8f;
    public float viewAngle = 90f;
    public int rayCount = 30;

    public LayerMask obstacleMask;
    public LayerMask targetMask;

    void Update()
    {
        CastVision();
    }

    void CastVision()
    {
        Debug.Log("Cast vision cone update");
        Vector2 origin = transform.position;

        float startAngle = -viewAngle / 2f;
        float angleStep = viewAngle / (rayCount - 1);

        for (int i = 0; i < rayCount; i++)
        {
            float angle = startAngle + angleStep * i;

            Vector2 dir =
                Quaternion.Euler(0, 0, angle) * transform.up;

            RaycastHit2D hit = Physics2D.Raycast(
                origin,
                dir,
                viewDistance,
                obstacleMask | targetMask
            );

            if (hit.collider != null)
            {
                Debug.DrawLine(origin, hit.point, Color.red);

                if (hit.collider.CompareTag("Player"))
                {
                    Debug.Log("Jogador detectado");
                }
            }
            else
            {
                Debug.DrawLine(
                    origin,
                    origin + dir * viewDistance,
                    Color.green
                );
            }
        }
    }
}