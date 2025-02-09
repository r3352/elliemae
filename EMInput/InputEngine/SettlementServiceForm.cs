// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.SettlementServiceForm
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
  public class SettlementServiceForm : 
    UserControl,
    IMainScreen,
    IWin32Window,
    IApplicationWindow,
    ISynchronizeInvoke,
    IRefreshContents,
    IOnlineHelpTarget
  {
    private const string className = "SettlementServiceForm";
    private static readonly string sw = Tracing.SwOutsideLoan;
    private IHtmlInput iData;
    private bool readOnly;
    private LoanScreen detailScreen;
    private LoanData loan;
    private Sessions.Session session;
    private bool snapshot;
    private WinFormInputHandler inputHandler;
    private Control[] fieldControlList;
    private ToolTip tooltipAdditionalText;
    private IContainer components;
    private PanelEx panelExDetail;
    private GroupContainer groupContainerList;
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
    private Panel panelMiddle;
    private Panel panelList;
    private GradientPanel gradientPanel3;
    private Label label3;
    private Panel panelTop;
    private TextBox txtAdditionalInfo;
    private GradientPanel gradientPanel2;
    private Label label2;
    private Panel panelRight;
    private GradientPanel gradientPanel1;
    private Label label1;
    private Panel panelServiceDetail;
    private GradientPanel gradientPanel4;
    private Label label4;
    private FieldLockButton lBtnDateIssued;
    private DatePicker dpDateIssued;
    private Panel panel1;

    public SettlementServiceForm(
      IHtmlInput iData,
      bool readOnly,
      Sessions.Session session,
      bool snapshot = false)
    {
      this.iData = iData;
      this.readOnly = readOnly;
      this.session = session;
      this.snapshot = snapshot;
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      this.loan = !(this.iData is LoanData) ? (!(this.iData is DisclosedSSPLHandler) ? this.loadTemplate() : Session.LoanData) : (LoanData) this.iData;
      if (snapshot)
        this.initSnapshot();
      else
        this.initForm();
      if (this.readOnly)
        this.btnIconAdd.Enabled = this.btnIconDelete.Enabled = this.btnIconDown.Enabled = this.btnIconUp.Enabled = false;
      if (readOnly || this.loan.IsTemplate)
        this.changeButtonStatus(true);
      if (this.loan.IsTemplate || !this.loan.Use2015RESPA)
      {
        this.rearrangeUIElementsForTemplateOr2010();
      }
      else
      {
        this.inputHandler = WinFormInputHandler.Create(this.loan);
        this.inputHandler.Attach(this.txtAdditionalInfo.Parent);
        this.tooltipAdditionalText = new ToolTip();
        this.fieldControlList = new Control[2]
        {
          (Control) this.txtAdditionalInfo,
          (Control) this.dpDateIssued
        };
        foreach (Control fieldControl in this.fieldControlList)
          this.setToolTip(fieldControl);
        if (this.loan.IsLocked("SP.DATEISSUED"))
        {
          this.toggleDateIssuedLock();
          this.dpDateIssued.Value = Utils.ParseDate((object) this.loan.GetField("SP.DATEISSUED"));
        }
        else
          this.dpDateIssued.Value = Utils.ParseDate((object) this.loan.GetField("LE1.X1"));
      }
    }

    private void setToolTip(Control ctl)
    {
      if (ctl is DatePicker)
      {
        DatePicker datePicker = (DatePicker) ctl;
        string helpKey = string.Concat(datePicker.Tag);
        datePicker.ToolTip = helpKey + ": " + FieldHelp.GetText(helpKey);
      }
      if (!(ctl is TextBox))
        return;
      TextBox textBox = (TextBox) ctl;
      string helpKey1 = string.Concat(textBox.Tag);
      if (!(textBox.Name == "txtAdditionalInfo"))
        return;
      this.tooltipAdditionalText.SetToolTip(ctl, helpKey1 + ": " + FieldHelp.GetText(helpKey1));
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
      if (this.loan == null)
        return;
      this.txtAdditionalInfo.Text = this.loan.GetField("SP.ADDITIONALINFO");
      int serviceProviders = this.loan.GetNumberOfSettlementServiceProviders();
      if (serviceProviders == 0)
        return;
      this.gridViewProviders.BeginUpdate();
      for (int index = 0; index < serviceProviders; ++index)
        this.gridViewProviders.Items.Add(this.buildListViewItem(index + 1, false));
      this.gridViewProviders.EndUpdate();
      this.gridViewProviders.Items[0].Selected = true;
    }

    private void initSnapshot()
    {
      this.gridViewProviders.Items.Clear();
      int serviceProviders = ((DisclosedSSPLHandler) this.iData).GetNumberOfSettlementServiceProviders();
      if (serviceProviders == 0)
        return;
      this.gridViewProviders.BeginUpdate();
      for (int index = 0; index < serviceProviders; ++index)
        this.gridViewProviders.Items.Add(this.buildSnapshotListView(index + 1, false));
      this.gridViewProviders.EndUpdate();
      this.gridViewProviders.Items[0].Selected = true;
    }

    private GVItem buildSnapshotListView(int newIndex, bool selected)
    {
      string str = newIndex.ToString("00");
      return new GVItem(string.Concat((object) newIndex))
      {
        SubItems = {
          (object) ((DisclosedSSPLHandler) this.iData).GetField("SP" + str + "01"),
          (object) ((DisclosedSSPLHandler) this.iData).GetField("SP" + str + "02"),
          (object) ((DisclosedSSPLHandler) this.iData).GetField("SP" + str + "03"),
          (object) ((DisclosedSSPLHandler) this.iData).GetField("SP" + str + "04"),
          (object) ((DisclosedSSPLHandler) this.iData).GetField("SP" + str + "05")
        },
        Selected = selected
      };
    }

    private void gridViewProviders_SelectedIndexChanged(object sender, EventArgs e)
    {
      StandardIconButton btnIconDelete = this.btnIconDelete;
      StandardIconButton btnIconUp = this.btnIconUp;
      bool flag1;
      this.btnIconDown.Enabled = flag1 = !this.readOnly && this.gridViewProviders.SelectedItems.Count == 1;
      int num1;
      bool flag2 = (num1 = flag1 ? 1 : 0) != 0;
      btnIconUp.Enabled = num1 != 0;
      int num2 = flag2 ? 1 : 0;
      btnIconDelete.Enabled = num2 != 0;
      if (this.gridViewProviders.SelectedItems.Count == 0)
        return;
      if (this.detailScreen != null)
        this.detailScreen.BrowserHandler.UpdateCurrentField();
      this.openSettlementServiceProvider(this.gridViewProviders.SelectedItems[0].Index);
    }

    private void btnIconAdd_Click(object sender, EventArgs e)
    {
      int newIndex = this.loan.NewSettlementServiceProvider();
      if (newIndex <= -1)
        return;
      this.gridViewProviders.SelectedItems.Clear();
      this.gridViewProviders.Items.Add(this.buildListViewItem(newIndex, true));
      this.editSettlementServiceProvider();
    }

    private void editSettlementServiceProvider()
    {
      if (this.gridViewProviders.SelectedItems.Count == 0)
        return;
      this.openSettlementServiceProvider(this.gridViewProviders.SelectedItems[0].Index);
      this.gridViewProviders.Focus();
    }

    private void openSettlementServiceProvider(int i)
    {
      if (i < 0)
        return;
      if (this.detailScreen == null)
      {
        this.detailScreen = !this.snapshot ? new LoanScreen(this.session, !(this.iData is LoanData) || this.loan.IsInFindFieldForm ? (IWin32Window) this.ParentForm : (IWin32Window) this, (IHtmlInput) this.loan) : new LoanScreen(this.session, !(this.iData is LoanData) || this.loan.IsInFindFieldForm ? (IWin32Window) this.ParentForm : (IWin32Window) this, this.iData);
        this.detailScreen.RemoveTitle();
        this.detailScreen.RemoveBorder();
        this.panelDetail.Controls.Add((Control) this.detailScreen);
        this.detailScreen.BrowserHandler.DocumentCompleted += new DocumentCompleteEventHandler(this.browserHandler_DocumentCompleted);
        this.detailScreen.BrowserHandler.SetHelpTarget((IOnlineHelpTarget) this);
        if (this.snapshot || this.iData is SettlementServiceTemplate || Utils.CheckIf2015RespaTila(this.iData.GetField("3969")))
          this.detailScreen.LoadForm(new InputFormInfo("SettlementServiceProvider", "Settlement Service Provider"));
        else
          this.detailScreen.LoadForm(new InputFormInfo("SettlementServiceProvider2010", "Settlement Service Provider"));
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
      this.detailScreen.BrowserHandler.SetGoToFieldFocus("SP0001", 0);
    }

    private void browserHandler_DocumentCompleted()
    {
      SETTLEMENTSERVICEPROVIDERInputHandler inputHandler = (SETTLEMENTSERVICEPROVIDERInputHandler) this.detailScreen.BrowserHandler.GetInputHandler();
      if (inputHandler == null)
        return;
      inputHandler.VerifSummaryChanged += new VerifSummaryChangedEventHandler(this.summaryInfoChangedHandler);
      if (!this.readOnly)
        return;
      inputHandler.FieldsReadOnly = true;
    }

    private GVItem buildListViewItem(int newIndex, bool selected)
    {
      string str = newIndex.ToString("00");
      return new GVItem(string.Concat((object) newIndex))
      {
        SubItems = {
          (object) this.loan.GetField("SP" + str + "01"),
          (object) this.loan.GetField("SP" + str + "02"),
          (object) this.loan.GetField("SP" + str + "03"),
          (object) this.loan.GetField("SP" + str + "04"),
          (object) this.loan.GetField("SP" + str + "05")
        },
        Selected = selected
      };
    }

    private void summaryInfoChangedHandler(VerifSummaryChangeInfo info)
    {
      int nItemIndex = Utils.ParseInt(this.detailScreen.BrowserHandler.Property) - 1;
      switch (info.ItemName)
      {
        case "SP0001":
          this.gridViewProviders.Items[nItemIndex].SubItems[1].Text = info.ItemValue.ToString();
          break;
        case "SP0002":
          this.gridViewProviders.Items[nItemIndex].SubItems[2].Text = info.ItemValue.ToString();
          break;
        case "SP0003":
          this.gridViewProviders.Items[nItemIndex].SubItems[3].Text = info.ItemValue.ToString();
          break;
        case "SP0004":
          this.gridViewProviders.Items[nItemIndex].SubItems[4].Text = info.ItemValue.ToString();
          break;
        case "SP0005":
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
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You have to select a provider first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (Utils.Dialog((IWin32Window) this, "Are you sure you want to delete this Settlement Service Provider?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
          return;
        int index = this.gridViewProviders.SelectedItems[0].Index;
        if (!this.loan.RemoveSettlementServiceProviderAt(index))
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "This settlement service provider can't be deleted.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You have to select a provider first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (!this.loan.IsTemplate && !this.loan.IsInFindFieldForm && this.session.LoanDataMgr.Writable && this.session.SessionObjects.AllowConcurrentEditing && !this.session.LoanDataMgr.LockLoanWithExclusiveA(true))
          return;
        int index = this.gridViewProviders.SelectedItems[0].Index;
        if (index >= this.gridViewProviders.Items.Count - 1)
          return;
        if (!this.loan.DownSettlementServiceProvider(index))
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "This settlement service provider can't be moved down.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You have to select a provider first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (!this.loan.IsTemplate && !this.loan.IsInFindFieldForm && this.session.LoanDataMgr.Writable && this.session.SessionObjects.AllowConcurrentEditing && !this.session.LoanDataMgr.LockLoanWithExclusiveA(true))
          return;
        int index = this.gridViewProviders.SelectedItems[0].Index;
        if (index == 0)
          return;
        if (!this.loan.UpSettlementServiceProvider(index))
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "This settlement service provider can't be moved up.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
      SettlementServiceTemplate iData = (SettlementServiceTemplate) this.iData;
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
        Tracing.Log(SettlementServiceForm.sw, TraceLevel.Error, nameof (SettlementServiceForm), "Error opening BlankLoan template. Message: " + ex.Message);
        return (LoanData) null;
      }
      foreach (string assignedFieldId in iData.GetAssignedFieldIDs())
        loanData.SetCurrentField(assignedFieldId, iData.GetField(assignedFieldId));
      loanData.IsTemplate = true;
      return loanData;
    }

    public bool SetTemplate(SettlementServiceTemplate template)
    {
      int serviceProviders = this.loan.GetNumberOfSettlementServiceProviders();
      if (serviceProviders == 0)
        return false;
      template.ClearAllFields();
      for (int index1 = 1; index1 <= serviceProviders; ++index1)
      {
        for (int index2 = 1; index2 <= 36; ++index2)
        {
          string id = "SP" + (index1 > 99 ? index1.ToString("000") : index1.ToString("00")) + index2.ToString("00");
          template.SetCurrentField(id, this.loan.GetSimpleField(id));
        }
      }
      return true;
    }

    public string GetHelpTargetName() => "Settlement Service Provider";

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, this.GetHelpTargetName());
    }

    private void SettlementServiceForm_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      this.ShowHelp();
    }

    private void btnApply_Click(object sender, EventArgs e)
    {
      if (!this.session.Application.GetService<ILoanEditor>().SelectSettlementServiceProviders())
        return;
      this.initForm();
    }

    public void DisplayTPOCompanySetting(ExternalOriginatorManagementData o)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void rearrangeUIElementsForTemplateOr2010()
    {
      this.panelTop.Visible = false;
      this.gradientPanel4.GradientColor2 = EncompassColors.Gradient1Color2;
      this.panelDetail.BorderStyle = BorderStyle.FixedSingle;
      this.panelDetail.BackColor = Color.Transparent;
      this.gradientPanel3.Visible = false;
      this.groupContainerList.Controls.Add((Control) this.btnApply);
      this.groupContainerList.Controls.Add((Control) this.verticalSeparator1);
      this.groupContainerList.Controls.Add((Control) this.btnIconAdd);
      this.groupContainerList.Controls.Add((Control) this.btnIconUp);
      this.groupContainerList.Controls.Add((Control) this.btnIconDelete);
      this.groupContainerList.Controls.Add((Control) this.btnIconDown);
    }

    private void freeEntryDate_ValueChanged(object sender, EventArgs e)
    {
      DatePicker datePicker = (DatePicker) sender;
      try
      {
        if (datePicker.Value != DateTime.MinValue)
          this.loan.SetField(string.Concat(datePicker.Tag), datePicker.Value.ToString("MM/dd/yyyy"));
        else
          this.loan.SetField(string.Concat(datePicker.Tag), "");
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.InnerException.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.loan.SetField(string.Concat(datePicker.Tag), "");
        datePicker.Text = "";
      }
    }

    private void freeEntryDate_Click(object sender, EventArgs e)
    {
      if (!(sender is DatePicker))
        return;
      DatePicker datePicker = (DatePicker) sender;
      IStatusDisplay service = Session.Application.GetService<IStatusDisplay>();
      if (datePicker.Tag.ToString().Length > 4)
        service.DisplayFieldID((string) datePicker.Tag);
      else
        service.DisplayFieldID(string.Concat(datePicker.Tag));
      this.field_MouseClick(sender, (MouseEventArgs) null);
    }

    private void lBtnDateIssued_Click(object sender, EventArgs e)
    {
      this.toggleDateIssuedLock();
      if (!this.dpDateIssued.ReadOnly)
      {
        this.loan.AddLock("SP.DATEISSUED");
        this.dpDateIssued.Value = new DateTime(0L);
      }
      else
      {
        this.loan.RemoveLock("SP.DATEISSUED");
        this.dpDateIssued.Value = Utils.ParseDate((object) this.loan.GetField("LE1.X1"));
      }
    }

    private void toggleDateIssuedLock()
    {
      this.lBtnDateIssued.Locked = !this.lBtnDateIssued.Locked;
      this.dpDateIssued.ReadOnly = !this.dpDateIssued.ReadOnly;
    }

    private void field_MouseClick(object sender, MouseEventArgs e)
    {
      if (sender == null || Control.ModifierKeys != Keys.Control)
        return;
      string helpKey = ((Control) sender).Tag.ToString();
      string text = helpKey + ": " + FieldHelp.GetText(helpKey);
      if (text == "")
        return;
      FieldHelpDialog.ShowHelp(text);
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
      this.panelMiddle = new Panel();
      this.panelList = new Panel();
      this.gridViewProviders = new GridView();
      this.gradientPanel3 = new GradientPanel();
      this.label3 = new Label();
      this.btnApply = new Button();
      this.verticalSeparator1 = new VerticalSeparator();
      this.btnIconAdd = new StandardIconButton();
      this.btnIconUp = new StandardIconButton();
      this.btnIconDelete = new StandardIconButton();
      this.btnIconDown = new StandardIconButton();
      this.panelTop = new Panel();
      this.panel1 = new Panel();
      this.txtAdditionalInfo = new TextBox();
      this.gradientPanel2 = new GradientPanel();
      this.label2 = new Label();
      this.panelRight = new Panel();
      this.dpDateIssued = new DatePicker();
      this.lBtnDateIssued = new FieldLockButton();
      this.gradientPanel1 = new GradientPanel();
      this.label1 = new Label();
      this.collapsibleSplitter1 = new CollapsibleSplitter();
      this.panelExDetail = new PanelEx();
      this.panelServiceDetail = new Panel();
      this.panelDetail = new Panel();
      this.gradientPanel4 = new GradientPanel();
      this.label4 = new Label();
      this.panelExList.SuspendLayout();
      this.groupContainerList.SuspendLayout();
      this.panelMiddle.SuspendLayout();
      this.panelList.SuspendLayout();
      this.gradientPanel3.SuspendLayout();
      ((ISupportInitialize) this.btnIconAdd).BeginInit();
      ((ISupportInitialize) this.btnIconUp).BeginInit();
      ((ISupportInitialize) this.btnIconDelete).BeginInit();
      ((ISupportInitialize) this.btnIconDown).BeginInit();
      this.panelTop.SuspendLayout();
      this.panel1.SuspendLayout();
      this.gradientPanel2.SuspendLayout();
      this.panelRight.SuspendLayout();
      this.gradientPanel1.SuspendLayout();
      this.panelExDetail.SuspendLayout();
      this.panelServiceDetail.SuspendLayout();
      this.gradientPanel4.SuspendLayout();
      this.SuspendLayout();
      this.panelExList.Controls.Add((Control) this.groupContainerList);
      this.panelExList.Dock = DockStyle.Fill;
      this.panelExList.Location = new Point(0, 0);
      this.panelExList.Name = "panelExList";
      this.panelExList.Size = new Size(700, 400);
      this.panelExList.TabIndex = 4;
      this.groupContainerList.Controls.Add((Control) this.panelMiddle);
      this.groupContainerList.Controls.Add((Control) this.panelTop);
      this.groupContainerList.Dock = DockStyle.Fill;
      this.groupContainerList.HeaderForeColor = SystemColors.ControlText;
      this.groupContainerList.Location = new Point(0, 0);
      this.groupContainerList.Name = "groupContainerList";
      this.groupContainerList.Size = new Size(700, 400);
      this.groupContainerList.TabIndex = 0;
      this.groupContainerList.Text = "Settlement Service Provider List";
      this.panelMiddle.Controls.Add((Control) this.panelList);
      this.panelMiddle.Controls.Add((Control) this.gradientPanel3);
      this.panelMiddle.Dock = DockStyle.Fill;
      this.panelMiddle.Location = new Point(1, 126);
      this.panelMiddle.Name = "panelMiddle";
      this.panelMiddle.Size = new Size(698, 273);
      this.panelMiddle.TabIndex = 8;
      this.panelList.Controls.Add((Control) this.gridViewProviders);
      this.panelList.Dock = DockStyle.Fill;
      this.panelList.Location = new Point(0, 24);
      this.panelList.Name = "panelList";
      this.panelList.Size = new Size(698, 249);
      this.panelList.TabIndex = 11;
      this.gridViewProviders.AllowMultiselect = false;
      this.gridViewProviders.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "";
      gvColumn1.Width = 30;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Service Category";
      gvColumn2.Width = 160;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Company Name";
      gvColumn3.Width = 160;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Text = "Address";
      gvColumn4.Width = 160;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.Text = "City";
      gvColumn5.Width = 120;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column6";
      gvColumn6.Text = "State";
      gvColumn6.Width = 60;
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
      this.gridViewProviders.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridViewProviders.Location = new Point(0, 0);
      this.gridViewProviders.Name = "gridViewProviders";
      this.gridViewProviders.Size = new Size(698, 249);
      this.gridViewProviders.SortOption = GVSortOption.None;
      this.gridViewProviders.TabIndex = 0;
      this.gridViewProviders.SelectedIndexChanged += new EventHandler(this.gridViewProviders_SelectedIndexChanged);
      this.gradientPanel3.Controls.Add((Control) this.label3);
      this.gradientPanel3.Controls.Add((Control) this.btnApply);
      this.gradientPanel3.Controls.Add((Control) this.verticalSeparator1);
      this.gradientPanel3.Controls.Add((Control) this.btnIconAdd);
      this.gradientPanel3.Controls.Add((Control) this.btnIconUp);
      this.gradientPanel3.Controls.Add((Control) this.btnIconDelete);
      this.gradientPanel3.Controls.Add((Control) this.btnIconDown);
      this.gradientPanel3.Dock = DockStyle.Top;
      this.gradientPanel3.GradientColor2 = Color.Silver;
      this.gradientPanel3.Location = new Point(0, 0);
      this.gradientPanel3.Name = "gradientPanel3";
      this.gradientPanel3.Size = new Size(698, 24);
      this.gradientPanel3.TabIndex = 10;
      this.label3.AutoSize = true;
      this.label3.BackColor = Color.Transparent;
      this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(12, 5);
      this.label3.Name = "label3";
      this.label3.Size = new Size(171, 13);
      this.label3.TabIndex = 1;
      this.label3.Text = "Settlement Service Providers";
      this.btnApply.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnApply.Location = new Point(602, 1);
      this.btnApply.Name = "btnApply";
      this.btnApply.Size = new Size(93, 21);
      this.btnApply.TabIndex = 5;
      this.btnApply.Text = "Apply Template";
      this.btnApply.UseVisualStyleBackColor = true;
      this.btnApply.Click += new EventHandler(this.btnApply_Click);
      this.verticalSeparator1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.verticalSeparator1.Location = new Point(596, 4);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 6;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.btnIconAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnIconAdd.BackColor = Color.Transparent;
      this.btnIconAdd.Location = new Point(507, 4);
      this.btnIconAdd.MouseDownImage = (Image) null;
      this.btnIconAdd.Name = "btnIconAdd";
      this.btnIconAdd.Size = new Size(16, 16);
      this.btnIconAdd.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnIconAdd.TabIndex = 1;
      this.btnIconAdd.TabStop = false;
      this.btnIconAdd.Click += new EventHandler(this.btnIconAdd_Click);
      this.btnIconUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnIconUp.BackColor = Color.Transparent;
      this.btnIconUp.Location = new Point(530, 4);
      this.btnIconUp.MouseDownImage = (Image) null;
      this.btnIconUp.Name = "btnIconUp";
      this.btnIconUp.Size = new Size(16, 16);
      this.btnIconUp.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.btnIconUp.TabIndex = 2;
      this.btnIconUp.TabStop = false;
      this.btnIconUp.Click += new EventHandler(this.btnIconUp_Click);
      this.btnIconDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnIconDelete.BackColor = Color.Transparent;
      this.btnIconDelete.Location = new Point(576, 4);
      this.btnIconDelete.MouseDownImage = (Image) null;
      this.btnIconDelete.Name = "btnIconDelete";
      this.btnIconDelete.Size = new Size(16, 16);
      this.btnIconDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnIconDelete.TabIndex = 4;
      this.btnIconDelete.TabStop = false;
      this.btnIconDelete.Click += new EventHandler(this.btnIconDelete_Click);
      this.btnIconDown.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnIconDown.BackColor = Color.Transparent;
      this.btnIconDown.Location = new Point(553, 4);
      this.btnIconDown.MouseDownImage = (Image) null;
      this.btnIconDown.Name = "btnIconDown";
      this.btnIconDown.Size = new Size(16, 16);
      this.btnIconDown.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.btnIconDown.TabIndex = 3;
      this.btnIconDown.TabStop = false;
      this.btnIconDown.Click += new EventHandler(this.btnIconDown_Click);
      this.panelTop.Controls.Add((Control) this.panel1);
      this.panelTop.Controls.Add((Control) this.gradientPanel2);
      this.panelTop.Controls.Add((Control) this.panelRight);
      this.panelTop.Dock = DockStyle.Top;
      this.panelTop.Location = new Point(1, 26);
      this.panelTop.Name = "panelTop";
      this.panelTop.Size = new Size(698, 100);
      this.panelTop.TabIndex = 7;
      this.panel1.Controls.Add((Control) this.txtAdditionalInfo);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(0, 24);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(567, 76);
      this.panel1.TabIndex = 10;
      this.txtAdditionalInfo.Dock = DockStyle.Fill;
      this.txtAdditionalInfo.Location = new Point(0, 0);
      this.txtAdditionalInfo.Multiline = true;
      this.txtAdditionalInfo.Name = "txtAdditionalInfo";
      this.txtAdditionalInfo.ScrollBars = ScrollBars.Both;
      this.txtAdditionalInfo.Size = new Size(567, 76);
      this.txtAdditionalInfo.TabIndex = 10;
      this.txtAdditionalInfo.Tag = (object) "SP.ADDITIONALINFO";
      this.txtAdditionalInfo.MouseClick += new MouseEventHandler(this.field_MouseClick);
      this.gradientPanel2.Controls.Add((Control) this.label2);
      this.gradientPanel2.Dock = DockStyle.Top;
      this.gradientPanel2.GradientColor2 = Color.Silver;
      this.gradientPanel2.Location = new Point(0, 0);
      this.gradientPanel2.Name = "gradientPanel2";
      this.gradientPanel2.Size = new Size(567, 24);
      this.gradientPanel2.TabIndex = 9;
      this.label2.AutoSize = true;
      this.label2.BackColor = Color.Transparent;
      this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(12, 5);
      this.label2.Name = "label2";
      this.label2.Size = new Size(130, 13);
      this.label2.TabIndex = 1;
      this.label2.Text = "Additional Information";
      this.panelRight.Controls.Add((Control) this.dpDateIssued);
      this.panelRight.Controls.Add((Control) this.lBtnDateIssued);
      this.panelRight.Controls.Add((Control) this.gradientPanel1);
      this.panelRight.Dock = DockStyle.Right;
      this.panelRight.Location = new Point(567, 0);
      this.panelRight.Name = "panelRight";
      this.panelRight.Size = new Size(131, 100);
      this.panelRight.TabIndex = 8;
      this.dpDateIssued.BackColor = SystemColors.Window;
      this.dpDateIssued.Location = new Point(29, 32);
      this.dpDateIssued.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpDateIssued.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpDateIssued.Name = "dpDateIssued";
      this.dpDateIssued.ReadOnly = true;
      this.dpDateIssued.Size = new Size(90, 21);
      this.dpDateIssued.TabIndex = 4;
      this.dpDateIssued.Tag = (object) "SP.DATEISSUED";
      this.dpDateIssued.ToolTip = "";
      this.dpDateIssued.Value = new DateTime(0L);
      this.dpDateIssued.ValueChanged += new EventHandler(this.freeEntryDate_ValueChanged);
      this.dpDateIssued.Click += new EventHandler(this.freeEntryDate_Click);
      this.dpDateIssued.MouseClick += new MouseEventHandler(this.field_MouseClick);
      this.lBtnDateIssued.Location = new Point(10, 33);
      this.lBtnDateIssued.LockedStateToolTip = "Use Default Value";
      this.lBtnDateIssued.MaximumSize = new Size(16, 16);
      this.lBtnDateIssued.MinimumSize = new Size(16, 16);
      this.lBtnDateIssued.Name = "lBtnDateIssued";
      this.lBtnDateIssued.Size = new Size(16, 16);
      this.lBtnDateIssued.TabIndex = 3;
      this.lBtnDateIssued.Tag = (object) "LOCKBUTTON_SPDATEISSUED";
      this.lBtnDateIssued.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnDateIssued.Click += new EventHandler(this.lBtnDateIssued_Click);
      this.gradientPanel1.Controls.Add((Control) this.label1);
      this.gradientPanel1.Dock = DockStyle.Top;
      this.gradientPanel1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.gradientPanel1.GradientColor2 = Color.Silver;
      this.gradientPanel1.Location = new Point(0, 0);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(131, 24);
      this.gradientPanel1.TabIndex = 0;
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Location = new Point(7, 5);
      this.label1.Name = "label1";
      this.label1.Size = new Size(75, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Date Issued";
      this.collapsibleSplitter1.AnimationDelay = 20;
      this.collapsibleSplitter1.AnimationStep = 20;
      this.collapsibleSplitter1.BorderStyle3D = Border3DStyle.Flat;
      this.collapsibleSplitter1.ControlToHide = (Control) this.panelExDetail;
      this.collapsibleSplitter1.Cursor = Cursors.HSplit;
      this.collapsibleSplitter1.Dock = DockStyle.Bottom;
      this.collapsibleSplitter1.ExpandParentForm = false;
      this.collapsibleSplitter1.Location = new Point(0, 400);
      this.collapsibleSplitter1.Name = "collapsibleSplitter1";
      this.collapsibleSplitter1.TabIndex = 3;
      this.collapsibleSplitter1.TabStop = false;
      this.collapsibleSplitter1.UseAnimations = false;
      this.collapsibleSplitter1.VisualStyle = VisualStyles.Encompass;
      this.panelExDetail.Controls.Add((Control) this.panelServiceDetail);
      this.panelExDetail.Controls.Add((Control) this.gradientPanel4);
      this.panelExDetail.Dock = DockStyle.Bottom;
      this.panelExDetail.Location = new Point(0, 407);
      this.panelExDetail.Name = "panelExDetail";
      this.panelExDetail.Size = new Size(700, 177);
      this.panelExDetail.TabIndex = 2;
      this.panelServiceDetail.Controls.Add((Control) this.panelDetail);
      this.panelServiceDetail.Dock = DockStyle.Fill;
      this.panelServiceDetail.Location = new Point(0, 24);
      this.panelServiceDetail.Name = "panelServiceDetail";
      this.panelServiceDetail.Size = new Size(700, 153);
      this.panelServiceDetail.TabIndex = 11;
      this.panelDetail.Dock = DockStyle.Fill;
      this.panelDetail.Location = new Point(0, 0);
      this.panelDetail.Name = "panelDetail";
      this.panelDetail.Size = new Size(700, 153);
      this.panelDetail.TabIndex = 0;
      this.gradientPanel4.Controls.Add((Control) this.label4);
      this.gradientPanel4.Dock = DockStyle.Top;
      this.gradientPanel4.GradientColor2 = Color.Silver;
      this.gradientPanel4.Location = new Point(0, 0);
      this.gradientPanel4.Name = "gradientPanel4";
      this.gradientPanel4.Size = new Size(700, 24);
      this.gradientPanel4.TabIndex = 10;
      this.label4.AutoSize = true;
      this.label4.BackColor = Color.Transparent;
      this.label4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label4.Location = new Point(8, 5);
      this.label4.Name = "label4";
      this.label4.Size = new Size(165, 13);
      this.label4.TabIndex = 1;
      this.label4.Text = "Settlement Service Provider";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.panelExList);
      this.Controls.Add((Control) this.collapsibleSplitter1);
      this.Controls.Add((Control) this.panelExDetail);
      this.Name = nameof (SettlementServiceForm);
      this.Size = new Size(700, 584);
      this.KeyUp += new KeyEventHandler(this.SettlementServiceForm_KeyUp);
      this.panelExList.ResumeLayout(false);
      this.groupContainerList.ResumeLayout(false);
      this.panelMiddle.ResumeLayout(false);
      this.panelList.ResumeLayout(false);
      this.gradientPanel3.ResumeLayout(false);
      this.gradientPanel3.PerformLayout();
      ((ISupportInitialize) this.btnIconAdd).EndInit();
      ((ISupportInitialize) this.btnIconUp).EndInit();
      ((ISupportInitialize) this.btnIconDelete).EndInit();
      ((ISupportInitialize) this.btnIconDown).EndInit();
      this.panelTop.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.gradientPanel2.ResumeLayout(false);
      this.gradientPanel2.PerformLayout();
      this.panelRight.ResumeLayout(false);
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      this.panelExDetail.ResumeLayout(false);
      this.panelServiceDetail.ResumeLayout(false);
      this.gradientPanel4.ResumeLayout(false);
      this.gradientPanel4.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
