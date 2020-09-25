using UnityEngine;

namespace MazeSolver.util
{
   public class NodeSpawner2D : MonoBehaviour
   {
      [SerializeField] private GameObject nodePrefab;

      [SerializeField] private Transform transformToParentTo;

      [SerializeField] private Transform spawnPositionTransform;

      private void OnMouseDown()
      {
         Debug.Log("Moose detected!");
         
         GameObject nodeTemplate = Instantiate(nodePrefab, new Vector2(-100, -100), Quaternion.identity);

         GameObject newNode = Instantiate(nodeTemplate, spawnPositionTransform.position, Quaternion.identity, transformToParentTo);
         
         Destroy(nodeTemplate);
      }
   }
}
 