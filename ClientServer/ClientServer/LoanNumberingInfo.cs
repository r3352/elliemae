// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.LoanNumberingInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class LoanNumberingInfo : ISerializable
  {
    public const int MaxPrefixLength = 18;
    public const int MaxSuffixLength = 18;
    public const int MaxNumberLength = 9;
    public const int MaxTotalLength = 18;
    private string prefix;
    private string suffix;
    private string nextNumber;
    private bool useOrgCode;
    private bool useMonth;
    private bool useYear;

    public LoanNumberingInfo(string prefix, string nextNumber, string suffix)
      : this(prefix, nextNumber, suffix, false, false, false)
    {
    }

    public LoanNumberingInfo(
      string prefix,
      string nextNumber,
      string suffix,
      bool useOrgCode,
      bool useMonth,
      bool useYear)
    {
      this.prefix = prefix;
      this.suffix = suffix;
      this.nextNumber = nextNumber;
      this.useOrgCode = useOrgCode;
      this.useMonth = useMonth;
      this.useYear = useYear;
      this.validate();
    }

    private LoanNumberingInfo(SerializationInfo info, StreamingContext context)
    {
      this.prefix = info.GetString(nameof (prefix));
      this.suffix = info.GetString(nameof (suffix));
      this.nextNumber = info.GetString(nameof (nextNumber));
      this.useOrgCode = info.GetBoolean(nameof (useOrgCode));
      this.useMonth = info.GetBoolean(nameof (useMonth));
      this.useYear = info.GetBoolean(nameof (useYear));
      this.validate();
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      info.AddValue("prefix", (object) this.prefix);
      info.AddValue("suffix", (object) this.suffix);
      info.AddValue("nextNumber", (object) this.nextNumber);
      info.AddValue("useOrgCode", this.useOrgCode);
      info.AddValue("useMonth", this.useMonth);
      info.AddValue("useYear", this.useYear);
    }

    public string Prefix => this.prefix;

    public string Suffix => this.suffix;

    public string NextNumber => this.nextNumber;

    public bool UseOrgCode => this.useOrgCode;

    public bool UseMonth => this.useMonth;

    public bool UseYear => this.useYear;

    private void validate()
    {
      try
      {
        int.Parse(this.nextNumber);
      }
      catch
      {
        throw new ArgumentException("Loan Number must represent an integer value", "nextNumber");
      }
      if (this.prefix.Length > 18)
        throw new ArgumentException("Prefix exceeds maximum length", "prefix");
      if (this.suffix.Length > 18)
        throw new ArgumentException("Suffix exceeds maximum length", "suffix");
      if (this.nextNumber.Length > 9)
        throw new ArgumentException("NextNumber exceeds maximum length", "nextNumber");
      if (this.prefix.Length + this.nextNumber.Length + this.suffix.Length > 18)
        throw new ArgumentException("Maximum total length exceeded");
    }
  }
}
