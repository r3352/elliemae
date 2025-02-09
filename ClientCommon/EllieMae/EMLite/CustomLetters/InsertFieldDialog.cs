// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CustomLetters.InsertFieldDialog
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DocEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.Encompass.AsmResolver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.CustomLetters
{
  internal class InsertFieldDialog : Form
  {
    private string className = nameof (InsertFieldDialog);
    private static readonly string sw = Tracing.SwCustomLetters;
    private Button fndBtn;
    private Button newBtn;
    private IContainer components;
    private Button insertBtn;
    private Button saveBtn;
    private Button closeBtn;
    private TextBox findTxt;
    private Label label1;
    private ComboBox catagoryCbo;
    private Label label3;
    private Label label4;
    private ComboBox listCbo;
    private TextBox idTxt;
    private WordHandler wordHandler;
    private FileSystemEntry fsEntry;
    private string localFile;
    private CustomLetterPanel parentForm;
    private string[] fldGroups;
    private ForBorrowerType originalIntendedFor;
    private ForBorrowerType currentIntendedFor;
    private bool hasBorrowerFields;
    private bool hasCoBorrowerFields;
    private Hashtable fieldTbl = new Hashtable();
    private List<LoanXDBField> auditTbl = new List<LoanXDBField>();
    private IMainScreen mainScreen;
    private Dictionary<string, string> auditTrailFieldList = new Dictionary<string, string>();
    private const string auditTrail = "Audit Trail";
    private const string docTracking = "Document Tracking";
    private const string tasks = "Tasks";
    private const string condTracking = "Condition Tracking";
    private const string preliminaryCond = "Preliminary Condition";
    private const string postClosingCond = "Post Closing Condition";
    private const string loanTeamMember = "Loan Team Member";
    private const string milestone = "Milestone";
    private const string rateLock = "Ratelock";
    private const string gfeDisclosure = "GFE Disclosure";
    private const string leDisclosure = "LE Disclosure";
    private const string cdDisclosure = "CD Disclosure";
    private const string tilDisclosure = "TIL Disclosure";
    private const string closingDocs = "Closing Documents";
    public const string RecentRateLock = "Most Recent Lock Fields";
    public const string PreviousRateLock = "Previous Lock Fields";
    public const string MostRecentRequest = "Most Recent Request Fields";
    public const string RecentRateLockRequest = "Most Recent Lock Request Fields";
    public const string PreviousRateLockRequest = "Previous Lock Request Fields";
    public const string SecondPreviousRateLockRequest = "2nd Previous Lock Request Fields";
    public const string RecentRateLockDeny = "Most Recent Lock Deny Fields";
    public const string PreviousRateLockDeny = "Previous Lock Deny Fields";
    public const string SecondPreviousRateLockDeny = "2nd Previous Lock Deny Fields";
    public const string enhancedConditions = "Enhanced Conditions (multiple conditions)";
    public const string enhancedCondition = "Enhanced Condition (single condition)";
    public const string enhancedConditionOps = "Enhanced Conditions (By Options)";
    private PictureBox coborInitialsPic;
    private PictureBox borInitialsPic;
    private PictureBox coborSignaturePic;
    private Label label2;
    private PictureBox borSignaturePic;
    private ImageList signatureImageLst;
    private ImageList initialsImageLst;
    public const string SecondPreviousRateLock = "2nd Previous Lock Fields";
    private PictureBox originatorSignaturePic;
    private PictureBox originatorInitialsPic;
    private GroupContainer grpFields;
    private Label label5;
    private GroupContainer grpSigPoints;
    private FieldSettings fieldSettings;
    private SplitContainer splitContainer1;
    private ComboBox cboIntendedFor;
    private Label label6;
    private PictureBox borInitialGrey;
    private PictureBox borSignatureGrey;
    private PictureBox coborInitialGrey;
    private PictureBox coborSignatureGrey;
    private Sessions.Session session;

    public event EventHandler CustomFormDetailChanged;

    public InsertFieldDialog(
      string localFile,
      GVItem lvItem,
      CustomLetterPanel parentForm,
      IMainScreen mainScreen,
      Sessions.Session session)
    {
      this.fsEntry = (FileSystemEntry) lvItem.Tag;
      this.session = session;
      this.localFile = localFile;
      this.mainScreen = mainScreen;
      this.fieldSettings = this.session.LoanManager.GetFieldSettings();
      this.InitializeComponent();
      this.Location = new Point(0, 0);
      this.parentForm = parentForm;
      this.prepopulateFieldList();
      this.cboIntendedFor.Items.Add((object) EnumUtil.GetEnumDescription((Enum) ForBorrowerType.All));
      this.cboIntendedFor.Items.Add((object) EnumUtil.GetEnumDescription((Enum) ForBorrowerType.Borrower));
      this.cboIntendedFor.Items.Add((object) EnumUtil.GetEnumDescription((Enum) ForBorrowerType.CoBorrower));
      this.originalIntendedFor = ForBorrowerType.All;
      this.currentIntendedFor = this.originalIntendedFor;
      this.cboIntendedFor.Text = EnumUtil.GetEnumDescription((Enum) this.originalIntendedFor);
    }

    private void prepopulateFieldList()
    {
      this.auditTrailFieldList.Clear();
      this.auditTrailFieldList.Add("Last Modified By", "AT0");
      this.auditTrailFieldList.Add("Last Modified By (First Name)", "AT1");
      this.auditTrailFieldList.Add("Last Modified By (Last Name)", "AT2");
      this.auditTrailFieldList.Add("Last Modified Date", "AT3");
      this.auditTrailFieldList.Add("Last Modified Value", "AT4");
      this.auditTrailFieldList.Add("Previous Value", "AT5");
    }

    public bool InitializeFieldList(bool bFileNew)
    {
      if (!this.populateCatagory())
        return false;
      this.buildFieldList(0);
      try
      {
        this.wordHandler = new WordHandler();
        this.wordHandler.InitEdit(this.localFile, bFileNew, (Form) this);
        this.hasBorrowerFields = this.wordHandler.HasBorrowerSignaturePoints;
        this.hasCoBorrowerFields = this.wordHandler.HasCoBorrowerSignaturePoints;
        this.session.Application.Disable();
        this.Show();
        this.listCbo.Focus();
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        Tracing.Log(InsertFieldDialog.sw, TraceLevel.Error, this.className, "Error occurred in MailMerge.");
        Tracing.Log(InsertFieldDialog.sw, TraceLevel.Error, this.className, ex.StackTrace);
        if (this.wordHandler != null)
          this.wordHandler.ShutDown();
      }
      return true;
    }

    private bool populateCatagory()
    {
      string resourceFileFullPath = AssemblyResolver.GetResourceFileFullPath(SystemSettings.CustomLetterMapRelPath, SystemSettings.LocalAppDir);
      if (!File.Exists(resourceFileFullPath))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "'LetterMap.Txt' file not found in folder " + resourceFileFullPath + ".", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      this.catagoryCbo.Items.Clear();
      string empty = string.Empty;
      string end;
      try
      {
        StreamReader streamReader = new StreamReader((Stream) new FileStream(resourceFileFullPath, FileMode.Open, FileAccess.Read, FileShare.Read));
        end = streamReader.ReadToEnd();
        streamReader.Close();
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        Tracing.Log(InsertFieldDialog.sw, TraceLevel.Error, this.className, "Message " + ex.Message);
        return false;
      }
      if (end == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The 'LetterMap.Txt' file format is not correct.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      this.fldGroups = end.Split('#');
      if (this.fldGroups.Length == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The 'LetterMap.Txt' file format is not correct.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      for (int index = 0; index < this.fldGroups.Length; ++index)
      {
        string fldGroup = this.fldGroups[index];
        int length = fldGroup.IndexOf("]");
        if (length > -1)
          this.catagoryCbo.Items.Add((object) fldGroup.Substring(0, length).Trim().Replace(Environment.NewLine, "").Replace("[", "").Replace("]", ""));
        string str = fldGroup.Substring(length + 1);
        if (str.IndexOf(Environment.NewLine) == 0)
          str = str.Substring(2);
        if (str.Length > 2 && str.EndsWith(Environment.NewLine))
          str = str.Substring(0, str.Length - 2);
        this.fldGroups[index] = str.Replace(Environment.NewLine, "@");
      }
      if (this.session.EncompassEdition == EncompassEdition.Banker)
      {
        this.catagoryCbo.Items.Add((object) "Audit Trail");
        this.catagoryCbo.Items.Add((object) "Condition Tracking");
      }
      this.catagoryCbo.Items.Add((object) "Enhanced Condition (single condition)");
      this.catagoryCbo.Items.Add((object) "Enhanced Conditions (multiple conditions)");
      this.catagoryCbo.Items.Add((object) "Enhanced Conditions (By Options)");
      this.catagoryCbo.Items.Add((object) "Document Tracking");
      this.catagoryCbo.Items.Add((object) "Tasks");
      this.catagoryCbo.Items.Add((object) "Loan Team Member");
      this.catagoryCbo.Items.Add((object) "Milestone");
      if (this.session.EncompassEdition == EncompassEdition.Banker)
      {
        this.catagoryCbo.Items.Add((object) "Preliminary Condition");
        this.catagoryCbo.Items.Add((object) "Post Closing Condition");
        this.catagoryCbo.Items.Add((object) "Ratelock");
      }
      this.catagoryCbo.Items.Add((object) "GFE Disclosure");
      this.catagoryCbo.Items.Add((object) "LE Disclosure");
      this.catagoryCbo.Items.Add((object) "CD Disclosure");
      this.catagoryCbo.Items.Add((object) "TIL Disclosure");
      if (this.session.Application.GetService<ILoanServices>().IsEncompassDocServiceAvailable(DocumentOrderType.Closing))
        this.catagoryCbo.Items.Add((object) "Closing Documents");
      if (this.catagoryCbo.Items.Count > 0)
        this.catagoryCbo.SelectedIndex = 0;
      return true;
    }

    private void buildFieldList(int order)
    {
      this.listCbo.Items.Clear();
      this.listCbo.Sorted = false;
      if (order + 1 <= this.fldGroups.Length)
      {
        string fldGroup = this.fldGroups[order];
        char[] chArray = new char[1]{ '@' };
        foreach (string str in fldGroup.Split(chArray))
        {
          int length = str.IndexOf("|");
          if (length >= 0)
          {
            string key = str.Substring(0, length).Trim();
            string upper = str.Substring(length + 1).Trim().ToUpper();
            this.listCbo.Items.Add((object) key);
            this.fieldTbl[(object) key] = (object) upper;
          }
        }
      }
      else
      {
        switch (this.catagoryCbo.Text)
        {
          case "Audit Trail":
            using (Dictionary<string, string>.KeyCollection.Enumerator enumerator = this.auditTrailFieldList.Keys.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                string current = enumerator.Current;
                this.listCbo.Items.Add((object) current);
                this.fieldTbl[(object) current] = (object) this.auditTrailFieldList[current];
              }
              break;
            }
          case "CD Disclosure":
            this.listCbo.Sorted = true;
            IEnumerator enumerator1 = LastDisclosedCDFields.All.GetEnumerator();
            try
            {
              while (enumerator1.MoveNext())
                this.listCbo.Items.Add((object) new FieldDefinitionObj((FieldDefinition) enumerator1.Current));
              break;
            }
            finally
            {
              if (enumerator1 is IDisposable disposable)
                disposable.Dispose();
            }
          case "Closing Documents":
            this.listCbo.Sorted = true;
            IEnumerator enumerator2 = DocEngineFieldList.All.GetEnumerator();
            try
            {
              while (enumerator2.MoveNext())
                this.listCbo.Items.Add((object) new FieldDefinitionObj((FieldDefinition) enumerator2.Current));
              break;
            }
            finally
            {
              if (enumerator2 is IDisposable disposable)
                disposable.Dispose();
            }
          case "Condition Tracking":
            IEnumerator enumerator3 = UnderwritingConditionFields.All.GetEnumerator();
            try
            {
              while (enumerator3.MoveNext())
                this.listCbo.Items.Add((object) new FieldDefinitionObj((FieldDefinition) enumerator3.Current));
              break;
            }
            finally
            {
              if (enumerator3 is IDisposable disposable)
                disposable.Dispose();
            }
          case "Document Tracking":
            IEnumerator enumerator4 = DocumentTrackingFields.All.GetEnumerator();
            try
            {
              while (enumerator4.MoveNext())
                this.listCbo.Items.Add((object) new FieldDefinitionObj((FieldDefinition) enumerator4.Current));
              break;
            }
            finally
            {
              if (enumerator4 is IDisposable disposable)
                disposable.Dispose();
            }
          case "Enhanced Condition (single condition)":
            IEnumerator enumerator5 = EnhancedConditionField.All.GetEnumerator();
            try
            {
              while (enumerator5.MoveNext())
                this.listCbo.Items.Add((object) new FieldDefinitionObj((FieldDefinition) enumerator5.Current));
              break;
            }
            finally
            {
              if (enumerator5 is IDisposable disposable)
                disposable.Dispose();
            }
          case "Enhanced Conditions (By Options)":
            IEnumerator enumerator6 = EnhancedConditionFieldByOption.All.GetEnumerator();
            try
            {
              while (enumerator6.MoveNext())
                this.listCbo.Items.Add((object) new FieldDefinitionObj((FieldDefinition) enumerator6.Current));
              break;
            }
            finally
            {
              if (enumerator6 is IDisposable disposable)
                disposable.Dispose();
            }
          case "Enhanced Conditions (multiple conditions)":
            IEnumerator enumerator7 = EnhancedConditionFields.All.GetEnumerator();
            try
            {
              while (enumerator7.MoveNext())
                this.listCbo.Items.Add((object) new FieldDefinitionObj((FieldDefinition) enumerator7.Current));
              break;
            }
            finally
            {
              if (enumerator7 is IDisposable disposable)
                disposable.Dispose();
            }
          case "GFE Disclosure":
            this.listCbo.Sorted = true;
            IEnumerator enumerator8 = LastDisclosedGFEFields.All.GetEnumerator();
            try
            {
              while (enumerator8.MoveNext())
                this.listCbo.Items.Add((object) new FieldDefinitionObj((FieldDefinition) enumerator8.Current));
              break;
            }
            finally
            {
              if (enumerator8 is IDisposable disposable)
                disposable.Dispose();
            }
          case "LE Disclosure":
            this.listCbo.Sorted = true;
            IEnumerator enumerator9 = LastDisclosedLEFields.All.GetEnumerator();
            try
            {
              while (enumerator9.MoveNext())
                this.listCbo.Items.Add((object) new FieldDefinitionObj((FieldDefinition) enumerator9.Current));
              break;
            }
            finally
            {
              if (enumerator9 is IDisposable disposable)
                disposable.Dispose();
            }
          case "Loan Team Member":
            IEnumerator enumerator10 = LoanAssociateFields.All.GetEnumerator();
            try
            {
              while (enumerator10.MoveNext())
                this.listCbo.Items.Add((object) new FieldDefinitionObj((FieldDefinition) enumerator10.Current));
              break;
            }
            finally
            {
              if (enumerator10 is IDisposable disposable)
                disposable.Dispose();
            }
          case "Milestone":
            IEnumerator enumerator11 = MilestoneFields.All.GetEnumerator();
            try
            {
              while (enumerator11.MoveNext())
              {
                FieldDefinition current = (FieldDefinition) enumerator11.Current;
                if (!(current is CoreMilestoneField))
                  this.listCbo.Items.Add((object) new FieldDefinitionObj(current));
              }
              break;
            }
            finally
            {
              if (enumerator11 is IDisposable disposable)
                disposable.Dispose();
            }
          case "Post Closing Condition":
            IEnumerator enumerator12 = PostClosingConditionFields.All.GetEnumerator();
            try
            {
              while (enumerator12.MoveNext())
                this.listCbo.Items.Add((object) new FieldDefinitionObj((FieldDefinition) enumerator12.Current));
              break;
            }
            finally
            {
              if (enumerator12 is IDisposable disposable)
                disposable.Dispose();
            }
          case "Preliminary Condition":
            IEnumerator enumerator13 = PreliminaryConditionFields.All.GetEnumerator();
            try
            {
              while (enumerator13.MoveNext())
                this.listCbo.Items.Add((object) new FieldDefinitionObj((FieldDefinition) enumerator13.Current));
              break;
            }
            finally
            {
              if (enumerator13 is IDisposable disposable)
                disposable.Dispose();
            }
          case "Ratelock":
            this.listCbo.Sorted = true;
            Dictionary<string, FieldDefinitionObj> dictionary = new Dictionary<string, FieldDefinitionObj>();
            foreach (FieldDefinition lockRequestField in EncompassFields.GetAllLockRequestFields(this.fieldSettings))
            {
              if (lockRequestField is RateLockField)
              {
                RateLockField rateLockField = (RateLockField) lockRequestField;
                FieldDefinitionObj fieldDefinitionObj;
                switch (rateLockField.GetRateLockOrder)
                {
                  case RateLockField.RateLockOrder.MostRecent:
                    fieldDefinitionObj = new FieldDefinitionObj("Most Recent Lock Fields", lockRequestField.InstanceSpecifierType, true, rateLockField.FieldID);
                    break;
                  case RateLockField.RateLockOrder.Previous:
                    fieldDefinitionObj = new FieldDefinitionObj("Previous Lock Fields", lockRequestField.InstanceSpecifierType, true, rateLockField.FieldID);
                    break;
                  case RateLockField.RateLockOrder.MostRecentRequest:
                    fieldDefinitionObj = new FieldDefinitionObj("Most Recent Request Fields", lockRequestField.InstanceSpecifierType, true, rateLockField.FieldID);
                    break;
                  case RateLockField.RateLockOrder.MostRecentDenied:
                    fieldDefinitionObj = new FieldDefinitionObj("Most Recent Lock Deny Fields", lockRequestField.InstanceSpecifierType, true, rateLockField.FieldID);
                    break;
                  case RateLockField.RateLockOrder.PreviousDenied:
                    fieldDefinitionObj = new FieldDefinitionObj("Previous Lock Deny Fields", lockRequestField.InstanceSpecifierType, true, rateLockField.FieldID);
                    break;
                  case RateLockField.RateLockOrder.Previous2Denied:
                    fieldDefinitionObj = new FieldDefinitionObj("2nd Previous Lock Deny Fields", lockRequestField.InstanceSpecifierType, true, rateLockField.FieldID);
                    break;
                  case RateLockField.RateLockOrder.MostRecentLockRequest:
                    fieldDefinitionObj = new FieldDefinitionObj("Most Recent Lock Request Fields", lockRequestField.InstanceSpecifierType, true, rateLockField.FieldID);
                    break;
                  case RateLockField.RateLockOrder.PreviousLockRequest:
                    fieldDefinitionObj = new FieldDefinitionObj("Previous Lock Request Fields", lockRequestField.InstanceSpecifierType, true, rateLockField.FieldID);
                    break;
                  case RateLockField.RateLockOrder.Previous2LockRequest:
                    fieldDefinitionObj = new FieldDefinitionObj("2nd Previous Lock Request Fields", lockRequestField.InstanceSpecifierType, true, rateLockField.FieldID);
                    break;
                  default:
                    fieldDefinitionObj = new FieldDefinitionObj("2nd Previous Lock Fields", lockRequestField.InstanceSpecifierType, true, rateLockField.FieldID);
                    break;
                }
                if (!dictionary.ContainsKey(fieldDefinitionObj.ToString()))
                  dictionary.Add(fieldDefinitionObj.ToString(), fieldDefinitionObj);
              }
              else
                this.listCbo.Items.Add((object) new FieldDefinitionObj(lockRequestField));
            }
            using (Dictionary<string, FieldDefinitionObj>.ValueCollection.Enumerator enumerator14 = dictionary.Values.GetEnumerator())
            {
              while (enumerator14.MoveNext())
                this.listCbo.Items.Add((object) enumerator14.Current);
              break;
            }
          case "TIL Disclosure":
            this.listCbo.Sorted = true;
            IEnumerator enumerator15 = LastDisclosedTILFields.All.GetEnumerator();
            try
            {
              while (enumerator15.MoveNext())
                this.listCbo.Items.Add((object) new FieldDefinitionObj((FieldDefinition) enumerator15.Current));
              break;
            }
            finally
            {
              if (enumerator15 is IDisposable disposable)
                disposable.Dispose();
            }
          case "Tasks":
            IEnumerator enumerator16 = MilestoneTaskFields.All.GetEnumerator();
            try
            {
              while (enumerator16.MoveNext())
                this.listCbo.Items.Add((object) new FieldDefinitionObj((FieldDefinition) enumerator16.Current));
              break;
            }
            finally
            {
              if (enumerator16 is IDisposable disposable)
                disposable.Dispose();
            }
        }
      }
      if (this.listCbo.Items.Count <= 1)
        return;
      if (order + 1 <= this.fldGroups.Length)
      {
        this.idTxt.Text = Convert.ToString(this.fieldTbl[(object) this.listCbo.Text]);
        this.listCbo.SelectedIndex = 0;
      }
      else
        this.idTxt.Text = "";
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (InsertFieldDialog));
      this.saveBtn = new Button();
      this.closeBtn = new Button();
      this.signatureImageLst = new ImageList(this.components);
      this.initialsImageLst = new ImageList(this.components);
      this.insertBtn = new Button();
      this.grpSigPoints = new GroupContainer();
      this.coborSignatureGrey = new PictureBox();
      this.coborInitialGrey = new PictureBox();
      this.borSignatureGrey = new PictureBox();
      this.borInitialGrey = new PictureBox();
      this.cboIntendedFor = new ComboBox();
      this.label6 = new Label();
      this.originatorSignaturePic = new PictureBox();
      this.borSignaturePic = new PictureBox();
      this.originatorInitialsPic = new PictureBox();
      this.borInitialsPic = new PictureBox();
      this.coborSignaturePic = new PictureBox();
      this.label2 = new Label();
      this.coborInitialsPic = new PictureBox();
      this.grpFields = new GroupContainer();
      this.label5 = new Label();
      this.listCbo = new ComboBox();
      this.catagoryCbo = new ComboBox();
      this.label3 = new Label();
      this.fndBtn = new Button();
      this.idTxt = new TextBox();
      this.newBtn = new Button();
      this.label1 = new Label();
      this.findTxt = new TextBox();
      this.label4 = new Label();
      this.splitContainer1 = new SplitContainer();
      this.grpSigPoints.SuspendLayout();
      ((ISupportInitialize) this.coborSignatureGrey).BeginInit();
      ((ISupportInitialize) this.coborInitialGrey).BeginInit();
      ((ISupportInitialize) this.borSignatureGrey).BeginInit();
      ((ISupportInitialize) this.borInitialGrey).BeginInit();
      ((ISupportInitialize) this.originatorSignaturePic).BeginInit();
      ((ISupportInitialize) this.borSignaturePic).BeginInit();
      ((ISupportInitialize) this.originatorInitialsPic).BeginInit();
      ((ISupportInitialize) this.borInitialsPic).BeginInit();
      ((ISupportInitialize) this.coborSignaturePic).BeginInit();
      ((ISupportInitialize) this.coborInitialsPic).BeginInit();
      this.grpFields.SuspendLayout();
      this.splitContainer1.BeginInit();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.SuspendLayout();
      this.saveBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.saveBtn.Location = new Point(656, 218);
      this.saveBtn.Name = "saveBtn";
      this.saveBtn.Size = new Size(75, 22);
      this.saveBtn.TabIndex = 7;
      this.saveBtn.Text = "&Save";
      this.saveBtn.UseVisualStyleBackColor = true;
      this.saveBtn.Click += new EventHandler(this.saveBtn_Click);
      this.closeBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.closeBtn.Location = new Point(737, 218);
      this.closeBtn.Name = "closeBtn";
      this.closeBtn.Size = new Size(75, 22);
      this.closeBtn.TabIndex = 8;
      this.closeBtn.Text = "&Close";
      this.closeBtn.UseVisualStyleBackColor = true;
      this.closeBtn.Click += new EventHandler(this.closeBtn_Click);
      this.signatureImageLst.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("signatureImageLst.ImageStream");
      this.signatureImageLst.TransparentColor = Color.Transparent;
      this.signatureImageLst.Images.SetKeyName(0, "tag-sign-borrower.png");
      this.signatureImageLst.Images.SetKeyName(1, "tag-sign-borrower-hover.png");
      this.signatureImageLst.Images.SetKeyName(2, "tag-sign-coborrower.png");
      this.signatureImageLst.Images.SetKeyName(3, "tag-sign-coborrower-hover.png");
      this.signatureImageLst.Images.SetKeyName(4, "tag-sign-originator.png");
      this.signatureImageLst.Images.SetKeyName(5, "tag-sign-originator-hover.png");
      this.initialsImageLst.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("initialsImageLst.ImageStream");
      this.initialsImageLst.TransparentColor = Color.Transparent;
      this.initialsImageLst.Images.SetKeyName(0, "tag-initial-borrower.png");
      this.initialsImageLst.Images.SetKeyName(1, "tag-initial-borrower-hover.png");
      this.initialsImageLst.Images.SetKeyName(2, "tag-initial-coborrower.png");
      this.initialsImageLst.Images.SetKeyName(3, "tag-initial-coborrower-hover.png");
      this.initialsImageLst.Images.SetKeyName(4, "tag-initial-originator.png");
      this.initialsImageLst.Images.SetKeyName(5, "tag-initial-originator-hover.png");
      this.insertBtn.Location = new Point(73, 141);
      this.insertBtn.Name = "insertBtn";
      this.insertBtn.Size = new Size(87, 22);
      this.insertBtn.TabIndex = 5;
      this.insertBtn.Text = "&Insert";
      this.insertBtn.UseVisualStyleBackColor = true;
      this.insertBtn.Click += new EventHandler(this.insertBtn_Click);
      this.grpSigPoints.Controls.Add((Control) this.coborSignatureGrey);
      this.grpSigPoints.Controls.Add((Control) this.coborInitialGrey);
      this.grpSigPoints.Controls.Add((Control) this.borSignatureGrey);
      this.grpSigPoints.Controls.Add((Control) this.borInitialGrey);
      this.grpSigPoints.Controls.Add((Control) this.cboIntendedFor);
      this.grpSigPoints.Controls.Add((Control) this.label6);
      this.grpSigPoints.Controls.Add((Control) this.originatorSignaturePic);
      this.grpSigPoints.Controls.Add((Control) this.borSignaturePic);
      this.grpSigPoints.Controls.Add((Control) this.originatorInitialsPic);
      this.grpSigPoints.Controls.Add((Control) this.borInitialsPic);
      this.grpSigPoints.Controls.Add((Control) this.coborSignaturePic);
      this.grpSigPoints.Controls.Add((Control) this.label2);
      this.grpSigPoints.Controls.Add((Control) this.coborInitialsPic);
      this.grpSigPoints.Dock = DockStyle.Fill;
      this.grpSigPoints.HeaderForeColor = SystemColors.ControlText;
      this.grpSigPoints.Location = new Point(0, 0);
      this.grpSigPoints.Name = "grpSigPoints";
      this.grpSigPoints.Size = new Size(408, 199);
      this.grpSigPoints.TabIndex = 38;
      this.grpSigPoints.Text = "Insert Signature Points";
      this.coborSignatureGrey.Image = (Image) componentResourceManager.GetObject("coborSignatureGrey.Image");
      this.coborSignatureGrey.Location = new Point(14, 94);
      this.coborSignatureGrey.Margin = new Padding(0);
      this.coborSignatureGrey.Name = "coborSignatureGrey";
      this.coborSignatureGrey.Size = new Size(256, 25);
      this.coborSignatureGrey.SizeMode = PictureBoxSizeMode.CenterImage;
      this.coborSignatureGrey.TabIndex = 42;
      this.coborSignatureGrey.TabStop = false;
      this.coborInitialGrey.Image = (Image) componentResourceManager.GetObject("coborInitialGrey.Image");
      this.coborInitialGrey.Location = new Point(279, 94);
      this.coborInitialGrey.Name = "coborInitialGrey";
      this.coborInitialGrey.Size = new Size(108, 25);
      this.coborInitialGrey.SizeMode = PictureBoxSizeMode.CenterImage;
      this.coborInitialGrey.TabIndex = 41;
      this.coborInitialGrey.TabStop = false;
      this.borSignatureGrey.Image = (Image) componentResourceManager.GetObject("borSignatureGrey.Image");
      this.borSignatureGrey.Location = new Point(14, 63);
      this.borSignatureGrey.Margin = new Padding(0);
      this.borSignatureGrey.Name = "borSignatureGrey";
      this.borSignatureGrey.Size = new Size(256, 25);
      this.borSignatureGrey.SizeMode = PictureBoxSizeMode.CenterImage;
      this.borSignatureGrey.TabIndex = 40;
      this.borSignatureGrey.TabStop = false;
      this.borInitialGrey.Image = (Image) componentResourceManager.GetObject("borInitialGrey.Image");
      this.borInitialGrey.Location = new Point(279, 63);
      this.borInitialGrey.Name = "borInitialGrey";
      this.borInitialGrey.Size = new Size(108, 25);
      this.borInitialGrey.SizeMode = PictureBoxSizeMode.CenterImage;
      this.borInitialGrey.TabIndex = 39;
      this.borInitialGrey.TabStop = false;
      this.cboIntendedFor.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboIntendedFor.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboIntendedFor.Location = new Point(81, 164);
      this.cboIntendedFor.Name = "cboIntendedFor";
      this.cboIntendedFor.Size = new Size(314, 22);
      this.cboIntendedFor.TabIndex = 37;
      this.cboIntendedFor.Visible = false;
      this.cboIntendedFor.SelectedIndexChanged += new EventHandler(this.cboIntendedFor_SelectedIndexChanged);
      this.label6.AutoSize = true;
      this.label6.Location = new Point(8, 167);
      this.label6.Name = "label6";
      this.label6.Size = new Size(67, 14);
      this.label6.TabIndex = 38;
      this.label6.Text = "Intended For";
      this.label6.Visible = false;
      this.originatorSignaturePic.Image = (Image) componentResourceManager.GetObject("originatorSignaturePic.Image");
      this.originatorSignaturePic.Location = new Point(14, 125);
      this.originatorSignaturePic.Margin = new Padding(0);
      this.originatorSignaturePic.Name = "originatorSignaturePic";
      this.originatorSignaturePic.Size = new Size(256, 25);
      this.originatorSignaturePic.SizeMode = PictureBoxSizeMode.CenterImage;
      this.originatorSignaturePic.TabIndex = 35;
      this.originatorSignaturePic.TabStop = false;
      this.originatorSignaturePic.Click += new EventHandler(this.signaturePic_Click);
      this.originatorSignaturePic.MouseEnter += new EventHandler(this.signaturePic_MouseEnter);
      this.originatorSignaturePic.MouseLeave += new EventHandler(this.signaturePic_MouseLeave);
      this.borSignaturePic.Image = (Image) componentResourceManager.GetObject("borSignaturePic.Image");
      this.borSignaturePic.Location = new Point(14, 63);
      this.borSignaturePic.Margin = new Padding(0);
      this.borSignaturePic.Name = "borSignaturePic";
      this.borSignaturePic.Size = new Size(256, 25);
      this.borSignaturePic.SizeMode = PictureBoxSizeMode.CenterImage;
      this.borSignaturePic.TabIndex = 31;
      this.borSignaturePic.TabStop = false;
      this.borSignaturePic.Click += new EventHandler(this.signaturePic_Click);
      this.borSignaturePic.MouseEnter += new EventHandler(this.signaturePic_MouseEnter);
      this.borSignaturePic.MouseLeave += new EventHandler(this.signaturePic_MouseLeave);
      this.originatorInitialsPic.Image = (Image) componentResourceManager.GetObject("originatorInitialsPic.Image");
      this.originatorInitialsPic.Location = new Point(279, 125);
      this.originatorInitialsPic.Name = "originatorInitialsPic";
      this.originatorInitialsPic.Size = new Size(108, 25);
      this.originatorInitialsPic.SizeMode = PictureBoxSizeMode.CenterImage;
      this.originatorInitialsPic.TabIndex = 36;
      this.originatorInitialsPic.TabStop = false;
      this.originatorInitialsPic.Click += new EventHandler(this.signaturePic_Click);
      this.originatorInitialsPic.MouseEnter += new EventHandler(this.signaturePic_MouseEnter);
      this.originatorInitialsPic.MouseLeave += new EventHandler(this.signaturePic_MouseLeave);
      this.borInitialsPic.Image = (Image) componentResourceManager.GetObject("borInitialsPic.Image");
      this.borInitialsPic.Location = new Point(279, 63);
      this.borInitialsPic.Name = "borInitialsPic";
      this.borInitialsPic.Size = new Size(108, 25);
      this.borInitialsPic.SizeMode = PictureBoxSizeMode.CenterImage;
      this.borInitialsPic.TabIndex = 33;
      this.borInitialsPic.TabStop = false;
      this.borInitialsPic.Click += new EventHandler(this.signaturePic_Click);
      this.borInitialsPic.MouseEnter += new EventHandler(this.signaturePic_MouseEnter);
      this.borInitialsPic.MouseLeave += new EventHandler(this.signaturePic_MouseLeave);
      this.coborSignaturePic.Image = (Image) componentResourceManager.GetObject("coborSignaturePic.Image");
      this.coborSignaturePic.Location = new Point(14, 94);
      this.coborSignaturePic.Margin = new Padding(0);
      this.coborSignaturePic.Name = "coborSignaturePic";
      this.coborSignaturePic.Size = new Size(256, 25);
      this.coborSignaturePic.SizeMode = PictureBoxSizeMode.CenterImage;
      this.coborSignaturePic.TabIndex = 32;
      this.coborSignaturePic.TabStop = false;
      this.coborSignaturePic.Click += new EventHandler(this.signaturePic_Click);
      this.coborSignaturePic.MouseEnter += new EventHandler(this.signaturePic_MouseEnter);
      this.coborSignaturePic.MouseLeave += new EventHandler(this.signaturePic_MouseLeave);
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Arial", 8.25f);
      this.label2.Location = new Point(11, 39);
      this.label2.Name = "label2";
      this.label2.Size = new Size(157, 14);
      this.label2.TabIndex = 30;
      this.label2.Text = "Click to insert a signature point:";
      this.coborInitialsPic.Image = (Image) componentResourceManager.GetObject("coborInitialsPic.Image");
      this.coborInitialsPic.Location = new Point(279, 94);
      this.coborInitialsPic.Name = "coborInitialsPic";
      this.coborInitialsPic.Size = new Size(108, 25);
      this.coborInitialsPic.SizeMode = PictureBoxSizeMode.CenterImage;
      this.coborInitialsPic.TabIndex = 34;
      this.coborInitialsPic.TabStop = false;
      this.coborInitialsPic.Click += new EventHandler(this.signaturePic_Click);
      this.coborInitialsPic.MouseEnter += new EventHandler(this.signaturePic_MouseEnter);
      this.coborInitialsPic.MouseLeave += new EventHandler(this.signaturePic_MouseLeave);
      this.grpFields.Controls.Add((Control) this.label5);
      this.grpFields.Controls.Add((Control) this.listCbo);
      this.grpFields.Controls.Add((Control) this.catagoryCbo);
      this.grpFields.Controls.Add((Control) this.insertBtn);
      this.grpFields.Controls.Add((Control) this.label3);
      this.grpFields.Controls.Add((Control) this.fndBtn);
      this.grpFields.Controls.Add((Control) this.idTxt);
      this.grpFields.Controls.Add((Control) this.newBtn);
      this.grpFields.Controls.Add((Control) this.label1);
      this.grpFields.Controls.Add((Control) this.findTxt);
      this.grpFields.Controls.Add((Control) this.label4);
      this.grpFields.Dock = DockStyle.Fill;
      this.grpFields.HeaderForeColor = SystemColors.ControlText;
      this.grpFields.Location = new Point(0, 0);
      this.grpFields.Name = "grpFields";
      this.grpFields.Size = new Size(568, 199);
      this.grpFields.TabIndex = 37;
      this.grpFields.Text = "Insert Data Fields";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(9, 42);
      this.label5.Name = "label5";
      this.label5.Size = new Size(42, 14);
      this.label5.TabIndex = 30;
      this.label5.Text = "Search";
      this.listCbo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.listCbo.DropDownStyle = ComboBoxStyle.DropDownList;
      this.listCbo.Location = new Point(73, 89);
      this.listCbo.MaxDropDownItems = 20;
      this.listCbo.Name = "listCbo";
      this.listCbo.Size = new Size(477, 22);
      this.listCbo.TabIndex = 3;
      this.listCbo.SelectedIndexChanged += new EventHandler(this.listCbo_SelectedIndexChanged);
      this.catagoryCbo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.catagoryCbo.DropDownStyle = ComboBoxStyle.DropDownList;
      this.catagoryCbo.Location = new Point(73, 63);
      this.catagoryCbo.Name = "catagoryCbo";
      this.catagoryCbo.Size = new Size(477, 22);
      this.catagoryCbo.TabIndex = 2;
      this.catagoryCbo.SelectedIndexChanged += new EventHandler(this.catagoryCbo_SelectedIndexChanged);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(9, 92);
      this.label3.Name = "label3";
      this.label3.Size = new Size(59, 14);
      this.label3.TabIndex = 27;
      this.label3.Text = "Field Name";
      this.fndBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.fndBtn.Location = new Point(314, 38);
      this.fndBtn.Name = "fndBtn";
      this.fndBtn.Size = new Size(66, 22);
      this.fndBtn.TabIndex = 1;
      this.fndBtn.Text = "Search";
      this.fndBtn.UseVisualStyleBackColor = true;
      this.fndBtn.Click += new EventHandler(this.fndBtn_Click);
      this.idTxt.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.idTxt.Enabled = false;
      this.idTxt.Location = new Point(73, 115);
      this.idTxt.Name = "idTxt";
      this.idTxt.Size = new Size(477, 20);
      this.idTxt.TabIndex = 4;
      this.idTxt.TabStop = false;
      this.newBtn.Location = new Point(166, 141);
      this.newBtn.Name = "newBtn";
      this.newBtn.Size = new Size(87, 22);
      this.newBtn.TabIndex = 6;
      this.newBtn.Text = "Insert Other...";
      this.newBtn.UseVisualStyleBackColor = true;
      this.newBtn.Click += new EventHandler(this.newBtn_Click);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(9, 66);
      this.label1.Name = "label1";
      this.label1.Size = new Size(51, 14);
      this.label1.TabIndex = 24;
      this.label1.Text = "Category";
      this.findTxt.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.findTxt.Location = new Point(73, 39);
      this.findTxt.Name = "findTxt";
      this.findTxt.Size = new Size(405, 20);
      this.findTxt.TabIndex = 0;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(10, 118);
      this.label4.Name = "label4";
      this.label4.Size = new Size(41, 14);
      this.label4.TabIndex = 29;
      this.label4.Text = "Field ID";
      this.splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.splitContainer1.FixedPanel = FixedPanel.Panel2;
      this.splitContainer1.IsSplitterFixed = true;
      this.splitContainer1.Location = new Point(10, 10);
      this.splitContainer1.Name = "splitContainer1";
      this.splitContainer1.Panel1.Controls.Add((Control) this.grpFields);
      this.splitContainer1.Panel2.Controls.Add((Control) this.grpSigPoints);
      this.splitContainer1.Size = new Size(981, 199);
      this.splitContainer1.SplitterDistance = 568;
      this.splitContainer1.SplitterWidth = 5;
      this.splitContainer1.TabIndex = 39;
      this.AcceptButton = (IButtonControl) this.insertBtn;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(1001, 250);
      this.ControlBox = false;
      this.Controls.Add((Control) this.splitContainer1);
      this.Controls.Add((Control) this.closeBtn);
      this.Controls.Add((Control) this.saveBtn);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (InsertFieldDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.Manual;
      this.Text = "Insert Fields";
      this.TopMost = true;
      this.Closing += new CancelEventHandler(this.InsertFieldDialog_Closing);
      this.Closed += new EventHandler(this.InsertFieldDialog_Closed);
      this.Shown += new EventHandler(this.InsertFieldDialog_Shown);
      this.grpSigPoints.ResumeLayout(false);
      this.grpSigPoints.PerformLayout();
      ((ISupportInitialize) this.coborSignatureGrey).EndInit();
      ((ISupportInitialize) this.coborInitialGrey).EndInit();
      ((ISupportInitialize) this.borSignatureGrey).EndInit();
      ((ISupportInitialize) this.borInitialGrey).EndInit();
      ((ISupportInitialize) this.originatorSignaturePic).EndInit();
      ((ISupportInitialize) this.borSignaturePic).EndInit();
      ((ISupportInitialize) this.originatorInitialsPic).EndInit();
      ((ISupportInitialize) this.borInitialsPic).EndInit();
      ((ISupportInitialize) this.coborSignaturePic).EndInit();
      ((ISupportInitialize) this.coborInitialsPic).EndInit();
      this.grpFields.ResumeLayout(false);
      this.grpFields.PerformLayout();
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.EndInit();
      this.splitContainer1.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void cboIntendedFor_SelectedIndexChanged(object sender, EventArgs e)
    {
      ForBorrowerType valueFromDescription = EnumUtil.GetEnumValueFromDescription<ForBorrowerType>(this.cboIntendedFor.Text);
      if (valueFromDescription != this.currentIntendedFor)
      {
        if (valueFromDescription == ForBorrowerType.Borrower && this.hasCoBorrowerFields || valueFromDescription == ForBorrowerType.CoBorrower && this.hasBorrowerFields)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Cannot modify the Intended For value for this document. The document contains one or more signature points for non-intended signing party.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          this.cboIntendedFor.Text = EnumUtil.GetEnumDescription((Enum) this.currentIntendedFor);
          return;
        }
        foreach (DocumentTemplate documentTemplate in this.session.ConfigurationManager.GetDocumentTrackingSetup())
        {
          if ((documentTemplate.SourceType == "Custom Form" || documentTemplate.SourceType == "Borrower Specific Custom Form") && (string.Compare(documentTemplate.Source, this.fsEntry.ToString(), StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(documentTemplate.SourceBorrower, this.fsEntry.ToString(), StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(documentTemplate.SourceCoborrower, this.fsEntry.ToString(), StringComparison.OrdinalIgnoreCase) == 0))
          {
            if (Utils.Dialog((IWin32Window) this, "Warning: This Custom Print Form template is associated with one or more Document templates and changing the Intended For value of this custom print form template will  affect the source of associated Document Template. Are you sure you would like to proceed?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
              this.cboIntendedFor.Text = EnumUtil.GetEnumDescription((Enum) this.currentIntendedFor);
              return;
            }
            break;
          }
        }
      }
      this.currentIntendedFor = valueFromDescription;
      this.borSignaturePic.Visible = this.currentIntendedFor == ForBorrowerType.Borrower || this.currentIntendedFor == ForBorrowerType.All;
      this.borInitialsPic.Visible = this.currentIntendedFor == ForBorrowerType.Borrower || this.currentIntendedFor == ForBorrowerType.All;
      this.borInitialGrey.Visible = this.currentIntendedFor == ForBorrowerType.CoBorrower;
      this.borSignatureGrey.Visible = this.currentIntendedFor == ForBorrowerType.CoBorrower;
      this.coborSignaturePic.Visible = this.currentIntendedFor == ForBorrowerType.CoBorrower || this.currentIntendedFor == ForBorrowerType.All;
      this.coborInitialsPic.Visible = this.currentIntendedFor == ForBorrowerType.CoBorrower || this.currentIntendedFor == ForBorrowerType.All;
      this.coborInitialGrey.Visible = this.currentIntendedFor == ForBorrowerType.Borrower;
      this.coborSignatureGrey.Visible = this.currentIntendedFor == ForBorrowerType.Borrower;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void insertBtn_Click(object sender, EventArgs e)
    {
      switch (this.catagoryCbo.Text)
      {
        case "Audit Trail":
        case "CD Disclosure":
        case "Closing Documents":
        case "Condition Tracking":
        case "Document Tracking":
        case "Enhanced Condition (single condition)":
        case "Enhanced Conditions (By Options)":
        case "Enhanced Conditions (multiple conditions)":
        case "GFE Disclosure":
        case "LE Disclosure":
        case "Loan Team Member":
        case "Milestone":
        case "Post Closing Condition":
        case "Preliminary Condition":
        case "Ratelock":
        case "TIL Disclosure":
        case "Tasks":
          this.insertDynamicField(this.idTxt.Text);
          break;
        default:
          string str = this.listCbo.SelectedItem.ToString();
          string fieldId = this.fieldTbl[(object) str].ToString();
          switch (fieldId)
          {
            case "LOCKRATE...":
              using (VirtualFieldSelectForm virtualFieldSelectForm = new VirtualFieldSelectForm(LoanReportFieldDef.RateLockFieldSelector().FieldID))
              {
                if (virtualFieldSelectForm.ShowDialog((IWin32Window) this) == DialogResult.OK)
                {
                  str = "M";
                  fieldId = virtualFieldSelectForm.SelectedField.FieldID;
                  break;
                }
                break;
              }
            case "ISPAY...":
              using (VirtualFieldSelectForm virtualFieldSelectForm = new VirtualFieldSelectForm(LoanReportFieldDef.InterimFieldSelector().FieldID))
              {
                if (virtualFieldSelectForm.ShowDialog((IWin32Window) this) == DialogResult.OK)
                {
                  str = "M";
                  fieldId = virtualFieldSelectForm.SelectedField.FieldID;
                  break;
                }
                break;
              }
            case "DISCLOSEDGFE.Snapshot...":
              using (VirtualFieldSelectForm virtualFieldSelectForm = new VirtualFieldSelectForm(LoanReportFieldDef.GFEDisclosedFieldSelector().FieldID))
              {
                if (virtualFieldSelectForm.ShowDialog((IWin32Window) this) == DialogResult.OK)
                {
                  str = "M";
                  fieldId = virtualFieldSelectForm.SelectedField.FieldID;
                  break;
                }
                break;
              }
          }
          this.InsertField(str, fieldId);
          break;
      }
      this.wordHandler.ActivateDocWindow();
    }

    private void insertDynamicField(string fieldID)
    {
      this.wordHandler.InsertData(this.encodeToHex(fieldID));
    }

    private void InsertField(string fielddesc, string fieldid)
    {
      fieldid = fieldid.Replace(".", "dot");
      if (fielddesc.Length > 0)
      {
        for (int index = 0; index < fielddesc.Length; ++index)
        {
          char ch = fielddesc[index];
          if (!char.IsLetterOrDigit(ch))
            fielddesc = fielddesc.Replace(ch, '_');
        }
        if (char.IsDigit(fielddesc[0]))
          fielddesc = "_" + fielddesc;
      }
      else
      {
        fielddesc = "M";
        int length = fieldid.IndexOf("#");
        if (length > -1)
        {
          int num = Utils.ParseInt((object) fieldid.Substring(length + 1));
          if (num > 1)
            fielddesc = "M" + (object) num;
          fieldid = fieldid.Substring(0, length);
        }
      }
      this.wordHandler.InsertData(fielddesc + "_" + fieldid);
    }

    private void saveBtn_Click(object sender, EventArgs e)
    {
      Cursor.Current = Cursors.WaitCursor;
      this.wordHandler.Save();
      this.hasBorrowerFields = this.wordHandler.HasBorrowerSignaturePoints;
      this.hasCoBorrowerFields = this.wordHandler.HasCoBorrowerSignaturePoints;
      Cursor.Current = Cursors.Default;
    }

    private void InsertFieldDialog_Closing(object sender, CancelEventArgs e)
    {
      this.wordHandler.bInsertFormClosed = true;
      this.Closing -= new CancelEventHandler(this.InsertFieldDialog_Closing);
      Tracing.Log(InsertFieldDialog.sw, TraceLevel.Verbose, this.className, "Insert Fields form is Closing...");
      if (!this.wordHandler.GetSaved())
      {
        switch (Utils.Dialog((IWin32Window) this, "Do you want to save the chages you made to " + this.localFile, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
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
    }

    private void InsertFieldDialog_Closed(object sender, EventArgs e)
    {
      this.session.Application.Enable();
      Thread.Sleep(3000);
      int num = 0;
      BinaryObject data;
      while (true)
      {
        ++num;
        try
        {
          data = new BinaryObject(this.localFile);
          break;
        }
        catch (Exception ex)
        {
          if (num > 5)
            return;
        }
        Thread.Sleep(2000);
      }
      this.session.ConfigurationManager.SaveCustomLetter(CustomLetterType.Generic, this.fsEntry, data);
      data?.Dispose();
    }

    private void closeBtn_Click(object sender, EventArgs e)
    {
      Cursor.Current = Cursors.WaitCursor;
      this.wordHandler.ClickByHand = true;
      if (this.currentIntendedFor != this.originalIntendedFor)
      {
        this.session.ConfigurationManager.SaveCustomFormDetail(new CustomFormDetail(this.fsEntry.ToString(), this.currentIntendedFor));
        if (this.CustomFormDetailChanged != null)
          this.CustomFormDetailChanged((object) this, new EventArgs());
      }
      this.Close();
    }

    private void catagoryCbo_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.buildFieldList(this.catagoryCbo.SelectedIndex);
    }

    private void newBtn_Click(object sender, EventArgs e)
    {
      InsertOtherDialog insertOtherDialog = new InsertOtherDialog();
      if (insertOtherDialog.ShowDialog((IWin32Window) this) != DialogResult.OK || !(insertOtherDialog.FieldID != string.Empty))
        return;
      this.InsertField("", insertOtherDialog.FieldID);
      this.wordHandler.ActivateDocWindow();
    }

    private void fndBtn_Click(object sender, EventArgs e)
    {
      string str1 = this.findTxt.Text.ToUpper().Trim();
      if (str1 == string.Empty)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please enter a word or phrase to search for.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        int index1 = this.catagoryCbo.SelectedIndex;
        int num2 = this.listCbo.SelectedIndex + 1;
        for (int index2 = 0; index2 < this.catagoryCbo.Items.Count + 1; ++index2)
        {
          if (index1 < this.fldGroups.Length)
          {
            string[] strArray = this.fldGroups[index1].Split('@');
            for (int index3 = num2; index3 < strArray.Length; ++index3)
            {
              string str2 = strArray[index3];
              int length = str2.IndexOf("|");
              if (length >= 0 && str2.Substring(0, length).ToUpper().Trim().IndexOf(str1) > -1)
              {
                this.catagoryCbo.SelectedIndex = index1;
                this.buildFieldList(index1);
                this.listCbo.SelectedIndex = index3;
                return;
              }
            }
          }
          else
          {
            string str3 = this.catagoryCbo.Items[index1].ToString();
            int num3 = 1;
            switch (str3)
            {
              case "Audit Trail":
                using (Dictionary<string, string>.KeyCollection.Enumerator enumerator = this.auditTrailFieldList.Keys.GetEnumerator())
                {
                  while (enumerator.MoveNext())
                  {
                    if (enumerator.Current.ToUpper().IndexOf(str1) > -1)
                    {
                      this.buildFieldList(index1);
                      this.catagoryCbo.SelectedIndex = index1;
                      this.listCbo.SelectedIndex = num3;
                      return;
                    }
                    ++num3;
                  }
                  break;
                }
              case "CD Disclosure":
                IEnumerator enumerator1 = LastDisclosedCDFields.All.GetEnumerator();
                try
                {
                  while (enumerator1.MoveNext())
                  {
                    if (new FieldDefinitionObj((FieldDefinition) enumerator1.Current).ToString().ToUpper().IndexOf(str1) > -1)
                    {
                      this.buildFieldList(index1);
                      this.catagoryCbo.SelectedIndex = index1;
                      this.listCbo.SelectedIndex = num3;
                      return;
                    }
                    ++num3;
                  }
                  break;
                }
                finally
                {
                  if (enumerator1 is IDisposable disposable)
                    disposable.Dispose();
                }
              case "Closing Documents":
                IEnumerator enumerator2 = DocEngineFieldList.All.GetEnumerator();
                try
                {
                  while (enumerator2.MoveNext())
                  {
                    if (new FieldDefinitionObj((FieldDefinition) enumerator2.Current).ToString().ToUpper().IndexOf(str1) > -1)
                    {
                      this.buildFieldList(index1);
                      this.catagoryCbo.SelectedIndex = index1;
                      this.listCbo.SelectedIndex = num3;
                      return;
                    }
                    ++num3;
                  }
                  break;
                }
                finally
                {
                  if (enumerator2 is IDisposable disposable)
                    disposable.Dispose();
                }
              case "Condition Tracking":
                IEnumerator enumerator3 = UnderwritingConditionFields.All.GetEnumerator();
                try
                {
                  while (enumerator3.MoveNext())
                  {
                    if (new FieldDefinitionObj((FieldDefinition) enumerator3.Current).ToString().ToUpper().IndexOf(str1) > -1)
                    {
                      this.buildFieldList(index1);
                      this.catagoryCbo.SelectedIndex = index1;
                      this.listCbo.SelectedIndex = num3;
                      return;
                    }
                    ++num3;
                  }
                  break;
                }
                finally
                {
                  if (enumerator3 is IDisposable disposable)
                    disposable.Dispose();
                }
              case "Document Tracking":
                IEnumerator enumerator4 = DocumentTrackingFields.All.GetEnumerator();
                try
                {
                  while (enumerator4.MoveNext())
                  {
                    if (new FieldDefinitionObj((FieldDefinition) enumerator4.Current).ToString().ToUpper().IndexOf(str1) > -1)
                    {
                      this.buildFieldList(index1);
                      this.catagoryCbo.SelectedIndex = index1;
                      this.listCbo.SelectedIndex = num3;
                      return;
                    }
                    ++num3;
                  }
                  break;
                }
                finally
                {
                  if (enumerator4 is IDisposable disposable)
                    disposable.Dispose();
                }
              case "GFE Disclosure":
                IEnumerator enumerator5 = LastDisclosedGFEFields.All.GetEnumerator();
                try
                {
                  while (enumerator5.MoveNext())
                  {
                    if (new FieldDefinitionObj((FieldDefinition) enumerator5.Current).ToString().ToUpper().IndexOf(str1) > -1)
                    {
                      this.buildFieldList(index1);
                      this.catagoryCbo.SelectedIndex = index1;
                      this.listCbo.SelectedIndex = num3;
                      return;
                    }
                    ++num3;
                  }
                  break;
                }
                finally
                {
                  if (enumerator5 is IDisposable disposable)
                    disposable.Dispose();
                }
              case "LE Disclosure":
                IEnumerator enumerator6 = LastDisclosedLEFields.All.GetEnumerator();
                try
                {
                  while (enumerator6.MoveNext())
                  {
                    if (new FieldDefinitionObj((FieldDefinition) enumerator6.Current).ToString().ToUpper().IndexOf(str1) > -1)
                    {
                      this.buildFieldList(index1);
                      this.catagoryCbo.SelectedIndex = index1;
                      this.listCbo.SelectedIndex = num3;
                      return;
                    }
                    ++num3;
                  }
                  break;
                }
                finally
                {
                  if (enumerator6 is IDisposable disposable)
                    disposable.Dispose();
                }
              case "Loan Team Member":
                IEnumerator enumerator7 = LoanAssociateFields.All.GetEnumerator();
                try
                {
                  while (enumerator7.MoveNext())
                  {
                    if (new FieldDefinitionObj((FieldDefinition) enumerator7.Current).ToString().ToUpper().IndexOf(str1) > -1)
                    {
                      this.buildFieldList(index1);
                      this.catagoryCbo.SelectedIndex = index1;
                      this.listCbo.SelectedIndex = num3;
                      return;
                    }
                    ++num3;
                  }
                  break;
                }
                finally
                {
                  if (enumerator7 is IDisposable disposable)
                    disposable.Dispose();
                }
              case "Milestone":
                IEnumerator enumerator8 = MilestoneFields.All.GetEnumerator();
                try
                {
                  while (enumerator8.MoveNext())
                  {
                    FieldDefinition current = (FieldDefinition) enumerator8.Current;
                    if (!(current is CoreMilestoneField))
                    {
                      if (new FieldDefinitionObj(current).ToString().ToUpper().IndexOf(str1) > -1)
                      {
                        this.buildFieldList(index1);
                        this.catagoryCbo.SelectedIndex = index1;
                        this.listCbo.SelectedIndex = num3;
                        return;
                      }
                      ++num3;
                    }
                  }
                  break;
                }
                finally
                {
                  if (enumerator8 is IDisposable disposable)
                    disposable.Dispose();
                }
              case "Post Closing Condition":
                IEnumerator enumerator9 = PostClosingConditionFields.All.GetEnumerator();
                try
                {
                  while (enumerator9.MoveNext())
                  {
                    if (new FieldDefinitionObj((FieldDefinition) enumerator9.Current).ToString().ToUpper().IndexOf(str1) > -1)
                    {
                      this.buildFieldList(index1);
                      this.catagoryCbo.SelectedIndex = index1;
                      this.listCbo.SelectedIndex = num3;
                      return;
                    }
                    ++num3;
                  }
                  break;
                }
                finally
                {
                  if (enumerator9 is IDisposable disposable)
                    disposable.Dispose();
                }
              case "Preliminary Condition":
                IEnumerator enumerator10 = PreliminaryConditionFields.All.GetEnumerator();
                try
                {
                  while (enumerator10.MoveNext())
                  {
                    if (new FieldDefinitionObj((FieldDefinition) enumerator10.Current).ToString().ToUpper().IndexOf(str1) > -1)
                    {
                      this.buildFieldList(index1);
                      this.catagoryCbo.SelectedIndex = index1;
                      this.listCbo.SelectedIndex = num3;
                      return;
                    }
                    ++num3;
                  }
                  break;
                }
                finally
                {
                  if (enumerator10 is IDisposable disposable)
                    disposable.Dispose();
                }
              case "Ratelock":
                Dictionary<string, FieldDefinitionObj> dictionary = new Dictionary<string, FieldDefinitionObj>();
                IEnumerator enumerator11 = EncompassFields.GetAllLockRequestFields(this.fieldSettings).GetEnumerator();
                try
                {
                  while (enumerator11.MoveNext())
                  {
                    FieldDefinition current = (FieldDefinition) enumerator11.Current;
                    if (current is RateLockField)
                    {
                      RateLockField rateLockField = (RateLockField) current;
                      string display;
                      switch (rateLockField.GetRateLockOrder)
                      {
                        case RateLockField.RateLockOrder.MostRecent:
                          display = "Most Recent Lock Fields";
                          break;
                        case RateLockField.RateLockOrder.Previous:
                          display = "Previous Lock Fields";
                          break;
                        case RateLockField.RateLockOrder.MostRecentRequest:
                          display = "Most Recent Request Fields";
                          break;
                        case RateLockField.RateLockOrder.MostRecentDenied:
                          display = "Most Recent Lock Deny Fields";
                          break;
                        case RateLockField.RateLockOrder.PreviousDenied:
                          display = "Previous Lock Deny Fields";
                          break;
                        case RateLockField.RateLockOrder.Previous2Denied:
                          display = "2nd Previous Lock Deny Fields";
                          break;
                        case RateLockField.RateLockOrder.MostRecentLockRequest:
                          display = "Most Recent Lock Request Fields";
                          break;
                        case RateLockField.RateLockOrder.PreviousLockRequest:
                          display = "Previous Lock Request Fields";
                          break;
                        case RateLockField.RateLockOrder.Previous2LockRequest:
                          display = "2nd Previous Lock Request Fields";
                          break;
                        default:
                          display = "2nd Previous Lock Fields";
                          break;
                      }
                      if (new FieldDefinitionObj(display, current.InstanceSpecifierType, true, rateLockField.FieldID).ToString().ToUpper().IndexOf(str1) > -1)
                      {
                        this.buildFieldList(index1);
                        this.catagoryCbo.SelectedIndex = index1;
                        this.listCbo.SelectedIndex = num3;
                        return;
                      }
                    }
                    else
                    {
                      FieldDefinitionObj fieldDefinitionObj = new FieldDefinitionObj(current);
                      if (fieldDefinitionObj.ToString().ToUpper().IndexOf(str1) > -1)
                      {
                        this.buildFieldList(index1);
                        this.catagoryCbo.SelectedIndex = index1;
                        for (int index4 = 0; index4 < this.listCbo.Items.Count; ++index4)
                        {
                          if (((FieldDefinitionObj) this.listCbo.Items[index4]).ToString().ToUpper() == fieldDefinitionObj.ToString().ToUpper())
                            this.listCbo.SelectedIndex = index4;
                        }
                        return;
                      }
                    }
                    ++num3;
                  }
                  break;
                }
                finally
                {
                  if (enumerator11 is IDisposable disposable)
                    disposable.Dispose();
                }
              case "TIL Disclosure":
                IEnumerator enumerator12 = LastDisclosedTILFields.All.GetEnumerator();
                try
                {
                  while (enumerator12.MoveNext())
                  {
                    if (new FieldDefinitionObj((FieldDefinition) enumerator12.Current).ToString().ToUpper().IndexOf(str1) > -1)
                    {
                      this.buildFieldList(index1);
                      this.catagoryCbo.SelectedIndex = index1;
                      this.listCbo.SelectedIndex = num3;
                      return;
                    }
                    ++num3;
                  }
                  break;
                }
                finally
                {
                  if (enumerator12 is IDisposable disposable)
                    disposable.Dispose();
                }
              case "Tasks":
                IEnumerator enumerator13 = MilestoneTaskFields.All.GetEnumerator();
                try
                {
                  while (enumerator13.MoveNext())
                  {
                    if (new FieldDefinitionObj((FieldDefinition) enumerator13.Current).ToString().ToUpper().IndexOf(str1) > -1)
                    {
                      this.buildFieldList(index1);
                      this.catagoryCbo.SelectedIndex = index1;
                      this.listCbo.SelectedIndex = num3;
                      return;
                    }
                    ++num3;
                  }
                  break;
                }
                finally
                {
                  if (enumerator13 is IDisposable disposable)
                    disposable.Dispose();
                }
            }
          }
          ++index1;
          if (index1 >= this.catagoryCbo.Items.Count)
            index1 = 0;
          num2 = 0;
        }
        int num4 = (int) Utils.Dialog((IWin32Window) this, "'" + this.findTxt.Text + "' is not in the field list.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private void listCbo_SelectedIndexChanged(object sender, EventArgs e)
    {
      string text = this.listCbo.Text;
      if (text == string.Empty)
        return;
      switch (this.catagoryCbo.Text)
      {
        case "Audit Trail":
          using (AuditFields auditFields = new AuditFields())
          {
            if (DialogResult.Cancel == auditFields.ShowDialog((IWin32Window) this))
            {
              this.idTxt.Text = string.Empty;
              break;
            }
            this.idTxt.Text = Convert.ToString(this.fieldTbl[(object) text]) + "." + auditFields.SelectedField;
            this.insertBtn_Click((object) null, (EventArgs) null);
            break;
          }
        case "CD Disclosure":
        case "Closing Documents":
        case "Condition Tracking":
        case "Document Tracking":
        case "GFE Disclosure":
        case "LE Disclosure":
        case "Loan Team Member":
        case "Milestone":
        case "Post Closing Condition":
        case "Preliminary Condition":
        case "TIL Disclosure":
        case "Tasks":
          FieldDefinitionObj selectedItem1 = (FieldDefinitionObj) this.listCbo.SelectedItem;
          if (!selectedItem1.MultiInstance)
          {
            this.idTxt.Text = selectedItem1.FieldID;
            break;
          }
          InstanceSelectorDialog instanceSelectorDialog1 = !(selectedItem1.FieldID.ToLower() == "log.ms.duration") ? new InstanceSelectorDialog(selectedItem1.Type, true) : new InstanceSelectorDialog(selectedItem1.Type, true, selectedItem1.FieldID);
          if (instanceSelectorDialog1.ShowDialog((IWin32Window) this) != DialogResult.OK)
            break;
          if (instanceSelectorDialog1.SelectedInstance != "")
          {
            this.idTxt.Text = selectedItem1.GetInstanceFieldID(instanceSelectorDialog1.SelectedInstance);
            this.insertBtn_Click((object) null, (EventArgs) null);
            break;
          }
          this.idTxt.Text = string.Empty;
          break;
        case "Enhanced Condition (single condition)":
        case "Enhanced Conditions (multiple conditions)":
          FieldDefinitionObj selectedItem2 = (FieldDefinitionObj) this.listCbo.SelectedItem;
          if (!selectedItem2.MultiInstance)
          {
            this.idTxt.Text = selectedItem2.FieldID;
            break;
          }
          InstanceSelectorDialog instanceSelectorDialog2 = new InstanceSelectorDialog(selectedItem2.Type, true);
          if (instanceSelectorDialog2.ShowDialog((IWin32Window) this) != DialogResult.OK)
            break;
          string instanceSpecifier = instanceSelectorDialog2.SelectedInstance;
          if (instanceSelectorDialog2.SelectedInstance != "")
          {
            if (selectedItem2.FieldID.ToLower().StartsWith("enhanced.ad") || selectedItem2.FieldID.ToLower().StartsWith("enhanced.tow"))
            {
              bool isTrackingOwner = false;
              if (selectedItem2.FieldID.ToLower().StartsWith("enhanced.tow"))
                isTrackingOwner = true;
              StatusTrackingSelectorDialog trackingSelectorDialog = new StatusTrackingSelectorDialog(instanceSelectorDialog2.SelectedInstance, isTrackingOwner);
              if (trackingSelectorDialog.ShowDialog((IWin32Window) this) != DialogResult.OK || trackingSelectorDialog.SelectedStatus == "")
              {
                this.idTxt.Text = string.Empty;
                break;
              }
              if (trackingSelectorDialog.SelectedStatus != "")
                instanceSpecifier = instanceSpecifier + "~%cbiz%~" + trackingSelectorDialog.SelectedStatus;
            }
            this.idTxt.Text = selectedItem2.GetInstanceFieldID(instanceSpecifier);
            this.insertBtn_Click((object) null, (EventArgs) null);
            break;
          }
          this.idTxt.Text = string.Empty;
          break;
        case "Enhanced Conditions (By Options)":
          FieldDefinitionObj selectedItem3 = (FieldDefinitionObj) this.listCbo.SelectedItem;
          if (!selectedItem3.MultiInstance)
          {
            this.idTxt.Text = selectedItem3.FieldID;
            break;
          }
          string attribute = "";
          if (selectedItem3.FieldID.ToLower().StartsWith("enhop.cop"))
            attribute = "Category";
          else if (selectedItem3.FieldID.ToLower().StartsWith("enhop.pto"))
            attribute = "Prior To";
          else if (selectedItem3.FieldID.ToLower().StartsWith("enhop.sop"))
            attribute = "Source";
          else if (selectedItem3.FieldID.ToLower().StartsWith("enhop.rop"))
            attribute = "Recipient";
          else if (selectedItem3.FieldID.ToLower().StartsWith("enhop.top"))
            attribute = "Tracking Option";
          else if (selectedItem3.FieldID.ToLower().StartsWith("enhop.tow"))
            attribute = "Tracking Owner";
          EnhancedConditionOptionSelector conditionOptionSelector = new EnhancedConditionOptionSelector(attribute);
          if (conditionOptionSelector.ShowDialog((IWin32Window) this) != DialogResult.OK || conditionOptionSelector.SelectedOption == "")
          {
            this.idTxt.Text = string.Empty;
            break;
          }
          string selectedOption = conditionOptionSelector.SelectedOption;
          this.idTxt.Text = selectedItem3.GetInstanceFieldID(selectedOption);
          this.insertBtn_Click((object) null, (EventArgs) null);
          break;
        case "Ratelock":
          FieldDefinitionObj selectedItem4 = (FieldDefinitionObj) this.listCbo.SelectedItem;
          if (!selectedItem4.MultiInstance)
          {
            this.idTxt.Text = selectedItem4.FieldID;
            break;
          }
          InstanceSelectorDialog instanceSelectorDialog3 = new InstanceSelectorDialog(selectedItem4.Type, selectedItem4.ToString(), true);
          if (instanceSelectorDialog3.ShowDialog((IWin32Window) this) != DialogResult.OK)
            break;
          if (instanceSelectorDialog3.SelectedInstance != "")
          {
            this.idTxt.Text = instanceSelectorDialog3.SelectedInstance;
            this.insertBtn_Click((object) null, (EventArgs) null);
            break;
          }
          this.idTxt.Text = string.Empty;
          break;
        default:
          if (!this.fieldTbl.Contains((object) text))
          {
            this.idTxt.Text = string.Empty;
            break;
          }
          this.idTxt.Text = Convert.ToString(this.fieldTbl[(object) text]);
          break;
      }
    }

    private string encodeToHex(string rawValue)
    {
      string str1 = "";
      string str2 = "VF_";
      for (int index = 0; index < rawValue.Length; ++index)
        str1 = !char.IsLetterOrDigit(rawValue[index]) ? (rawValue[index] != '.' ? str1 + "__" + Utils.HexEncode(rawValue[index]) : str1 + "_") : str1 + rawValue[index].ToString();
      return str2 + str1;
    }

    private void signaturePic_Click(object sender, EventArgs e)
    {
      try
      {
        string name = ((Control) sender).Name;
        string alternativeText = string.Empty;
        switch (name)
        {
          case "borSignaturePic":
            alternativeText = "BorrowerSignature";
            this.hasBorrowerFields = true;
            break;
          case "borInitialsPic":
            alternativeText = "BorrowerInitials";
            this.hasBorrowerFields = true;
            break;
          case "coborSignaturePic":
            alternativeText = "CoborrowerSignature";
            this.hasCoBorrowerFields = true;
            break;
          case "coborInitialsPic":
            alternativeText = "CoborrowerInitials";
            this.hasCoBorrowerFields = true;
            break;
          case "originatorSignaturePic":
            alternativeText = "OriginatorSignature";
            break;
          case "originatorInitialsPic":
            alternativeText = "OriginatorInitials";
            break;
        }
        string str = Path.GetTempPath() + "eSigningTempImage.png";
        ((Image) new ComponentResourceManager(typeof (InsertFieldDialog)).GetObject(name + ".Image")).Save(str, ImageFormat.Png);
        this.wordHandler.InsertInlineShape(str, alternativeText);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, "Exception: " + ex.Message, "Inserting InlineShape", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private void signaturePic_MouseEnter(object sender, EventArgs e)
    {
      switch (((Control) sender).Name)
      {
        case "borSignaturePic":
          this.borSignaturePic.Image = this.signatureImageLst.Images[1];
          break;
        case "borInitialsPic":
          this.borInitialsPic.Image = this.initialsImageLst.Images[1];
          break;
        case "coborSignaturePic":
          this.coborSignaturePic.Image = this.signatureImageLst.Images[3];
          break;
        case "coborInitialsPic":
          this.coborInitialsPic.Image = this.initialsImageLst.Images[3];
          break;
        case "originatorSignaturePic":
          this.originatorSignaturePic.Image = this.signatureImageLst.Images[5];
          break;
        case "originatorInitialsPic":
          this.originatorInitialsPic.Image = this.initialsImageLst.Images[5];
          break;
      }
    }

    private void signaturePic_MouseLeave(object sender, EventArgs e)
    {
      switch (((Control) sender).Name)
      {
        case "borSignaturePic":
          this.borSignaturePic.Image = this.signatureImageLst.Images[0];
          break;
        case "borInitialsPic":
          this.borInitialsPic.Image = this.initialsImageLst.Images[0];
          break;
        case "coborSignaturePic":
          this.coborSignaturePic.Image = this.signatureImageLst.Images[2];
          break;
        case "coborInitialsPic":
          this.coborInitialsPic.Image = this.initialsImageLst.Images[2];
          break;
        case "originatorSignaturePic":
          this.originatorSignaturePic.Image = this.signatureImageLst.Images[4];
          break;
        case "originatorInitialsPic":
          this.originatorInitialsPic.Image = this.initialsImageLst.Images[4];
          break;
      }
    }

    private void InsertFieldDialog_Shown(object sender, EventArgs e)
    {
      this.MinimumSize = new Size(this.Width, this.Height);
      this.MaximumSize = new Size(Screen.PrimaryScreen.Bounds.Width, this.Height);
    }
  }
}
