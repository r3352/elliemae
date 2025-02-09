// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.DisclosedSSPLDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class DisclosedSSPLDialog : Form, IOnlineHelpTarget, IHelp
  {
    private const string className = "DisclosedSSPLDialog";
    private static string sw = Tracing.SwInputEngine;
    private string formName = "Settlement Service Providers List Snapshot";
    private IDisclosureTracking2015Log log;
    private IHtmlInput input;
    private static object nobj = (object) Missing.Value;
    private IContainer components;
    private Panel panel1;
    private Button btnCancel;

    public DisclosedSSPLDialog(IDisclosureTracking2015Log log)
    {
      this.InitializeComponent();
      this.log = log;
      this.initialPage();
    }

    private void initialPage()
    {
      if (new InputFormList(Session.SessionObjects).GetFormByName(this.formName) == (InputFormInfo) null)
      {
        InputFormInfo inputFormInfo = new InputFormInfo("SettlementServiceProvider", "Settlement Service Provider");
      }
      this.log.GetDisclosedFields(DisclosureTracking2015Log.DisclosedSSPLFields);
      this.input = (IHtmlInput) new DisclosedSSPLHandler(this.log, LoanReportFieldDefs.GetFieldDefs(Session.DefaultInstance, LoanReportFieldFlags.AllLoanDataFields));
      new SettlementServiceForm(this.input, true, Session.DefaultInstance, true).Parent = (Control) this.panel1;
    }

    public string GetHelpTargetName() => "";

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, this.GetHelpTargetName());
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (DisclosedSSPLDialog));
      this.panel1 = new Panel();
      this.btnCancel = new Button();
      this.SuspendLayout();
      this.panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.panel1.BackColor = Color.Transparent;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(786, 529);
      this.panel1.TabIndex = 11;
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(699, 535);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 10;
      this.btnCancel.Text = "Close";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(786, 570);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.btnCancel);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (DisclosedSSPLDialog);
      this.Text = "Settlement Service Providers List Snapshot";
      this.ResumeLayout(false);
    }
  }
}
