using System;
using System.Text.RegularExpressions;

namespace SqLiteAppNetCoreV2
{
    public static class DateExt
    {
        public static void TestDateParse()
        {
            string text = "The event is scheduled for 10/08/2024 and the deadline is 12/31/2024.";
            string pattern = @"\b\d{2}/\d{2}/\d{4}\b";
            string format = "MM/dd/yyyy";

            Console.WriteLine("\r\n==== DATEPARSE ==== \r\n");

            Console.WriteLine("\r\n***" + text + "***");
            text.DateParse(pattern, format);

             text = "The event is scheduled for 091924 and the deadline is 123124.";
             pattern = @"\b\d{2}\d{2}\d{2}\b";     // US FORMAT MMddyy
             format = "MMddyy";
            Console.WriteLine("\r\n" + "***" + text + "***");
            text.DateParse(pattern, format);

             text = "Ohhh noooo bad date 401324 .";
             pattern = @"\b\d{2}\d{2}\d{2}\b";  
             format = "MMddyy";
            Console.WriteLine("\r\n" + "***" + text + "***");
            text.DateParse(pattern, format);


            Console.WriteLine("\r\n==== RES PARSE ==== \r\n");


            text = "hejsan 222 hoppsan.sdfsdf";
            Console.WriteLine("\r\n" + "***" + text + "***");
            text.ResParse();

            text = "hejsan hoppsan720hjasd";
            Console.WriteLine("\r\n" + "***" + text + "***");
            text.ResParse();

            text = "lkjsadfj fasdfasdlf sdfsdf1720ddddhjasd";
            Console.WriteLine("\r\n" + "***" + text + "***");
            text.ResParse();

            text = "hejsan.4444.hoppsan.777.ssdsf";
            Console.WriteLine("\r\n" + "***" + text + "***");
            text.ResParse();
        }

        public static string ResParse(this string text)
        {
            var pattern = @"\d{3,4}";
            MatchCollection matches = Regex.Matches(text, pattern);
            foreach (Match match in matches)
            {
                string resString = match.Value;
                Console.WriteLine("Found valid res: " + resString.ToString());
            }

            return "";
        }

        public static void DateParse(this string text, string pattern, string format)
        {
            MatchCollection matches = Regex.Matches(text, pattern);
            // string retval = "";        
            foreach (Match match in matches)
            {
                string dateString = match.Value;
                if (DateTime.TryParseExact(dateString, format, null, System.Globalization.DateTimeStyles.None, out DateTime validDate))
                {
                    Console.WriteLine("Found valid Date: " + validDate.ToString("yyMMdd"));
                }
                else
                {
                    Console.WriteLine("Invalid Date: " + dateString);
                }
            }
            // return dateString;
        }
    }
}