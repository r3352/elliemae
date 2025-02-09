// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CustomLetters.ContactFieldDialog
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.CustomFields;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.ContactUI.CustomFields;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.AsmResolver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.CustomLetters
{
  internal class ContactFieldDialog : Form
  {
    private const string CUSTOM_CONTACT_FIELDS = "Contact Custom Fields";
    private const string CUSTOM_CONTACT_FIELD_PREFIX = "CstFld";
    private const string STANDARD_CATEGORY_FIELDS = "Standard Category Fields";
    private const string STANDARD_CATEGORY_FIELD_PREFIX = "StdCat";
    private const string CUSTOM_CATEGORY_FIELDS = "Custom Category Fields";
    private const string CUSTOM_CATEGORY_FIELD_PREFIX = "CstCat";
    private const string CUSTOM_CATEGORY_FIELD_SEPARATOR = " - ";
    private string className = nameof (ContactFieldDialog);
    private static readonly string sw = Tracing.SwCustomLetters;
    private FileSystemEntry fsEntry;
    private string localFile;
    private CustomLetterType letterType;
    private WordHandler wordHandler;
    private string[] categoryNames;
    private ArrayList[] fieldLists;
    private Hashtable[] fieldMaps;
    private string letterMapFileName = string.Empty;
    private Sessions.Session session;
    private SortedList<int, string> bizCategoryNames;
    private System.ComponentModel.Container components;
    private GroupBox groupBox1;
    private ComboBox cboFieldList;
    private ComboBox cboCategoryList;
    private Label lblCategory;
    private Label lblField;
    private Button btnInsert;
    private Button btnSave;
    private Button btnClose;
    private Button btnFind;
    private TextBox txtFind;

    public ContactFieldDialog(
      string localFile,
      FileSystemEntry fsEntry,
      CustomLetterType letterType)
      : this(Session.DefaultInstance, localFile, fsEntry, letterType)
    {
    }

    public ContactFieldDialog(
      Sessions.Session session,
      string localFile,
      FileSystemEntry fsEntry,
      CustomLetterType letterType)
    {
      this.localFile = localFile;
      this.fsEntry = fsEntry;
      this.letterType = letterType;
      this.session = session;
      this.InitializeComponent();
      this.Location = new Point(0, 0);
      if (letterType == CustomLetterType.Borrower)
        this.letterMapFileName = AssemblyResolver.GetResourceFileFullPath(SystemSettings.BorLetterMapRelPath, SystemSettings.LocalAppDir);
      else
        this.letterMapFileName = AssemblyResolver.GetResourceFileFullPath(SystemSettings.BizLetterMapRelPath, SystemSettings.LocalAppDir);
    }

    public bool InitializeFieldList(bool bFileNew)
    {
      if (!this.initializeDataStructures())
        return false;
      try
      {
        this.wordHandler = new WordHandler();
        this.wordHandler.InitEdit(this.localFile, bFileNew, (Form) this);
        this.session.Application.Disable();
        this.Show();
        this.cboFieldList.Focus();
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        Tracing.Log(ContactFieldDialog.sw, TraceLevel.Error, this.className, "Error occurred in MailMerge.");
        Tracing.Log(ContactFieldDialog.sw, TraceLevel.Error, this.className, ex.StackTrace);
        if (this.wordHandler != null)
          this.wordHandler.ShutDown();
      }
      return true;
    }

    private bool initializeDataStructures()
    {
      string mappingData = this.readMappingFile();
      if (string.Empty == mappingData || !this.parseMappingData(mappingData))
        return false;
      this.populateCategoryList();
      return true;
    }

    private string readMappingFile()
    {
      string str = string.Empty;
      string letterMapFileName = this.letterMapFileName;
      if (!File.Exists(letterMapFileName))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, letterMapFileName + " file not found.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return str;
      }
      try
      {
        StreamReader streamReader = new StreamReader((Stream) new FileStream(letterMapFileName, FileMode.Open, FileAccess.Read, FileShare.Read));
        str = streamReader.ReadToEnd();
        streamReader.Close();
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        Tracing.Log(ContactFieldDialog.sw, TraceLevel.Error, this.className, "Message " + ex.Message);
        return str;
      }
      if (string.Empty == str)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "The " + letterMapFileName + " file format is not correct.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      return str.Replace(Environment.NewLine, "\n");
    }

    private bool parseMappingData(string mappingData)
    {
      string[] strArray1 = mappingData.Split('#');
      if (strArray1.Length == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The 'ContactLetterMap.Txt' file format is not correct.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      this.categoryNames = new string[strArray1.Length];
      this.fieldLists = new ArrayList[strArray1.Length];
      this.fieldMaps = new Hashtable[strArray1.Length];
      int index = 0;
      foreach (string str1 in strArray1)
      {
        int startIndex1 = str1.IndexOf("[") + 1;
        int length1 = str1.IndexOf("]") - startIndex1;
        this.categoryNames[index] = str1.Substring(startIndex1, length1).Trim();
        this.fieldLists[index] = new ArrayList();
        this.fieldMaps[index] = new Hashtable();
        if ("Contact Custom Fields" == this.categoryNames[index])
        {
          this.buildContactCustomFieldGroup(index);
          ++index;
        }
        else if ("Standard Category Fields" == this.categoryNames[index])
        {
          this.buildStandardCategoryFieldGroup(index);
          ++index;
        }
        else if ("Custom Category Fields" == this.categoryNames[index])
        {
          this.buildCustomCategoryFieldGroup(index);
          ++index;
        }
        else
        {
          int startIndex2 = startIndex1 + (length1 + 1);
          string[] strArray2 = str1.Substring(startIndex2).Split('\n');
          int fieldIndex = 0;
          foreach (string str2 in strArray2)
          {
            if (!(string.Empty == str2.Trim()))
            {
              int length2 = str2.Trim().IndexOf('|');
              if (-1 != length2)
              {
                string str3 = str2.Substring(0, length2).Trim();
                string fieldId = str2.Substring(length2 + 1).Trim();
                this.fieldLists[index].Add((object) str3);
                this.fieldMaps[index].Add((object) str3.ToUpper(), (object) new ContactFieldDialog.FieldMapValue(fieldId, fieldIndex));
                ++fieldIndex;
              }
            }
          }
          ++index;
        }
      }
      return true;
    }

    private void buildContactCustomFieldGroup(int index)
    {
      ContactCustomFieldInfoCollection customFieldInfo = this.session.ContactManager.GetCustomFieldInfo((ContactType) this.letterType);
      int fieldIndex = 0;
      foreach (ContactCustomFieldInfo contactCustomFieldInfo in customFieldInfo.Items)
      {
        if (!(string.Empty == contactCustomFieldInfo.Label.Trim()) && -1 != contactCustomFieldInfo.LabelID)
        {
          string str = contactCustomFieldInfo.Label.Trim();
          string fieldId = "CstFld" + contactCustomFieldInfo.LabelID.ToString();
          this.fieldLists[index].Add((object) str);
          this.fieldMaps[index].Add((object) str.ToUpper(), (object) new ContactFieldDialog.FieldMapValue(fieldId, fieldIndex));
          ++fieldIndex;
        }
      }
    }

    private void buildStandardCategoryFieldGroup(int groupIndex)
    {
      CustomFieldsDefinitionCollection definitionCollection = CustomFieldsDefinitionCollection.GetCustomFieldsDefinitionCollection(this.session.SessionObjects, new CustomFieldsDefinitionCollection.Criteria(CustomFieldsType.BizCategoryStandard));
      List<string> stringList = new List<string>();
      foreach (CustomFieldsDefinition fieldsDefinition in (CollectionBase) definitionCollection)
      {
        foreach (CustomFieldDefinition customFieldDefinition in (CollectionBase) fieldsDefinition.CustomFieldDefinitions)
        {
          if (!stringList.Contains(customFieldDefinition.FieldDescription))
            stringList.Add(customFieldDefinition.FieldDescription);
        }
      }
      int fieldIndex = 0;
      foreach (string str in stringList)
      {
        string fieldId = "StdCat";
        this.fieldLists[groupIndex].Add((object) str);
        this.fieldMaps[groupIndex].Add((object) str.ToUpper(), (object) new ContactFieldDialog.FieldMapValue(fieldId, fieldIndex));
        ++fieldIndex;
      }
    }

    private void buildCustomCategoryFieldGroup(int groupIndex)
    {
      if (this.bizCategoryNames == null)
      {
        BizCategory[] bizCategories = this.session.ContactManager.GetBizCategories();
        this.bizCategoryNames = new SortedList<int, string>();
        foreach (BizCategory bizCategory in bizCategories)
          this.bizCategoryNames.Add(bizCategory.CategoryID, bizCategory.Name);
      }
      CustomFieldsDefinitionCollection definitionCollection = CustomFieldsDefinitionCollection.GetCustomFieldsDefinitionCollection(this.session.SessionObjects, new CustomFieldsDefinitionCollection.Criteria(CustomFieldsType.BizCategoryCustom));
      int fieldIndex = 0;
      foreach (CustomFieldsDefinition fieldsDefinition in (CollectionBase) definitionCollection)
      {
        string bizCategoryName = this.bizCategoryNames[fieldsDefinition.RecordId];
        foreach (CustomFieldDefinition customFieldDefinition in (CollectionBase) fieldsDefinition.CustomFieldDefinitions)
        {
          string str = bizCategoryName + " - " + customFieldDefinition.FieldDescription;
          string fieldId = "CstCat" + bizCategoryName;
          this.fieldLists[groupIndex].Add((object) str);
          this.fieldMaps[groupIndex].Add((object) str.ToUpper(), (object) new ContactFieldDialog.FieldMapValue(fieldId, fieldIndex));
          ++fieldIndex;
        }
      }
    }

    private void populateCategoryList()
    {
      this.cboCategoryList.Items.Clear();
      this.cboCategoryList.Items.AddRange((object[]) this.categoryNames);
      this.cboCategoryList.SelectedIndex = this.cboCategoryList.Items.Count > 0 ? 0 : -1;
    }

    private void populateFieldList(int categoryIndex)
    {
      this.cboFieldList.Items.Clear();
      this.cboFieldList.Items.AddRange((object[]) this.fieldLists[categoryIndex].ToArray(typeof (string)));
      this.cboFieldList.SelectedIndex = this.cboFieldList.Items.Count > 0 ? 0 : -1;
      this.btnInsert.Enabled = this.cboFieldList.Items.Count > 0;
    }

    private void insertField(string fieldName, string fieldId)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (fieldId.StartsWith("CstFld"))
      {
        if (this.verifyEncodeDecode(fieldName))
          stringBuilder.Append(WordHandler.EncodeWordIdentifier(fieldName) + "CstFld");
      }
      else if (fieldId.StartsWith("StdCat"))
      {
        if (this.verifyEncodeDecode(fieldName))
          stringBuilder.Append(WordHandler.EncodeWordIdentifier(fieldName) + "StdCat");
      }
      else if (fieldId.StartsWith("CstCat"))
      {
        string str1 = fieldId.Substring("CstCat".Length);
        string str2 = fieldName.Substring(str1.Length + " - ".Length);
        if (this.verifyEncodeDecode(str1) && this.verifyEncodeDecode(str2))
          stringBuilder.Append(WordHandler.EncodeWordIdentifier(str2) + "CstCat" + WordHandler.EncodeWordIdentifier(str1));
      }
      else
        stringBuilder.Append(WordHandler.EncodeWordIdentifier(fieldName) + "_" + fieldId.Replace(".", "dot"));
      if (0 >= stringBuilder.Length)
        return;
      if (char.IsDigit(stringBuilder[0]))
        stringBuilder.Insert(0, '_');
      this.wordHandler.InsertData(stringBuilder.ToString());
    }

    private bool verifyEncodeDecode(string originalString)
    {
      string strB = WordHandler.DecodeWordIdentifier(WordHandler.EncodeWordIdentifier(originalString));
      if (string.Compare(originalString, strB) == 0)
        return true;
      string str = "";
      for (int index = 0; index < originalString.Length; ++index)
      {
        if (index >= strB.Length)
          str = originalString[index].ToString() ?? "";
        else if ((int) originalString[index] != (int) strB[index])
          str = originalString[index].ToString() ?? "";
        if (str != "")
          break;
      }
      int num = (int) Utils.Dialog((IWin32Window) this, string.Format("The field name \"{0}\" contains one or more of the characters ({1}) which are incompatable with the Mail Merge format.  Please modify the field name in order to include it in a Mail Merge document.", (object) originalString, (object) str), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      return false;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void cboCategoryList_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.populateFieldList(this.cboCategoryList.SelectedIndex);
    }

    private void btnInsert_Click(object sender, EventArgs e)
    {
      string fieldName = this.cboFieldList.SelectedItem.ToString();
      if (fieldName == null || string.Empty == fieldName)
        return;
      string fieldId = ((ContactFieldDialog.FieldMapValue) this.fieldMaps[this.cboCategoryList.SelectedIndex][(object) fieldName.ToUpper()]).FieldId;
      this.insertField(fieldName, fieldId);
      this.wordHandler.ActivateDocWindow();
    }

    private void btnFind_Click(object sender, EventArgs e)
    {
      string str = this.txtFind.Text.Trim();
      if (str == null || string.Empty == str)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You must enter a non-empty search string.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        int categoryIndex = this.cboCategoryList.SelectedIndex;
        for (int index = 0; index < this.categoryNames.Length; ++index)
        {
          if (this.fieldMaps[categoryIndex].Contains((object) str.ToUpper()))
          {
            this.populateFieldList(categoryIndex);
            this.cboCategoryList.SelectedIndex = categoryIndex;
            this.cboFieldList.SelectedIndex = ((ContactFieldDialog.FieldMapValue) this.fieldMaps[this.cboCategoryList.SelectedIndex][(object) str.ToUpper()]).FieldIndex;
            return;
          }
          categoryIndex = this.categoryNames.Length - 1 > categoryIndex ? categoryIndex + 1 : 0;
        }
        int num2 = (int) Utils.Dialog((IWin32Window) this, "'" + str + "' is not in any field list.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
      Cursor.Current = Cursors.WaitCursor;
      this.wordHandler.ClickByHand = true;
      this.Close();
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      Cursor.Current = Cursors.WaitCursor;
      this.wordHandler.Save();
    }

    private void InsertFieldDialog_Closing(object sender, CancelEventArgs e)
    {
      this.wordHandler.bInsertFormClosed = true;
      Tracing.Log(ContactFieldDialog.sw, TraceLevel.Verbose, this.className, "Insert Fields form is Closing...");
      this.Closing -= new CancelEventHandler(this.InsertFieldDialog_Closing);
      if (!this.wordHandler.GetSaved())
      {
        switch (Utils.Dialog((IWin32Window) this, "Do you want to save the chages you made to " + this.fsEntry.Name, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
        {
          case DialogResult.Cancel:
            e.Cancel = true;
            return;
          case DialogResult.Yes:
            this.wordHandler.Save();
            break;
          case DialogResult.No:
            this.wordHandler.SetSaved(true);
            break;
        }
      }
      this.wordHandler.ShutDown();
      this.Closing += new CancelEventHandler(this.InsertFieldDialog_Closing);
    }

    private void InsertFieldDialog_Closed(object sender, EventArgs e)
    {
      this.session.Application.Enable();
      Thread.Sleep(5000);
      int num1 = 0;
      BinaryObject data;
      while (true)
      {
        try
        {
          data = new BinaryObject(this.localFile);
          break;
        }
        catch (Exception ex)
        {
          if (5 > num1++)
            return;
        }
        Thread.Sleep(2000);
      }
      try
      {
        this.session.ConfigurationManager.SaveCustomLetter(this.letterType, this.fsEntry, data);
      }
      catch (ServerFileIOException ex)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) null, "Encompass server cannot access the file " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      catch (LocalFileIOException ex)
      {
        int num3 = (int) Utils.Dialog((IWin32Window) null, "Encompass client cannot access the file " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      catch (FileUploadException ex)
      {
        int num4 = (int) Utils.Dialog((IWin32Window) null, "Custom letter cannot be uploaded.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      data?.Dispose();
    }

    private void InitializeComponent()
    {
      this.btnInsert = new Button();
      this.btnSave = new Button();
      this.btnClose = new Button();
      this.btnFind = new Button();
      this.txtFind = new TextBox();
      this.lblCategory = new Label();
      this.cboCategoryList = new ComboBox();
      this.groupBox1 = new GroupBox();
      this.cboFieldList = new ComboBox();
      this.lblField = new Label();
      this.SuspendLayout();
      this.btnInsert.Location = new Point(258, 27);
      this.btnInsert.Name = "btnInsert";
      this.btnInsert.Size = new Size(75, 24);
      this.btnInsert.TabIndex = 5;
      this.btnInsert.Text = "&Insert";
      this.btnInsert.Click += new EventHandler(this.btnInsert_Click);
      this.btnSave.Location = new Point(174, 91);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 24);
      this.btnSave.TabIndex = 8;
      this.btnSave.Text = "&Save";
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.btnClose.Location = new Point(94, 91);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 24);
      this.btnClose.TabIndex = 7;
      this.btnClose.Text = "&Close";
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.btnFind.Location = new Point(258, 54);
      this.btnFind.Name = "btnFind";
      this.btnFind.Size = new Size(75, 24);
      this.btnFind.TabIndex = 5;
      this.btnFind.Text = "&Find";
      this.btnFind.Click += new EventHandler(this.btnFind_Click);
      this.txtFind.Location = new Point(58, 56);
      this.txtFind.Name = "txtFind";
      this.txtFind.Size = new Size(192, 20);
      this.txtFind.TabIndex = 3;
      this.txtFind.Text = "";
      this.lblCategory.AutoSize = true;
      this.lblCategory.Location = new Point(2, 8);
      this.lblCategory.Name = "lblCategory";
      this.lblCategory.Size = new Size(53, 16);
      this.lblCategory.TabIndex = 24;
      this.lblCategory.Text = "Category:";
      this.cboCategoryList.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboCategoryList.Location = new Point(58, 4);
      this.cboCategoryList.Name = "cboCategoryList";
      this.cboCategoryList.Size = new Size(192, 21);
      this.cboCategoryList.TabIndex = 1;
      this.cboCategoryList.SelectedIndexChanged += new EventHandler(this.cboCategoryList_SelectedIndexChanged);
      this.groupBox1.Location = new Point(2, 81);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(332, 4);
      this.groupBox1.TabIndex = 25;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "groupBox1";
      this.cboFieldList.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboFieldList.Location = new Point(58, 29);
      this.cboFieldList.MaxDropDownItems = 20;
      this.cboFieldList.Name = "cboFieldList";
      this.cboFieldList.Size = new Size(192, 21);
      this.cboFieldList.TabIndex = 2;
      this.lblField.AutoSize = true;
      this.lblField.Location = new Point(22, 33);
      this.lblField.Name = "lblField";
      this.lblField.Size = new Size(32, 16);
      this.lblField.TabIndex = 27;
      this.lblField.Text = "Field:";
      this.AcceptButton = (IButtonControl) this.btnInsert;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(338, 119);
      this.Controls.Add((Control) this.lblField);
      this.Controls.Add((Control) this.lblCategory);
      this.Controls.Add((Control) this.txtFind);
      this.Controls.Add((Control) this.cboFieldList);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.cboCategoryList);
      this.Controls.Add((Control) this.btnFind);
      this.Controls.Add((Control) this.btnClose);
      this.Controls.Add((Control) this.btnSave);
      this.Controls.Add((Control) this.btnInsert);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ContactFieldDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.Manual;
      this.Text = "Insert Fields";
      this.TopMost = true;
      this.Closing += new CancelEventHandler(this.InsertFieldDialog_Closing);
      this.Closed += new EventHandler(this.InsertFieldDialog_Closed);
      this.ResumeLayout(false);
    }

    public struct FieldMapValue(string fieldId, int fieldIndex)
    {
      public string FieldId = fieldId;
      public int FieldIndex = fieldIndex;
    }
  }
}
