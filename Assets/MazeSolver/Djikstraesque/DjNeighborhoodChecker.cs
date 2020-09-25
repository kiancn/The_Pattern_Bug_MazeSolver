using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.EventSystems;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace MazeSolver.Djikstraesque
{
    public class DjNeighborhoodChecker : MonoBehaviour, ICheckSurroundings<DjNode, float>
    {
      [SerializeField]  private float maxLinkDistance = 1.6f; // values defaults to 1.6 since that is good for me

      // [SerializeField] private DjNodeField field;

      public int MaxAcceptedPrice
      {
          get => maxAcceptedPrice;
          set => maxAcceptedPrice = value;
      }

      [SerializeField] private int  maxAcceptedPrice; // max accepted price of node as neighbor 
      
        private void Awake() { }

        public Dictionary<DjNode, float> FindNeighbors(DjNode baseNode)
        {
            List<DjNode> candidateNeighbors = CheckForNeighborCandidates(baseNode);

            Dictionary<DjNode, float> foundNeighbors = CreateNeighborDictionary(candidateNeighbors, baseNode);

            return foundNeighbors;
        }

        private List<DjNode> CheckForNeighborCandidates(DjNode node)
        {
            List<DjNode> candidateNeighbors = new List<DjNode>();

            Collider2D[] nearbyColliders = new Collider2D[20]; // probably this number will never be superseded
            var size = Physics2D.OverlapCircleNonAlloc(node.transform.position, maxLinkDistance, nearbyColliders);

            for (int i = 0; i < size; i++)
            {
                DjNode possibleNode = nearbyColliders[i].GetComponent<DjNode>();
                /* if node is the same or it wasn't a node (null), discard the find */
                if (possibleNode == null || possibleNode == node) { continue; }

                candidateNeighbors.Add(possibleNode); // now verified a node
            }

            return candidateNeighbors;
        }

        private Dictionary<DjNode, float> CreateNeighborDictionary(List<DjNode> candidateNeighbors,DjNode node)
        {
            Dictionary<DjNode, float> foundNeighbors = new Dictionary<DjNode, float>();
            DjNode hitNode = null;
            
            foreach (var candidateNode in candidateNeighbors)
            {
                // // 2D raycast from base node to immediately examined node n
                // // IF ray returns a collider with Obstacle attached, the 
                // // candidate node n was NOT a neighbor, else it is.
                //
                Vector2 candidateNodePosition = candidateNode.transform.position;
                Vector2 nodePosition = node.transform.position;
                //
                // Vector2 difference = (nodePosition - candidateNodePosition);
                // Vector2 direction = difference / difference.x;
                //
                // // RaycastHit2D hit = Physics2D.Raycast(candidateNodePosition, direction);
                // RaycastHit2D hit = Physics2D.Linecast(nodePosition,candidateNodePosition);
                //
                //
                // if (hit.collider != null)
                // {
                //     Obstacle obstacle = hit.collider.GetComponent<Obstacle>();
                //     
                //     /* we hit an obstacle instead of a node, so n and node are not neighbors */
                //     if (obstacle != null)
                //     {
                //         Debug.Log(obstacle.name + " found between "  + node.name + " and " + candidateNode.name );
                //         continue; // obstacles could have interesting effects and need not just be 'walls', but are such now
                //     }
                //     
                //     // Debug.Log("No obstacle found between " + node.name + " and " + hit.collider.name );
                //
                //     hitNode = hit.collider.GetComponent<DjNode>();
                //
                //     if (hitNode != null)
                //     {
                //         Debug.Log("DjNode detected: " + hitNode.name + " with hitNode.Cost >= maxAcceptedPrice being " + (hitNode.Cost <= maxAcceptedPrice).ToString());
                //         if (hitNode.Cost >= maxAcceptedPrice) { hitNode = null; }
                //     }
                // }

                if (candidateNode.Cost<maxAcceptedPrice)
                {
                    foundNeighbors.Add(candidateNode, Vector2.Distance(nodePosition, candidateNodePosition));
                }

                hitNode = null;
            }

            return foundNeighbors;
        }
    }
}