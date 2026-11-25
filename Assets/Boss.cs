using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Boss : Enemy
{
    [Header("랜덤 속도 및 이동 주기")]
    [SerializeField] private float minSpeed = 1f;
    [SerializeField] private float maxSpeed = 2f;
    [SerializeField] private float minMoveInterval = 1.0f;
    [SerializeField] private float maxMoveInterval = 2.5f;
    [SerializeField] private float arrivalThreshold = 0.2f;

    [Header("랜덤 위치 생성 여백 (뷰포트 기준)")]
    [Tooltip("0이면 카메라 화면(0~1) 안, 음수/1초과 값이면 화면 밖까지 목표 좌표를 뽑을 수 있습니다.")]
    [SerializeField] private float viewportSpawnMargin = 0.25f;

    [Header("다른 Enemy Collider 회피")]
    [Tooltip("0이면 이 오브젝트의 Collider2D 크기 기준으로 자동 계산합니다.")]
    [SerializeField] private float customCheckRadius = 0f;
    [SerializeField] private float colliderPadding = 0.2f;
    [Tooltip("Enemy 레이어만 인식하도록 기본 설정됩니다.")]
    [SerializeField] private LayerMask enemyLayerMask;
    [SerializeField] private int maxPickAttempts = 25;

    private Rigidbody2D rb;
    private Collider2D col;
    private Camera mainCam;

    private Vector2 targetPosition;
    private float currentSpeed;
    private float nextMoveTimer;
    private bool hasTarget;

    void Reset()
    {
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        if (enemyLayer >= 0)
        {
            enemyLayerMask = 1 << enemyLayer;
        }
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        mainCam = Camera.main;

        if (enemyLayerMask.value == 0)
        {
            int enemyLayer = LayerMask.NameToLayer("Enemy");
            enemyLayerMask = enemyLayer >= 0 ? (1 << enemyLayer) : ~0;
        }
    }

    void Start()
    {
        if (mainCam == null)
        {
            mainCam = Camera.main;
        }

        Move();
    }

    void Update()
    {
        if (IsColliderCompletelyOutsideGameView())
        {
            Destroy(gameObject);
            return;
        }

        nextMoveTimer -= Time.deltaTime;
        bool reachedTarget = hasTarget && Vector2.Distance(rb.position, targetPosition) <= arrivalThreshold;

        if (nextMoveTimer <= 0f || reachedTarget)
        {
            Move();
        }
    }

    void FixedUpdate()
    {
        if (!hasTarget) return;

        Vector2 nextPos = Vector2.MoveTowards(rb.position, targetPosition, currentSpeed * Time.fixedDeltaTime);
        float radius = GetCheckRadius();

        // 이동하려는 다음 위치에 다른 Enemy 콜라이더가 있으면 새 목표 위치 재선정
        if (HasOtherEnemyColliderAt(nextPos, radius))
        {
            Move();
            return;
        }

        rb.MovePosition(nextPos);
    }

    void Move()
    {
        nextMoveTimer = Random.Range(minMoveInterval, maxMoveInterval);
        currentSpeed = Random.Range(minSpeed, maxSpeed);

        if (mainCam == null)
        {
            mainCam = Camera.main;
            if (mainCam == null) return;
        }

        float zDist = Mathf.Abs(transform.position.z - mainCam.transform.position.z);
        float radius = GetCheckRadius();

        for (int i = 0; i < maxPickAttempts; i++)
        {
            float vpX = Random.Range(-viewportSpawnMargin, 1f + viewportSpawnMargin);
            float vpY = Random.Range(-viewportSpawnMargin, 1f + viewportSpawnMargin);

            Vector3 worldPos3 = mainCam.ViewportToWorldPoint(new Vector3(vpX, vpY, zDist));
            Vector2 candidate = new Vector2(worldPos3.x, worldPos3.y);

            if (!HasOtherEnemyColliderAt(candidate, radius))
            {
                targetPosition = candidate;
                hasTarget = true;
                return;
            }
        }

        hasTarget = false;
    }

    private float GetCheckRadius()
    {
        if (customCheckRadius > 0f)
        {
            return customCheckRadius;
        }

        if (col != null)
        {
            Vector2 ext = col.bounds.extents;
            return Mathf.Max(ext.x, ext.y) + colliderPadding;
        }

        return 0.5f + colliderPadding;
    }

    private bool HasOtherEnemyColliderAt(Vector2 point, float radius)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(point, radius, enemyLayerMask);

        foreach (Collider2D hit in hits)
        {
            if (hit == null || !hit.enabled) continue;

            // 자기 자신 및 자식 오브젝트 콜라이더 제외
            if (hit.gameObject == gameObject || hit.transform.IsChildOf(transform))
            {
                continue;
            }

            // 다른 Enemy 오브젝트의 Collider만 인식
            if (hit.GetComponentInParent<Enemy>() != null)
            {
                return true;
            }
        }

        return false;
    }

    private bool IsColliderCompletelyOutsideGameView()
    {
        if (mainCam == null) return false;

        if (col != null)
        {
            Bounds b = col.bounds;
            Vector3 minVp = mainCam.WorldToViewportPoint(b.min);
            Vector3 maxVp = mainCam.WorldToViewportPoint(b.max);

            return maxVp.x < 0f || minVp.x > 1f || maxVp.y < 0f || minVp.y > 1f;
        }

        Vector3 vp = mainCam.WorldToViewportPoint(transform.position);
        return vp.x < 0f || vp.x > 1f || vp.y < 0f || vp.y > 1f;
    }
}