// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.ServerMajorUpdateForm
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class ServerMajorUpdateForm : Form
  {
    private DateTime expirationDTTM;
    private IContainer components;
    private Label label1;
    private Timer timer1;
    private Button btnOK;
    private TextBox txtMessage;
    private Label lblTimer;

    public ServerMajorUpdateForm(string message, int waitTime)
    {
      this.InitializeComponent();
      this.txtMessage.Text = message;
      this.expirationDTTM = DateTime.Now.AddSeconds((double) waitTime);
      this.timer1.Start();
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
      TimeSpan timeSpan = this.expirationDTTM.Subtract(DateTime.Now);
      this.lblTimer.Text = timeSpan.Hours.ToString() + ":" + (object) timeSpan.Minutes + ":" + (object) timeSpan.Seconds;
      Application.DoEvents();
      Application.DoEvents();
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      this.WindowState = FormWindowState.Minimized;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ServerMajorUpdateForm));
      this.label1 = new Label();
      this.timer1 = new Timer(this.components);
      this.btnOK = new Button();
      this.txtMessage = new TextBox();
      this.lblTimer = new Label();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 117);
      this.label1.Name = "label1";
      this.label1.Size = new Size(86, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Remaining Time:";
      this.timer1.Tick += new EventHandler(this.timer1_Tick);
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(422, 112);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 1;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.txtMessage.Location = new Point(13, 13);
      this.txtMessage.Multiline = true;
      this.txtMessage.Name = "txtMessage";
      this.txtMessage.ReadOnly = true;
      this.txtMessage.Size = new Size(484, 88);
      this.txtMessage.TabIndex = 2;
      this.lblTimer.AutoSize = true;
      this.lblTimer.Location = new Point(104, 117);
      this.lblTimer.Name = "lblTimer";
      this.lblTimer.Size = new Size(83, 13);
      this.lblTimer.TabIndex = 3;
      this.lblTimer.Text = "Remaining Time";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(509, 142);
      this.ControlBox = false;
      this.Controls.Add((Control) this.lblTimer);
      this.Controls.Add((Control) this.txtMessage);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.Name = nameof (ServerMajorUpdateForm);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Server Major Update";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
