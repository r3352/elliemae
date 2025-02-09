// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Verification.PanelBase
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.Verification
{
  public abstract class PanelBase : WorkAreaPanelBase, IRefreshContents
  {
    protected Panel actionPanel;
    protected Panel upperContentPanel;
    protected Panel lowerContentPanel;
    protected Button btnAddDoc;
    protected Button btnOrderIRS;
    protected Button btnCheckIRS;
    protected StandardIconButton btnImport4506;
    protected StandardIconButton btnExport4506;
    protected StandardIconButton btnEdit;
    protected StandardIconButton btnNew;
    protected StandardIconButton btnDelete;
    protected StandardIconButton btnUp;
    protected StandardIconButton btnDown;
    protected VerticalSeparator verSeparator;
    private BorderPanel borderPanelTop;
    private BorderPanel borderPanelBottom;
    private CollapsibleSplitter horzSplitter;
    protected ToolTip toolTip = new ToolTip();
    protected LoanData loan;
    protected IWorkArea workArea;
    protected string className;
    protected IMainScreen mainScreen;
    protected GridView gridList;
    protected VerificationBase ver;

    protected abstract void InitPanel();

    public PanelBase(string title, IMainScreen mainScreen, IWorkArea workArea)
      : this(title, mainScreen, workArea, Session.LoanData)
    {
    }

    public PanelBase(string title, IMainScreen mainScreen, IWorkArea workArea, LoanData loan)
    {
      this.InitializeComponent();
      this.horzSplitter.VisualStyle = VisualStyles.Encompass;
      this.actionPanel.BringToFront();
      this.loan = loan != null ? loan : Session.LoanData;
      this.workArea = workArea;
      this.mainScreen = mainScreen;
      if (title.StartsWith("Verification of") && ShipInDarkValidation.IsURLA2020Form(title) && this.loan.Use2020URLA)
      {
        this.actionPanel.Controls.Remove((Control) this.btnAddDoc);
        this.actionPanel.Controls.Remove((Control) this.verSeparator);
      }
      this.paintActionPanel();
      this.InitPanel();
      this.titleLbl.ForeColor = Color.Black;
      this.titleLbl.Size = new Size(200, 26);
      this.titleLbl.Text = title;
      this.GotFocus += new EventHandler(this.RefreshListView);
      this.gridList.SelectedIndexChanged += new EventHandler(this.gridList_SelectedIndexChanged);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.toolTip != null)
      {
        this.toolTip.Dispose();
        this.toolTip = (ToolTip) null;
      }
      base.Dispose(disposing);
    }

    protected void gridList_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnAddDoc.Enabled = this.btnOrderIRS.Enabled = this.btnCheckIRS.Enabled = this.btnDelete.Enabled = this.btnExport4506.Enabled = this.gridList.SelectedItems.Count == 1;
      this.btnUp.Enabled = this.btnDown.Enabled = this.gridList.SelectedItems.Count == 1 && this.gridList.Items.Count > 1;
      if (this.gridList.SelectedItems.Count == 0)
        return;
      this.OpenVerification(this.gridList.SelectedItems[0].Index);
    }

    protected void paintActionPanel()
    {
      this.actionPanel.SuspendLayout();
      int x = 1;
      foreach (Control control in (ArrangedElementCollection) this.actionPanel.Controls)
      {
        control.Location = new Point(x, control.Location.Y);
        x += control.Size.Width + 5;
      }
      this.actionPanel.Size = new Size(x + 2, this.actionPanel.Size.Height);
      this.actionPanel.ResumeLayout(false);
    }

    protected void InitializeComponent()
    {
      this.btnAddDoc = new Button();
      this.btnOrderIRS = new Button();
      this.btnCheckIRS = new Button();
      this.btnImport4506 = new StandardIconButton();
      this.btnExport4506 = new StandardIconButton();
      this.btnEdit = new StandardIconButton();
      this.btnNew = new StandardIconButton();
      this.btnDelete = new StandardIconButton();
      this.btnUp = new StandardIconButton();
      this.btnDown = new StandardIconButton();
      this.verSeparator = new VerticalSeparator();
      this.borderPanelTop = new BorderPanel();
      this.borderPanelBottom = new BorderPanel();
      ((ISupportInitialize) this.btnEdit).BeginInit();
      ((ISupportInitialize) this.btnNew).BeginInit();
      ((ISupportInitialize) this.btnDelete).BeginInit();
      ((ISupportInitialize) this.btnUp).BeginInit();
      ((ISupportInitialize) this.btnDown).BeginInit();
      ((ISupportInitialize) this.btnImport4506).BeginInit();
      ((ISupportInitialize) this.btnExport4506).BeginInit();
      this.actionPanel = new Panel();
      this.upperContentPanel = new Panel();
      this.lowerContentPanel = new Panel();
      this.horzSplitter = new CollapsibleSplitter();
      this.upperContentPanel.SuspendLayout();
      this.lowerContentPanel.SuspendLayout();
      this.actionPanel.SuspendLayout();
      this.borderPanelTop.SuspendLayout();
      this.borderPanelBottom.SuspendLayout();
      this.horzSplitter.AnimationDelay = 20;
      this.horzSplitter.AnimationStep = 20;
      this.horzSplitter.BorderStyle3D = Border3DStyle.Raised;
      this.horzSplitter.ControlToHide = (Control) this.upperContentPanel;
      this.horzSplitter.ExpandParentForm = false;
      this.horzSplitter.Location = new Point(175, 0);
      this.horzSplitter.Name = "horzSplitter";
      this.horzSplitter.TabIndex = 1;
      this.horzSplitter.TabStop = false;
      this.horzSplitter.UseAnimations = false;
      this.horzSplitter.VisualStyle = VisualStyles.XP;
      this.horzSplitter.Dock = DockStyle.Top;
      this.horzSplitter.Paint += new PaintEventHandler(this.horzSplitter_Paint);
      this.actionPanel.Controls.AddRange(new Control[10]
      {
        (Control) this.btnNew,
        (Control) this.btnDelete,
        (Control) this.btnUp,
        (Control) this.btnDown,
        (Control) this.verSeparator,
        (Control) this.btnImport4506,
        (Control) this.btnExport4506,
        (Control) this.btnAddDoc,
        (Control) this.btnOrderIRS,
        (Control) this.btnCheckIRS
      });
      this.actionPanel.Dock = DockStyle.Right;
      this.actionPanel.Size = new Size(344, 22);
      this.actionPanel.BackColor = Color.Transparent;
      this.btnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnNew.BackColor = Color.Transparent;
      this.btnNew.Location = new Point(1, 5);
      this.btnNew.Name = "btnNew";
      this.btnNew.Size = new Size(16, 16);
      this.btnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnNew.TabStop = false;
      this.btnNew.Click += new EventHandler(this.newBtn_Click);
      this.toolTip.SetToolTip((Control) this.btnNew, "New Verif");
      this.btnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnEdit.BackColor = Color.Transparent;
      this.btnEdit.Location = new Point(22, 5);
      this.btnEdit.Name = "btnEdit";
      this.btnEdit.Size = new Size(16, 16);
      this.btnEdit.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnEdit.TabStop = false;
      this.btnEdit.Click += new EventHandler(this.editBtn_Click);
      this.toolTip.SetToolTip((Control) this.btnEdit, "Edit Verif");
      this.btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDelete.BackColor = Color.Transparent;
      this.btnDelete.Location = new Point(43, 5);
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(16, 16);
      this.btnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDelete.TabStop = false;
      this.btnDelete.Click += new EventHandler(this.deleteBtn_Click);
      this.toolTip.SetToolTip((Control) this.btnDelete, "Delete Verif");
      this.btnUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnUp.BackColor = Color.Transparent;
      this.btnUp.Location = new Point(64, 5);
      this.btnUp.Name = "btnUp";
      this.btnUp.Size = new Size(16, 16);
      this.btnUp.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.btnUp.TabStop = false;
      this.btnUp.Click += new EventHandler(this.upBtn_Click);
      this.toolTip.SetToolTip((Control) this.btnUp, "Move The Verif Up");
      this.btnDown.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDown.BackColor = Color.Transparent;
      this.btnDown.Location = new Point(85, 5);
      this.btnDown.Name = "btnDown";
      this.btnDown.Size = new Size(16, 16);
      this.btnDown.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.btnDown.TabStop = false;
      this.btnDown.Click += new EventHandler(this.downBtn_Click);
      this.toolTip.SetToolTip((Control) this.btnDown, "Move The Verif Down");
      this.verSeparator.Location = new Point(128, 5);
      this.verSeparator.Margin = new Padding(3, 3, 2, 3);
      this.verSeparator.MaximumSize = new Size(2, 16);
      this.verSeparator.MinimumSize = new Size(2, 16);
      this.verSeparator.Name = "verSeparator";
      this.verSeparator.Size = new Size(2, 16);
      this.verSeparator.TabIndex = 1;
      this.verSeparator.Text = "";
      this.btnImport4506.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnImport4506.BackColor = Color.Transparent;
      this.btnImport4506.Location = new Point(149, 5);
      this.btnImport4506.Name = "btnImport4506";
      this.btnImport4506.Size = new Size(16, 16);
      this.btnImport4506.StandardButtonType = StandardIconButton.ButtonType.ImportFrom4506Button;
      this.btnImport4506.TabStop = false;
      this.btnImport4506.Click += new EventHandler(this.btnImport4506_Click);
      this.toolTip.SetToolTip((Control) this.btnImport4506, "Copy data to this form from the Request for Copy of Tax Return (Classic) form.");
      this.btnExport4506.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExport4506.BackColor = Color.Transparent;
      this.btnExport4506.Location = new Point(170, 5);
      this.btnExport4506.Name = "btnExport4506";
      this.btnExport4506.Size = new Size(16, 16);
      this.btnExport4506.StandardButtonType = StandardIconButton.ButtonType.ExportTo4506Button;
      this.btnExport4506.TabStop = false;
      this.btnExport4506.Click += new EventHandler(this.btnExport4506_Click);
      this.toolTip.SetToolTip((Control) this.btnExport4506, "Copy data from this form to the Request for Copy of Tax Return (Classic) form.");
      this.btnAddDoc.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAddDoc.Location = new Point(262, 3);
      this.btnAddDoc.BackColor = SystemColors.Control;
      this.btnAddDoc.Name = "btnAddDoc";
      this.btnAddDoc.Size = new Size(90, 22);
      this.btnAddDoc.TabStop = false;
      this.btnAddDoc.Text = "&Add to eFolder";
      this.btnAddDoc.UseVisualStyleBackColor = true;
      this.btnAddDoc.TextAlign = ContentAlignment.MiddleCenter;
      this.btnAddDoc.Click += new EventHandler(this.btnAddDoc_Click);
      this.btnOrderIRS.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnOrderIRS.Location = new Point(352, 3);
      this.btnOrderIRS.BackColor = SystemColors.Control;
      this.btnOrderIRS.Name = "btnOrderIRS";
      this.btnOrderIRS.Size = new Size(95, 22);
      this.btnOrderIRS.TabStop = false;
      this.btnOrderIRS.Text = "&Order Transcript";
      this.btnOrderIRS.UseVisualStyleBackColor = true;
      this.btnOrderIRS.TextAlign = ContentAlignment.MiddleCenter;
      this.btnOrderIRS.Click += new EventHandler(this.btnOrderIRS_Click);
      this.btnCheckIRS.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnCheckIRS.Location = new Point(449, 3);
      this.btnCheckIRS.BackColor = SystemColors.Control;
      this.btnCheckIRS.Name = "btnCheckIRS";
      this.btnCheckIRS.Size = new Size(88, 22);
      this.btnCheckIRS.TabStop = false;
      this.btnCheckIRS.Text = "&Check Status";
      this.btnCheckIRS.UseVisualStyleBackColor = true;
      this.btnCheckIRS.TextAlign = ContentAlignment.MiddleCenter;
      this.btnCheckIRS.Click += new EventHandler(this.btnCheckIRS_Click);
      this.actionPanel.ResumeLayout(false);
      this.titlePanel.Controls.Add((Control) this.actionPanel);
      this.gridList = new GridView();
      this.gridList.BorderStyle = BorderStyle.None;
      this.gridList.Dock = DockStyle.Fill;
      this.gridList.Name = "gridList";
      this.gridList.Click += new EventHandler(this.editBtn_Click);
      this.gridList.SortOption = GVSortOption.None;
      this.gridList.AllowMultiselect = false;
      this.borderPanelTop.Borders = AnchorStyles.Bottom;
      this.borderPanelTop.Controls.Add((Control) this.gridList);
      this.borderPanelTop.Dock = DockStyle.Fill;
      this.borderPanelTop.Location = new Point(0, 0);
      this.borderPanelTop.Name = "borderPanelTop";
      this.borderPanelTop.Size = new Size(634, 293);
      this.borderPanelTop.TabIndex = 71;
      this.upperContentPanel.Controls.Add((Control) this.borderPanelTop);
      this.upperContentPanel.Dock = DockStyle.Top;
      this.lowerContentPanel.Size = new Size(337, 140);
      this.lowerContentPanel.Dock = DockStyle.Fill;
      this.borderPanelBottom.Borders = AnchorStyles.Top;
      this.borderPanelBottom.Controls.Add((Control) this.lowerContentPanel);
      this.borderPanelBottom.Dock = DockStyle.Fill;
      this.borderPanelBottom.Location = new Point(0, 0);
      this.borderPanelBottom.Name = "borderPanelBottom";
      this.borderPanelBottom.Size = new Size(634, 293);
      this.borderPanelBottom.TabIndex = 71;
      this.contentPanel.Controls.Add((Control) this.borderPanelBottom);
      this.contentPanel.Controls.Add((Control) this.horzSplitter);
      this.contentPanel.Controls.Add((Control) this.upperContentPanel);
      this.upperContentPanel.ResumeLayout(false);
      this.borderPanelTop.ResumeLayout(false);
      this.lowerContentPanel.ResumeLayout(false);
      this.borderPanelBottom.ResumeLayout(false);
    }

    protected abstract VerificationBase NewVerificationScreen();

    protected abstract void hookupEventHandler(IInputHandler inputHandler);

    public abstract void RefreshContents();

    public void RefreshLoanContents()
    {
      this.refreshList();
      this.RefreshContents();
    }

    private void DocumentLoadedEventHandler()
    {
      this.hookupEventHandler(this.ver.GetInputHandler());
    }

    public void OpenVerification(int i, string fieldGoTo, int count)
    {
      if (this.ver == null)
      {
        this.ver = this.NewVerificationScreen();
        this.ver.DocumentLoaded += new EllieMae.EMLite.Verification.DocumentLoadedEventHandler(this.DocumentLoadedEventHandler);
      }
      this.lowerContentPanel.Controls.Clear();
      this.lowerContentPanel.Controls.Add((Control) this.ver);
      this.ver.SetGoToFieldFocus(fieldGoTo, count);
      this.ver.LoadData(i);
    }

    public void OpenVerification(int i)
    {
      if (i < 0)
        return;
      if (this.ver == null)
      {
        this.ver = this.NewVerificationScreen();
        this.ver.DocumentLoaded += new EllieMae.EMLite.Verification.DocumentLoadedEventHandler(this.DocumentLoadedEventHandler);
      }
      this.lowerContentPanel.Controls.Clear();
      this.lowerContentPanel.Controls.Add((Control) this.ver);
      this.ver.LoadData(i);
    }

    public override string GetHelpTargetName() => this.className;

    public abstract void RefreshListView(object sender, EventArgs e);

    protected abstract void refreshList();

    protected abstract GVItem addVerifToListView(int newIndex);

    protected abstract void editBtn_Click(object sender, EventArgs e);

    public abstract void newBtn_Click(object sender, EventArgs e);

    protected void closeBtn_Click(object sender, EventArgs e) => this.workArea.RemoveFromWorkArea();

    protected DialogResult DeleteDialog(IWin32Window wnd)
    {
      return Utils.Dialog(wnd, "The selected record and associated data will be permanently deleted from the loan file. Are you sure you want to delete this record?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
    }

    protected abstract void deleteBtn_Click(object sender, EventArgs e);

    protected abstract void upBtn_Click(object sender, EventArgs e);

    protected abstract void downBtn_Click(object sender, EventArgs e);

    protected abstract void btnAddDoc_Click(object sender, EventArgs e);

    protected abstract void btnOrderIRS_Click(object sender, EventArgs e);

    protected abstract void btnCheckIRS_Click(object sender, EventArgs e);

    protected abstract void btnImport4506_Click(object sender, EventArgs e);

    protected abstract void btnExport4506_Click(object sender, EventArgs e);

    private void horzSplitter_Paint(object sender, PaintEventArgs e)
    {
      Session.Application.GetService<IEncompassApplication>().GetTipControl().Refresh();
    }
  }
}
