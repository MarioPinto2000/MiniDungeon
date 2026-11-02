using UnityEngine;
using System.Collections;

public class Hero : Character
{
    [Header("Hero stats")]
    [SerializeField] private float maxHP = 150f;
    [SerializeField] private float damage = 20f;
    [SerializeField, Min(0f)] private float invincibilityTime = 0.5f; //seconds without taking damage after a hit

    [Header("Hero settings")]
    [SerializeField] private Transform attackPoint; //child object placed in front of this character
    [SerializeField] private float attackRange = 1.5f; //radious of the attack
    [SerializeField] private LayerMask enemyLayers; //only this layers can be hit
    [SerializeField] private float attackCooldown = 0.4f;

    [Header("Cone attack")]
    [SerializeField, Range(10f, 360f)] private float coneAngle = 90f;
    [SerializeField] private Color slashColor = new Color(1f, 0.15f, 0.15f, 0.55f);
    [SerializeField, Min(0.02f)] private float slashDuration = 0.2f;

    private float nextAttackTime;//Time.time value between attacks
    private Coroutine slashRoutine;

    private Transform coneObject;      //child that holds the swing mesh
    private Mesh coneMesh;             //rebuilt on every frame of the swing
    private MeshRenderer coneRenderer; 

    public override float MaxHP => maxHP; //for hp ui
    public override float InvincibilityTime => invincibilityTime;

    //currentHp lives in the main class Character, but the maxhp lives here
    //every subclass has to initialize currentHp
    private void Awake()
    {
        currentHp = maxHP;
        CreateConeVisual();
    }

    private void CreateConeVisual()
    {
        GameObject go = new GameObject("SlashCone");
        coneObject = go.transform;
        coneObject.SetParent(transform, false); //it flips with the hero, local +X = forward

        coneMesh = new Mesh();
        go.AddComponent<MeshFilter>().sharedMesh = coneMesh;

        coneRenderer = go.AddComponent<MeshRenderer>();
        //Sprites/Default is an unlit shader with transparency and no culling
        coneRenderer.sharedMaterial = new Material(Shader.Find("Sprites/Default")) { color = slashColor };
        coneRenderer.sortingOrder = 9;
        coneRenderer.enabled = false;
    }

    //player switches class by enabling/disabling this component,
    private void OnDisable()
    {
        if (slashRoutine != null)
        {
            StopCoroutine(slashRoutine);
            slashRoutine = null;
        }
        if (coneRenderer != null) coneRenderer.enabled = false;
    }

    public override void Attack()
    {
        //CD: ignores this call of still in cd
        if (Time.time < nextAttackTime) return;
        nextAttackTime = Time.time + attackCooldown;

        ShowSlash();

        //same logic as mage for the fire
        Vector2 origin = attackPoint.position;
        Vector2 forward = new Vector2(Mathf.Sign(transform.localScale.x), 0f);

        //finds every collider inside the attack range
        //attack point is a child of hero, so it follows and flips with hero
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            origin,
            attackRange,
            enemyLayers
        );

        foreach (Collider2D hit in hits)
        {
            //hero can never hit itself
            if (hit.gameObject == gameObject) continue;

            Vector2 toTarget = hit.ClosestPoint(origin) - origin;
            if (toTarget.sqrMagnitude > 0.0001f && Vector2.Angle(forward, toTarget) > coneAngle * 0.5f) continue;

            //TryGetComponent avoids erros if the object cannot take damage
            if (hit.TryGetComponent(out IDamageable target))
            {
                target.TakeDamage(damage);
            }
        }
    }

    private void ShowSlash()
    {
        if (coneRenderer == null || attackPoint == null) return;

        //restart the animation if the player attacks again before the previous one ended.
        if (slashRoutine != null) StopCoroutine(slashRoutine);
        slashRoutine = StartCoroutine(SlashRoutine());
    }

    private IEnumerator SlashRoutine()
    {
        //place the apex at the attack point
        coneObject.localPosition = transform.InverseTransformPoint(attackPoint.position);

        //is scaled with the player
        coneObject.localScale = new Vector3(
            1f / Mathf.Abs(transform.lossyScale.x),
            1f / Mathf.Abs(transform.lossyScale.y),
            1f
        );

        coneRenderer.enabled = true;

        float elapsed = 0f;
        while (elapsed < slashDuration)
        {
            elapsed += Time.deltaTime;
            BuildSweepMesh(Mathf.Clamp01(elapsed / slashDuration));
            yield return null;
        }

        coneRenderer.enabled = false;
        slashRoutine = null;
    }

    private void BuildSweepMesh(float progress)
    {
        float half = coneAngle * 0.5f;
        float startAngle = half;                           //swing starts at the top edge
        float sweptAngle = coneAngle * progress;           //and moves down by this many degrees

        int segments = Mathf.Max(1, Mathf.CeilToInt(24f * progress));

        Vector3[] vertices = new Vector3[segments + 2];
        Color32[] colors = new Color32[segments + 2];
        int[] triangles = new int[segments * 3];

        vertices[0] = Vector3.zero; // Apex
        for (int i = 0; i <= segments; i++)
        {
            float angle = (startAngle - sweptAngle * i / segments) * Mathf.Deg2Rad;
            vertices[i + 1] = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * attackRange;
        }

        for (int i = 0; i < colors.Length; i++) colors[i] = new Color32(255, 255, 255, 255);

        for (int i = 0; i < segments; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        coneMesh.Clear();
        coneMesh.vertices = vertices;
        coneMesh.colors32 = colors;
        coneMesh.triangles = triangles;
    }

}