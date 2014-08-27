using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AStar
{
    public interface IHasNeighbours<N>
    {
        IEnumerable<N> Neighbours { get; }
    }
}
