// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.FieldAccessDlg
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class FieldAccessDlg : Form
  {
    private string lastSearch = "";
    private ReportFieldDefs fieldDefs;
    private Dictionary<string, AclTriState> accessColList;
    private Dictionary<string, AclTriState> accessColList_User;
    private Dictionary<string, AclTriState> accessColList_Personas;
    private Dictionary<string, AclTriState> oriAccessColList;
    private PersonaPipelineView pipelineView;
    private List<ReportFieldDef> tmpList = new List<ReportFieldDef>();
    private IContainer components;
    private GridView gvFields;
    private Button btnFind;
    private TextBox txtFind;
    private ContextMenuStrip cmsLink;
    private ToolStripMenuItem tsmiLink;
    private ToolStripMenuItem tsmiDisconnect;
    private Panel panel1;
    private Button btnAdd;
    private Button btnCancel;
    private Button btnOk;

    public FieldAccessDlg(
      ReportFieldDefs fieldDefs,
      PersonaPipelineView pipelineView,
      Dictionary<string, AclTriState> accessColList)
    {
      this.InitializeComponent();
      this.fieldDefs = fieldDefs;
      this.pipelineView = pipelineView;
      this.gvFields.ContextMenuStrip = (ContextMenuStrip) null;
      this.loadFieldList(fieldDefs, pipelineView, false, true, accessColList);
      this.gvFields.Sort(new GVColumnSort[2]
      {
        new GVColumnSort(0, SortOrder.Ascending),
        new GVColumnSort(1, SortOrder.Ascending)
      });
    }

    public FieldAccessDlg(ReportFieldDefs fieldDefs, Dictionary<string, AclTriState> accessColList)
    {
      this.InitializeComponent();
      this.fieldDefs = fieldDefs;
      this.accessColList = accessColList;
      this.oriAccessColList = new Dictionary<string, AclTriState>();
      foreach (string key in accessColList.Keys)
        this.oriAccessColList.Add(key, accessColList[key]);
      if (!(fieldDefs is LoanReportFieldDefs))
        this.gvFields.Columns.Remove(this.gvFields.Columns.GetColumnByName("colPair"));
      this.gvFields.ContextMenuStrip = (ContextMenuStrip) null;
      this.loadFieldList(fieldDefs, false, true, this.accessColList);
      this.gvFields.Sort(new GVColumnSort[2]
      {
        new GVColumnSort(0, SortOrder.Ascending),
        new GVColumnSort(1, SortOrder.Ascending)
      });
    }

    public FieldAccessDlg(
      ReportFieldDefs fieldDefs,
      Dictionary<string, AclTriState> accessColList_User,
      Dictionary<string, AclTriState> accessColList_Personas,
      bool readOnly)
    {
      this.InitializeComponent();
      this.fieldDefs = fieldDefs;
      this.accessColList_User = accessColList_User;
      this.oriAccessColList = new Dictionary<string, AclTriState>();
      foreach (string key in accessColList_User.Keys)
        this.oriAccessColList.Add(key, accessColList_User[key]);
      this.accessColList_Personas = accessColList_Personas;
      if (!(fieldDefs is LoanReportFieldDefs))
        this.gvFields.Columns.Remove(this.gvFields.Columns.GetColumnByName("colPair"));
      this.loadFieldList(fieldDefs, false, true, this.accessColList_User, this.accessColList_Personas);
      this.gvFields.Sort(new GVColumnSort[2]
      {
        new GVColumnSort(0, SortOrder.Ascending),
        new GVColumnSort(1, SortOrder.Ascending)
      });
      if (!readOnly)
        return;
      this.btnOk.Enabled = false;
      this.cmsLink.Enabled = false;
    }

    public Dictionary<string, AclTriState> AccessColumnList => this.accessColList;

    public Dictionary<string, AclTriState> AccessColumnList_User => this.accessColList_User;

    public PersonaPipelineView PipelineView => this.pipelineView;

    private void loadFieldList(
      ReportFieldDefs fieldDefs,
      bool dbFieldsOnly,
      bool allowVirtualFields,
      Dictionary<string, AclTriState> accessColList)
    {
      this.gvFields.Items.Clear();
      foreach (ReportFieldDef fieldDef in (ReportFieldDefContainer) fieldDefs)
      {
        if (!(!fieldDef.IsDatabaseField & dbFieldsOnly) && (allowVirtualFields || !(fieldDef.FieldDefinition is VirtualField)) && (!accessColList.ContainsKey(fieldDef.FieldID) || accessColList[fieldDef.FieldID] != AclTriState.False))
        {
          GVItem gvItem = new GVItem(fieldDef.Category);
          gvItem.SubItems[1].Text = fieldDef.Description;
          gvItem.SubItems[2].Value = (object) new FieldIDElement(fieldDef.FieldID);
          if (fieldDef is LoanReportFieldDef)
          {
            int borrowerPair = ((LoanReportFieldDef) fieldDef).BorrowerPair;
            if (borrowerPair > 0)
              gvItem.SubItems[3].Value = (object) FieldPairInfo.GetPairDescription(borrowerPair);
          }
          gvItem.Tag = (object) fieldDef;
          this.gvFields.Items.Add(gvItem);
        }
      }
    }

    private void loadFieldList(
      ReportFieldDefs fieldDefs,
      bool dbFieldsOnly,
      bool allowVirtualFields,
      Dictionary<string, AclTriState> accessColList_User,
      Dictionary<string, AclTriState> accessColList_Personas)
    {
      this.gvFields.Items.Clear();
      foreach (ReportFieldDef fieldDef in (ReportFieldDefContainer) fieldDefs)
      {
        if (!(!fieldDef.IsDatabaseField & dbFieldsOnly) && (allowVirtualFields || !(fieldDef.FieldDefinition is VirtualField)))
        {
          if (accessColList_User.ContainsKey(fieldDef.FieldID))
          {
            if (accessColList_User[fieldDef.FieldID] != AclTriState.True)
              continue;
          }
          else if (accessColList_Personas.ContainsKey(fieldDef.FieldID) && accessColList_Personas[fieldDef.FieldID] != AclTriState.True)
            continue;
          GVItem gvItem = new GVItem();
          gvItem.SubItems[0].Value = !accessColList_User.ContainsKey(fieldDef.FieldID) ? (object) new PersonaLinkImg(fieldDef.Category, true, true) : (object) new PersonaLinkImg(fieldDef.Category, false, true);
          gvItem.SubItems[1].Text = fieldDef.Description;
          gvItem.SubItems[2].Value = (object) new FieldIDElement(fieldDef.FieldID);
          if (fieldDef is LoanReportFieldDef)
          {
            int borrowerPair = ((LoanReportFieldDef) fieldDef).BorrowerPair;
            if (borrowerPair > 0)
              gvItem.SubItems[3].Value = (object) FieldPairInfo.GetPairDescription(borrowerPair);
          }
          gvItem.Tag = (object) fieldDef;
          this.gvFields.Items.Add(gvItem);
        }
      }
    }

    private void loadFieldList(
      ReportFieldDefs fieldDefs,
      PersonaPipelineView pipelineView,
      bool dbFieldsOnly,
      bool allowVirtualFields,
      Dictionary<string, AclTriState> accessColList)
    {
      this.gvFields.Items.Clear();
      List<string> stringList = new List<string>();
      stringList.AddRange((IEnumerable<string>) pipelineView.Columns.GetFieldDBNames());
      foreach (ReportFieldDef fieldDef in (ReportFieldDefContainer) fieldDefs)
      {
        if (!(!fieldDef.IsDatabaseField & dbFieldsOnly) && (allowVirtualFields || !(fieldDef.FieldDefinition is VirtualField)) && !stringList.Contains(fieldDef.CriterionFieldName) && (!accessColList.ContainsKey(fieldDef.FieldID) || accessColList[fieldDef.FieldID] == AclTriState.True))
        {
          GVItem gvItem = new GVItem(fieldDef.Category);
          gvItem.SubItems[1].Text = fieldDef.Description;
          gvItem.SubItems[2].Value = (object) new FieldIDElement(fieldDef.FieldID);
          if (fieldDef is LoanReportFieldDef)
          {
            int borrowerPair = ((LoanReportFieldDef) fieldDef).BorrowerPair;
            if (borrowerPair > 0)
              gvItem.SubItems[3].Value = (object) FieldPairInfo.GetPairDescription(borrowerPair);
          }
          gvItem.Tag = (object) fieldDef;
          this.gvFields.Items.Add(gvItem);
        }
      }
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.tmpList.Count > 0)
      {
        foreach (ReportFieldDef reportFieldDef in this.tmpList.ToArray())
        {
          if (this.accessColList != null)
          {
            if (this.accessColList.ContainsKey(reportFieldDef.FieldID))
              this.accessColList[reportFieldDef.FieldID] = AclTriState.False;
          }
          else if (this.accessColList_User != null)
          {
            if (this.accessColList_User.ContainsKey(reportFieldDef.FieldID))
              this.accessColList_User[reportFieldDef.FieldID] = AclTriState.False;
            else
              this.accessColList_User.Add(reportFieldDef.FieldID, AclTriState.False);
          }
          else
            this.pipelineView.Columns.Add(reportFieldDef.CriterionFieldName);
        }
      }
      foreach (GVItem selectedItem in this.gvFields.SelectedItems)
      {
        ReportFieldDef tag = (ReportFieldDef) selectedItem.Tag;
        if (this.accessColList != null)
        {
          if (this.accessColList.ContainsKey(tag.FieldID))
            this.accessColList[tag.FieldID] = AclTriState.False;
        }
        else if (this.accessColList_User != null)
        {
          if (this.accessColList_User.ContainsKey(tag.FieldID))
            this.accessColList_User[tag.FieldID] = AclTriState.False;
          else
            this.accessColList_User.Add(tag.FieldID, AclTriState.False);
        }
        else
          this.pipelineView.Columns.Add(tag.CriterionFieldName);
      }
      this.tmpList.Clear();
      this.DialogResult = DialogResult.OK;
    }

    private void btnFind_Click(object sender, EventArgs e)
    {
      string searchText = this.txtFind.Text.Trim();
      if (searchText == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Enter the text to find in the space provided.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
        this.searchForText(searchText);
    }

    private void searchForText(string searchText)
    {
      int num1 = 0;
      if (this.gvFields.SelectedItems.Count > 0 && searchText == this.lastSearch)
        num1 = this.gvFields.SelectedItems[0].Index + 1;
      this.lastSearch = searchText;
      if (num1 == 0)
      {
        for (int index = 0; index < this.gvFields.Items.Count; ++index)
        {
          if (string.Compare(this.gvFields.Items[index].SubItems[2].Text, searchText, true) == 0)
          {
            this.gvFields.Items[index].Selected = true;
            this.gvFields.EnsureVisible(index);
            this.gvFields.Focus();
            return;
          }
        }
      }
      if (this.gvFields.Items.Count > 0)
      {
        for (int index = 0; index <= this.gvFields.Items.Count; ++index)
        {
          for (int nItemIndex = 0; nItemIndex < this.gvFields.Columns.Count; ++nItemIndex)
          {
            int num2 = (num1 + index) % this.gvFields.Items.Count;
            if (this.gvFields.Items[num2].SubItems[nItemIndex].Text.IndexOf(searchText, StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
              this.gvFields.Items[num2].Selected = true;
              this.gvFields.EnsureVisible(num2);
              this.gvFields.Focus();
              return;
            }
          }
        }
      }
      int num3 = (int) Utils.Dialog((IWin32Window) this, "No match was found for '" + searchText + "'.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }

    private void txtFind_Enter(object sender, EventArgs e)
    {
      this.AcceptButton = (IButtonControl) this.btnFind;
    }

    private void gvFields_Enter(object sender, EventArgs e)
    {
      this.AcceptButton = (IButtonControl) this.btnOk;
    }

    private void tsmiLink_Click(object sender, EventArgs e)
    {
      if (this.gvFields.SelectedItems.Count == 0)
        return;
      foreach (GVItem selectedItem in this.gvFields.SelectedItems)
      {
        ReportFieldDef tag = (ReportFieldDef) selectedItem.Tag;
        if (this.accessColList_User.ContainsKey(tag.FieldID))
          this.accessColList_User.Remove(tag.FieldID);
      }
      this.loadFieldList(this.fieldDefs, false, true, this.accessColList_User, this.accessColList_Personas);
    }

    private void tsmiDisconnect_Click(object sender, EventArgs e)
    {
      if (this.gvFields.SelectedItems.Count == 0)
        return;
      foreach (GVItem selectedItem in this.gvFields.SelectedItems)
      {
        ReportFieldDef tag = (ReportFieldDef) selectedItem.Tag;
        if (this.accessColList_User.ContainsKey(tag.FieldID))
          this.accessColList_User[tag.FieldID] = AclTriState.True;
        else
          this.accessColList_User.Add(tag.FieldID, AclTriState.True);
      }
      this.loadFieldList(this.fieldDefs, false, true, this.accessColList_User, this.accessColList_Personas);
    }

    private void dlgButtons_Cancel(object sender, EventArgs e)
    {
      if (this.accessColList != null)
      {
        this.accessColList.Clear();
        foreach (string key in this.oriAccessColList.Keys)
          this.accessColList.Add(key, this.oriAccessColList[key]);
      }
      else if (this.accessColList_User != null)
      {
        this.accessColList_User.Clear();
        foreach (string key in this.oriAccessColList.Keys)
          this.accessColList_User.Add(key, this.oriAccessColList[key]);
      }
      this.DialogResult = DialogResult.Cancel;
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.tmpList.Clear();
      this.DialogResult = DialogResult.Cancel;
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      foreach (GVItem selectedItem in this.gvFields.SelectedItems)
      {
        ReportFieldDef tag = (ReportFieldDef) selectedItem.Tag;
        if (!this.tmpList.Contains(tag))
          this.tmpList.Add(tag);
      }
      foreach (GVItem selectedItem in this.gvFields.SelectedItems)
        this.gvFields.Items.Remove(selectedItem);
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
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      this.gvFields = new GridView();
      this.cmsLink = new ContextMenuStrip(this.components);
      this.tsmiLink = new ToolStripMenuItem();
      this.tsmiDisconnect = new ToolStripMenuItem();
      this.btnFind = new Button();
      this.txtFind = new TextBox();
      this.panel1 = new Panel();
      this.btnAdd = new Button();
      this.btnCancel = new Button();
      this.btnOk = new Button();
      this.cmsLink.SuspendLayout();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.gvFields.AllowMultiselect = false;
      this.gvFields.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Category";
      gvColumn1.Width = 116;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Description";
      gvColumn2.Width = 185;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.SortMethod = GVSortMethod.Custom;
      gvColumn3.Text = "Field ID";
      gvColumn3.Width = 94;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "colPair";
      gvColumn4.Text = "Borrower Pair";
      gvColumn4.Width = 90;
      this.gvFields.Columns.AddRange(new GVColumn[4]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4
      });
      this.gvFields.ContextMenuStrip = this.cmsLink;
      this.gvFields.Location = new Point(10, 34);
      this.gvFields.Name = "gvFields";
      this.gvFields.Size = new Size(487, 297);
      this.gvFields.TabIndex = 2;
      this.gvFields.Enter += new EventHandler(this.gvFields_Enter);
      this.cmsLink.Items.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.tsmiLink,
        (ToolStripItem) this.tsmiDisconnect
      });
      this.cmsLink.Name = "cmsLink";
      this.cmsLink.Size = new Size(238, 48);
      this.tsmiLink.Name = "tsmiLink";
      this.tsmiLink.Size = new Size(237, 22);
      this.tsmiLink.Text = "Link with Persona Rights";
      this.tsmiLink.Click += new EventHandler(this.tsmiLink_Click);
      this.tsmiDisconnect.Name = "tsmiDisconnect";
      this.tsmiDisconnect.Size = new Size(237, 22);
      this.tsmiDisconnect.Text = "Disconnect from Persona Rights";
      this.tsmiDisconnect.Click += new EventHandler(this.tsmiDisconnect_Click);
      this.btnFind.Location = new Point(10, 7);
      this.btnFind.Name = "btnFind";
      this.btnFind.Size = new Size(62, 22);
      this.btnFind.TabIndex = 5;
      this.btnFind.Text = "&Find";
      this.btnFind.UseVisualStyleBackColor = true;
      this.btnFind.Click += new EventHandler(this.btnFind_Click);
      this.txtFind.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtFind.Location = new Point(76, 8);
      this.txtFind.Name = "txtFind";
      this.txtFind.Size = new Size(421, 20);
      this.txtFind.TabIndex = 1;
      this.txtFind.Enter += new EventHandler(this.txtFind_Enter);
      this.panel1.Controls.Add((Control) this.btnAdd);
      this.panel1.Controls.Add((Control) this.btnCancel);
      this.panel1.Controls.Add((Control) this.btnOk);
      this.panel1.Dock = DockStyle.Bottom;
      this.panel1.Location = new Point(0, 337);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(508, 37);
      this.panel1.TabIndex = 7;
      this.btnAdd.Location = new Point(422, 8);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(75, 22);
      this.btnAdd.TabIndex = 2;
      this.btnAdd.Text = "Add";
      this.btnAdd.UseVisualStyleBackColor = true;
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.btnCancel.Location = new Point(341, 8);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.btnOk.Location = new Point(260, 8);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new Size(75, 22);
      this.btnOk.TabIndex = 0;
      this.btnOk.Text = "OK";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new EventHandler(this.btnOK_Click);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(508, 374);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.txtFind);
      this.Controls.Add((Control) this.btnFind);
      this.Controls.Add((Control) this.gvFields);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (FieldAccessDlg);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Select Field";
      this.cmsLink.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
