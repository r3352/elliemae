// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.Features.UserModulesDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using Elli.Metrics.Client;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.Features
{
  public class UserModulesDialog : Form
  {
    private const string className = "UserModulesDialog";
    private static string sw = Tracing.SwEpass;
    private string userID;
    private Label modulesLbl;
    private Button closeBtn;
    private ListView modulesLvw;
    private ColumnHeader moduleHdr;
    private Label noneLbl;
    private System.ComponentModel.Container components;

    public UserModulesDialog(string userID)
    {
      this.InitializeComponent();
      this.userID = userID;
      this.loadModuleList();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.modulesLbl = new Label();
      this.closeBtn = new Button();
      this.modulesLvw = new ListView();
      this.moduleHdr = new ColumnHeader();
      this.noneLbl = new Label();
      this.SuspendLayout();
      this.modulesLbl.Location = new Point(12, 12);
      this.modulesLbl.Name = "modulesLbl";
      this.modulesLbl.Size = new Size(284, 28);
      this.modulesLbl.TabIndex = 0;
      this.modulesLbl.Text = "Select the add-ons from the list below to which the user should have access.";
      this.closeBtn.DialogResult = DialogResult.OK;
      this.closeBtn.Location = new Point(212, 204);
      this.closeBtn.Name = "closeBtn";
      this.closeBtn.Size = new Size(80, 24);
      this.closeBtn.TabIndex = 3;
      this.closeBtn.Text = "&Close";
      this.modulesLvw.CheckBoxes = true;
      this.modulesLvw.Columns.AddRange(new ColumnHeader[1]
      {
        this.moduleHdr
      });
      this.modulesLvw.HeaderStyle = ColumnHeaderStyle.None;
      this.modulesLvw.Location = new Point(12, 48);
      this.modulesLvw.MultiSelect = false;
      this.modulesLvw.Name = "modulesLvw";
      this.modulesLvw.Size = new Size(280, 148);
      this.modulesLvw.Sorting = SortOrder.Ascending;
      this.modulesLvw.TabIndex = 1;
      this.modulesLvw.UseCompatibleStateImageBehavior = false;
      this.modulesLvw.View = View.Details;
      this.moduleHdr.Text = "Add-On Name";
      this.moduleHdr.Width = 245;
      this.noneLbl.BackColor = SystemColors.Window;
      this.noneLbl.Location = new Point(16, 52);
      this.noneLbl.Name = "noneLbl";
      this.noneLbl.Size = new Size(272, 140);
      this.noneLbl.TabIndex = 2;
      this.noneLbl.Text = "No Modules Available";
      this.noneLbl.TextAlign = ContentAlignment.MiddleCenter;
      this.noneLbl.Visible = false;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.closeBtn;
      this.ClientSize = new Size(304, 237);
      this.Controls.Add((Control) this.noneLbl);
      this.Controls.Add((Control) this.modulesLvw);
      this.Controls.Add((Control) this.closeBtn);
      this.Controls.Add((Control) this.modulesLbl);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (UserModulesDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Licensed Add-On Access";
      this.Load += new EventHandler(this.UserModulesDialog_Load);
      this.ResumeLayout(false);
    }

    private void loadModuleList()
    {
      EncompassModule[] encompassModuleArray = (EncompassModule[]) null;
      EncompassModule[] array = (EncompassModule[]) null;
      try
      {
        encompassModuleArray = Modules.GetClientModules();
        array = Modules.GetUserModules(this.userID);
      }
      catch (Exception ex)
      {
        MetricsFactory.IncrementErrorCounter(ex, "The following error occurred when", "D:\\ws\\24.3.0.0\\EmLite\\SetupUI\\Setup\\Features\\UserModulesDialog.cs", nameof (loadModuleList), 162);
        Tracing.Log(UserModulesDialog.sw, nameof (UserModulesDialog), TraceLevel.Error, ex.Message);
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "The following error occurred when trying to retrieve the company and user module licenses:\n\n" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      if (encompassModuleArray != null)
      {
        EncompassModuleNameProvider moduleNameProvider = new EncompassModuleNameProvider();
        foreach (EncompassModule module in encompassModuleArray)
        {
          string baseModuleName = moduleNameProvider.GetBaseModuleName(module);
          bool flag = false;
          if (array != null)
            flag = Array.IndexOf<EncompassModule>(array, module) >= 0;
          ListViewItem listViewItem = this.modulesLvw.Items.Add(baseModuleName);
          listViewItem.Checked = flag;
          listViewItem.Tag = (object) module;
        }
      }
      else
        this.noneLbl.Visible = true;
    }

    private void modulesLvw_ItemCheck(object sender, ItemCheckEventArgs e)
    {
      EncompassModule tag = (EncompassModule) this.modulesLvw.Items[e.Index].Tag;
      try
      {
        if (e.NewValue == CheckState.Checked)
          Modules.EnableModuleUser(tag, this.userID);
        else
          Modules.DisableModuleUser(tag, this.userID);
      }
      catch (Exception ex)
      {
        MetricsFactory.IncrementErrorCounter(ex, "The following error occurred when", "D:\\ws\\24.3.0.0\\EmLite\\SetupUI\\Setup\\Features\\UserModulesDialog.cs", nameof (modulesLvw_ItemCheck), 214);
        Tracing.Log(UserModulesDialog.sw, nameof (UserModulesDialog), TraceLevel.Error, ex.Message);
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "The following error occurred when trying to enable the module:\n\n" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        e.NewValue = e.CurrentValue;
      }
    }

    private void UserModulesDialog_Load(object sender, EventArgs e)
    {
      this.modulesLvw.ItemCheck += new ItemCheckEventHandler(this.modulesLvw_ItemCheck);
    }
  }
}
