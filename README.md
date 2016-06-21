# svn2git-prescanner
## Introduction?
For one of our clients I had to migrate a massive SVN server to GIT, I created a simple C# console application that searches the SVN server for directories that can be migrated with svn2git to ensure that we covered all repo's.


##Features
1. It searches the SVN LS output for directories that match the SVN branching conventions (trunk/branches/tags)
2. It lists all files that don't have a parent matches this convention.
3. It lists all files that don't follow the normal convention, e.g. files next to trunk directory.


##Usage

```
C:\>svn ls svn://my-svn-server/path/ -R | SearchForGitRepos.exe
```

###Disclaimer
This tool uses recursion and memory consumption might be high when you have a lot of files that don't follow the conventions or if you have repo's with a lot of branches.