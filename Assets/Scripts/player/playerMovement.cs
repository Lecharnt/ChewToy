using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class playerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of the player movement animation
    public Vector3Int gridPosition; // Current grid position
    public Grid grid; // Reference to the Unity Grid component

    private bool isMoving = false;
    private Vector3 targetPosition;
    private Vector3 prevPosition;
    public Vector3 offset = new Vector3(.5f, .5f, 0f);
    private bool lineOfSight;
    public static event Action OnPlayerMoved;
    private void Start()
    {
        // Initialize grid position based on starting position
        if (grid == null)
        {
            Debug.LogError("Grid reference is missing!");
            return;
        }

        Vector3Int cellPosition = grid.WorldToCell(transform.position);
        gridPosition = cellPosition;
        SnapToGrid();
    }

    private void Update()
    {
        if (isMoving) return;

        Vector2Int rayDirection = Vector2Int.zero;
        Vector3Int playerDirection = Vector3Int.zero;

        if (Input.GetKeyDown(KeyCode.W)) rayDirection = Vector2Int.up;
        if (Input.GetKeyDown(KeyCode.S)) rayDirection = Vector2Int.down;
        if (Input.GetKeyDown(KeyCode.A)) rayDirection = Vector2Int.left;
        if (Input.GetKeyDown(KeyCode.D)) rayDirection = Vector2Int.right;

        if (Input.GetKeyDown(KeyCode.W)) playerDirection = Vector3Int.right;
        if (Input.GetKeyDown(KeyCode.S)) playerDirection = Vector3Int.left;
        if (Input.GetKeyDown(KeyCode.A)) playerDirection = Vector3Int.down;
        if (Input.GetKeyDown(KeyCode.D)) playerDirection = Vector3Int.up;

        if (rayDirection != Vector2Int.zero)
        {
            MovePlayer(rayDirection, playerDirection);
        }
    }

    public void MovePlayer(Vector2Int rayDirection, Vector3Int playerDirection)
    {
        OnPlayerMoved?.Invoke();
        prevPosition = targetPosition;
        int layerMask = ~LayerMask.GetMask("player");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, 1f, layerMask);
        if (hit)
        {
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.CompareTag("item"))
                {
                    Destroy(hit.collider.gameObject);
                }
                if (hit.collider.gameObject.tag == "ArtPla")
                {
                    Destroy(hit.collider.gameObject);
                    Debug.DrawRay(transform.position, (Vector3Int)rayDirection, Color.green, 1);
                }
                if (hit.collider.gameObject.CompareTag("door"))
                {
                    if (!hit.collider.gameObject.GetComponent<TpDoor>().locked)
                    {
                        transform.position = hit.collider.gameObject.GetComponent<TpDoor>().exit.transform.position;
                        Vector3Int cellPosition = grid.WorldToCell(transform.position);
                        gridPosition = cellPosition;
                        SnapToGrid();
                    }
                    else
                    {
                        GameManager.Instance.DiologeScript.SetTextTypewriter("Door is locked!");
                    }
                }
                if (hit.collider.gameObject.CompareTag("human"))
                {
                    GameManager.Instance.DiologeScript.SetTextTypewriter("You Killed Her!");
                    //GameManager.Instance.FireSpreaderManager.StartFire(grid.WorldToCell(transform.position));
                    Destroy(hit.collider.gameObject);
                    //GameManager.Instance.winScean();
                }
                if (hit.collider.gameObject.CompareTag("humanLos"))
                {
                    gridPosition += playerDirection;


                    targetPosition = grid.CellToWorld(new Vector3Int(gridPosition.x, gridPosition.y, 0));
                    Debug.DrawRay(transform.position, (Vector3Int)rayDirection, Color.red, 1);
                    StartCoroutine(SmoothMove(transform.position, targetPosition + offset));
                }
            }

        }
        else
        {
            gridPosition += playerDirection;


            targetPosition = grid.CellToWorld(new Vector3Int(gridPosition.x, gridPosition.y, 0));
            Debug.DrawRay(transform.position, (Vector3Int)rayDirection, Color.red, 1);
            StartCoroutine(SmoothMove(transform.position, targetPosition + offset));
        }
    }

    private void SnapToGrid()
    {
        transform.position = grid.CellToWorld(gridPosition) + offset;
    }

    private IEnumerator SmoothMove(Vector3 start, Vector3 end)
    {
        isMoving = true;
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            transform.position = Vector3.Lerp(start, end, elapsedTime);
            elapsedTime += Time.deltaTime * moveSpeed;
            yield return null;
        }

        transform.position = end;
        isMoving = false;
    }
}