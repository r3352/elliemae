// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Export.InterimServicingExportWizard
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common.UI.Wizard;
using EllieMae.EMLite.DataEngine.InterimServicing;
using System.ComponentModel;
using System.Drawing;

#nullable disable
namespace EllieMae.EMLite.Export
{
  public class InterimServicingExportWizard : WizardBase
  {
    private IContainer components;

    public InterimServicingExportWizard(ServicingTransactionBase[] transactions)
    {
      this.InitializeComponent();
      this.BackButtonVisible = false;
      this.Current = (WizardItem) new InterimCsvExportPanel(transactions);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pnlFooter.SuspendLayout();
      this.SuspendLayout();
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.BackButtonVisible = true;
      this.ClientSize = new Size(496, 358);
      this.MinimizeBox = false;
      this.Name = nameof (InterimServicingExportWizard);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.Text = "Interim Servicing Export Wizard";
      this.pnlFooter.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
