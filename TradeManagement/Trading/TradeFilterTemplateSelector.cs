// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.TradeFilterTemplateSelector
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.UI;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class TradeFilterTemplateSelector : SimpleTemplateSelector
  {
    public TradeFilterTemplateSelector()
      : base(EllieMae.EMLite.ClientServer.TemplateSettingsType.TradeFilter, false)
    {
      this.Text = "Select Saved Search";
    }

    protected override void ConfigureTemplateGridView(GridView listView)
    {
      listView.Columns[1].Text = "Type";
      listView.Columns[1].Tag = (object) "Type";
    }
  }
}
