// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PipelineConfiguration
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class PipelineConfiguration : UserControl, IPersonaSecurityPage
  {
    private PipelineLoanTabPage pipelineLoanTabPage;
    private PipelinePage pipelinePage;
    private int personaID = -1;
    private bool needToSaveData;
    private Persona[] personas;
    private string userID = "";
    private Dictionary<string, AclTriState> accessColList = new Dictionary<string, AclTriState>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
    private Dictionary<string, AclTriState> accessColList_User = new Dictionary<string, AclTriState>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
    private Dictionary<string, AclTriState> accessColList_Personas = new Dictionary<string, AclTriState>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
    private FieldAccessAclManager fieldAccessMgr;
    private PipelineViewAclManager pipelineViewMgr;
    private LoanReportFieldDefs loanFieldDefs;
    private bool forcedReadOnly;
    private FeaturesAclManager featureMgr;
    private List<string> viewDBColumns = new List<string>();
    private Sessions.Session session;
    private IContainer components;
    private PanelEx pnlExLeft;
    private GroupContainer gcPipelineView;
    private GroupContainer gcAccessibility;
    private PanelEx pnlExNote;
    private Splitter splitter1;
    private Splitter splitter2;
    private Label label1;
    private GridView gvColumns;
    private GridView gvPipelineViews;
    private PanelEx pnlExRight;
    private StandardIconButton siBtnDelete;
    private StandardIconButton siBtnFieldList;
    private ContextMenuStrip cmsLink;
    private ToolStripMenuItem tsmiLink;
    private ToolStripMenuItem tsmiDisconnect;
    private StandardIconButton siBtnDeleteView;
    private StandardIconButton siBtnAddPipelineView;
    private PanelEx pnlExTop;
    private PanelEx pnlExBottom;
    private PanelEx pnlExPipelineLoanTab;
    private GradientPanel gradientPanel1;
    private ToolTip toolTip1;
    private Label lblPipelineViewNoAccess;
    private StandardIconButton siBtnEditPipelineView;
    private StandardIconButton siBtnMoveViewDown;
    private StandardIconButton siBtnMoveViewUp;

    public event EventHandler DirtyFlagChanged;

    public event PipelineConfiguration.PipelineConfigChanged FeatureStateChanged;

    public event PipelineConfiguration.PersonaAccess HasContactOriginateLoanAccessEvent;

    public PipelineConfiguration(
      Sessions.Session session,
      int personaId,
      EventHandler dirtyFlagChanged)
    {
      this.session = session;
      this.featureMgr = (FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features);
      this.personaID = personaId;
      this.pipelineLoanTabPage = new PipelineLoanTabPage(this.session, personaId, dirtyFlagChanged);
      this.pipelineLoanTabPage.FeatureStateChanged += new PipelineLoanTabPage.FeatureStatusChanged(this.pipelineLoanTabPage_FeatureStateChanged);
      this.pipelineLoanTabPage.HasContactOriginateLoanAccessEvent += new EllieMae.EMLite.Setup.PersonaSetup.SecurityPage.PersonaAccess(this.pipelineLoanTabPage_HasContactOriginateLoanAccessEvent);
      this.pipelinePage = new PipelinePage(this.session, personaId, this, dirtyFlagChanged);
      this.init(dirtyFlagChanged);
      this.gvColumns.ContextMenuStrip = (ContextMenuStrip) null;
    }

    public PipelineConfiguration(
      Sessions.Session session,
      string userID,
      Persona[] personas,
      EventHandler dirtyFlagChanged)
    {
      this.session = session;
      this.userID = userID;
      this.personas = personas;
      this.pipelineLoanTabPage = new PipelineLoanTabPage(this.session, userID, personas, dirtyFlagChanged);
      this.pipelineLoanTabPage.FeatureStateChanged += new PipelineLoanTabPage.FeatureStatusChanged(this.pipelineLoanTabPage_FeatureStateChanged);
      this.pipelineLoanTabPage.HasContactOriginateLoanAccessEvent += new EllieMae.EMLite.Setup.PersonaSetup.SecurityPage.PersonaAccess(this.pipelineLoanTabPage_HasContactOriginateLoanAccessEvent);
      this.pipelinePage = new PipelinePage(this.session, userID, personas, this, dirtyFlagChanged);
      this.init(dirtyFlagChanged);
    }

    private bool pipelineLoanTabPage_HasContactOriginateLoanAccessEvent()
    {
      return this.HasContactOriginateLoanAccessEvent != null && this.HasContactOriginateLoanAccessEvent();
    }

    public void pipelineLoanTabPage_FeatureStateChanged(
      AclFeature feature,
      AclTriState state,
      bool gotoContact)
    {
      if (feature != AclFeature.GlobalTab_Pipeline)
        return;
      if (state == AclTriState.True)
      {
        this.disableAccess(false);
        this.hideControl(false);
      }
      else
      {
        this.disableAccess(true);
        this.hideControl(true);
      }
      if (this.FeatureStateChanged == null)
        return;
      this.FeatureStateChanged(feature, state, gotoContact);
    }

    private void hideControl(bool hide)
    {
      this.pnlExBottom.Visible = !hide;
      this.lblPipelineViewNoAccess.Visible = hide;
      this.pipelinePage.HideControl(hide);
    }

    private void init(EventHandler dirtyFlagChanged)
    {
      this.InitializeComponent();
      this.pipelineLoanTabPage.Visible = true;
      this.pipelineLoanTabPage.ShowGroupContainer = false;
      this.pipelineLoanTabPage.TopLevel = false;
      this.pipelineLoanTabPage.Dock = DockStyle.Fill;
      this.pnlExPipelineLoanTab.Controls.Add((Control) this.pipelineLoanTabPage);
      this.pipelinePage.TopLevel = false;
      this.pipelinePage.Visible = true;
      this.pipelinePage.Dock = DockStyle.Fill;
      this.pnlExRight.Controls.Add((Control) this.pipelinePage);
      this.DirtyFlagChanged += dirtyFlagChanged;
      this.fieldAccessMgr = (FieldAccessAclManager) this.session.ACL.GetAclManager(AclCategory.FieldAccess);
      this.pipelineViewMgr = (PipelineViewAclManager) this.session.ACL.GetAclManager(AclCategory.PersonaPipelineView);
      this.loanFieldDefs = LoanReportFieldDefs.GetLoanReportFieldDefs(this.session).GetFieldDefsI(LoanReportFieldFlags.AllDatabaseFields, false, this.session);
      this.loadPipelineData(true);
      this.pipelineLoanTabPage_FeatureStateChanged(AclFeature.GlobalTab_Pipeline, this.pipelineLoanTabPage.HasAccessToPipelineLoanTab() ? AclTriState.True : AclTriState.False, false);
    }

    private void loadPipelineData(bool syncFields)
    {
      if (this.userID == "")
      {
        this.accessColList = this.fieldAccessMgr.GetFieldIDsPermission(this.personaID, true);
      }
      else
      {
        this.accessColList_User = this.fieldAccessMgr.GetFieldIDsPermission(this.userID);
        this.accessColList_Personas = this.fieldAccessMgr.GetFieldIDsPermission(this.personas, true);
      }
      if (syncFields)
        this.syncNewFields();
      this.loadNonAccessibleColumns();
      this.loadPipelineViews(!(this.userID == "") ? this.pipelineViewMgr.GetUserPipelineViews(this.userID) : this.pipelineViewMgr.GetPersonaPipelineViews(this.personaID));
    }

    public void SetPersona(int personaId)
    {
      if (this.personaID == personaId && !this.IsDirty)
        return;
      this.personaID = personaId;
      this.setStatus(false);
      this.pipelineLoanTabPage.SetPersona(personaId);
      AclTriState state = !this.featureMgr.GetPermission(AclFeature.GlobalTab_Pipeline, personaId) ? AclTriState.False : AclTriState.True;
      if (state == AclTriState.True)
        this.pipelineLoanTabPage_FeatureStateChanged(AclFeature.GlobalTab_Pipeline, state, false);
      this.pipelinePage.PersonaID = personaId;
      this.pipelinePage.SetPersona(personaId);
      this.loadPipelineData(true);
      if (state != AclTriState.False)
        return;
      this.pipelineLoanTabPage_FeatureStateChanged(AclFeature.GlobalTab_Pipeline, state, false);
    }

    public bool IsDirty
    {
      get
      {
        return this.pipelineLoanTabPage.NeedToSaveData() || this.pipelinePage.NeedToSaveData() || this.needToSaveData;
      }
    }

    public bool IsValid => true;

    public void Save()
    {
      if (!this.IsDirty)
        return;
      if (this.pipelinePage.NeedToSaveData())
        this.pipelinePage.Save();
      if (this.pipelineLoanTabPage.NeedToSaveData())
        this.pipelineLoanTabPage.Save();
      if (this.userID == "")
        this.fieldAccessMgr.SetFieldIDsPermission(this.personaID, this.accessColList);
      else
        this.fieldAccessMgr.SetFieldIDsPermission(this.userID, this.accessColList_User);
      if (this.userID == "")
        this.pipelineViewMgr.ReplacePersonaPipelineViews(this.personaID, this.getPipelineViews());
      this.setStatus(false);
    }

    public void Reset()
    {
      this.pipelinePage.Reset();
      this.pipelineLoanTabPage.Reset();
      this.loadPipelineData(false);
      this.setStatus(false);
    }

    private PersonaPipelineView[] getPipelineViews()
    {
      List<PersonaPipelineView> personaPipelineViewList = new List<PersonaPipelineView>();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvPipelineViews.Items)
        personaPipelineViewList.Add((PersonaPipelineView) gvItem.Tag);
      return personaPipelineViewList.ToArray();
    }

    private void disableAccess(bool makeReadOnly)
    {
      this.forcedReadOnly = makeReadOnly;
      this.pipelinePage.MakeReadOnly(makeReadOnly, false);
      this.cmsLink.Enabled = !makeReadOnly;
      this.siBtnFieldList.Enabled = !makeReadOnly;
      this.siBtnDelete.Enabled = !makeReadOnly;
      this.siBtnDeleteView.Enabled = !makeReadOnly;
      this.siBtnDeleteView.Visible = this.userID == "";
      this.siBtnAddPipelineView.Enabled = !makeReadOnly;
      this.siBtnAddPipelineView.Visible = this.userID == "";
      if (makeReadOnly)
        this.siBtnEditPipelineView.Enabled = false;
      this.siBtnEditPipelineView.Visible = this.userID == "";
      if (makeReadOnly)
        this.siBtnMoveViewDown.Enabled = false;
      this.siBtnMoveViewDown.Visible = this.userID == "";
      if (makeReadOnly)
        this.siBtnMoveViewUp.Enabled = false;
      this.siBtnMoveViewUp.Visible = this.userID == "";
      this.gvPipelineViews.Enabled = !makeReadOnly;
    }

    public void MakeReadOnly(bool makeReadOnly)
    {
      if (makeReadOnly)
      {
        this.pipelineLoanTabPage.MakeReadOnly(true);
        this.pipelinePage.MakeReadOnly(true);
        this.siBtnDelete.Enabled = false;
        this.siBtnFieldList.Enabled = false;
        this.cmsLink.Enabled = false;
      }
      else
      {
        this.pipelineLoanTabPage.MakeReadOnly(false);
        if (this.forcedReadOnly)
        {
          this.disableAccess(true);
          this.siBtnDelete.Enabled = false;
        }
        else
        {
          this.pipelinePage.MakeReadOnly(false);
          this.siBtnFieldList.Enabled = true;
          this.cmsLink.Enabled = true;
        }
      }
    }

    private void PipelineConfiguration_SizeChanged(object sender, EventArgs e)
    {
      if (!this.Visible)
        return;
      int num1 = (this.Width - 3) / 2;
      this.pnlExLeft.Width = num1 > 0 ? num1 : 10;
      int num2 = this.pnlExLeft.Height / 2;
      this.gcPipelineView.Height = num2 > 10 ? num2 : 10;
    }

    private void setStatus(bool dirty)
    {
      this.needToSaveData = dirty;
      if (this.DirtyFlagChanged == null)
        return;
      this.DirtyFlagChanged((object) this, (EventArgs) null);
    }

    private void loadPipelineViews(PersonaPipelineView[] views)
    {
      this.gvPipelineViews.Items.Clear();
      foreach (PersonaPipelineView view in views)
        this.gvPipelineViews.Items.Add(this.createGVItemForPipelineView(view));
    }

    private GVItem createGVItemForPipelineView(PersonaPipelineView view)
    {
      GVItem itemForPipelineView = new GVItem();
      itemForPipelineView.Tag = (object) view;
      this.populatePipelineViewGVItem(itemForPipelineView);
      return itemForPipelineView;
    }

    private void populatePipelineViewGVItem(GVItem item)
    {
      PersonaPipelineView tag = (PersonaPipelineView) item.Tag;
      item.SubItems[0].Text = tag.Name;
    }

    private void loadNonAccessibleColumns()
    {
      this.gvColumns.Items.Clear();
      if (this.userID == "")
      {
        foreach (string key in this.accessColList.Keys)
        {
          if (this.accessColList[key] != AclTriState.True)
          {
            LoanReportFieldDef fieldById = this.loanFieldDefs.GetFieldByID(key);
            if (fieldById != null)
              this.gvColumns.Items.Add(new GVItem(fieldById.Description)
              {
                SubItems = {
                  [1] = {
                    Value = (object) fieldById.FieldID
                  }
                },
                Tag = (object) fieldById
              });
          }
        }
      }
      else
      {
        foreach (string key in this.accessColList_Personas.Keys)
        {
          if (this.accessColList_User.ContainsKey(key))
          {
            if (this.accessColList_User[key] == AclTriState.True)
              continue;
          }
          else if (this.accessColList_Personas[key] == AclTriState.True)
            continue;
          LoanReportFieldDef fieldById = this.loanFieldDefs.GetFieldByID(key);
          if (fieldById != null)
            this.gvColumns.Items.Add(new GVItem()
            {
              SubItems = {
                [0] = {
                  Value = !this.accessColList_User.ContainsKey(key) ? (object) new PersonaLinkImg(fieldById.Name, true, true) : (object) new PersonaLinkImg(fieldById.Name, false, true)
                },
                [1] = {
                  Value = (object) fieldById.FieldID
                }
              },
              Tag = (object) fieldById
            });
        }
      }
    }

    private void siBtnFieldList_Click(object sender, EventArgs e)
    {
      if (this.userID == "")
      {
        using (FieldAccessDlg fieldAccessDlg = new FieldAccessDlg((ReportFieldDefs) this.loanFieldDefs, this.accessColList))
        {
          if (DialogResult.Cancel == fieldAccessDlg.ShowDialog((IWin32Window) this))
            return;
          this.setStatus(true);
          this.accessColList = fieldAccessDlg.AccessColumnList;
          this.loadNonAccessibleColumns();
        }
      }
      else
      {
        using (FieldAccessDlg fieldAccessDlg = new FieldAccessDlg((ReportFieldDefs) this.loanFieldDefs, this.accessColList_User, this.accessColList_Personas, false))
        {
          if (DialogResult.Cancel == fieldAccessDlg.ShowDialog((IWin32Window) this))
            return;
          this.setStatus(true);
          this.accessColList_User = fieldAccessDlg.AccessColumnList_User;
          this.loadNonAccessibleColumns();
        }
      }
    }

    private void siBtnDelete_Click(object sender, EventArgs e)
    {
      if (this.gvColumns.SelectedItems == null || this.gvColumns.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a field to remove.");
      }
      else
      {
        foreach (GVItem selectedItem in this.gvColumns.SelectedItems)
        {
          ReportFieldDef tag = (ReportFieldDef) selectedItem.Tag;
          if (this.userID == "")
          {
            if (this.accessColList.ContainsKey(tag.FieldID))
              this.accessColList[tag.FieldID] = AclTriState.True;
          }
          else if (this.accessColList_User.ContainsKey(tag.FieldID))
            this.accessColList_User[tag.FieldID] = AclTriState.True;
          else
            this.accessColList_User.Add(tag.FieldID, AclTriState.True);
        }
        this.loadNonAccessibleColumns();
        this.setStatus(true);
      }
    }

    private void syncNewFields()
    {
      Dictionary<string, string> dictionary1 = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
      Dictionary<string, string> dictionary2 = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
      Dictionary<string, AclTriState> dictionary3 = this.userID == "" ? this.accessColList : this.accessColList_Personas;
      foreach (ReportFieldDef loanFieldDef in (ReportFieldDefContainer) this.loanFieldDefs)
      {
        if (!dictionary3.ContainsKey(loanFieldDef.FieldID))
          dictionary1[loanFieldDef.FieldID] = loanFieldDef.FieldID;
      }
      foreach (string key in dictionary3.Keys)
      {
        if (this.loanFieldDefs.GetFieldByID(key) == null)
          dictionary2[key] = key;
      }
      if (dictionary1.Count <= 0 && dictionary2.Count <= 0)
        return;
      List<string> stringList1 = new List<string>((IEnumerable<string>) dictionary1.Keys);
      List<string> stringList2 = new List<string>((IEnumerable<string>) dictionary2.Keys);
      this.fieldAccessMgr.SyncFieldIDList(stringList1.ToArray(), stringList2.ToArray());
      if (this.userID == "")
      {
        this.accessColList = this.fieldAccessMgr.GetFieldIDsPermission(this.personaID);
      }
      else
      {
        this.accessColList_User = this.fieldAccessMgr.GetFieldIDsPermission(this.userID);
        this.accessColList_Personas = this.fieldAccessMgr.GetFieldIDsPermission(this.personas);
      }
    }

    private void tsmiLink_Click(object sender, EventArgs e)
    {
      if (this.gvColumns.SelectedItems.Count == 0)
        return;
      foreach (GVItem selectedItem in this.gvColumns.SelectedItems)
      {
        ReportFieldDef tag = (ReportFieldDef) selectedItem.Tag;
        if (this.accessColList_User.ContainsKey(tag.FieldID))
          this.accessColList_User.Remove(tag.FieldID);
      }
      this.setStatus(true);
      this.loadNonAccessibleColumns();
    }

    private void tsmiDisconnect_Click(object sender, EventArgs e)
    {
      if (this.gvColumns.SelectedItems.Count == 0)
        return;
      foreach (GVItem selectedItem in this.gvColumns.SelectedItems)
      {
        ReportFieldDef tag = (ReportFieldDef) selectedItem.Tag;
        if (this.accessColList_User.ContainsKey(tag.FieldID))
          this.accessColList_User[tag.FieldID] = AclTriState.False;
        else
          this.accessColList_User.Add(tag.FieldID, AclTriState.False);
      }
      this.setStatus(true);
      this.loadNonAccessibleColumns();
    }

    private void siBtnEditPipelineView_Click(object sender, EventArgs e)
    {
      this.editPipelineView(this.gvPipelineViews.SelectedItems[0]);
    }

    private void editPipelineView(GVItem viewItem)
    {
      PersonaPipelineView tag = (PersonaPipelineView) viewItem.Tag;
      using (PipelineViewEditor pipelineViewEditor = new PipelineViewEditor(this.session, tag, this.loanFieldDefs, this.getInUseViewNames(tag.Name)))
      {
        if (pipelineViewEditor.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.populatePipelineViewGVItem(viewItem);
        this.setStatus(true);
      }
    }

    private void siBtnAddPipelineView_Click(object sender, EventArgs e)
    {
      PersonaPipelineView view = (PersonaPipelineView) null;
      using (NewPipelineViewDialog pipelineViewDialog = new NewPipelineViewDialog(this.session, this.personaID))
      {
        if (pipelineViewDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        view = pipelineViewDialog.GetPipelineView();
      }
      using (PipelineViewEditor pipelineViewEditor = new PipelineViewEditor(this.session, view, this.loanFieldDefs, this.getInUseViewNames((string) null)))
      {
        if (pipelineViewEditor.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        int nItemIndex = this.gvPipelineViews.Items.Add(this.createGVItemForPipelineView(view));
        this.gvPipelineViews.SelectedItems.Clear();
        this.gvPipelineViews.Items[nItemIndex].Selected = true;
        this.setStatus(true);
      }
    }

    private void siBtnDeleteView_Click(object sender, EventArgs e)
    {
      if (this.gvPipelineViews.SelectedItems.Count == this.gvPipelineViews.Items.Count)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Each persona must have at least one Pipeline View.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        for (int nItemIndex = this.gvPipelineViews.Items.Count - 1; nItemIndex >= 0; --nItemIndex)
        {
          if (this.gvPipelineViews.Items[nItemIndex].Selected)
            this.gvPipelineViews.Items.RemoveAt(nItemIndex);
        }
        this.setStatus(true);
      }
    }

    private string[] getInUseViewNames(string nameToExclude)
    {
      List<string> stringList = new List<string>();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvPipelineViews.Items)
      {
        string name = ((PersonaPipelineView) gvItem.Tag).Name;
        if (nameToExclude == null || string.Compare(name, nameToExclude, true) != 0)
          stringList.Add(name);
      }
      return stringList.ToArray();
    }

    private void pnlExLeft_SizeChanged(object sender, EventArgs e)
    {
      this.gcPipelineView.Height = this.pnlExLeft.Height / 2;
    }

    public void GrandAccessToPipelineLoanTab()
    {
      this.pipelineLoanTabPage.GrandAccessToPipelineLoanTab();
    }

    public bool HasPipelineLoanTabAccess() => this.pipelineLoanTabPage.HasAccessToPipelineLoanTab();

    private void gvPipelineView_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!(this.userID == ""))
        return;
      int count = this.gvPipelineViews.SelectedItems.Count;
      this.siBtnDeleteView.Enabled = count > 0;
      this.siBtnEditPipelineView.Enabled = count == 1;
      this.siBtnMoveViewDown.Enabled = count > 0;
      this.siBtnMoveViewUp.Enabled = count > 0;
    }

    private void gvColumns_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.gvColumns.SelectedItems.Count > 0 && this.siBtnFieldList.Enabled)
        this.siBtnDelete.Enabled = true;
      else
        this.siBtnDelete.Enabled = false;
    }

    private void gvPipelineViews_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      if (!(this.userID == ""))
        return;
      this.editPipelineView(e.Item);
    }

    private void siBtnMoveViewUp_Click(object sender, EventArgs e)
    {
      for (int nItemIndex = 1; nItemIndex < this.gvPipelineViews.Items.Count; ++nItemIndex)
      {
        if (this.gvPipelineViews.Items[nItemIndex].Selected && !this.gvPipelineViews.Items[nItemIndex - 1].Selected)
        {
          GVItem gvItem = this.gvPipelineViews.Items[nItemIndex];
          this.gvPipelineViews.Items.RemoveAt(nItemIndex);
          this.gvPipelineViews.Items.Insert(nItemIndex - 1, gvItem);
          gvItem.Selected = true;
          this.setStatus(true);
        }
      }
    }

    private void siBtnMoveViewDown_Click(object sender, EventArgs e)
    {
      for (int nItemIndex = this.gvPipelineViews.Items.Count - 2; nItemIndex >= 0; --nItemIndex)
      {
        if (this.gvPipelineViews.Items[nItemIndex].Selected && !this.gvPipelineViews.Items[nItemIndex + 1].Selected)
        {
          GVItem gvItem = this.gvPipelineViews.Items[nItemIndex];
          this.gvPipelineViews.Items.RemoveAt(nItemIndex);
          this.gvPipelineViews.Items.Insert(nItemIndex + 1, gvItem);
          gvItem.Selected = true;
          this.setStatus(true);
        }
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
      GVColumn gvColumn3 = new GVColumn();
      this.pnlExLeft = new PanelEx();
      this.gcAccessibility = new GroupContainer();
      this.siBtnDelete = new StandardIconButton();
      this.siBtnFieldList = new StandardIconButton();
      this.gvColumns = new GridView();
      this.cmsLink = new ContextMenuStrip(this.components);
      this.tsmiLink = new ToolStripMenuItem();
      this.tsmiDisconnect = new ToolStripMenuItem();
      this.pnlExNote = new PanelEx();
      this.gradientPanel1 = new GradientPanel();
      this.label1 = new Label();
      this.splitter1 = new Splitter();
      this.gcPipelineView = new GroupContainer();
      this.siBtnMoveViewDown = new StandardIconButton();
      this.siBtnMoveViewUp = new StandardIconButton();
      this.siBtnEditPipelineView = new StandardIconButton();
      this.siBtnDeleteView = new StandardIconButton();
      this.siBtnAddPipelineView = new StandardIconButton();
      this.gvPipelineViews = new GridView();
      this.lblPipelineViewNoAccess = new Label();
      this.splitter2 = new Splitter();
      this.pnlExRight = new PanelEx();
      this.pnlExTop = new PanelEx();
      this.pnlExPipelineLoanTab = new PanelEx();
      this.pnlExBottom = new PanelEx();
      this.toolTip1 = new ToolTip(this.components);
      this.pnlExLeft.SuspendLayout();
      this.gcAccessibility.SuspendLayout();
      ((ISupportInitialize) this.siBtnDelete).BeginInit();
      ((ISupportInitialize) this.siBtnFieldList).BeginInit();
      this.cmsLink.SuspendLayout();
      this.pnlExNote.SuspendLayout();
      this.gradientPanel1.SuspendLayout();
      this.gcPipelineView.SuspendLayout();
      ((ISupportInitialize) this.siBtnMoveViewDown).BeginInit();
      ((ISupportInitialize) this.siBtnMoveViewUp).BeginInit();
      ((ISupportInitialize) this.siBtnEditPipelineView).BeginInit();
      ((ISupportInitialize) this.siBtnDeleteView).BeginInit();
      ((ISupportInitialize) this.siBtnAddPipelineView).BeginInit();
      this.pnlExTop.SuspendLayout();
      this.pnlExBottom.SuspendLayout();
      this.SuspendLayout();
      this.pnlExLeft.Controls.Add((Control) this.gcAccessibility);
      this.pnlExLeft.Controls.Add((Control) this.pnlExNote);
      this.pnlExLeft.Controls.Add((Control) this.splitter1);
      this.pnlExLeft.Controls.Add((Control) this.gcPipelineView);
      this.pnlExLeft.Dock = DockStyle.Left;
      this.pnlExLeft.Location = new Point(0, 0);
      this.pnlExLeft.Margin = new Padding(8, 7, 8, 7);
      this.pnlExLeft.Name = "pnlExLeft";
      this.pnlExLeft.Size = new Size(813, 1163);
      this.pnlExLeft.TabIndex = 0;
      this.pnlExLeft.SizeChanged += new EventHandler(this.pnlExLeft_SizeChanged);
      this.gcAccessibility.Controls.Add((Control) this.siBtnDelete);
      this.gcAccessibility.Controls.Add((Control) this.siBtnFieldList);
      this.gcAccessibility.Controls.Add((Control) this.gvColumns);
      this.gcAccessibility.Dock = DockStyle.Fill;
      this.gcAccessibility.HeaderForeColor = SystemColors.ControlText;
      this.gcAccessibility.Location = new Point(0, 610);
      this.gcAccessibility.Margin = new Padding(8, 7, 8, 7);
      this.gcAccessibility.Name = "gcAccessibility";
      this.gcAccessibility.Size = new Size(813, 491);
      this.gcAccessibility.TabIndex = 3;
      this.gcAccessibility.Text = "Not Accessible Columns*";
      this.siBtnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.siBtnDelete.BackColor = Color.Transparent;
      this.siBtnDelete.Enabled = false;
      this.siBtnDelete.Location = new Point(757, 10);
      this.siBtnDelete.Margin = new Padding(8, 7, 8, 7);
      this.siBtnDelete.MouseDownImage = (Image) null;
      this.siBtnDelete.Name = "siBtnDelete";
      this.siBtnDelete.Size = new Size(43, 38);
      this.siBtnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.siBtnDelete.TabIndex = 2;
      this.siBtnDelete.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.siBtnDelete, "Delete Column");
      this.siBtnDelete.Click += new EventHandler(this.siBtnDelete_Click);
      this.siBtnFieldList.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.siBtnFieldList.BackColor = Color.Transparent;
      this.siBtnFieldList.Enabled = false;
      this.siBtnFieldList.Location = new Point(701, 10);
      this.siBtnFieldList.Margin = new Padding(8, 7, 8, 7);
      this.siBtnFieldList.MouseDownImage = (Image) null;
      this.siBtnFieldList.Name = "siBtnFieldList";
      this.siBtnFieldList.Size = new Size(43, 38);
      this.siBtnFieldList.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.siBtnFieldList.TabIndex = 1;
      this.siBtnFieldList.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.siBtnFieldList, "Add Column");
      this.siBtnFieldList.Click += new EventHandler(this.siBtnFieldList_Click);
      this.gvColumns.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Field Description";
      gvColumn1.Width = 150;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Field ID";
      gvColumn2.Width = 100;
      this.gvColumns.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvColumns.ContextMenuStrip = this.cmsLink;
      this.gvColumns.Dock = DockStyle.Fill;
      this.gvColumns.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvColumns.Location = new Point(1, 26);
      this.gvColumns.Margin = new Padding(8, 7, 8, 7);
      this.gvColumns.Name = "gvColumns";
      this.gvColumns.Size = new Size(811, 464);
      this.gvColumns.TabIndex = 0;
      this.gvColumns.SelectedIndexChanged += new EventHandler(this.gvColumns_SelectedIndexChanged);
      this.cmsLink.ImageScalingSize = new Size(40, 40);
      this.cmsLink.Items.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.tsmiLink,
        (ToolStripItem) this.tsmiDisconnect
      });
      this.cmsLink.Name = "cmsLink";
      this.cmsLink.Size = new Size(519, 100);
      this.tsmiLink.Name = "tsmiLink";
      this.tsmiLink.Size = new Size(518, 48);
      this.tsmiLink.Text = "Link with Persona Rights";
      this.tsmiLink.Click += new EventHandler(this.tsmiLink_Click);
      this.tsmiDisconnect.Name = "tsmiDisconnect";
      this.tsmiDisconnect.Size = new Size(518, 48);
      this.tsmiDisconnect.Text = "Disconnect from Persona Rights";
      this.tsmiDisconnect.Click += new EventHandler(this.tsmiDisconnect_Click);
      this.pnlExNote.BackColor = Color.Transparent;
      this.pnlExNote.Controls.Add((Control) this.gradientPanel1);
      this.pnlExNote.Dock = DockStyle.Bottom;
      this.pnlExNote.Location = new Point(0, 1101);
      this.pnlExNote.Margin = new Padding(8, 7, 8, 7);
      this.pnlExNote.Name = "pnlExNote";
      this.pnlExNote.Size = new Size(813, 62);
      this.pnlExNote.TabIndex = 2;
      this.gradientPanel1.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gradientPanel1.Controls.Add((Control) this.label1);
      this.gradientPanel1.Dock = DockStyle.Fill;
      this.gradientPanel1.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel1.Location = new Point(0, 0);
      this.gradientPanel1.Margin = new Padding(8, 7, 8, 7);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(813, 25);
      this.gradientPanel1.Style = GradientPanel.PanelStyle.TableFooter;
      this.gradientPanel1.TabIndex = 1;
      this.label1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.label1.AutoEllipsis = true;
      this.label1.Location = new Point(13, 12);
      this.label1.Margin = new Padding(8, 0, 8, 0);
      this.label1.Name = "label1";
      this.label1.Size = new Size(755, 43);
      this.label1.TabIndex = 0;
      this.label1.Text = "*Also affects Trades, Contacts, Dashboard, and Reports.";
      this.splitter1.Dock = DockStyle.Top;
      this.splitter1.Location = new Point(0, 603);
      this.splitter1.Margin = new Padding(8, 7, 8, 7);
      this.splitter1.Name = "splitter1";
      this.splitter1.Size = new Size(813, 7);
      this.splitter1.TabIndex = 1;
      this.splitter1.TabStop = false;
      this.gcPipelineView.Controls.Add((Control) this.siBtnMoveViewDown);
      this.gcPipelineView.Controls.Add((Control) this.siBtnMoveViewUp);
      this.gcPipelineView.Controls.Add((Control) this.siBtnEditPipelineView);
      this.gcPipelineView.Controls.Add((Control) this.siBtnDeleteView);
      this.gcPipelineView.Controls.Add((Control) this.siBtnAddPipelineView);
      this.gcPipelineView.Controls.Add((Control) this.gvPipelineViews);
      this.gcPipelineView.Dock = DockStyle.Top;
      this.gcPipelineView.HeaderForeColor = SystemColors.ControlText;
      this.gcPipelineView.Location = new Point(0, 0);
      this.gcPipelineView.Margin = new Padding(8, 7, 8, 7);
      this.gcPipelineView.Name = "gcPipelineView";
      this.gcPipelineView.Size = new Size(813, 603);
      this.gcPipelineView.TabIndex = 0;
      this.gcPipelineView.Text = "Pipeline Views";
      this.siBtnMoveViewDown.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.siBtnMoveViewDown.BackColor = Color.Transparent;
      this.siBtnMoveViewDown.Enabled = false;
      this.siBtnMoveViewDown.Location = new Point(704, 10);
      this.siBtnMoveViewDown.Margin = new Padding(8, 7, 8, 7);
      this.siBtnMoveViewDown.MouseDownImage = (Image) null;
      this.siBtnMoveViewDown.Name = "siBtnMoveViewDown";
      this.siBtnMoveViewDown.Size = new Size(43, 38);
      this.siBtnMoveViewDown.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.siBtnMoveViewDown.TabIndex = 9;
      this.siBtnMoveViewDown.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.siBtnMoveViewDown, "Move View Down");
      this.siBtnMoveViewDown.Click += new EventHandler(this.siBtnMoveViewDown_Click);
      this.siBtnMoveViewUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.siBtnMoveViewUp.BackColor = Color.Transparent;
      this.siBtnMoveViewUp.Enabled = false;
      this.siBtnMoveViewUp.Location = new Point(648, 10);
      this.siBtnMoveViewUp.Margin = new Padding(8, 7, 8, 7);
      this.siBtnMoveViewUp.MouseDownImage = (Image) null;
      this.siBtnMoveViewUp.Name = "siBtnMoveViewUp";
      this.siBtnMoveViewUp.Size = new Size(43, 38);
      this.siBtnMoveViewUp.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.siBtnMoveViewUp.TabIndex = 8;
      this.siBtnMoveViewUp.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.siBtnMoveViewUp, "Move View Up");
      this.siBtnMoveViewUp.Click += new EventHandler(this.siBtnMoveViewUp_Click);
      this.siBtnEditPipelineView.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.siBtnEditPipelineView.BackColor = Color.Transparent;
      this.siBtnEditPipelineView.Enabled = false;
      this.siBtnEditPipelineView.Location = new Point(595, 10);
      this.siBtnEditPipelineView.Margin = new Padding(8, 7, 8, 7);
      this.siBtnEditPipelineView.MouseDownImage = (Image) null;
      this.siBtnEditPipelineView.Name = "siBtnEditPipelineView";
      this.siBtnEditPipelineView.Size = new Size(43, 38);
      this.siBtnEditPipelineView.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.siBtnEditPipelineView.TabIndex = 7;
      this.siBtnEditPipelineView.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.siBtnEditPipelineView, "Edit View");
      this.siBtnEditPipelineView.Click += new EventHandler(this.siBtnEditPipelineView_Click);
      this.siBtnDeleteView.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.siBtnDeleteView.BackColor = Color.Transparent;
      this.siBtnDeleteView.Enabled = false;
      this.siBtnDeleteView.Location = new Point(757, 10);
      this.siBtnDeleteView.Margin = new Padding(8, 7, 8, 7);
      this.siBtnDeleteView.MouseDownImage = (Image) null;
      this.siBtnDeleteView.Name = "siBtnDeleteView";
      this.siBtnDeleteView.Size = new Size(43, 38);
      this.siBtnDeleteView.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.siBtnDeleteView.TabIndex = 3;
      this.siBtnDeleteView.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.siBtnDeleteView, "Delete View");
      this.siBtnDeleteView.Click += new EventHandler(this.siBtnDeleteView_Click);
      this.siBtnAddPipelineView.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.siBtnAddPipelineView.BackColor = Color.Transparent;
      this.siBtnAddPipelineView.Location = new Point(539, 10);
      this.siBtnAddPipelineView.Margin = new Padding(8, 7, 8, 7);
      this.siBtnAddPipelineView.MouseDownImage = (Image) null;
      this.siBtnAddPipelineView.Name = "siBtnAddPipelineView";
      this.siBtnAddPipelineView.Size = new Size(43, 38);
      this.siBtnAddPipelineView.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.siBtnAddPipelineView.TabIndex = 2;
      this.siBtnAddPipelineView.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.siBtnAddPipelineView, "Add View");
      this.siBtnAddPipelineView.Click += new EventHandler(this.siBtnAddPipelineView_Click);
      this.gvPipelineViews.BorderStyle = BorderStyle.None;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column1";
      gvColumn3.SpringToFit = true;
      gvColumn3.Text = "Name";
      gvColumn3.Width = 303;
      this.gvPipelineViews.Columns.AddRange(new GVColumn[1]
      {
        gvColumn3
      });
      this.gvPipelineViews.Dock = DockStyle.Fill;
      this.gvPipelineViews.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvPipelineViews.Location = new Point(1, 26);
      this.gvPipelineViews.Margin = new Padding(8, 7, 8, 7);
      this.gvPipelineViews.Name = "gvPipelineViews";
      this.gvPipelineViews.Size = new Size(811, 576);
      this.gvPipelineViews.SortOption = GVSortOption.None;
      this.gvPipelineViews.TabIndex = 6;
      this.gvPipelineViews.SelectedIndexChanged += new EventHandler(this.gvPipelineView_SelectedIndexChanged);
      this.gvPipelineViews.ItemDoubleClick += new GVItemEventHandler(this.gvPipelineViews_ItemDoubleClick);
      this.lblPipelineViewNoAccess.BackColor = Color.Transparent;
      this.lblPipelineViewNoAccess.Dock = DockStyle.Top;
      this.lblPipelineViewNoAccess.Location = new Point(0, 88);
      this.lblPipelineViewNoAccess.Margin = new Padding(8, 0, 8, 0);
      this.lblPipelineViewNoAccess.Name = "lblPipelineViewNoAccess";
      this.lblPipelineViewNoAccess.Size = new Size(1677, 72);
      this.lblPipelineViewNoAccess.TabIndex = 0;
      this.lblPipelineViewNoAccess.Text = "The persona does not have access to the Pipeline, Loan, Forms and Tools, ePass tabs.";
      this.lblPipelineViewNoAccess.TextAlign = ContentAlignment.MiddleLeft;
      this.lblPipelineViewNoAccess.Visible = false;
      this.splitter2.Location = new Point(813, 0);
      this.splitter2.Margin = new Padding(8, 7, 8, 7);
      this.splitter2.Name = "splitter2";
      this.splitter2.Size = new Size(8, 1163);
      this.splitter2.TabIndex = 1;
      this.splitter2.TabStop = false;
      this.pnlExRight.Dock = DockStyle.Fill;
      this.pnlExRight.Location = new Point(821, 0);
      this.pnlExRight.Margin = new Padding(8, 7, 8, 7);
      this.pnlExRight.Name = "pnlExRight";
      this.pnlExRight.Size = new Size(856, 1163);
      this.pnlExRight.TabIndex = 2;
      this.pnlExTop.Controls.Add((Control) this.pnlExPipelineLoanTab);
      this.pnlExTop.Dock = DockStyle.Top;
      this.pnlExTop.Location = new Point(0, 0);
      this.pnlExTop.Margin = new Padding(8, 7, 8, 7);
      this.pnlExTop.Name = "pnlExTop";
      this.pnlExTop.Size = new Size(1677, 88);
      this.pnlExTop.TabIndex = 0;
      this.pnlExPipelineLoanTab.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlExPipelineLoanTab.Location = new Point(0, 26);
      this.pnlExPipelineLoanTab.Margin = new Padding(8, 7, 8, 7);
      this.pnlExPipelineLoanTab.Name = "pnlExPipelineLoanTab";
      this.pnlExPipelineLoanTab.Size = new Size(1677, 48);
      this.pnlExPipelineLoanTab.TabIndex = 0;
      this.pnlExBottom.Controls.Add((Control) this.pnlExRight);
      this.pnlExBottom.Controls.Add((Control) this.splitter2);
      this.pnlExBottom.Controls.Add((Control) this.pnlExLeft);
      this.pnlExBottom.Dock = DockStyle.Fill;
      this.pnlExBottom.Location = new Point(0, 160);
      this.pnlExBottom.Margin = new Padding(8, 7, 8, 7);
      this.pnlExBottom.Name = "pnlExBottom";
      this.pnlExBottom.Size = new Size(1677, 1163);
      this.pnlExBottom.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(16f, 31f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.Controls.Add((Control) this.pnlExBottom);
      this.Controls.Add((Control) this.lblPipelineViewNoAccess);
      this.Controls.Add((Control) this.pnlExTop);
      this.Margin = new Padding(8, 7, 8, 7);
      this.Name = nameof (PipelineConfiguration);
      this.Size = new Size(1677, 1323);
      this.SizeChanged += new EventHandler(this.PipelineConfiguration_SizeChanged);
      this.pnlExLeft.ResumeLayout(false);
      this.gcAccessibility.ResumeLayout(false);
      ((ISupportInitialize) this.siBtnDelete).EndInit();
      ((ISupportInitialize) this.siBtnFieldList).EndInit();
      this.cmsLink.ResumeLayout(false);
      this.pnlExNote.ResumeLayout(false);
      this.gradientPanel1.ResumeLayout(false);
      this.gcPipelineView.ResumeLayout(false);
      ((ISupportInitialize) this.siBtnMoveViewDown).EndInit();
      ((ISupportInitialize) this.siBtnMoveViewUp).EndInit();
      ((ISupportInitialize) this.siBtnEditPipelineView).EndInit();
      ((ISupportInitialize) this.siBtnDeleteView).EndInit();
      ((ISupportInitialize) this.siBtnAddPipelineView).EndInit();
      this.pnlExTop.ResumeLayout(false);
      this.pnlExBottom.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public delegate void PipelineConfigChanged(
      AclFeature feature,
      AclTriState state,
      bool gotoContact);

    public delegate bool PersonaAccess();
  }
}
