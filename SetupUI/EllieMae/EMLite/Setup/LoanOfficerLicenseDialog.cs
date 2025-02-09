// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.LoanOfficerLicenseDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class LoanOfficerLicenseDialog : Form
  {
    public static readonly string[] StateList = new string[54]
    {
      "Alabama",
      "Alaska",
      "Arizona",
      "Arkansas",
      "California",
      "Colorado",
      "Connecticut",
      "Delaware",
      "District of Columbia",
      "Florida",
      "Georgia",
      "Hawaii",
      "Idaho",
      "Illinois",
      "Indiana",
      "Iowa",
      "Kansas",
      "Kentucky",
      "Louisiana",
      "Maine",
      "Maryland",
      "Massachusetts",
      "Michigan",
      "Minnesota",
      "Mississippi",
      "Missouri",
      "Montana",
      "Nebraska",
      "Nevada",
      "New Hampshire",
      "New Jersey",
      "New Mexico",
      "New York",
      "North Carolina",
      "North Dakota",
      "Ohio",
      "Oklahoma",
      "Oregon",
      "Pennsylvania",
      "Rhode Island",
      "South Carolina",
      "South Dakota",
      "Tennessee",
      "Texas",
      "Utah",
      "Vermont",
      "Virginia",
      "Washington",
      "West Virginia",
      "Wisconsin",
      "Wyoming",
      "Virgin Islands",
      "Guam",
      "Puerto Rico"
    };
    private Button cancelBtn;
    private Button okBtn;
    private IContainer components;
    private Label label1;
    private Label label2;
    private string userID = "";
    private bool readOnly;
    private ToolTip toolTipLicense;
    private Panel panelLicense;
    private StateLicenseSetupControl stateLicenseSetupControl;
    private List<StateLicenseExtType> extLicense;
    private LOLicenseInfo[] loLicInfo;

    public LoanOfficerLicenseDialog(LOLicenseInfo[] loLicInfo, string userID, bool readOnly)
    {
      this.readOnly = readOnly;
      this.extLicense = new List<StateLicenseExtType>();
      if (loLicInfo != null)
        this.extLicense.AddRange((IEnumerable<StateLicenseExtType>) loLicInfo);
      this.loLicInfo = loLicInfo;
      this.userID = userID;
      this.InitializeComponent();
      this.stateLicenseSetupControl = new StateLicenseSetupControl(this.extLicense, false, readOnly, true);
      this.stateLicenseSetupControl.BorderVisible = true;
      this.panelLicense.Controls.Add((Control) this.stateLicenseSetupControl);
      if (!readOnly)
        return;
      this.label1.Visible = this.label2.Visible = false;
      this.okBtn.Visible = false;
      this.cancelBtn.Text = "Close";
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    public LOLicenseInfo[] LoLicInfo => this.loLicInfo;

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      this.cancelBtn = new Button();
      this.okBtn = new Button();
      this.label1 = new Label();
      this.label2 = new Label();
      this.toolTipLicense = new ToolTip(this.components);
      this.panelLicense = new Panel();
      this.SuspendLayout();
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(727, 444);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 22);
      this.cancelBtn.TabIndex = 5;
      this.cancelBtn.Text = "Cancel";
      this.okBtn.Location = new Point(644, 444);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 22);
      this.okBtn.TabIndex = 4;
      this.okBtn.Text = "&OK";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(8, 8);
      this.label1.Name = "label1";
      this.label1.Size = new Size(777, 14);
      this.label1.TabIndex = 7;
      this.label1.Text = "Select each state where the loan officer is licensed to originate loans. Loan officers cannot originate loans for subject properties in states that are not selected.";
      this.label2.Location = new Point(8, 27);
      this.label2.Name = "label2";
      this.label2.Size = new Size(777, 32);
      this.label2.TabIndex = 8;
      this.label2.Text = "Optionally, enter the loan officer’s license number and end date for each selected state. Loan officers cannot originate loans for subject properties if the end date is in the past.";
      this.panelLicense.Location = new Point(11, 60);
      this.panelLicense.Name = "panelLicense";
      this.panelLicense.Size = new Size(789, 376);
      this.panelLicense.TabIndex = 10;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(812, 475);
      this.Controls.Add((Control) this.panelLicense);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.okBtn);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (LoanOfficerLicenseDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Loan Officer Licenses";
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      if (!this.stateLicenseSetupControl.DataValidated() || !this.stateLicenseSetupControl.HasActiveItem && Utils.Dialog((IWin32Window) this, "The current settings do not authorize this user to originate loans for a subject property in any state. Would you like to add states now? Click Yes to add authorized states. Click No to accept these settings.", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
        return;
      List<StateLicenseExtType> licenses = this.stateLicenseSetupControl.GetLicenses(true);
      foreach (StateLicenseExtType stateLicenseExtType in licenses)
      {
        if (stateLicenseExtType.Approved && stateLicenseExtType.EndDate == DateTime.MinValue)
          stateLicenseExtType.EndDate = DateTime.MaxValue;
      }
      this.loLicInfo = licenses == null ? (LOLicenseInfo[]) null : licenses.ConvertAll<LOLicenseInfo>((Converter<StateLicenseExtType, LOLicenseInfo>) (x => new LOLicenseInfo(x))).ToArray();
      this.DialogResult = DialogResult.OK;
    }
  }
}
