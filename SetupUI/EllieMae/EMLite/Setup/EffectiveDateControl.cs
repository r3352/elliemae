// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.EffectiveDateControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class EffectiveDateControl : UserControl
  {
    protected EffectiveDateOperator operPleaseSelect = new EffectiveDateOperator("Please select");
    protected EffectiveDateOperator operBetween = new EffectiveDateOperator("Between");
    protected EffectiveDateOperator operBlank = new EffectiveDateOperator("Blank");
    protected EffectiveDateOperator operBlankOnOrAfter = new EffectiveDateOperator("Blank>=");
    protected EffectiveDateOperator operEquals = new EffectiveDateOperator("=");
    protected EffectiveDateOperator operAfter = new EffectiveDateOperator(">");
    protected EffectiveDateOperator operOnOrAfter = new EffectiveDateOperator(">=");
    protected EffectiveDateOperator operLTE = new EffectiveDateOperator("<=");
    protected EffectiveDateOperator operLT = new EffectiveDateOperator("<");
    private Sessions.Session session;
    private FieldDropdownItem currentEffectiveDateItem;
    private IContainer components;
    private ComboBox effectiveDateFieldComboBox;
    private ComboBox operatorComboBox;
    private TextBox startDateTextbox;
    private CalendarButton startDateCalendarButton;
    private Label label1;
    private Label labelCriteria;
    private Label labelDate;
    private TextBox endDateTextbox;
    private CalendarButton endDateCalendarButton;

    public EffectiveDateControl(Sessions.Session session)
    {
      this.InitializeComponent();
      this.session = session;
      this.LoadEffectiveDateDropDownDefaults();
      this.LoadEffectiveDateOperators();
    }

    public event EventHandler ChangesMadeToEffectiveDate;

    private void operatorComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.operatorComboBox.SelectedItem != null)
      {
        EffectiveDateOperator selectedItem = this.operatorComboBox.SelectedItem as EffectiveDateOperator;
        if (selectedItem.Operator.Equals("Between"))
        {
          this.handleStartDateControls(true, true);
          this.handleEndDateControls(true, true);
        }
        else if (selectedItem.Operator.Equals("Blank"))
        {
          this.handleStartDateControls(true, false);
          this.handleEndDateControls(false, false);
        }
        else
        {
          this.handleStartDateControls(true, true);
          this.handleEndDateControls(false, false);
        }
      }
      this.effectiveDate_Changed(sender, e);
    }

    private void handleStartDateControls(bool visible, bool enabled)
    {
      if (!visible || !enabled)
        this.StartDate = DateTime.MinValue;
      this.startDateTextbox.Visible = visible;
      this.startDateTextbox.Enabled = enabled;
      this.startDateCalendarButton.Visible = visible;
      this.startDateCalendarButton.Enabled = enabled;
    }

    private void handleEndDateControls(bool visible, bool enabled)
    {
      if (!visible || !enabled)
        this.EndDate = DateTime.MinValue;
      this.endDateTextbox.Visible = visible;
      this.endDateTextbox.Enabled = enabled;
      this.endDateCalendarButton.Visible = visible;
      this.endDateCalendarButton.Enabled = enabled;
    }

    public string EffectiveDateFieldId
    {
      get
      {
        return this.currentEffectiveDateItem != null ? this.currentEffectiveDateItem.ToString() : "Please select";
      }
      set
      {
        string[] source = value.Split(new char[1]{ '-' }, 2);
        if (((IEnumerable<string>) source).Count<string>() != 2)
          return;
        this.currentEffectiveDateItem = new FieldDropdownItem(source[0].Trim(), source[1].Trim());
        if (!this.effectiveDateFieldComboBox.Items.Contains((object) this.currentEffectiveDateItem))
          this.effectiveDateFieldComboBox.Items.Insert(this.effectiveDateFieldComboBox.Items.Count - 1, (object) this.currentEffectiveDateItem);
        this.effectiveDateFieldComboBox.SelectedItem = (object) this.currentEffectiveDateItem;
      }
    }

    public string Operator
    {
      get
      {
        return this.operatorComboBox.SelectedIndex != -1 ? (this.operatorComboBox.SelectedItem as EffectiveDateOperator).Operator : string.Empty;
      }
      set
      {
        switch (value)
        {
          case "<":
            this.operatorComboBox.SelectedItem = (object) this.operLT;
            break;
          case "<=":
            this.operatorComboBox.SelectedItem = (object) this.operLTE;
            break;
          case "=":
            this.operatorComboBox.SelectedItem = (object) this.operEquals;
            break;
          case ">":
            this.operatorComboBox.SelectedItem = (object) this.operAfter;
            break;
          case ">=":
            this.operatorComboBox.SelectedItem = (object) this.operOnOrAfter;
            break;
          case "Between":
            this.operatorComboBox.SelectedItem = (object) this.operBetween;
            break;
          case "Blank":
            this.operatorComboBox.SelectedItem = (object) this.operBlank;
            break;
          case "Blank>=":
            this.operatorComboBox.SelectedItem = (object) this.operBlankOnOrAfter;
            break;
          case "Please select":
            this.operatorComboBox.SelectedItem = (object) this.operPleaseSelect;
            break;
          default:
            this.operatorComboBox.SelectedItem = (object) this.operPleaseSelect;
            break;
        }
      }
    }

    public DateTime StartDate
    {
      get => Utils.ParseDate((object) this.startDateTextbox.Text);
      set => this.startDateTextbox.Text = Utils.DateFormat(value);
    }

    public DateTime EndDate
    {
      get => Utils.ParseDate((object) this.endDateTextbox.Text);
      set => this.endDateTextbox.Text = Utils.DateFormat(value);
    }

    public string EffectiveDateConcatenatedInfo
    {
      get => this.ToString();
      set
      {
        try
        {
          string[] strArray = value.Split('|');
          this.EffectiveDateFieldId = strArray[0] + "-" + strArray[1];
          this.Operator = strArray[2];
          this.StartDate = Convert.ToDateTime(strArray[3].Trim());
          this.EndDate = Convert.ToDateTime(strArray[4].Trim());
        }
        catch (Exception ex)
        {
        }
      }
    }

    private void effectiveDateFieldComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.effectiveDateFieldComboBox.SelectedItem == null)
        return;
      try
      {
        if (!(this.effectiveDateFieldComboBox.SelectedItem is FieldDropdownItem fieldDropdownItem) && (string) this.effectiveDateFieldComboBox.SelectedItem == "Other")
          fieldDropdownItem = this.selectField();
        if (fieldDropdownItem == null)
        {
          this.currentEffectiveDateItem = (FieldDropdownItem) null;
          this.StartDate = DateTime.MinValue;
          this.EndDate = DateTime.MinValue;
        }
        if (!this.effectiveDateFieldComboBox.Items.Contains((object) fieldDropdownItem))
          this.effectiveDateFieldComboBox.Items.Insert(this.effectiveDateFieldComboBox.Items.Count - 1, (object) fieldDropdownItem);
        if (!this.effectiveDateFieldComboBox.SelectedItem.Equals((object) fieldDropdownItem))
          this.effectiveDateFieldComboBox.SelectedItem = (object) fieldDropdownItem;
        if (!fieldDropdownItem.Equals((object) this.currentEffectiveDateItem))
          this.currentEffectiveDateItem = fieldDropdownItem;
        this.effectiveDate_Changed(sender, e);
      }
      catch (Exception ex)
      {
      }
    }

    private FieldDropdownItem selectField()
    {
      using (SelectOtherEffectiveDateForm effectiveDateForm = new SelectOtherEffectiveDateForm(this.session))
      {
        if (effectiveDateForm.ShowDialog((IWin32Window) this) == DialogResult.OK)
          return new FieldDropdownItem(effectiveDateForm.SelectedFIeldID, effectiveDateForm.SelectedFIeldDescription);
      }
      return (FieldDropdownItem) null;
    }

    private void LoadEffectiveDateDropDownDefaults()
    {
      this.effectiveDateFieldComboBox.Items.Add((object) "Please select");
      this.effectiveDateFieldComboBox.Items.Add((object) new FieldDropdownItem("2025", "Loan Created Date"));
      this.effectiveDateFieldComboBox.Items.Add((object) new FieldDropdownItem("3142", "Application Date"));
      this.effectiveDateFieldComboBox.Items.Add((object) new FieldDropdownItem("745", "Trans Details Application Date"));
      this.effectiveDateFieldComboBox.Items.Add((object) "Other");
      this.effectiveDateFieldComboBox.SelectedIndex = 0;
    }

    private void LoadEffectiveDateOperators()
    {
      this.operatorComboBox.Items.Clear();
      this.operatorComboBox.Items.AddRange((object[]) new EffectiveDateOperator[9]
      {
        this.operPleaseSelect,
        this.operBetween,
        this.operBlank,
        this.operBlankOnOrAfter,
        this.operEquals,
        this.operAfter,
        this.operOnOrAfter,
        this.operLTE,
        this.operLT
      });
      this.operatorComboBox.SelectedIndex = 0;
    }

    private void effectiveDate_Changed(object sender, EventArgs e)
    {
      if (this.ChangesMadeToEffectiveDate == null)
        return;
      this.ChangesMadeToEffectiveDate((object) this, EventArgs.Empty);
    }

    public static string GetDisplayString(string effectiveDateInfo)
    {
      if (string.IsNullOrEmpty(effectiveDateInfo))
        return string.Empty;
      string[] strArray = effectiveDateInfo.Split('|');
      string str1 = strArray[0];
      string str2 = strArray[1];
      string oper = strArray[2];
      string str3 = strArray[3];
      string str4 = strArray.Length > 4 ? strArray[4] : string.Empty;
      switch (oper)
      {
        case "Blank":
          return string.Format("{0} - {1} Is {2}", (object) strArray[0], (object) strArray[1], (object) EffectiveDateOperator.GetDisplayString(oper));
        case "Blank>=":
          return string.IsNullOrEmpty(str3) ? string.Format("{0} - {1} Is Blank", (object) strArray[0], (object) strArray[1]) : string.Format("{0} - {1} >= {2}", (object) strArray[0], (object) strArray[1], (object) str3);
        case "Between":
          return string.Format("{0} - {1} {2} {3} And {4}", (object) strArray[0], (object) strArray[1], (object) EffectiveDateOperator.GetDisplayString(oper), (object) str3, (object) str4);
        default:
          return string.Format("{0} - {1} {2} {3}", (object) strArray[0], (object) strArray[1], (object) EffectiveDateOperator.GetDisplayString(oper), (object) str3);
      }
    }

    public override string ToString()
    {
      try
      {
        return string.Format("{0}|{1}|{2}|{3}|{4}", (object) this.currentEffectiveDateItem.FieldID, (object) this.currentEffectiveDateItem.Description, (object) this.Operator, (object) Utils.DateFormat(this.StartDate), (object) Utils.DateFormat(this.EndDate));
      }
      catch
      {
      }
      return string.Empty;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (EffectiveDateControl));
      this.effectiveDateFieldComboBox = new ComboBox();
      this.operatorComboBox = new ComboBox();
      this.startDateTextbox = new TextBox();
      this.startDateCalendarButton = new CalendarButton();
      this.label1 = new Label();
      this.labelCriteria = new Label();
      this.labelDate = new Label();
      this.endDateTextbox = new TextBox();
      this.endDateCalendarButton = new CalendarButton();
      ((ISupportInitialize) this.startDateCalendarButton).BeginInit();
      ((ISupportInitialize) this.endDateCalendarButton).BeginInit();
      this.SuspendLayout();
      this.effectiveDateFieldComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
      this.effectiveDateFieldComboBox.FormattingEnabled = true;
      this.effectiveDateFieldComboBox.Location = new Point(82, 0);
      this.effectiveDateFieldComboBox.Name = "effectiveDateFieldComboBox";
      this.effectiveDateFieldComboBox.Size = new Size(281, 21);
      this.effectiveDateFieldComboBox.TabIndex = 0;
      this.effectiveDateFieldComboBox.SelectedIndexChanged += new EventHandler(this.effectiveDateFieldComboBox_SelectedIndexChanged);
      this.operatorComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
      this.operatorComboBox.FormattingEnabled = true;
      this.operatorComboBox.Items.AddRange(new object[6]
      {
        (object) "Please select",
        (object) "=",
        (object) ">",
        (object) ">=",
        (object) "<=",
        (object) "<"
      });
      this.operatorComboBox.Location = new Point(82, 25);
      this.operatorComboBox.Name = "operatorComboBox";
      this.operatorComboBox.Size = new Size(157, 21);
      this.operatorComboBox.TabIndex = 1;
      this.operatorComboBox.SelectedIndexChanged += new EventHandler(this.operatorComboBox_SelectedIndexChanged);
      this.startDateTextbox.Location = new Point(82, 50);
      this.startDateTextbox.Name = "startDateTextbox";
      this.startDateTextbox.Size = new Size(100, 20);
      this.startDateTextbox.TabIndex = 2;
      this.startDateTextbox.TextChanged += new EventHandler(this.effectiveDate_Changed);
      this.startDateCalendarButton.DateControl = (Control) this.startDateTextbox;
      ((IconButton) this.startDateCalendarButton).Image = (Image) componentResourceManager.GetObject("startDateCalendarButton.Image");
      this.startDateCalendarButton.Location = new Point(189, 50);
      this.startDateCalendarButton.MouseDownImage = (Image) null;
      this.startDateCalendarButton.Name = "startDateCalendarButton";
      this.startDateCalendarButton.Size = new Size(16, 16);
      this.startDateCalendarButton.SizeMode = PictureBoxSizeMode.AutoSize;
      this.startDateCalendarButton.TabIndex = 3;
      this.startDateCalendarButton.TabStop = false;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(0, 0);
      this.label1.Name = "label1";
      this.label1.Size = new Size(76, 13);
      this.label1.TabIndex = 9;
      this.label1.Text = "Effective date:";
      this.labelCriteria.AutoSize = true;
      this.labelCriteria.Location = new Point(0, 25);
      this.labelCriteria.Name = "labelCriteria";
      this.labelCriteria.Size = new Size(42, 13);
      this.labelCriteria.TabIndex = 10;
      this.labelCriteria.Text = "Criteria:";
      this.labelDate.AutoSize = true;
      this.labelDate.Location = new Point(0, 50);
      this.labelDate.Name = "labelDate";
      this.labelDate.Size = new Size(33, 13);
      this.labelDate.TabIndex = 11;
      this.labelDate.Text = "Date:";
      this.endDateTextbox.Location = new Point(240, 50);
      this.endDateTextbox.Name = "endDateTextbox";
      this.endDateTextbox.Size = new Size(100, 20);
      this.endDateTextbox.TabIndex = 12;
      this.endDateTextbox.TextChanged += new EventHandler(this.effectiveDate_Changed);
      this.endDateCalendarButton.DateControl = (Control) this.endDateTextbox;
      ((IconButton) this.endDateCalendarButton).Image = (Image) componentResourceManager.GetObject("endDateCalendarButton.Image");
      this.endDateCalendarButton.Location = new Point(346, 50);
      this.endDateCalendarButton.MouseDownImage = (Image) null;
      this.endDateCalendarButton.Name = "endDateCalendarButton";
      this.endDateCalendarButton.Size = new Size(16, 16);
      this.endDateCalendarButton.SizeMode = PictureBoxSizeMode.AutoSize;
      this.endDateCalendarButton.TabIndex = 13;
      this.endDateCalendarButton.TabStop = false;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.White;
      this.Controls.Add((Control) this.endDateCalendarButton);
      this.Controls.Add((Control) this.endDateTextbox);
      this.Controls.Add((Control) this.labelDate);
      this.Controls.Add((Control) this.labelCriteria);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.startDateCalendarButton);
      this.Controls.Add((Control) this.startDateTextbox);
      this.Controls.Add((Control) this.operatorComboBox);
      this.Controls.Add((Control) this.effectiveDateFieldComboBox);
      this.Name = nameof (EffectiveDateControl);
      this.Size = new Size(381, 79);
      ((ISupportInitialize) this.startDateCalendarButton).EndInit();
      ((ISupportInitialize) this.endDateCalendarButton).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
