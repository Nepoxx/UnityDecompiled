// Decompiled with JetBrains decompiler
// Type: UnityEditor.CloudBuild.CloudBuild
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEditor.Utils;
using UnityEditor.Web;

namespace UnityEditor.CloudBuild
{
  [InitializeOnLoad]
  internal class CloudBuild
  {
    static CloudBuild()
    {
      JSProxyMgr.GetInstance().AddGlobalObject("unity/cloudbuild", (object) new UnityEditor.CloudBuild.CloudBuild());
    }

    public Dictionary<string, Dictionary<string, string>> GetScmCandidates()
    {
      Dictionary<string, Dictionary<string, string>> dictionary1 = new Dictionary<string, Dictionary<string, string>>();
      Dictionary<string, string> dictionary2 = this.DetectGit();
      if (dictionary2 != null)
        dictionary1.Add("git", dictionary2);
      Dictionary<string, string> dictionary3 = this.DetectMercurial();
      if (dictionary3 != null)
        dictionary1.Add("mercurial", dictionary3);
      Dictionary<string, string> dictionary4 = this.DetectSubversion();
      if (dictionary4 != null)
        dictionary1.Add("subversion", dictionary4);
      Dictionary<string, string> dictionary5 = this.DetectPerforce();
      if (dictionary5 != null)
        dictionary1.Add("perforce", dictionary5);
      return dictionary1;
    }

    private Dictionary<string, string> DetectGit()
    {
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      string str = this.RunCommand("git", "config --get remote.origin.url");
      if (string.IsNullOrEmpty(str))
        return (Dictionary<string, string>) null;
      dictionary.Add("url", str);
      dictionary.Add("branch", this.RunCommand("git", "rev-parse --abbrev-ref HEAD"));
      dictionary.Add("root", this.RemoveProjectDirectory(this.RunCommand("git", "rev-parse --show-toplevel")));
      return dictionary;
    }

    private Dictionary<string, string> DetectMercurial()
    {
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      string str = this.RunCommand("hg", "paths default");
      if (string.IsNullOrEmpty(str))
        return (Dictionary<string, string>) null;
      dictionary.Add("url", str);
      dictionary.Add("branch", this.RunCommand("hg", "branch"));
      dictionary.Add("root", this.RemoveProjectDirectory(this.RunCommand("hg", "root")));
      return dictionary;
    }

    private Dictionary<string, string> DetectSubversion()
    {
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      string str1 = this.RunCommand("svn", "info");
      if (str1 == null)
        return (Dictionary<string, string>) null;
      foreach (string str2 in str1.Split(Environment.NewLine.ToCharArray()))
      {
        char[] separator = new char[1]{ ':' };
        int count = 2;
        string[] strArray = str2.Split(separator, count);
        if (strArray.Length == 2)
        {
          if (strArray[0].Equals("Repository Root"))
            dictionary.Add("url", strArray[1].Trim());
          if (strArray[0].Equals("URL"))
            dictionary.Add("branch", strArray[1].Trim());
          if (strArray[0].Equals("Working Copy Root Path"))
            dictionary.Add("root", this.RemoveProjectDirectory(strArray[1].Trim()));
        }
      }
      if (!dictionary.ContainsKey("url"))
        return (Dictionary<string, string>) null;
      return dictionary;
    }

    private Dictionary<string, string> DetectPerforce()
    {
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      string environmentVariable1 = Environment.GetEnvironmentVariable("P4PORT");
      if (string.IsNullOrEmpty(environmentVariable1))
        return (Dictionary<string, string>) null;
      dictionary.Add("url", environmentVariable1);
      string environmentVariable2 = Environment.GetEnvironmentVariable("P4CLIENT");
      if (!string.IsNullOrEmpty(environmentVariable2))
        dictionary.Add("workspace", environmentVariable2);
      return dictionary;
    }

    private string RunCommand(string command, string arguments)
    {
      try
      {
        Program program = new Program(new ProcessStartInfo(command) { Arguments = arguments });
        program.Start();
        program.WaitForExit();
        if (program.ExitCode < 0)
          return (string) null;
        StringBuilder stringBuilder = new StringBuilder();
        foreach (string str in program.GetStandardOutput())
          stringBuilder.AppendLine(str);
        return stringBuilder.ToString().TrimEnd(Environment.NewLine.ToCharArray());
      }
      catch (Win32Exception ex)
      {
        return (string) null;
      }
    }

    private string RemoveProjectDirectory(string workingDirectory)
    {
      string currentDirectory = Directory.GetCurrentDirectory();
      if (currentDirectory.StartsWith(workingDirectory.Replace('/', '\\')))
        workingDirectory = workingDirectory.Replace('/', '\\');
      return currentDirectory.Replace(workingDirectory, "").Trim(Path.DirectorySeparatorChar);
    }
  }
}
