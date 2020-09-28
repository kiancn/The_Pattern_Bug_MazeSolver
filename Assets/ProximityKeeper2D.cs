using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityKeeper2D : MonoBehaviour
{
    [SerializeField] private float searchRadius = 0.25f;

    /// <summary>
    /// represents distance per second to move away from too-close object
    /// </summary>
    [SerializeField] float movementSpeed = 2.3f; // movement;  

    [SerializeField] private int gracePeriod = 5;
    [SerializeField] private int currentIntrusionCount = 0;


    private BoxCollider2D _collider2D;
    private Transform _transform;


    // Start is called before the first frame update
    void Start() { OnEnable(); }


    private void OnEnable()
    {
        _transform = transform;
        _collider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Collider2D[] invaders = new Collider2D[21];

        Physics2D.OverlapCircleNonAlloc(_transform.position, searchRadius, invaders);

        if (invaders[0] == null)
        {
            currentIntrusionCount = 0;
            return;
        }

        currentIntrusionCount++;

        if (currentIntrusionCount > gracePeriod)
        {
            for (int j = -1; ++j < invaders.Length && invaders[j] != null;)
            {
                if (invaders[j] != _collider2D)
                {
                    _transform.position = Vector2.MoveTowards(
                        _transform.position,
                        invaders[j].transform.position,
                        -(movementSpeed * Time.deltaTime)/(invaders.Length>>1));
                }
            }
        }
    }

    public void Move(Vector2 direction)
    {
        var _transform = this.transform;
        _transform.position = (Vector2) (_transform.position) + direction * (Time.deltaTime * movementSpeed);
    }
}