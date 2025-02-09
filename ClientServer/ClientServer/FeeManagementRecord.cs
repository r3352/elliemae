// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.FeeManagementRecord
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class FeeManagementRecord
  {
    private int id = -1;
    private string feeName = string.Empty;
    private bool for700;
    private bool for800;
    private bool for900;
    private bool for1000;
    private bool for1100;
    private bool for1200;
    private bool for1300;
    private bool forPC;
    private string feeSource;
    private string feeNameInMavent;
    private string feeIDInMavent;
    private string feeNameInUCD;

    public FeeManagementRecord()
    {
      this.id = -1;
      this.feeName = string.Empty;
      this.for700 = false;
      this.for800 = false;
      this.for900 = false;
      this.for1000 = false;
      this.for1100 = false;
      this.for1200 = false;
      this.for1300 = false;
      this.ForPC = false;
      this.feeSource = string.Empty;
      this.feeNameInMavent = string.Empty;
      this.feeIDInMavent = string.Empty;
      this.feeNameInUCD = string.Empty;
    }

    public FeeManagementRecord(
      int id,
      string feeName,
      bool for700,
      bool for800,
      bool for900,
      bool for1000,
      bool for1100,
      bool for1200,
      bool for1300,
      bool forPC,
      string feeSource,
      string feeNameInMavent,
      string feeIDInMavent,
      string feeNameInUCD)
    {
      this.id = id;
      this.feeName = feeName;
      this.for700 = for700;
      this.for800 = for800;
      this.for900 = for900;
      this.for1000 = for1000;
      this.for1100 = for1100;
      this.for1200 = for1200;
      this.for1300 = for1300;
      this.forPC = forPC;
      this.feeSource = feeSource;
      this.feeNameInMavent = feeNameInMavent;
      this.feeIDInMavent = feeIDInMavent;
      this.feeNameInUCD = feeNameInUCD;
    }

    public FeeManagementRecord(
      string feeName,
      bool for700,
      bool for800,
      bool for900,
      bool for1000,
      bool for1100,
      bool for1200,
      bool for1300,
      bool forPC,
      string feeSource,
      string feeNameInMavent,
      string feeIDInMavent,
      string feeNameInUCD)
      : this(-1, feeName, for700, for800, for900, for1000, for1100, for1200, for1300, forPC, feeSource, feeNameInMavent, feeIDInMavent, feeNameInUCD)
    {
    }

    public bool Included(FeeSectionEnum sectionEnum)
    {
      if (sectionEnum == FeeSectionEnum.For700 && this.for700 || sectionEnum == FeeSectionEnum.For800 && this.for800 || sectionEnum == FeeSectionEnum.For900 && this.for900 || sectionEnum == FeeSectionEnum.For1000 && this.for1000 || sectionEnum == FeeSectionEnum.For1100 && this.for1100 || sectionEnum == FeeSectionEnum.For1200 && this.for1200 || sectionEnum == FeeSectionEnum.For1300 && this.for1300)
        return true;
      return sectionEnum == FeeSectionEnum.ForPC && this.forPC;
    }

    public void SetSection(FeeSectionEnum sectionEnum, bool enabled)
    {
      switch (sectionEnum)
      {
        case FeeSectionEnum.For700:
          this.SetSection(8, enabled);
          break;
        case FeeSectionEnum.For800:
          this.SetSection(1, enabled);
          break;
        case FeeSectionEnum.For900:
          this.SetSection(2, enabled);
          break;
        case FeeSectionEnum.For1000:
          this.SetSection(3, enabled);
          break;
        case FeeSectionEnum.For1100:
          this.SetSection(4, enabled);
          break;
        case FeeSectionEnum.For1200:
          this.SetSection(5, enabled);
          break;
        case FeeSectionEnum.For1300:
          this.SetSection(6, enabled);
          break;
        case FeeSectionEnum.ForPC:
          this.SetSection(7, enabled);
          break;
      }
    }

    public void SetSection(int i, bool enabled)
    {
      switch (i)
      {
        case 1:
          this.for700 = enabled;
          break;
        case 2:
          this.for800 = enabled;
          break;
        case 3:
          this.for900 = enabled;
          break;
        case 4:
          this.for1000 = enabled;
          break;
        case 5:
          this.for1100 = enabled;
          break;
        case 6:
          this.for1200 = enabled;
          break;
        case 7:
          this.for1300 = enabled;
          break;
        case 8:
          this.forPC = enabled;
          break;
      }
    }

    public void SetMaventFee(string feeName, string feeID)
    {
      this.feeNameInMavent = feeName;
      this.feeIDInMavent = feeID;
    }

    public void SetFeeSource(string feeSource) => this.feeSource = feeSource;

    public int Id
    {
      get => this.id;
      set => this.id = value;
    }

    public string FeeName
    {
      get => this.feeName;
      set => this.feeName = value;
    }

    public bool For700
    {
      get => this.for700;
      set => this.for700 = value;
    }

    public bool For800
    {
      get => this.for800;
      set => this.for800 = value;
    }

    public bool For900
    {
      get => this.for900;
      set => this.for900 = value;
    }

    public bool For1000
    {
      get => this.for1000;
      set => this.for1000 = value;
    }

    public bool For1100
    {
      get => this.for1100;
      set => this.for1100 = value;
    }

    public bool For1200
    {
      get => this.for1200;
      set => this.for1200 = value;
    }

    public bool For1300
    {
      get => this.for1300;
      set => this.for1300 = value;
    }

    public bool ForPC
    {
      get => this.forPC;
      set => this.forPC = value;
    }

    public string FeeSource
    {
      get => this.feeSource;
      set => this.feeSource = value;
    }

    public string FeeNameInMavent
    {
      get => this.feeNameInMavent;
      set => this.feeNameInMavent = value;
    }

    public string FeeIDInMavent
    {
      get => this.feeIDInMavent;
      set => this.feeIDInMavent = value;
    }

    public string FeeNameInUCD
    {
      get => this.feeNameInUCD;
      set => this.feeNameInUCD = value;
    }
  }
}
