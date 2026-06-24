
using UnityEngine;
using UnityEngine.Events;

//[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class VisionCone : MonoBehaviour
{
    [Header("Vision Settings")]
    public float viewDistance = 8f;
    public float viewAngle = 90f;
    public int rayCount = 15;
    public float blindTime = 3f;

    [Header("Layer Masks")]
    public LayerMask obstacleMask;
    public LayerMask targetMask;
    public LayerMask blindingMask;

    [Header("Cone Visual")]
    public Color coneColor = new Color(1f, 1f, 0f, 0.25f);
    public Color coneEdgeColor = new Color(1f, 1f, 0f, 0.7f);
    public Material coneMaterial;

    public UnityEvent eventoAchou;

    // -- Internal --
    private Mesh _coneMesh;
    [SerializeField] private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;
    private bool _playerDetected;
    public bool isSeeing {get; set;}
    private float timeBlinded = 0f;
    private bool jaTocouSom = false;

    // olhar
    private Vector2 _direcaoOlhar = Vector2.down;

    private patrol meuPatrulha;


    void Start()
    {  
        if (_meshFilter == null)
        {
            _meshFilter = GetComponentInChildren<MeshFilter>();
        }   

        _meshRenderer = GetComponentInChildren<MeshRenderer>();

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

        _meshRenderer.sortingLayerName = "Player"; 
        
        _meshRenderer.sortingOrder = 5;

        isSeeing = true;
        meuPatrulha = GetComponent<patrol>();
    }

    void Update()
    {
        _playerDetected = false;
        if (isSeeing)
        {
            CastVision();
            coneColor.a = 0.25f;
        }
        else
        {
            coneColor.a = 0f;
            timeBlinded += Time.deltaTime;
            if (timeBlinded >= blindTime)
            {
                timeBlinded = 0f;
                isSeeing = true;
            }
        }

        DrawConeMesh();

        if (meuPatrulha != null)
        {
            meuPatrulha.detectouPlayer = _playerDetected;
        }
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

                RaycastHit2D flashHit = Physics2D.Raycast(origin, dir, rayLen, blindingMask);

                if (flashHit.collider != null)
                {
                    //if (flashHit.collider.CompareTag("FlashObject"))
                    //{
                      //  Debug.Log("Viu flash");
                        isSeeing = false;
                    //}
                }

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
        if (!jaTocouSom)
        {
            eventoAchou?.Invoke();
            jaTocouSom = true;
        }
    }

    // ---------------------------------------------------------------
    // CONE MESH (visible in Game View)
    // ---------------------------------------------------------------

    void DrawConeMesh()
    {
        int vertexCount = rayCount + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(rayCount-1) * 3];

        // vertices[0] = new Vector3(0, 0, -0.1f);
        vertices[0] = new Vector3(0, 0, 0);

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
            //localPoint.z = -0.1f;
            vertices[i + 1] = localPoint;
        }

        for (int i = 0; i < rayCount - 1; i++)
        {
            triangles[i * 3 + 0] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        if (_coneMesh == null) 
        {
            _coneMesh = new Mesh();
        }

        _coneMesh.Clear();
        _coneMesh.vertices = vertices;
        _coneMesh.triangles = triangles;
        _coneMesh.RecalculateNormals();

        _meshFilter.mesh = _coneMesh;

        Color corFinal = _playerDetected ? new Color(1f, 0.1f, 0.1f, 0.35f) : coneColor;
        
        if (_meshRenderer.material.HasProperty("_BaseColor"))
        {
            _meshRenderer.material.SetColor("_BaseColor", corFinal); // Se for URP
        }
        else
        {
            _meshRenderer.material.color = corFinal; // Se for Unity Padrão
        }
    }

}
