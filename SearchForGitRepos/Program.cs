using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchForGitRepos
{
    class Program
    {
        static void Main(string[] args)
        {
            IEnumerable<string> items;
            if (args.Length == 0)
                items = GetFromConsole();
            else
                items = GetFromFile(args[0]);

            GitRootDetector detector = new GitRootDetector();
            foreach (var svnDirectory in detector.ScanSvnList(items))
            {
                if (svnDirectory.IsGitRoot())
                {
                    Console.WriteLine("FOUND A GIT REPO: " + svnDirectory.BasePath);
                    Console.WriteLine("Ignored files: " + svnDirectory.IgnoredFiles.Count());
                    foreach (var f in svnDirectory.IgnoredFiles)
                    {
                        Console.Write("-- ");
                        Console.WriteLine(f);
                    }
                }
                else
                {
                    Console.WriteLine("NOT A GIT REPO: " + svnDirectory.BasePath);
                    Console.WriteLine("Ignored files: " + svnDirectory.IgnoredFiles.Count());
                    foreach (var f in svnDirectory.IgnoredFiles)
                    {
                        Console.Write("-- ");
                        Console.WriteLine(f);
                    }
                }
            }
        }


        private static IEnumerable<string> GetFromFile(string path)
        {
            using (var f = File.OpenText(path))
            {
                string s;
                while ((s = f.ReadLine()) != null)
                {
                    yield return s;
                }
            }
        }

        private static IEnumerable<string> GetFromConsole()
        {
            string s;
            while ((s = Console.ReadLine()) != null)
            {
                yield return s;
            }
        }
    }
}