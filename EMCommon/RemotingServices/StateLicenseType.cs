// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.StateLicenseType
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  [Serializable]
  public class StateLicenseType
  {
    private string stateAbbrevation = string.Empty;
    private string licenseType = string.Empty;
    private bool selected;
    private bool exempt;

    public StateLicenseType()
    {
    }

    public StateLicenseType(
      string stateAbbrevation,
      string licenseType,
      bool selected,
      bool exempt)
    {
      this.stateAbbrevation = stateAbbrevation;
      this.licenseType = licenseType;
      this.selected = selected;
      this.exempt = exempt;
    }

    public string StateAbbrevation
    {
      get => this.stateAbbrevation;
      set => this.stateAbbrevation = value;
    }

    public string LicenseType
    {
      get => this.licenseType;
      set => this.licenseType = value;
    }

    public bool Selected
    {
      get => this.selected;
      set => this.selected = value;
    }

    public bool Exempt
    {
      get => this.exempt;
      set => this.exempt = value;
    }
  }
}
