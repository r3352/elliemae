// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.HUDToleranceViolationAlertPanel
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.CalculationEngine;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class HUDToleranceViolationAlertPanel : AlertPanelBase
  {
    public HUDToleranceViolationAlertPanel(PipelineInfo.Alert alert) => this.SetAlert(alert);

    protected override void ConfigureTriggerFieldList()
    {
      this.gvFields.Columns.Clear();
      this.gvFields.Columns.Add("Field ID", 150);
      this.gvFields.Columns.Add("Description", 250);
      this.gvFields.Columns.Add("Disclosed GFE Value", 130);
      this.gvFields.Columns.Add("HUD Value", 80);
      this.gvFields.Columns.Add("Tolerance Limit", 100);
    }

    private string getCuredValue(string triggerField)
    {
      string curedValue = "";
      foreach (string[] tolereanceAlertField in HUDGFE2010Fields.HudTolereanceAlertFields)
      {
        if (tolereanceAlertField[0].Equals(triggerField, StringComparison.OrdinalIgnoreCase))
        {
          curedValue = Session.LoanData.GetField(tolereanceAlertField[1]);
          break;
        }
      }
      return curedValue;
    }

    private bool hasCuredValue()
    {
      bool flag = false;
      foreach (string[] tolereanceAlertField in HUDGFE2010Fields.HudTolereanceAlertFields)
      {
        if (Session.LoanData.GetField(tolereanceAlertField[1]) != "")
        {
          flag = true;
          break;
        }
      }
      return flag;
    }

    protected override void PopulateTriggerFields()
    {
      bool flag = this.hasCuredValue();
      foreach (string[] strArray in HUDGFE2010Fields.HUD1PG3_EXACTFIELDS)
      {
        string field = Session.LoanData.GetField(strArray[0]);
        string curedValue = this.getCuredValue(strArray[0]);
        Decimal num = !(strArray[0] == "NEWHUD.X76") ? Utils.ParseDecimal((object) Session.LoanData.GetField(strArray[1]), 0M) + Utils.ParseDecimal((object) Session.LoanData.GetField(strArray[2]), 0M) : Utils.ParseDecimal((object) Session.LoanData.GetField(strArray[1]), 0M);
        GVItem gvItem = new GVItem();
        gvItem.SubItems[0].Text = strArray[0];
        gvItem.SubItems[1].Text = EncompassFields.GetDescription(strArray[0], Session.LoanData.Settings.FieldSettings);
        gvItem.SubItems[2].Text = field == "" ? "0.00" : field;
        gvItem.SubItems[3].Text = num == 0M ? "0.00" : num.ToString("N2");
        gvItem.SubItems[4].Text = "0";
        if (!flag)
        {
          if (Utils.ParseDecimal((object) field) < Utils.ParseDecimal((object) num))
            gvItem.ForeColor = EncompassColors.Alert2;
        }
        else if (curedValue != field)
          gvItem.ForeColor = EncompassColors.Alert2;
        this.gvFields.Items.Add(gvItem);
      }
      GVItem gvItem1 = new GVItem();
      string field1 = Session.LoanData.GetField("NEWHUD.X312");
      string curedValue1 = this.getCuredValue("NEWHUD.X312");
      Decimal num1 = Utils.ParseDecimal((object) Session.LoanData.GetField("NEWHUD.X313"), 0M);
      string field2 = Session.LoanData.GetField("NEWHUD.X315");
      gvItem1.SubItems[0].Text = "NEWHUD.X312";
      gvItem1.SubItems[1].Text = EncompassFields.GetDescription("NEWHUD.X312", Session.LoanData.Settings.FieldSettings);
      gvItem1.SubItems[2].Text = field1 == "" ? "0.00" : field1;
      gvItem1.SubItems[3].Text = num1 == 0M ? "0.00" : num1.ToString("N2");
      gvItem1.SubItems[4].Text = "10%";
      if (!flag)
      {
        if (Utils.ParseDecimal((object) field2) > 10M)
          gvItem1.ForeColor = EncompassColors.Alert2;
        else if (Utils.ParseDecimal((object) field1) <= 0M && Utils.ParseDecimal((object) num1) > 0M)
          gvItem1.ForeColor = EncompassColors.Alert2;
      }
      else if (curedValue1 != field1)
        gvItem1.ForeColor = EncompassColors.Alert2;
      this.gvFields.Items.Add(gvItem1);
    }

    private void clearButton_Click(object sender, EventArgs e)
    {
      using (HUDToleranceResolutionDialog resolutionDialog = new HUDToleranceResolutionDialog())
      {
        if (resolutionDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
      }
      Session.Application.GetService<ILoanEditor>().OpenFormByID("HUD1PG3_2010");
    }

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);
      Button c = new Button();
      c.Text = "Cure Tolerance";
      c.AutoSize = true;
      c.Margin = new Padding(3, 0, 2, 0);
      c.Click += new EventHandler(this.clearButton_Click);
      this.AddHeaderControl((Control) c);
      int width = c.Width;
      c.AutoSize = false;
      c.Height = 22;
      c.Width = width;
    }
  }
}
