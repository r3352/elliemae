// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.InvestorTemplateExplorer
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class InvestorTemplateExplorer : SimpleTemplateExplorer
  {
    private Sessions.Session session;

    public InvestorTemplateExplorer()
      : this(Session.DefaultInstance, false)
    {
    }

    public InvestorTemplateExplorer(Sessions.Session session, bool allowMultiSelect)
      : base(session, allowMultiSelect)
    {
      this.session = session;
    }

    protected override TemplateSettingsType TemplateType => TemplateSettingsType.Investor;

    protected override string HeaderText => "Investor Templates";

    protected override void ConfigureTemplateListView(GridView listView)
    {
      listView.Columns.Add("Bulk Sale", 100).Tag = (object) "BulkSale";
      listView.Columns.Add("Delivery Time Frame", 200).Tag = (object) "DeliveryTimeFrame";
      listView.Columns.Add("HMDA Information - Type of Purchaser", 250).Tag = (object) "TypeOfPurchaser";
      listView.Sort(0, SortOrder.Ascending);
    }

    protected override void UpdateTemplateProperties(FileSystemEntry fsEntry)
    {
      float single = Utils.ParseSingle(fsEntry.Properties[(object) "PairOffFee"]);
      if ((double) single == 0.0)
        fsEntry.Properties[(object) "PairOffFee"] = (object) "";
      else
        fsEntry.Properties[(object) "PairOffFee"] = (object) (single.ToString() + "%");
      foreach (FieldOption option in EncompassFields.GetField("1397").Options)
      {
        try
        {
          if (option.Value.Equals(fsEntry.Properties[(object) "TypeOfPurchaser"].ToString(), StringComparison.CurrentCultureIgnoreCase))
          {
            fsEntry.Properties[(object) "TypeOfPurchaser"] = (object) option.Text;
            break;
          }
        }
        catch
        {
        }
      }
    }

    protected override bool CreateNew()
    {
      using (InvestorTemplateEditor investorTemplateEditor = new InvestorTemplateEditor(this.session))
        return investorTemplateEditor.ShowDialog() == DialogResult.OK;
    }

    protected override bool Edit(BinaryObject template)
    {
      using (InvestorTemplateEditor investorTemplateEditor = new InvestorTemplateEditor(this.session, (InvestorTemplate) template))
        return investorTemplateEditor.ShowDialog() == DialogResult.OK;
    }

    protected override BinaryObject Duplicate(BinaryObject template)
    {
      return (BinaryObject) (BinaryConvertibleObject) ((InvestorTemplate) template).Duplicate();
    }
  }
}
