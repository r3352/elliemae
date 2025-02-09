// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.FedTresholdAdjustmentControl
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
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class FedTresholdAdjustmentControl : SettingsUserControl
  {
    private Sessions.Session session;
    private XmlDocument XmlDoc;
    private const string className = "FedTresholdAdjustmentControl";
    private static readonly string sw = Tracing.SwOutsideLoan;
    private IContainer components;
    private GroupContainer groupContainer2;
    private GridView fedAdjustmentList;
    private Button button2;
    private Button button1;

    public FedTresholdAdjustmentControl(SetUpContainer setupContainer, Sessions.Session session)
      : this(setupContainer, session, false)
    {
    }

    public FedTresholdAdjustmentControl(
      SetUpContainer setupContainer,
      Sessions.Session session,
      bool allowMultiSelect)
      : base(setupContainer)
    {
      this.session = session;
      this.InitializeComponent();
      this.initializeData();
      this.Reset();
    }

    private void initializeData()
    {
      FedTresholdAdjustment[] tresholdAdjustments = this.session.ConfigurationManager.GetFedTresholdAdjustments();
      this.populateFedAdjustmentList(tresholdAdjustments);
      int latestYearFromDb = this.findLatestYearFromDB(tresholdAdjustments);
      int num = 0;
      try
      {
        List<FedTresholdAdjustment> adjustmentsFromXml = this.getAdjustmentsFromXML();
        if (adjustmentsFromXml != null)
        {
          if (adjustmentsFromXml.Count > 0)
            num = adjustmentsFromXml.Max<FedTresholdAdjustment>((Func<FedTresholdAdjustment, int>) (x => x.AdjustmentYear));
        }
      }
      catch
      {
      }
      if ((latestYearFromDb != 0 || num == 0) && latestYearFromDb >= num || Utils.Dialog((IWin32Window) this, "Updated data is available for ATR/QM Thresholds. Would you like to import it?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
        return;
      this.synchronizeData((object) null, (EventArgs) null);
    }

    private int findLatestYearFromDB(FedTresholdAdjustment[] limitList)
    {
      int latestYearFromDb = 0;
      if (limitList == null || limitList.Length == 0 || limitList.Length < 1)
        return latestYearFromDb;
      latestYearFromDb = limitList[0].AdjustmentYear;
      return latestYearFromDb;
    }

    private void populateFedAdjustmentList(FedTresholdAdjustment[] limitList)
    {
      this.fedAdjustmentList.Items.Clear();
      this.fedAdjustmentList.BeginUpdate();
      foreach (FedTresholdAdjustment limit in limitList)
      {
        bool flag = limit.RuleType.ToLower() == "percent";
        this.fedAdjustmentList.Items.Add(new GVItem(new List<string>()
        {
          string.Concat((object) limit.AdjustmentYear),
          string.Concat((object) limit.RuleIndex),
          CountyLimitEditDialog.ToDoubleString(limit.LowerRange ?? ""),
          limit.RuleIndex != 1 || !(limit.UpperRange == "") ? CountyLimitEditDialog.ToDoubleString(limit.UpperRange ?? "") : "N/A",
          flag ? limit.RuleValue + "%" : "$" + CountyLimitEditDialog.ToDoubleString(limit.RuleValue.ToString())
        }.ToArray())
        {
          Tag = (object) limit
        });
      }
      this.fedAdjustmentList.EndUpdate();
    }

    private void synchronizeData(object sender, EventArgs e)
    {
      try
      {
        switch (new ProgressDialog("Please wait while Federal Threshold Adjustments are updated", new AsynchronousProcess(this.synchronize), (object) null, false).ShowDialog((IWin32Window) this))
        {
          case DialogResult.OK:
            this.initializeData();
            if (this.session == null || this.session.SessionObjects == null)
              break;
            this.session.SessionObjects.RefreshTresholdLimitsFromDB = true;
            break;
          case DialogResult.Cancel:
            int num1 = (int) Utils.Dialog((IWin32Window) this, "No Federal threshold adjustment record could be imported successfully.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            break;
          case DialogResult.Abort:
            int num2 = (int) Utils.Dialog((IWin32Window) this, "This feature and setting will be available when the ATR/QM Threshold for next year is available from the Consumer Finance Protection Bureau.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            break;
          default:
            int num3 = (int) Utils.Dialog((IWin32Window) this, "Exception occured while updating Federal Threshold Adjustments records.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            break;
        }
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "The following error occurred when trying to download the file:\n\n" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private DialogResult synchronize(object data, IProgressFeedback feedback)
    {
      try
      {
        feedback.Status = "Synchronizing...";
        feedback.MaxValue = 100;
        List<FedTresholdAdjustment> adjustmentsFromXml = this.getAdjustmentsFromXML();
        if (adjustmentsFromXml == null || adjustmentsFromXml.Count == 0)
          return DialogResult.Cancel;
        feedback.Increment(20);
        feedback.Status = "Inserting data into the database...";
        this.session.ConfigurationManager.ResetFedTresholdAdjustments(adjustmentsFromXml.ToArray());
        return DialogResult.OK;
      }
      catch (Exception ex)
      {
        Tracing.Log(FedTresholdAdjustmentControl.sw, TraceLevel.Error, nameof (FedTresholdAdjustmentControl), string.Format("Federal Threshold Adjustment import exception (Message {0})", (object) ex.Message));
        return ex.Message.Contains("404") ? DialogResult.Abort : DialogResult.Cancel;
      }
    }

    private List<FedTresholdAdjustment> getAdjustmentsFromXML()
    {
      string str1 = this.session?.StartupInfo?.ServiceUrls?.DownloadServiceUrl;
      if (string.IsNullOrWhiteSpace(str1) || !Uri.IsWellFormedUriString(str1, UriKind.Absolute))
        str1 = "https://encompass.elliemae.com/download/download.asp";
      byte[] bytes = (byte[]) null;
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("ClientID=" + HttpUtility.UrlEncode(this.session.CompanyInfo.ClientID));
      stringBuilder.Append("&UserID=" + HttpUtility.UrlEncode(this.session.UserInfo.Userid));
      stringBuilder.Append("&FileID=" + HttpUtility.UrlEncode("FederalThresholdLimits.xml"));
      HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(str1);
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
      string xml = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
      this.XmlDoc = new XmlDocument();
      this.XmlDoc.LoadXml(xml);
      XmlNodeList xmlNodeList = this.XmlDoc.SelectNodes("Root/FederalThresholdLimits/FederalThresholdLimit");
      List<FedTresholdAdjustment> adjustmentsFromXml = new List<FedTresholdAdjustment>();
      int id = 1;
      foreach (XmlNode node in xmlNodeList)
      {
        string str2 = this.readXMLAttribute(node, "AdjustmentYear");
        foreach (XmlNode selectNode in node.SelectNodes("Rules/Rule"))
        {
          string str3 = this.readXMLAttribute(selectNode, "RuleIndex");
          string lowerRange = this.readXMLAttribute(selectNode, "LowerRange");
          string upperRange = this.readXMLAttribute(selectNode, "UpperRange");
          string ruleValue = this.readXMLAttribute(selectNode, "RuleValue");
          string ruleType = this.readXMLAttribute(selectNode, "RuleType");
          FedTresholdAdjustment tresholdAdjustment = new FedTresholdAdjustment(id, Utils.ParseInt((object) str3), Utils.ParseInt((object) str2), lowerRange, upperRange, ruleValue, ruleType, DateTime.Now);
          adjustmentsFromXml.Add(tresholdAdjustment);
          ++id;
        }
      }
      return adjustmentsFromXml;
    }

    private string readSingleChildNode(XmlNode parentNode, string childNodeRelativePath)
    {
      XmlNode xmlNode = parentNode.SelectSingleNode(childNodeRelativePath);
      return xmlNode != null ? xmlNode.InnerText : "";
    }

    private string readXMLAttribute(XmlNode node, string attributeName)
    {
      try
      {
        return node?.Attributes[attributeName]?.Value;
      }
      catch (Exception ex)
      {
        return "";
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
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      this.groupContainer2 = new GroupContainer();
      this.button2 = new Button();
      this.button1 = new Button();
      this.fedAdjustmentList = new GridView();
      this.groupContainer2.SuspendLayout();
      this.SuspendLayout();
      this.groupContainer2.Controls.Add((Control) this.button2);
      this.groupContainer2.Controls.Add((Control) this.button1);
      this.groupContainer2.Controls.Add((Control) this.fedAdjustmentList);
      this.groupContainer2.Dock = DockStyle.Fill;
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(0, 0);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(974, 736);
      this.groupContainer2.TabIndex = 1;
      this.button2.Location = new Point(901, 95);
      this.button2.Name = "button2";
      this.button2.Size = new Size(8, 8);
      this.button2.TabIndex = 2;
      this.button2.Text = "button2";
      this.button2.UseVisualStyleBackColor = true;
      this.button1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.button1.Location = new Point(860, 1);
      this.button1.Name = "button1";
      this.button1.Size = new Size(110, 23);
      this.button1.TabIndex = 1;
      this.button1.Text = "Synchronize";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new EventHandler(this.synchronizeData);
      this.fedAdjustmentList.AllowMultiselect = false;
      this.fedAdjustmentList.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Year";
      gvColumn1.Width = 100;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column5";
      gvColumn2.Text = "Rule Index";
      gvColumn2.Width = 100;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column2";
      gvColumn3.Text = "Loan Amount Lower Range >=";
      gvColumn3.Width = 170;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column3";
      gvColumn4.Text = "Loan Amount Upper Range <";
      gvColumn4.Width = 170;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column4";
      gvColumn5.Text = "Rule $ or %";
      gvColumn5.Width = 100;
      this.fedAdjustmentList.Columns.AddRange(new GVColumn[5]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5
      });
      this.fedAdjustmentList.Dock = DockStyle.Fill;
      this.fedAdjustmentList.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.fedAdjustmentList.Location = new Point(1, 26);
      this.fedAdjustmentList.Name = "fedAdjustmentList";
      this.fedAdjustmentList.Size = new Size(972, 709);
      this.fedAdjustmentList.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.groupContainer2);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Margin = new Padding(2, 3, 2, 3);
      this.Name = nameof (FedTresholdAdjustmentControl);
      this.Size = new Size(974, 736);
      this.groupContainer2.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
