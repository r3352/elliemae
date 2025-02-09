// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.EncompassMessenger
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Events;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class EncompassMessenger : Form
  {
    private static EncompassMessenger encompassMessenger = (EncompassMessenger) null;
    private static Color activeColor = Color.Blue;
    private static Color inactiveColor = Color.Gray;
    private IMainScreen mainScreen;
    private SessionInfo[] sessions;
    private TreeView tvwMessenger;
    private ContextMenu contextMenu1;
    private MenuItem contextmenuAddContact;
    private MenuItem contextmenuAddGroup;
    private MenuItem contextmenuRenameGroup;
    private MenuItem contextmenuRemove;
    private MenuItem contextmenuSend;
    private MenuItem contextmenuProfile;
    private ImageList imageList1;
    private MenuItem menuItemRefresh;
    private MenuItem contextmenuDivid1;
    private MenuItem contextmenuDivid2;
    private MenuItem contextmenuDivid3;
    private MenuItem contextmenuDivid4;
    private Label lblNodeName;
    private RichTextBox rtBoxInit;
    private GradientMenuStrip gradMenuStripMain;
    private ToolStripMenuItem tsMenuItemMessenger;
    private ToolStripMenuItem tsMenuItemSend;
    private ToolStripMenuItem tsMenuItemClose;
    private ToolStripMenuItem tsMenuItemView;
    private ToolStripMenuItem tsMenuItemRefresh;
    private ToolStripMenuItem tsMenuItemContacts;
    private ToolStripMenuItem tsMenuItemHelp;
    private ToolStripMenuItem tsMenuItemEncHelp;
    private ToolStripMenuItem tsMenuItemAddContact;
    private ToolStripMenuItem tsMenuItemContactDetails;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripMenuItem tsMenuItemCreateNewGroup;
    private ToolStripMenuItem tsMenuItemRenameGroup;
    private ToolStripMenuItem tsMenuItemDelete;
    private ToolStripSeparator toolStripSeparator2;
    private IContainer components;
    private TreeNode dragNode;
    private int selectedGroupIndex = -1;

    private EncompassMessenger(IMainScreen mainScreen)
    {
      if (mainScreen != null)
        this.mainScreen = mainScreen;
      this.InitializeComponent();
      Font font1 = new Font(FontFamily.GenericSansSerif, 8f, FontStyle.Regular);
      Font font2 = new Font(FontFamily.GenericSansSerif, 8f, FontStyle.Bold);
      this.rtBoxInit.Clear();
      this.rtBoxInit.Select(this.rtBoxInit.Text.Length, 0);
      this.rtBoxInit.SelectionFont = font1;
      this.rtBoxInit.AppendText("\nTo add people to your Messenger List, click ");
      this.rtBoxInit.Select(this.rtBoxInit.Text.Length, 0);
      this.rtBoxInit.SelectionFont = font2;
      this.rtBoxInit.AppendText("Add Contact");
      this.rtBoxInit.Select(this.rtBoxInit.Text.Length, 0);
      this.rtBoxInit.SelectionFont = font1;
      this.rtBoxInit.AppendText(" on the ");
      this.rtBoxInit.Select(this.rtBoxInit.Text.Length, 0);
      this.rtBoxInit.SelectionFont = font2;
      this.rtBoxInit.AppendText("Contacts");
      this.rtBoxInit.Select(this.rtBoxInit.Text.Length, 0);
      this.rtBoxInit.SelectionFont = font1;
      this.rtBoxInit.AppendText(" menu.\n\nFor more help, press F1.");
      Session.ISession.RegisterForEvents(typeof (SessionEvent));
      this.Text = "Encompass Messenger";
      this.refreshContacts();
    }

    protected override void Dispose(bool disposing)
    {
      Session.ISession.UnregisterForEvents(typeof (SessionEvent));
      EncompassMessenger.encompassMessenger = (EncompassMessenger) null;
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (EncompassMessenger));
      this.tvwMessenger = new TreeView();
      this.contextMenu1 = new ContextMenu();
      this.contextmenuSend = new MenuItem();
      this.contextmenuDivid1 = new MenuItem();
      this.contextmenuAddContact = new MenuItem();
      this.contextmenuProfile = new MenuItem();
      this.contextmenuDivid4 = new MenuItem();
      this.contextmenuAddGroup = new MenuItem();
      this.contextmenuRenameGroup = new MenuItem();
      this.contextmenuDivid2 = new MenuItem();
      this.contextmenuRemove = new MenuItem();
      this.contextmenuDivid3 = new MenuItem();
      this.menuItemRefresh = new MenuItem();
      this.imageList1 = new ImageList(this.components);
      this.lblNodeName = new Label();
      this.rtBoxInit = new RichTextBox();
      this.gradMenuStripMain = new GradientMenuStrip();
      this.tsMenuItemMessenger = new ToolStripMenuItem();
      this.tsMenuItemSend = new ToolStripMenuItem();
      this.tsMenuItemClose = new ToolStripMenuItem();
      this.tsMenuItemView = new ToolStripMenuItem();
      this.tsMenuItemRefresh = new ToolStripMenuItem();
      this.tsMenuItemContacts = new ToolStripMenuItem();
      this.tsMenuItemAddContact = new ToolStripMenuItem();
      this.tsMenuItemContactDetails = new ToolStripMenuItem();
      this.toolStripSeparator1 = new ToolStripSeparator();
      this.tsMenuItemCreateNewGroup = new ToolStripMenuItem();
      this.tsMenuItemRenameGroup = new ToolStripMenuItem();
      this.toolStripSeparator2 = new ToolStripSeparator();
      this.tsMenuItemDelete = new ToolStripMenuItem();
      this.tsMenuItemHelp = new ToolStripMenuItem();
      this.tsMenuItemEncHelp = new ToolStripMenuItem();
      this.gradMenuStripMain.SuspendLayout();
      this.SuspendLayout();
      this.tvwMessenger.AllowDrop = true;
      this.tvwMessenger.ContextMenu = this.contextMenu1;
      this.tvwMessenger.Dock = DockStyle.Fill;
      this.tvwMessenger.HideSelection = false;
      this.tvwMessenger.ImageIndex = 0;
      this.tvwMessenger.ImageList = this.imageList1;
      this.tvwMessenger.Location = new Point(0, 24);
      this.tvwMessenger.Name = "tvwMessenger";
      this.tvwMessenger.SelectedImageIndex = 0;
      this.tvwMessenger.Size = new Size(226, 333);
      this.tvwMessenger.TabIndex = 4;
      this.tvwMessenger.DragLeave += new EventHandler(this.tvwMessenger_DragLeave);
      this.tvwMessenger.AfterLabelEdit += new NodeLabelEditEventHandler(this.tvwMessenger_AfterLabelEdit);
      this.tvwMessenger.DoubleClick += new EventHandler(this.tvwMessenger_DoubleClick);
      this.tvwMessenger.VisibleChanged += new EventHandler(this.tvwMessenger_VisibleChanged);
      this.tvwMessenger.DragDrop += new DragEventHandler(this.tvwMessenger_DragDrop);
      this.tvwMessenger.AfterSelect += new TreeViewEventHandler(this.tvwMessenger_AfterSelect);
      this.tvwMessenger.MouseDown += new MouseEventHandler(this.tvwMessenger_MouseDown);
      this.tvwMessenger.DragEnter += new DragEventHandler(this.tvwMessenger_DragEnter);
      this.tvwMessenger.ItemDrag += new ItemDragEventHandler(this.tvwMessenger_ItemDrag);
      this.tvwMessenger.DragOver += new DragEventHandler(this.tvwMessenger_DragOver);
      this.contextMenu1.MenuItems.AddRange(new MenuItem[11]
      {
        this.contextmenuSend,
        this.contextmenuDivid1,
        this.contextmenuAddContact,
        this.contextmenuProfile,
        this.contextmenuDivid4,
        this.contextmenuAddGroup,
        this.contextmenuRenameGroup,
        this.contextmenuDivid2,
        this.contextmenuRemove,
        this.contextmenuDivid3,
        this.menuItemRefresh
      });
      this.contextmenuSend.Index = 0;
      this.contextmenuSend.Text = "&Send";
      this.contextmenuSend.Click += new EventHandler(this.tsMenuItemSend_Click);
      this.contextmenuDivid1.Index = 1;
      this.contextmenuDivid1.Text = "-";
      this.contextmenuAddContact.Index = 2;
      this.contextmenuAddContact.Text = "&Add Contact...";
      this.contextmenuAddContact.Click += new EventHandler(this.tsMenuItemAddContact_Click);
      this.contextmenuProfile.Index = 3;
      this.contextmenuProfile.Text = "Contact D&etails";
      this.contextmenuProfile.Click += new EventHandler(this.tsMenuItemContactDetails_Click);
      this.contextmenuDivid4.Index = 4;
      this.contextmenuDivid4.Text = "-";
      this.contextmenuAddGroup.Index = 5;
      this.contextmenuAddGroup.Text = "Create &New Group...";
      this.contextmenuAddGroup.Click += new EventHandler(this.tsMenuItemCreateNewGroup_Click);
      this.contextmenuRenameGroup.Index = 6;
      this.contextmenuRenameGroup.Text = "&Rename Group...";
      this.contextmenuRenameGroup.Click += new EventHandler(this.tsMenuItemRenameGroup_Click);
      this.contextmenuDivid2.Index = 7;
      this.contextmenuDivid2.Text = "-";
      this.contextmenuRemove.Index = 8;
      this.contextmenuRemove.Text = "&Delete...";
      this.contextmenuRemove.Click += new EventHandler(this.tsMenuItemDelete_Click);
      this.contextmenuDivid3.Index = 9;
      this.contextmenuDivid3.Text = "-";
      this.menuItemRefresh.Index = 10;
      this.menuItemRefresh.Text = "&Refresh";
      this.menuItemRefresh.Click += new EventHandler(this.tsMenuItemRefresh_Click);
      this.imageList1.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imageList1.ImageStream");
      this.imageList1.TransparentColor = Color.Transparent;
      this.imageList1.Images.SetKeyName(0, "im-users-group.png");
      this.imageList1.Images.SetKeyName(1, "im-offline-user.png");
      this.imageList1.Images.SetKeyName(2, "im-online-user.png");
      this.imageList1.Images.SetKeyName(3, "im-invalid-user.png");
      this.lblNodeName.AutoSize = true;
      this.lblNodeName.BackColor = SystemColors.Window;
      this.lblNodeName.BorderStyle = BorderStyle.FixedSingle;
      this.lblNodeName.ForeColor = Color.Silver;
      this.lblNodeName.Location = new Point(128, 152);
      this.lblNodeName.Name = "lblNodeName";
      this.lblNodeName.Size = new Size(69, 15);
      this.lblNodeName.TabIndex = 5;
      this.lblNodeName.Text = "(NodeName)";
      this.lblNodeName.Visible = false;
      this.rtBoxInit.Dock = DockStyle.Fill;
      this.rtBoxInit.Location = new Point(0, 24);
      this.rtBoxInit.Name = "rtBoxInit";
      this.rtBoxInit.Size = new Size(226, 333);
      this.rtBoxInit.TabIndex = 6;
      this.rtBoxInit.Text = "<Add initial text>";
      this.gradMenuStripMain.Items.AddRange(new ToolStripItem[4]
      {
        (ToolStripItem) this.tsMenuItemMessenger,
        (ToolStripItem) this.tsMenuItemView,
        (ToolStripItem) this.tsMenuItemContacts,
        (ToolStripItem) this.tsMenuItemHelp
      });
      this.gradMenuStripMain.Location = new Point(0, 0);
      this.gradMenuStripMain.Name = "gradMenuStripMain";
      this.gradMenuStripMain.Padding = new Padding(0);
      this.gradMenuStripMain.Size = new Size(226, 24);
      this.gradMenuStripMain.TabIndex = 7;
      this.gradMenuStripMain.Text = "gradientMenuStrip1";
      this.tsMenuItemMessenger.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.tsMenuItemSend,
        (ToolStripItem) this.tsMenuItemClose
      });
      this.tsMenuItemMessenger.Name = "tsMenuItemMessenger";
      this.tsMenuItemMessenger.Size = new Size(71, 24);
      this.tsMenuItemMessenger.Text = "&Messenger";
      this.tsMenuItemSend.Name = "tsMenuItemSend";
      this.tsMenuItemSend.Size = new Size(269, 22);
      this.tsMenuItemSend.Text = "&Send";
      this.tsMenuItemSend.Click += new EventHandler(this.tsMenuItemSend_Click);
      this.tsMenuItemClose.Name = "tsMenuItemClose";
      this.tsMenuItemClose.ShortcutKeys = Keys.F4 | Keys.Alt;
      this.tsMenuItemClose.Size = new Size(269, 22);
      this.tsMenuItemClose.Text = "&Close Encompass Messenger";
      this.tsMenuItemClose.Click += new EventHandler(this.tsMenuItemClose_Click);
      this.tsMenuItemView.DropDownItems.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.tsMenuItemRefresh
      });
      this.tsMenuItemView.Name = "tsMenuItemView";
      this.tsMenuItemView.Size = new Size(41, 24);
      this.tsMenuItemView.Text = "&View";
      this.tsMenuItemRefresh.Image = (Image) componentResourceManager.GetObject("tsMenuItemRefresh.Image");
      this.tsMenuItemRefresh.Name = "tsMenuItemRefresh";
      this.tsMenuItemRefresh.ShortcutKeys = Keys.F5;
      this.tsMenuItemRefresh.Size = new Size(131, 22);
      this.tsMenuItemRefresh.Text = "&Refresh";
      this.tsMenuItemRefresh.Click += new EventHandler(this.tsMenuItemRefresh_Click);
      this.tsMenuItemContacts.DropDownItems.AddRange(new ToolStripItem[7]
      {
        (ToolStripItem) this.tsMenuItemAddContact,
        (ToolStripItem) this.tsMenuItemContactDetails,
        (ToolStripItem) this.toolStripSeparator1,
        (ToolStripItem) this.tsMenuItemCreateNewGroup,
        (ToolStripItem) this.tsMenuItemRenameGroup,
        (ToolStripItem) this.toolStripSeparator2,
        (ToolStripItem) this.tsMenuItemDelete
      });
      this.tsMenuItemContacts.Name = "tsMenuItemContacts";
      this.tsMenuItemContacts.Size = new Size(62, 24);
      this.tsMenuItemContacts.Text = "&Contacts";
      this.tsMenuItemAddContact.Image = (Image) componentResourceManager.GetObject("tsMenuItemAddContact.Image");
      this.tsMenuItemAddContact.Name = "tsMenuItemAddContact";
      this.tsMenuItemAddContact.Size = new Size(175, 22);
      this.tsMenuItemAddContact.Text = "&Add Contact...";
      this.tsMenuItemAddContact.Click += new EventHandler(this.tsMenuItemAddContact_Click);
      this.tsMenuItemContactDetails.Image = (Image) componentResourceManager.GetObject("tsMenuItemContactDetails.Image");
      this.tsMenuItemContactDetails.Name = "tsMenuItemContactDetails";
      this.tsMenuItemContactDetails.Size = new Size(175, 22);
      this.tsMenuItemContactDetails.Text = "Contact D&etails";
      this.tsMenuItemContactDetails.Click += new EventHandler(this.tsMenuItemContactDetails_Click);
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new Size(172, 6);
      this.tsMenuItemCreateNewGroup.Name = "tsMenuItemCreateNewGroup";
      this.tsMenuItemCreateNewGroup.Size = new Size(175, 22);
      this.tsMenuItemCreateNewGroup.Text = "Create &New Group...";
      this.tsMenuItemCreateNewGroup.Click += new EventHandler(this.tsMenuItemCreateNewGroup_Click);
      this.tsMenuItemRenameGroup.Name = "tsMenuItemRenameGroup";
      this.tsMenuItemRenameGroup.Size = new Size(175, 22);
      this.tsMenuItemRenameGroup.Text = "&Rename Group...";
      this.tsMenuItemRenameGroup.Click += new EventHandler(this.tsMenuItemRenameGroup_Click);
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new Size(172, 6);
      this.tsMenuItemDelete.Image = (Image) componentResourceManager.GetObject("tsMenuItemDelete.Image");
      this.tsMenuItemDelete.Name = "tsMenuItemDelete";
      this.tsMenuItemDelete.ShortcutKeys = Keys.D | Keys.Alt;
      this.tsMenuItemDelete.Size = new Size(175, 22);
      this.tsMenuItemDelete.Text = "&Delete...";
      this.tsMenuItemDelete.Click += new EventHandler(this.tsMenuItemDelete_Click);
      this.tsMenuItemHelp.DropDownItems.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.tsMenuItemEncHelp
      });
      this.tsMenuItemHelp.Name = "tsMenuItemHelp";
      this.tsMenuItemHelp.Size = new Size(40, 24);
      this.tsMenuItemHelp.Text = "&Help";
      this.tsMenuItemEncHelp.Image = (Image) componentResourceManager.GetObject("tsMenuItemEncHelp.Image");
      this.tsMenuItemEncHelp.Name = "tsMenuItemEncHelp";
      this.tsMenuItemEncHelp.ShortcutKeys = Keys.F1;
      this.tsMenuItemEncHelp.Size = new Size(182, 22);
      this.tsMenuItemEncHelp.Text = "Encompass &Help...";
      this.tsMenuItemEncHelp.Click += new EventHandler(this.tsMenuItemEncHelp_Click);
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(226, 357);
      this.Controls.Add((Control) this.lblNodeName);
      this.Controls.Add((Control) this.rtBoxInit);
      this.Controls.Add((Control) this.tvwMessenger);
      this.Controls.Add((Control) this.gradMenuStripMain);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.KeyPreview = true;
      this.MainMenuStrip = (MenuStrip) this.gradMenuStripMain;
      this.Name = nameof (EncompassMessenger);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Encompass Messenger";
      this.KeyUp += new KeyEventHandler(this.EncompassMessenger_KeyUp);
      this.gradMenuStripMain.ResumeLayout(false);
      this.gradMenuStripMain.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    internal static EncompassMessenger Instance => EncompassMessenger.encompassMessenger;

    public static void Start() => EncompassMessenger.Start((IMainScreen) null);

    public static void Start(IMainScreen mainScreen)
    {
      if (EncompassMessenger.encompassMessenger != null)
      {
        if (EncompassMessenger.encompassMessenger.InvokeRequired)
        {
          MethodInvoker method = new MethodInvoker(EncompassMessenger.Start);
          EncompassMessenger.encompassMessenger.Invoke((Delegate) method);
        }
        else
        {
          EncompassMessenger.encompassMessenger.WindowState = FormWindowState.Minimized;
          EncompassMessenger.encompassMessenger.WindowState = FormWindowState.Normal;
          EncompassMessenger.encompassMessenger.TopMost = true;
          EncompassMessenger.encompassMessenger.BringToFront();
          EncompassMessenger.encompassMessenger.Select();
          EncompassMessenger.encompassMessenger.TopMost = false;
        }
      }
      else
      {
        EncompassMessenger.encompassMessenger = new EncompassMessenger(mainScreen);
        EncompassMessenger.encompassMessenger.Show();
      }
    }

    internal void AddUserSession(SessionInfo session)
    {
      if (session.SessionID == Session.ISession.SessionID)
        return;
      foreach (TreeNode node1 in this.tvwMessenger.Nodes)
      {
        foreach (TreeNode node2 in node1.Nodes)
        {
          EncompassMessenger.MessengerListItem tag = (EncompassMessenger.MessengerListItem) node2.Tag;
          if (string.Compare(tag.UserID, session.UserID, StringComparison.OrdinalIgnoreCase) == 0 && session.SessionID != Session.ISession.SessionID)
          {
            tag.AddSessionInfo(session);
            node2.ForeColor = EncompassMessenger.activeColor;
            node2.ImageIndex = 2;
            node2.SelectedImageIndex = 2;
            break;
          }
        }
      }
    }

    internal void RemoveUserSession(SessionInfo session)
    {
      if (session.SessionID == Session.ISession.SessionID)
        return;
      foreach (TreeNode node1 in this.tvwMessenger.Nodes)
      {
        foreach (TreeNode node2 in node1.Nodes)
        {
          EncompassMessenger.MessengerListItem tag = (EncompassMessenger.MessengerListItem) node2.Tag;
          if (tag.SessionInfo != null)
          {
            tag.RemoveSessionInfo(session.SessionID);
            if (tag.SessionInfoCount == 0)
            {
              node2.ForeColor = EncompassMessenger.inactiveColor;
              node2.ImageIndex = 1;
              node2.SelectedImageIndex = 1;
            }
          }
        }
      }
    }

    private void sendMessage()
    {
      TreeNode selectedNode = this.tvwMessenger.SelectedNode;
      if (selectedNode == null)
      {
        int num1 = (int) MessageBox.Show((IWin32Window) this, "You must select a contact to send message to.", "Select a Contact", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        if (!this.isUserNode(selectedNode))
          return;
        EncompassMessenger.MessengerListItem tag = (EncompassMessenger.MessengerListItem) selectedNode.Tag;
        SessionInfo[] sessions = tag.SessionInfo;
        if (sessions != null)
        {
          ArrayList arrayList = new ArrayList();
          for (int index = 0; index < sessions.Length; ++index)
          {
            if (sessions[index].SessionID != Session.ISession.SessionID)
              arrayList.Add((object) sessions[index]);
          }
          sessions = (SessionInfo[]) arrayList.ToArray(typeof (SessionInfo));
        }
        if (sessions == null || sessions.Length == 0)
        {
          int num2 = (int) MessageBox.Show((IWin32Window) this, "You must select an online user to send message to.", "Select an Online User", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          SessionInfo selectedSessionInfo = sessions[0];
          if (sessions.Length > 1)
          {
            SessionSelectionDialog sessionSelectionDialog = new SessionSelectionDialog(tag.UserID.ToLower(), sessions);
            int num3 = (int) sessionSelectionDialog.ShowDialog();
            selectedSessionInfo = sessionSelectionDialog.SelectedSessionInfo;
          }
          EncompassMessenger.ProcessIMMessage((IMMessage) new IMChatMessage((string) null, ChatWindow.DefaultTextFont, ChatWindow.DefaultTextColor, selectedSessionInfo, Session.ISession.SessionID));
        }
      }
    }

    private void closeMessenger() => this.Close();

    private bool isGroupNode(TreeNode currentNode) => currentNode.Parent == null;

    private bool isUserNode(TreeNode currentNode) => currentNode.Parent != null;

    private SessionInfo[] getUserSessions(string userID)
    {
      SessionInfo[] allSessionInfo = Session.ServerManager.GetAllSessionInfo(true);
      ArrayList arrayList = new ArrayList();
      for (int index = 0; index < allSessionInfo.Length; ++index)
      {
        if (string.Compare(allSessionInfo[index].UserID, userID, StringComparison.OrdinalIgnoreCase) == 0)
        {
          arrayList.Add((object) allSessionInfo[index]);
          break;
        }
      }
      return (SessionInfo[]) arrayList.ToArray(typeof (SessionInfo));
    }

    private void tvwMessenger_DoubleClick(object sender, EventArgs e) => this.sendMessage();

    private void addEmptyGroup()
    {
      AddGroupDialog addGroupDialog = new AddGroupDialog();
      if (addGroupDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      string groupName = addGroupDialog.GroupName;
      Session.MessengerListManager.AddGroup(groupName);
      TreeNode treeNode = this.tvwMessenger.Nodes.Add(groupName);
      treeNode.ImageIndex = 0;
      treeNode.SelectedImageIndex = 0;
      this.rtBoxInit.Visible = false;
      this.tvwMessenger.Visible = true;
      this.tvwMessenger.BringToFront();
    }

    private TreeNode addGroup(string groupName)
    {
      groupName = groupName.Trim();
      TreeNode treeNode = (TreeNode) null;
      foreach (TreeNode node in this.tvwMessenger.Nodes)
      {
        if (node.Parent == null && node.Text.Trim() == groupName)
        {
          treeNode = node;
          break;
        }
      }
      if (treeNode == null)
        treeNode = this.tvwMessenger.Nodes.Add(groupName);
      return treeNode;
    }

    private void renameGroup()
    {
      TreeNode selectedNode = this.tvwMessenger.SelectedNode;
      if (selectedNode == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a group.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        TreeNode treeNode = selectedNode.Parent ?? selectedNode;
        this.tvwMessenger.LabelEdit = true;
        treeNode.BeginEdit();
      }
    }

    private void removeContactOrGroup()
    {
      TreeNode selectedNode = this.tvwMessenger.SelectedNode;
      if (selectedNode == null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You must select a group or a contact to remove.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        TreeNode parent = selectedNode.Parent;
        if (parent == null && selectedNode.Nodes.Count > 0)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "This group is not empty. You can't delete this group.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          if ((parent != null ? Utils.Dialog((IWin32Window) this, "Are you sure you want to delete the contact " + selectedNode.Text + ".", MessageBoxButtons.YesNo, MessageBoxIcon.Question) : Utils.Dialog((IWin32Window) this, "Are you sure you want to delete the group " + selectedNode.Text + ".", MessageBoxButtons.YesNo, MessageBoxIcon.Question)) != DialogResult.Yes)
            return;
          if (parent == null)
          {
            Session.MessengerListManager.DeleteGroup(selectedNode.Text.Trim());
          }
          else
          {
            EncompassMessenger.MessengerListItem tag = (EncompassMessenger.MessengerListItem) selectedNode.Tag;
            Session.MessengerListManager.DeleteContact(tag.GroupName, tag.UserID);
          }
          selectedNode.Remove();
          if (this.tvwMessenger.Nodes.Count != 0)
            return;
          this.tvwMessenger.Visible = false;
          this.rtBoxInit.Visible = true;
          this.rtBoxInit.BringToFront();
        }
      }
    }

    private void viewContactProfile()
    {
      TreeNode selectedNode = this.tvwMessenger.SelectedNode;
      if (selectedNode == null || !this.isUserNode(selectedNode))
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You must select a contact to view the profile.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        EncompassMessenger.MessengerListItem tag = (EncompassMessenger.MessengerListItem) selectedNode.Tag;
        if (tag == null)
          return;
        UserInfo user = Session.OrganizationManager.GetUser(tag.UserID.ToLower());
        if (user == (UserInfo) null)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "There is no contact profile for " + selectedNode.Text + ".", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          int num3 = (int) new ContactMessengerProfile(user).ShowDialog((IWin32Window) this);
        }
      }
    }

    private void refreshContacts()
    {
      if (this.InvokeRequired)
      {
        this.Invoke((Delegate) new MethodInvoker(this.refreshContacts));
      }
      else
      {
        this.tvwMessenger.Nodes.Clear();
        MessengerListInfo messengerList = Session.MessengerListManager.GetMessengerList();
        if (messengerList == null)
          return;
        MessengerGroupInfo[] messengerGroups = messengerList.GetMessengerGroups();
        if (messengerGroups == null || messengerGroups.Length == 0)
        {
          this.tvwMessenger.Visible = false;
          this.rtBoxInit.Visible = true;
          this.rtBoxInit.BringToFront();
        }
        else
        {
          this.rtBoxInit.Visible = false;
          this.tvwMessenger.Visible = true;
          this.tvwMessenger.BringToFront();
          Array.Sort<MessengerGroupInfo>(messengerGroups);
          this.sessions = Session.ServerManager.GetAllSessionInfo(true);
          for (int index1 = 0; index1 < messengerGroups.Length; ++index1)
          {
            TreeNode treeNode1 = this.addGroup(messengerGroups[index1].GroupName);
            treeNode1.ImageIndex = 0;
            treeNode1.SelectedImageIndex = 0;
            MessengerContactInfo[] messengerContacts = messengerGroups[index1].GetMessengerContacts();
            Array.Sort<MessengerContactInfo>(messengerContacts);
            for (int index2 = 0; index2 < messengerContacts.Length; ++index2)
            {
              string contactUserid = messengerContacts[index2].ContactUserid;
              string displayName = messengerContacts[index2].DisplayName;
              ArrayList sessions = new ArrayList();
              UserInfo user = Session.OrganizationManager.GetUser(contactUserid);
              if (user != (UserInfo) null)
              {
                for (int index3 = 0; index3 < this.sessions.Length; ++index3)
                {
                  if (string.Compare(this.sessions[index3].UserID, contactUserid, StringComparison.OrdinalIgnoreCase) == 0 && this.sessions[index3].SessionID != Session.ISession.SessionID)
                    sessions.Add((object) this.sessions[index3]);
                }
              }
              TreeNode treeNode2 = treeNode1.Nodes.Add(displayName);
              if (sessions != null && sessions.Count != 0)
              {
                treeNode2.ForeColor = EncompassMessenger.activeColor;
                treeNode2.ImageIndex = 2;
                treeNode2.SelectedImageIndex = 2;
              }
              else
              {
                treeNode2.ForeColor = EncompassMessenger.inactiveColor;
                if (user == (UserInfo) null || user.Status == UserInfo.UserStatusEnum.Disabled)
                {
                  treeNode2.ImageIndex = 3;
                  treeNode2.SelectedImageIndex = 3;
                }
                else
                {
                  treeNode2.ImageIndex = 1;
                  treeNode2.SelectedImageIndex = 1;
                }
              }
              treeNode2.Tag = (object) new EncompassMessenger.MessengerListItem(contactUserid, messengerGroups[index1].GroupName, sessions);
            }
          }
          this.tvwMessenger.ExpandAll();
        }
      }
    }

    private void tvwMessenger_ItemDrag(object sender, ItemDragEventArgs e)
    {
      TreeNode selectedNode = this.tvwMessenger.SelectedNode;
      if (selectedNode == null || !this.isUserNode(selectedNode))
        return;
      this.lblNodeName.Visible = true;
      this.lblNodeName.Text = selectedNode.Text;
      this.dragNode = (TreeNode) e.Item;
      int num = (int) this.DoDragDrop(e.Item, DragDropEffects.Move);
    }

    private void tvwMessenger_DragDrop(object sender, DragEventArgs e)
    {
      this.lblNodeName.Visible = false;
      TreeNode currentNode = this.tvwMessenger.GetNodeAt(this.tvwMessenger.PointToClient(new Point(e.X, e.Y)));
      if (currentNode == null)
        return;
      if (this.isUserNode(currentNode))
        currentNode = currentNode.Parent;
      string toGroup = currentNode.Text.Trim();
      EncompassMessenger.MessengerListItem tag = (EncompassMessenger.MessengerListItem) this.dragNode.Tag;
      if (tag.GroupName.ToLower() == toGroup.ToLower())
        return;
      Session.MessengerListManager.MoveContact(tag.UserID, tag.GroupName, toGroup);
      this.dragNode.Remove();
      TreeNode treeNode = currentNode.Nodes.Add(this.dragNode.Text);
      treeNode.ImageIndex = this.dragNode.ImageIndex;
      treeNode.SelectedImageIndex = this.dragNode.ImageIndex;
      treeNode.Tag = this.dragNode.Tag;
      treeNode.ForeColor = this.dragNode.ForeColor;
      currentNode.Expand();
    }

    private void tvwMessenger_DragEnter(object sender, DragEventArgs e)
    {
      this.lblNodeName.Visible = true;
      e.Effect = DragDropEffects.Move;
    }

    private void tvwMessenger_DragOver(object sender, DragEventArgs e)
    {
      this.lblNodeName.Location = this.PointToClient(new Point(e.X + 2, e.Y + 2));
    }

    private void tvwMessenger_DragLeave(object sender, EventArgs e)
    {
      this.lblNodeName.Visible = false;
    }

    private void tvwMessenger_MouseDown(object sender, MouseEventArgs e)
    {
      TreeNode nodeAt = this.tvwMessenger.GetNodeAt(e.X, e.Y);
      if (nodeAt == null || nodeAt.IsSelected)
        return;
      this.tvwMessenger.SelectedNode = nodeAt;
    }

    private void tvwMessenger_AfterSelect(object sender, TreeViewEventArgs e)
    {
      TreeNode selectedNode = this.tvwMessenger.SelectedNode;
      if (selectedNode == null)
        return;
      if (this.isUserNode(selectedNode))
      {
        this.contextmenuRenameGroup.Enabled = false;
        this.tsMenuItemRenameGroup.Enabled = false;
        this.contextmenuProfile.Enabled = true;
        this.contextmenuDivid4.Enabled = true;
        this.tsMenuItemContactDetails.Enabled = true;
        this.contextmenuDivid1.Enabled = true;
        SessionInfo[] sessionInfoArray = ((EncompassMessenger.MessengerListItem) selectedNode.Tag).SessionInfo;
        if (sessionInfoArray != null)
        {
          ArrayList arrayList = new ArrayList();
          for (int index = 0; index < sessionInfoArray.Length; ++index)
          {
            if (sessionInfoArray[index].SessionID != Session.ISession.SessionID)
              arrayList.Add((object) sessionInfoArray[index]);
          }
          sessionInfoArray = (SessionInfo[]) arrayList.ToArray(typeof (SessionInfo));
        }
        if (sessionInfoArray != null && sessionInfoArray.Length != 0)
        {
          this.contextmenuSend.Enabled = true;
          this.tsMenuItemSend.Enabled = true;
        }
        else
        {
          this.contextmenuSend.Enabled = false;
          this.tsMenuItemSend.Enabled = false;
        }
      }
      else
      {
        if (!this.isGroupNode(selectedNode))
          return;
        this.contextmenuRenameGroup.Enabled = true;
        this.tsMenuItemRenameGroup.Enabled = true;
        this.contextmenuProfile.Enabled = false;
        this.contextmenuDivid4.Enabled = false;
        this.tsMenuItemContactDetails.Enabled = false;
        this.contextmenuDivid1.Enabled = false;
        this.tsMenuItemSend.Enabled = false;
        this.contextmenuSend.Enabled = false;
      }
    }

    internal void addContact(string contactUserid)
    {
      if (this.InvokeRequired)
      {
        this.Invoke((Delegate) new EncompassMessenger.AddContactDelegate(this.addContact), (object[]) new string[1]
        {
          contactUserid
        });
      }
      else
      {
        AddContactDialog addContactDialog = new AddContactDialog(contactUserid, this.getAllGroups(), this.selectedGroupIndex);
        addContactDialog.TopMost = true;
        if (addContactDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
        {
          addContactDialog.TopMost = false;
        }
        else
        {
          addContactDialog.TopMost = false;
          string userid = addContactDialog.Userid;
          if (Session.MessengerListManager.ContainsContact(userid))
          {
            int num1 = (int) Utils.Dialog((IWin32Window) this, "Contact '" + userid + "' has already been added to your Messenger List.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
          }
          else
          {
            string newGroupName = addContactDialog.NewGroupName;
            if (newGroupName != null)
            {
              Session.MessengerListManager.AddGroup(newGroupName);
              this.tvwMessenger.SelectedNode = this.addGroup(newGroupName);
            }
            else
              this.setSelectedGroupNode(addContactDialog.SelectedGroupName);
            string firstName = addContactDialog.FirstName;
            string lastName = addContactDialog.LastName;
            string message = addContactDialog.Message;
            string text = userid;
            if (firstName != "" || lastName != "")
              text = (firstName + " " + lastName).Trim();
            SessionInfo[] sessions = (SessionInfo[]) null;
            TreeNode selectedNode = this.tvwMessenger.SelectedNode;
            if (selectedNode == null)
              throw new Exception("No group node selected");
            TreeNode treeNode1 = selectedNode.Parent ?? selectedNode;
            UserInfo user = Session.OrganizationManager.GetUser(userid);
            if (user != (UserInfo) null)
              sessions = this.getUserSessions(userid);
            else if (MessageBox.Show((IWin32Window) this, "Currently there is no valid/enabled user account associated with userid '" + userid + "'.  Do you still want to add this user to your Messenger List?", "Add to Messenger List", MessageBoxButtons.YesNo, MessageBoxIcon.Hand) == DialogResult.No)
              return;
            Session.MessengerListManager.AddContact(treeNode1.Text, userid, firstName, lastName, message);
            TreeNode treeNode2 = treeNode1.Nodes.Add(text);
            if (sessions != null && (sessions.Length > 1 || sessions.Length == 1 && sessions[0].SessionID != Session.ISession.SessionID))
            {
              treeNode2.ForeColor = EncompassMessenger.activeColor;
              treeNode2.ImageIndex = 2;
              treeNode2.SelectedImageIndex = 2;
            }
            else
            {
              treeNode2.ForeColor = EncompassMessenger.inactiveColor;
              if (user == (UserInfo) null || user.Status == UserInfo.UserStatusEnum.Disabled)
              {
                treeNode2.ImageIndex = 3;
                treeNode2.SelectedImageIndex = 3;
              }
              else
              {
                treeNode2.ImageIndex = 1;
                treeNode2.SelectedImageIndex = 1;
              }
            }
            treeNode2.EnsureVisible();
            treeNode2.Tag = (object) new EncompassMessenger.MessengerListItem(userid, treeNode1.Text, sessions);
            this.tvwMessenger.SelectedNode = treeNode2;
            treeNode1.Expand();
            if (user != (UserInfo) null && userid != Session.UserID)
            {
              int num2 = (int) MessageBox.Show((IWin32Window) this, userid + " has been added to your Messenger List pending approval of your request.", "Add to Messenger List", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            this.rtBoxInit.Visible = false;
            this.tvwMessenger.Visible = true;
            this.tvwMessenger.BringToFront();
          }
        }
      }
    }

    internal void addContact() => this.addContact((string) null);

    internal static void ProcessIMMessage(IMMessage imMsg)
    {
      if (imMsg is IMControlMessage)
      {
        EncompassMessenger.Start();
        IMControlMessage imControlMessage = (IMControlMessage) imMsg;
        switch (imControlMessage.MsgType)
        {
          case IMMessage.MessageType.RequestAddToList:
            AddContactRequest addContactRequest = new AddContactRequest(imControlMessage.FromUser, imControlMessage.Text);
            addContactRequest.TopMost = true;
            int num1 = (int) addContactRequest.ShowDialog();
            addContactRequest.TopMost = false;
            break;
          case IMMessage.MessageType.DenyAddToList:
            if (EncompassMessenger.Instance != null)
              EncompassMessenger.encompassMessenger.refreshContacts();
            int num2 = (int) new RequestDenied(imControlMessage.FromUser, imControlMessage.Text).ShowDialog();
            break;
        }
      }
      else
      {
        string sessionId = imMsg.Source.SessionID;
        string userId = imMsg.Source.UserID;
        if (!(imMsg is IMChatMessage imChatMessage))
          return;
        string receiverSessionId = imChatMessage.ReceiverSessionID;
        string text = imChatMessage.Text;
        Font font = imChatMessage.Font;
        Color color = imChatMessage.Color;
        if (sessionId != Session.ISession.SessionID && receiverSessionId != Session.ISession.SessionID)
          return;
        string sessionID = !(Session.ISession.SessionID == sessionId) ? sessionId : receiverSessionId;
        bool flag = false;
        ChatWindow chatWindow = ChatWindow.GetChatWindow(sessionID);
        if (chatWindow == null)
        {
          flag = true;
          chatWindow = new ChatWindow(sessionId, userId);
          ChatWindow.AddChatWindow(sessionId, chatWindow);
        }
        if (text != null)
        {
          text += Environment.NewLine;
          if (sessionId != Session.ISession.SessionID)
            chatWindow.AppendChatText(userId + ": ", new Font(FontFamily.GenericSansSerif, 8f, FontStyle.Bold), Color.Blue, false);
          else
            chatWindow.AppendChatText(userId + ": ", new Font(FontFamily.GenericSansSerif, 8f, FontStyle.Bold), ChatWindow.DefaultTextColor, false);
        }
        if (sessionId != Session.ISession.SessionID)
          chatWindow.AppendChatText(text, font, color);
        else
          chatWindow.AppendChatText(text, font, color, false);
        if (!flag)
          return;
        if (Application.MessageLoop)
          chatWindow.Show();
        else
          Application.Run((Form) chatWindow);
      }
    }

    private void tvwMessenger_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
    {
      if (e == null || e.Label == null)
      {
        this.tvwMessenger.LabelEdit = false;
      }
      else
      {
        TreeNode node = e.Node;
        if (e.Label.Trim() == "")
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "A group name cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          e.CancelEdit = true;
          node.BeginEdit();
        }
        else if (e.Label.Trim().ToLower() != node.Text.ToLower() && Session.MessengerListManager.ContainsGroup(e.Label.Trim()))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "A group with the name " + e.Label.Trim() + " already exists. Please try again.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          e.CancelEdit = true;
          node.BeginEdit();
        }
        else
        {
          Session.MessengerListManager.RenameGroup(node.Text, e.Label.Trim());
          this.tvwMessenger.LabelEdit = false;
        }
      }
    }

    private string[] getAllGroups()
    {
      this.selectedGroupIndex = -1;
      ArrayList arrayList = new ArrayList();
      for (int index = 0; index < this.tvwMessenger.Nodes.Count; ++index)
      {
        if (this.tvwMessenger.Nodes[index].Parent == null)
        {
          arrayList.Add((object) this.tvwMessenger.Nodes[index].Text.Trim());
          if (this.tvwMessenger.SelectedNode == this.tvwMessenger.Nodes[index])
            this.selectedGroupIndex = index;
        }
      }
      return (string[]) arrayList.ToArray(typeof (string));
    }

    private void setSelectedGroupNode(string name)
    {
      for (int index = 0; index < this.tvwMessenger.Nodes.Count; ++index)
      {
        if (this.tvwMessenger.Nodes[index].Parent == null && string.Compare(this.tvwMessenger.Nodes[index].Text.Trim(), name.Trim(), StringComparison.OrdinalIgnoreCase) == 0)
          this.tvwMessenger.SelectedNode = this.tvwMessenger.Nodes[index];
      }
    }

    private void EncompassMessenger_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      Session.Application.GetService<IEncompassApplication>().DisplayHelp("Encompass Instant Messenger");
    }

    private void tsMenuItemSend_Click(object sender, EventArgs e) => this.sendMessage();

    private void tsMenuItemClose_Click(object sender, EventArgs e) => this.closeMessenger();

    private void tsMenuItemRefresh_Click(object sender, EventArgs e) => this.refreshContacts();

    private void tsMenuItemAddContact_Click(object sender, EventArgs e) => this.addContact();

    private void tsMenuItemContactDetails_Click(object sender, EventArgs e)
    {
      this.viewContactProfile();
    }

    private void tsMenuItemCreateNewGroup_Click(object sender, EventArgs e) => this.addEmptyGroup();

    private void tsMenuItemRenameGroup_Click(object sender, EventArgs e) => this.renameGroup();

    private void tsMenuItemDelete_Click(object sender, EventArgs e) => this.removeContactOrGroup();

    private void tsMenuItemEncHelp_Click(object sender, EventArgs e)
    {
      this.mainScreen.ShowHelp((Control) this, "Encompass Instant Messenger");
    }

    private void tvwMessenger_VisibleChanged(object sender, EventArgs e)
    {
      this.tsMenuItemSend.Enabled = this.tsMenuItemDelete.Enabled = this.tsMenuItemContactDetails.Enabled = this.tsMenuItemRenameGroup.Enabled = this.tvwMessenger.Visible;
    }

    private class MessengerListItem
    {
      private ArrayList sessionInfo = new ArrayList();
      private string userID;
      private string groupName;

      public MessengerListItem(string userID, string groupName)
      {
        this.userID = userID.Trim();
        this.groupName = groupName.Trim();
      }

      public MessengerListItem(string userID, string groupName, SessionInfo[] sessions)
        : this(userID, groupName)
      {
        if (sessions == null)
          return;
        this.sessionInfo.AddRange((ICollection) sessions);
      }

      public MessengerListItem(string userID, string groupName, ArrayList sessions)
        : this(userID, groupName)
      {
        this.sessionInfo = sessions;
      }

      public void AddSessionInfo(SessionInfo info)
      {
        foreach (SessionInfo sessionInfo in this.sessionInfo)
        {
          if (sessionInfo.SessionID == info.SessionID)
            return;
        }
        this.sessionInfo.Add((object) info);
      }

      public void RemoveSessionInfo(string sessionID)
      {
        foreach (SessionInfo sessionInfo in this.sessionInfo)
        {
          if (sessionInfo.SessionID == sessionID)
          {
            this.sessionInfo.Remove((object) sessionInfo);
            break;
          }
        }
      }

      public int SessionInfoCount => this.sessionInfo.Count;

      public SessionInfo[] SessionInfo
      {
        get => (SessionInfo[]) this.sessionInfo.ToArray(typeof (SessionInfo));
      }

      public string UserID => this.userID.Trim();

      public string GroupName
      {
        get => this.groupName.Trim();
        set => this.groupName = value.Trim();
      }
    }

    private enum MessengerImage
    {
      Group,
      Offline,
      Online,
      DisabledUser,
    }

    private delegate void AddContactDelegate(string contactUserid);
  }
}
