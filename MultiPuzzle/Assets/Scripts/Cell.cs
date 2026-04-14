using UnityEngine;

public class Cell
{
    public Vector2Int Position {get; private set;}
    public CellState State {get; private set;}
    public Cell (Vector2Int position, CellState state) 
    {
        Position = position;
        State = state;
    }

    public void SetState (CellState newState)
    {
        State = newState;
    }

    public bool isEmpty() => State == CellState.EMPTY;
    public bool isShape() => State == CellState.SHAPE;
    public bool isFilled() => State == CellState.FILLED;
}
