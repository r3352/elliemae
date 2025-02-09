// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.ChangedCircumstancesAlertPanel
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class ChangedCircumstancesAlertPanel : AlertPanelBase
  {
    public ChangedCircumstancesAlertPanel(PipelineInfo.Alert alert)
    {
      this.SetAlert(alert);
      this.AlertDateCaption = "Changes Received Date";
      this.SetAlertDate(Utils.ParseDate((object) Session.LoanData.GetField("3165")));
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

    private void InitializeComponent()
    {
      this.grpFields.SuspendLayout();
      this.pnlAltDate.SuspendLayout();
      this.grpDetails.SuspendLayout();
      this.pnlFields.SuspendLayout();
      this.SuspendLayout();
      this.grpFields.Size = new Size(903, 336);
      this.gvFields.Size = new Size(1360, 550);
      this.pnlAltDate.Location = new Point(218, 96);
      this.pnlAltDate.Size = new Size(199, 21);
      this.grpDetails.Size = new Size(1362, 126);
      this.txtAlertDate.Location = new Point(71, 96);
      this.txtAltDate.Location = new Point(62, 0);
      this.txtMessage.Location = new Point(71, 58);
      this.txtMessage.Size = new Size(1282, 36);
      this.txtAlertName.Location = new Point(71, 36);
      this.txtAlertName.Size = new Size(1282, 20);
      this.pnlFields.Size = new Size(903, 336);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.Name = nameof (ChangedCircumstancesAlertPanel);
      this.Size = new Size(1362, 703);
      this.grpFields.ResumeLayout(false);
      this.pnlAltDate.ResumeLayout(false);
      this.pnlAltDate.PerformLayout();
      this.grpDetails.ResumeLayout(false);
      this.grpDetails.PerformLayout();
      this.pnlFields.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
