// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.GoodFaithFeeVarianceViolationAlertPanel
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class GoodFaithFeeVarianceViolationAlertPanel : AlertPanelBase
  {
    private Hashtable triggerFields;
    private ToolTip toolTipField;
    private IContainer components;

    public int fieldCount => this.gvFields.Items.Count;

    public string fieldID => this.gvFields.Items[1].SubItems[0].Text;

    public GoodFaithFeeVarianceViolationAlertPanel(PipelineInfo.Alert alert)
    {
      this.SetAlert(alert);
    }

    public void SelectFirstItem() => this.gvFields.Items[1].Selected = true;

    protected override void ConfigureTriggerFieldList()
    {
      this.gvFields.Columns.Clear();
      this.gvFields.Columns.Add("Field ID", 150);
      this.gvFields.Columns.Add("Description", 250);
      this.gvFields.Columns.Add("Initial LE $", 130);
      this.gvFields.Columns.Add("Baseline", 80);
      this.gvFields.Columns.Add("Disclosed $", 100);
      this.gvFields.Columns.Add("Itemization $", 100);
      this.gvFields.Columns.Add("Variance $", 100);
      this.gvFields.Columns.Add("Variance Limit", 200);
      this.components = (IContainer) new System.ComponentModel.Container();
      this.toolTipField = new ToolTip(this.components);
    }

    protected override void PopulateTriggerFields()
    {
      this.triggerFields = Session.LoanData.Calculator.GetGFFVarianceAlertDetails();
      string[] array = Session.LoanData.GetGoodFaithChangeOfCircumstanceRecordFieldStr("01").Replace("\r\n", " ").Split(' ');
      if (this.triggerFields.ContainsKey((object) "Cannot Decrease"))
      {
        ArrayList triggerField = (ArrayList) this.triggerFields[(object) "Cannot Decrease"];
        for (int index1 = 0; index1 < triggerField.Count; ++index1)
        {
          GFFVAlertTriggerField field = (GFFVAlertTriggerField) triggerField[index1];
          this.gvFields.Items.Add(this.createGVItem(field, index1));
          int index2 = Array.IndexOf<string>(array, field.FieldId);
          if (index2 > -1)
            array[index2] = "";
        }
      }
      if (this.triggerFields.ContainsKey((object) "Cannot Increase"))
      {
        ArrayList triggerField = (ArrayList) this.triggerFields[(object) "Cannot Increase"];
        for (int index3 = 0; index3 < triggerField.Count; ++index3)
        {
          GFFVAlertTriggerField field = (GFFVAlertTriggerField) triggerField[index3];
          this.gvFields.Items.Add(this.createGVItem(field, index3));
          int index4 = Array.IndexOf<string>(array, field.FieldId);
          if (index4 > -1)
            array[index4] = "";
        }
      }
      if (!this.triggerFields.ContainsKey((object) "10% Variance"))
        return;
      ArrayList triggerField1 = (ArrayList) this.triggerFields[(object) "10% Variance"];
      for (int index5 = 0; index5 < triggerField1.Count; ++index5)
      {
        GFFVAlertTriggerField field = (GFFVAlertTriggerField) triggerField1[index5];
        this.gvFields.Items.Add(this.createGVItem(field, index5));
        int index6 = Array.IndexOf<string>(array, field.FieldId);
        if (index6 > -1)
          array[index6] = "";
      }
    }

    private GVItem createGVItem(GFFVAlertTriggerField field, int index)
    {
      GVItem gvItem = new GVItem();
      gvItem.SubItems[0].Text = index != 0 ? field.FieldId : "[Category Total]";
      gvItem.SubItems[1].Text = field.Description;
      gvItem.SubItems[2].Text = field.InitialLEValue;
      gvItem.SubItems[3].Text = field.Baseline;
      gvItem.SubItems[4].Text = field.DisclosedValue;
      gvItem.SubItems[5].Text = field.ItemizationValue;
      Label label = new Label();
      label.Width = this.gvFields.Columns[6].Width;
      label.AutoSize = false;
      label.Text = field.VarianceValue;
      this.toolTipField.SetToolTip((Control) label, "Fields with the same Disclosed and Itemization values may have moved between sections of the Fee Variance Worksheet due to changes in the Paid to Type, Borrower can shop for, or Borrower did shop for options.");
      if (index == 0)
        label.Font = new Font(this.lblAlertDate.Font, FontStyle.Bold);
      gvItem.SubItems[6].Value = (object) label;
      gvItem.SubItems[7].Text = field.VarianceLimit;
      gvItem.ForeColor = EncompassColors.Alert2;
      gvItem.Tag = (object) field;
      if (index == 0)
      {
        for (int nItemIndex = 0; nItemIndex < gvItem.SubItems.Count; ++nItemIndex)
          gvItem.SubItems[nItemIndex].Font = new Font(this.lblAlertDate.Font, FontStyle.Bold);
      }
      return gvItem;
    }

    private void clearButton_Click(object sender, EventArgs e)
    {
      using (GoodFaithViolationResolutionDialog resolutionDialog = new GoodFaithViolationResolutionDialog())
      {
        if (resolutionDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        GoodFaithFeeVarianceCureLog rec = new GoodFaithFeeVarianceCureLog(Utils.ParseDate((object) Session.LoanData.GetField("3171")), Session.UserID, Session.UserInfo.FullName, Session.LoanData.GetField("FV.X366"), Session.LoanData.GetField("FV.X348"), Session.LoanData.GetField("3172"), "Variance Cured", DateTime.Now);
        foreach (DictionaryEntry triggerField1 in this.triggerFields)
        {
          ArrayList arrayList = (ArrayList) triggerField1.Value;
          for (int index = 0; index < arrayList.Count; ++index)
          {
            GFFVAlertTriggerField triggerField2 = (GFFVAlertTriggerField) arrayList[index];
            rec.TriggerFieldList.Add(triggerField2);
          }
        }
        Session.LoanData.GetLogList().AddRecord((LogRecordBase) rec);
      }
    }

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);
      bool flag = true;
      if (Session.StartupInfo.UserAclFeatureRights.Contains((object) AclFeature.ToolsTab_CureToleranceAlert))
        flag = (bool) Session.StartupInfo.UserAclFeatureRights[(object) AclFeature.ToolsTab_CureToleranceAlert];
      Button c = new Button();
      c.Text = "Cure Variance";
      c.AutoSize = true;
      c.Margin = new Padding(3, 0, 2, 0);
      c.Click += new EventHandler(this.clearButton_Click);
      this.AddHeaderControl((Control) c);
      int width = c.Width;
      c.AutoSize = false;
      c.Height = 22;
      c.Width = width;
      this.lblAltDate.Text = "Total Variance";
      this.pnlAltDate.Visible = true;
      this.txtAltDate.Text = Session.LoanData.GetField("FV.X348");
      if (!flag)
        c.Enabled = false;
      this.SetGroupHeaderText("Trigger Fields");
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      this.toolTipField = new ToolTip(this.components);
      this.pnlAltDate.SuspendLayout();
      this.grpDetails.SuspendLayout();
      this.SuspendLayout();
      this.gvFields.Size = new Size(1343, 456);
      this.pnlAltDate.Location = new Point(218, 96);
      this.pnlAltDate.Size = new Size(199, 21);
      this.grpDetails.Size = new Size(1345, 126);
      this.txtAlertDate.Location = new Point(71, 96);
      this.txtAltDate.Location = new Point(62, 0);
      this.txtMessage.Location = new Point(71, 58);
      this.txtMessage.Size = new Size(1265, 36);
      this.txtAlertName.Location = new Point(71, 36);
      this.txtAlertName.Size = new Size(1265, 20);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.Name = nameof (GoodFaithFeeVarianceViolationAlertPanel);
      this.Size = new Size(1345, 609);
      this.pnlAltDate.ResumeLayout(false);
      this.pnlAltDate.PerformLayout();
      this.grpDetails.ResumeLayout(false);
      this.grpDetails.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
