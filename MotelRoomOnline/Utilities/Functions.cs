using MotelRoomOnline.Models;
using System.Security.Cryptography;
using System.Text;

namespace MotelRoomOnline.Utilities
{
    public class Functions
    {
        public static Account? account { get; set; }
        public static long RoomId { get; set; }
        public static long ContractId { get; set; }
        public static long DetailId { get; set; }

        public static string message = string.Empty;

        public static string getCurrenDate()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string TitleSlugGeneration(string type, string title, long id)
        {
            string sTitle = type + "-" + SlugGenerator.SlugGenerator.GenerateSlug(title)
                + "-" + id.ToString() + ".html";
            return sTitle;
        }

        public static string TitleSlugGenerationAlias(string title)
        {
            return SlugGenerator.SlugGenerator.GenerateSlug(title);
        }

        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));
            byte[] result = md5.Hash;
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                stringBuilder.Append(result[i].ToString("x2"));
            }
            return stringBuilder.ToString();
        }

        public static string MD5Password(string? text)
        {
            string str = MD5Hash(text);
            for (int i = 0; i <= 5; i++)
            {
                str = MD5Hash(str + "_" + str);
            }
            return str;
        }

        public static bool IsLogin(int id)
        {
            if (Functions.account == null || Functions.account?.RoleID != id)
            {
                return false;
            }
            return true;
        }

        public static string ToVnd(decimal donGia)
        {
            return donGia.ToString("#,##0");
        }
    }
}
