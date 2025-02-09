// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ConcurrentEditing.MergeResultForm
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.UI;
using EllieMae.Encompass.Xml3WayMerge.Utils;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ConcurrentEditing
{
  public class MergeResultForm : Form
  {
    private static List<string> importantFields = new List<string>((IEnumerable<string>) new string[21]
    {
      "736",
      "912",
      "740",
      "742",
      "VASUMM.X23",
      "11",
      "15",
      "1821",
      "1811",
      "1881",
      "2",
      "3",
      "1172",
      "608",
      "763",
      "976",
      "5",
      "799",
      "762",
      "142",
      "MS.STATUS"
    });
    private string baseLoanDataXml = "";
    private string firstLoanDataXml = "";
    private string secondLoanDataXml = "";
    private IContainer components;
    private Button btnAcceptAndSave;
    private Button btnCancel;
    private BorderPanel borderPanel1;
    private TabControl tabControl1;
    private TabPage tpCurrentLoan;
    private TabPage tpPiggybackLoan;
    private GroupContainer gcCLReceivedChanges;
    private GroupContainer groupContainer2;
    private GridView gvCLOverwriteChanges;
    private GridView gvCLReceivedChanges;
    private Panel pnlCurrentLoan;
    private Panel panel3;
    private Panel pnlOverwrite;
    private Panel panel2;
    private Panel pnlPiggyImportant;
    private GroupContainer groupContainer3;
    private GridView gvPLOverwriteChanges;
    private GroupContainer gcPLOthers;
    private GridView gvPLReceivedChanges;
    private EllieMae.EMLite.UI.LinkLabel llPiggyConflict;
    private Label lblSubTitle;
    private Panel pnlBase;
    private Label lblNoConflicts;
    private Label lblNoOthers;
    private Label lblNoPiggyConflicts;
    private Label lblNoPiggyOthers;
    private FormattedLabel formattedLabel1;
    private FormattedLabel formattedLabel2;
    private FormattedLabel formattedLabel3;
    private FormattedLabel formattedLabel4;
    private CollapsibleSplitter splitCurrentLoan;
    private EllieMae.EMLite.UI.LinkLabel lblViewYours;
    private TabControl tcOthers;
    private TabPage tabPage1;
    private TabPage tabPage2;
    private GridView gvAllOthers;
    private TabControl tcPL;
    private TabPage tabPage3;
    private TabPage tabPage4;
    private GridView gvPLAllOther;
    private CollapsibleSplitter splitterPL;

    public MergeResultForm(
      List<MergedObject> primaryLoanReport,
      List<MergedObject> piggyBackLoanReport,
      Hashtable hiddenFields,
      bool save,
      bool defaultShowOthers,
      string baseLoanDataXml,
      string firstLoanDataXml,
      string secondLoanDataXml,
      CEMergeResultsOption mergeResultsDisplayOption)
    {
      this.InitializeComponent();
      this.baseLoanDataXml = baseLoanDataXml;
      this.firstLoanDataXml = firstLoanDataXml;
      this.secondLoanDataXml = secondLoanDataXml;
      int num = 0;
      switch (mergeResultsDisplayOption)
      {
        case CEMergeResultsOption.ConflictsOnly:
          num = 1;
          defaultShowOthers = false;
          break;
        case CEMergeResultsOption.ConflictsAndChangesByOthers:
          num = 2;
          break;
        case CEMergeResultsOption.All:
          num = 3;
          break;
      }
      if (!defaultShowOthers)
      {
        this.gcCLReceivedChanges.Visible = false;
        this.splitCurrentLoan.Visible = false;
        this.gcPLOthers.Visible = false;
        this.splitterPL.Visible = false;
      }
      using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Ellie Mae\\Encompass\\ConcurrentEditing"))
      {
        if (registryKey != null)
        {
          object obj = registryKey.GetValue("Display");
          if (obj != null)
            num = EllieMae.EMLite.Common.Utils.ParseInt((object) string.Concat(obj), 1);
        }
      }
      if (num == 3)
        this.lblViewYours.Visible = true;
      else
        this.lblViewYours.Visible = false;
      if (save)
      {
        this.Text = "Save";
        this.btnAcceptAndSave.Text = "Save";
        this.lblSubTitle.Text = "Your changes are about to be saved.  Changes made by other users will be handled as follows:";
      }
      else
      {
        this.Text = "Get Updates";
        this.btnAcceptAndSave.Text = "Get Updates";
        this.lblSubTitle.Text = "You are about to receive the latest changes made by other users.  Changes made by other users will be handled as follows:";
      }
      List<GVItem> gvItemList1 = new List<GVItem>();
      List<GVItem> gvItemList2 = new List<GVItem>();
      List<GVItem> gvItemList3 = new List<GVItem>();
      List<GVItem> gvItemList4 = new List<GVItem>();
      List<GVItem> gvItemList5 = new List<GVItem>();
      List<GVItem> gvItemList6 = new List<GVItem>();
      List<GVItem> gvItemList7 = new List<GVItem>();
      foreach (MergedObject mergedObject in primaryLoanReport)
      {
        gvItemList4.Clear();
        gvItemList5.Clear();
        gvItemList3.Clear();
        gvItemList7.Clear();
        gvItemList1.Clear();
        foreach (MergedField mergedField in mergedObject.MergedFields)
        {
          if (mergedField.ForcedChange == ChangeOP.NA || mergedField.ForcedChange == mergedField.ChangeForSecond)
          {
            Label label = new Label();
            label.Font = new Font(label.Font, FontStyle.Bold);
            switch (mergedField.ChangeForSecond)
            {
              case ChangeOP.NoChange:
                string str1 = this.enforceBusinessRule(hiddenFields, mergedField.FieldID, mergedField.BaseValue);
                label.Text = mergedField.SecondValue;
                if (mergedField.BaseValue != mergedField.FirstValue && mergedField.FirstValue != mergedField.SecondValue)
                {
                  string str2 = this.enforceBusinessRule(hiddenFields, mergedField.FieldID, mergedField.FirstValue);
                  GVItem gvItem;
                  if (mergedField.FieldID != "")
                    gvItem = new GVItem(new string[4]
                    {
                      mergedField.FieldID,
                      mergedField.Description,
                      str1,
                      str2
                    });
                  else
                    gvItem = new GVItem(new string[4]
                    {
                      mergedField.FieldID,
                      mergedObject.DisplayTitle + "- " + mergedField.Description,
                      str1,
                      str2
                    });
                  gvItem.SubItems.Add((object) label);
                  gvItemList5.Add(gvItem);
                  gvItemList3.Add(gvItem);
                  continue;
                }
                if (mergedField.BaseValue != mergedField.SecondValue)
                {
                  label.Text = mergedField.SecondValue;
                  GVItem gvItem;
                  if (mergedField.FieldID != "")
                    gvItem = new GVItem(new string[4]
                    {
                      mergedField.FieldID,
                      mergedField.Description,
                      mergedField.BaseValue,
                      mergedField.FirstValue
                    });
                  else
                    gvItem = new GVItem(new string[4]
                    {
                      mergedField.FieldID,
                      mergedObject.DisplayTitle + "- " + mergedField.Description,
                      mergedField.BaseValue,
                      mergedField.FirstValue
                    });
                  gvItem.SubItems.Add((object) label);
                  gvItemList7.Add(gvItem);
                  continue;
                }
                continue;
              case ChangeOP.Insert:
              case ChangeOP.Delete:
              case ChangeOP.Update:
                string str3 = this.enforceBusinessRule(hiddenFields, mergedField.FieldID, mergedField.BaseValue);
                string str4 = this.enforceBusinessRule(hiddenFields, mergedField.FieldID, mergedField.FirstValue);
                label.Text = str4;
                GVItem gvItem1;
                if (mergedField.FieldID != "")
                  gvItem1 = new GVItem(new string[3]
                  {
                    mergedField.FieldID,
                    mergedField.Description,
                    str3
                  });
                else
                  gvItem1 = new GVItem(new string[3]
                  {
                    mergedField.FieldID,
                    mergedObject.DisplayTitle + "- " + mergedField.Description,
                    str3
                  });
                gvItem1.SubItems.Add((object) label);
                if (MergeResultForm.importantFields.Contains(mergedField.FieldID))
                  gvItemList4.Add(gvItem1);
                else if (mergedField.FieldID == "")
                {
                  string description = mergedField.Description;
                  if (description == "Milestone Template Name" || description == "Milestone List")
                    gvItemList4.Add(gvItem1);
                }
                gvItemList1.Add(gvItem1);
                continue;
              default:
                continue;
            }
          }
        }
        if (gvItemList4.Count > 0)
          this.gvCLReceivedChanges.Items.AddRange(gvItemList4.ToArray());
        if (gvItemList5.Count > 0)
          this.gvCLOverwriteChanges.Items.AddRange(gvItemList5.ToArray());
        if (gvItemList1.Count > 0)
          this.gvAllOthers.Items.AddRange(gvItemList1.ToArray());
        if (gvItemList3.Count > 0)
          gvItemList2.AddRange((IEnumerable<GVItem>) gvItemList3.ToArray());
        if (gvItemList7.Count > 0)
          gvItemList6.AddRange((IEnumerable<GVItem>) gvItemList7.ToArray());
      }
      if (gvItemList6.Count > 0)
      {
        this.lblViewYours.Tag = (object) gvItemList6;
        this.lblViewYours.Enabled = true;
      }
      else
        this.lblViewYours.Enabled = false;
      if (piggyBackLoanReport == null)
      {
        this.tabControl1.Visible = false;
        this.pnlCurrentLoan.Dock = DockStyle.Fill;
        this.pnlBase.Controls.Add((Control) this.pnlCurrentLoan);
      }
      else
      {
        gvItemList1.Clear();
        gvItemList2.Clear();
        foreach (MergedObject mergedObject in piggyBackLoanReport)
        {
          gvItemList4.Clear();
          gvItemList5.Clear();
          gvItemList3.Clear();
          gvItemList1.Clear();
          foreach (MergedField mergedField in mergedObject.MergedFields)
          {
            if (mergedField.ForcedChange == ChangeOP.NA || mergedField.ForcedChange == mergedField.ChangeForSecond)
            {
              Label label = new Label();
              label.Font = new Font(label.Font, FontStyle.Bold);
              switch (mergedField.ChangeForSecond)
              {
                case ChangeOP.NoChange:
                  string str5 = this.enforceBusinessRule(hiddenFields, mergedField.FieldID, mergedField.BaseValue);
                  label.Text = mergedField.SecondValue;
                  if (mergedField.BaseValue != mergedField.FirstValue && mergedField.FirstValue != mergedField.SecondValue)
                  {
                    string str6 = this.enforceBusinessRule(hiddenFields, mergedField.FieldID, mergedField.FirstValue);
                    GVItem gvItem;
                    if (mergedField.FieldID != "")
                      gvItem = new GVItem(new string[4]
                      {
                        mergedField.FieldID,
                        mergedField.Description,
                        str5,
                        str6
                      });
                    else
                      gvItem = new GVItem(new string[4]
                      {
                        mergedField.FieldID,
                        mergedObject.DisplayTitle + "- " + mergedField.Description,
                        str5,
                        str6
                      });
                    gvItem.SubItems.Add((object) label);
                    gvItemList5.Add(gvItem);
                    gvItemList3.Add(gvItem);
                    continue;
                  }
                  continue;
                case ChangeOP.Insert:
                case ChangeOP.Delete:
                case ChangeOP.Update:
                  string str7 = this.enforceBusinessRule(hiddenFields, mergedField.FieldID, mergedField.BaseValue);
                  string str8 = this.enforceBusinessRule(hiddenFields, mergedField.FieldID, mergedField.FirstValue);
                  label.Text = str8;
                  GVItem gvItem2;
                  if (mergedField.FieldID != "")
                    gvItem2 = new GVItem(new string[3]
                    {
                      mergedField.FieldID,
                      mergedField.Description,
                      str7
                    });
                  else
                    gvItem2 = new GVItem(new string[3]
                    {
                      mergedField.FieldID,
                      mergedObject.DisplayTitle + "- " + mergedField.Description,
                      str7
                    });
                  gvItem2.SubItems.Add((object) label);
                  if (MergeResultForm.importantFields.Contains(mergedField.FieldID))
                    gvItemList4.Add(gvItem2);
                  gvItemList1.Add(gvItem2);
                  continue;
                default:
                  continue;
              }
            }
          }
          if (gvItemList4.Count > 0)
            this.gvPLReceivedChanges.Items.AddRange(gvItemList4.ToArray());
          if (gvItemList5.Count > 0)
            this.gvPLOverwriteChanges.Items.AddRange(gvItemList5.ToArray());
          if (gvItemList1.Count > 0)
            this.gvPLAllOther.Items.AddRange(gvItemList1.ToArray());
          if (gvItemList3.Count > 0)
            gvItemList2.AddRange((IEnumerable<GVItem>) gvItemList3.ToArray());
        }
        if (gvItemList2.Count > 0 && gvItemList2.Count != this.gvPLOverwriteChanges.Items.Count)
        {
          this.llPiggyConflict.Enabled = true;
          this.llPiggyConflict.Tag = (object) gvItemList2;
        }
        else
          this.llPiggyConflict.Enabled = false;
      }
      if (this.gvCLOverwriteChanges.Items.Count == 0)
        this.lblNoConflicts.Visible = true;
      if (this.gvCLReceivedChanges.Items.Count == 0)
        this.lblNoOthers.Visible = true;
      if (this.gvPLOverwriteChanges.Items.Count == 0)
        this.lblNoPiggyConflicts.Visible = true;
      if (this.gvPLReceivedChanges.Items.Count == 0)
        this.lblNoPiggyOthers.Visible = true;
      this.tcOthers.SelectedIndex = this.gvCLReceivedChanges.Items.Count <= 0 ? 1 : 0;
      if (this.gvPLReceivedChanges.Items.Count > 0)
        this.tcPL.SelectedIndex = 0;
      else
        this.tcPL.SelectedIndex = 1;
    }

    public bool NoConflicts => this.lblNoConflicts.Visible && this.lblNoPiggyConflicts.Visible;

    private string enforceBusinessRule(Hashtable hiddenFields, string fieldID, string defaultValue)
    {
      string str = defaultValue;
      if (fieldID != "" && hiddenFields.Contains((object) fieldID))
        str = "*";
      return str;
    }

    public bool MergeOnly
    {
      set
      {
        if (value)
        {
          this.btnAcceptAndSave.Text = "Get Updates";
          this.Text = "Get Updates";
          this.lblSubTitle.Text = "You are about to receive the latest changes made by other users.  Changes made by other users will be handled as follows:";
        }
        else
        {
          this.btnAcceptAndSave.Text = "Save";
          this.Text = "Save";
          this.lblSubTitle.Text = "Your changes are about to be saved.  Changes made by other users will be handled as follows:";
        }
      }
    }

    public bool HasConflictKeyChanges => this.gvCLOverwriteChanges.Items.Count > 0;

    private void borderPanel1_SizeChanged(object sender, EventArgs e)
    {
      if (!this.splitCurrentLoan.Visible)
        return;
      GridView clReceivedChanges = this.gvCLReceivedChanges;
      GroupContainer gcPlOthers = this.gcPLOthers;
      Size size1 = new Size(this.gvCLOverwriteChanges.Width, (this.borderPanel1.Height - this.splitCurrentLoan.Height) / 2);
      Size size2 = size1;
      gcPlOthers.Size = size2;
      Size size3 = size1;
      clReceivedChanges.Size = size3;
    }

    private void llPiggyConflict_Click(object sender, EventArgs e)
    {
      int num = (int) new MergeResultWholeView((List<GVItem>) this.llPiggyConflict.Tag).ShowDialog((IWin32Window) this);
    }

    private void lblViewYours_MouseClick(object sender, MouseEventArgs e)
    {
      if (e.Button != MouseButtons.Left)
        return;
      int num = (int) new MergeResultWholeView((List<GVItem>) this.lblViewYours.Tag).ShowDialog((IWin32Window) this);
    }

    private void lblViewYours_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      if (e.Button != MouseButtons.Right)
        return;
      int num = (int) new HiddenLoanFileExpoDialog(this.baseLoanDataXml, this.firstLoanDataXml, this.secondLoanDataXml).ShowDialog((IWin32Window) this);
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
      GVColumn gvColumn9 = new GVColumn();
      GVColumn gvColumn10 = new GVColumn();
      GVColumn gvColumn11 = new GVColumn();
      GVColumn gvColumn12 = new GVColumn();
      GVColumn gvColumn13 = new GVColumn();
      GVColumn gvColumn14 = new GVColumn();
      GVColumn gvColumn15 = new GVColumn();
      GVColumn gvColumn16 = new GVColumn();
      GVColumn gvColumn17 = new GVColumn();
      GVColumn gvColumn18 = new GVColumn();
      GVColumn gvColumn19 = new GVColumn();
      GVColumn gvColumn20 = new GVColumn();
      GVColumn gvColumn21 = new GVColumn();
      GVColumn gvColumn22 = new GVColumn();
      GVColumn gvColumn23 = new GVColumn();
      GVColumn gvColumn24 = new GVColumn();
      GVColumn gvColumn25 = new GVColumn();
      GVColumn gvColumn26 = new GVColumn();
      this.btnAcceptAndSave = new Button();
      this.btnCancel = new Button();
      this.borderPanel1 = new BorderPanel();
      this.pnlBase = new Panel();
      this.tabControl1 = new TabControl();
      this.tpCurrentLoan = new TabPage();
      this.pnlCurrentLoan = new Panel();
      this.pnlOverwrite = new Panel();
      this.groupContainer2 = new GroupContainer();
      this.lblViewYours = new EllieMae.EMLite.UI.LinkLabel();
      this.formattedLabel1 = new FormattedLabel();
      this.lblNoConflicts = new Label();
      this.gvCLOverwriteChanges = new GridView();
      this.splitCurrentLoan = new CollapsibleSplitter();
      this.gcCLReceivedChanges = new GroupContainer();
      this.tcOthers = new TabControl();
      this.tabPage1 = new TabPage();
      this.gvCLReceivedChanges = new GridView();
      this.lblNoOthers = new Label();
      this.tabPage2 = new TabPage();
      this.gvAllOthers = new GridView();
      this.formattedLabel2 = new FormattedLabel();
      this.tpPiggybackLoan = new TabPage();
      this.panel2 = new Panel();
      this.pnlPiggyImportant = new Panel();
      this.groupContainer3 = new GroupContainer();
      this.formattedLabel3 = new FormattedLabel();
      this.lblNoPiggyConflicts = new Label();
      this.llPiggyConflict = new EllieMae.EMLite.UI.LinkLabel();
      this.gvPLOverwriteChanges = new GridView();
      this.splitterPL = new CollapsibleSplitter();
      this.gcPLOthers = new GroupContainer();
      this.tcPL = new TabControl();
      this.tabPage3 = new TabPage();
      this.gvPLReceivedChanges = new GridView();
      this.lblNoPiggyOthers = new Label();
      this.tabPage4 = new TabPage();
      this.gvPLAllOther = new GridView();
      this.formattedLabel4 = new FormattedLabel();
      this.lblSubTitle = new Label();
      this.panel3 = new Panel();
      this.borderPanel1.SuspendLayout();
      this.pnlBase.SuspendLayout();
      this.tabControl1.SuspendLayout();
      this.tpCurrentLoan.SuspendLayout();
      this.pnlCurrentLoan.SuspendLayout();
      this.pnlOverwrite.SuspendLayout();
      this.groupContainer2.SuspendLayout();
      this.gcCLReceivedChanges.SuspendLayout();
      this.tcOthers.SuspendLayout();
      this.tabPage1.SuspendLayout();
      this.tabPage2.SuspendLayout();
      this.tpPiggybackLoan.SuspendLayout();
      this.panel2.SuspendLayout();
      this.pnlPiggyImportant.SuspendLayout();
      this.groupContainer3.SuspendLayout();
      this.gcPLOthers.SuspendLayout();
      this.tcPL.SuspendLayout();
      this.tabPage3.SuspendLayout();
      this.tabPage4.SuspendLayout();
      this.panel3.SuspendLayout();
      this.SuspendLayout();
      this.btnAcceptAndSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnAcceptAndSave.DialogResult = DialogResult.OK;
      this.btnAcceptAndSave.Location = new Point(640, 9);
      this.btnAcceptAndSave.Name = "btnAcceptAndSave";
      this.btnAcceptAndSave.Size = new Size(90, 25);
      this.btnAcceptAndSave.TabIndex = 1;
      this.btnAcceptAndSave.Text = "Accept && Save";
      this.btnAcceptAndSave.UseVisualStyleBackColor = true;
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(736, 9);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 25);
      this.btnCancel.TabIndex = 5;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.borderPanel1.Controls.Add((Control) this.pnlBase);
      this.borderPanel1.Controls.Add((Control) this.lblSubTitle);
      this.borderPanel1.Dock = DockStyle.Fill;
      this.borderPanel1.Location = new Point(0, 0);
      this.borderPanel1.Name = "borderPanel1";
      this.borderPanel1.Padding = new Padding(3);
      this.borderPanel1.Size = new Size(821, 668);
      this.borderPanel1.TabIndex = 7;
      this.borderPanel1.SizeChanged += new EventHandler(this.borderPanel1_SizeChanged);
      this.pnlBase.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlBase.Controls.Add((Control) this.tabControl1);
      this.pnlBase.Location = new Point(5, 34);
      this.pnlBase.Name = "pnlBase";
      this.pnlBase.Size = new Size(813, 626);
      this.pnlBase.TabIndex = 2;
      this.tabControl1.Controls.Add((Control) this.tpCurrentLoan);
      this.tabControl1.Controls.Add((Control) this.tpPiggybackLoan);
      this.tabControl1.Dock = DockStyle.Fill;
      this.tabControl1.Location = new Point(0, 0);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new Size(813, 626);
      this.tabControl1.TabIndex = 0;
      this.tpCurrentLoan.Controls.Add((Control) this.pnlCurrentLoan);
      this.tpCurrentLoan.Location = new Point(4, 23);
      this.tpCurrentLoan.Name = "tpCurrentLoan";
      this.tpCurrentLoan.Padding = new Padding(3);
      this.tpCurrentLoan.Size = new Size(805, 599);
      this.tpCurrentLoan.TabIndex = 0;
      this.tpCurrentLoan.Text = "Current Loan";
      this.tpCurrentLoan.UseVisualStyleBackColor = true;
      this.pnlCurrentLoan.Controls.Add((Control) this.pnlOverwrite);
      this.pnlCurrentLoan.Controls.Add((Control) this.splitCurrentLoan);
      this.pnlCurrentLoan.Controls.Add((Control) this.gcCLReceivedChanges);
      this.pnlCurrentLoan.Dock = DockStyle.Fill;
      this.pnlCurrentLoan.Location = new Point(3, 3);
      this.pnlCurrentLoan.Name = "pnlCurrentLoan";
      this.pnlCurrentLoan.Size = new Size(799, 593);
      this.pnlCurrentLoan.TabIndex = 7;
      this.pnlOverwrite.Controls.Add((Control) this.groupContainer2);
      this.pnlOverwrite.Dock = DockStyle.Fill;
      this.pnlOverwrite.Location = new Point(0, 0);
      this.pnlOverwrite.Name = "pnlOverwrite";
      this.pnlOverwrite.Size = new Size(799, 287);
      this.pnlOverwrite.TabIndex = 8;
      this.groupContainer2.Controls.Add((Control) this.lblViewYours);
      this.groupContainer2.Controls.Add((Control) this.formattedLabel1);
      this.groupContainer2.Controls.Add((Control) this.lblNoConflicts);
      this.groupContainer2.Controls.Add((Control) this.gvCLOverwriteChanges);
      this.groupContainer2.Dock = DockStyle.Fill;
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(0, 0);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(799, 287);
      this.groupContainer2.TabIndex = 0;
      this.lblViewYours.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lblViewYours.AutoSize = true;
      this.lblViewYours.BackColor = System.Drawing.Color.Transparent;
      this.lblViewYours.Location = new Point(681, 6);
      this.lblViewYours.Name = "lblViewYours";
      this.lblViewYours.Size = new Size(121, 14);
      this.lblViewYours.TabIndex = 3;
      this.lblViewYours.Text = "View All Your Changes";
      this.lblViewYours.Visible = false;
      this.lblViewYours.MouseDoubleClick += new MouseEventHandler(this.lblViewYours_MouseDoubleClick);
      this.lblViewYours.MouseClick += new MouseEventHandler(this.lblViewYours_MouseClick);
      this.formattedLabel1.BackColor = System.Drawing.Color.Transparent;
      this.formattedLabel1.Font = new Font("Arial", 8f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.formattedLabel1.Location = new Point(5, 6);
      this.formattedLabel1.Name = "formattedLabel1";
      this.formattedLabel1.Size = new Size(428, 16);
      this.formattedLabel1.TabIndex = 3;
      this.formattedLabel1.Text = "<b><c value=\"Red\">Conflicting Changes</c></b> - Your changes will overwrite the changes made by other users.";
      this.lblNoConflicts.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lblNoConflicts.AutoSize = true;
      this.lblNoConflicts.BackColor = System.Drawing.Color.Transparent;
      this.lblNoConflicts.Location = new Point(191, 138);
      this.lblNoConflicts.Name = "lblNoConflicts";
      this.lblNoConflicts.Size = new Size(356, 14);
      this.lblNoConflicts.TabIndex = 2;
      this.lblNoConflicts.Text = "No conflicting changes were made to any fields by you and other users.";
      this.lblNoConflicts.Visible = false;
      this.gvCLOverwriteChanges.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Field ID";
      gvColumn1.Width = 100;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Field Description";
      gvColumn2.Width = 270;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column6";
      gvColumn3.Text = "Original Value";
      gvColumn3.Width = 130;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column3";
      gvColumn4.Text = "Modified by Other Users";
      gvColumn4.Width = 130;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column4";
      gvColumn5.Text = "Modified by You";
      gvColumn5.Width = 130;
      this.gvCLOverwriteChanges.Columns.AddRange(new GVColumn[5]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5
      });
      this.gvCLOverwriteChanges.Dock = DockStyle.Fill;
      this.gvCLOverwriteChanges.Location = new Point(1, 26);
      this.gvCLOverwriteChanges.Name = "gvCLOverwriteChanges";
      this.gvCLOverwriteChanges.Size = new Size(797, 260);
      this.gvCLOverwriteChanges.TabIndex = 0;
      this.splitCurrentLoan.AnimationDelay = 20;
      this.splitCurrentLoan.AnimationStep = 20;
      this.splitCurrentLoan.BorderStyle3D = Border3DStyle.Flat;
      this.splitCurrentLoan.ControlToHide = (Control) this.gcCLReceivedChanges;
      this.splitCurrentLoan.Dock = DockStyle.Bottom;
      this.splitCurrentLoan.ExpandParentForm = false;
      this.splitCurrentLoan.Location = new Point(0, 287);
      this.splitCurrentLoan.Name = "splitCurrentLoan";
      this.splitCurrentLoan.TabIndex = 9;
      this.splitCurrentLoan.TabStop = false;
      this.splitCurrentLoan.UseAnimations = false;
      this.splitCurrentLoan.VisualStyle = VisualStyles.Encompass;
      this.gcCLReceivedChanges.Controls.Add((Control) this.tcOthers);
      this.gcCLReceivedChanges.Controls.Add((Control) this.formattedLabel2);
      this.gcCLReceivedChanges.Dock = DockStyle.Bottom;
      this.gcCLReceivedChanges.HeaderForeColor = SystemColors.ControlText;
      this.gcCLReceivedChanges.Location = new Point(0, 294);
      this.gcCLReceivedChanges.Name = "gcCLReceivedChanges";
      this.gcCLReceivedChanges.Padding = new Padding(3, 0, 0, 0);
      this.gcCLReceivedChanges.Size = new Size(799, 299);
      this.gcCLReceivedChanges.TabIndex = 5;
      this.tcOthers.Controls.Add((Control) this.tabPage1);
      this.tcOthers.Controls.Add((Control) this.tabPage2);
      this.tcOthers.Dock = DockStyle.Fill;
      this.tcOthers.Location = new Point(4, 26);
      this.tcOthers.Name = "tcOthers";
      this.tcOthers.SelectedIndex = 0;
      this.tcOthers.Size = new Size(794, 272);
      this.tcOthers.TabIndex = 5;
      this.tabPage1.Controls.Add((Control) this.gvCLReceivedChanges);
      this.tabPage1.Controls.Add((Control) this.lblNoOthers);
      this.tabPage1.Location = new Point(4, 23);
      this.tabPage1.Name = "tabPage1";
      this.tabPage1.Padding = new Padding(3);
      this.tabPage1.Size = new Size(786, 245);
      this.tabPage1.TabIndex = 0;
      this.tabPage1.Text = "Key Fields";
      this.tabPage1.UseVisualStyleBackColor = true;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column1";
      gvColumn6.Text = "Field ID";
      gvColumn6.Width = 100;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Column2";
      gvColumn7.Text = "Field Description";
      gvColumn7.Width = 400;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "Column5";
      gvColumn8.Text = "Original Value";
      gvColumn8.Width = 130;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "Column3";
      gvColumn9.Text = "Modified by Other Users";
      gvColumn9.Width = 130;
      this.gvCLReceivedChanges.Columns.AddRange(new GVColumn[4]
      {
        gvColumn6,
        gvColumn7,
        gvColumn8,
        gvColumn9
      });
      this.gvCLReceivedChanges.Dock = DockStyle.Fill;
      this.gvCLReceivedChanges.Location = new Point(3, 3);
      this.gvCLReceivedChanges.Name = "gvCLReceivedChanges";
      this.gvCLReceivedChanges.Size = new Size(780, 239);
      this.gvCLReceivedChanges.TabIndex = 0;
      this.lblNoOthers.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lblNoOthers.AutoSize = true;
      this.lblNoOthers.BackColor = System.Drawing.Color.Transparent;
      this.lblNoOthers.Location = new Point(290, 108);
      this.lblNoOthers.Name = "lblNoOthers";
      this.lblNoOthers.Size = new Size(282, 14);
      this.lblNoOthers.TabIndex = 3;
      this.lblNoOthers.Text = "No changes were made to any key fields by other users.";
      this.lblNoOthers.Visible = false;
      this.tabPage2.Controls.Add((Control) this.gvAllOthers);
      this.tabPage2.Location = new Point(4, 23);
      this.tabPage2.Name = "tabPage2";
      this.tabPage2.Padding = new Padding(3);
      this.tabPage2.Size = new Size(786, 245);
      this.tabPage2.TabIndex = 1;
      this.tabPage2.Text = "All Fields";
      this.tabPage2.UseVisualStyleBackColor = true;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "Column1";
      gvColumn10.Text = "Field ID";
      gvColumn10.Width = 100;
      gvColumn11.ImageIndex = -1;
      gvColumn11.Name = "Column2";
      gvColumn11.Text = "Field Description";
      gvColumn11.Width = 350;
      gvColumn12.ImageIndex = -1;
      gvColumn12.Name = "Column6";
      gvColumn12.Text = "Original Value";
      gvColumn12.Width = 130;
      gvColumn13.ImageIndex = -1;
      gvColumn13.Name = "Column3";
      gvColumn13.Text = "Modified by Other Users";
      gvColumn13.Width = 130;
      this.gvAllOthers.Columns.AddRange(new GVColumn[4]
      {
        gvColumn10,
        gvColumn11,
        gvColumn12,
        gvColumn13
      });
      this.gvAllOthers.Dock = DockStyle.Fill;
      this.gvAllOthers.Location = new Point(3, 3);
      this.gvAllOthers.Name = "gvAllOthers";
      this.gvAllOthers.Size = new Size(780, 239);
      this.gvAllOthers.TabIndex = 1;
      this.formattedLabel2.BackColor = System.Drawing.Color.Transparent;
      this.formattedLabel2.Location = new Point(5, 6);
      this.formattedLabel2.Name = "formattedLabel2";
      this.formattedLabel2.Size = new Size(469, 16);
      this.formattedLabel2.TabIndex = 4;
      this.formattedLabel2.Text = "<b>Changes Made by Other Users</b> - These changes will be added to your copy of this loan file.";
      this.tpPiggybackLoan.Controls.Add((Control) this.panel2);
      this.tpPiggybackLoan.Location = new Point(4, 23);
      this.tpPiggybackLoan.Name = "tpPiggybackLoan";
      this.tpPiggybackLoan.Padding = new Padding(3);
      this.tpPiggybackLoan.Size = new Size(805, 599);
      this.tpPiggybackLoan.TabIndex = 1;
      this.tpPiggybackLoan.Text = "Piggyback Loan";
      this.tpPiggybackLoan.UseVisualStyleBackColor = true;
      this.panel2.Controls.Add((Control) this.pnlPiggyImportant);
      this.panel2.Controls.Add((Control) this.splitterPL);
      this.panel2.Controls.Add((Control) this.gcPLOthers);
      this.panel2.Dock = DockStyle.Fill;
      this.panel2.Location = new Point(3, 3);
      this.panel2.Name = "panel2";
      this.panel2.Padding = new Padding(3);
      this.panel2.Size = new Size(799, 593);
      this.panel2.TabIndex = 9;
      this.pnlPiggyImportant.Controls.Add((Control) this.groupContainer3);
      this.pnlPiggyImportant.Dock = DockStyle.Fill;
      this.pnlPiggyImportant.Location = new Point(3, 3);
      this.pnlPiggyImportant.Name = "pnlPiggyImportant";
      this.pnlPiggyImportant.Size = new Size(793, 274);
      this.pnlPiggyImportant.TabIndex = 10;
      this.groupContainer3.Controls.Add((Control) this.formattedLabel3);
      this.groupContainer3.Controls.Add((Control) this.lblNoPiggyConflicts);
      this.groupContainer3.Controls.Add((Control) this.llPiggyConflict);
      this.groupContainer3.Controls.Add((Control) this.gvPLOverwriteChanges);
      this.groupContainer3.Dock = DockStyle.Fill;
      this.groupContainer3.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer3.Location = new Point(0, 0);
      this.groupContainer3.Name = "groupContainer3";
      this.groupContainer3.Size = new Size(793, 274);
      this.groupContainer3.TabIndex = 0;
      this.formattedLabel3.BackColor = System.Drawing.Color.Transparent;
      this.formattedLabel3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.formattedLabel3.Location = new Point(4, 6);
      this.formattedLabel3.Name = "formattedLabel3";
      this.formattedLabel3.Size = new Size(436, 16);
      this.formattedLabel3.TabIndex = 4;
      this.formattedLabel3.Text = "<b><c value=\"Red\">Conflicting Changes</c></b> - Your changes will overwrite the changes made by other users.";
      this.lblNoPiggyConflicts.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lblNoPiggyConflicts.AutoSize = true;
      this.lblNoPiggyConflicts.BackColor = System.Drawing.Color.Transparent;
      this.lblNoPiggyConflicts.Location = new Point(191, 111);
      this.lblNoPiggyConflicts.Name = "lblNoPiggyConflicts";
      this.lblNoPiggyConflicts.Size = new Size(356, 14);
      this.lblNoPiggyConflicts.TabIndex = 3;
      this.lblNoPiggyConflicts.Text = "No conflicting changes were made to any fields by you and other users.";
      this.lblNoPiggyConflicts.Visible = false;
      this.llPiggyConflict.AutoSize = true;
      this.llPiggyConflict.BackColor = System.Drawing.Color.Transparent;
      this.llPiggyConflict.Location = new Point(702, 6);
      this.llPiggyConflict.Name = "llPiggyConflict";
      this.llPiggyConflict.Size = new Size(93, 14);
      this.llPiggyConflict.TabIndex = 2;
      this.llPiggyConflict.Text = "View All Conflicts";
      this.llPiggyConflict.Visible = false;
      this.llPiggyConflict.Click += new EventHandler(this.llPiggyConflict_Click);
      this.gvPLOverwriteChanges.BorderStyle = BorderStyle.None;
      gvColumn14.ImageIndex = -1;
      gvColumn14.Name = "Column1";
      gvColumn14.Text = "Field ID";
      gvColumn14.Width = 100;
      gvColumn15.ImageIndex = -1;
      gvColumn15.Name = "Column2";
      gvColumn15.Text = "Field Description";
      gvColumn15.Width = 270;
      gvColumn16.ImageIndex = -1;
      gvColumn16.Name = "Column6";
      gvColumn16.Text = "Original Value";
      gvColumn16.Width = 130;
      gvColumn17.ImageIndex = -1;
      gvColumn17.Name = "Column3";
      gvColumn17.Text = "Modified by Other Users";
      gvColumn17.Width = 130;
      gvColumn18.ImageIndex = -1;
      gvColumn18.Name = "Column4";
      gvColumn18.Text = "Modified by You";
      gvColumn18.Width = 130;
      this.gvPLOverwriteChanges.Columns.AddRange(new GVColumn[5]
      {
        gvColumn14,
        gvColumn15,
        gvColumn16,
        gvColumn17,
        gvColumn18
      });
      this.gvPLOverwriteChanges.Dock = DockStyle.Fill;
      this.gvPLOverwriteChanges.Location = new Point(1, 26);
      this.gvPLOverwriteChanges.Name = "gvPLOverwriteChanges";
      this.gvPLOverwriteChanges.Size = new Size(791, 247);
      this.gvPLOverwriteChanges.TabIndex = 0;
      this.splitterPL.AnimationDelay = 20;
      this.splitterPL.AnimationStep = 20;
      this.splitterPL.BorderStyle3D = Border3DStyle.Flat;
      this.splitterPL.ControlToHide = (Control) this.gcPLOthers;
      this.splitterPL.Dock = DockStyle.Bottom;
      this.splitterPL.ExpandParentForm = false;
      this.splitterPL.Location = new Point(3, 277);
      this.splitterPL.Name = "splitCurrentLoan";
      this.splitterPL.TabIndex = 11;
      this.splitterPL.TabStop = false;
      this.splitterPL.UseAnimations = false;
      this.splitterPL.VisualStyle = VisualStyles.Encompass;
      this.gcPLOthers.Controls.Add((Control) this.tcPL);
      this.gcPLOthers.Controls.Add((Control) this.formattedLabel4);
      this.gcPLOthers.Dock = DockStyle.Bottom;
      this.gcPLOthers.HeaderForeColor = SystemColors.ControlText;
      this.gcPLOthers.Location = new Point(3, 280);
      this.gcPLOthers.Name = "gcPLOthers";
      this.gcPLOthers.Padding = new Padding(3, 0, 0, 0);
      this.gcPLOthers.Size = new Size(793, 310);
      this.gcPLOthers.TabIndex = 7;
      this.tcPL.Controls.Add((Control) this.tabPage3);
      this.tcPL.Controls.Add((Control) this.tabPage4);
      this.tcPL.Dock = DockStyle.Fill;
      this.tcPL.Location = new Point(4, 26);
      this.tcPL.Name = "tcPL";
      this.tcPL.SelectedIndex = 0;
      this.tcPL.Size = new Size(788, 283);
      this.tcPL.TabIndex = 6;
      this.tabPage3.Controls.Add((Control) this.gvPLReceivedChanges);
      this.tabPage3.Controls.Add((Control) this.lblNoPiggyOthers);
      this.tabPage3.Location = new Point(4, 23);
      this.tabPage3.Name = "tabPage3";
      this.tabPage3.Padding = new Padding(3);
      this.tabPage3.Size = new Size(780, 256);
      this.tabPage3.TabIndex = 0;
      this.tabPage3.Text = "Key Fields";
      this.tabPage3.UseVisualStyleBackColor = true;
      gvColumn19.ImageIndex = -1;
      gvColumn19.Name = "Column1";
      gvColumn19.Text = "Field ID";
      gvColumn19.Width = 100;
      gvColumn20.ImageIndex = -1;
      gvColumn20.Name = "Column2";
      gvColumn20.Text = "Field Description";
      gvColumn20.Width = 400;
      gvColumn21.ImageIndex = -1;
      gvColumn21.Name = "Column5";
      gvColumn21.Text = "Original Value";
      gvColumn21.Width = 130;
      gvColumn22.ImageIndex = -1;
      gvColumn22.Name = "Column3";
      gvColumn22.Text = "Modified by Other Users";
      gvColumn22.Width = 130;
      this.gvPLReceivedChanges.Columns.AddRange(new GVColumn[4]
      {
        gvColumn19,
        gvColumn20,
        gvColumn21,
        gvColumn22
      });
      this.gvPLReceivedChanges.Dock = DockStyle.Fill;
      this.gvPLReceivedChanges.Location = new Point(3, 3);
      this.gvPLReceivedChanges.Name = "gvPLReceivedChanges";
      this.gvPLReceivedChanges.Size = new Size(774, 250);
      this.gvPLReceivedChanges.TabIndex = 1;
      this.lblNoPiggyOthers.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lblNoPiggyOthers.AutoSize = true;
      this.lblNoPiggyOthers.BackColor = System.Drawing.Color.Transparent;
      this.lblNoPiggyOthers.Location = new Point(248, 107);
      this.lblNoPiggyOthers.Name = "lblNoPiggyOthers";
      this.lblNoPiggyOthers.Size = new Size(282, 14);
      this.lblNoPiggyOthers.TabIndex = 4;
      this.lblNoPiggyOthers.Text = "No changes were made to any key fields by other users.";
      this.lblNoPiggyOthers.Visible = false;
      this.tabPage4.Controls.Add((Control) this.gvPLAllOther);
      this.tabPage4.Location = new Point(4, 23);
      this.tabPage4.Name = "tabPage4";
      this.tabPage4.Padding = new Padding(3);
      this.tabPage4.Size = new Size(780, 256);
      this.tabPage4.TabIndex = 1;
      this.tabPage4.Text = "All Fields";
      this.tabPage4.UseVisualStyleBackColor = true;
      gvColumn23.ImageIndex = -1;
      gvColumn23.Name = "Column1";
      gvColumn23.Text = "Field ID";
      gvColumn23.Width = 100;
      gvColumn24.ImageIndex = -1;
      gvColumn24.Name = "Column2";
      gvColumn24.Text = "Field Description";
      gvColumn24.Width = 350;
      gvColumn25.ImageIndex = -1;
      gvColumn25.Name = "Column6";
      gvColumn25.Text = "Original Value";
      gvColumn25.Width = 130;
      gvColumn26.ImageIndex = -1;
      gvColumn26.Name = "Column3";
      gvColumn26.Text = "Modified by Other Users";
      gvColumn26.Width = 130;
      this.gvPLAllOther.Columns.AddRange(new GVColumn[4]
      {
        gvColumn23,
        gvColumn24,
        gvColumn25,
        gvColumn26
      });
      this.gvPLAllOther.Dock = DockStyle.Fill;
      this.gvPLAllOther.Location = new Point(3, 3);
      this.gvPLAllOther.Name = "gvPLAllOther";
      this.gvPLAllOther.Size = new Size(774, 250);
      this.gvPLAllOther.TabIndex = 2;
      this.formattedLabel4.BackColor = System.Drawing.Color.Transparent;
      this.formattedLabel4.Location = new Point(4, 8);
      this.formattedLabel4.Name = "formattedLabel4";
      this.formattedLabel4.Size = new Size(493, 16);
      this.formattedLabel4.TabIndex = 5;
      this.formattedLabel4.Text = "<b>Key Changes Made by Other Users</b> - These changes will be added to your copy of this loan file.";
      this.lblSubTitle.AutoSize = true;
      this.lblSubTitle.Location = new Point(10, 10);
      this.lblSubTitle.Name = "lblSubTitle";
      this.lblSubTitle.Size = new Size(609, 14);
      this.lblSubTitle.TabIndex = 1;
      this.lblSubTitle.Text = "You are about to receive the latest changes made by other users.  Changes made by other users will be handled as follows:";
      this.panel3.Controls.Add((Control) this.btnAcceptAndSave);
      this.panel3.Controls.Add((Control) this.btnCancel);
      this.panel3.Dock = DockStyle.Bottom;
      this.panel3.Location = new Point(0, 668);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(821, 40);
      this.panel3.TabIndex = 8;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.WhiteSmoke;
      this.ClientSize = new Size(821, 708);
      this.Controls.Add((Control) this.borderPanel1);
      this.Controls.Add((Control) this.panel3);
      this.Font = new Font("Arial", 8f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (MergeResultForm);
      this.ShowIcon = false;
      this.SizeGripStyle = SizeGripStyle.Hide;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Merge Result";
      this.borderPanel1.ResumeLayout(false);
      this.borderPanel1.PerformLayout();
      this.pnlBase.ResumeLayout(false);
      this.tabControl1.ResumeLayout(false);
      this.tpCurrentLoan.ResumeLayout(false);
      this.pnlCurrentLoan.ResumeLayout(false);
      this.pnlOverwrite.ResumeLayout(false);
      this.groupContainer2.ResumeLayout(false);
      this.groupContainer2.PerformLayout();
      this.gcCLReceivedChanges.ResumeLayout(false);
      this.gcCLReceivedChanges.PerformLayout();
      this.tcOthers.ResumeLayout(false);
      this.tabPage1.ResumeLayout(false);
      this.tabPage1.PerformLayout();
      this.tabPage2.ResumeLayout(false);
      this.tpPiggybackLoan.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.pnlPiggyImportant.ResumeLayout(false);
      this.groupContainer3.ResumeLayout(false);
      this.groupContainer3.PerformLayout();
      this.gcPLOthers.ResumeLayout(false);
      this.gcPLOthers.PerformLayout();
      this.tcPL.ResumeLayout(false);
      this.tabPage3.ResumeLayout(false);
      this.tabPage3.PerformLayout();
      this.tabPage4.ResumeLayout(false);
      this.panel3.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
