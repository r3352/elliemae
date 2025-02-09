// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.Forms.Packages.ExportPluginSelectorPanel
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
  public class ExportPluginSelectorPanel : WizardItemWithHeader
  {
    private IContainer components;
    private Panel panel2;
    private Label label1;
    private CheckedListBox lstPlugins;

    public ExportPluginSelectorPanel(WizardItem prevItem)
      : base(prevItem)
    {
      this.InitializeComponent();
      this.Header = this.Header.Replace("Export", PackageExportWizard.ExportMode.ToString());
      this.loadPlugins();
    }

    private void loadPlugins()
    {
      foreach (object pluginAssemblyName in Session.ConfigurationManager.GetPluginAssemblyNames())
        this.lstPlugins.Items.Add(pluginAssemblyName);
      foreach (string plugin in PackageExportWizard.CurrentPackage.Plugins)
      {
        int index = this.lstPlugins.Items.IndexOf((object) plugin);
        if (index >= 0)
          this.lstPlugins.SetItemChecked(index, true);
      }
    }

    public override WizardItem Next()
    {
      try
      {
        Cursor.Current = Cursors.WaitCursor;
        PackageExportWizard.CurrentPackage.Plugins.Clear();
        foreach (string checkedItem in this.lstPlugins.CheckedItems)
        {
          BinaryObject pluginAssembly = Session.ConfigurationManager.GetPluginAssembly(checkedItem);
          PackageExportWizard.CurrentPackage.Plugins.Add(checkedItem, pluginAssembly);
        }
        return this.GetNextScreen();
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "Error adding plugin(s) to package: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return (WizardItem) null;
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    private WizardItem GetNextScreen()
    {
      return Session.ConfigurationManager.GetFilteredCustomDataObjectNames().Length != 0 ? (WizardItem) new ExportCustomDataObjectSelectorPanel((WizardItem) this) : (WizardItem) new ExportSummaryPanel((WizardItem) this);
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
      this.label1 = new Label();
      this.lstPlugins = new CheckedListBox();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      this.panel2.BackColor = Color.White;
      this.panel2.Controls.Add((Control) this.label1);
      this.panel2.Controls.Add((Control) this.lstPlugins);
      this.panel2.Dock = DockStyle.Fill;
      this.panel2.Location = new Point(0, 60);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(496, 254);
      this.panel2.TabIndex = 13;
      this.label1.Location = new Point(39, 24);
      this.label1.Name = "label1";
      this.label1.Size = new Size(418, 32);
      this.label1.TabIndex = 11;
      this.label1.Text = "Your package may include one or more plugins. Select the plugins to be included from the list below.";
      this.lstPlugins.CheckOnClick = true;
      this.lstPlugins.Location = new Point(91, 62);
      this.lstPlugins.Name = "lstPlugins";
      this.lstPlugins.Size = new Size(322, 154);
      this.lstPlugins.Sorted = true;
      this.lstPlugins.TabIndex = 10;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.panel2);
      this.Header = "Export Package - Plugin Selection";
      this.Name = nameof (ExportPluginSelectorPanel);
      this.Subheader = "";
      this.Controls.SetChildIndex((Control) this.panel2, 0);
      this.panel2.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
