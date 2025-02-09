// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.CustomMilestoneExceptionTemplate
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.UI;
using EllieMae.EMLite.Workflow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class CustomMilestoneExceptionTemplate : Form
  {
    private IContainer components;
    private Label label1;
    private GroupContainer groupContainer1;
    private GridView gvAvaiTemplates;
    private Button button1;
    private Button button2;

    public CustomMilestoneExceptionTemplate(List<MilestoneTemplate> templates)
    {
      this.InitializeComponent();
      templates.ForEach((Action<MilestoneTemplate>) (x => this.gvAvaiTemplates.Items.Add(new GVItem("")
      {
        SubItems = {
          (object) x.Name
        },
        Tag = (object) x
      })));
    }

    public List<MilestoneTemplate> SelectedTemplates()
    {
      List<MilestoneTemplate> result = new List<MilestoneTemplate>();
      ((IEnumerable<GVItem>) this.gvAvaiTemplates.GetCheckedItems(0)).ToList<GVItem>().ForEach((Action<GVItem>) (x => result.Add((MilestoneTemplate) x.Tag)));
      return result;
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
      this.label1 = new Label();
      this.groupContainer1 = new GroupContainer();
      this.gvAvaiTemplates = new GridView();
      this.button1 = new Button();
      this.button2 = new Button();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(318, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Select the templates to add to your Templates with Exceptions list.";
      this.groupContainer1.Controls.Add((Control) this.gvAvaiTemplates);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(15, 25);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(344, 264);
      this.groupContainer1.TabIndex = 1;
      this.groupContainer1.Text = "Available Templates";
      this.gvAvaiTemplates.BorderStyle = BorderStyle.None;
      gvColumn1.CheckBoxes = true;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column2";
      gvColumn1.Text = "";
      gvColumn1.Width = 50;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column1";
      gvColumn2.Text = "Template";
      gvColumn2.Width = 250;
      this.gvAvaiTemplates.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvAvaiTemplates.Dock = DockStyle.Fill;
      this.gvAvaiTemplates.Location = new Point(1, 26);
      this.gvAvaiTemplates.Name = "gvAvaiTemplates";
      this.gvAvaiTemplates.Size = new Size(342, 237);
      this.gvAvaiTemplates.TabIndex = 0;
      this.button1.DialogResult = DialogResult.OK;
      this.button1.Location = new Point(202, 295);
      this.button1.Name = "button1";
      this.button1.Size = new Size(75, 23);
      this.button1.TabIndex = 2;
      this.button1.Text = "Submit";
      this.button1.UseVisualStyleBackColor = true;
      this.button2.DialogResult = DialogResult.Cancel;
      this.button2.Location = new Point(283, 295);
      this.button2.Name = "button2";
      this.button2.Size = new Size(75, 23);
      this.button2.TabIndex = 3;
      this.button2.Text = "Cancel";
      this.button2.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(377, 334);
      this.Controls.Add((Control) this.button2);
      this.Controls.Add((Control) this.button1);
      this.Controls.Add((Control) this.groupContainer1);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (CustomMilestoneExceptionTemplate);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Template Selection";
      this.groupContainer1.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
