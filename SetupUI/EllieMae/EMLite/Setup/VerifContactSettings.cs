// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.VerifContactSettings
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class VerifContactSettings : SettingsUserControl, IOnlineHelpTarget
  {
    private Sessions.Session session;
    private IContainer components;
    private GroupContainer groupContainer1;
    private RadioButton rdoCurrentUser;
    private Panel panelMilestones;
    private Label label1;

    public VerifContactSettings(Sessions.Session session, SetUpContainer setupContainer)
      : base(setupContainer)
    {
      this.session = session;
      this.InitializeComponent();
      this.reset();
    }

    private void reset()
    {
      string companySetting = this.session.ConfigurationManager.GetCompanySetting("VerifContact", "VerifContactID");
      RoleInfo[] allRoleFunctions = ((WorkflowManager) this.session.BPM.GetBpmManager(BpmCategory.Workflow)).GetAllRoleFunctions();
      List<EllieMae.EMLite.Workflow.Milestone> list = this.session.SessionObjects.BpmManager.GetMilestones(true).ToList<EllieMae.EMLite.Workflow.Milestone>();
      if (companySetting == "")
        this.rdoCurrentUser.Checked = true;
      this.rdoCurrentUser.Tag = (object) "";
      string empty = string.Empty;
      int i = 0;
      List<int> intList = new List<int>();
      for (int index = 0; index < list.Count; ++index)
      {
        EllieMae.EMLite.Workflow.Milestone ms = list[index];
        if (!ms.Archived && ms.Name != null)
        {
          RoleInfo roleInfo = ((IEnumerable<RoleInfo>) allRoleFunctions).FirstOrDefault<RoleInfo>((Func<RoleInfo, bool>) (item => item.RoleID == ms.RoleID));
          string str;
          if (ms.Name == "Started")
            str = "File Starter";
          else if (roleInfo != null)
            str = roleInfo.RoleName;
          else
            continue;
          string buttonText = str + " in " + ms.Name + " milestone";
          this.panelMilestones.Controls.Add((Control) this.createRadioButtonControl(i, buttonText, (roleInfo != null ? roleInfo.RoleID : 0).ToString() + ":" + ms.MilestoneID, companySetting));
          if (roleInfo != null)
            intList.Add(roleInfo.ID);
          ++i;
        }
      }
      for (int index = 0; index < allRoleFunctions.Length; ++index)
      {
        if (!intList.Contains(allRoleFunctions[index].ID))
        {
          string name = allRoleFunctions[index].Name;
          this.panelMilestones.Controls.Add((Control) this.createRadioButtonControl(i, name, allRoleFunctions[index].ID.ToString() + ":", companySetting));
          ++i;
        }
      }
      this.refreshVerifContact(this.panelMilestones.Controls, companySetting);
      this.setDirtyFlag(false);
    }

    private RadioButton createRadioButtonControl(
      int i,
      string buttonText,
      string roleIDwithMSID,
      string verifContactID)
    {
      RadioButton radioButtonControl = new RadioButton();
      radioButtonControl.Size = new Size(400, this.rdoCurrentUser.Size.Height);
      RadioButton radioButton = radioButtonControl;
      Point location = this.rdoCurrentUser.Location;
      int x = location.X;
      int num = (i + 1) * (this.rdoCurrentUser.Size.Height + 6);
      location = this.rdoCurrentUser.Location;
      int y1 = location.Y;
      int y2 = num + y1;
      Point point = new Point(x, y2);
      radioButton.Location = point;
      radioButtonControl.Text = "      " + buttonText;
      radioButtonControl.TextAlign = ContentAlignment.MiddleLeft;
      radioButtonControl.Checked = verifContactID == roleIDwithMSID;
      radioButtonControl.Click += new EventHandler(this.verifRadioButton_Clicked);
      radioButtonControl.Tag = (object) roleIDwithMSID;
      return radioButtonControl;
    }

    public override void Reset()
    {
      this.refreshVerifContact(this.panelMilestones.Controls, this.session.ConfigurationManager.GetCompanySetting("VerifContact", "VerifContactID"));
      this.setDirtyFlag(false);
    }

    public override void Save()
    {
      if (!this.IsDirty)
      {
        this.Dispose();
      }
      else
      {
        this.setVerifContact(this.panelMilestones.Controls);
        this.setDirtyFlag(false);
      }
    }

    private void refreshVerifContact(Control.ControlCollection cs, string verifContactID)
    {
      foreach (Control c in (ArrangedElementCollection) cs)
      {
        if (c is RadioButton)
        {
          RadioButton radioButton = (RadioButton) c;
          radioButton.Checked = radioButton.Tag.ToString() == verifContactID;
        }
      }
    }

    private void setVerifContact(Control.ControlCollection cs)
    {
      foreach (Control c in (ArrangedElementCollection) cs)
      {
        if (c is RadioButton)
        {
          RadioButton radioButton = (RadioButton) c;
          if (radioButton.Checked)
          {
            this.session.ConfigurationManager.SetCompanySetting("VerifContact", "VerifContactID", radioButton.Tag.ToString());
            break;
          }
        }
      }
    }

    private void verifRadioButton_Clicked(object sender, EventArgs e) => this.setDirtyFlag(true);

    string IOnlineHelpTarget.GetHelpTargetName() => "Setup\\Verification Contact Setup";

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.groupContainer1 = new GroupContainer();
      this.panelMilestones = new Panel();
      this.rdoCurrentUser = new RadioButton();
      this.label1 = new Label();
      this.groupContainer1.SuspendLayout();
      this.panelMilestones.SuspendLayout();
      this.SuspendLayout();
      this.groupContainer1.Controls.Add((Control) this.panelMilestones);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(861, 644);
      this.groupContainer1.TabIndex = 23;
      this.groupContainer1.Text = "Verification Contact Setup";
      this.panelMilestones.AutoScroll = true;
      this.panelMilestones.Controls.Add((Control) this.label1);
      this.panelMilestones.Controls.Add((Control) this.rdoCurrentUser);
      this.panelMilestones.Dock = DockStyle.Fill;
      this.panelMilestones.Location = new Point(1, 26);
      this.panelMilestones.Name = "panelMilestones";
      this.panelMilestones.Size = new Size(859, 617);
      this.panelMilestones.TabIndex = 1;
      this.rdoCurrentUser.AutoSize = true;
      this.rdoCurrentUser.Location = new Point(23, 53);
      this.rdoCurrentUser.Name = "rdoCurrentUser";
      this.rdoCurrentUser.Size = new Size(84, 17);
      this.rdoCurrentUser.TabIndex = 0;
      this.rdoCurrentUser.TabStop = true;
      this.rdoCurrentUser.Text = "Current User";
      this.rdoCurrentUser.UseVisualStyleBackColor = true;
      this.rdoCurrentUser.Click += new EventHandler(this.verifRadioButton_Clicked);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(20, 18);
      this.label1.Name = "label1";
      this.label1.Size = new Size(504, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Select the user or role whose contact information will display in the “From” section on all verification forms.";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.groupContainer1);
      this.Name = nameof (VerifContactSettings);
      this.Size = new Size(861, 644);
      this.groupContainer1.ResumeLayout(false);
      this.panelMilestones.ResumeLayout(false);
      this.panelMilestones.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
