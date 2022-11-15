using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WindowsFormsApp7
{
    public static class FileCompressor
    {
        private static Regex Utf8Regex = new Regex("");

        public static string Zip(this string content)
        {
            string result = "";
            var lines = content.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            int tokenCount;
            char tokenChar;
            bool first = true;
            foreach (var line in lines)
            {
                if (!first) result += Environment.NewLine;
                else first = false;

                for (int c = 0; c < line.Length; c++)
                {
                    if (c + 4 >= line.Length)
                    {
                        result += line[c];
                        continue;
                    }

                    if (line.Skip(c).Take(4).All(x => x == line[c]))
                    {
                        tokenChar = line[c];
                        tokenCount = 0;
                        while (c < line.Length && line[c] == tokenChar) { tokenCount++; c++; }
                        if (tokenCount < 256)
                        {
                            result += $"#{tokenChar}{(char)tokenCount}$";
                        }
                        else
                        {
                            result += $"#{tokenChar}";
                            while(tokenCount >= 256)
                            {
                                result += (char)255;
                                tokenCount -= 255;
                            }
                            result += $"{(char)tokenCount}$";
                        }
                        c--;
                    }
                    else
                    {
                        result += line[c];
                    }
                }
            }
            return result;
        }
        public static string UnZip(this string content)
        {
            string result = "";
            var lines = content.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            int tokenCount;
            char tokenChar;
            string tokenTemp;
            bool first = true;
            foreach (var line in lines)
            {
                if (!first) result += Environment.NewLine;
                else first = false;

                for (int c = 0; c < line.Length; c++)
                {
                    if (line[c] == '#')
                    {
                        c++;
                        tokenChar = line[c];
                        c++;
                        tokenTemp = "";
                        while (c < line.Length && (line[c] != '$' || (line[c] == '$' && (c + 1 >= line.Length - 1 ? false : line[c + 1] == '$')))) { tokenTemp += line[c]; c++; }
                        //List<char> tokenTempArray = new List<char>();
                        //for(int i=0; i< tokenTemp.Length; i++)
                        //{
                        //    if (tokenTemp[i] == '\\' && tokenTemp[i] == 'u' && i + 5 < tokenTemp.Length && Utf8Regex.IsMatch(string.Concat(tokenTemp.Skip(i).Take(6))))
                        //    {
                        //        tokenTempArray.Add(char.Parse(string.Concat(tokenTemp.Skip(i).Take(6))));
                        //        i += 6;
                        //    }
                        //    else
                        //    {
                        //        tokenTempArray.Add(tokenTemp[i]);
                        //    }
                        //}
                        //tokenCount = tokenTempArray.Select(x => (int)x).Sum();
                        tokenCount = tokenTemp.Select(x => (int)x).Sum();
                        result += string.Concat(Enumerable.Repeat(tokenChar, tokenCount));
                    }
                    else
                    {
                        result += line[c];
                    }
                }
            }
            return result;
        }
    }
}
