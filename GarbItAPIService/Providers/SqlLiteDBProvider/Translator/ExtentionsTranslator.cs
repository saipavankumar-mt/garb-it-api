using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace SQLiteDBProvider.Providers
{
    public static class ExtentionsTranslator
    {
        public static string ConvertDate(this string obj)
        {
            try
            {
                return DateTime.ParseExact(obj, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture).ToString("yyyy/MM/dd HH:mm");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //
            }

            return obj;
        }
    }
}
