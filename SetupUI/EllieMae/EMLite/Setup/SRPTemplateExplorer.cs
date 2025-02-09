// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.SRPTemplateExplorer
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Trading;
using EllieMae.EMLite.UI;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class SRPTemplateExplorer : SimpleTemplateExplorer
  {
    private Sessions.Session session;

    public SRPTemplateExplorer() => this.session = Session.DefaultInstance;

    public SRPTemplateExplorer(Sessions.Session session, bool allowMultiSelect)
      : base(session, allowMultiSelect)
    {
      this.session = session;
    }

    protected override TemplateSettingsType TemplateType => TemplateSettingsType.SRPTable;

    protected override string HeaderText => "SRP Templates";

    protected override void ConfigureTemplateListView(GridView listView)
    {
      listView.Columns.Add("Description", 300).Tag = (object) "Description";
      listView.Sort(0, SortOrder.Ascending);
    }

    protected override bool CreateNew()
    {
      using (SRPTemplateEditor srpTemplateEditor = new SRPTemplateEditor(new SRPTableTemplate(), this.session))
        return srpTemplateEditor.ShowDialog() == DialogResult.OK;
    }

    protected override bool Edit(BinaryObject template)
    {
      using (SRPTemplateEditor srpTemplateEditor = new SRPTemplateEditor((SRPTableTemplate) template, this.session))
        return srpTemplateEditor.ShowDialog() == DialogResult.OK;
    }

    protected override BinaryObject Duplicate(BinaryObject template)
    {
      return (BinaryObject) (BinaryConvertibleObject) ((SRPTableTemplate) template).Duplicate();
    }
  }
}
