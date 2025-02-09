// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ExternalOriginatorManagement.TPOWatchListControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.ExternalOriginatorManagement
{
  public class TPOWatchListControl : UserControl, IRefreshContents
  {
    private Sessions.Session session;
    private IContainer components;
    private Label label1;
    private StandardIconButton standardIconButton1;
    private BorderPanel borderPanel1;

    public TPOWatchListControl() => this.InitializeComponent();

    public TPOWatchListControl(Sessions.Session session)
      : this()
    {
      this.session = session;
    }

    public void Initialize(Sessions.Session session)
    {
      this.session = session;
      this.refreshContents();
    }

    public void RefreshLoanContents()
    {
      if (this.session == null)
        return;
      this.refreshContents();
    }

    public void RefreshContents()
    {
      if (this.session == null)
        return;
      this.refreshContents();
    }

    private void refreshContents()
    {
      string field1 = this.session.LoanData.GetField("TPO.X86");
      string field2 = this.session.LoanData.GetField("TPO.X87");
      if (field1 != string.Empty && field1.Equals("Y"))
      {
        if (field2.Equals(WatchListReasonType.WatchListReason.Company.ToString()) || field2.Equals(WatchListReasonType.WatchListReason.Both.ToString()))
        {
          this.label1.Text = "TPO Company is on WatchList";
        }
        else
        {
          if (!field2.Equals(WatchListReasonType.WatchListReason.User.ToString()))
            return;
          this.label1.Text = "TPO User is on WatchList";
        }
      }
      else
        this.label1.Text = "";
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
      this.standardIconButton1 = new StandardIconButton();
      this.borderPanel1 = new BorderPanel();
      ((ISupportInitialize) this.standardIconButton1).BeginInit();
      this.borderPanel1.SuspendLayout();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Location = new Point(18, 4);
      this.label1.Name = "label1";
      this.label1.Size = new Size(35, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "label1";
      this.standardIconButton1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      this.standardIconButton1.BackColor = Color.Transparent;
      this.standardIconButton1.Location = new Point(2, 3);
      this.standardIconButton1.MouseDownImage = (Image) null;
      this.standardIconButton1.Name = "standardIconButton1";
      this.standardIconButton1.Size = new Size(16, 16);
      this.standardIconButton1.StandardButtonType = StandardIconButton.ButtonType.AlertButton;
      this.standardIconButton1.TabIndex = 1;
      this.standardIconButton1.TabStop = false;
      this.borderPanel1.BackColor = Color.FromArgb(252, 220, 223);
      this.borderPanel1.BorderColor = Color.FromArgb(204, 0, 0);
      this.borderPanel1.Controls.Add((Control) this.label1);
      this.borderPanel1.Controls.Add((Control) this.standardIconButton1);
      this.borderPanel1.Dock = DockStyle.Fill;
      this.borderPanel1.Location = new Point(0, 0);
      this.borderPanel1.Name = "borderPanel1";
      this.borderPanel1.Size = new Size(175, 23);
      this.borderPanel1.TabIndex = 2;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.borderPanel1);
      this.Name = nameof (TPOWatchListControl);
      this.Size = new Size(175, 23);
      ((ISupportInitialize) this.standardIconButton1).EndInit();
      this.borderPanel1.ResumeLayout(false);
      this.borderPanel1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
