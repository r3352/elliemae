// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Design.FieldSelector
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Automation;
using EllieMae.Encompass.BusinessObjects.Loans;
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.Encompass.Forms.Design
{
  /// <summary>
  /// Provides the Field Selection user interface used within the Encompass Form Builder.
  /// </summary>
  /// <exclude />
  public class FieldSelector : System.Windows.Forms.Form
  {
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Panel panel1;
    private System.ComponentModel.Container components;
    private System.Windows.Forms.Button btnSelect;
    private ListViewSortManager sortMngrStandard;
    private ListViewSortManager sortMngrCustom;
    private ListViewSortManager sortMngrVirtual;
    private System.Windows.Forms.Panel panel2;
    private ListView lvwStandard;
    private ColumnHeader columnHeader1;
    private ColumnHeader columnHeader2;
    private TabPage tpStandard;
    private TabPage tpCustom;
    private ColumnHeader columnHeader3;
    private ColumnHeader columnHeader4;
    private ColumnHeader columnHeader5;
    private ColumnHeader columnHeader6;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Button btnFindCustom;
    private System.Windows.Forms.Button btnResetCustom;
    private System.Windows.Forms.Button btnNew;
    private System.Windows.Forms.Button btnResetStandard;
    private System.Windows.Forms.Button btnFindStandard;
    private System.Windows.Forms.TextBox txtFindStandard;
    private System.Windows.Forms.TextBox txtFindCustom;
    private ListView lvwCustom;
    private TabControl tabsFields;
    private TabPage tpVirtual;
    private System.Windows.Forms.Panel panel3;
    private System.Windows.Forms.TextBox txtFindVirtual;
    private System.Windows.Forms.Button btnFindVirtual;
    private System.Windows.Forms.Label label4;
    private ListView lvwVirtual;
    private ColumnHeader columnHeader10;
    private ColumnHeader columnHeader11;
    private ColumnHeader columnHeader12;
    private System.Windows.Forms.Button btnResetVirtual;
    private ColumnHeader columnHeader7;
    private ColumnHeader columnHeader8;
    private ColumnHeader columnHeader9;
    private FieldDescriptor selectedField;

    /// <summary>Default constructor for the FieldSelector dialog.</summary>
    public FieldSelector()
    {
      this.InitializeComponent();
      this.loadStandardFields((string) null);
      this.sortMngrStandard = new ListViewSortManager(this.lvwStandard, new System.Type[3]
      {
        typeof (FieldSelector.ListViewFieldIDSort),
        typeof (ListViewTextSort),
        typeof (ListViewTextSort)
      });
      this.sortMngrStandard.Sort(0);
      this.applySecuritySettings();
    }

    /// <summary>Clean up any resources being used.</summary>
    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      ListViewItem listViewItem1 = new ListViewItem("");
      ListViewItem listViewItem2 = new ListViewItem("");
      ListViewItem listViewItem3 = new ListViewItem("");
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnSelect = new System.Windows.Forms.Button();
      this.panel1 = new System.Windows.Forms.Panel();
      this.panel2 = new System.Windows.Forms.Panel();
      this.txtFindStandard = new System.Windows.Forms.TextBox();
      this.btnFindStandard = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.lvwStandard = new ListView();
      this.columnHeader1 = new ColumnHeader();
      this.columnHeader2 = new ColumnHeader();
      this.columnHeader3 = new ColumnHeader();
      this.btnResetStandard = new System.Windows.Forms.Button();
      this.tabsFields = new TabControl();
      this.tpStandard = new TabPage();
      this.tpVirtual = new TabPage();
      this.panel3 = new System.Windows.Forms.Panel();
      this.txtFindVirtual = new System.Windows.Forms.TextBox();
      this.btnFindVirtual = new System.Windows.Forms.Button();
      this.label4 = new System.Windows.Forms.Label();
      this.lvwVirtual = new ListView();
      this.columnHeader10 = new ColumnHeader();
      this.columnHeader11 = new ColumnHeader();
      this.columnHeader12 = new ColumnHeader();
      this.btnResetVirtual = new System.Windows.Forms.Button();
      this.tpCustom = new TabPage();
      this.txtFindCustom = new System.Windows.Forms.TextBox();
      this.btnNew = new System.Windows.Forms.Button();
      this.btnFindCustom = new System.Windows.Forms.Button();
      this.label2 = new System.Windows.Forms.Label();
      this.btnResetCustom = new System.Windows.Forms.Button();
      this.lvwCustom = new ListView();
      this.columnHeader4 = new ColumnHeader();
      this.columnHeader5 = new ColumnHeader();
      this.columnHeader6 = new ColumnHeader();
      this.columnHeader7 = new ColumnHeader();
      this.columnHeader8 = new ColumnHeader();
      this.columnHeader9 = new ColumnHeader();
      this.panel1.SuspendLayout();
      this.panel2.SuspendLayout();
      this.tabsFields.SuspendLayout();
      this.tpStandard.SuspendLayout();
      this.tpVirtual.SuspendLayout();
      this.panel3.SuspendLayout();
      this.tpCustom.SuspendLayout();
      this.SuspendLayout();
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(374, 10);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(76, 23);
      this.btnCancel.TabIndex = 10;
      this.btnCancel.Text = "&Cancel";
      this.btnSelect.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSelect.Location = new Point(293, 10);
      this.btnSelect.Name = "btnSelect";
      this.btnSelect.Size = new Size(75, 23);
      this.btnSelect.TabIndex = 9;
      this.btnSelect.Text = "&OK";
      this.btnSelect.Click += new EventHandler(this.btnSelect_Click);
      this.panel1.Controls.Add((System.Windows.Forms.Control) this.btnSelect);
      this.panel1.Controls.Add((System.Windows.Forms.Control) this.btnCancel);
      this.panel1.Dock = DockStyle.Bottom;
      this.panel1.Location = new Point(0, 347);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(460, 41);
      this.panel1.TabIndex = 3;
      this.panel2.Controls.Add((System.Windows.Forms.Control) this.txtFindStandard);
      this.panel2.Controls.Add((System.Windows.Forms.Control) this.btnFindStandard);
      this.panel2.Controls.Add((System.Windows.Forms.Control) this.label1);
      this.panel2.Controls.Add((System.Windows.Forms.Control) this.lvwStandard);
      this.panel2.Controls.Add((System.Windows.Forms.Control) this.btnResetStandard);
      this.panel2.Dock = DockStyle.Fill;
      this.panel2.Location = new Point(0, 0);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(452, 321);
      this.panel2.TabIndex = 4;
      this.txtFindStandard.Location = new Point(50, 10);
      this.txtFindStandard.Name = "txtFindStandard";
      this.txtFindStandard.Size = new Size(182, 20);
      this.txtFindStandard.TabIndex = 0;
      this.txtFindStandard.Enter += new EventHandler(this.txtFindStandard_Enter);
      this.btnFindStandard.Location = new Point(236, 10);
      this.btnFindStandard.Name = "btnFindStandard";
      this.btnFindStandard.Size = new Size(56, 21);
      this.btnFindStandard.TabIndex = 1;
      this.btnFindStandard.Text = "&Find >>";
      this.btnFindStandard.Click += new EventHandler(this.btnFindStandard_Click);
      this.label1.Location = new Point(10, 13);
      this.label1.Name = "label1";
      this.label1.Size = new Size(38, 16);
      this.label1.TabIndex = 5;
      this.label1.Text = "Find:";
      this.lvwStandard.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lvwStandard.Columns.AddRange(new ColumnHeader[3]
      {
        this.columnHeader1,
        this.columnHeader2,
        this.columnHeader3
      });
      this.lvwStandard.FullRowSelect = true;
      this.lvwStandard.HideSelection = false;
      this.lvwStandard.Items.AddRange(new ListViewItem[1]
      {
        listViewItem1
      });
      this.lvwStandard.Location = new Point(0, 40);
      this.lvwStandard.MultiSelect = false;
      this.lvwStandard.Name = "lvwStandard";
      this.lvwStandard.Size = new Size(452, 281);
      this.lvwStandard.TabIndex = 3;
      this.lvwStandard.UseCompatibleStateImageBehavior = false;
      this.lvwStandard.View = View.Details;
      this.lvwStandard.DoubleClick += new EventHandler(this.lvwFields_DoubleClick);
      this.lvwStandard.Enter += new EventHandler(this.lvwStandard_Enter);
      this.columnHeader1.Text = "Field ID";
      this.columnHeader1.Width = 120;
      this.columnHeader2.Text = "Description";
      this.columnHeader2.Width = 169;
      this.columnHeader3.Text = "Type";
      this.columnHeader3.Width = 114;
      this.btnResetStandard.Location = new Point(298, 10);
      this.btnResetStandard.Name = "btnResetStandard";
      this.btnResetStandard.Size = new Size(56, 21);
      this.btnResetStandard.TabIndex = 2;
      this.btnResetStandard.Text = "&Reset";
      this.btnResetStandard.Click += new EventHandler(this.btnResetStandard_Click);
      this.tabsFields.Controls.Add((System.Windows.Forms.Control) this.tpStandard);
      this.tabsFields.Controls.Add((System.Windows.Forms.Control) this.tpVirtual);
      this.tabsFields.Controls.Add((System.Windows.Forms.Control) this.tpCustom);
      this.tabsFields.Dock = DockStyle.Fill;
      this.tabsFields.Location = new Point(0, 0);
      this.tabsFields.Name = "tabsFields";
      this.tabsFields.SelectedIndex = 0;
      this.tabsFields.Size = new Size(460, 347);
      this.tabsFields.TabIndex = 11;
      this.tabsFields.SelectedIndexChanged += new EventHandler(this.tabsFields_SelectedIndexChanged);
      this.tpStandard.Controls.Add((System.Windows.Forms.Control) this.panel2);
      this.tpStandard.Location = new Point(4, 22);
      this.tpStandard.Name = "tpStandard";
      this.tpStandard.Size = new Size(452, 321);
      this.tpStandard.TabIndex = 0;
      this.tpStandard.Text = "Standard Fields";
      this.tpStandard.UseVisualStyleBackColor = true;
      this.tpVirtual.Controls.Add((System.Windows.Forms.Control) this.panel3);
      this.tpVirtual.Location = new Point(4, 22);
      this.tpVirtual.Name = "tpVirtual";
      this.tpVirtual.Size = new Size(280, 198);
      this.tpVirtual.TabIndex = 2;
      this.tpVirtual.Text = "Virtual Fields";
      this.tpVirtual.UseVisualStyleBackColor = true;
      this.panel3.Controls.Add((System.Windows.Forms.Control) this.txtFindVirtual);
      this.panel3.Controls.Add((System.Windows.Forms.Control) this.btnFindVirtual);
      this.panel3.Controls.Add((System.Windows.Forms.Control) this.label4);
      this.panel3.Controls.Add((System.Windows.Forms.Control) this.lvwVirtual);
      this.panel3.Controls.Add((System.Windows.Forms.Control) this.btnResetVirtual);
      this.panel3.Dock = DockStyle.Fill;
      this.panel3.Location = new Point(0, 0);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(280, 198);
      this.panel3.TabIndex = 5;
      this.txtFindVirtual.Location = new Point(50, 10);
      this.txtFindVirtual.Name = "txtFindVirtual";
      this.txtFindVirtual.Size = new Size(182, 20);
      this.txtFindVirtual.TabIndex = 0;
      this.txtFindVirtual.Enter += new EventHandler(this.txtFindVirtual_Enter);
      this.btnFindVirtual.Location = new Point(236, 10);
      this.btnFindVirtual.Name = "btnFindVirtual";
      this.btnFindVirtual.Size = new Size(56, 21);
      this.btnFindVirtual.TabIndex = 1;
      this.btnFindVirtual.Text = "&Find >>";
      this.btnFindVirtual.Click += new EventHandler(this.btnFindVirtual_Click);
      this.label4.Location = new Point(10, 13);
      this.label4.Name = "label4";
      this.label4.Size = new Size(38, 16);
      this.label4.TabIndex = 5;
      this.label4.Text = "Find:";
      this.lvwVirtual.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lvwVirtual.Columns.AddRange(new ColumnHeader[3]
      {
        this.columnHeader10,
        this.columnHeader11,
        this.columnHeader12
      });
      this.lvwVirtual.FullRowSelect = true;
      this.lvwVirtual.HideSelection = false;
      this.lvwVirtual.Items.AddRange(new ListViewItem[1]
      {
        listViewItem2
      });
      this.lvwVirtual.Location = new Point(0, 40);
      this.lvwVirtual.MultiSelect = false;
      this.lvwVirtual.Name = "lvwVirtual";
      this.lvwVirtual.Size = new Size(280, 158);
      this.lvwVirtual.TabIndex = 3;
      this.lvwVirtual.UseCompatibleStateImageBehavior = false;
      this.lvwVirtual.View = View.Details;
      this.lvwVirtual.DoubleClick += new EventHandler(this.lvwVirtual_DoubleClick);
      this.lvwVirtual.Enter += new EventHandler(this.lvwVirtual_Enter);
      this.columnHeader10.Text = "Field ID";
      this.columnHeader10.Width = 120;
      this.columnHeader11.Text = "Description";
      this.columnHeader11.Width = 169;
      this.columnHeader12.Text = "Type";
      this.columnHeader12.Width = 114;
      this.btnResetVirtual.Location = new Point(298, 10);
      this.btnResetVirtual.Name = "btnResetVirtual";
      this.btnResetVirtual.Size = new Size(56, 21);
      this.btnResetVirtual.TabIndex = 2;
      this.btnResetVirtual.Text = "&Reset";
      this.btnResetVirtual.Click += new EventHandler(this.btnResetVirtual_Click);
      this.tpCustom.Controls.Add((System.Windows.Forms.Control) this.txtFindCustom);
      this.tpCustom.Controls.Add((System.Windows.Forms.Control) this.btnNew);
      this.tpCustom.Controls.Add((System.Windows.Forms.Control) this.btnFindCustom);
      this.tpCustom.Controls.Add((System.Windows.Forms.Control) this.label2);
      this.tpCustom.Controls.Add((System.Windows.Forms.Control) this.btnResetCustom);
      this.tpCustom.Controls.Add((System.Windows.Forms.Control) this.lvwCustom);
      this.tpCustom.Location = new Point(4, 22);
      this.tpCustom.Name = "tpCustom";
      this.tpCustom.Size = new Size(280, 198);
      this.tpCustom.TabIndex = 1;
      this.tpCustom.Text = "Custom Fields";
      this.tpCustom.UseVisualStyleBackColor = true;
      this.txtFindCustom.Location = new Point(50, 10);
      this.txtFindCustom.Name = "txtFindCustom";
      this.txtFindCustom.Size = new Size(182, 20);
      this.txtFindCustom.TabIndex = 4;
      this.txtFindCustom.Enter += new EventHandler(this.txtFindCustom_Enter);
      this.btnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnNew.Location = new Point(206, 10);
      this.btnNew.Name = "btnNew";
      this.btnNew.Size = new Size(65, 21);
      this.btnNew.TabIndex = 7;
      this.btnNew.Text = "&New/Edit";
      this.btnNew.Click += new EventHandler(this.btnNew_Click);
      this.btnFindCustom.Location = new Point(236, 10);
      this.btnFindCustom.Name = "btnFindCustom";
      this.btnFindCustom.Size = new Size(56, 21);
      this.btnFindCustom.TabIndex = 5;
      this.btnFindCustom.Text = "&Find >>";
      this.btnFindCustom.Click += new EventHandler(this.btnFindCustom_Click);
      this.label2.Location = new Point(10, 13);
      this.label2.Name = "label2";
      this.label2.Size = new Size(38, 16);
      this.label2.TabIndex = 9;
      this.label2.Text = "Find:";
      this.btnResetCustom.Location = new Point(298, 10);
      this.btnResetCustom.Name = "btnResetCustom";
      this.btnResetCustom.Size = new Size(56, 21);
      this.btnResetCustom.TabIndex = 6;
      this.btnResetCustom.Text = "&Reset";
      this.btnResetCustom.Click += new EventHandler(this.btnResetCustom_Click);
      this.lvwCustom.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lvwCustom.Columns.AddRange(new ColumnHeader[3]
      {
        this.columnHeader4,
        this.columnHeader5,
        this.columnHeader6
      });
      this.lvwCustom.FullRowSelect = true;
      this.lvwCustom.HideSelection = false;
      this.lvwCustom.Items.AddRange(new ListViewItem[1]
      {
        listViewItem3
      });
      this.lvwCustom.Location = new Point(0, 40);
      this.lvwCustom.MultiSelect = false;
      this.lvwCustom.Name = "lvwCustom";
      this.lvwCustom.Size = new Size(279, 155);
      this.lvwCustom.TabIndex = 8;
      this.lvwCustom.UseCompatibleStateImageBehavior = false;
      this.lvwCustom.View = View.Details;
      this.lvwCustom.DoubleClick += new EventHandler(this.lvwCustom_DoubleClick);
      this.lvwCustom.Enter += new EventHandler(this.lvwCustom_Enter);
      this.columnHeader4.Text = "Field ID";
      this.columnHeader4.Width = 120;
      this.columnHeader5.Text = "Description";
      this.columnHeader5.Width = 165;
      this.columnHeader6.Text = "Type";
      this.columnHeader6.Width = 106;
      this.AcceptButton = (IButtonControl) this.btnSelect;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(460, 388);
      this.Controls.Add((System.Windows.Forms.Control) this.tabsFields);
      this.Controls.Add((System.Windows.Forms.Control) this.panel1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (FieldSelector);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Field Selector";
      this.Activated += new EventHandler(this.FieldSelector_Activated);
      this.panel1.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.tabsFields.ResumeLayout(false);
      this.tpStandard.ResumeLayout(false);
      this.tpVirtual.ResumeLayout(false);
      this.panel3.ResumeLayout(false);
      this.panel3.PerformLayout();
      this.tpCustom.ResumeLayout(false);
      this.tpCustom.PerformLayout();
      this.ResumeLayout(false);
    }

    /// <summary>
    /// Refreshes the Custom Fields list to ensure it's up-to-date.
    /// </summary>
    public void RefreshCustomFields() => this.loadCustomFields((string) null);

    private void applySecuritySettings()
    {
      if (((FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features)).CheckPermission(AclFeature.SettingsTab_Company_LoanCustomFields, Session.UserInfo))
        return;
      this.btnNew.Visible = false;
    }

    private void loadStandardFields(string filter)
    {
      if (this.sortMngrStandard != null)
        this.sortMngrStandard.Disable();
      this.lvwStandard.Items.Clear();
      this.lvwStandard.BeginUpdate();
      this.lvwStandard.Items.Add(this.createItemForField(FieldDescriptor.Empty));
      foreach (FieldDescriptor standardField in FieldDescriptors.StandardFields)
      {
        if (filter == null || standardField.Description.ToLower().IndexOf(filter) >= 0 || standardField.FieldID.ToLower().IndexOf(filter) >= 0)
          this.lvwStandard.Items.Add(this.createItemForField(standardField));
        if (standardField.MultiInstance)
        {
          for (int instanceIndexOrSpecifier = 1; instanceIndexOrSpecifier <= 20; ++instanceIndexOrSpecifier)
          {
            FieldDescriptor instanceDescriptor = standardField.GetInstanceDescriptor((object) instanceIndexOrSpecifier);
            if (filter == null || standardField.Description.ToLower().IndexOf(filter) >= 0 || standardField.FieldID.ToLower().IndexOf(filter) >= 0)
              this.lvwStandard.Items.Add(this.createItemForField(instanceDescriptor));
          }
        }
      }
      if (this.sortMngrStandard != null)
        this.sortMngrStandard.Enable();
      this.lvwStandard.EndUpdate();
      if (this.findField(this.lvwStandard, filter))
      {
        this.lvwStandard.Focus();
      }
      else
      {
        if (this.lvwStandard.Items.Count <= 0)
          return;
        this.lvwStandard.Items[0].Selected = true;
        this.lvwStandard.Focus();
      }
    }

    private void loadVirtualFields(string filter)
    {
      if (this.sortMngrVirtual != null)
        this.sortMngrVirtual.Disable();
      this.lvwVirtual.Items.Clear();
      this.lvwVirtual.BeginUpdate();
      foreach (FieldDescriptor virtualField in EncompassApplication.Session.Loans.FieldDescriptors.VirtualFields)
      {
        if (filter == null || virtualField.Description.ToLower().IndexOf(filter) >= 0 || virtualField.FieldID.ToLower().IndexOf(filter) >= 0)
          this.lvwVirtual.Items.Add(this.createItemForField(virtualField));
      }
      if (this.sortMngrVirtual != null)
        this.sortMngrVirtual.Enable();
      this.lvwVirtual.EndUpdate();
      if (this.findField(this.lvwVirtual, filter))
      {
        this.lvwVirtual.Focus();
      }
      else
      {
        if (this.lvwVirtual.Items.Count <= 0)
          return;
        this.lvwVirtual.Items[0].Selected = true;
        this.lvwVirtual.Focus();
      }
    }

    private void loadCustomFields(string filter)
    {
      if (this.sortMngrCustom != null)
        this.sortMngrCustom.Disable();
      this.lvwCustom.Items.Clear();
      this.lvwCustom.BeginUpdate();
      EncompassApplication.Session.Loans.FieldDescriptors.Refresh();
      foreach (FieldDescriptor customField in EncompassApplication.Session.Loans.FieldDescriptors.CustomFields)
      {
        if (customField.Format != LoanFieldFormat.NONE && (filter == null || customField.Description.ToLower().IndexOf(filter) >= 0 || customField.FieldID.ToLower().IndexOf(filter) >= 0))
          this.lvwCustom.Items.Add(this.createItemForField(customField));
      }
      if (this.sortMngrCustom != null)
        this.sortMngrCustom.Enable();
      this.lvwCustom.EndUpdate();
      if (this.findField(this.lvwCustom, filter))
      {
        this.lvwCustom.Focus();
      }
      else
      {
        if (this.lvwCustom.Items.Count <= 0)
          return;
        this.lvwCustom.Items[0].Selected = true;
        this.lvwCustom.Focus();
      }
    }

    private ListViewItem createItemForField(FieldDescriptor field)
    {
      return new ListViewItem(new string[3]
      {
        field.FieldID == "" ? "(Unassigned)" : field.FieldID,
        field.Description,
        this.getFormatDescription(field.Format)
      })
      {
        Tag = (object) field
      };
    }

    private bool findField(ListView lvw, string fieldId)
    {
      foreach (ListViewItem listViewItem in lvw.Items)
      {
        if (listViewItem.Text.ToLower() == fieldId)
        {
          listViewItem.Selected = true;
          listViewItem.EnsureVisible();
          return true;
        }
      }
      return false;
    }

    /// <summary>
    /// Displays the Form Selector disalog modally and pre-selects the specified field.
    /// </summary>
    /// <param name="parentForm"></param>
    /// <param name="selectedItem"></param>
    /// <returns></returns>
    public DialogResult ShowDialog(System.Windows.Forms.Form parentForm, FieldDescriptor selectedItem)
    {
      this.selectedField = selectedItem;
      return this.ShowDialog((IWin32Window) parentForm);
    }

    private void btnSelect_Click(object sender, EventArgs e)
    {
      ListView currentFieldList = this.getCurrentFieldList();
      if (currentFieldList.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must first select a field from the list.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        FieldDescriptor field = (FieldDescriptor) currentFieldList.SelectedItems[0].Tag;
        if (field.MultiInstance)
          field = this.resolveMultiInstanceField(field);
        if (field == null)
          return;
        this.selectedField = field;
        this.DialogResult = DialogResult.OK;
      }
    }

    private FieldDescriptor resolveMultiInstanceField(FieldDescriptor field)
    {
      using (InstanceSelectorDialog instanceSelectorDialog = new InstanceSelectorDialog((FieldInstanceSpecifierType) field.InstanceSpecifierType))
        return instanceSelectorDialog.ShowDialog((IWin32Window) this) != DialogResult.OK ? (FieldDescriptor) null : field.GetInstanceDescriptor((object) instanceSelectorDialog.SelectedInstance);
    }

    private ListView getCurrentFieldList()
    {
      if (this.tabsFields.SelectedTab == this.tpStandard)
        return this.lvwStandard;
      return this.tabsFields.SelectedTab == this.tpVirtual ? this.lvwVirtual : this.lvwCustom;
    }

    private void lvwFields_DoubleClick(object sender, EventArgs e)
    {
      this.btnSelect_Click(sender, e);
    }

    private void lvwCustom_DoubleClick(object sender, EventArgs e)
    {
      this.btnSelect_Click(sender, e);
    }

    private void lvwVirtual_DoubleClick(object sender, EventArgs e)
    {
      this.btnSelect_Click(sender, e);
    }

    private void FieldSelector_Activated(object sender, EventArgs e)
    {
      if (this.selectedField != null)
      {
        ListView fieldList;
        if (this.selectedField.IsCustomField)
        {
          this.tabsFields.SelectedTab = this.tpCustom;
          fieldList = this.lvwCustom;
        }
        else if (this.selectedField.IsVirtualField)
        {
          this.tabsFields.SelectedTab = this.tpVirtual;
          fieldList = this.lvwVirtual;
        }
        else
        {
          this.tabsFields.SelectedTab = this.tpStandard;
          fieldList = this.lvwStandard;
        }
        if (!this.selectField(fieldList, this.selectedField) && this.selectedField.IsInstance)
          this.selectField(fieldList, this.selectedField.ParentDescriptor);
        this.selectedField = (FieldDescriptor) null;
      }
      if (this.tabsFields.SelectedTab == this.tpStandard)
        this.txtFindStandard.Focus();
      else if (this.tabsFields.SelectedTab == this.tpVirtual)
        this.txtFindVirtual.Focus();
      else
        this.txtFindCustom.Focus();
    }

    private bool selectField(ListView fieldList, FieldDescriptor fieldToSelect)
    {
      fieldList.SelectedItems.Clear();
      foreach (ListViewItem listViewItem in fieldList.Items)
      {
        if (fieldToSelect.Equals(listViewItem.Tag))
        {
          listViewItem.Selected = true;
          listViewItem.EnsureVisible();
          return true;
        }
      }
      return false;
    }

    private void btnFindStandard_Click(object sender, EventArgs e)
    {
      this.loadStandardFields(this.txtFindStandard.Text.ToLower());
    }

    private void btnResetStandard_Click(object sender, EventArgs e)
    {
      this.txtFindStandard.Text = "";
      this.loadStandardFields((string) null);
    }

    private void txtFindStandard_Enter(object sender, EventArgs e)
    {
      this.AcceptButton = (IButtonControl) this.btnFindStandard;
    }

    private void lvwStandard_Enter(object sender, EventArgs e)
    {
      this.AcceptButton = (IButtonControl) this.btnSelect;
    }

    private void btnFindVirtual_Click(object sender, EventArgs e)
    {
      this.loadVirtualFields(this.txtFindVirtual.Text.ToLower());
    }

    private void btnResetVirtual_Click(object sender, EventArgs e)
    {
      this.txtFindVirtual.Text = "";
      this.loadVirtualFields((string) null);
    }

    private void txtFindVirtual_Enter(object sender, EventArgs e)
    {
      this.AcceptButton = (IButtonControl) this.btnFindVirtual;
    }

    private void lvwVirtual_Enter(object sender, EventArgs e)
    {
      this.AcceptButton = (IButtonControl) this.btnSelect;
    }

    private void btnFindCustom_Click(object sender, EventArgs e)
    {
      this.loadCustomFields(this.txtFindCustom.Text.ToLower());
    }

    private void txtFindCustom_Enter(object sender, EventArgs e)
    {
      this.AcceptButton = (IButtonControl) this.btnFindCustom;
    }

    private void lvwCustom_Enter(object sender, EventArgs e)
    {
      this.AcceptButton = (IButtonControl) this.btnSelect;
    }

    private void btnResetCustom_Click(object sender, EventArgs e)
    {
      this.txtFindCustom.Text = "";
      this.loadCustomFields((string) null);
    }

    /// <summary>Gets the FieldDescriptor selected by the user.</summary>
    public FieldDescriptor SelectedField => this.selectedField;

    private string getFormatDescription(LoanFieldFormat format)
    {
      return FieldFormatEnumUtil.ValueToName((FieldFormat) format);
    }

    private void btnNew_Click(object sender, EventArgs e)
    {
      using (CustomFieldsEditorDialog fieldsEditorDialog = new CustomFieldsEditorDialog())
      {
        if (fieldsEditorDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.loadCustomFields(this.txtFindCustom.Text);
      }
    }

    private void tabsFields_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.tabsFields.SelectedTab == this.tpVirtual && this.sortMngrVirtual == null)
      {
        Cursor.Current = Cursors.WaitCursor;
        this.loadVirtualFields((string) null);
        this.sortMngrVirtual = new ListViewSortManager(this.lvwVirtual, new System.Type[3]
        {
          typeof (FieldSelector.ListViewFieldIDSort),
          typeof (ListViewTextSort),
          typeof (ListViewTextSort)
        });
        this.sortMngrVirtual.Sort(0);
        Cursor.Current = Cursors.Default;
      }
      else
      {
        if (this.tabsFields.SelectedTab != this.tpCustom || this.sortMngrCustom != null)
          return;
        Cursor.Current = Cursors.WaitCursor;
        this.loadCustomFields((string) null);
        this.sortMngrCustom = new ListViewSortManager(this.lvwCustom, new System.Type[3]
        {
          typeof (FieldSelector.ListViewFieldIDSort),
          typeof (ListViewTextSort),
          typeof (ListViewTextSort)
        });
        this.sortMngrCustom.Sort(0);
        Cursor.Current = Cursors.Default;
      }
    }

    /// <summary>Provides integer (32 bits) and text sorting</summary>
    /// <exclude />
    private class ListViewFieldIDSort : IComparer
    {
      private bool ascending;

      /// <summary>Constructor</summary>
      /// <param name="sortColumn">Column to be sorted</param>
      /// <param name="ascending">true, if ascending order, false otherwise</param>
      /// <param name="sortMngr">Provides sorting of the items in the list</param>
      public ListViewFieldIDSort(int sortColumn, bool ascending, ListViewSortManager sortMngr)
      {
        this.ascending = ascending;
      }

      public int Compare(object lhs, object rhs)
      {
        ListViewItem listViewItem1 = lhs as ListViewItem;
        ListViewItem listViewItem2 = rhs as ListViewItem;
        if (listViewItem1 == null || listViewItem2 == null)
          return 0;
        int num = (listViewItem1.Tag as FieldDescriptor).CompareTo((object) (listViewItem2.Tag as FieldDescriptor));
        if (!this.ascending)
          num = -num;
        return num;
      }
    }
  }
}
