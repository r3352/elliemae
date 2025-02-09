// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.MSWorksheet.MSTemplateDetailForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.EMLite.Workflow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.MSWorksheet
{
  public class MSTemplateDetailForm : Form
  {
    private MilestoneTemplate mt;
    private IEnumerable<Milestone> allMilestones;
    private IContainer components;
    private GradientPanel gradientPanel1;
    private Label label1;
    private BorderPanel borderPanel1;
    private ComboBox cmbLoanNumbering;
    private Label label2;
    private Button btnOK;
    private Button btnCancel;

    public MSTemplateDetailForm(Sessions.Session session, MilestoneTemplate mt)
    {
      this.InitializeComponent();
      this.mt = mt;
      this.allMilestones = ((MilestoneTemplatesBpmManager) session.BPM.GetBpmManager(BpmCategory.Milestones)).GetAllMilestonesList();
      foreach (TemplateMilestone sequentialMilestone in this.mt.SequentialMilestones)
      {
        this.cmbLoanNumbering.Items.Add((object) this.createGVItemForTemplateMilestone(sequentialMilestone.MilestoneID));
        if (sequentialMilestone.MilestoneID == this.mt.UpdateBorrowerContactMilestoneID)
          this.cmbLoanNumbering.SelectedIndex = this.cmbLoanNumbering.Items.Count - 1;
      }
    }

    private MilestoneLabel createGVItemForTemplateMilestone(string milestoneId)
    {
      return new MilestoneLabel(this.getMilestoneByID(milestoneId));
    }

    private Milestone getMilestoneByID(string milestoneId)
    {
      return this.allMilestones.FirstOrDefault<Milestone>((Func<Milestone, bool>) (x => string.Compare(x.MilestoneID, milestoneId, true) == 0));
    }

    public TemplateMilestone SelectedMilestone
    {
      get
      {
        MilestoneLabel msLabel = (MilestoneLabel) this.cmbLoanNumbering.SelectedItem;
        return this.mt.SequentialMilestones.FirstOrDefault<TemplateMilestone>((Func<TemplateMilestone, bool>) (x => this.getMilestoneByID(x.MilestoneID).Name == msLabel.MilestoneName));
      }
    }

    private void btnOK_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MSTemplateDetailForm));
      this.gradientPanel1 = new GradientPanel();
      this.borderPanel1 = new BorderPanel();
      this.label1 = new Label();
      this.label2 = new Label();
      this.cmbLoanNumbering = new ComboBox();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.gradientPanel1.SuspendLayout();
      this.borderPanel1.SuspendLayout();
      this.SuspendLayout();
      this.gradientPanel1.Controls.Add((Control) this.label1);
      this.gradientPanel1.Dock = DockStyle.Top;
      this.gradientPanel1.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel1.Location = new Point(1, 1);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(595, 52);
      this.gradientPanel1.Style = GradientPanel.PanelStyle.LoanHeader;
      this.gradientPanel1.TabIndex = 0;
      this.borderPanel1.Controls.Add((Control) this.cmbLoanNumbering);
      this.borderPanel1.Controls.Add((Control) this.label2);
      this.borderPanel1.Controls.Add((Control) this.gradientPanel1);
      this.borderPanel1.Dock = DockStyle.Top;
      this.borderPanel1.Location = new Point(0, 0);
      this.borderPanel1.Name = "borderPanel1";
      this.borderPanel1.Size = new Size(597, 181);
      this.borderPanel1.TabIndex = 1;
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Location = new Point(21, 18);
      this.label1.Name = "label1";
      this.label1.Size = new Size(372, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Configure the milestone for each of the settings to be applied for this template.";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(22, 70);
      this.label2.Name = "label2";
      this.label2.Size = new Size(170, 13);
      this.label2.TabIndex = 1;
      this.label2.Text = "When to start auto loan numbering";
      this.cmbLoanNumbering.FormattingEnabled = true;
      this.cmbLoanNumbering.Location = new Point(198, 67);
      this.cmbLoanNumbering.Name = "cmbLoanNumbering";
      this.cmbLoanNumbering.Size = new Size(209, 21);
      this.cmbLoanNumbering.TabIndex = 2;
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(428, 189);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 2;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(509, 189);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(597, 224);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.borderPanel1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (MSTemplateDetailForm);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Template Settings";
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      this.borderPanel1.ResumeLayout(false);
      this.borderPanel1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
