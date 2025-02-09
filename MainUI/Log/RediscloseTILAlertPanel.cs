// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.RediscloseTILAlertPanel
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class RediscloseTILAlertPanel : AlertPanelBase
  {
    private Dictionary<string, string> currentToDisclosedMapping = new Dictionary<string, string>();

    public RediscloseTILAlertPanel(PipelineInfo.Alert alert)
    {
      this.currentToDisclosedMapping.Add("3121", "799");
      this.currentToDisclosedMapping.Add("675", "4018");
      this.currentToDisclosedMapping.Add("LE1.X5", "4017");
      this.SetAlert(alert);
    }

    protected override void ConfigureTriggerFieldList()
    {
      this.gvFields.Columns.Clear();
      this.gvFields.Columns.Add("Field ID", 150);
      this.gvFields.Columns.Add("Description", 250);
      this.gvFields.Columns.Add("Disclosed Value", 80);
      this.gvFields.Columns.Add("Current Value", 80);
    }

    protected override void PopulateTriggerFields()
    {
      if (this.Alert.AlertID != 49)
      {
        this.gvFields.Items.Add(new GVItem()
        {
          SubItems = {
            [0] = {
              Text = "3121"
            },
            [1] = {
              Text = EncompassFields.GetDescription("3121", Session.LoanData.Settings.FieldSettings)
            },
            [2] = {
              Text = Session.LoanData.GetField("3121")
            },
            [3] = {
              Text = Session.LoanData.GetField("799")
            }
          }
        });
      }
      else
      {
        if (this.Alert.AlertID != 49)
          return;
        base.PopulateTriggerFields();
        this.RemoveCDChangeCircumstanceTriggerFields();
      }
    }

    protected override GVItem CreateTriggerFieldItem(string fieldId)
    {
      GVItem triggerFieldItem = new GVItem();
      triggerFieldItem.SubItems[0].Text = fieldId;
      triggerFieldItem.SubItems[1].Text = EncompassFields.GetDescription(fieldId, Session.LoanDataMgr.SystemConfiguration.LoanSettings.FieldSettings);
      string id = "";
      if (fieldId != "CD1.X47")
        this.currentToDisclosedMapping.TryGetValue(fieldId, out id);
      if (fieldId == "3121")
      {
        triggerFieldItem.SubItems[3].Text = Session.LoanDataMgr.LoanData.GetField(id);
        triggerFieldItem.SubItems[2].Text = Session.LoanDataMgr.LoanData.GetField(fieldId);
      }
      else
      {
        triggerFieldItem.SubItems[2].Text = Session.LoanDataMgr.LoanData.GetField(id);
        triggerFieldItem.SubItems[3].Text = Session.LoanDataMgr.LoanData.GetField(fieldId);
      }
      return triggerFieldItem;
    }

    protected void RemoveCDChangeCircumstanceTriggerFields()
    {
      double num1 = Utils.ParseDouble((object) Session.LoanData.GetField("3121"));
      double num2 = Utils.ParseDouble((object) Session.LoanData.GetField("799"));
      List<string> stringList = new List<string>();
      if (!(Session.LoanData.GetField("4018") == "N") || !(Session.LoanData.GetField("675") == "Y"))
        stringList.Add("675");
      else if (!Session.LoanData.IsLocked("4018") && !Utils.IsDate((object) Session.LoanData.GetField("CD1.X47")))
        stringList.Add("675");
      if (Session.LoanData.GetField("4017") == Session.LoanData.GetField("LE1.X5") || Session.LoanData.GetField("4017") == string.Empty)
        stringList.Add("LE1.X5");
      else if (!Session.LoanData.IsLocked("4017") && !Utils.IsDate((object) Session.LoanData.GetField("CD1.X47")))
        stringList.Add("LE1.X5");
      double num3 = 0.125;
      bool complianceSetting1 = (bool) Session.LoanData.Settings.ComplianceSettings[(object) "Compliance.ApplyFixedAPRToleranceToARMs"];
      string field = Session.LoanData.GetField("19");
      if (!complianceSetting1 && (Session.LoanData.GetField("608") == "AdjustableRate" || field == "ConstructionOnly" || field == "ConstructionToPermanent"))
        num3 = 0.25;
      bool complianceSetting2 = (bool) Session.LoanData.Settings.ComplianceSettings[(object) "Compliance.SuppressNegativeAPRAlert"];
      double num4 = Utils.ArithmeticRounding(num2 - num1, 3);
      if (num1 != 0.0)
      {
        if (complianceSetting2 && num4 <= num3)
          stringList.Add("3121");
        else if (!complianceSetting2 && Math.Abs(num4) <= num3)
          stringList.Add("3121");
        else if (!Session.LoanData.IsLocked("3121") && !Utils.IsDate((object) Session.LoanData.GetField("CD1.X47")))
          stringList.Add("3121");
      }
      else if (!Session.LoanData.IsLocked("3121") && !Utils.IsDate((object) Session.LoanData.GetField("CD1.X47")))
        stringList.Add("3121");
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvFields.Items)
      {
        GVItem item = gvItem;
        if (stringList.Find((Predicate<string>) (t => t.Equals(item.SubItems[0].Text))) == null && item.SubItems[0].Text != "CD1.X47")
          item.ForeColor = EncompassColors.Alert2;
      }
    }

    protected override void GoToField(string fieldId)
    {
      ILoanEditor service = Session.Application.GetService<ILoanEditor>();
      if (!Utils.CheckIf2015RespaTila(Session.LoanData.GetField("3969")))
      {
        if (service.OpenFormByID("REGZ50"))
          return;
        base.GoToField(fieldId);
      }
      else
      {
        if (service.OpenFormByID("REGZCD"))
          return;
        base.GoToField(fieldId);
      }
    }

    private void InitializeComponent()
    {
      this.pnlAltDate.SuspendLayout();
      this.grpDetails.SuspendLayout();
      this.SuspendLayout();
      this.gvFields.Size = new Size(1358, 727);
      this.pnlAltDate.Location = new Point(218, 96);
      this.pnlAltDate.Size = new Size(199, 21);
      this.grpDetails.Size = new Size(1360, 126);
      this.txtAlertDate.Location = new Point(71, 96);
      this.txtAltDate.Location = new Point(62, 0);
      this.txtMessage.Location = new Point(71, 58);
      this.txtMessage.Size = new Size(1280, 36);
      this.txtAlertName.Location = new Point(71, 36);
      this.txtAlertName.Size = new Size(1280, 20);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.Name = nameof (RediscloseTILAlertPanel);
      this.Size = new Size(1360, 880);
      this.pnlAltDate.ResumeLayout(false);
      this.pnlAltDate.PerformLayout();
      this.grpDetails.ResumeLayout(false);
      this.grpDetails.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
