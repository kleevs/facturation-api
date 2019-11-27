using System.Collections.Generic;

namespace FacturationApi.Models
{
    public interface IPieceJointe : IIdentifiable
    {
        IEnumerable<IFile> Documents { get; }
    }

    public interface IFile
    {
        string FileName { get; }
        byte[] Content { get; }
    }

    public interface IFileDb
    {
        string FileName { get; set; }
        byte[] Content { get; set; }
    }
}
