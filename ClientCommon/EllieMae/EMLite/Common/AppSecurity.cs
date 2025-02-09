// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.AppSecurity
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using System.Collections.Generic;
using System.Text.RegularExpressions;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class AppSecurity
  {
    public static List<string> EncodeArguments(string[] argList)
    {
      List<string> stringList = new List<string>();
      foreach (string input in argList)
      {
        string str = Regex.Replace(input, "(\\\\+)$", "$1$1");
        stringList.Add(str);
      }
      return stringList;
    }

    public static string EncodeArgument(string argument)
    {
      return "\"" + Regex.Replace(argument, "(\\\\+)$", "$1$1") + "\"";
    }

    public static string EncodeCommand(string commandPath)
    {
      return commandPath.Replace("|", "").Replace("&", "");
    }

    public static string EncodeFileExtension(string fileExtension)
    {
      return new Regex("[\\\\/:*?\"<>|;]").Replace(fileExtension, "");
    }
  }
}
