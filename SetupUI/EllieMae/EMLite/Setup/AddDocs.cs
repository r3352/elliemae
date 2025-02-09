// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AddDocs
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
  public class AddDocs : AddFormBaseDialog
  {
    private const string className = "AddDocs";
    private static readonly string sw = Tracing.SwOutsideLoan;
    private Hashtable existDocs;
    private ConditionRulePanel container;
    private DocMilestonePair[] docsForRule;

    public AddDocs(
      Sessions.Session session,
      string[] milestoneList,
      Hashtable existDocs,
      ConditionRulePanel container)
      : base(session, true)
    {
      this.container = container;
      this.existDocs = existDocs;
      this.Text = "Add Required Documents";
      this.radioAll.Text = "Add from All Documents";
      this.radioTemplate.Text = "Add from Document Set Templates";
      this.radioAll.Checked = true;
      this.ifsExplorer = new TemplateIFSExplorer(this.session, EllieMae.EMLite.ClientServer.TemplateSettingsType.DocumentSet);
      this.fsExplorerTemplate.FileType = FSExplorer.FileTypes.DocumentSets;
      this.fsExplorerTemplate.HideAllButtons = true;
      this.fsExplorerTemplate.SingleSelection = true;
      this.fsExplorerTemplate.SetProperties(false, false, true, 5, true);
      this.cboRuleValues.Items.AddRange((object[]) milestoneList);
      this.initForm();
      this.gridViewTemplate_SizeChanged((object) null, (EventArgs) null);
    }

    public DocMilestonePair[] DocsForRule => this.docsForRule;

    public string SelectedMilestone => this.cboRuleValues.Text;

    public bool NeedAttachment => this.chkAttached.Checked;

    private void initForm()
    {
      this.gridViewAll.Items.Clear();
      DocumentTrackingSetup documentTrackingSetup = this.session.ConfigurationManager.GetDocumentTrackingSetup();
      this.gridViewAll.BeginUpdate();
      try
      {
        if (Tracing.IsSwitchActive(AddDocs.sw, TraceLevel.Verbose))
          Tracing.Log(AddDocs.sw, TraceLevel.Verbose, nameof (AddDocs), "initForm: Loading all document tracking...");
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
        Tracing.Log(AddDocs.sw, TraceLevel.Error, nameof (AddDocs), "initForm: Can't load accessible forms. Error: " + ex.Message);
      }
      this.gridViewAll.EndUpdate();
      this.initTemplates("DocumentSetTemplate");
    }

    protected override void okBtn_Click(object sender, EventArgs e)
    {
      if (this.radioTemplate.Checked)
      {
        if (this.gridViewTemplate.SelectedItems.Count == 0)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "You have to select an input form first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
        this.docsForRule = new DocMilestonePair[this.gridViewTemplate.SelectedItems.Count];
        for (int index = 0; index < this.gridViewTemplate.SelectedItems.Count; ++index)
          this.docsForRule[index] = new DocMilestonePair(this.gridViewTemplate.SelectedItems[index].Text, this.gridViewTemplate.SelectedItems[index].SubItems[1].Text, this.chkAttached.Checked);
      }
      else
      {
        if (this.gridViewAll.SelectedItems.Count == 0)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "You have to select an input form first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
        this.docsForRule = new DocMilestonePair[this.gridViewAll.SelectedItems.Count];
        for (int index = 0; index < this.gridViewAll.SelectedItems.Count; ++index)
        {
          DocumentTemplate tag = (DocumentTemplate) this.gridViewAll.SelectedItems[index].Tag;
          this.docsForRule[index] = !(this.cboRuleValues.Text != string.Empty) ? new DocMilestonePair(tag.Name, "Processing", this.chkAttached.Checked) : new DocMilestonePair(tag.Name, this.cboRuleValues.Text, this.chkAttached.Checked);
        }
      }
      this.DialogResult = DialogResult.OK;
    }

    protected override void fsExplorerTemplate_SelectedEntryChanged(object sender, EventArgs e)
    {
      base.fsExplorerTemplate_SelectedEntryChanged(sender, e);
      FileSystemEntry tag;
      try
      {
        tag = (FileSystemEntry) this.fsExplorerTemplate.SelectedItems[0].Tag;
        if (tag.Type == FileSystemEntry.Types.Folder)
          return;
      }
      catch (Exception ex)
      {
        Tracing.Log(AddDocs.sw, TraceLevel.Error, nameof (AddDocs), "fsExplorerTemplate_SelectedEntryChanged: Can't access document template. Error: " + ex.Message);
        return;
      }
      if (!this.session.ConfigurationManager.TemplateSettingsObjectExists(EllieMae.EMLite.ClientServer.TemplateSettingsType.DocumentSet, tag))
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "The document template '" + tag.Name + "' can't be accessed.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        DocumentSetTemplate documentSetTemplate = (DocumentSetTemplate) null;
        try
        {
          documentSetTemplate = (DocumentSetTemplate) this.session.ConfigurationManager.GetTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.DocumentSet, tag);
        }
        catch (Exception ex)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "Can't open " + tag.Name + " document template file. Message: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        for (int index = 0; index < this.container.MilestoneList.Length; ++index)
        {
          string milestone = this.container.MilestoneList[index];
          ArrayList documentsByMilestone = documentSetTemplate.GetDocumentsByMilestone(this.container.MilestoneList[index]);
          if (documentsByMilestone == null)
            return;
          foreach (string text in documentsByMilestone)
            this.gridViewTemplate.Items.Add(new GVItem(text)
            {
              SubItems = {
                (object) this.container.MilestoneList[index]
              }
            });
        }
        this.fsExplorerTemplate.SetFocusToFileListView();
      }
    }
  }
}
