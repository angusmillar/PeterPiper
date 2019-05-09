using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPeterPiper.Support
{
  public static class PathSupport
  {
    public static string AssemblyDirectory
    {
      get
      {
        string codeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
        UriBuilder uri = new UriBuilder(codeBase);
        string path = Uri.UnescapeDataString(uri.Path);
        var DirInfo = new DirectoryInfo(path);
        return DirInfo.Parent.FullName; ;
      }
    }
  }
}
