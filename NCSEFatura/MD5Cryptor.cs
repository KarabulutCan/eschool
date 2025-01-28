using System.Text;

namespace NCSEFatura
{
    internal class MD5Cryptor
    {
        private static MD5 Create()
        {
            return new MD5();
        }

        public static string GetHashedPassword(string passString)
        {
            return GetHashedPassword(Encoding.ASCII.GetBytes(passString));
        }

        public static string GetHashedPassword(byte[] passArray)
        {
            MD5 HashGenerator = MD5Cryptor.Create();
            byte[] hashData = HashGenerator.ComputeHash(passArray);
            return HashGenerator.ToHexString(hashData);
        }
    }
}
