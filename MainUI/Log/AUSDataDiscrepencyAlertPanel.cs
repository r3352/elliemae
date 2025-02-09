// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.AUSDataDiscrepencyAlertPanel
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class AUSDataDiscrepencyAlertPanel : AlertPanelBase
  {
    public AUSDataDiscrepencyAlertPanel(PipelineInfo.Alert alert) => this.SetAlert(alert);

    protected override void ConfigureTriggerFieldList()
    {
      this.gvFields.Columns.Clear();
      this.gvFields.Columns.Add("Trigger Field", 300);
      this.gvFields.Columns.Add("Description", 250);
      this.gvFields.Columns.Add("Loan Value", 130);
      this.gvFields.Columns.Add("AUS Value", 80);
    }

    protected override void PopulateTriggerFields()
    {
      GVItem gvItem = (GVItem) null;
      RegulationAlert definition = (RegulationAlert) StandardAlerts.GetDefinition(StandardAlertID.AUSDataDiscrepancyAlert);
      bool hasCuredValue = RegulationAlerts.HasAUSCuredValue(Session.LoanData);
      if (Session.LoanData.GetAUSTrackingHistoryList().HistoryCount == 0)
        return;
      AUSTrackingHistoryLog trackingLog = (AUSTrackingHistoryLog) null;
      for (int i = 0; i < Session.LoanData.GetAUSTrackingHistoryList().HistoryCount; ++i)
      {
        AUSTrackingHistoryLog historyAt = Session.LoanData.GetAUSTrackingHistoryList().GetHistoryAt(i);
        List<string> stringList = new List<string>()
        {
          "LQA",
          "EarlyCheck"
        };
        string field = historyAt.DataValues.GetField("AUS.X1");
        if (historyAt != null && !stringList.Contains(field))
        {
          trackingLog = historyAt;
          break;
        }
      }
      if (trackingLog == null)
        return;
      string compareFieldID = "";
      foreach (AlertTriggerField alertTriggerField in definition.TriggerFields.ToArray())
      {
        AlertTriggerField field = alertTriggerField;
        string[] strArray = (string[]) null;
        if ((string.Equals(field.FieldID, "MORNET.X158", StringComparison.InvariantCultureIgnoreCase) || string.Equals(field.FieldID, "MORNET.X159", StringComparison.InvariantCultureIgnoreCase) || string.Equals(field.FieldID, "MORNET.X160", StringComparison.InvariantCultureIgnoreCase) || string.Equals(field.FieldID, "4752", StringComparison.InvariantCultureIgnoreCase)) && (string.Equals(trackingLog.RecordType, "DU", StringComparison.InvariantCultureIgnoreCase) || string.Equals(trackingLog.SubmissionType, "DU", StringComparison.InvariantCultureIgnoreCase)))
        {
          if (string.Equals(field.FieldID, "MORNET.X158", StringComparison.InvariantCultureIgnoreCase))
            strArray = new string[3]
            {
              "MORNET.X158",
              "AUS.X14",
              "AUS.CuredX14"
            };
          else if (string.Equals(field.FieldID, "MORNET.X159", StringComparison.InvariantCultureIgnoreCase))
            strArray = new string[3]
            {
              "MORNET.X159",
              "AUS.X15",
              "AUS.CuredX15"
            };
          else if (string.Equals(field.FieldID, "MORNET.X160", StringComparison.InvariantCultureIgnoreCase))
            strArray = new string[3]
            {
              "MORNET.X160",
              "AUS.X41",
              "AUS.CuredX41"
            };
          else if (string.Equals(field.FieldID, "4752", StringComparison.InvariantCultureIgnoreCase))
            strArray = new string[3]
            {
              "4752",
              "AUS.X199",
              "AUS.CuredX199"
            };
        }
        else if (!string.Equals(field.FieldID, "740", StringComparison.InvariantCultureIgnoreCase) && !string.Equals(field.FieldID, "742", StringComparison.InvariantCultureIgnoreCase) && !string.Equals(field.FieldID, "1389", StringComparison.InvariantCultureIgnoreCase) || !string.Equals(trackingLog.RecordType, "DU", StringComparison.InvariantCultureIgnoreCase) && !string.Equals(trackingLog.SubmissionType, "DU", StringComparison.InvariantCultureIgnoreCase))
          strArray = ((IEnumerable<string[]>) RegulationAlerts.GetAUSDataDiscrepancyAlertFields(Session.LoanData.CheckIfAUSTrackingForLP(trackingLog), Session.LoanData.GetField("1811") == "PrimaryResidence", Utils.ParseDouble((object) Session.LoanData.GetField("1014")) == 0.0)).FirstOrDefault<string[]>((Func<string[], bool>) (x => x[0] == field.FieldID));
        if (strArray != null)
        {
          compareFieldID = strArray[1];
          string ausData = RegulationAlerts.GetAUSData(Session.LoanData, compareFieldID, hasCuredValue);
          if (((IEnumerable<AlertTriggerField>) definition.TriggerFields.ToArray()).FirstOrDefault<AlertTriggerField>((Func<AlertTriggerField, bool>) (x => x.FieldID == compareFieldID)) != null)
          {
            gvItem = new GVItem("Field " + field.FieldID + " - Field " + compareFieldID);
            string strA = Session.LoanData.GetField(field.FieldID);
            Decimal num1;
            switch (field.FieldID.ToLower())
            {
              case "4752":
                if (Session.LoanData.GetField("4830") == "Y")
                {
                  strA = "not applicable";
                  break;
                }
                break;
              case "740":
              case "742":
                if (!Session.LoanData.CheckIfAUSTrackingForLP(trackingLog))
                {
                  if (ausData != "" && Convert.ToDecimal(ausData) != 0M)
                  {
                    num1 = Math.Truncate(100M * Convert.ToDecimal(ausData)) / 100M;
                    ausData = num1.ToString();
                  }
                  if (strA != "" && Convert.ToDecimal(strA) != 0M)
                  {
                    num1 = Math.Truncate(100M * Convert.ToDecimal(strA)) / 100M;
                    strA = num1.ToString();
                    break;
                  }
                  break;
                }
                break;
              case "casasrn.x217":
                if (!string.IsNullOrEmpty(strA) && !string.IsNullOrEmpty(ausData) && Convert.ToDecimal(strA) != 0M && Convert.ToDecimal(ausData) != 0M)
                {
                  Decimal val = Convert.ToDecimal(strA);
                  Decimal num2 = Convert.ToDecimal(ausData);
                  if (Utils.ArithmeticRounding(val, 3) == num2)
                  {
                    num1 = Utils.ArithmeticRounding(val, 3);
                    strA = num1.ToString("N3");
                  }
                  else
                  {
                    num1 = Math.Truncate(1000M * Convert.ToDecimal(strA)) / 1000M;
                    strA = num1.ToString("N3");
                  }
                  ausData = num2.ToString("N3");
                  break;
                }
                if (!string.IsNullOrEmpty(strA) && Convert.ToDecimal(strA) != 0M)
                {
                  num1 = Math.Truncate(1000M * Convert.ToDecimal(strA)) / 1000M;
                  strA = num1.ToString("N3");
                  if (!string.IsNullOrEmpty(ausData))
                  {
                    num1 = Convert.ToDecimal(ausData);
                    ausData = num1.ToString("N3");
                    break;
                  }
                  break;
                }
                break;
              case "casasrn.x218":
                if (!string.IsNullOrEmpty(strA) && !string.IsNullOrEmpty(ausData) && Convert.ToDecimal(strA) != 0M && Convert.ToDecimal(ausData) != 0M)
                {
                  Decimal val = Convert.ToDecimal(strA);
                  Decimal num3 = Convert.ToDecimal(ausData);
                  if (Utils.ArithmeticRounding(val, 3) == num3)
                  {
                    num1 = Utils.ArithmeticRounding(val, 3);
                    strA = num1.ToString("N3");
                  }
                  else
                  {
                    num1 = Math.Truncate(1000M * Convert.ToDecimal(strA)) / 1000M;
                    strA = num1.ToString("N3");
                  }
                  ausData = num3.ToString("N3");
                  break;
                }
                if (!string.IsNullOrEmpty(strA) && Convert.ToDecimal(strA) != 0M)
                {
                  num1 = Math.Truncate(1000M * Convert.ToDecimal(strA)) / 1000M;
                  strA = num1.ToString("N3");
                  if (!string.IsNullOrEmpty(ausData))
                  {
                    num1 = Convert.ToDecimal(ausData);
                    ausData = num1.ToString("N3");
                    break;
                  }
                  break;
                }
                break;
              case "mornet.x75":
              case "mornet.x76":
                strA += "0";
                break;
            }
            gvItem.SubItems.Add((object) EncompassFields.GetDescription(field.FieldID, Session.LoanData.Settings.FieldSettings));
            gvItem.SubItems.Add((object) strA);
            gvItem.SubItems.Add((object) ausData);
            gvItem.Tag = (object) field;
            if (string.Compare(strA, ausData, true) != 0)
              gvItem.ForeColor = EncompassColors.Alert2;
          }
          if (gvItem != null)
            this.gvFields.Items.Add(gvItem);
        }
      }
    }

    private void clearButton_Click(object sender, EventArgs e)
    {
      using (AUSDataDiscrepancyResolutionDialog resolutionDialog = new AUSDataDiscrepancyResolutionDialog())
      {
        if (resolutionDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
      }
      Session.Application.GetService<ILoanEditor>().ShowAUSTrackingTool();
    }

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);
      Button c = new Button();
      c.Text = "Cure Violation";
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
