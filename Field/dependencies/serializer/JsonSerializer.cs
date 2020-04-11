using System.Text;
using Newtonsoft.Json;

namespace async_shell.dependencies.serializer
{
    public class JsonSerializer<TData> : ISerializer<TData>
    {
        public TData Deserialize(byte[] bytes)
        {
            return JsonConvert.DeserializeObject<TData>(Encoding.UTF8.GetString(bytes));
        }

        public byte[] Serialize(TData data)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
        }
    }
}
