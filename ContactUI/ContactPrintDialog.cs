// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.ContactPrintDialog
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class ContactPrintDialog : Form
  {
    private const int NUMBER_CUSTOM_PAGES = 5;
    private const int NUMBER_FIELDS_PAGE = 20;
    protected internal Button btnPrintPreview;
    private ContactType contactType;
    private bool printSummary;
    private bool printPreview = true;
    private int[] printPages = new int[0];
    private System.ComponentModel.Container components;
    private Button btnPreview;
    private Button btnPrint;
    private Button btnCancel;
    private ListView lvwPrintSelection;
    private ColumnHeader headerTabs;
    private Label lblInstructions;

    internal bool PrintSummary => this.printSummary;

    internal bool PrintPreview => this.printPreview;

    internal int[] PrintPages => this.printPages;

    private bool printBorrower => this.contactType == ContactType.Borrower;

    private bool printPartner => this.contactType != 0;

    public ContactPrintDialog(ContactType contactType)
    {
      this.contactType = contactType;
      this.InitializeComponent();
      this.btnPrintPreview.Visible = false;
      this.processContactCustomFields();
      this.processCategoryCustomFields();
      if (this.lvwPrintSelection.Items.Count != 1)
        return;
      this.Height -= this.lvwPrintSelection.Height;
      this.lvwPrintSelection.Visible = false;
      if (this.contactType == ContactType.Borrower)
        this.lblInstructions.Text = "Only Details, Extra, and Opportunity tabs will be printed.";
      else
        this.lblInstructions.Text = "Only Details will be printed.";
    }

    private void processContactCustomFields()
    {
      ContactCustomFieldInfoCollection customFieldInfo;
      if (this.printBorrower)
      {
        customFieldInfo = Session.ContactManager.GetCustomFieldInfo(ContactType.Borrower);
        this.lvwPrintSelection.Items[0].Text = "Details, Extra, Opportunity Tabs";
      }
      else
      {
        customFieldInfo = Session.ContactManager.GetCustomFieldInfo(ContactType.BizPartner);
        this.lvwPrintSelection.Items[0].Text = "Details";
      }
      if (customFieldInfo == null)
        return;
      ContactCustomFieldInfo[] items = customFieldInfo.Items;
      Array.Sort<ContactCustomFieldInfo>(items);
      bool[] flagArray = new bool[5];
      for (int index1 = 0; index1 < items.Length; ++index1)
      {
        int index2 = (items[index1].LabelID - 1) / 20;
        if (!flagArray[index2])
          flagArray[index2] = true;
      }
      this.setSelectionTitle(customFieldInfo.Page1Name, 1);
      this.setSelectionTitle(customFieldInfo.Page2Name, 2);
      this.setSelectionTitle(customFieldInfo.Page3Name, 3);
      this.setSelectionTitle(customFieldInfo.Page4Name, 4);
      this.setSelectionTitle(customFieldInfo.Page5Name, 5);
      for (int index = 4; index >= 0; --index)
      {
        if (!flagArray[index])
          this.lvwPrintSelection.Items.RemoveAt(index + 1);
      }
    }

    private void processCategoryCustomFields()
    {
      if (!this.printBorrower)
        return;
      this.lvwPrintSelection.Items.RemoveAt(this.lvwPrintSelection.Items.Count - 1);
    }

    private void setSelectionTitle(string title, int index)
    {
      if (title == null || string.Empty == title)
        title = "Custom Tab " + index.ToString();
      this.lvwPrintSelection.Items[index].Text = title;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void btnPrint_Click(object sender, EventArgs e)
    {
      this.printPreview = false;
      this.setPrintSettings();
      this.DialogResult = DialogResult.OK;
    }

    private void btnPreview_Click(object sender, EventArgs e)
    {
      this.printPreview = true;
      this.setPrintSettings();
      this.DialogResult = DialogResult.OK;
    }

    private void setPrintSettings()
    {
      this.printPages = new int[this.lvwPrintSelection.CheckedItems.Count];
      int num = 0;
      foreach (ListViewItem checkedItem in this.lvwPrintSelection.CheckedItems)
        this.printPages[num++] = int.Parse(checkedItem.Tag.ToString());
    }

    private void btnSelectAll_Click(object sender, EventArgs e)
    {
      foreach (ListViewItem listViewItem in this.lvwPrintSelection.Items)
        listViewItem.Checked = true;
    }

    private void btnPrintPrev_Click(object sender, EventArgs e)
    {
      this.printPreview = true;
      this.setPrintSettings();
      this.DialogResult = DialogResult.OK;
    }

    private void lvwPrintSelection_Resize(object sender, EventArgs e)
    {
      this.headerTabs.Width = this.lvwPrintSelection.ClientSize.Width;
    }

    private void lvwPrintSelection_ItemCheck(object sender, ItemCheckEventArgs e)
    {
      if (!this.printPartner)
        return;
      if (e.Index == 0 && e.NewValue == CheckState.Unchecked)
      {
        this.lvwPrintSelection.Items[1].Checked = false;
      }
      else
      {
        if (1 != e.Index || e.NewValue != CheckState.Checked)
          return;
        this.lvwPrintSelection.Items[0].Checked = true;
      }
    }

    private void lvwPrintSelection_Click(object sender, EventArgs e)
    {
      if (0 >= this.lvwPrintSelection.SelectedItems.Count)
        return;
      this.lvwPrintSelection.SelectedItems[0].Checked = !this.lvwPrintSelection.SelectedItems[0].Checked;
      this.lvwPrintSelection.SelectedItems.Clear();
    }

    private void InitializeComponent()
    {
      ListViewItem listViewItem1 = new ListViewItem("Detail");
      ListViewItem listViewItem2 = new ListViewItem("Contact Custom Fields Page 1");
      ListViewItem listViewItem3 = new ListViewItem("Contact Custom Fields Page 2");
      ListViewItem listViewItem4 = new ListViewItem("Contact Custom Fields Page 3");
      ListViewItem listViewItem5 = new ListViewItem("Contact Custom Fields Page 4");
      ListViewItem listViewItem6 = new ListViewItem("Contact Custom Fields Page 5");
      ListViewItem listViewItem7 = new ListViewItem("Custom Category Fields");
      this.lvwPrintSelection = new ListView();
      this.headerTabs = new ColumnHeader();
      this.btnPreview = new Button();
      this.btnPrint = new Button();
      this.btnCancel = new Button();
      this.btnPrintPreview = new Button();
      this.lblInstructions = new Label();
      this.SuspendLayout();
      this.lvwPrintSelection.BackColor = SystemColors.Control;
      this.lvwPrintSelection.BorderStyle = BorderStyle.None;
      this.lvwPrintSelection.CheckBoxes = true;
      this.lvwPrintSelection.Columns.AddRange(new ColumnHeader[1]
      {
        this.headerTabs
      });
      this.lvwPrintSelection.FullRowSelect = true;
      this.lvwPrintSelection.HeaderStyle = ColumnHeaderStyle.None;
      listViewItem1.Checked = true;
      listViewItem1.StateImageIndex = 1;
      listViewItem1.Tag = (object) "1";
      listViewItem2.StateImageIndex = 0;
      listViewItem2.Tag = (object) "2";
      listViewItem3.StateImageIndex = 0;
      listViewItem3.Tag = (object) "3";
      listViewItem4.StateImageIndex = 0;
      listViewItem4.Tag = (object) "4";
      listViewItem5.StateImageIndex = 0;
      listViewItem5.Tag = (object) "5";
      listViewItem6.StateImageIndex = 0;
      listViewItem6.Tag = (object) "6";
      listViewItem7.StateImageIndex = 0;
      listViewItem7.Tag = (object) "7";
      this.lvwPrintSelection.Items.AddRange(new ListViewItem[7]
      {
        listViewItem1,
        listViewItem2,
        listViewItem3,
        listViewItem4,
        listViewItem5,
        listViewItem6,
        listViewItem7
      });
      this.lvwPrintSelection.Location = new Point(12, 28);
      this.lvwPrintSelection.MultiSelect = false;
      this.lvwPrintSelection.Name = "lvwPrintSelection";
      this.lvwPrintSelection.Scrollable = false;
      this.lvwPrintSelection.Size = new Size(187, 126);
      this.lvwPrintSelection.TabIndex = 19;
      this.lvwPrintSelection.TabStop = false;
      this.lvwPrintSelection.UseCompatibleStateImageBehavior = false;
      this.lvwPrintSelection.View = View.Details;
      this.lvwPrintSelection.Resize += new EventHandler(this.lvwPrintSelection_Resize);
      this.lvwPrintSelection.ItemCheck += new ItemCheckEventHandler(this.lvwPrintSelection_ItemCheck);
      this.lvwPrintSelection.Click += new EventHandler(this.lvwPrintSelection_Click);
      this.headerTabs.Text = "Pages";
      this.headerTabs.Width = 300;
      this.btnPreview.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnPreview.Location = new Point(153, 160);
      this.btnPreview.Name = "btnPreview";
      this.btnPreview.Size = new Size(75, 24);
      this.btnPreview.TabIndex = 16;
      this.btnPreview.Text = "Preview";
      this.btnPreview.Click += new EventHandler(this.btnPreview_Click);
      this.btnPrint.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnPrint.Location = new Point(69, 160);
      this.btnPrint.Name = "btnPrint";
      this.btnPrint.Size = new Size(75, 24);
      this.btnPrint.TabIndex = 15;
      this.btnPrint.Text = "Print";
      this.btnPrint.Click += new EventHandler(this.btnPrint_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(237, 160);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 24);
      this.btnCancel.TabIndex = 17;
      this.btnCancel.Text = "Cancel";
      this.btnPrintPreview.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnPrintPreview.Location = new Point(133, 160);
      this.btnPrintPreview.Name = "btnPrintPreview";
      this.btnPrintPreview.Size = new Size(92, 24);
      this.btnPrintPreview.TabIndex = 18;
      this.btnPrintPreview.Text = "Print/ Preview";
      this.btnPrintPreview.Click += new EventHandler(this.btnPrintPrev_Click);
      this.lblInstructions.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblInstructions.Location = new Point(15, 9);
      this.lblInstructions.Name = "lblInstructions";
      this.lblInstructions.Size = new Size(283, 16);
      this.lblInstructions.TabIndex = 19;
      this.lblInstructions.Text = "Select the tabs you want to print.";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(317, 201);
      this.Controls.Add((Control) this.lblInstructions);
      this.Controls.Add((Control) this.btnPreview);
      this.Controls.Add((Control) this.btnPrint);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnPrintPreview);
      this.Controls.Add((Control) this.lvwPrintSelection);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ContactPrintDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Contact Print";
      this.KeyUp += new KeyEventHandler(this.ContactPrintDialog_KeyUp);
      this.ResumeLayout(false);
    }

    private void ContactPrintDialog_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Escape)
        return;
      this.btnCancel.PerformClick();
    }
  }
}
