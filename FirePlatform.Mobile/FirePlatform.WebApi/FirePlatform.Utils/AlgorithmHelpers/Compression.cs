using Newtonsoft.Json;
using System.Collections.Generic;

namespace FirePlatform.Utils.AlgorithmHelpers
{
    public static class Compression
    {
        public static List<string> SerializeAndCompress(this object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            List<string> list64 = new List<string>();

            int count = 0;
            int mainCounter = 0;
            var tempString = string.Empty;

            foreach (var @char in json)
            {
                mainCounter++;

                if (count <= 171)
                {
                    tempString += @char;
                    count++;
                }
                if (count > 171)
                {
                    list64.Add(LZStringCSharp.LZString.CompressToBase64(tempString));
                    tempString = string.Empty;
                    count = 0;
                }
                if (mainCounter == json.Length)
                {
                    list64.Add(LZStringCSharp.LZString.CompressToBase64(tempString));
                }
            }
            return list64;
        }
    }
}
