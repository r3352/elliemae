// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.Forms.Packages.ExportAssembliesSelectorPanel
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Wizard;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine.Forms.Packages
{
  public class ExportAssembliesSelectorPanel : WizardItemWithHeader
  {
    private Label label1;
    private CheckedListBox lstAssemblies;
    private Panel panel2;
    private IContainer components;

    public ExportAssembliesSelectorPanel(WizardItem prevItem)
      : base(prevItem)
    {
      this.InitializeComponent();
      this.Header = this.Header.Replace("Export", PackageExportWizard.ExportMode.ToString());
      this.loadAssemblyList();
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
      this.lstAssemblies = new CheckedListBox();
      this.panel2 = new Panel();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      this.label1.Location = new Point(39, 26);
      this.label1.Name = "label1";
      this.label1.Size = new Size(418, 42);
      this.label1.TabIndex = 13;
      this.label1.Text = "Select the custom input form assemblies to include in the package. The assemblies used by the forms you selected on the prior screen have been automatically selected for inclusion in the package.";
      this.lstAssemblies.CheckOnClick = true;
      this.lstAssemblies.Location = new Point(89, 72);
      this.lstAssemblies.Name = "lstAssemblies";
      this.lstAssemblies.Size = new Size(322, 139);
      this.lstAssemblies.Sorted = true;
      this.lstAssemblies.TabIndex = 12;
      this.panel2.BackColor = Color.White;
      this.panel2.Controls.Add((Control) this.lstAssemblies);
      this.panel2.Controls.Add((Control) this.label1);
      this.panel2.Dock = DockStyle.Fill;
      this.panel2.Location = new Point(0, 60);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(496, 254);
      this.panel2.TabIndex = 14;
      this.Controls.Add((Control) this.panel2);
      this.Header = "Export Package - Assembly Selection";
      this.Name = nameof (ExportAssembliesSelectorPanel);
      this.Subheader = "";
      this.Controls.SetChildIndex((Control) this.panel2, 0);
      this.panel2.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void loadAssemblyList()
    {
      foreach (object formAssemblyName in Session.FormManager.GetCustomFormAssemblyNames())
        this.lstAssemblies.Items.Add(formAssemblyName);
      foreach (string assembly in PackageExportWizard.CurrentPackage.Assemblies)
      {
        int index = this.lstAssemblies.Items.IndexOf((object) assembly);
        if (index >= 0)
          this.lstAssemblies.SetItemChecked(index, true);
      }
    }

    public override WizardItem Next()
    {
      try
      {
        Cursor.Current = Cursors.WaitCursor;
        PackageExportWizard.CurrentPackage.Assemblies.Clear();
        using (FormCache formCache = new FormCache(Session.DefaultInstance))
        {
          foreach (string checkedItem in this.lstAssemblies.CheckedItems)
          {
            using (BinaryObject assemblyData = new BinaryObject(formCache.FetchCustomFormAssembly(checkedItem)))
              PackageExportWizard.CurrentPackage.Assemblies.Add(checkedItem, assemblyData);
          }
        }
        return this.GetNextScreen();
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "Error adding forms to package: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return (WizardItem) null;
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    private WizardItem GetNextScreen()
    {
      if (Session.ConfigurationManager.GetLoanCustomFields().GetNonEmptyCount() > 0)
        return (WizardItem) new ExportFieldsSelectorPanel((WizardItem) this);
      if (Session.ConfigurationManager.GetPluginAssemblyNames().Length != 0)
        return (WizardItem) new ExportPluginSelectorPanel((WizardItem) this);
      return Session.ConfigurationManager.GetFilteredCustomDataObjectNames().Length != 0 ? (WizardItem) new ExportCustomDataObjectSelectorPanel((WizardItem) this) : (WizardItem) new ExportSummaryPanel((WizardItem) this);
    }
  }
}
