// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AreaMedianIncomeLimitsControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.ContactUI.Import;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class AreaMedianIncomeLimitsControl : SettingsUserControl
  {
    private Sessions.Session session;
    private readonly string sw = Tracing.SwCommon;
    private readonly MFILimit[] mfiData;
    private IContainer components;
    private GroupContainer groupContainer2;
    private Button downloadBtn;
    private GridView gridView1;
    private Label timestampLabel;
    private TabControl tabControl1;
    private TabPage tabPage1;
    private TabPage tabPage3;
    private GroupContainer groupContainer1;
    private Label timestampLabelMfi;
    private GridView gridView2;
    private Button button1;

    public AreaMedianIncomeLimitsControl(SetUpContainer setupContainer, Sessions.Session session)
      : this(setupContainer, session, false)
    {
    }

    public AreaMedianIncomeLimitsControl(
      SetUpContainer setupContainer,
      Sessions.Session session,
      bool allowMultiSelect)
      : base(setupContainer)
    {
      this.session = session;
      this.InitializeComponent();
      this.setLastDownloadTime();
      this.populateAMIData();
      try
      {
        this.mfiData = this.session.ConfigurationManager.GetMFILimits();
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Exception occured while getting MFI Limit records." + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return;
      }
      this.populateMFIData(this.mfiData);
      this.setMfiLastDownloadTime();
    }

    private void populateAMIData()
    {
      AMILimit[] amiLimits = this.session.ConfigurationManager.GetAMILimits();
      this.gridView1.Items.Clear();
      this.gridView1.BeginUpdate();
      foreach (AMILimit amiLimit in amiLimits)
        this.gridView1.Items.Add(new GVItem(new List<string>()
        {
          amiLimit.LimitYear.ToString(),
          amiLimit.FIPSCode,
          amiLimit.StateName,
          amiLimit.CountyName,
          amiLimit.AmiLimit100,
          amiLimit.AmiLimit80,
          amiLimit.AmiLimit50
        }.ToArray()));
      this.gridView1.EndUpdate();
    }

    private void downloadBtn_Click(object sender, EventArgs e)
    {
      string str = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");
      this.session.ConfigurationManager.SetCompanySetting("AMITABLE", "DOWNLOADTIME", str);
      this.timestampLabel.Text = str;
      try
      {
        switch (new ProgressDialog("Please wait while AMI (Area Median Income) Limits are updated", new AsynchronousProcess(this.synchronize), (object) null, false).ShowDialog((IWin32Window) this))
        {
          case DialogResult.OK:
            this.populateAMIData();
            break;
          case DialogResult.Cancel:
            int num1 = (int) Utils.Dialog((IWin32Window) this, "No AMI Limit record can be imported successfully.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            break;
          default:
            int num2 = (int) Utils.Dialog((IWin32Window) this, "Exception occured while updating AMI Limit records.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            break;
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(this.sw, TraceLevel.Error, nameof (AreaMedianIncomeLimitsControl), "Exception occured while updating AMI Limit records.");
      }
    }

    private DialogResult synchronize(object data, IProgressFeedback feedback)
    {
      try
      {
        feedback.Status = "downloading...";
        string str = this.session?.StartupInfo?.ServiceUrls?.DownloadServiceUrl;
        if (string.IsNullOrWhiteSpace(str) || !Uri.IsWellFormedUriString(str, UriKind.Absolute))
          str = "https://encompass.elliemae.com/download/download.asp";
        List<AMILimit> amiLimitList = new List<AMILimit>();
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("ClientID=" + HttpUtility.UrlEncode(this.session.CompanyInfo.ClientID));
        stringBuilder.Append("&UserID=" + HttpUtility.UrlEncode(this.session.UserInfo.Userid));
        stringBuilder.Append("&FileID=" + HttpUtility.UrlEncode("AMILimits.csv"));
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
            TextFieldParser textFieldParser = new TextFieldParser(responseStream);
            textFieldParser.HasFieldsEnclosedInQuotes = true;
            textFieldParser.SetDelimiters(",");
            feedback.MaxValue = 6500;
            while (!textFieldParser.EndOfData)
            {
              string[] strArray = textFieldParser.ReadFields();
              AMILimit amiLimit = new AMILimit();
              feedback.Increment(1);
              for (int index = 0; index < strArray.Length; ++index)
              {
                string s = strArray[index];
                switch (index)
                {
                  case 0:
                    amiLimit.LimitYear = (int) short.Parse(s);
                    break;
                  case 1:
                    amiLimit.FIPSCode = s;
                    break;
                  case 2:
                    amiLimit.StateName = s;
                    break;
                  case 3:
                    amiLimit.CountyName = s;
                    break;
                  case 4:
                    amiLimit.AmiLimit100 = s;
                    break;
                  case 5:
                    amiLimit.AmiLimit80 = s;
                    break;
                  case 6:
                    amiLimit.AmiLimit50 = s;
                    break;
                }
              }
              amiLimit.LastModifiedDateTime = DateTime.Now;
              amiLimitList.Add(amiLimit);
            }
          }
        }
        if (amiLimitList == null || amiLimitList.Count == 0)
          return DialogResult.Cancel;
        this.session.ConfigurationManager.ResetAMILimits(amiLimitList.ToArray());
        this.populateAMIData();
        return DialogResult.OK;
      }
      catch (Exception ex)
      {
        return DialogResult.Abort;
      }
    }

    private void setLastDownloadTime()
    {
      string companySetting = this.session.ConfigurationManager.GetCompanySetting("AMITABLE", "DOWNLOADTIME");
      if (string.IsNullOrEmpty(companySetting))
        return;
      this.timestampLabel.Text = companySetting;
    }

    private void populateMFIData(MFILimit[] mfiData)
    {
      this.gridView2.Items.Clear();
      this.gridView2.BeginUpdate();
      foreach (MFILimit mfiLimit in mfiData)
      {
        List<string> stringList1 = new List<string>();
        List<string> stringList2 = stringList1;
        int num = mfiLimit.SourceFileYear;
        string str1 = num.ToString();
        stringList2.Add(str1);
        stringList1.Add(mfiLimit.MSAMDCode);
        stringList1.Add(mfiLimit.MSAMDName);
        List<string> stringList3 = stringList1;
        num = mfiLimit.ActualMFIYear;
        string str2 = num.ToString();
        stringList3.Add(str2);
        stringList1.Add(mfiLimit.ActualMFIAmount);
        List<string> stringList4 = stringList1;
        num = mfiLimit.EstimatedMFIYear;
        string str3 = num.ToString();
        stringList4.Add(str3);
        stringList1.Add(mfiLimit.EstimatedMFIAmount);
        this.gridView2.Items.Add(new GVItem(stringList1.ToArray()));
      }
      this.gridView2.EndUpdate();
    }

    private void downloadMfiBtn_Click(object sender, EventArgs e)
    {
      try
      {
        switch (new ProgressDialog("Please wait while MFI (Median Family Income) Limits are updated", new AsynchronousProcess(this.synchronizeMfi), (object) null, false).ShowDialog((IWin32Window) this))
        {
          case DialogResult.OK:
            int num1 = (int) MessageBox.Show("All the MFI Limit Records were updated sucessfully!");
            break;
          case DialogResult.Cancel:
            int num2 = (int) Utils.Dialog((IWin32Window) this, "No MFI Limit record can be imported successfully.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            break;
          default:
            int num3 = (int) Utils.Dialog((IWin32Window) this, "Exception occured while updating MFI Limit records.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            break;
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(this.sw, TraceLevel.Error, "MedianFamilyIncomeLimitsControl", "Exception occured while updating MFI Limit records.");
        int num = (int) Utils.Dialog((IWin32Window) this, "Exception occured while updating MFI Limit records.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return;
      }
      string str = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");
      try
      {
        this.session.ConfigurationManager.SetCompanySetting("MFITABLE", "DOWNLOADTIME", Utils.DateTimeToUTCString(DateTime.Now));
      }
      catch
      {
        Tracing.Log(this.sw, TraceLevel.Error, "MedianFamilyIncomeLimitsControl", "Exception occured while updating MFI Limit records.");
        int num = (int) Utils.Dialog((IWin32Window) this, "Exception occured while updating DOWNLOADTIME in MFITABLE settings.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return;
      }
      this.timestampLabelMfi.Text = str;
    }

    private DialogResult synchronizeMfi(object data, IProgressFeedback feedback)
    {
      List<MFILimit> source = new List<MFILimit>();
      try
      {
        feedback.Status = "Downloading...";
        string str = this.session?.StartupInfo?.ServiceUrls?.DownloadServiceUrl;
        if (string.IsNullOrWhiteSpace(str) || !Uri.IsWellFormedUriString(str, UriKind.Absolute))
          str = "https://encompass.elliemae.com/download/download.asp";
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("ClientID=" + HttpUtility.UrlEncode(this.session.CompanyInfo.ClientID));
        stringBuilder.Append("&UserID=" + HttpUtility.UrlEncode(this.session.UserInfo.Userid));
        stringBuilder.Append("&FileID=" + HttpUtility.UrlEncode("MedianFamilyIncome.csv"));
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
            string[][] strArray = (string[][]) null;
            using (CsvParser csvParser = new CsvParser((TextReader) new StringReader(new StreamReader(responseStream, Encoding.Default).ReadToEnd()), true))
              strArray = csvParser.RemainingRows();
            for (int index = 0; index < strArray.Length; ++index)
              source.Add(new MFILimit()
              {
                SourceFileYear = (int) short.Parse(strArray[index][0]),
                MSAMDCode = strArray[index][1],
                MSAMDName = strArray[index][2],
                ActualMFIYear = (int) short.Parse(strArray[index][3]),
                ActualMFIAmount = strArray[index][4],
                EstimatedMFIYear = (int) short.Parse(strArray[index][5]),
                EstimatedMFIAmount = strArray[index][6],
                LastModifiedDateTime = DateTime.Now
              });
          }
        }
        if (source == null || source.Count == 0)
          return DialogResult.Cancel;
        this.session.ConfigurationManager.ResetMFILimits(source.ToArray());
      }
      catch (Exception ex)
      {
        Tracing.Log(this.sw, TraceLevel.Error, "MedianFamilyIncomeLimitsControl", ex.Message);
        return DialogResult.Abort;
      }
      this.populateMFIData(source.OrderByDescending<MFILimit, int>((Func<MFILimit, int>) (x => x.SourceFileYear)).ToArray<MFILimit>());
      return DialogResult.OK;
    }

    private void setMfiLastDownloadTime()
    {
      string str;
      try
      {
        str = DateTime.Parse(this.session.ConfigurationManager.GetCompanySetting("MFITABLE", "DOWNLOADTIME")).ToLocalTime().ToString();
      }
      catch (Exception ex)
      {
        Tracing.Log(this.sw, TraceLevel.Error, "MedianFamilyIncomeLimitsControl", ex.Message);
        return;
      }
      if (string.IsNullOrEmpty(str))
        return;
      this.timestampLabelMfi.Text = str;
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
      GVColumn gvColumn9 = new GVColumn();
      GVColumn gvColumn10 = new GVColumn();
      GVColumn gvColumn11 = new GVColumn();
      GVColumn gvColumn12 = new GVColumn();
      GVColumn gvColumn13 = new GVColumn();
      GVColumn gvColumn14 = new GVColumn();
      this.groupContainer2 = new GroupContainer();
      this.gridView1 = new GridView();
      this.timestampLabel = new Label();
      this.downloadBtn = new Button();
      this.tabControl1 = new TabControl();
      this.tabPage1 = new TabPage();
      this.tabPage3 = new TabPage();
      this.groupContainer1 = new GroupContainer();
      this.timestampLabelMfi = new Label();
      this.gridView2 = new GridView();
      this.button1 = new Button();
      this.groupContainer2.SuspendLayout();
      this.tabControl1.SuspendLayout();
      this.tabPage1.SuspendLayout();
      this.tabPage3.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.groupContainer2.Controls.Add((Control) this.gridView1);
      this.groupContainer2.Controls.Add((Control) this.timestampLabel);
      this.groupContainer2.Controls.Add((Control) this.downloadBtn);
      this.groupContainer2.Dock = DockStyle.Fill;
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(3, 3);
      this.groupContainer2.Margin = new Padding(4, 5, 4, 5);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(1096, 763);
      this.groupContainer2.TabIndex = 1;
      this.gridView1.AllowMultiselect = false;
      this.gridView1.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Year";
      gvColumn1.Width = 100;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "FIPS Code";
      gvColumn2.Width = 100;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "State Name";
      gvColumn3.Width = 100;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column5";
      gvColumn4.Text = "County Name";
      gvColumn4.Width = 100;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column6";
      gvColumn5.Text = "100% AMI Limit";
      gvColumn5.Width = 100;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column7";
      gvColumn6.Text = "80% AMI Limit";
      gvColumn6.Width = 100;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Column8";
      gvColumn7.Text = "50% AMI Limit";
      gvColumn7.Width = 100;
      this.gridView1.Columns.AddRange(new GVColumn[7]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7
      });
      this.gridView1.Dock = DockStyle.Fill;
      this.gridView1.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridView1.Location = new Point(1, 26);
      this.gridView1.Margin = new Padding(4, 5, 4, 5);
      this.gridView1.Name = "gridView1";
      this.gridView1.Size = new Size(1094, 736);
      this.gridView1.TabIndex = 2;
      this.timestampLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.timestampLabel.AutoSize = true;
      this.timestampLabel.BackColor = Color.Transparent;
      this.timestampLabel.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.timestampLabel.Location = new Point(756, 9);
      this.timestampLabel.Margin = new Padding(4, 0, 4, 0);
      this.timestampLabel.Name = "timestampLabel";
      this.timestampLabel.Size = new Size(0, 19);
      this.timestampLabel.TabIndex = 1;
      this.downloadBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.downloadBtn.BackColor = SystemColors.Control;
      this.downloadBtn.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.downloadBtn.Location = new Point(970, 2);
      this.downloadBtn.Margin = new Padding(4, 5, 4, 5);
      this.downloadBtn.Name = "downloadBtn";
      this.downloadBtn.Size = new Size(112, 35);
      this.downloadBtn.TabIndex = 0;
      this.downloadBtn.Text = "Download";
      this.downloadBtn.UseVisualStyleBackColor = false;
      this.downloadBtn.Click += new EventHandler(this.downloadBtn_Click);
      this.tabControl1.Controls.Add((Control) this.tabPage1);
      this.tabControl1.Controls.Add((Control) this.tabPage3);
      this.tabControl1.Dock = DockStyle.Fill;
      this.tabControl1.Location = new Point(0, 0);
      this.tabControl1.Margin = new Padding(0);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new Size(1110, 802);
      this.tabControl1.TabIndex = 2;
      this.tabPage1.Controls.Add((Control) this.groupContainer2);
      this.tabPage1.Location = new Point(4, 29);
      this.tabPage1.Name = "tabPage1";
      this.tabPage1.Padding = new Padding(3);
      this.tabPage1.Size = new Size(1102, 769);
      this.tabPage1.TabIndex = 0;
      this.tabPage1.Text = "AMI Limits";
      this.tabPage1.UseVisualStyleBackColor = true;
      this.tabPage3.Controls.Add((Control) this.groupContainer1);
      this.tabPage3.Location = new Point(4, 29);
      this.tabPage3.Name = "tabPage3";
      this.tabPage3.Padding = new Padding(3);
      this.tabPage3.Size = new Size(1102, 769);
      this.tabPage3.TabIndex = 2;
      this.tabPage3.Text = "MFI Limits";
      this.tabPage3.UseVisualStyleBackColor = true;
      this.groupContainer1.Controls.Add((Control) this.timestampLabelMfi);
      this.groupContainer1.Controls.Add((Control) this.gridView2);
      this.groupContainer1.Controls.Add((Control) this.button1);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(3, 3);
      this.groupContainer1.Margin = new Padding(4, 5, 4, 5);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(1096, 763);
      this.groupContainer1.TabIndex = 2;
      this.timestampLabelMfi.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.timestampLabelMfi.AutoSize = true;
      this.timestampLabelMfi.BackColor = Color.Transparent;
      this.timestampLabelMfi.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.timestampLabelMfi.Location = new Point(755, 9);
      this.timestampLabelMfi.Margin = new Padding(4, 0, 4, 0);
      this.timestampLabelMfi.Name = "timestampLabelMfi";
      this.timestampLabelMfi.Size = new Size(0, 19);
      this.timestampLabelMfi.TabIndex = 2;
      this.gridView2.AllowMultiselect = false;
      this.gridView2.BorderStyle = BorderStyle.None;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "Column1";
      gvColumn8.Text = "Year of Source File";
      gvColumn8.Width = 150;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "Column2";
      gvColumn9.Text = "MSA/MD Code Number";
      gvColumn9.Width = 150;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "Column3";
      gvColumn10.Text = "MSA/MD Name";
      gvColumn10.Width = 100;
      gvColumn11.ImageIndex = -1;
      gvColumn11.Name = "Column4";
      gvColumn11.Text = "Actual MFI Year";
      gvColumn11.Width = 100;
      gvColumn12.ImageIndex = -1;
      gvColumn12.Name = "Column5";
      gvColumn12.Text = "Actual Median Family Income";
      gvColumn12.Width = 170;
      gvColumn13.ImageIndex = -1;
      gvColumn13.Name = "Column6";
      gvColumn13.Text = "Estimated MFI Year";
      gvColumn13.Width = 150;
      gvColumn14.ImageIndex = -1;
      gvColumn14.Name = "Column7";
      gvColumn14.Text = "Estimated Median Family Income";
      gvColumn14.Width = 180;
      this.gridView2.Columns.AddRange(new GVColumn[7]
      {
        gvColumn8,
        gvColumn9,
        gvColumn10,
        gvColumn11,
        gvColumn12,
        gvColumn13,
        gvColumn14
      });
      this.gridView2.Dock = DockStyle.Fill;
      this.gridView2.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridView2.Location = new Point(1, 26);
      this.gridView2.Margin = new Padding(4, 5, 4, 5);
      this.gridView2.Name = "gridView2";
      this.gridView2.Size = new Size(1094, 736);
      this.gridView2.TabIndex = 0;
      this.button1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.button1.BackColor = SystemColors.Control;
      this.button1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.button1.Location = new Point(969, 2);
      this.button1.Margin = new Padding(4, 5, 4, 5);
      this.button1.Name = "button1";
      this.button1.Size = new Size(112, 35);
      this.button1.TabIndex = 1;
      this.button1.Text = "Download";
      this.button1.UseVisualStyleBackColor = false;
      this.button1.Click += new EventHandler(this.downloadMfiBtn_Click);
      this.AutoScaleDimensions = new SizeF(9f, 20f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.tabControl1);
      this.Margin = new Padding(4, 5, 4, 5);
      this.Name = nameof (AreaMedianIncomeLimitsControl);
      this.Size = new Size(1110, 802);
      this.groupContainer2.ResumeLayout(false);
      this.groupContainer2.PerformLayout();
      this.tabControl1.ResumeLayout(false);
      this.tabPage1.ResumeLayout(false);
      this.tabPage3.ResumeLayout(false);
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
