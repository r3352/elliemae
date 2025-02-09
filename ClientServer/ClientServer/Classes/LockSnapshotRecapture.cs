// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Classes.LockSnapshotRecapture
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Trading;
using System;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Classes
{
  [Serializable]
  public class LockSnapshotRecapture
  {
    public string LoanGUID { get; set; }

    public string LoanNumber { get; set; }

    public string UserName { get; set; }

    public RecaptureUserDecision UserDecision { get; set; }

    public DateTime UserDecisionTimeStamp { get; set; }

    public LockSnapshotRecapture()
    {
    }

    public LockSnapshotRecapture(string jsonRecapture)
    {
      try
      {
        string[] strArray1 = jsonRecapture.Split(',');
        if (strArray1.Length != 5)
          return;
        string[] strArray2 = strArray1[0].Split(':');
        if (strArray2.Length == 2)
          this.LoanGUID = strArray2[1].Replace("\"", "");
        string[] strArray3 = strArray1[1].Split(':');
        if (strArray3.Length == 2)
          this.LoanNumber = strArray3[1].Replace("\"", "");
        string[] strArray4 = strArray1[2].Split(':');
        if (strArray4.Length == 2)
          this.UserName = strArray4[1].Replace("\"", "");
        string[] strArray5 = strArray1[3].Split(':');
        if (strArray5.Length == 2)
        {
          string str = strArray5[1].Replace("\"", "");
          this.UserDecision = !str.Contains("Save Loan With Missing Lock Snapshot Data") ? (!str.Contains("Reopen Loan Without Saving Changes") ? RecaptureUserDecision.None : RecaptureUserDecision.ReopenLoan) : RecaptureUserDecision.SaveWithMissingData;
        }
        string[] strArray6 = strArray1[4].Split('"');
        if (strArray6.Length != 5)
          return;
        this.UserDecisionTimeStamp = Convert.ToDateTime(strArray6[3].Replace("\"", ""));
      }
      catch (Exception ex)
      {
      }
    }

    public string ToJson()
    {
      try
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("{");
        stringBuilder.Append("\"Loan GUID\":\"" + this.LoanGUID + "\", ");
        stringBuilder.Append("\"Loan Number\":\"" + this.LoanNumber + "\", ");
        stringBuilder.Append("\"User Name\":\"" + this.UserName + "\", ");
        stringBuilder.Append("\"User Decision\":\"" + this.UserDecision.ToDescription() + "\", ");
        stringBuilder.Append("\"Time Stamp\":\"" + this.UserDecisionTimeStamp.ToString() + "\"");
        stringBuilder.Append("}," + Environment.NewLine);
        return stringBuilder.ToString();
      }
      catch (Exception ex)
      {
        return string.Empty;
      }
    }
  }
}
