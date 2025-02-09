// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.FeeManagementControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using Elli.Metrics.Client;
using EllieMae.EMLite.CalculationEngine;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.Encompass.AsmResolver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class FeeManagementControl : SettingsUserControl, IOnlineHelpTarget
  {
    private const string className = "FeeManagementControl";
    private static readonly string sw = Tracing.SwOutsideLoan;
    private bool originalComanyOptIn;
    private bool feeIsDirty;
    private bool personaIsDirty;
    private FeeManagementSetting originalFeeManagementSetting;
    private FeeManagementSetting complianceFeeList;
    private List<GVColumn> sectionGVColumns;
    private int original1stColumnWidth;
    private int original2ndColumnWidth;
    private Sessions.Session session;
    private string[] fullUcdFeeList;
    private static string[] OVERWRITES = new string[8]
    {
      "Section 700 - Real Estate Fees",
      "Section 800 - Items Payable in Connection with Loan",
      "Section 900 - Items Required by Lender to be Paid in Advance",
      "Section 1000 - Reserves Depositied with Lender",
      "Section 1100 - Title Charges",
      "Section 1200 - Government Recording and Transfer Charges",
      "Section 1300 - Additional Settlement Charges",
      "Section PC - Construction Fees Collected Post Consummation"
    };
    private GVItem previousHoverItem;
    private List<int> selectedItems;
    private IContainer components;
    private GroupContainer groupContainer1;
    private TabControlEx tabControlEx1;
    private TabPageEx tabPageFeeList;
    private TabPageEx tabPagePersona;
    private CheckBox chkCompanyOptIn;
    private GroupContainer gcPersonaOverwrite;
    private GroupContainer gcPersonas;
    private GridView gridPersona;
    private GridView gridOverwrite;
    private Panel panelPersonaRight;
    private CollapsibleSplitter collapsibleSplitter1;
    private Panel panelPersonaLeft;
    private Panel panelFeeList;
    private GroupContainer groupContainer3;
    private GroupContainer groupContainer2;
    private Panel panelIconGroup;
    private VerticalSeparator verticalSeparator1;
    private StandardIconButton iconBtnNew;
    private StandardIconButton iconBtnDelete;
    private Panel panelLeft;
    private Panel panelRight;
    private Panel panelFeeTab;
    private CollapsibleSplitter splitterFeeList;
    private CheckBox chkAll;
    private ImageList imageListMap;
    private ContextMenuButton btnImport;
    private ContextMenuStrip mnuImportFees;
    private ToolStripMenuItem mnuItemImportCC;
    private ToolStripMenuItem mnuItemImportDefault;
    private GradientPanel gradientPanel1;
    private GridView gridFees;
    private Label label1;
    private GridView gridViewCompliance;
    private GradientPanel gradientPanel2;
    private Label label2;
    private GroupContainer groupContainer4;
    private GradientPanel gradientPanel3;
    private Label label3;
    private GridView gridViewUCD;
    private CollapsibleSplitter splitterComponentAndUcdFeeLists;
    private CheckBox chkAllUCD;

    public FeeManagementControl(SetUpContainer setupContainer)
      : this(setupContainer, Session.DefaultInstance, false, false)
    {
    }

    public FeeManagementControl(
      SetUpContainer setupContainer,
      Sessions.Session session,
      bool isMultiSelect,
      bool isSettingsSync)
      : base(setupContainer)
    {
      this.InitializeComponent();
      this.session = session;
      this.Dock = DockStyle.Fill;
      this.initForm();
      this.gridPersona.AllowMultiselect = isMultiSelect;
      this.sectionGVColumns = new List<GVColumn>();
      for (int nColumnIndex = 2; nColumnIndex < this.gridFees.Columns.Count; ++nColumnIndex)
        this.sectionGVColumns.Add(this.gridFees.Columns[nColumnIndex]);
      this.original1stColumnWidth = this.gridFees.Columns[0].Width;
      this.original2ndColumnWidth = this.gridFees.Columns[1].Width;
      this.Reset();
      if (string.Compare(this.session.UserID, "admin", true) != 0)
        this.tabControlEx1.TabPages.Remove(this.tabPagePersona);
      if (!isSettingsSync)
        return;
      this.panelIconGroup.Visible = false;
      this.chkCompanyOptIn.Enabled = false;
      this.btnImport.Enabled = false;
    }

    private void initForm()
    {
      this.gridOverwrite.BeginUpdate();
      for (int index = 0; index < FeeManagementControl.OVERWRITES.Length; ++index)
        this.gridOverwrite.Items.Add(new GVItem(FeeManagementControl.OVERWRITES[index]));
      this.gridOverwrite.EndUpdate();
    }

    public override void Reset()
    {
      this.gridFees.BeginUpdate();
      this.gridFees.Items.Clear();
      this.originalFeeManagementSetting = this.session.ConfigurationManager.GetFeeManagement();
      if (string.Compare(this.session.UserID, "admin", true) != 0)
        this.chkCompanyOptIn.Enabled = false;
      this.chkCompanyOptIn.Checked = this.originalComanyOptIn = this.originalFeeManagementSetting != null && this.originalFeeManagementSetting.CompanyOptIn;
      if (this.originalFeeManagementSetting != null)
      {
        FeeManagementRecord[] allFees = this.originalFeeManagementSetting.GetAllFees();
        this.validateFeeNameInUCD(allFees);
        for (int index = 0; index < allFees.Length; ++index)
          this.gridFees.Items.Add(this.buildFeeGVItem(allFees[index], false));
        this.originalFeeManagementSetting = this.originalFeeManagementSetting.Clone();
      }
      this.gridFees.Sort(0, SortOrder.Ascending);
      this.gridFees.EndUpdate();
      FeeManagementPersonaInfo managementPermission = ((FieldAccessAclManager) this.session.ACL.GetAclManager(AclCategory.FieldAccess)).GetFeeManagementPermission((int[]) null);
      this.gridPersona.Items.Clear();
      this.gridPersona.BeginUpdate();
      foreach (Persona allPersona in this.session.PersonaManager.GetAllPersonas())
      {
        if (allPersona.ID != 0)
        {
          FeeManagementPersonaRights managementPersonaRights = managementPermission.GetPersonaRights(allPersona.ID) ?? new FeeManagementPersonaRights(allPersona.ID, false, false, false, false, false, false, false, false);
          this.gridPersona.Items.Add(new GVItem(allPersona.Name)
          {
            Tag = (object) managementPersonaRights
          });
        }
      }
      this.gridPersona_SelectedIndexChanged((object) null, (EventArgs) null);
      this.gridPersona.EndUpdate();
      this.refreshMaventFeeList();
      this.refreshUcdFeeList();
      this.refreshUncheckedUcdFeeList();
      this.setDirtyFlag(false);
      this.feeIsDirty = false;
      this.personaIsDirty = false;
    }

    private void refreshTabPageAndMappingSections()
    {
      if (this.btnImport.Visible)
      {
        if (this.chkCompanyOptIn.Checked && !this.tabControlEx1.TabPages.Contains(this.tabPagePersona))
          this.tabControlEx1.TabPages.Add(this.tabPagePersona);
        else if (!this.chkCompanyOptIn.Checked && this.tabControlEx1.TabPages.Contains(this.tabPagePersona))
          this.tabControlEx1.TabPages.Remove(this.tabPagePersona);
        this.tabControlEx1.Refresh();
      }
      if (this.chkCompanyOptIn.Checked)
      {
        if (this.gridFees.Columns.Count > 2)
          return;
        for (int index = 0; index < this.sectionGVColumns.Count; ++index)
          this.gridFees.Columns.Add(this.sectionGVColumns[index]);
        this.gridFees.SubItemCheck -= new GVSubItemEventHandler(this.gridFees_SubItemCheck);
        for (int nItemIndex1 = 0; nItemIndex1 < this.gridFees.Items.Count; ++nItemIndex1)
        {
          FeeManagementRecord tag = (FeeManagementRecord) this.gridFees.Items[nItemIndex1].Tag;
          GVItem gvItem = this.gridFees.Items[nItemIndex1];
          for (int nItemIndex2 = 3; nItemIndex2 <= 9; ++nItemIndex2)
          {
            if (nItemIndex2 >= gvItem.SubItems.Count)
              gvItem.SubItems.Add((object) "");
            switch (nItemIndex2)
            {
              case 3:
                gvItem.SubItems[nItemIndex2].Checked = tag.Included(FeeSectionEnum.For700);
                break;
              case 4:
                gvItem.SubItems[nItemIndex2].Checked = tag.Included(FeeSectionEnum.For800);
                break;
              case 5:
                gvItem.SubItems[nItemIndex2].Checked = tag.Included(FeeSectionEnum.For900);
                break;
              case 6:
                gvItem.SubItems[nItemIndex2].Checked = tag.Included(FeeSectionEnum.For1000);
                break;
              case 7:
                gvItem.SubItems[nItemIndex2].Checked = tag.Included(FeeSectionEnum.For1100);
                break;
              case 8:
                gvItem.SubItems[nItemIndex2].Checked = tag.Included(FeeSectionEnum.For1200);
                break;
              case 9:
                gvItem.SubItems[nItemIndex2].Checked = tag.Included(FeeSectionEnum.For1300);
                break;
              case 10:
                gvItem.SubItems[nItemIndex2].Checked = tag.Included(FeeSectionEnum.ForPC);
                break;
            }
            gvItem.SubItems[nItemIndex2].Value = (object) null;
          }
        }
        this.gridFees.SubItemCheck += new GVSubItemEventHandler(this.gridFees_SubItemCheck);
        this.gridFees.Columns[1].SpringToFit = false;
        this.gridFees.Columns[0].Width = this.original1stColumnWidth > 0 ? this.original1stColumnWidth : 200;
        this.gridFees.Columns[1].Width = this.original2ndColumnWidth > 0 ? this.original2ndColumnWidth : 200;
      }
      else
      {
        if (this.gridFees.Columns.Count <= 2)
          return;
        for (int nColumnIndex = this.gridFees.Columns.Count - 1; nColumnIndex > 1; --nColumnIndex)
          this.gridFees.Columns.RemoveAt(nColumnIndex);
        this.original1stColumnWidth = this.gridFees.Columns[0].Width;
        this.original2ndColumnWidth = this.gridFees.Columns[1].Width;
        this.gridFees.Columns[0].Width = this.gridFees.Width / 2;
        this.gridFees.Columns[1].SpringToFit = true;
        this.refreshMaventFeeList();
      }
    }

    private GVItem buildFeeGVItem(FeeManagementRecord fee, bool selected)
    {
      GVItem gvItem = new GVItem(fee.FeeName);
      gvItem.SubItems.Add((fee.FeeNameInMavent ?? "") == string.Empty ? (object) "Not Mapped" : (object) fee.FeeNameInMavent);
      if ((fee.FeeNameInMavent ?? "") == string.Empty || fee.FeeNameInMavent == "Not Mapped")
      {
        gvItem.SubItems[1].ForeColor = Color.Red;
        gvItem.SubItems[1].ImageIndex = 0;
      }
      else
      {
        gvItem.SubItems[1].ForeColor = Color.Black;
        gvItem.SubItems[1].ImageIndex = 1;
      }
      gvItem.SubItems.Add((fee.FeeNameInUCD ?? "") == string.Empty ? (object) "Not Mapped" : (object) fee.FeeNameInUCD);
      if ((fee.FeeNameInUCD ?? "") == string.Empty || fee.FeeNameInUCD == "Not Mapped")
      {
        gvItem.SubItems[2].ForeColor = Color.Red;
        gvItem.SubItems[2].ImageIndex = 0;
      }
      else
      {
        gvItem.SubItems[2].ForeColor = Color.Black;
        gvItem.SubItems[2].ImageIndex = 1;
      }
      if (this.gridFees.Columns.Count > 2)
      {
        for (int nItemIndex = 3; nItemIndex <= 10; ++nItemIndex)
        {
          gvItem.SubItems.Add((object) "");
          switch (nItemIndex)
          {
            case 3:
              gvItem.SubItems[nItemIndex].Checked = fee.Included(FeeSectionEnum.For700);
              break;
            case 4:
              gvItem.SubItems[nItemIndex].Checked = fee.Included(FeeSectionEnum.For800);
              break;
            case 5:
              gvItem.SubItems[nItemIndex].Checked = fee.Included(FeeSectionEnum.For900);
              break;
            case 6:
              gvItem.SubItems[nItemIndex].Checked = fee.Included(FeeSectionEnum.For1000);
              break;
            case 7:
              gvItem.SubItems[nItemIndex].Checked = fee.Included(FeeSectionEnum.For1100);
              break;
            case 8:
              gvItem.SubItems[nItemIndex].Checked = fee.Included(FeeSectionEnum.For1200);
              break;
            case 9:
              gvItem.SubItems[nItemIndex].Checked = fee.Included(FeeSectionEnum.For1300);
              break;
            case 10:
              gvItem.SubItems[nItemIndex].Checked = fee.Included(FeeSectionEnum.ForPC);
              break;
          }
          gvItem.SubItems[nItemIndex].Value = (object) null;
        }
      }
      gvItem.Tag = (object) fee;
      gvItem.Selected = selected;
      return gvItem;
    }

    private void gridPersona_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.gridPersona.SelectedItems.Count == 0)
      {
        this.gridOverwrite.BeginUpdate();
        for (int nItemIndex = 0; nItemIndex < this.gridOverwrite.Items.Count; ++nItemIndex)
        {
          this.gridOverwrite.Items[nItemIndex].Checked = false;
          this.gridOverwrite.Items[nItemIndex].ForeColor = Color.Gray;
        }
        this.gridOverwrite.Enabled = false;
        this.gridOverwrite.EndUpdate();
      }
      else
      {
        FeeManagementPersonaRights tag = (FeeManagementPersonaRights) this.gridPersona.SelectedItems[0].Tag;
        this.gridOverwrite.BeginUpdate();
        for (int nItemIndex = 0; nItemIndex < this.gridOverwrite.Items.Count; ++nItemIndex)
          this.gridOverwrite.Items[nItemIndex].ForeColor = Color.Black;
        this.gridOverwrite.Enabled = true;
        this.gridOverwrite.Items[0].Checked = tag.Overwrite700;
        this.gridOverwrite.Items[1].Checked = tag.Overwrite800;
        this.gridOverwrite.Items[2].Checked = tag.Overwrite900;
        this.gridOverwrite.Items[3].Checked = tag.Overwrite1000;
        this.gridOverwrite.Items[4].Checked = tag.Overwrite1100;
        this.gridOverwrite.Items[5].Checked = tag.Overwrite1200;
        this.gridOverwrite.Items[6].Checked = tag.Overwrite1300;
        this.gridOverwrite.Items[7].Checked = tag.OverwritePC;
        this.gridOverwrite.EndUpdate();
      }
    }

    private void chkCompanyOptIn_Click(object sender, EventArgs e) => this.setDirtyFlag(true);

    private void gridOverwrite_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      if (this.gridPersona.SelectedItems.Count == 0)
        return;
      FeeManagementPersonaRights tag = (FeeManagementPersonaRights) this.gridPersona.SelectedItems[0].Tag;
      if (e.SubItem.Text.StartsWith("Section 700"))
        tag.SetRights(FeeSectionEnum.For700, e.SubItem.Checked);
      else if (e.SubItem.Text.StartsWith("Section 800"))
        tag.SetRights(FeeSectionEnum.For800, e.SubItem.Checked);
      else if (e.SubItem.Text.StartsWith("Section 900"))
        tag.SetRights(FeeSectionEnum.For900, e.SubItem.Checked);
      else if (e.SubItem.Text.StartsWith("Section 1000"))
        tag.SetRights(FeeSectionEnum.For1000, e.SubItem.Checked);
      else if (e.SubItem.Text.StartsWith("Section 1100"))
        tag.SetRights(FeeSectionEnum.For1100, e.SubItem.Checked);
      else if (e.SubItem.Text.StartsWith("Section 1200"))
        tag.SetRights(FeeSectionEnum.For1200, e.SubItem.Checked);
      else if (e.SubItem.Text.StartsWith("Section 1300"))
        tag.SetRights(FeeSectionEnum.For1300, e.SubItem.Checked);
      else if (e.SubItem.Text.StartsWith("Section PC"))
        tag.SetRights(FeeSectionEnum.ForPC, e.SubItem.Checked);
      this.personaIsDirty = true;
      this.setDirtyFlag(true);
    }

    private void iconBtnNew_Click(object sender, EventArgs e)
    {
      using (AddFeeForm addFeeForm = new AddFeeForm())
      {
        addFeeForm.OkButtonClick += new EventHandler(this.feeForm_OkButtonClick);
        if (addFeeForm.ShowDialog((IWin32Window) this.session.MainForm) == DialogResult.OK)
        {
          this.gridFees.BeginUpdate();
          this.gridFees.SelectedItems.Clear();
          GVItem gvItem = this.buildFeeGVItem(new FeeManagementRecord(addFeeForm.NewFeeName, false, false, false, false, false, false, false, false, this.session.UserID, "Not Mapped", (string) null, "Not Mapped"), true);
          this.gridFees.Items.Add(gvItem);
          this.gridFees.EnsureVisible(gvItem.Index);
          this.gridFees.EndUpdate();
          this.setDirtyFlag(true);
          this.feeIsDirty = true;
          if (this.gridFees.Columns.Count > 2)
            this.refreshMaventFeeList();
        }
        addFeeForm.OkButtonClick -= new EventHandler(this.feeForm_OkButtonClick);
      }
    }

    private void feeForm_OkButtonClick(object sender, EventArgs e)
    {
      AddFeeForm addFeeForm = (AddFeeForm) sender;
      for (int nItemIndex = 0; nItemIndex < this.gridFees.Items.Count; ++nItemIndex)
      {
        if (string.Compare(this.gridFees.Items[nItemIndex].Text, addFeeForm.NewFeeName, true) == 0)
          throw new InvalidOperationException("The Fee List already contains the fee '" + addFeeForm.NewFeeName + "'.");
      }
    }

    private void gridFees_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.iconBtnDelete.Enabled = this.gridFees.SelectedItems.Count > 0;
      if (this.gridFees.Columns.Count <= 2)
        return;
      this.refreshMaventFeeList();
    }

    private void iconBtnDelete_Click(object sender, EventArgs e)
    {
      if (this.gridFees.SelectedItems.Count == 0 || Utils.Dialog((IWin32Window) this, "Are you sure you want to delete the selected Fees?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      this.setDirtyFlag(true);
      this.feeIsDirty = true;
      int index1 = this.gridFees.SelectedItems[0].Index;
      List<GVItem> gvItemList = new List<GVItem>();
      for (int index2 = 0; index2 < this.gridFees.SelectedItems.Count; ++index2)
        gvItemList.Add(this.gridFees.SelectedItems[index2]);
      Dictionary<string, int> dictionary = new Dictionary<string, int>();
      if (!this.chkAllUCD.Checked)
      {
        for (int nItemIndex = 0; nItemIndex < this.gridFees.Items.Count; ++nItemIndex)
        {
          string text = this.gridFees.Items[nItemIndex].SubItems[2].Text;
          if (text != "Not Mapped")
          {
            if (dictionary.ContainsKey(text))
              dictionary[text]++;
            else
              dictionary.Add(text, 0);
          }
        }
      }
      for (int index3 = 0; index3 < gvItemList.Count; ++index3)
      {
        this.gridFees.Items.Remove(gvItemList[index3]);
        if (!this.chkAllUCD.Checked)
        {
          string text = gvItemList[index3].SubItems[2].Text;
          if (text != "Not Mapped")
          {
            if (dictionary[text] > 0)
            {
              dictionary[text]--;
            }
            else
            {
              this.gridViewUCD.SortOption = GVSortOption.None;
              this.gridViewUCD.BeginUpdate();
              this.gridViewUCD.Items.Add(new GVItem(gvItemList[index3].SubItems[2].Text));
              this.gridViewUCD.SortOption = GVSortOption.Auto;
              this.gridViewUCD.ReSort();
              this.gridViewUCD.EndUpdate();
            }
          }
        }
      }
      if (this.gridFees.Items.Count == 0)
        return;
      if (index1 + 1 >= this.gridFees.Items.Count)
        this.gridFees.Items[this.gridFees.Items.Count - 1].Selected = true;
      else
        this.gridFees.Items[index1].Selected = true;
    }

    public override void Save()
    {
      if ((this.feeIsDirty || this.originalComanyOptIn != this.chkCompanyOptIn.Checked) && this.chkCompanyOptIn.Checked && this.gridFees.Items.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You have to enter some fees if you check \"Apply to Itemization\"!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (this.originalComanyOptIn && !this.chkCompanyOptIn.Checked && Utils.Dialog((IWin32Window) this, "You are about to disable Itemization Fee Management feature. Are you sure you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
          return;
        if (this.feeIsDirty || this.originalComanyOptIn != this.chkCompanyOptIn.Checked)
        {
          FeeManagementSetting feeManagementSetting = new FeeManagementSetting();
          for (int nItemIndex = 0; nItemIndex < this.gridFees.Items.Count; ++nItemIndex)
          {
            FeeManagementRecord tag = (FeeManagementRecord) this.gridFees.Items[nItemIndex].Tag;
            FeeManagementRecord managementRecord = this.originalFeeManagementSetting != null ? this.originalFeeManagementSetting.GetFeeManagementRecord(tag.FeeName) : (FeeManagementRecord) null;
            if (managementRecord == null || (tag.FeeSource ?? "") == string.Empty || tag.For700 != managementRecord.For700 || tag.For800 != managementRecord.For800 || tag.For900 != managementRecord.For900 || tag.For1000 != managementRecord.For1000 || tag.For1100 != managementRecord.For1100 || tag.For1200 != managementRecord.For1200 || tag.For1300 != managementRecord.For1300 || tag.ForPC != managementRecord.ForPC)
              tag.SetFeeSource(this.session.UserID);
            feeManagementSetting.AddFee(tag);
          }
          feeManagementSetting.CompanyOptIn = this.chkCompanyOptIn.Checked;
          this.session.ConfigurationManager.UpdateFeeManagement(feeManagementSetting);
        }
        if (this.personaIsDirty)
        {
          FeeManagementPersonaInfo feeManagementPersonaInfo = new FeeManagementPersonaInfo();
          for (int nItemIndex = 0; nItemIndex < this.gridPersona.Items.Count; ++nItemIndex)
            feeManagementPersonaInfo.AddPersonaRights((FeeManagementPersonaRights) this.gridPersona.Items[nItemIndex].Tag);
          ((FieldAccessAclManager) this.session.ACL.GetAclManager(AclCategory.FieldAccess)).UpdateFeeManagementPermission(feeManagementPersonaInfo);
        }
        this.setDirtyFlag(false);
      }
    }

    private void gridFees_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      if (e.SubItem.Index < 1)
        return;
      ((FeeManagementRecord) e.SubItem.Item.Tag).SetSection(e.SubItem.Index - 2, e.SubItem.Checked);
      this.setDirtyFlag(true);
      this.feeIsDirty = true;
      this.refreshMaventFeeList();
    }

    string IOnlineHelpTarget.GetHelpTargetName() => "Setup\\Itemization Fee Management";

    private byte[] downloadFile(string fileID)
    {
      byte[] numArray = (byte[]) null;
      try
      {
        string str = this.session?.StartupInfo?.ServiceUrls?.DownloadServiceUrl;
        if (string.IsNullOrWhiteSpace(str) || !Uri.IsWellFormedUriString(str, UriKind.Absolute))
          str = "https://encompass.elliemae.com/download/download.asp";
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("ClientID=" + HttpUtility.UrlEncode(this.session.CompanyInfo.ClientID));
        stringBuilder.Append("&UserID=" + HttpUtility.UrlEncode(this.session.UserInfo.Userid));
        stringBuilder.Append("&FileID=" + HttpUtility.UrlEncode(fileID));
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(str);
        httpWebRequest.ContentType = "application/x-www-form-urlencoded";
        httpWebRequest.ContentLength = (long) stringBuilder.Length;
        httpWebRequest.KeepAlive = false;
        httpWebRequest.Method = "POST";
        using (Stream requestStream = httpWebRequest.GetRequestStream())
        {
          using (StreamWriter streamWriter = new StreamWriter(requestStream))
            streamWriter.Write(stringBuilder.ToString());
        }
        using (WebResponse response = httpWebRequest.GetResponse())
        {
          using (Stream responseStream = response.GetResponseStream())
          {
            using (MemoryStream memoryStream = new MemoryStream())
            {
              byte[] buffer = new byte[4096];
              for (int count = responseStream.Read(buffer, 0, buffer.Length); count > 0; count = responseStream.Read(buffer, 0, buffer.Length))
                memoryStream.Write(buffer, 0, count);
              numArray = memoryStream.ToArray();
            }
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(FeeManagementControl.sw, TraceLevel.Error, nameof (FeeManagementControl), ex.Message);
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "The following error occurred when trying to download the file:\n\n" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      return numArray;
    }

    private void gridViewCompliance_ItemDrag(object source, GVItemEventArgs e)
    {
      this.selectedItems = new List<int>();
      for (int index = 0; index < this.gridFees.SelectedItems.Count; ++index)
        this.selectedItems.Add(this.gridFees.SelectedItems[index].Index);
      int num = (int) this.DoDragDrop((object) e.Item, DragDropEffects.Copy);
    }

    private void gridViewUcd_ItemDrag(object source, GVItemEventArgs e)
    {
      this.selectedItems = new List<int>();
      for (int index = 0; index < this.gridFees.SelectedItems.Count; ++index)
        this.selectedItems.Add(this.gridFees.SelectedItems[index].Index);
      int num = (int) this.DoDragDrop((object) e.Item, DragDropEffects.Copy);
    }

    private void gridFees_DragDrop(object sender, DragEventArgs e)
    {
      GVItem data = (GVItem) e.Data.GetData(typeof (GVItem));
      bool flag1 = data.GridView.Name == "gridViewCompliance";
      bool flag2 = data.GridView.Name == "gridViewUCD";
      if (data == null)
        return;
      Point client = this.gridFees.PointToClient(new Point(e.X, e.Y));
      GVItem itemAt = this.gridFees.GetItemAt(client.X, client.Y);
      if (itemAt == null || itemAt.SubItems[1].Text == data.Text)
        return;
      FeeManagementRecord tag = (FeeManagementRecord) itemAt.Tag;
      if (tag == null)
        return;
      if (flag1)
      {
        tag.SetMaventFee(data.Text, "");
        itemAt.SubItems[1].Text = data.Text;
        itemAt.SubItems[1].ImageIndex = 1;
        itemAt.SubItems[1].ForeColor = Color.Black;
      }
      else
      {
        if (!this.chkAllUCD.Checked && itemAt.SubItems[2].Text != "Not Mapped")
          this.gridViewUCD.Items.Add(new GVItem(itemAt.SubItems[2].Text));
        tag.FeeNameInUCD = data.Text;
        itemAt.SubItems[2].Text = data.Text;
        itemAt.SubItems[2].ImageIndex = 1;
        itemAt.SubItems[2].ForeColor = Color.Black;
        if (!this.chkAllUCD.Checked)
        {
          this.gridViewUCD.Items.Remove(data);
          this.gridViewUCD.ReSort();
        }
      }
      this.setDirtyFlag(true);
      this.feeIsDirty = true;
      this.previousHoverItem = (GVItem) null;
    }

    private void gridFees_DragEnter(object sender, DragEventArgs e)
    {
      if (e.Data.GetDataPresent(typeof (GVSelectedItemCollection)) || e.Data.GetDataPresent(typeof (GVItem)))
        e.Effect = DragDropEffects.Copy;
      else
        e.Effect = DragDropEffects.None;
    }

    private void gridFees_DragOver(object sender, DragEventArgs e)
    {
      Point client = this.gridFees.PointToClient(new Point(e.X, e.Y));
      GVItem itemAt = this.gridFees.GetItemAt(client.X, client.Y);
      if (itemAt == null)
      {
        e.Effect = DragDropEffects.None;
      }
      else
      {
        e.Effect = DragDropEffects.Copy;
        this.gridFees.SelectedIndexChanged -= new EventHandler(this.gridFees_SelectedIndexChanged);
        this.gridFees.BeginUpdate();
        if (this.previousHoverItem != null)
          this.previousHoverItem.Selected = this.selectedItems.Contains(this.previousHoverItem.Index);
        itemAt.Selected = true;
        this.previousHoverItem = itemAt;
        this.gridFees.EndUpdate();
        this.gridFees.SelectedIndexChanged += new EventHandler(this.gridFees_SelectedIndexChanged);
        int topItemIndex = this.gridFees.TopItemIndex;
        int num1 = this.PointToScreen(new Point(this.gridFees.Location.X, this.gridFees.Location.Y)).Y + 86;
        int x = this.gridFees.Location.X + this.gridFees.Width;
        Point point = this.gridFees.Location;
        int y = point.Y + this.gridFees.Height;
        point = this.PointToScreen(new Point(x, y));
        int num2 = point.Y + 30;
        if (e.Y <= num1)
        {
          if (itemAt.Index < 1)
            return;
          this.gridFees.MoveToTop(topItemIndex - 1);
          Thread.Sleep(200 - (num1 - e.Y) * 16);
        }
        else
        {
          if (e.Y < num2 || itemAt.Index >= this.gridFees.Items.Count)
            return;
          this.gridFees.MoveToTop(topItemIndex + 1);
          Thread.Sleep(200 - (e.Y - num2) * 16);
        }
      }
    }

    private void refreshMaventFeeList()
    {
      Cursor.Current = Cursors.WaitCursor;
      if (this.complianceFeeList == null || this.complianceFeeList.NumberOfFees == 0)
        this.complianceFeeList = this.readMaventFeeList();
      if (this.gridFees.Columns.Count <= 2)
      {
        this.chkAll.CheckedChanged -= new EventHandler(this.chkAll_CheckedChanged);
        this.chkAll.Checked = true;
        this.chkAll.CheckedChanged += new EventHandler(this.chkAll_CheckedChanged);
      }
      bool[] flagArray = new bool[8];
      if (!this.chkAll.Checked)
      {
        if (this.gridFees.SelectedItems.Count > 0)
        {
          for (int index = 0; index < this.gridFees.SelectedItems.Count; ++index)
          {
            for (int nItemIndex = 3; nItemIndex <= 10; ++nItemIndex)
            {
              if (this.gridFees.SelectedItems[index].SubItems[nItemIndex].Checked)
                flagArray[nItemIndex - 3] = true;
            }
          }
        }
      }
      else
        flagArray = new bool[8]
        {
          true,
          true,
          true,
          true,
          true,
          true,
          true,
          true
        };
      this.gridViewCompliance.SortOption = GVSortOption.None;
      this.gridViewCompliance.BeginUpdate();
      this.gridViewCompliance.Items.Clear();
      FeeManagementRecord[] allFees = this.complianceFeeList.GetAllFees();
      for (int index1 = 0; index1 < allFees.Length; ++index1)
      {
        bool flag = false;
        for (int index2 = 0; index2 < 8; ++index2)
        {
          switch (index2)
          {
            case 0:
              if (flagArray[index2] && allFees[index1].For700)
              {
                flag = true;
                break;
              }
              break;
            case 1:
              if (flagArray[index2] && allFees[index1].For800)
              {
                flag = true;
                break;
              }
              break;
            case 2:
              if (flagArray[index2] && allFees[index1].For900)
              {
                flag = true;
                break;
              }
              break;
            case 3:
              if (flagArray[index2] && allFees[index1].For1000)
              {
                flag = true;
                break;
              }
              break;
            case 4:
              if (flagArray[index2] && allFees[index1].For1100)
              {
                flag = true;
                break;
              }
              break;
            case 5:
              if (flagArray[index2] && allFees[index1].For1200)
              {
                flag = true;
                break;
              }
              break;
            case 6:
              if (flagArray[index2] && allFees[index1].For1300)
              {
                flag = true;
                break;
              }
              break;
            case 7:
              if (flagArray[index2] && allFees[index1].ForPC)
              {
                flag = true;
                break;
              }
              break;
          }
          if (flag)
          {
            this.gridViewCompliance.Items.Add(new GVItem(allFees[index1].FeeName));
            break;
          }
        }
      }
      this.gridViewCompliance.SortOption = GVSortOption.Auto;
      this.gridViewCompliance.Sort(0, SortOrder.Ascending);
      this.gridViewCompliance.Items.Add(new GVItem("Other-Finance Charge"));
      this.gridViewCompliance.Items.Add(new GVItem("Other-Non-Finance Charge"));
      this.gridViewCompliance.EndUpdate();
      Cursor.Current = Cursors.Default;
    }

    private FeeManagementSetting readMaventFeeList()
    {
      FeeManagementSetting managementSetting = new FeeManagementSetting();
      byte[] bytes = this.downloadFile("ComplianceFeeList.xml");
      if (bytes == null || bytes.Length == 0)
        return managementSetting;
      XmlDocument xmlDocument = new XmlDocument();
      try
      {
        xmlDocument.LoadXml(Encoding.ASCII.GetString(bytes));
      }
      catch (Exception ex)
      {
        MetricsFactory.IncrementErrorCounter(ex, "The following error occurred when", "D:\\ws\\24.3.0.0\\EmLite\\SetupUI\\Setup\\FeeManagement\\FeeManagementControl.cs", nameof (readMaventFeeList), 898);
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "The following error occurred when trying to load Mavent Fee List: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return managementSetting;
      }
      XmlNodeList xmlNodeList1 = xmlDocument.SelectNodes("COMPLIANCE_FEES/FEE");
      if (xmlNodeList1 == null || xmlNodeList1.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "The file format of Mavent Fee List is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return managementSetting;
      }
      for (int i1 = 0; i1 < xmlNodeList1.Count; ++i1)
      {
        FeeManagementRecord fee = new FeeManagementRecord(((XmlElement) xmlNodeList1[i1]).GetAttribute("FeeDescription"), false, false, false, false, false, false, false, false, "", "", "", "");
        XmlNodeList xmlNodeList2 = xmlNodeList1[i1].SelectNodes("SECTION");
        if (xmlNodeList2 != null && xmlNodeList2.Count > 0)
        {
          for (int i2 = 0; i2 < xmlNodeList2.Count; ++i2)
          {
            switch (((XmlElement) xmlNodeList2[i2]).GetAttribute("Number"))
            {
              case "1000":
                fee.SetSection(FeeSectionEnum.For1000, true);
                break;
              case "1100":
                fee.SetSection(FeeSectionEnum.For1100, true);
                break;
              case "1200":
                fee.SetSection(FeeSectionEnum.For1200, true);
                break;
              case "1300":
                fee.SetSection(FeeSectionEnum.For1300, true);
                break;
              case "700":
                fee.SetSection(FeeSectionEnum.For700, true);
                break;
              case "800":
                fee.SetSection(FeeSectionEnum.For800, true);
                break;
              case "900":
                fee.SetSection(FeeSectionEnum.For900, true);
                break;
              case "PC":
                fee.SetSection(FeeSectionEnum.ForPC, true);
                break;
            }
          }
        }
        managementSetting.AddFee(fee);
      }
      return managementSetting;
    }

    private void refreshUcdFeeList()
    {
      string[] strArray = this.readUcdFeeList();
      this.gridViewUCD.SortOption = GVSortOption.None;
      this.gridViewUCD.BeginUpdate();
      this.gridViewUCD.Items.Clear();
      for (int index = 0; index < strArray.Length; ++index)
        this.gridViewUCD.Items.Add(new GVItem(strArray[index]));
      this.gridViewUCD.SortOption = GVSortOption.Auto;
      this.gridViewUCD.Sort(0, SortOrder.Ascending);
      this.gridViewUCD.EndUpdate();
      Cursor.Current = Cursors.Default;
      this.fullUcdFeeList = strArray;
    }

    private void refreshUncheckedUcdFeeList()
    {
      List<string> stringList = new List<string>();
      for (int nItemIndex = 0; nItemIndex < this.gridFees.Items.Count; ++nItemIndex)
      {
        string text = this.gridFees.Items[nItemIndex].SubItems[2].Text;
        if (text != "Not Mapped")
          stringList.Add(text);
      }
      if (stringList.Count == 0 || this.fullUcdFeeList == null)
        return;
      this.gridViewUCD.SortOption = GVSortOption.None;
      this.gridViewUCD.BeginUpdate();
      this.gridViewUCD.Items.Clear();
      for (int index = 0; index < this.fullUcdFeeList.Length; ++index)
      {
        if (!stringList.Contains(this.fullUcdFeeList[index]))
          this.gridViewUCD.Items.Add(new GVItem(this.fullUcdFeeList[index]));
      }
      this.gridViewUCD.SortOption = GVSortOption.Auto;
      this.gridViewUCD.ReSort();
      this.gridViewUCD.EndUpdate();
      Cursor.Current = Cursors.Default;
    }

    private string[] readUcdFeeList()
    {
      try
      {
        string resourceFileFullPath = AssemblyResolver.GetResourceFileFullPath(SystemSettings.UcdFeeTypeEnumsPath, SystemSettings.LocalAppDir);
        return System.IO.File.Exists(resourceFileFullPath) ? System.IO.File.ReadAllLines(resourceFileFullPath) : new string[0];
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Cannot open UCD Fee Type Enums File");
        return new string[0];
      }
    }

    public void validateFeeNameInUCD(FeeManagementRecord[] fees)
    {
      if (this.fullUcdFeeList == null)
        return;
      for (int index = 0; index < fees.Length; ++index)
      {
        if (!string.IsNullOrEmpty(fees[index].FeeNameInUCD) && !((IEnumerable<string>) this.fullUcdFeeList).Contains<string>(fees[index].FeeNameInUCD))
          fees[index].FeeNameInUCD = string.Empty;
      }
    }

    private void chkAll_CheckedChanged(object sender, EventArgs e) => this.refreshMaventFeeList();

    private void chkAllUCD_CheckedChanged(object sender, EventArgs e)
    {
      if (this.chkAllUCD.Checked)
        this.refreshUcdFeeList();
      else
        this.refreshUncheckedUcdFeeList();
    }

    private void mnuItemImportCC_Click(object sender, EventArgs e)
    {
      using (TemplateSelectionDialog templateSelectionDialog = new TemplateSelectionDialog(this.session, EllieMae.EMLite.ClientServer.TemplateSettingsType.ClosingCost, (FileSystemEntry) null, false, FSExplorer.RESPAFilter.All))
      {
        if (templateSelectionDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        FileSystemEntry selectedItem = templateSelectionDialog.SelectedItem;
        ClosingCost templateSettings;
        try
        {
          templateSettings = (ClosingCost) this.session.ConfigurationManager.GetTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.ClosingCost, selectedItem);
        }
        catch (Exception ex)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Can't open " + selectedItem.Name + " Closing Cost template file. " + ex.Message);
          return;
        }
        Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
        for (int nItemIndex = 0; nItemIndex < this.gridFees.Items.Count; ++nItemIndex)
        {
          if (!insensitiveHashtable.ContainsKey((object) this.gridFees.Items[nItemIndex].Text))
            insensitiveHashtable.Add((object) this.gridFees.Items[nItemIndex].Text, (object) (FeeManagementRecord) this.gridFees.Items[nItemIndex].Tag);
        }
        bool flag = false;
        foreach (string templateField in ClosingCost.TemplateFields)
        {
          string simpleField = templateSettings.GetSimpleField(templateField);
          if (!(simpleField == string.Empty))
          {
            FeeSectionEnum fieldSectionEnum = HUDGFE2010Fields.GetFieldSectionEnum(templateField);
            if (fieldSectionEnum != FeeSectionEnum.Nothing)
            {
              if (!insensitiveHashtable.ContainsKey((object) simpleField))
                insensitiveHashtable.Add((object) simpleField, (object) new FeeManagementRecord(simpleField, false, false, false, false, false, false, false, false, string.Empty, string.Empty, string.Empty, "Not Mapped"));
              ((FeeManagementRecord) insensitiveHashtable[(object) simpleField]).SetSection(fieldSectionEnum, true);
              this.feeIsDirty = true;
              this.setDirtyFlag(true);
              flag = true;
            }
          }
        }
        if (!flag)
          return;
        this.gridFees.BeginUpdate();
        this.gridFees.Items.Clear();
        foreach (DictionaryEntry dictionaryEntry in insensitiveHashtable)
          this.gridFees.Items.Add(this.buildFeeGVItem((FeeManagementRecord) dictionaryEntry.Value, false));
        this.gridFees.Sort(0, SortOrder.Ascending);
        this.gridFees.EndUpdate();
      }
    }

    private void mnuItemImportDefault_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Importing the default fees will overwrite all existing fees and customizations. Are you sure you want to DELETE all of your existing fees and replace them with the default list?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
        return;
      byte[] bytes = this.downloadFile("FeeManagementDefaultList.txt");
      if (bytes == null || bytes.Length == 0)
        return;
      string str1 = Encoding.ASCII.GetString(bytes);
      int num = 700;
      string empty = string.Empty;
      string feeID = string.Empty;
      string[] strArray1 = str1.Split('\r');
      if (strArray1 == null || strArray1.Length == 0)
        return;
      FeeManagementSetting managementSetting = new FeeManagementSetting();
      for (int index = 0; index < strArray1.Length; ++index)
      {
        string str2 = strArray1[index].Replace("\n", "").Trim();
        if (!(str2 == string.Empty))
        {
          if (string.Compare(str2, "Section: 700", true) == 0)
            num = 700;
          else if (string.Compare(str2, "Section: 800", true) == 0)
            num = 800;
          else if (string.Compare(str2, "Section: 900", true) == 0)
            num = 900;
          else if (string.Compare(str2, "Section: 1000", true) == 0)
            num = 1000;
          else if (string.Compare(str2, "Section: 1100", true) == 0)
            num = 1100;
          else if (string.Compare(str2, "Section: 1200", true) == 0)
            num = 1200;
          else if (string.Compare(str2, "Section: 1300", true) == 0)
            num = 1300;
          else if (string.Compare(str2, "Section: PC", true) == 0)
            num = 2000;
          else if (string.Compare(str2, "FeeIDList:", true) == 0)
          {
            num = 9999;
          }
          else
          {
            if (num == 9999)
            {
              string[] strArray2 = str2.Split('@');
              if (strArray2 != null && strArray2.Length == 2)
              {
                str2 = strArray2[0].Trim();
                feeID = strArray2[1].Trim();
              }
              else
                continue;
            }
            switch (str2)
            {
              case "USDA/RHS Upfront Guarantee Fee":
                str2 = "Guarantee Fee";
                break;
              case "USDA/RHS Annual Fee Monthly Premium-Impound":
                str2 = "USDA Annual Fee";
                break;
            }
            FeeManagementRecord fee = managementSetting.GetFeeManagementRecord(str2);
            if (fee != null || num != 9999)
            {
              string str3 = str2;
              switch (str2)
              {
                case "Lender Compensation Credit":
                  str3 = "Lump Sum Lender Credit";
                  break;
                case "Origination Credit":
                  str3 = "Lump Sum Lender Credit";
                  break;
                case "Origination Points":
                  str3 = "Loan Discount Points";
                  break;
                case "Guarantee Fee":
                  str3 = "USDA/RHS Upfront Guarantee Fee";
                  break;
                case "USDA Annual Fee":
                  str3 = "USDA/RHS Annual Fee Monthly Premium-Impound";
                  break;
              }
              if (fee == null)
              {
                fee = new FeeManagementRecord(str2, false, false, false, false, false, false, false, false, this.session.UserID, str3, (string) null, "");
                managementSetting.AddFee(fee);
              }
              switch (num)
              {
                case 700:
                  fee.SetSection(FeeSectionEnum.For700, true);
                  continue;
                case 800:
                  fee.SetSection(FeeSectionEnum.For800, true);
                  continue;
                case 900:
                  fee.SetSection(FeeSectionEnum.For900, true);
                  continue;
                case 1000:
                  fee.SetSection(FeeSectionEnum.For1000, true);
                  continue;
                case 1100:
                  fee.SetSection(FeeSectionEnum.For1100, true);
                  continue;
                case 1200:
                  fee.SetSection(FeeSectionEnum.For1200, true);
                  continue;
                case 1300:
                  fee.SetSection(FeeSectionEnum.For1300, true);
                  continue;
                case 2000:
                  fee.SetSection(FeeSectionEnum.ForPC, true);
                  continue;
                case 9999:
                  if (feeID != string.Empty)
                  {
                    fee.SetMaventFee(str3, feeID);
                    continue;
                  }
                  continue;
                default:
                  continue;
              }
            }
          }
        }
      }
      if (managementSetting.NumberOfFees <= 0)
        return;
      this.gridFees.Items.Clear();
      this.gridFees.BeginUpdate();
      FeeManagementRecord[] allFees = managementSetting.GetAllFees();
      this.validateFeeNameInUCD(allFees);
      for (int index = 0; index < allFees.Length; ++index)
        this.gridFees.Items.Add(this.buildFeeGVItem(allFees[index], false));
      this.gridFees.EndUpdate();
      this.setDirtyFlag(true);
      this.feeIsDirty = true;
      this.gridFees.Sort(0, SortOrder.Ascending);
    }

    public string[] SelectedFeeNames
    {
      get
      {
        return this.gridFees.SelectedItems.Count == 0 ? (string[]) null : this.gridFees.SelectedItems.Select<GVItem, string>((Func<GVItem, string>) (item => item.SubItems[0].Text)).ToArray<string>();
      }
    }

    public string[] SelectedPersonas
    {
      get
      {
        return this.gridPersona.SelectedItems.Count == 0 ? (string[]) null : this.gridPersona.SelectedItems.Select<GVItem, string>((Func<GVItem, string>) (item => item.SubItems[0].Text)).ToArray<string>();
      }
    }

    public void SetSelectedFeeNames(List<string> selectedFeeNames)
    {
      foreach (GVItem gvItem in this.gridFees.Items.Where<GVItem>((Func<GVItem, bool>) (item => selectedFeeNames.Contains(item.SubItems[0].Text))))
        gvItem.Selected = true;
    }

    public void DeselectPersonas()
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gridPersona.Items)
        gvItem.Selected = false;
    }

    public bool IsOnPersonaTab => this.tabControlEx1.SelectedPage == this.tabPagePersona;

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
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      GVColumn gvColumn7 = new GVColumn();
      GVColumn gvColumn8 = new GVColumn();
      GVColumn gvColumn9 = new GVColumn();
      GVColumn gvColumn10 = new GVColumn();
      GVColumn gvColumn11 = new GVColumn();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FeeManagementControl));
      GVColumn gvColumn12 = new GVColumn();
      GVColumn gvColumn13 = new GVColumn();
      GVColumn gvColumn14 = new GVColumn();
      GVColumn gvColumn15 = new GVColumn();
      this.groupContainer1 = new GroupContainer();
      this.chkCompanyOptIn = new CheckBox();
      this.tabControlEx1 = new TabControlEx();
      this.tabPageFeeList = new TabPageEx();
      this.panelFeeList = new Panel();
      this.panelFeeTab = new Panel();
      this.panelLeft = new Panel();
      this.groupContainer2 = new GroupContainer();
      this.gridFees = new GridView();
      this.imageListMap = new ImageList(this.components);
      this.gradientPanel1 = new GradientPanel();
      this.label1 = new Label();
      this.btnImport = new ContextMenuButton();
      this.mnuImportFees = new ContextMenuStrip(this.components);
      this.mnuItemImportCC = new ToolStripMenuItem();
      this.mnuItemImportDefault = new ToolStripMenuItem();
      this.panelIconGroup = new Panel();
      this.verticalSeparator1 = new VerticalSeparator();
      this.iconBtnNew = new StandardIconButton();
      this.iconBtnDelete = new StandardIconButton();
      this.splitterFeeList = new CollapsibleSplitter();
      this.panelRight = new Panel();
      this.groupContainer3 = new GroupContainer();
      this.gridViewCompliance = new GridView();
      this.gradientPanel2 = new GradientPanel();
      this.label2 = new Label();
      this.chkAll = new CheckBox();
      this.splitterComponentAndUcdFeeLists = new CollapsibleSplitter();
      this.groupContainer4 = new GroupContainer();
      this.chkAllUCD = new CheckBox();
      this.gridViewUCD = new GridView();
      this.gradientPanel3 = new GradientPanel();
      this.label3 = new Label();
      this.tabPagePersona = new TabPageEx();
      this.panelPersonaRight = new Panel();
      this.gcPersonaOverwrite = new GroupContainer();
      this.gridOverwrite = new GridView();
      this.collapsibleSplitter1 = new CollapsibleSplitter();
      this.panelPersonaLeft = new Panel();
      this.gcPersonas = new GroupContainer();
      this.gridPersona = new GridView();
      this.groupContainer1.SuspendLayout();
      this.tabControlEx1.SuspendLayout();
      this.tabPageFeeList.SuspendLayout();
      this.panelFeeList.SuspendLayout();
      this.panelFeeTab.SuspendLayout();
      this.panelLeft.SuspendLayout();
      this.groupContainer2.SuspendLayout();
      this.gradientPanel1.SuspendLayout();
      this.mnuImportFees.SuspendLayout();
      this.panelIconGroup.SuspendLayout();
      ((ISupportInitialize) this.iconBtnNew).BeginInit();
      ((ISupportInitialize) this.iconBtnDelete).BeginInit();
      this.panelRight.SuspendLayout();
      this.groupContainer3.SuspendLayout();
      this.gradientPanel2.SuspendLayout();
      this.groupContainer4.SuspendLayout();
      this.gradientPanel3.SuspendLayout();
      this.tabPagePersona.SuspendLayout();
      this.panelPersonaRight.SuspendLayout();
      this.gcPersonaOverwrite.SuspendLayout();
      this.panelPersonaLeft.SuspendLayout();
      this.gcPersonas.SuspendLayout();
      this.SuspendLayout();
      this.groupContainer1.Controls.Add((Control) this.chkCompanyOptIn);
      this.groupContainer1.Controls.Add((Control) this.tabControlEx1);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Margin = new Padding(4, 5, 4, 5);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(1308, 809);
      this.groupContainer1.TabIndex = 0;
      this.groupContainer1.Text = "Fee Management";
      this.chkCompanyOptIn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkCompanyOptIn.AutoSize = true;
      this.chkCompanyOptIn.BackColor = Color.Transparent;
      this.chkCompanyOptIn.Location = new Point(1129, 9);
      this.chkCompanyOptIn.Margin = new Padding(4, 5, 4, 5);
      this.chkCompanyOptIn.Name = "chkCompanyOptIn";
      this.chkCompanyOptIn.Size = new Size(174, 24);
      this.chkCompanyOptIn.TabIndex = 1;
      this.chkCompanyOptIn.Text = "Apply to Itemization";
      this.chkCompanyOptIn.UseVisualStyleBackColor = false;
      this.chkCompanyOptIn.Click += new EventHandler(this.chkCompanyOptIn_Click);
      this.tabControlEx1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.tabControlEx1.Location = new Point(6, 45);
      this.tabControlEx1.Margin = new Padding(4, 5, 4, 5);
      this.tabControlEx1.Name = "tabControlEx1";
      this.tabControlEx1.Size = new Size(1296, 760);
      this.tabControlEx1.TabHeight = 20;
      this.tabControlEx1.TabIndex = 0;
      this.tabControlEx1.TabPages.Add(this.tabPageFeeList);
      this.tabControlEx1.TabPages.Add(this.tabPagePersona);
      this.tabControlEx1.Text = "tabControlEx1";
      this.tabPageFeeList.BackColor = Color.Transparent;
      this.tabPageFeeList.Controls.Add((Control) this.panelFeeList);
      this.tabPageFeeList.Location = new Point(1, 23);
      this.tabPageFeeList.Name = "tabPageFeeList";
      this.tabPageFeeList.TabIndex = 0;
      this.tabPageFeeList.TabWidth = 100;
      this.tabPageFeeList.Text = "Fee List";
      this.tabPageFeeList.Value = (object) "Fee List";
      this.panelFeeList.Controls.Add((Control) this.panelFeeTab);
      this.panelFeeList.Dock = DockStyle.Fill;
      this.panelFeeList.Location = new Point(0, 0);
      this.panelFeeList.Name = "panelFeeList";
      this.panelFeeList.Size = new Size(1292, 734);
      this.panelFeeList.TabIndex = 1;
      this.panelFeeTab.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.panelFeeTab.Controls.Add((Control) this.panelLeft);
      this.panelFeeTab.Controls.Add((Control) this.splitterFeeList);
      this.panelFeeTab.Controls.Add((Control) this.panelRight);
      this.panelFeeTab.Location = new Point(3, 3);
      this.panelFeeTab.Name = "panelFeeTab";
      this.panelFeeTab.Size = new Size(1286, 728);
      this.panelFeeTab.TabIndex = 2;
      this.panelLeft.Controls.Add((Control) this.groupContainer2);
      this.panelLeft.Dock = DockStyle.Fill;
      this.panelLeft.Location = new Point(0, 0);
      this.panelLeft.Name = "panelLeft";
      this.panelLeft.Size = new Size(1079, 728);
      this.panelLeft.TabIndex = 2;
      this.groupContainer2.Controls.Add((Control) this.gridFees);
      this.groupContainer2.Controls.Add((Control) this.gradientPanel1);
      this.groupContainer2.Controls.Add((Control) this.btnImport);
      this.groupContainer2.Controls.Add((Control) this.panelIconGroup);
      this.groupContainer2.Dock = DockStyle.Fill;
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(0, 0);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(1079, 728);
      this.groupContainer2.TabIndex = 0;
      this.groupContainer2.Text = "Fee List";
      this.gridFees.AllowDrop = true;
      this.gridFees.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Encompass Fee Name";
      gvColumn1.Width = 200;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column8";
      gvColumn2.Text = "Mapped Compliance Fee";
      gvColumn2.Width = 200;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column9";
      gvColumn3.Text = "Mapped UCD Fee";
      gvColumn3.Width = 200;
      gvColumn4.CheckBoxes = true;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column11";
      gvColumn4.SortMethod = GVSortMethod.Checkbox;
      gvColumn4.Text = "Section 700";
      gvColumn4.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn4.Width = 75;
      gvColumn5.CheckBoxes = true;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column2";
      gvColumn5.SortMethod = GVSortMethod.Checkbox;
      gvColumn5.Text = "Section 800";
      gvColumn5.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn5.Width = 75;
      gvColumn6.CheckBoxes = true;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column3";
      gvColumn6.SortMethod = GVSortMethod.Checkbox;
      gvColumn6.Text = "Section 900";
      gvColumn6.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn6.Width = 75;
      gvColumn7.CheckBoxes = true;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Column4";
      gvColumn7.SortMethod = GVSortMethod.Checkbox;
      gvColumn7.Text = "Section 1000";
      gvColumn7.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn7.Width = 75;
      gvColumn8.CheckBoxes = true;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "Column5";
      gvColumn8.SortMethod = GVSortMethod.Checkbox;
      gvColumn8.Text = "Section 1100";
      gvColumn8.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn8.Width = 75;
      gvColumn9.CheckBoxes = true;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "Column6";
      gvColumn9.SortMethod = GVSortMethod.Checkbox;
      gvColumn9.Text = "Section 1200";
      gvColumn9.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn9.Width = 75;
      gvColumn10.CheckBoxes = true;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "Column7";
      gvColumn10.SortMethod = GVSortMethod.Checkbox;
      gvColumn10.Text = "Section 1300";
      gvColumn10.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn10.Width = 75;
      gvColumn11.CheckBoxes = true;
      gvColumn11.ImageIndex = -1;
      gvColumn11.Name = "Column10";
      gvColumn11.SortMethod = GVSortMethod.Checkbox;
      gvColumn11.Text = "Section PC";
      gvColumn11.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn11.Width = 100;
      this.gridFees.Columns.AddRange(new GVColumn[11]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7,
        gvColumn8,
        gvColumn9,
        gvColumn10,
        gvColumn11
      });
      this.gridFees.Dock = DockStyle.Fill;
      this.gridFees.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridFees.ImageList = this.imageListMap;
      this.gridFees.Location = new Point(1, 48);
      this.gridFees.Name = "gridFees";
      this.gridFees.Size = new Size(1077, 679);
      this.gridFees.TabIndex = 13;
      this.gridFees.SelectedIndexChanged += new EventHandler(this.gridFees_SelectedIndexChanged);
      this.gridFees.SubItemCheck += new GVSubItemEventHandler(this.gridFees_SubItemCheck);
      this.gridFees.DragDrop += new DragEventHandler(this.gridFees_DragDrop);
      this.gridFees.DragEnter += new DragEventHandler(this.gridFees_DragEnter);
      this.gridFees.DragOver += new DragEventHandler(this.gridFees_DragOver);
      this.imageListMap.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imageListMap.ImageStream");
      this.imageListMap.TransparentColor = Color.Transparent;
      this.imageListMap.Images.SetKeyName(0, "feelink-bad");
      this.imageListMap.Images.SetKeyName(1, "link");
      this.gradientPanel1.Borders = AnchorStyles.Bottom;
      this.gradientPanel1.Controls.Add((Control) this.label1);
      this.gradientPanel1.Dock = DockStyle.Top;
      this.gradientPanel1.GradientColor1 = Color.White;
      this.gradientPanel1.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel1.Location = new Point(1, 26);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(1077, 22);
      this.gradientPanel1.TabIndex = 12;
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Location = new Point(2, 4);
      this.label1.Name = "label1";
      this.label1.Size = new Size(158, 20);
      this.label1.TabIndex = 0;
      this.label1.Text = "Create or import fees";
      this.btnImport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnImport.BackColor = SystemColors.Control;
      this.btnImport.ButtonMenuStrip = this.mnuImportFees;
      this.btnImport.Location = new Point(990, 2);
      this.btnImport.Margin = new Padding(0);
      this.btnImport.Name = "btnImport";
      this.btnImport.Size = new Size(86, 22);
      this.btnImport.TabIndex = 11;
      this.btnImport.TabStop = false;
      this.btnImport.Text = "Import Fees";
      this.btnImport.UseVisualStyleBackColor = true;
      this.mnuImportFees.ImageScalingSize = new Size(24, 24);
      this.mnuImportFees.Items.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.mnuItemImportCC,
        (ToolStripItem) this.mnuItemImportDefault
      });
      this.mnuImportFees.Name = "mnuSend";
      this.mnuImportFees.Size = new Size(425, 64);
      this.mnuItemImportCC.Name = "mnuItemImportCC";
      this.mnuItemImportCC.Size = new Size(424, 30);
      this.mnuItemImportCC.Text = "Import Fees from Closing Cost Templates...";
      this.mnuItemImportCC.Click += new EventHandler(this.mnuItemImportCC_Click);
      this.mnuItemImportDefault.Name = "mnuItemImportDefault";
      this.mnuItemImportDefault.Size = new Size(424, 30);
      this.mnuItemImportDefault.Text = "Import Compliance Default Fees...";
      this.mnuItemImportDefault.Click += new EventHandler(this.mnuItemImportDefault_Click);
      this.panelIconGroup.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.panelIconGroup.BackColor = Color.Transparent;
      this.panelIconGroup.Controls.Add((Control) this.verticalSeparator1);
      this.panelIconGroup.Controls.Add((Control) this.iconBtnNew);
      this.panelIconGroup.Controls.Add((Control) this.iconBtnDelete);
      this.panelIconGroup.Location = new Point(938, 0);
      this.panelIconGroup.Name = "panelIconGroup";
      this.panelIconGroup.Size = new Size(52, 24);
      this.panelIconGroup.TabIndex = 6;
      this.verticalSeparator1.Location = new Point(46, 4);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 2;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.iconBtnNew.BackColor = Color.Transparent;
      this.iconBtnNew.Location = new Point(4, 4);
      this.iconBtnNew.MouseDownImage = (Image) null;
      this.iconBtnNew.Name = "iconBtnNew";
      this.iconBtnNew.Size = new Size(16, 16);
      this.iconBtnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.iconBtnNew.TabIndex = 4;
      this.iconBtnNew.TabStop = false;
      this.iconBtnNew.Click += new EventHandler(this.iconBtnNew_Click);
      this.iconBtnDelete.BackColor = Color.Transparent;
      this.iconBtnDelete.Location = new Point(25, 4);
      this.iconBtnDelete.MouseDownImage = (Image) null;
      this.iconBtnDelete.Name = "iconBtnDelete";
      this.iconBtnDelete.Size = new Size(16, 16);
      this.iconBtnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.iconBtnDelete.TabIndex = 3;
      this.iconBtnDelete.TabStop = false;
      this.iconBtnDelete.Click += new EventHandler(this.iconBtnDelete_Click);
      this.splitterFeeList.AnimationDelay = 20;
      this.splitterFeeList.AnimationStep = 20;
      this.splitterFeeList.BorderStyle3D = Border3DStyle.Flat;
      this.splitterFeeList.ControlToHide = (Control) this.panelRight;
      this.splitterFeeList.Dock = DockStyle.Right;
      this.splitterFeeList.ExpandParentForm = false;
      this.splitterFeeList.Location = new Point(1079, 0);
      this.splitterFeeList.Name = "splitterFeeList";
      this.splitterFeeList.TabIndex = 1;
      this.splitterFeeList.TabStop = false;
      this.splitterFeeList.UseAnimations = false;
      this.splitterFeeList.VisualStyle = VisualStyles.Encompass;
      this.panelRight.Controls.Add((Control) this.groupContainer3);
      this.panelRight.Controls.Add((Control) this.splitterComponentAndUcdFeeLists);
      this.panelRight.Controls.Add((Control) this.groupContainer4);
      this.panelRight.Dock = DockStyle.Right;
      this.panelRight.Location = new Point(1086, 0);
      this.panelRight.Name = "panelRight";
      this.panelRight.Size = new Size(200, 728);
      this.panelRight.TabIndex = 0;
      this.groupContainer3.Controls.Add((Control) this.gridViewCompliance);
      this.groupContainer3.Controls.Add((Control) this.gradientPanel2);
      this.groupContainer3.Controls.Add((Control) this.chkAll);
      this.groupContainer3.Dock = DockStyle.Fill;
      this.groupContainer3.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer3.Location = new Point(0, 0);
      this.groupContainer3.Name = "groupContainer3";
      this.groupContainer3.Size = new Size(200, 489);
      this.groupContainer3.TabIndex = 3;
      this.groupContainer3.Text = "Compliance Fees";
      this.gridViewCompliance.BorderStyle = BorderStyle.None;
      gvColumn12.ImageIndex = -1;
      gvColumn12.Name = "Column1";
      gvColumn12.SpringToFit = true;
      gvColumn12.Text = "Compliance Fee";
      gvColumn12.Width = 198;
      this.gridViewCompliance.Columns.AddRange(new GVColumn[1]
      {
        gvColumn12
      });
      this.gridViewCompliance.Dock = DockStyle.Fill;
      this.gridViewCompliance.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridViewCompliance.Location = new Point(1, 48);
      this.gridViewCompliance.Name = "gridViewCompliance";
      this.gridViewCompliance.Size = new Size(198, 440);
      this.gridViewCompliance.TabIndex = 14;
      this.gridViewCompliance.ItemDrag += new GVItemEventHandler(this.gridViewCompliance_ItemDrag);
      this.gradientPanel2.Borders = AnchorStyles.Bottom;
      this.gradientPanel2.Controls.Add((Control) this.label2);
      this.gradientPanel2.Dock = DockStyle.Top;
      this.gradientPanel2.GradientColor1 = Color.White;
      this.gradientPanel2.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel2.Location = new Point(1, 26);
      this.gradientPanel2.Name = "gradientPanel2";
      this.gradientPanel2.Size = new Size(198, 22);
      this.gradientPanel2.TabIndex = 13;
      this.label2.AutoSize = true;
      this.label2.BackColor = Color.Transparent;
      this.label2.Location = new Point(2, 4);
      this.label2.Name = "label2";
      this.label2.Size = new Size(173, 20);
      this.label2.TabIndex = 0;
      this.label2.Text = "Drag/drop to fee on left";
      this.chkAll.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkAll.AutoSize = true;
      this.chkAll.BackColor = Color.Transparent;
      this.chkAll.Location = new Point(104, 6);
      this.chkAll.Name = "chkAll";
      this.chkAll.Size = new Size(96, 24);
      this.chkAll.TabIndex = 3;
      this.chkAll.Text = "Show All";
      this.chkAll.UseVisualStyleBackColor = false;
      this.chkAll.CheckedChanged += new EventHandler(this.chkAll_CheckedChanged);
      this.splitterComponentAndUcdFeeLists.AnimationDelay = 20;
      this.splitterComponentAndUcdFeeLists.AnimationStep = 20;
      this.splitterComponentAndUcdFeeLists.BorderStyle3D = Border3DStyle.Flat;
      this.splitterComponentAndUcdFeeLists.ControlToHide = (Control) this.groupContainer4;
      this.splitterComponentAndUcdFeeLists.Dock = DockStyle.Bottom;
      this.splitterComponentAndUcdFeeLists.ExpandParentForm = false;
      this.splitterComponentAndUcdFeeLists.Location = new Point(0, 489);
      this.splitterComponentAndUcdFeeLists.Name = "splitterComponentAndUcdFeeLists";
      this.splitterComponentAndUcdFeeLists.TabIndex = 5;
      this.splitterComponentAndUcdFeeLists.TabStop = false;
      this.splitterComponentAndUcdFeeLists.UseAnimations = false;
      this.splitterComponentAndUcdFeeLists.VisualStyle = VisualStyles.Encompass;
      this.groupContainer4.Controls.Add((Control) this.chkAllUCD);
      this.groupContainer4.Controls.Add((Control) this.gridViewUCD);
      this.groupContainer4.Controls.Add((Control) this.gradientPanel3);
      this.groupContainer4.Dock = DockStyle.Bottom;
      this.groupContainer4.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer4.Location = new Point(0, 496);
      this.groupContainer4.Name = "groupContainer4";
      this.groupContainer4.Size = new Size(200, 232);
      this.groupContainer4.TabIndex = 4;
      this.groupContainer4.Text = "UCD Fees";
      this.chkAllUCD.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkAllUCD.AutoSize = true;
      this.chkAllUCD.BackColor = Color.Transparent;
      this.chkAllUCD.Location = new Point(104, 6);
      this.chkAllUCD.Name = "chkAllUCD";
      this.chkAllUCD.Size = new Size(96, 24);
      this.chkAllUCD.TabIndex = 16;
      this.chkAllUCD.Text = "Show All";
      this.chkAllUCD.UseVisualStyleBackColor = false;
      this.chkAllUCD.CheckedChanged += new EventHandler(this.chkAllUCD_CheckedChanged);
      this.gridViewUCD.BorderStyle = BorderStyle.None;
      gvColumn13.ImageIndex = -1;
      gvColumn13.Name = "Column1";
      gvColumn13.SpringToFit = true;
      gvColumn13.Text = "UCD Fee";
      gvColumn13.Width = 198;
      this.gridViewUCD.Columns.AddRange(new GVColumn[1]
      {
        gvColumn13
      });
      this.gridViewUCD.Dock = DockStyle.Fill;
      this.gridViewUCD.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridViewUCD.Location = new Point(1, 48);
      this.gridViewUCD.Name = "gridViewUCD";
      this.gridViewUCD.Size = new Size(198, 183);
      this.gridViewUCD.TabIndex = 15;
      this.gridViewUCD.ItemDrag += new GVItemEventHandler(this.gridViewUcd_ItemDrag);
      this.gradientPanel3.Borders = AnchorStyles.Bottom;
      this.gradientPanel3.Controls.Add((Control) this.label3);
      this.gradientPanel3.Dock = DockStyle.Top;
      this.gradientPanel3.GradientColor1 = Color.White;
      this.gradientPanel3.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel3.Location = new Point(1, 26);
      this.gradientPanel3.Name = "gradientPanel3";
      this.gradientPanel3.Size = new Size(198, 22);
      this.gradientPanel3.TabIndex = 14;
      this.label3.AutoSize = true;
      this.label3.BackColor = Color.Transparent;
      this.label3.Location = new Point(2, 4);
      this.label3.Name = "label3";
      this.label3.Size = new Size(173, 20);
      this.label3.TabIndex = 0;
      this.label3.Text = "Drag/drop to fee on left";
      this.tabPagePersona.BackColor = Color.Transparent;
      this.tabPagePersona.Controls.Add((Control) this.panelPersonaRight);
      this.tabPagePersona.Controls.Add((Control) this.collapsibleSplitter1);
      this.tabPagePersona.Controls.Add((Control) this.panelPersonaLeft);
      this.tabPagePersona.Location = new Point(1, 23);
      this.tabPagePersona.Name = "tabPagePersona";
      this.tabPagePersona.TabIndex = 0;
      this.tabPagePersona.TabWidth = 100;
      this.tabPagePersona.Text = "Persona Overwrite";
      this.tabPagePersona.Value = (object) "Persona Overwrite";
      this.panelPersonaRight.Controls.Add((Control) this.gcPersonaOverwrite);
      this.panelPersonaRight.Dock = DockStyle.Fill;
      this.panelPersonaRight.Location = new Point(257, 0);
      this.panelPersonaRight.Name = "panelPersonaRight";
      this.panelPersonaRight.Size = new Size(1035, 734);
      this.panelPersonaRight.TabIndex = 3;
      this.gcPersonaOverwrite.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gcPersonaOverwrite.Controls.Add((Control) this.gridOverwrite);
      this.gcPersonaOverwrite.HeaderForeColor = SystemColors.ControlText;
      this.gcPersonaOverwrite.Location = new Point(0, 6);
      this.gcPersonaOverwrite.Name = "gcPersonaOverwrite";
      this.gcPersonaOverwrite.Size = new Size(1088, 723);
      this.gcPersonaOverwrite.TabIndex = 2;
      this.gcPersonaOverwrite.Text = "Persona Overwrite";
      this.gridOverwrite.AllowMultiselect = false;
      this.gridOverwrite.BorderStyle = BorderStyle.None;
      gvColumn14.CheckBoxes = true;
      gvColumn14.ImageIndex = -1;
      gvColumn14.Name = "Column1";
      gvColumn14.SpringToFit = true;
      gvColumn14.Text = "Select sections that the persona can enter their own fee names.";
      gvColumn14.Width = 1086;
      this.gridOverwrite.Columns.AddRange(new GVColumn[1]
      {
        gvColumn14
      });
      this.gridOverwrite.Dock = DockStyle.Fill;
      this.gridOverwrite.HeaderHeight = 0;
      this.gridOverwrite.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridOverwrite.Location = new Point(1, 26);
      this.gridOverwrite.Name = "gridOverwrite";
      this.gridOverwrite.Size = new Size(1086, 696);
      this.gridOverwrite.TabIndex = 1;
      this.gridOverwrite.SubItemCheck += new GVSubItemEventHandler(this.gridOverwrite_SubItemCheck);
      this.collapsibleSplitter1.AnimationDelay = 20;
      this.collapsibleSplitter1.AnimationStep = 20;
      this.collapsibleSplitter1.BorderStyle3D = Border3DStyle.Flat;
      this.collapsibleSplitter1.ControlToHide = (Control) this.panelPersonaLeft;
      this.collapsibleSplitter1.ExpandParentForm = false;
      this.collapsibleSplitter1.Location = new Point(250, 0);
      this.collapsibleSplitter1.Name = "collapsibleSplitter1";
      this.collapsibleSplitter1.TabIndex = 2;
      this.collapsibleSplitter1.TabStop = false;
      this.collapsibleSplitter1.UseAnimations = false;
      this.collapsibleSplitter1.VisualStyle = VisualStyles.Encompass;
      this.panelPersonaLeft.Controls.Add((Control) this.gcPersonas);
      this.panelPersonaLeft.Dock = DockStyle.Left;
      this.panelPersonaLeft.Location = new Point(0, 0);
      this.panelPersonaLeft.Name = "panelPersonaLeft";
      this.panelPersonaLeft.Size = new Size(250, 734);
      this.panelPersonaLeft.TabIndex = 1;
      this.gcPersonas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gcPersonas.Controls.Add((Control) this.gridPersona);
      this.gcPersonas.HeaderForeColor = SystemColors.ControlText;
      this.gcPersonas.Location = new Point(4, 6);
      this.gcPersonas.Name = "gcPersonas";
      this.gcPersonas.Size = new Size(246, 723);
      this.gcPersonas.TabIndex = 0;
      this.gcPersonas.Text = "Personas";
      this.gridPersona.AllowMultiselect = false;
      this.gridPersona.BorderStyle = BorderStyle.None;
      gvColumn15.ImageIndex = -1;
      gvColumn15.Name = "Column1";
      gvColumn15.SpringToFit = true;
      gvColumn15.Text = "Persona";
      gvColumn15.Width = 244;
      this.gridPersona.Columns.AddRange(new GVColumn[1]
      {
        gvColumn15
      });
      this.gridPersona.Dock = DockStyle.Fill;
      this.gridPersona.HeaderHeight = 0;
      this.gridPersona.HeaderVisible = false;
      this.gridPersona.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridPersona.Location = new Point(1, 26);
      this.gridPersona.Name = "gridPersona";
      this.gridPersona.Size = new Size(244, 696);
      this.gridPersona.TabIndex = 0;
      this.gridPersona.SelectedIndexChanged += new EventHandler(this.gridPersona_SelectedIndexChanged);
      this.AutoScaleDimensions = new SizeF(9f, 20f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.groupContainer1);
      this.Margin = new Padding(4, 5, 4, 5);
      this.Name = nameof (FeeManagementControl);
      this.Size = new Size(1308, 809);
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.tabControlEx1.ResumeLayout(false);
      this.tabPageFeeList.ResumeLayout(false);
      this.panelFeeList.ResumeLayout(false);
      this.panelFeeTab.ResumeLayout(false);
      this.panelLeft.ResumeLayout(false);
      this.groupContainer2.ResumeLayout(false);
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      this.mnuImportFees.ResumeLayout(false);
      this.panelIconGroup.ResumeLayout(false);
      ((ISupportInitialize) this.iconBtnNew).EndInit();
      ((ISupportInitialize) this.iconBtnDelete).EndInit();
      this.panelRight.ResumeLayout(false);
      this.groupContainer3.ResumeLayout(false);
      this.groupContainer3.PerformLayout();
      this.gradientPanel2.ResumeLayout(false);
      this.gradientPanel2.PerformLayout();
      this.groupContainer4.ResumeLayout(false);
      this.groupContainer4.PerformLayout();
      this.gradientPanel3.ResumeLayout(false);
      this.gradientPanel3.PerformLayout();
      this.tabPagePersona.ResumeLayout(false);
      this.panelPersonaRight.ResumeLayout(false);
      this.gcPersonaOverwrite.ResumeLayout(false);
      this.panelPersonaLeft.ResumeLayout(false);
      this.gcPersonas.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
