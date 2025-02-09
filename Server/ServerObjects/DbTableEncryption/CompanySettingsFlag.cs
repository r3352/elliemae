// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.DbTableEncryption.CompanySettingsFlag
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Common;
using System;
using System.Data.SqlClient;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.DbTableEncryption
{
  public class CompanySettingsFlag : SettingsFlag
  {
    public CompanySettingsFlag(
      SqlConnection dbConn,
      string instanceName,
      string category,
      string attribute)
      : base(dbConn, "[company_settings]", category, attribute)
    {
      this.InstanceName = instanceName;
      this.Init();
    }

    public string InstanceName { get; protected set; }

    protected static string ServerVersion { get; } = SystemUtil.GetServerChangesetNumber() ?? "";

    public DateTime? Timestamp
    {
      get
      {
        string flagValue = this.FlagValue;
        string[] strArray1;
        if (flagValue == null)
          strArray1 = (string[]) null;
        else
          strArray1 = flagValue.Split(new char[1]{ ',' }, StringSplitOptions.RemoveEmptyEntries);
        string[] strArray2 = strArray1;
        DateTime result;
        if (strArray2 != null && strArray2.Length != 0 && (DateTime.TryParse(strArray2[0], out result) || DateTime.TryParse(this.FlagValue, out result)))
        {
          if (strArray2.Length < 3)
            this.Timestamp = new DateTime?(result);
          else
            this.InstanceName = strArray2[1].ToUpperInvariant();
          return new DateTime?(result);
        }
        return !string.IsNullOrEmpty(this.FlagValue) ? new DateTime?(DateTime.MinValue) : new DateTime?();
      }
      set
      {
        this.FlagValue = !value.HasValue ? "" : string.Format("{0:o},{1},{2}", (object) value, (object) this.InstanceName, (object) CompanySettingsFlag.ServerVersion);
      }
    }
  }
}
