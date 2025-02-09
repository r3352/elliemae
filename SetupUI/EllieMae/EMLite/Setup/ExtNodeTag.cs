// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ExtNodeTag
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  [Serializable]
  public class ExtNodeTag
  {
    private int oid;
    private string companyName;
    private bool allowAccess;
    private string companyType;

    public int Oid => this.oid;

    public string CompanyName => this.companyName;

    public bool AllowAccess
    {
      get => this.allowAccess;
      set => this.allowAccess = value;
    }

    public string CompanyType
    {
      get => this.companyType;
      set => this.companyType = value;
    }

    public ExtNodeTag(int oid, string CompanyName, string companyType)
    {
      this.oid = oid;
      this.companyName = CompanyName;
      this.companyType = companyType;
    }
  }
}
