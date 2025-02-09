// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DocEngine.PlanCodeDetailsDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.RemotingServices;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.DocEngine
{
  public class PlanCodeDetailsDialog : Form
  {
    private Plan currentPlan;
    private Sessions.Session session;
    private IContainer components;
    private Panel panel1;
    private FormBrowser ctlBrowser;
    private Button btnClose;

    public PlanCodeDetailsDialog(Plan plan) => this.session = Session.DefaultInstance;

    public PlanCodeDetailsDialog(Plan plan, Sessions.Session session)
    {
      this.currentPlan = plan;
      this.session = session;
      this.InitializeComponent();
      this.loadInputForm();
      switch (plan.PlanType)
      {
        case PlanType.Alias:
          this.Text = "ICE Mortgage Technology Plan Code Alias Details";
          break;
        case PlanType.Custom:
          this.Text = "ICE Mortgage Technology Plan Code Details";
          break;
      }
    }

    private void loadInputForm()
    {
      InputFormInfo formInfo = new InputFormInfo("PlanCodeDetails", "Plan Cod Details", InputFormType.Standard);
      this.ctlBrowser.DataSource = (IHtmlInput) this.currentPlan;
      this.ctlBrowser.OpenForm(formInfo, this.session);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.panel1 = new Panel();
      this.btnClose = new Button();
      this.ctlBrowser = new FormBrowser();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.panel1.Controls.Add((Control) this.btnClose);
      this.panel1.Dock = DockStyle.Bottom;
      this.panel1.Location = new Point(0, 574);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(654, 38);
      this.panel1.TabIndex = 0;
      this.btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnClose.DialogResult = DialogResult.Cancel;
      this.btnClose.Location = new Point(569, 7);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 22);
      this.btnClose.TabIndex = 0;
      this.btnClose.Text = "&Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.ctlBrowser.DataSource = (IHtmlInput) null;
      this.ctlBrowser.Dock = DockStyle.Fill;
      this.ctlBrowser.Location = new Point(0, 0);
      this.ctlBrowser.Name = "ctlBrowser";
      this.ctlBrowser.Size = new Size(654, 574);
      this.ctlBrowser.TabIndex = 1;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnClose;
      this.ClientSize = new Size(654, 612);
      this.Controls.Add((Control) this.ctlBrowser);
      this.Controls.Add((Control) this.panel1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.MaximumSize = new Size(756, 2000);
      this.Name = nameof (PlanCodeDetailsDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Plan Code Details";
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
