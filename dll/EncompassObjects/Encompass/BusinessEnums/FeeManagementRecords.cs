// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.FeeManagementRecords
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.Encompass.Client;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
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
      foreach (FeeManagementRecord allFee in feeManagement.GetAllFees())
      {
        this.AddItem((EnumItem) new FeeManagementRecord(id, allFee));
        ++id;
      }
    }

    public FeeManagementRecord this[string name] => (FeeManagementRecord) this.GetItemByName(name);
  }
}
