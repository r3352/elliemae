// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Diagnostics.DiagnosticDetailForm
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.Diagnostics
{
  public class DiagnosticDetailForm : Form
  {
    private string errorDetails = string.Empty;
    private string stkTraceDetails = string.Empty;
    private IContainer components;
    private Label lblDetails;
    private Button btnDetails;
    private Button btnClose;
    private FlowLayoutPanel flowLayoutPanel1;
    private TextBox txtStackTrace;
    private PictureBox errorPictureBox;

    public DiagnosticDetailForm(string erDetails, string stackTraceDetails)
    {
      this.InitializeComponent();
      this.errorDetails = erDetails;
      this.stkTraceDetails = stackTraceDetails;
    }

    private void btnClose_Click(object sender, EventArgs e) => this.Close();

    private void btnDetails_Click(object sender, EventArgs e)
    {
      this.txtStackTrace.Text = this.stkTraceDetails;
      if (!this.txtStackTrace.Visible)
      {
        this.btnDetails.Text = "Hide Details";
        this.txtStackTrace.Visible = true;
        this.txtStackTrace.Size = new Size(372, 171);
        this.flowLayoutPanel1.Size = new Size(374, 181);
        this.Size = new Size(412, 236);
      }
      else
      {
        if (!this.txtStackTrace.Visible)
          return;
        this.btnDetails.Text = "Show Details";
        this.txtStackTrace.Visible = false;
        this.txtStackTrace.Size = new Size(0, 0);
        this.flowLayoutPanel1.Size = new Size(0, 0);
        this.Size = new Size(412, 83);
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (DiagnosticDetailForm));
      this.lblDetails = new Label();
      this.btnDetails = new Button();
      this.btnClose = new Button();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.txtStackTrace = new TextBox();
      this.errorPictureBox = new PictureBox();
      this.flowLayoutPanel1.SuspendLayout();
      ((ISupportInitialize) this.errorPictureBox).BeginInit();
      this.SuspendLayout();
      this.lblDetails.AutoSize = true;
      this.lblDetails.Location = new Point(46, 9);
      this.lblDetails.Name = "lblDetails";
      this.lblDetails.Size = new Size(213, 13);
      this.lblDetails.TabIndex = 0;
      this.lblDetails.Text = "Product and Pricing Providers failed to load.";
      this.btnDetails.Location = new Point(230, 47);
      this.btnDetails.Name = "btnDetails";
      this.btnDetails.Size = new Size(80, 23);
      this.btnDetails.TabIndex = 1;
      this.btnDetails.Text = "Show Details";
      this.btnDetails.UseVisualStyleBackColor = true;
      this.btnDetails.Click += new EventHandler(this.btnDetails_Click);
      this.btnClose.Location = new Point(316, 47);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 23);
      this.btnClose.TabIndex = 2;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.flowLayoutPanel1.AutoSize = true;
      this.flowLayoutPanel1.Controls.Add((Control) this.txtStackTrace);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
      this.flowLayoutPanel1.Location = new Point(12, 89);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(6, 6);
      this.flowLayoutPanel1.TabIndex = 3;
      this.txtStackTrace.Location = new Point(3, 3);
      this.txtStackTrace.Multiline = true;
      this.txtStackTrace.Name = "txtStackTrace";
      this.txtStackTrace.ReadOnly = true;
      this.txtStackTrace.ScrollBars = ScrollBars.Both;
      this.txtStackTrace.Size = new Size(0, 0);
      this.txtStackTrace.TabIndex = 6;
      this.txtStackTrace.Visible = false;
      this.errorPictureBox.Image = (Image) componentResourceManager.GetObject("errorPictureBox.Image");
      this.errorPictureBox.Location = new Point(15, 12);
      this.errorPictureBox.Name = "errorPictureBox";
      this.errorPictureBox.Size = new Size(29, 35);
      this.errorPictureBox.TabIndex = 4;
      this.errorPictureBox.TabStop = false;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoSize = true;
      this.ClientSize = new Size(396, 83);
      this.Controls.Add((Control) this.errorPictureBox);
      this.Controls.Add((Control) this.btnDetails);
      this.Controls.Add((Control) this.lblDetails);
      this.Controls.Add((Control) this.btnClose);
      this.Controls.Add((Control) this.flowLayoutPanel1);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (DiagnosticDetailForm);
      this.ShowIcon = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "PRODUCT AND PRICING";
      this.flowLayoutPanel1.ResumeLayout(false);
      this.flowLayoutPanel1.PerformLayout();
      ((ISupportInitialize) this.errorPictureBox).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
