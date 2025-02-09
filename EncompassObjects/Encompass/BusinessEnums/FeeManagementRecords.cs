// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.FeeManagementRecords
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.Encompass.Client;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  /// <summary>
  /// The FeeManagementRecords class represents all of the fees that exist in the exist in the
  /// 2010 Itemization Fee Management section of the Encompass settings
  /// </summary>
  public class FeeManagementRecords : EnumBase, IFeeManagementRecords
  {
    private Session session;
    private IConfigurationManager configMngr;

    internal FeeManagementRecords(Session session)
    {
      this.session = session;
      this.configMngr = (IConfigurationManager) session.GetObject("ConfigurationManager");
      FeeManagementSetting feeManagement = this.configMngr.GetFeeManagement();
      int id = 0;
      if (feeManagement == null)
        return;
      foreach (EllieMae.EMLite.ClientServer.FeeManagementRecord allFee in feeManagement.GetAllFees())
      {
        this.AddItem((EnumItem) new FeeManagementRecord(id, allFee));
        ++id;
      }
    }

    /// <summary>
    /// Provides access to the <see cref="T:EllieMae.Encompass.BusinessEnums.FeeManagementRecord">FeeManagementRecord</see> with the specified name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public FeeManagementRecord this[string name] => (FeeManagementRecord) this.GetItemByName(name);
  }
}
