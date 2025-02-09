// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AddDocsForLoanActionCompletion
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class AddDocsForLoanActionCompletion : AddFormBaseDialog
  {
    private const string className = "AddDocsForLoanActionCompletion";
    private static readonly string sw = Tracing.SwOutsideLoan;
    private Hashtable existDocs;
    private LoanActionCompletionRulePanel container;
    private DocLoanActionPair[] docsForRule;

    public AddDocsForLoanActionCompletion(
      Sessions.Session session,
      string[] loanActionList,
      Hashtable existDocs,
      LoanActionCompletionRulePanel container)
      : base(session, true)
    {
      this.container = container;
      this.existDocs = existDocs;
      this.Text = "Add Required Documents";
      this.radioAll.Text = "Add from All Documents";
      this.radioTemplate.Visible = false;
      this.radioAll.Checked = true;
      this.ifsExplorer = new TemplateIFSExplorer(this.session, EllieMae.EMLite.ClientServer.TemplateSettingsType.DocumentSet);
      this.fsExplorerTemplate.FileType = FSExplorer.FileTypes.DocumentSets;
      this.fsExplorerTemplate.HideAllButtons = true;
      this.fsExplorerTemplate.SingleSelection = true;
      this.fsExplorerTemplate.SetProperties(false, false, true, 5, true);
      this.label1.Text = "For Loan Action";
      this.cboRuleValues.Items.AddRange((object[]) loanActionList);
      this.cboRuleValues.SelectedIndex = 0;
      this.initForm();
      this.gridViewTemplate_SizeChanged((object) null, (EventArgs) null);
    }

    public DocLoanActionPair[] DocsForRule => this.docsForRule;

    public string SelectedLoanAction => this.cboRuleValues.Text;

    public bool NeedAttachment => this.chkAttached.Checked;

    private void initForm()
    {
      this.gridViewAll.Items.Clear();
      DocumentTrackingSetup documentTrackingSetup = this.session.ConfigurationManager.GetDocumentTrackingSetup();
      this.gridViewAll.BeginUpdate();
      try
      {
        if (Tracing.IsSwitchActive(AddDocsForLoanActionCompletion.sw, TraceLevel.Verbose))
          Tracing.Log(AddDocsForLoanActionCompletion.sw, TraceLevel.Verbose, nameof (AddDocsForLoanActionCompletion), "initForm: Loading all document tracking...");
        foreach (DocumentTemplate documentTemplate in documentTrackingSetup.dictDocTrackByName.Values)
        {
          if (!this.existDocs.ContainsKey((object) documentTemplate.Name.ToLower()))
            this.gridViewAll.Items.Add(new GVItem(documentTemplate.Name)
            {
              Tag = (object) documentTemplate
            });
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(AddDocsForLoanActionCompletion.sw, TraceLevel.Error, nameof (AddDocsForLoanActionCompletion), "initForm: Can't load accessible forms. Error: " + ex.Message);
      }
      this.gridViewAll.EndUpdate();
      this.initTemplates("DocumentSetTemplate");
    }

    protected override void okBtn_Click(object sender, EventArgs e)
    {
      if (this.radioAll.Checked)
      {
        if (this.gridViewAll.SelectedItems.Count == 0)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "You have to select a document first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
        this.docsForRule = new DocLoanActionPair[this.gridViewAll.SelectedItems.Count];
        TriggerActivationType triggerActivationType = (TriggerActivationType) new TriggerActivationNameProvider().GetValue(this.cboRuleValues.Text);
        for (int index = 0; index < this.gridViewAll.SelectedItems.Count; ++index)
        {
          DocumentTemplate tag = (DocumentTemplate) this.gridViewAll.SelectedItems[index].Tag;
          this.docsForRule[index] = !(this.cboRuleValues.Text != string.Empty) ? new DocLoanActionPair(tag.Guid, "FieldModified", this.chkAttached.Checked) : new DocLoanActionPair(tag.Guid, triggerActivationType.ToString(), this.chkAttached.Checked);
        }
      }
      this.DialogResult = DialogResult.OK;
    }

    protected override void fsExplorerTemplate_SelectedEntryChanged(object sender, EventArgs e)
    {
    }
  }
}
