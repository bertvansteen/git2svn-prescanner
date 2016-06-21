using System;
using System.Collections.Generic;
using System.Linq;

namespace SearchForGitRepos
{
    public class SvnDirectory
    {        
        public string[] Folder { get; set; }
        public string BasePath { get; set; }
        public bool IsTrunk { get; set; }
        public bool IsTagsRoot { get; set; }
        public bool IsBranchesRoot { get; set; }

        public List<SvnDirectory> ChildDirectories { get; set; } = new List<SvnDirectory>();
        public List<string> Files { get; set; } = new List<string>();

        public IEnumerable<string> AllFiles => Files.Concat(ChildDirectories.SelectMany(d => d.AllFiles));

        public IEnumerable<string> IgnoredFiles
        {
            get
            {
                foreach (var file in Files)
                {
                    yield return file;
                }
                foreach (var childDirectory in ChildDirectories.Where(d=> !d.IsTrunk && !d.IsBranchFolder() && !d.IsTagFolder()))
                {
                    foreach (var ignoredFile in childDirectory.IgnoredFiles)
                    {
                        yield return ignoredFile;
                    }
                }            
            }
        }

        private bool IsTagFolder()
        {
            if (Parent == null) return false;
            return Parent.IsTagsRoot;
        }

        private bool IsBranchFolder()
        {
            if (Parent == null) return false;
            return Parent.IsBranchesRoot;
        }

        public SvnDirectory Parent { get; set; }

        public SvnDirectory(SvnListLine path)
        {
            BasePath = path.SvnPath;
            Folder = path.Folder;

            IsTrunk = "trunk".Equals(Folder.LastOrDefault(), StringComparison.InvariantCultureIgnoreCase);
            IsBranchesRoot = "branches".Equals(Folder.LastOrDefault(), StringComparison.InvariantCultureIgnoreCase);
            IsTagsRoot = "tags".Equals(Folder.LastOrDefault(), StringComparison.InvariantCultureIgnoreCase);
        }



        public bool IsGitRoot()
        {
            return ChildDirectories.Any(a => a.IsTrunk);
        }

    }
}