using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class HoverHighlight : MonoBehaviour
{
    [Header("Outline Look")]
    public Color outlineColor = Color.white;

    [Range(4, 32)] public int steps = 16;

    [Tooltip("Outline thickness (bigger = thicker)")]
    [Range(0.001f, 0.2f)]
    public float outlineSize = 0.05f;   // ? easier name

    [Tooltip("Drag your URP 2D Sprite-Unlit material here")]
    public Material outlineMaterial;

    [Header("Hover Behaviour")]
    public bool showOnlyOnHover = true;

    SpriteRenderer target;
    SpriteRenderer[] outline;
    Transform holder;

    void Awake()
    {
        target = GetComponent<SpriteRenderer>();
        Build();
        SetVisible(!showOnlyOnHover);
    }

#if UNITY_EDITOR
    // Rebuild automatically when you change size in Inspector
    void OnValidate()
    {
        if (Application.isPlaying)
            Build();
    }
#endif

    void Build()
    {
        if (target == null)
            target = GetComponent<SpriteRenderer>();

        if (holder != null)
            Destroy(holder.gameObject);

        holder = new GameObject("__OUTLINE__").transform;
        holder.SetParent(transform);
        holder.localPosition = Vector3.zero;
        holder.localRotation = Quaternion.identity;
        holder.localScale = Vector3.one;

        outline = new SpriteRenderer[steps];
        int order = target.sortingOrder - 1;

        for (int i = 0; i < steps; i++)
        {
            float a = i * Mathf.PI * 2f / steps;
            Vector3 off = new Vector3(Mathf.Cos(a), Mathf.Sin(a), 0f) * outlineSize;

            var go = new GameObject("o" + i);
            go.transform.SetParent(holder);
            go.transform.localPosition = off;

            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = target.sprite;
            sr.color = outlineColor;
            sr.sortingLayerID = target.sortingLayerID;
            sr.sortingOrder = order;

            if (outlineMaterial != null)
                sr.sharedMaterial = outlineMaterial;

            outline[i] = sr;
        }
    }

    void LateUpdate()
    {
        if (outline == null) return;

        foreach (var sr in outline)
        {
            if (!sr) continue;

            sr.sprite = target.sprite;
            sr.flipX = target.flipX;
            sr.flipY = target.flipY;
            sr.sortingLayerID = target.sortingLayerID;
            sr.sortingOrder = target.sortingOrder - 1;
            sr.color = outlineColor;

            if (outlineMaterial != null)
                sr.sharedMaterial = outlineMaterial;
        }
    }

    void SetVisible(bool v)
    {
        if (outline == null) return;
        foreach (var sr in outline)
            if (sr) sr.enabled = v;
    }

    void OnMouseEnter() { if (showOnlyOnHover) SetVisible(true); }
    void OnMouseExit() { if (showOnlyOnHover) SetVisible(false); }
}