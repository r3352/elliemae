// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DynamicDataManagement.DataTableList
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.DynamicDataManagement
{
  public class DataTableList : Form
  {
    private Sessions.Session session;
    private string feeLineNumber;
    private FeeValueDlg.ddmTab ddmTab;
    private readonly GridView _gridView = new GridView();
    private DDMFeeRuleValue feeRuleValue;
    private IContainer components;
    private GroupContainer gcBaseRate;
    private GridView listViewOptions;
    private GradientPanel gradientPanel2;
    private Label lblSubTitle;
    private Button btnSelect;
    private Button btnCancel;

    public TableDefinitionTag TableDefinition { get; private set; }

    public DataTableList(
      FeeValueDlg.ddmTab ddmTab,
      Sessions.Session session,
      string feeLineNumber,
      DDMFeeRuleValue feeRuleValue)
    {
      this.InitializeComponent();
      this.ddmTab = ddmTab;
      this.session = session;
      this.feeLineNumber = feeLineNumber;
      this.feeRuleValue = feeRuleValue;
      this.initializeTableList();
    }

    private void initializeTableList()
    {
      this._gridView.Items.Clear();
      DDMDataTable[] allDdmDataTable = ((DDMDataTableBpmManager) this.session.BPM.GetBpmManager(BpmCategory.DDMDataTables)).GetAllDDMDataTable(true);
      if (allDdmDataTable != null)
      {
        foreach (DDMDataTable ddmDataTable in allDdmDataTable)
        {
          if (ddmDataTable != null)
          {
            try
            {
              List<string> list = ((IEnumerable<string>) ddmDataTable.OutputIdList.Split('|')).ToList<string>();
              this._gridView.Items.Add(new GVItem(ddmDataTable.Name)
              {
                SubItems = {
                  (object) ddmDataTable.Description,
                  (object) "Data Table",
                  (object) ddmDataTable.LastModByFullName,
                  (object) ddmDataTable.LastModDt
                },
                Tag = (object) new TableDefinitionTag()
                {
                  TableName = ddmDataTable.Name,
                  Type = "DDM",
                  LineNumber = this.feeLineNumber,
                  OutputColumns = list
                }
              });
            }
            catch (Exception ex)
            {
            }
          }
          else
            break;
        }
      }
      if (this.ddmTab == FeeValueDlg.ddmTab.FeeField)
      {
        switch (this.feeRuleValue.FieldID)
        {
          case "1637":
            foreach (FeeListBase.FeeTable fee in ((FeeListBase) this.session.GetSystemSettings(typeof (FeeCityList))).FeeList)
              this._gridView.Items.Add(new GVItem(fee.FeeName)
              {
                SubItems = {
                  (object) string.Format("Calculation based on {0}", (object) fee.CalcBasedOn),
                  (object) "System table - City Tax"
                },
                Tag = (object) new TableDefinitionTag()
                {
                  TableName = fee.FeeName,
                  Type = "TAX",
                  LineNumber = "1204"
                }
              });
            break;
          case "1638":
            foreach (FeeListBase.FeeTable fee in ((FeeListBase) this.session.GetSystemSettings(typeof (FeeStateList))).FeeList)
              this._gridView.Items.Add(new GVItem(fee.FeeName)
              {
                SubItems = {
                  (object) string.Format("Calculation based on {0}", (object) fee.CalcBasedOn),
                  (object) "System table - State Tax"
                },
                Tag = (object) new TableDefinitionTag()
                {
                  TableName = fee.FeeName,
                  Type = "TAX",
                  LineNumber = "1205"
                }
              });
            break;
          case "1640":
          case "1641":
          case "1643":
          case "1644":
          case "373":
          case "374":
            foreach (FeeListBase.FeeTable fee in ((FeeListBase) this.session.GetSystemSettings(typeof (FeeUserList))).FeeList)
            {
              GVItem gvItem = new GVItem(fee.FeeName);
              gvItem.SubItems.Add((object) string.Format("Calculation based on {0}", (object) fee.CalcBasedOn));
              gvItem.SubItems.Add((object) "System table - User Defined Fee");
              string str = "";
              switch (this.feeRuleValue.FieldID)
              {
                case "1640":
                case "1641":
                  str = "1207";
                  break;
                case "1643":
                case "1644":
                  str = "1208";
                  break;
                case "373":
                case "374":
                  str = "1206";
                  break;
              }
              gvItem.Tag = (object) new TableDefinitionTag()
              {
                TableName = fee.FeeName,
                Type = "TAX",
                LineNumber = str
              };
              this._gridView.Items.Add(gvItem);
            }
            break;
          case "232":
          case "337":
          case "NEWHUD.X1707":
            this._gridView.Items.Add(new GVItem("MI Tables")
            {
              SubItems = {
                (object) "Get MI - Mortgage Insurance Tables",
                (object) "System Table"
              },
              Tag = (object) new TableDefinitionTag()
              {
                TableName = "MI Tables",
                Type = "MI",
                LineNumber = "902"
              }
            });
            break;
          case "NEWHUD.X572":
            foreach (DictionaryEntry feeTable1 in ((TableFeeListBase) this.session.GetSystemSettings(typeof (TblTitlePurList))).FeeTables)
            {
              TableFeeListBase.FeeTable feeTable2 = (TableFeeListBase.FeeTable) feeTable1.Value;
              if (!(feeTable2.FeeType != "Owner"))
                this._gridView.Items.Add(new GVItem(feeTable2.TableName)
                {
                  SubItems = {
                    (object) string.Format("Calculation based on {0}", (object) feeTable2.CalcBasedOn),
                    (object) "System table - Title Owner Purchase"
                  },
                  Tag = (object) new TableDefinitionTag()
                  {
                    TableName = feeTable2.TableName,
                    Type = "ESCROWTITLE",
                    LineNumber = "1103"
                  }
                });
            }
            IDictionaryEnumerator enumerator1 = ((TableFeeListBase) this.session.GetSystemSettings(typeof (TblTitleRefiList))).FeeTables.GetEnumerator();
            try
            {
              while (enumerator1.MoveNext())
              {
                TableFeeListBase.FeeTable feeTable = (TableFeeListBase.FeeTable) ((DictionaryEntry) enumerator1.Current).Value;
                if (!(feeTable.FeeType != "Owner"))
                  this._gridView.Items.Add(new GVItem(feeTable.TableName)
                  {
                    SubItems = {
                      (object) string.Format("Calculation based on {0}", (object) feeTable.CalcBasedOn),
                      (object) "System table - Title Owner Refinance"
                    },
                    Tag = (object) new TableDefinitionTag()
                    {
                      TableName = feeTable.TableName,
                      Type = "ESCROWTITLE",
                      LineNumber = "1103"
                    }
                  });
              }
              break;
            }
            finally
            {
              if (enumerator1 is IDisposable disposable)
                disposable.Dispose();
            }
          case "NEWHUD.X639":
            foreach (DictionaryEntry feeTable3 in ((TableFeeListBase) this.session.GetSystemSettings(typeof (TblTitlePurList))).FeeTables)
            {
              TableFeeListBase.FeeTable feeTable4 = (TableFeeListBase.FeeTable) feeTable3.Value;
              if (!(feeTable4.FeeType != "Lender"))
                this._gridView.Items.Add(new GVItem(feeTable4.TableName)
                {
                  SubItems = {
                    (object) string.Format("Calculation based on {0}", (object) feeTable4.CalcBasedOn),
                    (object) "System table - Title Lender Purchase"
                  },
                  Tag = (object) new TableDefinitionTag()
                  {
                    TableName = feeTable4.TableName,
                    Type = "ESCROWTITLE",
                    LineNumber = "1104"
                  }
                });
            }
            IDictionaryEnumerator enumerator2 = ((TableFeeListBase) this.session.GetSystemSettings(typeof (TblTitleRefiList))).FeeTables.GetEnumerator();
            try
            {
              while (enumerator2.MoveNext())
              {
                TableFeeListBase.FeeTable feeTable = (TableFeeListBase.FeeTable) ((DictionaryEntry) enumerator2.Current).Value;
                if (!(feeTable.FeeType != "Lender"))
                  this._gridView.Items.Add(new GVItem(feeTable.TableName)
                  {
                    SubItems = {
                      (object) string.Format("Calculation based on {0}", (object) feeTable.CalcBasedOn),
                      (object) "System table - Title Lender Refinance"
                    },
                    Tag = (object) new TableDefinitionTag()
                    {
                      TableName = feeTable.TableName,
                      Type = "ESCROWTITLE",
                      LineNumber = "1104"
                    }
                  });
              }
              break;
            }
            finally
            {
              if (enumerator2 is IDisposable disposable)
                disposable.Dispose();
            }
          case "NEWHUD.X808":
            foreach (DictionaryEntry feeTable5 in ((TableFeeListBase) this.session.GetSystemSettings(typeof (TblEscrowPurList))).FeeTables)
            {
              TableFeeListBase.FeeTable feeTable6 = (TableFeeListBase.FeeTable) feeTable5.Value;
              this._gridView.Items.Add(new GVItem(feeTable6.TableName)
              {
                SubItems = {
                  (object) string.Format("Calculation based on {0}", (object) feeTable6.CalcBasedOn),
                  (object) "System table - Escrow Purchase"
                },
                Tag = (object) new TableDefinitionTag()
                {
                  TableName = feeTable6.TableName,
                  Type = "ESCROWTITLE",
                  LineNumber = "1102c"
                }
              });
            }
            IDictionaryEnumerator enumerator3 = ((TableFeeListBase) this.session.GetSystemSettings(typeof (TblEscrowRefiList))).FeeTables.GetEnumerator();
            try
            {
              while (enumerator3.MoveNext())
              {
                TableFeeListBase.FeeTable feeTable = (TableFeeListBase.FeeTable) ((DictionaryEntry) enumerator3.Current).Value;
                this._gridView.Items.Add(new GVItem(feeTable.TableName)
                {
                  SubItems = {
                    (object) string.Format("Calculation based on {0}", (object) feeTable.CalcBasedOn),
                    (object) "System table - Escrow Refinance"
                  },
                  Tag = (object) new TableDefinitionTag()
                  {
                    TableName = feeTable.TableName,
                    Type = "ESCROWTITLE",
                    LineNumber = "1102c"
                  }
                });
              }
              break;
            }
            finally
            {
              if (enumerator3 is IDisposable disposable)
                disposable.Dispose();
            }
        }
      }
      this.listViewOptions.Items.Clear();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this._gridView.Items)
        this.listViewOptions.Items.Add(gvItem);
    }

    private void btnSelect_Click(object sender, EventArgs e)
    {
      if (this.listViewOptions.SelectedItems.Count != 1)
        return;
      this.TableDefinition = (TableDefinitionTag) this.listViewOptions.SelectedItems[0].Tag;
    }

    private void listViewOptions_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnSelect.Enabled = this.listViewOptions.SelectedItems.Count == 1;
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
      this.gcBaseRate = new GroupContainer();
      this.listViewOptions = new GridView();
      this.gradientPanel2 = new GradientPanel();
      this.lblSubTitle = new Label();
      this.btnSelect = new Button();
      this.btnCancel = new Button();
      this.gcBaseRate.SuspendLayout();
      this.gradientPanel2.SuspendLayout();
      this.SuspendLayout();
      this.gcBaseRate.Borders = AnchorStyles.Top;
      this.gcBaseRate.Controls.Add((Control) this.listViewOptions);
      this.gcBaseRate.HeaderForeColor = SystemColors.ControlText;
      this.gcBaseRate.Location = new Point(0, 31);
      this.gcBaseRate.Name = "gcBaseRate";
      this.gcBaseRate.Size = new Size(773, 441);
      this.gcBaseRate.TabIndex = 6;
      this.gcBaseRate.Text = "Data Tables List";
      this.listViewOptions.AutoHeight = true;
      this.listViewOptions.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Name";
      gvColumn1.Width = 150;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Description";
      gvColumn2.Width = 220;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Type";
      gvColumn3.Width = 100;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Text = "Last Modified By";
      gvColumn4.Width = 150;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.Text = "Last Modified Date & Time";
      gvColumn5.Width = 150;
      this.listViewOptions.Columns.AddRange(new GVColumn[5]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5
      });
      this.listViewOptions.Dock = DockStyle.Fill;
      this.listViewOptions.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.listViewOptions.HeaderHeight = 22;
      this.listViewOptions.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewOptions.Location = new Point(0, 26);
      this.listViewOptions.Name = "listViewOptions";
      this.listViewOptions.Size = new Size(773, 415);
      this.listViewOptions.SortingType = SortingType.AlphaNumeric;
      this.listViewOptions.TabIndex = 0;
      this.listViewOptions.SelectedIndexChanged += new EventHandler(this.listViewOptions_SelectedIndexChanged);
      this.gradientPanel2.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gradientPanel2.Controls.Add((Control) this.lblSubTitle);
      this.gradientPanel2.Dock = DockStyle.Top;
      this.gradientPanel2.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel2.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel2.GradientPaddingColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      this.gradientPanel2.Location = new Point(0, 0);
      this.gradientPanel2.Name = "gradientPanel2";
      this.gradientPanel2.Size = new Size(774, 31);
      this.gradientPanel2.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gradientPanel2.TabIndex = 7;
      this.lblSubTitle.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lblSubTitle.AutoEllipsis = true;
      this.lblSubTitle.BackColor = Color.Transparent;
      this.lblSubTitle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblSubTitle.Location = new Point(12, 9);
      this.lblSubTitle.Name = "lblSubTitle";
      this.lblSubTitle.Size = new Size(751, 14);
      this.lblSubTitle.TabIndex = 0;
      this.lblSubTitle.Text = "Select a Data Table for defining values to fee scenario and field scenario rules.";
      this.lblSubTitle.TextAlign = ContentAlignment.MiddleLeft;
      this.btnSelect.DialogResult = DialogResult.OK;
      this.btnSelect.Enabled = false;
      this.btnSelect.Location = new Point(600, 487);
      this.btnSelect.Name = "btnSelect";
      this.btnSelect.Size = new Size(75, 23);
      this.btnSelect.TabIndex = 8;
      this.btnSelect.Text = "Select";
      this.btnSelect.UseVisualStyleBackColor = true;
      this.btnSelect.Click += new EventHandler(this.btnSelect_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(694, 487);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 9;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(7f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(774, 522);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnSelect);
      this.Controls.Add((Control) this.gradientPanel2);
      this.Controls.Add((Control) this.gcBaseRate);
      this.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.Margin = new Padding(2);
      this.MaximizeBox = false;
      this.MaximumSize = new Size(790, 561);
      this.MinimizeBox = false;
      this.MinimumSize = new Size(790, 561);
      this.Name = nameof (DataTableList);
      this.ShowIcon = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "List of Tables";
      this.gcBaseRate.ResumeLayout(false);
      this.gradientPanel2.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
