// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CustomLetters.WordHandler
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.CustomFields;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.ContactUI.CustomFields;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Word;

#nullable disable
namespace EllieMae.EMLite.CustomLetters
{
  public class WordHandler
  {
    private static string className = nameof (WordHandler);
    private static readonly string sw = Tracing.SwCustomLetters;
    public SignaturePointInfo[] SignaturePointList;
    private _Application wordApp;
    private Word.Document wordDoc;
    private bool hasBorrowerSignaturePoints;
    private bool hasCoBorrowerSignaturePoints;
    private DocumentClass docClass;
    private string wordVersion = string.Empty;
    private Form insertForm;
    public bool bInsertFormClosed;
    private static object oFalse = (object) false;
    private static object oTrue = (object) true;
    private static object oMissing = (object) Missing.Value;
    private const string CUSTOM_CONTACT_FIELD_PREFIX = "CstFld";
    private bool wordVisible;
    private Thread docThread;
    private ArrayList tempFiles = new ArrayList();
    private BizCategoryUtil bizCategoryUtil;
    private char FIELD_DELIMITER = '\t';
    private bool clickByHand;
    private static int _wordBackgroundPrint = -1;
    private static float _officeVersion = 0.0f;
    private WdMailMergeActiveRecord activeRecord;
    private static object optOutBookmark = (object) "OptOutBookmark";
    private static object oEndOfDoc = (object) "\\endofdoc";
    private static object oDirStart = (object) WdCollapseDirection.wdCollapseStart;
    private static object oDirEnd = (object) WdCollapseDirection.wdCollapseEnd;
    private Hashtable lockedTable;

    internal static string EncodeWordIdentifier(string source)
    {
      if (source == null || string.Empty == source)
        return string.Empty;
      StringBuilder stringBuilder = new StringBuilder(source);
      for (int index = 0; index < stringBuilder.Length; ++index)
      {
        if (!char.IsLetterOrDigit(stringBuilder[index]))
          stringBuilder.Replace(stringBuilder[index], '_');
      }
      return stringBuilder.ToString();
    }

    internal static string DecodeWordIdentifier(string source)
    {
      return source == null || string.Empty == source ? string.Empty : source.Replace('_', ' ').Trim();
    }

    public _Application WordApp => this.wordApp;

    public Word.Document WordDoc => this.wordDoc;

    public bool HasBorrowerSignaturePoints => this.hasBorrowerSignaturePoints;

    public bool HasCoBorrowerSignaturePoints => this.hasCoBorrowerSignaturePoints;

    internal bool ClickByHand
    {
      get => this.clickByHand;
      set => this.clickByHand = value;
    }

    private void LogVerbose(string msg)
    {
      Tracing.Log(WordHandler.sw, TraceLevel.Verbose, WordHandler.className, msg);
    }

    private void LogInfo(string msg)
    {
      Tracing.Log(WordHandler.sw, TraceLevel.Info, WordHandler.className, msg);
    }

    private void LogError(string msg)
    {
      Tracing.Log(WordHandler.sw, TraceLevel.Error, WordHandler.className, msg);
    }

    public WordHandler()
    {
      this.LogInfo("Enter WordHandler constructor.");
      this.StartWordApp();
      this.LogInfo("Leave WordHandler constructor.");
    }

    private void StartWordApp()
    {
      this.LogInfo("Enter StartWordApp.");
      int num1 = 0;
      this.wordApp = (_Application) null;
      while (this.wordApp == null)
      {
        if (num1 < 5)
        {
          try
          {
            try
            {
              this.wordApp = (_Application) Marshal.GetActiveObject("Word.Application");
              if (this.wordApp != null)
              {
                this.wordVersion = this.wordApp.Version;
                this.LogVerbose("Use existing Word app");
              }
            }
            catch (Exception ex)
            {
              this.wordApp = (_Application) null;
            }
            if (this.wordApp == null)
            {
              this.wordApp = (_Application) new ApplicationClass();
              this.wordVersion = this.wordApp.Version;
              this.LogVerbose("Create new Word app");
            }
            else
              this.wordVisible = this.wordApp.Visible;
          }
          catch (Exception ex)
          {
            this.LogInfo("MS Word might not be installed. Exception is: " + (object) ex);
            this.wordApp = (_Application) null;
          }
          if (this.wordApp == null)
          {
            GC.Collect();
            Thread.Sleep(1000);
          }
          ++num1;
        }
        else
          break;
      }
      if (this.wordApp == null)
      {
        Tracing.Log(WordHandler.sw, TraceLevel.Error, WordHandler.className, "Failed to create instance of MS Word");
        if (!Session.HideUI)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) Session.Application, "The application cannot initialize Microsoft Word . Please confirm that MS Word is properly installed before opening custom letters.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        throw new MSWordNotAvailableException("Please check if Microsoft Word is properly installed. The action of MailMerge will abort.");
      }
      Tracing.Log(WordHandler.sw, TraceLevel.Verbose, WordHandler.className, "Quit Event handler is attached");
      this.LogInfo("Leave StartWordApp.");
    }

    public void CreateNewDoc()
    {
      int num = 0;
      while (num < 5)
      {
        try
        {
          this.wordDoc = this.wordApp.Documents.Add(ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oTrue);
          break;
        }
        catch
        {
          Thread.Sleep(2000);
          this.ShutDown();
          this.StartWordApp();
          ++num;
        }
      }
    }

    public void CreateEmptyDoc(string fileName)
    {
      this.wordApp.Documents.Add(ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oTrue);
      object FileName = (object) fileName;
      this.wordApp.ActiveDocument.SaveAs(ref FileName, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing);
      this.wordApp.ActiveDocument.Close(ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing);
    }

    public void CloseActiveDoc()
    {
      if (this.wordApp == null || this.wordApp.ActiveDocument == null)
        return;
      this.wordApp.ActiveDocument.Close(ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing);
    }

    public void ReplacePointFields()
    {
      foreach (MailMergeField field1 in this.wordDoc.MailMerge.Fields)
      {
        if (field1.Type == WdFieldType.wdFieldMergeField)
        {
          string str1 = field1.Code.Text.Trim();
          string field2 = str1.Substring(str1.LastIndexOf(' ') + 1);
          string fieldFromPointField = ImportCustomFormUtil.GetEMFieldFromPointField(field2);
          if (fieldFromPointField == "")
          {
            field1.Delete();
            ReportLog.AddLog("   Field " + field2 + " is removed because it doesn't have a corresponding field in Encompass.");
          }
          else
          {
            string str2 = str1.Substring(0, str1.LastIndexOf(' ') + 1) + fieldFromPointField;
            field1.Code.Text = str2;
          }
        }
      }
    }

    public void InitEdit(string fileName, bool newFile, Form form)
    {
      this.insertForm = form;
      if (newFile)
      {
        Tracing.Log(WordHandler.sw, TraceLevel.Verbose, WordHandler.className, "Adding a new empty document to collection.");
        this.wordApp.Documents.Add(ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oTrue);
        this.wordDoc = this.wordApp.ActiveDocument;
        object FileName = (object) fileName;
        this.wordApp.ActiveDocument.SaveAs(ref FileName, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing);
      }
      else
        this.OpenAndSetDoc(fileName);
      this.wordApp.Visible = true;
      this.wordApp.Activate();
      this.wordDoc.Activate();
      this.docClass = (DocumentClass) this.wordDoc;
      // ISSUE: method pointer
      this.docClass.add_Close(new DocumentEvents_CloseEventHandler((object) this, (UIntPtr) __methodptr(docClass_DocumentEvents_Event_Close)));
      this.clickByHand = false;
      this.SetBorrowerFlags();
    }

    private void SetBorrowerFlags()
    {
      this.hasBorrowerSignaturePoints = false;
      this.hasCoBorrowerSignaturePoints = false;
      foreach (InlineShape inlineShape in this.wordApp.ActiveDocument.InlineShapes)
      {
        if (inlineShape.AlternativeText == "BorrowerSignature" || inlineShape.AlternativeText == "BorrowerInitials")
          this.hasBorrowerSignaturePoints = true;
        if (inlineShape.AlternativeText == "CoborrowerSignature" || inlineShape.AlternativeText == "CoborrowerInitials")
          this.hasCoBorrowerSignaturePoints = true;
      }
    }

    public bool ImportLetter(string letterFile)
    {
      object FileName = (object) letterFile;
      this.wordApp.Documents.Open(ref FileName, ref WordHandler.oMissing, ref WordHandler.oFalse, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing);
      Tracing.Log(WordHandler.sw, TraceLevel.Verbose, WordHandler.className, "Open Document is successful");
      return this.RemoveMergeFields();
    }

    private bool RemoveMergeFields()
    {
      if (this.wordApp.ActiveDocument == null)
        return false;
      MailMergeFields fields = this.wordApp.ActiveDocument.MailMerge.Fields;
      if (fields.Count <= 0)
      {
        this.wordApp.ActiveDocument.Close(ref WordHandler.oTrue, ref WordHandler.oMissing, ref WordHandler.oMissing);
        this.ShutDown();
        return true;
      }
      foreach (MailMergeField mailMergeField in fields)
        mailMergeField.Delete();
      this.wordApp.ActiveDocument.Close(ref WordHandler.oTrue, ref WordHandler.oMissing, ref WordHandler.oMissing);
      this.ShutDown();
      return true;
    }

    public void InsertData(string sField)
    {
      Selection selection = this.wordApp.Selection;
      this.wordDoc.MailMerge.Fields.Add(selection.Range, sField);
      object Direction = (object) WdCollapseDirection.wdCollapseEnd;
      selection.Collapse(ref Direction);
      this.wordDoc.Activate();
    }

    public void InsertInlineShape(string imageFilename, string alternativeText)
    {
      object End = (object) Missing.Value;
      Word.Range range;
      try
      {
        object start = (object) this.wordApp.Selection.Range.Start;
        object end = (object) this.wordApp.Selection.Range.End;
        range = this.wordApp.ActiveDocument.Range(ref start, ref End);
      }
      catch
      {
        return;
      }
      object LinkToFile = (object) false;
      object SaveWithDocument = (object) true;
      object Range = (object) range;
      try
      {
        this.wordDoc.InlineShapes.AddPicture(imageFilename, ref LinkToFile, ref SaveWithDocument, ref Range).AlternativeText = alternativeText;
        this.wordDoc.Activate();
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message);
      }
    }

    public static Hashtable GetCustLetterList()
    {
      FileSystemEntry[] letterFileEntries = Session.ConfigurationManager.GetAllCustomLetterFileEntries(CustomLetterType.Generic, (string) null);
      Hashtable custLetterList = new Hashtable();
      string empty = string.Empty;
      for (int index = 0; index < letterFileEntries.Length; ++index)
      {
        string upper1 = letterFileEntries[index].Path.ToUpper();
        int num = upper1.LastIndexOf(".");
        if (num > -1 && upper1.Length > 4)
        {
          string upper2 = upper1.Substring(num).Trim().ToUpper();
          if (upper2 == ".DOC" || upper2 == ".RTF" || upper2 == ".DOCX")
          {
            upper1.Substring(0, num).ToUpper();
            custLetterList.Add((object) upper1, (object) upper1);
          }
        }
      }
      return custLetterList;
    }

    public void SetPrinter(string printerName)
    {
      if (this.wordApp == null)
        throw new NullReferenceException("Cannot get active printer name because MS Word is not initialized.");
      try
      {
        object[] args1 = new object[1];
        object[] args2 = new object[1];
        object target = (object) this.wordApp.Dialogs.Item(WdWordDialog.wdDialogFilePrintSetup);
        System.Type type = typeof (Dialog);
        args1[0] = (object) printerName;
        type.InvokeMember("Printer", BindingFlags.SetProperty, (Binder) null, target, args1);
        args2[0] = (object) true;
        type.InvokeMember("DoNotSetAsSysDefault", BindingFlags.SetProperty, (Binder) null, target, args2);
        ((Dialog) target).Execute();
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) null, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void OpenDoc(string docToOpen)
    {
      if (this.wordApp == null)
        throw new NullReferenceException("Cannot open and print documents because MS Word is not initialized.");
      if (docToOpen == null)
        throw new ArgumentNullException(nameof (docToOpen), "Cannot open document because specified document is null.");
      object FileName = File.Exists(docToOpen) ? (object) docToOpen : throw new FileNotFoundException("File not found.", docToOpen);
      Cursor.Current = Cursors.WaitCursor;
      Tracing.Log(WordHandler.sw, TraceLevel.Verbose, WordHandler.className, "Opening Document" + docToOpen + " from Merge Template");
      int num = 5;
      while (num > 0)
      {
        try
        {
          this.wordApp.Documents.Open(ref FileName, ref WordHandler.oMissing, ref WordHandler.oFalse, ref WordHandler.oFalse, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oTrue);
          break;
        }
        catch (Exception ex)
        {
          string msg = "Error opening document: " + ex.Message;
          this.LogInfo(msg);
          --num;
          if (num <= 0)
          {
            if (MessageBox.Show(msg + "\r\n\r\nDo you want to retry?", "Encompass", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
              num = 5;
          }
          else
            Thread.Sleep(1000);
          if (num <= 0)
          {
            this.LogError("OpenDoc: " + ex.Message);
            throw new CannotOpenWordDocumentException("Cannot open " + docToOpen + " in MS Word applicaton. " + ex.Message, ex);
          }
        }
      }
    }

    public void OpenAndSetDoc(string docToOpen)
    {
      this.OpenDoc(docToOpen);
      object Index = (object) docToOpen;
      int num = docToOpen.LastIndexOf("\\");
      if (num > 0)
        Index = (object) docToOpen.Substring(num + 1);
      this.wordDoc = this.wordApp.Documents.Item(ref Index);
    }

    public void CloseDoc()
    {
      if (this.wordApp == null)
        throw new NullReferenceException("Word app instance is null. MS Word is not initialized.");
      try
      {
        if (this.wordDoc == null)
          return;
        this.wordDoc.Saved = true;
        this.wordDoc.Close(ref WordHandler.oTrue, ref WordHandler.oMissing, ref WordHandler.oMissing);
        this.wordDoc = (Word.Document) null;
      }
      catch (Exception ex)
      {
      }
    }

    public void CloseAll()
    {
      try
      {
        while (this.wordApp.Documents.Count > 0)
        {
          object Index = (object) 1;
          Word.Document document = this.wordApp.Documents.Item(ref Index);
          document.Saved = true;
          document.Close(ref WordHandler.oTrue, ref WordHandler.oMissing, ref WordHandler.oMissing);
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(WordHandler.sw, WordHandler.className, TraceLevel.Warning, "Unable to close document: " + ex.Message);
      }
    }

    private static bool wordBackgroundPrint
    {
      get
      {
        if (WordHandler._wordBackgroundPrint == -1)
        {
          WordHandler._wordBackgroundPrint = (EnableDisableSetting) Session.GetPrintingSetting("WordBackgroundPrint", (object) EnableDisableSetting.Enabled) == EnableDisableSetting.Enabled ? 1 : 0;
          using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Ellie Mae\\Encompass", false))
          {
            if (registryKey == null)
            {
              int num = (int) Utils.Dialog((IWin32Window) null, "Registry key HKLM\\Software\\Ellie Mae\\Encompass not set. Please set this registry key.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
              string str = (string) registryKey.GetValue("WordBackgroundPrint");
              if ((str ?? "").Trim() == "0")
                WordHandler._wordBackgroundPrint = 0;
              else if ((str ?? "").Trim() == "1")
                WordHandler._wordBackgroundPrint = 1;
            }
          }
        }
        return WordHandler._wordBackgroundPrint == 1;
      }
    }

    public void PrintDoc(int numCopy, bool bCollate, bool bSynchronous)
    {
      if (this.wordApp == null)
        throw new NullReferenceException("Cannot open and print documents because MS Word is not initialized.");
      if (this.wordDoc == null)
        throw new ArgumentNullException("wordDoc", "Cannot open and print documents because specified document is null.");
      object Collate = (object) bCollate;
      object Copies = (object) numCopy;
      object wordBackgroundPrint = (object) WordHandler.wordBackgroundPrint;
      this.wordDoc.PrintOut(ref wordBackgroundPrint, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref Copies, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref Collate, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing);
      if (!bSynchronous)
        return;
      while (this.wordApp.BackgroundPrintingStatus > 0)
        Thread.Sleep(50);
    }

    public bool GetWordAppVisibility() => this.wordApp.Visible;

    public void SetWordAppVisibility(bool visible)
    {
      int num = 0;
      while (num < 5)
      {
        try
        {
          this.wordApp.Visible = visible;
          break;
        }
        catch
        {
          Thread.Sleep(2000);
          this.ShutDown();
          this.StartWordApp();
          ++num;
        }
      }
    }

    public void SetWordAppWindowStateMax()
    {
      this.wordApp.WindowState = WdWindowState.wdWindowStateMaximize;
    }

    public void ActivateDocWindow()
    {
      object Index = (object) 1;
      this.wordDoc.Windows.Item(ref Index).Activate();
      this.wordApp.Activate();
    }

    public void MergeCustomLetter(string templSrc, LoanData loan)
    {
      if (this.wordApp == null)
        throw new NullReferenceException("Cannot merge documents because MS Word is not initialized.");
      if (templSrc == null)
        throw new ArgumentNullException(nameof (templSrc), "Cannot merge documents because specified document template is null.");
      if (!File.Exists(templSrc))
        throw new FileNotFoundException("File not found.", templSrc);
      Cursor.Current = Cursors.WaitCursor;
      Tracing.Log(WordHandler.sw, TraceLevel.Verbose, WordHandler.className, "Opening Document" + templSrc + " from Merge Template");
      string empty = string.Empty;
      this.OpenDoc(templSrc);
      Word.Document activeDocument = this.wordApp.ActiveDocument;
      try
      {
        string dataSource = this.CreateDataSource(activeDocument, loan);
        this.MergeDataAndDoc(activeDocument, templSrc, false, string.Empty, false);
        this.tempFiles.Add((object) dataSource);
      }
      catch (Exception ex)
      {
        throw new WordHandlerException("MergeCustomLetter Exception", ex);
      }
    }

    private SignaturePointInfo[] gatherSignatureTags(Word.Document doc)
    {
      ArrayList arrayList = new ArrayList();
      foreach (InlineShape inlineShape in doc.InlineShapes)
      {
        if (inlineShape.AlternativeText == "BorrowerSignature" || inlineShape.AlternativeText == "BorrowerInitials" || inlineShape.AlternativeText == "CoborrowerSignature" || inlineShape.AlternativeText == "CoborrowerInitials" || inlineShape.AlternativeText == "OriginatorSignature" || inlineShape.AlternativeText == "OriginatorInitials")
        {
          arrayList.Add((object) new SignaturePointInfo()
          {
            ShapeID = inlineShape.AlternativeText,
            Page = int.Parse(inlineShape.Range.get_Information(WdInformation.wdActiveEndPageNumber).ToString()),
            Height = inlineShape.Height,
            Width = inlineShape.Width,
            VertPos = (float) inlineShape.Range.get_Information(WdInformation.wdVerticalPositionRelativeToPage),
            HorzPos = (float) inlineShape.Range.get_Information(WdInformation.wdHorizontalPositionRelativeToPage)
          });
          inlineShape.PictureFormat.Brightness = 1f;
        }
      }
      SignaturePointInfo[] signaturePointInfoArray = new SignaturePointInfo[arrayList.Count];
      arrayList.CopyTo((Array) signaturePointInfoArray);
      return signaturePointInfoArray;
    }

    public void MergeContactLetter(
      string templSrc,
      ContactType contactType,
      int[] contactIDs,
      bool bEmailMerge,
      string emailSubject,
      string[] emailAddressOption,
      string senderUserID)
    {
      this.MergeContactLetter(templSrc, contactType, contactIDs, bEmailMerge, emailSubject, false, emailAddressOption, senderUserID);
    }

    private static float officeVersion
    {
      get
      {
        if ((double) WordHandler._officeVersion == 0.0)
        {
          using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Office"))
          {
            if (registryKey != null)
            {
              foreach (string subKeyName in registryKey.GetSubKeyNames())
              {
                if (char.IsDigit(subKeyName[0]))
                {
                  try
                  {
                    float num = float.Parse(subKeyName);
                    if ((double) num > (double) WordHandler._officeVersion)
                      WordHandler._officeVersion = num;
                  }
                  catch
                  {
                  }
                }
              }
            }
          }
        }
        return WordHandler._officeVersion;
      }
    }

    private static bool office2007OrAbove => (double) WordHandler.officeVersion >= 12.0;

    private static bool office2003OrAbove => (double) WordHandler.officeVersion >= 11.0;

    public void MergeContactLetter(
      string templSrc,
      ContactType contactType,
      int[] contactIDs,
      bool bEmailMerge,
      string emailSubject,
      bool bShowPrintDlg,
      string[] emailAddressOption,
      string senderUserID)
    {
      this.LogInfo("Enter MergeContactLetter.");
      if (this.wordApp == null)
      {
        this.LogInfo("wordApp is null. A NullReferenceException is thrown.");
        throw new NullReferenceException("Cannot merge documents because MS Word is not initialized.");
      }
      if (templSrc == null)
      {
        this.LogInfo("templSrc is null. A ArgumentNullException is thrown.");
        throw new ArgumentNullException(nameof (templSrc), "Cannot merge documents because specified document template is null.");
      }
      if (contactIDs == null)
      {
        this.LogInfo("contactIDs is null. A ArgumentNullException is thrown.");
        throw new ArgumentNullException(nameof (contactIDs), "Cannot merge documents for contacts because specified contacts are null.");
      }
      if (!File.Exists(templSrc))
      {
        this.LogInfo("File " + templSrc + " does not exist.");
        throw new FileNotFoundException("File not found.", templSrc);
      }
      Cursor.Current = Cursors.WaitCursor;
      this.LogInfo("Opening custom letter template " + templSrc);
      this.OpenDoc(templSrc);
      Word.Document activeDocument = this.wordApp.ActiveDocument;
      string dataSource = this.CreateDataSource(activeDocument, contactType, contactIDs, bEmailMerge, emailAddressOption, senderUserID);
      if (bEmailMerge && Session.ServerIdentity != null && Session.ServerIdentity.IsHttp && (bool) Session.ServerManager.GetServerSetting("Policies.EmailUnsubscribeLink"))
      {
        this.addEmailOptOutLinkPlaceHolder(activeDocument);
        this.addEmailOptOutLink(activeDocument, (string) null);
      }
      this.MergeDataAndDoc(activeDocument, templSrc, bEmailMerge, emailSubject, bShowPrintDlg);
      try
      {
        this.tempFiles.Add((object) dataSource);
      }
      catch
      {
      }
      this.LogInfo("Leave MergeContactLetter.");
    }

    private void addEmailOptOutLinkPlaceHolder(Word.Document templDoc)
    {
      templDoc.Paragraphs.Add(ref WordHandler.oMissing).Range.InsertParagraphAfter();
      Paragraph paragraph = templDoc.Paragraphs.Add(ref WordHandler.oMissing);
      if (WordHandler.office2007OrAbove)
        paragraph.Range.Text = "If you do not wish to receive emails from us in the future, please let us know.";
      else
        paragraph.Range.Text = "\r\n";
    }

    private void word_MailMergeAfterRecordMerge(Word.Document doc)
    {
      ++doc.MailMerge.DataSource.ActiveRecord;
    }

    private void word_MailMergeBeforeRecordMerge(Word.Document doc, ref bool cancel)
    {
      MailMergeDataSource dataSource = doc.MailMerge.DataSource;
      if (dataSource.ActiveRecord.Equals((object) this.activeRecord))
      {
        cancel = true;
      }
      else
      {
        this.activeRecord = dataSource.ActiveRecord;
        string webLink = (string) null;
        foreach (MailMergeDataField dataField in dataSource.DataFields)
        {
          if (dataField.Name == "EMEncompass_EmailOptOut")
            webLink = dataField.Value;
        }
        this.addEmailOptOutLink(doc, webLink);
      }
    }

    private void addEmailOptOutLink(Word.Document templDoc, string webLink)
    {
      try
      {
        Word.Range range1 = templDoc.Bookmarks.Item(ref WordHandler.oEndOfDoc).Range;
        object end = (object) range1.End;
        object Start;
        if (!templDoc.Bookmarks.Exists((string) WordHandler.optOutBookmark))
        {
          Start = end;
          object Range = (object) range1;
          templDoc.Bookmarks.Add((string) WordHandler.optOutBookmark, ref Range);
        }
        else
          Start = (object) templDoc.Bookmarks.Item(ref WordHandler.optOutBookmark).Range.Start;
        Word.Range range2 = templDoc.Range(ref Start, ref end);
        range2.Select();
        range2.Delete(ref WordHandler.oMissing, ref WordHandler.oMissing);
        range2.Select();
        templDoc.Paragraphs.Add(ref WordHandler.oMissing).Range.Text = "If you do not wish to receive emails from us in the future, please " + (webLink == null ? "let us know using the following link.\r\n" : "");
        Word.Range range3 = templDoc.Bookmarks.Item(ref WordHandler.oEndOfDoc).Range;
        object TextToDisplay = webLink == null ? (object) "" : (object) "let us know.";
        object Address = webLink != null ? (object) webLink : (object) "http://www.elliemae.com";
        Hyperlink hyperlink = templDoc.Hyperlinks.Add((object) range3, ref Address, ref WordHandler.oMissing, ref WordHandler.oMissing, ref TextToDisplay, ref WordHandler.oMissing);
        hyperlink.Range.Underline = WdUnderline.wdUnderlineNone;
        hyperlink.Range.Font.Color = WdColor.wdColorBlue;
        if (webLink == null)
          templDoc.MailMerge.Fields.Add(hyperlink.Range, "EMEncompass_EmailOptOut");
        templDoc.Activate();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Error adding email optout link: " + ex.Message);
      }
    }

    public string CreateDataSource(
      Word.Document templDoc,
      ContactType contactType,
      int[] contactIDs,
      bool bEmailMerge,
      string[] emailAddressOption,
      string senderUserID)
    {
      this.LogInfo("Enter CreateDataSource.");
      if (templDoc == null)
      {
        this.LogInfo("templDoc is null. A ArgumentNullException is thrown.");
        throw new ArgumentNullException(nameof (templDoc), "Cannot create data source for MailMerge because specified document template is null.");
      }
      if (contactIDs == null)
      {
        this.LogInfo("contactIDs is null. A ArgumentNullException is thrown.");
        throw new ArgumentNullException(nameof (contactIDs), "Cannot create data source for MailMerge because specified contacts are null.");
      }
      if (this.wordApp == null)
      {
        this.LogInfo("wordApp is null. A NullReferenceException is thrown.");
        throw new NullReferenceException("Cannot create data source for MailMerge because MS Word is not initialized.");
      }
      string str = this.buildHeader(templDoc);
      if (bEmailMerge)
        str = str + this.FIELD_DELIMITER.ToString() + " EMEncompass_DestEmail";
      if (bEmailMerge && Session.ServerIdentity != null && Session.ServerIdentity.IsHttp)
        str = str + this.FIELD_DELIMITER.ToString() + " EMEncompass_EmailOptOut";
      Tracing.Log(WordHandler.sw, TraceLevel.Verbose, WordHandler.className, "Data Source header is prepared.");
      try
      {
        string nameWithExtension = SystemSettings.GetTempFileNameWithExtension("DOC");
        this.LogInfo("Open data source file.");
        string[] fieldNames = new string[0];
        if (str != "")
          fieldNames = str.Split(this.FIELD_DELIMITER);
        this.LogInfo("Writing data source file.");
        using (StreamWriter text = File.CreateText(nameWithExtension))
          this.FillContactLetterRows(text, fieldNames, contactType, contactIDs, bEmailMerge, emailAddressOption, senderUserID);
        string appSetting = EnConfigurationSettings.AppSettings["DataSourceFileCloseBuffer"];
        if (appSetting != "0")
          Thread.Sleep(Convert.ToInt32(appSetting));
        Tracing.Log(WordHandler.sw, TraceLevel.Verbose, WordHandler.className, "Data Source is created");
        this.LogInfo("Calling MailMerge.OpenDataSource.");
        templDoc.MailMerge.OpenDataSource(nameWithExtension, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing);
        this.LogInfo("Data Source is attached to the MailMerge template.");
        return nameWithExtension;
      }
      catch (Exception ex)
      {
        Tracing.Log(WordHandler.sw, TraceLevel.Error, WordHandler.className, ex.StackTrace);
        throw new CannotCreateMailMergeDataSourceException("Encompass cannot create data source for MailMerge. The action will be aborted. \n\n" + ex.Message, ex);
      }
    }

    public string CreateDataSource(Word.Document templDoc, LoanData loan)
    {
      if (templDoc == null)
        throw new ArgumentNullException(nameof (templDoc), "Cannot create data source for MailMerge because specified document template is null.");
      if (this.wordApp == null)
        throw new NullReferenceException("Cannot create data source for MailMerge because MS Word is not initialized.");
      string str = this.buildHeader(templDoc);
      try
      {
        string nameWithExtension = SystemSettings.GetTempFileNameWithExtension("DOC");
        string[] fieldNames = str.Split(this.FIELD_DELIMITER);
        using (StreamWriter text = File.CreateText(nameWithExtension))
          this.FillCustomLetterRows(text, fieldNames, loan);
        string appSetting = EnConfigurationSettings.AppSettings["DataSourceFileCloseBuffer"];
        if (appSetting != "0")
          Thread.Sleep(Convert.ToInt32(appSetting));
        templDoc.MailMerge.OpenDataSource(nameWithExtension, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing);
        Tracing.Log(WordHandler.sw, TraceLevel.Verbose, WordHandler.className, "Data Source is created");
        return nameWithExtension;
      }
      catch (Exception ex)
      {
        Tracing.Log(WordHandler.sw, TraceLevel.Error, WordHandler.className, ex.StackTrace);
        throw new CannotCreateMailMergeDataSourceException("Encompass cannot create data source for MailMerge. The action will be aborted. \n\n" + ex.Message, ex);
      }
    }

    private void FillCustomLetterRows(StreamWriter textSource, string[] fieldNames, LoanData loan)
    {
      for (int index = 0; index < fieldNames.Length; ++index)
      {
        textSource.Write(fieldNames[index]);
        textSource.Write(this.FIELD_DELIMITER);
      }
      textSource.Write("\r\n");
      string[] fieldIds = new string[fieldNames.Length];
      for (int index = 0; index < fieldNames.Length; ++index)
      {
        string str1 = this.DecodeFromHex(fieldNames[index].Trim());
        fieldIds[index] = str1;
        if (!str1.Trim().StartsWith("AT") && !str1.Trim().StartsWith("Document.") && !str1.Trim().StartsWith("UWC.") && !str1.Trim().StartsWith("PRECON.") && !str1.Trim().StartsWith("Condition.") && !str1.Trim().StartsWith("Enhanced.") && !str1.Trim().StartsWith("ENHCOND.") && !str1.Trim().StartsWith("ENHOP.") && !str1.Trim().StartsWith("PostCondition.") && !str1.Trim().StartsWith("PreliminaryCondition.") && !str1.Trim().StartsWith("LoanTeamMember.") && !str1.Trim().StartsWith("Log.MS.") && !str1.Trim().StartsWith("DISCLOSEDGFE.") && !str1.Trim().StartsWith("DISCLOSEDTIL.") && !str1.Trim().StartsWith("DISCLOSEDCD.") && !str1.Trim().StartsWith("DISCLOSEDLE.") && !str1.Trim().StartsWith("LOCKRATE") && !str1.Trim().StartsWith("LR.") && !str1.Trim().StartsWith("Task.") && !str1.Trim().StartsWith("Docs.", StringComparison.CurrentCultureIgnoreCase) || str1.IndexOf("CONDdotTITLES") > -1 || str1.IndexOf("CONDdotOPENdotTITLES") > -1 || str1.IndexOf("CONDdotDESCS") > -1 || str1.IndexOf("CONDdotOPENdotDESCS") > -1)
        {
          int length = str1.LastIndexOf('.');
          int num;
          try
          {
            num = Utils.ParseInt((object) str1.Substring(0, length).Replace("M", ""));
          }
          catch (Exception ex)
          {
            num = 0;
          }
          if (str1.ToLower().EndsWith("regz.day"))
            fieldIds[index] = "REGZ_DAY";
          else if (str1.ToLower().EndsWith("regz.month"))
            fieldIds[index] = "REGZ_MONTH";
          else if (str1.ToLower().EndsWith("regz.yr"))
            fieldIds[index] = "REGZ_YR";
          else if (str1.ToLower().EndsWith("ecoa.name"))
            fieldIds[index] = "ECOA_NAME";
          else if (str1.ToLower().EndsWith("ecoa.addr"))
            fieldIds[index] = "ECOA_ADDR";
          else if (str1.ToLower().EndsWith("ecoa.addr2"))
            fieldIds[index] = "ECOA_ADDR2";
          else if (str1.ToLower().EndsWith("ecoa.city"))
            fieldIds[index] = "ECOA_CITY";
          else if (str1.ToLower().EndsWith("ecoa.phone"))
            fieldIds[index] = "ECOA_PHONE";
          else if (str1.ToLower().EndsWith("ecoa.st"))
            fieldIds[index] = "ECOA_ST";
          else if (str1.ToLower().EndsWith("ecoa.zip"))
            fieldIds[index] = "ECOA_ZIP";
          else if (str1.ToLower().EndsWith("escrow.table"))
            fieldIds[index] = "ESCROW_TABLE";
          else if (str1.ToLower().EndsWith("regz.tabletype"))
            fieldIds[index] = "REGZ_TABLETYPE";
          else if (str1.ToLower().EndsWith("title.table"))
            fieldIds[index] = "TITLE_TABLE";
          else if (str1.ToLower().EndsWith("2010title.table"))
            fieldIds[index] = "2010TITLE_TABLE";
          else if (str1.ToLower().EndsWith("total.cc"))
            fieldIds[index] = "TOTAL_CC";
          else if (str1.ToLower().EndsWith("total.prepaid"))
            fieldIds[index] = "TOTAL_PREPAID";
          else if (str1.ToLower().EndsWith("link.19"))
            fieldIds[index] = "LINK_19";
          else if (str1.ToLower().EndsWith("link.1109"))
            fieldIds[index] = "LINK_1109";
          else if (str1.ToLower().EndsWith("link.4084"))
            fieldIds[index] = "LINK_4084";
          else if (str1.ToLower().EndsWith("link.1172"))
            fieldIds[index] = "LINK_1172";
          else if (str1.ToLower().EndsWith("link.420"))
            fieldIds[index] = "LINK_420";
          else if (str1.ToLower().EndsWith("link.1888"))
            fieldIds[index] = "LINK_1888";
          else if (str1.ToLower().EndsWith("link.4487"))
            fieldIds[index] = "LINK_4487";
          else if (str1.ToLower().EndsWith("link.4488"))
            fieldIds[index] = "LINK_4488";
          else if (str1.ToLower().EndsWith("link.4489"))
            fieldIds[index] = "LINK_4489";
          else if (str1.ToLower().EndsWith("link.4490"))
            fieldIds[index] = "LINK_4490";
          else if (str1.ToLower().EndsWith("link.4493"))
            fieldIds[index] = "LINK_4493";
          else if (str1.ToLower().EndsWith("link.4494"))
            fieldIds[index] = "LINK_4494";
          else if (str1.ToLower().EndsWith("link.364"))
            fieldIds[index] = "LINK_364";
          else if (str1.ToLower().EndsWith("link.2"))
            fieldIds[index] = "LINK_2";
          else if (str1.ToLower().EndsWith("link.228"))
            fieldIds[index] = "LINK_228";
          else if (str1.ToLower().EndsWith("link.229"))
            fieldIds[index] = "LINK_229";
          else if (str1.ToLower().EndsWith("link.234"))
            fieldIds[index] = "LINK_234";
          else if (str1.ToLower().EndsWith("link.qm.x337"))
            fieldIds[index] = "LINK_QM.X337";
          else if (str1.ToLower().EndsWith("link.1264"))
            fieldIds[index] = "LINK_1264";
          else if (str1.ToLower().EndsWith("link.urla.x210"))
            fieldIds[index] = "LINK_URLA.X210";
          else if (str1.ToLower().EndsWith("link.3"))
            fieldIds[index] = "LINK_3";
          else if (str1.ToLower().EndsWith("link.urla.x209"))
            fieldIds[index] = "LINK_URLA.X209";
          else if (str1.ToLower().EndsWith("link.5025"))
          {
            fieldIds[index] = "LINK_5025";
          }
          else
          {
            string str2 = str1.Substring(length + 1);
            fieldIds[index] = str2;
            fieldIds[index] = fieldIds[index].Substring(fieldIds[index].LastIndexOf('_') + 1);
          }
          fieldIds[index] = fieldIds[index].Replace("dot", ".").ToUpper();
          if (num > 1 && num <= 6)
          {
            ref string local = ref fieldIds[index];
            local = local + "#" + (object) num;
          }
        }
      }
      string[] customFormFieldValues = this.GetCustomFormFieldValues(fieldIds, loan);
      string empty = string.Empty;
      for (int index = 0; index < customFormFieldValues.Length; ++index)
      {
        string str = "\"" + customFormFieldValues[index].Replace(this.FIELD_DELIMITER.ToString(), "    ").Replace("\"", "\"\"") + "\"";
        textSource.Write(str);
        textSource.Write(this.FIELD_DELIMITER);
      }
      textSource.Write("\r\n");
    }

    private string DecodeFromHex(string encodedValue)
    {
      string str1 = "";
      if (encodedValue.StartsWith("M_"))
      {
        encodedValue = encodedValue.Substring(2);
        str1 = "M_";
      }
      bool flag = false;
      string str2 = "EN_";
      string str3 = "VF_";
      if (encodedValue.StartsWith(str3))
      {
        flag = true;
        encodedValue = encodedValue.Substring(str3.Length);
      }
      else if (encodedValue.StartsWith(str2))
      {
        flag = true;
        encodedValue = encodedValue.Substring(str2.Length);
      }
      for (int index = 0; index < encodedValue.Length; ++index)
      {
        if (encodedValue[index] == '_')
        {
          if (flag)
          {
            if (index + 1 < encodedValue.Length)
            {
              if (encodedValue[index + 1] == '_')
              {
                try
                {
                  str1 += Utils.HexDecode(encodedValue.Substring(index + 2, 2));
                  index += 3;
                  continue;
                }
                catch
                {
                  str1 += ".";
                  continue;
                }
              }
            }
            str1 += ".";
          }
          else
            str1 += ".";
        }
        else
          str1 += encodedValue[index].ToString();
      }
      return str1;
    }

    private string getAuditValue(
      LoanData loan,
      Dictionary<string, AuditRecord> audit,
      string fieldID)
    {
      string auditFieldId = this.getAuditFieldID(fieldID);
      if (!audit.ContainsKey(auditFieldId))
        return "";
      AuditRecord auditRecord = audit[auditFieldId];
      if (auditRecord == null)
        return "";
      switch (fieldID.Substring(2, 1))
      {
        case "0":
          return auditRecord.UserID;
        case "1":
          return auditRecord.FirstName;
        case "2":
          return auditRecord.LastName;
        case "3":
          return auditRecord.ModifiedDateTime.ToString("G");
        case "4":
          return loan.FormatValue(string.Concat(auditRecord.DataValue), loan.GetFormat(auditFieldId));
        case "5":
          return loan.FormatValue(string.Concat(auditRecord.PreviousValue), loan.GetFormat(auditFieldId));
        default:
          return "";
      }
    }

    private string getAuditFieldID(string fieldID)
    {
      int num = fieldID.IndexOf('.');
      if (num < 0)
        return fieldID;
      fieldID = fieldID.Substring(num + 1).Replace("_", ".");
      string str = "";
      for (int index = 1; index <= 9; ++index)
      {
        if (fieldID.EndsWith(".P" + (object) index))
        {
          str = "#" + (object) index;
          fieldID = fieldID.Substring(0, fieldID.Length - 3);
          break;
        }
        if (fieldID.EndsWith("#" + (object) index))
        {
          str = "#" + (object) index;
          fieldID = fieldID.Substring(0, fieldID.Length - 2);
          break;
        }
      }
      return fieldID + str;
    }

    public string[] GetCustomFormFieldValues(string[] fieldIds, LoanData loan)
    {
      string[] customFormFieldValues = new string[fieldIds.Length];
      PipelineInfo pipelineInfo = (PipelineInfo) null;
      List<string> stringList = new List<string>();
      for (int index = 0; index < fieldIds.Length; ++index)
      {
        if (fieldIds[index].StartsWith("AT"))
        {
          string auditFieldId = this.getAuditFieldID(fieldIds[index]);
          if (!stringList.Contains(auditFieldId))
            stringList.Add(auditFieldId);
        }
      }
      Dictionary<string, AuditRecord> audit = (Dictionary<string, AuditRecord>) null;
      if (stringList.Count > 0)
        audit = Session.LoanManager.GetLastAuditRecordsForLoan(loan.GUID, stringList.ToArray());
      bool flag = false;
      for (int index1 = 0; index1 < fieldIds.Length; ++index1)
      {
        if (loan == null)
          customFormFieldValues[index1] = string.Empty;
        else if (fieldIds[index1].StartsWith("AT"))
        {
          customFormFieldValues[index1] = this.getAuditValue(loan, audit, fieldIds[index1]);
        }
        else
        {
          string upper = fieldIds[index1].ToUpper();
          if (upper == "1895" && !flag && loan != null)
          {
            loan.Calculator.FormCalculation(upper, (string) null, (string) null);
            flag = true;
          }
          if (upper == "LOANFOLDER")
          {
            if (pipelineInfo == null)
              pipelineInfo = loan.ToPipelineInfo();
            customFormFieldValues[index1] = pipelineInfo == null ? string.Empty : pipelineInfo.LoanFolder;
          }
          else if (upper == "_LPID" || upper == "_LOID" || upper == "_CLID")
          {
            customFormFieldValues[index1] = loan.GetField(fieldIds[index1].Substring(1).ToUpper());
          }
          else
          {
            switch (upper)
            {
              case "LOANLASTMODIFIED":
                if (pipelineInfo == null)
                  pipelineInfo = loan.ToPipelineInfo();
                if (pipelineInfo != null && pipelineInfo.Info != null && pipelineInfo.Info.ContainsKey((object) "LastModified"))
                {
                  customFormFieldValues[index1] = pipelineInfo.Info[(object) "LastModified"].ToString();
                  break;
                }
                break;
              case "COMORTGAGORCOUNT":
                int num = 0;
                BorrowerPair[] borrowerPairs = loan.GetBorrowerPairs();
                for (int index2 = 1; index2 < borrowerPairs.Length; ++index2)
                {
                  if (borrowerPairs[index2].Borrower.FirstName.Trim() != string.Empty)
                    ++num;
                  if (borrowerPairs[index2].CoBorrower.FirstName.Trim() != string.Empty)
                    ++num;
                }
                customFormFieldValues[index1] = num.ToString();
                break;
              default:
                customFormFieldValues[index1] = loan.GetField(fieldIds[index1].ToUpper());
                break;
            }
          }
          if (upper == "1393" && (customFormFieldValues[index1] == string.Empty || customFormFieldValues[index1] == null))
          {
            customFormFieldValues[index1] = "Active Loan";
          }
          else
          {
            switch (upper)
            {
              case "663":
                customFormFieldValues[index1] = !(customFormFieldValues[index1] == "Y") ? "does not" : "does";
                continue;
              case "420":
                if (customFormFieldValues[index1] == "FirstLien")
                {
                  customFormFieldValues[index1] = "First Lien";
                  continue;
                }
                if (customFormFieldValues[index1] == "SecondLien")
                {
                  customFormFieldValues[index1] = "Second Lien";
                  continue;
                }
                continue;
              default:
                if ((upper.StartsWith("HTD") || upper.StartsWith("HTR")) && upper.Length == 7 && upper.EndsWith("01"))
                {
                  customFormFieldValues[index1] = customFormFieldValues[index1].Replace(",", "");
                  continue;
                }
                if (upper == "333" && loan.GetField("SYS.X8") == "Y" && customFormFieldValues[index1] != string.Empty)
                {
                  customFormFieldValues[index1] = Utils.ParseDecimal((object) customFormFieldValues[index1]).ToString("N2");
                  continue;
                }
                continue;
            }
          }
        }
      }
      this.lockedTable = (Hashtable) null;
      return customFormFieldValues;
    }

    private string mappingConfirmingLockRequest(string fieldID, LoanData loan)
    {
      if (this.lockedTable == null)
      {
        LogList logList = loan.GetLogList();
        if (logList == null)
          return string.Empty;
        LockRequestLog[] allLockRequests = logList.GetAllLockRequests();
        for (int index = 0; index < allLockRequests.Length; ++index)
        {
          if (allLockRequests[index].RequestedStatus == RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.RateLocked))
          {
            this.lockedTable = allLockRequests[index].GetLockRequestSnapshot();
            break;
          }
        }
        if (this.lockedTable == null)
          return string.Empty;
      }
      if (fieldID.ToUpper().StartsWith("LOCKRATE."))
        fieldID = fieldID.ToUpper().Replace("LOCKRATE.", "");
      return this.lockedTable.ContainsKey((object) fieldID) ? this.lockedTable[(object) fieldID].ToString() : string.Empty;
    }

    private void FillContactLetterRows(
      StreamWriter textSource,
      string[] fieldNames,
      ContactType contactType,
      int[] contactIDs,
      bool bEmailMerge,
      string[] emailAddressOption,
      string senderUserID)
    {
      this.LogInfo("Enter FillContactLetterRows.");
      if (contactType == ContactType.Borrower)
        this.FillBorLetterRows(textSource, fieldNames, contactIDs, bEmailMerge, emailAddressOption, senderUserID);
      else
        this.FillBizLetterRows(textSource, fieldNames, contactIDs, bEmailMerge, emailAddressOption, senderUserID);
      this.LogInfo("Leave FillContactLetterRows.");
    }

    private void FillBizLetterRows(
      StreamWriter textSource,
      string[] fieldDescriptors,
      int[] contactIds,
      bool bEmailMerge,
      string[] emailOptions,
      string senderUserID)
    {
      WordHandler.FieldInfo[] fieldInfoArray = new WordHandler.FieldInfo[fieldDescriptors.Length];
      Dictionary<string, int> dictionary1 = new Dictionary<string, int>();
      List<string> stringList = new List<string>();
      UserInfo userInfo = (UserInfo) null;
      if (senderUserID != null && string.Empty != senderUserID)
        userInfo = Session.OrganizationManager.GetUser(senderUserID);
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      bool flag4 = false;
      for (int index = 0; index < fieldDescriptors.Length; ++index)
      {
        WordHandler.FieldInfo fieldInfo = new WordHandler.FieldInfo(fieldDescriptors[index]);
        fieldInfoArray[index] = fieldInfo;
        if (fieldInfo.IsLoanField)
          flag1 = true;
        if (fieldInfo.IsCustomField)
          flag2 = true;
        if (fieldInfo.IsStandardCategoryField)
          flag3 = true;
        if (fieldInfo.IsCustomCategoryField)
        {
          flag4 = true;
          if (!stringList.Contains(fieldInfo.CategoryName))
            stringList.Add(fieldInfo.CategoryName);
        }
        textSource.Write(fieldInfo.FieldDescriptor);
        textSource.Write(this.FIELD_DELIMITER);
      }
      textSource.WriteLine();
      if (flag2)
      {
        ContactCustomFieldInfoCollection customFieldInfo = Session.ContactManager.GetCustomFieldInfo(ContactType.BizPartner);
        foreach (WordHandler.FieldInfo fieldInfo in fieldInfoArray)
        {
          if (fieldInfo.IsCustomField && !dictionary1.ContainsKey(fieldInfo.ColumnName))
          {
            foreach (ContactCustomFieldInfo contactCustomFieldInfo in customFieldInfo.Items)
            {
              if (contactCustomFieldInfo.Label == fieldInfo.ColumnName)
              {
                dictionary1.Add(fieldInfo.ColumnName, contactCustomFieldInfo.LabelID);
                break;
              }
            }
          }
        }
      }
      for (int index = 0; index < contactIds.Length; ++index)
      {
        int contactId = contactIds[index];
        IContactManager contactManager = Session.ContactManager;
        BizPartnerInfo bizPartner = contactManager.GetBizPartner(contactId);
        if (bizPartner == null)
          throw new Exception("Contact '" + (object) contactId + "' has been deleted");
        ContactLoanInfo contactLoanInfo = (ContactLoanInfo) null;
        if (flag1)
          contactLoanInfo = contactManager.GetLastClosedLoanForContact(contactId, ContactType.BizPartner);
        Dictionary<int, string> dictionary2 = new Dictionary<int, string>();
        if (flag2)
        {
          foreach (ContactCustomField contactCustomField in Session.ContactManager.GetCustomFieldsForContact(contactId, ContactType.BizPartner))
          {
            if (!dictionary2.ContainsKey(contactCustomField.FieldID))
              dictionary2.Add(contactCustomField.FieldID, contactCustomField.FieldValue);
          }
        }
        CustomFieldsDefinition fieldsDefinition = (CustomFieldsDefinition) null;
        CustomFieldValueCollection fieldValueCollection = (CustomFieldValueCollection) null;
        if (this.bizCategoryUtil == null)
          this.bizCategoryUtil = new BizCategoryUtil(Session.SessionObjects);
        string name = this.bizCategoryUtil.CategoryIdToName(bizPartner.CategoryID);
        if (flag3 || flag4 && stringList.Contains(name))
        {
          fieldsDefinition = CustomFieldsDefinition.GetCustomFieldsDefinition(Session.SessionObjects, CustomFieldsType.BizCategoryCustom | CustomFieldsType.BizCategoryStandard, bizPartner.CategoryID);
          fieldValueCollection = CustomFieldValueCollection.GetCustomFieldValueCollection(Session.SessionObjects, new CustomFieldValueCollection.Criteria(bizPartner.ContactID, bizPartner.CategoryID));
        }
        foreach (WordHandler.FieldInfo fieldInfo in fieldInfoArray)
        {
          string str = string.Empty;
          if (fieldInfo.IsBizPartnerField)
          {
            if (fieldInfo.ColumnName == "BizEmail")
            {
              str = bizPartner["DefaultEmail"].ToString();
              if (emailOptions != null && emailOptions.Length > index)
                str = !(emailOptions[index] == ContactUtils.emailAddressOption_Home) ? bizPartner["BizEmail"].ToString() : bizPartner["PersonalEmail"].ToString();
            }
            else
              str = bizPartner[fieldInfo.ColumnName.ToLower()].ToString();
          }
          else if (fieldInfo.IsLoanField && contactLoanInfo != null)
            str = this.FormatLoanField(contactLoanInfo[fieldInfo.ColumnName.ToLower()], fieldInfo.ColumnName);
          else if (fieldInfo.IsCustomField)
          {
            if (dictionary1.ContainsKey(fieldInfo.ColumnName))
            {
              int key = dictionary1[fieldInfo.ColumnName];
              if (dictionary2.ContainsKey(key))
                str = dictionary2[key];
            }
          }
          else if (fieldInfo.IsStandardCategoryField)
          {
            CustomFieldDefinition customFieldDefinition = fieldsDefinition.CustomFieldDefinitions.Find(fieldInfo.ColumnName);
            if (customFieldDefinition != null)
            {
              int fieldId = customFieldDefinition.FieldId;
              CustomFieldValue customFieldValue = fieldValueCollection.Find(fieldId);
              if (customFieldValue != null)
                str = customFieldValue.FieldValue;
            }
          }
          else if (fieldInfo.IsCustomCategoryField && fieldInfo.CategoryName == name)
          {
            CustomFieldDefinition customFieldDefinition = fieldsDefinition.CustomFieldDefinitions.Find(fieldInfo.ColumnName);
            if (customFieldDefinition != null)
            {
              int fieldId = customFieldDefinition.FieldId;
              CustomFieldValue customFieldValue = fieldValueCollection.Find(fieldId);
              if (customFieldValue != null)
                str = customFieldValue.FieldValue;
            }
          }
          else if (fieldInfo.IsSenderField && userInfo != (UserInfo) null)
          {
            switch (fieldInfo.ColumnName.ToLower())
            {
              case "firstname":
                str = userInfo.FirstName;
                break;
              case "lastname":
                str = userInfo.LastName;
                break;
              case "emailaddress":
                str = userInfo.Email;
                break;
              case "phone":
                str = userInfo.Phone;
                break;
              case "cell":
                str = userInfo.CellPhone;
                break;
              case "fax":
                str = userInfo.Fax;
                break;
            }
          }
          else if (fieldInfo.FieldDescriptor == "EMEncompass_EmailOptOut")
            str = this.emailOptOutUrl + "&ContactType=Business&ContactGuid=" + bizPartner.ContactGuid.ToString();
          textSource.Write(str.Replace("\"", "\"\""));
          textSource.Write(this.FIELD_DELIMITER);
        }
        textSource.WriteLine();
      }
    }

    private string emailOptOutUrl
    {
      get
      {
        if (Session.ServerIdentity == null)
          return "http://www.elliemae.com/";
        string str = Session.ServerIdentity.ToString(false, true);
        if (!str.EndsWith("/"))
          str += "/";
        return str + "Unsubscribe.aspx?InstanceName=" + (Session.ServerIdentity.InstanceName ?? "").Trim();
      }
    }

    private void FillBorLetterRows(
      StreamWriter textSource,
      string[] fieldNames,
      int[] contactIDs,
      bool bEmailMerge,
      string[] emailAddressOption,
      string senderUserID)
    {
      this.LogInfo("Enter FillBorLetterRows.");
      this.LogInfo("Write header line to data source file.");
      for (int index = 0; index < fieldNames.Length; ++index)
      {
        textSource.Write(fieldNames[index]);
        textSource.Write(this.FIELD_DELIMITER);
      }
      textSource.WriteLine();
      CompanyInfo companyInfo = Session.ConfigurationManager.GetCompanyInfo();
      ContactCustomFieldInfoCollection customFieldInfo = Session.ContactManager.GetCustomFieldInfo(ContactType.Borrower);
      Dictionary<string, int> dictionary1 = new Dictionary<string, int>();
      foreach (ContactCustomFieldInfo contactCustomFieldInfo in customFieldInfo.Items)
        dictionary1.Add(contactCustomFieldInfo.Label, contactCustomFieldInfo.LabelID);
      UserInfo userInfo = (UserInfo) null;
      if (senderUserID != null && string.Empty != senderUserID)
        userInfo = Session.OrganizationManager.GetUser(senderUserID);
      this.LogInfo("Loop through contacts and create records in data source file.");
      for (int index1 = 0; index1 < contactIDs.Length; ++index1)
      {
        int contactId = contactIDs[index1];
        this.LogInfo("Contact ID: " + (object) contactId);
        IContactManager contactManager = Session.ContactManager;
        this.LogInfo("Get borrower info.");
        BorrowerInfo borrower = contactManager.GetBorrower(contactId);
        if (borrower == null)
          throw new Exception("Contact '" + (object) contactId + "' has been deleted");
        ContactLoanInfo closedLoanForContact = contactManager.GetLastClosedLoanForContact(contactId, ContactType.Borrower);
        UserInfo user = Session.OrganizationManager.GetUser(borrower.OwnerID);
        OrgInfo avaliableOrganization = Session.OrganizationManager.GetFirstAvaliableOrganization(user.OrgId);
        Opportunity opportunityByBorrowerId = contactManager.GetOpportunityByBorrowerId(contactId);
        ContactCustomField[] fieldsForContact = Session.ContactManager.GetCustomFieldsForContact(contactId, ContactType.Borrower);
        Dictionary<int, string> dictionary2 = new Dictionary<int, string>();
        foreach (ContactCustomField contactCustomField in fieldsForContact)
          dictionary2.Add(contactCustomField.FieldID, contactCustomField.FieldValue);
        this.LogInfo("Fill in columns in each record.");
        for (int index2 = 0; index2 < fieldNames.Length; ++index2)
        {
          string str1 = fieldNames[index2].Trim();
          string str2 = string.Empty;
          this.LogInfo("Field name: " + str1);
          if (str1.EndsWith("CstFld"))
          {
            string key1 = WordHandler.DecodeWordIdentifier(str1.Substring(0, str1.Length - "CstFld".Length));
            if (dictionary1.ContainsKey(key1))
            {
              int key2 = dictionary1[key1];
              if (dictionary2.ContainsKey(key2))
                str2 = dictionary2[key2];
            }
          }
          else
          {
            string lower = str1.Substring(str1.LastIndexOf('_') + 1).Replace("dot", ".").ToLower();
            this.LogInfo("Actual field name: " + lower);
            switch (str1)
            {
              case "EMEncompass_DestEmail":
                str2 = borrower["DefaultEmail"].ToString();
                if (emailAddressOption != null && emailAddressOption.Length > index1)
                {
                  str2 = !(emailAddressOption[index1] == ContactUtils.emailAddressOption_Home) ? borrower["BizEmail"].ToString() : borrower["PersonalEmail"].ToString();
                  break;
                }
                break;
              case "EMEncompass_EmailOptOut":
                str2 = this.emailOptOutUrl + "&ContactType=Borrower&ContactGuid=" + borrower.ContactGuid.ToString();
                break;
              default:
                if (lower.StartsWith("borrower"))
                {
                  string columnName = lower.Substring(8);
                  str2 = this.FormatBorrowerField(borrower[columnName], columnName);
                  break;
                }
                if (lower.StartsWith("opp"))
                {
                  if (opportunityByBorrowerId != null)
                  {
                    string columnName = lower.Substring(3);
                    str2 = opportunityByBorrowerId.ColumnValueToString(columnName);
                    break;
                  }
                  break;
                }
                if (lower.StartsWith("loan"))
                {
                  if (closedLoanForContact != null)
                  {
                    string columnName = lower.Substring("loan".Length);
                    if (columnName == "closeddate")
                      columnName = "datecompleted";
                    str2 = this.FormatLoanField(closedLoanForContact[columnName], columnName);
                    break;
                  }
                  str2 = string.Empty;
                  break;
                }
                if (lower.StartsWith("sender") && userInfo != (UserInfo) null)
                {
                  switch (lower.Substring("sender".Length))
                  {
                    case "firstname":
                      str2 = userInfo.FirstName;
                      break;
                    case "lastname":
                      str2 = userInfo.LastName;
                      break;
                    case "emailaddress":
                      str2 = userInfo.Email;
                      break;
                    case "phone":
                      str2 = userInfo.Phone;
                      break;
                    case "cell":
                      str2 = userInfo.CellPhone;
                      break;
                    case "fax":
                      str2 = userInfo.Fax;
                      break;
                  }
                }
                else if (lower.StartsWith("owner"))
                {
                  switch (lower.Substring("owner".Length))
                  {
                    case "emailaddress":
                      str2 = user.Email;
                      break;
                    case "name":
                      str2 = user.FullName;
                      break;
                    case "nmlsid":
                      str2 = user.NMLSOriginatorID;
                      break;
                    case "orgaddress1":
                      str2 = avaliableOrganization == null ? companyInfo.Address : avaliableOrganization.CompanyAddress.Street1;
                      break;
                    case "orgaddress2":
                      str2 = avaliableOrganization == null ? "" : avaliableOrganization.CompanyAddress.Street2;
                      break;
                    case "orgcity":
                      str2 = avaliableOrganization == null ? companyInfo.City : avaliableOrganization.CompanyAddress.City;
                      break;
                    case "orgcode":
                      str2 = avaliableOrganization == null ? "" : avaliableOrganization.OrgCode;
                      break;
                    case "orgfax":
                      str2 = avaliableOrganization == null ? companyInfo.Fax : avaliableOrganization.CompanyFax;
                      break;
                    case "orgname":
                      str2 = avaliableOrganization == null ? companyInfo.Name : avaliableOrganization.CompanyName;
                      break;
                    case "orgphone":
                      str2 = avaliableOrganization == null ? companyInfo.Phone : avaliableOrganization.CompanyPhone;
                      break;
                    case "orgstate":
                      str2 = avaliableOrganization == null ? companyInfo.State : avaliableOrganization.CompanyAddress.State;
                      break;
                    case "orgzip":
                      str2 = avaliableOrganization == null ? companyInfo.Zip : avaliableOrganization.CompanyAddress.Zip;
                      break;
                    case "phonenumber":
                      str2 = user.Phone;
                      break;
                  }
                }
                else
                  break;
                break;
            }
          }
          textSource.Write(str2.Replace("\"", "\"\""));
          textSource.Write(this.FIELD_DELIMITER);
          this.LogInfo("Field filled.");
        }
        textSource.WriteLine();
        this.LogInfo("Record created.");
      }
      this.LogInfo("Leave FillBorLetterRows.");
    }

    private string FormatBorrowerField(object oValue, string columnName)
    {
      if (oValue == null || columnName == null)
        return string.Empty;
      switch (columnName.ToLower())
      {
        case "birthdate":
          DateTime dateTime1 = (DateTime) oValue;
          return dateTime1 == DateTime.MinValue ? "" : dateTime1.ToShortDateString();
        case "anniversary":
          DateTime dateTime2 = (DateTime) oValue;
          return dateTime2 == DateTime.MinValue ? "" : dateTime2.Month.ToString() + "/" + (object) dateTime2.Day;
        case "married":
          return !(bool) oValue ? "" : "X";
        default:
          return oValue.ToString();
      }
    }

    private string FormatLoanField(object oValue, string columnName)
    {
      if (oValue == null || columnName == null)
        return string.Empty;
      columnName = columnName.ToLower();
      switch (columnName)
      {
        case "amortization":
          return AmortizationTypeEnumUtil.ValueToName((AmortizationType) oValue);
        case "appraisedvalue":
          return oValue.GetType() == 1M.GetType() ? ((Decimal) oValue).ToString("c", (IFormatProvider) ContactFormat.NFI_AV) : string.Empty;
        case "datecompleted":
          return oValue.GetType() == new DateTime(1990, 1, 1).GetType() ? ((DateTime) oValue).ToString("d", (IFormatProvider) null) : string.Empty;
        case "downpayment":
          return oValue.GetType() == 1M.GetType() ? ((Decimal) oValue).ToString("c", (IFormatProvider) ContactFormat.NFI_DP) + "%" : string.Empty;
        case "interestrate":
          return oValue.GetType() == 1M.GetType() ? ((Decimal) oValue).ToString("c", (IFormatProvider) ContactFormat.NFI_IR) + "%" : string.Empty;
        case "lienposition":
          return LienEnumUtil.ValueToName((LienEnum) oValue);
        case "loanamount":
          return oValue.GetType() == 1M.GetType() ? ((Decimal) oValue).ToString("c", (IFormatProvider) ContactFormat.NFI_LA) : string.Empty;
        case "loantype":
          return LoanTypeEnumUtil.ValueToName((LoanTypeEnum) oValue);
        case "purpose":
          return LoanPurposeEnumUtil.ValueToName((EllieMae.EMLite.Common.Contact.LoanPurpose) oValue);
        default:
          return oValue.ToString();
      }
    }

    private void MergeDataAndDoc(
      Word.Document templDoc,
      string wordFile,
      bool bEmailMerge,
      string emailSubject,
      bool bShowPrintDlg)
    {
      this.LogInfo("Enter MergeDataAndDoc.");
      MailMerge mailMerge = templDoc.MailMerge;
      this.LogInfo("Getting mail merge state.");
      WdMailMergeState wdMailMergeState = WdMailMergeState.wdMainAndDataSource;
      int num = 5;
      while (num > 0)
      {
        try
        {
          wdMailMergeState = mailMerge.State;
          break;
        }
        catch (Exception ex)
        {
          string msg = "Error getting mail merge state: " + ex.Message;
          this.LogInfo(msg);
          --num;
          if (num <= 0)
          {
            if (MessageBox.Show(msg + "\r\n\r\nDo you want to retry?", "Encompass", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
              num = 5;
          }
          else
            Thread.Sleep(1000);
        }
      }
      if (wdMailMergeState != WdMailMergeState.wdMainAndDataSource)
      {
        Tracing.Log(WordHandler.sw, TraceLevel.Error, WordHandler.className, "Word Merge State is not good " + wdMailMergeState.ToString());
        templDoc.Saved = true;
        templDoc.Close(ref WordHandler.oFalse, ref WordHandler.oMissing, ref WordHandler.oMissing);
        this.ShutDown();
      }
      else
      {
        if (bEmailMerge)
        {
          this.LogInfo("Set MailMerge destination to Email.");
          mailMerge.Destination = WdMailMergeDestination.wdSendToEmail;
          this.LogInfo("Set MailAsAttachment to false.");
          mailMerge.MailAsAttachment = false;
          this.LogInfo("Set Mail AddressFieldName to EMEncompass_DestEmail.");
          mailMerge.MailAddressFieldName = "EMEncompass_DestEmail";
          this.LogInfo("Set MailSubject.");
          mailMerge.MailSubject = emailSubject;
        }
        else
        {
          this.LogInfo("Set MailMerge destination to Document.");
          mailMerge.Destination = WdMailMergeDestination.wdSendToNewDocument;
        }
        this.LogInfo("Calling MailMerge.Execute.");
        mailMerge.Execute(ref WordHandler.oTrue);
        if (!bEmailMerge)
        {
          this.SignaturePointList = this.gatherSignatureTags(this.wordApp.ActiveDocument);
          this.wordApp.ActiveDocument.Saved = true;
          this.wordDoc = this.wordApp.ActiveDocument;
        }
        templDoc.Saved = true;
        templDoc.Close(ref WordHandler.oFalse, ref WordHandler.oMissing, ref WordHandler.oMissing);
        this.LogInfo("Leave MergeDataAndDoc.");
      }
    }

    public static void OpenLetter(string fileName)
    {
      ApplicationClass applicationClass;
      try
      {
        applicationClass = new ApplicationClass();
      }
      catch (Exception ex)
      {
        if (!Session.HideUI)
        {
          int num = (int) Utils.Dialog((IWin32Window) null, "The application cannot initialize Microsoft Word application. Please confirm that MS Word is properly installed before previewing custom letters.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        Tracing.Log(WordHandler.sw, TraceLevel.Verbose, WordHandler.className, "Cannot create Word instance. " + ex.Message);
        return;
      }
      if (applicationClass == null)
        return;
      object FileName = (object) fileName;
      object obj1 = (object) Missing.Value;
      object ReadOnly = (object) false;
      object obj2 = (object) true;
      applicationClass.Documents.Open(ref FileName, ref obj1, ref ReadOnly, ref obj1, ref obj1, ref obj1, ref obj1, ref obj1, ref obj1, ref obj1, ref obj1, ref obj1);
      applicationClass.Visible = true;
      applicationClass.Activate();
    }

    private string buildHeader(Word.Document templDoc)
    {
      this.LogInfo("Enter BuildHeader.");
      if (templDoc == null)
      {
        this.LogInfo("templDoc is null. A ArgumentNullException is thrown.");
        throw new ArgumentNullException("templSrc", "Cannot create data source for MailMerge because specified document template is null.");
      }
      if (this.wordApp == null)
      {
        this.LogInfo("wordApp is null. A NullReferenceException is thrown.");
        throw new NullReferenceException("Cannot create data source for MailMerge because MS Word is not initialized.");
      }
      this.LogInfo("Get a reference to templDoc.MailMerge.");
      MailMerge mailMerge = (MailMerge) null;
      int num = 5;
      while (num > 0)
      {
        try
        {
          mailMerge = templDoc.MailMerge;
          break;
        }
        catch (Exception ex)
        {
          string msg = "Error getting mail merge object: " + ex.Message;
          this.LogInfo(msg);
          --num;
          if (num <= 0)
          {
            if (MessageBox.Show(msg + "\r\n\r\nDo you want to retry?", "Encompass", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
              num = 5;
          }
          else
            Thread.Sleep(1000);
        }
      }
      this.LogInfo("Get a reference to mailMerge.Fields.");
      MailMergeFields fields = mailMerge.Fields;
      if (fields.Count <= 0)
      {
        object Start = (object) (templDoc.Content.End - 1);
        object end = (object) templDoc.Content.End;
        fields.Add(templDoc.Range(ref Start, ref end), "EMDUMMY_FIELD");
      }
      string str1 = "";
      Tracing.Log(WordHandler.sw, TraceLevel.Info, WordHandler.className, fields.Count.ToString() + " fields to merge.");
      foreach (MailMergeField mailMergeField in fields)
      {
        string str2 = mailMergeField.Code.Text.Trim();
        string str3 = str2.IndexOf("MERGEFIELD") <= 0 ? str2.Substring(str2.LastIndexOf(' ') + 1) : "";
        str1 = str1 + str3 + this.FIELD_DELIMITER.ToString() + " ";
      }
      string str4 = str1.Remove(str1.Length - 2, 2);
      this.LogInfo("Leave BuildHeader.");
      return str4;
    }

    public void ShutDown() => this.ShutDown(false);

    public void ShutDown(bool keepVisible)
    {
      if (this.wordApp == null)
        return;
      try
      {
        if (this.wordDoc != null)
        {
          this.wordDoc.Saved = true;
          this.wordDoc.Close(ref WordHandler.oTrue, ref WordHandler.oMissing, ref WordHandler.oMissing);
        }
      }
      catch
      {
      }
      try
      {
        if (this.wordApp != null)
        {
          if (this.wordApp.Documents.Count > 0)
          {
            if (!keepVisible)
            {
              if (!this.wordVisible)
                goto label_11;
            }
            this.wordApp.Visible = true;
            this.CleanupTempFiles();
            return;
          }
        }
      }
      catch (Exception ex)
      {
      }
label_11:
      this.CloseAll();
      Tracing.Log(WordHandler.sw, TraceLevel.Info, WordHandler.className, "Shut down MS Word software");
      try
      {
        if (this.wordApp != null)
        {
          this.wordApp.Quit(ref WordHandler.oFalse, ref WordHandler.oMissing, ref WordHandler.oMissing);
          Marshal.ReleaseComObject((object) this.wordApp);
        }
      }
      catch (COMException ex)
      {
        Tracing.Log(WordHandler.sw, TraceLevel.Info, WordHandler.className, "Error while shutting down Word");
        Tracing.Log(WordHandler.sw, TraceLevel.Info, WordHandler.className, "Error Code: " + ex.ErrorCode.ToString());
        Tracing.Log(WordHandler.sw, TraceLevel.Info, WordHandler.className, "Error Code: " + ex.Message);
      }
      finally
      {
        this.wordApp = (_Application) null;
      }
      this.CleanupTempFiles();
    }

    private void OnQuit()
    {
      Tracing.Log(WordHandler.sw, TraceLevel.Verbose, WordHandler.className, "MS Word is closing...");
      if (this.wordDoc != null)
        this.wordDoc.Saved = true;
      this.insertForm.Close();
      Tracing.Log(WordHandler.sw, TraceLevel.Verbose, WordHandler.className, "Insert Fields form is closed");
    }

    public bool GetSaved()
    {
      bool flag = true;
      bool saved = true;
      int num = 0;
      for (; flag; flag = false)
      {
        try
        {
          saved = this.wordDoc == null || this.wordDoc.Saved;
        }
        catch (COMException ex)
        {
          Tracing.Log(WordHandler.sw, TraceLevel.Info, WordHandler.className, "Error while reading Word's Saved property");
          if (++num >= 10)
            throw ex;
        }
      }
      return saved;
    }

    public void SetSaved(bool bValue)
    {
      bool flag = true;
      int num = 0;
      for (; flag; flag = false)
      {
        try
        {
          if (this.wordDoc != null)
            this.wordDoc.Saved = bValue;
        }
        catch (COMException ex)
        {
          Tracing.Log(WordHandler.sw, TraceLevel.Info, WordHandler.className, "Error while writting Word's Saved property");
          if (++num >= 10)
            throw ex;
        }
      }
    }

    public void Save()
    {
      if (this.wordDoc == null)
        return;
      this.wordDoc.Save();
      this.SetBorrowerFlags();
    }

    public void SaveAs(string fileName) => this.SaveAs(fileName, WdSaveFormat.wdFormatDocument);

    public void SaveAs(string fileName, int format) => this.SaveAs(fileName, (WdSaveFormat) format);

    public void SaveAs(string fileName, WdSaveFormat format)
    {
      object FileName = (object) fileName;
      object FileFormat = (object) format;
      if (this.wordDoc == null)
        return;
      this.wordDoc.SaveAs(ref FileName, ref FileFormat, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing, ref WordHandler.oMissing);
    }

    public void CleanupTempFiles()
    {
      foreach (string tempFile in this.tempFiles)
      {
        try
        {
          File.Delete(tempFile);
        }
        catch
        {
        }
      }
    }

    private void docClass_DocumentEvents_Event_Close()
    {
      if (this.clickByHand || this.bInsertFormClosed)
        return;
      this.insertForm.Invoke((Delegate) new WordHandler.RemoveForm(this.insertForm.Close));
    }

    private void uploadCustomForm()
    {
      this.insertForm.Close();
      this.docThread.Abort();
    }

    private delegate void RemoveForm();

    private class FieldInfo
    {
      private const string CUSTOM_CONTACT_FIELD_PREFIX = "CstFld";
      private const string STANDARD_CATEGORY_FIELD_PREFIX = "StdCat";
      private const string CUSTOM_CATEGORY_FIELD_PREFIX = "CstCat";
      private const string BIZ_PARTNER_TABLE = "BizPartner";
      private const string LOAN_TABLE = "Loan";
      private const string CUSTOM_CONTACT_FIELD_TABLE = "BizCustomField";
      private const string CATEGORY_FIELD_TABLE = "BizCategoryCustomFieldValue";
      private const string SENDER_TABLE = "Sender";
      private const string BIZ_EMAIL_ALIAS_NAME = "EMEncompass_DestEmail";
      public const string BIZ_EMAIL_COLUMN_NAME = "BizEmail";
      public const string BIZ_HOME_EMAIL_COLUMN_NAME = "PersonalEmail";
      private const string DATE_COMPLETED_ALIAS_NAME = "ClosedDate";
      private const string DATE_COMPLETED_COLUMN_NAME = "DateCompleted";
      private const string CATEGORY_FIELD_COLUMN = "FieldValue";
      private string fieldDescriptor = string.Empty;
      private string tableName = string.Empty;
      private string columnName = string.Empty;
      private string categoryName = string.Empty;
      private bool isBizPartnerField;
      private bool isLoanField;
      private bool isCustomField;
      private bool isStandardCategoryField;
      private bool isCustomCategoryField;
      private bool isSenderField;

      public string FieldDescriptor => this.fieldDescriptor;

      public string TableName => this.tableName;

      public string ColumnName => this.columnName;

      public string CategoryName => this.categoryName;

      public bool IsBizPartnerField => this.isBizPartnerField;

      public bool IsLoanField => this.isLoanField;

      public bool IsCustomField => this.isCustomField;

      public bool IsStandardCategoryField => this.isStandardCategoryField;

      public bool IsCustomCategoryField => this.isCustomCategoryField;

      public bool IsSenderField => this.isSenderField;

      public FieldInfo(string descriptorString)
      {
        this.fieldDescriptor = descriptorString.Trim();
        if (this.fieldDescriptor.EndsWith("CstFld", StringComparison.CurrentCultureIgnoreCase))
        {
          this.tableName = "BizCustomField";
          this.columnName = WordHandler.DecodeWordIdentifier(this.fieldDescriptor.Substring(0, this.fieldDescriptor.Length - "CstFld".Length));
          this.isCustomField = true;
        }
        else if (this.fieldDescriptor.EndsWith("StdCat", StringComparison.CurrentCultureIgnoreCase))
        {
          this.tableName = "BizCategoryCustomFieldValue";
          this.columnName = WordHandler.DecodeWordIdentifier(this.fieldDescriptor.Substring(0, this.fieldDescriptor.Length - "StdCat".Length));
          this.isStandardCategoryField = true;
        }
        else if (-1 != this.fieldDescriptor.IndexOf("CstCat", StringComparison.CurrentCultureIgnoreCase))
        {
          this.tableName = "BizCategoryCustomFieldValue";
          int length = this.fieldDescriptor.IndexOf("CstCat");
          this.columnName = WordHandler.DecodeWordIdentifier(this.fieldDescriptor.Substring(0, length));
          this.categoryName = WordHandler.DecodeWordIdentifier(this.fieldDescriptor.Substring(length + "CstCat".Length));
          this.isCustomCategoryField = true;
        }
        else
        {
          int length = this.fieldDescriptor.LastIndexOf('_');
          this.fieldDescriptor.Substring(0, length);
          string str = this.fieldDescriptor.Substring(length + 1).Replace("dot", ".");
          if (string.Compare("EMEncompass_DestEmail", this.fieldDescriptor, true) == 0)
          {
            this.tableName = "BizPartner";
            this.columnName = "BizEmail";
            this.isBizPartnerField = true;
          }
          else if (str.StartsWith("BizPartner", StringComparison.CurrentCultureIgnoreCase))
          {
            this.tableName = "BizPartner";
            this.columnName = str.Substring("BizPartner".Length);
            this.isBizPartnerField = true;
          }
          else if (str.StartsWith("Loan", StringComparison.CurrentCultureIgnoreCase))
          {
            this.tableName = "Loan";
            this.columnName = str.Substring("Loan".Length);
            if (this.columnName == "ClosedDate")
              this.columnName = "DateCompleted";
            this.isLoanField = true;
          }
          else
          {
            if (!str.StartsWith("Sender", StringComparison.CurrentCultureIgnoreCase))
              return;
            this.tableName = "Sender";
            this.columnName = str.Substring("Sender".Length);
            this.isSenderField = true;
          }
        }
      }
    }
  }
}
