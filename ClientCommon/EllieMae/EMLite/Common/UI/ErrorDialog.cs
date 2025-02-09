// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.ErrorDialog
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using Elli.Metrics.Client;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class ErrorDialog : Form
  {
    private System.ComponentModel.Container components;
    private Label label1;
    private Label lblErrorDescription;
    private Button btnAdditionalInfo;
    private TextBox txtAdditionalInfo;
    private PictureBox pictureBox1;
    private string errorDescription;
    private string additionalInformation;
    private int collapsedHeight = 168;
    private int expandedHeight = 262;
    private Button btnExit;
    private Button btnOK;

    public static void Display(string errorDescription, string additionalInfo)
    {
      Cursor.Current = Cursors.Default;
      using (ErrorDialog errorDialog = new ErrorDialog(errorDescription, additionalInfo))
      {
        int num = (int) errorDialog.ShowDialog((IWin32Window) Session.MainScreen);
      }
    }

    public static void Display(string errorDescription)
    {
      ErrorDialog.Display(errorDescription, (string) null);
    }

    public static void Display(string errorDescription, Exception e)
    {
      Elli.Metrics.Client.MetricsFactory.IncrementCounter("LoanErrorIncCounter", new SFxTag(Elli.Metrics.Constants.ErrorType, "Displayed Exceptions"), (SFxTag) new SFxUiTag(), new SFxTag("ExceptionType", e.GetType().ToString()));
      ErrorDialog.Display(errorDescription, e.ToString());
    }

    public static void Display(Exception e)
    {
      Elli.Metrics.Client.MetricsFactory.IncrementCounter("LoanErrorIncCounter", new SFxTag(Elli.Metrics.Constants.ErrorType, "Unexpected Exceptions"), (SFxTag) new SFxUiTag(), new SFxTag("ExceptionType", e.GetType().ToString()));
      ErrorDialog.Display("An unexpected error has occurred while performing this operation. The error is described as: " + e.Message + "\n\nTo attempt to continue despite this error, press the OK button. To exit the application, press Exit.", e.ToString());
    }

    public ErrorDialog(string errorDescription, string additionalInfo)
    {
      this.InitializeComponent();
      this.errorDescription = errorDescription;
      this.additionalInformation = additionalInfo;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ErrorDialog));
      this.label1 = new Label();
      this.lblErrorDescription = new Label();
      this.btnAdditionalInfo = new Button();
      this.btnExit = new Button();
      this.txtAdditionalInfo = new TextBox();
      this.btnOK = new Button();
      this.pictureBox1 = new PictureBox();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.SuspendLayout();
      this.label1.Font = new Font("Arial", 10f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(58, 13);
      this.label1.Name = "label1";
      this.label1.Size = new Size(170, 16);
      this.label1.TabIndex = 0;
      this.label1.Text = "An error has occurred!";
      this.lblErrorDescription.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblErrorDescription.Location = new Point(58, 36);
      this.lblErrorDescription.Name = "lblErrorDescription";
      this.lblErrorDescription.Size = new Size(484, 112);
      this.lblErrorDescription.TabIndex = 1;
      this.lblErrorDescription.Text = "Error description goes here";
      this.btnAdditionalInfo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnAdditionalInfo.Location = new Point(58, 160);
      this.btnAdditionalInfo.Name = "btnAdditionalInfo";
      this.btnAdditionalInfo.Size = new Size(102, 23);
      this.btnAdditionalInfo.TabIndex = 2;
      this.btnAdditionalInfo.Text = "Show &Details >>";
      this.btnAdditionalInfo.Click += new EventHandler(this.btnAdditionalInfo_Click);
      this.btnExit.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnExit.Location = new Point(337, 160);
      this.btnExit.Name = "btnExit";
      this.btnExit.Size = new Size(102, 23);
      this.btnExit.TabIndex = 3;
      this.btnExit.Text = "E&xit";
      this.btnExit.Click += new EventHandler(this.btnExit_Click);
      this.txtAdditionalInfo.AcceptsReturn = true;
      this.txtAdditionalInfo.AcceptsTab = true;
      this.txtAdditionalInfo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.txtAdditionalInfo.Location = new Point(58, 196);
      this.txtAdditionalInfo.Multiline = true;
      this.txtAdditionalInfo.Name = "txtAdditionalInfo";
      this.txtAdditionalInfo.ReadOnly = true;
      this.txtAdditionalInfo.ScrollBars = ScrollBars.Both;
      this.txtAdditionalInfo.Size = new Size(484, 89);
      this.txtAdditionalInfo.TabIndex = 4;
      this.txtAdditionalInfo.Visible = false;
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnOK.DialogResult = DialogResult.OK;
      this.btnOK.Location = new Point(440, 160);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(102, 23);
      this.btnOK.TabIndex = 5;
      this.btnOK.Text = "&OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.pictureBox1.Image = (Image) componentResourceManager.GetObject("pictureBox1.Image");
      this.pictureBox1.Location = new Point(14, 36);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(32, 32);
      this.pictureBox1.TabIndex = 6;
      this.pictureBox1.TabStop = false;
      this.AcceptButton = (IButtonControl) this.btnExit;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(548, 194);
      this.Controls.Add((Control) this.pictureBox1);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.txtAdditionalInfo);
      this.Controls.Add((Control) this.btnExit);
      this.Controls.Add((Control) this.btnAdditionalInfo);
      this.Controls.Add((Control) this.lblErrorDescription);
      this.Controls.Add((Control) this.label1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ErrorDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Encompass Error";
      this.Load += new EventHandler(this.ErrorDialog_Load);
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void ErrorDialog_Load(object sender, EventArgs e)
    {
      this.lblErrorDescription.Text = this.errorDescription;
      if ((this.additionalInformation ?? "") != "")
      {
        this.txtAdditionalInfo.Text = this.additionalInformation;
        this.btnAdditionalInfo.Visible = true;
      }
      else
        this.btnAdditionalInfo.Visible = false;
      int num = this.ClientSize.Height - this.lblErrorDescription.Height;
      using (Graphics graphics = this.lblErrorDescription.CreateGraphics())
        this.ClientSize = new Size(this.ClientSize.Width, (int) graphics.MeasureString(this.lblErrorDescription.Text, this.lblErrorDescription.Font, this.lblErrorDescription.Width).Height + num);
      this.lblErrorDescription.Anchor = AnchorStyles.Top;
      this.btnOK.Anchor = AnchorStyles.Top;
      this.btnExit.Anchor = AnchorStyles.Top;
      this.btnAdditionalInfo.Anchor = AnchorStyles.Top;
      this.txtAdditionalInfo.Anchor = AnchorStyles.Top;
      this.collapsedHeight = this.Height;
      this.expandedHeight = this.collapsedHeight + this.txtAdditionalInfo.Height + (this.ClientSize.Height - this.btnOK.Top - this.btnOK.Height);
    }

    private void btnAdditionalInfo_Click(object sender, EventArgs e)
    {
      if (this.txtAdditionalInfo.Visible)
      {
        this.txtAdditionalInfo.Visible = false;
        this.Size = new Size(this.Size.Width, this.collapsedHeight);
        this.btnAdditionalInfo.Text = "Show &Details >>";
      }
      else
      {
        this.txtAdditionalInfo.Visible = true;
        this.Size = new Size(this.Size.Width, this.expandedHeight);
        this.btnAdditionalInfo.Text = "Hide &Details <<";
      }
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
      if (MessageBox.Show((IWin32Window) this, "You are about to close this application. Any unsaved data will be lost.", "Encompass", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
        return;
      this.Close();
      Application.Exit();
    }

    private void btnOK_Click(object sender, EventArgs e) => this.Close();
  }
}
