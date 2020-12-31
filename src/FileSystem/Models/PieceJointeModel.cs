using FacturationApi.Models;
using System.Collections.Generic;

namespace FileSystem
{
    public class DocFile : IFile, IFileDb
    {
        public string FileName { get; set; }
        public byte[] Content { get; set; }
    }

    public class PieceJointeModel : IPieceJointe
    {
        public int Id { get; set; }
        public IEnumerable<DocFile> Documents { get; set; }
        IEnumerable<IFile> IPieceJointe.Documents => Documents;
    }
}
