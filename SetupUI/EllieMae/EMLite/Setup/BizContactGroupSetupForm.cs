// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.BizContactGroupSetupForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ContactUI;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class BizContactGroupSetupForm : Form
  {
    private IContainer components;
    private BizPartnerListForm bizPartnerListForm;
    private Color backColor = Color.FromArgb(247, 235, 206);
    private Color btnColor = Color.FromArgb(221, 221, 221);
    private Color contactTabColor = Color.FromArgb(239, 239, 239);
    private Color unselectedButtonColor = Color.FromArgb(0, 0, 0);
    private Panel panelMain;
    private Color selectedButtonColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);

    public BizContactGroupSetupForm()
    {
      this.InitializeComponent();
      this.loadBizPartnerScreen();
    }

    private void loadBizPartnerScreen()
    {
      Cursor.Current = Cursors.WaitCursor;
      this.bizPartnerListForm = new BizPartnerListForm((ContactMainForm) null, ContactType.PublicBiz);
      this.bizPartnerListForm.TopLevel = false;
      this.bizPartnerListForm.Visible = true;
      this.bizPartnerListForm.Dock = DockStyle.Fill;
      this.panelMain.Controls.Add((Control) this.bizPartnerListForm);
      this.bizPartnerListForm.RefreshContactList();
      Cursor.Current = Cursors.Default;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.panelMain = new Panel();
      this.SuspendLayout();
      this.panelMain.Dock = DockStyle.Fill;
      this.panelMain.Location = new Point(0, 0);
      this.panelMain.Name = "panelMain";
      this.panelMain.Size = new Size(732, 448);
      this.panelMain.TabIndex = 1;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(732, 448);
      this.Controls.Add((Control) this.panelMain);
      this.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (BizContactGroupSetupForm);
      this.Text = "Organization Hierarchy";
      this.ResumeLayout(false);
    }
  }
}
