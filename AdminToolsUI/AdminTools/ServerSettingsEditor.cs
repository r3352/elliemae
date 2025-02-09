// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.ServerSettingsEditor
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using EllieMae.EMLite.AdminTools.EnhancedConditionsTool;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.eFolder;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.eFolder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.AdminTools
{
  public class ServerSettingsEditor : UserControl
  {
    public string errSetting;
    private System.ComponentModel.Container components;
    private Panel pnlSettings;
    private IServerManager svrManager;
    private Hashtable labels = new Hashtable();
    private Hashtable viewers = new Hashtable();
    private Hashtable editors = new Hashtable();
    private Hashtable previousValues = new Hashtable();
    private ArrayList changedItems = new ArrayList();
    private Control currentEditor;
    private int rowHeight = 22;
    private int dividerHeight = -1;
    private int editorWidth = 340;
    private int labelWidth = 200;
    private int rowCount;
    private const string className = "ServerSettingsEditor";
    private bool useIPRestriction;

    public event SettingChangedEventHandler SettingChanged;

    public ServerSettingsEditor() => this.InitializeComponent();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pnlSettings = new Panel();
      this.SuspendLayout();
      this.pnlSettings.AutoScroll = true;
      this.pnlSettings.BackColor = SystemColors.Window;
      this.pnlSettings.BorderStyle = BorderStyle.Fixed3D;
      this.pnlSettings.Dock = DockStyle.Fill;
      this.pnlSettings.Location = new Point(0, 0);
      this.pnlSettings.Margin = new Padding(0, 3, 0, 3);
      this.pnlSettings.Name = "pnlSettings";
      this.pnlSettings.Size = new Size(540, 236);
      this.pnlSettings.TabIndex = 1;
      this.pnlSettings.Click += new EventHandler(this.pnlSettings_Click);
      this.AutoScroll = true;
      this.Controls.Add((Control) this.pnlSettings);
      this.Name = nameof (ServerSettingsEditor);
      this.Size = new Size(540, 236);
      this.Layout += new LayoutEventHandler(this.ServerSettingsEditor_Layout);
      this.MouseDown += new MouseEventHandler(this.ServerSettingsEditor_MouseDown);
      this.ResumeLayout(false);
    }

    public IServerManager ServerManager
    {
      get => this.svrManager;
      set => this.svrManager = value;
    }

    public void LoadSettings(ICollection settingDefs)
    {
      this.Clear();
      ArrayList arrayList = new ArrayList();
      foreach (SettingDefinition settingDef in (IEnumerable) settingDefs)
      {
        if (this.DisplaySetting(settingDef))
          arrayList.Add((object) settingDef);
      }
      foreach (SettingDefinition def in arrayList)
        this.appendSetting(def);
    }

    public void Clear()
    {
      this.pnlSettings.Controls.Clear();
      this.labels = new Hashtable();
      this.viewers = new Hashtable();
      this.editors = new Hashtable();
      this.changedItems = new ArrayList();
      this.rowCount = 0;
      this.addHeaderRow();
    }

    public bool SaveChanges()
    {
      if (this.changedItems != null && this.changedItems.Count == 0)
        return true;
      bool flag1 = false;
      bool flag2 = false;
      this.errSetting = (string) null;
      Hashtable changedSettings = CollectionsUtil.CreateCaseInsensitiveHashtable();
      for (int index = 0; index < this.changedItems.Count; ++index)
      {
        Control editor = (Control) this.editors[(object) this.changedItems[index].ToString()];
        SettingDefinition tag = (SettingDefinition) editor.Tag;
        if (tag.Path == "Components.ENABLEFASTLOANLOAD")
        {
          object obj = !(editor.Text.ToLower() == "enabled") ? (object) bool.Parse("False") : (object) bool.Parse("True");
          changedSettings.Add((object) "Feature.ENABLEFASTLOANLOAD", obj);
        }
        else if (tag.Path == "Policies.ENHANCEDCONDITIONSWORKFLOW")
        {
          object obj = (object) (bool) (editor.Text.ToLower() == "enabled" ? (bool.Parse("True") ? 1 : 0) : (bool.Parse("False") ? 1 : 0));
          changedSettings.Add((object) tag.Path, obj);
        }
        else if (tag.Path == "Policies.ENHANCEDCONDITIONSWORKFLOWSTDATE")
        {
          object obj = (object) DateTime.Parse(this.pnlSettings.Controls["Policies_ENHANCEDCONDITIONSWORKFLOWSTDATE_viewer"].Text);
          changedSettings.Add((object) tag.Path, obj);
        }
        else if (tag.Path == "Client.EnableTokenLoginOnly")
        {
          string selectedValue = this.getEditorValue(editor, tag).ToString();
          if ((!selectedValue.Equals("false", StringComparison.InvariantCultureIgnoreCase) ? 1 : (Utils.Dialog((IWin32Window) this, "Please note: Any Restricted users will no longer be able to login to SmartClient if web login is turned off.  Would you like to continue?", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.Cancel ? 1 : 0)) != 0)
          {
            string serverInstanceName = Session.StartupInfo.ServerInstanceName;
            this.svrManager.UpdateWebLoginSetting(string.Format("TokenLoginOnly_{0}", (object) serverInstanceName), serverInstanceName, selectedValue, Session.StartupInfo.UserInfo.Userid);
          }
        }
        else
          changedSettings.Add((object) tag.Path, this.getEditorValue(editor, tag));
        if (tag.Path == "Cache.Setting")
          flag1 = true;
        if (tag.Path.Contains("Password."))
          flag2 = true;
      }
      if (flag2)
      {
        StringBuilder stringBuilder = this.ValidatePasswordPolicy(changedSettings);
        if (stringBuilder.Length > 0)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, stringBuilder.ToString());
          Control editor = (Control) this.editors[(object) this.errSetting];
          ((Control) this.viewers[(object) this.errSetting]).Visible = false;
          editor.BringToFront();
          editor.Visible = true;
          this.currentEditor = editor;
          editor.Focus();
          return false;
        }
      }
      bool flag3 = this.useIPRestriction;
      if (changedSettings.ContainsKey((object) "Policies.IPRestriction"))
      {
        if ((EnableDisableSetting) changedSettings[(object) "Policies.IPRestriction"] == EnableDisableSetting.Enabled)
        {
          int num = (int) new IPRestrictionForm().ShowDialog();
        }
        flag3 = (EnableDisableSetting) changedSettings[(object) "Policies.IPRestriction"] == EnableDisableSetting.Enabled;
      }
      this.svrManager.UpdateServerSettings((IDictionary) changedSettings);
      if (flag1)
        this.svrManager.RefreshCache(true);
      this.useIPRestriction = flag3;
      if (changedSettings.Contains((object) "Policies.ENABLEGEICOINTEGRATION"))
      {
        try
        {
          RestApiHelper helper = new RestApiHelper(Session.DefaultInstance);
          ApiResponse result = Task.Run<ApiResponse>((Func<Task<ApiResponse>>) (async () => await helper.SendHOIStatus(changedSettings[(object) "Policies.ENABLEGEICOINTEGRATION"].ToString().ToLower()))).Result;
          if (result != null)
          {
            if (!string.IsNullOrEmpty(result.ResponseString))
              RemoteLogger.Write(TraceLevel.Info, nameof (ServerSettingsEditor) + string.Format(" Updating GEICO Integration setting - Status Code: {0}, CorrelationId: {1}, Response Message: {2}", (object) (((int) result.StatusCode).ToString() + " " + (object) result.StatusCode), (object) result.CorrelationId, (object) result.ResponseString));
            else
              RemoteLogger.Write(TraceLevel.Info, nameof (ServerSettingsEditor) + string.Format(" Updating GEICO Integration setting - Status Code: {0}, CorrelationId: {1}", (object) (((int) result.StatusCode).ToString() + " " + (object) result.StatusCode), (object) result.CorrelationId));
          }
        }
        catch (Exception ex)
        {
          RemoteLogger.Write(TraceLevel.Info, nameof (ServerSettingsEditor) + string.Format(" Error Updating GEICO Integration setting. EX: {0}", (object) ex));
        }
      }
      if (!changedSettings.ContainsKey((object) "Components.UseERDB") && (changedSettings.ContainsKey((object) "POLICIES.AllowConcurrentEditing") || changedSettings.ContainsKey((object) "POLICIES.HideChangesMadeByOthers") || changedSettings.ContainsKey((object) "POLICIES.AllowSdkCE")))
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You must stop and restart the Encompass Server for the changes to take effect.");
      }
      this.changedItems.Clear();
      return true;
    }

    internal void SetAllowedIPs()
    {
      int num = (int) new IPRestrictionForm().ShowDialog((IWin32Window) this);
    }

    internal void SetMFAenabled()
    {
      int num = (int) new MFAEnabledProducts().ShowDialog((IWin32Window) this);
    }

    private void appendSetting(SettingDefinition def)
    {
      if (string.Compare(def.Path, "Policies.EmailUnsubscribeLink", true) == 0 && (Session.ServerIdentity == null || !Session.ServerIdentity.IsHttp))
        return;
      object obj = this.svrManager.GetServerSetting(def.Path);
      if (def.Path == "Client.EnableTokenLoginOnly")
      {
        List<ScAttribute> attributesByName = SmartClientUtils.GetAttributesByName(Session.StartupInfo.ServerInstanceName, "TokenLoginOnly");
        obj = (object) (bool) (attributesByName == null ? 0 : (attributesByName.All<ScAttribute>((Func<ScAttribute, bool>) (item => !string.IsNullOrWhiteSpace(item.AttrValue) && item.AttrValue == "1")) ? 1 : 0));
      }
      this.pnlSettings.Controls.Add(this.createLabel(def));
      this.pnlSettings.Controls.Add(this.createViewer(def, obj));
      this.pnlSettings.Controls.Add(this.createEditor(def, obj));
      ++this.rowCount;
      if (!(def.Path == "Policies.ENHANCEDCONDITIONSWORKFLOWSTDATE") || !(this.svrManager.GetServerSetting("Policies.ENHANCEDCONDITIONSWORKFLOW").ToString().ToLower() == "false"))
        return;
      this.pnlSettings.Controls["Policies_ENHANCEDCONDITIONSWORKFLOWSTDATE_label"].Visible = false;
      this.pnlSettings.Controls["Policies_ENHANCEDCONDITIONSWORKFLOWSTDATE_viewer"].Visible = false;
    }

    private Control createDivider()
    {
      Label divider = new Label();
      divider.Width = this.pnlSettings.Width;
      divider.Height = this.dividerHeight;
      divider.Location = new Point(0, this.rowY() + this.rowHeight);
      divider.BackColor = Color.LightGray;
      divider.Text = "";
      return (Control) divider;
    }

    private void addHeaderRow()
    {
      Label label1 = new Label();
      label1.Height = this.rowHeight;
      label1.Width = this.labelWidth;
      label1.Location = new Point(0, 0);
      label1.BackColor = Color.Gray;
      label1.TextAlign = ContentAlignment.MiddleLeft;
      label1.Font = new Font(label1.Font.FontFamily, label1.Font.SizeInPoints, FontStyle.Bold);
      label1.Text = "Setting Name";
      this.pnlSettings.Controls.Add((Control) label1);
      Label label2 = new Label();
      label2.Height = this.rowHeight;
      label2.Width = this.editorWidth;
      label2.Location = new Point(this.labelWidth, 0);
      label2.BackColor = Color.LightGray;
      label2.TextAlign = ContentAlignment.MiddleLeft;
      label2.Font = label1.Font;
      label2.Text = "Value";
      this.pnlSettings.Controls.Add((Control) label2);
    }

    private int rowY() => (this.rowCount + 1) * (this.rowHeight + this.dividerHeight);

    private Control createLabel(SettingDefinition def)
    {
      Label label = new Label();
      label.Name = this.pathToControlName(def.Path) + "_label";
      label.UseMnemonic = false;
      label.Tag = (object) def;
      label.Height = this.rowHeight;
      label.Width = this.labelWidth;
      label.Location = new Point(0, this.rowY());
      label.Text = def.DisplayName + (def.RequiresRestart ? " *" : "");
      label.BackColor = AppColors.SandySky;
      label.TextAlign = ContentAlignment.MiddleLeft;
      label.BorderStyle = BorderStyle.FixedSingle;
      label.Click += new EventHandler(this.stopEditing);
      label.DoubleClick += new EventHandler(this.switchToEditor);
      new ToolTip()
      {
        AutoPopDelay = 30000,
        InitialDelay = 1000
      }.SetToolTip((Control) label, def.DisplayName + "\r\n" + def.Description);
      this.labels.Add((object) def.Path, (object) label);
      return (Control) label;
    }

    private Control createViewer(SettingDefinition def, object value)
    {
      Label viewer = !(def is EnumSettingDefinition) ? this.createDefaultViewer(def, value) : this.createEnumViewer((EnumSettingDefinition) def, value);
      viewer.Name = this.pathToControlName(def.Path) + "_viewer";
      viewer.Tag = (object) def;
      viewer.Height = this.rowHeight;
      viewer.Width = this.pnlSettings.VerticalScroll.Visible ? this.editorWidth + SystemInformation.VerticalScrollBarWidth : this.editorWidth;
      viewer.BackColor = Color.WhiteSmoke;
      viewer.BorderStyle = BorderStyle.Fixed3D;
      viewer.TextAlign = ContentAlignment.MiddleLeft;
      viewer.Location = new Point(this.labelWidth, this.rowY());
      viewer.Click += new EventHandler(this.stopEditing);
      viewer.DoubleClick += new EventHandler(this.switchToEditor);
      this.viewers.Add((object) def.Path, (object) viewer);
      return (Control) viewer;
    }

    private Label createEnumViewer(EnumSettingDefinition def, object value)
    {
      return (Label) new ServerSettingsEditor.EnumLabel(def)
      {
        Value = value
      };
    }

    private Label createDefaultViewer(SettingDefinition def, object value)
    {
      Label defaultViewer = new Label();
      if (def is DateTimeSettingDefinition)
      {
        DateTime date = Utils.ParseDate(value);
        defaultViewer.Text = date.ToString("MM/dd/yyyy");
      }
      else if (def.Path == "Policies.ENHANCEDCONDITIONSWORKFLOW")
        defaultViewer.Text = value.ToString().ToLower() == "false" ? "Disabled" : "Enabled";
      else
        defaultViewer.Text = value.ToString();
      return defaultViewer;
    }

    internal bool UseIPRestriction => this.useIPRestriction;

    private Control createEditor(SettingDefinition def, object value)
    {
      Control editor = (Control) null;
      switch (def)
      {
        case StringSettingDefinition _:
          editor = this.createTextEditor((StringSettingDefinition) def, (string) value);
          break;
        case BitmaskSettingDefinition _:
          editor = this.createBitmaskEditor((BitmaskSettingDefinition) def, value);
          break;
        case EnumSettingDefinition _:
          editor = this.createEnumEditor((EnumSettingDefinition) def, value);
          if (def.Name == "IPRestriction")
          {
            this.useIPRestriction = (EnableDisableSetting) value == EnableDisableSetting.Enabled;
            if (this.Parent is ServerSettingsManager parent)
            {
              parent.BtnReRegisterERDBVisible = this.useIPRestriction && !parent.BtnApplyEnabled;
              parent.BtnReRegisterERDBText = "Set Allowed IPs";
              break;
            }
            break;
          }
          break;
        case IntegerSettingDefinition _:
          editor = this.createIntegerEditor((IntegerSettingDefinition) def, (int) value);
          break;
        case BooleanSettingDefinition _:
          editor = this.createBooleanEditor((BooleanSettingDefinition) def, (bool) value);
          break;
        case DateTimeSettingDefinition _:
          editor = this.createDateTimeEditor((DateTimeSettingDefinition) def, value);
          if (def.Name == "DefaulttoUseCDRebaselineDate")
          {
            ((DateTimeSettingDefinition) def).SetDateRangeValidation(Utils.ParseDate((object) "06/01/2018"), DateTime.MaxValue, "Date cannot be earlier than June 1, 2018.", true);
            break;
          }
          break;
        case DoubleSettingDefinition _:
          editor = this.createDoubleEditor((DoubleSettingDefinition) def, value.ToString());
          break;
      }
      editor.Name = this.pathToControlName(def.Path);
      editor.Tag = (object) def;
      editor.Width = this.pnlSettings.VerticalScroll.Visible ? this.editorWidth + SystemInformation.VerticalScrollBarWidth : this.editorWidth;
      editor.Location = this.pnlSettings.VerticalScroll.Visible ? new Point(this.labelWidth, Math.Min(this.rowY(), this.Height + SystemInformation.VerticalScrollBarWidth + editor.Height)) : new Point(this.labelWidth, Math.Min(this.rowY(), this.Height - editor.Height));
      editor.Validating += new CancelEventHandler(this.switchToViewer);
      editor.CausesValidation = true;
      editor.Visible = false;
      editor.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.editors.Add((object) def.Path, (object) editor);
      return editor;
    }

    private Control createDoubleEditor(DoubleSettingDefinition def, string value)
    {
      TextBox doubleEditor = new TextBox();
      doubleEditor.Text = value.ToString();
      doubleEditor.Height = this.rowHeight;
      doubleEditor.KeyPress += new KeyPressEventHandler(this.DoubleEditorKeyPress);
      doubleEditor.TextChanged += new EventHandler(this.controlChanged);
      return (Control) doubleEditor;
    }

    private Control createDateTimeEditor(DateTimeSettingDefinition def, object value)
    {
      DateTimePicker dateTimeEditor = new DateTimePicker();
      dateTimeEditor.CalendarTitleBackColor = Color.FromArgb(29, 110, 174);
      dateTimeEditor.CustomFormat = "MM/dd/yyyy";
      dateTimeEditor.Format = DateTimePickerFormat.Custom;
      if (Utils.ParseDate(value, false) != DateTime.MinValue)
        dateTimeEditor.Value = Utils.ParseDate(value, false);
      return (Control) dateTimeEditor;
    }

    private Control createTextEditor(StringSettingDefinition def, string value)
    {
      TextBox textEditor = new TextBox();
      textEditor.MaxLength = def.MaxLength;
      textEditor.Text = value;
      textEditor.Height = this.rowHeight;
      textEditor.TextChanged += new EventHandler(this.controlChanged);
      textEditor.KeyPress += new KeyPressEventHandler(this.defaultEditorKeyPress);
      return (Control) textEditor;
    }

    private Control createIntegerEditor(IntegerSettingDefinition def, int value)
    {
      TextBox integerEditor = new TextBox();
      integerEditor.MaxLength = def.MaxValue.ToString().Length;
      integerEditor.Text = value.ToString();
      integerEditor.Height = this.rowHeight;
      integerEditor.KeyPress += new KeyPressEventHandler(this.integerEditorKeyPress);
      integerEditor.TextChanged += new EventHandler(this.controlChanged);
      return (Control) integerEditor;
    }

    private Control createEnumEditor(EnumSettingDefinition def, object value)
    {
      ComboBox enumEditor = new ComboBox();
      enumEditor.Height = this.rowHeight;
      enumEditor.DropDownStyle = ComboBoxStyle.DropDownList;
      enumEditor.Items.AddRange((object[]) def.NameProvider.GetNames());
      enumEditor.SelectedItem = (object) def.NameProvider.GetName(value);
      enumEditor.SelectedIndexChanged += new EventHandler(this.controlChanged);
      enumEditor.KeyPress += new KeyPressEventHandler(this.defaultEditorKeyPress);
      return (Control) enumEditor;
    }

    private Control createBitmaskEditor(BitmaskSettingDefinition def, object value)
    {
      ServerSettingsEditor.BitmaskListBox bitmaskEditor = new ServerSettingsEditor.BitmaskListBox(def);
      bitmaskEditor.Height = Math.Min(5, Enum.GetValues(def.EnumType).Length) * this.rowHeight;
      bitmaskEditor.Value = value;
      bitmaskEditor.SelectedIndexChanged += new EventHandler(this.controlChanged);
      bitmaskEditor.KeyPress += new KeyPressEventHandler(this.defaultEditorKeyPress);
      return (Control) bitmaskEditor;
    }

    private Control createBooleanEditor(BooleanSettingDefinition def, bool value)
    {
      ComboBox booleanEditor = new ComboBox();
      booleanEditor.Height = this.rowHeight;
      booleanEditor.DropDownStyle = ComboBoxStyle.DropDownList;
      booleanEditor.Items.Add((object) "False");
      booleanEditor.Items.Add((object) "True");
      if (def.Path == "Policies.ENHANCEDCONDITIONSWORKFLOW")
      {
        booleanEditor.Items.Clear();
        booleanEditor.Items.Add((object) "Disabled");
        booleanEditor.Items.Add((object) "Enabled");
      }
      if (value)
        booleanEditor.SelectedIndex = 1;
      else
        booleanEditor.SelectedIndex = 0;
      booleanEditor.SelectedIndexChanged += new EventHandler(this.controlChanged);
      booleanEditor.KeyPress += new KeyPressEventHandler(this.defaultEditorKeyPress);
      return (Control) booleanEditor;
    }

    private string pathToControlName(string path) => path.Replace(".", "_");

    private void integerEditorKeyPress(object sender, KeyPressEventArgs e)
    {
      if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
        e.Handled = true;
      else
        this.defaultEditorKeyPress(sender, e);
    }

    private void DoubleEditorKeyPress(object sender, KeyPressEventArgs e)
    {
      if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != '.')
        e.Handled = true;
      else
        this.defaultEditorKeyPress(sender, e);
    }

    private void defaultEditorKeyPress(object sender, KeyPressEventArgs e)
    {
      if (Encoding.ASCII.GetBytes(new char[1]{ e.KeyChar })[0] != (byte) 27)
        return;
      this.cancelEditing();
      e.Handled = true;
    }

    private bool validateEditor(Control editor)
    {
      switch ((SettingDefinition) editor.Tag)
      {
        case IntegerSettingDefinition _:
          return this.validateIntegerEditor(editor);
        case DateTimeSettingDefinition _:
          return this.validateDateTimeEditor(editor);
        default:
          return true;
      }
    }

    private bool validateIntegerEditor(Control editor)
    {
      TextBox textBox = (TextBox) editor;
      IntegerSettingDefinition tag = (IntegerSettingDefinition) textBox.Tag;
      try
      {
        int num = int.Parse(textBox.Text);
        if (num >= tag.MinValue && num <= tag.MaxValue)
          return true;
        this.displayIntegerRangeError(tag);
        return false;
      }
      catch (OverflowException ex)
      {
        this.displayIntegerRangeError(tag);
        return false;
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Invalid numeric format for value '" + tag.DisplayName + "'", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
    }

    private bool validateDateTimeEditor(Control editor)
    {
      DateTimePicker dateTimePicker = (DateTimePicker) editor;
      DateTimeSettingDefinition tag = (DateTimeSettingDefinition) dateTimePicker.Tag;
      bool flag;
      try
      {
        if (!tag.CheckValidation)
          return true;
        flag = tag.ValidateDateRange(Utils.ParseDate((object) dateTimePicker.Text));
        if (!flag)
        {
          if (tag.ErrorMessage != "")
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "Invalid date value for '" + tag.DisplayName + "'. " + tag.ErrorMessage, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
        }
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Invalid date value for '" + tag.DisplayName + "'. " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        flag = false;
      }
      if (!flag)
      {
        Control viewer = (Control) this.viewers[(object) tag.Path];
        dateTimePicker.Value = Utils.ParseDate((object) viewer.Text).Date;
      }
      return flag;
    }

    private void displayIntegerRangeError(IntegerSettingDefinition def)
    {
      int num = (int) Utils.Dialog((IWin32Window) this, "The value for the '" + def.DisplayName + "' setting must be in the range from " + (object) def.MinValue + " to " + (object) def.MaxValue + ", inclusive.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    }

    private object getEditorValue(Control editor, SettingDefinition def)
    {
      return def is EnumSettingDefinition ? ((EnumSettingDefinition) def).NameProvider.GetValue(editor.Text) : def.Parse(editor.Text);
    }

    private void switchToEditor(object sender, EventArgs e)
    {
      if (!this.validateCurrentEditor())
        return;
      SettingDefinition tag = (SettingDefinition) ((Control) sender).Tag;
      Control editor = (Control) this.editors[(object) tag.Path];
      Control viewer = (Control) this.viewers[(object) tag.Path];
      viewer.Visible = false;
      Control control = editor;
      Point location = viewer.Location;
      int x = location.X;
      location = viewer.Location;
      int y = location.Y;
      Point point = new Point(x, y);
      control.Location = point;
      editor.BringToFront();
      editor.Visible = true;
      this.currentEditor = editor;
      editor.Focus();
    }

    private void switchToViewer(object sender, CancelEventArgs e)
    {
      if (sender != this.currentEditor)
        return;
      e.Cancel = !this.validateCurrentEditor();
    }

    private bool validateCurrentEditor()
    {
      if (this.currentEditor == null)
        return true;
      if (!this.validateEditor(this.currentEditor))
        return false;
      SettingDefinition tag = (SettingDefinition) this.currentEditor.Tag;
      Control viewer = (Control) this.viewers[(object) tag.Path];
      bool flag = !(tag is BitmaskSettingDefinition) ? viewer.Text != this.currentEditor.Text : !((ServerSettingsEditor.EnumLabel) viewer).Value.Equals(((ServerSettingsEditor.BitmaskListBox) this.currentEditor).Value);
      viewer.Text = this.currentEditor.Text;
      this.currentEditor.Visible = false;
      viewer.Visible = true;
      this.currentEditor = (Control) null;
      if (flag)
      {
        if (!this.changedItems.Contains((object) tag.Path))
          this.changedItems.Add((object) tag.Path);
        if (this.SettingChanged != null)
          this.SettingChanged(tag.Path);
      }
      return true;
    }

    private void controlChanged(object sender, EventArgs e)
    {
      if (this.currentEditor == null)
        return;
      string path = ((SettingDefinition) this.currentEditor.Tag).Path;
      if ((path == "Policies.AllowConcurrentEditing" || path == "Policies.AllowSdkCE") && Utils.Dialog((IWin32Window) this, "You must stop the Encompass Server before you can change this setting. Have you stopped the Encompass Server?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.No)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please stop the Encompass Server before you change this setting.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        if (!(this.currentEditor is ComboBox currentEditor))
          return;
        currentEditor.SelectedIndexChanged -= new EventHandler(this.controlChanged);
        currentEditor.SelectedIndex = (currentEditor.SelectedIndex + 1) % 2;
        currentEditor.SelectedIndexChanged += new EventHandler(this.controlChanged);
      }
      else
      {
        if (path == "Policies.VALIDATESYSRULEPRIORORDERINGDOC")
        {
          ComboBox currentEditor = this.currentEditor as ComboBox;
          if (currentEditor.SelectedItem.ToString() == "True" && Utils.Dialog((IWin32Window) this, "When enabled, a save will be required prior to ordering docs. This may impact performance due to evaluation of DDM and Business Rules.", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
          {
            currentEditor.SelectedIndex = currentEditor.FindStringExact("False");
            return;
          }
        }
        if (path == "Policies.ENHANCEDCONDITIONSWORKFLOW")
        {
          if (this.currentEditor.Text.ToLower() == "disabled")
          {
            this.pnlSettings.Controls["Policies_ENHANCEDCONDITIONSWORKFLOWSTDATE_label"].Visible = false;
            this.pnlSettings.Controls["Policies_ENHANCEDCONDITIONSWORKFLOWSTDATE_viewer"].Visible = false;
            if (this.changedItems.Contains((object) "Policies.ENHANCEDCONDITIONSWORKFLOWSTDATE"))
              this.changedItems.Remove((object) "Policies.ENHANCEDCONDITIONSWORKFLOWSTDATE");
          }
          else
          {
            this.pnlSettings.Controls["Policies_ENHANCEDCONDITIONSWORKFLOWSTDATE_label"].Visible = true;
            string str = Convert.ToDateTime(this.svrManager.GetServerSetting("Policies.ENHANCEDCONDITIONSWORKFLOWSTDATE")).ToString("MM/dd/yyyy");
            this.pnlSettings.Controls["Policies_ENHANCEDCONDITIONSWORKFLOWSTDATE_viewer"].Visible = true;
            this.pnlSettings.Controls["Policies_ENHANCEDCONDITIONSWORKFLOWSTDATE_viewer"].Text = str;
            this.pnlSettings.Controls["Policies_ENHANCEDCONDITIONSWORKFLOWSTDATE"].Text = str;
            if (!this.changedItems.Contains((object) "Policies.ENHANCEDCONDITIONSWORKFLOWSTDATE"))
              this.changedItems.Add((object) "Policies.ENHANCEDCONDITIONSWORKFLOWSTDATE");
          }
        }
        if (path == "eClose.AllowHybridWithENoteClosing")
        {
          if (this.currentEditor.Text.ToLower() == "enabled")
          {
            try
            {
              if (!new eCloseRestClient().CheckDCSSetupStatus())
              {
                int num = (int) Utils.Dialog((IWin32Window) this, " In order to enable eClose with eNotes, please complete the onboarding process for eClose eVault", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                this.currentEditor.Text = "Disabled";
              }
            }
            catch (Exception ex)
            {
              int num = (int) Utils.Dialog((IWin32Window) this, " There was an issue enabling this setting.  Please try again later or contact your administrator. ", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              this.currentEditor.Text = "Disabled";
            }
          }
        }
        if (this.SettingChanged == null)
          return;
        this.SettingChanged(path);
      }
    }

    private void ServerSettingsEditor_Layout(object sender, LayoutEventArgs e)
    {
      this.labelWidth = 320;
      this.editorWidth = this.pnlSettings.ClientSize.Width - this.labelWidth;
    }

    private void ServerSettingsEditor_MouseDown(object sender, MouseEventArgs e)
    {
      if (this.GetChildAtPoint(new Point(e.X, e.Y)) == this.currentEditor)
        return;
      this.stopEditing(sender, (EventArgs) e);
    }

    private void stopEditing(object sender, EventArgs e) => this.validateCurrentEditor();

    private void cancelEditing()
    {
      if (this.currentEditor == null)
        return;
      SettingDefinition tag = (SettingDefinition) this.currentEditor.Tag;
      Control viewer = (Control) this.viewers[(object) tag.Path];
      if (tag is BitmaskSettingDefinition)
        ((ServerSettingsEditor.BitmaskListBox) this.currentEditor).Value = ((ServerSettingsEditor.EnumLabel) viewer).Value;
      else
        this.currentEditor.Text = viewer.Text;
      this.currentEditor.Visible = false;
      viewer.Visible = true;
      this.currentEditor = (Control) null;
    }

    private void pnlSettings_Click(object sender, EventArgs e) => this.stopEditing(sender, e);

    private StringBuilder ValidatePasswordPolicy(Hashtable changeItems)
    {
      StringBuilder errorList = new StringBuilder();
      IList orderedDefinitions = SettingDefinitions.GetOrderedDefinitions("Password", (this.Parent as ServerSettingsManager).targetSystem);
      ArrayList arrayList = new ArrayList();
      this.errSetting = (string) null;
      foreach (SettingDefinition settingDefinition in (IEnumerable) orderedDefinitions)
      {
        if (this.DisplaySetting(settingDefinition))
          arrayList.Add((object) settingDefinition);
      }
      foreach (SettingDefinition settingDefinition in arrayList)
      {
        object obj = !changeItems.ContainsKey((object) settingDefinition.Path) ? this.svrManager.GetServerSetting(settingDefinition.Path) : changeItems[(object) settingDefinition.Path];
        this.PopulateErrorList(ref errorList, settingDefinition.Path, obj);
      }
      return errorList;
    }

    private void PopulateErrorList(ref StringBuilder errorList, string path, object value)
    {
      int int32 = Convert.ToInt32(value);
      switch (path)
      {
        case "Password.MinLength":
          if (int32 < 8)
          {
            errorList.AppendLine("The password length can not be less than 8 characters");
            break;
          }
          break;
        case "Password.NumUpperCase":
          if (int32 < 1)
          {
            errorList.AppendLine("There should be at least one Upper Case letter in the password");
            break;
          }
          break;
        case "Password.NumLowerCase":
          if (int32 < 1)
          {
            errorList.AppendLine("There should be at least one Lower Case letter in the password");
            break;
          }
          break;
        case "Password.NumDigits":
          if (int32 < 1)
          {
            errorList.AppendLine("There should be at least one numeric character in the password");
            break;
          }
          break;
        case "Password.Lifetime":
          if (int32 > 183)
          {
            errorList.AppendLine("The password can not be set to expire in more than 183 days");
            break;
          }
          if (int32 < 1)
          {
            errorList.AppendLine("The password should be set to expire in atleast one day");
            break;
          }
          break;
      }
      if (errorList.Length <= 0 || !string.IsNullOrEmpty(this.errSetting))
        return;
      this.errSetting = path;
    }

    private bool DisplaySetting(SettingDefinition settingDefinition)
    {
      if (settingDefinition.Path.Contains("EnableLockSnapshotRecapture"))
      {
        string str = (string) null;
        try
        {
          str = SmartClientUtils.GetAttribute(Session.CompanyInfo.ClientID, "Encompass.exe", "EnableLockSnapshotRecapture");
        }
        catch (Exception ex)
        {
        }
        if (string.IsNullOrEmpty(str) || str.Trim() == "0")
          return false;
      }
      if (settingDefinition.Path.EndsWith("URLA2020") || settingDefinition.Path.Contains("URLAPageNumbering"))
      {
        string str = "false";
        try
        {
          str = (string) this.svrManager.GetServerSetting("FEATURE.ENABLEURLA2020", false);
        }
        catch (Exception ex)
        {
        }
        if (string.IsNullOrEmpty(str) || str.ToLower() != "true")
          return false;
      }
      if (settingDefinition.Path.Contains("ENHANCEDCONDITIONSWORKFLOW"))
      {
        string str = "false";
        try
        {
          str = (string) this.svrManager.GetServerSetting("CLIENT.ENHANCEDCONDITIONSETTINGS", false);
        }
        catch (Exception ex)
        {
        }
        if (string.IsNullOrEmpty(str) || str.ToLower() != "true")
          return false;
      }
      if (settingDefinition.Path.Contains("ENABLEGEICOINTEGRATION"))
      {
        string str = "false";
        try
        {
          str = (string) this.svrManager.GetServerSetting("POLICIES.ENABLEUIHOMEINSURANCE", false);
        }
        catch (Exception ex)
        {
        }
        if (string.IsNullOrEmpty(str) || str.ToLower() != "true")
          return false;
      }
      return settingDefinition.DisplayEnabled;
    }

    private class EnumLabel : Label
    {
      private EnumSettingDefinition def;
      private object innerValue = (object) 0;

      public EnumLabel(EnumSettingDefinition def) => this.def = def;

      public override string Text
      {
        get => base.Text;
        set => this.Value = this.def.NameProvider.GetValue(value);
      }

      public object Value
      {
        get => this.innerValue;
        set
        {
          this.innerValue = value;
          string name = this.def.NameProvider.GetName(value);
          string[] strArray = name.Split(',');
          if (strArray.Length == 1)
            base.Text = name;
          else
            base.Text = strArray[0] + ", ... (" + (object) (strArray.Length - 1) + " other" + (strArray.Length == 2 ? (object) "" : (object) "s") + ")";
        }
      }
    }

    private class BitmaskListBox : CheckedListBox
    {
      private BitmaskSettingDefinition def;

      public BitmaskListBox(BitmaskSettingDefinition def)
      {
        this.def = def;
        Array values = Enum.GetValues(def.EnumType);
        for (int index = 0; index < values.Length; ++index)
        {
          if ((int) values.GetValue(index) != 0)
            this.Items.Add((object) def.NameProvider.GetName(values.GetValue(index)), false);
        }
        this.SelectionMode = SelectionMode.One;
        this.CheckOnClick = true;
      }

      public override string Text
      {
        get => this.def.NameProvider.GetName(this.Value);
        set => this.Value = this.def.NameProvider.GetValue(value);
      }

      public object Value
      {
        get
        {
          int num = 0;
          for (int index = 0; index < this.Items.Count; ++index)
          {
            if (this.GetItemChecked(index))
              num |= (int) this.def.NameProvider.GetValue(this.Items[index].ToString());
          }
          return Enum.ToObject(this.def.EnumType, num);
        }
        set
        {
          for (int index = 0; index < this.Items.Count; ++index)
          {
            int num = (int) this.def.NameProvider.GetValue(this.Items[index].ToString());
            this.SetItemChecked(index, ((int) value & num) == num);
          }
        }
      }

      protected override void OnItemCheck(ItemCheckEventArgs e)
      {
        base.OnItemCheck(e);
        int num1 = (int) this.def.NameProvider.GetValue(this.Items[e.Index].ToString());
        if (e.NewValue == CheckState.Checked)
        {
          for (int index = 0; index < this.Items.Count; ++index)
          {
            if (index != e.Index && !this.GetItemChecked(index))
            {
              int num2 = (int) this.def.NameProvider.GetValue(this.Items[index].ToString());
              if ((num2 & num1) == num2)
                this.SetItemChecked(index, true);
            }
          }
        }
        else
        {
          for (int index = 0; index < this.Items.Count; ++index)
          {
            if (index != e.Index && this.GetItemChecked(index) && ((int) this.def.NameProvider.GetValue(this.Items[index].ToString()) & num1) == num1)
              this.SetItemChecked(index, false);
          }
        }
      }
    }
  }
}
