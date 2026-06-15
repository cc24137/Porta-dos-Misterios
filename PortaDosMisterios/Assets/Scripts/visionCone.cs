using UnityEngine;

//[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class VisionCone : MonoBehaviour
{
    [Header("Vision Settings")]
    public float viewDistance = 8f;
    public float viewAngle = 90f;
    public int rayCount = 15;

    [Header("Layer Masks")]
    public LayerMask obstacleMask;
    public LayerMask targetMask;

    [Header("Cone Visual")]
    public Color coneColor = new Color(1f, 1f, 0f, 0.25f);
    public Color coneEdgeColor = new Color(1f, 1f, 0f, 0.7f);
    public Material coneMaterial;

    // -- Internal --
    private Mesh _coneMesh;
    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;
    private bool _playerDetected;
    private bool isSeeing = true;

    // olhar
    private Vector2 _direcaoOlhar = Vector2.down;

    /*void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();

        _coneMesh = new Mesh { name = "VisionConeMesh" };
        _meshFilter.mesh = _coneMesh;

        if (coneMaterial != null)
            _meshRenderer.material = coneMaterial;

        // Fallback: creates a basic transparent material at runtime if none is assigned
        if (_meshRenderer.material == null || coneMaterial == null)
        {
            var mat = new Material(Shader.Find("Sprites/Default"));
            mat.color = coneColor;
            _meshRenderer.material = mat;
        }
    }*/

    //void Start()
    //{
     //   _coneMesh = CreateVisionConeMesh();
      //  GameObject coneObject = new GameObject("VisionConeMesh");
       // coneObject.transform.SetParent(transform, false);
        //coneObject.AddComponent<MeshFilter>().mesh = _coneMesh;
        //coneObject.AddComponent<MeshRenderer>().material = coneMaterial;
   // }

    void Update()
    {
        _playerDetected = false;
        CastVision();
        //CastVision();
        //DrawConeMesh();
        //InvokeRepeating(nameof(CastVision), 1f, 0.5f);
    }

    public void SetDirection(Vector2 novaDirecao)
        {
            if (novaDirecao != Vector2.zero)
            {
                _direcaoOlhar = novaDirecao.normalized;
            }
        }

    // ---------------------------------------------------------------
    // VISION RAYCASTING
    // ---------------------------------------------------------------

    void CastVision()
        {
            Vector2 origin = transform.position;
            float startAngle = -viewAngle / 2f;
            float angleStep = viewAngle / (rayCount - 1);

            for (int i = 0; i < rayCount; i++)
            {
                float angle = startAngle + angleStep * i;

                Vector2 dir = Quaternion.Euler(0f, 0f, angle) * _direcaoOlhar;

                RaycastHit2D obstacleHit = Physics2D.Raycast(origin, dir, viewDistance, obstacleMask);
                float rayLen = obstacleHit.collider != null ? obstacleHit.distance : viewDistance;

                RaycastHit2D playerHit = Physics2D.Raycast(origin, dir, rayLen, targetMask);

                if (playerHit.collider != null)
                {
                    if (playerHit.collider.CompareTag("Player"))
                    {
                        _playerDetected = true;
                        Debug.DrawLine(origin, playerHit.point, Color.red);
                        OnPlayerDetected();
                    }
                }
                else
                {
                    Debug.DrawLine(origin, origin + dir * rayLen, Color.green);
                }
            }
        }

    void OnPlayerDetected()
    {
        // Hook: add alert logic here (e.g., state machine transition, event, sound)
        //Debug.Log("Jogador detectado!");
    }

    // ---------------------------------------------------------------
    // CONE MESH (visible in Game View)
    // ---------------------------------------------------------------

    void DrawConeMesh()
    {
        int vertexCount = rayCount + 2;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(rayCount) * 3];

        vertices[0] = Vector3.zero;

        float startAngle = -viewAngle / 2f;
        float angleStep = viewAngle / (rayCount - 1);

        for (int i = 0; i < rayCount; i++)
        {
            float angle = startAngle + angleStep * i;

            // MODIFICADO: Usando _direcaoOlhar no Mesh também
            Vector2 dir = Quaternion.Euler(0f, 0f, angle) * _direcaoOlhar;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, viewDistance, obstacleMask);
            float dist = hit.collider != null ? hit.distance : viewDistance;

            Vector3 localPoint = transform.InverseTransformPoint((Vector2)transform.position + dir * dist);
            vertices[i + 1] = localPoint;
        }

        for (int i = 0; i < rayCount - 1; i++)
        {
            triangles[i * 3 + 0] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        _coneMesh.Clear();
        _coneMesh.vertices = vertices;
        _coneMesh.triangles = triangles;
        _coneMesh.RecalculateNormals();

        _meshRenderer.material.color = _playerDetected ? new Color(1f, 0.1f, 0.1f, 0.35f) : coneColor;
    }

    private Mesh CreateVisionConeMesh()
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[rayCount + 2];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = Vector3.zero; // Origin point

        float angleIncrement = viewAngle / rayCount;
        for (int i = 0; i <= rayCount; i++)
        {
            float angle = -viewAngle / 2 + angleIncrement * i;
            Vector3 vertex = Quaternion.Euler(0, angle, 0) * Vector3.forward * viewDistance;
            vertices[i + 1] = vertex;
        }

        for (int i = 0; i < rayCount; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }
}
