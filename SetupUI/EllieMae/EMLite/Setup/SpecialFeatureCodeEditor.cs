// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.SpecialFeatureCodeEditor
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public sealed class SpecialFeatureCodeEditor : Form
  {
    private Dictionary<string, Regex> ValidCharacters = new Dictionary<string, Regex>()
    {
      {
        "Fannie Mae",
        new Regex("^[0-9]{3}$", RegexOptions.IgnoreCase | RegexOptions.Compiled)
      },
      {
        "Freddie Mac",
        new Regex("^[0-9A-Z]{3}$", RegexOptions.IgnoreCase | RegexOptions.Compiled)
      }
    };
    private IContainer components;
    private TableLayoutPanel tableLayoutPanel1;
    private TextBox txtDescription;
    private Label lblCode;
    private Label lblDescription;
    private Label lblComment;
    private Label lblSource;
    private TextBox txtCode;
    private TextBox txtComment;
    private FlowLayoutPanel flowLayoutPanel1;
    private Button btnOK;
    private Button btnCancel;
    private ComboBox cmbSource;
    private Label lblDivider;

    public bool IsNewCode { get; set; }

    public SpecialFeatureCodeDefinition SpecialFeatureCode { get; set; }

    private ISpecialFeatureCodeManager Service { get; set; }

    private SpecialFeatureCodeEditor.ChangeTracker Tracker { get; set; }

    public SpecialFeatureCodeEditor(
      ISpecialFeatureCodeManager service,
      SpecialFeatureCodeDefinition sfc = null)
    {
      this.InitializeComponent();
      this.Service = service;
      this.SpecialFeatureCode = sfc;
      this.IsNewCode = sfc == null;
      this.Tracker = new SpecialFeatureCodeEditor.ChangeTracker(sfc);
      this.txtCode.Enabled = this.cmbSource.Enabled = true;
      this.Text = string.Format("{0} Special Feature Code", this.IsNewCode ? (object) "Add" : (object) "Edit");
      this.txtCode.Text = sfc?.Code;
      this.cmbSource.Text = sfc?.Source;
      this.txtDescription.Text = sfc?.Description;
      this.txtComment.Text = sfc?.Comment;
      this.ActiveControl = this.IsNewCode ? (Control) this.txtCode : (Control) this.txtDescription;
    }

    private void btnCancel_Click(object sender, EventArgs e) => this.HandleExit(true);

    private void btnOK_Click(object sender, EventArgs e) => this.HandleExit(false);

    private void HandleExit(bool cancelEdit)
    {
      this.DialogResult = DialogResult.None;
      SpecialFeatureCodeDefinition currentFields = this.GetCurrentFields();
      if (cancelEdit && this.VerifyCancel(currentFields) || this.Service.IsUsedinFieldTriggerRule(currentFields.ID) && DialogResult.Cancel == Utils.Dialog((IWin32Window) this, "The original Special Feature Code and Source is currently referenced in a field trigger business rule.  Any corresponding field trigger business rules must be updated if edited.\nWould you like to continue saving edits?", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) || !this.Validate(currentFields) || this.CancelDeactivate(currentFields) || !this.TrySave(currentFields))
        return;
      this.CloseEditor(DialogResult.OK);
    }

    private SpecialFeatureCodeDefinition GetCurrentFields()
    {
      SpecialFeatureCodeDefinition currentFields = new SpecialFeatureCodeDefinition(this.txtCode.Text.Trim(), this.cmbSource.Text.Trim(), this.txtDescription.Text.Trim(), this.txtComment.Text.Trim());
      if (this.SpecialFeatureCode != null)
        currentFields.ID = this.SpecialFeatureCode.ID;
      return currentFields;
    }

    private bool VerifyCancel(SpecialFeatureCodeDefinition currentSfc)
    {
      bool flag = this.Tracker.IsDifferentFrom(currentSfc);
      if (flag)
      {
        DialogResult dialogResult = Utils.Dialog((IWin32Window) this, "There are unsaved changes\nDo you wish to save before exiting?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
        if (DialogResult.Cancel == dialogResult)
          return true;
        if (DialogResult.No == dialogResult)
          flag = false;
      }
      if (flag)
        return false;
      this.CloseEditor(DialogResult.Cancel);
      return true;
    }

    private bool Validate(SpecialFeatureCodeDefinition sfc)
    {
      if (string.IsNullOrEmpty(sfc.Code) || string.IsNullOrEmpty(sfc.Source))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Code and Source are required fields", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.FocusAndSelect(string.IsNullOrEmpty(sfc.Code) ? (Control) this.txtCode : (Control) this.cmbSource);
        return false;
      }
      Regex regex;
      if (!this.ValidCharacters.TryGetValue(sfc.Source, out regex) || regex.IsMatch(sfc.Code))
        return true;
      int num1 = (int) Utils.Dialog((IWin32Window) this, "Code for " + sfc.Source + " must be exactly three " + (sfc.Source == "Freddie Mac" ? "letters and/or " : "") + "digits, including any leading zeros.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      this.FocusAndSelect((Control) this.txtCode);
      return false;
    }

    private bool CancelDeactivate(SpecialFeatureCodeDefinition currentSfc)
    {
      if (this.IsNewCode || !this.SpecialFeatureCode.IsActive || !(currentSfc.Code != this.SpecialFeatureCode.Code) && !(currentSfc.Source != this.SpecialFeatureCode.Source))
        return false;
      IList<SpecialFeatureCodeDefinition> list = this.FetchList();
      return list == null || list.IsOtherActiveCodeSource(currentSfc) && (DialogResult.OK != Utils.Dialog((IWin32Window) this, "An existing entry matching the selected code and source is currently active.  This entry will be marked inactive if saved.  Continue saving?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) || !this.TryDeactivate(currentSfc));
    }

    private IList<SpecialFeatureCodeDefinition> FetchList()
    {
      IList<SpecialFeatureCodeDefinition> featureCodeDefinitionList = (IList<SpecialFeatureCodeDefinition>) null;
      try
      {
        featureCodeDefinitionList = this.Service.GetAll();
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Error while fetching special feature codes list\nThe error was: '" + ex.Message + "'", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      return featureCodeDefinitionList;
    }

    private bool TryDeactivate(SpecialFeatureCodeDefinition currentSfc)
    {
      try
      {
        this.Service.Deactivate(this.SpecialFeatureCode);
        return true;
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Error while deactivating\nThe error was: '" + ex.Message + "'", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      return false;
    }

    private void FocusAndSelect(Control control)
    {
      control.Focus();
      if (!(control is TextBoxBase textBoxBase))
        return;
      textBoxBase.SelectAll();
    }

    private void CloseEditor(DialogResult result)
    {
      this.DialogResult = result;
      this.Close();
    }

    private bool TrySave(SpecialFeatureCodeDefinition sfc)
    {
      bool flag = false;
      try
      {
        if (this.IsNewCode)
          this.Service.Add(sfc);
        else
          this.Service.Update(sfc);
        this.SpecialFeatureCode = sfc;
        flag = true;
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Error while saving\nThe error was: '" + ex.Message + "'", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      return flag;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.tableLayoutPanel1 = new TableLayoutPanel();
      this.lblDivider = new Label();
      this.txtComment = new TextBox();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.txtDescription = new TextBox();
      this.lblCode = new Label();
      this.lblComment = new Label();
      this.txtCode = new TextBox();
      this.cmbSource = new ComboBox();
      this.lblSource = new Label();
      this.lblDescription = new Label();
      this.tableLayoutPanel1.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      this.tableLayoutPanel1.AutoSize = true;
      this.tableLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.tableLayoutPanel1.ColumnCount = 4;
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 73f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 73f));
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
      this.tableLayoutPanel1.Controls.Add((Control) this.lblDivider, 0, 3);
      this.tableLayoutPanel1.Controls.Add((Control) this.txtComment, 1, 2);
      this.tableLayoutPanel1.Controls.Add((Control) this.flowLayoutPanel1, 0, 5);
      this.tableLayoutPanel1.Controls.Add((Control) this.txtDescription, 1, 1);
      this.tableLayoutPanel1.Controls.Add((Control) this.lblCode, 0, 0);
      this.tableLayoutPanel1.Controls.Add((Control) this.lblComment, 0, 2);
      this.tableLayoutPanel1.Controls.Add((Control) this.txtCode, 1, 0);
      this.tableLayoutPanel1.Controls.Add((Control) this.cmbSource, 3, 0);
      this.tableLayoutPanel1.Controls.Add((Control) this.lblSource, 2, 0);
      this.tableLayoutPanel1.Controls.Add((Control) this.lblDescription, 0, 1);
      this.tableLayoutPanel1.Location = new Point(0, 0);
      this.tableLayoutPanel1.Margin = new Padding(0);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.Padding = new Padding(7, 7, 7, 7);
      this.tableLayoutPanel1.RowCount = 6;
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 11f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
      this.tableLayoutPanel1.Size = new Size(404, 191);
      this.tableLayoutPanel1.TabIndex = 100;
      this.lblDivider.Anchor = AnchorStyles.Left | AnchorStyles.Right;
      this.lblDivider.BorderStyle = BorderStyle.Fixed3D;
      this.tableLayoutPanel1.SetColumnSpan((Control) this.lblDivider, 4);
      this.lblDivider.Location = new Point(9, 132);
      this.lblDivider.Margin = new Padding(2, 10, 2, 10);
      this.lblDivider.Name = "lblDivider";
      this.lblDivider.Size = new Size(386, 2);
      this.lblDivider.TabIndex = 100;
      this.txtComment.AcceptsReturn = true;
      this.tableLayoutPanel1.SetColumnSpan((Control) this.txtComment, 3);
      this.txtComment.Dock = DockStyle.Fill;
      this.txtComment.Location = new Point(82, 82);
      this.txtComment.Margin = new Padding(2, 4, 2, 2);
      this.txtComment.MaxLength = (int) byte.MaxValue;
      this.txtComment.MinimumSize = new Size(4, 38);
      this.txtComment.Multiline = true;
      this.txtComment.Name = "txtComment";
      this.txtComment.ScrollBars = ScrollBars.Vertical;
      this.txtComment.Size = new Size(313, 38);
      this.txtComment.TabIndex = 4;
      this.flowLayoutPanel1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.flowLayoutPanel1.AutoSize = true;
      this.flowLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.tableLayoutPanel1.SetColumnSpan((Control) this.flowLayoutPanel1, 4);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnOK);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnCancel);
      this.flowLayoutPanel1.Location = new Point(236, 157);
      this.flowLayoutPanel1.Margin = new Padding(2, 2, 2, 2);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(159, 25);
      this.flowLayoutPanel1.TabIndex = 100;
      this.btnOK.Location = new Point(2, 2);
      this.btnOK.Margin = new Padding(2, 2, 2, 2);
      this.btnOK.MinimumSize = new Size(73, 21);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(73, 21);
      this.btnOK.TabIndex = 5;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(84, 2);
      this.btnCancel.Margin = new Padding(7, 2, 2, 2);
      this.btnCancel.MinimumSize = new Size(73, 21);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(73, 21);
      this.btnCancel.TabIndex = 6;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.txtDescription.AcceptsReturn = true;
      this.tableLayoutPanel1.SetColumnSpan((Control) this.txtDescription, 3);
      this.txtDescription.Dock = DockStyle.Fill;
      this.txtDescription.Location = new Point(82, 38);
      this.txtDescription.Margin = new Padding(2, 4, 2, 2);
      this.txtDescription.MaxLength = (int) byte.MaxValue;
      this.txtDescription.MinimumSize = new Size(4, 38);
      this.txtDescription.Multiline = true;
      this.txtDescription.Name = "txtDescription";
      this.txtDescription.ScrollBars = ScrollBars.Vertical;
      this.txtDescription.Size = new Size(313, 38);
      this.txtDescription.TabIndex = 3;
      this.lblCode.AutoSize = true;
      this.lblCode.Location = new Point(9, 13);
      this.lblCode.Margin = new Padding(2, 6, 2, 2);
      this.lblCode.Name = "lblCode";
      this.lblCode.Size = new Size(32, 13);
      this.lblCode.TabIndex = 100;
      this.lblCode.Text = "Code";
      this.lblComment.AutoSize = true;
      this.lblComment.Location = new Point(9, 84);
      this.lblComment.Margin = new Padding(2, 6, 2, 2);
      this.lblComment.Name = "lblComment";
      this.lblComment.Size = new Size(56, 13);
      this.lblComment.TabIndex = 100;
      this.lblComment.Text = "Comments";
      this.txtCode.Dock = DockStyle.Fill;
      this.txtCode.Location = new Point(82, 11);
      this.txtCode.Margin = new Padding(2, 4, 2, 2);
      this.txtCode.MaxLength = 3;
      this.txtCode.MinimumSize = new Size(4, 32);
      this.txtCode.Name = "txtCode";
      this.txtCode.Size = new Size(118, 20);
      this.txtCode.TabIndex = 0;
      this.cmbSource.Dock = DockStyle.Fill;
      this.cmbSource.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbSource.FormattingEnabled = true;
      this.cmbSource.Items.AddRange(new object[2]
      {
        (object) "Fannie Mae",
        (object) "Freddie Mac"
      });
      this.cmbSource.Location = new Point(277, 11);
      this.cmbSource.Margin = new Padding(2, 4, 2, 2);
      this.cmbSource.MaxLength = 64;
      this.cmbSource.Name = "cmbSource";
      this.cmbSource.Size = new Size(118, 21);
      this.cmbSource.TabIndex = 1;
      this.lblSource.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lblSource.AutoSize = true;
      this.lblSource.Location = new Point(232, 13);
      this.lblSource.Margin = new Padding(2, 6, 2, 2);
      this.lblSource.Name = "lblSource";
      this.lblSource.Size = new Size(41, 13);
      this.lblSource.TabIndex = 100;
      this.lblSource.Text = "Source";
      this.lblDescription.AutoSize = true;
      this.lblDescription.Location = new Point(9, 40);
      this.lblDescription.Margin = new Padding(2, 6, 2, 2);
      this.lblDescription.Name = "lblDescription";
      this.lblDescription.Size = new Size(60, 13);
      this.lblDescription.TabIndex = 100;
      this.lblDescription.Text = "Description";
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(96f, 96f);
      this.AutoScaleMode = AutoScaleMode.Dpi;
      this.AutoSize = true;
      this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(402, 183);
      this.Controls.Add((Control) this.tableLayoutPanel1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Margin = new Padding(2, 2, 2, 2);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SpecialFeatureCodeEditor);
      this.ShowIcon = false;
      this.SizeGripStyle = SizeGripStyle.Hide;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Special Feature Code Editor";
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      this.flowLayoutPanel1.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private class ChangeTracker
    {
      private IList<int> capturedHashCodes;

      public ChangeTracker(SpecialFeatureCodeDefinition sfc)
      {
        this.capturedHashCodes = (IList<int>) this.GetHashCodes(sfc).ToList<int>();
      }

      public bool IsDifferentFrom(SpecialFeatureCodeDefinition sfc)
      {
        return this.GetHashCodes(sfc).Where<int>((Func<int, int, bool>) ((v, i) => v != this.capturedHashCodes[i])).Any<int>();
      }

      private IEnumerable<int> GetHashCodes(SpecialFeatureCodeDefinition sfc)
      {
        return ((IEnumerable<string>) new string[4]
        {
          sfc?.Code,
          sfc?.Source,
          sfc?.Description,
          sfc?.Comment
        }).Select<string, int>((Func<string, int>) (s => (s ?? "").GetHashCode()));
      }
    }
  }
}
