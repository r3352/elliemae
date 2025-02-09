// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.MilestoneTemplateSelector
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine.Log;
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
namespace EllieMae.EMLite.InputEngine
{
  public class MilestoneTemplateSelector : Form
  {
    private IEnumerable<EllieMae.EMLite.Workflow.Milestone> allMilestones;
    private Font font;
    private bool manual;
    private MilestoneTemplate selectedTemplate;
    private const string className = "MilestoneTemplateSelector";
    private RoleInfo[] roles;
    private IContainer components;
    private Label lblTitle;
    private GroupContainer groupContainer1;
    private BorderPanel borderPanel1;
    private GridView gvMilestones;
    private GroupContainer grpTemplates;
    private GridView gvTemplates;
    private Label lblCurrentTemplate;
    private Button btnSelect;
    private Button btnCancel;
    private EMHelpLink emHelpLink1;
    private PictureBox pictureBox1;
    private Label lblTemplateName;
    private Label label1;
    private BorderPanel panel1;
    private GradientPanel gradientPanel2;
    private Splitter splitter1;
    private Label lblDesc;
    private Panel panel2;

    public MilestoneTemplateSelector(
      IEnumerable<MilestoneTemplate> milestoneTemplates,
      List<string> satisfiedTemplates,
      RoleInfo[] roles,
      IEnumerable<EllieMae.EMLite.Workflow.Milestone> allMilestones,
      bool hideMilestoneTemplate,
      string currentTemplate,
      bool manual)
    {
      this.InitializeComponent();
      this.allMilestones = allMilestones;
      this.selectedTemplate = milestoneTemplates.FirstOrDefault<MilestoneTemplate>((Func<MilestoneTemplate, bool>) (x => x.Name == satisfiedTemplates[0]));
      this.roles = roles;
      this.manual = manual;
      this.font = new Font(this.gvTemplates.Font, FontStyle.Bold);
      List<GVItem> gvItemList1 = new List<GVItem>();
      List<GVItem> gvItemList2 = new List<GVItem>();
      foreach (MilestoneTemplate milestoneTemplate in milestoneTemplates)
      {
        if (milestoneTemplate.Active)
        {
          GVItem gvItem = new GVItem(milestoneTemplate.Name);
          gvItem.Tag = (object) milestoneTemplate;
          if (milestoneTemplate.Name.Equals(currentTemplate))
            gvItem.SubItems[0].Text += " (Current)";
          if (satisfiedTemplates.Contains(milestoneTemplate.Name))
          {
            gvItemList1.Add(gvItem);
            gvItem.SubItems[0].Font = this.font;
          }
          else
            gvItemList2.Add(gvItem);
        }
      }
      this.gvTemplates.Items.AddRange(gvItemList1.ToArray());
      if (!hideMilestoneTemplate)
      {
        if (gvItemList2.Count > 0)
          this.gvTemplates.Items.Add(new GVItem("       -----Non-matching Templates-----"));
        this.gvTemplates.Items.AddRange(gvItemList2.ToArray());
      }
      this.gvTemplates.Items[0].Selected = true;
      this.lblDesc.Text = "Select a milestone template to apply to this loan file. The template that best matches the loan’s data is listed first, and so on.";
      this.lblTemplateName.Text = currentTemplate;
    }

    public MilestoneTemplate SelectedTemplate
    {
      get => this.selectedTemplate;
      set => this.selectedTemplate = value;
    }

    private void btnSelect_Click(object sender, EventArgs e)
    {
      if (this.gvTemplates.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a template", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.selectedTemplate = (MilestoneTemplate) this.gvTemplates.SelectedItems[0].Tag;
        if (this.selectedTemplate == null)
          return;
        this.DialogResult = this.selectedTemplate == null ? DialogResult.Cancel : DialogResult.OK;
      }
    }

    private string getText(KeyValuePair<UserInfo, List<string>> user)
    {
      string str1 = user.Key.FullName + " (";
      string str2;
      if (user.Value.Count > 1)
      {
        foreach (string str3 in user.Value)
          str1 = str1 + str3 + ", ";
        str2 = str1.Substring(0, str1.Length - 2);
      }
      else
        str2 = str1 + user.Value[0];
      return str2 + ")";
    }

    private void gvTemplates_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.gvTemplates.SelectedItems.Count <= 0)
        return;
      this.SelectedTemplate = (MilestoneTemplate) this.gvTemplates.SelectedItems[0].Tag;
      if (this.SelectedTemplate == null)
        return;
      this.populateTemplateMilestones(this.SelectedTemplate);
    }

    private void populateTemplateMilestones(MilestoneTemplate template)
    {
      this.gvMilestones.Items.Clear();
      this.groupContainer1.Text = template.Name + " Milestones";
      foreach (TemplateMilestone sequentialMilestone in template.SequentialMilestones)
        this.gvMilestones.Items.Add(this.createGVItemForTemplateMilestone(sequentialMilestone));
    }

    private GVItem createGVItemForTemplateMilestone(TemplateMilestone templateMs)
    {
      return this.createGVItemForMilestone(this.getMilestoneByID(templateMs.MilestoneID), templateMs.RoleID, templateMs.DaysToComplete);
    }

    private GVItem createGVItemForMilestoneLog(MilestoneLog log)
    {
      return this.createGVItemForMilestone(this.getMilestoneByID(log.MilestoneID), log.RoleID, log.Days);
    }

    private GVItem createGVItemForMilestone(EllieMae.EMLite.Workflow.Milestone ms, int RoleID, int DaysToComplete)
    {
      GVItem itemForMilestone = new GVItem();
      itemForMilestone.SubItems[0].Value = (object) new MilestoneLabel(ms);
      if (RoleID == 0)
      {
        itemForMilestone.SubItems[1].Text = this.getRoleName(ms.RoleID);
        itemForMilestone.SubItems[1].Tag = (object) ms.RoleID;
      }
      else
      {
        itemForMilestone.SubItems[1].Text = this.getRoleName(RoleID);
        itemForMilestone.SubItems[1].Tag = (object) RoleID;
      }
      itemForMilestone.SubItems[2].Value = (object) DaysToComplete;
      itemForMilestone.Tag = (object) ms;
      return itemForMilestone;
    }

    private EllieMae.EMLite.Workflow.Milestone getMilestoneByID(string milestoneId)
    {
      return this.allMilestones.FirstOrDefault<EllieMae.EMLite.Workflow.Milestone>((Func<EllieMae.EMLite.Workflow.Milestone, bool>) (x => string.Compare(x.MilestoneID, milestoneId, true) == 0));
    }

    private string getRoleName(int roleId)
    {
      RoleInfo roleInfo = ((IEnumerable<RoleInfo>) this.roles).FirstOrDefault<RoleInfo>((Func<RoleInfo, bool>) (x => x.RoleID == roleId));
      return roleInfo != null ? roleInfo.RoleName : "";
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
      GVColumn gvColumn4 = new GVColumn();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MilestoneTemplateSelector));
      this.lblTitle = new Label();
      this.groupContainer1 = new GroupContainer();
      this.borderPanel1 = new BorderPanel();
      this.gvMilestones = new GridView();
      this.grpTemplates = new GroupContainer();
      this.gvTemplates = new GridView();
      this.lblCurrentTemplate = new Label();
      this.btnSelect = new Button();
      this.btnCancel = new Button();
      this.emHelpLink1 = new EMHelpLink();
      this.lblTemplateName = new Label();
      this.label1 = new Label();
      this.pictureBox1 = new PictureBox();
      this.panel1 = new BorderPanel();
      this.splitter1 = new Splitter();
      this.gradientPanel2 = new GradientPanel();
      this.lblDesc = new Label();
      this.panel2 = new Panel();
      this.groupContainer1.SuspendLayout();
      this.borderPanel1.SuspendLayout();
      this.grpTemplates.SuspendLayout();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.panel1.SuspendLayout();
      this.gradientPanel2.SuspendLayout();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      this.lblTitle.AutoSize = true;
      this.lblTitle.Dock = DockStyle.Top;
      this.lblTitle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblTitle.Location = new Point(32, 0);
      this.lblTitle.Name = "lblTitle";
      this.lblTitle.Size = new Size(167, 13);
      this.lblTitle.TabIndex = 0;
      this.lblTitle.Text = "Select a milestone template.";
      this.lblTitle.Visible = false;
      this.groupContainer1.Borders = AnchorStyles.Left;
      this.groupContainer1.Controls.Add((Control) this.borderPanel1);
      this.groupContainer1.Dock = DockStyle.Right;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(223, 32);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(390, 384);
      this.groupContainer1.TabIndex = 3;
      this.groupContainer1.Text = "Milestone Template";
      this.borderPanel1.Borders = AnchorStyles.None;
      this.borderPanel1.Controls.Add((Control) this.gvMilestones);
      this.borderPanel1.Dock = DockStyle.Fill;
      this.borderPanel1.Location = new Point(1, 25);
      this.borderPanel1.Name = "borderPanel1";
      this.borderPanel1.Size = new Size(389, 359);
      this.borderPanel1.TabIndex = 1;
      this.gvMilestones.AllowDrop = true;
      this.gvMilestones.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Milestone";
      gvColumn1.Width = 125;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Role";
      gvColumn2.Width = 115;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Days";
      gvColumn3.Width = 131;
      this.gvMilestones.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.gvMilestones.Dock = DockStyle.Fill;
      this.gvMilestones.DropTarget = GVDropTarget.BetweenItems;
      this.gvMilestones.Location = new Point(0, 0);
      this.gvMilestones.Name = "gvMilestones";
      this.gvMilestones.Size = new Size(389, 359);
      this.gvMilestones.SortOption = GVSortOption.None;
      this.gvMilestones.TabIndex = 1;
      this.grpTemplates.Borders = AnchorStyles.Right;
      this.grpTemplates.Controls.Add((Control) this.gvTemplates);
      this.grpTemplates.Dock = DockStyle.Fill;
      this.grpTemplates.HeaderForeColor = SystemColors.ControlText;
      this.grpTemplates.Location = new Point(1, 32);
      this.grpTemplates.Name = "grpTemplates";
      this.grpTemplates.Size = new Size(219, 384);
      this.grpTemplates.TabIndex = 4;
      this.grpTemplates.Text = "Templates List";
      this.gvTemplates.AllowMultiselect = false;
      this.gvTemplates.BorderStyle = BorderStyle.None;
      this.gvTemplates.ClearSelectionsOnEmptyRowClick = false;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column2";
      gvColumn4.Text = "Name";
      gvColumn4.Width = 215;
      this.gvTemplates.Columns.AddRange(new GVColumn[1]
      {
        gvColumn4
      });
      this.gvTemplates.Dock = DockStyle.Fill;
      this.gvTemplates.Location = new Point(0, 25);
      this.gvTemplates.Name = "gvTemplates";
      this.gvTemplates.Size = new Size(218, 359);
      this.gvTemplates.SortOption = GVSortOption.None;
      this.gvTemplates.TabIndex = 1;
      this.gvTemplates.SelectedIndexChanged += new EventHandler(this.gvTemplates_SelectedIndexChanged);
      this.lblCurrentTemplate.AutoSize = true;
      this.lblCurrentTemplate.Location = new Point(105, 30);
      this.lblCurrentTemplate.Name = "lblCurrentTemplate";
      this.lblCurrentTemplate.Size = new Size(0, 13);
      this.lblCurrentTemplate.TabIndex = 5;
      this.btnSelect.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSelect.Location = new Point(431, 472);
      this.btnSelect.Name = "btnSelect";
      this.btnSelect.Size = new Size(116, 23);
      this.btnSelect.TabIndex = 6;
      this.btnSelect.Text = "Select and Continue";
      this.btnSelect.UseVisualStyleBackColor = true;
      this.btnSelect.Click += new EventHandler(this.btnSelect_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(553, 472);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 7;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = "ApplyMilestoneTemplate";
      this.emHelpLink1.Location = new Point(12, 479);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 10;
      this.lblTemplateName.AutoSize = true;
      this.lblTemplateName.BackColor = Color.Transparent;
      this.lblTemplateName.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblTemplateName.Location = new Point(100, 10);
      this.lblTemplateName.Name = "lblTemplateName";
      this.lblTemplateName.Size = new Size(91, 13);
      this.lblTemplateName.TabIndex = 1;
      this.lblTemplateName.Text = "template Name";
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Location = new Point(3, 10);
      this.label1.Name = "label1";
      this.label1.Size = new Size(94, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Current Template: ";
      this.pictureBox1.Dock = DockStyle.Left;
      this.pictureBox1.Image = (Image) componentResourceManager.GetObject("pictureBox1.Image");
      this.pictureBox1.Location = new Point(0, 0);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(32, 36);
      this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox1.TabIndex = 11;
      this.pictureBox1.TabStop = false;
      this.pictureBox1.Visible = false;
      this.panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.panel1.Controls.Add((Control) this.grpTemplates);
      this.panel1.Controls.Add((Control) this.splitter1);
      this.panel1.Controls.Add((Control) this.groupContainer1);
      this.panel1.Controls.Add((Control) this.gradientPanel2);
      this.panel1.Location = new Point(14, 49);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(614, 417);
      this.panel1.TabIndex = 13;
      this.splitter1.Dock = DockStyle.Right;
      this.splitter1.Location = new Point(220, 32);
      this.splitter1.Name = "splitter1";
      this.splitter1.Size = new Size(3, 384);
      this.splitter1.TabIndex = 5;
      this.splitter1.TabStop = false;
      this.gradientPanel2.Borders = AnchorStyles.Bottom;
      this.gradientPanel2.Controls.Add((Control) this.lblTemplateName);
      this.gradientPanel2.Controls.Add((Control) this.label1);
      this.gradientPanel2.Dock = DockStyle.Top;
      this.gradientPanel2.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel2.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel2.Location = new Point(1, 1);
      this.gradientPanel2.Name = "gradientPanel2";
      this.gradientPanel2.Size = new Size(612, 31);
      this.gradientPanel2.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gradientPanel2.TabIndex = 0;
      this.lblDesc.Dock = DockStyle.Top;
      this.lblDesc.Location = new Point(32, 13);
      this.lblDesc.Name = "lblDesc";
      this.lblDesc.Size = new Size(600, 22);
      this.lblDesc.TabIndex = 14;
      this.lblDesc.Text = "Current Template:  ";
      this.panel2.Controls.Add((Control) this.lblDesc);
      this.panel2.Controls.Add((Control) this.lblTitle);
      this.panel2.Controls.Add((Control) this.pictureBox1);
      this.panel2.Location = new Point(15, 12);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(600, 36);
      this.panel2.TabIndex = 15;
      this.AcceptButton = (IButtonControl) this.btnSelect;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(636, 504);
      this.Controls.Add((Control) this.panel2);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.emHelpLink1);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnSelect);
      this.Controls.Add((Control) this.lblCurrentTemplate);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (MilestoneTemplateSelector);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Apply Milestone Template";
      this.groupContainer1.ResumeLayout(false);
      this.borderPanel1.ResumeLayout(false);
      this.grpTemplates.ResumeLayout(false);
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.panel1.ResumeLayout(false);
      this.gradientPanel2.ResumeLayout(false);
      this.gradientPanel2.PerformLayout();
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
