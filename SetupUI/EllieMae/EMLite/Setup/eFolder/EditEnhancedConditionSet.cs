// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.eFolder.EditEnhancedConditionSet
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.eFolder
{
  public class EditEnhancedConditionSet : Form
  {
    private Sessions.Session session;
    private Guid setid;
    private GridViewFilterManager filterManager;
    private bool isNew;
    public string SetName = "";
    private ComboBox cbConditionType;
    private static readonly TableLayout ConditionSetsLayout = new TableLayout(new TableLayout.Column[2]
    {
      new TableLayout.Column("EnhancedConditionSet.ConditionName", "Condition Name", HorizontalAlignment.Left, 105),
      new TableLayout.Column("EnhancedConditionSet.ConditionType", "Condition Type", HorizontalAlignment.Left, 105)
    });
    private IContainer components;
    private TextBox txtDesc;
    private Label label2;
    private Label label1;
    private GridView gvAvailable;
    private GridView gvSelected;
    private Label label3;
    private Label label4;
    private Button btnAdd;
    private Button btnRemove;
    private Button btnSave;
    private Button btnCancel;
    private EMHelpLink emHelpLink1;
    private Panel pnlAddRemove;
    private Panel pnlSelected;
    private Panel pnlAllConds;
    private TextBox txtSetName;
    private GroupBox groupBox1;

    public event EventHandler FilterChanged;

    public EditEnhancedConditionSet(Sessions.Session session, object id)
    {
      this.session = session;
      this.InitializeComponent();
      this.emHelpLink1.AssignSession(session);
      if (id == null)
      {
        this.isNew = true;
        id = (object) Guid.NewGuid();
      }
      this.setid = (Guid) id;
      this.InitAvailableList();
      this.PopulateConditionTemplates();
    }

    public void InitAvailableList()
    {
      this.filterManager = new GridViewFilterManager(this.session, this.gvAvailable);
      this.filterManager.FilterChanged += new EventHandler(this.onFilterChanged);
    }

    public void PopulateConditionTemplates()
    {
      DataSet templateNameAndType = this.session.ConfigurationManager.GetEnhanceConditionTemplateNameAndType(this.setid);
      DataTable table1 = templateNameAndType.Tables[0];
      DataTable table2 = templateNameAndType.Tables[1];
      DataTable table3 = templateNameAndType.Tables[2];
      DataColumn[] dataColumnArray = new DataColumn[1]
      {
        table2.Columns["tempid"]
      };
      table2.PrimaryKey = dataColumnArray;
      if (table1.Rows.Count > 0)
      {
        this.txtSetName.Text = table1.Rows[0]["setname"].ToString();
        this.txtDesc.Text = table1.Rows[0]["description"].ToString();
      }
      foreach (DataRow row in (InternalDataCollectionBase) table2.Rows)
        this.gvSelected.Items.Add(new GVItem()
        {
          SubItems = {
            [0] = {
              Text = row["title"].ToString()
            },
            [1] = {
              Text = row["conditiontype"].ToString()
            }
          },
          Tag = (object) Guid.Parse(row["tempid"].ToString())
        });
      foreach (DataRow row in (InternalDataCollectionBase) table3.Rows)
      {
        if (!table2.Rows.Contains((object) Guid.Parse(row["tempid"].ToString())))
          this.gvAvailable.Items.Add(new GVItem()
          {
            SubItems = {
              [0] = {
                Text = row["title"].ToString()
              },
              [1] = {
                Text = row["conditiontype"].ToString()
              }
            },
            Tag = (object) Guid.Parse(row["tempid"].ToString())
          });
      }
      foreach (TableLayout.Column column in EditEnhancedConditionSet.ConditionSetsLayout)
      {
        GVColumn newColumn = new GVColumn();
        newColumn.Text = column.Description;
        newColumn.TextAlign = column.Alignment;
        newColumn.Width = column.Width;
        int columnIndex = this.gvAvailable.Columns.Add(newColumn);
        FieldDefinition field = (FieldDefinition) StandardFields.GetField(column.ColumnID);
        if (field == null && column.ColumnID == "EnhancedConditionSet.ConditionName")
        {
          this.filterManager.CreateColumnFilter(columnIndex, FieldFormat.STRING);
          newColumn.Tag = (object) "EnhancedConditionSet.ConditionName";
        }
        if (field == null && column.ColumnID == "EnhancedConditionSet.ConditionType")
        {
          this.cbConditionType = (ComboBox) this.filterManager.CreateColumnFilter(columnIndex, FieldFormat.DROPDOWNLIST);
          List<string> conditionTypeList = this.session.ConfigurationManager.GetEnhancedConditionTypeList(false);
          this.cbConditionType.Items.Add((object) "");
          foreach (object obj in conditionTypeList)
            this.cbConditionType.Items.Add(obj);
          newColumn.Tag = (object) "EnhancedConditionSet.ConditionType";
        }
      }
      this.gvAvailable.Sort(0, SortOrder.Ascending);
      this.gvSelected.Sort(0, SortOrder.Ascending);
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      ArrayList arrayList = new ArrayList();
      foreach (GVItem selectedItem in this.gvAvailable.SelectedItems)
        arrayList.Add((object) selectedItem);
      foreach (GVItem gvItem in arrayList)
      {
        this.gvAvailable.Items.Remove(gvItem);
        gvItem.Selected = false;
        this.gvSelected.Items.Add(gvItem);
        gvItem.Selected = true;
      }
      this.gvSelected.ReSort();
      this.gvSelected.Focus();
    }

    private void btnRemove_Click(object sender, EventArgs e)
    {
      ArrayList arrayList = new ArrayList();
      foreach (GVItem selectedItem in this.gvSelected.SelectedItems)
        arrayList.Add((object) selectedItem);
      foreach (GVItem gvItem in arrayList)
      {
        this.gvSelected.Items.Remove(gvItem);
        gvItem.Selected = false;
        this.gvAvailable.Items.Add(gvItem);
        gvItem.Selected = true;
      }
      this.gvAvailable.ReSort();
      this.gvAvailable.Focus();
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      EnhancedConditionSet conditionset = new EnhancedConditionSet();
      conditionset.Id = this.setid;
      string str1 = this.txtSetName.Text.Trim();
      if (str1 == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must enter a name for this condition set.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtSetName.Focus();
      }
      else if (str1.Length > 200)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The condition set name entered is too long. The maximum length is 200 characters long.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtSetName.Focus();
      }
      else
      {
        conditionset.SetName = str1;
        string str2 = this.txtDesc.Text.Trim();
        if (str2.Length > 500)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The description entered is too long. The maximum length is 500 characters long.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.txtDesc.Focus();
        }
        else
        {
          conditionset.Description = str2;
          List<EnhancedConditionTemplate> conditionTemplateList = new List<EnhancedConditionTemplate>();
          foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvSelected.Items)
            conditionTemplateList.Add(new EnhancedConditionTemplate()
            {
              Id = new Guid?((Guid) gvItem.Tag)
            });
          conditionset.ConditionTemplates = conditionTemplateList;
          if (this.isNew)
            conditionset.CreatedBy = this.session.UserID;
          conditionset.LastModifiedBy = this.session.UserID;
          if (this.session.ConfigurationManager.IsUniqueSetName(conditionset.SetName, conditionset.Id))
            this.session.ConfigurationManager.UpdateEnhancedConditionSet(conditionset);
          else if (Utils.Dialog((IWin32Window) Form.ActiveForm, "The condition set you entered has been used, please enter an unique name.", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            return;
          this.SetName = str1;
          this.DialogResult = DialogResult.OK;
        }
      }
    }

    private void btnCancel_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

    private void onFilterChanged(object sender, EventArgs e)
    {
      this.refreshConditionList();
      if (this.FilterChanged != null)
        this.FilterChanged((object) this, e);
      this.filterManager.ApplyFilter();
    }

    private void refreshConditionList() => this.filterManager.ToFieldFilterList();

    private void groupBox1_Resize(object sender, EventArgs e)
    {
      this.pnlAllConds.Size = new Size((this.groupBox1.Width - this.pnlAddRemove.Width) / 2, this.groupBox1.Height);
    }

    private void keypress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        return;
      char keyChar = e.KeyChar;
      if (!keyChar.Equals('\\'))
      {
        keyChar = e.KeyChar;
        if (!keyChar.Equals('"'))
          return;
      }
      e.Handled = true;
    }

    private void Help_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      this.ShowHelp();
    }

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, "Setup\\Enhanced Condition Sets");
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      this.txtDesc = new TextBox();
      this.label2 = new Label();
      this.label1 = new Label();
      this.gvAvailable = new GridView();
      this.gvSelected = new GridView();
      this.label3 = new Label();
      this.label4 = new Label();
      this.btnAdd = new Button();
      this.btnRemove = new Button();
      this.pnlSelected = new Panel();
      this.pnlAllConds = new Panel();
      this.pnlAddRemove = new Panel();
      this.btnSave = new Button();
      this.btnCancel = new Button();
      this.txtSetName = new TextBox();
      this.emHelpLink1 = new EMHelpLink();
      this.groupBox1 = new GroupBox();
      this.groupBox1.SuspendLayout();
      this.pnlSelected.SuspendLayout();
      this.pnlAddRemove.SuspendLayout();
      this.pnlAllConds.SuspendLayout();
      this.SuspendLayout();
      this.groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupBox1.Controls.Add((Control) this.pnlSelected);
      this.groupBox1.Controls.Add((Control) this.pnlAddRemove);
      this.groupBox1.Controls.Add((Control) this.pnlAllConds);
      this.groupBox1.Location = new Point(12, 112);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(568, 388);
      this.groupBox1.TabIndex = 12;
      this.groupBox1.TabStop = false;
      this.groupBox1.Resize += new EventHandler(this.groupBox1_Resize);
      this.gvSelected.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Condition Name";
      gvColumn1.Width = 105;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Condition Type";
      gvColumn2.Width = 105;
      this.gvSelected.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvSelected.FilterVisible = true;
      this.gvSelected.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvSelected.Location = new Point(6, 34);
      this.gvSelected.Name = "gvSelected";
      this.gvSelected.Size = new Size(220, 326);
      this.gvSelected.TabIndex = 6;
      this.label4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label4.Location = new Point(3, 10);
      this.label4.Name = "label4";
      this.label4.Size = new Size(212, 23);
      this.label4.TabIndex = 4;
      this.label4.Text = "Selected Conditions";
      this.label4.TextAlign = ContentAlignment.MiddleLeft;
      this.btnRemove.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.btnRemove.Location = new Point(5, 189);
      this.btnRemove.Name = "btnRemove";
      this.btnRemove.Size = new Size(72, 23);
      this.btnRemove.TabIndex = 5;
      this.btnRemove.Text = "<- Remove";
      this.btnRemove.Click += new EventHandler(this.btnRemove_Click);
      this.btnAdd.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.btnAdd.Location = new Point(5, 160);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(72, 23);
      this.btnAdd.TabIndex = 4;
      this.btnAdd.Text = "Add ->";
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.gvAvailable.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gvAvailable.FilterVisible = true;
      this.gvAvailable.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvAvailable.Location = new Point(15, 34);
      this.gvAvailable.Name = "gvAvailable";
      this.gvAvailable.Size = new Size(220, 326);
      this.gvAvailable.TabIndex = 3;
      this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(19, 10);
      this.label3.Name = "label3";
      this.label3.Size = new Size(204, 23);
      this.label3.TabIndex = 3;
      this.label3.Text = "All Conditions";
      this.label2.TextAlign = ContentAlignment.MiddleLeft;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(9, 38);
      this.label2.Name = "label2";
      this.label2.Size = new Size(60, 13);
      this.label2.TabIndex = 1;
      this.label2.Text = "Description";
      this.txtDesc.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDesc.Location = new Point(120, 38);
      this.txtDesc.Multiline = true;
      this.txtDesc.Name = "txtDesc";
      this.txtDesc.ScrollBars = ScrollBars.Both;
      this.txtDesc.Size = new Size(460, 74);
      this.txtDesc.TabIndex = 2;
      this.txtSetName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtSetName.Location = new Point(120, 12);
      this.txtSetName.MaxLength = 200;
      this.txtSetName.Name = "txtSetName";
      this.txtSetName.Size = new Size(460, 20);
      this.txtSetName.TabIndex = 1;
      this.txtSetName.KeyPress += new KeyPressEventHandler(this.keypress);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(9, 15);
      this.label1.Name = "label1";
      this.label1.Size = new Size(82, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Condition Set Name";
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.Location = new Point(504, 512);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 24);
      this.btnCancel.TabIndex = 8;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSave.Location = new Point(424, 512);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 24);
      this.btnSave.TabIndex = 7;
      this.btnSave.Text = "Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = "Setup\\Enhanced Condition Sets";
      this.emHelpLink1.Location = new Point(12, 516);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 25;
      this.pnlAllConds.Controls.Add((Control) this.label3);
      this.pnlAllConds.Controls.Add((Control) this.gvAvailable);
      this.pnlAllConds.Dock = DockStyle.Left;
      this.pnlAllConds.Location = new Point(3, 16);
      this.pnlAllConds.Name = "pnlAllConds";
      this.pnlAllConds.Size = new Size(241, 369);
      this.pnlAllConds.TabIndex = 20;
      this.pnlAddRemove.Controls.Add((Control) this.btnAdd);
      this.pnlAddRemove.Controls.Add((Control) this.btnRemove);
      this.pnlAddRemove.Dock = DockStyle.Left;
      this.pnlAddRemove.Location = new Point(244, 16);
      this.pnlAddRemove.Name = "pnlAddRemove";
      this.pnlAddRemove.Size = new Size(82, 369);
      this.pnlAddRemove.TabIndex = 21;
      this.pnlSelected.Controls.Add((Control) this.label4);
      this.pnlSelected.Controls.Add((Control) this.gvSelected);
      this.pnlSelected.Dock = DockStyle.Fill;
      this.pnlSelected.Location = new Point(326, 16);
      this.pnlSelected.Name = "pnlSelected";
      this.pnlSelected.Size = new Size(239, 369);
      this.pnlSelected.TabIndex = 22;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(596, 549);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.txtDesc);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.txtSetName);
      this.Controls.Add((Control) this.btnSave);
      this.Controls.Add((Control) this.emHelpLink1);
      this.KeyPreview = true;
      this.MinimizeBox = false;
      this.Name = nameof (EditEnhancedConditionSet);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Create / Edit Enhanced Condition Sets";
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      this.groupBox1.ResumeLayout(false);
      this.pnlAllConds.ResumeLayout(false);
      this.pnlAddRemove.ResumeLayout(false);
      this.pnlSelected.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
