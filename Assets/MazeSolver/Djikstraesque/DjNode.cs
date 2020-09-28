using System;
using System.Collections.Generic;
using UnityEngine;

namespace MazeSolver.Djikstraesque
{
    public class
        DjNode : MonoBehaviour, INode
    {
        [SerializeField] private Dictionary<DjNode, float> neighborhood; // directly reachable, unblocked nodes
        [SerializeField] private Dictionary<DjNode, float> unvisitedNeighborhood; // directly reachable, unblocked nodes
        [SerializeField] private LinkedList<DjNode> path; // LinkedList represents the journey so far, from start to goal
        [SerializeField] private int cost; // distance from starting node, because nodes are picked based on cost
        private int _originalCost; // stored original cost value, for resetting
        [SerializeField] private DjNeighborhoodChecker neighborhoodChecker;

        [SerializeField] private bool isWinningNode;

        [SerializeField] private bool isStartingNode;

        public int numberOfNeighbors;

        private void Awake() { OnEnable(); }

        private void OnEnable()
        {
            neighborhood = new Dictionary<DjNode, float>();



            if (neighborhoodChecker == null)
            {
                neighborhoodChecker = this.GetComponentInParent<DjNeighborhoodChecker>();
            }

            if (neighborhoodChecker == null) { Debug.Log("Neighborhood checker not found on Node named " + gameObject.name); }
        }

        // private void Start() { InitializeNode(); }

        public void InitializeNode()
        {
            neighborhood = neighborhoodChecker.FindNeighbors(this);
            numberOfNeighbors = neighborhood.Count;
            
            // _originalCost = cost;

            ResetUnvisitedNeighbors();

            path = new LinkedList<DjNode>();
        }

        public void ResetUnvisitedNeighbors()
        {
            unvisitedNeighborhood = new Dictionary<DjNode, float>(neighborhood);
        }

        public Dictionary<DjNode, float> Neighborhood
        {
            get => neighborhood;
            set => neighborhood = value;
        }

        public Dictionary<DjNode, float> UnvisitedNeighborhood
        {
            get => unvisitedNeighborhood;
            set => unvisitedNeighborhood = value;
        }

        public LinkedList<DjNode> Path
        {
            get => path;
            set => path = value;
        }

        public int Cost
        {
            get => cost;
            set => cost = value;
        }


        public bool IsWinningNode
        {
            get => isWinningNode;
            set => isWinningNode = value;
        }

        public bool IsStartingNode
        {
            get => isStartingNode;
            set => isStartingNode = value;
        }

        public DjNeighborhoodChecker NeighborhoodChecker
        {
            get => neighborhoodChecker;
            set => neighborhoodChecker = value;
        }
    }
}