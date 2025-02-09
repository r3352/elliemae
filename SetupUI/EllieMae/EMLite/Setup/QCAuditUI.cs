// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.QCAuditUI
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using System.Collections;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  internal class QCAuditUI : IComparer
  {
    private InvestorServiceAclInfo investorServiceAclInfo;
    private int displayOrder;
    private string displayName;

    public QCAuditUI(
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
      QCAuditUI qcAuditUi1 = (QCAuditUI) (listViewItem1.Tag ?? (object) null);
      QCAuditUI qcAuditUi2 = (QCAuditUI) (listViewItem2.Tag ?? (object) null);
      if (qcAuditUi1 == null && qcAuditUi2 == null || qcAuditUi1.DisplayOrder == qcAuditUi2.DisplayOrder)
        return 0;
      return qcAuditUi1 == null || qcAuditUi2 != null && qcAuditUi1.DisplayOrder >= qcAuditUi2.DisplayOrder && qcAuditUi1.DisplayOrder > qcAuditUi2.DisplayOrder ? 1 : -1;
    }
  }
}
