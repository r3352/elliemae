// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.CSManagementDialog
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Calendar;
using EllieMae.EMLite.ClientServer.Events;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices.CS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class CSManagementDialog : Form
  {
    private static CSManagementDialog csManagementDialog;
    private ListViewSortManager sortContactMgr;
    private ListViewSortManager sortViewerMgr;
    private IContainer components;
    private Button btnRequest;
    private ListView lsvContact;
    private ColumnHeader chUserID;
    private ColumnHeader chLastName;
    private ColumnHeader chFirstName;
    private ColumnHeader chAccessLevel;
    private Button btnRemove;
    private Button btnAccess;
    private Button btnClose;
    private TabControl tcCS;
    private TabPage tabPageContact;
    private TabPage tabPageViewer;
    private ListView lsvViewer;
    private ColumnHeader chVUserID;
    private ColumnHeader chVLastName;
    private ColumnHeader chvFirstName;
    private ColumnHeader chVAccessLevel;
    private Button btnEdit;
    private Button btnVRemove;
    private Button btnVClose;
    private ContextMenuStrip cmsContact;
    private ToolStripMenuItem requestToolStripMenuItem;
    private ToolStripMenuItem removeToolStripMenuItem;
    private ToolStripMenuItem viewToolStripMenuItem;
    private ContextMenuStrip cmsViewer;
    private ToolStripMenuItem editToolStripMenuItem;
    private ToolStripMenuItem removeToolStripMenuItemViewer;
    private Button btnProfile;
    private Button btnViewerProfile;
    private ToolStripMenuItem viewProfileToolStripMenuItem;
    private ToolStripMenuItem viewViewerProfileToolStripMenuItem;
    private Label label1;
    private Label label2;

    private CSManagementDialog()
    {
      this.InitializeComponent();
      Session.ISession.RegisterForEvents(typeof (SessionEvent));
      this.refreshContacts(false);
      this.refreshViewer(false);
      this.ResetButtonState(this.tabPageContact);
      this.ResetButtonState(this.tabPageViewer);
    }

    internal static CSManagementDialog Instance => CSManagementDialog.csManagementDialog;

    internal void RequestContact()
    {
      if (this.InvokeRequired)
        this.Invoke((Delegate) new CSManagementDialog.RequestContactDelegate(this.RequestContact));
      else if (this.lsvContact.SelectedItems != null && this.lsvContact.SelectedItems.Count > 0)
      {
        UserInfo tag = (UserInfo) this.lsvContact.SelectedItems[0].Tag;
        string userid = tag.Userid;
        if (Session.CalendarManager.ContainsContact(userid, Session.UserID))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Contact '" + tag.FullName + "' has already been added to your Contact List.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
        }
        else
        {
          RequestCSDialog requestCsDialog = new RequestCSDialog(tag.FullName);
          if (requestCsDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return;
          CSControlMessage msg = new CSControlMessage(Session.UserID, userid, requestCsDialog.MessageType, requestCsDialog.Message);
          Session.CalendarManager.AddContact(userid, Session.UserID, CSMessage.AccessLevel.Pending, msg);
          Session.ServerManager.SendMessage((EllieMae.EMLite.ClientServer.Message) msg);
          this.refreshContacts(true);
        }
      }
      else
      {
        int num1 = (int) Utils.Dialog((IWin32Window) null, "Please select a user", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    internal void RemoveContact()
    {
      if (this.InvokeRequired)
        this.Invoke((Delegate) new CSManagementDialog.RemoveContactDelegate(this.RemoveContact));
      else if (this.lsvContact.SelectedItems == null || this.lsvContact.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) null, "Please select a user", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        UserInfo tag = (UserInfo) this.lsvContact.SelectedItems[0].Tag;
        foreach (CSContactInfo csContactInfo in Session.CalendarManager.GetCalenderSharingContactList(Session.UserID, false).GetCSContactInfoList())
        {
          if (csContactInfo.OwnerID == tag.Userid)
          {
            Session.CalendarManager.DeleteContact(csContactInfo.OwnerID, Session.UserID);
            Session.MainScreen.ShowCalendar((IWin32Window) this, csContactInfo.OwnerID, CSMessage.AccessLevel.Pending, true);
            if (csContactInfo.AccessLevel != CSMessage.AccessLevel.Pending)
            {
              Session.ServerManager.SendMessage((EllieMae.EMLite.ClientServer.Message) new CSControlMessage(Session.UserID, csContactInfo.OwnerID, CSMessage.MessageType.WithdrawAccess, ""));
              break;
            }
            break;
          }
        }
        this.refreshContacts(true);
      }
    }

    internal void ViewContact()
    {
      if (this.InvokeRequired)
        this.Invoke((Delegate) new CSManagementDialog.ViewContactDelegate(this.ViewContact));
      else if (this.lsvContact.SelectedItems == null || this.lsvContact.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) null, "Please select a user", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        UserInfo tag = (UserInfo) this.lsvContact.SelectedItems[0].Tag;
        CSMessage.AccessLevel accessLevel = CSMessage.AccessLevel.ReadOnly;
        switch (this.lsvContact.SelectedItems[0].SubItems[3].Text)
        {
          case "Partial":
            accessLevel = CSMessage.AccessLevel.Partial;
            break;
          case "Full":
            accessLevel = CSMessage.AccessLevel.Full;
            break;
        }
        Session.MainScreen.ShowCalendar((IWin32Window) this, tag.Userid, accessLevel, false);
        this.Close();
      }
    }

    internal void ViewContactProfile()
    {
      if (this.InvokeRequired)
      {
        this.Invoke((Delegate) new MethodInvoker(this.ViewContactProfile));
      }
      else
      {
        if (this.lsvContact.SelectedItems == null || this.lsvContact.SelectedItems.Count <= 0)
          return;
        int num = (int) new CSUserProfile((UserInfo) this.lsvContact.SelectedItems[0].Tag).ShowDialog((IWin32Window) this);
      }
    }

    internal void ViewViewerProfile()
    {
      if (this.InvokeRequired)
      {
        this.Invoke((Delegate) new MethodInvoker(this.ViewViewerProfile));
      }
      else
      {
        if (this.lsvViewer.SelectedItems == null || this.lsvViewer.SelectedItems.Count <= 0)
          return;
        int num = (int) new CSUserProfile(((CSContactInfo) this.lsvViewer.SelectedItems[0].Tag).UserID).ShowDialog((IWin32Window) this);
      }
    }

    internal void RemoveViewer()
    {
      if (this.InvokeRequired)
      {
        this.Invoke((Delegate) new MethodInvoker(this.RemoveViewer));
      }
      else
      {
        if (this.lsvViewer.SelectedItems == null || this.lsvViewer.SelectedItems.Count <= 0 || new EditCSAccess((CSContactInfo) this.lsvViewer.SelectedItems[0].Tag, true).ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.refreshViewer(true);
      }
    }

    internal void EditViewer()
    {
      if (this.InvokeRequired)
      {
        this.Invoke((Delegate) new MethodInvoker(this.EditViewer));
      }
      else
      {
        if (this.lsvViewer.SelectedItems == null || this.lsvViewer.SelectedItems.Count <= 0 || new EditCSAccess((CSContactInfo) this.lsvViewer.SelectedItems[0].Tag, false).ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.refreshViewer(true);
      }
    }

    public static void Start()
    {
      if (CSManagementDialog.csManagementDialog != null)
      {
        if (CSManagementDialog.csManagementDialog.InvokeRequired)
        {
          MethodInvoker method = new MethodInvoker(CSManagementDialog.Start);
          CSManagementDialog.csManagementDialog.Invoke((Delegate) method);
        }
        else
        {
          CSManagementDialog.csManagementDialog.WindowState = FormWindowState.Minimized;
          CSManagementDialog.csManagementDialog.WindowState = FormWindowState.Normal;
          CSManagementDialog.csManagementDialog.TopMost = true;
          CSManagementDialog.csManagementDialog.BringToFront();
          CSManagementDialog.csManagementDialog.Select();
          CSManagementDialog.csManagementDialog.TopMost = false;
        }
      }
      else
      {
        CSManagementDialog.csManagementDialog = new CSManagementDialog();
        CSManagementDialog.csManagementDialog.Show();
      }
    }

    public static void ProcessCSMessage(CSMessage imMsg)
    {
      if (!(imMsg is CSControlMessage))
        return;
      CSManagementDialog.Start();
      CSControlMessage csControlMessage = (CSControlMessage) imMsg;
      switch (csControlMessage.MsgType)
      {
        case CSMessage.MessageType.RequestReadOnlyCalendarAccess:
        case CSMessage.MessageType.RequestPartialCalendarAccess:
        case CSMessage.MessageType.RequestFullCalendarAccess:
          AddCSRequest addCsRequest = new AddCSRequest(csControlMessage);
          addCsRequest.TopMost = true;
          int num1 = (int) addCsRequest.ShowDialog((IWin32Window) CSManagementDialog.Instance);
          addCsRequest.TopMost = false;
          if (CSManagementDialog.Instance == null)
            break;
          CSManagementDialog.csManagementDialog.refreshViewer(true);
          CSManagementDialog.csManagementDialog.TopMost = true;
          break;
        case CSMessage.MessageType.AllowAddToList:
        case CSMessage.MessageType.DenyAddToList:
        case CSMessage.MessageType.ModifyAccess:
        case CSMessage.MessageType.DeleteAccess:
        case CSMessage.MessageType.WithdrawAccess:
          RequestAck requestAck = new RequestAck(csControlMessage);
          requestAck.TopMost = true;
          int num2 = (int) requestAck.ShowDialog((IWin32Window) CSManagementDialog.Instance);
          requestAck.TopMost = false;
          if (CSManagementDialog.Instance != null)
          {
            if (csControlMessage.MsgType == CSMessage.MessageType.WithdrawAccess)
              CSManagementDialog.csManagementDialog.refreshViewer(true);
            else
              CSManagementDialog.csManagementDialog.refreshContacts(true);
          }
          if (csControlMessage.MsgType != CSMessage.MessageType.DeleteAccess && csControlMessage.MsgType != CSMessage.MessageType.ModifyAccess)
            break;
          if (csControlMessage.MsgType == CSMessage.MessageType.DeleteAccess)
          {
            Session.MainScreen.ShowCalendar((IWin32Window) CSManagementDialog.csManagementDialog, csControlMessage.FromUser, CSMessage.AccessLevel.Pending, true);
            break;
          }
          foreach (CSContactInfo csContactInfo in Session.CalendarManager.GetCalenderSharingContactList(Session.UserID, false).GetCSContactInfoList())
          {
            if (csContactInfo.OwnerID == csControlMessage.FromUser)
            {
              Session.MainScreen.ShowCalendar((IWin32Window) CSManagementDialog.csManagementDialog, csControlMessage.FromUser, csContactInfo.AccessLevel, true);
              break;
            }
          }
          break;
      }
    }

    private static void threadStart()
    {
      CSManagementDialog.csManagementDialog = new CSManagementDialog();
      Application.Run((Form) CSManagementDialog.csManagementDialog);
    }

    private void refreshContacts(bool selectTab)
    {
      if (this.InvokeRequired)
      {
        this.Invoke((Delegate) new CSManagementDialog.ContactsViewerDelegate(this.refreshContacts), (object) selectTab);
      }
      else
      {
        this.lsvContact.Items.Clear();
        this.sortContactMgr = new ListViewSortManager(this.lsvContact, new System.Type[4]
        {
          typeof (ListViewTextCaseInsensitiveSort),
          typeof (ListViewTextCaseInsensitiveSort),
          typeof (ListViewTextCaseInsensitiveSort),
          typeof (ListViewTextCaseInsensitiveSort)
        });
        this.sortContactMgr.Sort(0);
        this.loadUsers();
        if (selectTab)
          this.tcCS.SelectedTab = this.tabPageContact;
        this.ResetButtonState(this.tabPageContact);
        this.TopMost = true;
      }
    }

    private void refreshViewer(bool selectTab)
    {
      if (this.InvokeRequired)
      {
        this.Invoke((Delegate) new CSManagementDialog.ContactsViewerDelegate(this.refreshViewer), (object) selectTab);
      }
      else
      {
        this.lsvViewer.Items.Clear();
        this.sortViewerMgr = new ListViewSortManager(this.lsvViewer, new System.Type[4]
        {
          typeof (ListViewTextCaseInsensitiveSort),
          typeof (ListViewTextCaseInsensitiveSort),
          typeof (ListViewTextCaseInsensitiveSort),
          typeof (ListViewTextCaseInsensitiveSort)
        });
        this.sortViewerMgr.Sort(0);
        this.loadViewer();
        if (selectTab)
          this.tcCS.SelectedTab = this.tabPageViewer;
        this.ResetButtonState(this.tabPageViewer);
        this.TopMost = true;
      }
    }

    private void loadUsers()
    {
      IOrganizationManager organizationManager = Session.OrganizationManager;
      Dictionary<string, UserInfo> dictionary1 = new Dictionary<string, UserInfo>();
      Dictionary<string, UserInfo> dictionary2 = new Dictionary<string, UserInfo>();
      FeaturesAclManager aclManager = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      string[] personaListByFeature = aclManager.GetPersonaListByFeature(new AclFeature[1]
      {
        AclFeature.GlobalTab_Contacts
      }, AclTriState.True);
      RoleInfo[] allRoleFunctions = ((WorkflowManager) Session.BPM.GetBpmManager(BpmCategory.Workflow)).GetAllRoleFunctions();
      List<string> stringList = new List<string>();
      stringList.AddRange((IEnumerable<string>) personaListByFeature);
      foreach (RoleInfo roleInfo in allRoleFunctions)
      {
        bool flag = false;
        UserInfo[] scopedUsersWithRole = Session.OrganizationManager.GetScopedUsersWithRole(roleInfo.RoleID);
        foreach (int personaId in roleInfo.PersonaIDs)
        {
          if (stringList.Contains(string.Concat((object) personaId)))
          {
            flag = true;
            break;
          }
        }
        foreach (UserInfo userInfo in scopedUsersWithRole)
        {
          if (flag && !dictionary1.ContainsKey(userInfo.Userid))
            dictionary1.Add(userInfo.Userid, userInfo);
          if (!dictionary2.ContainsKey(userInfo.Userid))
            dictionary2.Add(userInfo.Userid, userInfo);
        }
      }
      FeaturesAclManager featuresAclManager1 = aclManager;
      AclFeature[] features1 = new AclFeature[1]
      {
        AclFeature.GlobalTab_Contacts
      };
      foreach (string str in featuresAclManager1.GetUserListByFeature(features1, AclTriState.True))
      {
        if (dictionary2.ContainsKey(str) && !dictionary1.ContainsKey(str))
        {
          UserInfo user = Session.OrganizationManager.GetUser(str);
          dictionary1.Add(str, user);
        }
      }
      FeaturesAclManager featuresAclManager2 = aclManager;
      AclFeature[] features2 = new AclFeature[1]
      {
        AclFeature.GlobalTab_Contacts
      };
      foreach (string key in featuresAclManager2.GetUserListByFeature(features2, AclTriState.False))
      {
        if (dictionary1.ContainsKey(key))
          dictionary1.Remove(key);
      }
      foreach (UserInfo userInfo in Session.OrganizationManager.GetAccessibleUsersWithPersona(Persona.Administrator.ID, false))
      {
        if (!dictionary1.ContainsKey(userInfo.Userid))
          dictionary1.Add(userInfo.Userid, userInfo);
      }
      foreach (UserInfo userInfo in Session.OrganizationManager.GetAccessibleUsersWithPersona(Persona.SuperAdministrator.ID, false))
      {
        if (!dictionary1.ContainsKey(userInfo.Userid))
          dictionary1.Add(userInfo.Userid, userInfo);
      }
      UserInfo[] array = new UserInfo[dictionary1.Count];
      dictionary1.Values.CopyTo(array, 0);
      CSListInfo sharingContactList = Session.CalendarManager.GetCalenderSharingContactList(Session.UserID, false);
      if (array == null)
        return;
      this.sortContactMgr.Disable();
      this.lsvContact.BeginUpdate();
      for (int index = 0; index < array.Length; ++index)
      {
        if (array[index].Userid != Session.UserID)
        {
          ListViewItem listViewItem = new ListViewItem(array[index].Userid);
          listViewItem.Tag = (object) array[index];
          listViewItem.SubItems.Add(array[index].LastName);
          listViewItem.SubItems.Add(array[index].FirstName);
          string text = "";
          foreach (CSContactInfo csContactInfo in sharingContactList.GetCSContactInfoList())
          {
            if (csContactInfo.OwnerID == array[index].Userid)
            {
              switch (csContactInfo.AccessLevel)
              {
                case CSMessage.AccessLevel.ReadOnly:
                  text = "Read Only";
                  goto label_48;
                case CSMessage.AccessLevel.Partial:
                  text = "Partial";
                  goto label_48;
                case CSMessage.AccessLevel.Full:
                  text = "Full";
                  goto label_48;
                case CSMessage.AccessLevel.Pending:
                  text = "Pending";
                  goto label_48;
                default:
                  goto label_48;
              }
            }
          }
label_48:
          listViewItem.SubItems.Add(text);
          this.lsvContact.Items.Insert(this.lsvContact.Items.Count, listViewItem);
        }
      }
      this.sortContactMgr.Enable();
      this.lsvContact.EndUpdate();
    }

    private void loadViewer()
    {
      CSListInfo sharingContactList = Session.CalendarManager.GetCalenderSharingContactList(Session.UserID, true);
      this.lsvViewer.Items.Clear();
      if (sharingContactList == null || sharingContactList.ContactCount == 0)
        return;
      this.sortViewerMgr.Disable();
      this.lsvViewer.BeginUpdate();
      foreach (CSContactInfo csContactInfo in sharingContactList.GetCSContactInfoList())
      {
        ListViewItem listViewItem = new ListViewItem(csContactInfo.UserID);
        listViewItem.Tag = (object) csContactInfo;
        listViewItem.SubItems.Add(csContactInfo.UserLastName);
        listViewItem.SubItems.Add(csContactInfo.UserFirstName);
        string text = "";
        switch (csContactInfo.AccessLevel)
        {
          case CSMessage.AccessLevel.ReadOnly:
            text = "Read Only";
            break;
          case CSMessage.AccessLevel.Partial:
            text = "Partial";
            break;
          case CSMessage.AccessLevel.Full:
            text = "Full";
            break;
          case CSMessage.AccessLevel.Pending:
            text = "Pending";
            break;
        }
        listViewItem.SubItems.Add(text);
        this.lsvViewer.Items.Insert(this.lsvViewer.Items.Count, listViewItem);
      }
      this.sortViewerMgr.Enable();
      this.lsvViewer.EndUpdate();
    }

    private void ResetButtonState(TabPage selectedPage)
    {
      if (selectedPage == this.tabPageContact)
      {
        this.btnAccess.Enabled = false;
        this.btnRemove.Enabled = false;
        this.btnRequest.Enabled = false;
        this.btnProfile.Enabled = false;
        this.viewToolStripMenuItem.Enabled = false;
        this.removeToolStripMenuItem.Enabled = false;
        this.requestToolStripMenuItem.Enabled = false;
        this.viewProfileToolStripMenuItem.Enabled = false;
      }
      else
      {
        this.btnEdit.Enabled = false;
        this.btnVRemove.Enabled = false;
        this.btnViewerProfile.Enabled = false;
        this.editToolStripMenuItem.Enabled = false;
        this.removeToolStripMenuItemViewer.Enabled = false;
        this.viewViewerProfileToolStripMenuItem.Enabled = false;
      }
    }

    private void btnRequest_Click(object sender, EventArgs e) => this.RequestContact();

    private void btnRemove_Click(object sender, EventArgs e) => this.RemoveContact();

    private void btnAccess_Click(object sender, EventArgs e) => this.ViewContact();

    private void lsvContact_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.ResetButtonState(this.tabPageContact);
      if (this.lsvContact.SelectedItems == null || this.lsvContact.SelectedItems.Count <= 0)
        return;
      this.btnProfile.Enabled = true;
      this.viewProfileToolStripMenuItem.Enabled = true;
      switch (this.lsvContact.SelectedItems[0].SubItems[3].Text)
      {
        case "":
          this.btnRequest.Enabled = true;
          this.requestToolStripMenuItem.Enabled = true;
          break;
        case "Pending":
          this.btnRemove.Enabled = true;
          this.removeToolStripMenuItem.Enabled = true;
          break;
        case "Read Only":
        case "Partial":
        case "Full":
          this.btnAccess.Enabled = true;
          this.viewToolStripMenuItem.Enabled = true;
          this.btnRemove.Enabled = true;
          this.removeToolStripMenuItem.Enabled = true;
          break;
      }
    }

    private void lsvContact_DoubleClick(object sender, EventArgs e)
    {
      if (!this.btnAccess.Enabled)
        return;
      this.btnAccess_Click((object) null, (EventArgs) null);
    }

    private void btnClose_Click(object sender, EventArgs e) => this.Close();

    private void btnProfile_Click(object sender, EventArgs e) => this.ViewContactProfile();

    private void ContactToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if ((ToolStripMenuItem) sender == this.requestToolStripMenuItem)
        this.btnRequest_Click((object) null, (EventArgs) null);
      else if ((ToolStripMenuItem) sender == this.removeToolStripMenuItem)
        this.btnRemove_Click((object) null, (EventArgs) null);
      else if ((ToolStripMenuItem) sender == this.viewToolStripMenuItem)
      {
        this.btnAccess_Click((object) null, (EventArgs) null);
      }
      else
      {
        if ((ToolStripMenuItem) sender != this.viewProfileToolStripMenuItem)
          return;
        this.btnProfile_Click((object) null, (EventArgs) null);
      }
    }

    private void btnEdit_Click(object sender, EventArgs e) => this.EditViewer();

    private void lsvViewer_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.ResetButtonState(this.tabPageViewer);
      if (this.lsvViewer.SelectedItems != null && this.lsvViewer.SelectedItems.Count > 0)
      {
        this.btnEdit.Enabled = true;
        this.btnVRemove.Enabled = true;
        this.editToolStripMenuItem.Enabled = true;
        this.removeToolStripMenuItemViewer.Enabled = true;
        this.btnViewerProfile.Enabled = true;
        this.viewViewerProfileToolStripMenuItem.Enabled = true;
      }
      else
      {
        this.btnEdit.Enabled = false;
        this.btnVRemove.Enabled = false;
        this.editToolStripMenuItem.Enabled = false;
        this.removeToolStripMenuItemViewer.Enabled = false;
        this.btnViewerProfile.Enabled = false;
        this.viewViewerProfileToolStripMenuItem.Enabled = false;
      }
    }

    private void lsvViewer_DoubleClick(object sender, EventArgs e)
    {
      this.btnEdit_Click((object) null, (EventArgs) null);
    }

    private void btnVRemove_Click(object sender, EventArgs e) => this.RemoveViewer();

    private void btnVClose_Click(object sender, EventArgs e)
    {
      this.btnClose_Click((object) null, (EventArgs) null);
    }

    private void btnViewerProfile_Click(object sender, EventArgs e) => this.ViewViewerProfile();

    private void ViewerToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if ((ToolStripMenuItem) sender == this.editToolStripMenuItem)
        this.btnEdit_Click((object) null, (EventArgs) null);
      else if ((ToolStripMenuItem) sender == this.removeToolStripMenuItemViewer)
      {
        this.btnVRemove_Click((object) null, (EventArgs) null);
      }
      else
      {
        if ((ToolStripMenuItem) sender != this.viewViewerProfileToolStripMenuItem)
          return;
        this.btnViewerProfile_Click((object) null, (EventArgs) null);
      }
    }

    private void CSManagementDialog_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Escape)
        return;
      this.btnClose.PerformClick();
    }

    protected override void Dispose(bool disposing)
    {
      Session.ISession.UnregisterForEvents(typeof (SessionEvent));
      CSManagementDialog.csManagementDialog = (CSManagementDialog) null;
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (CSManagementDialog));
      this.btnRequest = new Button();
      this.lsvContact = new ListView();
      this.chUserID = new ColumnHeader();
      this.chLastName = new ColumnHeader();
      this.chFirstName = new ColumnHeader();
      this.chAccessLevel = new ColumnHeader();
      this.cmsContact = new ContextMenuStrip(this.components);
      this.viewToolStripMenuItem = new ToolStripMenuItem();
      this.viewProfileToolStripMenuItem = new ToolStripMenuItem();
      this.requestToolStripMenuItem = new ToolStripMenuItem();
      this.removeToolStripMenuItem = new ToolStripMenuItem();
      this.btnRemove = new Button();
      this.btnAccess = new Button();
      this.btnClose = new Button();
      this.tcCS = new TabControl();
      this.tabPageContact = new TabPage();
      this.label1 = new Label();
      this.btnProfile = new Button();
      this.tabPageViewer = new TabPage();
      this.label2 = new Label();
      this.btnViewerProfile = new Button();
      this.btnVClose = new Button();
      this.btnVRemove = new Button();
      this.btnEdit = new Button();
      this.lsvViewer = new ListView();
      this.chVUserID = new ColumnHeader();
      this.chVLastName = new ColumnHeader();
      this.chvFirstName = new ColumnHeader();
      this.chVAccessLevel = new ColumnHeader();
      this.cmsViewer = new ContextMenuStrip(this.components);
      this.viewViewerProfileToolStripMenuItem = new ToolStripMenuItem();
      this.editToolStripMenuItem = new ToolStripMenuItem();
      this.removeToolStripMenuItemViewer = new ToolStripMenuItem();
      this.cmsContact.SuspendLayout();
      this.tcCS.SuspendLayout();
      this.tabPageContact.SuspendLayout();
      this.tabPageViewer.SuspendLayout();
      this.cmsViewer.SuspendLayout();
      this.SuspendLayout();
      this.btnRequest.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnRequest.Location = new Point(157, 290);
      this.btnRequest.Name = "btnRequest";
      this.btnRequest.Size = new Size(75, 23);
      this.btnRequest.TabIndex = 0;
      this.btnRequest.Text = "Request";
      this.btnRequest.UseVisualStyleBackColor = true;
      this.btnRequest.Click += new EventHandler(this.btnRequest_Click);
      this.lsvContact.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lsvContact.Columns.AddRange(new ColumnHeader[4]
      {
        this.chUserID,
        this.chLastName,
        this.chFirstName,
        this.chAccessLevel
      });
      this.lsvContact.ContextMenuStrip = this.cmsContact;
      this.lsvContact.FullRowSelect = true;
      this.lsvContact.GridLines = true;
      this.lsvContact.HideSelection = false;
      this.lsvContact.Location = new Point(6, 20);
      this.lsvContact.MultiSelect = false;
      this.lsvContact.Name = "lsvContact";
      this.lsvContact.Size = new Size(487, 264);
      this.lsvContact.TabIndex = 1;
      this.lsvContact.UseCompatibleStateImageBehavior = false;
      this.lsvContact.View = View.Details;
      this.lsvContact.SelectedIndexChanged += new EventHandler(this.lsvContact_SelectedIndexChanged);
      this.lsvContact.DoubleClick += new EventHandler(this.lsvContact_DoubleClick);
      this.chUserID.Text = "User ID";
      this.chUserID.Width = 70;
      this.chLastName.Text = "Last Name";
      this.chLastName.Width = 173;
      this.chFirstName.Text = "First Name";
      this.chFirstName.Width = 160;
      this.chAccessLevel.Text = "Access Level";
      this.chAccessLevel.Width = 80;
      this.cmsContact.Items.AddRange(new ToolStripItem[4]
      {
        (ToolStripItem) this.viewToolStripMenuItem,
        (ToolStripItem) this.viewProfileToolStripMenuItem,
        (ToolStripItem) this.requestToolStripMenuItem,
        (ToolStripItem) this.removeToolStripMenuItem
      });
      this.cmsContact.Name = "cmsContact";
      this.cmsContact.Size = new Size(154, 92);
      this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
      this.viewToolStripMenuItem.Size = new Size(153, 22);
      this.viewToolStripMenuItem.Text = "View Calendar";
      this.viewToolStripMenuItem.Click += new EventHandler(this.ContactToolStripMenuItem_Click);
      this.viewProfileToolStripMenuItem.Name = "viewProfileToolStripMenuItem";
      this.viewProfileToolStripMenuItem.Size = new Size(153, 22);
      this.viewProfileToolStripMenuItem.Text = "View Profile";
      this.viewProfileToolStripMenuItem.Click += new EventHandler(this.ContactToolStripMenuItem_Click);
      this.requestToolStripMenuItem.Name = "requestToolStripMenuItem";
      this.requestToolStripMenuItem.Size = new Size(153, 22);
      this.requestToolStripMenuItem.Text = "Request";
      this.requestToolStripMenuItem.Click += new EventHandler(this.ContactToolStripMenuItem_Click);
      this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
      this.removeToolStripMenuItem.Size = new Size(153, 22);
      this.removeToolStripMenuItem.Text = "Remove";
      this.removeToolStripMenuItem.Click += new EventHandler(this.ContactToolStripMenuItem_Click);
      this.btnRemove.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnRemove.Location = new Point(76, 290);
      this.btnRemove.Name = "btnRemove";
      this.btnRemove.Size = new Size(75, 23);
      this.btnRemove.TabIndex = 2;
      this.btnRemove.Text = "Remove";
      this.btnRemove.UseVisualStyleBackColor = true;
      this.btnRemove.Click += new EventHandler(this.btnRemove_Click);
      this.btnAccess.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnAccess.Location = new Point(320, 290);
      this.btnAccess.Name = "btnAccess";
      this.btnAccess.Size = new Size(92, 23);
      this.btnAccess.TabIndex = 3;
      this.btnAccess.Text = "View Calendar";
      this.btnAccess.UseVisualStyleBackColor = true;
      this.btnAccess.Click += new EventHandler(this.btnAccess_Click);
      this.btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnClose.DialogResult = DialogResult.OK;
      this.btnClose.Location = new Point(418, 290);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 23);
      this.btnClose.TabIndex = 4;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.tcCS.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.tcCS.Controls.Add((Control) this.tabPageContact);
      this.tcCS.Controls.Add((Control) this.tabPageViewer);
      this.tcCS.Location = new Point(12, 12);
      this.tcCS.Name = "tcCS";
      this.tcCS.SelectedIndex = 0;
      this.tcCS.Size = new Size(507, 345);
      this.tcCS.TabIndex = 5;
      this.tabPageContact.Controls.Add((Control) this.label1);
      this.tabPageContact.Controls.Add((Control) this.btnProfile);
      this.tabPageContact.Controls.Add((Control) this.btnRequest);
      this.tabPageContact.Controls.Add((Control) this.lsvContact);
      this.tabPageContact.Controls.Add((Control) this.btnClose);
      this.tabPageContact.Controls.Add((Control) this.btnRemove);
      this.tabPageContact.Controls.Add((Control) this.btnAccess);
      this.tabPageContact.Location = new Point(4, 22);
      this.tabPageContact.Name = "tabPageContact";
      this.tabPageContact.Padding = new Padding(3);
      this.tabPageContact.Size = new Size(499, 319);
      this.tabPageContact.TabIndex = 0;
      this.tabPageContact.Text = "Contact";
      this.tabPageContact.UseVisualStyleBackColor = true;
      this.label1.AutoSize = true;
      this.label1.Dock = DockStyle.Top;
      this.label1.Location = new Point(3, 3);
      this.label1.Name = "label1";
      this.label1.Size = new Size(336, 13);
      this.label1.TabIndex = 6;
      this.label1.Text = "Use the Contact tab to manage your access to other users' calendars.";
      this.btnProfile.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnProfile.Location = new Point(238, 290);
      this.btnProfile.Name = "btnProfile";
      this.btnProfile.Size = new Size(75, 23);
      this.btnProfile.TabIndex = 5;
      this.btnProfile.Text = "View Profile";
      this.btnProfile.UseVisualStyleBackColor = true;
      this.btnProfile.Click += new EventHandler(this.btnProfile_Click);
      this.tabPageViewer.Controls.Add((Control) this.label2);
      this.tabPageViewer.Controls.Add((Control) this.btnViewerProfile);
      this.tabPageViewer.Controls.Add((Control) this.btnVClose);
      this.tabPageViewer.Controls.Add((Control) this.btnVRemove);
      this.tabPageViewer.Controls.Add((Control) this.btnEdit);
      this.tabPageViewer.Controls.Add((Control) this.lsvViewer);
      this.tabPageViewer.Location = new Point(4, 22);
      this.tabPageViewer.Name = "tabPageViewer";
      this.tabPageViewer.Padding = new Padding(3);
      this.tabPageViewer.Size = new Size(499, 319);
      this.tabPageViewer.TabIndex = 1;
      this.tabPageViewer.Text = "Viewer";
      this.tabPageViewer.UseVisualStyleBackColor = true;
      this.label2.AutoSize = true;
      this.label2.Dock = DockStyle.Top;
      this.label2.Location = new Point(3, 3);
      this.label2.Name = "label2";
      this.label2.Size = new Size(326, 13);
      this.label2.TabIndex = 5;
      this.label2.Text = "Use the Viewer tab to manage other users' access to your calendar.";
      this.btnViewerProfile.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnViewerProfile.Location = new Point(337, 293);
      this.btnViewerProfile.Name = "btnViewerProfile";
      this.btnViewerProfile.Size = new Size(75, 23);
      this.btnViewerProfile.TabIndex = 4;
      this.btnViewerProfile.Text = "View Profile";
      this.btnViewerProfile.UseVisualStyleBackColor = true;
      this.btnViewerProfile.Click += new EventHandler(this.btnViewerProfile_Click);
      this.btnVClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnVClose.Location = new Point(418, 293);
      this.btnVClose.Name = "btnVClose";
      this.btnVClose.Size = new Size(75, 23);
      this.btnVClose.TabIndex = 3;
      this.btnVClose.Text = "Close";
      this.btnVClose.UseVisualStyleBackColor = true;
      this.btnVClose.Click += new EventHandler(this.btnVClose_Click);
      this.btnVRemove.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnVRemove.Location = new Point(175, 293);
      this.btnVRemove.Name = "btnVRemove";
      this.btnVRemove.Size = new Size(75, 23);
      this.btnVRemove.TabIndex = 2;
      this.btnVRemove.Text = "Remove";
      this.btnVRemove.UseVisualStyleBackColor = true;
      this.btnVRemove.Click += new EventHandler(this.btnVRemove_Click);
      this.btnEdit.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnEdit.Location = new Point(256, 293);
      this.btnEdit.Name = "btnEdit";
      this.btnEdit.Size = new Size(75, 23);
      this.btnEdit.TabIndex = 1;
      this.btnEdit.Text = "Edit";
      this.btnEdit.UseVisualStyleBackColor = true;
      this.btnEdit.Click += new EventHandler(this.btnEdit_Click);
      this.lsvViewer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lsvViewer.Columns.AddRange(new ColumnHeader[4]
      {
        this.chVUserID,
        this.chVLastName,
        this.chvFirstName,
        this.chVAccessLevel
      });
      this.lsvViewer.ContextMenuStrip = this.cmsViewer;
      this.lsvViewer.FullRowSelect = true;
      this.lsvViewer.GridLines = true;
      this.lsvViewer.HideSelection = false;
      this.lsvViewer.Location = new Point(6, 19);
      this.lsvViewer.MultiSelect = false;
      this.lsvViewer.Name = "lsvViewer";
      this.lsvViewer.Size = new Size(487, 267);
      this.lsvViewer.TabIndex = 0;
      this.lsvViewer.UseCompatibleStateImageBehavior = false;
      this.lsvViewer.View = View.Details;
      this.lsvViewer.SelectedIndexChanged += new EventHandler(this.lsvViewer_SelectedIndexChanged);
      this.lsvViewer.DoubleClick += new EventHandler(this.lsvViewer_DoubleClick);
      this.chVUserID.Text = "User ID";
      this.chVUserID.Width = 70;
      this.chVLastName.Text = "Last Name";
      this.chVLastName.Width = 173;
      this.chvFirstName.Text = "First Name";
      this.chvFirstName.Width = 158;
      this.chVAccessLevel.Text = "Access Level";
      this.chVAccessLevel.Width = 80;
      this.cmsViewer.Items.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.viewViewerProfileToolStripMenuItem,
        (ToolStripItem) this.editToolStripMenuItem,
        (ToolStripItem) this.removeToolStripMenuItemViewer
      });
      this.cmsViewer.Name = "cmsViewer";
      this.cmsViewer.Size = new Size(141, 70);
      this.viewViewerProfileToolStripMenuItem.Name = "viewViewerProfileToolStripMenuItem";
      this.viewViewerProfileToolStripMenuItem.Size = new Size(140, 22);
      this.viewViewerProfileToolStripMenuItem.Text = "View Profile";
      this.viewViewerProfileToolStripMenuItem.Click += new EventHandler(this.ViewerToolStripMenuItem_Click);
      this.editToolStripMenuItem.Name = "editToolStripMenuItem";
      this.editToolStripMenuItem.Size = new Size(140, 22);
      this.editToolStripMenuItem.Text = "Edit";
      this.editToolStripMenuItem.Click += new EventHandler(this.ViewerToolStripMenuItem_Click);
      this.removeToolStripMenuItemViewer.Name = "removeToolStripMenuItemViewer";
      this.removeToolStripMenuItemViewer.Size = new Size(140, 22);
      this.removeToolStripMenuItemViewer.Text = "Remove";
      this.removeToolStripMenuItemViewer.Click += new EventHandler(this.ViewerToolStripMenuItem_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(528, 367);
      this.Controls.Add((Control) this.tcCS);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.KeyPreview = true;
      this.MinimizeBox = false;
      this.Name = nameof (CSManagementDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.Text = "Calendar Sharing";
      this.KeyUp += new KeyEventHandler(this.CSManagementDialog_KeyUp);
      this.cmsContact.ResumeLayout(false);
      this.tcCS.ResumeLayout(false);
      this.tabPageContact.ResumeLayout(false);
      this.tabPageContact.PerformLayout();
      this.tabPageViewer.ResumeLayout(false);
      this.tabPageViewer.PerformLayout();
      this.cmsViewer.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private delegate void RequestContactDelegate();

    private delegate void RemoveContactDelegate();

    private delegate void ViewContactDelegate();

    private delegate void ContactsViewerDelegate(bool selectTab);
  }
}
