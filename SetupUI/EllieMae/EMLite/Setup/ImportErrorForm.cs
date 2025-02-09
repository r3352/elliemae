// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ImportErrorForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.Setup.DynamicDataManagement;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class ImportErrorForm : Form
  {
    private FieldSettings _fieldSettings;
    private StandardFields _standardFields;
    private List<List<string>> _importErrorData;
    private ImportType _importType;
    private IContainer components;
    private Label lblFormText;
    private GroupContainer groupContainer;
    private Button btnAbort;
    private GridView gvErrors;
    private StandardIconButton stdButtonExport;
    private ToolTip toolTip;

    public ImportErrorForm() => this.InitializeComponent();

    public ImportErrorForm(
      string title,
      string subtitle,
      List<List<string>> importErrorData,
      StandardFields standardFields,
      FieldSettings fieldSettings,
      ImportType importType)
      : this(title, subtitle, importErrorData, standardFields, fieldSettings)
    {
      this._importType = importType;
    }

    public ImportErrorForm(
      string title,
      string subtitle,
      List<List<string>> importErrorData,
      StandardFields standardFields,
      FieldSettings fieldSettings)
    {
      this.InitializeComponent();
      this.Text = title;
      this.lblFormText.Text = subtitle;
      this._importErrorData = importErrorData;
      this._standardFields = standardFields;
      this._fieldSettings = fieldSettings;
      this.AddGridColumns(importErrorData[0]);
      importErrorData.RemoveAt(0);
      foreach (List<string> rowData in importErrorData)
        this.AddGridRow(rowData);
    }

    private void AddGridRow(List<string> rowData)
    {
      GVItem gvItem = new GVItem();
      foreach (string str in rowData)
        gvItem.SubItems.Add(new GVSubItem()
        {
          Text = str,
          Value = (object) str
        });
      this.gvErrors.Items.Add(gvItem);
    }

    private void AddGridColumns(List<string> columns)
    {
      foreach (string column in columns)
      {
        GVColumn newColumn = new GVColumn(column);
        if (string.Compare(column, "Error", true) == 0)
        {
          newColumn.Width = 120;
          this.gvErrors.Columns.Add(newColumn);
        }
        else
        {
          FieldDefinition fieldDefinition = EncompassFields.GetField(column, this._fieldSettings);
          if (fieldDefinition == null && this._standardFields.VirtualFields.Contains(column))
            fieldDefinition = this._standardFields.VirtualFields[column];
          newColumn.Width = fieldDefinition == null || fieldDefinition.MaxLength < 100 ? 100 : fieldDefinition.MaxLength;
          this.gvErrors.Columns.Add(newColumn);
        }
      }
    }

    private void btnAbort_Click(object sender, EventArgs e) => this.Close();

    private void stdButtonExport_Click(object sender, EventArgs e)
    {
      SaveFileDialog saveFileDialog = new SaveFileDialog();
      saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
      string str = "";
      switch (this._importType)
      {
        case ImportType.None:
        case ImportType.DataTable:
          str = "DataTable";
          break;
        case ImportType.FieldRule:
          str = "FieldRule";
          break;
      }
      saveFileDialog.FileName = str + "ImportErrors_" + DateTime.Now.ToString("MMddyyyy_hms") + ".csv";
      if (saveFileDialog.ShowDialog() != DialogResult.OK)
        return;
      if (File.Exists(saveFileDialog.FileName))
        File.Delete(saveFileDialog.FileName);
      using (TextWriter writer = (TextWriter) new StreamWriter(saveFileDialog.FileName, false))
        CsvUtility.WriteRows(writer, this._importErrorData);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      this.lblFormText = new Label();
      this.groupContainer = new GroupContainer();
      this.stdButtonExport = new StandardIconButton();
      this.gvErrors = new GridView();
      this.btnAbort = new Button();
      this.toolTip = new ToolTip(this.components);
      this.groupContainer.SuspendLayout();
      ((ISupportInitialize) this.stdButtonExport).BeginInit();
      this.SuspendLayout();
      this.lblFormText.AutoSize = true;
      this.lblFormText.Location = new Point(13, 13);
      this.lblFormText.Name = "lblFormText";
      this.lblFormText.Size = new Size(0, 13);
      this.lblFormText.TabIndex = 0;
      this.groupContainer.Controls.Add((Control) this.stdButtonExport);
      this.groupContainer.Controls.Add((Control) this.gvErrors);
      this.groupContainer.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer.Location = new Point(16, 30);
      this.groupContainer.Name = "groupContainer";
      this.groupContainer.Size = new Size(879, 387);
      this.groupContainer.TabIndex = 1;
      this.stdButtonExport.BackColor = Color.Transparent;
      this.stdButtonExport.Location = new Point(853, 5);
      this.stdButtonExport.MouseDownImage = (Image) null;
      this.stdButtonExport.Name = "stdButtonExport";
      this.stdButtonExport.Size = new Size(16, 16);
      this.stdButtonExport.StandardButtonType = StandardIconButton.ButtonType.ExcelButton;
      this.stdButtonExport.TabIndex = 1;
      this.stdButtonExport.TabStop = false;
      this.stdButtonExport.Tag = (object) "";
      this.toolTip.SetToolTip((Control) this.stdButtonExport, "Export");
      this.stdButtonExport.Click += new EventHandler(this.stdButtonExport_Click);
      this.gvErrors.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvErrors.Location = new Point(0, 29);
      this.gvErrors.Name = "gvErrors";
      this.gvErrors.Size = new Size(879, 358);
      this.gvErrors.TabIndex = 0;
      this.btnAbort.Location = new Point(808, 423);
      this.btnAbort.Name = "btnAbort";
      this.btnAbort.Size = new Size(75, 23);
      this.btnAbort.TabIndex = 2;
      this.btnAbort.Text = "Abort";
      this.btnAbort.UseVisualStyleBackColor = true;
      this.btnAbort.Click += new EventHandler(this.btnAbort_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(895, 462);
      this.Controls.Add((Control) this.btnAbort);
      this.Controls.Add((Control) this.groupContainer);
      this.Controls.Add((Control) this.lblFormText);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ImportErrorForm);
      this.StartPosition = FormStartPosition.CenterParent;
      this.groupContainer.ResumeLayout(false);
      ((ISupportInitialize) this.stdButtonExport).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
