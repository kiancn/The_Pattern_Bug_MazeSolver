using System;
using System.Collections.Generic;
using UnityEngine;

namespace MazeSolver.Djikstraesque
{
    
    public class DjNodeField : MonoBehaviour
    {
       [SerializeField] private List<DjNode> field;

       [SerializeField] private bool initializeOnlyChildNodes;

       public void InitializeField()
       {
           DjNode[] nodes = new DjNode[1];

           if (!initializeOnlyChildNodes) { nodes = this.GetComponents<DjNode>(); }
           else { nodes = FindObjectsOfType<DjNode>(); }

           field = new List<DjNode>(nodes);

           foreach (var node in field)
           {
               node.InitializeNode();
           }
       }

       public void ReInitializeUnvisitedNeighbors()
       {
           foreach (var node in field)
           {
               node.ResetUnvisitedNeighbors();
           }
       }


       private void OnDrawGizmos()
       {
           if (field != null)
           {
               foreach (var node in field)
               {
                   Vector3 nodePosition = node.transform.position;
                   
                   foreach (var neighbor in node.Neighborhood)
                   {
                       Gizmos.DrawLine(nodePosition, neighbor.Key.transform.position);
                   }
               }
           }
       }

       public List<DjNode> Field
       {
           get => field;
           set => field = value;
       }

       public bool InitializeOnlyChildNodes
       {
           get => initializeOnlyChildNodes;
           set => initializeOnlyChildNodes = value;
       }
    }
    
}