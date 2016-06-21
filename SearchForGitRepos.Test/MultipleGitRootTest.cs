using System.Linq;
using Xunit;

namespace SearchForGitRepos.Test
{
    public class MultipleGitRootTest
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
                "Folder2/",
                "Folder2/trunk/",
                "Folder2/trunk/file.txt",
            };

            var folders = SUT.ScanSvnList(files);
            Assert.Equal(2, folders.Count(s => s.IsGitRoot()));
        }
    }
}