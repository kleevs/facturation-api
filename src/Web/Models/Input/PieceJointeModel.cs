using FacturationApi.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Web.Models.Input
{
    public class DocFile : IFile
    {
        public string FileName { get; set; }
        public byte[] Content { get; set; }
    }

    public class PieceJointeModel : IPieceJointe
    {
        private IEnumerable<IFile> _documentsCached;
        public int Id { get; set; }
        public IEnumerable<IFormFile> Documents { get; set; }

        IEnumerable<IFile> IPieceJointe.Documents => _documentsCached = _documentsCached ?? Documents.Select(_ => 
        {
            using (MemoryStream ms = new MemoryStream())
            {
                _.OpenReadStream().CopyTo(ms);
                return new DocFile
                {
                    FileName = _.FileName,
                    Content = ms.ToArray()
                };
            }
        });
    }
}
