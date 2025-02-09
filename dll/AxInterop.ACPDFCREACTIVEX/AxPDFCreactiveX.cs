// Decompiled with JetBrains decompiler
// Type: AxACPDFCREACTIVEX.AxPDFCreactiveX
// Assembly: AxInterop.ACPDFCREACTIVEX, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50348D5A-A8E2-4894-AD2C-0D88350B72D8
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\AxInterop.ACPDFCREACTIVEX.dll

using ACPDFCREACTIVEX;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

#nullable disable
namespace AxACPDFCREACTIVEX
{
  [DesignTimeVisible(true)]
  [DefaultProperty("Document")]
  [AxHost.Clsid("{525ca8d6-81ee-4aed-b95c-9c4dbad5980b}")]
  [DefaultEvent("BeforeDelete")]
  public class AxPDFCreactiveX : AxHost
  {
    private IPDFCreactiveX ocx;
    private AxPDFCreactiveXEventMulticaster eventMulticaster;
    private AxHost.ConnectionPointCookie cookie;

    public AxPDFCreactiveX()
      : base("525ca8d6-81ee-4aed-b95c-9c4dbad5980b")
    {
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [DispId(1)]
    public virtual short RulerSize
    {
      get
      {
        return this.ocx != null ? this.ocx.RulerSize : throw new AxHost.InvalidActiveXStateException(nameof (RulerSize), AxHost.ActiveXInvokeKind.PropertyGet);
      }
      set
      {
        if (this.ocx == null)
          throw new AxHost.InvalidActiveXStateException(nameof (RulerSize), AxHost.ActiveXInvokeKind.PropertySet);
        this.ocx.RulerSize = value;
      }
    }

    [DispId(3)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual ReportViewConstants ReportView
    {
      get
      {
        return this.ocx != null ? this.ocx.ReportView : throw new AxHost.InvalidActiveXStateException(nameof (ReportView), AxHost.ActiveXInvokeKind.PropertyGet);
      }
      set
      {
        if (this.ocx == null)
          throw new AxHost.InvalidActiveXStateException(nameof (ReportView), AxHost.ActiveXInvokeKind.PropertySet);
        this.ocx.ReportView = value;
      }
    }

    [DispId(5)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual ReportStateConstants ReportState
    {
      get
      {
        return this.ocx != null ? this.ocx.ReportState : throw new AxHost.InvalidActiveXStateException(nameof (ReportState), AxHost.ActiveXInvokeKind.PropertyGet);
      }
      set
      {
        if (this.ocx == null)
          throw new AxHost.InvalidActiveXStateException(nameof (ReportState), AxHost.ActiveXInvokeKind.PropertySet);
        this.ocx.ReportState = value;
      }
    }

    [DispId(6)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual int ZoomFactor
    {
      get
      {
        return this.ocx != null ? this.ocx.ZoomFactor : throw new AxHost.InvalidActiveXStateException(nameof (ZoomFactor), AxHost.ActiveXInvokeKind.PropertyGet);
      }
      set
      {
        if (this.ocx == null)
          throw new AxHost.InvalidActiveXStateException(nameof (ZoomFactor), AxHost.ActiveXInvokeKind.PropertySet);
        this.ocx.ZoomFactor = value;
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [DispId(8)]
    public virtual GridViewConstants GridView
    {
      get
      {
        return this.ocx != null ? this.ocx.GridView : throw new AxHost.InvalidActiveXStateException(nameof (GridView), AxHost.ActiveXInvokeKind.PropertyGet);
      }
      set
      {
        if (this.ocx == null)
          throw new AxHost.InvalidActiveXStateException(nameof (GridView), AxHost.ActiveXInvokeKind.PropertySet);
        this.ocx.GridView = value;
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [DispId(10)]
    public virtual int AutoRefresh
    {
      get
      {
        return this.ocx != null ? this.ocx.AutoRefresh : throw new AxHost.InvalidActiveXStateException(nameof (AutoRefresh), AxHost.ActiveXInvokeKind.PropertyGet);
      }
      set
      {
        if (this.ocx == null)
          throw new AxHost.InvalidActiveXStateException(nameof (AutoRefresh), AxHost.ActiveXInvokeKind.PropertySet);
        this.ocx.AutoRefresh = value;
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [DispId(11)]
    public virtual int FitToParent
    {
      get
      {
        return this.ocx != null ? this.ocx.FitToParent : throw new AxHost.InvalidActiveXStateException(nameof (FitToParent), AxHost.ActiveXInvokeKind.PropertyGet);
      }
      set
      {
        if (this.ocx == null)
          throw new AxHost.InvalidActiveXStateException(nameof (FitToParent), AxHost.ActiveXInvokeKind.PropertySet);
        this.ocx.FitToParent = value;
      }
    }

    [DispId(24)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual int MinimumGap
    {
      get
      {
        return this.ocx != null ? this.ocx.MinimumGap : throw new AxHost.InvalidActiveXStateException(nameof (MinimumGap), AxHost.ActiveXInvokeKind.PropertyGet);
      }
      set
      {
        if (this.ocx == null)
          throw new AxHost.InvalidActiveXStateException(nameof (MinimumGap), AxHost.ActiveXInvokeKind.PropertySet);
        this.ocx.MinimumGap = value;
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [DispId(25)]
    public virtual int HorzScrollBar
    {
      get
      {
        return this.ocx != null ? this.ocx.HorzScrollBar : throw new AxHost.InvalidActiveXStateException(nameof (HorzScrollBar), AxHost.ActiveXInvokeKind.PropertyGet);
      }
      set
      {
        if (this.ocx == null)
          throw new AxHost.InvalidActiveXStateException(nameof (HorzScrollBar), AxHost.ActiveXInvokeKind.PropertySet);
        this.ocx.HorzScrollBar = value;
      }
    }

    [DispId(26)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual int VertScrollBar
    {
      get
      {
        return this.ocx != null ? this.ocx.VertScrollBar : throw new AxHost.InvalidActiveXStateException(nameof (VertScrollBar), AxHost.ActiveXInvokeKind.PropertyGet);
      }
      set
      {
        if (this.ocx == null)
          throw new AxHost.InvalidActiveXStateException(nameof (VertScrollBar), AxHost.ActiveXInvokeKind.PropertySet);
        this.ocx.VertScrollBar = value;
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [DispId(27)]
    public virtual int ShowMargins
    {
      get
      {
        return this.ocx != null ? this.ocx.ShowMargins : throw new AxHost.InvalidActiveXStateException(nameof (ShowMargins), AxHost.ActiveXInvokeKind.PropertyGet);
      }
      set
      {
        if (this.ocx == null)
          throw new AxHost.InvalidActiveXStateException(nameof (ShowMargins), AxHost.ActiveXInvokeKind.PropertySet);
        this.ocx.ShowMargins = value;
      }
    }

    [DispId(32)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual string DataSource
    {
      get
      {
        return this.ocx != null ? this.ocx.DataSource : throw new AxHost.InvalidActiveXStateException(nameof (DataSource), AxHost.ActiveXInvokeKind.PropertyGet);
      }
      set
      {
        if (this.ocx == null)
          throw new AxHost.InvalidActiveXStateException(nameof (DataSource), AxHost.ActiveXInvokeKind.PropertySet);
        this.ocx.DataSource = value;
      }
    }

    [DispId(33)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual object DataSourceEx
    {
      get
      {
        return this.ocx != null ? this.ocx.DataSourceEx : throw new AxHost.InvalidActiveXStateException(nameof (DataSourceEx), AxHost.ActiveXInvokeKind.PropertyGet);
      }
      set
      {
        if (this.ocx == null)
          throw new AxHost.InvalidActiveXStateException(nameof (DataSourceEx), AxHost.ActiveXInvokeKind.PropertySet);
        this.ocx.DataSourceEx = value;
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    [DispId(34)]
    public virtual acBookmark RootBookmark
    {
      get
      {
        return this.ocx != null ? this.ocx.RootBookmark : throw new AxHost.InvalidActiveXStateException(nameof (RootBookmark), AxHost.ActiveXInvokeKind.PropertyGet);
      }
    }

    [DispId(37)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual int Protected
    {
      get
      {
        return this.ocx != null ? this.ocx.Protected : throw new AxHost.InvalidActiveXStateException(nameof (Protected), AxHost.ActiveXInvokeKind.PropertyGet);
      }
    }

    [DispId(40)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual int PageCount
    {
      get
      {
        return this.ocx != null ? this.ocx.PageCount : throw new AxHost.InvalidActiveXStateException(nameof (PageCount), AxHost.ActiveXInvokeKind.PropertyGet);
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [DispId(41)]
    public virtual int CurrentPage
    {
      get
      {
        return this.ocx != null ? this.ocx.CurrentPage : throw new AxHost.InvalidActiveXStateException(nameof (CurrentPage), AxHost.ActiveXInvokeKind.PropertyGet);
      }
      set
      {
        if (this.ocx == null)
          throw new AxHost.InvalidActiveXStateException(nameof (CurrentPage), AxHost.ActiveXInvokeKind.PropertySet);
        this.ocx.CurrentPage = value;
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [DispId(43)]
    public virtual int Modified
    {
      get
      {
        return this.ocx != null ? this.ocx.Modified : throw new AxHost.InvalidActiveXStateException(nameof (Modified), AxHost.ActiveXInvokeKind.PropertyGet);
      }
      set
      {
        if (this.ocx == null)
          throw new AxHost.InvalidActiveXStateException(nameof (Modified), AxHost.ActiveXInvokeKind.PropertySet);
        this.ocx.Modified = value;
      }
    }

    [DispId(44)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual string DataTableOrView
    {
      get
      {
        return this.ocx != null ? this.ocx.DataTableOrView : throw new AxHost.InvalidActiveXStateException(nameof (DataTableOrView), AxHost.ActiveXInvokeKind.PropertyGet);
      }
      set
      {
        if (this.ocx == null)
          throw new AxHost.InvalidActiveXStateException(nameof (DataTableOrView), AxHost.ActiveXInvokeKind.PropertySet);
        this.ocx.DataTableOrView = value;
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [DispId(45)]
    public virtual int Copies
    {
      get
      {
        return this.ocx != null ? this.ocx.Copies : throw new AxHost.InvalidActiveXStateException(nameof (Copies), AxHost.ActiveXInvokeKind.PropertyGet);
      }
      set
      {
        if (this.ocx == null)
          throw new AxHost.InvalidActiveXStateException(nameof (Copies), AxHost.ActiveXInvokeKind.PropertySet);
        this.ocx.Copies = value;
      }
    }

    [DispId(46)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual short Duplex
    {
      get
      {
        return this.ocx != null ? this.ocx.Duplex : throw new AxHost.InvalidActiveXStateException(nameof (Duplex), AxHost.ActiveXInvokeKind.PropertyGet);
      }
      set
      {
        if (this.ocx == null)
          throw new AxHost.InvalidActiveXStateException(nameof (Duplex), AxHost.ActiveXInvokeKind.PropertySet);
        this.ocx.Duplex = value;
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [DispId(49)]
    public virtual int Linearized
    {
      get
      {
        return this.ocx != null ? this.ocx.Linearized : throw new AxHost.InvalidActiveXStateException(nameof (Linearized), AxHost.ActiveXInvokeKind.PropertyGet);
      }
      set
      {
        if (this.ocx == null)
          throw new AxHost.InvalidActiveXStateException(nameof (Linearized), AxHost.ActiveXInvokeKind.PropertySet);
        this.ocx.Linearized = value;
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [DispId(54)]
    public virtual int StatusBar
    {
      get
      {
        return this.ocx != null ? this.ocx.StatusBar : throw new AxHost.InvalidActiveXStateException(nameof (StatusBar), AxHost.ActiveXInvokeKind.PropertyGet);
      }
      set
      {
        if (this.ocx == null)
          throw new AxHost.InvalidActiveXStateException(nameof (StatusBar), AxHost.ActiveXInvokeKind.PropertySet);
        this.ocx.StatusBar = value;
      }
    }

    [DispId(55)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual object SelectedObject
    {
      get
      {
        return this.ocx != null ? this.ocx.SelectedObject : throw new AxHost.InvalidActiveXStateException(nameof (SelectedObject), AxHost.ActiveXInvokeKind.PropertyGet);
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [DispId(56)]
    public virtual int PageWidth
    {
      get
      {
        return this.ocx != null ? this.ocx.PageWidth : throw new AxHost.InvalidActiveXStateException(nameof (PageWidth), AxHost.ActiveXInvokeKind.PropertyGet);
      }
      set
      {
        if (this.ocx == null)
          throw new AxHost.InvalidActiveXStateException(nameof (PageWidth), AxHost.ActiveXInvokeKind.PropertySet);
        this.ocx.PageWidth = value;
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [DispId(57)]
    public virtual int PageLength
    {
      get
      {
        return this.ocx != null ? this.ocx.PageLength : throw new AxHost.InvalidActiveXStateException(nameof (PageLength), AxHost.ActiveXInvokeKind.PropertyGet);
      }
      set
      {
        if (this.ocx == null)
          throw new AxHost.InvalidActiveXStateException(nameof (PageLength), AxHost.ActiveXInvokeKind.PropertySet);
        this.ocx.PageLength = value;
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [DispId(60)]
    public virtual int VerticalNaviguationBar
    {
      get
      {
        return this.ocx != null ? this.ocx.VerticalNaviguationBar : throw new AxHost.InvalidActiveXStateException(nameof (VerticalNaviguationBar), AxHost.ActiveXInvokeKind.PropertyGet);
      }
      set
      {
        if (this.ocx == null)
          throw new AxHost.InvalidActiveXStateException(nameof (VerticalNaviguationBar), AxHost.ActiveXInvokeKind.PropertySet);
        this.ocx.VerticalNaviguationBar = value;
      }
    }

    [DispId(68)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual string PrintToFileName
    {
      get
      {
        return this.ocx != null ? this.ocx.PrintToFileName : throw new AxHost.InvalidActiveXStateException(nameof (PrintToFileName), AxHost.ActiveXInvokeKind.PropertyGet);
      }
      set
      {
        if (this.ocx == null)
          throw new AxHost.InvalidActiveXStateException(nameof (PrintToFileName), AxHost.ActiveXInvokeKind.PropertySet);
        this.ocx.PrintToFileName = value;
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [DispId(70)]
    public virtual object ParentContainer
    {
      get
      {
        return this.ocx != null ? this.ocx.ParentContainer : throw new AxHost.InvalidActiveXStateException(nameof (ParentContainer), AxHost.ActiveXInvokeKind.PropertyGet);
      }
      set
      {
        if (this.ocx == null)
          throw new AxHost.InvalidActiveXStateException(nameof (ParentContainer), AxHost.ActiveXInvokeKind.PropertySet);
        this.ocx.ParentContainer = value;
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [DispId(77)]
    public virtual acScaleConstants ScaleToWindow
    {
      get
      {
        return this.ocx != null ? this.ocx.ScaleToWindow : throw new AxHost.InvalidActiveXStateException(nameof (ScaleToWindow), AxHost.ActiveXInvokeKind.PropertyGet);
      }
      set
      {
        if (this.ocx == null)
          throw new AxHost.InvalidActiveXStateException(nameof (ScaleToWindow), AxHost.ActiveXInvokeKind.PropertySet);
        this.ocx.ScaleToWindow = value;
      }
    }

    [DispId(78)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual acScaleConstants ScaleToPrinter
    {
      get
      {
        return this.ocx != null ? this.ocx.ScaleToPrinter : throw new AxHost.InvalidActiveXStateException(nameof (ScaleToPrinter), AxHost.ActiveXInvokeKind.PropertyGet);
      }
      set
      {
        if (this.ocx == null)
          throw new AxHost.InvalidActiveXStateException(nameof (ScaleToPrinter), AxHost.ActiveXInvokeKind.PropertySet);
        this.ocx.ScaleToPrinter = value;
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [DispId(0)]
    public virtual object Document
    {
      get
      {
        return this.ocx != null ? this.ocx.Document : throw new AxHost.InvalidActiveXStateException(nameof (Document), AxHost.ActiveXInvokeKind.PropertyGet);
      }
    }

    [DispId(79)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual int SelectedObjectCount
    {
      get
      {
        return this.ocx != null ? this.ocx.SelectedObjectCount : throw new AxHost.InvalidActiveXStateException(nameof (SelectedObjectCount), AxHost.ActiveXInvokeKind.PropertyGet);
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [DispId(90)]
    public virtual int ReadOnly
    {
      get
      {
        return this.ocx != null ? this.ocx.ReadOnly : throw new AxHost.InvalidActiveXStateException(nameof (ReadOnly), AxHost.ActiveXInvokeKind.PropertyGet);
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [DispId(92)]
    public virtual int TemplateMode
    {
      get
      {
        return this.ocx != null ? this.ocx.TemplateMode : throw new AxHost.InvalidActiveXStateException(nameof (TemplateMode), AxHost.ActiveXInvokeKind.PropertyGet);
      }
      set
      {
        if (this.ocx == null)
          throw new AxHost.InvalidActiveXStateException(nameof (TemplateMode), AxHost.ActiveXInvokeKind.PropertySet);
        this.ocx.TemplateMode = value;
      }
    }

    [DispId(93)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual int TemplateVisible
    {
      get
      {
        return this.ocx != null ? this.ocx.TemplateVisible : throw new AxHost.InvalidActiveXStateException(nameof (TemplateVisible), AxHost.ActiveXInvokeKind.PropertyGet);
      }
      set
      {
        if (this.ocx == null)
          throw new AxHost.InvalidActiveXStateException(nameof (TemplateVisible), AxHost.ActiveXInvokeKind.PropertySet);
        this.ocx.TemplateVisible = value;
      }
    }

    [DispId(94)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual int TemplatePrint
    {
      get
      {
        return this.ocx != null ? this.ocx.TemplatePrint : throw new AxHost.InvalidActiveXStateException(nameof (TemplatePrint), AxHost.ActiveXInvokeKind.PropertyGet);
      }
      set
      {
        if (this.ocx == null)
          throw new AxHost.InvalidActiveXStateException(nameof (TemplatePrint), AxHost.ActiveXInvokeKind.PropertySet);
        this.ocx.TemplatePrint = value;
      }
    }

    [DispId(95)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual int TemplateRepeat
    {
      get
      {
        return this.ocx != null ? this.ocx.TemplateRepeat : throw new AxHost.InvalidActiveXStateException(nameof (TemplateRepeat), AxHost.ActiveXInvokeKind.PropertyGet);
      }
      set
      {
        if (this.ocx == null)
          throw new AxHost.InvalidActiveXStateException(nameof (TemplateRepeat), AxHost.ActiveXInvokeKind.PropertySet);
        this.ocx.TemplateRepeat = value;
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [DispId(96)]
    public virtual int WantTabs
    {
      get
      {
        return this.ocx != null ? this.ocx.WantTabs : throw new AxHost.InvalidActiveXStateException(nameof (WantTabs), AxHost.ActiveXInvokeKind.PropertyGet);
      }
      set
      {
        if (this.ocx == null)
          throw new AxHost.InvalidActiveXStateException(nameof (WantTabs), AxHost.ActiveXInvokeKind.PropertySet);
        this.ocx.WantTabs = value;
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [DispId(106)]
    public virtual int RulerBackColor
    {
      get
      {
        return this.ocx != null ? this.ocx.RulerBackColor : throw new AxHost.InvalidActiveXStateException(nameof (RulerBackColor), AxHost.ActiveXInvokeKind.PropertyGet);
      }
      set
      {
        if (this.ocx == null)
          throw new AxHost.InvalidActiveXStateException(nameof (RulerBackColor), AxHost.ActiveXInvokeKind.PropertySet);
        this.ocx.RulerBackColor = value;
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [DispId(301)]
    public virtual acRefreshEventReasons RefreshEventReason
    {
      get
      {
        return this.ocx != null ? this.ocx.RefreshEventReason : throw new AxHost.InvalidActiveXStateException(nameof (RefreshEventReason), AxHost.ActiveXInvokeKind.PropertyGet);
      }
    }

    [DispId(380)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual string OptionalRegistryKeyForSavingSettings
    {
      get
      {
        return this.ocx != null ? this.ocx.OptionalRegistryKeyForSavingSettings : throw new AxHost.InvalidActiveXStateException(nameof (OptionalRegistryKeyForSavingSettings), AxHost.ActiveXInvokeKind.PropertyGet);
      }
      set
      {
        if (this.ocx == null)
          throw new AxHost.InvalidActiveXStateException(nameof (OptionalRegistryKeyForSavingSettings), AxHost.ActiveXInvokeKind.PropertySet);
        this.ocx.OptionalRegistryKeyForSavingSettings = value;
      }
    }

    public event _IPDFCreactiveXEvents_BeforeDeleteEventHandler BeforeDelete;

    public event _IPDFCreactiveXEvents_PrintPageEventHandler PrintPageEvent;

    public event _IPDFCreactiveXEvents_SavePageEventHandler SavePageEvent;

    public event _IPDFCreactiveXEvents_ClickHyperlinkEventHandler ClickHyperlink;

    public event EventHandler RefreshEvent;

    public event EventHandler SelectedObjectChange;

    public event _IPDFCreactiveXEvents_ObjectTextChangeEventHandler ObjectTextChange;

    public event _IPDFCreactiveXEvents_ContextSensitiveMenuEventHandler ContextSensitiveMenu;

    public event _IPDFCreactiveXEvents_MouseDownEventHandler MouseDownEvent;

    public event _IPDFCreactiveXEvents_MouseMoveEventHandler MouseMoveEvent;

    public event _IPDFCreactiveXEvents_MouseUpEventHandler MouseUpEvent;

    public event _IPDFCreactiveXEvents_NewObjectEventHandler NewObject;

    public event _IPDFCreactiveXEvents_ActivateObjectEventHandler ActivateObjectEvent;

    public event _IPDFCreactiveXEvents_LoadPageEventHandler LoadPage;

    public event _IPDFCreactiveXEvents_EvaluateExpressionEventHandler EvaluateExpression;

    public event _IPDFCreactiveXEvents_ProcessingProgressEventHandler ProcessingProgress;

    public virtual void StartSave(string fileName, FileSaveOptionConstants saveOption)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (StartSave), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.StartSave(fileName, saveOption);
    }

    public virtual void SavePage(int pageNumber)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (SavePage), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.SavePage(pageNumber);
    }

    public virtual void EndSave()
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (EndSave), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.EndSave();
    }

    public virtual void ClearPage(int pageNumber)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (ClearPage), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.ClearPage(pageNumber);
    }

    public virtual void Encrypt128(string ownerPassword, string userPassword, int options)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (Encrypt128), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.Encrypt128(ownerPassword, userPassword, options);
    }

    public virtual int GetWarningLevel()
    {
      return this.ocx != null ? this.ocx.GetWarningLevel() : throw new AxHost.InvalidActiveXStateException(nameof (GetWarningLevel), AxHost.ActiveXInvokeKind.MethodInvoke);
    }

    public virtual void ProcessHyperlinksBookmarksFromIniFile(string iniFileName)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (ProcessHyperlinksBookmarksFromIniFile), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.ProcessHyperlinksBookmarksFromIniFile(iniFileName);
    }

    public virtual void AutoHyperlinks(string prefixes)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (AutoHyperlinks), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.AutoHyperlinks(prefixes);
    }

    public virtual void AutoBookmarks(int levels, string fonts, int startPage)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (AutoBookmarks), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.AutoBookmarks(levels, fonts, startPage);
    }

    public virtual void ExportToTiff(string fileName, int resolution, int tiffFormat)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (ExportToTiff), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.ExportToTiff(fileName, resolution, tiffFormat);
    }

    public virtual void RotatePage(int pageNumber, RotatePageConstants rotation)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (RotatePage), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.RotatePage(pageNumber, rotation);
    }

    public virtual void MovePages(int pageNumber, int pageCount, int destination)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (MovePages), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.MovePages(pageNumber, pageCount, destination);
    }

    public virtual void AddExternalObjectTemplate(IacExternalObject externalObject)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (AddExternalObjectTemplate), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.AddExternalObjectTemplate(externalObject);
    }

    public virtual void CreateObjectEx(
      ObjectTypeConstants objectType,
      string reference,
      int subType,
      int pageNumber)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (CreateObjectEx), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.CreateObjectEx(objectType, reference, subType, pageNumber);
    }

    public virtual object GetObjectsInRectangle(
      int left,
      int top,
      int right,
      int bottom,
      int flags)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (GetObjectsInRectangle), AxHost.ActiveXInvokeKind.MethodInvoke);
      return this.ocx.GetObjectsInRectangle(left, top, right, bottom, flags);
    }

    public virtual void SetTargetDevmode(int hDevMode)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (SetTargetDevmode), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.SetTargetDevmode(hDevMode);
    }

    public virtual void PrinterSetup()
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (PrinterSetup), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.PrinterSetup();
    }

    public virtual void DigitalSignature(
      string signerName,
      string reason,
      string imageFile,
      string location,
      int pageNumber,
      int horzPos,
      int vertPos,
      int width,
      int height,
      int flags)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (DigitalSignature), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.DigitalSignature(signerName, reason, imageFile, location, pageNumber, horzPos, vertPos, width, height, flags);
    }

    public virtual void GetFontAttributes(string fontName, out int fontDescent, out int fontAscent)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (GetFontAttributes), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.GetFontAttributes(fontName, out fontDescent, out fontAscent);
    }

    public virtual void SetAttributeForMultipleSelectionEx(
      string attribName,
      object attribVal,
      int canUndo)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (SetAttributeForMultipleSelectionEx), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.SetAttributeForMultipleSelectionEx(attribName, attribVal, canUndo);
    }

    public virtual void SetRefreshEventReason(
      acRefreshEventReasons newVal,
      int bAddToExistingReason)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (SetRefreshEventReason), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.SetRefreshEventReason(newVal, bAddToExistingReason);
    }

    public virtual void SetToolTipText(string tooltipText)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (SetToolTipText), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.SetToolTipText(tooltipText);
    }

    public virtual acPlugin NewPluginBaseObject()
    {
      return this.ocx != null ? this.ocx.NewPluginBaseObject() : throw new AxHost.InvalidActiveXStateException(nameof (NewPluginBaseObject), AxHost.ActiveXInvokeKind.MethodInvoke);
    }

    public virtual void GetPlugin(
      CommandToolConstants pluginID,
      int bOnlyIfLoaded,
      ref object ppUnknown)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (GetPlugin), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.GetPlugin(pluginID, bOnlyIfLoaded, ref ppUnknown);
    }

    public virtual void StartSaveEx(
      string fileName,
      FileSaveOptionConstants saveOption,
      out int lpHandle)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (StartSaveEx), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.StartSaveEx(fileName, saveOption, out lpHandle);
    }

    public virtual void SavePageEx(int handle, int pageNumber)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (SavePageEx), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.SavePageEx(handle, pageNumber);
    }

    public virtual void EndSaveEx(int handle)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (EndSaveEx), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.EndSaveEx(handle);
    }

    public virtual void DeleteAllHyperlinks()
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (DeleteAllHyperlinks), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.DeleteAllHyperlinks();
    }

    public virtual object GetPageText(int pageNumber)
    {
      return this.ocx != null ? this.ocx.GetPageText(pageNumber) : throw new AxHost.InvalidActiveXStateException(nameof (GetPageText), AxHost.ActiveXInvokeKind.MethodInvoke);
    }

    public virtual string GetRawPageText(int pageNumber)
    {
      return this.ocx != null ? this.ocx.GetRawPageText(pageNumber) : throw new AxHost.InvalidActiveXStateException(nameof (GetRawPageText), AxHost.ActiveXInvokeKind.MethodInvoke);
    }

    public virtual void ExportToHTMLEx(
      string fileName,
      acHtmlExportOptions options,
      string imageNamingInfo,
      int downsamplingResolution)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (ExportToHTMLEx), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.ExportToHTMLEx(fileName, options, imageNamingInfo, downsamplingResolution);
    }

    public virtual void AutoHyperlinksEx(string prefixes, int clearExistingHyperlinks)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (AutoHyperlinksEx), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.AutoHyperlinksEx(prefixes, clearExistingHyperlinks);
    }

    public virtual void GetScreenCoordinates(
      string objectName,
      out int left,
      out int top,
      out int right,
      out int bottom)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (GetScreenCoordinates), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.GetScreenCoordinates(objectName, out left, out top, out right, out bottom);
    }

    public virtual void AddPage(int pageIndex)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (AddPage), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.AddPage(pageIndex);
    }

    public virtual void Encrypt(string ownerPassword, string userPassword, int options)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (Encrypt), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.Encrypt(ownerPassword, userPassword, options);
    }

    public virtual void ReceiveDoc(int lPort, int lTimeout)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (ReceiveDoc), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.ReceiveDoc(lPort, lTimeout);
    }

    public virtual void SendDoc(string szAddress, int lPort, string szUsername)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (SendDoc), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.SendDoc(szAddress, lPort, szUsername);
    }

    public virtual string ReachTextEx(
      ReachTextOptionConstants option,
      string text,
      string fontName,
      short fontSize,
      int bold,
      int italic)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (ReachTextEx), AxHost.ActiveXInvokeKind.MethodInvoke);
      return this.ocx.ReachTextEx(option, text, fontName, fontSize, bold, italic);
    }

    public virtual void ObjectAttributeStr(string @object, string attribute, string newVal)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (ObjectAttributeStr), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.ObjectAttributeStr(@object, attribute, newVal);
    }

    public virtual void DataReceived(ref byte data, int length)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (DataReceived), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.DataReceived(ref data, length);
    }

    public virtual void PageReceived()
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (PageReceived), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.PageReceived();
    }

    public virtual void StartPrint(string printerName, int prompt)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (StartPrint), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.StartPrint(printerName, prompt);
    }

    public virtual void PrintPage(int pageNumber)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (PrintPage), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.PrintPage(pageNumber);
    }

    public virtual void EndPrint()
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (EndPrint), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.EndPrint();
    }

    public virtual acObject GetObjectXY(int x, int y)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (GetObjectXY), AxHost.ActiveXInvokeKind.MethodInvoke);
      return this.ocx.GetObjectXY(x, y);
    }

    public virtual void ExportToHTML(string fileName, acHtmlExportOptions options)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (ExportToHTML), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.ExportToHTML(fileName, options);
    }

    public virtual void ExportToRTF(string fileName, acRtfExportOptions options, int useTabs)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (ExportToRTF), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.ExportToRTF(fileName, options, useTabs);
    }

    public virtual void ExportToJPeg(string fileName, int resolution, int jPegLevel)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (ExportToJPeg), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.ExportToJPeg(fileName, resolution, jPegLevel);
    }

    public virtual void ExportToXPS(string fileName, acXPSExportOptions options)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (ExportToXPS), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.ExportToXPS(fileName, options);
    }

    public virtual void OptimizeDocument(int level)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (OptimizeDocument), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.OptimizeDocument(level);
    }

    public virtual void EmbedFont(string baseFont, acEmbedFontOptions option)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (EmbedFont), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.EmbedFont(baseFont, option);
    }

    public virtual void DrawCurrentPage(int hDC, int prepareDC)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (DrawCurrentPage), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.DrawCurrentPage(hDC, prepareDC);
    }

    public virtual void ActivateObject(string @object)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (ActivateObject), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.ActivateObject(@object);
    }

    public virtual int OpenEx(string fileName, string password)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (OpenEx), AxHost.ActiveXInvokeKind.MethodInvoke);
      return this.ocx.OpenEx(fileName, password);
    }

    public virtual void SetLicenseKey(string company, string licKey)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (SetLicenseKey), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.SetLicenseKey(company, licKey);
    }

    public virtual void ExportToExcel(string fileName, acExcelExportOptions options)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (ExportToExcel), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.ExportToExcel(fileName, options);
    }

    public virtual acObject GetObjectByName(string name)
    {
      return this.ocx != null ? this.ocx.GetObjectByName(name) : throw new AxHost.InvalidActiveXStateException(nameof (GetObjectByName), AxHost.ActiveXInvokeKind.MethodInvoke);
    }

    public virtual void SetAttributeForMultipleSelection(string attribName, object attribVal)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (SetAttributeForMultipleSelection), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.SetAttributeForMultipleSelection(attribName, attribVal);
    }

    public virtual void SelectAllObjects(int val)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (SelectAllObjects), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.SelectAllObjects(val);
    }

    public virtual void DuplicatePage(int pageIndex)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (DuplicatePage), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.DuplicatePage(pageIndex);
    }

    public virtual void DeletePage(int pageNumber, int canUndo)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (DeletePage), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.DeletePage(pageNumber, canUndo);
    }

    public virtual void LockAllObjects(int @lock)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (LockAllObjects), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.LockAllObjects(@lock);
    }

    public virtual void Append(string fileName, string password)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (Append), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.Append(fileName, password);
    }

    public virtual void Merge(string fileName, string password, int options)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (Merge), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.Merge(fileName, password, options);
    }

    public virtual void AppendEx(PDFCreactiveX document)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (AppendEx), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.AppendEx(document);
    }

    public virtual void MergeEx(PDFCreactiveX document, int options)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (MergeEx), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.MergeEx(document, options);
    }

    public virtual void SetPageNumbering(
      acPageNumbersPositions position,
      string font,
      int extraMarginHorz,
      int extraMarginVert,
      int color,
      int startWithPage,
      string format)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (SetPageNumbering), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.SetPageNumbering(position, font, extraMarginHorz, extraMarginVert, color, startWithPage, format);
    }

    public virtual void ScrollWindow(int horzScroll, int vertScroll)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (ScrollWindow), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.ScrollWindow(horzScroll, vertScroll);
    }

    public virtual void RedrawObject(string @object)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (RedrawObject), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.RedrawObject(@object);
    }

    public virtual void InitTable(int rowCount, int colCount)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (InitTable), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.InitTable(rowCount, colCount);
    }

    public new virtual void Refresh()
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (Refresh), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.Refresh();
    }

    public virtual void InsertObject(ObjectTypeConstants objectType)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (InsertObject), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.InsertObject(objectType);
    }

    public virtual int UndoLevels()
    {
      return this.ocx != null ? this.ocx.UndoLevels() : throw new AxHost.InvalidActiveXStateException(nameof (UndoLevels), AxHost.ActiveXInvokeKind.MethodInvoke);
    }

    public virtual int RedoLevels()
    {
      return this.ocx != null ? this.ocx.RedoLevels() : throw new AxHost.InvalidActiveXStateException(nameof (RedoLevels), AxHost.ActiveXInvokeKind.MethodInvoke);
    }

    public virtual void Undo()
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (Undo), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.Undo();
    }

    public virtual void Redo()
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (Redo), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.Redo();
    }

    public virtual void DeleteObject(string @object, int canUndo)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (DeleteObject), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.DeleteObject(@object, canUndo);
    }

    public virtual ObjectTypeConstants ActiveTool()
    {
      return this.ocx != null ? this.ocx.ActiveTool() : throw new AxHost.InvalidActiveXStateException(nameof (ActiveTool), AxHost.ActiveXInvokeKind.MethodInvoke);
    }

    public virtual void DoCommandTool(CommandToolConstants id)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (DoCommandTool), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.DoCommandTool(id);
    }

    public virtual CommandToolStatusConstants UpdateCommandTool(CommandToolConstants id)
    {
      return this.ocx != null ? this.ocx.UpdateCommandTool(id) : throw new AxHost.InvalidActiveXStateException(nameof (UpdateCommandTool), AxHost.ActiveXInvokeKind.MethodInvoke);
    }

    public virtual void RedrawRect(int left, int top, int right, int bottom)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (RedrawRect), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.RedrawRect(left, top, right, bottom);
    }

    public virtual void Save(string fileName, FileSaveOptionConstants saveOption)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (Save), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.Save(fileName, saveOption);
    }

    public virtual int Open(string fileName, string password)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (Open), AxHost.ActiveXInvokeKind.MethodInvoke);
      return this.ocx.Open(fileName, password);
    }

    public virtual void InitBlank()
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (InitBlank), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.InitBlank();
    }

    public virtual void InitReport()
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (InitReport), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.InitReport();
    }

    public virtual void CreateSection(short groups)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (CreateSection), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.CreateSection(groups);
    }

    public virtual void Print(string printerName, int prompt)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (Print), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.Print(printerName, prompt);
    }

    public virtual void ReachBookmark(string bookmark)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (ReachBookmark), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.ReachBookmark(bookmark);
    }

    public virtual int ReachText(
      ReachTextOptionConstants option,
      string text,
      string fontName,
      short fontSize,
      int bold,
      int italic)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (ReachText), AxHost.ActiveXInvokeKind.MethodInvoke);
      return this.ocx.ReachText(option, text, fontName, fontSize, bold, italic);
    }

    public virtual void CreateObject(ObjectTypeConstants objectType, string reference)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (CreateObject), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.CreateObject(objectType, reference);
    }

    public virtual object get_ObjectAttribute(string @object, string attribute)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (get_ObjectAttribute), AxHost.ActiveXInvokeKind.MethodInvoke);
      return this.ocx.get_ObjectAttribute(@object, attribute);
    }

    public virtual void set_ObjectAttribute(string @object, string attribute, object pVal)
    {
      if (this.ocx == null)
        throw new AxHost.InvalidActiveXStateException(nameof (set_ObjectAttribute), AxHost.ActiveXInvokeKind.MethodInvoke);
      this.ocx.set_ObjectAttribute(@object, attribute, pVal);
    }

    protected override void CreateSink()
    {
      try
      {
        this.eventMulticaster = new AxPDFCreactiveXEventMulticaster(this);
        this.cookie = new AxHost.ConnectionPointCookie((object) this.ocx, (object) this.eventMulticaster, typeof (_IPDFCreactiveXEvents));
      }
      catch (Exception ex)
      {
      }
    }

    protected override void DetachSink()
    {
      try
      {
        this.cookie.Disconnect();
      }
      catch (Exception ex)
      {
      }
    }

    protected override void AttachInterfaces()
    {
      try
      {
        this.ocx = (IPDFCreactiveX) this.GetOcx();
      }
      catch (Exception ex)
      {
      }
    }

    internal void RaiseOnBeforeDelete(object sender, _IPDFCreactiveXEvents_BeforeDeleteEvent e)
    {
      if (this.BeforeDelete == null)
        return;
      this.BeforeDelete(sender, e);
    }

    internal void RaiseOnPrintPageEvent(object sender, _IPDFCreactiveXEvents_PrintPageEvent e)
    {
      if (this.PrintPageEvent == null)
        return;
      this.PrintPageEvent(sender, e);
    }

    internal void RaiseOnSavePageEvent(object sender, _IPDFCreactiveXEvents_SavePageEvent e)
    {
      if (this.SavePageEvent == null)
        return;
      this.SavePageEvent(sender, e);
    }

    internal void RaiseOnClickHyperlink(object sender, _IPDFCreactiveXEvents_ClickHyperlinkEvent e)
    {
      if (this.ClickHyperlink == null)
        return;
      this.ClickHyperlink(sender, e);
    }

    internal void RaiseOnRefreshEvent(object sender, EventArgs e)
    {
      if (this.RefreshEvent == null)
        return;
      this.RefreshEvent(sender, e);
    }

    internal void RaiseOnSelectedObjectChange(object sender, EventArgs e)
    {
      if (this.SelectedObjectChange == null)
        return;
      this.SelectedObjectChange(sender, e);
    }

    internal void RaiseOnObjectTextChange(
      object sender,
      _IPDFCreactiveXEvents_ObjectTextChangeEvent e)
    {
      if (this.ObjectTextChange == null)
        return;
      this.ObjectTextChange(sender, e);
    }

    internal void RaiseOnContextSensitiveMenu(
      object sender,
      _IPDFCreactiveXEvents_ContextSensitiveMenuEvent e)
    {
      if (this.ContextSensitiveMenu == null)
        return;
      this.ContextSensitiveMenu(sender, e);
    }

    internal void RaiseOnMouseDownEvent(object sender, _IPDFCreactiveXEvents_MouseDownEvent e)
    {
      if (this.MouseDownEvent == null)
        return;
      this.MouseDownEvent(sender, e);
    }

    internal void RaiseOnMouseMoveEvent(object sender, _IPDFCreactiveXEvents_MouseMoveEvent e)
    {
      if (this.MouseMoveEvent == null)
        return;
      this.MouseMoveEvent(sender, e);
    }

    internal void RaiseOnMouseUpEvent(object sender, _IPDFCreactiveXEvents_MouseUpEvent e)
    {
      if (this.MouseUpEvent == null)
        return;
      this.MouseUpEvent(sender, e);
    }

    internal void RaiseOnNewObject(object sender, _IPDFCreactiveXEvents_NewObjectEvent e)
    {
      if (this.NewObject == null)
        return;
      this.NewObject(sender, e);
    }

    internal void RaiseOnActivateObjectEvent(
      object sender,
      _IPDFCreactiveXEvents_ActivateObjectEvent e)
    {
      if (this.ActivateObjectEvent == null)
        return;
      this.ActivateObjectEvent(sender, e);
    }

    internal void RaiseOnLoadPage(object sender, _IPDFCreactiveXEvents_LoadPageEvent e)
    {
      if (this.LoadPage == null)
        return;
      this.LoadPage(sender, e);
    }

    internal void RaiseOnEvaluateExpression(
      object sender,
      _IPDFCreactiveXEvents_EvaluateExpressionEvent e)
    {
      if (this.EvaluateExpression == null)
        return;
      this.EvaluateExpression(sender, e);
    }

    internal void RaiseOnProcessingProgress(
      object sender,
      _IPDFCreactiveXEvents_ProcessingProgressEvent e)
    {
      if (this.ProcessingProgress == null)
        return;
      this.ProcessingProgress(sender, e);
    }
  }
}
