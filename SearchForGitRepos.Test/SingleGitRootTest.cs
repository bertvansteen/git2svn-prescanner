using System;
using System.Linq;
using Xunit;

namespace SearchForGitRepos.Test
{
    public class SingleGitRootTest
    {
        GitRootDetector SUT = new GitRootDetector();

        [Fact]
        public void Find1()
        {
            var files = new[]
            {
                "Folder/",
                "Folder/trunk/",
                "Folder/trunk/file.txt",
            };

            var folders = SUT.ScanSvnList(files);
            Assert.Equal(1, folders.Count(s=> s.IsGitRoot()));
        }

        [Fact]
        public void FindSubfolder1()
        {
            var files = new[]
            {
                "Folder/",
                "Folder/Subfolder/",
                "Folder/Subfolder/trunk/",
                "Folder/Subfolder/trunk/file.txt",
            };

            var folders = SUT.ScanSvnList(files);
            Assert.Equal(1, folders.Count(s => s.IsGitRoot()));
        }

        [Fact]
        public void FindIgnoredFiles1()
        {
            var files = new[]
            {
                "Folder/",
                "Folder/Subfolder/",
                "Folder/Subfolder/trunk/",
                "Folder/Subfolder/trunk/file.txt",
                "Folder/Subfolder/file2.txt",
            };

            var folders = SUT.ScanSvnList(files).ToList();
            Assert.Equal(1, folders.Count(s => s.IsGitRoot()));
            Assert.Equal(1, folders.First(s => s.IsGitRoot()).IgnoredFiles.Count());
        }

        [Fact]
        public void CombineFiles2()
        {
            var files = new[]
            {
                "Folder/",
                "Folder/Subfolder/",
                "Folder/Subfolder/subfolder/",
                "Folder/Subfolder/subfolder/file.txt",
                "Folder/Subfolder/file2.txt",
            };

            var folders = SUT.ScanSvnList(files).ToList();
            Assert.Equal(1, folders.Count);

            var svnFolder = folders.First();
            Assert.Equal(false, svnFolder.IsGitRoot());
            Assert.Equal(2, svnFolder.IgnoredFiles.Count());
        }

        [Fact]
        public void CheckBranches1()
        {
            var files = new[]
            {
                "Folder/",
                "Folder/Subfolder/",
                "Folder/Subfolder/trunk/",
                "Folder/Subfolder/trunk/file.txt",
                "Folder/Subfolder/branches/",
                "Folder/Subfolder/branches/A/",
                "Folder/Subfolder/branches/A/file.txt",
            };

            var folders = SUT.ScanSvnList(files).ToList();
            Assert.Equal(1, folders.Count(f => f.IsGitRoot()));

            var svnFolder = folders.First(f => f.IsGitRoot());
            Assert.Equal(0, svnFolder.IgnoredFiles.Count());
        }

        [Fact]
        public void CheckTags1()
        {
            var files = new[]
            {
                "Folder/",
                "Folder/Subfolder/",
                "Folder/Subfolder/trunk/",
                "Folder/Subfolder/trunk/file.txt",
                "Folder/Subfolder/tags/",
                "Folder/Subfolder/tags/A/",
                "Folder/Subfolder/tags/A/file.txt",
            };

            var folders = SUT.ScanSvnList(files).ToList();
            Assert.Equal(1, folders.Count(f => f.IsGitRoot()));

            var svnFolder = folders.First(f => f.IsGitRoot());
            Assert.Equal(0, svnFolder.IgnoredFiles.Count());
        }

        [Fact]
        public void CheckTagsAndBranches1()
        {
            var files = new[]
            {
                "Folder/",
                "Folder/Subfolder/",
                "Folder/Subfolder/trunk/",
                "Folder/Subfolder/trunk/file.txt",
                "Folder/Subfolder/tags/",
                "Folder/Subfolder/tags/A/",
                "Folder/Subfolder/tags/A/file.txt",
                "Folder/Subfolder/branches/",
                "Folder/Subfolder/branches/A2/",
                "Folder/Subfolder/branches/A2/file.txt",
            };

            var folders = SUT.ScanSvnList(files).ToList();
            Assert.Equal(1, folders.Count(f=> f.IsGitRoot()));

            var svnFolder = folders.First(f => f.IsGitRoot());
            Assert.Equal(0, svnFolder.IgnoredFiles.Count());
        }
    }
}
