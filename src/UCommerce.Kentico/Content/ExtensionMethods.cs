using System;
using System.Text;

namespace UCommerce.Kentico.Content
{
    /// <summary>
    /// Extension methods used by the various content services in the Kentico integration.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Encodes a path in a way that makes it safe to pass to a browser client.
        /// </summary>
        /// <param name="this">The path to encode.</param>
        /// <returns>The encoded path.</returns>
        public static string EncodePath(this string @this)
        {
            var pathAsBytes = Encoding.UTF8.GetBytes(@this);
            var encodedPath = Convert.ToBase64String(pathAsBytes);

            return encodedPath;
        }

        /// <summary>
        /// Decodes a path from an encoded value.
        /// </summary>
        /// <param name="this">The encoded path to decode.</param>
        /// <returns>The decoded path.</returns>
        public static string DecodePath(this string @this)
        {
            var bytesToString = Convert.FromBase64String(@this);
            var decodedPath = Encoding.UTF8.GetString(bytesToString);

            return decodedPath;
        }
    }
}
