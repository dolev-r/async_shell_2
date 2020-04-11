using System.Text;
using Newtonsoft.Json.Linq;

namespace async_shell.dependencies.serializer
{
    public class JsonExtractor : IExtractor
    {
        public string Extract(byte[] data, string fieldName)
        {
            return JObject.Parse(Encoding.UTF8.GetString(data))[fieldName].ToString();
        }
    }
}