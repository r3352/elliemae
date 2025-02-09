// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanDataDelta.LoanDeltaDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.LoanDataDelta
{
  public class LoanDeltaDialog : Form
  {
    private readonly Func<List<string>> _getColumnTextFunc;
    private readonly Func<List<LoanDeltaItem>> _getData;
    private List<LoanDeltaItem> _items = new List<LoanDeltaItem>();
    private IContainer components;
    private Button btnClose;
    private BindingSource loanDeltaItemBindingSource;
    private FlowLayoutPanel flowLayoutPanel1;
    private StandardIconButton btnRefresh;
    private GradientPanel topPnl;
    private Label captionLbl;
    private GridView gvDiffs;

    public int NumberOfDiffs { get; private set; }

    public LoanDeltaDialog(Func<List<LoanDeltaItem>> getData, Func<List<string>> getColumnTextFunc)
    {
      this.InitializeComponent();
      this._getData = getData;
      this._getColumnTextFunc = getColumnTextFunc;
      this.InitializeData();
    }

    public LoanDeltaDialog(
      Func<List<LoanDeltaItem>> getData,
      string windowTitle,
      Func<List<string>> getColumnTextFunc)
      : this(getData, getColumnTextFunc)
    {
      if (windowTitle == null)
        return;
      this.Text = windowTitle;
    }

    public override sealed string Text
    {
      get => base.Text;
      set => base.Text = value;
    }

    private void InitializeData() => this.BuidLoanDeltaGrid();

    private void BuidLoanDeltaGrid()
    {
      this.Cursor = Cursors.WaitCursor;
      Application.DoEvents();
      this._items.Clear();
      this.gvDiffs.Items.Clear();
      try
      {
        if (this._getData != null)
          this._items = this._getData();
        foreach (LoanDeltaItem loanDeltaItem in this._items)
          this.gvDiffs.Items.Add(new GVItem()
          {
            SubItems = {
              (object) loanDeltaItem.FieldId,
              (object) loanDeltaItem.FieldDescription,
              (object) loanDeltaItem.Value,
              (object) loanDeltaItem.SnapshotValue
            }
          });
        this.NumberOfDiffs = this._items.Count;
        if (this._getColumnTextFunc != null)
        {
          List<string> stringList = this._getColumnTextFunc();
          if (stringList != null)
          {
            this.gvDiffs.Columns[2].Text = stringList[0];
            this.gvDiffs.Columns[3].Text = stringList[1];
          }
        }
        if (this.NumberOfDiffs > 0)
          return;
        int num = (int) MessageBox.Show((IWin32Window) this, "There are no loan data discrepancies.", "Loan Data Delta", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      finally
      {
        this.Cursor = Cursors.Default;
      }
    }

    private void btnRefresh_Click(object sender, EventArgs e) => this.BuidLoanDeltaGrid();

    private void btnClose_Click(object sender, EventArgs e) => this.Close();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (LoanDeltaDialog));
      this.btnClose = new Button();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.btnRefresh = new StandardIconButton();
      this.topPnl = new GradientPanel();
      this.captionLbl = new Label();
      this.gvDiffs = new GridView();
      this.loanDeltaItemBindingSource = new BindingSource(this.components);
      this.flowLayoutPanel1.SuspendLayout();
      ((ISupportInitialize) this.btnRefresh).BeginInit();
      this.topPnl.SuspendLayout();
      ((ISupportInitialize) this.loanDeltaItemBindingSource).BeginInit();
      this.SuspendLayout();
      this.btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnClose.Location = new Point(711, 243);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 23);
      this.btnClose.TabIndex = 0;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.btnRefresh);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(714, 2);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(71, 24);
      this.flowLayoutPanel1.TabIndex = 43;
      this.btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnRefresh.BackColor = Color.Transparent;
      this.btnRefresh.Location = new Point(55, 3);
      this.btnRefresh.Margin = new Padding(4, 3, 0, 3);
      this.btnRefresh.MouseDownImage = (Image) null;
      this.btnRefresh.Name = "btnRefresh";
      this.btnRefresh.Size = new Size(16, 17);
      this.btnRefresh.StandardButtonType = StandardIconButton.ButtonType.RefreshButton;
      this.btnRefresh.TabIndex = 29;
      this.btnRefresh.TabStop = false;
      this.btnRefresh.Click += new EventHandler(this.btnRefresh_Click);
      this.topPnl.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.topPnl.Controls.Add((Control) this.flowLayoutPanel1);
      this.topPnl.Controls.Add((Control) this.captionLbl);
      this.topPnl.Dock = DockStyle.Top;
      this.topPnl.Location = new Point(0, 0);
      this.topPnl.Name = "topPnl";
      this.topPnl.Size = new Size(794, 26);
      this.topPnl.TabIndex = 8;
      this.captionLbl.AutoSize = true;
      this.captionLbl.BackColor = Color.Transparent;
      this.captionLbl.Location = new Point(4, 6);
      this.captionLbl.Name = "captionLbl";
      this.captionLbl.Size = new Size(105, 13);
      this.captionLbl.TabIndex = 1;
      this.captionLbl.Text = "Compare Data Fields";
      this.gvDiffs.AllowMultiselect = false;
      this.gvDiffs.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gvDiffs.ClearSelectionsOnEmptyRowClick = false;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Field ID";
      gvColumn1.Width = 120;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Description";
      gvColumn2.Width = 300;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.SortMethod = GVSortMethod.Date;
      gvColumn3.Text = "Current Loan Value";
      gvColumn3.Width = 170;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Text = "Original Order Value";
      gvColumn4.Width = 170;
      this.gvDiffs.Columns.AddRange(new GVColumn[4]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4
      });
      this.gvDiffs.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvDiffs.Location = new Point(0, 25);
      this.gvDiffs.Name = "gvDiffs";
      this.gvDiffs.Size = new Size(794, 210);
      this.gvDiffs.TabIndex = 7;
      this.loanDeltaItemBindingSource.DataSource = (object) typeof (LoanDeltaItem);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(794, 275);
      this.Controls.Add((Control) this.topPnl);
      this.Controls.Add((Control) this.gvDiffs);
      this.Controls.Add((Control) this.btnClose);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new Size(300, 150);
      this.Name = nameof (LoanDeltaDialog);
      this.StartPosition = FormStartPosition.Manual;
      this.Text = "Loan Data Delta";
      this.TopMost = true;
      this.flowLayoutPanel1.ResumeLayout(false);
      ((ISupportInitialize) this.btnRefresh).EndInit();
      this.topPnl.ResumeLayout(false);
      this.topPnl.PerformLayout();
      ((ISupportInitialize) this.loanDeltaItemBindingSource).EndInit();
      this.ResumeLayout(false);
    }
  }
}
