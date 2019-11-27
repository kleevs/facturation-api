using FacturationApi.Models;
using FacturationApi.Spi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileSystem
{
    public partial class FileManager :
        IPieceJointeProvider
    {
        private readonly string _host;
        private readonly int _port;
        private readonly string _user;
        private readonly string _password;
        private readonly NetworkCredential _credential;
        private readonly List<IFileDb> _newFiles;

        public FileManager(string host, int port, string user, string password)
        {
            _host = host;
            _port = port;
            _user = user;
            _password = password;
            _credential = new NetworkCredential(_user, _password);
            _newFiles = new List<IFileDb>();
        }

        public IQueryable<IFileDb> Files(string path)
        {
            var directory = GetDirectory(path);

            if (directory == path)
            {
                return FileQueryable<IFileDb>.Create((expression) =>
                {
                    try
                    {
                        FtpWebRequest request = (FtpWebRequest)WebRequest.Create($"ftp://{_user}@{_host}:{_port}{path}");
                        request.Method = WebRequestMethods.Ftp.ListDirectory;
                        request.Credentials = _credential;
                        FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                        Stream responseStream = response.GetResponseStream();
                        StreamReader reader = new StreamReader(responseStream);
                        var strResult = reader.ReadToEnd();
                        response.Close();
                        var files = strResult.Replace("\r", string.Empty).Split('\n')
                            .Select(str => Regex.Replace(str, @"^id\d+\/(.*)", "$1"))
                            .Where(str => !string.IsNullOrEmpty(str))
                            .Select(str => new DocFile { FileName = str })
                            .ToList();

                        var query = files.AsQueryable();
                        return expression?.Compile()(query) ?? query ?? Enumerable.Empty<IFileDb>();
                    }
                    catch (Exception)
                    {
                        return Enumerable.Empty<IFileDb>();
                    }
                });
            }
            else
            {
                return FileQueryable<IFileDb>.Create((expression) =>
                {
                    try
                    {
                        FtpWebRequest request = (FtpWebRequest)WebRequest.Create($"ftp://{_user}@{_host}:{_port}{path}");
                        request.Method = WebRequestMethods.Ftp.DownloadFile;
                        request.Credentials = _credential;
                        FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                        Stream responseStream = response.GetResponseStream();

                        using (MemoryStream ms = new MemoryStream())
                        {
                            responseStream.CopyTo(ms);
                            var files = new DocFile[] { new DocFile { Content = ms.ToArray() } };
                            var query = files.AsQueryable();
                            return expression?.Compile()(query) ?? query ?? Enumerable.Empty<IFileDb>();
                        }
                    }
                    catch (Exception)
                    {
                        return Enumerable.Empty<IFileDb>();
                    }
                });
            }
        }

        public IFileDb NewFile()
        {
            var res = new DocFile();
            _newFiles.Add(res);
            return res;
        }

        public void RemoveFile(IFileDb item)
        {
        }

        public async Task SaveChangesAsync() => await Task.Run(() =>
        {
            var directories = _newFiles
                .Select(file => GetDirectory(file.FileName))
                .Where(_ => !string.IsNullOrEmpty(_))
                .ToList();

            foreach (var directory in directories)
            {
                try
                {
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create($"ftp://{_user}@{_host}:{_port}{directory}");
                    request.Method = WebRequestMethods.Ftp.MakeDirectory;
                    request.Credentials = _credential;
                    FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                    response.Close();
                }
                catch (Exception)
                {
                }
            }

            foreach (var doc in _newFiles)
            {
                try
                {
                    var request = (FtpWebRequest)WebRequest.Create($"ftp://{_user}@{_host}:{_port}{doc.FileName}");
                    request.Method = WebRequestMethods.Ftp.UploadFile;
                    request.Credentials = _credential;
                    byte[] fileContents = doc.Content;
                    request.ContentLength = fileContents.Length;

                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(fileContents, 0, fileContents.Length);
                    requestStream.Close();

                    var response = (FtpWebResponse)request.GetResponse();

                    response.Close();
                }
                catch (Exception) { }
            }
        });

        private string GetDirectory(string path)
        {
            var match = Regex.Match(path, @"^(.+)\/.+\..+$");
            var directory = match?.Groups?.Count > 1 ? match.Groups[1].Value : null;
            return directory ?? path;
        }
    }
}
