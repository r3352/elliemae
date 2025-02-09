// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.Import.CsvExportFileSelectionPanel
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.CustomFields;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Wizard;
using EllieMae.EMLite.ContactUI.CustomFields;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI.Import
{
  public class CsvExportFileSelectionPanel : WizardItemWithHeader
  {
    private Panel panel2;
    private Label label1;
    private TextBox txtFile;
    private Button btnBrowse;
    private IContainer components;
    private ContactType contactType;
    private int[] contactIds;
    private Hashtable cfInfoTable = new Hashtable();
    private OpenFileDialog ofdBrowse;
    private BizCategoryUtil catUtil;
    private CsvImportColumn[] contactColumns;

    public override string NextLabel => "&Export >";

    public CsvExportFileSelectionPanel(ContactType contactType, int[] contactIds)
    {
      this.InitializeComponent();
      this.contactType = contactType;
      this.contactIds = contactIds;
      ContactCustomFieldInfo[] items = Session.ContactManager.GetCustomFieldInfo(this.contactType).Items;
      for (int index = 0; index < items.Length; ++index)
        this.cfInfoTable.Add((object) items[index].Label, (object) items[index].LabelID);
      if (this.contactType == ContactType.BizPartner)
        this.catUtil = new BizCategoryUtil(Session.SessionObjects);
      this.txtFile.Text = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\contact.csv";
      this.txtFile.Select(0, 0);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.ofdBrowse = new OpenFileDialog();
      this.panel2 = new Panel();
      this.label1 = new Label();
      this.txtFile = new TextBox();
      this.btnBrowse = new Button();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      this.ofdBrowse.CheckFileExists = false;
      this.ofdBrowse.CheckPathExists = false;
      this.ofdBrowse.DefaultExt = "csv";
      this.ofdBrowse.Filter = "Comma-Separated Value Files|*.csv|All Files|*.*";
      this.ofdBrowse.Title = "File Location and Name";
      this.panel2.BackColor = Color.White;
      this.panel2.Controls.Add((Control) this.label1);
      this.panel2.Controls.Add((Control) this.txtFile);
      this.panel2.Controls.Add((Control) this.btnBrowse);
      this.panel2.Dock = DockStyle.Fill;
      this.panel2.Location = new Point(0, 60);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(496, 254);
      this.panel2.TabIndex = 10;
      this.label1.Location = new Point(38, 62);
      this.label1.Name = "label1";
      this.label1.Size = new Size(280, 18);
      this.label1.TabIndex = 3;
      this.label1.Text = "Export contact data to the following location and file:";
      this.txtFile.Location = new Point(38, 80);
      this.txtFile.Name = "txtFile";
      this.txtFile.Size = new Size(316, 20);
      this.txtFile.TabIndex = 4;
      this.txtFile.Text = "";
      this.btnBrowse.BackColor = SystemColors.Control;
      this.btnBrowse.Location = new Point(356, 78);
      this.btnBrowse.Name = "btnBrowse";
      this.btnBrowse.TabIndex = 5;
      this.btnBrowse.Text = "Browse...";
      this.btnBrowse.Click += new EventHandler(this.btnBrowse_Click);
      this.Controls.Add((Control) this.panel2);
      this.Header = "File Location and Name";
      this.Name = nameof (CsvExportFileSelectionPanel);
      this.Subheader = "Select the location for the exported contacts and enter a new file name.";
      this.Controls.SetChildIndex((Control) this.panel2, 0);
      this.panel2.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public static void ChangeDlgItemThreadProc()
    {
      int window;
      while ((window = Win32.FindWindow((string) null, "File Location and Name")) == 0)
        Thread.Sleep(10);
      Win32.SetDlgItemText(window, Win32.IDOK, "OK");
    }

    private void btnBrowse_Click(object sender, EventArgs e)
    {
      new Thread(new ThreadStart(CsvExportFileSelectionPanel.ChangeDlgItemThreadProc)).Start();
      if (this.ofdBrowse.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      this.txtFile.Text = this.ofdBrowse.FileName;
    }

    public override WizardItem Next()
    {
      if (this.txtFile.Text.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please enter a valid file name in the space provided.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return (WizardItem) null;
      }
      if (File.Exists(this.txtFile.Text) && DialogResult.No == Utils.Dialog((IWin32Window) this, "The specified file already exits. Do you want to overwrite it?", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk))
        return (WizardItem) null;
      new ProgressDialog("Exporting Contacts", new AsynchronousProcess(this.exportContactData), (object) null, true).ShowDialog((IWin32Window) this.ParentForm);
      return WizardItem.Finished;
    }

    private void writeCsvHeader(StreamWriter sw)
    {
      this.contactColumns = this.contactType != ContactType.Borrower ? CsvImportParameters.getAllBizPartnerColumns() : CsvImportParameters.getAllBorrowerColumns();
      for (int index = 0; index < this.contactColumns.Length - 1; ++index)
        sw.Write("\"" + this.contactColumns[index].Description + "\",");
      sw.Write("\"" + this.contactColumns[this.contactColumns.Length - 1].Description + "\"\r\n");
    }

    private void writeCsvRecord(StreamWriter sw, int contactId)
    {
      if (this.contactType == ContactType.Borrower)
        this.writeCsvBorrowerRecord(sw, contactId);
      else
        this.writeCsvBizPartnerRecord(sw, contactId);
    }

    private void writeCsvBizPartnerRecord(StreamWriter sw, int contactId)
    {
      BizPartnerInfo bizPartner = Session.ContactManager.GetBizPartner(contactId);
      ContactCustomField[] fieldsForContact = Session.ContactManager.GetCustomFieldsForContact(contactId, this.contactType);
      Hashtable hashtable = new Hashtable();
      for (int index = 0; index < fieldsForContact.Length; ++index)
        hashtable.Add((object) fieldsForContact[index].FieldID, (object) fieldsForContact[index].FieldValue);
      CustomFieldValueCollection fieldValueCollection1 = (CustomFieldValueCollection) null;
      CustomFieldValueCollection fieldValueCollection2 = (CustomFieldValueCollection) null;
      for (int index = 0; index < this.contactColumns.Length; ++index)
      {
        object obj = (object) null;
        string str = "";
        if (this.contactColumns[index] is CvsCategoryFieldImportColumn)
        {
          if (CustomFieldsType.BizCategoryStandard == ((CvsCategoryFieldImportColumn) this.contactColumns[index]).CustomFieldsType)
          {
            if (fieldValueCollection1 == null)
              fieldValueCollection1 = CustomFieldValueCollection.GetCustomFieldValueCollection(Session.SessionObjects, new CustomFieldValueCollection.Criteria(bizPartner.ContactID, bizPartner.CategoryID));
            if (fieldValueCollection1.Contains(((CvsCategoryFieldImportColumn) this.contactColumns[index]).FieldId))
              obj = (object) fieldValueCollection1.Find(((CvsCategoryFieldImportColumn) this.contactColumns[index]).FieldId).FieldValue;
          }
          else
          {
            if (fieldValueCollection2 == null)
              fieldValueCollection2 = CustomFieldValueCollection.GetCustomFieldValueCollection(Session.SessionObjects, new CustomFieldValueCollection.Criteria(bizPartner.ContactID, bizPartner.CategoryID));
            if (fieldValueCollection2.Contains(((CvsCategoryFieldImportColumn) this.contactColumns[index]).FieldId))
              obj = (object) fieldValueCollection2.Find(((CvsCategoryFieldImportColumn) this.contactColumns[index]).FieldId).FieldValue;
          }
        }
        else if (this.contactColumns[index] is CvsCustomFieldImportColumn)
        {
          if (this.cfInfoTable.ContainsKey((object) this.contactColumns[index].PropertyName))
          {
            object key = this.cfInfoTable[(object) this.contactColumns[index].PropertyName];
            if (hashtable.ContainsKey(key))
              obj = hashtable[key];
          }
        }
        else if (this.contactColumns[index].PropertySource == "Contact")
        {
          obj = bizPartner[this.contactColumns[index].PropertyName];
          if (this.contactColumns[index].PropertyName.ToLower() == "categoryid")
            obj = (object) this.catUtil.CategoryIdToName((int) obj);
          else if (this.contactColumns[index].PropertyName.ToLower() == "fees" && (int) obj == -1)
            obj = (object) string.Empty;
        }
        else if (this.contactColumns[index].PropertySource == "PersonalInfoLicense")
          obj = bizPartner.PersonalInfoLicense[this.contactColumns[index].PropertyName];
        else if (this.contactColumns[index].PropertySource == "BizContactLicense")
          obj = bizPartner.BizContactLicense[this.contactColumns[index].PropertyName];
        if (obj != null)
          str = this.formatCsvValue(obj, this.contactColumns[index].Format);
        int startIndex1;
        for (int startIndex2 = 0; (startIndex1 = str.IndexOf('"', startIndex2)) != -1; startIndex2 = startIndex1 + 2)
          str = str.Insert(startIndex1, "\"");
        sw.Write("\"" + str + "\"");
        if (index != this.contactColumns.Length - 1)
          sw.Write(",");
        else
          sw.Write("\r\n");
      }
    }

    private string formatCsvValue(object valObj, FieldFormat format)
    {
      bool needsUpdate = false;
      string orgval = valObj.ToString();
      if (format == FieldFormat.X)
        return !(orgval.ToLower() == "true") && !(orgval.ToLower() == "x") ? "" : "X";
      if (format == FieldFormat.DATE)
      {
        try
        {
          orgval = ((DateTime) valObj).ToString("MM/dd/yyyy");
        }
        catch
        {
          orgval = valObj.ToString();
        }
      }
      if (format == FieldFormat.MONTHDAY)
      {
        try
        {
          orgval = ((DateTime) valObj).ToString("MM/dd");
        }
        catch
        {
          orgval = valObj.ToString();
        }
      }
      return Utils.FormatInput(orgval, format, ref needsUpdate);
    }

    private void writeCsvBorrowerRecord(StreamWriter sw, int contactId)
    {
      BorrowerInfo borrower = Session.ContactManager.GetBorrower(contactId);
      Opportunity opportunityByBorrowerId = Session.ContactManager.GetOpportunityByBorrowerId(contactId);
      ContactCustomField[] fieldsForContact = Session.ContactManager.GetCustomFieldsForContact(contactId, this.contactType);
      Hashtable hashtable = new Hashtable();
      for (int index = 0; index < fieldsForContact.Length; ++index)
        hashtable.Add((object) fieldsForContact[index].FieldID, (object) fieldsForContact[index].FieldValue);
      for (int index = 0; index < this.contactColumns.Length; ++index)
      {
        string str = "";
        object valObj = (object) null;
        if (this.contactColumns[index] is CvsCustomFieldImportColumn)
        {
          if (this.cfInfoTable.ContainsKey((object) this.contactColumns[index].PropertyName))
          {
            object key = this.cfInfoTable[(object) this.contactColumns[index].PropertyName];
            if (hashtable.ContainsKey(key))
              valObj = hashtable[key];
          }
        }
        else if (this.contactColumns[index].PropertySource == "Opportunity")
        {
          if (opportunityByBorrowerId == null)
          {
            str = string.Empty;
          }
          else
          {
            str = opportunityByBorrowerId.ColumnValueToString(this.contactColumns[index].PropertyName);
            if (this.contactColumns[index].PropertyName.ToLower() == "purpose" && str.ToLower() == "other")
              str = opportunityByBorrowerId.ColumnValueToString("purposeother");
          }
        }
        else
          str = borrower.ColumnValueToString(this.contactColumns[index].PropertyName);
        if (valObj != null)
          str = this.formatCsvValue(valObj, this.contactColumns[index].Format);
        int startIndex1;
        for (int startIndex2 = 0; (startIndex1 = str.IndexOf('"', startIndex2)) != -1; startIndex2 = startIndex1 + 2)
          str = str.Insert(startIndex1, "\"");
        sw.Write("\"" + str + "\"");
        if (index != this.contactColumns.Length - 1)
          sw.Write(",");
        else
          sw.Write("\r\n");
      }
    }

    private DialogResult exportContactData(object state, IProgressFeedback feedback)
    {
      int num1 = 0;
      StreamWriter sw = (StreamWriter) null;
      try
      {
        feedback.Status = "Preparing to export...";
        sw = new StreamWriter(this.txtFile.Text);
        feedback.ResetCounter(this.contactIds.Length);
        this.writeCsvHeader(sw);
        feedback.Status = "Exporting Contacts...";
        for (int index = 0; index < this.contactIds.Length; ++index)
        {
          this.writeCsvRecord(sw, this.contactIds[index]);
          ++num1;
          feedback.Increment(1);
          feedback.Details = "Completed " + (object) num1 + " of " + (object) this.contactIds.Length;
          if (feedback.Cancel)
          {
            int num2 = (int) Utils.Dialog((IWin32Window) feedback.ParentForm, "Operation cancelled after importing " + (object) num1 + " contacts.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            return DialogResult.Cancel;
          }
        }
        int num3 = (int) Utils.Dialog((IWin32Window) feedback.ParentForm, "Successfully exported " + (object) num1 + " contacts.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return DialogResult.OK;
      }
      catch (Exception ex)
      {
        int num4 = (int) Utils.Dialog((IWin32Window) this.ParentForm, "An error has occurred while exporting contacts: " + ex.Message + ".", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return DialogResult.Abort;
      }
      finally
      {
        sw.Close();
      }
    }

    private void timerCallback(object feedbackAsObject)
    {
      ((IServerProgressFeedback) feedbackAsObject).Increment(1);
    }
  }
}
