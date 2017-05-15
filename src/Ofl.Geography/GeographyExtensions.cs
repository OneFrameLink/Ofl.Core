using System;

namespace Ofl.Geography
{
    //////////////////////////////////////////////////
    ///
    /// <author>Nicholas Paldino</author>
    /// <created>2014-01-01</created>
    /// <summary>Extensions for working with geographic data.</summary>
    ///
    //////////////////////////////////////////////////
    public static class GeographyExtensions
    {
        #region Static, read-only state.

        /// <summary>The constant for kilomoeters when determining the distance
        /// between two points of latitude and longitude.</summary>
        private const double KilometerConstant = 6371;

        #endregion

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2014-01-01</created>
        /// <summary>Gets the distaince between two points in latitude
        /// and longitude.</summary>
        /// <param name="firstLatitude">The latitude of the first point.</param>
        /// <param name="firstLongitude">The longitude of the first point.</param>
        /// <param name="secondLatitude">The latitude of the second point.</param>
        /// <param name="secondLongitude">The longitude of the second point.</param>
        /// <returns>The distance between the latitude and longitude, in kilometers.</returns>
        ///
        //////////////////////////////////////////////////
        public static double GetSphericalLawOfCosinesDistance(double firstLatitude, double firstLongitude,
            double secondLatitude, double secondLongitude)
        {
            // Calculate the distance.
            double distance = Math.Acos((Math.Sin(firstLatitude) * Math.Sin(secondLatitude)) +
                (Math.Cos(firstLatitude) * Math.Cos(secondLatitude) *
                Math.Cos(secondLongitude - firstLongitude))) * KilometerConstant;

            // Return the distance.
            return distance;
        }
    }
}
