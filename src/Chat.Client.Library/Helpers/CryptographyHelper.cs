using System.Security.Cryptography;
using System.Text;

namespace Chat.Client.Library.Helpers
{
    public static class CryptographyHelper
    {
        public static string CalculateHash(string input)
        {
            var md5 = MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var hash = md5.ComputeHash(inputBytes);

            var sb = new StringBuilder();
            foreach (var hashByte in hash)
            {
                sb.Append(hashByte.ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
