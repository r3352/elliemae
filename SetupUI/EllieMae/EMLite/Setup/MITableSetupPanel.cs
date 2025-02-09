// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.MITableSetupPanel
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.Properties;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class MITableSetupPanel : UserControl
  {
    private const string className = "MITableSetupPanel";
    private static readonly string sw = Tracing.SwOutsideLoan;
    private TabPage currTabPage;
    private MITableControl currMITable;
    private MITableControl convTable;
    private MITableControl fhaTable;
    private MITableControl vaTable;
    private MITableControl usdaTable;
    private MITableControl otherTable;
    private MITableControl fhaDownloadTable;
    private MITableControl vaDownloadTable;
    private MITableControl usdaDownloadTable;
    private Sessions.Session session;
    private string[] lastDownloadTime;
    private IContainer components;
    private TabControl tabControlMI;
    private TabPage tabPageFHADL;
    private TabPage tabPageVADL;
    private TabPage tabPageConv;
    private TabPage tabPageOther;
    private TabPage tabPageFHA;
    private TabPage tabPageVA;
    private Button btnExport;
    private TabControl tabControlConv;
    private TabPage tabPageConvGeneral;
    private GroupContainer gcMITables;
    private GroupContainer gcConventional;
    private StandardIconButton stdIconBtnDelete;
    private ToolTip toolTip1;
    private StandardIconButton stdIconBtnNewTab;
    private StandardIconButton stdIconBtnRenameTab;
    private StandardIconButton stdIconBtnDeleteTab;
    private StandardIconButton stdIconBtnNew;
    private StandardIconButton stdIconBtnDuplicate;
    private StandardIconButton stdIconBtnEdit;
    private Button btnDownload;
    private BorderPanel borderPanel1;
    private IconButton iconbtnHelp;
    private VerticalSeparator verticalSeparator1;
    private TabPage tabPageUSDA;
    private TabPage tabPageUSDADL;
    private Label labelLastDownload;

    public MITableSetupPanel(Sessions.Session session)
    {
      this.InitializeComponent();
      this.tabControlMI.TabPages.Remove(this.tabPageUSDA);
      this.tabControlMI.TabPages.Remove(this.tabPageUSDADL);
      this.session = session;
      this.Dock = DockStyle.Fill;
      this.initForm();
      if (EnConfigurationSettings.GlobalSettings.Debug)
        this.btnExport.Visible = true;
      else
        this.btnExport.Visible = false;
      this.verticalSeparator1.Visible = false;
    }

    private void initForm()
    {
      string[] miTabNames = this.session.ConfigurationManager.GetMITabNames();
      if (miTabNames != null && miTabNames.Length != 0)
      {
        for (int index = 0; index < miTabNames.Length; ++index)
          this.tabControlConv.TabPages.Add(miTabNames[index]);
      }
      this.lastDownloadTime = this.session.ConfigurationManager.GetCompanySetting("MITABLE", "DOWNLOADTIME").Split('|');
      if (this.lastDownloadTime == null || this.lastDownloadTime.Length != 3)
        this.lastDownloadTime = new string[3];
      this.tabControlConv.SelectedIndex = 0;
      this.tabControlConv_SelectedIndexChanged((object) null, (EventArgs) null);
      this.tabControlMI_SelectedIndexChanged((object) null, (EventArgs) null);
    }

    private void miTableSelectedIndexChanged(object sender, EventArgs e)
    {
      this.stdIconBtnEdit.Enabled = this.stdIconBtnDuplicate.Enabled = this.currMITable != null && this.currMITable.ListViewSelectedItemCount == 1;
      this.stdIconBtnDelete.Enabled = this.currMITable != null && this.currMITable.ListViewSelectedItemCount > 0;
    }

    private void tabControlMI_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.tabControlMI.SelectedTab == this.tabPageFHADL || this.tabControlMI.SelectedTab == this.tabPageVADL || this.tabControlMI.SelectedTab == this.tabPageUSDADL)
      {
        this.stdIconBtnNew.Visible = false;
        this.stdIconBtnEdit.Visible = false;
        this.stdIconBtnDuplicate.Visible = false;
        this.btnDownload.Visible = true;
        this.verticalSeparator1.Visible = true;
        this.btnExport.Visible = false;
        this.rearrangeButtons();
      }
      else
      {
        this.btnDownload.Visible = false;
        this.verticalSeparator1.Visible = false;
        this.stdIconBtnNew.Visible = true;
        this.stdIconBtnEdit.Visible = true;
        this.stdIconBtnDuplicate.Visible = true;
        this.labelLastDownload.Visible = false;
        if (EnConfigurationSettings.GlobalSettings.Debug)
        {
          this.verticalSeparator1.Visible = true;
          this.btnExport.Visible = true;
          this.btnExport.Left = this.verticalSeparator1.Left - this.btnExport.Width - 4;
        }
        this.stdIconBtnDuplicate.Left = EnConfigurationSettings.GlobalSettings.Debug ? this.btnExport.Left - this.stdIconBtnDuplicate.Width - 6 : this.stdIconBtnDelete.Left - this.stdIconBtnDuplicate.Width - 6;
        this.stdIconBtnEdit.Left = this.stdIconBtnDuplicate.Left - this.stdIconBtnEdit.Width - 6;
        this.stdIconBtnNew.Left = this.stdIconBtnEdit.Left - this.stdIconBtnNew.Width - 6;
      }
      if (this.currTabPage != null && this.currTabPage.Controls.Contains((Control) this.gcMITables))
        this.currTabPage.Controls.Remove((Control) this.gcMITables);
      this.currTabPage = this.tabControlMI.SelectedTab;
      if (this.currMITable != null && this.gcMITables.Controls.Contains((Control) this.currMITable))
        this.gcMITables.Controls.Remove((Control) this.currMITable);
      if (this.tabControlMI.SelectedTab == this.tabPageConv)
      {
        this.tabControlConv_SelectedIndexChanged((object) this, (EventArgs) null);
      }
      else
      {
        this.tabControlMI.SelectedTab.Controls.Add((Control) this.gcMITables);
        if (this.tabControlMI.SelectedTab == this.tabPageFHA)
        {
          if (this.fhaTable == null)
          {
            this.fhaTable = new MITableControl(LoanTypeEnum.FHA, "", false, this.session);
            this.fhaTable.ListViewSelectedIndexChanged += new EventHandler(this.miTableSelectedIndexChanged);
          }
          this.currMITable = this.fhaTable;
        }
        else if (this.tabControlMI.SelectedTab == this.tabPageVA)
        {
          if (this.vaTable == null)
          {
            this.vaTable = new MITableControl(LoanTypeEnum.VA, "", false, this.session);
            this.vaTable.ListViewSelectedIndexChanged += new EventHandler(this.miTableSelectedIndexChanged);
          }
          this.currMITable = this.vaTable;
        }
        else if (this.tabControlMI.SelectedTab == this.tabPageUSDA)
        {
          if (this.usdaTable == null)
          {
            this.usdaTable = new MITableControl(LoanTypeEnum.USDA, "", false, this.session);
            this.usdaTable.ListViewSelectedIndexChanged += new EventHandler(this.miTableSelectedIndexChanged);
          }
          this.currMITable = this.usdaTable;
        }
        else if (this.tabControlMI.SelectedTab == this.tabPageOther)
        {
          if (this.otherTable == null)
          {
            this.otherTable = new MITableControl(LoanTypeEnum.Other, "", false, this.session);
            this.otherTable.ListViewSelectedIndexChanged += new EventHandler(this.miTableSelectedIndexChanged);
          }
          this.currMITable = this.otherTable;
        }
        else if (this.tabControlMI.SelectedTab == this.tabPageFHADL)
        {
          if (this.fhaDownloadTable == null)
          {
            this.fhaDownloadTable = new MITableControl(LoanTypeEnum.FHA, "", true, this.session);
            this.fhaDownloadTable.ListViewSelectedIndexChanged += new EventHandler(this.miTableSelectedIndexChanged);
          }
          this.currMITable = this.fhaDownloadTable;
        }
        else if (this.tabControlMI.SelectedTab == this.tabPageVADL)
        {
          if (this.vaDownloadTable == null)
          {
            this.vaDownloadTable = new MITableControl(LoanTypeEnum.VA, "", true, this.session);
            this.vaDownloadTable.ListViewSelectedIndexChanged += new EventHandler(this.miTableSelectedIndexChanged);
          }
          this.currMITable = this.vaDownloadTable;
        }
        else if (this.tabControlMI.SelectedTab == this.tabPageUSDADL)
        {
          if (this.usdaDownloadTable == null)
          {
            this.usdaDownloadTable = new MITableControl(LoanTypeEnum.USDA, "", true, this.session);
            this.usdaDownloadTable.ListViewSelectedIndexChanged += new EventHandler(this.miTableSelectedIndexChanged);
          }
          this.currMITable = this.usdaDownloadTable;
        }
      }
      this.gcMITables.Controls.Add((Control) this.currMITable);
      this.miTableSelectedIndexChanged((object) this, (EventArgs) null);
      this.setGcMITablesTitle();
    }

    private void rearrangeButtons()
    {
      if (this.tabControlMI.SelectedTab == this.tabPageFHADL)
        this.labelLastDownload.Text = this.lastDownloadTime[0];
      else if (this.tabControlMI.SelectedTab == this.tabPageVADL)
        this.labelLastDownload.Text = this.lastDownloadTime[1];
      else if (this.tabControlMI.SelectedTab == this.tabPageUSDADL)
        this.labelLastDownload.Text = this.lastDownloadTime[2];
      else
        this.labelLastDownload.Text = string.Empty;
      this.labelLastDownload.Visible = this.labelLastDownload.Text != string.Empty;
      if (!(this.labelLastDownload.Text != string.Empty))
        return;
      this.labelLastDownload.Left = this.btnDownload.Left - this.labelLastDownload.Width;
    }

    public string[] SelectedItems
    {
      get
      {
        if (this.tabControlMI.SelectedTab == this.tabPageConv)
          return this.convTable.SelectedItems;
        if (this.tabControlMI.SelectedTab == this.tabPageOther)
          return this.otherTable.SelectedItems;
        if (this.tabControlMI.SelectedTab == this.tabPageFHA)
          return this.fhaTable.SelectedItems;
        if (this.tabControlMI.SelectedTab == this.tabPageFHADL)
          return this.fhaDownloadTable.SelectedItems;
        if (this.tabControlMI.SelectedTab == this.tabPageVA)
          return this.vaTable.SelectedItems;
        if (this.tabControlMI.SelectedTab == this.tabPageVADL)
          return this.vaDownloadTable.SelectedItems;
        if (this.tabControlMI.SelectedTab == this.tabPageUSDA)
          return this.usdaTable.SelectedItems;
        return this.tabControlMI.SelectedTab == this.tabPageUSDADL ? this.usdaDownloadTable.SelectedItems : new string[0];
      }
      set
      {
        if (this.tabControlMI.SelectedTab == this.tabPageConv)
          this.convTable.SelectedItems = value;
        else if (this.tabControlMI.SelectedTab == this.tabPageOther)
          this.otherTable.SelectedItems = value;
        else if (this.tabControlMI.SelectedTab == this.tabPageFHA)
          this.fhaTable.SelectedItems = value;
        else if (this.tabControlMI.SelectedTab == this.tabPageFHADL)
          this.fhaDownloadTable.SelectedItems = value;
        else if (this.tabControlMI.SelectedTab == this.tabPageVA)
          this.vaTable.SelectedItems = value;
        else if (this.tabControlMI.SelectedTab == this.tabPageVADL)
          this.vaDownloadTable.SelectedItems = value;
        else if (this.tabControlMI.SelectedTab == this.tabPageUSDA)
        {
          this.usdaTable.SelectedItems = value;
        }
        else
        {
          if (this.tabControlMI.SelectedTab != this.tabPageUSDADL)
            return;
          this.usdaDownloadTable.SelectedItems = value;
        }
      }
    }

    public string SelectedTable
    {
      get
      {
        if (this.tabControlMI.SelectedTab == this.tabPageConv)
          return "MIConv";
        if (this.tabControlMI.SelectedTab == this.tabPageOther)
          return "MIOther";
        if (this.tabControlMI.SelectedTab == this.tabPageFHA)
          return "MIFHA";
        if (this.tabControlMI.SelectedTab == this.tabPageFHADL)
          return "MIFHADownload";
        if (this.tabControlMI.SelectedTab == this.tabPageVA)
          return "MIVA";
        if (this.tabControlMI.SelectedTab == this.tabPageVADL)
          return "MIVADownload";
        if (this.tabControlMI.SelectedTab == this.tabPageUSDA)
          return "MIUSDA";
        return this.tabControlMI.SelectedTab == this.tabPageUSDADL ? "MIUSDADownload" : "";
      }
      set
      {
        switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(value))
        {
          case 7104205:
            if (!(value == "MIConv"))
              break;
            this.tabControlMI.SelectedTab = this.tabPageConv;
            break;
          case 380960447:
            if (!(value == "MIOther"))
              break;
            this.tabControlMI.SelectedTab = this.tabPageOther;
            break;
          case 506702090:
            if (!(value == "MIVA"))
              break;
            this.tabControlMI.SelectedTab = this.tabPageVA;
            break;
          case 1561549660:
            if (!(value == "MIFHA"))
              break;
            this.tabControlMI.SelectedTab = this.tabPageFHA;
            break;
          case 1712550892:
            if (!(value == "MIUSDA"))
              break;
            this.tabControlMI.SelectedTab = this.tabPageUSDA;
            break;
          case 3035070932:
            if (!(value == "MIUSDADownload"))
              break;
            this.tabControlMI.SelectedTab = this.tabPageUSDADL;
            break;
          case 3983362458:
            if (!(value == "MIVADownload"))
              break;
            this.tabControlMI.SelectedTab = this.tabPageVADL;
            break;
          case 4141228196:
            if (!(value == "MIFHADownload"))
              break;
            this.tabControlMI.SelectedTab = this.tabPageFHADL;
            break;
        }
      }
    }

    private void tabControlConv_SelectedIndexChanged(object sender, EventArgs e)
    {
      string text = this.tabControlConv.SelectedTab.Text;
      this.currTabPage = this.tabControlConv.SelectedTab;
      if (this.convTable == null)
      {
        this.convTable = new MITableControl(LoanTypeEnum.Conventional, text, false, this.session);
        this.convTable.ListViewSelectedIndexChanged += new EventHandler(this.miTableSelectedIndexChanged);
      }
      else
        this.convTable.ReloadRecords(LoanTypeEnum.Conventional, text, false);
      if (this.currMITable != null && this.gcMITables.Controls.Contains((Control) this.currMITable))
        this.gcMITables.Controls.Remove((Control) this.currMITable);
      this.currMITable = this.convTable;
      this.gcMITables.Controls.Add((Control) this.currMITable);
      this.setGcMITablesTitle();
      this.tabControlConv.SelectedTab.Controls.Clear();
      this.tabControlConv.SelectedTab.Controls.Add((Control) this.gcMITables);
      this.miTableSelectedIndexChanged((object) this, (EventArgs) null);
      this.setGcMITablesTitle();
    }

    private void setGcMITablesTitle()
    {
      this.gcMITables.Text = "MI Tables (" + (object) this.currMITable.ListViewItemCount + ")";
    }

    private void addBtn_Click(object sender, EventArgs e)
    {
      if (this.currMITable != null)
        this.currMITable.AddRecord();
      this.setGcMITablesTitle();
    }

    private void btnDownload_Click(object sender, EventArgs e)
    {
      if (this.tabControlMI.SelectedTab == this.tabPageFHADL)
        this.downloadTable(LoanTypeEnum.FHA);
      else if (this.tabControlMI.SelectedTab == this.tabPageVADL)
        this.downloadTable(LoanTypeEnum.VA);
      else if (this.tabControlMI.SelectedTab == this.tabPageUSDADL)
        this.downloadTable(LoanTypeEnum.USDA);
      this.setGcMITablesTitle();
    }

    private void downloadTable(LoanTypeEnum loanType)
    {
      string str1;
      switch (loanType)
      {
        case LoanTypeEnum.FHA:
          str1 = "FHA";
          break;
        case LoanTypeEnum.VA:
          str1 = "VA";
          break;
        default:
          str1 = "USDA";
          break;
      }
      string str2 = str1;
      bool flag = false;
      if (loanType == LoanTypeEnum.FHA && this.fhaDownloadTable.ListViewItemCount > 0 || loanType == LoanTypeEnum.VA && this.vaDownloadTable.ListViewItemCount > 0 || loanType == LoanTypeEnum.USDA && this.usdaDownloadTable.ListViewItemCount > 0)
        flag = true;
      if (flag && Utils.Dialog((IWin32Window) this, "The current list will be cleared. Are you sure you want to download the latest " + str2 + " MI table?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
        return;
      try
      {
        Cursor.Current = Cursors.WaitCursor;
        byte[] data = this.downloadFile(str2 + ".XML");
        if (data == null || data.Length == 0)
        {
          Cursor.Current = Cursors.Default;
          Tracing.Log(MITableSetupPanel.sw, TraceLevel.Error, nameof (MITableSetupPanel), str2 + ".XML cannnot be downloaded.");
          return;
        }
        BinaryObject binaryObject = new BinaryObject(data);
        if (binaryObject == null)
        {
          Cursor.Current = Cursors.Default;
          Tracing.Log(MITableSetupPanel.sw, TraceLevel.Error, nameof (MITableSetupPanel), "The downloaded bytes cannot be converted to BinaryObject.");
          int num = (int) Utils.Dialog((IWin32Window) this, "Encompass cannot download MI table. Error: file is not XML format.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
        MIRecordXML miRecordXml = (MIRecordXML) binaryObject;
        if (miRecordXml == null)
        {
          Cursor.Current = Cursors.Default;
          int num = (int) Utils.Dialog((IWin32Window) this, "Encompass cannot load XML file to MI table.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
        DateTime now = DateTime.Now;
        string empty = string.Empty;
        this.session.ConfigurationManager.UpdateMITable(miRecordXml.GetRecords(), loanType, "", true);
        switch (loanType)
        {
          case LoanTypeEnum.USDA:
            this.gcMITables.Controls.Remove((Control) this.usdaDownloadTable);
            this.usdaDownloadTable.Dispose();
            this.usdaDownloadTable = new MITableControl(LoanTypeEnum.USDA, "", true, this.session);
            this.usdaDownloadTable.ListViewSelectedIndexChanged += new EventHandler(this.miTableSelectedIndexChanged);
            this.gcMITables.Controls.Add((Control) this.usdaDownloadTable);
            this.currMITable = this.usdaDownloadTable;
            this.lastDownloadTime[2] = now.ToString("MM/dd/yyyy hh:mm:ss tt");
            break;
          case LoanTypeEnum.FHA:
            this.gcMITables.Controls.Remove((Control) this.fhaDownloadTable);
            this.fhaDownloadTable.Dispose();
            this.fhaDownloadTable = new MITableControl(LoanTypeEnum.FHA, "", true, this.session);
            this.fhaDownloadTable.ListViewSelectedIndexChanged += new EventHandler(this.miTableSelectedIndexChanged);
            this.gcMITables.Controls.Add((Control) this.fhaDownloadTable);
            this.currMITable = this.fhaDownloadTable;
            this.lastDownloadTime[0] = now.ToString("MM/dd/yyyy hh:mm:ss tt");
            break;
          case LoanTypeEnum.VA:
            this.gcMITables.Controls.Remove((Control) this.vaDownloadTable);
            this.vaDownloadTable.Dispose();
            this.vaDownloadTable = new MITableControl(LoanTypeEnum.VA, "", true, this.session);
            this.vaDownloadTable.ListViewSelectedIndexChanged += new EventHandler(this.miTableSelectedIndexChanged);
            this.gcMITables.Controls.Add((Control) this.vaDownloadTable);
            this.currMITable = this.vaDownloadTable;
            this.lastDownloadTime[1] = now.ToString("MM/dd/yyyy hh:mm:ss tt");
            break;
        }
        this.session.ConfigurationManager.SetCompanySetting("MITABLE", "DOWNLOADTIME", this.lastDownloadTime[0] + "|" + this.lastDownloadTime[1] + "|" + this.lastDownloadTime[2]);
        this.rearrangeButtons();
      }
      catch (Exception ex)
      {
        Tracing.Log(MITableSetupPanel.sw, TraceLevel.Error, nameof (MITableSetupPanel), "Encompass cannot download MI table. " + ex.Message);
        int num = (int) Utils.Dialog((IWin32Window) this, "Encompass cannot download MI table. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      Cursor.Current = Cursors.Default;
    }

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
        Tracing.Log(MITableSetupPanel.sw, TraceLevel.Error, nameof (MITableSetupPanel), ex.Message);
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "The following error occurred when trying to download the file:\n\n" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      return numArray;
    }

    private void editBtn_Click(object sender, EventArgs e)
    {
      if (this.currMITable == null)
        return;
      this.currMITable.UpdateRecord();
    }

    private void deleteBtn_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Do you want to delete the selected entry?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
        return;
      if (this.currMITable != null)
        this.currMITable.DeleteRecord();
      this.setGcMITablesTitle();
    }

    private void btnExport_Click(object sender, EventArgs e)
    {
      string path = string.Empty;
      using (SaveFileDialog saveFileDialog = new SaveFileDialog())
      {
        saveFileDialog.DefaultExt = "XML";
        saveFileDialog.Filter = "XML | *.XML";
        if (saveFileDialog.ShowDialog((IWin32Window) this) == DialogResult.OK)
          path = saveFileDialog.FileName;
      }
      if (path == string.Empty)
        return;
      LoanTypeEnum miType = LoanTypeEnum.Conventional;
      if (this.tabControlMI.SelectedTab == this.tabPageFHA || this.tabControlMI.SelectedTab == this.tabPageFHADL)
        miType = LoanTypeEnum.FHA;
      else if (this.tabControlMI.SelectedTab == this.tabPageVA || this.tabControlMI.SelectedTab == this.tabPageVADL)
        miType = LoanTypeEnum.VA;
      else if (this.tabControlMI.SelectedTab == this.tabPageUSDA || this.tabControlMI.SelectedTab == this.tabPageUSDADL)
        miType = LoanTypeEnum.USDA;
      else if (this.tabControlMI.SelectedTab == this.tabPageOther)
        miType = LoanTypeEnum.Other;
      bool isForDownload = false;
      if (this.tabControlMI.SelectedTab == this.tabPageFHADL || this.tabControlMI.SelectedTab == this.tabPageVADL || this.tabControlMI.SelectedTab == this.tabPageUSDADL)
        isForDownload = true;
      try
      {
        string tabName = string.Empty;
        if (miType == LoanTypeEnum.Conventional)
          tabName = this.tabControlConv.SelectedTab.Text;
        MIRecordXML miRecordXml = this.session.ConfigurationManager.ExportMIRecord(miType, tabName, isForDownload);
        if (miRecordXml == null)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Nothing to export.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          return;
        }
        BinaryObject binaryObject = (BinaryObject) (BinaryConvertibleObject) miRecordXml;
        using (FileStream destination = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
          binaryObject.AsStream().CopyTo((Stream) destination);
        binaryObject.Dispose();
      }
      catch (IOException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Encompass can not export MI Table to a XML file. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return;
      }
      int num1 = (int) Utils.Dialog((IWin32Window) this, "The '" + path + "' MI Table file has been saved.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }

    private void btnDuplicate_Click(object sender, EventArgs e)
    {
      if (this.currMITable != null)
        this.currMITable.DuplicateRecord();
      this.setGcMITablesTitle();
    }

    private void btnAddTab_Click(object sender, EventArgs e)
    {
      using (AddTabNameForm addTabNameForm = new AddTabNameForm(this.session, ""))
      {
        if (addTabNameForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.session.ConfigurationManager.AddMITab(addTabNameForm.NewTabName);
        this.tabControlConv.TabPages.Add(addTabNameForm.NewTabName);
        this.tabControlConv.SelectedIndex = this.tabControlConv.TabPages.Count - 1;
      }
    }

    private void btnRemoveTab_Click(object sender, EventArgs e)
    {
      if (this.tabControlConv.SelectedTab.Text == "General")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The general conventional tab cannot be removed.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (this.convTable.ListViewItemCount > 0 && Utils.Dialog((IWin32Window) this, "Tab '" + this.tabControlConv.SelectedTab.Text + "' is not empty. Are you sure you want to remove this tab and delete its MI records from database?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
          return;
        this.session.ConfigurationManager.DeleteMITab(this.tabControlConv.SelectedTab.Text);
        this.tabControlConv.TabPages.Remove(this.tabControlConv.SelectedTab);
        this.tabControlConv.SelectedIndex = 0;
      }
    }

    private void btnRenameTab_Click(object sender, EventArgs e)
    {
      if (this.tabControlConv.SelectedTab.Text == "General")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The general conventional tab cannot be renamed.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        string text = this.tabControlConv.SelectedTab.Text;
        using (AddTabNameForm addTabNameForm = new AddTabNameForm(this.session, text))
        {
          if (addTabNameForm.ShowDialog((IWin32Window) this) != DialogResult.OK || !(text != addTabNameForm.NewTabName))
            return;
          this.session.ConfigurationManager.UpdateMITab(text, addTabNameForm.NewTabName);
          this.tabControlConv.SelectedTab.Text = addTabNameForm.NewTabName;
          this.tabControlConv_SelectedIndexChanged((object) null, (EventArgs) null);
        }
      }
    }

    private void iconbtnHelp_Click(object sender, EventArgs e)
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, "MI Tables");
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
      this.tabControlMI = new TabControl();
      this.tabPageConv = new TabPage();
      this.gcConventional = new GroupContainer();
      this.stdIconBtnNewTab = new StandardIconButton();
      this.stdIconBtnRenameTab = new StandardIconButton();
      this.stdIconBtnDeleteTab = new StandardIconButton();
      this.tabControlConv = new TabControl();
      this.tabPageConvGeneral = new TabPage();
      this.gcMITables = new GroupContainer();
      this.verticalSeparator1 = new VerticalSeparator();
      this.iconbtnHelp = new IconButton();
      this.stdIconBtnNew = new StandardIconButton();
      this.stdIconBtnDuplicate = new StandardIconButton();
      this.stdIconBtnEdit = new StandardIconButton();
      this.stdIconBtnDelete = new StandardIconButton();
      this.btnExport = new Button();
      this.btnDownload = new Button();
      this.tabPageFHA = new TabPage();
      this.tabPageVA = new TabPage();
      this.tabPageUSDA = new TabPage();
      this.tabPageOther = new TabPage();
      this.tabPageFHADL = new TabPage();
      this.tabPageVADL = new TabPage();
      this.tabPageUSDADL = new TabPage();
      this.toolTip1 = new ToolTip(this.components);
      this.borderPanel1 = new BorderPanel();
      this.labelLastDownload = new Label();
      this.tabControlMI.SuspendLayout();
      this.tabPageConv.SuspendLayout();
      this.gcConventional.SuspendLayout();
      ((ISupportInitialize) this.stdIconBtnNewTab).BeginInit();
      ((ISupportInitialize) this.stdIconBtnRenameTab).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDeleteTab).BeginInit();
      this.tabControlConv.SuspendLayout();
      this.tabPageConvGeneral.SuspendLayout();
      this.gcMITables.SuspendLayout();
      ((ISupportInitialize) this.iconbtnHelp).BeginInit();
      ((ISupportInitialize) this.stdIconBtnNew).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDuplicate).BeginInit();
      ((ISupportInitialize) this.stdIconBtnEdit).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDelete).BeginInit();
      this.borderPanel1.SuspendLayout();
      this.SuspendLayout();
      this.tabControlMI.Controls.Add((Control) this.tabPageConv);
      this.tabControlMI.Controls.Add((Control) this.tabPageFHA);
      this.tabControlMI.Controls.Add((Control) this.tabPageVA);
      this.tabControlMI.Controls.Add((Control) this.tabPageUSDA);
      this.tabControlMI.Controls.Add((Control) this.tabPageOther);
      this.tabControlMI.Controls.Add((Control) this.tabPageFHADL);
      this.tabControlMI.Controls.Add((Control) this.tabPageVADL);
      this.tabControlMI.Controls.Add((Control) this.tabPageUSDADL);
      this.tabControlMI.Dock = DockStyle.Fill;
      this.tabControlMI.Location = new Point(4, 4);
      this.tabControlMI.Name = "tabControlMI";
      this.tabControlMI.SelectedIndex = 0;
      this.tabControlMI.Size = new Size(618, 512);
      this.tabControlMI.TabIndex = 0;
      this.tabControlMI.SelectedIndexChanged += new EventHandler(this.tabControlMI_SelectedIndexChanged);
      this.tabPageConv.Controls.Add((Control) this.gcConventional);
      this.tabPageConv.Location = new Point(4, 23);
      this.tabPageConv.Name = "tabPageConv";
      this.tabPageConv.Size = new Size(610, 485);
      this.tabPageConv.TabIndex = 2;
      this.tabPageConv.Text = "Conventional";
      this.tabPageConv.UseVisualStyleBackColor = true;
      this.gcConventional.Controls.Add((Control) this.stdIconBtnNewTab);
      this.gcConventional.Controls.Add((Control) this.stdIconBtnRenameTab);
      this.gcConventional.Controls.Add((Control) this.stdIconBtnDeleteTab);
      this.gcConventional.Controls.Add((Control) this.tabControlConv);
      this.gcConventional.Dock = DockStyle.Fill;
      this.gcConventional.HeaderForeColor = SystemColors.ControlText;
      this.gcConventional.Location = new Point(0, 0);
      this.gcConventional.Name = "gcConventional";
      this.gcConventional.Padding = new Padding(3, 0, 0, 0);
      this.gcConventional.Size = new Size(610, 485);
      this.gcConventional.TabIndex = 25;
      this.gcConventional.Text = "Conventional";
      this.stdIconBtnNewTab.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnNewTab.BackColor = Color.Transparent;
      this.stdIconBtnNewTab.Location = new Point(547, 4);
      this.stdIconBtnNewTab.MouseDownImage = (Image) null;
      this.stdIconBtnNewTab.Name = "stdIconBtnNewTab";
      this.stdIconBtnNewTab.Size = new Size(16, 17);
      this.stdIconBtnNewTab.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconBtnNewTab.TabIndex = 27;
      this.stdIconBtnNewTab.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnNewTab, "New Tab");
      this.stdIconBtnNewTab.Click += new EventHandler(this.btnAddTab_Click);
      this.stdIconBtnRenameTab.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnRenameTab.BackColor = Color.Transparent;
      this.stdIconBtnRenameTab.Location = new Point(569, 4);
      this.stdIconBtnRenameTab.MouseDownImage = (Image) null;
      this.stdIconBtnRenameTab.Name = "stdIconBtnRenameTab";
      this.stdIconBtnRenameTab.Size = new Size(16, 17);
      this.stdIconBtnRenameTab.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdIconBtnRenameTab.TabIndex = 28;
      this.stdIconBtnRenameTab.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnRenameTab, "Rename Tab");
      this.stdIconBtnRenameTab.Click += new EventHandler(this.btnRenameTab_Click);
      this.stdIconBtnDeleteTab.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDeleteTab.BackColor = Color.Transparent;
      this.stdIconBtnDeleteTab.Location = new Point(589, 4);
      this.stdIconBtnDeleteTab.MouseDownImage = (Image) null;
      this.stdIconBtnDeleteTab.Name = "stdIconBtnDeleteTab";
      this.stdIconBtnDeleteTab.Size = new Size(16, 17);
      this.stdIconBtnDeleteTab.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdIconBtnDeleteTab.TabIndex = 27;
      this.stdIconBtnDeleteTab.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnDeleteTab, "Delete Tab");
      this.stdIconBtnDeleteTab.Click += new EventHandler(this.btnRemoveTab_Click);
      this.tabControlConv.Controls.Add((Control) this.tabPageConvGeneral);
      this.tabControlConv.Dock = DockStyle.Fill;
      this.tabControlConv.Location = new Point(4, 26);
      this.tabControlConv.Name = "tabControlConv";
      this.tabControlConv.SelectedIndex = 0;
      this.tabControlConv.Size = new Size(605, 458);
      this.tabControlConv.TabIndex = 21;
      this.tabControlConv.SelectedIndexChanged += new EventHandler(this.tabControlConv_SelectedIndexChanged);
      this.tabPageConvGeneral.Controls.Add((Control) this.gcMITables);
      this.tabPageConvGeneral.Location = new Point(4, 23);
      this.tabPageConvGeneral.Name = "tabPageConvGeneral";
      this.tabPageConvGeneral.Padding = new Padding(3);
      this.tabPageConvGeneral.Size = new Size(597, 431);
      this.tabPageConvGeneral.TabIndex = 0;
      this.tabPageConvGeneral.Text = "General";
      this.tabPageConvGeneral.UseVisualStyleBackColor = true;
      this.gcMITables.Controls.Add((Control) this.labelLastDownload);
      this.gcMITables.Controls.Add((Control) this.verticalSeparator1);
      this.gcMITables.Controls.Add((Control) this.iconbtnHelp);
      this.gcMITables.Controls.Add((Control) this.stdIconBtnNew);
      this.gcMITables.Controls.Add((Control) this.stdIconBtnDuplicate);
      this.gcMITables.Controls.Add((Control) this.stdIconBtnEdit);
      this.gcMITables.Controls.Add((Control) this.stdIconBtnDelete);
      this.gcMITables.Controls.Add((Control) this.btnExport);
      this.gcMITables.Controls.Add((Control) this.btnDownload);
      this.gcMITables.Dock = DockStyle.Fill;
      this.gcMITables.HeaderForeColor = SystemColors.ControlText;
      this.gcMITables.Location = new Point(3, 3);
      this.gcMITables.Name = "gcMITables";
      this.gcMITables.Size = new Size(591, 425);
      this.gcMITables.TabIndex = 21;
      this.gcMITables.Text = "MI Tables (0)";
      this.verticalSeparator1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.verticalSeparator1.Location = new Point(542, 4);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 32;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.iconbtnHelp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.iconbtnHelp.BackColor = Color.Transparent;
      this.iconbtnHelp.DisabledImage = (Image) null;
      this.iconbtnHelp.Image = (Image) Resources.help;
      this.iconbtnHelp.Location = new Point(569, 5);
      this.iconbtnHelp.MouseDownImage = (Image) null;
      this.iconbtnHelp.MouseOverImage = (Image) Resources.help_over;
      this.iconbtnHelp.Name = "iconbtnHelp";
      this.iconbtnHelp.Size = new Size(16, 17);
      this.iconbtnHelp.TabIndex = 31;
      this.iconbtnHelp.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.iconbtnHelp, "Help");
      this.iconbtnHelp.Click += new EventHandler(this.iconbtnHelp_Click);
      this.stdIconBtnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnNew.BackColor = Color.Transparent;
      this.stdIconBtnNew.Location = new Point(481, 5);
      this.stdIconBtnNew.MouseDownImage = (Image) null;
      this.stdIconBtnNew.Name = "stdIconBtnNew";
      this.stdIconBtnNew.Size = new Size(16, 17);
      this.stdIconBtnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconBtnNew.TabIndex = 27;
      this.stdIconBtnNew.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnNew, "New");
      this.stdIconBtnNew.Click += new EventHandler(this.addBtn_Click);
      this.stdIconBtnDuplicate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDuplicate.BackColor = Color.Transparent;
      this.stdIconBtnDuplicate.Location = new Point(503, 5);
      this.stdIconBtnDuplicate.MouseDownImage = (Image) null;
      this.stdIconBtnDuplicate.Name = "stdIconBtnDuplicate";
      this.stdIconBtnDuplicate.Size = new Size(16, 17);
      this.stdIconBtnDuplicate.StandardButtonType = StandardIconButton.ButtonType.DuplicateButton;
      this.stdIconBtnDuplicate.TabIndex = 28;
      this.stdIconBtnDuplicate.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnDuplicate, "Duplicate");
      this.stdIconBtnDuplicate.Click += new EventHandler(this.btnDuplicate_Click);
      this.stdIconBtnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnEdit.BackColor = Color.Transparent;
      this.stdIconBtnEdit.Location = new Point(525, 5);
      this.stdIconBtnEdit.MouseDownImage = (Image) null;
      this.stdIconBtnEdit.Name = "stdIconBtnEdit";
      this.stdIconBtnEdit.Size = new Size(16, 17);
      this.stdIconBtnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdIconBtnEdit.TabIndex = 29;
      this.stdIconBtnEdit.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnEdit, "Edit");
      this.stdIconBtnEdit.Click += new EventHandler(this.editBtn_Click);
      this.stdIconBtnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDelete.BackColor = Color.Transparent;
      this.stdIconBtnDelete.Location = new Point(547, 5);
      this.stdIconBtnDelete.MouseDownImage = (Image) null;
      this.stdIconBtnDelete.Name = "stdIconBtnDelete";
      this.stdIconBtnDelete.Size = new Size(16, 17);
      this.stdIconBtnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdIconBtnDelete.TabIndex = 26;
      this.stdIconBtnDelete.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnDelete, "Delete");
      this.stdIconBtnDelete.Click += new EventHandler(this.deleteBtn_Click);
      this.btnExport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExport.BackColor = SystemColors.Control;
      this.btnExport.Location = new Point(394, 2);
      this.btnExport.Name = "btnExport";
      this.btnExport.Size = new Size(65, 22);
      this.btnExport.TabIndex = 18;
      this.btnExport.Text = "E&xport";
      this.btnExport.UseVisualStyleBackColor = true;
      this.btnExport.Click += new EventHandler(this.btnExport_Click);
      this.btnDownload.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDownload.BackColor = SystemColors.Control;
      this.btnDownload.Location = new Point(462, 2);
      this.btnDownload.Name = "btnDownload";
      this.btnDownload.Size = new Size(75, 22);
      this.btnDownload.TabIndex = 30;
      this.btnDownload.Text = "Download";
      this.btnDownload.UseVisualStyleBackColor = true;
      this.btnDownload.Visible = false;
      this.btnDownload.Click += new EventHandler(this.btnDownload_Click);
      this.tabPageFHA.Location = new Point(4, 23);
      this.tabPageFHA.Name = "tabPageFHA";
      this.tabPageFHA.Size = new Size(610, 485);
      this.tabPageFHA.TabIndex = 4;
      this.tabPageFHA.Text = "FHA";
      this.tabPageFHA.UseVisualStyleBackColor = true;
      this.tabPageVA.Location = new Point(4, 23);
      this.tabPageVA.Name = "tabPageVA";
      this.tabPageVA.Size = new Size(610, 485);
      this.tabPageVA.TabIndex = 5;
      this.tabPageVA.Text = "VA";
      this.tabPageVA.UseVisualStyleBackColor = true;
      this.tabPageUSDA.Location = new Point(4, 23);
      this.tabPageUSDA.Name = "tabPageUSDA";
      this.tabPageUSDA.Size = new Size(610, 485);
      this.tabPageUSDA.TabIndex = 7;
      this.tabPageUSDA.Text = "USDA";
      this.tabPageUSDA.UseVisualStyleBackColor = true;
      this.tabPageOther.Location = new Point(4, 23);
      this.tabPageOther.Name = "tabPageOther";
      this.tabPageOther.Size = new Size(610, 485);
      this.tabPageOther.TabIndex = 3;
      this.tabPageOther.Text = "Other";
      this.tabPageOther.UseVisualStyleBackColor = true;
      this.tabPageFHADL.Location = new Point(4, 23);
      this.tabPageFHADL.Name = "tabPageFHADL";
      this.tabPageFHADL.Padding = new Padding(3);
      this.tabPageFHADL.Size = new Size(610, 485);
      this.tabPageFHADL.TabIndex = 0;
      this.tabPageFHADL.Text = "FHA (Download)";
      this.tabPageFHADL.UseVisualStyleBackColor = true;
      this.tabPageVADL.Location = new Point(4, 23);
      this.tabPageVADL.Name = "tabPageVADL";
      this.tabPageVADL.Padding = new Padding(3);
      this.tabPageVADL.Size = new Size(610, 485);
      this.tabPageVADL.TabIndex = 1;
      this.tabPageVADL.Text = "VA (Download)";
      this.tabPageVADL.UseVisualStyleBackColor = true;
      this.tabPageUSDADL.Location = new Point(4, 23);
      this.tabPageUSDADL.Name = "tabPageUSDADL";
      this.tabPageUSDADL.Size = new Size(610, 485);
      this.tabPageUSDADL.TabIndex = 6;
      this.tabPageUSDADL.Text = "USDA (Download)";
      this.tabPageUSDADL.UseVisualStyleBackColor = true;
      this.borderPanel1.Controls.Add((Control) this.tabControlMI);
      this.borderPanel1.Dock = DockStyle.Fill;
      this.borderPanel1.Location = new Point(0, 0);
      this.borderPanel1.Name = "borderPanel1";
      this.borderPanel1.Padding = new Padding(3, 3, 0, 0);
      this.borderPanel1.Size = new Size(623, 517);
      this.borderPanel1.TabIndex = 1;
      this.labelLastDownload.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.labelLastDownload.AutoSize = true;
      this.labelLastDownload.BackColor = Color.Transparent;
      this.labelLastDownload.Location = new Point(186, 6);
      this.labelLastDownload.Name = "labelLastDownload";
      this.labelLastDownload.Size = new Size(202, 14);
      this.labelLastDownload.TabIndex = 33;
      this.labelLastDownload.Text = "Last Download: 07/01/2013 12:00:00 pm";
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.Controls.Add((Control) this.borderPanel1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (MITableSetupPanel);
      this.Size = new Size(623, 517);
      this.tabControlMI.ResumeLayout(false);
      this.tabPageConv.ResumeLayout(false);
      this.gcConventional.ResumeLayout(false);
      ((ISupportInitialize) this.stdIconBtnNewTab).EndInit();
      ((ISupportInitialize) this.stdIconBtnRenameTab).EndInit();
      ((ISupportInitialize) this.stdIconBtnDeleteTab).EndInit();
      this.tabControlConv.ResumeLayout(false);
      this.tabPageConvGeneral.ResumeLayout(false);
      this.gcMITables.ResumeLayout(false);
      this.gcMITables.PerformLayout();
      ((ISupportInitialize) this.iconbtnHelp).EndInit();
      ((ISupportInitialize) this.stdIconBtnNew).EndInit();
      ((ISupportInitialize) this.stdIconBtnDuplicate).EndInit();
      ((ISupportInitialize) this.stdIconBtnEdit).EndInit();
      ((ISupportInitialize) this.stdIconBtnDelete).EndInit();
      this.borderPanel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
