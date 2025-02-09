// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.Forms.Packages.ExportFormSelectorPanel
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Wizard;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine.Forms.Packages
{
  public class ExportFormSelectorPanel : WizardItemWithHeader
  {
    private CheckedListBox lstForms;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Panel panel2;
    private IContainer components;

    public ExportFormSelectorPanel()
    {
      this.InitializeComponent();
      this.Header = this.Header.Replace("Export", PackageExportWizard.ExportMode.ToString());
      this.loadFormList();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lstForms = new CheckedListBox();
      this.label1 = new System.Windows.Forms.Label();
      this.panel2 = new System.Windows.Forms.Panel();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      this.lstForms.CheckOnClick = true;
      this.lstForms.Location = new Point(91, 62);
      this.lstForms.Name = "lstForms";
      this.lstForms.Size = new Size(322, 154);
      this.lstForms.Sorted = true;
      this.lstForms.TabIndex = 10;
      this.label1.Location = new Point(39, 24);
      this.label1.Name = "label1";
      this.label1.Size = new Size(418, 32);
      this.label1.TabIndex = 11;
      this.label1.Text = "Your package may include one or more custom input forms. Select the forms to be included from the list below.";
      this.panel2.BackColor = Color.White;
      this.panel2.Controls.Add((System.Windows.Forms.Control) this.label1);
      this.panel2.Controls.Add((System.Windows.Forms.Control) this.lstForms);
      this.panel2.Dock = DockStyle.Fill;
      this.panel2.Location = new Point(0, 60);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(496, 254);
      this.panel2.TabIndex = 12;
      this.Controls.Add((System.Windows.Forms.Control) this.panel2);
      this.Header = "Export Package - Form Selection";
      this.Name = nameof (ExportFormSelectorPanel);
      this.Subheader = "";
      this.Controls.SetChildIndex((System.Windows.Forms.Control) this.panel2, 0);
      this.panel2.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void loadFormList()
    {
      foreach (object formInfo in Session.FormManager.GetFormInfos(InputFormType.Custom))
        this.lstForms.Items.Add(formInfo);
      foreach (InputFormInfo form in PackageExportWizard.CurrentPackage.Forms)
      {
        int index = this.lstForms.Items.IndexOf((object) form);
        if (index >= 0)
          this.lstForms.SetItemChecked(index, true);
      }
    }

    public override WizardItem Next()
    {
      try
      {
        Cursor.Current = Cursors.WaitCursor;
        PackageExportWizard.CurrentPackage.Forms.Clear();
        using (FormCache formCache = new FormCache(Session.DefaultInstance))
        {
          foreach (InputFormInfo checkedItem in this.lstForms.CheckedItems)
          {
            using (BinaryObject formData = new BinaryObject(formCache.FetchCustomForm(checkedItem)))
              PackageExportWizard.CurrentPackage.Forms.Add(checkedItem, formData);
            FormParser formParser = new FormParser(new FileInfo(FormStore.GetFormHTMLPath(Session.DefaultInstance, checkedItem)));
            CodeBase codeBase = formParser.GetCodeBase();
            if (codeBase != null)
            {
              string path = formCache.FetchCustomFormAssembly(codeBase.AssemblyName);
              if (path != null)
              {
                using (BinaryObject assemblyData = new BinaryObject(path))
                  PackageExportWizard.CurrentPackage.Assemblies.Add(codeBase.AssemblyName, assemblyData);
              }
            }
            CustomFieldsInfo loanCustomFields = Session.ConfigurationManager.GetLoanCustomFields();
            foreach (string referencedField in formParser.GetReferencedFields())
            {
              if (CustomFieldInfo.IsCustomFieldID(referencedField))
              {
                CustomFieldInfo field = loanCustomFields.GetField(referencedField);
                if (field != null)
                  PackageExportWizard.CurrentPackage.Fields.Add(field);
              }
            }
          }
        }
        return this.GetNextScreen();
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) System.Windows.Forms.Form.ActiveForm, "Error adding forms to package: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return (WizardItem) null;
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    private WizardItem GetNextScreen()
    {
      if (Session.FormManager.GetCustomFormAssemblyNames().Length != 0)
        return (WizardItem) new ExportAssembliesSelectorPanel((WizardItem) this);
      if (Session.ConfigurationManager.GetLoanCustomFields().GetNonEmptyCount() > 0)
        return (WizardItem) new ExportFieldsSelectorPanel((WizardItem) this);
      if (Session.ConfigurationManager.GetPluginAssemblyNames().Length != 0)
        return (WizardItem) new ExportPluginSelectorPanel((WizardItem) this);
      return Session.ConfigurationManager.GetFilteredCustomDataObjectNames().Length != 0 ? (WizardItem) new ExportCustomDataObjectSelectorPanel((WizardItem) this) : (WizardItem) new ExportSummaryPanel((WizardItem) this);
    }
  }
}
