using System;
using UnityEngine;

namespace MazeSolver.Djikstraesque
{
    public class NodeModeSwitcher : MonoBehaviour
    {
        [Serializable]
        public enum NodeMode
        {
            NORMAL,
            STARTING,
            WINNING,
            BLOCKED
        }

        [SerializeField] private NodeMode mode = NodeMode.NORMAL;

        [SerializeField] private Sprite normalNodeSprite;
        [SerializeField] private Sprite blockedNodeSprite;
        [SerializeField] private Sprite startingNodeSprite;
        [SerializeField] private Sprite winningNodeSprite;

        [SerializeField] private SpriteRenderer renderer;
        [SerializeField] private DjNode node;

        // when node is switched to be a 'blocking' node
        [SerializeField] private int originalNodeCost;
        [SerializeField] private int originalNodeMaxLinkCost;

        [SerializeField] private float maximumDoubleClickTimeDifference;
        [SerializeField] private float timeAtLastClick;

        // [SerializeField] private int 

        public void Initialize()
        {
            renderer = GetComponent<SpriteRenderer>();
            node = GetComponent<DjNode>();

            originalNodeCost = node.Cost;
            originalNodeMaxLinkCost = node.NeighborhoodChecker.MaxAcceptedPrice;

            SwitchMode(mode);
        }

        // Start is called before the first frame update
        void Start() { Initialize(); }

        private void OnMouseDown()
        {
            float timeNow = Time.time;
            float timeDifference = timeNow - timeAtLastClick;

            if (timeDifference < maximumDoubleClickTimeDifference)
            {
                SwitchMode();
            }

            timeAtLastClick = Time.time;
        }
        
        /* Decides what new mode will be */
        private void SwitchMode()
        {
            switch (mode)
            {
                case NodeMode.NORMAL:
                    SwitchMode(NodeMode.BLOCKED);
                    break;
                case NodeMode.BLOCKED:
                    SwitchMode(NodeMode.STARTING);
                    break;
                case NodeMode.STARTING:
                    SwitchMode(NodeMode.WINNING);
                    break;
                case NodeMode.WINNING:
                    SwitchMode(NodeMode.NORMAL);
                    break;
            }
        }
        /* Adjusts values to match new mode and sets sprite for mode */
        private void SwitchMode(NodeMode modeToChangeTo)
        {
            mode = modeToChangeTo; // variable set to now current mode     
            AdjustNodeValues(modeToChangeTo);
            SetSpriteForNewMode(modeToChangeTo);
        }

        /* Sets sprite of GO to match the new mode */
        private void SetSpriteForNewMode(NodeMode modeToChangeTo)
        {
            switch (modeToChangeTo) /* CHANGE GRAPHIC */
            {
                case NodeMode.NORMAL:
                    renderer.sprite = normalNodeSprite;
                    break;
                case NodeMode.BLOCKED:
                    renderer.sprite = blockedNodeSprite;
                    break;
                case NodeMode.STARTING:
                    renderer.sprite = startingNodeSprite;
                    break;
                case NodeMode.WINNING:
                    renderer.sprite = winningNodeSprite;
                    break;
            }
        }
        /* Adjusts values internal to the node */
        private void AdjustNodeValues(NodeMode modeToChangeTo)
        {

            
            /* deciding if node is starting or winning node now */
            node.IsStartingNode = (modeToChangeTo == NodeMode.STARTING);
            node.IsWinningNode = (modeToChangeTo == NodeMode.WINNING);

            if (modeToChangeTo == NodeMode.BLOCKED) /* node cost adjusted depending of change */
            {
                node.Cost = Int32.MaxValue; // means that a node contacting the block will get that price back
                node.NeighborhoodChecker.MaxAcceptedPrice = 0; // means that node creates no links over the price 0, so no links
            }
            else /* reversing 'blocked' values */
            {
                node.Cost = originalNodeCost;
                node.NeighborhoodChecker.MaxAcceptedPrice = originalNodeMaxLinkCost;
            }
        }
    }
}