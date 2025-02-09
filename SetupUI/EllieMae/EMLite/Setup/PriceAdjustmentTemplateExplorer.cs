// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PriceAdjustmentTemplateExplorer
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
  public class PriceAdjustmentTemplateExplorer : SimpleTemplateExplorer
  {
    private Sessions.Session session;

    public PriceAdjustmentTemplateExplorer() => this.session = Session.DefaultInstance;

    public PriceAdjustmentTemplateExplorer(Sessions.Session session, bool allowMultiSelect)
      : base(session, allowMultiSelect)
    {
      this.session = session;
    }

    protected override TemplateSettingsType TemplateType
    {
      get => TemplateSettingsType.TradePriceAdjustment;
    }

    protected override string HeaderText => "Adjustment Templates";

    protected override void ConfigureTemplateListView(GridView listView)
    {
      listView.Columns.Add("Description", 300).Tag = (object) "Description";
      listView.Sort(0, SortOrder.Ascending);
    }

    protected override bool CreateNew()
    {
      using (PriceAdjustmentTemplateEditor adjustmentTemplateEditor = new PriceAdjustmentTemplateEditor(this.session))
        return adjustmentTemplateEditor.ShowDialog() == DialogResult.OK;
    }

    protected override bool Edit(BinaryObject template)
    {
      using (PriceAdjustmentTemplateEditor adjustmentTemplateEditor = new PriceAdjustmentTemplateEditor((PriceAdjustmentTemplate) template, this.session))
        return adjustmentTemplateEditor.ShowDialog() == DialogResult.OK;
    }

    protected override BinaryObject Duplicate(BinaryObject template)
    {
      return (BinaryObject) (BinaryConvertibleObject) ((PriceAdjustmentTemplate) template).Duplicate();
    }
  }
}
