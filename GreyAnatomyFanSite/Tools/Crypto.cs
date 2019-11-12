using System.Text;
using System.Security.Cryptography;


namespace GreyAnatomyFanSite.Tools
{
    public static class Crypto
    {
        
        public static string HashMdp(string Password)
        {
            byte[] raw = UTF8Encoding.UTF8.GetBytes(Password);

            byte[] hash;
            using (MD5 md5Hash = MD5.Create())
            {
                hash = md5Hash.ComputeHash(raw);
            }

            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
                sBuilder.Append(hash[i].ToString("x2"));


            return sBuilder.ToString();
        }

    }
}
