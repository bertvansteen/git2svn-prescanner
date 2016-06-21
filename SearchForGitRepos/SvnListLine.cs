using System;
using System.Linq;

namespace SearchForGitRepos
{
    public struct SvnListLine
    {
        private string[] folder;
        public string SvnPath { get; set; }

        public string[] Folder
        {
            get { return folder; }
            set { folder = value; }
        }

        public string FileName { get; set; }

        public SvnListLine(string[] folder, string svnPath, string fileName)
        {
            this.folder = folder;
            SvnPath = svnPath;
            FileName = fileName;
        }
        
        public static SvnListLine Parse(string svnPath)
        {
            if (svnPath.EndsWith("/"))
            {
                var folder = svnPath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                return new SvnListLine(folder, svnPath, null);
            }
            else
            {
                var folder = svnPath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                var fileName = folder[folder.Length - 1];

                Array.Resize(ref folder, folder.Length - 1);
                return new SvnListLine(folder, svnPath, fileName);
            }
        }

    }
}