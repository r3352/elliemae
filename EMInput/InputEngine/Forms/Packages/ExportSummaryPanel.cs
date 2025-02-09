// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.Forms.Packages.ExportSummaryPanel
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Login;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Wizard;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.Packages;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine.Forms.Packages
{
  public class ExportSummaryPanel : WizardItemWithHeader
  {
    private Panel panel2;
    private RichTextBox rtfInfo;
    private SaveFileDialog sfdBrowse;
    private Label lblCaption;
    private Label lblPublish;
    private IContainer components;

    public ExportSummaryPanel(WizardItem prevItem)
      : base(prevItem)
    {
      this.InitializeComponent();
      this.Header = this.Header.Replace("Export", PackageExportWizard.ExportMode.ToString());
      if (PackageExportWizard.ExportMode == PackageExportMode.Publish)
      {
        this.lblCaption.Text = "The following items will be published:";
        this.lblPublish.Visible = true;
      }
      this.createPackageSummary();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.panel2 = new Panel();
      this.lblPublish = new Label();
      this.rtfInfo = new RichTextBox();
      this.lblCaption = new Label();
      this.sfdBrowse = new SaveFileDialog();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      this.panel2.BackColor = Color.White;
      this.panel2.Controls.Add((Control) this.lblPublish);
      this.panel2.Controls.Add((Control) this.rtfInfo);
      this.panel2.Controls.Add((Control) this.lblCaption);
      this.panel2.Dock = DockStyle.Fill;
      this.panel2.Location = new Point(0, 60);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(496, 254);
      this.panel2.TabIndex = 10;
      this.lblPublish.Location = new Point(4, 230);
      this.lblPublish.Name = "lblPublish";
      this.lblPublish.Size = new Size(400, 20);
      this.lblPublish.TabIndex = 2;
      this.lblPublish.Text = "To log in to the remote server and publish this package, click Next.";
      this.lblPublish.TextAlign = ContentAlignment.BottomLeft;
      this.lblPublish.Visible = false;
      this.rtfInfo.Location = new Point(78, 55);
      this.rtfInfo.Name = "rtfInfo";
      this.rtfInfo.ReadOnly = true;
      this.rtfInfo.Size = new Size(340, 134);
      this.rtfInfo.TabIndex = 1;
      this.rtfInfo.TabStop = false;
      this.rtfInfo.Text = "";
      this.lblCaption.Location = new Point(78, 37);
      this.lblCaption.Name = "lblCaption";
      this.lblCaption.Size = new Size(314, 16);
      this.lblCaption.TabIndex = 0;
      this.lblCaption.Text = "The generated package will contain the following items:";
      this.sfdBrowse.DefaultExt = "empkg";
      this.sfdBrowse.Filter = "Encompass Package Files (*.empkg)|*.empkg|All Files (*.*)|*.*";
      this.sfdBrowse.Title = "Save the Export Package";
      this.Controls.Add((Control) this.panel2);
      this.Header = "Export Package - Summary";
      this.Name = nameof (ExportSummaryPanel);
      this.Subheader = "";
      this.Controls.SetChildIndex((Control) this.panel2, 0);
      this.panel2.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public override string NextLabel
    {
      get
      {
        return PackageExportWizard.ExportMode != PackageExportMode.Export ? base.NextLabel : "E&xport";
      }
    }

    public override WizardItem Next()
    {
      if (!PackageExportWizard.CurrentPackage.HasItem)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must specify one or more items to include in the package.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return (WizardItem) null;
      }
      if (PackageExportWizard.ExportMode == PackageExportMode.Publish)
      {
        if (!LoginUtil.IsTokenLoginOnly)
          return (WizardItem) new ExportPublishLoginPanel((WizardItem) this);
        ThinThickForm loginForm = new LoginUtil().GetLoginForm(AppName.InputFormBuilder, "", ExportSummaryPanel.argumentExists("-us"), new Func<LoginContext, bool>(LoginHelper.LoginFormEditor), LoginUtil.DefaultInstanceID);
        int num = (int) loginForm.ShowDialog((IWin32Window) this);
        if (loginForm.DialogResult != DialogResult.OK)
          return (WizardItem) null;
        ExportPublishLoginPanel publishLoginPanel = new ExportPublishLoginPanel((WizardItem) this, LoginHelper.loginConnection);
        return WizardItem.Finished;
      }
      if (this.sfdBrowse.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return (WizardItem) null;
      try
      {
        PackageExportWizard.CurrentPackage.Save(this.sfdBrowse.FileName);
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "The package has been successfully exported.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return WizardItem.Finished;
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "Export failed: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return (WizardItem) null;
      }
    }

    private void createPackageSummary()
    {
      ExportPackage currentPackage = PackageExportWizard.CurrentPackage;
      if (currentPackage.Forms.Count > 0)
      {
        this.writeBold("Input Screens:");
        foreach (InputFormInfo form in currentPackage.Forms)
          this.writeIndented(form.Name);
        this.writeBold("");
      }
      if (currentPackage.Assemblies.Count > 0)
      {
        this.writeBold("Assemblies:");
        foreach (string assembly in currentPackage.Assemblies)
          this.writeIndented(assembly);
        this.writeBold("");
      }
      if (currentPackage.Fields.Count > 0)
      {
        this.writeBold("Custom Fields:");
        foreach (CustomFieldInfo field in currentPackage.Fields)
          this.writeIndented(field.FieldID + " (" + field.Description + ")");
        this.writeBold("");
      }
      if (currentPackage.Plugins.Count > 0)
      {
        this.writeBold("Plugins:");
        foreach (string plugin in currentPackage.Plugins)
          this.writeIndented(plugin);
        this.writeBold("");
      }
      if (currentPackage.CustomDataObjects.Count > 0)
      {
        this.writeBold("Custom Data Objects:");
        foreach (string customDataObject in currentPackage.CustomDataObjects)
          this.writeIndented(customDataObject);
        this.writeBold("");
      }
      this.rtfInfo.SelectionStart = 0;
      this.rtfInfo.ScrollToCaret();
    }

    private void writeBold(string text)
    {
      this.rtfInfo.SelectionStart = this.rtfInfo.Text.Length;
      this.rtfInfo.SelectionFont = new Font(this.rtfInfo.Font.Name, this.rtfInfo.Font.Size, FontStyle.Bold);
      this.rtfInfo.AppendText(text + Environment.NewLine);
      this.rtfInfo.SelectionStart = this.rtfInfo.Text.Length;
      this.rtfInfo.SelectionFont = this.rtfInfo.Font;
    }

    private void writeIndented(string text)
    {
      this.rtfInfo.SelectionStart = this.rtfInfo.Text.Length;
      this.rtfInfo.SelectionBullet = true;
      this.rtfInfo.AppendText(text + Environment.NewLine);
      this.rtfInfo.SelectionStart = this.rtfInfo.Text.Length;
      this.rtfInfo.SelectionBullet = false;
    }

    private static bool argumentExists(string arg)
    {
      foreach (string commandLineArg in Environment.GetCommandLineArgs())
      {
        if (arg == commandLineArg.ToLower())
          return true;
      }
      return false;
    }
  }
}
