// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.MersNumberingInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class MersNumberingInfo : ISerializable
  {
    public const int MaxCompanyIDLength = 7;
    public const int MaxNumberLength = 10;
    public const int MaxTotalLength = 17;
    private bool autoGenerate;
    private string companyId;
    private string nextNumber;

    public MersNumberingInfo(bool autoGenerate, string companyId, string nextNumber)
    {
      this.autoGenerate = autoGenerate;
      this.companyId = companyId;
      this.nextNumber = nextNumber;
      this.validate();
    }

    private MersNumberingInfo(SerializationInfo info, StreamingContext context)
    {
      this.autoGenerate = info.GetBoolean(nameof (autoGenerate));
      this.companyId = info.GetString(nameof (companyId));
      this.nextNumber = info.GetString(nameof (nextNumber));
      this.validate();
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      info.AddValue("autoGenerate", this.autoGenerate);
      info.AddValue("companyId", (object) this.companyId);
      info.AddValue("nextNumber", (object) this.nextNumber);
    }

    public bool AutoGenerate => this.autoGenerate;

    public string CompanyID => this.companyId;

    public string NextNumber => this.nextNumber;

    private void validate()
    {
      try
      {
        if (this.nextNumber.Trim() != string.Empty)
          long.Parse(this.nextNumber);
      }
      catch
      {
        throw new ArgumentException("Loan Number must represent an integer value", "nextNumber");
      }
      if (this.companyId.Length > 7)
        throw new ArgumentException("CompanyID exceeds maximum length", "companyId");
      if (this.nextNumber.Length > 10)
        throw new ArgumentException("NextNumber exceeds maximum length", "nextNumber");
    }
  }
}
