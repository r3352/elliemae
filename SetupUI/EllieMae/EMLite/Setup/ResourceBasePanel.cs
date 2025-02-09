// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ResourceBasePanel
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ContactGroup;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class ResourceBasePanel : UserControl
  {
    private Sessions.Session session;
    private GroupResourceType resType;
    private AclFileType fileType = AclFileType.LoanProgram;
    private int _currentGroupId = -1;
    private string userID = "";
    private AclGroup[] groupList;
    private bool personal;
    private bool oldAccessIsViewOnly;
    private Hashtable printFormKeyToName = new Hashtable();
    private Hashtable bizGroupIdToName = new Hashtable();
    private List<AclFileResource> conditionalApprovalLetters = new List<AclFileResource>();
    private bool firstTime = true;
    private PrintFormSelectionDialog stdPrintFormDlg;
    private ConditionalLetterSelectionDialog condAppLetterDlg;
    private BizContactGroupSelectionDialog bizContactGroupDlg;
    private ChangeOfCircumstanceSelectionPage cocPage;
    private ResourceSetViewer resourceViewerDlg;
    private ListViewEx listView1;
    private IContainer components;
    private ColumnHeader columnHeaderName;
    private ColumnHeader columnHeaderAccessRight;
    private ImageList imgListTv;
    private ComboBox cmbBoxAccessRight;
    private bool dirty;
    private ToolTip toolTip1;
    private GroupContainer gcTemplate;
    private StandardIconButton stdIconBtnAdd;
    private StandardIconButton stdIconBtnDelete;
    private bool loadFromDB = true;

    public event EventHandler DirtyFlagChanged;

    public ResourceBasePanel(
      Sessions.Session session,
      int groupId,
      GroupResourceType resType,
      EventHandler dirtyFlagChanged)
    {
      this.DirtyFlagChanged += dirtyFlagChanged;
      this.InitializeComponent();
      this.session = session;
      this.resType = resType;
      this.init();
      if (groupId <= 0)
        return;
      this.currentGroupId = groupId;
    }

    public ResourceBasePanel(
      Sessions.Session session,
      string userID,
      AclGroup[] groups,
      GroupResourceType resType,
      EventHandler dirtyFlagChanged)
    {
      this.DirtyFlagChanged += dirtyFlagChanged;
      this.InitializeComponent();
      this.session = session;
      this.resType = resType;
      this.groupList = groups;
      this.userID = userID;
      this.personal = true;
      this.stdIconBtnAdd.Enabled = false;
      this.init();
      this.initialPageValue();
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ResourceBasePanel));
      this.listView1 = new ListViewEx();
      this.columnHeaderName = new ColumnHeader();
      this.columnHeaderAccessRight = new ColumnHeader();
      this.imgListTv = new ImageList(this.components);
      this.cmbBoxAccessRight = new ComboBox();
      this.toolTip1 = new ToolTip(this.components);
      this.gcTemplate = new GroupContainer();
      this.stdIconBtnAdd = new StandardIconButton();
      this.stdIconBtnDelete = new StandardIconButton();
      this.gcTemplate.SuspendLayout();
      ((ISupportInitialize) this.stdIconBtnAdd).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDelete).BeginInit();
      this.SuspendLayout();
      this.listView1.AllowColumnReorder = true;
      this.listView1.BorderStyle = BorderStyle.None;
      this.listView1.Columns.AddRange(new ColumnHeader[2]
      {
        this.columnHeaderName,
        this.columnHeaderAccessRight
      });
      this.listView1.Dock = DockStyle.Fill;
      this.listView1.DoubleClickActivation = false;
      this.listView1.FullRowSelect = true;
      this.listView1.GridLines = true;
      this.listView1.HideSelection = false;
      this.listView1.Location = new Point(1, 26);
      this.listView1.MultiSelect = false;
      this.listView1.Name = "listView1";
      this.listView1.Size = new Size(370, 200);
      this.listView1.SmallImageList = this.imgListTv;
      this.listView1.TabIndex = 3;
      this.listView1.UseCompatibleStateImageBehavior = false;
      this.listView1.View = View.Details;
      this.listView1.SubItemClicked += new SubItemEventHandler(this.listView1_SubItemClicked);
      this.listView1.SelectedIndexChanged += new EventHandler(this.listView1_SelectedIndexChanged);
      this.columnHeaderName.Text = "Name";
      this.columnHeaderName.Width = 182;
      this.columnHeaderAccessRight.Text = "Access Right";
      this.columnHeaderAccessRight.Width = 87;
      this.imgListTv.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgListTv.ImageStream");
      this.imgListTv.TransparentColor = Color.White;
      this.imgListTv.Images.SetKeyName(0, "members-this-group-and-below.png");
      this.imgListTv.Images.SetKeyName(1, "members-this-group.png");
      this.imgListTv.Images.SetKeyName(2, "member-group.png");
      this.imgListTv.Images.SetKeyName(3, "");
      this.imgListTv.Images.SetKeyName(4, "");
      this.imgListTv.Images.SetKeyName(5, "");
      this.imgListTv.Images.SetKeyName(6, "");
      this.cmbBoxAccessRight.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxAccessRight.Location = new Point(125, 3);
      this.cmbBoxAccessRight.Name = "cmbBoxAccessRight";
      this.cmbBoxAccessRight.Size = new Size(92, 21);
      this.cmbBoxAccessRight.TabIndex = 10;
      this.cmbBoxAccessRight.Visible = false;
      this.cmbBoxAccessRight.SelectedIndexChanged += new EventHandler(this.cmbBoxAccessRight_SelectedIndexChanged);
      this.toolTip1.AutomaticDelay = 300;
      this.gcTemplate.Controls.Add((Control) this.stdIconBtnAdd);
      this.gcTemplate.Controls.Add((Control) this.listView1);
      this.gcTemplate.Controls.Add((Control) this.stdIconBtnDelete);
      this.gcTemplate.Controls.Add((Control) this.cmbBoxAccessRight);
      this.gcTemplate.Dock = DockStyle.Fill;
      this.gcTemplate.Location = new Point(0, 0);
      this.gcTemplate.Name = "gcTemplate";
      this.gcTemplate.Size = new Size(372, 227);
      this.gcTemplate.TabIndex = 11;
      this.gcTemplate.Text = "<Template Name>";
      this.stdIconBtnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnAdd.BackColor = Color.Transparent;
      this.stdIconBtnAdd.Location = new Point(329, 5);
      this.stdIconBtnAdd.Name = "stdIconBtnAdd";
      this.stdIconBtnAdd.Size = new Size(16, 16);
      this.stdIconBtnAdd.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconBtnAdd.TabIndex = 12;
      this.stdIconBtnAdd.TabStop = false;
      this.stdIconBtnAdd.Click += new EventHandler(this.btnSelect_Click);
      this.stdIconBtnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDelete.BackColor = Color.Transparent;
      this.stdIconBtnDelete.Location = new Point(351, 5);
      this.stdIconBtnDelete.Name = "stdIconBtnDelete";
      this.stdIconBtnDelete.Size = new Size(16, 16);
      this.stdIconBtnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdIconBtnDelete.TabIndex = 11;
      this.stdIconBtnDelete.TabStop = false;
      this.stdIconBtnDelete.Click += new EventHandler(this.stdIconBtnDelete_Click);
      this.Controls.Add((Control) this.gcTemplate);
      this.Name = nameof (ResourceBasePanel);
      this.Size = new Size(372, 227);
      this.gcTemplate.ResumeLayout(false);
      ((ISupportInitialize) this.stdIconBtnAdd).EndInit();
      ((ISupportInitialize) this.stdIconBtnDelete).EndInit();
      this.ResumeLayout(false);
    }

    private void init()
    {
      this.fileType = this.resourceTypeToFileType();
      this.cmbBoxAccessRight.Items.AddRange((object[]) new string[2]
      {
        "View Only",
        "Edit"
      });
      switch (this.resType)
      {
        case GroupResourceType.LoanPrograms:
          this.gcTemplate.Text = "Loan Programs";
          break;
        case GroupResourceType.ClosingCosts:
          this.gcTemplate.Text = "Closing Costs";
          break;
        case GroupResourceType.DocumentSets:
          this.gcTemplate.Text = "Document Sets";
          break;
        case GroupResourceType.InputFormSets:
          this.gcTemplate.Text = "Input Form Sets";
          break;
        case GroupResourceType.SettlementServiceProviders:
          this.gcTemplate.Text = "Settlement Service Providers";
          break;
        case GroupResourceType.AffiliatedBusinessArrangements:
          this.gcTemplate.Text = "Affiliated Business Arrangement Templates";
          break;
        case GroupResourceType.MiscDataTemplates:
          this.gcTemplate.Text = "Misc. Data Templates";
          break;
        case GroupResourceType.LoanTemplateSets:
          this.gcTemplate.Text = "Loan Template Sets";
          break;
        case GroupResourceType.StandardPrintForms:
          this.gcTemplate.Text = "Standard Print Forms";
          this.buildPrintFormKeyToNameTable();
          this.columnHeaderName.Width += this.columnHeaderAccessRight.Width;
          this.listView1.Columns.Remove(this.columnHeaderAccessRight);
          this.listView1.StateImageList = (ImageList) null;
          break;
        case GroupResourceType.CustomPrintForms:
          this.gcTemplate.Text = "Custom Print Forms";
          break;
        case GroupResourceType.PrintGroups:
          this.gcTemplate.Text = "Print Form Groups";
          break;
        case GroupResourceType.BizContacts:
          this.gcTemplate.Text = "Public Business Contact Groups";
          this.buildBizGroupIdToNameTable();
          this.listView1.StateImageList = (ImageList) null;
          break;
        case GroupResourceType.BorrowerCustomLetters:
          this.gcTemplate.Text = "Borrower Custom Letters";
          break;
        case GroupResourceType.BusinessCustomLetters:
          this.gcTemplate.Text = "Business Custom Letters";
          break;
        case GroupResourceType.Reports:
          this.gcTemplate.Text = "Reports";
          break;
        case GroupResourceType.CampaignTemplates:
          this.gcTemplate.Text = "Campaign Templates";
          break;
        case GroupResourceType.DashboardTemplates:
          this.gcTemplate.Text = "Dashboard Templates";
          break;
        case GroupResourceType.DashboardViewTemplate:
          this.gcTemplate.Text = "Dashboard View Templates";
          break;
        case GroupResourceType.ConditionalApprovalLetter:
          this.gcTemplate.Text = "Condition Forms";
          this.buildConditionalApprovalLetterTable();
          this.columnHeaderName.Width += this.columnHeaderAccessRight.Width;
          this.listView1.Columns.Remove(this.columnHeaderAccessRight);
          this.listView1.StateImageList = (ImageList) null;
          break;
        case GroupResourceType.TaskSets:
          this.gcTemplate.Text = "Task Sets";
          break;
        case GroupResourceType.ChangeOfCircumstanceOptions:
          this.gcTemplate.Text = "Change of Circumstance Options";
          this.columnHeaderName.Text = "Description";
          this.columnHeaderAccessRight.Text = "LE or CD";
          this.listView1.SubItemClicked -= new SubItemEventHandler(this.listView1_SubItemClicked);
          break;
      }
      this.listView1_SelectedIndexChanged((object) this, (EventArgs) null);
    }

    private void setDirtyFlag(bool val)
    {
      this.dirty = val;
      if (this.DirtyFlagChanged == null)
        return;
      this.DirtyFlagChanged((object) this, (EventArgs) null);
    }

    private void buildBizGroupIdToNameTable()
    {
      foreach (ContactGroupInfo publicBizContactGroup in this.session.ContactGroupManager.GetPublicBizContactGroups())
        this.bizGroupIdToName.Add((object) publicBizContactGroup.GroupId, (object) publicBizContactGroup.GroupName);
    }

    private void buildPrintFormKeyToNameTable()
    {
      PrintFormList printFormList = PrintFormList.Parse(this.session.GetFormConfigFile(FormConfigFile.OutFormAndFileMapping), this.session.EncompassEdition);
      bool allowUrlA2020 = this.session.StartupInfo.AllowURLA2020;
      foreach (PrintForm printForm in printFormList)
      {
        if (allowUrlA2020 || !ShipInDarkValidation.IsURLA2020OutputForm(printForm.FormID))
          this.printFormKeyToName.Add((object) printForm.FormID, (object) printForm.UIName);
      }
    }

    private void buildConditionalApprovalLetterTable()
    {
      this.conditionalApprovalLetters = new List<AclFileResource>();
      FileSystemEntry[] templateDirEntries = this.session.ConfigurationManager.GetTemplateDirEntries(TemplateSettingsType.ConditionalLetter, new FileSystemEntry("\\", FileSystemEntry.Types.Folder, (string) null));
      if (templateDirEntries == null)
        return;
      foreach (FileSystemEntry fileSystemEntry in templateDirEntries)
      {
        if (fileSystemEntry.Type == FileSystemEntry.Types.File)
          this.conditionalApprovalLetters.Add(new AclFileResource(-1, fileSystemEntry.ToString(), AclFileType.ConditionalApprovalLetter, false, (string) null));
      }
      Dictionary<int, AclFileResource> dictionary = this.session.AclGroupManager.AddAclFileResources(this.conditionalApprovalLetters.ToArray());
      this.conditionalApprovalLetters.Clear();
      foreach (int key in dictionary.Keys)
        this.conditionalApprovalLetters.Add(dictionary[key]);
    }

    private AclFileType resourceTypeToFileType()
    {
      switch (this.resType)
      {
        case GroupResourceType.LoanPrograms:
          return AclFileType.LoanProgram;
        case GroupResourceType.ClosingCosts:
          return AclFileType.ClosingCost;
        case GroupResourceType.DocumentSets:
          return AclFileType.DocumentSet;
        case GroupResourceType.InputFormSets:
          return AclFileType.FormList;
        case GroupResourceType.SettlementServiceProviders:
          return AclFileType.SettlementServiceProviders;
        case GroupResourceType.AffiliatedBusinessArrangements:
          return AclFileType.AffiliatedBusinessArrangements;
        case GroupResourceType.MiscDataTemplates:
          return AclFileType.MiscData;
        case GroupResourceType.LoanTemplateSets:
          return AclFileType.LoanTemplate;
        case GroupResourceType.CustomPrintForms:
          return AclFileType.CustomPrintForms;
        case GroupResourceType.PrintGroups:
          return AclFileType.PrintGroups;
        case GroupResourceType.BorrowerCustomLetters:
          return AclFileType.BorrowerCustomLetters;
        case GroupResourceType.BusinessCustomLetters:
          return AclFileType.BizCustomLetters;
        case GroupResourceType.Reports:
          return AclFileType.Reports;
        case GroupResourceType.CampaignTemplates:
          return AclFileType.CampaignTemplate;
        case GroupResourceType.DashboardTemplates:
          return AclFileType.DashboardTemplate;
        case GroupResourceType.DashboardViewTemplate:
          return AclFileType.DashboardViewTemplate;
        case GroupResourceType.ConditionalApprovalLetter:
          return AclFileType.ConditionalApprovalLetter;
        case GroupResourceType.TaskSets:
          return AclFileType.TaskSet;
        case GroupResourceType.ChangeOfCircumstanceOptions:
          return AclFileType.ChangeOfCircumstanceOptions;
        default:
          return AclFileType.LoanProgram;
      }
    }

    public void SetGroup(int groupId) => this.currentGroupId = groupId;

    private int currentGroupId
    {
      get => this._currentGroupId;
      set => this.setCurrentGroupId(value);
    }

    private void setCurrentGroupId(int groupId)
    {
      if (this.firstTime)
      {
        if (this.resType == GroupResourceType.StandardPrintForms)
        {
          this.stdPrintFormDlg = (PrintFormSelectionDialog) null;
          this.setCurrentGroupIdForStdPrintForm(groupId);
          this.loadFromDB = true;
        }
        else if (this.resType == GroupResourceType.ConditionalApprovalLetter)
        {
          this.condAppLetterDlg = (ConditionalLetterSelectionDialog) null;
          this.setCurrentGroupIdForConditionalApprovalLetters(groupId);
          this.loadFromDB = true;
        }
        else if (this.resType == GroupResourceType.BizContacts)
        {
          this.setCurrentGroupIdForBizGroup(groupId);
          this.bizContactGroupDlg = (BizContactGroupSelectionDialog) null;
          this.loadFromDB = true;
        }
        else if (this.resType == GroupResourceType.ChangeOfCircumstanceOptions)
        {
          this.setCurrentGroupIdForChangeCircumstanceOptions(groupId);
          this.cocPage = (ChangeOfCircumstanceSelectionPage) null;
          this.loadFromDB = true;
        }
        else
        {
          this.setCurrentGroupIdForFileRefType(groupId);
          this.resourceViewerDlg = (ResourceSetViewer) null;
          this.loadFromDB = true;
        }
        this.setDirtyFlag(false);
      }
      else if (!this.firstTime && !this._currentGroupId.Equals(groupId))
      {
        this.firstTime = true;
        this.setDirtyFlag(false);
        if (this.resType == GroupResourceType.StandardPrintForms)
        {
          this.setCurrentGroupIdForStdPrintForm(groupId);
          this.stdPrintFormDlg = (PrintFormSelectionDialog) null;
          this.loadFromDB = true;
        }
        else if (this.resType == GroupResourceType.ConditionalApprovalLetter)
        {
          this.setCurrentGroupIdForConditionalApprovalLetters(groupId);
          this.condAppLetterDlg = (ConditionalLetterSelectionDialog) null;
          this.loadFromDB = true;
        }
        else if (this.resType == GroupResourceType.BizContacts)
        {
          this.setCurrentGroupIdForBizGroup(groupId);
          this.bizContactGroupDlg = (BizContactGroupSelectionDialog) null;
          this.loadFromDB = true;
        }
        else if (this.resType == GroupResourceType.ChangeOfCircumstanceOptions)
        {
          this.setCurrentGroupIdForChangeCircumstanceOptions(groupId);
          this.cocPage = (ChangeOfCircumstanceSelectionPage) null;
          this.loadFromDB = true;
        }
        else
        {
          this.setCurrentGroupIdForFileRefType(groupId);
          this.resourceViewerDlg = (ResourceSetViewer) null;
          this.loadFromDB = true;
        }
      }
      else if (!this.firstTime && this._currentGroupId.Equals(groupId))
        this.loadFromDB = this.resType != GroupResourceType.StandardPrintForms && this.resType != GroupResourceType.ConditionalApprovalLetter && this.resType != GroupResourceType.BizContacts && false;
      this.listView1_SelectedIndexChanged((object) this, (EventArgs) null);
    }

    private void setCurrentGroupIdForBizGroup(int groupId)
    {
      this._currentGroupId = groupId;
      this.listView1.Items.Clear();
      BizGroupRef[] contactGroupRefs = this.session.AclGroupManager.GetBizContactGroupRefs(this.currentGroupId);
      ArrayList arrayList = new ArrayList();
      for (int index = 0; index < contactGroupRefs.Length; ++index)
      {
        if (this.bizGroupIdToName.Contains((object) contactGroupRefs[index].BizGroupID))
        {
          string str = contactGroupRefs[index].Access == AclResourceAccess.ReadOnly ? "View Only" : "Edit";
          arrayList.Add((object) new ListViewItem(new string[2]
          {
            (string) this.bizGroupIdToName[(object) contactGroupRefs[index].BizGroupID],
            str
          })
          {
            Tag = (object) contactGroupRefs[index]
          });
        }
      }
      this.listView1.Items.AddRange((ListViewItem[]) arrayList.ToArray(typeof (ListViewItem)));
    }

    private void setCurrentGroupIdForBizGroup(BizGroupRef[] refs)
    {
      this.listView1.Items.Clear();
      ArrayList arrayList = new ArrayList();
      for (int index = 0; index < refs.Length; ++index)
      {
        if (this.bizGroupIdToName.Contains((object) refs[index].BizGroupID))
        {
          string str = refs[index].Access == AclResourceAccess.ReadOnly ? "View Only" : "Edit";
          arrayList.Add((object) new ListViewItem(new string[2]
          {
            (string) this.bizGroupIdToName[(object) refs[index].BizGroupID],
            str
          })
          {
            Tag = (object) refs[index]
          });
        }
      }
      this.listView1.Items.AddRange((ListViewItem[]) arrayList.ToArray(typeof (ListViewItem)));
    }

    private void setCurrentGroupListForBizGroup()
    {
      this.listView1.Items.Clear();
      Hashtable hashtable = new Hashtable();
      foreach (AclGroup group in this.groupList)
      {
        BizGroupRef[] contactGroupRefs = this.session.AclGroupManager.GetBizContactGroupRefs(group.ID);
        ArrayList arrayList = new ArrayList();
        for (int index = 0; index < contactGroupRefs.Length; ++index)
        {
          if (this.bizGroupIdToName.Contains((object) contactGroupRefs[index].BizGroupID))
          {
            string key = (string) this.bizGroupIdToName[(object) contactGroupRefs[index].BizGroupID];
            if (hashtable[(object) key] == null || ((ListViewItem) hashtable[(object) key]).SubItems[1].Text != "Edit")
            {
              string str = contactGroupRefs[index].Access == AclResourceAccess.ReadOnly ? "View Only" : "Edit";
              ListViewItem listViewItem = new ListViewItem(new string[2]
              {
                key,
                str
              });
              if (hashtable[(object) key] == null || ((ListViewItem) hashtable[(object) key]).SubItems[1].Text != str)
              {
                if (str == "Edit" && hashtable[(object) key] != null && ((ListViewItem) hashtable[(object) key]).SubItems[1].Text == "View Only")
                  hashtable.Remove((object) key);
                listViewItem.Tag = (object) contactGroupRefs[index];
                hashtable.Add((object) key, (object) listViewItem);
              }
            }
          }
        }
      }
      if (hashtable.Count <= 0)
        return;
      ArrayList arrayList1 = new ArrayList();
      IDictionaryEnumerator enumerator = hashtable.GetEnumerator();
      while (enumerator.MoveNext())
        arrayList1.Add((object) (ListViewItem) enumerator.Value);
      this.listView1.Items.AddRange((ListViewItem[]) arrayList1.ToArray(typeof (ListViewItem)));
    }

    private void setCurrentGroupIdForConditionalApprovalLetters(int groupId)
    {
      this._currentGroupId = groupId;
      this.listView1.Items.Clear();
      FileInGroup[] aclGroupFileRefs = this.session.AclGroupManager.GetAclGroupFileRefs(this.currentGroupId, AclFileType.ConditionalApprovalLetter);
      Hashtable hashtable = new Hashtable();
      for (int index = 0; index < aclGroupFileRefs.Length; ++index)
        hashtable.Add((object) aclGroupFileRefs[index].FileID, (object) aclGroupFileRefs[index]);
      int[] fileIds = new int[aclGroupFileRefs.Length];
      hashtable.Keys.CopyTo((Array) fileIds, 0);
      AclFileResource[] aclFileResources = this.session.AclGroupManager.GetAclFileResources(fileIds);
      List<ListViewItem> listViewItemList = new List<ListViewItem>();
      for (int index = 0; index < aclFileResources.Length; ++index)
        listViewItemList.Add(new ListViewItem(aclFileResources[index].FileName)
        {
          Tag = (object) aclFileResources[index]
        });
      this.listView1.Items.AddRange(listViewItemList.ToArray());
    }

    private void setCurrentGroupIdForStdPrintForm(int groupId)
    {
      this._currentGroupId = groupId;
      this.listView1.Items.Clear();
      string[] groupStdPrintForms = this.session.AclGroupManager.GetAclGroupStdPrintForms(this.currentGroupId);
      ArrayList arrayList = new ArrayList();
      for (int index = 0; index < groupStdPrintForms.Length; ++index)
      {
        if (this.printFormKeyToName.Contains((object) groupStdPrintForms[index]))
          arrayList.Add((object) new ListViewItem(new string[1]
          {
            (string) this.printFormKeyToName[(object) groupStdPrintForms[index]]
          })
          {
            Tag = (object) groupStdPrintForms[index]
          });
      }
      this.listView1.Items.AddRange((ListViewItem[]) arrayList.ToArray(typeof (ListViewItem)));
    }

    private void setCurrentGroupIdForStdPrintForm(string[] fileIds)
    {
      this.listView1.Items.Clear();
      ArrayList arrayList = new ArrayList();
      for (int index = 0; index < fileIds.Length; ++index)
      {
        if (this.printFormKeyToName.Contains((object) fileIds[index]))
          arrayList.Add((object) new ListViewItem(new string[1]
          {
            (string) this.printFormKeyToName[(object) fileIds[index]]
          })
          {
            Tag = (object) fileIds[index]
          });
      }
      this.listView1.Items.AddRange((ListViewItem[]) arrayList.ToArray(typeof (ListViewItem)));
    }

    private void setCurrentGroupIdForConditionalApprovalLetters(AclFileResource[] aclFileResource)
    {
      this.listView1.Items.Clear();
      List<ListViewItem> listViewItemList = new List<ListViewItem>();
      for (int index = 0; index < aclFileResource.Length; ++index)
        listViewItemList.Add(new ListViewItem(aclFileResource[index].FileName)
        {
          Tag = (object) aclFileResource[index]
        });
      this.listView1.Items.AddRange(listViewItemList.ToArray());
    }

    private void setCurrentGroupListForStdPrintForm()
    {
      this.listView1.Items.Clear();
      foreach (AclGroup group in this.groupList)
      {
        string[] groupStdPrintForms = this.session.AclGroupManager.GetAclGroupStdPrintForms(group.ID);
        ArrayList arrayList = new ArrayList();
        for (int index = 0; index < groupStdPrintForms.Length; ++index)
        {
          if (this.printFormKeyToName.Contains((object) groupStdPrintForms[index]))
          {
            ListViewItem listViewItem = new ListViewItem(new string[1]
            {
              (string) this.printFormKeyToName[(object) groupStdPrintForms[index]]
            });
            if (!this.listView1.Items.Contains(listViewItem))
            {
              listViewItem.Tag = (object) groupStdPrintForms[index];
              arrayList.Add((object) listViewItem);
            }
          }
        }
        this.listView1.Items.AddRange((ListViewItem[]) arrayList.ToArray(typeof (ListViewItem)));
      }
    }

    private void setCurrentGroupListForConditionalApprovalLetter()
    {
      this.listView1.Items.Clear();
      Dictionary<int, FileInGroup> dictionary = new Dictionary<int, FileInGroup>();
      foreach (AclGroup group in this.groupList)
      {
        FileInGroup[] aclGroupFileRefs = this.session.AclGroupManager.GetAclGroupFileRefs(group.ID, AclFileType.ConditionalApprovalLetter);
        for (int index = 0; index < aclGroupFileRefs.Length; ++index)
        {
          if (!dictionary.ContainsKey(aclGroupFileRefs[index].FileID))
            dictionary.Add(aclGroupFileRefs[index].FileID, aclGroupFileRefs[index]);
        }
      }
      int[] numArray = new int[dictionary.Count];
      dictionary.Keys.CopyTo(numArray, 0);
      AclFileResource[] aclFileResources = this.session.AclGroupManager.GetAclFileResources(numArray);
      List<ListViewItem> listViewItemList = new List<ListViewItem>();
      for (int index = 0; index < aclFileResources.Length; ++index)
        listViewItemList.Add(new ListViewItem(aclFileResources[index].FileName)
        {
          Tag = (object) aclFileResources[index]
        });
      this.listView1.Items.AddRange(listViewItemList.ToArray());
    }

    private void setCurrentGroupIdForChangeCircumstanceOptions(int groupId)
    {
      this._currentGroupId = groupId;
      this.setCurrentGroupIdForChangeCircumstanceOptions();
    }

    private void setCurrentGroupIdForChangeCircumstanceOptions()
    {
      this.listView1.Items.Clear();
      string[] options = this.session.AclGroupManager.GetAclGroupChangeCircumstanceOptions(this.currentGroupId);
      List<ChangeCircumstanceSettings> circumstanceSettings1 = this.session.ConfigurationManager.GetAllChangeCircumstanceSettings();
      ArrayList arrayList = new ArrayList();
      for (int i = 0; i < options.Length; i++)
      {
        ChangeCircumstanceSettings circumstanceSettings2 = circumstanceSettings1.FirstOrDefault<ChangeCircumstanceSettings>((Func<ChangeCircumstanceSettings, bool>) (x => x.optionId.ToString() == options[i]));
        if (circumstanceSettings2 != null)
          arrayList.Add((object) new ListViewItem(new string[2]
          {
            circumstanceSettings2.Description,
            circumstanceSettings2.CocType
          })
          {
            Tag = (object) circumstanceSettings2
          });
      }
      this.listView1.Items.AddRange((ListViewItem[]) arrayList.ToArray(typeof (ListViewItem)));
    }

    private void setCurrentGroupIdForChangeCircumstanceOptions(
      List<ChangeCircumstanceSettings> settings)
    {
      this.listView1.Items.Clear();
      ArrayList arrayList = new ArrayList();
      foreach (ChangeCircumstanceSettings setting in settings)
        arrayList.Add((object) new ListViewItem(new string[2]
        {
          setting.Description,
          setting.CocType
        })
        {
          Tag = (object) setting
        });
      this.listView1.Items.AddRange((ListViewItem[]) arrayList.ToArray(typeof (ListViewItem)));
    }

    private void setCurrentGroupIdForFileRefType(int groupId)
    {
      this._currentGroupId = groupId;
      this.listView1.Items.Clear();
      FileInGroup[] aclGroupFileRefs = this.session.AclGroupManager.GetAclGroupFileRefs(this.currentGroupId, this.fileType);
      Hashtable hashtable = new Hashtable();
      for (int index = 0; index < aclGroupFileRefs.Length; ++index)
        hashtable.Add((object) aclGroupFileRefs[index].FileID, (object) aclGroupFileRefs[index]);
      int[] fileIds = new int[aclGroupFileRefs.Length];
      hashtable.Keys.CopyTo((Array) fileIds, 0);
      AclFileResource[] aclFileResources = this.session.AclGroupManager.GetAclFileResources(fileIds);
      ListViewItem[] items = new ListViewItem[aclFileResources.Length];
      for (int index = 0; index < aclFileResources.Length; ++index)
      {
        string str = ((FileInGroup) hashtable[(object) aclFileResources[index].FileID]).Access == AclResourceAccess.ReadOnly ? "View Only" : "Edit";
        items[index] = new ListViewItem(new string[2]
        {
          this.ModifyPath(aclFileResources[index].FilePath),
          str
        });
        items[index].Tag = (object) aclFileResources[index];
        items[index].ImageIndex = !aclFileResources[index].IsFolder ? 2 : 0;
      }
      this.listView1.Items.AddRange(items);
    }

    private void setCurrentGroupIdForFileRefType(FileInGroup[] filesInGroup)
    {
      this.session.AclGroupManager.GetAclGroupFileRefs(this.currentGroupId, this.fileType);
      Hashtable hashtable = new Hashtable();
      for (int index = 0; index < filesInGroup.Length; ++index)
      {
        foreach (ListViewItem listViewItem in this.listView1.Items)
        {
          if (((AclFileResource) listViewItem.Tag).FileID == filesInGroup[index].FileID)
          {
            filesInGroup[index].Access = !(listViewItem.SubItems[1].Text == "Edit") ? AclResourceAccess.ReadOnly : AclResourceAccess.ReadWrite;
            break;
          }
        }
        hashtable.Add((object) filesInGroup[index].FileID, (object) filesInGroup[index]);
      }
      int[] fileIds = new int[filesInGroup.Length];
      hashtable.Keys.CopyTo((Array) fileIds, 0);
      AclFileResource[] aclFileResources = this.session.AclGroupManager.GetAclFileResources(fileIds);
      ListViewItem[] items = new ListViewItem[aclFileResources.Length];
      for (int index = 0; index < aclFileResources.Length; ++index)
      {
        string str = ((FileInGroup) hashtable[(object) aclFileResources[index].FileID]).Access == AclResourceAccess.ReadOnly ? "View Only" : "Edit";
        items[index] = new ListViewItem(new string[2]
        {
          this.ModifyPath(aclFileResources[index].FilePath),
          str
        });
        items[index].Tag = (object) aclFileResources[index];
        items[index].ImageIndex = !aclFileResources[index].IsFolder ? 2 : 0;
      }
      this.listView1.Items.Clear();
      this.listView1.Items.AddRange(items);
    }

    private void setCurrentGroupListForFileRefType()
    {
      this.listView1.Items.Clear();
      Hashtable hashtable1 = new Hashtable();
      foreach (AclGroup group in this.groupList)
      {
        FileInGroup[] aclGroupFileRefs = this.session.AclGroupManager.GetAclGroupFileRefs(group.ID, this.fileType);
        Hashtable hashtable2 = new Hashtable();
        for (int index = 0; index < aclGroupFileRefs.Length; ++index)
          hashtable2.Add((object) aclGroupFileRefs[index].FileID, (object) aclGroupFileRefs[index]);
        int[] fileIds = new int[aclGroupFileRefs.Length];
        hashtable2.Keys.CopyTo((Array) fileIds, 0);
        AclFileResource[] aclFileResources = this.session.AclGroupManager.GetAclFileResources(fileIds);
        for (int index = 0; index < aclFileResources.Length; ++index)
        {
          FileInGroup fileInGroup = (FileInGroup) hashtable2[(object) aclFileResources[index].FileID];
          if (hashtable1[(object) aclFileResources[index].FilePath] == null || ((ListViewItem) hashtable1[(object) aclFileResources[index].FilePath]).SubItems[1].Text != "Edit")
          {
            string str = fileInGroup.Access == AclResourceAccess.ReadOnly ? "View Only" : "Edit";
            ListViewItem listViewItem = new ListViewItem(new string[2]
            {
              this.ModifyPath(aclFileResources[index].FilePath),
              str
            });
            listViewItem.ImageIndex = !aclFileResources[index].IsFolder ? 2 : 0;
            listViewItem.Tag = (object) aclFileResources[index];
            if (hashtable1[(object) aclFileResources[index].FilePath] == null || ((ListViewItem) hashtable1[(object) aclFileResources[index].FilePath]).SubItems[1].Text != str)
            {
              if (str == "Edit" && hashtable1[(object) aclFileResources[index].FilePath] != null && ((ListViewItem) hashtable1[(object) aclFileResources[index].FilePath]).SubItems[1].Text == "View Only")
                hashtable1.Remove((object) aclFileResources[index].FilePath);
              listViewItem.ImageIndex = !aclFileResources[index].IsFolder ? 2 : 0;
              hashtable1.Add((object) aclFileResources[index].FilePath, (object) listViewItem);
            }
          }
        }
      }
      if (hashtable1.Count <= 0)
        return;
      ArrayList arrayList = new ArrayList();
      IDictionaryEnumerator enumerator = hashtable1.GetEnumerator();
      while (enumerator.MoveNext())
        arrayList.Add((object) (ListViewItem) enumerator.Value);
      this.listView1.Items.AddRange((ListViewItem[]) arrayList.ToArray(typeof (ListViewItem)));
    }

    private string ModifyPath(string currentPath)
    {
      int num = currentPath.IndexOf("Public:");
      return num > -1 ? currentPath.Substring(num + 7) : currentPath;
    }

    private void listView1_SubItemClicked(object sender, SubItemEventArgs e)
    {
      if (e.SubItem != 1 || this.personal)
        return;
      this.oldAccessIsViewOnly = e.Item.SubItems[1].Text == "View Only";
      this.listView1.StartEditing((Control) this.cmbBoxAccessRight, e.Item, e.SubItem);
    }

    public bool HasBeenModified()
    {
      return this.dirty || this.resourceViewerDlg != null && this.resourceViewerDlg.HasBeenModified() || this.bizContactGroupDlg != null && this.bizContactGroupDlg.HasBeenModified() || this.stdPrintFormDlg != null && this.stdPrintFormDlg.HasBeenModified() || this.condAppLetterDlg != null && this.condAppLetterDlg.HasBeenModified() || this.cocPage != null && this.cocPage.HasBeenModified();
    }

    private void initialPageValue()
    {
      if (this.resType == GroupResourceType.StandardPrintForms)
        this.setCurrentGroupListForStdPrintForm();
      else if (this.resType == GroupResourceType.ConditionalApprovalLetter)
        this.setCurrentGroupListForConditionalApprovalLetter();
      else if (this.resType == GroupResourceType.BizContacts)
        this.setCurrentGroupListForBizGroup();
      else if (this.resType == GroupResourceType.ChangeOfCircumstanceOptions)
        this.setCurrentGroupIdForChangeCircumstanceOptions();
      else
        this.setCurrentGroupListForFileRefType();
    }

    public void SaveData()
    {
      if (!this.firstTime)
      {
        if (this.resType == GroupResourceType.StandardPrintForms && this.stdPrintFormDlg != null)
          this.stdPrintFormDlg.SaveData();
        else if (this.resType == GroupResourceType.ConditionalApprovalLetter && this.condAppLetterDlg != null)
          this.condAppLetterDlg.SaveData();
        else if (this.resType == GroupResourceType.BizContacts && this.bizContactGroupDlg != null)
          this.bizContactGroupDlg.SaveData();
        else if (this.resType == GroupResourceType.ChangeOfCircumstanceOptions && this.cocPage != null)
          this.cocPage.SaveData();
        else if (this.resourceViewerDlg != null)
          this.resourceViewerDlg.Save();
      }
      this.UpdateAccessRight();
      this.setDirtyFlag(false);
    }

    private void UpdateAccessRight()
    {
      foreach (ListViewItem listViewItem in this.listView1.Items)
      {
        bool flag = listViewItem.SubItems.Count > 1 && listViewItem.SubItems[1].Text == "View Only";
        if (listViewItem.Tag is AclFileResource)
        {
          FileInGroup aclGroupFileRef = this.session.AclGroupManager.GetAclGroupFileRef(this.currentGroupId, ((AclFileResource) listViewItem.Tag).FileID);
          if (aclGroupFileRef != (FileInGroup) null)
          {
            aclGroupFileRef.Access = flag ? AclResourceAccess.ReadOnly : AclResourceAccess.ReadWrite;
            this.session.AclGroupManager.UpdateAclGroupFileRef(this.currentGroupId, aclGroupFileRef);
          }
        }
        else if ((object) (listViewItem.Tag as BizGroupRef) != null)
        {
          BizGroupRef tag = (BizGroupRef) listViewItem.Tag;
          tag.Access = flag ? AclResourceAccess.ReadOnly : AclResourceAccess.ReadWrite;
          this.session.AclGroupManager.UpdateBizContactGroupRef(this.currentGroupId, tag);
        }
      }
    }

    public void ResetDate()
    {
      this.firstTime = true;
      this.currentGroupId = this.currentGroupId;
    }

    private FileInGroup[] getFileSystemSource(TreeView tv)
    {
      ArrayList files = new ArrayList();
      ArrayList folders = new ArrayList();
      ArrayList inclusiveOfFolders = new ArrayList();
      foreach (TreeNode node in tv.Nodes)
        this.saveCheckedNodeToGroup(this.fileType, node, folders, files, inclusiveOfFolders);
      int[] array1 = (int[]) new ArrayList((ICollection) this.session.AclGroupManager.AddAclFileResources((AclFileResource[]) folders.ToArray(typeof (AclFileResource))).Keys).ToArray(typeof (int));
      int[] array2 = (int[]) new ArrayList((ICollection) this.session.AclGroupManager.AddAclFileResources((AclFileResource[]) files.ToArray(typeof (AclFileResource))).Keys).ToArray(typeof (int));
      int length = array1.Length;
      FileInGroup[] fileSystemSource = new FileInGroup[length + array2.Length];
      for (int index = 0; index < array1.Length; ++index)
        fileSystemSource[index] = new FileInGroup()
        {
          FileID = array1[index],
          IsInclusive = (bool) inclusiveOfFolders[index],
          Access = AclResourceAccess.ReadOnly
        };
      for (int index = 0; index < array2.Length; ++index)
        fileSystemSource[length + index] = new FileInGroup()
        {
          FileID = array2[index],
          IsInclusive = false,
          Access = AclResourceAccess.ReadOnly
        };
      return fileSystemSource;
    }

    private void saveCheckedNodeToGroup(
      AclFileType fileType,
      TreeNode node,
      ArrayList folders,
      ArrayList files,
      ArrayList inclusiveOfFolders)
    {
      FileSystemEntry tag = (FileSystemEntry) node.Tag;
      if (node.ImageIndex == 0)
      {
        folders.Add((object) new AclFileResource(-1, tag.ToString(), fileType, true, tag.Owner));
        inclusiveOfFolders.Add((object) true);
      }
      else if (node.ImageIndex == 1)
      {
        folders.Add((object) new AclFileResource(-1, tag.ToString(), fileType, true, tag.Owner));
        inclusiveOfFolders.Add((object) false);
      }
      else if (node.ImageIndex == 2)
        files.Add((object) new AclFileResource(-1, tag.ToString(), fileType, false, tag.Owner));
      foreach (TreeNode node1 in node.Nodes)
        this.saveCheckedNodeToGroup(fileType, node1, folders, files, inclusiveOfFolders);
    }

    private AclFileResource[] GetResetFileRefTypeTreeView()
    {
      ArrayList arrayList = new ArrayList();
      foreach (ListViewItem listViewItem in this.listView1.Items)
        arrayList.Add((object) (AclFileResource) listViewItem.Tag);
      return (AclFileResource[]) arrayList.ToArray(typeof (AclFileResource));
    }

    private void ResetBizGroupView()
    {
      ArrayList arrayList = new ArrayList();
      foreach (ListViewItem listViewItem in this.listView1.Items)
      {
        if (listViewItem.SubItems[1].Text == "Edit")
          ((BizGroupRef) listViewItem.Tag).Access = AclResourceAccess.ReadWrite;
        else
          ((BizGroupRef) listViewItem.Tag).Access = AclResourceAccess.ReadOnly;
        arrayList.Add((object) (BizGroupRef) listViewItem.Tag);
      }
      this.bizContactGroupDlg.SetTempState((BizGroupRef[]) arrayList.ToArray(typeof (BizGroupRef)));
    }

    private void ResetStdFormView()
    {
      ArrayList arrayList = new ArrayList();
      foreach (ListViewItem listViewItem in this.listView1.Items)
        arrayList.Add((object) (string) listViewItem.Tag);
      this.stdPrintFormDlg.SetTempState((string[]) arrayList.ToArray(typeof (string)));
    }

    private void ResetConditionalApprovalView()
    {
      List<AclFileResource> fileList = new List<AclFileResource>();
      foreach (ListViewItem listViewItem in this.listView1.Items)
        fileList.Add((AclFileResource) listViewItem.Tag);
      this.condAppLetterDlg.SetTempState(fileList);
    }

    private void ResetChangeCircumstanceView()
    {
      List<ChangeCircumstanceSettings> settings = new List<ChangeCircumstanceSettings>();
      foreach (ListViewItem listViewItem in this.listView1.Items)
        settings.Add((ChangeCircumstanceSettings) listViewItem.Tag);
      this.cocPage.SetTempState(settings);
    }

    private void listView1_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.personal)
        this.stdIconBtnDelete.Enabled = false;
      else if (this.listView1.SelectedItems != null && this.listView1.SelectedItems.Count > 0)
      {
        this.toolTip1.SetToolTip((Control) this.listView1, this.listView1.SelectedItems[0].Text);
        this.stdIconBtnDelete.Enabled = true;
      }
      else
      {
        this.toolTip1.RemoveAll();
        this.stdIconBtnDelete.Enabled = false;
      }
    }

    private void cmbBoxAccessRight_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.setDirtyFlag(true);
      this.listView1.SelectedItems[0].SubItems[1].Text = this.cmbBoxAccessRight.Text;
    }

    private void btnSelect_Click(object sender, EventArgs e)
    {
      this.listView1.EndEditing(true);
      if (this.loadFromDB)
      {
        if (this.resType == GroupResourceType.StandardPrintForms)
          this.stdPrintFormDlg = new PrintFormSelectionDialog(this.session, this.currentGroupId, this.DirtyFlagChanged);
        else if (this.resType == GroupResourceType.ConditionalApprovalLetter)
          this.condAppLetterDlg = new ConditionalLetterSelectionDialog(this.session, this.currentGroupId, this.conditionalApprovalLetters, this.DirtyFlagChanged);
        else if (this.resType == GroupResourceType.BizContacts)
          this.bizContactGroupDlg = new BizContactGroupSelectionDialog(this.session, this.currentGroupId, this.DirtyFlagChanged);
        else if (this.resType == GroupResourceType.ChangeOfCircumstanceOptions)
        {
          this.cocPage = new ChangeOfCircumstanceSelectionPage(this.session, this.currentGroupId, this.DirtyFlagChanged);
        }
        else
        {
          this.resourceViewerDlg = new ResourceSetViewer(this.session, (object) new FileSystemResourceSet(this.currentGroupId, this.fileType, this.session.AclGroupManager.GetAclFileResources(this.session.AclGroupManager.GetAclGroupFileRefIDs(this.currentGroupId, this.fileType))), this.DirtyFlagChanged, this._currentGroupId);
          this.resourceViewerDlg.SetResetDataSourceAclFileResource = this.GetResetFileRefTypeTreeView();
        }
      }
      if (this.resType == GroupResourceType.StandardPrintForms)
      {
        if (DialogResult.Cancel == this.stdPrintFormDlg.ShowDialog((IWin32Window) this))
          this.ResetStdFormView();
        else
          this.setCurrentGroupIdForStdPrintForm(this.stdPrintFormDlg.getCurrentSelectedFiles());
      }
      else if (this.resType == GroupResourceType.ConditionalApprovalLetter)
      {
        if (DialogResult.Cancel == this.condAppLetterDlg.ShowDialog((IWin32Window) this))
          this.ResetConditionalApprovalView();
        else
          this.setCurrentGroupIdForConditionalApprovalLetters(this.condAppLetterDlg.GetSelectedFileID());
      }
      else if (this.resType == GroupResourceType.BizContacts)
      {
        if (DialogResult.Cancel != this.bizContactGroupDlg.ShowDialog((IWin32Window) this))
          this.setCurrentGroupIdForBizGroup(this.bizContactGroupDlg.getCurrentSelection());
        else
          this.ResetBizGroupView();
      }
      else if (this.resType == GroupResourceType.ChangeOfCircumstanceOptions)
      {
        if (DialogResult.Cancel != this.cocPage.ShowDialog((IWin32Window) this))
          this.setCurrentGroupIdForChangeCircumstanceOptions(this.cocPage.getCurrentSelectedFiles());
        else
          this.ResetChangeCircumstanceView();
      }
      else if (DialogResult.Cancel != this.resourceViewerDlg.ShowDialog((IWin32Window) this))
      {
        this.setCurrentGroupIdForFileRefType(this.getFileSystemSource((TreeView) this.resourceViewerDlg.GetCurrentTreeView()));
        this.resourceViewerDlg.SetResetDataSourceAclFileResource = this.GetResetFileRefTypeTreeView();
      }
      this.firstTime = false;
      this.loadFromDB = false;
    }

    private void stdIconBtnDelete_Click(object sender, EventArgs e)
    {
      this.listView1.EndEditing(false);
      if (this.listView1.SelectedItems == null || this.listView1.SelectedItems.Count == 0)
        return;
      if (this.loadFromDB)
      {
        if (this.resType == GroupResourceType.StandardPrintForms)
          this.stdPrintFormDlg = new PrintFormSelectionDialog(this.session, this.currentGroupId, this.DirtyFlagChanged);
        else if (this.resType == GroupResourceType.ConditionalApprovalLetter)
          this.condAppLetterDlg = new ConditionalLetterSelectionDialog(this.session, this.currentGroupId, this.conditionalApprovalLetters, this.DirtyFlagChanged);
        else if (this.resType == GroupResourceType.BizContacts)
          this.bizContactGroupDlg = new BizContactGroupSelectionDialog(this.session, this.currentGroupId, this.DirtyFlagChanged);
        else if (this.resType == GroupResourceType.ChangeOfCircumstanceOptions)
        {
          this.cocPage = new ChangeOfCircumstanceSelectionPage(this.session, this.currentGroupId, this.DirtyFlagChanged);
        }
        else
        {
          this.resourceViewerDlg = new ResourceSetViewer(this.session, (object) new FileSystemResourceSet(this.currentGroupId, this.fileType, this.session.AclGroupManager.GetAclFileResources(this.session.AclGroupManager.GetAclGroupFileRefIDs(this.currentGroupId, this.fileType))), this.DirtyFlagChanged, this._currentGroupId);
          this.resourceViewerDlg.SetResetDataSourceAclFileResource = this.GetResetFileRefTypeTreeView();
        }
      }
      this.firstTime = false;
      this.loadFromDB = false;
      if (this.resType == GroupResourceType.StandardPrintForms)
      {
        foreach (ListViewItem selectedItem in this.listView1.SelectedItems)
          selectedItem.Remove();
        this.ResetStdFormView();
      }
      else if (this.resType == GroupResourceType.ConditionalApprovalLetter)
      {
        foreach (ListViewItem selectedItem in this.listView1.SelectedItems)
          selectedItem.Remove();
        this.ResetConditionalApprovalView();
      }
      else if (this.resType == GroupResourceType.BizContacts)
      {
        foreach (ListViewItem selectedItem in this.listView1.SelectedItems)
          selectedItem.Remove();
        this.ResetBizGroupView();
      }
      else if (this.resType == GroupResourceType.ChangeOfCircumstanceOptions)
      {
        foreach (ListViewItem selectedItem in this.listView1.SelectedItems)
          selectedItem.Remove();
        this.ResetChangeCircumstanceView();
      }
      else
      {
        List<TreeNode> treeNodeList = new List<TreeNode>();
        foreach (ListViewItem selectedItem in this.listView1.SelectedItems)
        {
          AclFileResource tag = (AclFileResource) selectedItem.Tag;
          FileSystemEntry fileSystemEntry = !tag.IsFolder ? new FileSystemEntry(tag.FilePath.Replace("Public:", ""), FileSystemEntry.Types.File, (string) null) : new FileSystemEntry(tag.FilePath.Replace("Public:", ""), FileSystemEntry.Types.Folder, (string) null);
          treeNodeList.Add(new TreeNode(fileSystemEntry.Name)
          {
            Tag = (object) fileSystemEntry
          });
          selectedItem.Remove();
        }
        this.resourceViewerDlg.SetResetDataSourceAclFileResource = this.GetResetFileRefTypeTreeView();
        foreach (TreeNode node in treeNodeList)
          this.resourceViewerDlg.RemoveTreeNode(node);
      }
      this.setDirtyFlag(true);
    }
  }
}
