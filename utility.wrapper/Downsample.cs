using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Utility.Wrapper
{

    [StructLayout(LayoutKind.Sequential)]
    public struct MeshNode
    {
        public int node_id;
        public double x;
        public double y;
        public double z;

        public MeshNode(int nodeid, double X, double Y, double Z)
        {
            node_id = nodeid;
            x = X;
            y = Y;
            z = Z;
        }
    }

    public static class Downsample
    {
        [DllImport("utility.downsample.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void downsample_points(
        [In, Out] MeshNode[] nodes,
        int meshSize,
        int clusterSize,
        double maxVar,
        ref IntPtr samplesNodeIds,
        ref UIntPtr samplesNodeIdsCount
        );

        public static int[] Points(
            IEnumerable<MeshNode> meshNodes,
            int clusterSize,
            double maxVar)
        {
            IntPtr buffer = IntPtr.Zero;
            var bufferSize = UIntPtr.Zero;
            downsample_points(
                meshNodes.ToArray(),
                meshNodes.Count(),
                clusterSize,
                maxVar,
                ref buffer,
                ref bufferSize);            
            if(bufferSize.ToUInt64() > 0) {
                var nodeIds = new Int32[(int)bufferSize];
                Marshal.Copy(buffer, nodeIds, 0, (int)bufferSize);
                return nodeIds;
            }
            throw new ApplicationException("Error in sampling. Final size is 0.");
        }
    }
}
