//using SharpCompress.Common;
//using SharpCompress.Writers;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.IO.Compression;
//using System.Linq;
//using System.Text;
//using System.Xml;

//namespace Excel
//{
//    public class ZipBuilder : IDisposable
//    {
//        private readonly MemoryStream _archiveStream;
//        private readonly IWriter _writer;

//        public ZipBuilder() 
//        {
//            _archiveStream = new MemoryStream();
//            _writer = WriterFactory.Open(_archiveStream, ArchiveType.Zip, new WriterOptions(CompressionType.None));
//        }

//        public ZipBuilder AppendFile(string path, string content) 
//        {
//            using (var stream = new MemoryStream())
//            {
//                var buffer = Encoding.UTF8.GetBytes(content);
//                stream.Write(buffer, 0, buffer.Length);
//                stream.Position = 0;
//                _writer.Write(path, stream);
//            }
//            return this;
//        }

//        public void Dispose()
//        {
//            _writer.Dispose();
//            _archiveStream.Dispose();
//        }

//        public byte[] Build() 
//        {
//            return _archiveStream.ToArray();
//        }
//    }
//}
