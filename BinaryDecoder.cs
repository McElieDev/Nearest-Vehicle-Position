using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixTelematicsAssessment
{
    /// <summary>
    ///  BinaryDecoder class focus on Reading the Binary file (.dat),
    ///  With the help of ReadNullTerminatedString method to read the null-terminated
    /// </summary>
    public class BinaryDecoder
    {
        public static List<Vehicle> DecodeVehicleData(string filePath)
        {
            List<Vehicle> vehicles = new List<Vehicle>();

            using BinaryReader reader = new(File.Open(filePath, FileMode.Open));
            int count = 0;
            while (reader.BaseStream.Position < reader.BaseStream.Length && ++count < 15)
            {
                reader.BaseStream.Seek(0, SeekOrigin.Begin);
                Vehicle vehicle = new(
                    reader.ReadInt32(),
                    ReadNullTerminatedString(reader),//reader.ReadString(),
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadUInt64()
                );

                vehicles.Add(vehicle);
            }

            return vehicles;
        }

        /// <summary>
        /// This method correctly read the null-terminated ASCII strings from the binary file
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static string ReadNullTerminatedString(BinaryReader reader)
        {
            List<byte> bytes = new List<byte>();
            byte b;
            while ((b = reader.ReadByte()) != 0) { bytes.Add(b); }
            return Encoding.ASCII.GetString(bytes.ToArray());
        }
    }
}
