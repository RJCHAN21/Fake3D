using UnityEngine;

// Doing this so the movement can happen before projection
[DefaultExecutionOrder(100)]
public class Item : MonoBehaviour
{
    public Vector3 itemPosition;
    [SerializeField] private bool randomizeColor = true;

    private SpriteRenderer spriteRenderer;
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    private void OnEnable()
    {
        if (spriteRenderer != null && randomizeColor)
        {
            spriteRenderer.color = GetRandomColor();
        }
    }

    private void LateUpdate()
    {
        float perspective = CameraComponent.focalLength / (CameraComponent.focalLength + itemPosition.z);

        transform.localScale = Vector3.one * perspective;
        transform.position = new Vector3(itemPosition.x, itemPosition.y, 0f) * perspective;

        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = -Mathf.RoundToInt(itemPosition.z);
        }
    }

    private Color GetRandomColor()
    {
        var rRand = Random.Range(0f, 1f);
        var gRand = Random.Range(0f, 1f);
        var bRand = Random.Range(0f, 1f);
        return new Color(rRand, gRand, bRand);                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                
    }
}