// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.LoanLogSnapshotDataParser
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using System.Collections.Generic;
using System.Text.RegularExpressions;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects
{
  public static class LoanLogSnapshotDataParser
  {
    private const string FieldSeparator = "!~@�";
    private const string FieldIdValuePairSeparator = "~!@�";
    private const int FieldIdValuePairSplitCount = 2;

    public static IEnumerable<KeyValuePair<string, string>> ParseFieldPairs(
      string snapshotLogFieldData)
    {
      string[] strArray = Regex.Split(snapshotLogFieldData, "!~@");
      for (int index = 0; index < strArray.Length; ++index)
      {
        string[] strArray1 = Regex.Split(strArray[index], "~!@");
        if (strArray1.Length == 2)
          yield return new KeyValuePair<string, string>(strArray1[0], strArray1[1]);
      }
      strArray = (string[]) null;
    }
  }
}
