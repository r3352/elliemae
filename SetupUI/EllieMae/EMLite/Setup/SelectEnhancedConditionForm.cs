// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.SelectEnhancedConditionForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.DataEngine.eFolder;
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
  public class SelectEnhancedConditionForm : Form
  {
    private Sessions.Session session;
    private IContainer components;
    private Button btnAdd;
    private Button btnCancel;
    private GridView gridViewConditions;
    private ComboBox cboConditionType;
    private Label label1;

    public SelectEnhancedConditionForm(Sessions.Session session)
    {
      this.session = session;
      this.InitializeComponent();
      this.populateConditionTypes();
      this.gridViewConditions_SelectedIndexChanged((object) null, (EventArgs) null);
    }

    private void populateConditionTypes()
    {
      EnhancedConditionType[] enhancedConditionTypes = EnhancedConditionRestApiHelper.GetEnhancedConditionTypes(true);
      if (enhancedConditionTypes == null)
        return;
      this.cboConditionType.DisplayMember = "title";
      this.cboConditionType.ValueMember = "id";
      this.cboConditionType.DataSource = (object) enhancedConditionTypes;
    }

    private void cboConditionType_SelectedIndexChanged(object sender, EventArgs e)
    {
      Cursor.Current = Cursors.WaitCursor;
      this.reloadConditions(EnhancedConditionRestApiHelper.GetEnhancedConditionTemplatesByConditionType(Convert.ToString(this.cboConditionType.SelectedItem), getActive: true));
      Cursor.Current = Cursors.Default;
    }

    private void reloadConditions(EnhancedConditionTemplate[] conditions)
    {
      this.gridViewConditions.BeginUpdate();
      this.gridViewConditions.Items.Clear();
      if (conditions != null)
      {
        for (int index = 0; index < conditions.Length; ++index)
          this.gridViewConditions.Items.Add(new GVItem(conditions[index].Title)
          {
            Tag = (object) conditions[index]
          });
      }
      this.gridViewConditions.EndUpdate();
    }

    private void gridViewConditions_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnAdd.Enabled = this.gridViewConditions.SelectedItems.Count > 0;
    }

    private void btnAdd_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

    public List<EnhancedConditionTemplate> GetSelectedConditions()
    {
      List<EnhancedConditionTemplate> selectedConditions = new List<EnhancedConditionTemplate>();
      for (int index = 0; index < this.gridViewConditions.SelectedItems.Count; ++index)
        selectedConditions.Add((EnhancedConditionTemplate) this.gridViewConditions.SelectedItems[index].Tag);
      return selectedConditions;
    }

    public string SelectedConditionTypeString => this.cboConditionType.Text;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      GVColumn gvColumn = new GVColumn();
      this.btnAdd = new Button();
      this.btnCancel = new Button();
      this.gridViewConditions = new GridView();
      this.cboConditionType = new ComboBox();
      this.label1 = new Label();
      this.SuspendLayout();
      this.btnAdd.Location = new Point(678, 454);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(75, 23);
      this.btnAdd.TabIndex = 0;
      this.btnAdd.Text = "&Add";
      this.btnAdd.UseVisualStyleBackColor = true;
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(759, 454);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      gvColumn.ImageIndex = -1;
      gvColumn.Name = "headerName";
      gvColumn.SpringToFit = true;
      gvColumn.Text = "Condition Name";
      gvColumn.Width = 819;
      this.gridViewConditions.Columns.AddRange(new GVColumn[1]
      {
        gvColumn
      });
      this.gridViewConditions.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridViewConditions.Location = new Point(12, 45);
      this.gridViewConditions.Name = "gridViewConditions";
      this.gridViewConditions.Size = new Size(821, 403);
      this.gridViewConditions.TabIndex = 50;
      this.gridViewConditions.SelectedIndexChanged += new EventHandler(this.gridViewConditions_SelectedIndexChanged);
      this.cboConditionType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboConditionType.FormattingEnabled = true;
      this.cboConditionType.Location = new Point(96, 16);
      this.cboConditionType.Name = "cboConditionType";
      this.cboConditionType.Size = new Size(239, 21);
      this.cboConditionType.TabIndex = 51;
      this.cboConditionType.SelectedIndexChanged += new EventHandler(this.cboConditionType_SelectedIndexChanged);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 19);
      this.label1.Name = "label1";
      this.label1.Size = new Size(78, 13);
      this.label1.TabIndex = 52;
      this.label1.Text = "Condition Type";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(845, 489);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.cboConditionType);
      this.Controls.Add((Control) this.gridViewConditions);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnAdd);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SelectEnhancedConditionForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Select Condition";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
