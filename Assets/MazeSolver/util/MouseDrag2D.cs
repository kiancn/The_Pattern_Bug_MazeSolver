using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDrag2D : MonoBehaviour
{
    [SerializeField] private Vector2 startingPosition;

    private Camera _mainCamera;

    private void OnEnable()
    {
        _mainCamera = Camera.main;
        startingPosition = transform.position;
    }
    private void Start() { OnEnable(); }

    private void OnMouseDrag()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector2 mPosWorld = _mainCamera.ScreenToWorldPoint(mousePosition);
        transform.position = mPosWorld;
    }

    public void ResetObject()
    {
        transform.position = startingPosition;
    }
}
