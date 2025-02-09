// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.HUD1ESSetupDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class HUD1ESSetupDialog : Form
  {
    private Button cancelBtn;
    private Button okBtn;
    private IContainer components;
    private bool isBiweekly;
    private DateTime firstPaymentDate;
    private HUDItem[] hudItems = new HUDItem[31];
    private Button clearBtn;
    private LoanData loan;
    private const int MAXLINES = 53;
    private const int YEARLYSTOPAT = 27;
    private ControlListView controlListViewSetup;
    private ColumnHeader columnHeader1;
    private ColumnHeader columnHeader2;
    private ColumnHeader columnHeader3;
    private ColumnHeader columnHeader4;
    private ColumnHeader columnHeader5;
    private ColumnHeader columnHeader6;
    private ColumnHeader columnHeader7;
    private ColumnHeader columnHeader8;
    private ColumnHeader columnHeader9;
    private ContextMenuStrip boxContextMenu;
    private ToolStripMenuItem prepaidToolStripMenuItem;
    private ToolStripMenuItem removePrepaidToolStripMenuItem;
    private PaymentScheduleSnapshot paySnapshot;
    private Label labelPrepaidColor;
    private TextBox currentBox;
    private Color PREPAIDCOLOR = Color.FromArgb(221, 233, 249);
    private GroupContainer groupContainer1;
    private CheckBox chkHUD49;
    private ToolTip toolTip1;
    private ColumnHeader columnHeader10;
    private Color NOPREPAIDCOLOR = Color.White;

    public HUD1ESSetupDialog(PaymentScheduleSnapshot paySnapshot)
    {
      this.paySnapshot = paySnapshot;
      this.InitializeComponent();
      this.labelPrepaidColor.BackColor = this.PREPAIDCOLOR;
      this.initializeHudItems();
      this.initializeDataGrid();
      this.okBtn.Visible = false;
      this.clearBtn.Visible = false;
      this.cancelBtn.Text = "OK";
      this.chkHUD49.Visible = false;
    }

    public HUD1ESSetupDialog(LoanData loan)
    {
      this.paySnapshot = (PaymentScheduleSnapshot) null;
      this.loan = loan;
      this.InitializeComponent();
      this.labelPrepaidColor.BackColor = this.PREPAIDCOLOR;
      this.initializeHudItems();
      this.initializeDataGrid();
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
      this.cancelBtn = new Button();
      this.okBtn = new Button();
      this.clearBtn = new Button();
      this.controlListViewSetup = new ControlListView();
      this.columnHeader1 = new ColumnHeader();
      this.columnHeader2 = new ColumnHeader();
      this.columnHeader3 = new ColumnHeader();
      this.columnHeader4 = new ColumnHeader();
      this.columnHeader5 = new ColumnHeader();
      this.columnHeader6 = new ColumnHeader();
      this.columnHeader7 = new ColumnHeader();
      this.columnHeader8 = new ColumnHeader();
      this.columnHeader9 = new ColumnHeader();
      this.columnHeader10 = new ColumnHeader();
      this.boxContextMenu = new ContextMenuStrip(this.components);
      this.prepaidToolStripMenuItem = new ToolStripMenuItem();
      this.removePrepaidToolStripMenuItem = new ToolStripMenuItem();
      this.labelPrepaidColor = new Label();
      this.groupContainer1 = new GroupContainer();
      this.chkHUD49 = new CheckBox();
      this.toolTip1 = new ToolTip(this.components);
      this.boxContextMenu.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.cancelBtn.Anchor = AnchorStyles.Bottom;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(649, 401);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 24);
      this.cancelBtn.TabIndex = 24;
      this.cancelBtn.Text = "&Cancel";
      this.okBtn.Anchor = AnchorStyles.Bottom;
      this.okBtn.Location = new Point(404, 401);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 24);
      this.okBtn.TabIndex = 23;
      this.okBtn.Text = "&OK";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.clearBtn.Anchor = AnchorStyles.Bottom;
      this.clearBtn.Location = new Point(485, 401);
      this.clearBtn.Name = "clearBtn";
      this.clearBtn.Size = new Size(158, 24);
      this.clearBtn.TabIndex = 25;
      this.clearBtn.Text = "Clear &All Impound Settings";
      this.clearBtn.Click += new EventHandler(this.clearBtn_Click);
      this.controlListViewSetup.BorderStyle = BorderStyle.None;
      this.controlListViewSetup.Columns.AddRange(new ColumnHeader[10]
      {
        this.columnHeader1,
        this.columnHeader2,
        this.columnHeader3,
        this.columnHeader4,
        this.columnHeader5,
        this.columnHeader6,
        this.columnHeader7,
        this.columnHeader8,
        this.columnHeader9,
        this.columnHeader10
      });
      this.controlListViewSetup.Dock = DockStyle.Fill;
      this.controlListViewSetup.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.controlListViewSetup.GridLines = true;
      this.controlListViewSetup.HeaderStyle = ColumnHeaderStyle.Nonclickable;
      this.controlListViewSetup.Location = new Point(1, 26);
      this.controlListViewSetup.Name = "controlListViewSetup";
      this.controlListViewSetup.Size = new Size(712, 335);
      this.controlListViewSetup.SubControlFont = new Font("Microsoft Sans Serif", 8f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.controlListViewSetup.TabIndex = 26;
      this.controlListViewSetup.UseCompatibleStateImageBehavior = false;
      this.controlListViewSetup.View = View.Details;
      this.controlListViewSetup.ColumnWidthChanged += new ColumnWidthChangedEventHandler(this.controlListViewSetup_ColumnWidthChanged);
      this.columnHeader1.Text = "Date";
      this.columnHeader1.Width = 86;
      this.columnHeader2.Text = "Tax";
      this.columnHeader2.Width = 67;
      this.columnHeader3.Text = "Haz Ins";
      this.columnHeader3.Width = 67;
      this.columnHeader4.Text = "Mtg Ins";
      this.columnHeader4.Width = 67;
      this.columnHeader5.Text = "Fld Ins";
      this.columnHeader5.Width = 67;
      this.columnHeader6.Text = "City Taxes";
      this.columnHeader6.Width = 70;
      this.columnHeader7.Text = "User #1";
      this.columnHeader7.Width = 67;
      this.columnHeader8.Text = "User #2";
      this.columnHeader8.Width = 67;
      this.columnHeader9.Text = "User #3";
      this.columnHeader9.Width = 67;
      this.columnHeader10.Text = "Annual Fee";
      this.columnHeader10.Width = 75;
      this.boxContextMenu.Items.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.prepaidToolStripMenuItem,
        (ToolStripItem) this.removePrepaidToolStripMenuItem
      });
      this.boxContextMenu.Name = "boxContextMenu";
      this.boxContextMenu.Size = new Size(161, 48);
      this.prepaidToolStripMenuItem.Name = "prepaidToolStripMenuItem";
      this.prepaidToolStripMenuItem.Size = new Size(160, 22);
      this.prepaidToolStripMenuItem.Text = "Prepaid";
      this.prepaidToolStripMenuItem.Click += new EventHandler(this.boxMenuItem_Click);
      this.removePrepaidToolStripMenuItem.Name = "removePrepaidToolStripMenuItem";
      this.removePrepaidToolStripMenuItem.Size = new Size(160, 22);
      this.removePrepaidToolStripMenuItem.Text = "Remove Prepaid";
      this.removePrepaidToolStripMenuItem.Click += new EventHandler(this.boxMenuItem_Click);
      this.labelPrepaidColor.BackColor = Color.Goldenrod;
      this.labelPrepaidColor.BorderStyle = BorderStyle.FixedSingle;
      this.labelPrepaidColor.Location = new Point(607, 5);
      this.labelPrepaidColor.Name = "labelPrepaidColor";
      this.labelPrepaidColor.Size = new Size(100, 16);
      this.labelPrepaidColor.TabIndex = 28;
      this.labelPrepaidColor.Text = "Prepaid";
      this.labelPrepaidColor.TextAlign = ContentAlignment.MiddleCenter;
      this.groupContainer1.Controls.Add((Control) this.controlListViewSetup);
      this.groupContainer1.Controls.Add((Control) this.labelPrepaidColor);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(10, 10);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(714, 362);
      this.groupContainer1.TabIndex = 29;
      this.groupContainer1.Text = "First Payment Date:";
      this.chkHUD49.AutoSize = true;
      this.chkHUD49.Enabled = false;
      this.chkHUD49.Location = new Point(10, 378);
      this.chkHUD49.Name = "chkHUD49";
      this.chkHUD49.Size = new Size(270, 17);
      this.chkHUD49.TabIndex = 30;
      this.chkHUD49.Text = "Servicer to refund Mtg Ins Cushion upon termination";
      this.toolTip1.SetToolTip((Control) this.chkHUD49, "HUD49");
      this.chkHUD49.UseVisualStyleBackColor = true;
      this.AcceptButton = (IButtonControl) this.okBtn;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(734, 437);
      this.Controls.Add((Control) this.chkHUD49);
      this.Controls.Add((Control) this.groupContainer1);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.okBtn);
      this.Controls.Add((Control) this.clearBtn);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (HUD1ESSetupDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Initial Escrow Account Setup";
      this.KeyPress += new KeyPressEventHandler(this.HUD1ESSetupDialog_KeyPress);
      this.boxContextMenu.ResumeLayout(false);
      this.groupContainer1.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void initializeHudItems()
    {
      this.chkHUD49.Checked = this.getFieldFromLoan("HUD49") == "Y";
      if (Utils.ParseInt((object) this.getFieldFromLoan("HUD32")) > 0)
        this.chkHUD49.Enabled = true;
      this.isBiweekly = this.getFieldFromLoan("423") == "Biweekly";
      this.hudItems = !this.isBiweekly ? new HUDItem[29] : new HUDItem[53];
      this.hudItems[0] = new HUDItem("Cushion", this.getFieldFromLoan("HUD30"), this.getFieldFromLoan("HUD31"), this.getFieldFromLoan("HUD32"), this.getFieldFromLoan("HUD33"), this.getFieldFromLoan("HUD34"), this.getFieldFromLoan("HUD35"), this.getFieldFromLoan("HUD36"), this.getFieldFromLoan("HUD37"), this.getFieldFromLoan("HUD38"));
      for (int index = 1; index <= 4; ++index)
      {
        string str = "HUD" + index.ToString("00");
        this.hudItems[index] = this.loan == null || !this.loan.IsTemplate ? new HUDItem("Due Date " + index.ToString(), this.getDateField(str + "41"), this.getDateField(str + "42"), this.getDateField(str + "43"), this.getDateField(str + "44"), this.getDateField(str + "45"), this.getDateField(str + "46"), this.getDateField(str + "47"), this.getDateField(str + "48"), this.getDateField(str + "49")) : new HUDItem("Due Date " + index.ToString(), "", "", "", "", "", "", "", "", "");
      }
      string fieldFromLoan = this.getFieldFromLoan("HUD68");
      DateTime dateTime = DateTime.Now;
      if (fieldFromLoan != string.Empty && fieldFromLoan != "//")
        dateTime = Utils.ParseDate((object) fieldFromLoan);
      this.firstPaymentDate = dateTime;
      int num1 = 0;
      string[] strArray = new string[9];
      for (int index1 = 5; index1 < 53; ++index1)
      {
        string empty1 = string.Empty;
        string disburseDate = !(dateTime == DateTime.MinValue) ? (!this.isBiweekly ? dateTime.ToString("MM/yyyy").ToUpper() : dateTime.ToString("MM/dd/yyyy")) : string.Empty;
        string empty2 = string.Empty;
        for (int index2 = 0; index2 < 9; ++index2)
        {
          num1 = index1 - 4;
          int num2 = index2 + 13;
          string id = "HUD" + num1.ToString("00") + num2.ToString("00");
          strArray[index2] = this.getFieldFromLoan(id);
        }
        this.hudItems[index1] = this.loan == null || !this.loan.IsTemplate ? new HUDItem(disburseDate, strArray[0], strArray[1], strArray[2], strArray[3], strArray[4], strArray[5], strArray[6], strArray[7], strArray[8]) : new HUDItem(string.Concat((object) num1), strArray[0], strArray[1], strArray[2], strArray[3], strArray[4], strArray[5], strArray[6], strArray[7], strArray[8]);
        if (dateTime != DateTime.MinValue)
          dateTime = !this.isBiweekly ? dateTime.AddMonths(1) : dateTime.AddDays(14.0);
        if (!this.isBiweekly && index1 > 27)
          break;
      }
    }

    private string getDateField(string id)
    {
      string empty = string.Empty;
      string str = this.paySnapshot == null ? this.loan.GetField(id) : this.getFieldFromLoan(id);
      return str == "//" ? string.Empty : str;
    }

    private void initializeDataGrid()
    {
      this.controlListViewSetup.Items.Clear();
      this.controlListViewSetup.BeginUpdate();
      for (int index = 0; index < this.hudItems.Length; ++index)
        this.controlListViewSetup.Items.Add(new ListViewItem(this.hudItems[index].DisburseDate)
        {
          SubItems = {
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
          },
          Tag = (object) this.hudItems[index]
        });
      int num1 = 0;
      string id = string.Empty;
      for (int index1 = 0; index1 < this.hudItems.Length; ++index1)
      {
        HUDItem tag = (HUDItem) this.controlListViewSetup.Items[index1].Tag;
        for (int index2 = 1; index2 <= 9; ++index2)
        {
          TextBox textBox = new TextBox();
          textBox.BorderStyle = BorderStyle.None;
          textBox.Location = new Point(1, 1);
          textBox.Name = "box_" + (object) num1;
          textBox.TextAlign = HorizontalAlignment.Center;
          textBox.Size = new Size(this.controlListViewSetup.Columns[index2].Width - 2, 12);
          textBox.TabIndex = num1++;
          TextBoxPosition textBoxPosition = new TextBoxPosition(index1, index2);
          switch (index2)
          {
            case 1:
              textBoxPosition.Val = tag.Tax;
              break;
            case 2:
              textBoxPosition.Val = tag.Haz;
              break;
            case 3:
              textBoxPosition.Val = tag.Mtg;
              break;
            case 4:
              textBoxPosition.Val = tag.Flood;
              break;
            case 5:
              textBoxPosition.Val = tag.CityTax;
              break;
            case 6:
              textBoxPosition.Val = tag.User1;
              break;
            case 7:
              textBoxPosition.Val = tag.User2;
              break;
            case 8:
              textBoxPosition.Val = tag.User3;
              break;
            case 9:
              textBoxPosition.Val = tag.USDAAnnualFee;
              break;
          }
          int num2;
          if (index1 >= 5)
          {
            int num3 = index1 - 4;
            num2 = index2 + 49;
            if (this.getFieldFromLoan("HUD" + num3.ToString("00") + num2.ToString("00")) == "Y")
              textBox.BackColor = this.PREPAIDCOLOR;
            num2 = index2 + 12;
            id = "HUD" + num3.ToString("00") + num2.ToString("00");
          }
          else if (index1 >= 1 && index1 <= 4)
          {
            num2 = index2 + 40;
            id = "HUD" + index1.ToString("00") + num2.ToString("00");
          }
          else if (index1 == 0)
          {
            num2 = index2 + 29;
            id = "HUD" + num2.ToString("00");
          }
          textBox.Tag = (object) textBoxPosition;
          if (this.paySnapshot != null || this.loan != null && this.loan.IsFieldReadOnly(id) || this.loan != null && this.loan.IsTemplate && index1 > 0 && index1 < 5)
          {
            textBox.ReadOnly = true;
            textBox.TabStop = false;
          }
          this.setBoxValue(textBox, textBoxPosition.Val);
          textBox.TextChanged += new EventHandler(this.textBox_TextChanged);
          textBox.KeyPress += new KeyPressEventHandler(this.textBox_KeyPress);
          textBox.Leave += new EventHandler(this.textBox_Leave);
          textBox.Enter += new EventHandler(this.textBox_Enter);
          textBox.MouseDown += new MouseEventHandler(this.textBox_MouseDown);
          textBox.KeyDown += new KeyEventHandler(this.textBox_KeyDown);
          if (index1 >= 1 && index1 <= 4)
            textBox.KeyUp += new KeyEventHandler(this.textBox_KeyUp);
          else if (index1 > 4)
            textBox.ContextMenuStrip = this.boxContextMenu;
          this.controlListViewSetup.AddEmbeddedControl((Control) textBox, index2, index1, DockStyle.None);
        }
      }
      this.controlListViewSetup.EndUpdate();
      DateTime date = Utils.ParseDate((object) this.getFieldFromLoan("HUD68"));
      this.groupContainer1.Text = "Escrow First Payment Date: " + (date != DateTime.MinValue ? date.ToString("MM/dd/yyyy") : "");
    }

    private void textBox_KeyDown(object sender, KeyEventArgs e)
    {
      TextBoxPosition tag = (TextBoxPosition) ((Control) sender).Tag;
      if (e.KeyCode != Keys.Home && e.KeyCode != Keys.Down && e.KeyCode != Keys.Up && e.KeyCode != Keys.Left && e.KeyCode != Keys.Right)
        return;
      int num = tag.Row;
      int col = tag.Col;
      switch (e.KeyCode)
      {
        case Keys.Home:
          num = 0;
          col = 1;
          break;
        case Keys.Left:
          --col;
          break;
        case Keys.Up:
          --num;
          break;
        case Keys.Right:
          ++col;
          break;
        case Keys.Down:
          ++num;
          break;
        default:
          return;
      }
      if (num < 0)
        num = 0;
      if (col < 1)
        col = 9;
      if (num >= this.hudItems.Length)
        num = 0;
      if (col > 9)
        col = 1;
      TextBox embeddedControl = (TextBox) this.controlListViewSetup.GetEmbeddedControl(col, num);
      if (embeddedControl == null)
        return;
      this.controlListViewSetup.Items[num].EnsureVisible();
      embeddedControl.BringToFront();
      embeddedControl.Focus();
    }

    private void textBox_MouseDown(object sender, MouseEventArgs e) => ((Control) sender).Focus();

    private void textBox_Enter(object sender, EventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      TextBoxPosition tag = (TextBoxPosition) textBox.Tag;
      string id = "HUD";
      this.currentBox = (TextBox) null;
      if (tag.Row == 0)
      {
        int num = tag.Col + 29;
        id += num.ToString();
      }
      else if (tag.Row >= 1 && tag.Row <= 4)
      {
        int num = tag.Col + 40;
        id = id + tag.Row.ToString("00") + num.ToString("00");
      }
      else if (tag.Row > 4)
      {
        int num1 = tag.Col + 12;
        int num2 = tag.Row - 4;
        id = id + num2.ToString("00") + num1.ToString("00");
        this.currentBox = textBox;
      }
      if (this.loan == null || this.loan.IsTemplate)
        return;
      Session.Application.GetService<IStatusDisplay>().DisplayFieldID(id);
    }

    private void textBox_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (this.paySnapshot != null)
        return;
      char keyChar = e.KeyChar;
      if (keyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        return;
      TextBox textBox = (TextBox) sender;
      TextBoxPosition tag = (TextBoxPosition) textBox.Tag;
      if (tag.Row >= 1 && tag.Row <= 4)
        return;
      if (tag.Row == 0)
      {
        keyChar = e.KeyChar;
        if (!keyChar.Equals('0'))
        {
          keyChar = e.KeyChar;
          if (!keyChar.Equals('1'))
          {
            keyChar = e.KeyChar;
            if (!keyChar.Equals('2'))
            {
              if (this.loan.GetField("423") == "Biweekly")
              {
                keyChar = e.KeyChar;
                if (keyChar.Equals('4'))
                  goto label_12;
              }
              if (this.isBiweekly)
              {
                int num1 = (int) Utils.Dialog((IWin32Window) this, "The cushion value can only be '0', '1', '2' and '4'.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              }
              else
              {
                int num2 = (int) Utils.Dialog((IWin32Window) this, "The cushion value can only be '0', '1' and '2'.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              }
              e.Handled = true;
              return;
            }
          }
        }
label_12:
        if (textBox.Text.Trim().Length > 0)
          e.Handled = true;
        else
          e.Handled = false;
      }
      else
      {
        if (!char.IsDigit(e.KeyChar))
        {
          keyChar = e.KeyChar;
          if (!keyChar.Equals('('))
          {
            keyChar = e.KeyChar;
            if (!keyChar.Equals(')'))
            {
              e.Handled = true;
              goto label_24;
            }
          }
        }
        e.Handled = false;
label_24:
        if (textBox.Text.Length <= 1)
          return;
        e.Handled = true;
      }
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      if (this.paySnapshot != null)
      {
        this.DialogResult = DialogResult.Cancel;
      }
      else
      {
        int num1 = 17;
        if (this.isBiweekly)
          num1 = 21;
        for (int row = num1; row < 49; ++row)
        {
          if (this.getBoxValue((TextBox) this.controlListViewSetup.GetEmbeddedControl(3, row)) != string.Empty)
          {
            if (Utils.Dialog((IWin32Window) this, "Mtg Ins escrows should not exceed 12 months. Continue anyway?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
              return;
            break;
          }
          if (!this.isBiweekly && row > 27)
            break;
        }
        string empty1 = string.Empty;
        for (int index = 30; index < 39; ++index)
        {
          TextBox embeddedControl = (TextBox) this.controlListViewSetup.GetEmbeddedControl(index - 29, 0);
          this.loan.SetCurrentField("HUD" + index.ToString("00"), this.getBoxValue(embeddedControl));
        }
        string empty2 = string.Empty;
        int num2;
        if (!this.loan.IsTemplate)
        {
          for (int row = 1; row <= 4; ++row)
          {
            for (int col = 1; col < 10; ++col)
            {
              num2 = col + 40;
              string id = "HUD" + row.ToString("00") + num2.ToString("00");
              TextBox embeddedControl = (TextBox) this.controlListViewSetup.GetEmbeddedControl(col, row);
              if (this.getBoxValue(embeddedControl) != "")
              {
                DateTime date = Utils.ParseDate((object) this.getBoxValue(embeddedControl));
                if (date == DateTime.MinValue || date.Year < 1900 || date.Year > 2199)
                {
                  int num3 = (int) Utils.Dialog((IWin32Window) this, "The date year range should be between '1900 - 2199'.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                  embeddedControl.Focus();
                  return;
                }
              }
              this.loan.SetCurrentField(id, this.getBoxValue(embeddedControl));
            }
          }
        }
        for (int index = 5; index < 53; ++index)
        {
          empty2 = string.Empty;
          for (int col = 0; col < 10; ++col)
          {
            int num4 = index - 4;
            num2 = col + 12;
            string id1 = "HUD" + num4.ToString("00") + num2.ToString("00");
            if (col == 0)
            {
              if (!this.loan.IsTemplate)
                this.loan.SetCurrentField(id1, this.controlListViewSetup.Items[index].Text);
            }
            else
            {
              TextBox embeddedControl = (TextBox) this.controlListViewSetup.GetEmbeddedControl(col, index);
              this.loan.SetCurrentField(id1, this.getBoxValue(embeddedControl));
              num2 = col + 49;
              string id2 = "HUD" + num4.ToString("00") + num2.ToString("00");
              if (embeddedControl.BackColor == this.PREPAIDCOLOR)
                this.loan.SetCurrentField(id2, "Y");
              else
                this.loan.SetCurrentField(id2, "");
            }
          }
          if (!this.isBiweekly && index > 27)
            break;
        }
        this.loan.SetCurrentField("HUD49", this.chkHUD49.Checked ? "Y" : "");
        this.loan.SetCurrentField("HUD23", "");
        bool flag = true;
        for (int index = 1; index <= 9; ++index)
        {
          if (this.loan.GetField("HUD014" + (object) index) != "" && this.loan.GetField("HUD014" + (object) index) != "//")
          {
            flag = false;
            break;
          }
        }
        if (flag)
        {
          for (int index = 132; index <= 140; ++index)
            this.loan.SetCurrentField("NEWHUD2.X" + (object) index, "");
          for (int index = 337; index <= 342; ++index)
            this.loan.SetCurrentField("NEWHUD.X" + (object) index, "");
          this.loan.SetCurrentField("NEWHUD.X1726", "");
          this.loan.SetCurrentField("NEWHUD2.X4769", "");
          this.loan.SetCurrentField("NEWHUD.X1728", "");
        }
        this.DialogResult = DialogResult.OK;
      }
    }

    private void textBox_Leave(object sender, EventArgs e)
    {
      if (this.paySnapshot != null)
        return;
      TextBox box = (TextBox) sender;
      TextBoxPosition tag = (TextBoxPosition) box.Tag;
      if (this.paySnapshot != null)
        return;
      if (box.Text != "*")
        this.setBoxValue(box, box.Text);
      if (tag.Row == 0 && tag.Col == 3)
      {
        if ((double) Utils.ParseInt((object) box.Text) > 0.0)
          this.chkHUD49.Enabled = true;
        else
          this.chkHUD49.Checked = this.chkHUD49.Enabled = false;
      }
      if (this.loan.IsTemplate)
        return;
      if (tag.Row >= 1 && tag.Row <= 4)
      {
        DateTime date1 = Utils.ParseDate((object) this.getBoxValue(box));
        if (date1 == DateTime.MinValue)
          this.setBoxValue(box, "");
        for (int row = tag.Row - 1; row >= 1; --row)
        {
          DateTime date2 = Utils.ParseDate((object) this.getBoxValue((TextBox) this.controlListViewSetup.GetEmbeddedControl(tag.Col, row)));
          if (date1 < date2)
          {
            this.setBoxValue(box, "");
            break;
          }
        }
        if (box.Text.Trim() != string.Empty)
        {
          for (int row = tag.Row + 1; row <= 4; ++row)
          {
            TextBox embeddedControl = (TextBox) this.controlListViewSetup.GetEmbeddedControl(tag.Col, row);
            DateTime date3 = Utils.ParseDate((object) this.getBoxValue(embeddedControl));
            if (date1 > date3)
              this.setBoxValue(embeddedControl, "");
          }
        }
        for (int row1 = 1; row1 <= 4; ++row1)
        {
          TextBox embeddedControl1 = (TextBox) this.controlListViewSetup.GetEmbeddedControl(tag.Col, row1);
          if (this.getBoxValue(embeddedControl1) == string.Empty)
          {
            for (int row2 = row1 + 1; row2 <= 4; ++row2)
            {
              TextBox embeddedControl2 = (TextBox) this.controlListViewSetup.GetEmbeddedControl(tag.Col, row2);
              if (this.getBoxValue(embeddedControl2) != string.Empty)
              {
                this.setBoxValue(embeddedControl1, embeddedControl2.Text);
                this.setBoxValue(embeddedControl2, "");
                break;
              }
            }
          }
        }
      }
      int num1 = 24;
      if (this.isBiweekly)
        num1 = 48;
      int num2 = num1 + 5;
      int row3 = 0;
      if (tag.Row >= 1 && tag.Row <= 4)
      {
        int num3 = 1;
        for (int row4 = 1; row4 <= 4; ++row4)
        {
          if (this.getBoxValue((TextBox) this.controlListViewSetup.GetEmbeddedControl(tag.Col, row4)) != string.Empty)
            ++num3;
        }
        for (int row5 = 5; row5 < num2; ++row5)
          this.setBoxValue((TextBox) this.controlListViewSetup.GetEmbeddedControl(tag.Col, row5), "");
        if (num3 == 1)
          return;
        int num4 = 12 / (num3 - 1);
        for (int row6 = 1; row6 <= 4; ++row6)
        {
          DateTime date = Utils.ParseDate((object) this.getBoxValue((TextBox) this.controlListViewSetup.GetEmbeddedControl(tag.Col, row6)));
          if (date == DateTime.MinValue)
            break;
          DateTime dateTime = this.firstPaymentDate;
          for (int row7 = 5; row7 < num2; ++row7)
          {
            if (this.isBiweekly)
            {
              if (dateTime > date)
              {
                if (row7 != 5)
                {
                  this.setBoxValue((TextBox) this.controlListViewSetup.GetEmbeddedControl(tag.Col, row7 - 1), num4.ToString());
                  break;
                }
                break;
              }
              dateTime = dateTime.AddDays(14.0);
            }
            else
            {
              if (dateTime.Month == date.Month && dateTime.Year == date.Year)
              {
                this.setBoxValue((TextBox) this.controlListViewSetup.GetEmbeddedControl(tag.Col, row7), num4.ToString());
                break;
              }
              dateTime = dateTime.AddMonths(1);
            }
          }
        }
      }
      else
      {
        if (tag.Row <= 4)
          return;
        for (int row8 = 5; row8 < num2; ++row8)
        {
          if (this.getBoxValue((TextBox) this.controlListViewSetup.GetEmbeddedControl(tag.Col, row8)) != string.Empty)
          {
            ++row3;
            if (row3 <= 4)
            {
              DateTime dateTime = !this.isBiweekly ? this.firstPaymentDate.AddMonths(row8 - 5) : this.firstPaymentDate.AddDays((double) ((row8 - 5) * 14));
              TextBox embeddedControl = (TextBox) this.controlListViewSetup.GetEmbeddedControl(tag.Col, row3);
              DateTime date = Utils.ParseDate((object) this.getBoxValue(embeddedControl));
              if (date == DateTime.MinValue)
                this.setBoxValue(embeddedControl, dateTime.ToString("MM/dd/yyyy"));
              else if (this.isBiweekly && dateTime.Day != date.Day && date.Month > dateTime.Month && date.Year >= dateTime.Year)
                this.setBoxValue(embeddedControl, date.ToString("MM/dd/yyyy"));
              else
                this.setBoxValue(embeddedControl, dateTime.Month.ToString("00") + "/" + date.Day.ToString("00") + "/" + dateTime.Year.ToString("0000"));
            }
            else
              break;
          }
        }
        if (row3 >= 4)
          return;
        for (int row9 = row3 + 1; row9 <= 4; ++row9)
          this.setBoxValue((TextBox) this.controlListViewSetup.GetEmbeddedControl(tag.Col, row9), "");
      }
    }

    private void setBoxValue(TextBox box, string val)
    {
      TextBoxPosition tag = (TextBoxPosition) box.Tag;
      int num1 = tag.Row;
      int col = tag.Col;
      int num2;
      if (num1 >= 1 && num1 <= 4)
        num2 = tag.Col + 40;
      else if (num1 >= 5)
      {
        num1 = tag.Row - 4;
        num2 = tag.Col + 12;
      }
      else
        num2 = tag.Col + 29;
      string id = "HUD" + (num1 == 0 ? "" : num1.ToString("00")) + num2.ToString("00");
      if (this.loan != null && this.loan.IsTemplate)
        box.Text = val;
      else if (Session.LoanDataMgr.GetFieldAccessRights(id) == BizRule.FieldAccessRight.Hide)
        box.Text = "*";
      else
        box.Text = val;
      tag.Val = val;
    }

    private string getBoxValue(TextBox box)
    {
      return box.Text == "*" ? ((TextBoxPosition) box.Tag).Val.Trim() : box.Text.Trim();
    }

    private void textBox_TextChanged(object sender, EventArgs e)
    {
      if (this.paySnapshot != null)
        return;
      TextBox box = (TextBox) sender;
      TextBoxPosition tag = (TextBoxPosition) box.Tag;
      if (tag.Row < 1 || tag.Row > 4)
        return;
      FieldFormat dataFormat = FieldFormat.DATE;
      bool needsUpdate = false;
      string val = Utils.FormatInput(box.Text, dataFormat, ref needsUpdate);
      if (!needsUpdate)
        return;
      this.setBoxValue(box, val);
      box.SelectionStart = val.Length;
    }

    private void textBox_KeyUp(object sender, KeyEventArgs e)
    {
      if (!e.Control || e.KeyCode != Keys.D)
        return;
      this.setBoxValue((TextBox) sender, DateTime.Today.ToString("MM/dd/yyyy"));
    }

    private void clearBtn_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Are you sure you want to clear all impounding settings?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
        return;
      for (int row = 0; row < this.hudItems.Length; ++row)
      {
        for (int col = 1; col < 10; ++col)
        {
          TextBox embeddedControl = (TextBox) this.controlListViewSetup.GetEmbeddedControl(col, row);
          this.setBoxValue(embeddedControl, string.Empty);
          embeddedControl.BackColor = this.NOPREPAIDCOLOR;
        }
      }
    }

    private string getFieldFromLoan(string id)
    {
      return this.paySnapshot != null ? this.paySnapshot.GetField(id) : this.loan.GetSimpleField(id);
    }

    private void boxMenuItem_Click(object sender, EventArgs e)
    {
      ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem) sender;
      if (toolStripMenuItem.Text == "Prepaid")
      {
        if (this.currentBox == null)
          return;
        this.currentBox.BackColor = this.PREPAIDCOLOR;
      }
      else
      {
        if (!(toolStripMenuItem.Text == "Remove Prepaid") || this.currentBox == null)
          return;
        this.currentBox.BackColor = this.NOPREPAIDCOLOR;
      }
    }

    private void controlListViewSetup_ColumnWidthChanged(
      object sender,
      ColumnWidthChangedEventArgs e)
    {
      int width = this.controlListViewSetup.Columns[e.ColumnIndex].Width;
      for (int row = 0; row < this.hudItems.Length; ++row)
      {
        Control embeddedControl = this.controlListViewSetup.GetEmbeddedControl(e.ColumnIndex, row);
        if (embeddedControl == null)
          break;
        embeddedControl.Width = width - 2;
      }
    }

    private void HUD1ESSetupDialog_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\u001B')
        return;
      this.DialogResult = DialogResult.Cancel;
    }
  }
}
