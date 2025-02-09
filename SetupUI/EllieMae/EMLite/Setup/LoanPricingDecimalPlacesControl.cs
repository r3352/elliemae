// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.LoanPricingDecimalPlacesControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class LoanPricingDecimalPlacesControl : SettingsUserControl
  {
    private bool suspendEvent;
    private IContainer components;
    private GroupContainer groupContainer1;
    private Label label1;
    private RadioButton radioBtn10Digit;
    private Label labelHowManyDecimalPlaces;
    private RadioButton radioBtn3Digit;
    private GridView gvFields;

    public LoanPricingDecimalPlacesControl(SetUpContainer setupContainer)
      : base(setupContainer)
    {
      this.InitializeComponent();
      this.initialPage();
    }

    private void initialPage()
    {
      this.suspendEvent = true;
      this.gvFields.Items.Clear();
      if (Session.ConfigurationManager.GetCompanySetting("Policies", "Use10DigitDecimalPricing") == "True")
        this.radioBtn10Digit.Checked = true;
      else
        this.radioBtn3Digit.Checked = true;
      StandardFields standardFields = Session.LoanManager.GetStandardFields();
      foreach (string requestAreaField in LockRequestUtils.LockRequestAreaFields)
      {
        StandardField fieldI = standardFields.GetFieldI(requestAreaField);
        if (fieldI != null)
          this.gvFields.Items.Add(new GVItem("Lock Request Form")
          {
            SubItems = {
              (object) requestAreaField,
              (object) fieldI.Description
            }
          });
      }
      foreach (string registrationAreaField in LockRequestUtils.SecondaryRegistrationAreaFields())
      {
        StandardField fieldI = standardFields.GetFieldI(registrationAreaField);
        if (fieldI != null)
          this.gvFields.Items.Add(new GVItem("Secondary Registration")
          {
            SubItems = {
              (object) registrationAreaField,
              (object) fieldI.Description
            }
          });
      }
      foreach (string str in LockRequestUtils.TradeAreaFieldDescriptions().ToArray())
        this.gvFields.Items.Add(new GVItem("Trade")
        {
          SubItems = {
            (object) "",
            (object) str
          }
        });
      base.Reset();
      this.suspendEvent = false;
    }

    public override void Save()
    {
      Session.ConfigurationManager.SetCompanySetting("Policies", "Use10DigitDecimalPricing", this.radioBtn10Digit.Checked ? "True" : "False");
      base.Save();
    }

    public override void Reset() => this.initialPage();

    private void data_Changed(object sender, EventArgs e)
    {
      if (this.suspendEvent)
        return;
      if (this.radioBtn3Digit.Checked && sender == this.radioBtn3Digit && Utils.Dialog((IWin32Window) this, "You are about to restrict loan prices and price adjustments to three decimal places.  This will cause prices and price adjustments to be rounded to the nearest thousandth when existing loans are updated.  Do you wish to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
        this.radioBtn10Digit.Checked = true;
      this.setDirtyFlag(true);
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
      this.groupContainer1 = new GroupContainer();
      this.gvFields = new GridView();
      this.label1 = new Label();
      this.radioBtn10Digit = new RadioButton();
      this.labelHowManyDecimalPlaces = new Label();
      this.radioBtn3Digit = new RadioButton();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.groupContainer1.Controls.Add((Control) this.gvFields);
      this.groupContainer1.Controls.Add((Control) this.label1);
      this.groupContainer1.Controls.Add((Control) this.radioBtn10Digit);
      this.groupContainer1.Controls.Add((Control) this.labelHowManyDecimalPlaces);
      this.groupContainer1.Controls.Add((Control) this.radioBtn3Digit);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(602, 427);
      this.groupContainer1.TabIndex = 7;
      this.groupContainer1.Text = "Loan Pricing Decimal Places";
      this.gvFields.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Area";
      gvColumn1.Width = 140;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Field ID";
      gvColumn2.Width = 60;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Field Description";
      gvColumn3.Width = 340;
      this.gvFields.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.gvFields.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvFields.Location = new Point(13, 133);
      this.gvFields.Name = "gvFields";
      this.gvFields.Size = new Size(572, 278);
      this.gvFields.TabIndex = 5;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(10, 108);
      this.label1.Name = "label1";
      this.label1.Size = new Size(227, 13);
      this.label1.TabIndex = 4;
      this.label1.Text = "The following fields are affected by this setting:";
      this.radioBtn10Digit.AutoSize = true;
      this.radioBtn10Digit.Location = new Point(13, 78);
      this.radioBtn10Digit.Name = "radioBtn10Digit";
      this.radioBtn10Digit.Size = new Size(37, 17);
      this.radioBtn10Digit.TabIndex = 3;
      this.radioBtn10Digit.TabStop = true;
      this.radioBtn10Digit.Text = "10";
      this.radioBtn10Digit.UseVisualStyleBackColor = true;
      this.radioBtn10Digit.CheckedChanged += new EventHandler(this.data_Changed);
      this.labelHowManyDecimalPlaces.AutoSize = true;
      this.labelHowManyDecimalPlaces.Location = new Point(10, 38);
      this.labelHowManyDecimalPlaces.Name = "labelHowManyDecimalPlaces";
      this.labelHowManyDecimalPlaces.Size = new Size(314, 13);
      this.labelHowManyDecimalPlaces.TabIndex = 1;
      this.labelHowManyDecimalPlaces.Text = "How many decimal places should be shown in loan pricing fields?";
      this.radioBtn3Digit.AutoSize = true;
      this.radioBtn3Digit.Location = new Point(13, 57);
      this.radioBtn3Digit.Name = "radioBtn3Digit";
      this.radioBtn3Digit.Size = new Size(31, 17);
      this.radioBtn3Digit.TabIndex = 2;
      this.radioBtn3Digit.TabStop = true;
      this.radioBtn3Digit.Text = "3";
      this.radioBtn3Digit.UseVisualStyleBackColor = true;
      this.radioBtn3Digit.CheckedChanged += new EventHandler(this.data_Changed);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.Controls.Add((Control) this.groupContainer1);
      this.Name = nameof (LoanPricingDecimalPlacesControl);
      this.Size = new Size(602, 427);
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
