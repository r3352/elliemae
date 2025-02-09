// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DynamicDataManagement.DDMRuleImportInfoDlg
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Setup.DynamicDataManagement.ImportExport;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.DynamicDataManagement
{
  public class DDMRuleImportInfoDlg : Form
  {
    private string viewAll = "View all";
    private string viewFailedDependencies = "View failed dependecies";
    private ImportType importRule;
    private string ruleName = string.Empty;
    private string feeLineNum = string.Empty;
    private string lastModifiedDateTime = string.Empty;
    private string lastModifiedBy = string.Empty;
    private bool isSucceeded;
    private List<DDMFieldValueValidation> validationInfo;
    private string exceptionLog;
    private IContainer components;
    private Panel panel1;
    private Button cancel_btn;
    private Button importRule_btn;
    private System.Windows.Forms.LinkLabel exceptionLog_Label;
    private GroupContainer groupContainer1;
    private ComboBox filter_comboBox;
    private FlowLayoutPanel flowLayoutPanel1;
    private Label label8;
    private Label label5;
    private Label label6;
    private Label label7;
    private GridView gvDependencies;
    private ImageList imageListValidationStatus;

    public DDMRuleImportInfoDlg(
      ImportType ruleType,
      string ruleName,
      string feeLineNumber,
      string dateTime,
      string lastModifiedBy,
      DDMRuleValidationResult validationResult)
    {
      this.importRule = ruleType;
      this.ruleName = ruleName;
      this.feeLineNum = feeLineNumber;
      this.lastModifiedDateTime = dateTime;
      this.lastModifiedBy = lastModifiedBy;
      this.validationInfo = validationResult.fieldValidationResults;
      this.exceptionLog = validationResult.exceptionLog;
      this.isSucceeded = validationResult.validationSucceeded;
      this.InitializeComponent();
    }

    private void DDMRuleImportInfoDlg_Load(object sender, EventArgs e)
    {
      this.filter_comboBox.SelectedIndex = this.isSucceeded ? 0 : 1;
      this.exceptionLog_Label.Visible = !this.isSucceeded;
      this.label6.Text = "Last Modified By: " + this.lastModifiedBy;
      this.label7.Text = "Last Modified Date Time: " + this.lastModifiedDateTime;
      switch (this.importRule)
      {
        case ImportType.FieldRule:
          this.Text = "Field Rule Import";
          this.label8.Text = "Field Rule: " + this.ruleName;
          this.label5.Hide();
          break;
        case ImportType.FeeRule:
          this.Text = "Fee Rule Import";
          this.label8.Text = "Fee Rule Name: " + this.ruleName;
          this.label5.Text = "Fee Line: " + this.feeLineNum;
          break;
      }
      if (this.isSucceeded)
        return;
      this.disableImportBtn();
      int num = (int) Utils.Dialog((IWin32Window) this, "One or more dependencies have failed validation. Click OK to view the dependencies.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }

    private void filter_comboBox_selectedIndexChanged(object sender, EventArgs e)
    {
      if (this.filter_comboBox.SelectedItem.ToString() == this.viewAll)
      {
        this.gvDependencies.Items.Clear();
        foreach (DDMFieldValueValidation fieldValueValidation in this.validationInfo)
          this.addGVItem(fieldValueValidation);
      }
      else
      {
        if (!(this.filter_comboBox.SelectedItem.ToString() == this.viewFailedDependencies))
          return;
        this.gvDependencies.Items.Clear();
        foreach (DDMFieldValueValidation fieldValueValidation in this.validationInfo.Where<DDMFieldValueValidation>((Func<DDMFieldValueValidation, bool>) (record => !record.validatedCorrectly)))
          this.addGVItem(fieldValueValidation);
      }
    }

    private void addGVItem(DDMFieldValueValidation fieldValueValidation)
    {
      GVItem gvItem = new GVItem((object) (this.gvDependencies.Items.Count + 1));
      gvItem.SubItems.Add((object) fieldValueValidation.scenarioName);
      gvItem.SubItems.Add((object) fieldValueValidation.dependency);
      gvItem.SubItems.Add((object) fieldValueValidation.fieldID);
      gvItem.SubItems.Add((object) fieldValueValidation.fieldDescription);
      if (fieldValueValidation.isRequired)
        gvItem.SubItems.Add((object) "Required");
      else
        gvItem.SubItems.Add((object) "Optional");
      gvItem.SubItems.Add((object) "");
      gvItem.SubItems[6].ImageIndex = !fieldValueValidation.validatedCorrectly ? 2 : 0;
      if (!fieldValueValidation.validatedCorrectly && fieldValueValidation.isRequired)
        gvItem.SubItems.Add((object) this.sourceInfo(new string[2]
        {
          fieldValueValidation.fieldID,
          fieldValueValidation.sourceXmlPortion
        }));
      gvItem.Tag = (object) fieldValueValidation;
      this.gvDependencies.Items.Add(gvItem);
    }

    private EllieMae.EMLite.UI.LinkLabel sourceInfo(string[] info)
    {
      EllieMae.EMLite.UI.LinkLabel linkLabel = new EllieMae.EMLite.UI.LinkLabel();
      linkLabel.Text = "View Info";
      linkLabel.ForeColor = Color.FromArgb(29, 110, 174);
      linkLabel.Tag = (object) info;
      linkLabel.Click += new EventHandler(this.infoLabel_Click);
      return linkLabel;
    }

    private void infoLabel_Click(object sender, EventArgs e)
    {
      string[] tag = (string[]) ((Control) sender).Tag;
      try
      {
        using (DependencyStatusInfoDialog statusInfoDialog = new DependencyStatusInfoDialog(tag[0], tag[1]))
        {
          statusInfoDialog.setRuleImportUI();
          int num = (int) statusInfoDialog.ShowDialog();
        }
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The info is not able to show: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void cancel_btn_Click(object sender, EventArgs e)
    {
      this.Close();
      this.DialogResult = DialogResult.Cancel;
    }

    private void exceptionLog_Label_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      SaveFileDialog saveFileDialog = new SaveFileDialog();
      saveFileDialog.Filter = "TXT files (*.txt)|*.txt";
      string str = "";
      switch (this.importRule)
      {
        case ImportType.None:
        case ImportType.FeeRule:
          str = "FeeRule";
          break;
        case ImportType.FieldRule:
          str = "FieldRule";
          break;
      }
      saveFileDialog.FileName = str + "ImportErrors_" + DateTime.Now.ToString("MMddyyyy_hms") + ".txt";
      if (saveFileDialog.ShowDialog() != DialogResult.OK)
        return;
      if (File.Exists(saveFileDialog.FileName))
        File.Delete(saveFileDialog.FileName);
      try
      {
        File.WriteAllText(saveFileDialog.FileName, this.exceptionLog);
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The file can not be saved: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void importRule_btn_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    public void disableImportBtn() => this.importRule_btn.Enabled = false;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      GVColumn gvColumn7 = new GVColumn();
      GVColumn gvColumn8 = new GVColumn();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (DDMRuleImportInfoDlg));
      this.panel1 = new Panel();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.label8 = new Label();
      this.label5 = new Label();
      this.label7 = new Label();
      this.label6 = new Label();
      this.groupContainer1 = new GroupContainer();
      this.gvDependencies = new GridView();
      this.imageListValidationStatus = new ImageList(this.components);
      this.filter_comboBox = new ComboBox();
      this.exceptionLog_Label = new System.Windows.Forms.LinkLabel();
      this.cancel_btn = new Button();
      this.importRule_btn = new Button();
      this.panel1.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.panel1.Controls.Add((Control) this.flowLayoutPanel1);
      this.panel1.Controls.Add((Control) this.groupContainer1);
      this.panel1.Controls.Add((Control) this.exceptionLog_Label);
      this.panel1.Location = new Point(3, 12);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(873, 500);
      this.panel1.TabIndex = 0;
      this.flowLayoutPanel1.Controls.Add((Control) this.label8);
      this.flowLayoutPanel1.Controls.Add((Control) this.label5);
      this.flowLayoutPanel1.Controls.Add((Control) this.label7);
      this.flowLayoutPanel1.Controls.Add((Control) this.label6);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
      this.flowLayoutPanel1.Location = new Point(0, 0);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(670, 90);
      this.flowLayoutPanel1.TabIndex = 4;
      this.label8.AutoSize = true;
      this.label8.Location = new Point(3, 0);
      this.label8.Name = "label8";
      this.label8.Padding = new Padding(4);
      this.label8.Size = new Size(95, 21);
      this.label8.TabIndex = 4;
      this.label8.Text = "Fee Rule Name: ";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(3, 21);
      this.label5.Name = "label5";
      this.label5.Padding = new Padding(4);
      this.label5.Size = new Size(62, 21);
      this.label5.TabIndex = 7;
      this.label5.Text = "Fee Line: ";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(3, 42);
      this.label7.Name = "label7";
      this.label7.Padding = new Padding(4);
      this.label7.Size = new Size(136, 21);
      this.label7.TabIndex = 5;
      this.label7.Text = "Last Modified Date Time: ";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(3, 63);
      this.label6.Name = "label6";
      this.label6.Padding = new Padding(4);
      this.label6.Size = new Size(99, 21);
      this.label6.TabIndex = 6;
      this.label6.Text = "Last Modified By: ";
      this.groupContainer1.Controls.Add((Control) this.gvDependencies);
      this.groupContainer1.Controls.Add((Control) this.filter_comboBox);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 96);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(870, 401);
      this.groupContainer1.TabIndex = 3;
      this.groupContainer1.Text = "   List of Scenarios and Dependencies";
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Order";
      gvColumn1.SortMethod = GVSortMethod.Numeric;
      gvColumn1.Text = "#";
      gvColumn1.Width = 30;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Scenario Name";
      gvColumn2.Text = "Scenario Name";
      gvColumn2.Width = 150;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Dependency On";
      gvColumn3.Text = "Dependency On";
      gvColumn3.Width = 160;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Identifier";
      gvColumn4.Text = "Identifier";
      gvColumn4.Width = 100;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Name";
      gvColumn5.Text = "Name";
      gvColumn5.Width = 168;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "RequiredOptional";
      gvColumn6.Text = "Required";
      gvColumn6.Width = 110;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Validation";
      gvColumn7.Text = "Validation";
      gvColumn7.Width = 60;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "Source";
      gvColumn8.Text = "Source";
      gvColumn8.Width = 73;
      this.gvDependencies.Columns.AddRange(new GVColumn[8]
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
      this.gvDependencies.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvDependencies.ImageList = this.imageListValidationStatus;
      this.gvDependencies.Location = new Point(0, 26);
      this.gvDependencies.Name = "gvDependencies";
      this.gvDependencies.Size = new Size(870, 359);
      this.gvDependencies.TabIndex = 1;
      this.imageListValidationStatus.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imageListValidationStatus.ImageStream");
      this.imageListValidationStatus.TransparentColor = Color.Transparent;
      this.imageListValidationStatus.Images.SetKeyName(0, "check-mark-green.png");
      this.imageListValidationStatus.Images.SetKeyName(1, "status-warning.png");
      this.imageListValidationStatus.Images.SetKeyName(2, "stop.png");
      this.filter_comboBox.FormattingEnabled = true;
      this.filter_comboBox.Items.AddRange(new object[2]
      {
        (object) "View all",
        (object) "View failed dependecies"
      });
      this.filter_comboBox.Location = new Point(652, 2);
      this.filter_comboBox.Name = "filter_comboBox";
      this.filter_comboBox.Size = new Size(176, 21);
      this.filter_comboBox.TabIndex = 0;
      this.filter_comboBox.SelectedIndexChanged += new EventHandler(this.filter_comboBox_selectedIndexChanged);
      this.exceptionLog_Label.AutoSize = true;
      this.exceptionLog_Label.Location = new Point(702, 77);
      this.exceptionLog_Label.Name = "exceptionLog_Label";
      this.exceptionLog_Label.Size = new Size(126, 13);
      this.exceptionLog_Label.TabIndex = 1;
      this.exceptionLog_Label.TabStop = true;
      this.exceptionLog_Label.Text = "Download Exception Log";
      this.exceptionLog_Label.LinkClicked += new LinkLabelLinkClickedEventHandler(this.exceptionLog_Label_LinkClicked);
      this.cancel_btn.Location = new Point(679, 518);
      this.cancel_btn.Name = "cancel_btn";
      this.cancel_btn.Size = new Size(75, 23);
      this.cancel_btn.TabIndex = 1;
      this.cancel_btn.Text = "Cancel";
      this.cancel_btn.UseVisualStyleBackColor = true;
      this.cancel_btn.Click += new EventHandler(this.cancel_btn_Click);
      this.importRule_btn.Location = new Point(760, 518);
      this.importRule_btn.Name = "importRule_btn";
      this.importRule_btn.Size = new Size(99, 23);
      this.importRule_btn.TabIndex = 2;
      this.importRule_btn.Text = "Import Rule";
      this.importRule_btn.UseVisualStyleBackColor = true;
      this.importRule_btn.Click += new EventHandler(this.importRule_btn_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(879, 551);
      this.Controls.Add((Control) this.importRule_btn);
      this.Controls.Add((Control) this.cancel_btn);
      this.Controls.Add((Control) this.panel1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (DDMRuleImportInfoDlg);
      this.ShowIcon = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Fee Rule Import";
      this.Load += new EventHandler(this.DDMRuleImportInfoDlg_Load);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.flowLayoutPanel1.ResumeLayout(false);
      this.flowLayoutPanel1.PerformLayout();
      this.groupContainer1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
