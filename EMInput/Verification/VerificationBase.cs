// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Verification.VerificationBase
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Verification
{
  public abstract class VerificationBase : LoanScreen
  {
    private Button listBtn;
    private StandardIconButton btnNew;
    private StandardIconButton btnDelete;
    private StandardIconButton btnNext;
    private StandardIconButton btnPrevious;
    private Panel actionPanel;
    protected PanelBase selPanel;
    protected string verType;
    protected IWorkArea workArea;

    public VerificationBase(
      string title,
      IMainScreen mainScreen,
      IWorkArea area,
      LoanData loanData)
      : base(Session.DefaultInstance, (IWin32Window) mainScreen, (IHtmlInput) loanData)
    {
      this.initPanel(title, area);
    }

    public VerificationBase(string title, IMainScreen mainScreen, IWorkArea area)
      : base(Session.DefaultInstance)
    {
      this.initPanel(title, area);
    }

    private void initPanel(string title, IWorkArea area)
    {
      this.InitializeComponent();
      this.titleLbl.Text = title;
      this.titlePanel.Size = new Size(56, 0);
      this.workArea = area;
      this.RemoveBorder();
      this.brwHandler.DocumentCompleted += new DocumentCompleteEventHandler(this.DocumentLoadedEventHandler);
      this.Show();
    }

    public override string GetHelpTargetName() => this.verType + "Panel";

    protected void InitializeComponent()
    {
      this.titleLbl.Size = new Size(100, this.titleLbl.Height);
      this.listBtn = new Button();
      this.btnNew = new StandardIconButton();
      this.btnDelete = new StandardIconButton();
      this.btnNext = new StandardIconButton();
      this.btnPrevious = new StandardIconButton();
      ((ISupportInitialize) this.btnNew).BeginInit();
      ((ISupportInitialize) this.btnDelete).BeginInit();
      ((ISupportInitialize) this.btnNext).BeginInit();
      ((ISupportInitialize) this.btnPrevious).BeginInit();
      this.actionPanel = new Panel();
      this.actionPanel.SuspendLayout();
      this.actionPanel.Controls.AddRange(new Control[5]
      {
        (Control) this.btnNew,
        (Control) this.btnNext,
        (Control) this.btnPrevious,
        (Control) this.btnDelete,
        (Control) this.listBtn
      });
      this.actionPanel.Dock = DockStyle.Right;
      this.actionPanel.Size = new Size(348, 22);
      this.btnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnNew.BackColor = Color.Transparent;
      this.btnNew.Location = new Point(1, 5);
      this.btnNew.Name = "btnNew";
      this.btnNew.Size = new Size(16, 16);
      this.btnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnNew.TabStop = false;
      this.btnNew.Click += new EventHandler(this.newBtn_Click);
      this.btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDelete.BackColor = Color.Transparent;
      this.btnDelete.Location = new Point(22, 5);
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(16, 16);
      this.btnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDelete.TabStop = false;
      this.btnDelete.Click += new EventHandler(this.deleteBtn_Click);
      this.btnNext.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnNext.BackColor = Color.Transparent;
      this.btnNext.Location = new Point(43, 5);
      this.btnNext.Name = "btnNext";
      this.btnNext.Size = new Size(16, 16);
      this.btnNext.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.btnNext.TabStop = false;
      this.btnNext.Click += new EventHandler(this.nextBtn_Click);
      this.btnPrevious.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnPrevious.BackColor = Color.Transparent;
      this.btnPrevious.Location = new Point(64, 5);
      this.btnPrevious.Name = "btnPrevious";
      this.btnPrevious.Size = new Size(16, 16);
      this.btnPrevious.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.btnPrevious.TabStop = false;
      this.btnPrevious.Click += new EventHandler(this.previousBtn_Click);
      this.listBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.listBtn.Location = new Point(57, 1);
      this.listBtn.Name = "listBtn";
      this.listBtn.Size = new Size(56, 20);
      this.listBtn.TabStop = false;
      this.listBtn.Text = "&List";
      this.listBtn.BackColor = Color.NavajoWhite;
      this.listBtn.Click += new EventHandler(this.listBtn_Click);
      this.actionPanel.ResumeLayout(false);
    }

    public event EllieMae.EMLite.Verification.DocumentLoadedEventHandler DocumentLoaded;

    private void DocumentLoadedEventHandler()
    {
      if (this.DocumentLoaded == null)
        return;
      this.DocumentLoaded();
    }

    public abstract void LoadData(int i);

    protected abstract void previousBtn_Click(object sender, EventArgs e);

    protected abstract void nextBtn_Click(object sender, EventArgs e);

    protected abstract void newBtn_Click(object sender, EventArgs e);

    protected abstract void deleteBtn_Click(object sender, EventArgs e);

    protected void listBtn_Click(object sender, EventArgs e)
    {
      this.workArea.ShowVerifPanel(this.verType);
    }

    protected void closeBtn_Click(object sender, EventArgs e)
    {
      this.workArea.RemoveFromWorkArea();
      this.workArea.RefreshContents();
    }

    public abstract int CurrentVerificationNo();
  }
}
