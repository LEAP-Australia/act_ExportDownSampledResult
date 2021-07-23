using System;

namespace Utility.Wrappertest
{
    class Program
    {
        static void Main(string[] args)
        {
            var points = new Utility.Wrapper.MeshNode[3];
            points[0].node_id = 0;
            points[0].x = 0.0;

            points[1].node_id = 10;
            points[1].x = 50.0;

            points[2].node_id = 20;
            points[2].x = 20.0;

            var value = Utility.Wrapper.Downsample.Points(
                points,
                3,
                0.2);

            foreach(var v in value)
            {
                Console.WriteLine($"Roof = {v}");
            }
        }
    }
}
