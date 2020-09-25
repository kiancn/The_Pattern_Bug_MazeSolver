using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using UnityEngine.EventSystems;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace MazeSolver.Djikstraesque
{
    public class DjNeighborhoodChecker : MonoBehaviour, ICheckSurroundings<DjNode, float>
    {
      [SerializeField]  private float maxLinkDistance = 1.6f; // values defaults to 1.6 since that is good for me

      [SerializeField] private DjNodeField field;

      public int MaxAcceptedPrice
      {
          get => maxAcceptedPrice;
          set => maxAcceptedPrice = value;
      }

      [SerializeField] private int  maxAcceptedPrice; // max accepted price of node as neighbor 

      private void Awake()
      {
          if (field == null)
          {
              field = GetComponentInParent<DjNodeField>();
          }
      }

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

            foreach (var candidateNode in candidateNeighbors)
            {

                Vector2 candidateNodePosition = candidateNode.transform.position;
                Vector2 nodePosition = node.transform.position;

                if (candidateNode.Cost<maxAcceptedPrice)
                {
                    /* find closest winning node, for node weight */
                    
                    List<DjNode> winningNodes = new List<DjNode>();

                    if (field == null)
                    {
                        Debug.Log("Field found null on " + gameObject.name);
                    }

                    foreach (var posWinN in field.Field)
                    {
                        if(posWinN.IsWinningNode){winningNodes.Add(posWinN);} 
                    }
                    // winningNodes = field.Field.FindAll(n => n.IsWinningNode);

                    float distanceToWinningNode = Single.MaxValue;
                    
                    foreach (var wNode in winningNodes) // looking 
                    {
                        float candidateWinningNodeDistance = Vector2.Distance(candidateNodePosition, wNode.transform.position);
                        if (distanceToWinningNode > candidateWinningNodeDistance)
                        {
                            distanceToWinningNode = candidateWinningNodeDistance;
                        }                        
                    }
                    
                    /* distance to winning node plus distance between neighbor and base node */
                    foundNeighbors.Add(candidateNode,distanceToWinningNode + Vector2.Distance(nodePosition, candidateNodePosition));
                }


            }

            return foundNeighbors;
        }
    }
}