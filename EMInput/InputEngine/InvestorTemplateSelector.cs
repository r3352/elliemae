// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.InvestorTemplateSelector
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class InvestorTemplateSelector : Form
  {
    private FileSystemEntry selectedItem;
    private InvestorTemplate selectedTemplate;
    private bool bulkSaleOnly;
    private IContainer components;
    private GridView gvTemplates;
    private InvestorAddressEditor ctlEditor;
    private Button btnCancel;
    private Button btnOK;

    public InvestorTemplateSelector()
      : this(false)
    {
    }

    public InvestorTemplateSelector(bool bulkSaleOnly)
    {
      this.InitializeComponent();
      this.bulkSaleOnly = bulkSaleOnly;
      this.refreshTemplateList();
    }

    public FileSystemEntry SelectedItem => this.selectedItem;

    public InvestorTemplate SelectedTemplate => this.selectedTemplate;

    private void refreshTemplateList()
    {
      FileSystemEntry[] settingsFileEntries = Session.ConfigurationManager.GetAllPublicTemplateSettingsFileEntries(TemplateSettingsType.Investor, true);
      this.gvTemplates.Items.Clear();
      foreach (FileSystemEntry e in settingsFileEntries)
      {
        if (!this.bulkSaleOnly || e.Properties[(object) "BulkSale"].ToString() == "Yes")
          this.gvTemplates.Items.Add(this.createListViewItem(e));
      }
    }

    private GVItem createListViewItem(FileSystemEntry e)
    {
      return new GVItem(e.Name) { Tag = (object) e };
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.selectedItem == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must first select a template from the list.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
        this.DialogResult = DialogResult.OK;
    }

    private void gvTemplates_DoubleClick(object sender, GVItemEventArgs e)
    {
      if (!e.Item.Selected)
        return;
      FileSystemEntry tag = e.Item.Tag as FileSystemEntry;
      if (tag != this.selectedItem)
      {
        this.selectedItem = tag;
        this.selectedTemplate = InvestorTemplateSelector.getTemplateFromServer(this.selectedItem);
      }
      this.DialogResult = DialogResult.OK;
    }

    private void gvTemplates_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.selectedTemplate = (InvestorTemplate) null;
      this.selectedItem = (FileSystemEntry) null;
      if (this.gvTemplates.SelectedItems.Count > 0)
      {
        this.selectedItem = this.gvTemplates.SelectedItems[0].Tag as FileSystemEntry;
        this.selectedTemplate = InvestorTemplateSelector.getTemplateFromServer(this.selectedItem);
      }
      if (this.selectedTemplate != null)
        this.ctlEditor.CurrentInvestor = this.selectedTemplate.CompanyInformation;
      else
        this.ctlEditor.Clear();
    }

    private static InvestorTemplate getTemplateFromServer(FileSystemEntry e)
    {
      try
      {
        Cursor.Current = Cursors.WaitCursor;
        return (InvestorTemplate) Session.ConfigurationManager.GetTemplateSettings(TemplateSettingsType.Investor, e);
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      GVColumn gvColumn = new GVColumn();
      this.gvTemplates = new GridView();
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.ctlEditor = new InvestorAddressEditor();
      this.SuspendLayout();
      this.gvTemplates.AllowMultiselect = false;
      gvColumn.ImageIndex = -1;
      gvColumn.Name = "Column1";
      gvColumn.SpringToFit = true;
      gvColumn.Text = "Investor Name";
      gvColumn.Width = 335;
      this.gvTemplates.Columns.AddRange(new GVColumn[1]
      {
        gvColumn
      });
      this.gvTemplates.Location = new Point(10, 14);
      this.gvTemplates.Name = "gvTemplates";
      this.gvTemplates.Size = new Size(337, 154);
      this.gvTemplates.TabIndex = 0;
      this.gvTemplates.SelectedIndexChanged += new EventHandler(this.gvTemplates_SelectedIndexChanged);
      this.gvTemplates.ItemDoubleClick += new GVItemEventHandler(this.gvTemplates_DoubleClick);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(273, 443);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 21);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(195, 443);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 21);
      this.btnOK.TabIndex = 3;
      this.btnOK.Text = "&Select";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.ctlEditor.BackColor = Color.Transparent;
      this.ctlEditor.CurrentInvestor = (Investor) null;
      this.ctlEditor.Location = new Point(10, 175);
      this.ctlEditor.Name = "ctlEditor";
      this.ctlEditor.ReadOnly = true;
      this.ctlEditor.Size = new Size(339, 259);
      this.ctlEditor.TabIndex = 1;
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(357, 473);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.ctlEditor);
      this.Controls.Add((Control) this.gvTemplates);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (InvestorTemplateSelector);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Select Investor Template";
      this.ResumeLayout(false);
    }
  }
}
