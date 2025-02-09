// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.ContactSearchPage
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.Query;
using System.Drawing;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class ContactSearchPage : ContactAdvancedSearchForm
  {
    private System.ComponentModel.Container components;
    protected ContactQuery _StaticQuery = new ContactQuery();

    public ContactSearchPage() => this.InitializeComponent();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      this.Size = new Size(300, 300);
      this.Text = nameof (ContactSearchPage);
    }

    public string Description => this.description;

    public ContactQuery StaticQuery
    {
      get => this._StaticQuery;
      set
      {
        this._StaticQuery = value;
        this.loadQuery();
      }
    }

    protected virtual void loadQuery()
    {
    }

    public virtual void Reset()
    {
    }

    public virtual ContactQuery GetSearchCriteria() => (ContactQuery) null;

    public virtual void LoadQuery(
      QueryCriterion[] defaultCriteria,
      RelatedLoanMatchType defaultMatchType)
    {
    }

    public virtual void SetLoanMatchType(RelatedLoanMatchType matchType)
    {
    }

    public virtual bool ValidateUserInput() => true;
  }
}
