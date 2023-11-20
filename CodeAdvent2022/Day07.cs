using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAdvent2022
{
    internal class Day07 : IDay
    {
        public int Order => 7;
        const string _inputFilename = "day07_input.txt";

        private const int _diskSize = 70000000;
        private const int _neededSpace = 30000000;

        private AdventFolder _homeFolder = new AdventFolder()
        {
            Dirname = "/",
            Parent = null
        };
        private AdventFolder _currentFolder;

        private List<AdventFolder> _folders = new List<AdventFolder>();

        public void Run()
        {
            string[] lines = File.ReadAllLines(_inputFilename);
            _currentFolder = _homeFolder;

            string activeCommand = "";
            foreach (string line in lines)
            {
                var parts = line.Split(' ');
                if (line.StartsWith('$'))
                {
                    activeCommand = "";
                    switch (parts[1])
                    {
                        case "ls":
                            activeCommand = "ls";
                            continue;
                        case "cd":
                            if (parts[2] == "/")
                            {
                                _currentFolder = _homeFolder;
                                continue;
                            }
                            if (parts[2] == "..")
                            {
                                _currentFolder = _currentFolder.Parent;

                                continue;
                            }
                            _currentFolder = _currentFolder.ChildFolders
                                                           .Where(x => x.Dirname == parts[2])
                                                           .Single();
                            break;
                    }
                }
                if (activeCommand == "ls")
                {
                    if(line.StartsWith("dir"))
                    {
                        var temp = new AdventFolder()
                        {
                            Dirname = parts[1],
                            Parent = _currentFolder
                        };
                        _currentFolder.ChildFolders.Add(temp);
                        _folders.Add(temp);
                    }
                    else
                    {
                        var tempFile = new AdventFile()
                        {
                            Filename = parts[1],
                            Folder = _currentFolder,
                            Size = int.Parse(parts[0])
                        };
                        _currentFolder.Files.Add(tempFile);
                    }
                }
            }

            List<long> folderSizes = new();
            long homeFolderSize = 0;
            CalculateFolderSize(_homeFolder, ref homeFolderSize);
            var needed = _neededSpace - (_diskSize - homeFolderSize);

            long total = 0;
            foreach(var folder in _folders)
            {
                long folderSize = 0;
                CalculateFolderSize(folder, ref folderSize);

                if(folderSize <= 100000)
                {
                    total += folderSize;
                }
                if(folderSize > needed)
                {
                    folderSizes.Add(folderSize);
                }
            }

            Console.WriteLine($"Total on folder at most 100000: {total}");

            var foo = folderSizes.OrderBy(x => x).First();
            Console.WriteLine($"Closest to needed: {foo}");
        }

        private void CalculateFolderSize(AdventFolder folder, ref long size)
        {
            size += folder.Files.Sum(x => x.Size);
            foreach (var child in folder.ChildFolders)
            {
                CalculateFolderSize(child, ref size);
            }
        }
    }

    

    internal class AdventFolder
    {
        public AdventFolder()
        {
            ChildFolders = new List<AdventFolder>();
            Files = new List<AdventFile>();
        }

        public string Dirname { get; set; }

        public AdventFolder Parent { get; set; }

        public List<AdventFolder> ChildFolders { get; set; }

        public List<AdventFile> Files { get; set; }
    }

    internal class AdventFile
    {
        public string Filename { get; set; }

        public AdventFolder Folder { get; set; }

        public long Size { get; set; }
    }
}
