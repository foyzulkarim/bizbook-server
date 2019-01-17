using System;

namespace Utility
{
    public static class Extensions
    {
        public static bool IdIsOk(this string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return false;
            }
            Guid guid;
            if (!Guid.TryParse(id, out guid))
            {
                return false;
            }
            if (guid == new Guid())
            {
                return false;
            }
            return true;
        }

        public static double ToDecimal(this double input)
        {
            return Math.Round(input, 2, MidpointRounding.AwayFromZero);
        }
    }


}
