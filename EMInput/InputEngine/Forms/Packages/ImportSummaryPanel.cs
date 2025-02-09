// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.Forms.Packages.ImportSummaryPanel
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
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
  public class ImportSummaryPanel : WizardItemWithHeader
  {
    private Panel panel2;
    private RichTextBox rtfInfo;
    private Label label1;
    private IContainer components;

    public ImportSummaryPanel(WizardItem prevItem)
      : base(prevItem)
    {
      this.InitializeComponent();
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
      this.rtfInfo = new RichTextBox();
      this.label1 = new Label();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      this.panel2.BackColor = Color.White;
      this.panel2.Controls.Add((Control) this.rtfInfo);
      this.panel2.Controls.Add((Control) this.label1);
      this.panel2.Dock = DockStyle.Fill;
      this.panel2.Location = new Point(0, 60);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(496, 254);
      this.panel2.TabIndex = 10;
      this.rtfInfo.Location = new Point(76, 60);
      this.rtfInfo.Name = "rtfInfo";
      this.rtfInfo.ReadOnly = true;
      this.rtfInfo.Size = new Size(340, 134);
      this.rtfInfo.TabIndex = 3;
      this.rtfInfo.TabStop = false;
      this.rtfInfo.Text = "";
      this.label1.Location = new Point(76, 40);
      this.label1.Name = "label1";
      this.label1.Size = new Size(314, 20);
      this.label1.TabIndex = 2;
      this.label1.Text = "The following items will be imported from the package:";
      this.Controls.Add((Control) this.panel2);
      this.Header = "Import Package - Summary";
      this.Name = nameof (ImportSummaryPanel);
      this.Subheader = "";
      this.Controls.SetChildIndex((Control) this.panel2, 0);
      this.panel2.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public override string NextLabel => "&Import";

    public override WizardItem Next()
    {
      try
      {
        if (!PackageImportWizard.PackageImporter.ImportPackage(PackageImportWizard.CurrentPackage))
          return (WizardItem) null;
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "Package import completed successfully.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return WizardItem.Finished;
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "Import failed: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return (WizardItem) null;
      }
    }

    private void createPackageSummary()
    {
      ExportPackage currentPackage = PackageImportWizard.CurrentPackage;
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
  }
}
