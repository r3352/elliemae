// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.AlertDialogPanel
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.Properties;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class AlertDialogPanel : UserControl
  {
    private PipelineInfo.Alert alert;
    private AlertConfig alertConfig;
    private IContainer components;
    private PictureBox pictureBox1;
    private Label lblTitle;
    private Label lblMessage;
    private Button btnDetails;

    public event EventHandler ViewDetails;

    public AlertDialogPanel(PipelineInfo.Alert alert)
    {
      this.InitializeComponent();
      this.alert = alert;
      this.alertConfig = Session.LoanDataMgr.SystemConfiguration.AlertSetupData.GetAlertConfig(alert.AlertID);
      this.lblTitle.Text = this.alertConfig.Definition.Name;
      this.lblMessage.Text = this.alertConfig.Message;
    }

    public PipelineInfo.Alert Alert => this.alert;

    public AlertConfig AlertConfig => this.alertConfig;

    public bool DetailsButtonVisible
    {
      get => this.btnDetails.Visible;
      set
      {
        if (value == this.btnDetails.Visible)
          return;
        this.btnDetails.Visible = value;
        if (value)
          this.Height += this.btnDetails.Height;
        else
          this.Height -= this.btnDetails.Height;
      }
    }

    private void btnDetails_Click(object sender, EventArgs e)
    {
      if (this.ViewDetails == null)
        return;
      this.ViewDetails((object) this, EventArgs.Empty);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lblTitle = new Label();
      this.lblMessage = new Label();
      this.btnDetails = new Button();
      this.pictureBox1 = new PictureBox();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.SuspendLayout();
      this.lblTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblTitle.AutoEllipsis = true;
      this.lblTitle.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblTitle.Location = new Point(48, 12);
      this.lblTitle.Name = "lblTitle";
      this.lblTitle.Size = new Size(234, 14);
      this.lblTitle.TabIndex = 1;
      this.lblTitle.Text = "<Alert Title Here>";
      this.lblMessage.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblMessage.AutoEllipsis = true;
      this.lblMessage.Location = new Point(48, 30);
      this.lblMessage.Name = "lblMessage";
      this.lblMessage.Size = new Size(318, 29);
      this.lblMessage.TabIndex = 2;
      this.lblMessage.Text = "<Alert Description Here>";
      this.btnDetails.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDetails.Location = new Point(282, 73);
      this.btnDetails.Name = "btnDetails";
      this.btnDetails.Size = new Size(84, 22);
      this.btnDetails.TabIndex = 3;
      this.btnDetails.Text = "View Details";
      this.btnDetails.UseVisualStyleBackColor = true;
      this.btnDetails.Click += new EventHandler(this.btnDetails_Click);
      this.pictureBox1.Image = (Image) Resources.flag_compliance_48x48;
      this.pictureBox1.Location = new Point(0, 10);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(48, 48);
      this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox1.TabIndex = 0;
      this.pictureBox1.TabStop = false;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.btnDetails);
      this.Controls.Add((Control) this.lblMessage);
      this.Controls.Add((Control) this.pictureBox1);
      this.Controls.Add((Control) this.lblTitle);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (AlertDialogPanel);
      this.Size = new Size(375, 94);
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
