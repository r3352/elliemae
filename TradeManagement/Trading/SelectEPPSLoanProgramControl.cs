// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.SelectEPPSLoanProgramControl
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class SelectEPPSLoanProgramControl : UserControl
  {
    private bool modified;
    private bool loading = true;
    private bool readOnly;
    private IContainer components;
    private GroupContainer grpHeader;
    private Panel pnlBody;
    private TableLayoutPanel tableLayoutPanel1;
    private GroupContainer grpEPPSProgram;
    private FlowLayoutPanel flpHeader;
    private GridView gvLP;
    private Panel pnlFields;
    private Label label2;
    private TextBox txtMinNoteRate;
    private TextBox txtMaxNoteRate;
    private Label label3;
    private Label label4;
    private StandardIconButton btnRemove;
    private StandardIconButton btnAdd;
    private ToolTip toolTip1;

    public event EventHandler DataChange;

    public SelectEPPSLoanProgramControl()
    {
      this.InitializeComponent();
      TextBoxFormatter.Attach(this.txtMinNoteRate, TextBoxContentRule.NonNegativeDecimal, "0.000");
      TextBoxFormatter.Attach(this.txtMaxNoteRate, TextBoxContentRule.NonNegativeDecimal, "0.000");
    }

    [Browsable(false)]
    public bool DataModified
    {
      get => this.modified;
      set => this.modified = value;
    }

    public bool ReadOnly
    {
      get => this.readOnly;
      set
      {
        if (this.readOnly == value)
          return;
        this.readOnly = value;
        this.setReadOnly();
      }
    }

    public void AddControlToHeader(Control c)
    {
      this.flpHeader.Controls.Add(c);
      c.BringToFront();
    }

    private void setReadOnly()
    {
      this.txtMinNoteRate.ReadOnly = this.txtMaxNoteRate.ReadOnly = this.readOnly;
      this.btnAdd.Visible = this.btnRemove.Visible = !this.readOnly;
    }

    private void onDataChange()
    {
      if (this.loading)
        return;
      this.modified = true;
      if (this.DataChange == null)
        return;
      this.DataChange((object) this, EventArgs.Empty);
    }

    private void onControlValueChanges(object sender, EventArgs e) => this.onDataChange();

    public void HideAdditionalDetails() => --this.tableLayoutPanel1.ColumnCount;

    public void SetCurrentFilter(SimpleTradeFilter filter)
    {
      if (filter.NoteRateRange != null)
      {
        TextBox txtMinNoteRate = this.txtMinNoteRate;
        Decimal num;
        string str1;
        if (!(filter.NoteRateRange.Minimum >= 0M))
        {
          str1 = "";
        }
        else
        {
          num = filter.NoteRateRange.Minimum;
          str1 = num.ToString("0.000");
        }
        txtMinNoteRate.Text = str1;
        TextBox txtMaxNoteRate = this.txtMaxNoteRate;
        string str2;
        if (!(filter.NoteRateRange.Maximum < Decimal.MaxValue))
        {
          str2 = "";
        }
        else
        {
          num = filter.NoteRateRange.Maximum;
          str2 = num.ToString("0.000");
        }
        txtMaxNoteRate.Text = str2;
      }
      else
      {
        this.txtMinNoteRate.Text = "";
        this.txtMaxNoteRate.Text = "";
      }
      if (!this.loading)
        return;
      this.loading = false;
    }

    public void SetEPPSLoanPrograms(EPPSLoanProgramFilters filter)
    {
      this.gvLP.Items.Clear();
      foreach (EPPSLoanProgramFilter loanProgramFilter in filter)
        this.gvLP.Items.Add(new GVItem()
        {
          SubItems = {
            (object) loanProgramFilter.ProgramId,
            (object) loanProgramFilter.ProgramName
          },
          Tag = (object) loanProgramFilter
        });
      this.gvLP.Sort(1, SortOrder.Ascending);
      if (Session.StartupInfo.ProductPricingPartner != null && Session.StartupInfo.ProductPricingPartner.IsEPPS)
      {
        this.btnAdd.Enabled = true;
        this.btnRemove.Enabled = true;
      }
      else
      {
        this.btnAdd.Enabled = false;
        this.btnRemove.Enabled = false;
      }
    }

    public SimpleTradeFilter GetCurrentFilter()
    {
      return new SimpleTradeFilter(false)
      {
        NoteRateRange = Range<Decimal>.Parse(this.txtMinNoteRate.Text, this.txtMaxNoteRate.Text, Decimal.MinValue, Decimal.MaxValue)
      };
    }

    public EPPSLoanProgramFilters GetEPPSLoanPorgramFilter()
    {
      EPPSLoanProgramFilters loanPorgramFilter = new EPPSLoanProgramFilters();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvLP.Items)
      {
        EPPSLoanProgramFilter tag = (EPPSLoanProgramFilter) gvItem.Tag;
        tag.ProgramId = gvItem.SubItems[0].Value.ToString();
        tag.ProgramName = gvItem.SubItems[1].Value.ToString();
        EPPSLoanProgramFilter loanProgramFilter = new EPPSLoanProgramFilter(tag);
        loanPorgramFilter.Add(loanProgramFilter);
      }
      return loanPorgramFilter;
    }

    public void ClearFilter() => this.gvLP.Items.Clear();

    public List<EPPSProgram> GetCurrentSelection() => new List<EPPSProgram>();

    public string NoteRateMin
    {
      set => this.txtMinNoteRate.Text = value;
      get => this.txtMinNoteRate.Text;
    }

    public string NoteRateMax
    {
      set => this.txtMaxNoteRate.Text = value;
      get => this.txtMaxNoteRate.Text;
    }

    public void validateNoteRateRange(object sender, CancelEventArgs e)
    {
      if (this.txtMaxNoteRate.Text == "" || this.txtMinNoteRate.Text == "" || (double) Utils.ParseSingle((object) this.txtMaxNoteRate.Text) >= (double) Utils.ParseSingle((object) this.txtMinNoteRate.Text))
        return;
      int num = (int) Utils.Dialog((IWin32Window) this.ParentForm, "This minimum note rate must be less than or equal to the maximum.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      e.Cancel = true;
    }

    private void SelectEPPSLoanProgramControl_Resize(object sender, EventArgs e)
    {
      this.grpEPPSProgram.Width = this.tableLayoutPanel1.Width;
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      using (EPPSLoanProgramSelector loanProgramSelector = new EPPSLoanProgramSelector())
      {
        if (loanProgramSelector.ShowDialog((IWin32Window) this) == DialogResult.OK)
        {
          foreach (EPPSLoanProgram selectedProgram in loanProgramSelector.selectedPrograms)
          {
            EPPSLoanProgram p = selectedProgram;
            if (this.gvLP.Items.Where<GVItem>((Func<GVItem, bool>) (x => x.SubItems[0].Value.ToString() == p.ProgramID)).Count<GVItem>() <= 0)
            {
              GVItem gvItem = new GVItem();
              gvItem.SubItems.Add((object) p.ProgramID);
              gvItem.SubItems.Add((object) p.ProgramName);
              EPPSLoanProgramFilter loanProgramFilter = new EPPSLoanProgramFilter(Guid.NewGuid(), p.ProgramID, p.ProgramName);
              gvItem.Tag = (object) loanProgramFilter;
              this.gvLP.Items.Add(gvItem);
            }
          }
        }
        if (loanProgramSelector.selectedPrograms.Count<EPPSLoanProgram>() > 0)
          this.modified = true;
      }
      this.gvLP.Sort(1, SortOrder.Ascending);
    }

    private void gvLP_Click(object sender, EventArgs e)
    {
      this.btnRemove.Enabled = this.gvLP.SelectedItems.Count > 0 && !this.readOnly;
    }

    private void gvLP_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnRemove.Enabled = this.gvLP.SelectedItems.Count > 0 && !this.readOnly;
    }

    private void btnRemove_Click(object sender, EventArgs e)
    {
      if (this.gvLP.SelectedItems.Count > 0 && Utils.Dialog((IWin32Window) this, "Are you sure you want to delete these entries?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
        return;
      while (this.gvLP.SelectedItems.Count > 0)
        this.gvLP.Items.Remove(this.gvLP.SelectedItems[0]);
      this.onDataChange();
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
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      this.grpHeader = new GroupContainer();
      this.pnlBody = new Panel();
      this.tableLayoutPanel1 = new TableLayoutPanel();
      this.pnlFields = new Panel();
      this.label2 = new Label();
      this.txtMinNoteRate = new TextBox();
      this.txtMaxNoteRate = new TextBox();
      this.label3 = new Label();
      this.label4 = new Label();
      this.grpEPPSProgram = new GroupContainer();
      this.btnRemove = new StandardIconButton();
      this.btnAdd = new StandardIconButton();
      this.gvLP = new GridView();
      this.flpHeader = new FlowLayoutPanel();
      this.toolTip1 = new ToolTip(this.components);
      this.grpHeader.SuspendLayout();
      this.pnlBody.SuspendLayout();
      this.tableLayoutPanel1.SuspendLayout();
      this.pnlFields.SuspendLayout();
      this.grpEPPSProgram.SuspendLayout();
      ((ISupportInitialize) this.btnRemove).BeginInit();
      ((ISupportInitialize) this.btnAdd).BeginInit();
      this.SuspendLayout();
      this.grpHeader.Controls.Add((Control) this.pnlBody);
      this.grpHeader.Controls.Add((Control) this.flpHeader);
      this.grpHeader.Dock = DockStyle.Fill;
      this.grpHeader.HeaderForeColor = SystemColors.ControlText;
      this.grpHeader.Location = new Point(0, 0);
      this.grpHeader.Name = "grpHeader";
      this.grpHeader.Size = new Size(951, 494);
      this.grpHeader.TabIndex = 11;
      this.grpHeader.Text = "Eligible Loans";
      this.pnlBody.AutoScroll = true;
      this.pnlBody.AutoScrollMinSize = new Size(677, 210);
      this.pnlBody.Controls.Add((Control) this.tableLayoutPanel1);
      this.pnlBody.Dock = DockStyle.Fill;
      this.pnlBody.Location = new Point(1, 26);
      this.pnlBody.Name = "pnlBody";
      this.pnlBody.Size = new Size(949, 467);
      this.pnlBody.TabIndex = 9;
      this.tableLayoutPanel1.ColumnCount = 1;
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20f));
      this.tableLayoutPanel1.Controls.Add((Control) this.pnlFields, 0, 1);
      this.tableLayoutPanel1.Controls.Add((Control) this.grpEPPSProgram, 0, 0);
      this.tableLayoutPanel1.Dock = DockStyle.Fill;
      this.tableLayoutPanel1.Location = new Point(0, 0);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 2;
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 58f));
      this.tableLayoutPanel1.Size = new Size(949, 467);
      this.tableLayoutPanel1.TabIndex = 1;
      this.pnlFields.BackColor = Color.Transparent;
      this.pnlFields.BorderStyle = BorderStyle.FixedSingle;
      this.pnlFields.Controls.Add((Control) this.label2);
      this.pnlFields.Controls.Add((Control) this.txtMinNoteRate);
      this.pnlFields.Controls.Add((Control) this.txtMaxNoteRate);
      this.pnlFields.Controls.Add((Control) this.label3);
      this.pnlFields.Controls.Add((Control) this.label4);
      this.pnlFields.Dock = DockStyle.Fill;
      this.pnlFields.Location = new Point(3, 412);
      this.pnlFields.Name = "pnlFields";
      this.pnlFields.Size = new Size(943, 52);
      this.pnlFields.TabIndex = 6;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(22, 19);
      this.label2.Name = "label2";
      this.label2.Size = new Size(56, 13);
      this.label2.TabIndex = 4;
      this.label2.Text = "Note Rate";
      this.txtMinNoteRate.Location = new Point(83, 15);
      this.txtMinNoteRate.MaxLength = 6;
      this.txtMinNoteRate.Name = "txtMinNoteRate";
      this.txtMinNoteRate.Size = new Size(72, 20);
      this.txtMinNoteRate.TabIndex = 1;
      this.txtMinNoteRate.TextAlign = HorizontalAlignment.Right;
      this.txtMinNoteRate.TextChanged += new EventHandler(this.onControlValueChanges);
      this.txtMinNoteRate.Validating += new CancelEventHandler(this.validateNoteRateRange);
      this.txtMaxNoteRate.Location = new Point(169, 15);
      this.txtMaxNoteRate.MaxLength = 6;
      this.txtMaxNoteRate.Name = "txtMaxNoteRate";
      this.txtMaxNoteRate.Size = new Size(72, 20);
      this.txtMaxNoteRate.TabIndex = 2;
      this.txtMaxNoteRate.TextAlign = HorizontalAlignment.Right;
      this.txtMaxNoteRate.TextChanged += new EventHandler(this.onControlValueChanges);
      this.txtMaxNoteRate.Validating += new CancelEventHandler(this.validateNoteRateRange);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(157, 19);
      this.label3.Name = "label3";
      this.label3.Size = new Size(10, 13);
      this.label3.TabIndex = 7;
      this.label3.Text = "-";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(243, 19);
      this.label4.Name = "label4";
      this.label4.Size = new Size(15, 13);
      this.label4.TabIndex = 8;
      this.label4.Text = "%";
      this.grpEPPSProgram.Controls.Add((Control) this.btnRemove);
      this.grpEPPSProgram.Controls.Add((Control) this.btnAdd);
      this.grpEPPSProgram.Controls.Add((Control) this.gvLP);
      this.grpEPPSProgram.Dock = DockStyle.Fill;
      this.grpEPPSProgram.HeaderForeColor = SystemColors.ControlText;
      this.grpEPPSProgram.Location = new Point(3, 3);
      this.grpEPPSProgram.Name = "grpEPPSProgram";
      this.grpEPPSProgram.Size = new Size(943, 403);
      this.grpEPPSProgram.TabIndex = 5;
      this.grpEPPSProgram.Text = "Select ICE PPE Loan Programs";
      this.btnRemove.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnRemove.BackColor = Color.Transparent;
      this.btnRemove.Enabled = false;
      this.btnRemove.Location = new Point(923, 4);
      this.btnRemove.MouseDownImage = (Image) null;
      this.btnRemove.Name = "btnRemove";
      this.btnRemove.Size = new Size(16, 16);
      this.btnRemove.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnRemove.TabIndex = 106;
      this.btnRemove.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnRemove, "Remove Loan Program");
      this.btnRemove.Click += new EventHandler(this.btnRemove_Click);
      this.btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAdd.BackColor = Color.Transparent;
      this.btnAdd.Location = new Point(903, 4);
      this.btnAdd.MouseDownImage = (Image) null;
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(16, 16);
      this.btnAdd.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAdd.TabIndex = 105;
      this.btnAdd.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnAdd, "Add Loan Program");
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.gvLP.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.SortMethod = GVSortMethod.Numeric;
      gvColumn1.Text = "Program ID";
      gvColumn1.Width = 100;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Program Name";
      gvColumn2.Width = 300;
      this.gvLP.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvLP.Dock = DockStyle.Fill;
      this.gvLP.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvLP.Location = new Point(1, 26);
      this.gvLP.Name = "gvLP";
      this.gvLP.Size = new Size(941, 376);
      this.gvLP.TabIndex = 104;
      this.gvLP.SelectedIndexChanged += new EventHandler(this.gvLP_SelectedIndexChanged);
      this.gvLP.Click += new EventHandler(this.gvLP_Click);
      this.flpHeader.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.flpHeader.BackColor = Color.Transparent;
      this.flpHeader.FlowDirection = FlowDirection.RightToLeft;
      this.flpHeader.Location = new Point(87, 2);
      this.flpHeader.Name = "flpHeader";
      this.flpHeader.Size = new Size(858, 22);
      this.flpHeader.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.grpHeader);
      this.Name = nameof (SelectEPPSLoanProgramControl);
      this.Size = new Size(951, 494);
      this.Resize += new EventHandler(this.SelectEPPSLoanProgramControl_Resize);
      this.grpHeader.ResumeLayout(false);
      this.pnlBody.ResumeLayout(false);
      this.tableLayoutPanel1.ResumeLayout(false);
      this.pnlFields.ResumeLayout(false);
      this.pnlFields.PerformLayout();
      this.grpEPPSProgram.ResumeLayout(false);
      ((ISupportInitialize) this.btnRemove).EndInit();
      ((ISupportInitialize) this.btnAdd).EndInit();
      this.ResumeLayout(false);
    }
  }
}
