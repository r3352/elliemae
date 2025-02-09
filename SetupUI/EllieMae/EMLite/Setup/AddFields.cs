// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AddFields
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class AddFields : Form
  {
    private Label label1;
    private Button cancelBtn;
    private Button okBtn;
    private System.ComponentModel.Container components;
    private Label label2;
    private Label label3;
    private Label label4;
    private Label label5;
    private TextBox textBoxID1;
    private TextBox textBoxID2;
    private TextBox textBoxID3;
    private TextBox textBoxID4;
    private TextBox textBoxID5;
    private Label label6;
    private Label label7;
    private Label label8;
    private Label label9;
    private Label label10;
    private TextBox textBoxID10;
    private TextBox textBoxID9;
    private TextBox textBoxID8;
    private TextBox textBoxID7;
    private TextBox textBoxID6;
    private Button moreBtn;
    private FieldSettings fieldSettings;
    private List<string> fieldsNotAllowed;
    private GroupContainer groupContainer1;
    private Label lblComboText;
    private Panel panelMilestone;
    private ComboBox cboRuleValues;
    private Panel panelButtons;
    private Button importBtn;
    private AddFieldOptions options = AddFieldOptions.AllowAll;
    private ArrayList pickFieldIDs = new ArrayList();
    private string[] selectedFieldIDs;
    private ArrayList errorFields = new ArrayList();
    private ArrayList importedFieldIDs = new ArrayList();
    private AddFields.ImportableLoanXDBFieldTable importedFieldTable;
    private AddFields.ImportableLoanXDBFieldTable importedErrorFieldTable;
    private bool isAddedByImport;

    public event EventHandler OnAddMoreButtonClick;

    public event EventHandler OnImportButtonClick;

    public AddFields(Sessions.Session session)
      : this(session, (List<string>) null)
    {
    }

    public AddFields(Sessions.Session session, List<string> fieldsNotAllowed)
      : this(session, (string) null, AddFieldOptions.AllowAll, fieldsNotAllowed)
    {
    }

    public AddFields(Sessions.Session session, string title, AddFieldOptions options)
      : this(session, title, options, (List<string>) null)
    {
    }

    public AddFields(
      Sessions.Session session,
      string title,
      AddFieldOptions options,
      List<string> fieldsNotAllowed)
    {
      this.options = options;
      this.fieldsNotAllowed = fieldsNotAllowed;
      this.InitializeComponent();
      this.okBtn.Enabled = false;
      this.moreBtn.Enabled = false;
      this.importBtn.Visible = false;
      if ((title ?? "") != "")
        this.Text = title;
      this.fieldSettings = session.LoanManager.GetFieldSettings();
      if (this.panelMilestone.Visible)
        return;
      this.panelButtons.Top = this.panelMilestone.Top;
      this.Height = this.panelButtons.Top + this.panelButtons.Height * 2;
    }

    public AddFieldOptions Options
    {
      get => this.options;
      set => this.options = value;
    }

    public string SetComboLabelText
    {
      set
      {
        if (string.IsNullOrEmpty(value))
          return;
        this.lblComboText.Text = value;
      }
    }

    public void SetComboIndex(int indexNumber) => this.cboRuleValues.SelectedIndex = indexNumber;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    public bool ImportButtonEnabled
    {
      get => this.importBtn.Visible;
      set => this.importBtn.Visible = value;
    }

    public string[] SelectedFieldIDs => this.selectedFieldIDs;

    public ArrayList ErrorFields => this.errorFields;

    public ArrayList ImportedFieldIDs => this.importedFieldIDs;

    public AddFields.ImportableLoanXDBFieldTable ImportedFieldTable => this.importedFieldTable;

    public AddFields.ImportableLoanXDBFieldTable ImportedErrorFieldTable
    {
      get => this.importedErrorFieldTable;
    }

    public bool IsAddedByImport => this.isAddedByImport;

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.textBoxID1 = new TextBox();
      this.textBoxID10 = new TextBox();
      this.label6 = new Label();
      this.textBoxID9 = new TextBox();
      this.label7 = new Label();
      this.textBoxID8 = new TextBox();
      this.label8 = new Label();
      this.textBoxID7 = new TextBox();
      this.label9 = new Label();
      this.textBoxID6 = new TextBox();
      this.label10 = new Label();
      this.textBoxID5 = new TextBox();
      this.label5 = new Label();
      this.textBoxID4 = new TextBox();
      this.label4 = new Label();
      this.textBoxID3 = new TextBox();
      this.label3 = new Label();
      this.textBoxID2 = new TextBox();
      this.label2 = new Label();
      this.cancelBtn = new Button();
      this.okBtn = new Button();
      this.moreBtn = new Button();
      this.groupContainer1 = new GroupContainer();
      this.lblComboText = new Label();
      this.panelMilestone = new Panel();
      this.cboRuleValues = new ComboBox();
      this.panelButtons = new Panel();
      this.importBtn = new Button();
      this.groupContainer1.SuspendLayout();
      this.panelMilestone.SuspendLayout();
      this.panelButtons.SuspendLayout();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(8, 39);
      this.label1.Name = "label1";
      this.label1.Size = new Size(41, 14);
      this.label1.TabIndex = 0;
      this.label1.Text = "Field ID";
      this.textBoxID1.Location = new Point(56, 36);
      this.textBoxID1.Name = "textBoxID1";
      this.textBoxID1.Size = new Size(216, 20);
      this.textBoxID1.TabIndex = 1;
      this.textBoxID1.TextChanged += new EventHandler(this.textBoxID_TextChanged);
      this.textBoxID10.Location = new Point(56, 234);
      this.textBoxID10.Name = "textBoxID10";
      this.textBoxID10.Size = new Size(216, 20);
      this.textBoxID10.TabIndex = 15;
      this.textBoxID10.TextChanged += new EventHandler(this.textBoxID_TextChanged);
      this.label6.AutoSize = true;
      this.label6.Location = new Point(8, 237);
      this.label6.Name = "label6";
      this.label6.Size = new Size(41, 14);
      this.label6.TabIndex = 17;
      this.label6.Text = "Field ID";
      this.textBoxID9.Location = new Point(56, 212);
      this.textBoxID9.Name = "textBoxID9";
      this.textBoxID9.Size = new Size(216, 20);
      this.textBoxID9.TabIndex = 14;
      this.textBoxID9.TextChanged += new EventHandler(this.textBoxID_TextChanged);
      this.label7.AutoSize = true;
      this.label7.Location = new Point(8, 215);
      this.label7.Name = "label7";
      this.label7.Size = new Size(41, 14);
      this.label7.TabIndex = 16;
      this.label7.Text = "Field ID";
      this.textBoxID8.Location = new Point(56, 190);
      this.textBoxID8.Name = "textBoxID8";
      this.textBoxID8.Size = new Size(216, 20);
      this.textBoxID8.TabIndex = 13;
      this.textBoxID8.TextChanged += new EventHandler(this.textBoxID_TextChanged);
      this.label8.AutoSize = true;
      this.label8.Location = new Point(8, 193);
      this.label8.Name = "label8";
      this.label8.Size = new Size(41, 14);
      this.label8.TabIndex = 12;
      this.label8.Text = "Field ID";
      this.textBoxID7.Location = new Point(56, 168);
      this.textBoxID7.Name = "textBoxID7";
      this.textBoxID7.Size = new Size(216, 20);
      this.textBoxID7.TabIndex = 11;
      this.textBoxID7.TextChanged += new EventHandler(this.textBoxID_TextChanged);
      this.label9.AutoSize = true;
      this.label9.Location = new Point(8, 171);
      this.label9.Name = "label9";
      this.label9.Size = new Size(41, 14);
      this.label9.TabIndex = 9;
      this.label9.Text = "Field ID";
      this.textBoxID6.Location = new Point(56, 146);
      this.textBoxID6.Name = "textBoxID6";
      this.textBoxID6.Size = new Size(216, 20);
      this.textBoxID6.TabIndex = 10;
      this.textBoxID6.TextChanged += new EventHandler(this.textBoxID_TextChanged);
      this.label10.AutoSize = true;
      this.label10.Location = new Point(8, 149);
      this.label10.Name = "label10";
      this.label10.Size = new Size(41, 14);
      this.label10.TabIndex = 8;
      this.label10.Text = "Field ID";
      this.textBoxID5.Location = new Point(56, 124);
      this.textBoxID5.Name = "textBoxID5";
      this.textBoxID5.Size = new Size(216, 20);
      this.textBoxID5.TabIndex = 5;
      this.textBoxID5.TextChanged += new EventHandler(this.textBoxID_TextChanged);
      this.label5.AutoSize = true;
      this.label5.Location = new Point(8, (int) sbyte.MaxValue);
      this.label5.Name = "label5";
      this.label5.Size = new Size(41, 14);
      this.label5.TabIndex = 7;
      this.label5.Text = "Field ID";
      this.textBoxID4.Location = new Point(56, 102);
      this.textBoxID4.Name = "textBoxID4";
      this.textBoxID4.Size = new Size(216, 20);
      this.textBoxID4.TabIndex = 4;
      this.textBoxID4.TextChanged += new EventHandler(this.textBoxID_TextChanged);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(8, 105);
      this.label4.Name = "label4";
      this.label4.Size = new Size(41, 14);
      this.label4.TabIndex = 5;
      this.label4.Text = "Field ID";
      this.textBoxID3.Location = new Point(56, 80);
      this.textBoxID3.Name = "textBoxID3";
      this.textBoxID3.Size = new Size(216, 20);
      this.textBoxID3.TabIndex = 3;
      this.textBoxID3.TextChanged += new EventHandler(this.textBoxID_TextChanged);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(8, 83);
      this.label3.Name = "label3";
      this.label3.Size = new Size(41, 14);
      this.label3.TabIndex = 3;
      this.label3.Text = "Field ID";
      this.textBoxID2.Location = new Point(56, 58);
      this.textBoxID2.Name = "textBoxID2";
      this.textBoxID2.Size = new Size(216, 20);
      this.textBoxID2.TabIndex = 2;
      this.textBoxID2.TextChanged += new EventHandler(this.textBoxID_TextChanged);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(8, 61);
      this.label2.Name = "label2";
      this.label2.Size = new Size(41, 14);
      this.label2.TabIndex = 1;
      this.label2.Text = "Field ID";
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(218, 8);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(64, 22);
      this.cancelBtn.TabIndex = 4;
      this.cancelBtn.Text = "&Cancel";
      this.okBtn.Location = new Point(75, 8);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(64, 22);
      this.okBtn.TabIndex = 2;
      this.okBtn.Text = "&Add";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.moreBtn.Location = new Point(146, 8);
      this.moreBtn.Name = "moreBtn";
      this.moreBtn.Size = new Size(65, 22);
      this.moreBtn.TabIndex = 3;
      this.moreBtn.Text = "Add &More";
      this.moreBtn.Click += new EventHandler(this.moreBtn_Click);
      this.groupContainer1.Controls.Add((Control) this.textBoxID10);
      this.groupContainer1.Controls.Add((Control) this.label1);
      this.groupContainer1.Controls.Add((Control) this.label6);
      this.groupContainer1.Controls.Add((Control) this.textBoxID1);
      this.groupContainer1.Controls.Add((Control) this.textBoxID9);
      this.groupContainer1.Controls.Add((Control) this.label2);
      this.groupContainer1.Controls.Add((Control) this.label7);
      this.groupContainer1.Controls.Add((Control) this.textBoxID2);
      this.groupContainer1.Controls.Add((Control) this.textBoxID8);
      this.groupContainer1.Controls.Add((Control) this.label3);
      this.groupContainer1.Controls.Add((Control) this.label8);
      this.groupContainer1.Controls.Add((Control) this.textBoxID3);
      this.groupContainer1.Controls.Add((Control) this.textBoxID7);
      this.groupContainer1.Controls.Add((Control) this.label4);
      this.groupContainer1.Controls.Add((Control) this.label9);
      this.groupContainer1.Controls.Add((Control) this.textBoxID4);
      this.groupContainer1.Controls.Add((Control) this.textBoxID6);
      this.groupContainer1.Controls.Add((Control) this.label5);
      this.groupContainer1.Controls.Add((Control) this.label10);
      this.groupContainer1.Controls.Add((Control) this.textBoxID5);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(10, 10);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(286, 266);
      this.groupContainer1.TabIndex = 1;
      this.groupContainer1.Text = "Fields to Add";
      this.lblComboText.AutoSize = true;
      this.lblComboText.Location = new Point(8, 10);
      this.lblComboText.Name = "lblComboText";
      this.lblComboText.Size = new Size(71, 14);
      this.lblComboText.TabIndex = 5;
      this.lblComboText.Text = "For Milestone";
      this.panelMilestone.Controls.Add((Control) this.cboRuleValues);
      this.panelMilestone.Controls.Add((Control) this.lblComboText);
      this.panelMilestone.Location = new Point(10, 276);
      this.panelMilestone.Name = "panelMilestone";
      this.panelMilestone.Size = new Size(286, 34);
      this.panelMilestone.TabIndex = 6;
      this.panelMilestone.Visible = false;
      this.cboRuleValues.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboRuleValues.FormattingEnabled = true;
      this.cboRuleValues.Location = new Point(93, 6);
      this.cboRuleValues.Name = "cboMilestones";
      this.cboRuleValues.Size = new Size(186, 22);
      this.cboRuleValues.TabIndex = 6;
      this.panelButtons.Controls.Add((Control) this.importBtn);
      this.panelButtons.Controls.Add((Control) this.okBtn);
      this.panelButtons.Controls.Add((Control) this.cancelBtn);
      this.panelButtons.Controls.Add((Control) this.moreBtn);
      this.panelButtons.Location = new Point(10, 308);
      this.panelButtons.Name = "panelButtons";
      this.panelButtons.Size = new Size(286, 37);
      this.panelButtons.TabIndex = 7;
      this.importBtn.Location = new Point(4, 8);
      this.importBtn.Name = "importBtn";
      this.importBtn.Size = new Size(64, 22);
      this.importBtn.TabIndex = 8;
      this.importBtn.Text = "&Import...";
      this.importBtn.Click += new EventHandler(this.importBtn_Click);
      this.AcceptButton = (IButtonControl) this.okBtn;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(427, 436);
      this.Controls.Add((Control) this.panelButtons);
      this.Controls.Add((Control) this.panelMilestone);
      this.Controls.Add((Control) this.groupContainer1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddFields);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Add Required Fields";
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.panelMilestone.ResumeLayout(false);
      this.panelMilestone.PerformLayout();
      this.panelButtons.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public void SetMilestoneSelection(string[] milestones)
    {
      this.panelMilestone.Visible = this.panelButtons.Visible = true;
      this.panelButtons.Top = this.panelMilestone.Top + this.panelMilestone.Height;
      this.Height = this.panelButtons.Top + this.panelButtons.Height * 2;
      this.cboRuleValues.Items.Clear();
      this.cboRuleValues.Items.AddRange((object[]) milestones);
    }

    public string SelectedRuleValue => this.cboRuleValues.Text;

    private void okBtn_Click(object sender, EventArgs e)
    {
      if (this.collectFields() == null)
        return;
      this.selectedFieldIDs = new string[this.pickFieldIDs.Count];
      this.pickFieldIDs.CopyTo((Array) this.selectedFieldIDs);
      this.DialogResult = DialogResult.OK;
    }

    private ArrayList collectFields()
    {
      string[] strArray = new string[10]
      {
        this.textBoxID1.Text.Trim(),
        this.textBoxID2.Text.Trim(),
        this.textBoxID3.Text.Trim(),
        this.textBoxID4.Text.Trim(),
        this.textBoxID5.Text.Trim(),
        this.textBoxID6.Text.Trim(),
        this.textBoxID7.Text.Trim(),
        this.textBoxID8.Text.Trim(),
        this.textBoxID9.Text.Trim(),
        this.textBoxID10.Text.Trim()
      };
      for (int index = 0; index < 10; ++index)
      {
        if (!(strArray[index] == string.Empty) && (!strArray[index].ToLower().StartsWith("button_") && !strArray[index].ToLower().StartsWith("lockbutton_") || (this.options & AddFieldOptions.AllowButtons) == AddFieldOptions.None))
        {
          string upper = strArray[index].Trim().ToUpper();
          string text = this.validateField(upper);
          if (!string.IsNullOrEmpty(text))
          {
            int num = (int) Utils.Dialog((IWin32Window) this, text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            switch (index)
            {
              case 0:
                this.textBoxID1.Focus();
                break;
              case 1:
                this.textBoxID2.Focus();
                break;
              case 2:
                this.textBoxID3.Focus();
                break;
              case 3:
                this.textBoxID4.Focus();
                break;
              case 4:
                this.textBoxID5.Focus();
                break;
              case 5:
                this.textBoxID6.Focus();
                break;
              case 6:
                this.textBoxID7.Focus();
                break;
              case 7:
                this.textBoxID8.Focus();
                break;
              case 8:
                this.textBoxID9.Focus();
                break;
              case 9:
                this.textBoxID10.Focus();
                break;
            }
            return (ArrayList) null;
          }
          FieldDefinition field = EncompassFields.GetField(upper, this.fieldSettings);
          strArray[index] = field.FieldID;
        }
      }
      ArrayList arrayList = new ArrayList();
      for (int index = 0; index < 10; ++index)
      {
        if (strArray[index] != "")
          arrayList.Add((object) strArray[index]);
      }
      if (arrayList.Count == 0 && this.pickFieldIDs.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please enter a field ID.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return (ArrayList) null;
      }
      for (int index = 0; index < arrayList.Count; ++index)
      {
        if (!this.pickFieldIDs.Contains((object) arrayList[index].ToString()))
          this.pickFieldIDs.Add((object) arrayList[index].ToString());
      }
      return arrayList;
    }

    private void textBoxID_TextChanged(object sender, EventArgs e)
    {
      if (!(((Control) sender).Text.Trim() != "") || this.okBtn.Enabled)
        return;
      this.okBtn.Enabled = true;
      this.moreBtn.Enabled = true;
    }

    private void moreBtn_Click(object sender, EventArgs e)
    {
      if (this.collectFields() == null)
        return;
      this.selectedFieldIDs = new string[this.pickFieldIDs.Count];
      this.pickFieldIDs.CopyTo((Array) this.selectedFieldIDs);
      this.textBoxID1.Text = "";
      this.textBoxID2.Text = "";
      this.textBoxID3.Text = "";
      this.textBoxID4.Text = "";
      this.textBoxID5.Text = "";
      this.textBoxID6.Text = "";
      this.textBoxID7.Text = "";
      this.textBoxID8.Text = "";
      this.textBoxID9.Text = "";
      this.textBoxID10.Text = "";
      if (this.OnAddMoreButtonClick == null)
        return;
      this.OnAddMoreButtonClick((object) this, e);
      this.selectedFieldIDs = (string[]) null;
      this.pickFieldIDs = new ArrayList();
      this.okBtn.Enabled = false;
      this.moreBtn.Enabled = false;
    }

    private void importBtn_Click(object sender, EventArgs e)
    {
      bool flag = true;
      AddFields.ImportableLoanXDBFieldTable loanXdbFieldTable = (AddFields.ImportableLoanXDBFieldTable) null;
      ImportableLoanXDBFieldName[] headers = this.getDefaultHeader();
      OpenFileDialog openFileDialog = new OpenFileDialog();
      if (openFileDialog.ShowDialog() != DialogResult.OK || !File.Exists(openFileDialog.FileName))
        return;
      using (StreamReader streamReader = File.OpenText(openFileDialog.FileName))
      {
        string lineText;
        while ((lineText = streamReader.ReadLine()) != null)
        {
          if (!(lineText == string.Empty))
          {
            if (flag)
            {
              flag = false;
              if (this.hasHeader(lineText))
              {
                headers = this.parseHeader(lineText);
                continue;
              }
              headers = new ImportableLoanXDBFieldName[1];
            }
            string str1 = "";
            string str2 = "";
            string str3 = "";
            string str4 = "";
            string str5 = "";
            string str6 = "";
            string str7 = "";
            string[] strArray = lineText.Split(',');
            if (strArray == null)
            {
              str1 = lineText.Trim().ToUpper();
            }
            else
            {
              if (strArray.Length != 0 && strArray[0] != null)
                str1 = strArray[0].ToString().Trim().ToUpper();
              if (strArray.Length > 1 && strArray[1] != null)
                str2 = strArray[1].ToString().Trim().ToUpper();
              if (strArray.Length > 2 && strArray[2] != null)
                str3 = strArray[2].ToString().Trim().ToUpper();
              if (strArray.Length > 3 && strArray[3] != null)
                str4 = strArray[3].ToString().Trim().ToUpper();
              if (strArray.Length > 4 && strArray[4] != null)
                str5 = strArray[4].ToString().Trim().ToUpper();
              if (strArray.Length > 5 && strArray[5] != null)
                str6 = strArray[5].ToString().Trim().ToUpper();
              if (strArray.Length > 6 && strArray[6] != null)
                str7 = strArray[6].ToString().Trim().ToUpper();
            }
            if (loanXdbFieldTable == null)
              loanXdbFieldTable = new AddFields.ImportableLoanXDBFieldTable(headers);
            int rowIndex = loanXdbFieldTable.AddNewRow(str1, str2, str3, str4, str5, str6, str7);
            if (rowIndex >= 0)
            {
              DataRow row = loanXdbFieldTable.GetRow(rowIndex);
              if (row != null)
              {
                string loanXdbFieldValue1 = this.getImportableLoanXDBFieldValue(row, ImportableLoanXDBFieldName.FieldID);
                string loanXdbFieldValue2 = this.getImportableLoanXDBFieldValue(row, ImportableLoanXDBFieldName.BorrowerPair);
                string loanXdbFieldValue3 = this.getImportableLoanXDBFieldValue(row, ImportableLoanXDBFieldName.InstanceIndex);
                string str8 = this.validateField(loanXdbFieldValue1, loanXdbFieldValue2, loanXdbFieldValue3);
                if (!string.IsNullOrEmpty(str8))
                {
                  this.errorFields.Add((object) new string[3]
                  {
                    loanXdbFieldValue1,
                    loanXdbFieldValue2,
                    str8
                  });
                  loanXdbFieldTable.GetRow(rowIndex)[ImportableLoanXDBFieldName.FieldID.ToString()] = (object) ("__DELETED__" + this.getImportableLoanXDBFieldValue(loanXdbFieldTable.GetRow(rowIndex), ImportableLoanXDBFieldName.FieldID));
                }
              }
            }
          }
        }
      }
      if (this.OnImportButtonClick != null)
        this.isAddedByImport = true;
      this.importedFieldTable = new AddFields.ImportableLoanXDBFieldTable(headers);
      this.importedErrorFieldTable = new AddFields.ImportableLoanXDBFieldTable(headers);
      if (loanXdbFieldTable != null)
      {
        foreach (DataRow row in (InternalDataCollectionBase) loanXdbFieldTable.Rows)
        {
          DataRow dataRow1 = row;
          ImportableLoanXDBFieldName loanXdbFieldName = ImportableLoanXDBFieldName.FieldID;
          string columnName1 = loanXdbFieldName.ToString();
          if (!dataRow1[columnName1].ToString().StartsWith("__DELETED__"))
          {
            this.importedFieldTable.AddNewRow(row);
          }
          else
          {
            DataRow dataRow2 = row;
            loanXdbFieldName = ImportableLoanXDBFieldName.FieldID;
            string columnName2 = loanXdbFieldName.ToString();
            DataRow dataRow3 = row;
            loanXdbFieldName = ImportableLoanXDBFieldName.FieldID;
            string columnName3 = loanXdbFieldName.ToString();
            string str = dataRow3[columnName3].ToString().Replace("__DELETED__", "");
            dataRow2[columnName2] = (object) str;
            this.importedErrorFieldTable.AddNewRow(row);
          }
        }
      }
      this.DialogResult = DialogResult.OK;
    }

    private string getImportableLoanXDBFieldValue(DataRow dr, ImportableLoanXDBFieldName fieldName)
    {
      try
      {
        if (dr[fieldName.ToString()] != null)
          return dr[fieldName.ToString()].ToString();
      }
      catch
      {
        return "";
      }
      return "";
    }

    private string validateField(string fieldID, string fieldPair, string fieldInstance)
    {
      if (string.IsNullOrEmpty(fieldID))
        return "The field ID '" + fieldID + "' is invalid.";
      if (this.fieldsNotAllowed != null && this.fieldsNotAllowed.Contains(fieldID))
        return "The field ID '" + fieldID + "' is not allowed.";
      if (fieldID.ToLower() == "loanfolder")
        return "";
      if (!string.IsNullOrEmpty(fieldPair))
      {
        int result = -1;
        if (int.TryParse(fieldPair, out result) && (result < 1 || result > 6))
          return "The field ID " + fieldID + "'s pair number '" + fieldPair + "' is invalid.";
      }
      FieldDefinition field = EncompassFields.GetField(fieldID, this.fieldSettings);
      if (field == null)
        return "The field ID '" + fieldID + "' is invalid.";
      if (field is CustomField && (this.options & AddFieldOptions.AllowCustomFields) == AddFieldOptions.None)
        return "The field ID '" + fieldID + "' is a custom field and cannot be used in this context.";
      if (field is VirtualField && (this.options & AddFieldOptions.AllowVirtualFields) == AddFieldOptions.None)
        return "The field ID '" + fieldID + "' is a virtual field and cannot be used in this context.";
      return !field.AllowInReportingDatabase && (this.options & AddFieldOptions.AllowHiddenFields) == AddFieldOptions.None ? "The field ID '" + fieldID + "' is a hidden field and cannot be used in this context." : "";
    }

    private string validateField(string fieldID, string fieldPair)
    {
      return this.validateField(fieldID, fieldPair, string.Empty);
    }

    private string validateField(string fieldID) => this.validateField(fieldID, string.Empty);

    private bool hasHeader(string lineText)
    {
      string str1 = lineText;
      char[] chArray = new char[1]{ ',' };
      foreach (string str2 in str1.Split(chArray))
      {
        string lower = (str2 ?? "").ToLower();
        if (lower == ImportableLoanXDBFieldName.FieldID.ToString().ToLower() || lower == ImportableLoanXDBFieldName.FieldID.ToString().ToLower() || lower == ImportableLoanXDBFieldName.BorrowerPair.ToString().ToLower() || lower == ImportableLoanXDBFieldName.InstanceIndex.ToString().ToLower() || lower == ImportableLoanXDBFieldName.FieldSize.ToString().ToLower() || lower == ImportableLoanXDBFieldName.Description.ToString().ToLower() || lower == ImportableLoanXDBFieldName.IncludeInAuditTrail.ToString().ToLower() || lower == ImportableLoanXDBFieldName.UseIndexForThisField.ToString().ToLower())
          return true;
      }
      return false;
    }

    private ImportableLoanXDBFieldName[] parseHeader(string lineText)
    {
      ImportableLoanXDBFieldName[] header = new ImportableLoanXDBFieldName[Enum.GetNames(typeof (ImportableLoanXDBFieldName)).Length - 1];
      try
      {
        for (int index = 0; index < header.Length; ++index)
        {
          if (header[index] != ImportableLoanXDBFieldName.Unknown)
            header[index] = ImportableLoanXDBFieldName.Unknown;
        }
        string[] strArray = lineText.Split(',');
        for (int index = 0; index < strArray.Length; ++index)
        {
          if (header.Length > index)
            header[index] = AddFields.GetImportableLoanXDBFieldNameEnum(strArray[index]);
        }
      }
      catch
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Invalid CSV header row.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      return header;
    }

    private ImportableLoanXDBFieldName[] getDefaultHeader()
    {
      ImportableLoanXDBFieldName[] defaultHeader = new ImportableLoanXDBFieldName[Enum.GetNames(typeof (ImportableLoanXDBFieldName)).Length - 1];
      if (defaultHeader.Length == 0)
        defaultHeader[0] = ImportableLoanXDBFieldName.FieldID;
      for (int index = 0; index < defaultHeader.Length; ++index)
      {
        if (defaultHeader[index] != ImportableLoanXDBFieldName.Unknown)
          defaultHeader[index] = ImportableLoanXDBFieldName.Unknown;
      }
      return defaultHeader;
    }

    public static ImportableLoanXDBFieldName GetImportableLoanXDBFieldNameEnum(string name)
    {
      foreach (ImportableLoanXDBFieldName xdbFieldNameEnum in Enum.GetValues(typeof (ImportableLoanXDBFieldName)))
      {
        if (xdbFieldNameEnum.ToString().ToLower().Equals(name.Trim().ToLower()))
          return xdbFieldNameEnum;
      }
      return ImportableLoanXDBFieldName.Unknown;
    }

    public class ImportableLoanXDBFieldTable
    {
      private DataTable dt = new DataTable();
      private ImportableLoanXDBFieldName[] headers;

      public ImportableLoanXDBFieldTable(ImportableLoanXDBFieldName[] headers)
      {
        this.headers = headers;
        for (int index = 0; index < headers.Length; ++index)
        {
          if (headers[index] != ImportableLoanXDBFieldName.Unknown)
            this.dt.Columns.Add(headers[index].ToString());
        }
      }

      public DataRowCollection Rows => this.dt.Rows;

      public int AddNewRow(params string[] fields)
      {
        if (fields.Length < this.headers.Length)
          return -1;
        int columnIndex = 0;
        DataRow row = this.dt.NewRow();
        foreach (string field in fields)
        {
          if (this.dt.Columns.Count > columnIndex)
            row[columnIndex] = string.IsNullOrEmpty(field) ? (object) "" : (object) field.Trim();
          ++columnIndex;
        }
        this.dt.Rows.Add(row);
        return this.dt.Rows.Count - 1;
      }

      public int AddNewRow(DataRow dr)
      {
        this.dt.ImportRow(dr);
        return this.dt.Rows.Count - 1;
      }

      public DataRow GetRow(int rowIndex)
      {
        return rowIndex < this.dt.Rows.Count ? this.dt.Rows[rowIndex] : (DataRow) null;
      }

      public string ToCSV()
      {
        StringBuilder stringBuilder = new StringBuilder();
        bool flag1 = true;
        foreach (DataColumn column in (InternalDataCollectionBase) this.dt.Columns)
        {
          if (flag1)
            stringBuilder.Append(column.ColumnName);
          else
            stringBuilder.Append(",").Append(column.ColumnName);
          flag1 = false;
        }
        stringBuilder.Append("\r\n");
        foreach (DataRow row in (InternalDataCollectionBase) this.dt.Rows)
        {
          bool flag2 = true;
          foreach (DataColumn column in (InternalDataCollectionBase) this.dt.Columns)
          {
            if (flag2)
              stringBuilder.Append(row[column.ColumnName].ToString());
            else
              stringBuilder.Append(",").Append(row[column.ColumnName].ToString());
            flag2 = false;
          }
          stringBuilder.Append("\r\n");
        }
        return stringBuilder.ToString();
      }
    }
  }
}
