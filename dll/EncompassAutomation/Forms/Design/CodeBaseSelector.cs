// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Design.CodeBaseSelector
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.Encompass.Forms.Design
{
  public class CodeBaseSelector : System.Windows.Forms.Form
  {
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox txtAssembly;
    private System.Windows.Forms.Button btnBrowse;
    private System.Windows.Forms.Label label3;
    private ComboBox cboClass;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnOK;
    private OpenFileDialog ofdBrowse;
    private System.ComponentModel.Container components;
    private System.Windows.Forms.Button btnClear;
    private CodeBase codeBase;

    public CodeBaseSelector(CodeBase codeBase)
    {
      this.InitializeComponent();
      this.codeBase = codeBase;
      try
      {
        if (File.Exists(codeBase.AssemblyPath))
        {
          this.txtAssembly.Text = codeBase.AssemblyPath;
          this.loadClasses();
        }
      }
      catch
      {
      }
      if (!(this.txtAssembly.Text == ""))
        return;
      this.txtAssembly.Text = codeBase.AssemblyName;
      this.cboClass.Items.Clear();
      if (codeBase.ClassName != "")
      {
        this.cboClass.Items.Add((object) codeBase.ClassName);
        this.cboClass.SelectedIndex = 0;
      }
      this.cboClass.Enabled = false;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.txtAssembly = new System.Windows.Forms.TextBox();
      this.btnBrowse = new System.Windows.Forms.Button();
      this.label3 = new System.Windows.Forms.Label();
      this.cboClass = new ComboBox();
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnOK = new System.Windows.Forms.Button();
      this.ofdBrowse = new OpenFileDialog();
      this.btnClear = new System.Windows.Forms.Button();
      this.SuspendLayout();
      this.label1.Location = new Point(20, 16);
      this.label1.Name = "label1";
      this.label1.Size = new Size(298, 36);
      this.label1.TabIndex = 0;
      this.label1.Text = "Your custom form can be set to derive functionality from a Form class in a separate assembly.";
      this.label2.Location = new Point(20, 62);
      this.label2.Name = "label2";
      this.label2.Size = new Size(298, 18);
      this.label2.TabIndex = 1;
      this.label2.Text = "Assembly containing your custom Form class:";
      this.txtAssembly.Location = new Point(20, 80);
      this.txtAssembly.Name = "txtAssembly";
      this.txtAssembly.ReadOnly = true;
      this.txtAssembly.Size = new Size(232, 20);
      this.txtAssembly.TabIndex = 2;
      this.txtAssembly.Text = "";
      this.btnBrowse.Location = new Point(254, 80);
      this.btnBrowse.Name = "btnBrowse";
      this.btnBrowse.Size = new Size(64, 20);
      this.btnBrowse.TabIndex = 3;
      this.btnBrowse.Text = "Browse...";
      this.btnBrowse.Click += new EventHandler(this.btnBrowse_Click);
      this.label3.Location = new Point(20, 116);
      this.label3.Name = "label3";
      this.label3.Size = new Size(298, 18);
      this.label3.TabIndex = 4;
      this.label3.Text = "Form class from which this form will inherit:";
      this.cboClass.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboClass.Location = new Point(20, 134);
      this.cboClass.Name = "cboClass";
      this.cboClass.Size = new Size(298, 21);
      this.cboClass.TabIndex = 5;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(242, 176);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.TabIndex = 6;
      this.btnCancel.Text = "&Cancel";
      this.btnOK.Location = new Point(164, 176);
      this.btnOK.Name = "btnOK";
      this.btnOK.TabIndex = 7;
      this.btnOK.Text = "&OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.ofdBrowse.Filter = ".NET Assemblies (*.dll)|*.dll";
      this.ofdBrowse.Title = "Select Code Base Assembly";
      this.btnClear.Location = new Point(86, 176);
      this.btnClear.Name = "btnClear";
      this.btnClear.TabIndex = 8;
      this.btnClear.Text = "&Clear";
      this.btnClear.Click += new EventHandler(this.btnClear_Click);
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(332, 217);
      this.Controls.Add((System.Windows.Forms.Control) this.btnClear);
      this.Controls.Add((System.Windows.Forms.Control) this.btnOK);
      this.Controls.Add((System.Windows.Forms.Control) this.btnCancel);
      this.Controls.Add((System.Windows.Forms.Control) this.cboClass);
      this.Controls.Add((System.Windows.Forms.Control) this.label3);
      this.Controls.Add((System.Windows.Forms.Control) this.btnBrowse);
      this.Controls.Add((System.Windows.Forms.Control) this.txtAssembly);
      this.Controls.Add((System.Windows.Forms.Control) this.label2);
      this.Controls.Add((System.Windows.Forms.Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.Name = nameof (CodeBaseSelector);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Form Inheritance";
      this.ResumeLayout(false);
    }

    public CodeBase CodeBase
    {
      get
      {
        return this.txtAssembly.Text == "" ? CodeBase.Empty : new CodeBase(this.txtAssembly.Text, Path.GetFileNameWithoutExtension(this.txtAssembly.Text), this.codeBase.AssemblyVersion, this.cboClass.SelectedItem.ToString());
      }
    }

    private void btnBrowse_Click(object sender, EventArgs e)
    {
      try
      {
        this.ofdBrowse.FileName = this.txtAssembly.Text;
      }
      catch
      {
      }
      if (this.ofdBrowse.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      this.txtAssembly.Text = this.ofdBrowse.FileName;
      this.loadClasses();
    }

    private void loadClasses()
    {
      this.cboClass.Items.Clear();
      try
      {
        string tempFileName = Path.GetTempFileName();
        File.Copy(this.txtAssembly.Text, tempFileName, true);
        foreach (Type type in Assembly.LoadFile(tempFileName).GetTypes())
        {
          if (typeof (EllieMae.Encompass.Forms.Form).IsAssignableFrom(type))
            this.cboClass.Items.Add((object) type.FullName);
        }
        try
        {
          this.cboClass.SelectedItem = (object) this.codeBase.ClassName;
        }
        catch
        {
        }
        this.cboClass.Enabled = true;
      }
      catch (TypeLoadException ex)
      {
        this.cboClass.Enabled = false;
        int num = (int) MessageBox.Show((IWin32Window) this, "One or more types from the specified assembly could not be loaded. The most likely cause of this error is that your assembly has one or more dependencies which are not in the Global Assembly Cache.", "Form Code Base Editor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      catch
      {
        this.cboClass.Enabled = false;
        int num = (int) MessageBox.Show((IWin32Window) this, "The specified assembly is not a valid .NET assembly or could not be loaded.", "Form Code Base Editor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.txtAssembly.Text != "" && this.cboClass.SelectedIndex < 0)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, "A suitable base class must be selected in order to set the code base for this form.", "Form Code Base Editor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
        this.DialogResult = DialogResult.OK;
    }

    private void btnClear_Click(object sender, EventArgs e)
    {
      this.txtAssembly.Text = "";
      this.cboClass.Items.Clear();
      this.cboClass.Enabled = false;
    }
  }
}
