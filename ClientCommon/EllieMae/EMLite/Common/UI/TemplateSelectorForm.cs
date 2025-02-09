// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.TemplateSelectorForm
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public abstract class TemplateSelectorForm : Form
  {
    private FileSystemEntry selectedItem;
    private IContainer components;
    private GridView gvTemplates;
    private Button btnCancel;
    private Button btnOK;
    private CheckBox chkAppend;

    public TemplateSelectorForm() => this.InitializeComponent();

    public FileSystemEntry SelectedItem => this.selectedItem;

    public bool AppendTemplate
    {
      get => this.chkAppend.Checked;
      set => this.chkAppend.Checked = value;
    }

    public bool DisplayAppendCheckbox
    {
      get => this.chkAppend.Visible;
      set
      {
        this.chkAppend.Visible = value;
        if (value)
          return;
        this.chkAppend.Checked = false;
      }
    }

    protected abstract EllieMae.EMLite.ClientServer.TemplateSettingsType TemplateType { get; }

    protected virtual void ConfigureTemplateGridView(GridView listView)
    {
    }

    protected virtual void UpdateTemplateProperties(FileSystemEntry fsEntry)
    {
    }

    protected virtual FileSystemEntry[] GetTemplateEntries()
    {
      return Session.ConfigurationManager.GetAllPublicTemplateSettingsFileEntries(this.TemplateType, true);
    }

    private void refreshTemplateList()
    {
      FileSystemEntry[] templateEntries = this.GetTemplateEntries();
      this.gvTemplates.Items.Clear();
      foreach (FileSystemEntry e in templateEntries)
        this.gvTemplates.Items.Add(this.createGVItem(e));
    }

    private GVItem createGVItem(FileSystemEntry e)
    {
      this.UpdateTemplateProperties(e);
      GVItem gvItem = new GVItem(e.Name);
      foreach (GVColumn column in this.gvTemplates.Columns)
      {
        if (column.Index > 0 && column.Tag != null)
          gvItem.SubItems[column.Index].Text = string.Concat(e.Properties[(object) string.Concat(column.Tag)]);
      }
      gvItem.Tag = (object) e;
      return gvItem;
    }

    private void SimpleTemplateSelector_Load(object sender, EventArgs e)
    {
      this.ConfigureTemplateGridView(this.gvTemplates);
      if (this.DesignMode)
        return;
      this.refreshTemplateList();
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.gvTemplates.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must first select a template from the list.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        this.selectedItem = (FileSystemEntry) this.gvTemplates.SelectedItems[0].Tag;
        this.DialogResult = DialogResult.OK;
      }
    }

    private void gvTemplates_DoubleClick(object sender, EventArgs e)
    {
      if (this.chkAppend.Visible || this.gvTemplates.HitTest(this.gvTemplates.PointToClient(Cursor.Position)).RowIndex < 0)
        return;
      this.selectedItem = (FileSystemEntry) this.gvTemplates.SelectedItems[0].Tag;
      this.DialogResult = DialogResult.OK;
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
      this.gvTemplates = new GridView();
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.chkAppend = new CheckBox();
      this.SuspendLayout();
      this.gvTemplates.AllowMultiselect = false;
      this.gvTemplates.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      gvColumn1.DisplayIndex = 0;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Name";
      gvColumn1.Width = 164;
      gvColumn2.DisplayIndex = 1;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.SpringToFit = true;
      gvColumn2.Tag = (object) "Description";
      gvColumn2.Text = "Description";
      gvColumn2.Width = 274;
      this.gvTemplates.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvTemplates.Location = new Point(8, 9);
      this.gvTemplates.Name = "gvTemplates";
      this.gvTemplates.Size = new Size(440, 362);
      this.gvTemplates.TabIndex = 0;
      this.gvTemplates.DoubleClick += new EventHandler(this.gvTemplates_DoubleClick);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(370, 387);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(287, 387);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 22);
      this.btnOK.TabIndex = 2;
      this.btnOK.Text = "&Select";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.chkAppend.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.chkAppend.AutoSize = true;
      this.chkAppend.Location = new Point(8, 375);
      this.chkAppend.Name = "chkAppend";
      this.chkAppend.Size = new Size(183, 18);
      this.chkAppend.TabIndex = 3;
      this.chkAppend.Text = "Append template to existing data";
      this.chkAppend.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(457, 425);
      this.Controls.Add((Control) this.chkAppend);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.gvTemplates);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (TemplateSelectorForm);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Select Template";
      this.Load += new EventHandler(this.SimpleTemplateSelector_Load);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
