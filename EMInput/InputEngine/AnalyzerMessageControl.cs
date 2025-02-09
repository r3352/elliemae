// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.AnalyzerMessageControl
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientCommon.AIQCapsilon;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.LoanUtils.JSonObjects;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class AnalyzerMessageControl : UserControl
  {
    private IncomeAnalyzerControl incomeAnalyzerControl;
    private Sessions.Session session;
    private LoanData loan;
    private AIQButtonHelper aiqHelper;
    private IContainer components;
    private GroupContainer containerMessageType;
    private TextBox txtAlertDate;
    private Label label21;
    private TextBox txtAlertDescription;
    private Label label20;
    private TextBox txtAlertName;
    private Label label19;
    private Panel panelMessageDetail;
    private Panel panelBorrowers;
    private Panel panelAllErrors;
    private Label labelErrorFromAIQAPI;
    private Button btn_OpenAnalyzer;

    public AnalyzerMessageControl(
      string alertName,
      string alertMessage,
      DateTime alertDate,
      int noOfAlerts,
      Sessions.Session session,
      LoanData loan,
      string messageID)
    {
      this.session = session;
      this.loan = loan;
      this.InitializeComponent();
      this.Dock = DockStyle.Left;
      this.txtAlertName.Text = alertName;
      this.txtAlertDescription.Text = alertMessage;
      this.txtAlertDate.Text = alertDate.ToString("MM/dd/yyyy");
      this.containerMessageType.Text = alertName;
      if (alertName.ToLower().IndexOf("income") > -1)
      {
        this.incomeAnalyzerControl = new IncomeAnalyzerControl(this.session, this.loan);
        this.panelBorrowers.Controls.Add((Control) this.incomeAnalyzerControl);
        if (this.incomeAnalyzerControl != null)
          this.incomeAnalyzerControl.MessageID = messageID;
      }
      else
      {
        this.aiqHelper = new AIQButtonHelper((IWin32Window) this, this.btn_OpenAnalyzer);
        this.aiqHelper.EnableAIQLaunchButton((LoanDataMgr) this.loan.DataMgr, true);
        this.panelBorrowers.BorderStyle = BorderStyle.FixedSingle;
        this.setTagInfoToAnalyzerButton(alertName);
      }
      this.panelAllErrors.Visible = false;
    }

    public void RefreshIncomePage()
    {
      AIQIncomeData aiqData = (AIQIncomeData) null;
      bool flag = false;
      try
      {
        aiqData = new AIQAnalyzerHelper(this.loan).GetIncomeAnalyzerResultJSON();
      }
      catch (Exception ex)
      {
        if ((this.session.ServerLicense.ClientID == "3010000024" || this.session.ServerLicense.ClientID == "3011220192" || this.session.ServerLicense.ClientID == "3011220390" || this.session.ServerLicense.ClientID == "3011173698") && Utils.Dialog((IWin32Window) this, "The Mortgage Analyzers Data cannot be retrieved due to this error: " + ex.Message + "\r\n\r\nWould you like to load Analyzer Json file from local?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
          flag = true;
        if (!flag)
        {
          this.labelErrorFromAIQAPI.Text = ex.Message;
          this.panelAllErrors.Visible = true;
          this.panelBorrowers.Enabled = false;
          this.incomeAnalyzerControl.Enabled = false;
          return;
        }
      }
      if (flag)
      {
        string path = "";
        using (OpenFileDialog openFileDialog = new OpenFileDialog())
        {
          openFileDialog.Title = "Note: this dialog is available only for QA to select a Json file to test:";
          openFileDialog.Filter = "Json files (*.json)|*.json";
          openFileDialog.Multiselect = false;
          if (openFileDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          {
            this.panelBorrowers.Enabled = false;
            this.incomeAnalyzerControl.Enabled = false;
            return;
          }
          path = openFileDialog.FileNames[0];
        }
        string JSONstring;
        try
        {
          JSONstring = File.ReadAllText(path);
        }
        catch (Exception ex)
        {
          this.labelErrorFromAIQAPI.Text = ex.Message;
          this.panelAllErrors.Visible = true;
          return;
        }
        if (string.IsNullOrEmpty(JSONstring))
        {
          this.labelErrorFromAIQAPI.Text = "The Mortgage Analyzers Json file is invalid!";
          this.panelAllErrors.Visible = true;
          return;
        }
        JAIQIncome incomeDataFromJson = JSonUtil.getAIQIncomeDataFromJSON(JSONstring);
        if (incomeDataFromJson == null)
        {
          this.labelErrorFromAIQAPI.Text = "The Json file from Mortgage Analyzers doesn't have a valid format!";
          this.panelAllErrors.Visible = true;
          return;
        }
        aiqData = new AIQIncomeData(this.loan, incomeDataFromJson);
      }
      this.panelAllErrors.Visible = false;
      string str = this.incomeAnalyzerControl.PopulateData(aiqData);
      if (!string.IsNullOrEmpty(str))
      {
        this.labelErrorFromAIQAPI.Text = "The Mortgage Analyzers Json cannot be parsed correctly due to this error: " + str;
        this.panelAllErrors.Visible = true;
        this.panelBorrowers.Visible = false;
      }
      else
      {
        this.panelAllErrors.Visible = false;
        this.panelBorrowers.Visible = true;
      }
    }

    private void btn_OpenAnalyzer_Click(object sender, EventArgs e)
    {
      if (this.aiqHelper == null)
        return;
      this.aiqHelper.btnClick_action(this.loan.GUID);
    }

    private void setTagInfoToAnalyzerButton(string alertName)
    {
      if (alertName.ToLower().IndexOf("aus") > -1)
        this.btn_OpenAnalyzer.Tag = (object) "aus";
      else if (alertName.ToLower().IndexOf("asset") > -1)
      {
        this.btn_OpenAnalyzer.Tag = (object) "asset";
      }
      else
      {
        if (alertName.ToLower().IndexOf("audit") <= -1)
          return;
        this.btn_OpenAnalyzer.Tag = (object) "audit";
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
      this.containerMessageType = new GroupContainer();
      this.btn_OpenAnalyzer = new Button();
      this.txtAlertDate = new TextBox();
      this.label21 = new Label();
      this.txtAlertDescription = new TextBox();
      this.label20 = new Label();
      this.txtAlertName = new TextBox();
      this.label19 = new Label();
      this.panelMessageDetail = new Panel();
      this.panelBorrowers = new Panel();
      this.panelAllErrors = new Panel();
      this.labelErrorFromAIQAPI = new Label();
      this.containerMessageType.SuspendLayout();
      this.panelMessageDetail.SuspendLayout();
      this.panelAllErrors.SuspendLayout();
      this.SuspendLayout();
      this.containerMessageType.Controls.Add((Control) this.btn_OpenAnalyzer);
      this.containerMessageType.Controls.Add((Control) this.txtAlertDate);
      this.containerMessageType.Controls.Add((Control) this.label21);
      this.containerMessageType.Controls.Add((Control) this.txtAlertDescription);
      this.containerMessageType.Controls.Add((Control) this.label20);
      this.containerMessageType.Controls.Add((Control) this.txtAlertName);
      this.containerMessageType.Controls.Add((Control) this.label19);
      this.containerMessageType.Dock = DockStyle.Top;
      this.containerMessageType.HeaderForeColor = SystemColors.ControlText;
      this.containerMessageType.Location = new Point(0, 0);
      this.containerMessageType.Name = "containerMessageType";
      this.containerMessageType.Size = new Size(661, 142);
      this.containerMessageType.TabIndex = 6;
      this.containerMessageType.Text = "Income Analyzer Message";
      this.btn_OpenAnalyzer.Location = new Point(516, 2);
      this.btn_OpenAnalyzer.Name = "btn_OpenAnalyzer";
      this.btn_OpenAnalyzer.Size = new Size(118, 20);
      this.btn_OpenAnalyzer.TabIndex = 33;
      this.btn_OpenAnalyzer.Text = "Open Analyzer";
      this.btn_OpenAnalyzer.UseVisualStyleBackColor = true;
      this.btn_OpenAnalyzer.Visible = false;
      this.btn_OpenAnalyzer.Click += new EventHandler(this.btn_OpenAnalyzer_Click);
      this.txtAlertDate.Location = new Point(78, 113);
      this.txtAlertDate.Name = "txtAlertDate";
      this.txtAlertDate.ReadOnly = true;
      this.txtAlertDate.Size = new Size(136, 20);
      this.txtAlertDate.TabIndex = 3;
      this.txtAlertDate.Tag = (object) "Seller3.Name";
      this.label21.AutoSize = true;
      this.label21.Location = new Point(10, 116);
      this.label21.Name = "label21";
      this.label21.Size = new Size(54, 13);
      this.label21.TabIndex = 32;
      this.label21.Text = "Alert Date";
      this.txtAlertDescription.Location = new Point(78, 59);
      this.txtAlertDescription.Multiline = true;
      this.txtAlertDescription.Name = "txtAlertDescription";
      this.txtAlertDescription.ReadOnly = true;
      this.txtAlertDescription.Size = new Size(580, 50);
      this.txtAlertDescription.TabIndex = 2;
      this.txtAlertDescription.Tag = (object) "VEND.X412";
      this.label20.AutoSize = true;
      this.label20.Location = new Point(10, 62);
      this.label20.Name = "label20";
      this.label20.Size = new Size(57, 13);
      this.label20.TabIndex = 30;
      this.label20.Text = "Desciption";
      this.txtAlertName.Location = new Point(78, 36);
      this.txtAlertName.Name = "txtAlertName";
      this.txtAlertName.ReadOnly = true;
      this.txtAlertName.Size = new Size(580, 20);
      this.txtAlertName.TabIndex = 1;
      this.txtAlertName.Tag = (object) "638";
      this.label19.AutoSize = true;
      this.label19.Location = new Point(10, 39);
      this.label19.Name = "label19";
      this.label19.Size = new Size(59, 13);
      this.label19.TabIndex = 28;
      this.label19.Text = "Alert Name";
      this.panelMessageDetail.Controls.Add((Control) this.panelBorrowers);
      this.panelMessageDetail.Controls.Add((Control) this.panelAllErrors);
      this.panelMessageDetail.Dock = DockStyle.Fill;
      this.panelMessageDetail.Location = new Point(0, 142);
      this.panelMessageDetail.Name = "panelMessageDetail";
      this.panelMessageDetail.Size = new Size(661, 196);
      this.panelMessageDetail.TabIndex = 7;
      this.panelBorrowers.AutoScroll = true;
      this.panelBorrowers.Dock = DockStyle.Fill;
      this.panelBorrowers.Location = new Point(0, 48);
      this.panelBorrowers.Name = "panelBorrowers";
      this.panelBorrowers.Size = new Size(661, 148);
      this.panelBorrowers.TabIndex = 1;
      this.panelAllErrors.Controls.Add((Control) this.labelErrorFromAIQAPI);
      this.panelAllErrors.Dock = DockStyle.Top;
      this.panelAllErrors.Location = new Point(0, 0);
      this.panelAllErrors.Name = "panelAllErrors";
      this.panelAllErrors.Size = new Size(661, 48);
      this.panelAllErrors.TabIndex = 0;
      this.labelErrorFromAIQAPI.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.labelErrorFromAIQAPI.ForeColor = Color.Red;
      this.labelErrorFromAIQAPI.Location = new Point(13, 7);
      this.labelErrorFromAIQAPI.Name = "labelErrorFromAIQAPI";
      this.labelErrorFromAIQAPI.Size = new Size(644, 38);
      this.labelErrorFromAIQAPI.TabIndex = 0;
      this.labelErrorFromAIQAPI.Text = "{Mortgage Analyzers Error}";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.panelMessageDetail);
      this.Controls.Add((Control) this.containerMessageType);
      this.Name = nameof (AnalyzerMessageControl);
      this.Size = new Size(661, 338);
      this.containerMessageType.ResumeLayout(false);
      this.containerMessageType.PerformLayout();
      this.panelMessageDetail.ResumeLayout(false);
      this.panelAllErrors.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
