using Db;
using FacturationApi.Spi;
using FileSystem;
using Microsoft.EntityFrameworkCore;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace RepositoryExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Request.Run(args);
            Console.WriteLine("FINISH!");
            Console.ReadLine();
        }

        static void FileManager()
        {
        }

        static void QueryFile()
        {
            var query = FileSystem.FileQueryable<int>.Create((expression) => 
            {
                var str = expression.Reduce().ToString();
                var res = new List<int> { 1, 0, 2 }.AsQueryable();
                return expression.Compile()(res);
            });

            var tmp = query.Where(_ => _ % 2 == 0)
                .Select(_ => _ + 10)
                .Where(_ => _ > 10)
                .ToList();
        }

        static void Ftp()
        {
        }

        static void Repository()
        {
        }
    }
}
