// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.eDisclosureDetails
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.InputEngine.HtmlEmail;
using EllieMae.EMLite.UI;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class eDisclosureDetails : Form
  {
    private IContainer components;
    private GroupContainer groupContainer1;
    private Button btnClose;
    private HtmlEmailControl htmlEmailControl;

    public eDisclosureDetails(IDisclosureTrackingLog log)
    {
      this.InitializeComponent();
      this.htmlEmailControl.ShowToolbar = false;
      if (log is EnhancedDisclosureTracking2015Log disclosureTracking2015Log)
      {
        HtmlEmailLog htmlEmailLog = disclosureTracking2015Log.Log.Loan.GetLogList()?.GetHTMLEmailLog(disclosureTracking2015Log.Guid);
        if (htmlEmailLog == null)
          return;
        if (htmlEmailLog.Body.ToUpper().Contains("<HTML"))
          this.htmlEmailControl.LoadHtml(htmlEmailLog.Body, true);
        else
          this.htmlEmailControl.LoadText(htmlEmailLog.Body, true);
      }
      else if (log.eDisclosureDisclosedMessage.ToUpper().Contains("<HTML"))
        this.htmlEmailControl.LoadHtml(log.eDisclosureDisclosedMessage, true);
      else
        this.htmlEmailControl.LoadText(log.eDisclosureDisclosedMessage, true);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.groupContainer1 = new GroupContainer();
      this.btnClose = new Button();
      this.htmlEmailControl = new HtmlEmailControl();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.groupContainer1.Controls.Add((Control) this.htmlEmailControl);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(12, 12);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(446, 242);
      this.groupContainer1.TabIndex = 0;
      this.groupContainer1.Text = "Sent Message Details";
      this.btnClose.DialogResult = DialogResult.OK;
      this.btnClose.Location = new Point(383, 260);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 23);
      this.btnClose.TabIndex = 1;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.htmlEmailControl.Dock = DockStyle.Fill;
      this.htmlEmailControl.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.htmlEmailControl.Location = new Point(1, 26);
      this.htmlEmailControl.Name = "htmlEmailControl";
      this.htmlEmailControl.Size = new Size(444, 215);
      this.htmlEmailControl.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(470, 292);
      this.Controls.Add((Control) this.btnClose);
      this.Controls.Add((Control) this.groupContainer1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (eDisclosureDetails);
      this.Text = "Sent Message Details";
      this.groupContainer1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
