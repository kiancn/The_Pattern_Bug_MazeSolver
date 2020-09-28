using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;

/// <summary>
/// Class instance is able to intantiate a field of GameObjects based on two demands and a circumstance.
/// You get to demand:
/// 1. separation of each cell from another, expressed as a Vector3
/// 2. create n-size flat fields of cells, expressed as a ... nah, its just gonna be 3d
///
/// KCN
/// FieldSpawner2D Script 2D SpriteRenderer version
/// </summary>
public class FieldSpawner2D : MonoBehaviour
{
    // gameobject to repeat
    [SerializeField] private GameObject prefabGameObject;
    [SerializeField] private string memberIdentifier = "Cell";
    [SerializeField, Header("Scale of field; X*X*X. Supply no zero."), Range(1, 3300), Tooltip("Number of objects in each dimension")]
    private int xScale;

    [SerializeField, Range(1, 3300)] private int yScale;

    [SerializeField, Range(1, 3300)] private int zScale;


    // spawning demands
    [Header("Percentage separation between instances of prefab"),
     Tooltip("Numbers represents the percentage sepatation between on instance and the next.")]
    [SerializeField, Range(100, 200)]
    private float sepX;

    [SerializeField, Range(100, 200)] private float sepY;
    [SerializeField, Range(100, 200)] private float sepZ;

    [Header("Mesh bounds size:"), SerializeField]
    private float xSize;

    public float XSize
    {
        get => xSize;
        set => xSize = value;
    }

    public float YSize
    {
        get => ySize;
        set => ySize = value;
    }

    public float ZSize
    {
        get => zSize;
        set => zSize = value;
    }

    [SerializeField] private float ySize;
    [SerializeField] private float zSize;

    [FormerlySerializedAs("xAdjustedSeparation")] [Header("Per instance seperation:"), SerializeField]
    private float xAbsoluteSeparation;

    public float XAbsoluteSeparation
    {
        get => xAbsoluteSeparation;
        set => xAbsoluteSeparation = value;
    }

    [FormerlySerializedAs("yAdjustedSeparation")] [SerializeField] private float yAbsoluteSeparation;
    [FormerlySerializedAs("zAdjustedSeparation")] [SerializeField] private float zAbsoluteSeparation;

    [SerializeField] private bool adjustYOnlyPerLine = true; // when true, y coordinate of next prefab will only update
    // when a new line is created.

    /// <summary>
    /// Spawned objects will be parented to supplied GameObject (become part of its sub-hierarchy).
    /// Defaults to GameObject carrying this script
    /// </summary>
    [SerializeField] private GameObject parent;
    /// <summary>
    /// Position of this transform will be the zero-point offset when placing requested field objects
    /// Defaults to parent
    /// </summary>
    [SerializeField] private Transform zeroPointTransform;

    private SpriteRenderer renderer; // renderer was changed from standard version of script, MeshRenderer ->> SpriteRenderer

    /// <summary>
    /// At awake, the specifics necessary to create a field of uniformly separated objects.
    /// Supplied prefabGameObject is used for these calculations.
    /// </summary>
    private void Awake()
    {
        if (InitializeData())
        {
            Debug.Log("Field Spawner initialized.");
        }
        else
        {
            Debug.Log("Could not initialize FieldSpawner2D.");
        }
    }

    /// <summary>
    /// Method returns true if it was possible to initialize object.
    /// </summary>
    /// <returns></returns>
    public bool InitializeData()
    {
        /// creating an instance of prefab supplied & dereferencing the supplied prefab in favor of that fresh GameObject instance
        prefabGameObject = Instantiate(prefabGameObject, new Vector3(-100, -100, -100), Quaternion.identity);

        // if no gameobject is supplied, there is nothing to do.
        if (prefabGameObject == null) { return false; }

        renderer = prefabGameObject.GetComponent<SpriteRenderer>(); // 
        if (renderer == null) { return false; }

        // if no parent is supplied, parent defaults to the carrier gameobject
        if (parent == null) { parent = this.gameObject; }

        // and zero-p offset to parent
        if (zeroPointTransform == null) { zeroPointTransform = parent.transform;}

        Bounds meshBounds = renderer.bounds;

        xSize = meshBounds.size.x;
        ySize = meshBounds.size.y;
        zSize = meshBounds.size.z;

        // NOTE: This blocked section is the only effect of versioning this script from 3D to 2D 
        // renderer = prefabGameObject.GetComponent<MeshRenderer>();
        // if (renderer == null) { return false; }
        //
        // // if no parent is supplied, parent defaults to the carrier gameobject
        // if (parent == null) { parent = this.gameObject; }
        //
        // Bounds meshBounds = renderer.bounds;
        //
        // xSize = meshBounds.size.x;
        // ySize = meshBounds.size.y;
        // zSize = meshBounds.size.z;
        //
        if (xScale == 0) { Debug.Log("From FieldSpawner2D: Mesh Bounds are no good"); }

        xAbsoluteSeparation = (xSize / 100) * sepX;
        yAbsoluteSeparation = (ySize / 100) * sepY;
        zAbsoluteSeparation = (zSize / 100) * sepZ;

        //prefabGameObject.SetActive(false);

        return true;
    }

    public void StartSpawnField() { StartCoroutine(SpawnField());}

    public IEnumerator SpawnField()
    {
        Vector3 basePosition = zeroPointTransform.transform.position;

        GameObject[,,] matrixOfGameObjects = new GameObject[xScale, yScale, zScale];

        float currentXOffset;
        float currentYOffset;
        float currentZOffset;

        // per number of rounds: zScale
        for (int z = 0; z < zScale; z++)
        {
            currentZOffset = basePosition.z + zAbsoluteSeparation * z;

            for (int y = 0; y < yScale; y++)
            {
                currentYOffset = basePosition.y + yAbsoluteSeparation * y;

                for (int x = 0; x < xScale; x++)
                {
                    currentXOffset = basePosition.x + xAbsoluteSeparation * x;

                    matrixOfGameObjects[x, y, z] =
                        SpawnInstance(currentXOffset, currentYOffset, currentZOffset, x, y, z);
                    yield return new WaitForEndOfFrame();
                }
            }
        }

    }

    private GameObject SpawnInstance(float x, float y, float z, int orderX, int orderY, int orderZ)
    {
        GameObject newFieldMember = Instantiate(prefabGameObject, new Vector3(x, y, z), zeroPointTransform.rotation, parent.transform);
        newFieldMember.name = memberIdentifier + " x: " + orderX + " y: " + orderY + " z: " + orderZ;
        return newFieldMember;
    }
}