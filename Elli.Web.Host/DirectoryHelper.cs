// Decompiled with JetBrains decompiler
// Type: Elli.Web.Host.DirectoryHelper
// Assembly: Elli.Web.Host, Version=19.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9006AF5-0CBA-41F3-A51F-BE05E37C7972
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Web.Host.dll

using System.IO;

#nullable disable
namespace Elli.Web.Host
{
  public class DirectoryHelper
  {
    public static bool IsCreated(string path, bool create)
    {
      bool flag = false;
      if (!Directory.Exists(path))
      {
        if (create)
        {
          Directory.CreateDirectory(path);
          flag = true;
        }
      }
      else
        flag = true;
      return flag;
    }
  }
}
