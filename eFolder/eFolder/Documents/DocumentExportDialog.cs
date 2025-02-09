// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Documents.DocumentExportDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.eFolder.Properties;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Documents
{
  public class DocumentExportDialog : Form
  {
    private const string CLOSINGDATEFIELDID = "748";
    private const string ESTIMATEDCLOSINGDATEFIELDID = "763";
    private const string BORROWERLASTNAMEFIELDID = "37";
    private const string BORROWERFIRSTNAMEFIELDID = "36";
    private const int ERRORCOUNTSUBITEMINDEX = 4;
    private string[] loanguids;
    private StackingOrderSetTemplate stackingTemplate;
    private DocumentExportTemplate exportTemplate;
    private string warningPriorToExport = "NoWarning";
    private ILoanServices serviceMgr = Session.Application.GetService<ILoanServices>();
    private IContainer components;
    internal GridView gvDocuments;
    private Button btnCancel;
    private Button btnSave;
    private Label lblDescription;
    private GroupContainer grpResults;
    private EMHelpLink helpLink;
    private Panel pnlStackingKey;
    private Label lblOptionalDesc;
    private Label lblRequiredDesc;
    private Label lblRequiredBox;
    private Label lblOptionalBox;
    private Button button1;
    private Button btnContinue;
    private Panel panel1;
    private Label lblTqlLoanWarningDesc;
    private Label lblTqlLoanNotExportedDesc;
    private Label label5;
    private Label label6;
    private ToolTip toolTip1;
    private PictureBox picTqlLoanWarningIcon;
    private PictureBox picTqlLoanNotExportedIcon;

    public DocumentExportDialog(
      string[] loanguids,
      StackingOrderSetTemplate stackingTemplate,
      DocumentExportTemplate exportTemplate)
    {
      this.InitializeComponent();
      this.loanguids = loanguids;
      this.stackingTemplate = stackingTemplate;
      this.exportTemplate = exportTemplate;
      this.lblOptionalBox.BackColor = ColorTranslator.FromHtml("#FFF4BF");
      this.lblRequiredBox.BackColor = ColorTranslator.FromHtml("#FDD1D2");
      Hashtable servicesWarningSettings = this.serviceMgr.GetClientServicesWarningSettings();
      if (servicesWarningSettings != null)
        this.warningPriorToExport = servicesWarningSettings[(object) "WarningPriorToExport"].ToString();
      this.gvDocuments.ImageList = new ImageList()
      {
        Images = {
          this.picTqlLoanWarningIcon.Image,
          this.picTqlLoanNotExportedIcon.Image
        }
      };
      this.loadDocuments();
    }

    private void loadDocuments()
    {
      foreach (string loanguid in this.loanguids)
      {
        LoanDataMgr loanDataMgr = LoanDataMgr.OpenLoan(Session.SessionObjects, loanguid, false);
        Application.DoEvents();
        this.gvDocuments.Items.Add(this.createGVItemForLoan(loanDataMgr));
        Application.DoEvents();
      }
      this.grpResults.Text = "Loans (" + (object) this.loanguids.Length + ")";
    }

    private GVItem createGVItemForLoan(LoanDataMgr loanDataMgr)
    {
      LoanData loanData = loanDataMgr.LoanData;
      string field = loanData.GetField("763");
      if (Utils.IsDate((object) loanData.GetField("748")))
        field = loanData.GetField("748");
      GVItem gvItemForLoan = new GVItem(loanData.GetField("37") + ", " + loanData.GetField("36"));
      gvItemForLoan.GroupItems.DisableSort = true;
      gvItemForLoan.SubItems[1].Value = (object) loanData.LoanNumber;
      gvItemForLoan.SubItems[2].Value = (object) field;
      gvItemForLoan.SubItems[3].Value = (object) "0";
      gvItemForLoan.SubItems[4].Value = (object) "0";
      gvItemForLoan.SubItems[5].ImageAlignment = HorizontalAlignment.Center;
      gvItemForLoan.SubItems[5].Tag = (object) false;
      gvItemForLoan.Tag = (object) loanDataMgr;
      ArrayList requiredDocs = this.stackingTemplate.RequiredDocs;
      int num1 = 0;
      int num2 = 0;
      if (this.stackingTemplate == null)
        return (GVItem) null;
      DocumentLog[] allDocuments = loanDataMgr.LoanData.GetLogList().GetAllDocuments();
      foreach (string docName in this.stackingTemplate.DocNames)
      {
        if (requiredDocs.Contains((object) docName))
        {
          bool flag1 = false;
          bool flag2 = false;
          foreach (DocumentLog doc in allDocuments)
          {
            if (this.docMatchesStackingItem(doc, docName) && doc.IsThirdPartyDoc)
            {
              flag1 = true;
              if (loanDataMgr.FileAttachments.ContainsAttachment(doc))
              {
                flag2 = true;
                ++num1;
              }
            }
          }
          if (!flag1 || !flag2)
          {
            gvItemForLoan.GroupItems.Add(new GVItem(docName)
            {
              BackColor = this.lblRequiredBox.BackColor
            });
            ++num2;
          }
        }
      }
      foreach (string docName in this.stackingTemplate.DocNames)
      {
        if (!requiredDocs.Contains((object) docName))
        {
          bool flag3 = false;
          bool flag4 = false;
          foreach (DocumentLog doc in allDocuments)
          {
            if (this.docMatchesStackingItem(doc, docName) && doc.IsThirdPartyDoc)
            {
              flag3 = true;
              if (loanDataMgr.FileAttachments.ContainsAttachment(doc))
              {
                flag4 = true;
                ++num1;
              }
            }
          }
          if (!flag3 || !flag4)
            gvItemForLoan.GroupItems.Add(new GVItem(docName)
            {
              BackColor = this.lblOptionalBox.BackColor
            });
        }
      }
      gvItemForLoan.SubItems[3].Value = (object) num1;
      gvItemForLoan.SubItems[4].Value = (object) num2;
      int investorID = this.intValue(loanData.GetSimpleField("3318"));
      if (investorID != 0 && this.warningPriorToExport != "NoWarning")
      {
        bool flag = this.serviceMgr.IsInvestorMissingRequiredReports(loanDataMgr, investorID);
        if (flag && this.warningPriorToExport == "Warning")
          gvItemForLoan.SubItems[5].ImageIndex = 0;
        else if (flag && this.warningPriorToExport == "WarningAndHardStop")
        {
          gvItemForLoan.SubItems[5].ImageIndex = 1;
          gvItemForLoan.SubItems[5].Tag = (object) true;
        }
      }
      return gvItemForLoan;
    }

    private int intValue(string val)
    {
      try
      {
        return int.Parse(val);
      }
      catch (Exception ex)
      {
        return 0;
      }
    }

    private bool docMatchesStackingItem(DocumentLog doc, string stackingName)
    {
      return doc.Title.ToLower() == stackingName.ToLower() || this.stackingTemplate.NDEDocGroups.Contains((object) stackingName) && doc.GroupName.ToLower() == stackingName.ToLower();
    }

    private void btnContinue_Click(object sender, EventArgs e)
    {
      try
      {
        int num1 = 0;
        string str1 = "\r\nLOANS NOT EXPORTED:\r\n";
        string str2 = "\r\nLOANS EXPORTED:\r\n";
        string str3 = DateTime.Now.ToString() + "\r\nDOCUMENT EXPORT using " + this.exportTemplate.TemplateName + ".\r\n";
        this.Cursor = Cursors.WaitCursor;
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvDocuments.Items)
        {
          LoanDataMgr tag = (LoanDataMgr) gvItem.Tag;
          if (gvItem.SubItems[4].Value.ToString() == "0" && !(bool) gvItem.SubItems[5].Tag)
          {
            DocumentLog[] allDocuments = tag.LoanData.GetLogList().GetAllDocuments();
            List<DocumentLog> documentLogList = new List<DocumentLog>();
            int num2 = 0;
            if (this.stackingTemplate != null)
            {
              foreach (string docName in this.stackingTemplate.DocNames)
              {
                foreach (DocumentLog doc in allDocuments)
                {
                  if (tag.FileAttachments.ContainsAttachment(doc) && this.docMatchesStackingItem(doc, docName) && doc.IsThirdPartyDoc)
                  {
                    documentLogList.Add(doc);
                    ++num2;
                  }
                }
              }
            }
            if (documentLogList.Count > 0)
              Session.Application.GetService<IEFolder>().ExportDocuments(tag, documentLogList.ToArray(), this.exportTemplate);
            ++num1;
            str2 = str2 + "\r\nLoan: " + tag.LoanData.LoanNumber;
            str2 = str2 + "\r\n" + num2.ToString() + " documents exported.";
          }
          else
            str1 = str1 + "\r\nLoan: " + tag.LoanData.LoanNumber;
        }
        string str4 = str3 + str1 + "\r\n" + str2;
        if (!Directory.Exists(this.exportTemplate.ExportLocation))
          Directory.CreateDirectory(this.exportTemplate.ExportLocation);
        string path = Path.Combine(this.exportTemplate.ExportLocation, "DocumentExportLog.txt");
        int num3 = 1;
        while (File.Exists(path))
        {
          path = Path.Combine(this.exportTemplate.ExportLocation, "DocumentExportLog-" + num3.ToString() + ".txt");
          ++num3;
        }
        using (StreamWriter text = File.CreateText(path))
          text.Write(str4);
        string text1 = string.Format("Export Complete: Documents for {0} loan(s) were exported of {1} selected.", (object) num1.ToString(), (object) this.gvDocuments.Items.Count.ToString());
        if (num1 < this.gvDocuments.Items.Count)
          text1 = text1 + "\r\n\r\nSee log " + path + " for details.";
        this.Cursor = Cursors.Default;
        int num4 = (int) MessageBox.Show(text1, "Export Complete");
      }
      catch (DirectoryNotFoundException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Error exporting documents: " + ex.Message);
      }
      catch (IOException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Error exporting documents: Could not find path " + this.exportTemplate.ExportLocation + ". " + ex.Message);
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Error exporting documents: " + ex.Message);
      }
      finally
      {
        this.DialogResult = DialogResult.OK;
      }
    }

    private void DocumentExportDialog_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      Session.Application.DisplayHelp(this.helpLink.HelpTag);
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
      this.gvDocuments = new GridView();
      this.btnCancel = new Button();
      this.btnSave = new Button();
      this.lblDescription = new Label();
      this.grpResults = new GroupContainer();
      this.helpLink = new EMHelpLink();
      this.pnlStackingKey = new Panel();
      this.lblOptionalDesc = new Label();
      this.lblRequiredDesc = new Label();
      this.lblRequiredBox = new Label();
      this.lblOptionalBox = new Label();
      this.button1 = new Button();
      this.btnContinue = new Button();
      this.panel1 = new Panel();
      this.picTqlLoanNotExportedIcon = new PictureBox();
      this.picTqlLoanWarningIcon = new PictureBox();
      this.lblTqlLoanWarningDesc = new Label();
      this.lblTqlLoanNotExportedDesc = new Label();
      this.label5 = new Label();
      this.label6 = new Label();
      this.toolTip1 = new ToolTip(this.components);
      this.grpResults.SuspendLayout();
      this.pnlStackingKey.SuspendLayout();
      this.panel1.SuspendLayout();
      ((ISupportInitialize) this.picTqlLoanNotExportedIcon).BeginInit();
      ((ISupportInitialize) this.picTqlLoanWarningIcon).BeginInit();
      this.SuspendLayout();
      this.gvDocuments.Anchor = AnchorStyles.None;
      this.gvDocuments.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "colBorrowerName";
      gvColumn1.SpringToFit = true;
      gvColumn1.Text = "Borrower Name";
      gvColumn1.Width = 205;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "colLoanNo";
      gvColumn2.Text = "Loan Number";
      gvColumn2.Width = 110;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "colClosingDate";
      gvColumn3.Text = "Closing Date";
      gvColumn3.Width = 100;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "colDocCount";
      gvColumn4.Text = "Documents to Export";
      gvColumn4.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn4.Width = 120;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "colErrors";
      gvColumn5.Text = "Document Errors";
      gvColumn5.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn5.Width = 120;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "colTQLErrors";
      gvColumn6.Text = "TQL Status Check";
      gvColumn6.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn6.Width = 120;
      this.gvDocuments.Columns.AddRange(new GVColumn[6]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6
      });
      this.gvDocuments.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvDocuments.ItemGrouping = true;
      this.gvDocuments.Location = new Point(1, 28);
      this.gvDocuments.Name = "gvDocuments";
      this.gvDocuments.Size = new Size(775, 354);
      this.gvDocuments.TabIndex = 0;
      this.gvDocuments.TextTrimming = StringTrimming.EllipsisCharacter;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(288, 336);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 5;
      this.btnCancel.Text = "Cancel";
      this.btnSave.Location = new Point(212, 336);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 22);
      this.btnSave.TabIndex = 4;
      this.btnSave.Text = "Save";
      this.lblDescription.Location = new Point(10, 4);
      this.lblDescription.Name = "lblDescription";
      this.lblDescription.Size = new Size(732, 34);
      this.lblDescription.TabIndex = 0;
      this.lblDescription.Text = "Documents highlighted below are missing required or optional files in the eFolder. If required files are missing, none of the documents will be exported for the loan. ";
      this.grpResults.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.grpResults.Controls.Add((Control) this.gvDocuments);
      this.grpResults.HeaderForeColor = SystemColors.ControlText;
      this.grpResults.Location = new Point(10, 38);
      this.grpResults.Name = "grpResults";
      this.grpResults.Size = new Size(777, 385);
      this.grpResults.TabIndex = 1;
      this.grpResults.Text = "Loans";
      this.helpLink.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.helpLink.BackColor = Color.Transparent;
      this.helpLink.Cursor = Cursors.Hand;
      this.helpLink.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.helpLink.HelpTag = "DocumentExportAuditReport";
      this.helpLink.Location = new Point(12, 479);
      this.helpLink.Name = "helpLink";
      this.helpLink.Size = new Size(90, 18);
      this.helpLink.TabIndex = 8;
      this.helpLink.TabStop = false;
      this.pnlStackingKey.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.pnlStackingKey.BackColor = Color.Transparent;
      this.pnlStackingKey.Controls.Add((Control) this.lblOptionalDesc);
      this.pnlStackingKey.Controls.Add((Control) this.lblRequiredDesc);
      this.pnlStackingKey.Controls.Add((Control) this.lblRequiredBox);
      this.pnlStackingKey.Controls.Add((Control) this.lblOptionalBox);
      this.pnlStackingKey.Location = new Point(14, 446);
      this.pnlStackingKey.Name = "pnlStackingKey";
      this.pnlStackingKey.Size = new Size(317, 22);
      this.pnlStackingKey.TabIndex = 1;
      this.lblOptionalDesc.AutoSize = true;
      this.lblOptionalDesc.Location = new Point(19, 4);
      this.lblOptionalDesc.Name = "lblOptionalDesc";
      this.lblOptionalDesc.Size = new Size((int) sbyte.MaxValue, 14);
      this.lblOptionalDesc.TabIndex = 3;
      this.lblOptionalDesc.Text = "Optional files are missing";
      this.lblRequiredDesc.AutoSize = true;
      this.lblRequiredDesc.Location = new Point(182, 4);
      this.lblRequiredDesc.Name = "lblRequiredDesc";
      this.lblRequiredDesc.Size = new Size(131, 14);
      this.lblRequiredDesc.TabIndex = 5;
      this.lblRequiredDesc.Text = "Required files are missing";
      this.lblRequiredBox.AutoSize = true;
      this.lblRequiredBox.BorderStyle = BorderStyle.FixedSingle;
      this.lblRequiredBox.Location = new Point(165, 2);
      this.lblRequiredBox.Name = "lblRequiredBox";
      this.lblRequiredBox.Size = new Size(15, 16);
      this.lblRequiredBox.TabIndex = 4;
      this.lblRequiredBox.Text = "  ";
      this.lblOptionalBox.AutoSize = true;
      this.lblOptionalBox.BorderStyle = BorderStyle.FixedSingle;
      this.lblOptionalBox.Location = new Point(2, 2);
      this.lblOptionalBox.Name = "lblOptionalBox";
      this.lblOptionalBox.Size = new Size(15, 16);
      this.lblOptionalBox.TabIndex = 2;
      this.lblOptionalBox.Text = "  ";
      this.button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.button1.DialogResult = DialogResult.Cancel;
      this.button1.Location = new Point(711, 475);
      this.button1.Margin = new Padding(1, 0, 0, 0);
      this.button1.Name = "button1";
      this.button1.Size = new Size(75, 24);
      this.button1.TabIndex = 10;
      this.button1.Text = "Cancel";
      this.button1.UseVisualStyleBackColor = true;
      this.btnContinue.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnContinue.Location = new Point(630, 475);
      this.btnContinue.Margin = new Padding(1, 0, 0, 0);
      this.btnContinue.Name = "btnContinue";
      this.btnContinue.Size = new Size(75, 24);
      this.btnContinue.TabIndex = 9;
      this.btnContinue.Text = "Next >";
      this.btnContinue.UseVisualStyleBackColor = true;
      this.btnContinue.Click += new EventHandler(this.btnContinue_Click);
      this.panel1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.panel1.BackColor = Color.Transparent;
      this.panel1.Controls.Add((Control) this.picTqlLoanNotExportedIcon);
      this.panel1.Controls.Add((Control) this.picTqlLoanWarningIcon);
      this.panel1.Controls.Add((Control) this.lblTqlLoanWarningDesc);
      this.panel1.Controls.Add((Control) this.lblTqlLoanNotExportedDesc);
      this.panel1.Location = new Point(425, 446);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(317, 22);
      this.panel1.TabIndex = 6;
      this.picTqlLoanNotExportedIcon.Image = (Image) Resources.status_alert;
      this.picTqlLoanNotExportedIcon.Location = new Point(130, 2);
      this.picTqlLoanNotExportedIcon.Name = "picTqlLoanNotExportedIcon";
      this.picTqlLoanNotExportedIcon.Size = new Size(16, 16);
      this.picTqlLoanNotExportedIcon.TabIndex = 0;
      this.picTqlLoanNotExportedIcon.TabStop = false;
      this.picTqlLoanWarningIcon.Image = (Image) Resources.status_warning;
      this.picTqlLoanWarningIcon.Location = new Point(1, 2);
      this.picTqlLoanWarningIcon.Name = "picTqlLoanWarningIcon";
      this.picTqlLoanWarningIcon.Size = new Size(16, 16);
      this.picTqlLoanWarningIcon.TabIndex = 1;
      this.picTqlLoanWarningIcon.TabStop = false;
      this.lblTqlLoanWarningDesc.AutoSize = true;
      this.lblTqlLoanWarningDesc.Location = new Point(20, 4);
      this.lblTqlLoanWarningDesc.Name = "lblTqlLoanWarningDesc";
      this.lblTqlLoanWarningDesc.Size = new Size(93, 14);
      this.lblTqlLoanWarningDesc.TabIndex = 29;
      this.lblTqlLoanWarningDesc.Text = "TQL loan warning";
      this.toolTip1.SetToolTip((Control) this.lblTqlLoanWarningDesc, "Indicates that the loan has missing required TQL documents, but will still export");
      this.lblTqlLoanNotExportedDesc.AutoSize = true;
      this.lblTqlLoanNotExportedDesc.Location = new Point(148, 4);
      this.lblTqlLoanNotExportedDesc.Name = "lblTqlLoanNotExportedDesc";
      this.lblTqlLoanNotExportedDesc.Size = new Size(148, 14);
      this.lblTqlLoanNotExportedDesc.TabIndex = 7;
      this.lblTqlLoanNotExportedDesc.Text = "TQL loan will not be exported";
      this.toolTip1.SetToolTip((Control) this.lblTqlLoanNotExportedDesc, "Indicates that the loan has missing required TQL documents, and will not be exported");
      this.label5.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label5.AutoSize = true;
      this.label5.BackColor = Color.Transparent;
      this.label5.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label5.Location = new Point(10, 429);
      this.label5.Name = "label5";
      this.label5.Size = new Size(139, 14);
      this.label5.TabIndex = 28;
      this.label5.Text = "Document Status Check";
      this.label6.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label6.AutoSize = true;
      this.label6.BackColor = Color.Transparent;
      this.label6.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label6.Location = new Point(419, 429);
      this.label6.Name = "label6";
      this.label6.Size = new Size(105, 14);
      this.label6.TabIndex = 29;
      this.label6.Text = "TQL Status Check";
      this.toolTip1.AutomaticDelay = 300;
      this.AcceptButton = (IButtonControl) this.btnSave;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(800, 505);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.button1);
      this.Controls.Add((Control) this.btnContinue);
      this.Controls.Add((Control) this.pnlStackingKey);
      this.Controls.Add((Control) this.helpLink);
      this.Controls.Add((Control) this.grpResults);
      this.Controls.Add((Control) this.lblDescription);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnSave);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (DocumentExportDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Document Export Audit Report";
      this.KeyDown += new KeyEventHandler(this.DocumentExportDialog_KeyDown);
      this.grpResults.ResumeLayout(false);
      this.pnlStackingKey.ResumeLayout(false);
      this.pnlStackingKey.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      ((ISupportInitialize) this.picTqlLoanNotExportedIcon).EndInit();
      ((ISupportInitialize) this.picTqlLoanWarningIcon).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
