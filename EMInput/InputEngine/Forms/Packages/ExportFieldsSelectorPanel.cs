// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.Forms.Packages.ExportFieldsSelectorPanel
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common.UI.Wizard;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.Packages;
using EllieMae.EMLite.RemotingServices;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine.Forms.Packages
{
  public class ExportFieldsSelectorPanel : WizardItemWithHeader
  {
    private Label label1;
    private CheckedListBox lstFields;
    private Panel panel2;
    private IContainer components;

    public ExportFieldsSelectorPanel(WizardItem prevItem)
      : base(prevItem)
    {
      this.InitializeComponent();
      this.Header = this.Header.Replace("Export", PackageExportWizard.ExportMode.ToString());
      this.loadFieldList();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.lstFields = new CheckedListBox();
      this.panel2 = new Panel();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      this.label1.Location = new Point(39, 26);
      this.label1.Name = "label1";
      this.label1.Size = new Size(418, 32);
      this.label1.TabIndex = 13;
      this.label1.Text = "Select the custom field definitions to include in the package. Fields used by the Input Screens included in this package have automatically been selected.";
      this.lstFields.CheckOnClick = true;
      this.lstFields.Location = new Point(91, 64);
      this.lstFields.Name = "lstFields";
      this.lstFields.Size = new Size(322, 154);
      this.lstFields.Sorted = true;
      this.lstFields.TabIndex = 12;
      this.panel2.BackColor = Color.White;
      this.panel2.Controls.Add((Control) this.lstFields);
      this.panel2.Controls.Add((Control) this.label1);
      this.panel2.Dock = DockStyle.Fill;
      this.panel2.Location = new Point(0, 60);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(496, 254);
      this.panel2.TabIndex = 14;
      this.Controls.Add((Control) this.panel2);
      this.Header = "Export Package - Custom Fields Selection";
      this.Name = nameof (ExportFieldsSelectorPanel);
      this.Subheader = "";
      this.Controls.SetChildIndex((Control) this.panel2, 0);
      this.panel2.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void loadFieldList()
    {
      CustomFieldsInfo loanCustomFields = Session.ConfigurationManager.GetLoanCustomFields();
      ExportPackage currentPackage = PackageExportWizard.CurrentPackage;
      foreach (CustomFieldInfo field in loanCustomFields)
      {
        if (!field.IsEmpty())
          this.lstFields.Items.Add((object) field, currentPackage.Fields.Contains(field));
      }
    }

    public override WizardItem Next()
    {
      ExportPackage currentPackage = PackageExportWizard.CurrentPackage;
      currentPackage.Fields.Clear();
      foreach (CustomFieldInfo checkedItem in this.lstFields.CheckedItems)
        currentPackage.Fields.Add(checkedItem);
      return this.GetNextScreen();
    }

    private WizardItem GetNextScreen()
    {
      if (Session.ConfigurationManager.GetPluginAssemblyNames().Length != 0)
        return (WizardItem) new ExportPluginSelectorPanel((WizardItem) this);
      return Session.ConfigurationManager.GetFilteredCustomDataObjectNames().Length != 0 ? (WizardItem) new ExportCustomDataObjectSelectorPanel((WizardItem) this) : (WizardItem) new ExportSummaryPanel((WizardItem) this);
    }
  }
}
