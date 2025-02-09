// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ViewDocAssignedTPOs
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.ExternalOriginatorManagement;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class ViewDocAssignedTPOs : Form
  {
    private Sessions.Session session;
    private List<ExternalSettingValue> docCategories;
    private DocumentSettingInfo document;
    private IContainer components;
    private Label lblChannel;
    private Label label1;
    private Label lblEndDate;
    private Label lblStartDate;
    private Label lblDocDisplayName;
    private Label lblFile;
    private Label lblChannelVal;
    private Label lblCategoryVal;
    private Label labelEndDateVal;
    private Label lblStartDateVal;
    private Label lblDocDisplayNameVal;
    private Label lblFileNameVal;
    private GroupContainer grpTPOCompanies;
    private StandardIconButton stdBtnSearchTpos;
    private GridView grdViewTpos;
    private Button btnOK;

    public ViewDocAssignedTPOs(
      DocumentSettingInfo document,
      List<ExternalSettingValue> documentCategories,
      Sessions.Session session)
    {
      this.session = session;
      this.docCategories = documentCategories;
      this.InitializeComponent();
      this.PopulateDocumentInfo(document);
      this.PopluateTpos();
      this.InitializeTPOSearchControl();
    }

    private void InitializeTPOSearchControl()
    {
      if (((FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.ExternalSettings_TPOWCEditDocuments))
        return;
      this.stdBtnSearchTpos.Enabled = false;
    }

    private void PopulateDocumentInfo(DocumentSettingInfo document)
    {
      this.lblFileNameVal.Text = document.FileName;
      this.lblDocDisplayNameVal.Text = document.DisplayName;
      this.lblChannelVal.Text = document.ChannelStr;
      this.lblCategoryVal.Text = this.GetDocCategory(document);
      this.lblStartDateVal.Text = document.StartDate != DateTime.MinValue ? document.StartDate.ToString("d") : "";
      this.labelEndDateVal.Text = document.EndDate != DateTime.MinValue ? document.EndDate.ToString("d") : "";
      this.document = document;
    }

    private void PopluateTpos()
    {
      List<int> externalOrgIds = this.session.ConfigurationManager.GetExternalOrgsByDocumentGuid(this.document.Guid);
      this.InitializeAssignmentGrid(this.session.ConfigurationManager.GetAllExternalParentOrganizations(false).Where<ExternalOriginatorManagementData>((Func<ExternalOriginatorManagementData, bool>) (c => externalOrgIds.Contains(c.oid))).Distinct<ExternalOriginatorManagementData>().ToList<ExternalOriginatorManagementData>());
    }

    private string GetDocCategory(DocumentSettingInfo doc)
    {
      string docCategory = "Not Set";
      if (this.docCategories != null && this.docCategories.Any<ExternalSettingValue>())
      {
        ExternalSettingValue externalSettingValue = this.docCategories.Where<ExternalSettingValue>((Func<ExternalSettingValue, bool>) (a => a.settingId == doc.Category)).SingleOrDefault<ExternalSettingValue>();
        if (externalSettingValue != null)
          docCategory = externalSettingValue.settingValue;
      }
      return docCategory;
    }

    private void btnOK_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

    private void stdBtnSearchTpos_Click(object sender, EventArgs e)
    {
      TPOList tpoList = new TPOList(this.session.ConfigurationManager.GetAllExternalParentOrganizations(false), this.grdViewTpos.Items.Select<GVItem, ExternalOriginatorManagementData>((Func<GVItem, ExternalOriginatorManagementData>) (item => (ExternalOriginatorManagementData) item.Tag)).ToList<ExternalOriginatorManagementData>());
      if (tpoList.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      this.InitializeAssignmentGrid(tpoList.selectedCompanies);
      this.UpdateDocAssignedTPOs(tpoList);
    }

    private void InitializeAssignmentGrid(
      List<ExternalOriginatorManagementData> selectedCompanies)
    {
      this.grdViewTpos.Items.Clear();
      Dictionary<string, bool> docAssignedTpOs = this.session.ConfigurationManager.GetDocAssignedTPOs(this.document.Guid);
      foreach (ExternalOriginatorManagementData selectedCompany in selectedCompanies)
      {
        bool flag = docAssignedTpOs.ContainsKey(selectedCompany.OrganizationName) ? docAssignedTpOs[selectedCompany.OrganizationName] : this.document.Active;
        this.grdViewTpos.Items.Add(new GVItem()
        {
          SubItems = {
            (object) selectedCompany.OrganizationName,
            flag ? (object) "Yes" : (object) "No"
          },
          Tag = (object) selectedCompany
        });
      }
    }

    private void UpdateDocAssignedTPOs(TPOList tpoList)
    {
      IEnumerable<int> ints = tpoList.selectedCompanies.Select<ExternalOriginatorManagementData, int>((Func<ExternalOriginatorManagementData, int>) (c => c.oid));
      List<int> orgsByDocumentGuid = this.session.ConfigurationManager.GetExternalOrgsByDocumentGuid(this.document.Guid);
      if (!ints.Any<int>() && !orgsByDocumentGuid.Any<int>())
        return;
      List<int> list1 = orgsByDocumentGuid.Except<int>(ints).ToList<int>();
      if (list1.Any<int>())
        this.session.ConfigurationManager.RemoveAssignedDocFromTPOs(this.document.Guid.ToString(), list1);
      List<int> list2 = ints.Except<int>((IEnumerable<int>) orgsByDocumentGuid).ToList<int>();
      if (!list2.Any<int>())
        return;
      this.session.ConfigurationManager.AssignDocumentToTpos(list2, this.document);
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ViewDocAssignedTPOs));
      this.lblChannel = new Label();
      this.label1 = new Label();
      this.lblEndDate = new Label();
      this.lblStartDate = new Label();
      this.lblDocDisplayName = new Label();
      this.lblFile = new Label();
      this.lblChannelVal = new Label();
      this.lblCategoryVal = new Label();
      this.labelEndDateVal = new Label();
      this.lblStartDateVal = new Label();
      this.lblDocDisplayNameVal = new Label();
      this.lblFileNameVal = new Label();
      this.grpTPOCompanies = new GroupContainer();
      this.grdViewTpos = new GridView();
      this.btnOK = new Button();
      this.stdBtnSearchTpos = new StandardIconButton();
      this.grpTPOCompanies.SuspendLayout();
      ((ISupportInitialize) this.stdBtnSearchTpos).BeginInit();
      this.SuspendLayout();
      this.lblChannel.AutoSize = true;
      this.lblChannel.Location = new Point(28, 160);
      this.lblChannel.Name = "lblChannel";
      this.lblChannel.Size = new Size(46, 13);
      this.lblChannel.TabIndex = 74;
      this.lblChannel.Text = "Channel";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(28, 133);
      this.label1.Name = "label1";
      this.label1.Size = new Size(49, 13);
      this.label1.TabIndex = 73;
      this.label1.Text = "Category";
      this.lblEndDate.AutoSize = true;
      this.lblEndDate.Location = new Point(27, 106);
      this.lblEndDate.Name = "lblEndDate";
      this.lblEndDate.Size = new Size(52, 13);
      this.lblEndDate.TabIndex = 72;
      this.lblEndDate.Text = "End Date";
      this.lblStartDate.AutoSize = true;
      this.lblStartDate.Location = new Point(27, 79);
      this.lblStartDate.Name = "lblStartDate";
      this.lblStartDate.Size = new Size(55, 13);
      this.lblStartDate.TabIndex = 71;
      this.lblStartDate.Text = "Start Date";
      this.lblDocDisplayName.AutoSize = true;
      this.lblDocDisplayName.Location = new Point(28, 52);
      this.lblDocDisplayName.Name = "lblDocDisplayName";
      this.lblDocDisplayName.Size = new Size(124, 13);
      this.lblDocDisplayName.TabIndex = 70;
      this.lblDocDisplayName.Text = "Document Display Name";
      this.lblFile.AutoSize = true;
      this.lblFile.Location = new Point(28, 25);
      this.lblFile.Name = "lblFile";
      this.lblFile.Size = new Size(54, 13);
      this.lblFile.TabIndex = 69;
      this.lblFile.Text = "File Name";
      this.lblChannelVal.AutoSize = true;
      this.lblChannelVal.Location = new Point(192, 160);
      this.lblChannelVal.Name = "lblChannelVal";
      this.lblChannelVal.Size = new Size(18, 13);
      this.lblChannelVal.TabIndex = 80;
      this.lblChannelVal.Text = "All";
      this.lblCategoryVal.AutoSize = true;
      this.lblCategoryVal.Location = new Point(192, 133);
      this.lblCategoryVal.Name = "lblCategoryVal";
      this.lblCategoryVal.Size = new Size(66, 13);
      this.lblCategoryVal.TabIndex = 79;
      this.lblCategoryVal.Text = "No Category";
      this.labelEndDateVal.AutoSize = true;
      this.labelEndDateVal.Location = new Point(191, 106);
      this.labelEndDateVal.Name = "labelEndDateVal";
      this.labelEndDateVal.Size = new Size(43, 13);
      this.labelEndDateVal.TabIndex = 78;
      this.labelEndDateVal.Text = "Not Set";
      this.lblStartDateVal.AutoSize = true;
      this.lblStartDateVal.Location = new Point(191, 79);
      this.lblStartDateVal.Name = "lblStartDateVal";
      this.lblStartDateVal.Size = new Size(43, 13);
      this.lblStartDateVal.TabIndex = 77;
      this.lblStartDateVal.Text = "Not Set";
      this.lblDocDisplayNameVal.AutoSize = true;
      this.lblDocDisplayNameVal.Location = new Point(192, 52);
      this.lblDocDisplayNameVal.Name = "lblDocDisplayNameVal";
      this.lblDocDisplayNameVal.Size = new Size(40, 13);
      this.lblDocDisplayNameVal.TabIndex = 76;
      this.lblDocDisplayNameVal.Text = "<Text>";
      this.lblFileNameVal.AutoEllipsis = true;
      this.lblFileNameVal.Location = new Point(192, 25);
      this.lblFileNameVal.Name = "lblFileNameVal";
      this.lblFileNameVal.Size = new Size(356, 13);
      this.lblFileNameVal.TabIndex = 75;
      this.lblFileNameVal.Text = "<Text>";
      this.grpTPOCompanies.Controls.Add((Control) this.stdBtnSearchTpos);
      this.grpTPOCompanies.Controls.Add((Control) this.grdViewTpos);
      this.grpTPOCompanies.HeaderForeColor = SystemColors.ControlText;
      this.grpTPOCompanies.Location = new Point(30, 196);
      this.grpTPOCompanies.Name = "grpTPOCompanies";
      this.grpTPOCompanies.Size = new Size(519, 248);
      this.grpTPOCompanies.TabIndex = 81;
      this.grpTPOCompanies.Text = "Assigned TPOs";
      this.stdBtnSearchTpos.BackColor = Color.Transparent;
      this.stdBtnSearchTpos.Location = new Point(490, 4);
      this.stdBtnSearchTpos.MouseDownImage = (Image) null;
      this.stdBtnSearchTpos.Name = "stdBtnSearchTpos";
      this.stdBtnSearchTpos.Size = new Size(16, 16);
      this.stdBtnSearchTpos.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.stdBtnSearchTpos.TabIndex = 2;
      this.stdBtnSearchTpos.TabStop = false;
      this.stdBtnSearchTpos.Enabled = true;
      this.stdBtnSearchTpos.Click += new EventHandler(this.stdBtnSearchTpos_Click);
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "ColumnCompany";
      gvColumn1.Text = "TPO Company";
      gvColumn1.Width = 300;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "ColumnActive";
      gvColumn2.Text = "Active";
      gvColumn2.Width = 100;
      this.grdViewTpos.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.grdViewTpos.Dock = DockStyle.Fill;
      this.grdViewTpos.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.grdViewTpos.Location = new Point(1, 26);
      this.grdViewTpos.Name = "grdViewTpos";
      this.grdViewTpos.Size = new Size(517, 221);
      this.grdViewTpos.TabIndex = 0;
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(476, 470);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 82;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(584, 517);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.grpTPOCompanies);
      this.Controls.Add((Control) this.lblChannelVal);
      this.Controls.Add((Control) this.lblCategoryVal);
      this.Controls.Add((Control) this.labelEndDateVal);
      this.Controls.Add((Control) this.lblStartDateVal);
      this.Controls.Add((Control) this.lblDocDisplayNameVal);
      this.Controls.Add((Control) this.lblFileNameVal);
      this.Controls.Add((Control) this.lblChannel);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.lblEndDate);
      this.Controls.Add((Control) this.lblStartDate);
      this.Controls.Add((Control) this.lblDocDisplayName);
      this.Controls.Add((Control) this.lblFile);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ViewDocAssignedTPOs);
      this.Text = "Assigned TPOs";
      this.grpTPOCompanies.ResumeLayout(false);
      ((ISupportInitialize) this.stdBtnSearchTpos).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
