using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DragHandler : MonoBehaviour
{
    private bool isDragging;
    private bool isPlaced = false;
    private Vector3 offset;
    private Vector3 startPosition;
    private BlockSnapper snapper;

    [SerializeField] private float idleScale = 0.5f;
    [SerializeField] private float normalScale = 1f;
    [SerializeField] private float fingerOffset = 1.5f;

    private void Awake()
    {
        snapper = FindFirstObjectByType<BlockSnapper>();
    }
    private void Start()
    {
        transform.localScale = Vector3.one * idleScale;
        startPosition = transform.position;
    }

    private void OnMouseDown()
    {
        isDragging = true;

        Vector3 mousePos = GetMouseWorldPosition();

        // calculate offset so block doesn't jump
        offset = transform.position - mousePos;

        transform.localScale = Vector3.one * normalScale;
    }

    private void OnMouseDrag()
    {
        if (!isDragging) return;

        Vector3 mousePos = GetMouseWorldPosition();

        transform.position = new Vector3(
            mousePos.x + offset.x,
            mousePos.y + offset.y + fingerOffset,
            0
        );
    }

    private void OnMouseUp()
    {
        if (isPlaced) return;

        isDragging = false;

        if (snapper != null && snapper.TrySnapAndPlace(transform))
        {
            // successfully placed
            isPlaced = true;

            transform.localScale = Vector3.one;

            enabled = false; // lock block
            return;
        }

        // failed → return to start
        transform.position = startPosition;
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