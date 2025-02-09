// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.WarehouseLenderUI
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using System.Collections;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  internal class WarehouseLenderUI : IComparer
  {
    private InvestorServiceAclInfo investorServiceAclInfo;
    private int displayOrder;
    private string displayName;

    public WarehouseLenderUI(
      InvestorServiceAclInfo investorServiceAclInfo,
      string displayName,
      int displayOrder)
    {
      this.investorServiceAclInfo = investorServiceAclInfo;
      this.displayName = displayName;
      this.displayOrder = displayOrder;
    }

    public string DisplayName => this.displayName;

    public string ProviderCompanyCode => this.investorServiceAclInfo.ProviderCompanyCode;

    public InvestorServiceAclInfo InvestorServiceAclInfo
    {
      get => this.investorServiceAclInfo;
      set => this.investorServiceAclInfo = value;
    }

    public int DisplayOrder => this.displayOrder;

    public int Compare(object x, object y)
    {
      ListViewItem listViewItem1 = (ListViewItem) x;
      ListViewItem listViewItem2 = (ListViewItem) y;
      WarehouseLenderUI warehouseLenderUi1 = (WarehouseLenderUI) (listViewItem1.Tag ?? (object) null);
      WarehouseLenderUI warehouseLenderUi2 = (WarehouseLenderUI) (listViewItem2.Tag ?? (object) null);
      if (warehouseLenderUi1 == null && warehouseLenderUi2 == null || warehouseLenderUi1.DisplayOrder == warehouseLenderUi2.DisplayOrder)
        return 0;
      return warehouseLenderUi1 == null || warehouseLenderUi2 != null && warehouseLenderUi1.DisplayOrder >= warehouseLenderUi2.DisplayOrder && warehouseLenderUi1.DisplayOrder > warehouseLenderUi2.DisplayOrder ? 1 : -1;
    }
  }
}
