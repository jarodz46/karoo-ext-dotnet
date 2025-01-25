using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karooext.dotnet.Classes
{
    public class PolylineDecoder
    {
        public static List<(double Latitude, double Longitude)> DecodePolyline(string encodedPolyline)
        {
            var polyline = new List<(double Latitude, double Longitude)>();
            int index = 0, currentLatitude = 0, currentLongitude = 0;

            while (index < encodedPolyline.Length)
            {
                // Decode latitude
                int latitudeChange = DecodeNextValue(encodedPolyline, ref index);
                currentLatitude += latitudeChange;

                // Decode longitude
                int longitudeChange = DecodeNextValue(encodedPolyline, ref index);
                currentLongitude += longitudeChange;

                // Convert to coordinates
                double latitude = currentLatitude / 1e5;
                double longitude = currentLongitude / 1e5;

                polyline.Add((latitude, longitude));
            }

            return polyline;
        }

        private static int DecodeNextValue(string encodedPolyline, ref int index)
        {
            int result = 0, shift = 0, b;

            do
            {
                b = encodedPolyline[index++] - 63;
                result |= (b & 0x1F) << shift;
                shift += 5;
            } while (b >= 0x20);

            // Apply the sign and return
            return (result & 1) != 0 ? ~(result >> 1) : (result >> 1);
        }
    }

}
