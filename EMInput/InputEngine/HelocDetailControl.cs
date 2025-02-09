// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.HelocDetailControl
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class HelocDetailControl : UserControl
  {
    private Button cancelBtn;
    private Button okBtn;
    private Button addBtn;
    private Button deleteBtn;
    private Button editBtn;
    private Label labelNote;
    private HelocRateTable helocTable;
    private bool isDirty;
    private Button duplicateBtn;
    private GridView gridViewYearList;
    private EMHelpLink emHelpLink1;
    private Sessions.Session session;
    private Panel panelNewHeloc;
    private ComboBox cboIndexName;
    private Label label5;
    private TextBox txtMargin;
    private Label label4;
    private ComboBox cboMonth;
    private Label label3;
    private ComboBox cboDay;
    private Label label2;
    private Panel panelBottom;
    private Panel panelMiddle;
    private Panel panelTable;
    private Panel panelButtons;
    private bool useNewHELOCHistoricTable;
    private FieldDefinition field1959;
    private CheckBox chkUseAlternateSchedule;
    private Label label1;
    private ComboBox cboPrecision;
    private Panel panelPrecision;
    private Panel panelForDynamic;
    private System.ComponentModel.Container components;

    public event EventHandler HelocButtonClick;

    public HelocDetailControl(HelocRateTable helocTable, bool readOnly)
      : this(helocTable, readOnly, Session.DefaultInstance)
    {
    }

    public HelocDetailControl(HelocRateTable helocTable, bool readOnly, Sessions.Session session)
      : this(helocTable, readOnly, session, false)
    {
    }

    public HelocDetailControl(
      HelocRateTable helocTable,
      bool readOnly,
      Sessions.Session session,
      bool useNewHELOCHistoricTable)
    {
      this.session = session;
      this.useNewHELOCHistoricTable = useNewHELOCHistoricTable;
      this.helocTable = helocTable;
      this.InitializeComponent();
      if (!this.useNewHELOCHistoricTable)
      {
        this.panelForDynamic.Visible = false;
        this.panelPrecision.Top = 0;
        this.panelPrecision.Left = 0;
        this.panelNewHeloc.Height = 25;
      }
      else
      {
        this.gridViewYearList.Columns.RemoveAt(5);
        this.gridViewYearList.Columns.RemoveAt(4);
        this.gridViewYearList.Columns.RemoveAt(3);
        this.gridViewYearList.Columns.RemoveAt(1);
        this.gridViewYearList.Columns[0].Width = this.gridViewYearList.Width / 2;
        this.gridViewYearList.Columns[1].SpringToFit = true;
        this.cboDay.Items.Add((object) "");
        for (int index = 1; index <= 31; ++index)
          this.cboDay.Items.Add((object) index);
        this.cboMonth.Items.Add((object) "");
        for (int index = 1; index <= 12; ++index)
          this.cboMonth.Items.Add((object) index);
        this.field1959 = EncompassFields.GetField("1959");
        if (this.field1959.Options != null && this.field1959.Options.Count > 0)
        {
          for (int index = 0; index < this.field1959.Options.Count; ++index)
            this.cboIndexName.Items.Add((object) this.field1959.Options[index].Text);
        }
        this.labelNote.Visible = false;
      }
      this.emHelpLink1.AssignSession(this.session);
      if (readOnly)
      {
        this.okBtn.Text = "Close";
        this.addBtn.Visible = false;
        this.editBtn.Visible = false;
        this.deleteBtn.Visible = false;
        this.cancelBtn.Visible = false;
      }
      this.initForm();
      this.Dock = DockStyle.Fill;
      this.isDirty = false;
      this.cboPrecision.SelectedIndexChanged += new EventHandler(this.cboPrecision_SelectedIndexChanged);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    public HelocRateTable HelocTable => this.helocTable;

    public int HelocCount => this.gridViewYearList.Items.Count;

    private void InitializeComponent()
    {
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      this.cancelBtn = new Button();
      this.okBtn = new Button();
      this.addBtn = new Button();
      this.deleteBtn = new Button();
      this.editBtn = new Button();
      this.labelNote = new Label();
      this.duplicateBtn = new Button();
      this.gridViewYearList = new GridView();
      this.panelNewHeloc = new Panel();
      this.panelPrecision = new Panel();
      this.cboPrecision = new ComboBox();
      this.label1 = new Label();
      this.panelForDynamic = new Panel();
      this.label2 = new Label();
      this.cboDay = new ComboBox();
      this.label3 = new Label();
      this.chkUseAlternateSchedule = new CheckBox();
      this.cboMonth = new ComboBox();
      this.cboIndexName = new ComboBox();
      this.label4 = new Label();
      this.label5 = new Label();
      this.txtMargin = new TextBox();
      this.panelBottom = new Panel();
      this.emHelpLink1 = new EMHelpLink();
      this.panelMiddle = new Panel();
      this.panelTable = new Panel();
      this.panelButtons = new Panel();
      this.panelNewHeloc.SuspendLayout();
      this.panelPrecision.SuspendLayout();
      this.panelForDynamic.SuspendLayout();
      this.panelBottom.SuspendLayout();
      this.panelMiddle.SuspendLayout();
      this.panelTable.SuspendLayout();
      this.panelButtons.SuspendLayout();
      this.SuspendLayout();
      this.cancelBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.cancelBtn.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.cancelBtn.Location = new Point(477, 11);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 24);
      this.cancelBtn.TabIndex = 11;
      this.cancelBtn.Text = "&Cancel";
      this.cancelBtn.Click += new EventHandler(this.cancelBtn_Click);
      this.okBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.okBtn.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.okBtn.Location = new Point(396, 11);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 24);
      this.okBtn.TabIndex = 10;
      this.okBtn.Text = "&OK";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.addBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.addBtn.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.addBtn.Location = new Point(3, -1);
      this.addBtn.Name = "addBtn";
      this.addBtn.Size = new Size(75, 24);
      this.addBtn.TabIndex = 6;
      this.addBtn.Text = "&Add";
      this.addBtn.Click += new EventHandler(this.addBtn_Click);
      this.deleteBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.deleteBtn.Enabled = false;
      this.deleteBtn.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.deleteBtn.Location = new Point(3, 61);
      this.deleteBtn.Name = "deleteBtn";
      this.deleteBtn.Size = new Size(75, 24);
      this.deleteBtn.TabIndex = 8;
      this.deleteBtn.Text = "&Delete";
      this.deleteBtn.Click += new EventHandler(this.deleteBtn_Click);
      this.editBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.editBtn.Enabled = false;
      this.editBtn.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.editBtn.Location = new Point(3, 30);
      this.editBtn.Name = "editBtn";
      this.editBtn.Size = new Size(75, 24);
      this.editBtn.TabIndex = 7;
      this.editBtn.Text = "&Edit";
      this.editBtn.Click += new EventHandler(this.editBtn_Click);
      this.labelNote.AutoSize = true;
      this.labelNote.Location = new Point(3, 2);
      this.labelNote.Name = "labelNote";
      this.labelNote.Size = new Size(369, 13);
      this.labelNote.TabIndex = 0;
      this.labelNote.Text = "* Minimum Monthly Payment is calculated based on loan amount $10,000.00.";
      this.duplicateBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.duplicateBtn.Enabled = false;
      this.duplicateBtn.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.duplicateBtn.Location = new Point(3, 92);
      this.duplicateBtn.Name = "duplicateBtn";
      this.duplicateBtn.Size = new Size(75, 24);
      this.duplicateBtn.TabIndex = 9;
      this.duplicateBtn.Text = "Du&plicate";
      this.duplicateBtn.Click += new EventHandler(this.duplicateBtn_Click);
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.SortMethod = GVSortMethod.Numeric;
      gvColumn1.Text = "Year";
      gvColumn1.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn1.Width = 60;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Period Type";
      gvColumn2.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn2.Width = 76;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.SortMethod = GVSortMethod.Numeric;
      gvColumn3.Text = "Index %";
      gvColumn3.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn3.Width = 60;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.SortMethod = GVSortMethod.Numeric;
      gvColumn4.Text = "Margin %";
      gvColumn4.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn4.Width = 60;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.SortMethod = GVSortMethod.Numeric;
      gvColumn5.Text = "APR %";
      gvColumn5.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn5.Width = 60;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column6";
      gvColumn6.SortMethod = GVSortMethod.Numeric;
      gvColumn6.SpringToFit = true;
      gvColumn6.Text = "Minimum Monthly Payment";
      gvColumn6.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn6.Width = 159;
      this.gridViewYearList.Columns.AddRange(new GVColumn[6]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6
      });
      this.gridViewYearList.Dock = DockStyle.Fill;
      this.gridViewYearList.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridViewYearList.Location = new Point(0, 0);
      this.gridViewYearList.Name = "gridViewYearList";
      this.gridViewYearList.Size = new Size(477, 257);
      this.gridViewYearList.SortOption = GVSortOption.None;
      this.gridViewYearList.TabIndex = 0;
      this.gridViewYearList.SelectedIndexChanged += new EventHandler(this.gridViewYearList_SelectedIndexChanged);
      this.gridViewYearList.DoubleClick += new EventHandler(this.editBtn_Click);
      this.panelNewHeloc.Controls.Add((Control) this.panelPrecision);
      this.panelNewHeloc.Controls.Add((Control) this.panelForDynamic);
      this.panelNewHeloc.Dock = DockStyle.Top;
      this.panelNewHeloc.Location = new Point(0, 0);
      this.panelNewHeloc.Name = "panelNewHeloc";
      this.panelNewHeloc.Size = new Size(555, 105);
      this.panelNewHeloc.TabIndex = 16;
      this.panelPrecision.Controls.Add((Control) this.cboPrecision);
      this.panelPrecision.Controls.Add((Control) this.label1);
      this.panelPrecision.Location = new Point(236, 28);
      this.panelPrecision.Name = "panelPrecision";
      this.panelPrecision.Size = new Size(245, 22);
      this.panelPrecision.TabIndex = 8;
      this.cboPrecision.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboPrecision.FormattingEnabled = true;
      this.cboPrecision.Items.AddRange(new object[2]
      {
        (object) "Three Decimals",
        (object) "Five Decimals"
      });
      this.cboPrecision.Location = new Point(128, 0);
      this.cboPrecision.Name = "cboPrecision";
      this.cboPrecision.Size = new Size(113, 21);
      this.cboPrecision.TabIndex = 3;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(4, 3);
      this.label1.Name = "label1";
      this.label1.Size = new Size(118, 13);
      this.label1.TabIndex = 6;
      this.label1.Text = "HELOC Index Precision";
      this.panelForDynamic.Controls.Add((Control) this.label2);
      this.panelForDynamic.Controls.Add((Control) this.cboDay);
      this.panelForDynamic.Controls.Add((Control) this.label3);
      this.panelForDynamic.Controls.Add((Control) this.chkUseAlternateSchedule);
      this.panelForDynamic.Controls.Add((Control) this.cboMonth);
      this.panelForDynamic.Controls.Add((Control) this.cboIndexName);
      this.panelForDynamic.Controls.Add((Control) this.label4);
      this.panelForDynamic.Controls.Add((Control) this.label5);
      this.panelForDynamic.Controls.Add((Control) this.txtMargin);
      this.panelForDynamic.Dock = DockStyle.Left;
      this.panelForDynamic.Location = new Point(0, 0);
      this.panelForDynamic.Name = "panelForDynamic";
      this.panelForDynamic.Size = new Size(486, 105);
      this.panelForDynamic.TabIndex = 7;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(3, 7);
      this.label2.Name = "label2";
      this.label2.Size = new Size(81, 13);
      this.label2.TabIndex = 0;
      this.label2.Text = "Index as of Day";
      this.cboDay.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboDay.FormattingEnabled = true;
      this.cboDay.Location = new Point(128, 4);
      this.cboDay.Name = "cboDay";
      this.cboDay.Size = new Size(86, 21);
      this.cboDay.TabIndex = 0;
      this.cboDay.SelectedIndexChanged += new EventHandler(this.field_SelectedIndexChanged);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(3, 31);
      this.label3.Name = "label3";
      this.label3.Size = new Size(92, 13);
      this.label3.TabIndex = 1;
      this.label3.Text = "Index as of Month";
      this.chkUseAlternateSchedule.AutoSize = true;
      this.chkUseAlternateSchedule.Location = new Point(243, 55);
      this.chkUseAlternateSchedule.Name = "chkUseAlternateSchedule";
      this.chkUseAlternateSchedule.Size = new Size(138, 17);
      this.chkUseAlternateSchedule.TabIndex = 4;
      this.chkUseAlternateSchedule.Text = "Use Alternate Schedule";
      this.chkUseAlternateSchedule.UseVisualStyleBackColor = true;
      this.chkUseAlternateSchedule.CheckedChanged += new EventHandler(this.useAlternateSchedule_checkChanged);
      this.cboMonth.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboMonth.FormattingEnabled = true;
      this.cboMonth.Location = new Point(128, 28);
      this.cboMonth.Name = "cboMonth";
      this.cboMonth.Size = new Size(86, 21);
      this.cboMonth.TabIndex = 1;
      this.cboMonth.SelectedIndexChanged += new EventHandler(this.field_SelectedIndexChanged);
      this.cboIndexName.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboIndexName.FormattingEnabled = true;
      this.cboIndexName.Location = new Point(128, 75);
      this.cboIndexName.Name = "cboIndexName";
      this.cboIndexName.Size = new Size(350, 21);
      this.cboIndexName.TabIndex = 5;
      this.cboIndexName.SelectedIndexChanged += new EventHandler(this.field_SelectedIndexChanged);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(3, 55);
      this.label4.Name = "label4";
      this.label4.Size = new Size(122, 13);
      this.label4.TabIndex = 0;
      this.label4.Text = "Default Historical Margin";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(3, 78);
      this.label5.Name = "label5";
      this.label5.Size = new Size(64, 13);
      this.label5.TabIndex = 0;
      this.label5.Text = "Index Name";
      this.txtMargin.Location = new Point(128, 52);
      this.txtMargin.Name = "txtMargin";
      this.txtMargin.Size = new Size(86, 20);
      this.txtMargin.TabIndex = 2;
      this.txtMargin.TextChanged += new EventHandler(this.txtMargin_TextChanged);
      this.txtMargin.KeyPress += new KeyPressEventHandler(this.txtMargin_KeyPress);
      this.txtMargin.KeyUp += new KeyEventHandler(this.txtMargin_KeyUp);
      this.txtMargin.Leave += new EventHandler(this.txtMargin_Leave);
      this.panelBottom.Controls.Add((Control) this.labelNote);
      this.panelBottom.Controls.Add((Control) this.emHelpLink1);
      this.panelBottom.Controls.Add((Control) this.cancelBtn);
      this.panelBottom.Controls.Add((Control) this.okBtn);
      this.panelBottom.Dock = DockStyle.Bottom;
      this.panelBottom.Location = new Point(0, 362);
      this.panelBottom.Name = "panelBottom";
      this.panelBottom.Size = new Size(555, 43);
      this.panelBottom.TabIndex = 6;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = "HELOC Table";
      this.emHelpLink1.Location = new Point(3, 23);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 1;
      this.panelMiddle.Controls.Add((Control) this.panelTable);
      this.panelMiddle.Controls.Add((Control) this.panelButtons);
      this.panelMiddle.Dock = DockStyle.Fill;
      this.panelMiddle.Location = new Point(0, 105);
      this.panelMiddle.Name = "panelMiddle";
      this.panelMiddle.Size = new Size(555, 257);
      this.panelMiddle.TabIndex = 18;
      this.panelTable.Controls.Add((Control) this.gridViewYearList);
      this.panelTable.Dock = DockStyle.Fill;
      this.panelTable.Location = new Point(0, 0);
      this.panelTable.Name = "panelTable";
      this.panelTable.Size = new Size(477, 257);
      this.panelTable.TabIndex = 16;
      this.panelButtons.Controls.Add((Control) this.addBtn);
      this.panelButtons.Controls.Add((Control) this.deleteBtn);
      this.panelButtons.Controls.Add((Control) this.editBtn);
      this.panelButtons.Controls.Add((Control) this.duplicateBtn);
      this.panelButtons.Dock = DockStyle.Right;
      this.panelButtons.Location = new Point(477, 0);
      this.panelButtons.Name = "panelButtons";
      this.panelButtons.Size = new Size(78, 257);
      this.panelButtons.TabIndex = 15;
      this.Controls.Add((Control) this.panelMiddle);
      this.Controls.Add((Control) this.panelBottom);
      this.Controls.Add((Control) this.panelNewHeloc);
      this.Name = nameof (HelocDetailControl);
      this.Size = new Size(555, 405);
      this.panelNewHeloc.ResumeLayout(false);
      this.panelPrecision.ResumeLayout(false);
      this.panelPrecision.PerformLayout();
      this.panelForDynamic.ResumeLayout(false);
      this.panelForDynamic.PerformLayout();
      this.panelBottom.ResumeLayout(false);
      this.panelBottom.PerformLayout();
      this.panelMiddle.ResumeLayout(false);
      this.panelTable.ResumeLayout(false);
      this.panelButtons.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public void ResetForm(HelocRateTable helocTable)
    {
      this.helocTable = helocTable;
      this.initForm();
      this.sortList();
      this.isDirty = true;
    }

    private void sortList()
    {
      this.gridViewYearList.SortOption = GVSortOption.Auto;
      this.gridViewYearList.Sort(0, SortOrder.Ascending);
      this.gridViewYearList.SortOption = GVSortOption.None;
    }

    private void initForm()
    {
      this.gridViewYearList.Items.Clear();
      if (this.helocTable == null)
      {
        this.cboPrecision.SelectedIndex = 0;
      }
      else
      {
        if (this.useNewHELOCHistoricTable)
        {
          if (this.helocTable.IndexDay > 0)
            this.cboDay.SelectedIndex = this.helocTable.IndexDay;
          if (this.helocTable.IndexMonth > 0)
            this.cboMonth.SelectedIndex = this.helocTable.IndexMonth;
          if ((this.helocTable.IndexName ?? "") != "")
          {
            if (this.field1959 != null && this.field1959.Options != null && this.field1959.Options.Count > 0)
            {
              for (int index = 0; index < this.field1959.Options.Count; ++index)
              {
                if (string.Compare(this.helocTable.IndexName, this.field1959.Options[index].Value, true) == 0)
                {
                  this.cboIndexName.Text = this.field1959.Options[index].Text;
                  break;
                }
              }
            }
            this.cboIndexName.Text = this.helocTable.IndexName;
          }
          this.chkUseAlternateSchedule.Checked = this.helocTable.UseAlternateSchedule;
          if (this.helocTable.DefaultHistoricMargin != 0M)
            this.txtMargin.Text = Utils.ArithmeticRounding(this.helocTable.DefaultHistoricMargin, 3).ToString("N3");
        }
        this.cboPrecision.SelectedIndex = this.helocTable.DecimalsUseForIndex == "FiveDecimals" ? 1 : 0;
        for (int i = 0; i < this.helocTable.Count; ++i)
        {
          HelocRateTable.YearRecord yearRecordAt = this.helocTable.GetYearRecordAt(i);
          GVItem gvItem = new GVItem(yearRecordAt.Year);
          if (this.useNewHELOCHistoricTable)
          {
            gvItem.SubItems.Add((object) yearRecordAt.IndexRate);
          }
          else
          {
            gvItem.SubItems.Add((object) yearRecordAt.PeriodType);
            gvItem.SubItems.Add((object) yearRecordAt.IndexRate);
            gvItem.SubItems.Add((object) yearRecordAt.MarginRate);
            gvItem.SubItems.Add((object) yearRecordAt.APR);
            gvItem.SubItems.Add((object) yearRecordAt.MinimumPayment);
          }
          this.gridViewYearList.Items.Add(gvItem);
        }
        if (this.gridViewYearList.Items.Count <= 0)
          return;
        this.gridViewYearList.Items[0].Selected = true;
      }
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      if (this.HelocButtonClick == null)
        return;
      if (this.isDirty)
      {
        this.sortList();
        if (this.gridViewYearList.Items.Count == 0)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Please enter index rates and years to list.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
        this.helocTable = new HelocRateTable();
        if (this.useNewHELOCHistoricTable)
        {
          this.helocTable.IsNewHELOC = true;
          this.helocTable.IndexDay = this.cboDay.Text == "" ? 0 : Utils.ParseInt((object) this.cboDay.Text);
          this.helocTable.IndexMonth = this.cboMonth.Text == "" ? 0 : Utils.ParseInt((object) this.cboMonth.Text);
          this.helocTable.UseAlternateSchedule = this.chkUseAlternateSchedule.Checked;
          this.helocTable.DefaultHistoricMargin = Utils.ParseDecimal((object) this.txtMargin.Text, 0M);
          this.helocTable.IndexName = this.cboIndexName.Text;
          this.helocTable.DecimalsUseForIndex = this.cboPrecision.SelectedIndex == 1 ? "FiveDecimals" : "ThreeDecimals";
          if (this.field1959 != null && this.field1959.Options != null && this.field1959.Options.Count > 0)
          {
            for (int index = 0; index < this.field1959.Options.Count; ++index)
            {
              if (string.Compare(this.cboIndexName.Text, this.field1959.Options[index].Text, true) == 0)
              {
                this.helocTable.IndexName = this.field1959.Options[index].Value;
                break;
              }
            }
          }
        }
        else
        {
          this.helocTable.IsNewHELOC = false;
          this.helocTable.DecimalsUseForIndex = this.cboPrecision.SelectedIndex == 1 ? "FiveDecimals" : "ThreeDecimals";
        }
        for (int nItemIndex = 0; nItemIndex < this.gridViewYearList.Items.Count; ++nItemIndex)
        {
          if (this.useNewHELOCHistoricTable)
            this.helocTable.InsertYearRecord(this.gridViewYearList.Items[nItemIndex].Text, "", this.gridViewYearList.Items[nItemIndex].SubItems[1].Text, "", "", "");
          else
            this.helocTable.InsertYearRecord(this.gridViewYearList.Items[nItemIndex].Text, this.gridViewYearList.Items[nItemIndex].SubItems[1].Text, this.gridViewYearList.Items[nItemIndex].SubItems[2].Text, this.gridViewYearList.Items[nItemIndex].SubItems[3].Text, this.gridViewYearList.Items[nItemIndex].SubItems[4].Text, this.gridViewYearList.Items[nItemIndex].SubItems[5].Text);
        }
      }
      this.HelocButtonClick(sender, e);
    }

    private void cancelBtn_Click(object sender, EventArgs e)
    {
      if (this.HelocButtonClick == null)
        return;
      this.HelocButtonClick(sender, e);
    }

    private void addBtn_Click(object sender, EventArgs e)
    {
      using (MMPFormDialog mmpFormDialog = new MMPFormDialog(this.getExistingYears(), this.useNewHELOCHistoricTable, this.cboPrecision.SelectedIndex == 1 ? "FiveDecimals" : "ThreeDecimals"))
      {
        if (mmpFormDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.gridViewYearList.SelectedItems.Clear();
        GVItem gvItem = new GVItem(mmpFormDialog.Year.ToString());
        if (this.useNewHELOCHistoricTable)
        {
          gvItem.SubItems.Add((object) mmpFormDialog.IndexRate.ToString(this.cboPrecision.SelectedIndex == 1 ? "N5" : "N3"));
        }
        else
        {
          gvItem.SubItems.Add((object) mmpFormDialog.PeriodType);
          gvItem.SubItems.Add((object) mmpFormDialog.IndexRate.ToString(this.cboPrecision.SelectedIndex == 1 ? "N5" : "N3"));
          gvItem.SubItems.Add((object) mmpFormDialog.MarginRate.ToString("N3"));
          gvItem.SubItems.Add((object) mmpFormDialog.AprRate.ToString("N3"));
          gvItem.SubItems.Add((object) mmpFormDialog.MonthlyPay.ToString("N2"));
        }
        gvItem.Selected = true;
        this.gridViewYearList.Items.Add(gvItem);
        this.isDirty = true;
        this.sortList();
      }
    }

    private void editBtn_Click(object sender, EventArgs e)
    {
      if (this.gridViewYearList.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You have to select an item first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        int year = Utils.ParseInt((object) this.gridViewYearList.SelectedItems[0].Text);
        GVSubItemCollection subItems = this.gridViewYearList.SelectedItems[0].SubItems;
        int nItemIndex = this.useNewHELOCHistoricTable ? 1 : 2;
        double num2;
        double indexRate = num2 = (double) Utils.ParseDecimal((object) subItems[nItemIndex].Text);
        string periodType = "";
        double marginRate = 0.0;
        double aprRate = 0.0;
        double monthlyPay = 0.0;
        if (!this.useNewHELOCHistoricTable)
        {
          periodType = this.gridViewYearList.SelectedItems[0].SubItems[1].Text;
          marginRate = (double) Utils.ParseDecimal((object) this.gridViewYearList.SelectedItems[0].SubItems[3].Text);
          aprRate = (double) Utils.ParseDecimal((object) this.gridViewYearList.SelectedItems[0].SubItems[4].Text);
          monthlyPay = (double) Utils.ParseDecimal((object) this.gridViewYearList.SelectedItems[0].SubItems[5].Text);
        }
        ArrayList existingYears = this.getExistingYears();
        using (MMPFormDialog mmpFormDialog = new MMPFormDialog(year, periodType, indexRate, marginRate, aprRate, monthlyPay, existingYears, this.useNewHELOCHistoricTable, this.cboPrecision.SelectedIndex == 1 ? "FiveDecimals" : "ThreeDecimals"))
        {
          if (mmpFormDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return;
          this.gridViewYearList.SelectedItems[0].Text = mmpFormDialog.Year.ToString();
          GVSubItem subItem1 = this.gridViewYearList.SelectedItems[0].SubItems[this.useNewHELOCHistoricTable ? 1 : 2];
          double num3 = mmpFormDialog.IndexRate;
          string str1 = num3.ToString(this.cboPrecision.SelectedIndex == 1 ? "N5" : "N3");
          subItem1.Text = str1;
          if (!this.useNewHELOCHistoricTable)
          {
            this.gridViewYearList.SelectedItems[0].SubItems[1].Text = mmpFormDialog.PeriodType;
            GVSubItem subItem2 = this.gridViewYearList.SelectedItems[0].SubItems[3];
            num3 = mmpFormDialog.MarginRate;
            string str2 = num3.ToString("N3");
            subItem2.Text = str2;
            GVSubItem subItem3 = this.gridViewYearList.SelectedItems[0].SubItems[4];
            num3 = mmpFormDialog.AprRate;
            string str3 = num3.ToString("N3");
            subItem3.Text = str3;
            GVSubItem subItem4 = this.gridViewYearList.SelectedItems[0].SubItems[5];
            num3 = mmpFormDialog.MonthlyPay;
            string str4 = num3.ToString("N2");
            subItem4.Text = str4;
          }
          this.isDirty = true;
          this.sortList();
        }
      }
    }

    private ArrayList getExistingYears()
    {
      ArrayList existingYears = new ArrayList();
      for (int nItemIndex = 0; nItemIndex < this.gridViewYearList.Items.Count; ++nItemIndex)
        existingYears.Add((object) this.gridViewYearList.Items[nItemIndex].Text);
      return existingYears;
    }

    private void deleteBtn_Click(object sender, EventArgs e)
    {
      int index = this.gridViewYearList.SelectedItems[0].Index;
      if (Utils.Dialog((IWin32Window) this, "Are you sure you want to delete selected year" + (this.gridViewYearList.SelectedItems.Count > 1 ? "s?" : "?"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
        return;
      List<GVItem> gvItemList = new List<GVItem>();
      foreach (GVItem selectedItem in this.gridViewYearList.SelectedItems)
        gvItemList.Add(selectedItem);
      foreach (GVItem gvItem in gvItemList)
        this.gridViewYearList.Items.Remove(gvItem);
      this.isDirty = true;
      if (this.gridViewYearList.Items.Count == 0)
        return;
      if (index + 1 > this.gridViewYearList.Items.Count)
        this.gridViewYearList.Items[this.gridViewYearList.Items.Count - 1].Selected = true;
      else
        this.gridViewYearList.Items[index].Selected = true;
    }

    private void yearListView_DoubleClick(object sender, EventArgs e)
    {
      this.editBtn_Click((object) null, (EventArgs) null);
    }

    private void duplicateBtn_Click(object sender, EventArgs e)
    {
      int year = 0;
      double indexRate = (double) Utils.ParseDecimal((object) this.gridViewYearList.SelectedItems[0].SubItems[this.useNewHELOCHistoricTable ? 1 : 2].Text);
      string text = this.useNewHELOCHistoricTable ? "" : this.gridViewYearList.SelectedItems[0].SubItems[1].Text;
      double marginRate = this.useNewHELOCHistoricTable ? 0.0 : (double) Utils.ParseDecimal((object) this.gridViewYearList.SelectedItems[0].SubItems[3].Text);
      double aprRate = this.useNewHELOCHistoricTable ? 0.0 : (double) Utils.ParseDecimal((object) this.gridViewYearList.SelectedItems[0].SubItems[4].Text);
      double monthlyPay = this.useNewHELOCHistoricTable ? 0.0 : (double) Utils.ParseDecimal((object) this.gridViewYearList.SelectedItems[0].SubItems[5].Text);
      ArrayList existingYears = this.getExistingYears();
      using (MMPFormDialog mmpFormDialog = new MMPFormDialog(year, text, indexRate, marginRate, aprRate, monthlyPay, existingYears, this.useNewHELOCHistoricTable, this.cboPrecision.SelectedIndex == 1 ? "FiveDecimals" : "ThreeDecimals"))
      {
        if (mmpFormDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.gridViewYearList.SelectedItems.Clear();
        GVItem gvItem = new GVItem(mmpFormDialog.Year.ToString());
        if (this.useNewHELOCHistoricTable)
        {
          gvItem.SubItems.Add((object) mmpFormDialog.IndexRate.ToString(this.cboPrecision.SelectedIndex == 1 ? "N5" : "N3"));
        }
        else
        {
          gvItem.SubItems.Add((object) mmpFormDialog.PeriodType);
          GVSubItemCollection subItems1 = gvItem.SubItems;
          double num = mmpFormDialog.IndexRate;
          string str1 = num.ToString("N3");
          subItems1.Add((object) str1);
          GVSubItemCollection subItems2 = gvItem.SubItems;
          num = mmpFormDialog.MarginRate;
          string str2 = num.ToString("N3");
          subItems2.Add((object) str2);
          GVSubItemCollection subItems3 = gvItem.SubItems;
          num = mmpFormDialog.AprRate;
          string str3 = num.ToString("N3");
          subItems3.Add((object) str3);
          GVSubItemCollection subItems4 = gvItem.SubItems;
          num = mmpFormDialog.MonthlyPay;
          string str4 = num.ToString("N2");
          subItems4.Add((object) str4);
        }
        gvItem.Selected = true;
        this.gridViewYearList.Items.Add(gvItem);
        this.isDirty = true;
        this.sortList();
      }
    }

    private void gridViewYearList_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.editBtn.Enabled = this.duplicateBtn.Enabled = this.gridViewYearList.SelectedItems.Count == 1;
      this.deleteBtn.Enabled = this.gridViewYearList.SelectedItems.Count > 0;
    }

    private void txtMargin_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        return;
      if (!char.IsDigit(e.KeyChar))
      {
        char keyChar = e.KeyChar;
        if (!keyChar.Equals('.'))
        {
          keyChar = e.KeyChar;
          if (!keyChar.Equals('-'))
          {
            e.Handled = true;
            return;
          }
        }
      }
      e.Handled = false;
    }

    private void txtMargin_Leave(object sender, EventArgs e)
    {
      this.txtMargin.Text = Utils.ApplyFieldFormatting(this.txtMargin.Text, FieldFormat.DECIMAL_3);
    }

    private void txtMargin_KeyUp(object sender, KeyEventArgs e)
    {
      FieldFormat dataFormat = FieldFormat.DECIMAL_3;
      TextBox textBox = (TextBox) sender;
      bool needsUpdate = false;
      string str = Utils.FormatInput(textBox.Text, dataFormat, ref needsUpdate);
      if (!needsUpdate)
        return;
      textBox.Text = str;
      textBox.SelectionStart = str.Length;
    }

    private void field_SelectedIndexChanged(object sender, EventArgs e) => this.isDirty = true;

    private void txtMargin_TextChanged(object sender, EventArgs e) => this.isDirty = true;

    private void useAlternateSchedule_checkChanged(object sender, EventArgs e)
    {
      this.isDirty = true;
    }

    private void cboPrecision_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.isDirty = true;
      if (this.gridViewYearList.Items.Count == 0)
        return;
      bool flag = this.cboPrecision.SelectedIndex == 1;
      int nItemIndex1 = this.useNewHELOCHistoricTable ? 1 : 2;
      for (int nItemIndex2 = 0; nItemIndex2 < this.gridViewYearList.Items.Count; ++nItemIndex2)
      {
        string text = this.gridViewYearList.Items[nItemIndex2].SubItems[nItemIndex1].Text;
        this.gridViewYearList.Items[nItemIndex2].SubItems[nItemIndex1].Text = Utils.ParseDecimal((object) text).ToString(flag ? "N5" : "N3");
      }
    }
  }
}
