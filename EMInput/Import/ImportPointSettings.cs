// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Import.ImportPointSettings
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.Import
{
  public class ImportPointSettings : Form
  {
    private const string className = "ImportPointSettings";
    private static readonly string sw = Tracing.SwImportExport;
    private Button importBtn;
    private Button cancelBtn;
    private Label label1;
    private GroupBox groupBox1;
    private FolderBrowserDialog folderDialog;
    private CheckBox escrowChk;
    private CheckBox titleChk;
    private CheckBox feeChk;
    private CheckBox disclosureChk;
    private CheckBox customChk;
    private TextBox templatesTxt;
    private Button templatesBtn;
    private Label templatesLbl;
    private Label dataLbl;
    private TextBox dataTxt;
    private Button dataBtn;
    private System.ComponentModel.Container components;
    private string templatePath = string.Empty;
    private string dataPath = string.Empty;
    private string type = string.Empty;

    [DllImport("kernel32", EntryPoint = "GetPrivateProfileStringA", CharSet = CharSet.Ansi)]
    private static extern int GetPrivateProfileString(
      string sectionName,
      string keyName,
      string defaultValue,
      StringBuilder returnbuffer,
      int buffersize,
      string filename);

    public ImportPointSettings(string type)
    {
      this.InitializeComponent();
      this.ResetAll();
      this.importBtn.Enabled = true;
      this.cancelBtn.Enabled = true;
      this.type = type;
      if (!((FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features)).CheckPermission(AclFeature.SettingsTab_Company_LoanCustomFields, Session.UserInfo))
        this.customChk.Enabled = false;
      this.templatePath = this.getInstallationPath("Templates");
      this.dataPath = this.getInstallationPath("Folder0");
      this.templatesTxt.Text = this.templatePath;
      this.dataTxt.Text = this.dataPath;
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(type))
      {
        case 230981954:
          if (!(type == "city"))
            break;
          this.feeChk.Checked = true;
          break;
        case 825816201:
          if (!(type == "Title Fee (Refinance)"))
            break;
          this.titleChk.Checked = true;
          break;
        case 1565807884:
          if (!(type == "Escrow Fee (Purchase)"))
            break;
          this.escrowChk.Checked = true;
          break;
        case 1618501362:
          if (!(type == "user"))
            break;
          this.feeChk.Checked = true;
          break;
        case 1642923690:
          if (!(type == "Escrow Fee (Refinance)"))
            break;
          this.escrowChk.Checked = true;
          break;
        case 1678290829:
          if (!(type == "Title Fee (Purchase)"))
            break;
          this.titleChk.Checked = true;
          break;
        case 2016490230:
          if (!(type == "state"))
            break;
          this.feeChk.Checked = true;
          break;
        case 2721769847:
          if (!(type == "Custom Fields"))
            break;
          this.customChk.Checked = true;
          break;
        case 3190415608:
          if (!(type == "Disclosure"))
            break;
          this.disclosureChk.Checked = true;
          break;
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
      this.importBtn = new Button();
      this.cancelBtn = new Button();
      this.label1 = new Label();
      this.templatesTxt = new TextBox();
      this.templatesBtn = new Button();
      this.escrowChk = new CheckBox();
      this.titleChk = new CheckBox();
      this.feeChk = new CheckBox();
      this.customChk = new CheckBox();
      this.disclosureChk = new CheckBox();
      this.groupBox1 = new GroupBox();
      this.folderDialog = new FolderBrowserDialog();
      this.templatesLbl = new Label();
      this.dataLbl = new Label();
      this.dataTxt = new TextBox();
      this.dataBtn = new Button();
      this.SuspendLayout();
      this.importBtn.BackColor = SystemColors.Control;
      this.importBtn.Location = new Point(248, 304);
      this.importBtn.Name = "importBtn";
      this.importBtn.Size = new Size(75, 24);
      this.importBtn.TabIndex = 22;
      this.importBtn.Text = "&Import";
      this.importBtn.Click += new EventHandler(this.importBtn_Click);
      this.cancelBtn.BackColor = SystemColors.Control;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(336, 304);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 24);
      this.cancelBtn.TabIndex = 23;
      this.cancelBtn.Text = "&Cancel";
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.ForeColor = SystemColors.ControlText;
      this.label1.Location = new Point(24, 8);
      this.label1.Name = "label1";
      this.label1.Size = new Size(400, 16);
      this.label1.TabIndex = 12;
      this.label1.Text = "Select and verify the location of the setting(s) you want to import";
      this.templatesTxt.Location = new Point(120, 32);
      this.templatesTxt.Name = "templatesTxt";
      this.templatesTxt.Size = new Size(216, 20);
      this.templatesTxt.TabIndex = 2;
      this.templatesTxt.Text = "";
      this.templatesBtn.Location = new Point(344, 32);
      this.templatesBtn.Name = "templatesBtn";
      this.templatesBtn.Size = new Size(68, 24);
      this.templatesBtn.TabIndex = 3;
      this.templatesBtn.Text = "Browse...";
      this.templatesBtn.Click += new EventHandler(this.templatesBtn_Click);
      this.escrowChk.Location = new Point(144, 72);
      this.escrowChk.Name = "escrowChk";
      this.escrowChk.TabIndex = 1;
      this.escrowChk.Text = "Escrow Tables";
      this.titleChk.Location = new Point(144, 104);
      this.titleChk.Name = "titleChk";
      this.titleChk.TabIndex = 4;
      this.titleChk.Text = "Title Tables";
      this.feeChk.Location = new Point(144, 224);
      this.feeChk.Name = "feeChk";
      this.feeChk.Size = new Size(152, 24);
      this.feeChk.TabIndex = 7;
      this.feeChk.Text = "City, State and User Fees";
      this.customChk.Location = new Point(144, 136);
      this.customChk.Name = "customChk";
      this.customChk.Size = new Size(152, 24);
      this.customChk.TabIndex = 13;
      this.customChk.Text = "Custom Screen Settings";
      this.disclosureChk.Location = new Point(144, 256);
      this.disclosureChk.Name = "disclosureChk";
      this.disclosureChk.Size = new Size(128, 24);
      this.disclosureChk.TabIndex = 19;
      this.disclosureChk.Text = "Servicing Disclosure";
      this.groupBox1.Location = new Point(18, 176);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(390, 3);
      this.groupBox1.TabIndex = 49;
      this.groupBox1.TabStop = false;
      this.folderDialog.ShowNewFolderButton = false;
      this.templatesLbl.Location = new Point(16, 32);
      this.templatesLbl.Name = "templatesLbl";
      this.templatesLbl.Size = new Size(112, 23);
      this.templatesLbl.TabIndex = 50;
      this.templatesLbl.Text = "Templates Location:";
      this.dataLbl.Location = new Point(24, 192);
      this.dataLbl.Name = "dataLbl";
      this.dataLbl.Size = new Size(80, 23);
      this.dataLbl.TabIndex = 53;
      this.dataLbl.Text = "Data Location:";
      this.dataTxt.Location = new Point(104, 192);
      this.dataTxt.Name = "dataTxt";
      this.dataTxt.Size = new Size(224, 20);
      this.dataTxt.TabIndex = 51;
      this.dataTxt.Text = "";
      this.dataBtn.Location = new Point(344, 192);
      this.dataBtn.Name = "dataBtn";
      this.dataBtn.Size = new Size(68, 24);
      this.dataBtn.TabIndex = 52;
      this.dataBtn.Text = "Browse...";
      this.dataBtn.Click += new EventHandler(this.dataBtn_Click);
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(426, 342);
      this.Controls.Add((Control) this.dataLbl);
      this.Controls.Add((Control) this.dataTxt);
      this.Controls.Add((Control) this.templatesTxt);
      this.Controls.Add((Control) this.dataBtn);
      this.Controls.Add((Control) this.templatesLbl);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.disclosureChk);
      this.Controls.Add((Control) this.customChk);
      this.Controls.Add((Control) this.feeChk);
      this.Controls.Add((Control) this.titleChk);
      this.Controls.Add((Control) this.escrowChk);
      this.Controls.Add((Control) this.templatesBtn);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.importBtn);
      this.Controls.Add((Control) this.cancelBtn);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ImportPointSettings);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Import Settings from Calyx Point";
      this.ResumeLayout(false);
    }

    [STAThread]
    private void importBtn_Click(object sender, EventArgs e)
    {
      this.Cursor = Cursors.WaitCursor;
      ArrayList arrayList = new ArrayList();
      int num1 = 0;
      foreach (Control control in (ArrangedElementCollection) this.Controls)
      {
        if (control is CheckBox)
        {
          CheckBox checkBox = (CheckBox) control;
          if (checkBox.Checked)
          {
            switch (checkBox.Name)
            {
              case "escrowChk":
                if (this.ImportEscrowTables() == -1)
                {
                  arrayList.Add((object) "Escrow Tables");
                  continue;
                }
                ++num1;
                continue;
              case "titleChk":
                if (this.ImportTitleTables() == -1)
                {
                  arrayList.Add((object) "Title Tables");
                  continue;
                }
                ++num1;
                continue;
              case "feeChk":
                if (this.ImportFeeTables() == -1)
                {
                  arrayList.Add((object) "City, State and User Fees");
                  continue;
                }
                ++num1;
                continue;
              case "customChk":
                int num2 = this.ImportCustomFields();
                if (num2 == -1)
                {
                  arrayList.Add((object) "Custom Screen");
                  continue;
                }
                if (num2 > 0)
                {
                  ++num1;
                  continue;
                }
                continue;
              case "disclosureChk":
                if (this.ImportDisclosureInfo() == -1)
                {
                  arrayList.Add((object) "Servicing Disclosure");
                  continue;
                }
                ++num1;
                continue;
              default:
                continue;
            }
          }
        }
      }
      this.Cursor = Cursors.Default;
      if (arrayList.Count == 0)
      {
        if (num1 <= 0)
          return;
        int num3 = (int) Utils.Dialog((IWin32Window) this, "Your settings were successfully imported.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        string str = "";
        for (int index = 0; index < arrayList.Count; ++index)
          str = !(str == "") ? str + arrayList[index].ToString() + ", " : arrayList[index].ToString();
        string text = "";
        if (num1 > 0 && str == string.Empty)
          text = "The selected settings were successfully imported.";
        else if (num1 > 0 && str != string.Empty)
          text = "Some of your settings were successfully imported. The following import(s) have problem: " + str;
        else if (num1 == 0 && str == string.Empty)
          text = "The importing has aborted.";
        else if (num1 == 0 && str != string.Empty)
          text = "All selected seetings can't be imported. The following import(s) have problem: " + str;
        int num4 = (int) Utils.Dialog((IWin32Window) this, text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      this.DialogResult = DialogResult.OK;
    }

    private void ResetAll()
    {
      foreach (Control control in (ArrangedElementCollection) this.Controls)
      {
        if (control is CheckBox)
          ((CheckBox) control).Checked = false;
      }
    }

    private int ImportEscrowTables()
    {
      string text = this.templatesTxt.Text;
      if (text == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You need to select the Escrow Tables path in order to import.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return -1;
      }
      string path = text + "Escrow\\";
      if (!Directory.Exists(path))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Unable to find '" + path + "'. Make sure that this folder exist.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return -1;
      }
      PointTableImport pointTableImport = new PointTableImport(path, "Escrow");
      return 1;
    }

    private int ImportTitleTables()
    {
      string text = this.templatesTxt.Text;
      if (text == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You need to select the Title Tables path in order to import.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return -1;
      }
      string path = text + "Title\\";
      if (!Directory.Exists(path))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Unable to find '" + path + "'. Make sure that this folder exist.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return -1;
      }
      PointTableImport pointTableImport = new PointTableImport(path, "Title");
      return 1;
    }

    private int ImportFeeTables()
    {
      string text = this.dataTxt.Text;
      if (text == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You need to select the Fee Tables path in order to import.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return -1;
      }
      string path = text + "RATES.INI";
      if (!File.Exists(path))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "File '" + path + "' does not exists. Please verify your path and try again.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return -1;
      }
      PointFeeImport pointFeeImport = new PointFeeImport(text);
      return 1;
    }

    private int ImportCustomFields()
    {
      if (Utils.Dialog((IWin32Window) this, "Please Note that your existing Custom Fields will get overwritten by the imported ones. Do you wish to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
        return 0;
      string text = this.templatesTxt.Text;
      if (text == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You need to select the Custom Screen Settings path in order to import.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return -1;
      }
      PointCustomFieldsImport customFieldsImport = new PointCustomFieldsImport(text);
      return 1;
    }

    private int ImportDisclosureInfo()
    {
      string text = this.dataTxt.Text;
      if (text == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You need to select the Servicing Disclosure Settings path in order to import.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return -1;
      }
      PointSDSImport pointSdsImport = new PointSDSImport(text);
      return 1;
    }

    private void dataBtn_Click(object sender, EventArgs e)
    {
      this.folderDialog.Description = "Select Point Data Folder";
      this.folderDialog.SelectedPath = this.dataPath;
      if (this.folderDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      this.dataTxt.Text = ImportPointSettings.normalizePath(this.folderDialog.SelectedPath);
    }

    private void templatesBtn_Click(object sender, EventArgs e)
    {
      this.folderDialog.Description = "Select Point Templates Folder";
      this.folderDialog.SelectedPath = this.templatePath;
      if (this.folderDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      this.templatesTxt.Text = ImportPointSettings.normalizePath(this.folderDialog.SelectedPath);
    }

    private static string normalizePath(string path) => path.EndsWith("\\") ? path : path + "\\";

    public string GetPointInstallationPath(string keyName)
    {
      string str = Environment.GetEnvironmentVariable("winDir") + "\\winpoint.ini";
      if (!File.Exists(str))
        return string.Empty;
      StringBuilder returnbuffer = new StringBuilder(256);
      ImportPointSettings.GetPrivateProfileString("Directories", keyName, "", returnbuffer, 256, str);
      string path = returnbuffer.ToString();
      return path == string.Empty ? path : ImportPointSettings.normalizePath(path);
    }

    private string getInstallationPath(string keyName)
    {
      string str = Environment.GetEnvironmentVariable("winDir") + "\\winpoint.ini";
      if (!File.Exists(str))
        return string.Empty;
      StringBuilder returnbuffer = new StringBuilder(256);
      ImportPointSettings.GetPrivateProfileString("Directories", keyName, "", returnbuffer, 256, str);
      string path = returnbuffer.ToString();
      if (!(path == string.Empty))
        return ImportPointSettings.normalizePath(path);
      int num = (int) Utils.Dialog((IWin32Window) this, "Unable to find default path. If you do not have Calyx Point installed then browse or type in the folder name where you want to import from.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      return string.Empty;
    }
  }
}
