// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.BrokerUserGroup.AccessPage
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ContactGroup;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.BrokerUserGroup
{
  public class AccessPage : Form, IGroupSecurityPage
  {
    private int _currentGroupId = -1;
    private bool dirty;
    private string userID = "";
    private AclGroup[] groupList;
    private bool firstTime = true;
    private bool suspendEvent;
    private AclFileType[] loanTempFileTypes = new AclFileType[7]
    {
      AclFileType.LoanProgram,
      AclFileType.ClosingCost,
      AclFileType.FormList,
      AclFileType.DocumentSet,
      AclFileType.TaskSet,
      AclFileType.MiscData,
      AclFileType.LoanTemplate
    };
    private AclFileType[] dashReportFileTypes = new AclFileType[3]
    {
      AclFileType.Reports,
      AclFileType.DashboardTemplate,
      AclFileType.DashboardViewTemplate
    };
    private AclFileType[] outputFileTypes = new AclFileType[2]
    {
      AclFileType.CustomPrintForms,
      AclFileType.PrintGroups
    };
    private AclFileType[] letterCampaignFileTypes = new AclFileType[3]
    {
      AclFileType.BorrowerCustomLetters,
      AclFileType.BizCustomLetters,
      AclFileType.CampaignTemplate
    };
    private IContainer components;
    private GroupContainer groupContainer1;
    private CheckBox chkBoxViewSubordinateContacts;
    private ComboBox cmbSuperiorAccess;
    private Label label3;
    private CheckBox chkCusLetterCampaignTemp;
    private CheckBox chkPrintFormsGroups;
    private CheckBox chkDashboardReport;
    private CheckBox chkLoanTemplate;
    private CheckBox chkPublicBizContact;

    public event EventHandler DirtyFlagChanged;

    public AccessPage(int groupID, EventHandler dirtyFlagChanged)
    {
      this.InitializeComponent();
      this.cmbSuperiorAccess.Items.AddRange((object[]) new string[2]
      {
        "View Only",
        "Edit"
      });
      this.currentGroupId = groupID;
      this.DirtyFlagChanged += dirtyFlagChanged;
    }

    public AccessPage(string userID, AclGroup[] groups, EventHandler dirtyFlagChanged)
    {
      this.InitializeComponent();
      this.cmbSuperiorAccess.Items.AddRange((object[]) new string[2]
      {
        "View Only",
        "Edit"
      });
      this.userID = userID;
      this.groupList = groups;
      this.loadPageValueForUser();
      this.MakeReadOnly();
      this.DirtyFlagChanged += dirtyFlagChanged;
    }

    public bool HasBeenModified() => this.dirty;

    private void setDirtyFlag(bool val)
    {
      this.dirty = val;
      if (this.DirtyFlagChanged == null)
        return;
      this.DirtyFlagChanged((object) this, (EventArgs) null);
    }

    private int currentGroupId
    {
      get => this._currentGroupId;
      set
      {
        if (this.firstTime)
        {
          this._currentGroupId = value;
          this.setDirtyFlag(false);
          this.loadPageValueForGroup(value);
        }
        else
        {
          if (this.firstTime || this._currentGroupId.Equals(value))
            return;
          this.firstTime = true;
          this._currentGroupId = value;
          this.setDirtyFlag(false);
          this.loadPageValueForGroup(value);
        }
      }
    }

    private void loadPageValueForGroup(int groupID)
    {
      this.suspendEvent = true;
      AclGroup groupById = Session.AclGroupManager.GetGroupById(groupID);
      this.chkBoxViewSubordinateContacts.Checked = groupById.ViewSubordinatesContacts;
      if (this.chkBoxViewSubordinateContacts.Checked)
        this.cmbSuperiorAccess.Enabled = true;
      else
        this.cmbSuperiorAccess.Enabled = false;
      if (groupById.ContactAccess == AclResourceAccess.ReadOnly)
        this.cmbSuperiorAccess.SelectedIndex = 0;
      else
        this.cmbSuperiorAccess.SelectedIndex = 1;
      this.chkLoanTemplate.Checked = this.canManageTemplates(groupID, this.loanTempFileTypes);
      this.chkDashboardReport.Checked = this.canManageTemplates(groupID, this.dashReportFileTypes);
      this.chkPrintFormsGroups.Checked = this.canManageTemplates(groupID, this.outputFileTypes);
      this.chkCusLetterCampaignTemp.Checked = this.canManageTemplates(groupID, this.letterCampaignFileTypes);
      this.chkPublicBizContact.Checked = this.canManagePublicBusinessContact(groupID);
      this.suspendEvent = false;
    }

    private bool canManageTemplates(int groupID, AclFileType[] fileTypes)
    {
      return this.canManageTemplates(new int[1]{ groupID }, fileTypes);
    }

    private bool canManagePublicBusinessContact(int groupID)
    {
      return this.canManagePublicBusinessContact(new int[1]
      {
        groupID
      });
    }

    private bool canManageTemplates(int[] groupIDs, AclFileType[] fileTypes)
    {
      Dictionary<AclFileType, FileInGroup[]> aclGroupFileRefs = Session.AclGroupManager.GetAclGroupFileRefs(groupIDs, fileTypes);
      foreach (AclFileType fileType in fileTypes)
      {
        if (!aclGroupFileRefs.ContainsKey(fileType) || aclGroupFileRefs[fileType] == null || aclGroupFileRefs[fileType].Length == 0)
          return false;
        bool flag = false;
        foreach (FileInGroup fileInGroup in aclGroupFileRefs[fileType])
        {
          if (fileInGroup.Access == AclResourceAccess.ReadWrite)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          return false;
      }
      return true;
    }

    private bool canManagePublicBusinessContact(int[] groupdIDs)
    {
      foreach (int groupdId in groupdIDs)
      {
        BizGroupRef[] contactGroupRefs = Session.AclGroupManager.GetBizContactGroupRefs(groupdId);
        if (contactGroupRefs != null && contactGroupRefs.Length != 0)
        {
          foreach (BizGroupRef bizGroupRef in contactGroupRefs)
          {
            this.chkPublicBizContact.Tag = (object) bizGroupRef;
            if (bizGroupRef.Access == AclResourceAccess.ReadWrite)
              return true;
          }
        }
      }
      if (this.chkPublicBizContact.Tag == null)
      {
        ContactGroupInfo[] bizContactGroups = Session.ContactGroupManager.GetPublicBizContactGroups();
        if (bizContactGroups != null && bizContactGroups.Length != 0)
          this.chkPublicBizContact.Tag = (object) new BizGroupRef(bizContactGroups[0].GroupId, AclResourceAccess.ReadOnly);
      }
      return false;
    }

    private void cmbSuperiorAccess_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.suspendEvent)
        return;
      this.setDirtyFlag(true);
    }

    private void loadPageValueForUser()
    {
      this.suspendEvent = true;
      foreach (AclGroup group in this.groupList)
      {
        if (group.ViewSubordinatesContacts)
        {
          if (!this.chkBoxViewSubordinateContacts.Checked)
            this.chkBoxViewSubordinateContacts.Checked = true;
          if (group.ContactAccess == AclResourceAccess.ReadWrite)
            this.cmbSuperiorAccess.SelectedIndex = 1;
          else if (this.cmbSuperiorAccess.SelectedItem == null)
            this.cmbSuperiorAccess.SelectedIndex = 0;
        }
      }
      List<int> intList = new List<int>();
      foreach (AclGroup group in this.groupList)
        intList.Add(group.ID);
      this.chkLoanTemplate.Checked = this.canManageTemplates(intList.ToArray(), this.loanTempFileTypes);
      this.chkDashboardReport.Checked = this.canManageTemplates(intList.ToArray(), this.dashReportFileTypes);
      this.chkPrintFormsGroups.Checked = this.canManageTemplates(intList.ToArray(), this.outputFileTypes);
      this.chkCusLetterCampaignTemp.Checked = this.canManageTemplates(intList.ToArray(), this.letterCampaignFileTypes);
      this.chkPublicBizContact.Checked = this.canManagePublicBusinessContact(intList.ToArray());
      this.suspendEvent = false;
    }

    private void chkBoxViewSubordinateContacts_CheckedChanged(object sender, EventArgs e)
    {
      if (this.suspendEvent)
        return;
      this.setDirtyFlag(true);
      if (this.chkBoxViewSubordinateContacts.Checked)
      {
        this.cmbSuperiorAccess.Enabled = true;
      }
      else
      {
        this.cmbSuperiorAccess.Enabled = false;
        this.cmbSuperiorAccess.SelectedIndex = 0;
      }
    }

    private string getAccessString(AclResourceAccess access)
    {
      return access == AclResourceAccess.ReadOnly ? "View Only" : "Edit";
    }

    private void MakeReadOnly()
    {
      this.chkBoxViewSubordinateContacts.Enabled = false;
      this.cmbSuperiorAccess.Enabled = false;
      this.chkLoanTemplate.Enabled = false;
      this.chkDashboardReport.Enabled = false;
      this.chkCusLetterCampaignTemp.Enabled = false;
      this.chkPrintFormsGroups.Enabled = false;
      this.chkPublicBizContact.Enabled = false;
    }

    public void SaveData()
    {
      if (this.currentGroupId <= 0)
        return;
      AclGroup groupById = Session.AclGroupManager.GetGroupById(this.currentGroupId);
      if (this.chkBoxViewSubordinateContacts.Checked)
      {
        groupById.ViewSubordinatesContacts = true;
        groupById.ContactAccess = this.cmbSuperiorAccess.SelectedIndex != 0 ? AclResourceAccess.ReadWrite : AclResourceAccess.ReadOnly;
      }
      else
      {
        groupById.ViewSubordinatesContacts = false;
        groupById.ContactAccess = AclResourceAccess.ReadOnly;
      }
      Session.AclGroupManager.UpdateGroup(groupById);
      List<AclFileType> aclFileTypeList = new List<AclFileType>();
      aclFileTypeList.AddRange((IEnumerable<AclFileType>) this.loanTempFileTypes);
      aclFileTypeList.AddRange((IEnumerable<AclFileType>) this.dashReportFileTypes);
      aclFileTypeList.AddRange((IEnumerable<AclFileType>) this.outputFileTypes);
      aclFileTypeList.AddRange((IEnumerable<AclFileType>) this.letterCampaignFileTypes);
      Dictionary<AclFileType, FileSystemEntry> rootFileSystemEntry = Session.AclGroupManager.GetRootFileSystemEntry(aclFileTypeList.ToArray());
      List<AclFileResource> aclFileResourceList = new List<AclFileResource>();
      foreach (AclFileType aclFileType in aclFileTypeList.ToArray())
        aclFileResourceList.Add(new AclFileResource(-1, rootFileSystemEntry[aclFileType].ToString(), aclFileType, true, (string) null));
      Dictionary<int, AclFileResource> dictionary = Session.AclGroupManager.AddAclFileResources(aclFileResourceList.ToArray());
      Dictionary<AclFileType, FileInGroup[]> updateList = new Dictionary<AclFileType, FileInGroup[]>();
      foreach (int key in dictionary.Keys)
      {
        AclFileResource aclFileResource = dictionary[key];
        FileInGroup fileInGroup;
        switch (aclFileResource.FileType)
        {
          case AclFileType.LoanProgram:
          case AclFileType.ClosingCost:
          case AclFileType.MiscData:
          case AclFileType.FormList:
          case AclFileType.DocumentSet:
          case AclFileType.LoanTemplate:
          case AclFileType.TaskSet:
            fileInGroup = new FileInGroup(key, true, this.chkLoanTemplate.Checked ? AclResourceAccess.ReadWrite : AclResourceAccess.ReadOnly);
            break;
          case AclFileType.CustomPrintForms:
          case AclFileType.PrintGroups:
            fileInGroup = new FileInGroup(key, true, this.chkPrintFormsGroups.Checked ? AclResourceAccess.ReadWrite : AclResourceAccess.ReadOnly);
            break;
          case AclFileType.Reports:
          case AclFileType.DashboardTemplate:
          case AclFileType.DashboardViewTemplate:
            fileInGroup = new FileInGroup(key, true, this.chkDashboardReport.Checked ? AclResourceAccess.ReadWrite : AclResourceAccess.ReadOnly);
            break;
          case AclFileType.BorrowerCustomLetters:
          case AclFileType.BizCustomLetters:
          case AclFileType.CampaignTemplate:
            fileInGroup = new FileInGroup(key, true, this.chkCusLetterCampaignTemp.Checked ? AclResourceAccess.ReadWrite : AclResourceAccess.ReadOnly);
            break;
          default:
            throw new Exception("No matching AclFileType can be found: " + (object) aclFileResource.FileType);
        }
        List<FileInGroup> fileInGroupList = new List<FileInGroup>();
        if (updateList.ContainsKey(aclFileResource.FileType))
          fileInGroupList.AddRange((IEnumerable<FileInGroup>) updateList[aclFileResource.FileType]);
        fileInGroupList.Add(fileInGroup);
        updateList.Add(aclFileResource.FileType, fileInGroupList.ToArray());
      }
      Session.AclGroupManager.ResetAclGroupFileRefs(this.currentGroupId, updateList);
      BizGroupRef tag = (BizGroupRef) this.chkPublicBizContact.Tag;
      tag.Access = !this.chkPublicBizContact.Checked ? AclResourceAccess.ReadOnly : AclResourceAccess.ReadWrite;
      Session.AclGroupManager.ResetBizContactGroupRefs(this.currentGroupId, new BizGroupRef[1]
      {
        tag
      });
      this.setDirtyFlag(false);
    }

    public void ResetData()
    {
      this.firstTime = true;
      this.currentGroupId = this.currentGroupId;
    }

    public void SetGroup(int groupId) => this.currentGroupId = groupId;

    private void chk_CheckedChanged(object sender, EventArgs e)
    {
      if (this.suspendEvent)
        return;
      this.setDirtyFlag(true);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.groupContainer1 = new GroupContainer();
      this.chkCusLetterCampaignTemp = new CheckBox();
      this.chkPrintFormsGroups = new CheckBox();
      this.chkDashboardReport = new CheckBox();
      this.chkLoanTemplate = new CheckBox();
      this.label3 = new Label();
      this.chkBoxViewSubordinateContacts = new CheckBox();
      this.cmbSuperiorAccess = new ComboBox();
      this.chkPublicBizContact = new CheckBox();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.groupContainer1.Controls.Add((Control) this.chkPublicBizContact);
      this.groupContainer1.Controls.Add((Control) this.chkCusLetterCampaignTemp);
      this.groupContainer1.Controls.Add((Control) this.chkPrintFormsGroups);
      this.groupContainer1.Controls.Add((Control) this.chkDashboardReport);
      this.groupContainer1.Controls.Add((Control) this.chkLoanTemplate);
      this.groupContainer1.Controls.Add((Control) this.label3);
      this.groupContainer1.Controls.Add((Control) this.chkBoxViewSubordinateContacts);
      this.groupContainer1.Controls.Add((Control) this.cmbSuperiorAccess);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(544, 360);
      this.groupContainer1.TabIndex = 0;
      this.groupContainer1.Text = "Borrower Contacts/Public Loan Templates/Public Campaigns";
      this.chkCusLetterCampaignTemp.AutoSize = true;
      this.chkCusLetterCampaignTemp.Location = new Point(12, 129);
      this.chkCusLetterCampaignTemp.Name = "chkCusLetterCampaignTemp";
      this.chkCusLetterCampaignTemp.Size = new Size(293, 17);
      this.chkCusLetterCampaignTemp.TabIndex = 21;
      this.chkCusLetterCampaignTemp.Text = "Manage Public Custom Letters and Campaign Templates";
      this.chkCusLetterCampaignTemp.UseVisualStyleBackColor = true;
      this.chkCusLetterCampaignTemp.CheckedChanged += new EventHandler(this.chk_CheckedChanged);
      this.chkPrintFormsGroups.AutoSize = true;
      this.chkPrintFormsGroups.Location = new Point(12, 175);
      this.chkPrintFormsGroups.Name = "chkPrintFormsGroups";
      this.chkPrintFormsGroups.Size = new Size(274, 17);
      this.chkPrintFormsGroups.TabIndex = 20;
      this.chkPrintFormsGroups.Text = "Manage Public Custom Print Forms and Form Groups";
      this.chkPrintFormsGroups.UseVisualStyleBackColor = true;
      this.chkPrintFormsGroups.CheckedChanged += new EventHandler(this.chk_CheckedChanged);
      this.chkDashboardReport.AutoSize = true;
      this.chkDashboardReport.Location = new Point(12, 198);
      this.chkDashboardReport.Name = "chkDashboardReport";
      this.chkDashboardReport.Size = new Size(213, 17);
      this.chkDashboardReport.TabIndex = 19;
      this.chkDashboardReport.Text = "Manage Public Dashboard and Reports";
      this.chkDashboardReport.UseVisualStyleBackColor = true;
      this.chkDashboardReport.CheckedChanged += new EventHandler(this.chk_CheckedChanged);
      this.chkLoanTemplate.AutoSize = true;
      this.chkLoanTemplate.Location = new Point(12, 152);
      this.chkLoanTemplate.Name = "chkLoanTemplate";
      this.chkLoanTemplate.Size = new Size(176, 17);
      this.chkLoanTemplate.TabIndex = 18;
      this.chkLoanTemplate.Text = "Manage Public Loan Templates";
      this.chkLoanTemplate.UseVisualStyleBackColor = true;
      this.chkLoanTemplate.CheckedChanged += new EventHandler(this.chk_CheckedChanged);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(29, 77);
      this.label3.Name = "label3";
      this.label3.Size = new Size(73, 13);
      this.label3.TabIndex = 17;
      this.label3.Text = "Access Right:";
      this.chkBoxViewSubordinateContacts.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.chkBoxViewSubordinateContacts.Location = new Point(12, 40);
      this.chkBoxViewSubordinateContacts.Name = "chkBoxViewSubordinateContacts";
      this.chkBoxViewSubordinateContacts.Size = new Size(528, 34);
      this.chkBoxViewSubordinateContacts.TabIndex = 15;
      this.chkBoxViewSubordinateContacts.Text = "Borrower contacts created by this group are public and viewable by superiors in organization hierarchy.";
      this.chkBoxViewSubordinateContacts.CheckedChanged += new EventHandler(this.chkBoxViewSubordinateContacts_CheckedChanged);
      this.cmbSuperiorAccess.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbSuperiorAccess.Enabled = false;
      this.cmbSuperiorAccess.Location = new Point(108, 74);
      this.cmbSuperiorAccess.Name = "cmbSuperiorAccess";
      this.cmbSuperiorAccess.Size = new Size((int) sbyte.MaxValue, 21);
      this.cmbSuperiorAccess.TabIndex = 16;
      this.cmbSuperiorAccess.SelectedIndexChanged += new EventHandler(this.cmbSuperiorAccess_SelectedIndexChanged);
      this.chkPublicBizContact.AutoSize = true;
      this.chkPublicBizContact.Location = new Point(12, 106);
      this.chkPublicBizContact.Name = "chkPublicBizContact";
      this.chkPublicBizContact.Size = new Size(187, 17);
      this.chkPublicBizContact.TabIndex = 22;
      this.chkPublicBizContact.Text = "Manage Public Business Contacts";
      this.chkPublicBizContact.UseVisualStyleBackColor = true;
      this.chkPublicBizContact.CheckedChanged += new EventHandler(this.chk_CheckedChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoScroll = true;
      this.ClientSize = new Size(544, 360);
      this.Controls.Add((Control) this.groupContainer1);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (AccessPage);
      this.Text = nameof (AccessPage);
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
