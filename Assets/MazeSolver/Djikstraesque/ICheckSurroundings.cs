using System.Collections.Generic;

namespace MazeSolver.Djikstraesque
{
    public interface ICheckSurroundings<TNode,TCost>
    {
        Dictionary<TNode,TCost> FindNeighbors(TNode baseNode);
    }
}