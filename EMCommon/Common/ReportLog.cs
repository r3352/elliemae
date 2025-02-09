// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.ReportLog
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System.Collections;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class ReportLog
  {
    private static ArrayList logs = new ArrayList();

    public static void AddLog(string log) => ReportLog.logs.Add((object) log);

    public static void ClearAllLogs() => ReportLog.logs.Clear();

    public static string[] GetAllLogs() => (string[]) ReportLog.logs.ToArray(typeof (string));

    public static bool HasLog() => ReportLog.logs.Count > 0;

    public static int GetLogCount() => ReportLog.logs.Count;

    public static void RemoveLastLog() => ReportLog.logs.RemoveAt(ReportLog.logs.Count - 1);
  }
}
