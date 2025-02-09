// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.FeeManagementRecord
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  /// <summary>
  /// The FeeManagementRecord represents a single 2010 Itemization Fee as defined in Encompass settings
  /// </summary>
  public class FeeManagementRecord : EnumItem, IFeeManagementRecord
  {
    private EllieMae.EMLite.ClientServer.FeeManagementRecord managementRecord;

    internal FeeManagementRecord(int id, EllieMae.EMLite.ClientServer.FeeManagementRecord managementRecord)
      : base(id, managementRecord.FeeName)
    {
      this.managementRecord = managementRecord;
    }

    /// <summary>Gets the name of the mapped Mavent compliance fee.</summary>
    public string MaventFeeName => this.managementRecord.FeeNameInMavent;

    /// <summary>Get the source (user) fee.</summary>
    public string FeeSource => this.managementRecord.FeeSource;

    /// <summary>
    /// Gets a flag indicating if this fee is used for HUD Section 700
    /// </summary>
    public bool For700 => this.managementRecord.For700;

    /// <summary>
    /// Gets a flag indicating if this fee is used for HUD Section 800
    /// </summary>
    public bool For800 => this.managementRecord.For800;

    /// <summary>
    /// Gets a flag indicating if this fee is used for HUD Section 900
    /// </summary>
    public bool For900 => this.managementRecord.For900;

    /// <summary>
    /// Gets a flag indicating if this fee is used for HUD Section 1000
    /// </summary>
    public bool For1000 => this.managementRecord.For1000;

    /// <summary>
    /// Gets a flag indicating if this fee is used for HUD Section 1100
    /// </summary>
    public bool For1100 => this.managementRecord.For1100;

    /// <summary>
    /// Gets a flag indicating if this fee is used for HUD Section 1200
    /// </summary>
    public bool For1200 => this.managementRecord.For1200;

    /// <summary>
    /// Gets a flag indicating if this fee is used for HUD Section 1300
    /// </summary>
    public bool For1300 => this.managementRecord.For1300;

    /// <summary>
    /// Gets a flag indicating if this fee is used for HUD Section PC
    /// </summary>
    public bool ForPC => this.managementRecord.ForPC;

    /// <summary>Provides access to the underlying FeeManagementRecord</summary>
    internal EllieMae.EMLite.ClientServer.FeeManagementRecord Unwrap() => this.managementRecord;
  }
}
