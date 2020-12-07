using System;
using System.Collections.Generic;
using System.Text;

namespace Planeminator.Algorithm.DataStructures
{
    internal interface IPackageGenerator
    {
        List<AlgorithmPackage> NextPool();
    }
}
