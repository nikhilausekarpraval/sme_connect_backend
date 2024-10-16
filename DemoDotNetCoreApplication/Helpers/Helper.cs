using System.Text;
using System.Security.Cryptography;

namespace DemoDotNetCoreApplication.Helpers
{
    public class Helper
    {
        public static byte[] HashString(string answer)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(answer));
            }
        }

    }
}
