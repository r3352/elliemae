// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.LoanProgramAdditionalFieldsControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class LoanProgramAdditionalFieldsControl : SettingsUserControl
  {
    private Sessions.Session session;
    private FieldSettings fieldSettings;
    private IContainer components;
    private GroupContainer grpFields;
    private FlowLayoutPanel flowLayoutPanel1;
    private Button button1;
    private VerticalSeparator verticalSeparator1;
    private StandardIconButton btnMoveUp;
    private StandardIconButton btnMoveDown;
    private StandardIconButton btnRemoveFields;
    private StandardIconButton btnFindFields;
    private StandardIconButton btnAddFields;
    private ToolTip toolTip1;
    private GridView gvFields;

    public LoanProgramAdditionalFieldsControl(
      Sessions.Session session,
      SetUpContainer setupContainer)
      : base(setupContainer)
    {
      this.session = session;
      this.InitializeComponent();
      this.Reset();
    }

    public override void Reset()
    {
      this.fieldSettings = this.session.LoanManager.GetFieldSettings();
      foreach (string programAdditionalField in this.session.ConfigurationManager.GetLoanProgramAdditionalFields())
      {
        GVItem gvItemForField = this.createGVItemForField(programAdditionalField);
        if (gvItemForField != null)
          this.gvFields.Items.Add(gvItemForField);
      }
      base.Reset();
    }

    private GVItem createGVItemForField(string fieldId)
    {
      FieldDefinition field = EncompassFields.GetField(fieldId, this.fieldSettings);
      if (field == null)
        return (GVItem) null;
      return new GVItem()
      {
        SubItems = {
          [0] = {
            Text = field.FieldID
          },
          [1] = {
            Text = field.Description
          }
        },
        Tag = (object) field
      };
    }

    public override void Save()
    {
      this.session.ConfigurationManager.SetLoanProgramAdditionalFields(this.getAdditionalFields());
      base.Save();
    }

    private string[] getAdditionalFields()
    {
      List<string> stringList = new List<string>();
      for (int nItemIndex = 0; nItemIndex < this.gvFields.Items.Count; ++nItemIndex)
        stringList.Add(((FieldDefinition) this.gvFields.Items[nItemIndex].Tag).FieldID);
      return stringList.ToArray();
    }

    private void btnAddFields_Click(object sender, EventArgs e)
    {
      using (AddFields addFields = new AddFields(this.session, "Add Fields", AddFieldOptions.AllowCustomFields | AddFieldOptions.AllowHiddenFields))
      {
        addFields.OnAddMoreButtonClick += new EventHandler(this.onAddMoreFields);
        if (addFields.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.addFields(addFields.SelectedFieldIDs);
      }
    }

    private void onAddMoreFields(object sender, EventArgs e)
    {
      this.addFields(((AddFields) sender).SelectedFieldIDs);
    }

    private void addFields(string[] fieldIds)
    {
      foreach (string fieldId in fieldIds)
      {
        if (!this.isFieldInList(fieldId))
        {
          GVItem gvItemForField = this.createGVItemForField(fieldId);
          if (gvItemForField != null)
            this.gvFields.Items.Add(gvItemForField);
        }
      }
      this.setDirtyFlag(true);
    }

    private bool isFieldInList(string fieldId)
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvFields.Items)
      {
        if (string.Compare((gvItem.Tag as FieldDefinition).FieldID, fieldId, true) == 0)
          return true;
      }
      return false;
    }

    private void btnFindFields_Click(object sender, EventArgs e)
    {
      using (BusinessRuleFindFieldDialog ruleFindFieldDialog = new BusinessRuleFindFieldDialog(this.session, this.getAdditionalFields(), true, "", false, false))
      {
        if (ruleFindFieldDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.addFields(ruleFindFieldDialog.SelectedRequiredFields);
      }
    }

    private void btnRemoveFields_Click(object sender, EventArgs e)
    {
      int count = this.gvFields.SelectedItems.Count;
      if (count == 0 || Utils.Dialog((IWin32Window) this, "Are you sure you want to remove the " + (object) count + " selected field(s)?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
        return;
      while (this.gvFields.SelectedItems.Count > 0)
        this.gvFields.Items.Remove(this.gvFields.SelectedItems[0]);
    }

    private void btnMoveDown_Click(object sender, EventArgs e)
    {
      for (int nItemIndex = this.gvFields.Items.Count - 2; nItemIndex >= 0; --nItemIndex)
      {
        if (this.gvFields.Items[nItemIndex].Selected && !this.gvFields.Items[nItemIndex + 1].Selected)
        {
          GVItem gvItem = this.gvFields.Items[nItemIndex];
          this.gvFields.Items.RemoveAt(nItemIndex);
          this.gvFields.Items.Insert(nItemIndex + 1, gvItem);
          gvItem.Selected = true;
          this.setDirtyFlag(true);
        }
      }
    }

    private void btnMoveUp_Click(object sender, EventArgs e)
    {
      for (int nItemIndex = 1; nItemIndex < this.gvFields.Items.Count; ++nItemIndex)
      {
        if (this.gvFields.Items[nItemIndex].Selected && !this.gvFields.Items[nItemIndex - 1].Selected)
        {
          GVItem gvItem = this.gvFields.Items[nItemIndex];
          this.gvFields.Items.RemoveAt(nItemIndex);
          this.gvFields.Items.Insert(nItemIndex - 1, gvItem);
          gvItem.Selected = true;
          this.setDirtyFlag(true);
        }
      }
    }

    private void gvFields_SelectedIndexChanged(object sender, EventArgs e)
    {
      bool flag1 = false;
      bool flag2 = false;
      if (this.gvFields.Items.Count > 0)
      {
        flag1 = this.gvFields.Items[0].Selected;
        flag2 = this.gvFields.Items[this.gvFields.Items.Count - 1].Selected;
      }
      this.btnRemoveFields.Enabled = this.gvFields.SelectedItems.Count > 0;
      this.btnMoveDown.Enabled = this.gvFields.SelectedItems.Count > 0 && !flag2;
      this.btnMoveUp.Enabled = this.gvFields.SelectedItems.Count > 0 && !flag1;
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
      this.grpFields = new GroupContainer();
      this.gvFields = new GridView();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.button1 = new Button();
      this.verticalSeparator1 = new VerticalSeparator();
      this.btnMoveUp = new StandardIconButton();
      this.btnMoveDown = new StandardIconButton();
      this.btnRemoveFields = new StandardIconButton();
      this.btnFindFields = new StandardIconButton();
      this.btnAddFields = new StandardIconButton();
      this.toolTip1 = new ToolTip(this.components);
      this.grpFields.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      ((ISupportInitialize) this.btnMoveUp).BeginInit();
      ((ISupportInitialize) this.btnMoveDown).BeginInit();
      ((ISupportInitialize) this.btnRemoveFields).BeginInit();
      ((ISupportInitialize) this.btnFindFields).BeginInit();
      ((ISupportInitialize) this.btnAddFields).BeginInit();
      this.SuspendLayout();
      this.grpFields.Controls.Add((Control) this.gvFields);
      this.grpFields.Controls.Add((Control) this.flowLayoutPanel1);
      this.grpFields.Dock = DockStyle.Fill;
      this.grpFields.HeaderForeColor = SystemColors.ControlText;
      this.grpFields.Location = new Point(0, 0);
      this.grpFields.Name = "grpFields";
      this.grpFields.Size = new Size(654, 486);
      this.grpFields.TabIndex = 0;
      this.grpFields.Text = "Additional Fields";
      this.gvFields.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Field ID";
      gvColumn1.Width = 150;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Description";
      gvColumn2.Width = 400;
      this.gvFields.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvFields.Dock = DockStyle.Fill;
      this.gvFields.Location = new Point(1, 26);
      this.gvFields.Name = "gvFields";
      this.gvFields.Size = new Size(652, 459);
      this.gvFields.SortOption = GVSortOption.None;
      this.gvFields.TabIndex = 1;
      this.gvFields.SelectedIndexChanged += new EventHandler(this.gvFields_SelectedIndexChanged);
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.button1);
      this.flowLayoutPanel1.Controls.Add((Control) this.verticalSeparator1);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnMoveUp);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnMoveDown);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnRemoveFields);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnFindFields);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnAddFields);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(458, 2);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(190, 22);
      this.flowLayoutPanel1.TabIndex = 0;
      this.button1.Location = new Point(115, 0);
      this.button1.Margin = new Padding(0);
      this.button1.Name = "button1";
      this.button1.Size = new Size(75, 22);
      this.button1.TabIndex = 0;
      this.button1.Text = "Preview";
      this.button1.UseVisualStyleBackColor = true;
      this.verticalSeparator1.Location = new Point(110, 3);
      this.verticalSeparator1.Margin = new Padding(4, 3, 3, 3);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 1;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.btnMoveUp.BackColor = Color.Transparent;
      this.btnMoveUp.Location = new Point(90, 3);
      this.btnMoveUp.Margin = new Padding(5, 3, 0, 3);
      this.btnMoveUp.MouseDownImage = (Image) null;
      this.btnMoveUp.Name = "btnMoveUp";
      this.btnMoveUp.Size = new Size(16, 16);
      this.btnMoveUp.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.btnMoveUp.TabIndex = 2;
      this.btnMoveUp.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnMoveUp, "Move Fields Up");
      this.btnMoveUp.Click += new EventHandler(this.btnMoveUp_Click);
      this.btnMoveDown.BackColor = Color.Transparent;
      this.btnMoveDown.Location = new Point(69, 3);
      this.btnMoveDown.Margin = new Padding(5, 3, 0, 3);
      this.btnMoveDown.MouseDownImage = (Image) null;
      this.btnMoveDown.Name = "btnMoveDown";
      this.btnMoveDown.Size = new Size(16, 16);
      this.btnMoveDown.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.btnMoveDown.TabIndex = 3;
      this.btnMoveDown.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnMoveDown, "Move Fields Down");
      this.btnMoveDown.Click += new EventHandler(this.btnMoveDown_Click);
      this.btnRemoveFields.BackColor = Color.Transparent;
      this.btnRemoveFields.Location = new Point(48, 3);
      this.btnRemoveFields.Margin = new Padding(5, 3, 0, 3);
      this.btnRemoveFields.MouseDownImage = (Image) null;
      this.btnRemoveFields.Name = "btnRemoveFields";
      this.btnRemoveFields.Size = new Size(16, 16);
      this.btnRemoveFields.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnRemoveFields.TabIndex = 4;
      this.btnRemoveFields.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnRemoveFields, "Remove Fields");
      this.btnRemoveFields.Click += new EventHandler(this.btnRemoveFields_Click);
      this.btnFindFields.BackColor = Color.Transparent;
      this.btnFindFields.Location = new Point(27, 3);
      this.btnFindFields.Margin = new Padding(5, 3, 0, 3);
      this.btnFindFields.MouseDownImage = (Image) null;
      this.btnFindFields.Name = "btnFindFields";
      this.btnFindFields.Size = new Size(16, 16);
      this.btnFindFields.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnFindFields.TabIndex = 5;
      this.btnFindFields.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnFindFields, "Find Fields");
      this.btnFindFields.Click += new EventHandler(this.btnFindFields_Click);
      this.btnAddFields.BackColor = Color.Transparent;
      this.btnAddFields.Location = new Point(6, 3);
      this.btnAddFields.Margin = new Padding(5, 3, 0, 3);
      this.btnAddFields.MouseDownImage = (Image) null;
      this.btnAddFields.Name = "btnAddFields";
      this.btnAddFields.Size = new Size(16, 16);
      this.btnAddFields.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddFields.TabIndex = 6;
      this.btnAddFields.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnAddFields, "Add Fields");
      this.btnAddFields.Click += new EventHandler(this.btnAddFields_Click);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.grpFields);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (LoanProgramAdditionalFieldsControl);
      this.Size = new Size(654, 486);
      this.grpFields.ResumeLayout(false);
      this.flowLayoutPanel1.ResumeLayout(false);
      ((ISupportInitialize) this.btnMoveUp).EndInit();
      ((ISupportInitialize) this.btnMoveDown).EndInit();
      ((ISupportInitialize) this.btnRemoveFields).EndInit();
      ((ISupportInitialize) this.btnFindFields).EndInit();
      ((ISupportInitialize) this.btnAddFields).EndInit();
      this.ResumeLayout(false);
    }
  }
}
