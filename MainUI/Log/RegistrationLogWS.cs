// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.RegistrationLogWS
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class RegistrationLogWS : UserControl
  {
    private RegistrationLog regLog;
    private IContainer components;
    private Label label1;
    private Label label2;
    private Label label5;
    private Label label3;
    private TextBox boxCreatedBy;
    private TextBox boxInvestor;
    private TextBox boxReference;
    private Label label4;
    private TextBox boxCreatedDate;
    private TextBox boxExpiredDate;
    private Label labelStatus;
    private BorderPanel borderPanel2;
    private GradientPanel panelHeader;

    public RegistrationLogWS(RegistrationLog regLog)
    {
      this.regLog = regLog;
      this.Dock = DockStyle.Fill;
      this.InitializeComponent();
      this.boxCreatedBy.Text = regLog.RegisteredByName;
      this.boxCreatedDate.Text = regLog.RegisteredDate.ToString("MM/dd/yyyy");
      if (regLog.ExpiredDate != DateTime.MinValue)
        this.boxExpiredDate.Text = regLog.ExpiredDate.ToString("MM/dd/yyyy");
      else
        this.boxExpiredDate.Text = string.Empty;
      this.boxInvestor.Text = regLog.InvestorName;
      this.boxReference.Text = regLog.Reference;
      this.refreshTitle();
    }

    private void refreshTitle()
    {
      string str1 = "Old Registration";
      string str2 = string.Empty;
      if (this.regLog.IsCurrent)
      {
        str1 = "";
        DateTime today = DateTime.Today;
        DateTime dateTime = this.regLog.ExpiredDate;
        DateTime date = dateTime.Date;
        int days = (today - date).Days;
        dateTime = this.regLog.ExpiredDate;
        if (dateTime.Date == DateTime.Today)
        {
          str2 = "Registration expires today!";
        }
        else
        {
          string str3 = days > 0 ? "expired" : "expires";
          dateTime = this.regLog.ExpiredDate;
          dateTime = dateTime.Date;
          string str4 = dateTime.ToString("MM/dd/yyyy");
          str2 = "Registration " + str3 + " on " + str4;
        }
        if (days > 0)
          str2 = str2 + " (" + (object) days + " day(s) ago!)";
        else if (days < 0)
        {
          int num = -days;
          str2 = str2 + " (in " + (object) num + " day(s))";
        }
        if (this.isAlert())
        {
          this.labelStatus.ForeColor = Color.White;
          this.panelHeader.Style = GradientPanel.PanelStyle.Alert;
        }
      }
      this.labelStatus.Text = str1 + str2;
    }

    private bool isAlert()
    {
      foreach (PipelineInfo.Alert usersPipelineAlert in Session.LoanDataMgr.AccessRules.GetUsersPipelineAlerts())
      {
        if (usersPipelineAlert.LogRecordID == this.regLog.Guid)
          return true;
      }
      return false;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.label2 = new Label();
      this.label5 = new Label();
      this.label3 = new Label();
      this.boxCreatedBy = new TextBox();
      this.boxInvestor = new TextBox();
      this.boxReference = new TextBox();
      this.label4 = new Label();
      this.boxCreatedDate = new TextBox();
      this.boxExpiredDate = new TextBox();
      this.labelStatus = new Label();
      this.borderPanel2 = new BorderPanel();
      this.panelHeader = new GradientPanel();
      this.borderPanel2.SuspendLayout();
      this.panelHeader.SuspendLayout();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(8, 12);
      this.label1.Name = "label1";
      this.label1.Size = new Size(75, 14);
      this.label1.TabIndex = 153;
      this.label1.Text = "Registered By";
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(8, 34);
      this.label2.Name = "label2";
      this.label2.Size = new Size(76, 14);
      this.label2.TabIndex = 154;
      this.label2.Text = "Registered On";
      this.label5.AutoSize = true;
      this.label5.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label5.Location = new Point(8, 100);
      this.label5.Name = "label5";
      this.label5.Size = new Size(98, 14);
      this.label5.TabIndex = 160;
      this.label5.Text = "Reference Number";
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(8, 56);
      this.label3.Name = "label3";
      this.label3.Size = new Size(103, 14);
      this.label3.TabIndex = 155;
      this.label3.Text = "Registration Expires";
      this.boxCreatedBy.BackColor = Color.WhiteSmoke;
      this.boxCreatedBy.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.boxCreatedBy.Location = new Point(114, 10);
      this.boxCreatedBy.Name = "boxCreatedBy";
      this.boxCreatedBy.ReadOnly = true;
      this.boxCreatedBy.Size = new Size(184, 20);
      this.boxCreatedBy.TabIndex = 149;
      this.boxInvestor.BackColor = Color.WhiteSmoke;
      this.boxInvestor.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.boxInvestor.Location = new Point(114, 76);
      this.boxInvestor.Name = "boxInvestor";
      this.boxInvestor.ReadOnly = true;
      this.boxInvestor.Size = new Size(184, 20);
      this.boxInvestor.TabIndex = 152;
      this.boxReference.BackColor = Color.WhiteSmoke;
      this.boxReference.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.boxReference.Location = new Point(114, 98);
      this.boxReference.Multiline = true;
      this.boxReference.Name = "boxReference";
      this.boxReference.ReadOnly = true;
      this.boxReference.Size = new Size(184, 20);
      this.boxReference.TabIndex = 159;
      this.label4.AutoSize = true;
      this.label4.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label4.Location = new Point(8, 78);
      this.label4.Name = "label4";
      this.label4.Size = new Size(76, 14);
      this.label4.TabIndex = 156;
      this.label4.Text = "Investor Name";
      this.boxCreatedDate.BackColor = Color.WhiteSmoke;
      this.boxCreatedDate.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.boxCreatedDate.Location = new Point(114, 32);
      this.boxCreatedDate.Name = "boxCreatedDate";
      this.boxCreatedDate.ReadOnly = true;
      this.boxCreatedDate.Size = new Size(184, 20);
      this.boxCreatedDate.TabIndex = 150;
      this.boxExpiredDate.BackColor = Color.WhiteSmoke;
      this.boxExpiredDate.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.boxExpiredDate.Location = new Point(114, 54);
      this.boxExpiredDate.Name = "boxExpiredDate";
      this.boxExpiredDate.ReadOnly = true;
      this.boxExpiredDate.Size = new Size(184, 20);
      this.boxExpiredDate.TabIndex = 151;
      this.labelStatus.AutoSize = true;
      this.labelStatus.BackColor = Color.Transparent;
      this.labelStatus.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.labelStatus.Location = new Point(7, 6);
      this.labelStatus.Name = "labelStatus";
      this.labelStatus.Size = new Size(50, 14);
      this.labelStatus.TabIndex = 161;
      this.labelStatus.Text = "(Status)";
      this.borderPanel2.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.borderPanel2.Controls.Add((Control) this.label1);
      this.borderPanel2.Controls.Add((Control) this.label2);
      this.borderPanel2.Controls.Add((Control) this.boxInvestor);
      this.borderPanel2.Controls.Add((Control) this.boxExpiredDate);
      this.borderPanel2.Controls.Add((Control) this.boxReference);
      this.borderPanel2.Controls.Add((Control) this.label5);
      this.borderPanel2.Controls.Add((Control) this.boxCreatedBy);
      this.borderPanel2.Controls.Add((Control) this.boxCreatedDate);
      this.borderPanel2.Controls.Add((Control) this.label4);
      this.borderPanel2.Controls.Add((Control) this.label3);
      this.borderPanel2.Dock = DockStyle.Fill;
      this.borderPanel2.Location = new Point(0, 26);
      this.borderPanel2.Name = "borderPanel2";
      this.borderPanel2.Size = new Size(458, 203);
      this.borderPanel2.TabIndex = 164;
      this.panelHeader.Controls.Add((Control) this.labelStatus);
      this.panelHeader.Dock = DockStyle.Top;
      this.panelHeader.Location = new Point(0, 0);
      this.panelHeader.Name = "panelHeader";
      this.panelHeader.Size = new Size(458, 26);
      this.panelHeader.TabIndex = 161;
      this.BackColor = Color.WhiteSmoke;
      this.Controls.Add((Control) this.borderPanel2);
      this.Controls.Add((Control) this.panelHeader);
      this.Name = nameof (RegistrationLogWS);
      this.Size = new Size(458, 229);
      this.borderPanel2.ResumeLayout(false);
      this.borderPanel2.PerformLayout();
      this.panelHeader.ResumeLayout(false);
      this.panelHeader.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
