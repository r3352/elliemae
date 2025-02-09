// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.DocOrderLogWS
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.DocEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.JedScriptEngine;
using EllieMae.EMLite.LoanUtils.SkyDrive;
using EllieMae.EMLite.Properties;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class DocOrderLogWS : UserControl, IOnlineHelpTarget
  {
    private DocumentOrderLog orderLog;
    private ECloseLog ecloseLog;
    private string reportTempPath;
    private Sessions.Session session;
    private IContainer components;
    private GroupContainer grpDocuments;
    protected GridView gvDocuments;
    protected GroupContainer grpDetails;
    protected TextBox txtDateOrdered;
    protected Label lblAlertName;
    protected TextBox txtOrderedBy;
    protected Label label1;
    private FlowLayoutPanel flowLayoutPanel2;
    private Button btnView;
    private GroupContainer grpAudit;
    private GradientPanel pnlAuditOptions;
    protected GridView gvAudit;
    private FlowLayoutPanel pnlAuditControls;
    private RadioButton radComplianceAudit;
    private RadioButton radDocAudit;
    private IconButton btnAuditReport;
    private GroupContainer grpRecipients;
    protected GridView gvRecipients;
    private FlowLayoutPanel flowLayoutPanel1;
    private SplitContainer splitContainer1;
    private SplitContainer splitContainer2;

    public DocOrderLogWS(Sessions.Session session, DocumentOrderLog orderLog)
    {
      this.orderLog = orderLog;
      this.InitializeComponent();
      this.session = session;
      this.refreshOrderDetails();
      this.gvAudit.Sort(0, SortOrder.Ascending);
      this.radDocAudit.Checked = true;
      if (this.orderLog.OrderType == DocumentOrderType.Opening)
        this.pnlAuditOptions.Visible = false;
      else if (this.orderLog.Audit != null && this.orderLog.Audit.GetAlertsBySource("DocEngine").Length == 0 && this.orderLog.Audit.GetAlertsBySource("Compliance").Length != 0)
        this.radComplianceAudit.Checked = true;
      this.Dock = DockStyle.Fill;
    }

    public DocOrderLogWS(Sessions.Session session, ECloseLog ecloseLog)
    {
      this.ecloseLog = ecloseLog;
      this.InitializeComponent();
      this.session = session;
      this.refresheCloseLogDetails();
      this.pnlAuditOptions.Visible = true;
      this.gvAudit.Sort(0, SortOrder.Ascending);
      this.radDocAudit.Checked = true;
      if (this.ecloseLog.Audit != null)
      {
        if (this.ecloseLog.Audit.GetAlertsBySource("DocEngine").Length == 0 && this.ecloseLog.Audit.GetAlertsBySource("Compliance").Length != 0)
          this.radComplianceAudit.Checked = true;
        if (ecloseLog.Audit.FileKey != null && ecloseLog.Audit.FileKey.Length > 0)
          this.btnAuditReport.Visible = true;
      }
      this.Dock = DockStyle.Fill;
    }

    private void refresheCloseLogDetails()
    {
      this.grpDetails.Text = "eClose Ordered";
      this.txtDateOrdered.Text = this.ecloseLog.Date.ToString("M/d/yyyy h:mm tt");
      this.txtOrderedBy.Text = this.ecloseLog.UserID;
      this.gvDocuments.Items.Clear();
      foreach (ECloseLog.Document document in (IEnumerable<ECloseLog.Document>) this.ecloseLog.Documents)
        this.gvDocuments.Items.Add(this.createGVItemForDocument(document));
      this.gvRecipients.Items.Clear();
      foreach (ECloseLog.Party party in (IEnumerable<ECloseLog.Party>) this.ecloseLog.Parties)
        this.gvRecipients.Items.Add(this.createGVItemForRecipients(party));
      this.splitContainer1.Panel1Collapsed = true;
      this.splitContainer1.Panel1Collapsed = false;
      this.grpDocuments.Text = "Documents (" + (object) this.gvDocuments.Items.Count + ")";
      if (this.ecloseLog.Audit == null)
      {
        this.grpAudit.Visible = false;
      }
      else
      {
        this.grpAudit.Text = "Audited on " + this.ecloseLog.Audit.AuditTime.ToString("M/d/yyyy h:mm tt");
        this.radDocAudit.Text = "Data Audit Results";
      }
    }

    private void refreshOrderDetails()
    {
      this.grpDetails.Text = this.orderLog.OrderType != DocumentOrderType.Closing ? "eDisclosures Ordered" : "Closing Documents Ordered";
      this.splitContainer1.Panel1Collapsed = true;
      this.txtDateOrdered.Text = this.orderLog.Date.ToString("M/d/yyyy h:mm tt");
      this.txtOrderedBy.Text = this.orderLog.OrderedByUserID;
      this.gvDocuments.Items.Clear();
      foreach (OrderedDocument document in this.orderLog.Documents)
        this.gvDocuments.Items.Add(this.createGVItemForDocument(document));
      if (this.orderLog.DateFilesPurged != DateTime.MinValue)
        this.btnView.Visible = false;
      this.grpDocuments.Text = "Documents (" + (object) this.gvDocuments.Items.Count + ")";
      if (this.orderLog.Audit == null)
      {
        this.grpAudit.Visible = false;
      }
      else
      {
        this.grpAudit.Text = "Audited on " + this.orderLog.Audit.AuditDateTime.ToString("M/d/yyyy h:mm tt");
        this.radDocAudit.Text = "Data Audit Results (" + (object) this.orderLog.Audit.GetAlertsBySource("DocEngine").Length + ")";
        this.radComplianceAudit.Text = "Compliance Review Results (" + (object) this.orderLog.Audit.GetAlertsBySource("Compliance").Length + ")";
        this.radComplianceAudit.Left = this.radDocAudit.Right + 8;
      }
    }

    private GVItem createGVItemForDocument(OrderedDocument doc)
    {
      return new GVItem()
      {
        SubItems = {
          [0] = {
            Text = doc.Title
          },
          [1] = {
            Text = doc.DocumentType
          }
        },
        Tag = (object) doc
      };
    }

    private GVItem createGVItemForDocument(ECloseLog.Document doc)
    {
      return new GVItem()
      {
        SubItems = {
          [0] = {
            Text = doc.Title
          },
          [1] = {
            Text = doc.Type
          }
        },
        Tag = (object) doc.FileKey
      };
    }

    private GVItem createGVItemForRecipients(ECloseLog.Party recipient)
    {
      return new GVItem()
      {
        SubItems = {
          [0] = {
            Text = recipient.FullName
          },
          [1] = {
            Text = recipient.Type
          }
        },
        Tag = (object) recipient.Id
      };
    }

    private void btnView_Click(object sender, EventArgs e)
    {
      if (this.orderLog != null)
      {
        Session.Application.GetService<ILoanServices>().ViewClosingDocs(this.orderLog.Guid);
        this.refreshOrderDetails();
      }
      else
      {
        if (this.ecloseLog == null)
          return;
        if (this.gvDocuments.SelectedItems == null || this.gvDocuments.SelectedItems.Count == 0)
        {
          int num1 = (int) MessageBox.Show("Please select document to view.");
        }
        else
        {
          string filekey = this.gvDocuments.SelectedItems[0].Tag.ToString();
          if (filekey.Length > 0)
          {
            this.downloadECloseLogDocs(filekey);
          }
          else
          {
            int num2 = (int) MessageBox.Show("Document selected to view does not have available attachment.");
          }
          this.refresheCloseLogDetails();
        }
      }
    }

    private void radDocAudit_CheckedChanged(object sender, EventArgs e)
    {
      if (this.orderLog != null)
      {
        this.populateAuditsForClosingDocOrder();
      }
      else
      {
        if (this.ecloseLog == null)
          return;
        this.populateAuditsForECloseLog();
      }
    }

    private void populateAuditsForECloseLog()
    {
      if (this.ecloseLog.Audit == null)
        return;
      this.pnlAuditControls.Visible = this.radComplianceAudit.Checked && !string.IsNullOrEmpty(this.ecloseLog.Audit.FileKey);
      this.gvAudit.Items.Clear();
      string str = this.radComplianceAudit.Checked ? "Compliance" : "DocEngine";
      foreach (Alert alert in (IEnumerable<Alert>) this.ecloseLog.Audit.Alerts)
      {
        if (alert.Source == str)
          this.gvAudit.Items.Add(new GVItem()
          {
            ForeColor = DocOrderLogWS.getAlertTypeColor(alert.Type),
            SubItems = {
              [0] = {
                Text = DocOrderLogWS.getAlertTypeText(alert.Type),
                SortValue = (object) (alert.Type == "Fatal" ? 0 : 1)
              },
              [1] = {
                Text = alert.Text
              }
            }
          });
      }
      this.gvAudit.ReSort();
    }

    private void populateAuditsForClosingDocOrder()
    {
      if (this.orderLog.Audit == null)
        return;
      this.pnlAuditControls.Visible = this.radComplianceAudit.Checked && !string.IsNullOrEmpty(this.orderLog.Audit.ReportFileKey);
      this.gvAudit.Items.Clear();
      string str = this.radComplianceAudit.Checked ? "Compliance" : "DocEngine";
      foreach (DocumentAudit.AuditAlert alert in this.orderLog.Audit.Alerts)
      {
        if (alert.Source == str)
          this.gvAudit.Items.Add(new GVItem()
          {
            ForeColor = DocOrderLogWS.getAlertTypeColor(alert.AlertType),
            SubItems = {
              [0] = {
                Text = DocOrderLogWS.getAlertTypeText(alert.AlertType),
                SortValue = (object) (alert.AlertType == "Fatal" ? 0 : 1)
              },
              [1] = {
                Text = alert.Text
              }
            }
          });
      }
      this.gvAudit.ReSort();
    }

    private static string getAlertTypeText(string alertType)
    {
      switch (alertType)
      {
        case "Fatal":
          return "Required";
        case "Warning":
          return "Recommended";
        default:
          return alertType;
      }
    }

    private static Color getAlertTypeColor(string alertType)
    {
      return alertType == "Fatal" ? EncompassColors.Alert2 : Color.Black;
    }

    private void btnAuditReport_Click(object sender, EventArgs e)
    {
      if (this.reportTempPath == null)
        this.downloadAuditReport();
      if (this.reportTempPath == null)
        return;
      Process.Start(this.reportTempPath);
    }

    private void downloadAuditReport()
    {
      string str = Path.Combine(SystemSettings.TempFolderRoot, "OutputPdf");
      string path2 = "";
      if (this.orderLog != null)
        path2 = this.orderLog.Audit.ReportFileKey;
      else if (this.ecloseLog != null)
        path2 = this.ecloseLog.Audit.FileKey;
      using (ProgressDialog progressDialog = new ProgressDialog("Encompass", new AsynchronousProcess(new BinaryObjectDownloader(str, new string[1]
      {
        path2
      }, new BinaryObjectDownloader.ObjectRetriever(this.downloadSupportingDoc)).DownloadFiles), (object) null, false))
      {
        if (progressDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.reportTempPath = Path.Combine(str, path2);
      }
    }

    private void downloadECloseLogDocs(string filekey)
    {
      string str = Path.Combine(Path.Combine(SystemSettings.TempFolderRoot, "OutputPdf"), filekey);
      SkyDriveStreamingClient driveStreamingClient = new SkyDriveStreamingClient(this.session.LoanDataMgr);
      try
      {
        Task<BinaryObject> supportingData = driveStreamingClient.GetSupportingData(filekey);
        Task.WaitAll((Task) supportingData);
        if (supportingData != null && supportingData.Result != null)
        {
          using (supportingData.Result)
            supportingData.Result.Write(str);
        }
        int num = (int) new PdfPreviewDialog(str, true, true, false).ShowDialog((IWin32Window) this);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("An error occurred while downloading the document." + ex.Message);
      }
    }

    private BinaryObject downloadSupportingDoc(string fileKey)
    {
      return Session.LoanDataMgr.GetSupportingData(fileKey);
    }

    private void deleteReportPdf()
    {
      try
      {
        if (this.reportTempPath == null)
          return;
        File.Delete(this.reportTempPath);
      }
      catch
      {
      }
    }

    public string GetHelpTargetName() => "DocOrderWS";

    protected override void Dispose(bool disposing)
    {
      if (disposing)
        this.deleteReportPdf();
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
      this.grpDocuments = new GroupContainer();
      this.gvDocuments = new GridView();
      this.flowLayoutPanel2 = new FlowLayoutPanel();
      this.btnView = new Button();
      this.grpDetails = new GroupContainer();
      this.txtOrderedBy = new TextBox();
      this.label1 = new Label();
      this.txtDateOrdered = new TextBox();
      this.lblAlertName = new Label();
      this.grpAudit = new GroupContainer();
      this.gvAudit = new GridView();
      this.pnlAuditOptions = new GradientPanel();
      this.radComplianceAudit = new RadioButton();
      this.radDocAudit = new RadioButton();
      this.pnlAuditControls = new FlowLayoutPanel();
      this.btnAuditReport = new IconButton();
      this.grpRecipients = new GroupContainer();
      this.gvRecipients = new GridView();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.splitContainer1 = new SplitContainer();
      this.splitContainer2 = new SplitContainer();
      this.grpDocuments.SuspendLayout();
      this.flowLayoutPanel2.SuspendLayout();
      this.grpDetails.SuspendLayout();
      this.grpAudit.SuspendLayout();
      this.pnlAuditOptions.SuspendLayout();
      this.pnlAuditControls.SuspendLayout();
      ((ISupportInitialize) this.btnAuditReport).BeginInit();
      this.grpRecipients.SuspendLayout();
      this.splitContainer1.BeginInit();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.splitContainer2.BeginInit();
      this.splitContainer2.Panel1.SuspendLayout();
      this.splitContainer2.Panel2.SuspendLayout();
      this.splitContainer2.SuspendLayout();
      this.SuspendLayout();
      this.grpDocuments.Controls.Add((Control) this.gvDocuments);
      this.grpDocuments.Controls.Add((Control) this.flowLayoutPanel2);
      this.grpDocuments.Dock = DockStyle.Fill;
      this.grpDocuments.HeaderForeColor = SystemColors.ControlText;
      this.grpDocuments.Location = new Point(0, 0);
      this.grpDocuments.Name = "grpDocuments";
      this.grpDocuments.Size = new Size(703, 292);
      this.grpDocuments.TabIndex = 3;
      this.grpDocuments.Text = "Documents";
      this.gvDocuments.AllowMultiselect = false;
      this.gvDocuments.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.SpringToFit = true;
      gvColumn1.Text = "Title";
      gvColumn1.Width = 551;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Type";
      gvColumn2.Width = 150;
      this.gvDocuments.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvDocuments.Dock = DockStyle.Fill;
      this.gvDocuments.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvDocuments.Location = new Point(1, 26);
      this.gvDocuments.Name = "gvDocuments";
      this.gvDocuments.Size = new Size(701, 265);
      this.gvDocuments.TabIndex = 2;
      this.flowLayoutPanel2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel2.AutoSize = true;
      this.flowLayoutPanel2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.flowLayoutPanel2.BackColor = Color.Transparent;
      this.flowLayoutPanel2.Controls.Add((Control) this.btnView);
      this.flowLayoutPanel2.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel2.Location = new Point(619, 2);
      this.flowLayoutPanel2.Name = "flowLayoutPanel2";
      this.flowLayoutPanel2.Size = new Size(80, 22);
      this.flowLayoutPanel2.TabIndex = 1;
      this.btnView.Location = new Point(3, 0);
      this.btnView.Margin = new Padding(3, 0, 2, 0);
      this.btnView.Name = "btnView";
      this.btnView.Size = new Size(75, 22);
      this.btnView.TabIndex = 0;
      this.btnView.Text = "&View";
      this.btnView.UseVisualStyleBackColor = true;
      this.btnView.Click += new EventHandler(this.btnView_Click);
      this.grpDetails.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.grpDetails.Controls.Add((Control) this.txtOrderedBy);
      this.grpDetails.Controls.Add((Control) this.label1);
      this.grpDetails.Controls.Add((Control) this.txtDateOrdered);
      this.grpDetails.Controls.Add((Control) this.lblAlertName);
      this.grpDetails.Dock = DockStyle.Top;
      this.grpDetails.ForeColor = SystemColors.ControlText;
      this.grpDetails.HeaderForeColor = SystemColors.ControlText;
      this.grpDetails.Location = new Point(0, 0);
      this.grpDetails.Name = "grpDetails";
      this.grpDetails.Size = new Size(703, 67);
      this.grpDetails.TabIndex = 2;
      this.grpDetails.Text = "Closing Documents Ordered";
      this.txtOrderedBy.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtOrderedBy.Location = new Point(326, 36);
      this.txtOrderedBy.Name = "txtOrderedBy";
      this.txtOrderedBy.ReadOnly = true;
      this.txtOrderedBy.Size = new Size(366, 20);
      this.txtOrderedBy.TabIndex = 9;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(262, 39);
      this.label1.Name = "label1";
      this.label1.Size = new Size(63, 14);
      this.label1.TabIndex = 8;
      this.label1.Text = "Ordered By";
      this.txtDateOrdered.Location = new Point(82, 36);
      this.txtDateOrdered.Name = "txtDateOrdered";
      this.txtDateOrdered.ReadOnly = true;
      this.txtDateOrdered.Size = new Size(159, 20);
      this.txtDateOrdered.TabIndex = 2;
      this.lblAlertName.AutoSize = true;
      this.lblAlertName.Location = new Point(7, 39);
      this.lblAlertName.Name = "lblAlertName";
      this.lblAlertName.Size = new Size(72, 14);
      this.lblAlertName.TabIndex = 1;
      this.lblAlertName.Text = "Date Ordered";
      this.grpAudit.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.grpAudit.Controls.Add((Control) this.gvAudit);
      this.grpAudit.Controls.Add((Control) this.pnlAuditOptions);
      this.grpAudit.Controls.Add((Control) this.pnlAuditControls);
      this.grpAudit.Dock = DockStyle.Fill;
      this.grpAudit.HeaderForeColor = SystemColors.ControlText;
      this.grpAudit.Location = new Point(0, 0);
      this.grpAudit.Name = "grpAudit";
      this.grpAudit.Size = new Size(703, 185);
      this.grpAudit.TabIndex = 4;
      this.grpAudit.Text = "Audited on ...";
      this.gvAudit.AllowMultiselect = false;
      this.gvAudit.BorderStyle = BorderStyle.None;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column1";
      gvColumn3.Text = "Type";
      gvColumn3.Width = 100;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column2";
      gvColumn4.SpringToFit = true;
      gvColumn4.Text = "Description";
      gvColumn4.Width = 601;
      this.gvAudit.Columns.AddRange(new GVColumn[2]
      {
        gvColumn3,
        gvColumn4
      });
      this.gvAudit.Dock = DockStyle.Fill;
      this.gvAudit.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvAudit.Location = new Point(1, 50);
      this.gvAudit.Name = "gvAudit";
      this.gvAudit.Selectable = false;
      this.gvAudit.Size = new Size(701, 134);
      this.gvAudit.TabIndex = 2;
      this.pnlAuditOptions.Borders = AnchorStyles.Bottom;
      this.pnlAuditOptions.Controls.Add((Control) this.radComplianceAudit);
      this.pnlAuditOptions.Controls.Add((Control) this.radDocAudit);
      this.pnlAuditOptions.Dock = DockStyle.Top;
      this.pnlAuditOptions.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.pnlAuditOptions.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.pnlAuditOptions.Location = new Point(1, 25);
      this.pnlAuditOptions.Name = "pnlAuditOptions";
      this.pnlAuditOptions.Size = new Size(701, 25);
      this.pnlAuditOptions.Style = GradientPanel.PanelStyle.TableFooter;
      this.pnlAuditOptions.TabIndex = 3;
      this.radComplianceAudit.AutoSize = true;
      this.radComplianceAudit.BackColor = Color.Transparent;
      this.radComplianceAudit.Location = new Point((int) sbyte.MaxValue, 4);
      this.radComplianceAudit.Name = "radComplianceAudit";
      this.radComplianceAudit.Size = new Size(159, 18);
      this.radComplianceAudit.TabIndex = 1;
      this.radComplianceAudit.TabStop = true;
      this.radComplianceAudit.Text = "Compliance Review Results";
      this.radComplianceAudit.UseVisualStyleBackColor = false;
      this.radDocAudit.AutoSize = true;
      this.radDocAudit.BackColor = Color.Transparent;
      this.radDocAudit.Location = new Point(10, 4);
      this.radDocAudit.Name = "radDocAudit";
      this.radDocAudit.Size = new Size(113, 18);
      this.radDocAudit.TabIndex = 0;
      this.radDocAudit.TabStop = true;
      this.radDocAudit.Text = "Data Audit Results";
      this.radDocAudit.UseVisualStyleBackColor = false;
      this.radDocAudit.CheckedChanged += new EventHandler(this.radDocAudit_CheckedChanged);
      this.pnlAuditControls.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pnlAuditControls.AutoSize = true;
      this.pnlAuditControls.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.pnlAuditControls.BackColor = Color.Transparent;
      this.pnlAuditControls.Controls.Add((Control) this.btnAuditReport);
      this.pnlAuditControls.FlowDirection = FlowDirection.RightToLeft;
      this.pnlAuditControls.Location = new Point(677, 1);
      this.pnlAuditControls.Name = "pnlAuditControls";
      this.pnlAuditControls.Size = new Size(22, 22);
      this.pnlAuditControls.TabIndex = 1;
      this.btnAuditReport.DisabledImage = (Image) null;
      this.btnAuditReport.Image = (Image) Resources.pdf;
      this.btnAuditReport.Location = new Point(3, 3);
      this.btnAuditReport.MouseDownImage = (Image) Resources.pdf_over;
      this.btnAuditReport.MouseOverImage = (Image) Resources.pdf_over;
      this.btnAuditReport.Name = "btnAuditReport";
      this.btnAuditReport.Size = new Size(16, 16);
      this.btnAuditReport.TabIndex = 0;
      this.btnAuditReport.TabStop = false;
      this.btnAuditReport.Click += new EventHandler(this.btnAuditReport_Click);
      this.grpRecipients.Controls.Add((Control) this.gvRecipients);
      this.grpRecipients.Controls.Add((Control) this.flowLayoutPanel1);
      this.grpRecipients.Dock = DockStyle.Fill;
      this.grpRecipients.HeaderForeColor = SystemColors.ControlText;
      this.grpRecipients.Location = new Point(0, 0);
      this.grpRecipients.Name = "grpRecipients";
      this.grpRecipients.Size = new Size(703, 116);
      this.grpRecipients.TabIndex = 4;
      this.grpRecipients.Text = "Recipients";
      this.gvRecipients.AllowMultiselect = false;
      this.gvRecipients.BorderStyle = BorderStyle.None;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Name";
      gvColumn5.SpringToFit = true;
      gvColumn5.Text = "Full Name";
      gvColumn5.Width = 551;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Borrower Type";
      gvColumn6.Text = "Type";
      gvColumn6.Width = 150;
      this.gvRecipients.Columns.AddRange(new GVColumn[2]
      {
        gvColumn5,
        gvColumn6
      });
      this.gvRecipients.Dock = DockStyle.Fill;
      this.gvRecipients.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvRecipients.Location = new Point(1, 26);
      this.gvRecipients.Name = "gvRecipients";
      this.gvRecipients.Size = new Size(701, 89);
      this.gvRecipients.TabIndex = 2;
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel1.AutoSize = true;
      this.flowLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(699, 2);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(0, 0);
      this.flowLayoutPanel1.TabIndex = 1;
      this.splitContainer1.Dock = DockStyle.Fill;
      this.splitContainer1.Location = new Point(0, 67);
      this.splitContainer1.Name = "splitContainer1";
      this.splitContainer1.Orientation = Orientation.Horizontal;
      this.splitContainer1.Panel1.Controls.Add((Control) this.grpRecipients);
      this.splitContainer1.Panel2.Controls.Add((Control) this.splitContainer2);
      this.splitContainer1.Size = new Size(703, 601);
      this.splitContainer1.SplitterDistance = 116;
      this.splitContainer1.TabIndex = 5;
      this.splitContainer2.Dock = DockStyle.Fill;
      this.splitContainer2.Location = new Point(0, 0);
      this.splitContainer2.Name = "splitContainer2";
      this.splitContainer2.Orientation = Orientation.Horizontal;
      this.splitContainer2.Panel1.Controls.Add((Control) this.grpDocuments);
      this.splitContainer2.Panel2.Controls.Add((Control) this.grpAudit);
      this.splitContainer2.Size = new Size(703, 481);
      this.splitContainer2.SplitterDistance = 292;
      this.splitContainer2.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoSize = true;
      this.Controls.Add((Control) this.splitContainer1);
      this.Controls.Add((Control) this.grpDetails);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (DocOrderLogWS);
      this.Size = new Size(703, 668);
      this.grpDocuments.ResumeLayout(false);
      this.grpDocuments.PerformLayout();
      this.flowLayoutPanel2.ResumeLayout(false);
      this.grpDetails.ResumeLayout(false);
      this.grpDetails.PerformLayout();
      this.grpAudit.ResumeLayout(false);
      this.grpAudit.PerformLayout();
      this.pnlAuditOptions.ResumeLayout(false);
      this.pnlAuditOptions.PerformLayout();
      this.pnlAuditControls.ResumeLayout(false);
      ((ISupportInitialize) this.btnAuditReport).EndInit();
      this.grpRecipients.ResumeLayout(false);
      this.grpRecipients.PerformLayout();
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.EndInit();
      this.splitContainer1.ResumeLayout(false);
      this.splitContainer2.Panel1.ResumeLayout(false);
      this.splitContainer2.Panel2.ResumeLayout(false);
      this.splitContainer2.EndInit();
      this.splitContainer2.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
