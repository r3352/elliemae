// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.FeeManagementRecord
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  public class FeeManagementRecord : EnumItem, IFeeManagementRecord
  {
    private FeeManagementRecord managementRecord;

    internal FeeManagementRecord(int id, FeeManagementRecord managementRecord)
      : base(id, managementRecord.FeeName)
    {
      this.managementRecord = managementRecord;
    }

    public string MaventFeeName => this.managementRecord.FeeNameInMavent;

    public string FeeSource => this.managementRecord.FeeSource;

    public bool For700 => this.managementRecord.For700;

    public bool For800 => this.managementRecord.For800;

    public bool For900 => this.managementRecord.For900;

    public bool For1000 => this.managementRecord.For1000;

    public bool For1100 => this.managementRecord.For1100;

    public bool For1200 => this.managementRecord.For1200;

    public bool For1300 => this.managementRecord.For1300;

    public bool ForPC => this.managementRecord.ForPC;

    internal FeeManagementRecord Unwrap() => this.managementRecord;
  }
}
