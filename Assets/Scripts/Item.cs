using UnityEngine;

public class Item : MonoBehaviour
{
    public Vector3 itemPosition;
    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if(spriteRenderer != null)
        {
            spriteRenderer.color = GetRandomColor();
        }
    }

    private void LateUpdate()
    {
        var perspective = CameraComponent.focalLength / (CameraComponent.focalLength + itemPosition.z);
        transform.localScale = Vector3.one * perspective;
        transform.position = new Vector3(itemPosition.x, itemPosition.y, 0f) * perspective;
    }

    private Color GetRandomColor()
    {
        var rRand = Random.Range(0f, 1f);
        var gRand = Random.Range(0f, 1f);
        var bRand = Random.Range(0f, 1f);
        return new Color(rRand, gRand, bRand);                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                
    }
}