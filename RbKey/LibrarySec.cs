using System.Management;
using System.Security.Cryptography;
using System.Text;

namespace RbKey
{
    public class LibrarySec
    {
        /// <summary>
        /// вычисление md5
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// key по id процца и операционки
        /// </summary>
        /// <returns></returns>
        public static string GetProcessorIdAndGetOSSerialNumberID()
        {
            string str = "";
            ManagementObjectSearcher searcher =
                   new ManagementObjectSearcher("root\\CIMV2",
                   "SELECT * FROM Win32_Processor");
            foreach (ManagementObject queryObj in searcher.Get())
            {
                str = queryObj["ProcessorId"].ToString();
            }

            searcher =
                  new ManagementObjectSearcher("root\\CIMV2",
                  "SELECT * FROM CIM_OperatingSystem");
            foreach (ManagementObject queryObj in searcher.Get())
            {
                str += queryObj["SerialNumber"].ToString();
            }

            return str;
        }
    }
}
