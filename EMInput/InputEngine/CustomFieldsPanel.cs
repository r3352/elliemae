// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.CustomFieldsPanel
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using System;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class CustomFieldsPanel : QuickLinksControl
  {
    private LoanScreen loanScreen;

    public CustomFieldsPanel(LoanScreen screen, int page, Sessions.Session session)
      : base((ILoanEditor) null, CustomFieldsPanel.createCustomFieldLinks(session), (string) null, session)
    {
      this.loanScreen = screen;
      this.LoadCustomFieldPage(page);
    }

    public CustomFieldsPanel(LoanScreen screen, Sessions.Session session)
      : this(screen, 1, session)
    {
    }

    protected override void OnQuickLinkClick(object sender, EventArgs e)
    {
      QuickLinkLabel quickLinkLabel = (QuickLinkLabel) sender;
      if (quickLinkLabel.Text == "Page 2")
        this.LoadCustomFieldPage(2);
      else if (quickLinkLabel.Text == "Page 3")
        this.LoadCustomFieldPage(3);
      else if (quickLinkLabel.Text == "Page 4")
        this.LoadCustomFieldPage(4);
      else
        this.LoadCustomFieldPage(1);
    }

    public void LoadCustomFieldPage(int index)
    {
      this.loanScreen.LoadCustomFields(index);
      this.SetSelectedForm("CF_" + (object) index);
    }

    private static QuickLink[] createCustomFieldLinks(Sessions.Session session)
    {
      return QuickLinksControl.GetQuickLinksForForm("CUSTOMFIELDS", session);
    }
  }
}
