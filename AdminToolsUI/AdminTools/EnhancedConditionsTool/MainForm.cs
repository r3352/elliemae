// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.EnhancedConditionsTool.MainForm
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.eFolder;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.AdminTools.EnhancedConditionsTool
{
  public class MainForm : Form
  {
    private int _unpainted = 1;
    private IContainer components;
    private TableLayoutPanel FormLayout;
    private FlowLayoutPanel ActionButtonsLayout;
    private Button btnBack;
    private Button btnNext;
    private Button btnCancel;
    private SourceModeSelector SourceModeSelector;
    private EnhancedConditionsSelector EnhancedConditionsSelector;
    private Panel StageContainer;
    private ValidationViewer ValidationViewer;
    private SyncResultViewer SyncResultsViewer;
    private StandardConditionsConverter StandardConditionsConverter;

    private Sessions.Session GridViewConfig => Session.DefaultInstance;

    private IEnhancedConditionsProvider Provider { get; set; }

    private MainForm.SourceMode SelectedMode => this.SourceModeSelector.SelectedMode;

    private MainForm.FormStage CurrentStage { get; set; }

    private ExportablePackage SourcePackage { get; set; }

    private IList<ConditionTemplate> SourceConditions { get; set; }

    private ExportablePackage TargetPackage { get; set; }

    private MainForm.StageWindowManager WindowManager { get; set; }

    public MainForm()
    {
      this.InitializeComponent();
      this.Provider = (IEnhancedConditionsProvider) new DefaultProvider((Control) this);
      this.WindowManager = new MainForm.StageWindowManager((Form) this);
      this.SetStage(MainForm.FormStage.ModeSelector);
    }

    protected override void OnClosing(CancelEventArgs e)
    {
      this.WindowManager.OnClosing();
      base.OnClosing(e);
    }

    protected void SetStage(MainForm.FormStage newStage)
    {
      bool flag1 = true;
      bool flag2 = this.CurrentStage != newStage && (this.CurrentStage != MainForm.FormStage.SourceLogin || newStage != MainForm.FormStage.ModeSelector) && (this.CurrentStage != MainForm.FormStage.TargetLogin || newStage != MainForm.FormStage.SourceSelector) && (this.CurrentStage != MainForm.FormStage.TargetImporting || newStage != MainForm.FormStage.TargetConfirmation);
      if (flag2)
      {
        this.StageContainer.Controls.OfType<Control>().ToList<Control>().ForEach((Action<Control>) (c => c.Visible = false));
        this.WindowManager?.SetStageView(newStage);
      }
      this.CurrentStage = newStage;
      this.btnCancel.Text = newStage != MainForm.FormStage.Completed ? "Cancel" : "Exit";
      this.btnBack.Visible = newStage != MainForm.FormStage.ModeSelector;
      this.btnNext.Visible = newStage != MainForm.FormStage.Completed;
      Control control = (Control) null;
      string str = this.SelectedMode == MainForm.SourceMode.Transfer ? "to be migrated between instances" : "to be imported from a file";
      switch (newStage)
      {
        case MainForm.FormStage.ModeSelector:
          control = (Control) this.SourceModeSelector;
          this.btnNext.Enabled = this.SelectedMode != 0;
          this.btnNext.Focus();
          flag1 = false;
          break;
        case MainForm.FormStage.SourceSelector:
          if (this.SelectedMode == MainForm.SourceMode.Convert)
          {
            control = (Control) this.StandardConditionsConverter;
            Button btnNext = this.btnNext;
            IList<Guid> assignedItemIds = this.StandardConditionsConverter.GetAssignedItemIDs();
            int num = (assignedItemIds != null ? new bool?(assignedItemIds.Any<Guid>()) : new bool?()).Value ? 1 : 0;
            btnNext.Enabled = num != 0;
            break;
          }
          control = (Control) this.EnhancedConditionsSelector;
          this.EnhancedConditionsSelector.SetMessage("Please select condition templates " + str);
          Button btnNext1 = this.btnNext;
          IList<Guid?> selectedItemIds1 = this.EnhancedConditionsSelector.GetSelectedItemIDs();
          int num1 = (selectedItemIds1 != null ? new bool?(selectedItemIds1.Any<Guid?>()) : new bool?()).Value ? 1 : 0;
          btnNext1.Enabled = num1 != 0;
          break;
        case MainForm.FormStage.TargetConfirmation:
          control = (Control) this.ValidationViewer;
          this.ValidationViewer.SetMessage("Please review and confirm condition templates " + str);
          Button btnNext2 = this.btnNext;
          IList<Guid?> selectedItemIds2 = this.ValidationViewer.GetSelectedItemIDs();
          int num2 = (selectedItemIds2 != null ? new bool?(selectedItemIds2.Any<Guid?>()) : new bool?()).Value ? 1 : 0;
          btnNext2.Enabled = num2 != 0;
          break;
        case MainForm.FormStage.Completed:
          control = (Control) this.SyncResultsViewer;
          SyncResult result = this.SyncResultsViewer.Result;
          if (result.ImportCount != 0 || result.Status == SyncStatus.ImportCanceled)
          {
            this.btnBack.Visible = false;
            break;
          }
          break;
      }
      this.MaximizeBox = flag1;
      if (flag2 && !flag1 && this.WindowState == FormWindowState.Maximized)
      {
        this.WindowState = FormWindowState.Normal;
        this.WindowManager?.CenterWindow();
      }
      if (control == null)
        return;
      control.Location = new Point(0, 0);
      control.Dock = DockStyle.Fill;
      control.Visible = true;
    }

    private async Task DoNext()
    {
      MainForm mainForm = this;
      MainForm.FormStage nextStage = mainForm.CurrentStage;
      switch (mainForm.CurrentStage)
      {
        case MainForm.FormStage.ModeSelector:
          mainForm.CurrentStage = MainForm.FormStage.SourceLogin;
          nextStage = await mainForm.GetSourceConditions() ? MainForm.FormStage.SourceSelector : MainForm.FormStage.ModeSelector;
          if (MainForm.FormStage.SourceSelector == nextStage)
          {
            // ISSUE: reference to a compiler-generated method
            if (!await mainForm.CatchErrors("initializing", new Action(mainForm.\u003CDoNext\u003Eb__34_0)))
            {
              nextStage = MainForm.FormStage.ModeSelector;
              break;
            }
            break;
          }
          break;
        case MainForm.FormStage.SourceSelector:
          if (mainForm.SelectedMode != MainForm.SourceMode.Convert)
          {
            mainForm.CurrentStage = MainForm.FormStage.TargetLogin;
            nextStage = await mainForm.GetTargetConditions() ? MainForm.FormStage.TargetConfirmation : MainForm.FormStage.SourceSelector;
            if (MainForm.FormStage.TargetConfirmation == nextStage)
            {
              // ISSUE: reference to a compiler-generated method
              if (!await mainForm.CatchErrors("validating", new Action(mainForm.\u003CDoNext\u003Eb__34_1)))
              {
                nextStage = MainForm.FormStage.SourceSelector;
                break;
              }
              break;
            }
            break;
          }
          goto case MainForm.FormStage.TargetConfirmation;
        case MainForm.FormStage.TargetConfirmation:
          mainForm.CurrentStage = MainForm.FormStage.TargetImporting;
          // ISSUE: reference to a compiler-generated method
          SyncResult result = await mainForm.CatchErrorsAsync<SyncResult>("synchronizing", new Func<Task<SyncResult>>(mainForm.\u003CDoNext\u003Eb__34_2));
          nextStage = result != null ? MainForm.FormStage.Completed : MainForm.FormStage.TargetConfirmation;
          if (MainForm.FormStage.Completed == nextStage)
          {
            mainForm.SyncResultsViewer.Init(mainForm.SelectedMode, mainForm.SourcePackage, result, mainForm.GridViewConfig);
            break;
          }
          if (mainForm.SelectedMode == MainForm.SourceMode.Convert)
          {
            nextStage = MainForm.FormStage.SourceSelector;
            break;
          }
          break;
      }
      mainForm.SetStage(nextStage);
    }

    private void DoBack()
    {
      MainForm.FormStage newStage = this.CurrentStage;
      switch (this.CurrentStage)
      {
        case MainForm.FormStage.SourceSelector:
          newStage = MainForm.FormStage.ModeSelector;
          break;
        case MainForm.FormStage.TargetConfirmation:
          newStage = MainForm.FormStage.SourceSelector;
          break;
        case MainForm.FormStage.Completed:
          newStage = MainForm.FormStage.TargetConfirmation;
          if (this.SelectedMode == MainForm.SourceMode.Convert)
          {
            newStage = MainForm.FormStage.SourceSelector;
            break;
          }
          break;
      }
      this.SetStage(newStage);
    }

    private async Task<T> CatchErrorsAsync<T>(string description, Func<Task<T>> toTry)
    {
      MainForm owner = this;
      T result = default (T);
      Cursor cursor = owner.Cursor;
      owner.SetCursor(Cursors.WaitCursor);
      try
      {
        result = await toTry();
      }
      catch (Exception ex)
      {
        IEnumerable<string> source;
        if (ex is AggregateException aggregateException)
          source = aggregateException.InnerExceptions.Select<Exception, string>((Func<Exception, string>) (ie => ie.Message));
        else
          source = (IEnumerable<string>) new string[1]
          {
            ex.Message
          };
        List<string> list = source.ToList<string>();
        Console.WriteLine("Error while " + description + "\nStack trace: " + ex.StackTrace);
        int num = (int) Utils.Dialog((IWin32Window) owner, "Error while " + description + "\nThe error" + (list.Count > 1 ? "s were" : " was") + ": " + string.Join("\n", (IEnumerable<string>) list), MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      owner.SetCursor(cursor);
      return result;
    }

    private async void SetCursor(Cursor cursor)
    {
      this.Cursor = cursor;
      Cursor.Current = cursor;
      await Task.Delay(0);
    }

    private async Task<T> CatchErrors<T>(string description, Func<T> toTry)
    {
      return await this.CatchErrorsAsync<T>(description, (Func<Task<T>>) (() => Task.Run<T>((Func<T>) (() => toTry()))));
    }

    private async Task<bool> CatchErrors(string description, Action toTry)
    {
      return await this.CatchErrorsAsync<bool>(description, (Func<Task<bool>>) (() =>
      {
        toTry();
        return Task.FromResult<bool>(true);
      }));
    }

    private async Task<bool> GetSourceConditions()
    {
      bool isSource = true;
      switch (this.SelectedMode)
      {
        case MainForm.SourceMode.Transfer:
          return (this.SourcePackage = await this.CatchErrorsAsync<ExportablePackage>("reading from source system", (Func<Task<ExportablePackage>>) (() => this.Provider.GetEnhancedConditions(isSource)))) != null;
        case MainForm.SourceMode.Convert:
          bool sourceConditions = (this.SourcePackage = this.TargetPackage = await this.CatchErrorsAsync<ExportablePackage>("reading types", (Func<Task<ExportablePackage>>) (() => this.Provider.GetEnhancedConditions(isSource)))) != null;
          if (sourceConditions)
            sourceConditions = (this.SourceConditions = await this.CatchErrorsAsync<IList<ConditionTemplate>>("reading standard conditions", (Func<Task<IList<ConditionTemplate>>>) (() => this.Provider.GetStandardConditions()))) != null;
          return sourceConditions;
        case MainForm.SourceMode.FileImport:
          return (this.SourcePackage = await this.CatchErrors<ExportablePackage>("reading from source file", (Func<ExportablePackage>) (() => new JsonFile<ExportablePackage>((Control) this).DoImport()))) != null;
        default:
          return false;
      }
    }

    private async Task<bool> GetTargetConditions()
    {
      MainForm mainForm = this;
      // ISSUE: reference to a compiler-generated method
      ExportablePackage exportablePackage = await mainForm.CatchErrorsAsync<ExportablePackage>("reading from target system", new Func<Task<ExportablePackage>>(mainForm.\u003CGetTargetConditions\u003Eb__41_0));
      return (mainForm.TargetPackage = exportablePackage) != null;
    }

    private void InitializeSourceSelector()
    {
      if (this.SelectedMode != MainForm.SourceMode.Convert)
        this.EnhancedConditionsSelector.Init(this.SourcePackage.Templates, this.GridViewConfig);
      else
        this.StandardConditionsConverter.Init((IEnumerable<ConditionTemplate>) this.SourceConditions, this.SourcePackage, this.GridViewConfig);
    }

    private ExportablePackage GetPackageToValidate()
    {
      if (MainForm.SourceMode.Convert == this.SelectedMode)
        throw new InvalidOperationException("Invalid function for this mode");
      ExportablePackage packageToValidate = new ExportablePackage();
      packageToValidate.Documents = this.SourcePackage.Documents;
      packageToValidate.Roles = this.SourcePackage.Roles;
      packageToValidate.Types = this.SourcePackage.Types;
      packageToValidate.Owner = this.SourcePackage.Owner;
      IList<Guid?> selectedItemIDs = this.EnhancedConditionsSelector.GetSelectedItemIDs();
      packageToValidate.Templates = this.SourcePackage.Templates.Where<EnhancedConditionTemplate>((Func<EnhancedConditionTemplate, bool>) (t => selectedItemIDs.Contains(t.Id)));
      return packageToValidate;
    }

    private async Task<SyncResult> DoSynchronization()
    {
      SyncResult syncResult = new SyncResult();
      bool flag1 = this.SelectedMode == MainForm.SourceMode.Convert;
      ExportablePackage target = this.ValidationViewer.Validator?.Target;
      ExportablePackage source = this.SourcePackage = flag1 ? this.StandardConditionsConverter.GetConversionPackage(this.TargetPackage.Documents) : this.ValidationViewer.GetSyncPackage();
      int totalCount = source.Templates.Count<EnhancedConditionTemplate>();
      SyncProgressManager progressManager = new SyncProgressManager(totalCount);
      CancellationToken cancellationToken = progressManager.CancellationToken;
      if (!flag1)
        source.RemapIdentifiers(target);
      bool[] flagArray = new bool[2]{ true, false };
      for (int index = 0; index < flagArray.Length; ++index)
      {
        bool useInsert = flagArray[index];
        if (!cancellationToken.IsCancellationRequested)
        {
          EnhancedConditionTemplate[] group = source.Templates.Where<EnhancedConditionTemplate>((Func<EnhancedConditionTemplate, bool>) (t => !useInsert ? t.Id.HasValue : !t.Id.HasValue)).ToArray<EnhancedConditionTemplate>();
          if (group.Length != 0)
          {
            try
            {
              progressManager.Dialog.Refresh();
              syncResult.ImportCount += (await Task.Run<SyncResult>((Func<Task<SyncResult>>) (() => this.Provider.UpsertEnhancedConditionTemplates((IEnumerable<EnhancedConditionTemplate>) group, useInsert, cancellationToken)), cancellationToken)).ImportCount;
              progressManager.Progress.Report(new Ratio(syncResult.ImportCount, totalCount));
            }
            catch (TaskCanceledException ex)
            {
              syncResult.Status = SyncStatus.ImportCanceled;
              syncResult.Errors.Add(new SyncError()
              {
                Message = "Operation canceled."
              });
              syncResult.Errors.AddRange(((IEnumerable<EnhancedConditionTemplate>) group).Select<EnhancedConditionTemplate, SyncError>((Func<EnhancedConditionTemplate, SyncError>) (t => new SyncError()
              {
                Template = t,
                Message = "Unknown status"
              })));
            }
            catch (Exception ex)
            {
              HttpException httpException = ex as HttpException;
              if (ex is AggregateException aggregateException)
                httpException = aggregateException.InnerExceptions[0] as HttpException;
              if (httpException == null)
              {
                progressManager.Dialog.DoClose();
                progressManager.Dispose();
                throw ex;
              }
              syncResult.Errors.AddRange(SyncError.MapApiErrors(httpException.Data[(object) "error"] as EnhanceConditionError, (IList<EnhancedConditionTemplate>) group));
            }
            progressManager.Progress.Report(new Ratio(totalCount, totalCount));
            await Task.Delay(250);
            progressManager.Dialog.DoClose();
          }
        }
        else
          break;
      }
      flagArray = (bool[]) null;
      progressManager.Dispose();
      bool flag2 = syncResult.ImportCount > 0 || syncResult.Errors.Count > 0;
      if (syncResult.Status == SyncStatus.None)
        syncResult.Status = flag2 ? SyncStatus.Success : SyncStatus.ImportFailed;
      return syncResult;
    }

    private void MainForm_Paint(object sender, PaintEventArgs e)
    {
      if (Interlocked.CompareExchange(ref this._unpainted, 0, 1) != 1)
        return;
      this.WindowManager?.CenterWindow();
    }

    private async void btnNext_Click(object sender, EventArgs e) => await this.DoNext();

    private void btnBack_Click(object sender, EventArgs e) => this.DoBack();

    private void btnCancel_Click(object sender, EventArgs e) => this.Close();

    private void sourceModeSelector_OnSelectionChanged(object sender, MainForm.SourceMode e)
    {
      this.SetStage(this.CurrentStage);
    }

    private async void sourceModeSelector_OnSelectionDoubleClick(
      object sender,
      MainForm.SourceMode e)
    {
      await this.DoNext();
    }

    private void standardConditionsConverter_OnSelectionChanged(object sender, int e)
    {
      this.SetStage(this.CurrentStage);
    }

    private void enhancedConditionsSelector_OnSelectionChanged(object sender, int e)
    {
      this.SetStage(this.CurrentStage);
    }

    private void ValidationViewer_OnSelectionChanged(object sender, int e)
    {
      this.SetStage(this.CurrentStage);
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
      if (this.CurrentStage == MainForm.FormStage.ModeSelector)
      {
        bool flag1 = Keys.Left == keyData;
        bool flag2 = Keys.Right == keyData;
        bool flag3 = Keys.Tab == keyData || (Keys.Tab | Keys.Shift) == keyData;
        bool flag4 = keyData.HasFlag((Enum) Keys.Shift);
        if (flag1 | flag2 | flag3)
        {
          this.SourceModeSelector.SelectNextOption(flag1 || flag3 & flag4);
          return true;
        }
      }
      return base.ProcessCmdKey(ref msg, keyData);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MainForm));
      this.FormLayout = new TableLayoutPanel();
      this.ActionButtonsLayout = new FlowLayoutPanel();
      this.btnBack = new Button();
      this.btnNext = new Button();
      this.btnCancel = new Button();
      this.StageContainer = new Panel();
      this.StandardConditionsConverter = new StandardConditionsConverter();
      this.SyncResultsViewer = new SyncResultViewer();
      this.EnhancedConditionsSelector = new EnhancedConditionsSelector();
      this.ValidationViewer = new ValidationViewer();
      this.SourceModeSelector = new SourceModeSelector();
      this.FormLayout.SuspendLayout();
      this.ActionButtonsLayout.SuspendLayout();
      this.StageContainer.SuspendLayout();
      this.SuspendLayout();
      this.FormLayout.ColumnCount = 1;
      this.FormLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
      this.FormLayout.Controls.Add((Control) this.ActionButtonsLayout, 0, 1);
      this.FormLayout.Controls.Add((Control) this.StageContainer, 0, 0);
      this.FormLayout.Dock = DockStyle.Fill;
      this.FormLayout.Location = new Point(0, 0);
      this.FormLayout.Margin = new Padding(0);
      this.FormLayout.Name = "FormLayout";
      this.FormLayout.Padding = new Padding(8);
      this.FormLayout.RowCount = 2;
      this.FormLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
      this.FormLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 64f));
      this.FormLayout.Size = new Size(1355, 1172);
      this.FormLayout.TabIndex = 0;
      this.ActionButtonsLayout.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.ActionButtonsLayout.AutoSize = true;
      this.ActionButtonsLayout.Controls.Add((Control) this.btnBack);
      this.ActionButtonsLayout.Controls.Add((Control) this.btnNext);
      this.ActionButtonsLayout.Controls.Add((Control) this.btnCancel);
      this.ActionButtonsLayout.Location = new Point(999, 1126);
      this.ActionButtonsLayout.Margin = new Padding(0);
      this.ActionButtonsLayout.Name = "ActionButtonsLayout";
      this.ActionButtonsLayout.Size = new Size(348, 38);
      this.ActionButtonsLayout.TabIndex = 0;
      this.ActionButtonsLayout.WrapContents = false;
      this.btnBack.AutoSize = true;
      this.btnBack.Location = new Point(8, 0);
      this.btnBack.Margin = new Padding(8, 0, 0, 0);
      this.btnBack.MinimumSize = new Size(108, 0);
      this.btnBack.Name = "btnBack";
      this.btnBack.Size = new Size(108, 38);
      this.btnBack.TabIndex = 0;
      this.btnBack.Text = "< Back";
      this.btnBack.UseVisualStyleBackColor = true;
      this.btnBack.Click += new EventHandler(this.btnBack_Click);
      this.btnNext.AutoSize = true;
      this.btnNext.Location = new Point(124, 0);
      this.btnNext.Margin = new Padding(8, 0, 0, 0);
      this.btnNext.MinimumSize = new Size(108, 0);
      this.btnNext.Name = "btnNext";
      this.btnNext.Size = new Size(108, 38);
      this.btnNext.TabIndex = 1;
      this.btnNext.Text = "Next >";
      this.btnNext.UseVisualStyleBackColor = true;
      this.btnNext.Click += new EventHandler(this.btnNext_Click);
      this.btnCancel.AutoSize = true;
      this.btnCancel.Location = new Point(240, 0);
      this.btnCancel.Margin = new Padding(8, 0, 0, 0);
      this.btnCancel.MinimumSize = new Size(108, 0);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(108, 38);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.StageContainer.Controls.Add((Control) this.StandardConditionsConverter);
      this.StageContainer.Controls.Add((Control) this.SyncResultsViewer);
      this.StageContainer.Controls.Add((Control) this.EnhancedConditionsSelector);
      this.StageContainer.Controls.Add((Control) this.ValidationViewer);
      this.StageContainer.Controls.Add((Control) this.SourceModeSelector);
      this.StageContainer.Dock = DockStyle.Fill;
      this.StageContainer.Location = new Point(11, 11);
      this.StageContainer.Name = "StageContainer";
      this.StageContainer.Size = new Size(1333, 1086);
      this.StageContainer.TabIndex = 3;
      this.StandardConditionsConverter.Location = new Point(-2, 150);
      this.StandardConditionsConverter.Margin = new Padding(0);
      this.StandardConditionsConverter.Name = "StandardConditionsConverter";
      this.StandardConditionsConverter.Size = new Size(1333, 268);
      this.StandardConditionsConverter.TabIndex = 6;
      this.StandardConditionsConverter.OnAssignmentChanged += new EventHandler<int>(this.standardConditionsConverter_OnSelectionChanged);
      this.SyncResultsViewer.Location = new Point(3, 932);
      this.SyncResultsViewer.Name = "SyncResultsViewer";
      this.SyncResultsViewer.Size = new Size(1333, 147);
      this.SyncResultsViewer.TabIndex = 5;
      this.EnhancedConditionsSelector.Location = new Point(-2, 418);
      this.EnhancedConditionsSelector.Margin = new Padding(0);
      this.EnhancedConditionsSelector.Name = "EnhancedConditionsSelector";
      this.EnhancedConditionsSelector.Size = new Size(1333, 268);
      this.EnhancedConditionsSelector.TabIndex = 2;
      this.EnhancedConditionsSelector.OnSelectionChanged += new EventHandler<int>(this.enhancedConditionsSelector_OnSelectionChanged);
      this.ValidationViewer.Location = new Point(1, 689);
      this.ValidationViewer.Name = "ValidationViewer";
      this.ValidationViewer.Size = new Size(1333, 237);
      this.ValidationViewer.TabIndex = 4;
      this.ValidationViewer.Validator = (PackageValidator) null;
      this.ValidationViewer.OnSelectionChanged += new EventHandler<int>(this.ValidationViewer_OnSelectionChanged);
      this.SourceModeSelector.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.SourceModeSelector.Location = new Point(0, 0);
      this.SourceModeSelector.Margin = new Padding(0);
      this.SourceModeSelector.Name = "SourceModeSelector";
      this.SourceModeSelector.Size = new Size(1333, 150);
      this.SourceModeSelector.TabIndex = 1;
      this.SourceModeSelector.OnSelectionChanged += new EventHandler<MainForm.SourceMode>(this.sourceModeSelector_OnSelectionChanged);
      this.SourceModeSelector.OnSelectionDoubleClick += new EventHandler<MainForm.SourceMode>(this.sourceModeSelector_OnSelectionDoubleClick);
      this.AutoScaleDimensions = new SizeF(9f, 20f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1355, 1172);
      this.Controls.Add((Control) this.FormLayout);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MinimumSize = new Size(720, 320);
      this.Name = nameof (MainForm);
      this.Text = "Enhanced Conditions Tool";
      this.Paint += new PaintEventHandler(this.MainForm_Paint);
      this.FormLayout.ResumeLayout(false);
      this.FormLayout.PerformLayout();
      this.ActionButtonsLayout.ResumeLayout(false);
      this.ActionButtonsLayout.PerformLayout();
      this.StageContainer.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public enum SourceMode
    {
      None,
      Transfer,
      Convert,
      FileImport,
    }

    protected enum FormStage
    {
      None,
      ModeSelector,
      SourceLogin,
      SourceSelector,
      TargetLogin,
      TargetConfirmation,
      TargetImporting,
      Completed,
    }

    private class StageWindowManager
    {
      private MainForm.StageWindowManager.WindowView _deferredView;

      public StageWindowManager(Form window) => this.Init(window);

      private Form Window { get; set; }

      private Screen DefaultScreen { get; set; }

      private Dictionary<MainForm.FormStage, MainForm.StageWindowManager.WindowView> StageViews { get; set; }

      private bool IsResizing { get; set; }

      protected void Init(Form window)
      {
        ProcessStartInfo startInfo = Process.GetCurrentProcess().StartInfo;
        WeakReference weakReference = startInfo.GetType().GetField("weakParentProcess", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue((object) startInfo) as WeakReference;
        IntPtr? nullable = (weakReference.IsAlive ? weakReference.Target : (object) null) is Process target ? new IntPtr?(target.MainWindowHandle) : new IntPtr?();
        Screen screen = nullable.HasValue ? Screen.FromHandle(nullable.Value) : Screen.PrimaryScreen;
        this.Window = window;
        this.DefaultScreen = screen;
        this.StageViews = new Dictionary<MainForm.FormStage, MainForm.StageWindowManager.WindowView>()
        {
          {
            MainForm.FormStage.None,
            new MainForm.StageWindowManager.WindowView(new Size(720, 320), this.DefaultScreen)
          },
          {
            MainForm.FormStage.ModeSelector,
            new MainForm.StageWindowManager.WindowView(new Size(485, 222), this.DefaultScreen)
          },
          {
            MainForm.FormStage.SourceSelector,
            new MainForm.StageWindowManager.WindowView(new Size(1042, 480), this.DefaultScreen)
          },
          {
            MainForm.FormStage.TargetConfirmation,
            new MainForm.StageWindowManager.WindowView(new Size(864, 480), this.DefaultScreen)
          },
          {
            MainForm.FormStage.Completed,
            new MainForm.StageWindowManager.WindowView(new Size(864, 480), this.DefaultScreen)
          }
        };
        this.Window.ResizeBegin += new EventHandler(this.Window_ResizeBegin);
        this.Window.ResizeEnd += new EventHandler(this.Window_ResizeEnd);
      }

      public void OnClosing()
      {
        this.Window.ResizeBegin -= new EventHandler(this.Window_ResizeBegin);
        this.Window.ResizeEnd -= new EventHandler(this.Window_ResizeEnd);
      }

      private void Window_ResizeBegin(object sender, EventArgs e) => this.IsResizing = true;

      private void Window_ResizeEnd(object sender, EventArgs e)
      {
        this.IsResizing = false;
        MainForm.StageWindowManager.WindowView savedState = Interlocked.Exchange<MainForm.StageWindowManager.WindowView>(ref this._deferredView, (MainForm.StageWindowManager.WindowView) null);
        if (savedState == null)
          return;
        this.UpdateWindow(savedState);
      }

      private void UpdateWindow(MainForm.StageWindowManager.WindowView savedState)
      {
        this.Window.Size = savedState.Size;
        this.Window.WindowState = savedState.State;
        this.Window.Location = savedState.Location;
        this.CenterWindow(this.Window.Location);
      }

      private MainForm.FormStage LastSetStage { get; set; }

      public void SetStageView(MainForm.FormStage stage)
      {
        Dictionary<MainForm.FormStage, MainForm.StageWindowManager.WindowView> stageViews = this.StageViews;
        int lastSetStage = (int) this.LastSetStage;
        Size size = this.Window.Size;
        int windowState = (int) this.Window.WindowState;
        Point location = this.Window.Location;
        int x = location.X;
        location = this.Window.Location;
        int y = location.Y;
        MainForm.StageWindowManager.WindowView windowView = new MainForm.StageWindowManager.WindowView(size, (FormWindowState) windowState, x, y);
        stageViews[(MainForm.FormStage) lastSetStage] = windowView;
        MainForm.StageWindowManager.WindowView stageView = this.StageViews[stage];
        this.LastSetStage = stage;
        if (this.IsResizing)
          this._deferredView = stageView;
        else
          this.UpdateWindow(stageView);
      }

      public void CenterWindow()
      {
        Rectangle bounds = this.DefaultScreen.Bounds;
        this.CenterWindow(new Point(bounds.X, bounds.Y));
      }

      public void CenterWindow(Point screenLocation)
      {
        Screen screen = Screen.FromPoint(screenLocation);
        if (screen == null)
          return;
        Rectangle bounds = screen.Bounds;
        this.Window.Location = new Point(bounds.X + (bounds.Width - this.Window.Size.Width >> 1), bounds.Y + (bounds.Height - this.Window.Size.Height >> 1));
      }

      private class WindowView
      {
        public Size Size { get; set; }

        public FormWindowState State { get; set; }

        public Point Location { get; set; }

        public WindowView(Size size, FormWindowState state = FormWindowState.Normal, int x = 0, int y = 0)
        {
          this.Size = size;
          this.State = state;
          this.Location = new Point(x, y);
        }

        public WindowView(Size size, Screen screen)
        {
          this.Size = size;
          this.State = FormWindowState.Normal;
          Rectangle bounds = screen.Bounds;
          int x = bounds.X;
          bounds = screen.Bounds;
          int y = bounds.Y;
          this.Location = new Point(x, y);
        }
      }
    }
  }
}
