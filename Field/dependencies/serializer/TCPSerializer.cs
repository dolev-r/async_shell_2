using System;
using System.Collections.Generic;
using System.Text;

namespace async_shell.dependencies.serializer
{
    public class TcpSerializer<TData> : ISerializer<TData>
    {
        public TData Deserialize(byte[] outData)
        {
            throw new NotImplementedException();
        }

        public byte[] Serialize(TData data)
        {
            throw new NotImplementedException();
        }
    }
}
