using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.para
{
    public class JsonUtils
    {
        public static T ToObject<T>(string jsonText)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(jsonText);
            }
            catch
            {
                return default(T);
            }
        }
        public static List<T> TSoList<T>(string jsonStr)
        {
            try
            {
                StringReader sr = new StringReader(jsonStr);
                JsonReader jr = new JsonTextReader(sr);
                JsonSerializer js = new JsonSerializer();
                return js.Deserialize<List<T>>(jr);
            }
            catch
            {
                return default(List<T>);
            }
        }

        public static string ObjectToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
