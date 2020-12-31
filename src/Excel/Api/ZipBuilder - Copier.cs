using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Xml;

namespace Excel
{
    public class ZipBuilder : IDisposable
    {
        private readonly MemoryStream _archiveStream;
        private readonly ZipArchive _archive;

        public ZipBuilder()
        {
            _archiveStream = new MemoryStream();
            _archive = new ZipArchive(_archiveStream, ZipArchiveMode.Create);
        }

        public ZipBuilder AppendFile(string path, string content)
        {
            using (var contentTypeStream = new StreamWriter(_archive.CreateEntry(path, CompressionLevel.Fastest).Open(), Encoding.UTF8))
            {
                contentTypeStream.WriteLine(content);
            }

            return this;
        }

        public void Dispose()
        {
            _archive.Dispose();
            _archiveStream.Dispose();
        }

        public byte[] Build()
        {
            return _archiveStream.ToArray();
        }
    }
}
