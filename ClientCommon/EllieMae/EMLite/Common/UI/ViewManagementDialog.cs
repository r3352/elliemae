// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.ViewManagementDialog
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class ViewManagementDialog : Form
  {
    private EllieMae.EMLite.ClientServer.TemplateSettingsType templateType;
    private bool publicViews;
    private string defaultViewSetting;
    private List<BinaryConvertibleObject> staticViews = new List<BinaryConvertibleObject>();
    private List<PipelineViewList> userViews = new List<PipelineViewList>();
    private string defaultView;
    private string defaultViewName;
    private bool hasManagePipelineViewRights = Session.ACL.IsAuthorizedForFeature(AclFeature.LoanMgmt_CreatePipelineViews);
    private List<PipelineViewList> personaViews = new List<PipelineViewList>();
    private IContainer components;
    private GridView gvViews;
    private Button btnRename;
    private Button btnClose;
    private GroupContainer grpViews;
    private FlowLayoutPanel flowLayoutPanel1;
    private Button btnSetAsDefault;
    private VerticalSeparator verticalSeparator1;
    private StandardIconButton btnDelete;
    private ToolTip toolTip1;
    private StandardIconButton btnDuplicate;

    public FileSystemEntry[] FileSystemEntries => this.getFileSystemEntries();

    public ViewManagementDialog(
      EllieMae.EMLite.ClientServer.TemplateSettingsType templateType,
      bool publicViews,
      string defaultViewSetting)
    {
      this.templateType = templateType;
      this.publicViews = publicViews;
      this.defaultViewSetting = defaultViewSetting;
      this.InitializeComponent();
      this.gvViews.Sort(0, SortOrder.Ascending);
      this.loadViews();
    }

    private void loadViews()
    {
      if (this.templateType == EllieMae.EMLite.ClientServer.TemplateSettingsType.DocumentTrackingView)
      {
        List<ViewSummary> viewsSummary = Session.EfolderDocTrackViewManager.GetViewsSummary();
        this.gvViews.Items.Clear();
        if (viewsSummary == null)
          return;
        foreach (ViewSummary viewSummary in viewsSummary)
        {
          GVItem gvItem = new GVItem();
          gvItem.SubItems[0].Text = viewSummary.Name;
          if (viewSummary.IsDefault)
          {
            gvItem.SubItems[1].Text = "Yes";
            this.defaultView = viewSummary.Name;
          }
          gvItem.Tag = (object) viewSummary;
          this.gvViews.Items.Add(gvItem);
        }
        this.gvViews.ReSort();
        this.grpViews.Text = "Views (" + (object) this.gvViews.Items.Count + ")";
      }
      else if (this.templateType == EllieMae.EMLite.ClientServer.TemplateSettingsType.PipelineView)
      {
        this.defaultViewName = this.defaultView = Session.GetPrivateProfileString(this.defaultViewSetting);
        int num = this.defaultViewName.LastIndexOf("\\");
        if (num > 0)
          this.defaultViewName = this.defaultViewName.Substring(num + 1);
        this.gvViews.Items.Clear();
        foreach (PipelineViewList personaView in this.personaViews)
          this.gvViews.Items.Add(this.createViewListItem(personaView));
        foreach (PipelineViewList userView in this.userViews)
          this.gvViews.Items.Add(this.createViewListItem(userView));
        this.gvViews.ReSort();
        this.grpViews.Text = "Views (" + (object) this.gvViews.Items.Count + ")";
      }
      else
      {
        FileSystemEntry[] fileSystemEntryArray = !this.publicViews ? Session.ConfigurationManager.GetTemplateDirEntries(this.templateType, FileSystemEntry.PrivateRoot(Session.UserID)) : Session.ConfigurationManager.GetAllPublicTemplateSettingsFileEntries(this.templateType, false);
        this.defaultView = Session.GetPrivateProfileString(this.defaultViewSetting);
        this.gvViews.Items.Clear();
        foreach (BinaryConvertibleObject staticView in this.staticViews)
          this.gvViews.Items.Add(this.createViewListItem(staticView));
        foreach (FileSystemEntry e in fileSystemEntryArray)
          this.gvViews.Items.Add(this.createViewListItem(e));
        this.gvViews.ReSort();
        this.grpViews.Text = "Views (" + (object) this.gvViews.Items.Count + ")";
      }
    }

    public void AddStaticView(BinaryConvertibleObject view)
    {
      this.staticViews.Add(view);
      this.gvViews.Items.Add(this.createViewListItem(view));
      this.gvViews.ReSort();
      this.grpViews.Text = "Views (" + (object) this.gvViews.Items.Count + ")";
    }

    public void AddUserView(PipelineViewList view)
    {
      if (!view.PipelineView.IsUserView)
        this.personaViews.Add(view);
      else
        this.userViews.Add(view);
      this.gvViews.Items.Add(this.createViewListItem(view));
      this.gvViews.ReSort();
      this.grpViews.Text = "Views (" + (object) this.gvViews.Items.Count + ")";
    }

    private GVItem createViewListItem(FileSystemEntry e)
    {
      GVItem viewListItem = new GVItem();
      viewListItem.SubItems[0].Text = e.Name;
      if (e.ToString() == this.defaultView)
        viewListItem.SubItems[1].Text = "Yes";
      viewListItem.Tag = (object) e;
      return viewListItem;
    }

    private GVItem createViewListItem(PipelineView e)
    {
      GVItem viewListItem = new GVItem();
      viewListItem.SubItems[0].Text = e.Name;
      if (e.ToString() == this.defaultView)
        viewListItem.SubItems[1].Text = "Yes";
      viewListItem.Tag = (object) e;
      return viewListItem;
    }

    private GVItem createViewListItem(BinaryConvertibleObject view)
    {
      ITemplateSetting templateSetting = (ITemplateSetting) view;
      GVItem viewListItem = new GVItem();
      viewListItem.SubItems[0].Text = templateSetting.TemplateName;
      if (templateSetting.TemplateName == this.defaultView)
        viewListItem.SubItems[1].Text = "Yes";
      viewListItem.Tag = (object) view;
      return viewListItem;
    }

    private GVItem createViewListItem(PipelineViewList view)
    {
      GVItem viewListItem = new GVItem();
      viewListItem.SubItems[0].Text = view.PipelineView.Name;
      if (view.PipelineView.Name == this.defaultViewName)
        viewListItem.SubItems[1].Text = "Yes";
      viewListItem.Tag = (object) view;
      return viewListItem;
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (this.gvViews.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You must first select the search to be deleted.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        if (Utils.Dialog((IWin32Window) this, "Are you sure you want to delete the selected view(s)?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
          return;
        try
        {
          foreach (GVItem selectedItem in this.gvViews.SelectedItems)
          {
            FileSystemEntry fileEntry = (FileSystemEntry) null;
            PipelineViewList pipelineViewList = (PipelineViewList) null;
            if (this.templateType == EllieMae.EMLite.ClientServer.TemplateSettingsType.PipelineView)
            {
              pipelineViewList = selectedItem.Tag as PipelineViewList;
              ((PipelineViewAclManager) Session.ACL.GetAclManager(AclCategory.PersonaPipelineView)).DeleteUserCustomPipelineView(Session.UserID, pipelineViewList.PipelineView.Name);
              this.userViews.Remove(pipelineViewList);
            }
            else if (this.templateType == EllieMae.EMLite.ClientServer.TemplateSettingsType.DocumentTrackingView)
            {
              Session.EfolderDocTrackViewManager.DeleteView((selectedItem.Tag as ViewSummary).Id);
            }
            else
            {
              fileEntry = selectedItem.Tag as FileSystemEntry;
              Session.ConfigurationManager.DeleteTemplateSettingsObject(this.templateType, fileEntry);
            }
            if (this.defaultView == (fileEntry != null ? fileEntry.ToString() : pipelineViewList?.PipelineView?.Name))
            {
              Session.WritePrivateProfileString(this.defaultViewSetting, string.Empty);
              this.defaultView = string.Empty;
              if (this.templateType == EllieMae.EMLite.ClientServer.TemplateSettingsType.PreliminaryConditionTrackingView)
                Session.StartupInfo.DefaultPreliminaryConditionTrackingView = (ConditionTrackingView) null;
              else if (this.templateType == EllieMae.EMLite.ClientServer.TemplateSettingsType.PostClosingConditionTrackingView)
                Session.StartupInfo.DefaultPostClosingConditionTrackingView = (ConditionTrackingView) null;
              else if (this.templateType == EllieMae.EMLite.ClientServer.TemplateSettingsType.UnderwritingConditionTrackingView)
                Session.StartupInfo.DefaultUnderwritingConditionTrackingView = (ConditionTrackingView) null;
              else if (this.templateType == EllieMae.EMLite.ClientServer.TemplateSettingsType.SellConditionTrackingView)
                Session.StartupInfo.DefaultSellConditionTrackingView = (ConditionTrackingView) null;
              else if (this.templateType == EllieMae.EMLite.ClientServer.TemplateSettingsType.EnhancedConditionTrackingView)
                Session.StartupInfo.DefaultEnhancedConditionTrackingView = (ConditionTrackingView) null;
            }
          }
        }
        catch (Exception ex)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "An unexpected error occurred while attempting to delete this item: " + (object) ex);
        }
        this.loadViews();
      }
    }

    private void btnRename_Click(object sender, EventArgs e)
    {
      if (this.gvViews.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You must first select the item to be renamed.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        GVItem selectedItem = this.gvViews.SelectedItems[0];
        FileSystemEntry fileEntry = (FileSystemEntry) null;
        PipelineViewList pipelineViewList = (PipelineViewList) null;
        string name1;
        if (this.templateType == EllieMae.EMLite.ClientServer.TemplateSettingsType.PipelineView)
        {
          pipelineViewList = selectedItem.Tag as PipelineViewList;
          name1 = pipelineViewList.PipelineView.Name;
        }
        else if (this.templateType == EllieMae.EMLite.ClientServer.TemplateSettingsType.DocumentTrackingView)
        {
          name1 = ((ViewSummary) selectedItem.Tag).Name;
        }
        else
        {
          fileEntry = selectedItem.Tag as FileSystemEntry;
          name1 = fileEntry.Name;
        }
        string entryName = "";
        using (ViewNameSelector viewNameSelector = new ViewNameSelector(this.templateType, this.getNameList(), name1))
        {
          if (viewNameSelector.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return;
          entryName = viewNameSelector.ViewName;
        }
        if (this.templateType == EllieMae.EMLite.ClientServer.TemplateSettingsType.DocumentTrackingView)
        {
          try
          {
            DocumentTrackingView view = Session.EfolderDocTrackViewManager?.GetView(((ViewSummary) selectedItem.Tag).Id);
            view.Name = entryName;
            DocumentTrackingView documentTrackingView = Session.EfolderDocTrackViewManager?.UpdateView(view);
            selectedItem.Text = entryName;
            selectedItem.Tag = (object) new ViewSummary()
            {
              Id = documentTrackingView.Id,
              Name = documentTrackingView.Name
            };
            if (!(this.defaultView == name1))
              return;
            Session.WritePrivateProfileString(this.defaultViewSetting, "Personal:\\" + Session.UserID + "\\" + entryName);
            this.defaultView = entryName;
          }
          catch (Exception ex)
          {
            int num2 = (int) Utils.Dialog((IWin32Window) this, "An unexpected error occurred while attempting to rename this item: " + (object) ex);
          }
        }
        else if (fileEntry != null)
        {
          FileSystemEntry entry = new FileSystemEntry("\\", entryName, FileSystemEntry.Types.File, this.publicViews ? (string) null : Session.UserID);
          try
          {
            BinaryConvertibleObject data = (BinaryConvertibleObject) null;
            BinaryObject templateSettings = Session.ConfigurationManager.GetTemplateSettings(this.templateType, fileEntry);
            if (templateSettings != null)
              data = TemplateSettingsTypeConverter.ConvertToTemplateObject(this.templateType, templateSettings);
            if (templateSettings != null)
              Session.ConfigurationManager.DeleteTemplateSettingsObject(this.templateType, fileEntry);
            ((ITemplateSetting) data).TemplateName = entryName;
            Session.ConfigurationManager.SaveTemplateSettings(this.templateType, entry, (BinaryObject) data);
            if (this.defaultView == fileEntry.ToString())
            {
              Session.WritePrivateProfileString(this.defaultViewSetting, entry.ToString());
              this.defaultView = entry.ToString();
              if (this.templateType == EllieMae.EMLite.ClientServer.TemplateSettingsType.PreliminaryConditionTrackingView)
                Session.StartupInfo.DefaultPreliminaryConditionTrackingView = (ConditionTrackingView) data;
              else if (this.templateType == EllieMae.EMLite.ClientServer.TemplateSettingsType.PostClosingConditionTrackingView)
                Session.StartupInfo.DefaultPostClosingConditionTrackingView = (ConditionTrackingView) data;
              else if (this.templateType == EllieMae.EMLite.ClientServer.TemplateSettingsType.UnderwritingConditionTrackingView)
                Session.StartupInfo.DefaultUnderwritingConditionTrackingView = (ConditionTrackingView) data;
              else if (this.templateType == EllieMae.EMLite.ClientServer.TemplateSettingsType.SellConditionTrackingView)
                Session.StartupInfo.DefaultSellConditionTrackingView = (ConditionTrackingView) data;
              else if (this.templateType == EllieMae.EMLite.ClientServer.TemplateSettingsType.EnhancedConditionTrackingView)
                Session.StartupInfo.DefaultEnhancedConditionTrackingView = (ConditionTrackingView) data;
            }
            selectedItem.Text = entryName;
            selectedItem.Tag = (object) entry;
          }
          catch (Exception ex)
          {
            int num3 = (int) Utils.Dialog((IWin32Window) this, "An unexpected error occurred while attempting to rename this item: " + (object) ex);
          }
        }
        else
        {
          if (pipelineViewList == null)
            return;
          try
          {
            string viewName = entryName?.Trim();
            PipelineViewAclManager aclManager = (PipelineViewAclManager) Session.ACL.GetAclManager(AclCategory.PersonaPipelineView);
            string name2 = pipelineViewList.PipelineView.Name;
            pipelineViewList.PipelineView.Name = viewName;
            UserPipelineView userPipelineView1 = this.GetUserPipelineView(pipelineViewList.PipelineView);
            userPipelineView1.ViewID = pipelineViewList.PipelineView.ViewID;
            aclManager.UpdatePipelineUserView(userPipelineView1, Session.UserID, name2);
            if (this.defaultView == pipelineViewList.PipelineView.Name)
            {
              Session.WritePrivateProfileString(this.defaultViewSetting, "Personal:\\" + Session.UserID + "\\" + viewName);
              this.defaultView = pipelineViewList.PipelineView.Name;
            }
            selectedItem.Text = viewName;
            UserPipelineView userPipelineView2 = aclManager.GetUserPipelineView(Session.UserID, viewName);
            pipelineViewList.PipelineView.ViewID = userPipelineView2 != null ? userPipelineView2.ViewID : 0;
            selectedItem.Tag = (object) pipelineViewList;
          }
          catch (Exception ex)
          {
            int num4 = (int) Utils.Dialog((IWin32Window) this, "An unexpected error occurred while attempting to rename this item: " + (object) ex);
          }
        }
      }
    }

    private UserPipelineView GetUserPipelineView(PipelineView pipelineView)
    {
      UserPipelineView userPipelineView = new UserPipelineView(Session.UserID, "New View");
      userPipelineView.Name = pipelineView.Name;
      userPipelineView.LoanFolders = pipelineView.LoanFolder;
      userPipelineView.ExternalOrgId = pipelineView.ExternalOrgId;
      userPipelineView.Filter = pipelineView.Filter;
      userPipelineView.Ownership = pipelineView.LoanOwnership;
      userPipelineView.OrgType = pipelineView.LoanOrgType;
      foreach (TableLayout.Column column in pipelineView.Layout)
        userPipelineView.Columns.Add(column.ColumnID, column.SortOrder, column.SortPriority, column.Width, column.Alignment.ToString());
      return userPipelineView;
    }

    private void gvViews_SelectedIndexChanged(object sender, EventArgs e)
    {
      int count = this.gvViews.SelectedItems.Count;
      bool flag1 = false;
      bool flag2 = false;
      foreach (GVItem selectedItem in this.gvViews.SelectedItems)
      {
        bool flag3 = false;
        bool flag4 = false;
        System.Type type = selectedItem.Tag.GetType();
        if (type == typeof (PipelineViewList))
          flag3 = true;
        if (type != typeof (FileSystemEntry) && this.templateType == EllieMae.EMLite.ClientServer.TemplateSettingsType.PipelineView)
          flag4 = flag3 ? ((PipelineViewList) selectedItem.Tag).PipelineView.IsUserView : ((PipelineView) selectedItem.Tag).IsUserView;
        if (selectedItem.Tag is BinaryConvertibleObject && !flag4)
          flag1 = true;
        else if (flag3 && !((PipelineViewList) selectedItem.Tag).PipelineView.IsUserView)
          flag1 = true;
        if (selectedItem.SubItems[1].Text != "")
          flag2 = true;
        if (this.templateType == EllieMae.EMLite.ClientServer.TemplateSettingsType.DocumentTrackingView && string.Equals(selectedItem.SubItems[0].Text, "Standard View", StringComparison.CurrentCultureIgnoreCase))
          flag1 = true;
      }
      this.btnDelete.Enabled = count > 0 && !flag1 && !flag2 && this.hasManagePipelineViewRights;
      this.btnRename.Enabled = count == 1 && !flag1 && this.hasManagePipelineViewRights;
      this.btnDuplicate.Enabled = count == 1 && this.hasManagePipelineViewRights;
      this.btnSetAsDefault.Enabled = count == 1 && !flag2;
    }

    private void btnSetAsDefault_Click(object sender, EventArgs e)
    {
      FileSystemEntry tag1 = this.gvViews.SelectedItems[0].Tag as FileSystemEntry;
      ITemplateSetting tag2 = this.gvViews.SelectedItems[0].Tag as ITemplateSetting;
      PipelineViewList tag3 = this.gvViews.SelectedItems[0].Tag as PipelineViewList;
      if (tag1 != null)
      {
        Session.WritePrivateProfileString(this.defaultViewSetting, tag1.ToString());
        this.defaultView = tag1.ToString();
      }
      else if (tag2 != null)
      {
        Session.WritePrivateProfileString(this.defaultViewSetting, tag2.TemplateName);
        this.defaultView = tag2.TemplateName;
      }
      if (tag3 != null)
      {
        Session.WritePrivateProfileString(this.defaultViewSetting, tag3.PipelineView.IsUserView ? "Personal:\\" + Session.UserID + "\\" + tag3.PipelineView.Name.ToString() : tag3.PipelineView.Name);
        this.defaultView = tag3.PipelineView.Name;
      }
      if (this.templateType == EllieMae.EMLite.ClientServer.TemplateSettingsType.DocumentTrackingView)
        Session.WritePrivateProfileString(this.defaultViewSetting, "Personal:\\" + Session.UserID + "\\" + ((ViewSummary) this.gvViews.SelectedItems[0].Tag).Name.ToString());
      else if (this.templateType == EllieMae.EMLite.ClientServer.TemplateSettingsType.PreliminaryConditionTrackingView)
      {
        if (tag1 != null)
          Session.StartupInfo.DefaultPreliminaryConditionTrackingView = (ConditionTrackingView) Session.ConfigurationManager.GetTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.PreliminaryConditionTrackingView, tag1);
      }
      else if (this.templateType == EllieMae.EMLite.ClientServer.TemplateSettingsType.PostClosingConditionTrackingView)
      {
        if (tag1 != null)
          Session.StartupInfo.DefaultPostClosingConditionTrackingView = (ConditionTrackingView) Session.ConfigurationManager.GetTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.PostClosingConditionTrackingView, tag1);
      }
      else if (this.templateType == EllieMae.EMLite.ClientServer.TemplateSettingsType.UnderwritingConditionTrackingView)
      {
        if (tag1 != null)
          Session.StartupInfo.DefaultUnderwritingConditionTrackingView = (ConditionTrackingView) Session.ConfigurationManager.GetTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.UnderwritingConditionTrackingView, tag1);
      }
      else if (this.templateType == EllieMae.EMLite.ClientServer.TemplateSettingsType.SellConditionTrackingView)
      {
        if (tag1 != null)
          Session.StartupInfo.DefaultSellConditionTrackingView = (ConditionTrackingView) Session.ConfigurationManager.GetTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.SellConditionTrackingView, tag1);
      }
      else if (this.templateType == EllieMae.EMLite.ClientServer.TemplateSettingsType.EnhancedConditionTrackingView && tag1 != null)
        Session.StartupInfo.DefaultEnhancedConditionTrackingView = (ConditionTrackingView) Session.ConfigurationManager.GetTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.EnhancedConditionTrackingView, tag1);
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvViews.Items)
        gvItem.SubItems[1].Text = !gvItem.Selected ? "" : "Yes";
    }

    private void btnDuplicate_Click(object sender, EventArgs e)
    {
      if (this.gvViews.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You must first select the view to be duplicated.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        GVItem selectedItem = this.gvViews.SelectedItems[0];
        string entryName = "";
        using (ViewNameSelector viewNameSelector = new ViewNameSelector(this.templateType, this.getNameList()))
        {
          if (viewNameSelector.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return;
          entryName = viewNameSelector.ViewName;
        }
        if (this.templateType == EllieMae.EMLite.ClientServer.TemplateSettingsType.DocumentTrackingView)
        {
          try
          {
            DocumentTrackingView view = Session.EfolderDocTrackViewManager.GetView(((ViewSummary) selectedItem.Tag).Id);
            view.Name = entryName;
            Session.EfolderDocTrackViewManager.CreateView(view);
            this.loadViews();
          }
          catch (Exception ex)
          {
            int num2 = (int) Utils.Dialog((IWin32Window) this, "An unexpected error occurred while attempting to duplicate this item: " + (object) ex);
          }
        }
        else if (this.templateType == EllieMae.EMLite.ClientServer.TemplateSettingsType.PipelineView)
        {
          try
          {
            string viewName = entryName?.Trim();
            if (selectedItem.Tag is PipelineViewList pipelineViewList)
              pipelineViewList = (PipelineViewList) pipelineViewList.Duplicate();
            pipelineViewList.PipelineView.Name = viewName;
            pipelineViewList.PipelineView.IsUserView = true;
            PipelineViewAclManager aclManager = (PipelineViewAclManager) Session.ACL.GetAclManager(AclCategory.PersonaPipelineView);
            UserPipelineView userPipelineView1 = this.GetUserPipelineView(pipelineViewList.PipelineView);
            userPipelineView1.ViewID = pipelineViewList.PipelineView.ViewID;
            aclManager.CreatePipelineUserView(userPipelineView1, Session.UserID);
            UserPipelineView userPipelineView2 = aclManager.GetUserPipelineView(Session.UserID, viewName);
            pipelineViewList.PipelineView.ViewID = userPipelineView2 != null ? userPipelineView2.ViewID : 0;
            this.userViews.Add(pipelineViewList);
            this.loadViews();
          }
          catch (Exception ex)
          {
            int num3 = (int) Utils.Dialog((IWin32Window) this, "An unexpected error occurred while attempting to duplicate this item: " + (object) ex);
          }
        }
        else
        {
          FileSystemEntry entry = new FileSystemEntry("\\", entryName, FileSystemEntry.Types.File, this.publicViews ? (string) null : Session.UserID);
          try
          {
            object obj = selectedItem.Tag;
            if (this.templateType == EllieMae.EMLite.ClientServer.TemplateSettingsType.PipelineView && selectedItem.Tag is PipelineViewList)
              obj = selectedItem.Tag is PipelineViewList tag ? (object) tag.PipelineView : (object) (PipelineView) null;
            BinaryConvertibleObject data = !(obj is BinaryConvertibleObject convertibleObject) ? TemplateSettingsTypeConverter.ConvertToTemplateObject(this.templateType, Session.ConfigurationManager.GetTemplateSettings(this.templateType, selectedItem.Tag as FileSystemEntry)) : (BinaryConvertibleObject) convertibleObject.Clone();
            ((ITemplateSetting) data).TemplateName = entryName;
            Session.ConfigurationManager.SaveTemplateSettings(this.templateType, entry, (BinaryObject) data);
            this.loadViews();
          }
          catch (Exception ex)
          {
            int num4 = (int) Utils.Dialog((IWin32Window) this, "An unexpected error occurred while attempting to duplicate this item: " + (object) ex);
          }
        }
      }
    }

    private string[] getNameList()
    {
      List<string> stringList = new List<string>();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvViews.Items)
        stringList.Add(gvItem.SubItems[0].Text);
      return stringList.ToArray();
    }

    private FileSystemEntry[] getFileSystemEntries()
    {
      List<FileSystemEntry> fileSystemEntryList = new List<FileSystemEntry>();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvViews.Items)
      {
        if (gvItem.Tag is FileSystemEntry tag)
          fileSystemEntryList.Add(tag);
      }
      return fileSystemEntryList.ToArray();
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
      this.gvViews = new GridView();
      this.btnRename = new Button();
      this.btnClose = new Button();
      this.grpViews = new GroupContainer();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.btnSetAsDefault = new Button();
      this.verticalSeparator1 = new VerticalSeparator();
      this.btnDelete = new StandardIconButton();
      this.btnDuplicate = new StandardIconButton();
      this.toolTip1 = new ToolTip(this.components);
      this.grpViews.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      ((ISupportInitialize) this.btnDelete).BeginInit();
      ((ISupportInitialize) this.btnDuplicate).BeginInit();
      this.SuspendLayout();
      this.gvViews.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.SpringToFit = true;
      gvColumn1.Text = "Name";
      gvColumn1.Width = 368;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Default";
      gvColumn2.Width = 122;
      this.gvViews.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvViews.Dock = DockStyle.Fill;
      this.gvViews.Location = new Point(1, 26);
      this.gvViews.Name = "gvViews";
      this.gvViews.Size = new Size(490, 257);
      this.gvViews.TabIndex = 0;
      this.gvViews.SelectedIndexChanged += new EventHandler(this.gvViews_SelectedIndexChanged);
      this.btnRename.BackColor = SystemColors.Control;
      this.btnRename.Enabled = false;
      this.btnRename.Location = new Point(68, 0);
      this.btnRename.Margin = new Padding(0);
      this.btnRename.Name = "btnRename";
      this.btnRename.Padding = new Padding(2, 0, 0, 0);
      this.btnRename.Size = new Size(62, 22);
      this.btnRename.TabIndex = 2;
      this.btnRename.Text = "&Rename";
      this.btnRename.UseVisualStyleBackColor = true;
      this.btnRename.Click += new EventHandler(this.btnRename_Click);
      this.btnClose.DialogResult = DialogResult.Cancel;
      this.btnClose.Location = new Point(427, 303);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 23);
      this.btnClose.TabIndex = 3;
      this.btnClose.Text = "&Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.grpViews.Controls.Add((Control) this.flowLayoutPanel1);
      this.grpViews.Controls.Add((Control) this.gvViews);
      this.grpViews.Location = new Point(10, 10);
      this.grpViews.Name = "grpViews";
      this.grpViews.Size = new Size(492, 284);
      this.grpViews.TabIndex = 4;
      this.grpViews.Text = "Views";
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.btnSetAsDefault);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnRename);
      this.flowLayoutPanel1.Controls.Add((Control) this.verticalSeparator1);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnDelete);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnDuplicate);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(264, 2);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(222, 22);
      this.flowLayoutPanel1.TabIndex = 1;
      this.btnSetAsDefault.BackColor = SystemColors.Control;
      this.btnSetAsDefault.Enabled = false;
      this.btnSetAsDefault.Location = new Point(130, 0);
      this.btnSetAsDefault.Margin = new Padding(0);
      this.btnSetAsDefault.Name = "btnSetAsDefault";
      this.btnSetAsDefault.Padding = new Padding(2, 0, 0, 0);
      this.btnSetAsDefault.Size = new Size(92, 22);
      this.btnSetAsDefault.TabIndex = 0;
      this.btnSetAsDefault.Text = "Set As Default";
      this.btnSetAsDefault.UseVisualStyleBackColor = true;
      this.btnSetAsDefault.Click += new EventHandler(this.btnSetAsDefault_Click);
      this.verticalSeparator1.Location = new Point(63, 3);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 1;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.btnDelete.BackColor = Color.Transparent;
      this.btnDelete.Enabled = false;
      this.btnDelete.Location = new Point(42, 3);
      this.btnDelete.Margin = new Padding(3, 3, 2, 3);
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(16, 16);
      this.btnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDelete.TabIndex = 3;
      this.btnDelete.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnDelete, "Delete");
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.btnDuplicate.BackColor = Color.Transparent;
      this.btnDuplicate.Enabled = false;
      this.btnDuplicate.Location = new Point(21, 3);
      this.btnDuplicate.Margin = new Padding(3, 3, 2, 3);
      this.btnDuplicate.Name = "btnDuplicate";
      this.btnDuplicate.Size = new Size(16, 16);
      this.btnDuplicate.StandardButtonType = StandardIconButton.ButtonType.DuplicateButton;
      this.btnDuplicate.TabIndex = 4;
      this.btnDuplicate.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnDuplicate, "Duplicate");
      this.btnDuplicate.Click += new EventHandler(this.btnDuplicate_Click);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnClose;
      this.ClientSize = new Size(512, 335);
      this.Controls.Add((Control) this.grpViews);
      this.Controls.Add((Control) this.btnClose);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ViewManagementDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Manage Views";
      this.grpViews.ResumeLayout(false);
      this.flowLayoutPanel1.ResumeLayout(false);
      ((ISupportInitialize) this.btnDelete).EndInit();
      ((ISupportInitialize) this.btnDuplicate).EndInit();
      this.ResumeLayout(false);
    }
  }
}
