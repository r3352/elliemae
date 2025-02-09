// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.CustomMilestoneExceptionNotification
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.Properties;
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
namespace EllieMae.EMLite.Setup
{
  public class CustomMilestoneExceptionNotification : Form
  {
    private bool resolveIssue;
    private string defaultMilestone = "";
    private IEnumerable<Milestone> milestones;
    private IContainer components;
    private Label label1;
    private Label label2;
    private Label label3;
    private GridView gvTemplates;
    private Label label4;
    private Label label5;
    private EllieMae.EMLite.UI.LinkLabel linkLabel2;
    private Label label6;
    private Button button1;
    private Button button2;
    private PictureBox pictureBox1;
    private EMHelpLink emHelpLink1;

    public CustomMilestoneExceptionNotification(
      Dictionary<MilestoneTemplate, bool> exceptionList,
      string impactedArea,
      string defaultMilestoneID)
    {
      this.InitializeComponent();
      this.milestones = Session.SessionObjects.BpmManager.GetMilestones(true);
      this.defaultMilestone = defaultMilestoneID != string.Empty ? this.milestones.FirstOrDefault<Milestone>((Func<Milestone, bool>) (x => x.MilestoneID == defaultMilestoneID)).Name : "";
      if (impactedArea.Contains("eDisclosure"))
        this.label1.Text = this.label1.Text.Replace("Auto Loan Numbering", "eDisclosure Packages");
      this.label1.Text = this.label1.Text.Replace("System Default", this.defaultMilestone);
      foreach (KeyValuePair<MilestoneTemplate, bool> exception in exceptionList)
      {
        if (!exception.Value)
          this.gvTemplates.Items.Add(new GVItem(exception.Key.Name)
          {
            Tag = (object) exception.Key
          });
      }
      this.SetHelpTag(impactedArea);
      if (Session.EncompassEdition != EncompassEdition.Broker)
        return;
      this.label4.Visible = this.label5.Visible = this.label6.Visible = this.linkLabel2.Visible = false;
      Size clientSize = this.ClientSize;
      int width = clientSize.Width;
      clientSize = this.ClientSize;
      int height = clientSize.Height - 26;
      this.ClientSize = new Size(width, height);
    }

    public bool ResolveIssue => this.resolveIssue;

    private void linkLabel2_Click(object sender, EventArgs e)
    {
      this.resolveIssue = true;
      this.DialogResult = DialogResult.Yes;
    }

    private void SetHelpTag(string impactedArea)
    {
      if (impactedArea.Contains("AutoLoan"))
      {
        this.emHelpLink1.HelpTag = "BranchLoanNumberDialog";
      }
      else
      {
        if (!impactedArea.Contains("eDis"))
          return;
        this.emHelpLink1.HelpTag = "EDisclosuresExceptions";
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (CustomMilestoneExceptionNotification));
      GVColumn gvColumn = new GVColumn();
      this.label1 = new Label();
      this.label2 = new Label();
      this.label3 = new Label();
      this.gvTemplates = new GridView();
      this.label4 = new Label();
      this.label5 = new Label();
      this.linkLabel2 = new EllieMae.EMLite.UI.LinkLabel();
      this.label6 = new Label();
      this.button1 = new Button();
      this.button2 = new Button();
      this.pictureBox1 = new PictureBox();
      this.emHelpLink1 = new EMHelpLink();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.SuspendLayout();
      this.label1.Location = new Point(49, 23);
      this.label1.Name = "label1";
      this.label1.Size = new Size(315, 60);
      this.label1.TabIndex = 0;
      this.label1.Text = componentResourceManager.GetString("label1.Text");
      this.label2.AutoSize = true;
      this.label2.Location = new Point(49, 83);
      this.label2.Name = "label2";
      this.label2.Size = new Size(172, 13);
      this.label2.TabIndex = 1;
      this.label2.Text = "Are you sure you want to proceed?";
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(49, 114);
      this.label3.Name = "label3";
      this.label3.Size = new Size(321, 13);
      this.label3.TabIndex = 2;
      this.label3.Text = "These templates do not contain the selected milestone:";
      gvColumn.ImageIndex = -1;
      gvColumn.Name = "Column1";
      gvColumn.Text = "Templates";
      gvColumn.Width = 270;
      this.gvTemplates.Columns.AddRange(new GVColumn[1]
      {
        gvColumn
      });
      this.gvTemplates.Location = new Point(52, 130);
      this.gvTemplates.Name = "gvTemplates";
      this.gvTemplates.Size = new Size(312, 148);
      this.gvTemplates.TabIndex = 3;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(54, 305);
      this.label4.Name = "label4";
      this.label4.Size = new Size(196, 13);
      this.label4.TabIndex = 4;
      this.label4.Text = "the trigger milestone for these templates.";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(52, 292);
      this.label5.Name = "label5";
      this.label5.Size = new Size(73, 13);
      this.label5.TabIndex = 5;
      this.label5.Text = "Note: Use the";
      this.linkLabel2.AutoSize = true;
      this.linkLabel2.Location = new Point(120, 292);
      this.linkLabel2.Name = "linkLabel2";
      this.linkLabel2.Size = new Size(101, 13);
      this.linkLabel2.TabIndex = 7;
      this.linkLabel2.Text = "Manage Exceptions";
      this.linkLabel2.Click += new EventHandler(this.linkLabel2_Click);
      this.label6.AutoSize = true;
      this.label6.Location = new Point(217, 292);
      this.label6.Name = "label6";
      this.label6.Size = new Size(142, 13);
      this.label6.TabIndex = 8;
      this.label6.Text = "feature at any time to update";
      this.button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.button1.DialogResult = DialogResult.Yes;
      this.button1.Location = new Point(208, 328);
      this.button1.Name = "button1";
      this.button1.Size = new Size(75, 23);
      this.button1.TabIndex = 9;
      this.button1.Text = "Yes";
      this.button1.UseVisualStyleBackColor = true;
      this.button2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.button2.DialogResult = DialogResult.No;
      this.button2.Location = new Point(289, 328);
      this.button2.Name = "button2";
      this.button2.Size = new Size(75, 23);
      this.button2.TabIndex = 10;
      this.button2.Text = "No";
      this.button2.UseVisualStyleBackColor = true;
      this.pictureBox1.Image = (Image) Resources.Warning;
      this.pictureBox1.Location = new Point(12, 23);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(32, 34);
      this.pictureBox1.TabIndex = 11;
      this.pictureBox1.TabStop = false;
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = "CustomExceptionNotification";
      this.emHelpLink1.Location = new Point(12, 334);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 41;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(388, 362);
      this.Controls.Add((Control) this.emHelpLink1);
      this.Controls.Add((Control) this.pictureBox1);
      this.Controls.Add((Control) this.button2);
      this.Controls.Add((Control) this.button1);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.linkLabel2);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.gvTemplates);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (CustomMilestoneExceptionNotification);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Encompass";
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
