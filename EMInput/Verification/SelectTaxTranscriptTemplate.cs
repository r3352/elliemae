// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Verification.SelectTaxTranscriptTemplate
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Verification
{
  public class SelectTaxTranscriptTemplate : Form
  {
    private IRS4506TTemplate selectedTemplate;
    private IContainer components;
    private GridView templateGrid;
    private Button SaveBtn;
    private Button button2;

    public SelectTaxTranscriptTemplate()
    {
      this.InitializeComponent();
      this.populateGrid();
    }

    public IRS4506TFields SelectedTemplate => this.selectedTemplate.GetTemplateData();

    private void SaveBtn_Click(object sender, EventArgs e)
    {
      IRS4506TTemplate tag = (IRS4506TTemplate) this.templateGrid.SelectedItems[0].Tag;
      this.selectedTemplate = Session.ConfigurationManager.GetIRS4506TTemplate(tag.TemplateID);
      if (this.selectedTemplate == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The template '" + tag.TemplateName + "' is currently unavailable for use. Please check the template settings to ensure that it is still available.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
        this.DialogResult = DialogResult.OK;
    }

    private void listView_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.SaveBtn.Enabled = this.templateGrid.SelectedItems.Count > 0;
    }

    private void populateGrid()
    {
      foreach (IRS4506TTemplate irS4506Ttemplate in Session.ConfigurationManager.GetIRS4506TTemplates(true))
        this.templateGrid.Items.Add(new GVItem()
        {
          SubItems = {
            (object) irS4506Ttemplate.RequestVersion,
            (object) irS4506Ttemplate.TemplateName,
            (object) irS4506Ttemplate.RequestYears
          },
          Tag = (object) irS4506Ttemplate
        });
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
      GVColumn gvColumn3 = new GVColumn();
      this.templateGrid = new GridView();
      this.SaveBtn = new Button();
      this.button2 = new Button();
      this.SuspendLayout();
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Request Form and Version";
      gvColumn1.Text = "Request Form and Version";
      gvColumn1.Width = 150;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Template Name";
      gvColumn2.Text = "Template Name";
      gvColumn2.Width = 400;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "YearsRequested";
      gvColumn3.SpringToFit = true;
      gvColumn3.Text = "Years Requested";
      gvColumn3.Width = 111;
      this.templateGrid.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.templateGrid.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.templateGrid.Location = new Point(14, 12);
      this.templateGrid.Name = "templateGrid";
      this.templateGrid.Size = new Size(663, 354);
      this.templateGrid.TabIndex = 1;
      this.templateGrid.SelectedIndexChanged += new EventHandler(this.listView_SelectedIndexChanged);
      this.SaveBtn.Enabled = false;
      this.SaveBtn.Location = new Point(511, 373);
      this.SaveBtn.Name = "SaveBtn";
      this.SaveBtn.Size = new Size(80, 23);
      this.SaveBtn.TabIndex = 2;
      this.SaveBtn.Text = "Select";
      this.SaveBtn.UseVisualStyleBackColor = true;
      this.SaveBtn.Click += new EventHandler(this.SaveBtn_Click);
      this.button2.DialogResult = DialogResult.Cancel;
      this.button2.Location = new Point(597, 372);
      this.button2.Name = "button2";
      this.button2.Size = new Size(80, 23);
      this.button2.TabIndex = 3;
      this.button2.Text = "Cancel";
      this.button2.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(688, 405);
      this.Controls.Add((Control) this.button2);
      this.Controls.Add((Control) this.SaveBtn);
      this.Controls.Add((Control) this.templateGrid);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SelectTaxTranscriptTemplate);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Select Request for Transcript of Tax Template";
      this.ResumeLayout(false);
    }
  }
}
