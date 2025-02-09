// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ConventionalCountyLimitControl
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
using System.Net;
using System.Text;
using System.Web;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class ConventionalCountyLimitControl : SettingsUserControl
  {
    private Sessions.Session session;
    private XmlDocument XmlDoc;
    private bool urlUpdated;
    private IContainer components;
    private Label label1;
    private TextBox txtFindCounty;
    private Button btnSearch;
    private GridView lsvCountyLimit;
    private GroupContainer gcListLoanLimits;
    private ToolTip toolTip1;
    private FlowLayoutPanel flowLayoutPanel1;
    private Button btnSynchronize;
    private GroupContainer gcFHASite;
    private Panel radioButtonsPanel;
    private RadioButton rdbCustom;
    private TextBox txtSyncURL;

    public ConventionalCountyLimitControl(SetUpContainer setupContainer, Sessions.Session session)
      : this(setupContainer, session, false)
    {
    }

    public ConventionalCountyLimitControl(
      SetUpContainer setupContainer,
      Sessions.Session session,
      bool allowMultiSelect)
      : base(setupContainer)
    {
      this.session = session;
      this.InitializeComponent();
      this.initializeData();
      this.Reset();
      this.downloadFile();
      this.getLastSelectedCountyLimitsSelection();
      this.lsvCountyLimit.Sort(0, SortOrder.Ascending);
      this.lsvCountyLimit.AllowMultiselect = allowMultiSelect;
      this.rdbCustom.Tag = (object) new object[2]
      {
        (object) this.txtSyncURL.Text,
        (object) "custom"
      };
      this.txtSyncURL.Text = string.Concat(this.session.ServerManager.GetServerSetting("ConventionalCountyLimits.CountyLimitURL"));
      this.urlUpdated = false;
      this.setDirtyFlag(false);
    }

    private void initializeData()
    {
      this.populateCountyLimitList(this.session.ConfigurationManager.GetConventionalCountyLimits());
    }

    private DialogResult synchronize(object tagData, IProgressFeedback feedback)
    {
      try
      {
        feedback.Status = "Synchronizing...";
        feedback.MaxValue = 100;
        object[] objArray = (object[]) tagData;
        string url = (string) objArray[0];
        bool isCustomUrl = (string) objArray[1] == "custom";
        int year = -1;
        if (!isCustomUrl)
          year = Convert.ToInt32((string) objArray[1]);
        feedback.Increment(40);
        ConventionalCountyLimitUtil conventionalCountyLimitUtil = new ConventionalCountyLimitUtil();
        feedback.Status = "Downloading file...";
        ConventionalCountyLimit[] conventionalCountyLimits = conventionalCountyLimitUtil.GetConventionalCountyLimits(url, year, isCustomUrl);
        if (conventionalCountyLimits == null || conventionalCountyLimits.Length == 0)
          return DialogResult.Cancel;
        feedback.Increment(20);
        feedback.Status = "Inserting data into the database...";
        this.session.ConfigurationManager.ResetConventionalCountyLimits(conventionalCountyLimits);
        return DialogResult.OK;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
        return DialogResult.Abort;
      }
    }

    private void btnSynchronize_Click(object sender, EventArgs e)
    {
      string str1 = "";
      object[] state = (object[]) null;
      foreach (Control control in (ArrangedElementCollection) this.radioButtonsPanel.Controls)
      {
        if (control is RadioButton)
        {
          RadioButton radioButton = (RadioButton) control;
          if (radioButton.Checked)
          {
            string str2;
            if (radioButton.Name == "rdbCustom")
            {
              string str3 = this.txtSyncURL.Text.Trim();
              str2 = "custom";
              state = new object[2]
              {
                (object) str3,
                (object) str2
              };
            }
            else
            {
              state = (object[]) radioButton.Tag;
              str1 = (string) state[0];
              str2 = (string) state[1];
            }
            this.setLastSelectedCountyLimitsSelection("rdb" + str2);
            break;
          }
        }
      }
      if (state == null)
        return;
      switch (new ProgressDialog("Please wait while Conventional County Limits are updated", new AsynchronousProcess(this.synchronize), (object) state, false).ShowDialog((IWin32Window) this))
      {
        case DialogResult.OK:
          this.initializeData();
          break;
        case DialogResult.Cancel:
          int num1 = (int) Utils.Dialog((IWin32Window) this, "No county limit record can be imported successfully with the provided URL.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          break;
        default:
          int num2 = (int) Utils.Dialog((IWin32Window) this, "Exception occured while updating Conventional County Limit records.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          break;
      }
    }

    private void downloadFile()
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
      }
      string xml = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
      this.XmlDoc = new XmlDocument();
      this.XmlDoc.LoadXml(xml);
      XmlNodeList xmlNodeList = this.XmlDoc.SelectNodes("Root/ConventionalLimits/ConventionalLimit");
      int num1 = 0;
      foreach (XmlNode parentNode in xmlNodeList)
      {
        string str1 = this.readSingleChildNode(parentNode, "URL");
        string str2 = this.readSingleChildNode(parentNode, "OptionName");
        string str3 = this.readSingleChildNode(parentNode, "Year");
        RadioButton radioButton = new RadioButton();
        radioButton.Text = str2;
        radioButton.Tag = (object) new object[2]
        {
          (object) str1,
          (object) str3
        };
        radioButton.Location = new Point(10, num1 * 20);
        radioButton.Width = this.radioButtonsPanel.Width;
        this.radioButtonsPanel.Controls.Add((Control) radioButton);
        ++num1;
      }
    }

    private string readSingleChildNode(XmlNode parentNode, string childNodeRelativePath)
    {
      XmlNode xmlNode = parentNode.SelectSingleNode(childNodeRelativePath);
      return xmlNode != null ? xmlNode.InnerText : "";
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
          if (string.Compare(gvItem.SubItems[4].Text.Trim(), this.txtFindCounty.Text.Trim(), StringComparison.OrdinalIgnoreCase) == 0)
          {
            gvItem.Selected = true;
            this.txtFindCounty.Text = gvItem.SubItems[4].Text;
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

    private void populateCountyLimitList(ConventionalCountyLimit[] limitList)
    {
      this.lsvCountyLimit.Items.Clear();
      this.lsvCountyLimit.BeginUpdate();
      foreach (ConventionalCountyLimit limit in limitList)
        this.lsvCountyLimit.Items.Add(new GVItem(new List<string>()
        {
          string.Concat((object) limit.LimitYear),
          limit.FIPSStateCode,
          limit.StateCode,
          limit.FIPSCountyCode ?? "",
          limit.CountyName ?? "",
          limit.CBSANumber ?? "",
          CountyLimitEditDialog.ToDoubleString(string.Concat((object) limit.LimitFor1Unit)),
          CountyLimitEditDialog.ToDoubleString(string.Concat((object) limit.LimitFor2Units)),
          CountyLimitEditDialog.ToDoubleString(string.Concat((object) limit.LimitFor3Units)),
          CountyLimitEditDialog.ToDoubleString(string.Concat((object) limit.LimitFor4Units))
        }.ToArray())
        {
          Tag = (object) limit
        });
      this.lsvCountyLimit.EndUpdate();
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

    private void setLastSelectedCountyLimitsSelection(string val)
    {
      this.session.ConfigurationManager.SetCompanySetting("ConventionalCountyLimits", "Selection", val);
    }

    private void getLastSelectedCountyLimitsSelection()
    {
      string companySetting = this.session.ConfigurationManager.GetCompanySetting("ConventionalCountyLimits", "Selection");
      try
      {
        foreach (Control control in (ArrangedElementCollection) this.radioButtonsPanel.Controls)
        {
          if (control is RadioButton)
          {
            RadioButton radioButton = (RadioButton) control;
            if ("rdb" + (!(radioButton.Name == "rdbCustom") ? (string) ((object[]) radioButton.Tag)[1] : "custom") == companySetting)
            {
              radioButton.Checked = true;
              break;
            }
          }
        }
      }
      catch (Exception ex)
      {
      }
    }

    private void txtSyncURL_TextChanged(object sender, EventArgs e)
    {
      this.urlUpdated = true;
      this.setDirtyFlag(true);
    }

    public override void Save()
    {
      if (this.urlUpdated)
        this.session.ServerManager.UpdateServerSetting("ConventionalCountyLimits.CountyLimitURL", (object) this.txtSyncURL.Text.Trim());
      this.setDirtyFlag(false);
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
      this.label1 = new Label();
      this.txtFindCounty = new TextBox();
      this.btnSearch = new Button();
      this.lsvCountyLimit = new GridView();
      this.gcListLoanLimits = new GroupContainer();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.toolTip1 = new ToolTip(this.components);
      this.btnSynchronize = new Button();
      this.gcFHASite = new GroupContainer();
      this.radioButtonsPanel = new Panel();
      this.txtSyncURL = new TextBox();
      this.rdbCustom = new RadioButton();
      this.gcListLoanLimits.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      this.gcFHASite.SuspendLayout();
      this.radioButtonsPanel.SuspendLayout();
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
      this.lsvCountyLimit.AllowMultiselect = false;
      this.lsvCountyLimit.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Year";
      gvColumn1.Width = 50;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "FIPS State Code";
      gvColumn2.Width = 93;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "State";
      gvColumn3.Width = 36;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Text = "FIPS County Code";
      gvColumn4.Width = 102;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.Text = "County Name";
      gvColumn5.Width = 120;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column6";
      gvColumn6.Text = "CBSA Number";
      gvColumn6.Width = 85;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Column7";
      gvColumn7.Text = "One-Unit Limit";
      gvColumn7.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn7.Width = 83;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "Column8";
      gvColumn8.Text = "Two-Unit Limit";
      gvColumn8.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn8.Width = 92;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "Column9";
      gvColumn9.Text = "Three-Unit Limit";
      gvColumn9.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn9.Width = 90;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "Column10";
      gvColumn10.Text = "Four-Unit Limit";
      gvColumn10.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn10.Width = 89;
      this.lsvCountyLimit.Columns.AddRange(new GVColumn[10]
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
        gvColumn10
      });
      this.lsvCountyLimit.Dock = DockStyle.Fill;
      this.lsvCountyLimit.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.lsvCountyLimit.Location = new Point(1, 26);
      this.lsvCountyLimit.MinColumnWidth = 30;
      this.lsvCountyLimit.Name = "lsvCountyLimit";
      this.lsvCountyLimit.Size = new Size(877, 514);
      this.lsvCountyLimit.TabIndex = 10;
      this.gcListLoanLimits.AutoSize = true;
      this.gcListLoanLimits.Controls.Add((Control) this.flowLayoutPanel1);
      this.gcListLoanLimits.Controls.Add((Control) this.lsvCountyLimit);
      this.gcListLoanLimits.Dock = DockStyle.Fill;
      this.gcListLoanLimits.HeaderForeColor = SystemColors.ControlText;
      this.gcListLoanLimits.Location = new Point(0, 158);
      this.gcListLoanLimits.Name = "gcListLoanLimits";
      this.gcListLoanLimits.Size = new Size(879, 541);
      this.gcListLoanLimits.TabIndex = 26;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.label1);
      this.flowLayoutPanel1.Controls.Add((Control) this.txtFindCounty);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnSearch);
      this.flowLayoutPanel1.Location = new Point(3, 0);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(664, 26);
      this.flowLayoutPanel1.TabIndex = 26;
      this.btnSynchronize.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSynchronize.BackColor = SystemColors.Control;
      this.btnSynchronize.Location = new Point(787, 1);
      this.btnSynchronize.Name = "btnSynchronize";
      this.btnSynchronize.Size = new Size(87, 22);
      this.btnSynchronize.TabIndex = 2;
      this.btnSynchronize.Text = "Synchronize";
      this.btnSynchronize.UseVisualStyleBackColor = true;
      this.btnSynchronize.Click += new EventHandler(this.btnSynchronize_Click);
      this.gcFHASite.Borders = AnchorStyles.Left | AnchorStyles.Right;
      this.gcFHASite.Controls.Add((Control) this.radioButtonsPanel);
      this.gcFHASite.Controls.Add((Control) this.btnSynchronize);
      this.gcFHASite.Dock = DockStyle.Top;
      this.gcFHASite.HeaderForeColor = SystemColors.ControlText;
      this.gcFHASite.Location = new Point(0, 0);
      this.gcFHASite.Name = "gcFHASite";
      this.gcFHASite.Size = new Size(879, 158);
      this.gcFHASite.TabIndex = 24;
      this.gcFHASite.Text = "FHFA Web Site";
      this.radioButtonsPanel.AutoScroll = true;
      this.radioButtonsPanel.Controls.Add((Control) this.txtSyncURL);
      this.radioButtonsPanel.Controls.Add((Control) this.rdbCustom);
      this.radioButtonsPanel.Dock = DockStyle.Fill;
      this.radioButtonsPanel.Location = new Point(1, 25);
      this.radioButtonsPanel.Name = "radioButtonsPanel";
      this.radioButtonsPanel.Size = new Size(877, 133);
      this.radioButtonsPanel.TabIndex = 3;
      this.txtSyncURL.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtSyncURL.Location = new Point(29, 104);
      this.txtSyncURL.Name = "txtSyncURL";
      this.txtSyncURL.Size = new Size(819, 26);
      this.txtSyncURL.TabIndex = 11;
      this.txtSyncURL.TextChanged += new EventHandler(this.txtSyncURL_TextChanged);
      this.rdbCustom.AutoSize = true;
      this.rdbCustom.Location = new Point(10, 106);
      this.rdbCustom.Name = "rdbCustom";
      this.rdbCustom.Size = new Size(21, 20);
      this.rdbCustom.TabIndex = 10;
      this.rdbCustom.Tag = (object) "";
      this.rdbCustom.UseVisualStyleBackColor = true;
      this.AutoScaleMode = AutoScaleMode.Inherit;
      this.Controls.Add((Control) this.gcListLoanLimits);
      this.Controls.Add((Control) this.gcFHASite);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (ConventionalCountyLimitControl);
      this.Size = new Size(879, 699);
      this.gcListLoanLimits.ResumeLayout(false);
      this.flowLayoutPanel1.ResumeLayout(false);
      this.flowLayoutPanel1.PerformLayout();
      this.gcFHASite.ResumeLayout(false);
      this.radioButtonsPanel.ResumeLayout(false);
      this.radioButtonsPanel.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
