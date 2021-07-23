#include <vector>
#include <iostream>
#include <cstdlib>

#include <CGAL/Exact_predicates_inexact_constructions_kernel.h>
#include <CGAL/hierarchy_simplify_point_set.h>


namespace utilities {

using Kernel = CGAL::Exact_predicates_inexact_constructions_kernel;
using Point = Kernel::Point_3;

struct MeshNode
{
    int node_id;
    double x{};
    double y{};
    double z{};
};

class PointWithId : public Kernel::Point_3
{
  public:
    int node_id{};

    PointWithId(double x, double y, double z, int id) : Kernel::Point_3(x, y, z)
    {
        node_id = id;
    }
};
}// namespace utilities

extern "C" __declspec(dllexport) void downsample_points(
    const utilities::MeshNode *nodes,
    const int mesh_size,
    const int cluster_size,
    const double mav_var,
    std::int32_t *(samples_node_ids[]),
    std::size_t *samples_node_ids_count)
{
    auto points = std::vector<utilities::PointWithId>();
    points.reserve(mesh_size);

    for (int idx = 0; idx < mesh_size; ++idx) {
        points.emplace_back(utilities::PointWithId(nodes[idx].x, nodes[idx].y, nodes[idx].z, nodes[idx].node_id));
    }

    auto eraseIter = CGAL::hierarchy_simplify_point_set(
        points,
        CGAL::parameters::size(cluster_size)// Max cluster size
            .maximum_variation(mav_var));

    points.erase(
        eraseIter,// Max surface variation
        points.end());

    *samples_node_ids_count = points.size();
    *samples_node_ids = static_cast<int32_t*>(std::calloc(points.size(), sizeof(std::int32_t)));


    for(std::size_t idx = 0; idx < points.size(); ++idx)
    {
        (*samples_node_ids)[idx] = points[idx].node_id;
    }
}