// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.MSWorksheet.ArchiveTemplatesList
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Reporting;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.MSWorksheet
{
  public class ArchiveTemplatesList : Form
  {
    private List<string> templateNames = new List<string>();
    private Sessions.Session session;
    private IContainer components;
    private PictureBox pictureBox1;
    private Label label1;
    private GroupContainer grpTemplateLists;
    private GridView gvTemplates;
    private Button btnContinue;
    private Button btnCancel;
    private StandardIconButton btnExport;

    public ArchiveTemplatesList() => this.InitializeComponent();

    public ArchiveTemplatesList(Sessions.Session session, string Names)
    {
      this.InitializeComponent();
      this.session = session;
      ((IEnumerable<string>) Names.Split(',')).ToList<string>().ForEach((Action<string>) (item =>
      {
        if (item.Equals(""))
          return;
        this.templateNames.Add(item);
      }));
      this.templateNames.ForEach((Action<string>) (item => this.gvTemplates.Items.Add(this.createGVItemFortemplates(item.ToString()))));
      this.grpTemplateLists.Text = "Affected Milestone Templates(" + (object) this.templateNames.Count + ")";
    }

    private GVItem createGVItemFortemplates(string Name)
    {
      return new GVItem() { Value = (object) Name };
    }

    private void btnContinue_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.OK;
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
    }

    private void btnExport_Click(object sender, EventArgs e)
    {
      using (CursorActivator.Wait())
      {
        ExcelHandler excelHandler = new ExcelHandler();
        excelHandler.AddHeaderColumn(this.grpTemplateLists.Text);
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvTemplates.Items)
        {
          string[] data = new string[this.gvTemplates.Columns.Count];
          for (int index1 = 0; index1 < data.Length; ++index1)
          {
            int index2 = this.gvTemplates.Columns.DisplaySequence[index1].Index;
            data[index1] = gvItem.SubItems[index2].Text;
          }
          excelHandler.AddDataRow(data);
        }
        excelHandler.CreateExcel();
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ArchiveTemplatesList));
      GVColumn gvColumn = new GVColumn();
      this.pictureBox1 = new PictureBox();
      this.label1 = new Label();
      this.grpTemplateLists = new GroupContainer();
      this.gvTemplates = new GridView();
      this.btnContinue = new Button();
      this.btnCancel = new Button();
      this.btnExport = new StandardIconButton();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.grpTemplateLists.SuspendLayout();
      ((ISupportInitialize) this.btnExport).BeginInit();
      this.SuspendLayout();
      this.pictureBox1.Image = (Image) componentResourceManager.GetObject("pictureBox1.Image");
      this.pictureBox1.Location = new Point(21, 12);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(32, 32);
      this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox1.TabIndex = 8;
      this.pictureBox1.TabStop = false;
      this.label1.Location = new Point(71, 12);
      this.label1.Name = "label1";
      this.label1.Size = new Size(376, 42);
      this.label1.TabIndex = 9;
      this.label1.Text = "The following milestone templates are associated with the selected milestone. If the milestone is archived, they will be removed from the templates.";
      this.grpTemplateLists.AutoScroll = true;
      this.grpTemplateLists.AutoSize = true;
      this.grpTemplateLists.Controls.Add((Control) this.btnExport);
      this.grpTemplateLists.Controls.Add((Control) this.gvTemplates);
      this.grpTemplateLists.HeaderForeColor = SystemColors.ControlText;
      this.grpTemplateLists.Location = new Point(5, 55);
      this.grpTemplateLists.Name = "grpTemplateLists";
      this.grpTemplateLists.Size = new Size(467, 220);
      this.grpTemplateLists.TabIndex = 10;
      this.grpTemplateLists.Text = "Affected Milestone Templates";
      this.gvTemplates.BorderStyle = BorderStyle.None;
      gvColumn.ImageIndex = -1;
      gvColumn.Name = "Column1";
      gvColumn.Text = "";
      gvColumn.Width = 465;
      this.gvTemplates.Columns.AddRange(new GVColumn[1]
      {
        gvColumn
      });
      this.gvTemplates.Dock = DockStyle.Fill;
      this.gvTemplates.HeaderHeight = 0;
      this.gvTemplates.Location = new Point(1, 26);
      this.gvTemplates.Name = "gvTemplates";
      this.gvTemplates.Size = new Size(465, 193);
      this.gvTemplates.TabIndex = 2;
      this.btnContinue.Location = new Point(311, 291);
      this.btnContinue.Name = "btnContinue";
      this.btnContinue.Size = new Size(75, 23);
      this.btnContinue.TabIndex = 11;
      this.btnContinue.Text = "Continue";
      this.btnContinue.UseVisualStyleBackColor = true;
      this.btnContinue.Click += new EventHandler(this.btnContinue_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(392, 291);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 12;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.btnExport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExport.BackColor = Color.Transparent;
      this.btnExport.Location = new Point(444, 4);
      this.btnExport.MouseDownImage = (Image) null;
      this.btnExport.Name = "btnExport";
      this.btnExport.Size = new Size(16, 16);
      this.btnExport.StandardButtonType = StandardIconButton.ButtonType.ExcelButton;
      this.btnExport.TabIndex = 17;
      this.btnExport.TabStop = false;
      this.btnExport.Click += new EventHandler(this.btnExport_Click);
      this.AcceptButton = (IButtonControl) this.btnContinue;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(477, 323);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnContinue);
      this.Controls.Add((Control) this.grpTemplateLists);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.pictureBox1);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (ArchiveTemplatesList);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Archive Milestone";
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.grpTemplateLists.ResumeLayout(false);
      ((ISupportInitialize) this.btnExport).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
