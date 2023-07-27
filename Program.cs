///
// -----------------------------------------------
//              ~~~ AUTHOR ~~~
// Program/Codes    -  Create by: McElie Dev
//                  -  Create Date: 15 July 2023
//                  -  Modified Date: 19 July 2023
// -----------------------------------------------
//              ~~~ OBJECTIVE: ~~~
//  This Code aims to find the Nearest Vehicle Position from a binary data file,
//  To locate the nearest position based on each of the 10 coordinates (longitude/latitude),
//  and give details about the nearest vehicle position if one is available.
///

using static System.Console;
using System;
using System.Linq;

namespace MixTelematicsAssessment
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var filename = @"D:\Source\repo\MixTelematcis\MixTelematicsAssessment\BinaryFile\VehiclePositions.dat";
            LoadDataFromFile(filename);

            ReadKey();

        }
        /// <summary>
        /// Method focus on Loading vehicle data from binary file
        /// Also Find the nearest vehicle position to the test coordinates
        /// </summary>
        /// <param name="fileName">Binary Data File</param>
        static void LoadDataFromFile(string fileName)
        {
            List<Vehicle> vehicles = BinaryDecoder.DecodeVehicleData(fileName);

            float[] cordLatitude = { 34.544909f, 32.345544f, 33.234235f, 35.195739f, 31.895839f, 32.895839f, 34.115839f, 32.335839f, 33.535339f, 32.234235f };
            float[] cordLongitude = { -102.100843f, -99.123124f, -100.214124f, -95.348899f, -97.789573f, -101.789573f, -100.225732f, -99.992232f, -94.792232f, -100.222222f };

            TreeNode tn = new();
            tn.BuildTree(vehicles);

            //Find Nearest Vehicle Position
            for (int i = 0; i <= vehicles.Count; i++)
            {
                Vehicle nearestVehicle = tn.FindNearestVehicles(cordLatitude[i], cordLongitude[i]);
                WriteLine($"Nearest Vehicle Position Details #{i + 1}: " +
                    $"VehicleID: {nearestVehicle.VehicleId}, " +
                    $"Registration: {nearestVehicle.VehicleRegistration}," +
                    $"Latitude: {nearestVehicle.Latitude}," +
                    $"Longitude: {nearestVehicle.Longitude}," +
                    $"Recorded Time UTC: {nearestVehicle.RecordedTimeUTC}");
            }

        }
    }

}