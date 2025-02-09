// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.EditCompanyAttachmentControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.ExternalOriginatorManagement;
using EllieMae.EMLite.Setup.ExternalOriginatorManagement.RestApi;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class EditCompanyAttachmentControl : UserControl
  {
    private List<ExternalOrgAttachments> attachments;
    private int externalOrgId;
    private Sessions.Session session;
    private List<ExternalSettingValue> settingsList;
    private AnimationProgress an;
    private Task<bool> t;
    private SessionObjects obj;
    private IContainer components;
    private StandardIconButton btnView;
    private StandardIconButton btnDelete;
    private StandardIconButton btnAdd;
    private GridView gridViewAttachments;
    private StandardIconButton btnEdit;
    private GroupContainer grpAll;
    private GroupContainer groupContainer3;
    private Panel panelHeader;
    private Label label33;

    public event EventHandler AlertChanged;

    public EditCompanyAttachmentControl(Sessions.Session session, int externalOrgId, bool edit)
    {
      this.InitializeComponent();
      this.session = session;
      this.externalOrgId = externalOrgId;
      this.settingsList = this.session.ConfigurationManager.GetExternalOrgSettingsByName("Attachment Category");
      this.settingsList.Insert(0, new ExternalSettingValue(-1, -1, "", "", 0));
      this.Dock = DockStyle.Fill;
      this.RefreshData();
      if (!edit)
        return;
      bool hierarchyAccess;
      Session.TpoHierarchyAccessCache.TryGetValue(externalOrgId, out hierarchyAccess);
      TPOClientUtils.DisableControls((UserControl) this, hierarchyAccess);
    }

    public EditCompanyAttachmentControl(int externalOrgId, SessionObjects obj)
    {
      this.InitializeComponent();
      this.obj = obj;
      this.externalOrgId = externalOrgId;
      this.settingsList = this.obj.ConfigurationManager.GetExternalOrgSettingsByName("Attachment Category");
      this.settingsList.Insert(0, new ExternalSettingValue(-1, -1, "", "", 0));
      this.Dock = DockStyle.Fill;
      this.RefreshData();
    }

    public void RefreshData()
    {
      this.gridViewAttachments.Items.Clear();
      this.attachments = this.session.ConfigurationManager.GetExternalAttachmentsByOid(this.externalOrgId);
      foreach (ExternalOrgAttachments attachment in this.attachments)
        this.gridViewAttachments.Items.Add(this.createGVItemForAttachment(attachment));
      this.gridViewAttachments.ReSort();
    }

    private GVItem createGVItemForAttachment(ExternalOrgAttachments attachment)
    {
      GVItem itemForAttachment = new GVItem();
      itemForAttachment.SubItems.Add((object) attachment.DateAdded);
      itemForAttachment.SubItems.Add((object) attachment.UserWhoAdded);
      itemForAttachment.SubItems.Add((object) attachment.FileName);
      if (attachment.Description.Length > 50)
        itemForAttachment.SubItems.Add((object) attachment.Description.Substring(0, 50));
      else
        itemForAttachment.SubItems.Add((object) attachment.Description);
      DateTime dateTime;
      if (attachment.FileDate == DateTime.MinValue)
      {
        itemForAttachment.SubItems.Add((object) "");
      }
      else
      {
        GVSubItemCollection subItems = itemForAttachment.SubItems;
        dateTime = attachment.FileDate;
        string str = dateTime.ToString("d");
        subItems.Add((object) str);
      }
      itemForAttachment.SubItems.Add((object) this.settingsList.FirstOrDefault<ExternalSettingValue>((Func<ExternalSettingValue, bool>) (it => it.settingId == attachment.Category)).settingValue);
      if (attachment.ExpirationDate == DateTime.MinValue)
      {
        itemForAttachment.SubItems.Add((object) "");
      }
      else
      {
        GVSubItemCollection subItems = itemForAttachment.SubItems;
        dateTime = attachment.ExpirationDate;
        string str = dateTime.ToString("d");
        subItems.Add((object) str);
      }
      if (itemForAttachment.SubItems[6].Text != "")
        itemForAttachment.SubItems.Add((object) attachment.DaysToExpire);
      itemForAttachment.Tag = (object) attachment;
      return itemForAttachment;
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      using (AddAttachmentForm addAttachmentForm = new AddAttachmentForm((ExternalOrgAttachments) null, this.externalOrgId, this.session.UserID, this.settingsList))
      {
        if (addAttachmentForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        using (BinaryObject data = new BinaryObject(addAttachmentForm.Path))
          this.addProgressAttachment(new FileSystemEntry("\\\\" + addAttachmentForm.FileName, FileSystemEntry.Types.File, (string) null), data);
        this.session.ConfigurationManager.InsertExternalAttachment(addAttachmentForm.Attachment);
        WebhookApiHelper.PublishExternalOrgWebhookEvent(this.session.UserID, this.Parent.Text, this.externalOrgId);
        this.RefreshData();
        this.AlertChanged(sender, e);
      }
    }

    private void btnEdit_Click(object sender, EventArgs e)
    {
      if (this.gridViewAttachments.SelectedItems.Count == 0)
        return;
      using (AddAttachmentForm addAttachmentForm = new AddAttachmentForm((ExternalOrgAttachments) this.gridViewAttachments.SelectedItems[0].Tag, this.externalOrgId, this.session.UserID, this.settingsList))
      {
        if (addAttachmentForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        if (addAttachmentForm.AttachmentChanged)
        {
          using (BinaryObject data = new BinaryObject(addAttachmentForm.Path))
            this.addProgressAttachment(new FileSystemEntry("\\\\" + addAttachmentForm.FileName, FileSystemEntry.Types.File, (string) null), data);
        }
        this.session.ConfigurationManager.UpdateExternalAttachment(addAttachmentForm.Attachment);
        WebhookApiHelper.PublishExternalOrgWebhookEvent(this.session.UserID, this.Parent.Text, this.externalOrgId);
        this.RefreshData();
        this.AlertChanged(sender, e);
      }
    }

    private void addProgressAttachment(FileSystemEntry newEntry, BinaryObject data)
    {
      TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
      this.an = new AnimationProgress();
      this.t = Task<bool>.Factory.StartNew((Func<bool>) (() => this.addAttachment(newEntry, data)));
      this.t.ContinueWith((Action<Task<bool>>) (task =>
      {
        if (!task.IsCompleted)
          return;
        this.an.Dispose();
        this.an.Close();
        if (task.IsCanceled || task.IsFaulted || !task.Result)
        {
          int num1 = (int) Utils.Dialog((IWin32Window) this, "Attachment upload failed!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "Attachment uploaded successfully!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
      }), scheduler);
      this.an.StartPosition = FormStartPosition.CenterParent;
      int num = (int) this.an.ShowDialog((IWin32Window) this);
    }

    private bool addAttachment(FileSystemEntry newEntry, BinaryObject data)
    {
      try
      {
        this.session.ConfigurationManager.AddAttachment(newEntry, data);
        return true;
      }
      catch
      {
        return false;
      }
    }

    private void btnView_Click(object sender, EventArgs e)
    {
      if (this.gridViewAttachments.SelectedItems.Count == 0)
        return;
      ExternalOrgAttachments tag = (ExternalOrgAttachments) this.gridViewAttachments.SelectedItems[0].Tag;
      // ISSUE: variable of a boxed type
      __Boxed<Guid> guid = (ValueType) tag.Guid;
      string str = ((IEnumerable<string>) tag.RealFileName.Split('.')).Last<string>();
      string fileName = guid.ToString() + "." + str;
      string realFileName = ((IEnumerable<string>) tag.RealFileName.Split('\\')).Last<string>();
      try
      {
        this.previewAttachment(this.session.ConfigurationManager.ReadAttachment(fileName), realFileName);
      }
      catch (Exception ex)
      {
        string message = ex.Message;
        if (ex.Message.Contains(":"))
          message = ex.Message.Split(':')[1];
        int num = (int) Utils.Dialog((IWin32Window) this, message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void previewAttachment(BinaryObject obj, string realFileName)
    {
      string externalAttachmentsDir = SystemSettings.LocalExternalAttachmentsDir;
      Directory.CreateDirectory(Path.GetDirectoryName(externalAttachmentsDir));
      string str = SystemUtil.CombinePath(externalAttachmentsDir, realFileName);
      if (obj == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The '" + realFileName + "' attachment cannot be found or no longer exists.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        try
        {
          obj.Write(str);
          obj.Download();
          Process.Start(str);
        }
        catch (Exception ex)
        {
          throw new Exception("The attachment is already open.\r\n");
        }
      }
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (this.gridViewAttachments.SelectedItems.Count == 0 || Utils.Dialog((IWin32Window) this, "Are you sure you want to delete the selected attachment(s)?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
        return;
      foreach (GVItem selectedItem in this.gridViewAttachments.SelectedItems)
      {
        ExternalOrgAttachments tag = (ExternalOrgAttachments) selectedItem.Tag;
        // ISSUE: variable of a boxed type
        __Boxed<Guid> guid = (ValueType) tag.Guid;
        string str = ((IEnumerable<string>) tag.RealFileName.Split('.')).Last<string>();
        FileSystemEntry entry = new FileSystemEntry("\\\\" + (guid.ToString() + "." + str), FileSystemEntry.Types.File, (string) null);
        this.session.ConfigurationManager.DeleteExternalAttachment(tag.Guid, entry);
      }
      WebhookApiHelper.PublishExternalOrgWebhookEvent(this.session.UserID, this.Parent.Text, this.externalOrgId);
      this.RefreshData();
      this.AlertChanged(sender, e);
    }

    public bool AnyAttachmentExpired
    {
      get => this.session.ConfigurationManager.GetExternalAttachmentIsExpired(this.externalOrgId);
    }

    private void gridViewAttachments_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnDelete.Enabled = this.gridViewAttachments.SelectedItems.Count > 0;
      this.btnEdit.Enabled = this.btnView.Enabled = this.gridViewAttachments.SelectedItems.Count == 1;
    }

    private void gridViewAttachments_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      this.btnEdit_Click((object) null, (EventArgs) null);
    }

    public void AssignOid(int oid)
    {
      if (this.externalOrgId == -1)
        this.externalOrgId = oid;
      this.attachments = this.session.ConfigurationManager.GetExternalAttachmentsByOid(this.externalOrgId);
      bool hierarchyAccess;
      Session.TpoHierarchyAccessCache.TryGetValue(this.externalOrgId, out hierarchyAccess);
      TPOClientUtils.DisableControls((UserControl) this, hierarchyAccess);
    }

    public void DisableControls()
    {
      this.btnAdd.Visible = this.btnDelete.Visible = this.btnEdit.Visible = false;
      this.btnView.Left = this.btnDelete.Left;
      if (this.gridViewAttachments.Items.Count > 0)
      {
        this.gridViewAttachments.SelectedIndexChanged -= new EventHandler(this.gridViewAttachments_SelectedIndexChanged);
        this.gridViewAttachments.Items[0].Selected = true;
        this.btnView.Enabled = true;
        this.gridViewAttachments.SelectedIndexChanged += new EventHandler(this.gridViewAttachments_SelectedIndexChanged);
      }
      else
        this.btnView.Visible = false;
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
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      GVColumn gvColumn7 = new GVColumn();
      GVColumn gvColumn8 = new GVColumn();
      this.btnEdit = new StandardIconButton();
      this.btnView = new StandardIconButton();
      this.btnDelete = new StandardIconButton();
      this.btnAdd = new StandardIconButton();
      this.gridViewAttachments = new GridView();
      this.grpAll = new GroupContainer();
      this.groupContainer3 = new GroupContainer();
      this.panelHeader = new Panel();
      this.label33 = new Label();
      ((ISupportInitialize) this.btnEdit).BeginInit();
      ((ISupportInitialize) this.btnView).BeginInit();
      ((ISupportInitialize) this.btnDelete).BeginInit();
      ((ISupportInitialize) this.btnAdd).BeginInit();
      this.grpAll.SuspendLayout();
      this.groupContainer3.SuspendLayout();
      this.panelHeader.SuspendLayout();
      this.SuspendLayout();
      this.btnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnEdit.BackColor = Color.Transparent;
      this.btnEdit.Enabled = false;
      this.btnEdit.Location = new Point(793, 6);
      this.btnEdit.MouseDownImage = (Image) null;
      this.btnEdit.Name = "btnEdit";
      this.btnEdit.Size = new Size(16, 16);
      this.btnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnEdit.TabIndex = 13;
      this.btnEdit.TabStop = false;
      this.btnEdit.Click += new EventHandler(this.btnEdit_Click);
      this.btnView.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnView.BackColor = Color.Transparent;
      this.btnView.Enabled = false;
      this.btnView.Location = new Point(815, 6);
      this.btnView.MouseDownImage = (Image) null;
      this.btnView.Name = "btnView";
      this.btnView.Size = new Size(16, 16);
      this.btnView.StandardButtonType = StandardIconButton.ButtonType.ZoomInButton;
      this.btnView.TabIndex = 12;
      this.btnView.TabStop = false;
      this.btnView.Click += new EventHandler(this.btnView_Click);
      this.btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDelete.BackColor = Color.Transparent;
      this.btnDelete.Enabled = false;
      this.btnDelete.Location = new Point(837, 6);
      this.btnDelete.MouseDownImage = (Image) null;
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(16, 16);
      this.btnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDelete.TabIndex = 11;
      this.btnDelete.TabStop = false;
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAdd.BackColor = Color.Transparent;
      this.btnAdd.Location = new Point(771, 6);
      this.btnAdd.MouseDownImage = (Image) null;
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(16, 16);
      this.btnAdd.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAdd.TabIndex = 9;
      this.btnAdd.TabStop = false;
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.gridViewAttachments.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "ColumnDate";
      gvColumn1.SortMethod = GVSortMethod.DateTime;
      gvColumn1.Text = "Date/Time";
      gvColumn1.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn1.Width = 120;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "ColumnUser";
      gvColumn2.Text = "User";
      gvColumn2.Width = 120;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "ColumnFileName";
      gvColumn3.Text = "Attachment Name";
      gvColumn3.Width = 206;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "ColumnDescription";
      gvColumn4.Text = "Description";
      gvColumn4.Width = 200;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "ColumnFileDate";
      gvColumn5.SortMethod = GVSortMethod.DateTime;
      gvColumn5.Text = "File Date";
      gvColumn5.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn5.Width = 100;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "ColumnCategory";
      gvColumn6.Text = "Category";
      gvColumn6.Width = 100;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "ColumnExpDate";
      gvColumn7.SortMethod = GVSortMethod.Date;
      gvColumn7.Text = "Expiration Date";
      gvColumn7.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn7.Width = 100;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "ColumnDateToExp";
      gvColumn8.SortMethod = GVSortMethod.Numeric;
      gvColumn8.Text = "Days to Expire";
      gvColumn8.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn8.Width = 100;
      this.gridViewAttachments.Columns.AddRange(new GVColumn[8]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7,
        gvColumn8
      });
      this.gridViewAttachments.Dock = DockStyle.Fill;
      this.gridViewAttachments.Location = new Point(0, 26);
      this.gridViewAttachments.Name = "gridViewAttachments";
      this.gridViewAttachments.Size = new Size(860, 531);
      this.gridViewAttachments.TabIndex = 8;
      this.gridViewAttachments.SelectedIndexChanged += new EventHandler(this.gridViewAttachments_SelectedIndexChanged);
      this.gridViewAttachments.ItemDoubleClick += new GVItemEventHandler(this.gridViewAttachments_ItemDoubleClick);
      this.grpAll.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.grpAll.Controls.Add((Control) this.groupContainer3);
      this.grpAll.Controls.Add((Control) this.panelHeader);
      this.grpAll.HeaderForeColor = SystemColors.ControlText;
      this.grpAll.Location = new Point(5, 5);
      this.grpAll.Margin = new Padding(0, 0, 0, 0);
      this.grpAll.Name = "grpAll";
      this.grpAll.Size = new Size(862, 610);
      this.grpAll.TabIndex = 12;
      this.grpAll.Text = "Attachments";
      this.groupContainer3.Borders = AnchorStyles.Top;
      this.groupContainer3.Controls.Add((Control) this.btnEdit);
      this.groupContainer3.Controls.Add((Control) this.btnView);
      this.groupContainer3.Controls.Add((Control) this.gridViewAttachments);
      this.groupContainer3.Controls.Add((Control) this.btnDelete);
      this.groupContainer3.Controls.Add((Control) this.btnAdd);
      this.groupContainer3.Dock = DockStyle.Fill;
      this.groupContainer3.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer3.Location = new Point(1, 52);
      this.groupContainer3.Name = "groupContainer3";
      this.groupContainer3.Size = new Size(860, 557);
      this.groupContainer3.TabIndex = 3;
      this.groupContainer3.Text = "Attachments";
      this.panelHeader.Controls.Add((Control) this.label33);
      this.panelHeader.Dock = DockStyle.Top;
      this.panelHeader.Location = new Point(1, 26);
      this.panelHeader.Name = "panelHeader";
      this.panelHeader.Size = new Size(860, 26);
      this.panelHeader.TabIndex = 2;
      this.label33.AutoSize = true;
      this.label33.Location = new Point(6, 6);
      this.label33.Name = "label33";
      this.label33.Size = new Size(531, 13);
      this.label33.TabIndex = 35;
      this.label33.Text = "Attach and manage relevant files and documents associated with the Third Party Originator company or branch.";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.grpAll);
      this.Margin = new Padding(0, 0, 0, 0);
      this.Name = nameof (EditCompanyAttachmentControl);
      this.Padding = new Padding(5);
      this.Size = new Size(872, 620);
      ((ISupportInitialize) this.btnEdit).EndInit();
      ((ISupportInitialize) this.btnView).EndInit();
      ((ISupportInitialize) this.btnDelete).EndInit();
      ((ISupportInitialize) this.btnAdd).EndInit();
      this.grpAll.ResumeLayout(false);
      this.groupContainer3.ResumeLayout(false);
      this.panelHeader.ResumeLayout(false);
      this.panelHeader.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
