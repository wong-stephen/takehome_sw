using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FileUtility
{
    class Program
    {
        static string _DirectoryToTEST = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}test_data"; // For testing
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                switch (args[0])
                {
                    case "-index":
                        if (args.Length >= 2)
                            CreateIndex(args[1]);
                        else
                            Console.WriteLine("usage: FileUtility -index (folderPath)");
                        break;
                    case "-search":
                        if (args.Length >= 4)
                            Search(args[1], args[2], args[3]);
                        else
                            Console.WriteLine("usage: FileUtility -search (type) (value) (folderPath)");
                        break;
                }
                
            }
            else
                Console.WriteLine("usage: FileUtility [-index] or [-search (type) (value)]");
        }

        static void CreateIndex(string path)
        {
            var indexBuilder = new StringBuilder();
            foreach (var filepath in Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories))
            {
                var fileInfo = new FileInfo(filepath);
                if (fileInfo.Name != "files.txt")
                    indexBuilder.AppendLine($"{fileInfo.Name}|{fileInfo.FullName}|{fileInfo.Length}|{fileInfo.Extension}");
            }

            var indexName = $"{path}{Path.DirectorySeparatorChar}files.txt";
            using (var writer = new StreamWriter(File.Create(indexName, 8192, FileOptions.WriteThrough)))
            {
                writer.Write(indexBuilder.ToString());
                writer.Flush();
                writer.Close();
            }

            Console.WriteLine("index created");
        }

        static void Search(string type, string value, string path)
        {
            var index = InitSearchIndex(path, type);

            var results = from x in index
                          where x.Item1 == value
                          select x.Item2;

            if (results.Count() > 0)
            {
                foreach (var result in results)
                {
                    Console.WriteLine(result);
                }
            }
            else
                Console.WriteLine("No results found!");
        }

        static List<Tuple<string, string>> InitSearchIndex(string path, string searchType)
        {
            var index = new List<Tuple<string, string>>();

            var indexName = $"{path}{Path.DirectorySeparatorChar}files.txt";
            using (var reader = new StreamReader(File.OpenRead(indexName)))
            {
                bool isDone = false;
                while (!isDone)
                {
                    var line = reader.ReadLine();

                    if (string.IsNullOrWhiteSpace(line))
                        isDone = true;
                    else
                    {
                        var tokens = line.Split('|');

                        switch (searchType)
                        {
                            case "name":
                                var nameKey = tokens[0].Split('.')[0];
                                index.Add(new Tuple<string, string>(nameKey, tokens[1]));
                                break;
                            case "size":
                                index.Add(new Tuple<string, string>(tokens[2], tokens[1]));
                                break;
                            case "type":
                                var typeKey = tokens[3].Substring(tokens[3].LastIndexOf('.') + 1);
                                index.Add(new Tuple<string, string>(typeKey, tokens[1]));
                                break;
                        }
                    }
                }
            }
            return index;
        }
    }
}
 