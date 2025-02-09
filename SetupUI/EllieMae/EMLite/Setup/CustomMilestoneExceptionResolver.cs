// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.CustomMilestoneExceptionResolver
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
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
  public class CustomMilestoneExceptionResolver : Form
  {
    private Dictionary<MilestoneTemplate, string> exceptions;
    private IEnumerable<EllieMae.EMLite.Workflow.Milestone> milestones;
    private IEnumerable<MilestoneTemplate> milestoneTemplates;
    private string selectedMilestoneID;
    private string defaultMilestoneID;
    private bool suspendEvent;
    private Sessions.Session session;
    private IContainer components;
    private Label label1;
    private GroupContainer groupContainer1;
    private GridView gvExceptions;
    protected ImageList imgListTv;
    private Button button1;
    private Button button2;
    private StandardIconButton btnRemove;
    private StandardIconButton btnAssignPersona;
    private ToolTip toolTip1;
    private EMHelpLink emHelpLink1;
    protected Label lblDisconnectedFromPersona;
    protected Label lblLinkedWithPersona;

    public CustomMilestoneExceptionResolver(
      Dictionary<MilestoneTemplate, string> exceptions,
      string selectedMilestoneID,
      string defaultMilestoneID,
      string impactedArea)
      : this(Session.DefaultInstance, exceptions, selectedMilestoneID, defaultMilestoneID, impactedArea)
    {
    }

    public CustomMilestoneExceptionResolver(
      Sessions.Session session,
      Dictionary<MilestoneTemplate, string> exceptions,
      string selectedMilestoneID,
      string defaultMilestoneID,
      string impactedArea)
    {
      this.session = session;
      this.InitializeComponent();
      this.exceptions = exceptions;
      this.defaultMilestoneID = defaultMilestoneID;
      this.selectedMilestoneID = selectedMilestoneID;
      this.milestones = this.session.SessionObjects.BpmManager.GetMilestones(true);
      this.milestoneTemplates = this.session.SessionObjects.BpmManager.GetMilestoneTemplates(true);
      if (impactedArea.Contains("AutoLoan"))
      {
        this.Text = "Auto Loan Numbering Exceptions";
        this.label1.Text = "The milestone templates listed below have been set up to use a different trigger milestone for auto loan numbering than the trigger milestone indicated on the Auto Loan Numbering global setting. Here you can select a different auto loan numbering trigger milestone for each template or click the Add icon to add milestone templates to this list.";
      }
      else if (impactedArea.Contains("eDis"))
      {
        this.Text = "eDisclosure Packages Exceptions";
        this.label1.Text = "The milestone templates listed below have been set up to use a different trigger milestone for eDisclosure packages than the trigger milestone indicated on the eDisclosure Packages global setting. Here you can select a different eDisclosure package trigger milestone for each template or click the Add icon to add milestone templates to this list.";
      }
      this.label1.Text += "\n\nNote: Milestone templates that do not contain the ";
      if (impactedArea.Contains("AutoLoan"))
        this.label1.Text += "auto loan numbering trigger milestone indicated on the Auto Loan Numbering";
      else if (impactedArea.Contains("eDis"))
        this.label1.Text += "eDisclosure packages trigger milestone indicated on the eDisclosure Packages";
      this.label1.Text += " global setting are also listed here. By default, the system selects ";
      this.label1.Text += this.defaultMilestoneID != string.Empty ? this.milestones.FirstOrDefault<EllieMae.EMLite.Workflow.Milestone>((Func<EllieMae.EMLite.Workflow.Milestone, bool>) (x => x.MilestoneID == this.defaultMilestoneID)).Name : "";
      this.label1.Text += " as the trigger milestone for these templates.  ";
      if (this.defaultMilestoneID != string.Empty)
        this.milestones.FirstOrDefault<EllieMae.EMLite.Workflow.Milestone>((Func<EllieMae.EMLite.Workflow.Milestone, bool>) (x => x.MilestoneID == this.defaultMilestoneID)).Name += " (System Default)";
      this.loadGVExceptions();
      this.SetHelpTag(impactedArea);
    }

    private void loadGVExceptions()
    {
      this.gvExceptions.SuspendLayout();
      this.gvExceptions.Items.Clear();
      foreach (MilestoneTemplate key in this.exceptions.Keys)
      {
        GVItem gvItem = new GVItem(key.Name);
        gvItem.ImageIndex = this.exceptions[key] != null ? 1 : 0;
        MilestoneDropdownBox milestoneListControl = this.getMilestoneListControl(key, this.exceptions[key]);
        milestoneListControl.SelectedIndexChanged += new EventHandler(this.mileSelection_SelectedIndexChanged);
        milestoneListControl.Tag = (object) this.gvExceptions.Items.Count;
        milestoneListControl.SetPreferredSize(new Size(milestoneListControl.Width, 18));
        milestoneListControl.Font = new Font(milestoneListControl.Font.FontFamily, 6f);
        gvItem.SubItems.Add((object) milestoneListControl);
        gvItem.Tag = (object) key;
        this.gvExceptions.Items.Add(gvItem);
      }
      this.gvExceptions.ResumeLayout();
    }

    private void mileSelection_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.suspendEvent)
        return;
      this.gvExceptions.Items[Utils.ParseInt((object) string.Concat(((Control) sender).Tag))].ImageIndex = 1;
    }

    public Dictionary<MilestoneTemplate, string> ExceptionSettings()
    {
      Dictionary<MilestoneTemplate, string> dictionary = new Dictionary<MilestoneTemplate, string>();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvExceptions.Items)
      {
        MilestoneTemplate tag = (MilestoneTemplate) gvItem.Tag;
        MilestoneDropdownBox milestoneDropdownBox = (MilestoneDropdownBox) gvItem.SubItems[1].Value;
        if (gvItem.ImageIndex == 0 && milestoneDropdownBox.MilestoneID == this.defaultMilestoneID)
          dictionary.Add(tag, (string) null);
        else
          dictionary.Add(tag, milestoneDropdownBox.MilestoneID);
      }
      return dictionary;
    }

    private MilestoneDropdownBox getMilestoneListControl(
      MilestoneTemplate template,
      string selectedMilestoneID)
    {
      MilestoneDropdownBox milestoneListControl = new MilestoneDropdownBox();
      milestoneListControl.PopulateAllMilestones(this.getTempSpecMilestones(template), true, false);
      milestoneListControl.MilestoneID = selectedMilestoneID == null ? this.defaultMilestoneID : selectedMilestoneID ?? "";
      return milestoneListControl;
    }

    private void button1_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

    private IEnumerable<EllieMae.EMLite.Workflow.Milestone> getTempSpecMilestones(
      MilestoneTemplate template)
    {
      List<EllieMae.EMLite.Workflow.Milestone> result = new List<EllieMae.EMLite.Workflow.Milestone>();
      template.SequentialMilestones.ToList<TemplateMilestone>().ForEach((Action<TemplateMilestone>) (x => result.Add(this.milestones.FirstOrDefault<EllieMae.EMLite.Workflow.Milestone>((Func<EllieMae.EMLite.Workflow.Milestone, bool>) (y => y.MilestoneID == x.MilestoneID)))));
      return (IEnumerable<EllieMae.EMLite.Workflow.Milestone>) result;
    }

    private void btnRemove_Click(object sender, EventArgs e)
    {
      if (this.gvExceptions.SelectedItems.Count == 0)
        return;
      MilestoneTemplate tag = (MilestoneTemplate) this.gvExceptions.SelectedItems[0].Tag;
      if (tag.SequentialMilestones.FirstOrDefault<TemplateMilestone>((Func<TemplateMilestone, bool>) (x => x.MilestoneID == this.selectedMilestoneID)) == null)
      {
        this.suspendEvent = true;
        tag.AutoLoanNumberingMilestoneID = "";
        GVItem selectedItem = this.gvExceptions.SelectedItems[0];
        selectedItem.ImageIndex = 0;
        ((MilestoneDropdownBox) selectedItem.SubItems[1].Value).MilestoneID = this.defaultMilestoneID;
        selectedItem.Tag = (object) tag;
        this.suspendEvent = false;
      }
      else
        this.gvExceptions.SelectedItems.ToList<GVItem>().ForEach((Action<GVItem>) (x => this.gvExceptions.Items.Remove(x)));
    }

    private void btnAssign_Click(object sender, EventArgs e)
    {
      List<MilestoneTemplate> result = new List<MilestoneTemplate>();
      this.milestoneTemplates.ToList<MilestoneTemplate>().ForEach((Action<MilestoneTemplate>) (x =>
      {
        if (this.ExceptionSettings().Keys.FirstOrDefault<MilestoneTemplate>((Func<MilestoneTemplate, bool>) (y => y.TemplateID == x.TemplateID)) != null)
          return;
        result.Add(x);
      }));
      CustomMilestoneExceptionTemplate exceptionTemplate = new CustomMilestoneExceptionTemplate(result);
      if (DialogResult.OK != exceptionTemplate.ShowDialog())
        return;
      this.SuspendLayout();
      foreach (MilestoneTemplate selectedTemplate in exceptionTemplate.SelectedTemplates())
        this.gvExceptions.Items.Add(new GVItem(selectedTemplate.Name)
        {
          ImageIndex = 1,
          SubItems = {
            (object) this.getMilestoneListControl(selectedTemplate, this.selectedMilestoneID)
          },
          Tag = (object) selectedTemplate
        });
      this.gvExceptions.Invalidate();
      this.ResumeLayout();
    }

    private void gvExceptions_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.toolTip1.SetToolTip((Control) this.btnRemove, "");
      if (this.gvExceptions.SelectedItems.Count == 0)
      {
        this.btnRemove.Enabled = false;
      }
      else
      {
        this.btnRemove.Enabled = true;
        GVItem selectedItem = this.gvExceptions.SelectedItems[0];
        if (selectedItem.ImageIndex == 1 || !(((MilestoneDropdownBox) selectedItem.SubItems[1].Value).MilestoneID == this.defaultMilestoneID))
          return;
        this.toolTip1.SetToolTip((Control) this.btnRemove, "The template cannot be deleted because the configured milestone is not available on this template.  Either select a different milestone to apply to this template or add the configured milestone to the template to remove it from this exception table.");
        this.btnRemove.Enabled = false;
      }
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
      this.components = (IContainer) new System.ComponentModel.Container();
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (CustomMilestoneExceptionResolver));
      this.label1 = new Label();
      this.groupContainer1 = new GroupContainer();
      this.btnRemove = new StandardIconButton();
      this.btnAssignPersona = new StandardIconButton();
      this.gvExceptions = new GridView();
      this.imgListTv = new ImageList(this.components);
      this.button1 = new Button();
      this.button2 = new Button();
      this.toolTip1 = new ToolTip(this.components);
      this.emHelpLink1 = new EMHelpLink();
      this.lblDisconnectedFromPersona = new Label();
      this.lblLinkedWithPersona = new Label();
      this.groupContainer1.SuspendLayout();
      ((ISupportInitialize) this.btnRemove).BeginInit();
      ((ISupportInitialize) this.btnAssignPersona).BeginInit();
      this.SuspendLayout();
      this.label1.Location = new Point(12, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(532, 112);
      this.label1.TabIndex = 4;
      this.groupContainer1.Controls.Add((Control) this.btnRemove);
      this.groupContainer1.Controls.Add((Control) this.btnAssignPersona);
      this.groupContainer1.Controls.Add((Control) this.gvExceptions);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(12, 124);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(525, 226);
      this.groupContainer1.TabIndex = 0;
      this.groupContainer1.Text = "Templates with Exceptions";
      this.btnRemove.BackColor = Color.Transparent;
      this.btnRemove.Enabled = false;
      this.btnRemove.Location = new Point(505, 4);
      this.btnRemove.Margin = new Padding(2, 3, 3, 3);
      this.btnRemove.MouseDownImage = (Image) null;
      this.btnRemove.Name = "btnRemove";
      this.btnRemove.Size = new Size(16, 16);
      this.btnRemove.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnRemove.TabIndex = 24;
      this.btnRemove.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnRemove, "Remove Exception");
      this.btnRemove.Click += new EventHandler(this.btnRemove_Click);
      this.btnAssignPersona.BackColor = Color.Transparent;
      this.btnAssignPersona.Location = new Point(484, 4);
      this.btnAssignPersona.Margin = new Padding(2, 3, 3, 3);
      this.btnAssignPersona.MouseDownImage = (Image) null;
      this.btnAssignPersona.Name = "btnAssignPersona";
      this.btnAssignPersona.Size = new Size(16, 16);
      this.btnAssignPersona.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAssignPersona.TabIndex = 25;
      this.btnAssignPersona.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnAssignPersona, "Add an Exception");
      this.btnAssignPersona.Click += new EventHandler(this.btnAssign_Click);
      this.gvExceptions.AllowMultiselect = false;
      this.gvExceptions.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Template";
      gvColumn1.Width = 240;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Milestone";
      gvColumn2.Width = 250;
      this.gvExceptions.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvExceptions.Dock = DockStyle.Fill;
      this.gvExceptions.ImageList = this.imgListTv;
      this.gvExceptions.Location = new Point(1, 26);
      this.gvExceptions.Name = "gvExceptions";
      this.gvExceptions.Size = new Size(523, 199);
      this.gvExceptions.SortOption = GVSortOption.None;
      this.gvExceptions.TabIndex = 1;
      this.gvExceptions.SelectedIndexChanged += new EventHandler(this.gvExceptions_SelectedIndexChanged);
      this.imgListTv.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgListTv.ImageStream");
      this.imgListTv.TransparentColor = Color.White;
      this.imgListTv.Images.SetKeyName(0, "");
      this.imgListTv.Images.SetKeyName(1, "");
      this.button1.DialogResult = DialogResult.OK;
      this.button1.Location = new Point(384, 395);
      this.button1.Name = "button1";
      this.button1.Size = new Size(75, 23);
      this.button1.TabIndex = 2;
      this.button1.Text = "Save";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new EventHandler(this.button1_Click);
      this.button2.DialogResult = DialogResult.Cancel;
      this.button2.Location = new Point(465, 395);
      this.button2.Name = "button2";
      this.button2.Size = new Size(75, 23);
      this.button2.TabIndex = 3;
      this.button2.Text = "Cancel";
      this.button2.UseVisualStyleBackColor = true;
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = "CustomExceptionResolver";
      this.emHelpLink1.Location = new Point(12, 402);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 41;
      this.lblDisconnectedFromPersona.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblDisconnectedFromPersona.AutoSize = true;
      this.lblDisconnectedFromPersona.Image = (Image) Resources.link_broken;
      this.lblDisconnectedFromPersona.ImageAlign = ContentAlignment.MiddleLeft;
      this.lblDisconnectedFromPersona.Location = new Point(9, 379);
      this.lblDisconnectedFromPersona.Name = "lblDisconnectedFromPersona";
      this.lblDisconnectedFromPersona.Size = new Size(356, 13);
      this.lblDisconnectedFromPersona.TabIndex = 43;
      this.lblDisconnectedFromPersona.Text = "        Trigger milestone selected by admin (here or using Milestones setting)";
      this.lblDisconnectedFromPersona.TextAlign = ContentAlignment.MiddleLeft;
      this.lblLinkedWithPersona.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblLinkedWithPersona.AutoSize = true;
      this.lblLinkedWithPersona.Image = (Image) Resources.link;
      this.lblLinkedWithPersona.ImageAlign = ContentAlignment.MiddleLeft;
      this.lblLinkedWithPersona.Location = new Point(9, 363);
      this.lblLinkedWithPersona.Name = "lblLinkedWithPersona";
      this.lblLinkedWithPersona.Size = new Size(279, 13);
      this.lblLinkedWithPersona.TabIndex = 42;
      this.lblLinkedWithPersona.Text = "        Trigger milestone selected by system (system default)";
      this.lblLinkedWithPersona.TextAlign = ContentAlignment.MiddleLeft;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(552, 430);
      this.Controls.Add((Control) this.lblDisconnectedFromPersona);
      this.Controls.Add((Control) this.lblLinkedWithPersona);
      this.Controls.Add((Control) this.emHelpLink1);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.button2);
      this.Controls.Add((Control) this.button1);
      this.Controls.Add((Control) this.groupContainer1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (CustomMilestoneExceptionResolver);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Auto Loan Numbering Exceptions";
      this.groupContainer1.ResumeLayout(false);
      ((ISupportInitialize) this.btnRemove).EndInit();
      ((ISupportInitialize) this.btnAssignPersona).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
