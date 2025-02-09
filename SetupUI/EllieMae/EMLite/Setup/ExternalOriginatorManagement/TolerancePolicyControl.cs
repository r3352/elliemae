// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ExternalOriginatorManagement.TolerancePolicyControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.Setup.ExternalOriginatorManagement
{
  public class TolerancePolicyControl : UserControl
  {
    private const string _labelTemplate = "Policy for {0} Tolerance";
    private Hashtable toleranceTypes = new Hashtable();
    private string _label;
    private ExternalOriginatorCommitmentTolerancePolicy _policy;
    private Decimal _tolerancePct;
    private Decimal _toleranceAmt;
    private IContainer components;
    private GroupContainer groupContainer1;
    private ComboBox cmbTolerancePolicy;
    private System.Windows.Forms.Label PercentLbl;
    private System.Windows.Forms.Label DollarSignLbl;
    private System.Windows.Forms.Label label4;
    private TextBox TolerenceDollarLimitTxt;
    private TextBox TolerencePctLimitTxt;
    private System.Windows.Forms.Label OrLbl;
    private System.Windows.Forms.Label ToleranceLbl;

    public TolerancePolicyControl()
    {
      this.InitializeComponent();
      this.toleranceTypes.Add((object) 0, (object) string.Empty);
      this.toleranceTypes.Add((object) 1, (object) string.Empty);
      this.toleranceTypes.Add((object) 2, (object) string.Empty);
    }

    public string Label
    {
      set
      {
        this._label = value;
        if (this.groupContainer1 == null)
          return;
        this.groupContainer1.Text = string.Format("Policy for {0} Tolerance", (object) value);
      }
      get => this._label;
    }

    public TolerancePolicyControl.dirtyDelegate Dirty { get; set; }

    private void IsDirty(bool dirty)
    {
      TolerancePolicyControl.dirtyDelegate dirty1 = this.Dirty;
      if (dirty1 == null)
        return;
      dirty1(dirty);
    }

    public ExternalOriginatorCommitmentTolerancePolicy Policy
    {
      set
      {
        this._policy = value;
        if (this.cmbTolerancePolicy == null)
          return;
        if (this.Enabled)
          this.cmbTolerancePolicy.SelectedIndex = (int) value;
        else
          this.cmbTolerancePolicy.SelectedIndex = 0;
      }
      get
      {
        if (this.cmbTolerancePolicy == null)
          return ExternalOriginatorCommitmentTolerancePolicy.NoPolicy;
        return this.Enabled ? (ExternalOriginatorCommitmentTolerancePolicy) this.cmbTolerancePolicy.SelectedIndex : this._policy;
      }
    }

    public void SetFocus(string ctrl)
    {
      switch (ctrl)
      {
        case "TolerencePctLimitTxt":
          this.TolerencePctLimitTxt.Focus();
          break;
        case "TolerenceDollarLimitTxt":
          this.TolerenceDollarLimitTxt.Focus();
          break;
      }
    }

    private void cmbTolerancePolicy_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cmbTolerancePolicy.SelectedIndex == 0)
      {
        this.ToleranceLbl.Text = "";
        this.ToleranceLbl.Visible = false;
        this.TolerencePctLimitTxt.Visible = false;
        this.PercentLbl.Visible = false;
        this.TolerencePctLimitTxt.Visible = false;
        this.OrLbl.Visible = false;
        this.DollarSignLbl.Visible = false;
        this.TolerenceDollarLimitTxt.Visible = false;
      }
      else if (this.cmbTolerancePolicy.SelectedIndex == 1)
      {
        this.ToleranceLbl.Text = "Flat Tolerance %";
        this.ToleranceLbl.Visible = true;
        this.TolerencePctLimitTxt.Visible = true;
        this.PercentLbl.Visible = true;
        this.OrLbl.Visible = false;
        this.DollarSignLbl.Visible = false;
        this.TolerenceDollarLimitTxt.Visible = false;
      }
      else
      {
        this.ToleranceLbl.Text = "Lesser of";
        this.ToleranceLbl.Visible = true;
        this.TolerencePctLimitTxt.Visible = true;
        this.PercentLbl.Visible = true;
        this.OrLbl.Visible = true;
        this.DollarSignLbl.Visible = true;
        this.TolerenceDollarLimitTxt.Visible = true;
      }
      this.IsDirty(true);
      this.TolerencePctLimitTxt.Text = this.toleranceTypes[(object) this.cmbTolerancePolicy.SelectedIndex].ToString();
    }

    public Decimal TolerancePct
    {
      get
      {
        if (this.Enabled)
        {
          Decimal result = 0M;
          Decimal.TryParse(this.TolerencePctLimitTxt.Text.Replace(",", ""), out result);
          this._tolerancePct = result;
        }
        return this._tolerancePct;
      }
      set
      {
        this._tolerancePct = value;
        this.TolerencePctLimitTxt.Text = value > 0M ? value.ToString() : string.Empty;
        this.toleranceTypes[(object) this.cmbTolerancePolicy.SelectedIndex] = (object) this.TolerencePctLimitTxt.Text;
      }
    }

    public Decimal ToleranceAmt
    {
      get
      {
        if (this.Enabled)
        {
          Decimal result = 0M;
          Decimal.TryParse(this.TolerenceDollarLimitTxt.Text.Replace(",", ""), out result);
          this._toleranceAmt = result;
        }
        return this._toleranceAmt;
      }
      set
      {
        this._toleranceAmt = value;
        this.TolerenceDollarLimitTxt.Text = value > 0M ? value.ToString("###,###") : string.Empty;
      }
    }

    private void NumericTxt_KeyPress(object sender, KeyPressEventArgs e)
    {
      int num1 = -1;
      if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
        e.Handled = true;
      if (!char.IsControl(e.KeyChar) && sender is TextBox)
      {
        TextBox textBox = sender as TextBox;
        int result1 = 0;
        int result2 = 0;
        if (textBox.Tag != null && textBox.Tag is string tag && tag.Contains<char>('.'))
        {
          string[] strArray = tag.Split('.');
          int.TryParse(strArray[0], out result2);
          int.TryParse(strArray[1], out result1);
          if (result1 == 0 && e.KeyChar == '.')
          {
            e.Handled = true;
          }
          else
          {
            int num2 = 0;
            int num3 = 0;
            bool flag = false;
            num1 = textBox.Text.IndexOf('.');
            if (textBox.SelectionLength > 0)
            {
              if (num1 == -1)
                num2 = textBox.SelectionLength - textBox.SelectionStart;
              else if (textBox.SelectionStart < num1)
                num2 = Math.Min(textBox.SelectionLength, num1 - textBox.SelectionStart);
              int num4 = textBox.SelectionStart + textBox.SelectionLength - 1;
              if (num1 != -1 && num4 > num1)
                num3 = Math.Min(textBox.SelectionLength, num4 - num1);
              flag = num1 != -1 && textBox.SelectionStart <= num1 && num4 <= num1;
            }
            if (num1 >= 0 && (!flag || e.KeyChar != '.'))
            {
              if (num2 == 0 && textBox.Text.Length - num3 - num1 > result1)
                e.Handled = true;
              if (num1 - num2 > result2)
                e.Handled = true;
            }
            else if (e.KeyChar != '.' && textBox.Text.Length + 1 > result2)
              e.Handled = true;
          }
        }
        if (e.KeyChar == '.' && num1 > -1)
          e.Handled = true;
        if (!e.Handled && e.KeyChar != '.' && textBox.Name == "TolerencePctLimitTxt")
        {
          int selectionStart = textBox.SelectionStart;
          int startIndex = textBox.SelectionStart + textBox.SelectionLength;
          if (Convert.ToDouble(textBox.Text.Substring(0, selectionStart) + e.KeyChar.ToString() + textBox.Text.Substring(startIndex)) > 100.0)
            e.Handled = true;
        }
      }
      else
      {
        TextBox textBox = sender as TextBox;
        e.Handled = textBox.Name == "TolerencePctLimitTxt" && (e.KeyChar == '\b' || e.KeyChar == '.') && !string.IsNullOrEmpty(textBox.Text) && textBox.Text != "." && Convert.ToDouble(textBox.Text) <= 100.0 && (textBox.SelectedText.Length == 0 && textBox.SelectionStart > 0 && textBox.Text.Length > 1 && textBox.Text.Remove(textBox.SelectionStart - 1, 1) != "." && Convert.ToDouble(textBox.Text.Remove(textBox.SelectionStart - 1, 1)) > 100.0 || textBox.SelectedText.Length > 0 && textBox.SelectedText.Length != textBox.Text.Length && textBox.Text.Remove(textBox.SelectionStart, textBox.SelectedText.Length) != "." && Convert.ToDouble(textBox.Text.Remove(textBox.SelectionStart, textBox.SelectedText.Length)) > 100.0);
      }
      if (e.Handled)
        return;
      this.IsDirty(true);
    }

    private void TolerencePctLimitTxt_Leave(object sender, EventArgs e)
    {
      Decimal result1 = 0M;
      if (string.IsNullOrEmpty(this.TolerencePctLimitTxt.Text))
        this._tolerancePct = 0M;
      else if (Decimal.TryParse(this.TolerencePctLimitTxt.Text.Replace(",", ""), out result1))
      {
        if (this.TolerencePctLimitTxt.Tag != null)
        {
          int result2 = 0;
          int result3 = 0;
          if (this.TolerencePctLimitTxt.Tag is string tag && tag.Contains<char>('.'))
          {
            string[] strArray = tag.Split('.');
            int.TryParse(strArray[0], out result3);
            int.TryParse(strArray[1], out result2);
            int num1 = this.TolerencePctLimitTxt.Text.IndexOf('.');
            int num2 = num1 == -1 ? this.TolerencePctLimitTxt.Text.Length : num1;
            if (num2 >= 0)
            {
              if (this.TolerencePctLimitTxt.Text.Length - num2 - 1 > result2 || num2 > result3)
              {
                this.TolerencePctLimitTxt.Text = this._tolerancePct.ToString("###.######");
                return;
              }
            }
            else if (this.TolerencePctLimitTxt.Text.Length > result3)
            {
              this.TolerencePctLimitTxt.Text = this._tolerancePct.ToString("###.######");
              return;
            }
          }
        }
        this._tolerancePct = result1;
        this.TolerencePctLimitTxt.Text = result1 > 0M ? result1.ToString("###.######") : string.Empty;
      }
      else
        this.TolerencePctLimitTxt.Text = this._tolerancePct.ToString("###.######");
      this.toleranceTypes[(object) this.cmbTolerancePolicy.SelectedIndex] = (object) this.TolerencePctLimitTxt.Text;
    }

    private void TolerencePctLimitTxt_Enter(object sender, EventArgs e)
    {
      if (this.TolerancePct > 0M)
        this.TolerencePctLimitTxt.Text = this.TolerancePct.ToString("###.######");
      else
        this.TolerencePctLimitTxt.Text = string.Empty;
    }

    private void TolerenceDollarLimitTxt_Leave(object sender, EventArgs e)
    {
      Decimal result = 0M;
      if (string.IsNullOrEmpty(this.TolerenceDollarLimitTxt.Text))
        this._toleranceAmt = 0M;
      else if (Decimal.TryParse(this.TolerenceDollarLimitTxt.Text.Replace(",", ""), out result))
      {
        this._toleranceAmt = result;
        this.TolerenceDollarLimitTxt.Text = result > 0M ? result.ToString("###,###") : string.Empty;
      }
      this.TolerenceDollarLimitTxt.Text = this._toleranceAmt.ToString("###,###");
    }

    private void TolerenceDollarLimitTxt_Enter(object sender, EventArgs e)
    {
      if (this.ToleranceAmt > 0M)
        this.TolerenceDollarLimitTxt.Text = this.ToleranceAmt.ToString("###,###");
      else
        this.TolerenceDollarLimitTxt.Text = string.Empty;
    }

    private void EnabledChildControls(Control ctrl, bool enabled)
    {
      if (ctrl.Controls == null || ctrl.Controls.Count == 0)
        return;
      foreach (Control control in (ArrangedElementCollection) ctrl.Controls)
      {
        control.Enabled = enabled;
        switch (control)
        {
          case TextBox _:
            if (control is TextBox textBox)
            {
              textBox.ReadOnly = !enabled;
              if (!enabled)
              {
                textBox.Text = string.Empty;
                break;
              }
              if (textBox.Name == "TolerenceDollarLimitTxt")
              {
                this.TolerenceDollarLimitTxt_Enter((object) this, (EventArgs) null);
                break;
              }
              if (textBox.Name == "TolerencePctLimitTxt")
              {
                this.TolerencePctLimitTxt_Enter((object) this, (EventArgs) null);
                break;
              }
              break;
            }
            break;
          case ComboBox _:
            if (control is ComboBox comboBox && comboBox.Name == "cmbTolerancePolicy")
            {
              if (enabled)
              {
                comboBox.SelectedIndex = (int) this._policy;
                break;
              }
              comboBox.SelectedIndex = 0;
              break;
            }
            break;
        }
        this.EnabledChildControls(control, enabled);
      }
    }

    private void TolerancePolicyControl_EnabledChanged(object sender, EventArgs e)
    {
      this.EnabledChildControls((Control) this, this.Enabled);
    }

    private void TextBox_TextChanged(object sender, EventArgs e) => this.IsDirty(true);

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.groupContainer1 = new GroupContainer();
      this.cmbTolerancePolicy = new ComboBox();
      this.PercentLbl = new System.Windows.Forms.Label();
      this.DollarSignLbl = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.TolerenceDollarLimitTxt = new TextBox();
      this.TolerencePctLimitTxt = new TextBox();
      this.OrLbl = new System.Windows.Forms.Label();
      this.ToleranceLbl = new System.Windows.Forms.Label();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.groupContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainer1.Controls.Add((Control) this.cmbTolerancePolicy);
      this.groupContainer1.Controls.Add((Control) this.PercentLbl);
      this.groupContainer1.Controls.Add((Control) this.DollarSignLbl);
      this.groupContainer1.Controls.Add((Control) this.label4);
      this.groupContainer1.Controls.Add((Control) this.TolerenceDollarLimitTxt);
      this.groupContainer1.Controls.Add((Control) this.TolerencePctLimitTxt);
      this.groupContainer1.Controls.Add((Control) this.OrLbl);
      this.groupContainer1.Controls.Add((Control) this.ToleranceLbl);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(689, 93);
      this.groupContainer1.TabIndex = 2;
      this.groupContainer1.Text = "Policy for Best Efforts Tolerance";
      this.cmbTolerancePolicy.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbTolerancePolicy.FormattingEnabled = true;
      this.cmbTolerancePolicy.Items.AddRange(new object[3]
      {
        (object) "",
        (object) "Flat Tolerance",
        (object) "Conditional Tolerance"
      });
      this.cmbTolerancePolicy.Location = new Point(161, 38);
      this.cmbTolerancePolicy.Name = "cmbTolerancePolicy";
      this.cmbTolerancePolicy.Size = new Size(135, 21);
      this.cmbTolerancePolicy.TabIndex = 32;
      this.cmbTolerancePolicy.SelectedIndexChanged += new EventHandler(this.cmbTolerancePolicy_SelectedIndexChanged);
      this.PercentLbl.AutoSize = true;
      this.PercentLbl.Location = new Point(298, 68);
      this.PercentLbl.Name = "PercentLbl";
      this.PercentLbl.Size = new Size(15, 13);
      this.PercentLbl.TabIndex = 28;
      this.PercentLbl.Text = "%";
      this.DollarSignLbl.AutoSize = true;
      this.DollarSignLbl.Location = new Point(340, 68);
      this.DollarSignLbl.Name = "DollarSignLbl";
      this.DollarSignLbl.Size = new Size(13, 13);
      this.DollarSignLbl.TabIndex = 28;
      this.DollarSignLbl.Text = "$";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(30, 41);
      this.label4.Name = "label4";
      this.label4.Size = new Size(125, 13);
      this.label4.TabIndex = 4;
      this.label4.Text = "Tolerance Control Option";
      this.TolerenceDollarLimitTxt.Location = new Point(359, 65);
      this.TolerenceDollarLimitTxt.MaxLength = 14;
      this.TolerenceDollarLimitTxt.Name = "TolerenceDollarLimitTxt";
      this.TolerenceDollarLimitTxt.ShortcutsEnabled = false;
      this.TolerenceDollarLimitTxt.Size = new Size(177, 20);
      this.TolerenceDollarLimitTxt.TabIndex = 3;
      this.TolerenceDollarLimitTxt.Tag = (object) "14.0";
      this.TolerenceDollarLimitTxt.TextChanged += new EventHandler(this.TextBox_TextChanged);
      this.TolerenceDollarLimitTxt.Enter += new EventHandler(this.TolerenceDollarLimitTxt_Enter);
      this.TolerenceDollarLimitTxt.KeyPress += new KeyPressEventHandler(this.NumericTxt_KeyPress);
      this.TolerenceDollarLimitTxt.Leave += new EventHandler(this.TolerenceDollarLimitTxt_Leave);
      this.TolerencePctLimitTxt.Location = new Point(161, 65);
      this.TolerencePctLimitTxt.MaxLength = 14;
      this.TolerencePctLimitTxt.Name = "TolerencePctLimitTxt";
      this.TolerencePctLimitTxt.ShortcutsEnabled = false;
      this.TolerencePctLimitTxt.Size = new Size(135, 20);
      this.TolerencePctLimitTxt.TabIndex = 3;
      this.TolerencePctLimitTxt.Tag = (object) "3.5";
      this.TolerencePctLimitTxt.TextChanged += new EventHandler(this.TextBox_TextChanged);
      this.TolerencePctLimitTxt.Enter += new EventHandler(this.TolerencePctLimitTxt_Enter);
      this.TolerencePctLimitTxt.KeyPress += new KeyPressEventHandler(this.NumericTxt_KeyPress);
      this.TolerencePctLimitTxt.Leave += new EventHandler(this.TolerencePctLimitTxt_Leave);
      this.OrLbl.AutoSize = true;
      this.OrLbl.Location = new Point(319, 68);
      this.OrLbl.Name = "OrLbl";
      this.OrLbl.Size = new Size(16, 13);
      this.OrLbl.TabIndex = 2;
      this.OrLbl.Text = "or";
      this.ToleranceLbl.Location = new Point(72, 68);
      this.ToleranceLbl.Name = "ToleranceLbl";
      this.ToleranceLbl.Size = new Size(83, 13);
      this.ToleranceLbl.TabIndex = 2;
      this.ToleranceLbl.Text = "Lesser of";
      this.ToleranceLbl.TextAlign = ContentAlignment.TopRight;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.groupContainer1);
      this.Name = nameof (TolerancePolicyControl);
      this.Size = new Size(689, 93);
      this.EnabledChanged += new EventHandler(this.TolerancePolicyControl_EnabledChanged);
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.ResumeLayout(false);
    }

    public delegate void dirtyDelegate(bool dirty);
  }
}
