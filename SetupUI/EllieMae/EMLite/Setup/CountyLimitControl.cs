// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.CountyLimitControl
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
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class CountyLimitControl : SettingsUserControl
  {
    private bool urlUpdated;
    private bool enforceChanged;
    private Sessions.Session session;
    private XmlDocument XmlDoc;
    private IContainer components;
    private Label label1;
    private TextBox txtFindCounty;
    private Button btnSearch;
    private TextBox txtSyncURL;
    private Button btnSynchronize;
    private GridView lsvCountyLimit;
    private RadioButton rdbCustom;
    private CheckBox chbEnforceSetting;
    private GroupContainer gcFHASite;
    private GroupContainer gcEnforceLimits;
    private GroupContainer gcListFHALimits;
    private ToolTip toolTip1;
    private StandardIconButton stdIconBtnEdit;
    private FlowLayoutPanel flowLayoutPanel1;
    private Panel radioButtonsPanel;

    public CountyLimitControl(SetUpContainer setupContainer, Sessions.Session session)
      : this(setupContainer, session, false)
    {
    }

    public CountyLimitControl(
      SetUpContainer setupContainer,
      Sessions.Session session,
      bool allowMultiSelect)
      : base(setupContainer)
    {
      this.session = session;
      this.InitializeComponent();
      this.initializeData();
      this.Reset();
      this.downloadFileAndPopulateRadioButtons();
      this.lsvCountyLimit.Sort(0, SortOrder.Ascending);
      this.lsvCountyLimit.AllowMultiselect = allowMultiSelect;
      this.lsvCountyLimit_SelectedIndexChanged((object) this, (EventArgs) null);
      this.chbEnforceSetting.CheckedChanged += new EventHandler(this.chbEnforceSetting_CheckedChanged);
      this.getLastSelectedCountyLimitsSelection();
    }

    private void initializeData()
    {
      this.populateCountyLimitList(this.session.ConfigurationManager.GetCountyLimits());
    }

    private void getLastSelectedCountyLimitsSelection()
    {
      string companySetting = this.session.ConfigurationManager.GetCompanySetting("CountyLimits", "Selection");
      try
      {
        ((RadioButton) this.radioButtonsPanel.Controls[companySetting]).Checked = true;
      }
      catch (Exception ex)
      {
      }
    }

    private void downloadFileAndPopulateRadioButtons()
    {
      byte[] bytes = (byte[]) null;
      try
      {
        string str = this.session?.StartupInfo?.ServiceUrls?.DownloadServiceUrl;
        if (string.IsNullOrWhiteSpace(str) || !Uri.IsWellFormedUriString(str, UriKind.Absolute))
          str = "https://encompass.elliemae.com/download/download.asp";
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("ClientID=" + HttpUtility.UrlEncode(this.session.CompanyInfo.ClientID));
        stringBuilder.Append("&UserID=" + HttpUtility.UrlEncode(this.session.UserInfo.Userid));
        stringBuilder.Append("&FileID=" + HttpUtility.UrlEncode("CountyLimitOptions.xml"));
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
              bytes = memoryStream.ToArray();
            }
          }
        }
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "The following error occurred when trying to download the file:\n\n" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return;
      }
      string xml = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
      this.XmlDoc = new XmlDocument();
      this.XmlDoc.LoadXml(xml);
      XmlNodeList xmlNodeList = this.XmlDoc.SelectNodes("Root/FHALimits/FHALimit");
      int num1 = 0;
      foreach (XmlNode parentNode in xmlNodeList)
      {
        string str1 = this.readSingleChildNode(parentNode, "OptionName");
        string str2 = this.readSingleChildNode(parentNode, "URL");
        string str3 = this.readSingleChildNode(parentNode, "Year");
        RadioButton radioButton = new RadioButton();
        radioButton.Text = str1;
        radioButton.Tag = (object) new object[2]
        {
          (object) str2,
          (object) str3
        };
        radioButton.Location = new Point(10, num1 * 20);
        radioButton.Width = this.radioButtonsPanel.Width;
        radioButton.Name = "rdbJan" + str3;
        this.radioButtonsPanel.Controls.Add((Control) radioButton);
        ++num1;
      }
    }

    private string readSingleChildNode(XmlNode parentNode, string childNodeRelativePath)
    {
      XmlNode xmlNode = parentNode.SelectSingleNode(childNodeRelativePath);
      return xmlNode != null ? xmlNode.InnerText : "";
    }

    private void setLastSelectedCountyLimitsSelection()
    {
      foreach (Control control in (ArrangedElementCollection) this.radioButtonsPanel.Controls)
      {
        if (control is RadioButton)
        {
          RadioButton radioButton = (RadioButton) control;
          if (radioButton != null && radioButton.Checked)
          {
            this.session.ConfigurationManager.SetCompanySetting("CountyLimits", "Selection", radioButton.Name);
            break;
          }
        }
      }
    }

    private void mySetDirtyFlag(CountyLimitControl.SettingEnum setting, bool val)
    {
      switch (setting)
      {
        case CountyLimitControl.SettingEnum.EnforceLimits:
          this.enforceChanged = val;
          if (this.enforceChanged)
          {
            this.setDirtyFlag(true);
            break;
          }
          if (this.urlUpdated)
            break;
          this.setDirtyFlag(false);
          break;
        case CountyLimitControl.SettingEnum.Url:
          this.urlUpdated = val;
          if (this.urlUpdated)
          {
            this.setDirtyFlag(true);
            break;
          }
          if (this.enforceChanged)
            break;
          this.setDirtyFlag(false);
          break;
        case CountyLimitControl.SettingEnum.Both:
          this.enforceChanged = this.urlUpdated = val;
          this.setDirtyFlag(val);
          break;
      }
    }

    public override void Save()
    {
      if (this.enforceChanged)
        this.session.ServerManager.UpdateServerSetting("Policies.EnforceCountyLimit", (object) this.chbEnforceSetting.Checked);
      if (this.urlUpdated)
        this.session.ServerManager.UpdateServerSetting("Policies.CountyLimitURL", (object) this.txtSyncURL.Text.Trim());
      this.mySetDirtyFlag(CountyLimitControl.SettingEnum.Both, false);
    }

    public override void Reset()
    {
      this.chbEnforceSetting.Checked = (bool) this.session.ServerManager.GetServerSetting("Policies.EnforceCountyLimit");
      this.txtSyncURL.Text = string.Concat(this.session.ServerManager.GetServerSetting("Policies.CountyLimitURL"));
      this.mySetDirtyFlag(CountyLimitControl.SettingEnum.Both, false);
    }

    private void btnSynchronize_Click(object sender, EventArgs e)
    {
      this.setLastSelectedCountyLimitsSelection();
      string state = "";
      foreach (Control control in (ArrangedElementCollection) this.radioButtonsPanel.Controls)
      {
        if (control is RadioButton)
        {
          RadioButton radioButton = (RadioButton) control;
          if (radioButton.Checked)
          {
            state = !(radioButton.Name == "rdbCustom") ? (string) ((object[]) radioButton.Tag)[0] : this.txtSyncURL.Text.Trim();
            break;
          }
        }
      }
      if (state == string.Empty)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please provide a URL for synchronization.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        switch (new ProgressDialog("Please wait while updating FHA County Limits", new AsynchronousProcess(this.synchronize), (object) state, false).ShowDialog((IWin32Window) this))
        {
          case DialogResult.OK:
            this.initializeData();
            break;
          case DialogResult.Cancel:
            int num2 = (int) Utils.Dialog((IWin32Window) this, "No county limit record can be imported successfully with the provided URL.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            break;
          default:
            int num3 = (int) Utils.Dialog((IWin32Window) this, "Exception occured while updating County Limit records.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            break;
        }
      }
    }

    private DialogResult synchronize(object url, IProgressFeedback feedback)
    {
      try
      {
        feedback.Status = "synchronizing...";
        feedback.MaxValue = 100;
        string url1 = (string) url;
        List<CountyLimit> countyLimitList = new List<CountyLimit>();
        Dictionary<CountyLimit, CountyLimit> conflicts = new Dictionary<CountyLimit, CountyLimit>();
        CountyLimit[] countyLimitFromUrl = CountyLimitUtil.GetCountyLimitFromUrl(url1);
        feedback.Increment(15);
        CountyLimit[] countyLimits = this.session.ConfigurationManager.GetCountyLimits();
        feedback.Increment(15);
        foreach (CountyLimit countyLimit1 in countyLimitFromUrl)
        {
          bool flag = false;
          int index;
          for (index = 0; index < countyLimits.Length; ++index)
          {
            CountyLimit countyLimit2 = countyLimits[index];
            if (countyLimit2.Customized && countyLimit2.StateAbbreviation == countyLimit1.StateAbbreviation && countyLimit2.CountyName == countyLimit1.CountyName)
            {
              flag = true;
              break;
            }
          }
          if (!flag)
            countyLimitList.Add(countyLimit1);
          else
            conflicts.Add(countyLimits[index], countyLimit1);
        }
        feedback.Increment(10);
        if (conflicts.Count > 0)
        {
          feedback.Status = "resolving conflicts...";
          CountyLimitConflictDialog limitConflictDialog = new CountyLimitConflictDialog(conflicts);
          int num = (int) limitConflictDialog.ShowDialog();
          countyLimitList.AddRange((IEnumerable<CountyLimit>) limitConflictDialog.SelectedCountyLimits);
        }
        feedback.Increment(25);
        if (countyLimitList == null || countyLimitList.Count == 0)
          return DialogResult.Cancel;
        this.session.ConfigurationManager.ResetCountyLimits(countyLimitList.ToArray());
        return DialogResult.OK;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
        return DialogResult.Abort;
      }
    }

    private void btnSearch_Click(object sender, EventArgs e)
    {
      if (this.txtFindCounty.Text.Trim() == string.Empty)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please provide a county name", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        int nItemIndex = 0;
        int num2 = 0;
        if (this.lsvCountyLimit.SelectedItems != null && this.lsvCountyLimit.SelectedItems.Count > 0)
        {
          nItemIndex = this.lsvCountyLimit.SelectedItems[0].Index + 1;
          if (nItemIndex == this.lsvCountyLimit.Items.Count)
            nItemIndex = 0;
          num2 = nItemIndex;
          this.lsvCountyLimit.SelectedItems[0].Selected = false;
        }
        bool flag = false;
        while (nItemIndex < this.lsvCountyLimit.Items.Count)
        {
          GVItem gvItem = this.lsvCountyLimit.Items[nItemIndex];
          if (string.Compare(gvItem.SubItems[2].Text.Trim(), this.txtFindCounty.Text.Trim(), StringComparison.OrdinalIgnoreCase) == 0)
          {
            gvItem.Selected = true;
            this.txtFindCounty.Text = gvItem.SubItems[2].Text;
            this.lsvCountyLimit.EnsureVisible(gvItem.Index);
            flag = true;
            break;
          }
          ++nItemIndex;
          if (nItemIndex == this.lsvCountyLimit.Items.Count)
            nItemIndex = 0;
          if (nItemIndex == num2)
            break;
        }
        if (flag)
          return;
        int num3 = (int) Utils.Dialog((IWin32Window) this, "Invalid County name. Please enter a valid County name and try again.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private void updateURL()
    {
      if (!this.urlUpdated)
        return;
      this.session.ServerManager.UpdateServerSetting("Policies.CountyLimitURL", (object) this.txtSyncURL.Text);
    }

    private void populateCountyLimitList(CountyLimit[] limitList)
    {
      this.lsvCountyLimit.Items.Clear();
      this.lsvCountyLimit.BeginUpdate();
      foreach (CountyLimit limit in limitList)
      {
        List<string> stringList = new List<string>();
        stringList.Add(limit.SoaCode);
        stringList.Add(limit.StateAbbreviation);
        stringList.Add(limit.CountyName);
        stringList.Add(string.Concat((object) limit.CountyCode));
        stringList.Add(string.Concat((object) limit.MsaCode));
        stringList.Add(CountyLimitEditDialog.ToDoubleString(string.Concat((object) limit.LimitFor1Unit)));
        stringList.Add(CountyLimitEditDialog.ToDoubleString(string.Concat((object) limit.LimitFor2Units)));
        stringList.Add(CountyLimitEditDialog.ToDoubleString(string.Concat((object) limit.LimitFor3Units)));
        stringList.Add(CountyLimitEditDialog.ToDoubleString(string.Concat((object) limit.LimitFor4Units)));
        stringList.Add(this.formatDate(limit.CountyTransactionDate));
        stringList.Add(this.formatDate(limit.LimitTransactionDate));
        stringList.Add(limit.LastModifiedDateTime.ToString("g"));
        if (limit.Customized)
          stringList.Add("True");
        else
          stringList.Add("False");
        this.lsvCountyLimit.Items.Add(new GVItem(stringList.ToArray())
        {
          Tag = (object) limit
        });
      }
      this.lsvCountyLimit.EndUpdate();
    }

    private void lsvCountyLimit_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.lsvCountyLimit.SelectedItems == null || this.lsvCountyLimit.SelectedItems.Count == 0)
      {
        this.stdIconBtnEdit.Enabled = false;
      }
      else
      {
        this.stdIconBtnEdit.Enabled = true;
        this.txtFindCounty.Text = this.lsvCountyLimit.SelectedItems[0].SubItems[2].Text;
      }
    }

    private string formatDate(string strValue)
    {
      if (strValue.Length < 8)
        return "";
      string str = strValue.Substring(0, 4);
      return strValue.Substring(4, 2) + "/" + strValue.Substring(6, 2) + "/" + str;
    }

    private void txtFindCounty_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\r')
        return;
      this.btnSearch_Click((object) null, (EventArgs) null);
    }

    private void stdIconBtnSave_Click(object sender, EventArgs e) => this.Save();

    private void stdIconBtnReset_Click(object sender, EventArgs e)
    {
      if (ResetConfirmDialog.ShowDialog((IWin32Window) this.setupContainer, (string) null) == DialogResult.No)
        return;
      this.Reset();
    }

    private void stdIconBtnEdit_Click(object sender, EventArgs e) => this.editSelected();

    private void editSelected()
    {
      if (this.lsvCountyLimit.SelectedItems == null || this.lsvCountyLimit.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a record to update.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        CountyLimitEditDialog countyLimitEditDialog = new CountyLimitEditDialog(this.lsvCountyLimit.SelectedItems[0].SubItems[5].Text, this.lsvCountyLimit.SelectedItems[0].SubItems[6].Text, this.lsvCountyLimit.SelectedItems[0].SubItems[7].Text, this.lsvCountyLimit.SelectedItems[0].SubItems[8].Text);
        if (countyLimitEditDialog.ShowDialog((IWin32Window) this.setupContainer) == DialogResult.Cancel)
          return;
        CountyLimit tag = (CountyLimit) this.lsvCountyLimit.SelectedItems[0].Tag;
        tag.LimitFor1Unit = countyLimitEditDialog.Limit1;
        tag.LimitFor2Units = countyLimitEditDialog.Limit2;
        tag.LimitFor3Units = countyLimitEditDialog.Limit3;
        tag.LimitFor4Units = countyLimitEditDialog.Limit4;
        tag.LastModifiedDateTime = DateTime.Now;
        tag.Customized = true;
        this.session.ConfigurationManager.UpdateCountyLimits(new CountyLimit[1]
        {
          tag
        });
        this.lsvCountyLimit.BeginUpdate();
        this.lsvCountyLimit.SelectedItems[0].SubItems[5].Text = countyLimitEditDialog.Limit1Text;
        this.lsvCountyLimit.SelectedItems[0].SubItems[6].Text = countyLimitEditDialog.Limit2Text;
        this.lsvCountyLimit.SelectedItems[0].SubItems[7].Text = countyLimitEditDialog.Limit3Text;
        this.lsvCountyLimit.SelectedItems[0].SubItems[8].Text = countyLimitEditDialog.Limit4Text;
        this.lsvCountyLimit.SelectedItems[0].SubItems[11].Text = tag.LastModifiedDateTime.ToString("g");
        this.lsvCountyLimit.SelectedItems[0].SubItems[12].Text = !tag.Customized ? "False" : "True";
        this.lsvCountyLimit.EndUpdate();
      }
    }

    private void txtSyncURL_TextChanged(object sender, EventArgs e)
    {
      this.mySetDirtyFlag(CountyLimitControl.SettingEnum.Url, true);
    }

    private void chbEnforceSetting_CheckedChanged(object sender, EventArgs e)
    {
      this.mySetDirtyFlag(CountyLimitControl.SettingEnum.EnforceLimits, true);
    }

    private void lsvCountyLimit_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      this.editSelected();
    }

    public List<string> SelectedCountyLimit
    {
      get
      {
        List<string> selectedCountyLimit = new List<string>();
        IEnumerable<string> collection = this.lsvCountyLimit.SelectedItems.Where<GVItem>((Func<GVItem, bool>) (gvItem => gvItem.SubItems[12].Text.Equals("True"))).Select<GVItem, string>((Func<GVItem, string>) (gvItem => string.Format("{0}_{1}_{2}", (object) gvItem.SubItems[0], (object) gvItem.SubItems[1], (object) gvItem.SubItems[2])));
        selectedCountyLimit.AddRange(collection);
        return selectedCountyLimit;
      }
    }

    public void SetSelectedCountyLimit(List<string> selectedCountyLimits)
    {
      foreach (GVItem gvItem in this.lsvCountyLimit.Items.Where<GVItem>((Func<GVItem, bool>) (gvItem => selectedCountyLimits.Contains(string.Format("{0}_{1}_{2}", (object) gvItem.SubItems[0], (object) gvItem.SubItems[1], (object) gvItem.SubItems[2])))))
        gvItem.Selected = true;
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
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      GVColumn gvColumn7 = new GVColumn();
      GVColumn gvColumn8 = new GVColumn();
      GVColumn gvColumn9 = new GVColumn();
      GVColumn gvColumn10 = new GVColumn();
      GVColumn gvColumn11 = new GVColumn();
      GVColumn gvColumn12 = new GVColumn();
      GVColumn gvColumn13 = new GVColumn();
      this.label1 = new Label();
      this.txtFindCounty = new TextBox();
      this.btnSearch = new Button();
      this.txtSyncURL = new TextBox();
      this.btnSynchronize = new Button();
      this.lsvCountyLimit = new GridView();
      this.rdbCustom = new RadioButton();
      this.chbEnforceSetting = new CheckBox();
      this.gcFHASite = new GroupContainer();
      this.radioButtonsPanel = new Panel();
      this.gcEnforceLimits = new GroupContainer();
      this.gcListFHALimits = new GroupContainer();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.stdIconBtnEdit = new StandardIconButton();
      this.toolTip1 = new ToolTip(this.components);
      this.gcFHASite.SuspendLayout();
      this.radioButtonsPanel.SuspendLayout();
      this.gcEnforceLimits.SuspendLayout();
      this.gcListFHALimits.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      ((ISupportInitialize) this.stdIconBtnEdit).BeginInit();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Location = new Point(3, 5);
      this.label1.Margin = new Padding(3, 5, 3, 0);
      this.label1.Name = "label1";
      this.label1.Size = new Size(98, 19);
      this.label1.TabIndex = 1;
      this.label1.Text = "Find County";
      this.txtFindCounty.Location = new Point(107, 3);
      this.txtFindCounty.Name = "txtFindCounty";
      this.txtFindCounty.Size = new Size(300, 26);
      this.txtFindCounty.TabIndex = 2;
      this.txtFindCounty.KeyPress += new KeyPressEventHandler(this.txtFindCounty_KeyPress);
      this.btnSearch.BackColor = SystemColors.Control;
      this.btnSearch.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnSearch.Location = new Point(413, 3);
      this.btnSearch.Name = "btnSearch";
      this.btnSearch.Size = new Size(71, 22);
      this.btnSearch.TabIndex = 3;
      this.btnSearch.Text = "Find";
      this.btnSearch.UseVisualStyleBackColor = true;
      this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
      this.txtSyncURL.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtSyncURL.Location = new Point(28, 105);
      this.txtSyncURL.Name = "txtSyncURL";
      this.txtSyncURL.Size = new Size(702, 26);
      this.txtSyncURL.TabIndex = 10;
      this.txtSyncURL.TextChanged += new EventHandler(this.txtSyncURL_TextChanged);
      this.btnSynchronize.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSynchronize.BackColor = SystemColors.Control;
      this.btnSynchronize.Location = new Point(648, 1);
      this.btnSynchronize.Name = "btnSynchronize";
      this.btnSynchronize.Size = new Size(87, 22);
      this.btnSynchronize.TabIndex = 2;
      this.btnSynchronize.Text = "Synchronize";
      this.btnSynchronize.UseVisualStyleBackColor = true;
      this.btnSynchronize.Click += new EventHandler(this.btnSynchronize_Click);
      this.lsvCountyLimit.AllowMultiselect = false;
      this.lsvCountyLimit.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "SOA Code";
      gvColumn1.Width = 68;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "State";
      gvColumn2.Width = 41;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "County Name";
      gvColumn3.Width = 80;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Text = "County Code";
      gvColumn4.Width = 77;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.Text = "MSA Code";
      gvColumn5.Width = 69;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column6";
      gvColumn6.Text = "Limit for 1 Unit";
      gvColumn6.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn6.Width = 83;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Column7";
      gvColumn7.Text = "Limit for 2 Units";
      gvColumn7.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn7.Width = 92;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "Column8";
      gvColumn8.Text = "Limit for 3 Units";
      gvColumn8.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn8.Width = 90;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "Column9";
      gvColumn9.Text = "Limit for 4 Units";
      gvColumn9.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn9.Width = 89;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "Column10";
      gvColumn10.Text = "County Transaction Date";
      gvColumn10.Width = 61;
      gvColumn11.ImageIndex = -1;
      gvColumn11.Name = "Column11";
      gvColumn11.Text = "Limit Transaction Date";
      gvColumn11.Width = 100;
      gvColumn12.ImageIndex = -1;
      gvColumn12.Name = "Column12";
      gvColumn12.Text = "Last Modified";
      gvColumn12.Width = 100;
      gvColumn13.ImageIndex = -1;
      gvColumn13.Name = "Column13";
      gvColumn13.Text = "Customized";
      gvColumn13.Width = 100;
      this.lsvCountyLimit.Columns.AddRange(new GVColumn[13]
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
        gvColumn11,
        gvColumn12,
        gvColumn13
      });
      this.lsvCountyLimit.Dock = DockStyle.Fill;
      this.lsvCountyLimit.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.lsvCountyLimit.Location = new Point(1, 26);
      this.lsvCountyLimit.Name = "lsvCountyLimit";
      this.lsvCountyLimit.Size = new Size(738, 350);
      this.lsvCountyLimit.TabIndex = 10;
      this.lsvCountyLimit.SelectedIndexChanged += new EventHandler(this.lsvCountyLimit_SelectedIndexChanged);
      this.lsvCountyLimit.ItemDoubleClick += new GVItemEventHandler(this.lsvCountyLimit_ItemDoubleClick);
      this.rdbCustom.AutoSize = true;
      this.rdbCustom.Location = new Point(10, 109);
      this.rdbCustom.Name = "rdbCustom";
      this.rdbCustom.Size = new Size(21, 20);
      this.rdbCustom.TabIndex = 9;
      this.rdbCustom.UseVisualStyleBackColor = true;
      this.chbEnforceSetting.AutoSize = true;
      this.chbEnforceSetting.Location = new Point(10, 33);
      this.chbEnforceSetting.Name = "chbEnforceSetting";
      this.chbEnforceSetting.Size = new Size(569, 23);
      this.chbEnforceSetting.TabIndex = 1;
      this.chbEnforceSetting.Text = "Do not allow loan amount higher than maximum county limit to be entered.";
      this.chbEnforceSetting.UseVisualStyleBackColor = true;
      this.chbEnforceSetting.CheckedChanged += new EventHandler(this.chbEnforceSetting_CheckedChanged);
      this.gcFHASite.Borders = AnchorStyles.Left | AnchorStyles.Right;
      this.gcFHASite.Controls.Add((Control) this.radioButtonsPanel);
      this.gcFHASite.Controls.Add((Control) this.btnSynchronize);
      this.gcFHASite.Dock = DockStyle.Top;
      this.gcFHASite.HeaderForeColor = SystemColors.ControlText;
      this.gcFHASite.Location = new Point(0, 59);
      this.gcFHASite.Name = "gcFHASite";
      this.gcFHASite.Size = new Size(740, 159);
      this.gcFHASite.TabIndex = 24;
      this.gcFHASite.Text = "HUD Web Site";
      this.radioButtonsPanel.Controls.Add((Control) this.txtSyncURL);
      this.radioButtonsPanel.Controls.Add((Control) this.rdbCustom);
      this.radioButtonsPanel.Dock = DockStyle.Fill;
      this.radioButtonsPanel.Location = new Point(1, 25);
      this.radioButtonsPanel.Name = "radioButtonsPanel";
      this.radioButtonsPanel.Size = new Size(738, 134);
      this.radioButtonsPanel.TabIndex = 11;
      this.gcEnforceLimits.Controls.Add((Control) this.chbEnforceSetting);
      this.gcEnforceLimits.Dock = DockStyle.Top;
      this.gcEnforceLimits.HeaderForeColor = SystemColors.ControlText;
      this.gcEnforceLimits.Location = new Point(0, 0);
      this.gcEnforceLimits.Name = "gcEnforceLimits";
      this.gcEnforceLimits.Size = new Size(740, 59);
      this.gcEnforceLimits.TabIndex = 25;
      this.gcEnforceLimits.Text = "Enforce Loan Limits";
      this.gcListFHALimits.Controls.Add((Control) this.flowLayoutPanel1);
      this.gcListFHALimits.Controls.Add((Control) this.stdIconBtnEdit);
      this.gcListFHALimits.Controls.Add((Control) this.lsvCountyLimit);
      this.gcListFHALimits.Dock = DockStyle.Fill;
      this.gcListFHALimits.HeaderForeColor = SystemColors.ControlText;
      this.gcListFHALimits.Location = new Point(0, 218);
      this.gcListFHALimits.Name = "gcListFHALimits";
      this.gcListFHALimits.Size = new Size(740, 377);
      this.gcListFHALimits.TabIndex = 26;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.label1);
      this.flowLayoutPanel1.Controls.Add((Control) this.txtFindCounty);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnSearch);
      this.flowLayoutPanel1.Location = new Point(3, 0);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(664, 29);
      this.flowLayoutPanel1.TabIndex = 26;
      this.stdIconBtnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnEdit.BackColor = Color.Transparent;
      this.stdIconBtnEdit.Location = new Point(717, 5);
      this.stdIconBtnEdit.MouseDownImage = (Image) null;
      this.stdIconBtnEdit.Name = "stdIconBtnEdit";
      this.stdIconBtnEdit.Size = new Size(16, 16);
      this.stdIconBtnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdIconBtnEdit.TabIndex = 25;
      this.stdIconBtnEdit.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnEdit, "Edit");
      this.stdIconBtnEdit.Click += new EventHandler(this.stdIconBtnEdit_Click);
      this.AutoScaleMode = AutoScaleMode.Inherit;
      this.Controls.Add((Control) this.gcListFHALimits);
      this.Controls.Add((Control) this.gcFHASite);
      this.Controls.Add((Control) this.gcEnforceLimits);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (CountyLimitControl);
      this.Size = new Size(740, 595);
      this.gcFHASite.ResumeLayout(false);
      this.radioButtonsPanel.ResumeLayout(false);
      this.radioButtonsPanel.PerformLayout();
      this.gcEnforceLimits.ResumeLayout(false);
      this.gcEnforceLimits.PerformLayout();
      this.gcListFHALimits.ResumeLayout(false);
      this.flowLayoutPanel1.ResumeLayout(false);
      this.flowLayoutPanel1.PerformLayout();
      ((ISupportInitialize) this.stdIconBtnEdit).EndInit();
      this.ResumeLayout(false);
    }

    private enum SettingEnum
    {
      EnforceLimits,
      Url,
      Both,
    }
  }
}
