// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AddLoanActionsForPersonaAccess
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class AddLoanActionsForPersonaAccess : Form
  {
    private const string className = "AddLoanActionsForPersonaAccess";
    private Sessions.Session session;
    private static readonly string sw = Tracing.SwOutsideLoan;
    private List<string> selectedLoanActions;
    private IContainer components;
    private DialogButtons dialogButtons1;
    private Panel panelForListView;
    private GridView lvwLoanActionList;

    public AddLoanActionsForPersonaAccess(Sessions.Session session)
    {
      this.InitializeComponent();
      this.session = session;
      this.initForm();
    }

    private void initForm()
    {
      this.lvwLoanActionList.Items.Clear();
      TriggerActivationNameProvider activationNameProvider = new TriggerActivationNameProvider();
      string[] activationTypesByParent = new TriggerActivationNameProvider().GetActivationTypesByParent("TPO actions");
      this.lvwLoanActionList.BeginUpdate();
      try
      {
        if (Tracing.IsSwitchActive(AddLoanActionsForPersonaAccess.sw, TraceLevel.Verbose))
          Tracing.Log(AddLoanActionsForPersonaAccess.sw, TraceLevel.Verbose, nameof (AddLoanActionsForPersonaAccess), "initForm: Loading all document tracking...");
        for (int index = 0; index < activationTypesByParent.Length; ++index)
        {
          TriggerActivationType triggerActivationType = (TriggerActivationType) new TriggerActivationNameProvider().GetValue(activationTypesByParent[index]);
          switch (triggerActivationType)
          {
            case TriggerActivationType.ViewPurchaseAdvice:
            case TriggerActivationType.SubmitPurchase:
              continue;
            default:
              this.lvwLoanActionList.Items.Add(new GVItem(activationTypesByParent[index])
              {
                Tag = (object) Convert.ToString((object) triggerActivationType)
              });
              continue;
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(AddLoanActionsForPersonaAccess.sw, TraceLevel.Error, nameof (AddLoanActionsForPersonaAccess), "initForm: Can't load accessible forms. Error: " + ex.Message);
      }
      this.lvwLoanActionList.EndUpdate();
    }

    public List<string> SelectedLoanActions => this.selectedLoanActions;

    private void dialogButtons1_OK(object sender, EventArgs e)
    {
      if (this.lvwLoanActionList.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You have to select a loan action first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.selectedLoanActions = new List<string>();
        for (int index = 0; index < this.lvwLoanActionList.SelectedItems.Count; ++index)
          this.selectedLoanActions.Add(this.lvwLoanActionList.SelectedItems[index].Text.ToString());
        this.DialogResult = DialogResult.OK;
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      GVColumn gvColumn = new GVColumn();
      this.dialogButtons1 = new DialogButtons();
      this.panelForListView = new Panel();
      this.lvwLoanActionList = new GridView();
      this.panelForListView.SuspendLayout();
      this.SuspendLayout();
      this.dialogButtons1.Dock = DockStyle.Bottom;
      this.dialogButtons1.Location = new Point(0, 455);
      this.dialogButtons1.Name = "dialogButtons1";
      this.dialogButtons1.Size = new Size(434, 44);
      this.dialogButtons1.TabIndex = 0;
      this.dialogButtons1.OK += new EventHandler(this.dialogButtons1_OK);
      this.panelForListView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.panelForListView.Controls.Add((Control) this.lvwLoanActionList);
      this.panelForListView.Location = new Point(13, 12);
      this.panelForListView.Name = "panelForListView";
      this.panelForListView.Size = new Size(409, 411);
      this.panelForListView.TabIndex = 5;
      gvColumn.ImageIndex = -1;
      gvColumn.Name = "loanActionName";
      gvColumn.Text = "Loan Action";
      gvColumn.Width = 400;
      this.lvwLoanActionList.Columns.AddRange(new GVColumn[1]
      {
        gvColumn
      });
      this.lvwLoanActionList.Dock = DockStyle.Fill;
      this.lvwLoanActionList.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.lvwLoanActionList.Location = new Point(0, 0);
      this.lvwLoanActionList.Name = "lvwLoanActionList";
      this.lvwLoanActionList.Size = new Size(409, 411);
      this.lvwLoanActionList.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(434, 499);
      this.Controls.Add((Control) this.panelForListView);
      this.Controls.Add((Control) this.dialogButtons1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MinimizeBox = false;
      this.Name = nameof (AddLoanActionsForPersonaAccess);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Add Loan Actions";
      this.panelForListView.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
