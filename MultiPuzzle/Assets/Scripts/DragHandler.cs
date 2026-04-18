using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DragHandler : MonoBehaviour
{
    private bool isDragging;
    private Vector3 offset;

    [SerializeField] private float idleScale = 0.5f;
    [SerializeField] private float dragScale = 1f;

    private void Start()
    {
        transform.localScale = Vector3.one * idleScale;
    }

    private void OnMouseDown()
    {

        isDragging = true;

        Vector3 mousePos = GetMouseWorldPosition();
        offset = transform.position - mousePos;

        transform.localScale = Vector3.one * dragScale;
    }

    private void OnMouseDrag()
    {
        if (!isDragging) return;

        Vector3 mousePos = GetMouseWorldPosition();
        transform.position = mousePos + offset;
    }

    private void OnMouseUp()
    {
        isDragging = false;

        transform.localScale = Vector3.one * idleScale;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouse = Input.mousePosition;
        mouse.z = Mathf.Abs(Camera.main.transform.position.z);

        Vector3 world = Camera.main.ScreenToWorldPoint(mouse);
        world.z = 0;

        return world;
    }
}