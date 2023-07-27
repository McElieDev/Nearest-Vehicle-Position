using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixTelematicsAssessment
{
    /// <summary>
    /// This class is focus on the calculation that required to find the nearest vehicles
    /// 
    /// The Builtree method recursively constructs a balanced binary tree by sorting 
    /// and partitioning the vehicle data along alternating dimensions 
    /// (latitude and longitude) at each leve
    /// 
    /// The CalculateDistance Method involve to compute the distances between 
    /// latitude and longitude coordinates
    /// </summary>
    public class TreeNode
    {
        Node root;
        public class Node
        {
            public Vehicle Vehicle { get; set; }
            public Node Left { get; set; }
            public Node Right { get; set; }
        }

        public void BuildTree(List<Vehicle> vehicles)
        {
            root = new Node();
            root = BuildSubTree(vehicles, 0, vehicles.Count - 1, 0);
        }

        private Node BuildSubTree(List<Vehicle> vehicles, int start, int end, int depth)
        {
            //return null if line ends
            if (start > end) { return null; }
            int axis = depth % 2;

            int mid = (start + end) / 2;

            vehicles.Sort(axis switch
            {
                0 => (a, b) => a.Latitude.CompareTo(b.Latitude),
                _ => (a, b) => a.Longitude.CompareTo(b.Longitude)
            });

            Node node = new()
            {
                Vehicle = vehicles[mid],
                Left = BuildSubTree(vehicles, start, mid - 1, depth + 1),
                Right = BuildSubTree(vehicles, mid + 1, end, depth + 1)
            };

            return node;
        }

        public Vehicle FindNearestVehicles(float targetLatitude, float targetLongitude)
        {
            Vehicle nearest = null;
            double minDistance = double.MaxValue;
            FindNearestVehicles(root, targetLatitude, targetLongitude, 0, ref nearest, ref minDistance);
            return nearest;
        }

        private void FindNearestVehicles(Node node, float targetLatitude, float targetLongitude, int depth, ref Vehicle nearest, ref double minDistance)
        {
            if (node == null)
                return;

            double distance = CalculateDistance(node.Vehicle.Latitude, node.Vehicle.Longitude, targetLatitude, targetLongitude);

            if (distance < minDistance)
            {
                nearest = node.Vehicle;
                minDistance = distance;
            }

            int axis = depth % 2;
            if (axis == 0)
            {
                FindNearestVehicles(targetLatitude < node.Vehicle.Latitude ? node.Left : node.Right, targetLatitude, targetLongitude, depth + 1, ref nearest, ref minDistance);
            }
            else
            {
                FindNearestVehicles(targetLongitude < node.Vehicle.Longitude ? node.Left : node.Right, targetLatitude, targetLongitude, depth + 1, ref nearest, ref minDistance);
            }

            double axisDistance = axis == 0 ? Math.Abs(targetLatitude - node.Vehicle.Latitude) : Math.Abs(targetLongitude - node.Vehicle.Longitude);

            //if (axisDistance < minDistance)
            //{
            //    FindNearestVehicles(axis == 0 ? targetLatitude >= node.Vehicle.Latitude ? node.Left : node.Right
            //                                   : targetLongitude >= node.Vehicle.Longitude ? node.Left : node.Right,
            //                        targetLatitude, targetLongitude, depth + 1, ref nearest, ref minDistance);
            //}
            if (axisDistance < minDistance)
            {
                if (axis == 0)
                {
                    if (targetLatitude >= node.Vehicle.Latitude)
                        FindNearestVehicles(node.Left, targetLatitude, targetLongitude, depth + 1, ref nearest, ref minDistance);
                    else
                        FindNearestVehicles(node.Right, targetLatitude, targetLongitude, depth + 1, ref nearest, ref minDistance);
                }
                else
                {
                    if (targetLongitude >= node.Vehicle.Longitude)
                        FindNearestVehicles(node.Left, targetLatitude, targetLongitude, depth + 1, ref nearest, ref minDistance);
                    else
                        FindNearestVehicles(node.Right, targetLatitude, targetLongitude, depth + 1, ref nearest, ref minDistance);
                }
            }
        }

        private double CalculateDistance(float lat1, float lon1, float lat2, float lon2)
        {
            const double R = 6371; // Earth's radius in kilometers

            double dLat = ToRadians(lat2 - lat1);
            double dLon = ToRadians(lon2 - lon1);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c;
        }

        private double ToRadians(double angle)
        {
            return Math.PI * angle / 180.0;
        }
    }
}
