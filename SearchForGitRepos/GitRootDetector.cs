using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchForGitRepos
{
    public class GitRootDetector
    {
        public IEnumerable<SvnDirectory> ScanSvnList(IEnumerable<string> svnListResult)
        {
            var currentDir = new SvnDirectory(SvnListLine.Parse("/"));
            foreach (var svnLine in svnListResult.Select(SvnListLine.Parse))
            {
                while (!IsSubfolderOfCurrentItem(currentDir, svnLine.Folder))
                {
                    if (currentDir.IsGitRoot())
                    {
                        yield return currentDir;
                        currentDir = RemoveLinkWithParent(currentDir);
                    }
                    else
                    {
                        currentDir = currentDir.Parent;
                    }                    
                }
                
                if (!currentDir.Folder.SequenceEqual(svnLine.Folder))
                {
                    if (!IsSubFolder(currentDir.Folder, svnLine.Folder))
                        throw new Exception("Expecting folder declaration first");

                    var newFolder = new SvnDirectory(svnLine) {Parent = currentDir};

                    currentDir.ChildDirectories.Add(newFolder);
                    currentDir = newFolder;
                }
                
                if (!string.IsNullOrEmpty(svnLine.FileName))
                {
                    currentDir.Files.Add(svnLine.SvnPath);
                }
            }
            
            while (currentDir.Parent != null)
            {
                if (currentDir.IsGitRoot())
                {
                    yield return currentDir;
                    currentDir.Parent.ChildDirectories.Remove(currentDir);
                }

                currentDir = currentDir.Parent;
            }

            yield return currentDir;
        }
        
        public static bool IsSubFolder(string[] files1, string[] files)
        {
            if (files.Length != files1.Length + 1)
                return false;

            for (int i = 0; i < files1.Length; i++)
            {
                if (files[i] != files1[i])
                    return false;
            }

            return true;
        }

        public bool IsSubfolderOfCurrentItem(SvnDirectory dir, string[] files)
        {
            if (files.Length < dir.Folder.Length)
                return false;

            for (int i = 0; i < dir.Folder.Length; i++)
            {
                if (files[i] != dir.Folder[i])
                    return false;
            }

            return true;
        }

        private static SvnDirectory RemoveLinkWithParent(SvnDirectory currentDir)
        {
            var parent = currentDir.Parent;
            currentDir.Parent.ChildDirectories.Remove(currentDir);
            currentDir.Parent = null;
            currentDir = parent;
            return currentDir;
        }
    }
}
