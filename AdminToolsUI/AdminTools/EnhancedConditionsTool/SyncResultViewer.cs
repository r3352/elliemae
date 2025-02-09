// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.EnhancedConditionsTool.SyncResultViewer
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using EllieMae.EMLite.AdminTools.Properties;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.AdminTools.EnhancedConditionsTool
{
  public class SyncResultViewer : UserControl
  {
    protected const int ImportStatusColumn = 6;
    protected const int SyncErrorColumn = 7;
    private const string SyncResult_Success = "Imported";
    private const string SyncResult_Errors = "Failed";
    private IContainer components;
    private Label lblSummary;
    private Label lblTitle;
    private TableLayoutPanel ResultLayout;
    private GridView gvSummary;
    private TextBox txtGeneralErrors;

    public SyncResult Result { get; private set; }

    public SyncResultViewer() => this.InitializeComponent();

    protected MainForm.SourceMode SourceMode { get; set; }

    protected string[] UiText { get; set; }

    public void Init(
      MainForm.SourceMode sourceMode,
      ExportablePackage source,
      SyncResult result,
      Sessions.Session session)
    {
      this.SourceMode = sourceMode;
      this.Result = result;
      string[] strArray;
      if (this.SourceMode == MainForm.SourceMode.Convert)
        strArray = new string[4]
        {
          "conversion",
          "converted",
          "Conversion",
          "Converted"
        };
      else
        strArray = new string[4]
        {
          "import",
          "imported",
          "Import",
          "Imported"
        };
      this.UiText = strArray;
      this.lblTitle.Text = this.UiText[2] + " results";
      this.gvSummary.Columns[6].Text = this.UiText[2] + " Status";
      Label lblSummary = this.lblSummary;
      string str1 = result.ImportCount > 0 ? string.Format("Successfully {0} {1} {2}.", (object) this.UiText[1], (object) result.ImportCount, (object) this.Pluralize(result.ImportCount, "record")) : "No records " + this.UiText[1] + ".";
      string newLine = Environment.NewLine;
      List<SyncError> errors = result.Errors;
      // ISSUE: explicit non-virtual call
      string str2 = (errors != null ? (__nonvirtual (errors.Count) > 0 ? 1 : 0) : 0) != 0 ? "Errors encountered during " + this.UiText[0] + ":" : "No errors.";
      string str3 = str1 + newLine + str2;
      lblSummary.Text = str3;
      List<string> list = result.Errors.Where<SyncError>((Func<SyncError, bool>) (e => e.Template == null)).Select<SyncError, string>((Func<SyncError, string>) (e => e.Message)).ToList<string>();
      this.txtGeneralErrors.Text = string.Join(Environment.NewLine, (IEnumerable<string>) list);
      this.txtGeneralErrors.Visible = list.Any<string>();
      this.txtGeneralErrors.ScrollBars = list.Count > 1 ? ScrollBars.Vertical : ScrollBars.None;
      this.ResizeTextBox(this.txtGeneralErrors);
      GridViewDataManager gridViewDataManager = new GridViewDataManager(session, this.gvSummary, (LoanDataMgr) null);
      gridViewDataManager.ClearItems();
      foreach (EnhancedConditionTemplate template in source.Templates)
        gridViewDataManager.AddItem(template);
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvSummary.Items)
        this.SetItemErrorMessage(gvItem, (IList<SyncError>) result.Errors);
      if (this.SourceMode != MainForm.SourceMode.Convert)
        return;
      new List<int>() { 0, 250, 250, 100, 0, 0 }.Select<int, int>((Func<int, int, int>) ((width, i) => this.gvSummary.Columns[i].Width = width)).ToList<int>();
    }

    private string Pluralize(int count, string singular, string plural = null)
    {
      return (count == 1 ? singular : (string.IsNullOrEmpty(plural) ? singular + "s" : plural)) ?? "";
    }

    private void ResizeTextBox(TextBox textBox, int maxLinesShown = 5)
    {
      int num1 = 3;
      int num2 = textBox.Height - textBox.ClientSize.Height;
      int num3 = Math.Min(maxLinesShown, textBox.GetLineFromCharIndex(textBox.TextLength) + 1);
      textBox.Height = Math.Min(textBox.MaximumSize.Height, textBox.Font.Height * num3 + num1 + num2);
    }

    private void SetItemErrorMessage(GVItem gvItem, IList<SyncError> errors)
    {
      EnhancedConditionTemplate template = gvItem.Tag as EnhancedConditionTemplate;
      IEnumerable<string> source = errors.Where<SyncError>((Func<SyncError, bool>) (err => template.Matches(err.Template))).Select<SyncError, string>((Func<SyncError, string>) (e => e.Message));
      int num = source.Any<string>() ? 1 : 0;
      string text = num != 0 ? "Failed" : this.UiText[3];
      string str = string.Join("\n", source.Where<string>((Func<string, bool>) (t => !string.IsNullOrEmpty(t))));
      MultiValueElement multiValueElement = new MultiValueElement((IEnumerable<Element>) source.Select<string, TextElement>((Func<string, TextElement>) (e => new TextElement(e))).ToArray<TextElement>());
      Bitmap bitmap = num != 0 ? Resources.audit_red_icon : Resources.audit_green_icon;
      gvItem.SubItems[6].Value = (object) new ObjectWithImage(text, (Image) bitmap);
      gvItem.SubItems[7].Text = str;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      GVColumn gvColumn7 = new GVColumn();
      GVColumn gvColumn8 = new GVColumn();
      this.lblSummary = new Label();
      this.lblTitle = new Label();
      this.ResultLayout = new TableLayoutPanel();
      this.txtGeneralErrors = new TextBox();
      this.gvSummary = new GridView();
      this.ResultLayout.SuspendLayout();
      this.SuspendLayout();
      this.lblSummary.AutoSize = true;
      this.lblSummary.Location = new Point(3, 36);
      this.lblSummary.Name = "lblSummary";
      this.lblSummary.Size = new Size(237, 20);
      this.lblSummary.TabIndex = 0;
      this.lblSummary.Text = "Successfully imported 0 records.";
      this.lblTitle.AutoSize = true;
      this.lblTitle.Location = new Point(3, 0);
      this.lblTitle.Name = "lblTitle";
      this.lblTitle.Size = new Size(173, 20);
      this.lblTitle.TabIndex = 1;
      this.lblTitle.Text = "Synchronization results";
      this.ResultLayout.ColumnCount = 1;
      this.ResultLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
      this.ResultLayout.Controls.Add((Control) this.txtGeneralErrors, 0, 2);
      this.ResultLayout.Controls.Add((Control) this.gvSummary, 0, 3);
      this.ResultLayout.Controls.Add((Control) this.lblSummary, 0, 1);
      this.ResultLayout.Controls.Add((Control) this.lblTitle, 0, 0);
      this.ResultLayout.Dock = DockStyle.Fill;
      this.ResultLayout.Location = new Point(0, 0);
      this.ResultLayout.Name = "ResultLayout";
      this.ResultLayout.RowCount = 4;
      this.ResultLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36f));
      this.ResultLayout.RowStyles.Add(new RowStyle());
      this.ResultLayout.RowStyles.Add(new RowStyle());
      this.ResultLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
      this.ResultLayout.Size = new Size(1407, 252);
      this.ResultLayout.TabIndex = 3;
      this.txtGeneralErrors.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtGeneralErrors.BackColor = SystemColors.Window;
      this.txtGeneralErrors.Cursor = Cursors.Arrow;
      this.txtGeneralErrors.Location = new Point(3, 59);
      this.txtGeneralErrors.MaximumSize = new Size(1407, 192);
      this.txtGeneralErrors.Multiline = true;
      this.txtGeneralErrors.Name = "txtGeneralErrors";
      this.txtGeneralErrors.ReadOnly = true;
      this.txtGeneralErrors.ScrollBars = ScrollBars.Vertical;
      this.txtGeneralErrors.Size = new Size(1401, 96);
      this.txtGeneralErrors.TabIndex = 4;
      this.gvSummary.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gvSummary.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "InternalId";
      gvColumn1.Tag = (object) "InternalId";
      gvColumn1.Text = "Internal ID";
      gvColumn1.Width = 100;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Name";
      gvColumn2.Tag = (object) "NAME";
      gvColumn2.Text = "Name";
      gvColumn2.Width = 100;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "InternalDescription";
      gvColumn3.Tag = (object) "INTERNALDESCRIPTION";
      gvColumn3.Text = "Internal Description";
      gvColumn3.Width = 100;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Type";
      gvColumn4.Tag = (object) "TYPE";
      gvColumn4.Text = "Type";
      gvColumn4.Width = 100;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Category";
      gvColumn5.Tag = (object) "CATEGORY";
      gvColumn5.Text = "Category";
      gvColumn5.Width = 100;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Customized";
      gvColumn6.Tag = (object) "CUSTOMIZED";
      gvColumn6.Text = "Customized";
      gvColumn6.Width = 100;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "ImportStatus";
      gvColumn7.Tag = (object) "IMPORTSTATUS";
      gvColumn7.Text = "Import Status";
      gvColumn7.Width = 100;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "SyncErrors";
      gvColumn8.Tag = (object) "SYNCERRORS";
      gvColumn8.Text = "Errors";
      gvColumn8.Width = 108;
      this.gvSummary.Columns.AddRange(new GVColumn[8]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7,
        gvColumn8
      });
      this.gvSummary.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvSummary.Location = new Point(3, 161);
      this.gvSummary.Name = "gvSummary";
      this.gvSummary.Selectable = false;
      this.gvSummary.Size = new Size(1401, 88);
      this.gvSummary.TabIndex = 2;
      this.gvSummary.TextTrimming = StringTrimming.EllipsisCharacter;
      this.AutoScaleDimensions = new SizeF(9f, 20f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.ResultLayout);
      this.Name = nameof (SyncResultViewer);
      this.Size = new Size(1407, 252);
      this.ResultLayout.ResumeLayout(false);
      this.ResultLayout.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
