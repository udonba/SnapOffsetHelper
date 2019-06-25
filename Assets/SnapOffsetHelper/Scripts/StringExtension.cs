using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Udonba.SnapOffsetHelper
{
    public static class StringExtension
    {
        private const char UnitySeparatoChar = '/';

        /// <summary>
        /// Replace DirectorySeparatorChar
        /// </summary>
        /// <param name="value"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static string ReplaceSeparator(this string value, ReplaceOrder order)
        {
            if (order == ReplaceOrder.UnityToSystem)
            {
                return value.Replace(UnitySeparatoChar, System.IO.Path.DirectorySeparatorChar);
            }
            else
            {
                return value.Replace(System.IO.Path.DirectorySeparatorChar, UnitySeparatoChar);
            }
        }
    }

    public enum ReplaceOrder
    {
        /// <summary>
        /// '/' -> System.IO.Path.DirectorySeparatorChar
        /// </summary>
        UnityToSystem = 0,
        /// <summary>
        /// System.IO.Path.DirectorySeparatorChar -> '/'
        /// </summary>
        SystemToUnity = 1
    }
}
