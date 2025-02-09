// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ePass.Services.EpassServicesControl
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
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
using System.Threading;
using System.Web;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.ePass.Services
{
  public class EpassServicesControl : UserControl
  {
    private const string className = "EpassServicesControl";
    private static readonly string sw = Tracing.SwEpass;
    private EpassServicesControl.LoadCategoryListDelegate loadCategoryListDelegate;
    private EpassCategoryControl allCategory;
    private EpassCategoryControl customCategory;
    private EpassCategoryControl[] categoryList;
    private LoanData loanData;
    private IContainer components;
    private Panel pnlSeparator;
    private GradientPanel pnlOptions;
    private Panel pnlCategories;
    private CheckBox chkAlpha;
    private System.Windows.Forms.Timer tmrRefresh;

    public EpassServicesControl()
    {
      this.InitializeComponent();
      this.loadCategoryListDelegate = new EpassServicesControl.LoadCategoryListDelegate(this.loadCategoryList);
    }

    private void LoanDataMgr_OnEPCIntegrationClose(object sender, EventArgs e)
    {
      Tracing.Log(EpassServicesControl.sw, TraceLevel.Info, nameof (EpassServicesControl), "Refreshing category list on EPC integration close.");
      this.refreshCategoryList();
    }

    public void InitializeContents()
    {
      Session.LoanDataMgr.OnEPCIntegrationClose += new EventHandler(this.LoanDataMgr_OnEPCIntegrationClose);
      this.allCategory = new EpassCategoryControl("All Services", "All", "View", "_EPASS_SIGNATURE;EPASSAI;2;All+Services");
      this.customCategory = new EpassCategoryControl("My Custom Links", (string) null, "View", "_EPASS_SIGNATURE;EPASSAI;2;Custom+Links");
      this.categoryList = new EpassCategoryControl[2]
      {
        this.allCategory,
        this.customCategory
      };
      this.loadCategoryList();
      Tracing.Log(EpassServicesControl.sw, TraceLevel.Verbose, nameof (EpassServicesControl), "Starting Thread");
      new Thread(new ThreadStart(this.downloadCategoryList))
      {
        IsBackground = true
      }.Start();
    }

    public void InitializeLoan(LoanDataMgr loanDataMgr)
    {
      this.ReleaseLoan();
      this.loanData = loanDataMgr.LoanData;
      this.loanData.LogRecordAdded += new LogRecordEventHandler(this.logRecordChanged);
      this.loanData.LogRecordChanged += new LogRecordEventHandler(this.logRecordChanged);
      this.loanData.LogRecordRemoved += new LogRecordEventHandler(this.logRecordChanged);
      Session.LoanDataMgr.OnEPCIntegrationClose += new EventHandler(this.LoanDataMgr_OnEPCIntegrationClose);
      this.refreshCategoryList();
    }

    public void ReleaseLoan()
    {
      foreach (EpassCategoryControl category in this.categoryList)
        category.RefreshContents(new DocumentLog[0]);
      if (this.loanData == null)
        return;
      this.loanData.LogRecordAdded -= new LogRecordEventHandler(this.logRecordChanged);
      this.loanData.LogRecordChanged -= new LogRecordEventHandler(this.logRecordChanged);
      this.loanData.LogRecordRemoved -= new LogRecordEventHandler(this.logRecordChanged);
      this.loanData = (LoanData) null;
    }

    public ToolStripMenuItem[] GetMenuItems()
    {
      EpassCategoryControl[] categoryList = this.getCategoryList();
      List<ToolStripMenuItem> toolStripMenuItemList = new List<ToolStripMenuItem>();
      foreach (EpassCategoryControl epassCategoryControl in categoryList)
        toolStripMenuItemList.Add(epassCategoryControl.MenuItem);
      return toolStripMenuItemList.ToArray();
    }

    public bool IsEncompassSelfHosted => EpassLogin.IsEncompassSelfHosted;

    private void downloadCategoryList()
    {
      try
      {
        string requestUriString = "https://www.epassbusinesscenter.com/epassai/getallservices.asp";
        string str1 = "https://core.elliemaeservices.com/epassai/getallservices.asp";
        string str2 = "version=" + HttpUtility.UrlEncode(EpassLogin.EpassVersion(Session.DefaultInstance));
        XmlDocument xmlDocument = new XmlDocument();
        int num1 = 2;
        int num2 = 1;
        for (bool flag = false; !flag && num2 <= num1; ++num2)
        {
          if (num2 == 2)
          {
            requestUriString = str1;
            Tracing.Log(EpassServicesControl.sw, TraceLevel.Info, nameof (EpassServicesControl), "Calling HA URL");
          }
          flag = true;
          try
          {
            Tracing.Log(EpassServicesControl.sw, TraceLevel.Info, nameof (EpassServicesControl), "Get ePASS Services");
            HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(requestUriString);
            httpWebRequest.KeepAlive = false;
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.ContentLength = (long) str2.Length;
            httpWebRequest.Timeout = 5000;
            using (Stream requestStream = httpWebRequest.GetRequestStream())
            {
              using (StreamWriter streamWriter = new StreamWriter(requestStream))
                streamWriter.Write(str2);
            }
            using (WebResponse response = httpWebRequest.GetResponse())
            {
              using (Stream responseStream = response.GetResponseStream())
              {
                using (StreamReader streamReader = new StreamReader(responseStream))
                  xmlDocument.LoadXml(streamReader.ReadToEnd());
              }
            }
          }
          catch (Exception ex)
          {
            Tracing.Log(EpassServicesControl.sw, TraceLevel.Error, nameof (EpassServicesControl), ex.ToString());
            flag = false;
            if (num2 == num1)
              throw ex;
          }
        }
        List<EpassCategoryControl> epassCategoryControlList = new List<EpassCategoryControl>();
        foreach (XmlElement selectNode in xmlDocument.DocumentElement.SelectNodes("S"))
        {
          EpassCategoryControl epassCategoryControl = new EpassCategoryControl(selectNode.GetAttribute("Title"), selectNode.GetAttribute("DocType"), selectNode.GetAttribute("Button"), selectNode.GetAttribute("Url"));
          epassCategoryControlList.Add(epassCategoryControl);
        }
        epassCategoryControlList.Add(this.customCategory);
        this.BeginInvoke((Delegate) this.loadCategoryListDelegate, (object) epassCategoryControlList.ToArray());
      }
      catch (Exception ex)
      {
        Tracing.Log(EpassServicesControl.sw, TraceLevel.Error, nameof (EpassServicesControl), ex.ToString());
      }
    }

    private EpassCategoryControl[] getCategoryList()
    {
      EpassCategoryControl[] categoryList = this.categoryList;
      if (this.chkAlpha.Checked)
      {
        List<string> stringList = new List<string>();
        foreach (EpassCategoryControl epassCategoryControl in categoryList)
          stringList.Add(epassCategoryControl.Title);
        stringList.Sort();
        List<EpassCategoryControl> epassCategoryControlList = new List<EpassCategoryControl>();
        foreach (string str in stringList)
        {
          foreach (EpassCategoryControl epassCategoryControl in categoryList)
          {
            if (epassCategoryControl.Title == str)
              epassCategoryControlList.Add(epassCategoryControl);
          }
        }
        categoryList = epassCategoryControlList.ToArray();
      }
      return categoryList;
    }

    private void loadCategoryList(EpassCategoryControl[] categoryList)
    {
      if (this.categoryList != null)
      {
        foreach (EpassCategoryControl category in this.categoryList)
          category.RefreshContents(new DocumentLog[0]);
      }
      this.categoryList = categoryList;
      this.loadCategoryList();
    }

    private void loadCategoryList()
    {
      this.pnlCategories.SuspendLayout();
      this.pnlCategories.Controls.Clear();
      foreach (EpassCategoryControl category in this.getCategoryList())
      {
        this.pnlCategories.Controls.Add((Control) category);
        category.Dock = DockStyle.Top;
        category.BringToFront();
      }
      this.refreshCategoryList();
      this.pnlCategories.ResumeLayout();
    }

    private void refreshCategoryList()
    {
      this.tmrRefresh.Stop();
      if (this.loanData == null)
        return;
      DocumentLog[] allDocuments = this.loanData.GetLogList().GetAllDocuments();
      DocumentLog[] array = ((IEnumerable<DocumentLog>) this.loanData.GetLogList().GetAllDocuments(false, true)).Except<DocumentLog>((IEnumerable<DocumentLog>) allDocuments).ToArray<DocumentLog>();
      foreach (EpassCategoryControl category in this.categoryList)
        category.RefreshContents(allDocuments, array);
    }

    private void chkAlpha_Click(object sender, EventArgs e) => this.loadCategoryList();

    private void logRecordChanged(object source, LogRecordEventArgs e)
    {
      Tracing.Log(EpassServicesControl.sw, TraceLevel.Verbose, nameof (EpassServicesControl), "Checking InvokeRequired For LogRecordEventHandler");
      if (this.InvokeRequired)
      {
        LogRecordEventHandler method = new LogRecordEventHandler(this.logRecordChanged);
        Tracing.Log(EpassServicesControl.sw, TraceLevel.Verbose, nameof (EpassServicesControl), "Calling BeginInvoke For LogRecordEventHandler");
        this.BeginInvoke((Delegate) method, source, (object) e);
      }
      else
      {
        if (!(e.LogRecord is DocumentLog))
          return;
        this.tmrRefresh.Stop();
        this.tmrRefresh.Start();
      }
    }

    private void tmrRefresh_Tick(object sender, EventArgs e) => this.refreshCategoryList();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      this.pnlSeparator = new Panel();
      this.pnlOptions = new GradientPanel();
      this.chkAlpha = new CheckBox();
      this.pnlCategories = new Panel();
      this.tmrRefresh = new System.Windows.Forms.Timer(this.components);
      this.pnlOptions.SuspendLayout();
      this.SuspendLayout();
      this.pnlSeparator.BackColor = Color.FromArgb(200, 199, 199);
      this.pnlSeparator.Dock = DockStyle.Bottom;
      this.pnlSeparator.Location = new Point(0, 289);
      this.pnlSeparator.Name = "pnlSeparator";
      this.pnlSeparator.Size = new Size(335, 1);
      this.pnlSeparator.TabIndex = 1;
      this.pnlOptions.Borders = AnchorStyles.None;
      this.pnlOptions.Controls.Add((Control) this.chkAlpha);
      this.pnlOptions.Dock = DockStyle.Bottom;
      this.pnlOptions.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.pnlOptions.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.pnlOptions.Location = new Point(0, 290);
      this.pnlOptions.Name = "pnlOptions";
      this.pnlOptions.Padding = new Padding(8, 0, 0, 0);
      this.pnlOptions.Size = new Size(335, 24);
      this.pnlOptions.Style = GradientPanel.PanelStyle.TableFooter;
      this.pnlOptions.TabIndex = 6;
      this.chkAlpha.AutoSize = true;
      this.chkAlpha.BackColor = Color.Transparent;
      this.chkAlpha.Location = new Point(4, 3);
      this.chkAlpha.Name = "chkAlpha";
      this.chkAlpha.Size = new Size(128, 18);
      this.chkAlpha.TabIndex = 0;
      this.chkAlpha.Text = "Show in Alpha Order";
      this.chkAlpha.UseVisualStyleBackColor = false;
      this.chkAlpha.Click += new EventHandler(this.chkAlpha_Click);
      this.pnlCategories.AutoScroll = true;
      this.pnlCategories.AutoScrollMargin = new Size(0, 1);
      this.pnlCategories.BackColor = Color.WhiteSmoke;
      this.pnlCategories.Dock = DockStyle.Fill;
      this.pnlCategories.Location = new Point(0, 0);
      this.pnlCategories.Name = "pnlCategories";
      this.pnlCategories.Size = new Size(335, 289);
      this.pnlCategories.TabIndex = 8;
      this.tmrRefresh.Interval = 250;
      this.tmrRefresh.Tick += new EventHandler(this.tmrRefresh_Tick);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.pnlCategories);
      this.Controls.Add((Control) this.pnlSeparator);
      this.Controls.Add((Control) this.pnlOptions);
      this.DoubleBuffered = true;
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (EpassServicesControl);
      this.Size = new Size(335, 314);
      this.pnlOptions.ResumeLayout(false);
      this.pnlOptions.PerformLayout();
      this.ResumeLayout(false);
    }

    private delegate void LoadCategoryListDelegate(EpassCategoryControl[] categoryList);
  }
}
