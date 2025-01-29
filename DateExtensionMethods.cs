using System;
using System.Text.RegularExpressions;

namespace SqLiteAppNetCoreV2
{
    public static class DateExt
    {
        public static void TestYearParse()
        {
            string text = "Here is some text with a year 2023 and some more text with another year 1999.";
            var result = text.YearToDateParse();
            // string pattern = @"\b\d{4}\b";

            // MatchCollection matches = Regex.Matches(text, pattern);
            // int currentYear = DateTime.Now.Year;
            // int startSearchYear = 2018;

            // string result = Regex.Replace(text, pattern, match =>
            // {
            //     int year = int.Parse(match.Value);
            //     if (year <= currentYear && year >= startSearchYear)
            //     {
            //         return year.ToString() + "1231";
            //     }
            //     return match.Value;
            // });

            Console.WriteLine(result);
        }
        
        

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


            Console.WriteLine("\r\n" + "*** DateParseSweDateWithReplace ***");
            // pattern = @"\b(\d\s*\d\s*\d\s*\d)\s*-\s*(\d\s*\d)\s*-\s*(\d\s*\d)\b";
            pattern = @"\b(\d\s*\d\s*\d\s*\d)[\s-]*(\d\s*\d)[\s-]*(\d\s*\d)\b";
            format = "yyMMdd";

            text = "Detta är ett exempel med datum 2 0 2 4 - 1 0 - 2 9 och mer text.";
            var result = text.DateParseYYYYMMDD_AndReplace(pattern, format);
            Console.WriteLine($"text: {result.input}, date: {result.date}");
            Console.WriteLine(".\r\n");

            text = "Detta är ett exempel med datum 2024 - 10 - 29 och mer text.";
            result = text.DateParseYYYYMMDD_AndReplace(pattern, format);
            Console.WriteLine($"text: {result.input}, date: {result.date}");
            Console.WriteLine(".\r\n");

            text = "Detta är ett exempel med datum 2 0 2 4-1 0-2 9 och mer text.";
            result = text.DateParseYYYYMMDD_AndReplace(pattern, format);
            Console.WriteLine($"text: {result.input}, date: {result.date}");
            Console.WriteLine(".\r\n");
            
            text = "The event is on 2 0 2 4 - 1 0 - 3 0 and the deadline is 2 0 2 4 - 1 1 - 1 5.";
            result = text.DateParseYYYYMMDD_AndReplace(pattern, format);
            Console.WriteLine($"text: {result.input}, date: {result.date}");
            Console.WriteLine(".\r\n");

            text = "The event is on 20 24 - 10 - 30 and the deadline is 2 0 2 4- 1 1-1 5.";
            result = text.DateParseYYYYMMDD_AndReplace(pattern, format);
            Console.WriteLine($"text: {result.input}, date: {result.date}");
            Console.WriteLine(".\r\n");

            text = "The event is on 20 24 10 30 and the deadline is 2 0 2 4 1 11 5.";
            result = text.DateParseYYYYMMDD_AndReplace(pattern, format);
            Console.WriteLine($"text: {result.input}, date: {result.date}");
            Console.WriteLine(".\r\n");

            TestReplace();

        }

        public static void TestResoParse()
        {
            Console.WriteLine("\r\n==== RES PARSE ==== \r\n");

            string text = "hejsan 222 hoppsan.sdfsdf";
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

            TestReplace();
        }

        public static void TestReplace()
        {
            Console.WriteLine("\r\n==== REPLACE TEXT ==== \r\n");
            var text = "Hejsan . Hoppsan - ";
            Console.WriteLine("Input: <" + text + ">");
            Console.WriteLine("After Replace: <" + text.ReplaceText(" - ", "") + ">");
            
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
            foreach (Match match in matches)
            {
                string dateString = match.Value;
                if (DateTime.TryParseExact(dateString, format, null, System.Globalization.DateTimeStyles.None, out DateTime validDate))
                {
                    Console.WriteLine("Found valid Date: " + validDate.ToString(format));
                }
                else
                {
                    Console.WriteLine("Invalid Date: " + dateString);
                }
            }
        }

        public static string YearToDateParse(this string text)
        {
            string pattern = @"\b\d{4}\b";
            var result = text;
            MatchCollection matches = Regex.Matches(text, pattern);
            int currentYear = DateTime.Now.Year;
            int startSearchYear = 2018;

            result = Regex.Replace(text, pattern, match =>
            {
                int year = int.Parse(match.Value);
                if (year <= currentYear && year >= startSearchYear)
                {
                    return year.ToString() + "1231";
                }
                return match.Value;
            });
            
            return result;
        }

        public static (string input, string date) DateParseYYYYMMDD_AndReplace(this string input, string pattern, string format)
        {
            if (string.IsNullOrEmpty(format) || string.IsNullOrEmpty(pattern))
                return (string.Empty, string.Empty);

            // string pattern = @"\b(\d{4})\s*-\s*(\d{2})\s*-\s*(\d{2})\b";
            Console.WriteLine($"Try this: {input} ");
            
            var newDate = string.Empty;
            Regex regex = new Regex(pattern);
            MatchCollection matches = regex.Matches(input);
            if (matches.Count == 0)
            {
                Console.WriteLine("-- FAIL! No matches found!!");
            }
            else
            {
                foreach (Match match in matches)
                {
                    string originalDate = match.Value;
                    string year = match.Groups[1].Value.Replace(" ", "");
                    string month = match.Groups[2].Value.Replace(" ", "");
                    string day = match.Groups[3].Value.Replace(" ", "");
                    newDate = $"{year}-{month}-{day}";

                    Console.WriteLine($"Found date (unformatted): {newDate}");

                     if (DateTime.TryParse(newDate, out DateTime validDate))
                    //if (DateTime.TryParseExact(newDate, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime validDate))
                    {
                        newDate = validDate.ToString(format);
                        input = input.Replace(originalDate, string.Empty);
                        Console.WriteLine($"Valid formatted date: {newDate}");
                    }
                    else
                    {
                        Console.WriteLine($"--INVALID date found: {originalDate}");
                    }

                }
                Console.WriteLine($"Result: {input}");
            }
            // Console.WriteLine(".\r\n");
            return (input, newDate);
        }

        public static string ReplaceText(this string input, string replaceThis, string withThis)
        {
            return input.Replace(replaceThis, withThis, StringComparison.OrdinalIgnoreCase);
        }

    }
}
