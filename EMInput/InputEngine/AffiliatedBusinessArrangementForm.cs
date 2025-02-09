// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.AffiliatedBusinessArrangementForm
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Calendar;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.Encompass.AsmResolver;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class AffiliatedBusinessArrangementForm : 
    UserControl,
    IMainScreen,
    IWin32Window,
    IApplicationWindow,
    ISynchronizeInvoke,
    IRefreshContents,
    IOnlineHelpTarget
  {
    private const string className = "AffiliatedBusinessArrangementForm";
    private static readonly string sw = Tracing.SwOutsideLoan;
    private IHtmlInput iData;
    private bool readOnly;
    private LoanScreen detailScreen;
    private LoanData loan;
    private Sessions.Session session;
    private IContainer components;
    private PanelEx panelExDetail;
    private GroupContainer groupContainerList;
    private GroupContainer groupContainerDetail;
    private GridView gridViewProviders;
    private StandardIconButton btnIconDelete;
    private StandardIconButton btnIconDown;
    private StandardIconButton btnIconUp;
    private StandardIconButton btnIconAdd;
    private Panel panelDetail;
    private CollapsibleSplitter collapsibleSplitter1;
    private PanelEx panelExList;
    private Button btnApply;
    private VerticalSeparator verticalSeparator1;

    public AffiliatedBusinessArrangementForm(
      IHtmlInput iData,
      bool readOnly,
      Sessions.Session session)
    {
      this.iData = iData;
      this.readOnly = readOnly;
      this.session = session;
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      this.loan = !(this.iData is LoanData) ? this.loadTemplate() : (LoanData) this.iData;
      this.initForm();
      if (this.readOnly)
        this.btnIconAdd.Enabled = this.btnIconDelete.Enabled = this.btnIconDown.Enabled = this.btnIconUp.Enabled = false;
      if (!readOnly && !this.loan.IsTemplate)
        return;
      this.changeButtonStatus(true);
    }

    public void RefreshContents() => this.initForm();

    private void changeButtonStatus(bool hideApply)
    {
      this.verticalSeparator1.Visible = this.btnApply.Visible = !hideApply;
      if (hideApply)
        this.btnIconDelete.Left = this.btnApply.Left + this.btnApply.Width - this.btnIconDelete.Width;
      this.btnIconDown.Left = this.btnIconDelete.Left - this.btnIconDown.Width - 7;
      this.btnIconUp.Left = this.btnIconDown.Left - this.btnIconUp.Width - 7;
      this.btnIconAdd.Left = this.btnIconUp.Left - this.btnIconAdd.Width - 7;
    }

    public void RefreshLoanContents() => this.initForm();

    private void initForm()
    {
      this.gridViewProviders.Items.Clear();
      if (this.loan != null)
      {
        int numberOfAffiliates = this.loan.GetNumberOfAffiliates();
        if (numberOfAffiliates > 0)
        {
          this.gridViewProviders.BeginUpdate();
          for (int index = 0; index < numberOfAffiliates; ++index)
            this.gridViewProviders.Items.Add(this.buildListViewItem(index + 1, false));
          this.gridViewProviders.EndUpdate();
          this.gridViewProviders.Items[0].Selected = true;
        }
      }
      this.gridViewProviders_SelectedIndexChanged((object) null, (EventArgs) null);
    }

    private void gridViewProviders_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnIconUp.Enabled = !this.readOnly && this.gridViewProviders.SelectedItems.Count == 1 && this.gridViewProviders.SelectedItems[0].Index > 0;
      this.btnIconDown.Enabled = !this.readOnly && this.gridViewProviders.SelectedItems.Count == 1 && this.gridViewProviders.SelectedItems[0].Index < this.gridViewProviders.Items.Count - 1;
      this.btnIconDelete.Enabled = !this.readOnly && this.gridViewProviders.SelectedItems.Count == 1;
      if (this.gridViewProviders.SelectedItems.Count == 0)
        return;
      if (this.detailScreen != null)
        this.detailScreen.BrowserHandler.UpdateCurrentField();
      this.openAffiliatedBusinessArrangement(this.gridViewProviders.SelectedItems[0].Index);
    }

    private void btnIconAdd_Click(object sender, EventArgs e)
    {
      int newIndex = this.loan.NewAffiliate();
      if (newIndex <= -1)
        return;
      this.gridViewProviders.SelectedItems.Clear();
      this.gridViewProviders.Items.Add(this.buildListViewItem(newIndex, true));
      this.gridViewProviders.EnsureVisible(this.gridViewProviders.SelectedItems[0].Index);
      this.editAffiliatedBusinessArrangement();
    }

    private void editAffiliatedBusinessArrangement()
    {
      if (this.gridViewProviders.SelectedItems.Count == 0)
        return;
      this.openAffiliatedBusinessArrangement(this.gridViewProviders.SelectedItems[0].Index);
      this.gridViewProviders.Focus();
    }

    private void openAffiliatedBusinessArrangement(int i)
    {
      if (i < 0)
        return;
      if (this.detailScreen == null)
      {
        this.detailScreen = new LoanScreen(this.session, !(this.iData is LoanData) || this.loan.IsInFindFieldForm ? (IWin32Window) this.ParentForm : (IWin32Window) this, (IHtmlInput) this.loan);
        this.detailScreen.RemoveTitle();
        this.detailScreen.RemoveBorder();
        this.panelDetail.Controls.Add((Control) this.detailScreen);
        this.detailScreen.BrowserHandler.DocumentCompleted += new DocumentCompleteEventHandler(this.browserHandler_DocumentCompleted);
        this.detailScreen.BrowserHandler.SetHelpTarget((IOnlineHelpTarget) this);
        this.detailScreen.LoadForm(new InputFormInfo("AFFILIATEDBA", "Affiliated Business Arrangements"));
      }
      else
      {
        this.detailScreen.Visible = true;
        this.detailScreen.BringToFront();
      }
      this.detailScreen.BrowserHandler.Property = (object) (i + 1);
      this.detailScreen.RefreshLoanContents();
      this.detailScreen.RefreshToolTips();
      if (this.readOnly)
        return;
      ((InputHandlerBase) this.detailScreen.BrowserHandler.GetInputHandler())?.ApplyFieldAccessRights(false);
      this.detailScreen.BrowserHandler.SetGoToFieldFocus("AB0001", 0);
    }

    private void browserHandler_DocumentCompleted()
    {
      AFFILIATEDBAInputHandler inputHandler = (AFFILIATEDBAInputHandler) this.detailScreen.BrowserHandler.GetInputHandler();
      if (inputHandler == null)
        return;
      inputHandler.VerifSummaryChanged += new VerifSummaryChangedEventHandler(this.summaryInfoChangedHandler);
      if (!this.readOnly)
        return;
      inputHandler.FieldsReadOnly = true;
      inputHandler.SetFieldReadOnly();
    }

    private GVItem buildListViewItem(int newIndex, bool selected)
    {
      string str = newIndex.ToString("00");
      return new GVItem(string.Concat((object) newIndex))
      {
        SubItems = {
          (object) this.loan.GetField("AB" + str + "06"),
          (object) this.loan.GetField("AB" + str + "07"),
          (object) this.loan.GetField("AB" + str + "10"),
          (object) this.loan.GetField("AB" + str + "22"),
          (object) this.loan.GetField("AB" + str + "08")
        },
        Selected = selected
      };
    }

    private void summaryInfoChangedHandler(VerifSummaryChangeInfo info)
    {
      int nItemIndex = Utils.ParseInt(this.detailScreen.BrowserHandler.Property) - 1;
      switch (info.ItemName)
      {
        case "AB0006":
          this.gridViewProviders.Items[nItemIndex].SubItems[1].Text = info.ItemValue.ToString();
          break;
        case "AB0007":
          this.gridViewProviders.Items[nItemIndex].SubItems[2].Text = info.ItemValue.ToString();
          break;
        case "AB0010":
          this.gridViewProviders.Items[nItemIndex].SubItems[3].Text = info.ItemValue.ToString();
          break;
        case "AB0022":
          this.gridViewProviders.Items[nItemIndex].SubItems[4].Text = info.ItemValue.ToString();
          break;
        case "AB0008":
          this.gridViewProviders.Items[nItemIndex].SubItems[5].Text = info.ItemValue.ToString();
          break;
      }
    }

    public void ShowHelp(Control control)
    {
    }

    public void ShowHelp(Control control, string helpTargetName)
    {
    }

    public void ShowLeadCenter()
    {
    }

    public void ShowCalendar(
      IWin32Window owner,
      string userID,
      CSMessage.AccessLevel accessLevel,
      bool accessUpdate)
    {
    }

    public bool AllowCalendarSharing() => false;

    public void SwitchToOrgUserSetup(string userid)
    {
    }

    public void SwitchToExternalOrgUserSetup(string userid)
    {
    }

    public void NavigateHome(string url)
    {
    }

    public void NavigateToContact(CategoryType contactType)
    {
    }

    public void NavigateToContact(ContactInfo selectedContact)
    {
    }

    public void AddNewBorrowerToContactManagerList(int contactID)
    {
    }

    public void HandleCEMessage(CEMessage message)
    {
    }

    public void OpenURL(string url, string title, int width, int height)
    {
    }

    public Form OpenURL(string windowName, string url, string title, int width, int height)
    {
      return (Form) null;
    }

    public bool IsClientEnabledToExportFNMFRE => false;

    public bool IsUnderwriterSummaryAccessibleForBroker => false;

    public void RefreshCE()
    {
    }

    public void NavigateToTradesTab(int tradeId)
    {
    }

    private void btnIconDelete_Click(object sender, EventArgs e)
    {
      if (this.gridViewProviders.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You have to select a affiliate first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (Utils.Dialog((IWin32Window) this, "Are you sure you want to delete this affiliate?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
          return;
        int index = this.gridViewProviders.SelectedItems[0].Index;
        if (!this.loan.RemoveAffiliateAt(index))
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "This affiliate can't be deleted.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          this.gridViewProviders.Items.RemoveAt(index);
          this.gridViewProviders.SelectedItems.Clear();
          if (this.gridViewProviders.Items.Count == 0)
          {
            this.detailScreen.Visible = false;
          }
          else
          {
            this.gridViewProviders.BeginUpdate();
            for (int nItemIndex = index; nItemIndex < this.gridViewProviders.Items.Count; ++nItemIndex)
              this.gridViewProviders.Items[nItemIndex].Text = string.Concat((object) (this.gridViewProviders.Items[nItemIndex].Index + 1));
            this.gridViewProviders.EndUpdate();
            if (index >= this.gridViewProviders.Items.Count)
              this.gridViewProviders.Items[this.gridViewProviders.Items.Count - 1].Selected = true;
            else
              this.gridViewProviders.Items[index].Selected = true;
          }
        }
      }
    }

    private void btnIconDown_Click(object sender, EventArgs e)
    {
      if (this.gridViewProviders.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You have to select a affiliate first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (!this.loan.IsTemplate && !this.loan.IsInFindFieldForm && this.session.LoanDataMgr.Writable && this.session.SessionObjects.AllowConcurrentEditing && !this.session.LoanDataMgr.LockLoanWithExclusiveA(true))
          return;
        int index = this.gridViewProviders.SelectedItems[0].Index;
        if (index >= this.gridViewProviders.Items.Count - 1)
          return;
        if (!this.loan.DownAffiliate(index))
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "This affiliate can't be moved down.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          string empty = string.Empty;
          for (int nItemIndex = 1; nItemIndex < this.gridViewProviders.Columns.Count; ++nItemIndex)
          {
            string text = this.gridViewProviders.Items[index].SubItems[nItemIndex].Text;
            this.gridViewProviders.Items[index].SubItems[nItemIndex].Text = this.gridViewProviders.Items[index + 1].SubItems[nItemIndex].Text;
            this.gridViewProviders.Items[index + 1].SubItems[nItemIndex].Text = text;
          }
          this.gridViewProviders.SelectedItems.Clear();
          this.gridViewProviders.Items[index + 1].Selected = true;
        }
      }
    }

    private void btnIconUp_Click(object sender, EventArgs e)
    {
      if (this.gridViewProviders.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You have to select a affiliate first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (!this.loan.IsTemplate && !this.loan.IsInFindFieldForm && this.session.LoanDataMgr.Writable && this.session.SessionObjects.AllowConcurrentEditing && !this.session.LoanDataMgr.LockLoanWithExclusiveA(true))
          return;
        int index = this.gridViewProviders.SelectedItems[0].Index;
        if (index == 0)
          return;
        if (!this.loan.UpAffiliate(index))
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "This affiliate can't be moved up.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          string empty = string.Empty;
          for (int nItemIndex = 1; nItemIndex < this.gridViewProviders.Columns.Count; ++nItemIndex)
          {
            string text = this.gridViewProviders.Items[index].SubItems[nItemIndex].Text;
            this.gridViewProviders.Items[index].SubItems[nItemIndex].Text = this.gridViewProviders.Items[index - 1].SubItems[nItemIndex].Text;
            this.gridViewProviders.Items[index - 1].SubItems[nItemIndex].Text = text;
          }
          this.gridViewProviders.SelectedItems.Clear();
          this.gridViewProviders.Items[index - 1].Selected = true;
        }
      }
    }

    private LoanData loadTemplate()
    {
      AffiliateTemplate iData = (AffiliateTemplate) this.iData;
      LoanData loanData;
      try
      {
        FileStream fileStream = File.OpenRead(AssemblyResolver.GetResourceFileFullPath("Documents\\Templates\\BlankLoan\\BlankData.XML", SystemSettings.LocalAppDir));
        byte[] numArray = new byte[fileStream.Length];
        fileStream.Read(numArray, 0, numArray.Length);
        fileStream.Close();
        loanData = new LoanData(Encoding.ASCII.GetString(numArray), this.session.SessionObjects.LoanManager.GetLoanConfigurationInfo().LoanSettings, false);
      }
      catch (Exception ex)
      {
        Tracing.Log(AffiliatedBusinessArrangementForm.sw, TraceLevel.Error, nameof (AffiliatedBusinessArrangementForm), "Error opening BlankLoan template. Message: " + ex.Message);
        return (LoanData) null;
      }
      foreach (string assignedFieldId in iData.GetAssignedFieldIDs())
        loanData.SetCurrentField(assignedFieldId, iData.GetField(assignedFieldId));
      loanData.IsTemplate = true;
      return loanData;
    }

    public bool SetTemplate(AffiliateTemplate template)
    {
      int numberOfAffiliates = this.loan.GetNumberOfAffiliates();
      if (numberOfAffiliates == 0)
        return false;
      template.ClearAllFields();
      for (int index1 = 1; index1 <= numberOfAffiliates; ++index1)
      {
        for (int index2 = 1; index2 <= 28; ++index2)
        {
          string id = "AB" + (index1 > 99 ? index1.ToString("000") : index1.ToString("00")) + index2.ToString("00");
          template.SetCurrentField(id, this.loan.GetSimpleField(id));
        }
      }
      return true;
    }

    public string GetHelpTargetName()
    {
      return this.iData is LoanData ? "Affiliated Business Arrangements" : "Setup\\Affiliates";
    }

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, this.GetHelpTargetName());
    }

    private void btnApply_Click(object sender, EventArgs e)
    {
      if (!this.session.Application.GetService<ILoanEditor>().SelectAffilatesTemplate())
        return;
      this.initForm();
    }

    private void Form_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      this.ShowHelp();
    }

    public void DisplayTPOCompanySetting(ExternalOriginatorManagementData o)
    {
      throw new Exception("The method or operation is not implemented.");
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
      this.panelExList = new PanelEx();
      this.groupContainerList = new GroupContainer();
      this.verticalSeparator1 = new VerticalSeparator();
      this.btnApply = new Button();
      this.btnIconDelete = new StandardIconButton();
      this.btnIconDown = new StandardIconButton();
      this.btnIconUp = new StandardIconButton();
      this.btnIconAdd = new StandardIconButton();
      this.gridViewProviders = new GridView();
      this.collapsibleSplitter1 = new CollapsibleSplitter();
      this.panelExDetail = new PanelEx();
      this.groupContainerDetail = new GroupContainer();
      this.panelDetail = new Panel();
      this.panelExList.SuspendLayout();
      this.groupContainerList.SuspendLayout();
      ((ISupportInitialize) this.btnIconDelete).BeginInit();
      ((ISupportInitialize) this.btnIconDown).BeginInit();
      ((ISupportInitialize) this.btnIconUp).BeginInit();
      ((ISupportInitialize) this.btnIconAdd).BeginInit();
      this.panelExDetail.SuspendLayout();
      this.groupContainerDetail.SuspendLayout();
      this.SuspendLayout();
      this.panelExList.Controls.Add((Control) this.groupContainerList);
      this.panelExList.Dock = DockStyle.Fill;
      this.panelExList.Location = new Point(0, 0);
      this.panelExList.Name = "panelExList";
      this.panelExList.Size = new Size(700, 393);
      this.panelExList.TabIndex = 4;
      this.groupContainerList.Controls.Add((Control) this.verticalSeparator1);
      this.groupContainerList.Controls.Add((Control) this.btnApply);
      this.groupContainerList.Controls.Add((Control) this.btnIconDelete);
      this.groupContainerList.Controls.Add((Control) this.btnIconDown);
      this.groupContainerList.Controls.Add((Control) this.btnIconUp);
      this.groupContainerList.Controls.Add((Control) this.btnIconAdd);
      this.groupContainerList.Controls.Add((Control) this.gridViewProviders);
      this.groupContainerList.Dock = DockStyle.Fill;
      this.groupContainerList.HeaderForeColor = SystemColors.ControlText;
      this.groupContainerList.Location = new Point(0, 0);
      this.groupContainerList.Name = "groupContainerList";
      this.groupContainerList.Size = new Size(700, 393);
      this.groupContainerList.TabIndex = 0;
      this.groupContainerList.Text = "Affiliated Business Arrangements";
      this.verticalSeparator1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.verticalSeparator1.Location = new Point(597, 5);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 6;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.btnApply.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnApply.Location = new Point(603, 2);
      this.btnApply.Name = "btnApply";
      this.btnApply.Size = new Size(93, 21);
      this.btnApply.TabIndex = 5;
      this.btnApply.Text = "Apply Template";
      this.btnApply.UseVisualStyleBackColor = true;
      this.btnApply.Click += new EventHandler(this.btnApply_Click);
      this.btnIconDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnIconDelete.BackColor = Color.Transparent;
      this.btnIconDelete.Location = new Point(577, 5);
      this.btnIconDelete.MouseDownImage = (Image) null;
      this.btnIconDelete.Name = "btnIconDelete";
      this.btnIconDelete.Size = new Size(16, 16);
      this.btnIconDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnIconDelete.TabIndex = 4;
      this.btnIconDelete.TabStop = false;
      this.btnIconDelete.Click += new EventHandler(this.btnIconDelete_Click);
      this.btnIconDown.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnIconDown.BackColor = Color.Transparent;
      this.btnIconDown.Location = new Point(554, 5);
      this.btnIconDown.MouseDownImage = (Image) null;
      this.btnIconDown.Name = "btnIconDown";
      this.btnIconDown.Size = new Size(16, 16);
      this.btnIconDown.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.btnIconDown.TabIndex = 3;
      this.btnIconDown.TabStop = false;
      this.btnIconDown.Click += new EventHandler(this.btnIconDown_Click);
      this.btnIconUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnIconUp.BackColor = Color.Transparent;
      this.btnIconUp.Location = new Point(531, 5);
      this.btnIconUp.MouseDownImage = (Image) null;
      this.btnIconUp.Name = "btnIconUp";
      this.btnIconUp.Size = new Size(16, 16);
      this.btnIconUp.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.btnIconUp.TabIndex = 2;
      this.btnIconUp.TabStop = false;
      this.btnIconUp.Click += new EventHandler(this.btnIconUp_Click);
      this.btnIconAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnIconAdd.BackColor = Color.Transparent;
      this.btnIconAdd.Location = new Point(508, 5);
      this.btnIconAdd.MouseDownImage = (Image) null;
      this.btnIconAdd.Name = "btnIconAdd";
      this.btnIconAdd.Size = new Size(16, 16);
      this.btnIconAdd.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnIconAdd.TabIndex = 1;
      this.btnIconAdd.TabStop = false;
      this.btnIconAdd.Click += new EventHandler(this.btnIconAdd_Click);
      this.gridViewProviders.AllowMultiselect = false;
      this.gridViewProviders.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "";
      gvColumn1.Width = 30;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Affiliate Name";
      gvColumn2.Width = 200;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Nature of Relationship";
      gvColumn3.Width = 160;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Text = "Service Description";
      gvColumn4.Width = 200;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.Text = "Required Use";
      gvColumn5.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn5.Width = 80;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column6";
      gvColumn6.Text = "Settlement";
      gvColumn6.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn6.Width = 80;
      this.gridViewProviders.Columns.AddRange(new GVColumn[6]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6
      });
      this.gridViewProviders.Dock = DockStyle.Fill;
      this.gridViewProviders.Location = new Point(1, 26);
      this.gridViewProviders.Name = "gridViewProviders";
      this.gridViewProviders.Size = new Size(698, 366);
      this.gridViewProviders.SortOption = GVSortOption.None;
      this.gridViewProviders.TabIndex = 0;
      this.gridViewProviders.SelectedIndexChanged += new EventHandler(this.gridViewProviders_SelectedIndexChanged);
      this.collapsibleSplitter1.AnimationDelay = 20;
      this.collapsibleSplitter1.AnimationStep = 20;
      this.collapsibleSplitter1.BorderStyle3D = Border3DStyle.Flat;
      this.collapsibleSplitter1.ControlToHide = (Control) this.panelExDetail;
      this.collapsibleSplitter1.Cursor = Cursors.HSplit;
      this.collapsibleSplitter1.Dock = DockStyle.Bottom;
      this.collapsibleSplitter1.ExpandParentForm = false;
      this.collapsibleSplitter1.Location = new Point(0, 393);
      this.collapsibleSplitter1.Name = "collapsibleSplitter1";
      this.collapsibleSplitter1.TabIndex = 3;
      this.collapsibleSplitter1.TabStop = false;
      this.collapsibleSplitter1.UseAnimations = false;
      this.collapsibleSplitter1.VisualStyle = VisualStyles.Encompass;
      this.panelExDetail.Controls.Add((Control) this.groupContainerDetail);
      this.panelExDetail.Dock = DockStyle.Bottom;
      this.panelExDetail.Location = new Point(0, 400);
      this.panelExDetail.Name = "panelExDetail";
      this.panelExDetail.Size = new Size(700, 360);
      this.panelExDetail.TabIndex = 2;
      this.groupContainerDetail.Controls.Add((Control) this.panelDetail);
      this.groupContainerDetail.Dock = DockStyle.Fill;
      this.groupContainerDetail.HeaderForeColor = SystemColors.ControlText;
      this.groupContainerDetail.Location = new Point(0, 0);
      this.groupContainerDetail.Name = "groupContainerDetail";
      this.groupContainerDetail.Size = new Size(700, 360);
      this.groupContainerDetail.TabIndex = 0;
      this.groupContainerDetail.Text = "Affiliate";
      this.panelDetail.Dock = DockStyle.Fill;
      this.panelDetail.Location = new Point(1, 26);
      this.panelDetail.Name = "panelDetail";
      this.panelDetail.Size = new Size(698, 333);
      this.panelDetail.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.panelExList);
      this.Controls.Add((Control) this.collapsibleSplitter1);
      this.Controls.Add((Control) this.panelExDetail);
      this.Name = nameof (AffiliatedBusinessArrangementForm);
      this.Size = new Size(700, 760);
      this.KeyUp += new KeyEventHandler(this.Form_KeyUp);
      this.panelExList.ResumeLayout(false);
      this.groupContainerList.ResumeLayout(false);
      ((ISupportInitialize) this.btnIconDelete).EndInit();
      ((ISupportInitialize) this.btnIconDown).EndInit();
      ((ISupportInitialize) this.btnIconUp).EndInit();
      ((ISupportInitialize) this.btnIconAdd).EndInit();
      this.panelExDetail.ResumeLayout(false);
      this.groupContainerDetail.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
