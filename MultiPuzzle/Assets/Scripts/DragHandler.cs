using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DragHandler : MonoBehaviour
{
    private bool isDragging;
    private bool isPlaced = false;
    private Vector3 offset;
    private Vector3 startPosition;
    private Vector2Int[] currentPlacedCells;
    private Vector2Int[] previousPlacedCells;
    private bool releasedPlacedCellsThisDrag;
    private BlockSnapper snapper;
    private BlockSpawner blockSpawner;

    [SerializeField] private float idleScale = 0.5f;
    [SerializeField] private float normalScale = 1f;
    [SerializeField] private float fingerOffset = 1.5f;

    private void Awake()
    {
        snapper = FindFirstObjectByType<BlockSnapper>();
        blockSpawner = FindFirstObjectByType<BlockSpawner>();
    }
    private void Start()
    {
        transform.localScale = Vector3.one * idleScale;
        startPosition = transform.position;
    }

    private void OnMouseDown()
    {
        isDragging = true;
        releasedPlacedCellsThisDrag = false;

        // Allow repositioning: free previous occupied cells while dragging this block.
        if (isPlaced && snapper != null && currentPlacedCells != null)
        {
            previousPlacedCells = (Vector2Int[])currentPlacedCells.Clone();
            snapper.SetCellsState(currentPlacedCells, CellState.SHAPE);
            currentPlacedCells = null;
            isPlaced = false;
            releasedPlacedCellsThisDrag = true;
        }
        else
        {
            previousPlacedCells = null;
        }

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
        isDragging = false;

        if (snapper != null && snapper.TrySnapAndPlace(transform, out var placedCells))
        {
            // successfully placed (first time or reposition)
            isPlaced = true;
            currentPlacedCells = placedCells;
            previousPlacedCells = null;
            releasedPlacedCellsThisDrag = false;

            transform.localScale = Vector3.one;
            if (blockSpawner != null)
                blockSpawner.RemoveFromPreview(this);
            return;
        }

        // failed drop: return to preview list for both initial placement and reposition attempts.
        previousPlacedCells = null;
        currentPlacedCells = null;
        isPlaced = false;
        releasedPlacedCellsThisDrag = false;
        ReturnToPreview();
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouse = Input.mousePosition;
        mouse.z = Mathf.Abs(Camera.main.transform.position.z);

        Vector3 world = Camera.main.ScreenToWorldPoint(mouse);
        world.z = 0;

        return world;
    }

    public void SetPreviewPosition(Vector3 previewPosition)
    {
        startPosition = previewPosition;
        transform.position = startPosition;
        transform.localScale = Vector3.one * idleScale;
    }

    private void ReturnToPreview()
    {
        if (blockSpawner != null && blockSpawner.ReturnToPreview(this))
            return;

        transform.position = startPosition;
        transform.localScale = Vector3.one * idleScale;
    }

    public void SetScales(float previewScale, float dragScale)
    {
        idleScale = previewScale;
        normalScale = dragScale;
        transform.localScale = Vector3.one * idleScale;
    }
}