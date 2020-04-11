using System;
using System.Collections.Generic;
using System.Text;

namespace async_shell.dependencies.data_holders
{
    public class FileData : DataWithAttributes
    {
        public string Name { get; set; }

        public byte[] Content { get; set; }

        public uint Size { get; set; }
    }
}
