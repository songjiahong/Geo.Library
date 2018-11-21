using System;

namespace GeoLibrary.Extension
{
    internal static class DoubleExtension
    {
        public static bool AlmostEqual(this double val, double valOther, double eps = Constants.Eps)
        {
            return Math.Abs(val - valOther) < eps;
        }

        public static bool GreaterThan(this double val, double valOther, double eps = Constants.Eps)
        {
            return val > valOther + eps;
        }

        public static bool GreaterOrEqual(this double val, double valOther, double eps = Constants.Eps)
        {
            return val > valOther - eps;
        }

        public static bool LessThan(this double val, double valOther, double eps = Constants.Eps)
        {
            return val < valOther - eps;
        }

        public static bool LessOrEqual(this double val, double valOther, double eps = Constants.Eps)
        {
            return val < valOther + eps;
        }
    }
}
