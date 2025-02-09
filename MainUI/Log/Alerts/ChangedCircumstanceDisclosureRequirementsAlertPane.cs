// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.Alerts.ChangedCircumstanceDisclosureRequirementsAlertPanel
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log.Alerts
{
  public class ChangedCircumstanceDisclosureRequirementsAlertPanel : 
    AlertPanelWithDataCompletionFields
  {
    private IContainer components;

    public ChangedCircumstanceDisclosureRequirementsAlertPanel(PipelineInfo.Alert alert)
    {
      this.SetAlert(alert);
      this.AlertDateCaption = "Changes Received Date";
      this.SetAlertDate(Utils.ParseDate((object) Session.LoanData.GetField("3165")));
      Session.LoanData?.Calculator?.CalcChangeCircumstanceRequirementsDate();
      this.SetDataCompletionDateControls("Disclosure Ready Date", Utils.ParseDate((object) Session.LoanData.GetField("5007")));
      if (Utils.CheckIf2015RespaTila(Session.LoanData.GetField("3969")) && alert.AlertID == 48)
      {
        this.ShowAlternateDate("Revised CD Due Date", Utils.ParseDate((object) Session.LoanData.GetField("CD1.X63")).Date);
        if (Utils.IsDate((object) Session.LoanData.GetField("CD1.X62")))
          this.SetAlertDate(Convert.ToDateTime(Session.LoanData.GetField("CD1.X62")));
        this.RemoveCDChangeCircumstanceTriggerFields();
      }
      else if (Utils.CheckIf2015RespaTila(Session.LoanData.GetField("3969")))
        this.ShowAlternateDate("Revised LE Due Date", alert.Date);
      else
        this.ShowAlternateDate("Revised GFE Due Date", alert.Date);
    }

    protected void RemoveCDChangeCircumstanceTriggerFields()
    {
      List<string> stringList = new List<string>();
      if (Session.LoanData.GetField("CD1.X61") != "Y" || Session.LoanData.GetField("CD1.X57") != "Y" && Session.LoanData.GetField("CD1.X55") != "Y" && Session.LoanData.GetField("CD1.X58") != "Y" && Session.LoanData.GetField("CD1.X59") != "Y" && Session.LoanData.GetField("CD1.X68") != "Y" && Session.LoanData.GetField("CD1.X66") != "Y" && Session.LoanData.GetField("CD1.X67") != "Y")
      {
        stringList.Add("CD1.X61");
        stringList.Add("CD1.X62");
      }
      foreach (string str in stringList)
      {
        GVItem gvItem1 = (GVItem) null;
        foreach (GVItem gvItem2 in (IEnumerable<GVItem>) this.gvFields.Items)
        {
          if (gvItem2.SubItems[0].Text == str)
          {
            gvItem1 = gvItem2;
            break;
          }
        }
        if (gvItem1 != null)
          this.gvFields.Items.Remove(gvItem1);
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      this.AutoScaleMode = AutoScaleMode.Font;
    }
  }
}
