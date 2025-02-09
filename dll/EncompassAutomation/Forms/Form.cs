// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Form
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.InputEngine;
using EllieMae.Encompass.Automation;
using EllieMae.Encompass.BusinessObjects.Loans;
using EllieMae.Encompass.Forms.Design;
using mshtml;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Reflection;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  [PropertyTab(typeof (EventsTab), PropertyTabScope.Component)]
  public class Form : ContainerControl, ISupportsLoadEvent, ISupportsEvents
  {
    private const string className = "Form";
    private static readonly string sw = Tracing.SwInputEngine;
    private static readonly string[] supportedEvents = new string[2]
    {
      "Load",
      "Unload"
    };
    private static readonly Hashtable controlConstructorCache = new Hashtable();
    private ScopedEventHandler<FormEventArgs> contextMenu = new ScopedEventHandler<FormEventArgs>(nameof (Form), "ContextMenu");
    private ScopedEventHandler<FormEventArgs> selectionChanged = new ScopedEventHandler<FormEventArgs>(nameof (Form), "SelectionChanged");
    private ScopedEventHandler<FormEventArgs> resize = new ScopedEventHandler<FormEventArgs>(nameof (Form), "Resize");
    private ScopedEventHandler<FormEventArgs> move = new ScopedEventHandler<FormEventArgs>(nameof (Form), "Move");
    private ScopedEventHandler<FormEventArgs> propertyChange = new ScopedEventHandler<FormEventArgs>(nameof (Form), "PropertyChange");
    private ScopedEventHandler<FormEventArgs> controlAdded = new ScopedEventHandler<FormEventArgs>(nameof (Form), "ControlAdded");
    private ScopedEventHandler<FormEventArgs> controlDeleted = new ScopedEventHandler<FormEventArgs>(nameof (Form), "ControlDeleted");
    private ScopedEventHandler<FormEventArgs> fieldBindingChanged = new ScopedEventHandler<FormEventArgs>(nameof (Form), "FieldBindingChanged");
    private ScopedEventHandler<ControlIDChangedEventArgs> controlIDChanged = new ScopedEventHandler<ControlIDChangedEventArgs>(nameof (Form), "ControlIDChanged");
    private ScopedEventHandler<EventCodeEditorArgs> eventEditor = new ScopedEventHandler<EventCodeEditorArgs>(nameof (Form), "EventEditor");
    private ScopedEventHandler<KeyEventArgs> keyPress = new ScopedEventHandler<KeyEventArgs>(nameof (Form), "KeyPress");
    private ScopedEventHandler<EventArgs> load = new ScopedEventHandler<EventArgs>(nameof (Form), "Load");
    private ScopedEventHandler<EventArgs> unload = new ScopedEventHandler<EventArgs>(nameof (Form), "Unload");
    private FormOptions options;
    private bool editingEnabled;
    private Color normalBgColor = Color.Empty;
    private IFormScreen formScreen;
    private string name;
    private ControlSelection selectedControls;
    private DocumentEvents eventHandler;
    private RolodexGroups rolodexGroups;
    private HTMLDocument baseDocument;
    private ControlEvents controlEvents;
    private FormCodeBase codeBase;
    private ControlCache controlCache;

    public event FormEventHandler ContextMenu
    {
      add
      {
        if (value == null)
          return;
        this.contextMenu.Add(new ScopedEventHandler<FormEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
      remove
      {
        if (value == null)
          return;
        this.contextMenu.Remove(new ScopedEventHandler<FormEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
    }

    public event FormEventHandler SelectionChanged
    {
      add
      {
        if (value == null)
          return;
        this.selectionChanged.Add(new ScopedEventHandler<FormEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
      remove
      {
        if (value == null)
          return;
        this.selectionChanged.Remove(new ScopedEventHandler<FormEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
    }

    public event FormEventHandler Resize
    {
      add
      {
        if (value == null)
          return;
        this.resize.Add(new ScopedEventHandler<FormEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
      remove
      {
        if (value == null)
          return;
        this.resize.Remove(new ScopedEventHandler<FormEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
    }

    public event FormEventHandler Move
    {
      add
      {
        if (value == null)
          return;
        this.move.Add(new ScopedEventHandler<FormEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
      remove
      {
        if (value == null)
          return;
        this.move.Remove(new ScopedEventHandler<FormEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
    }

    public event FormEventHandler PropertyChange
    {
      add
      {
        if (value == null)
          return;
        this.propertyChange.Add(new ScopedEventHandler<FormEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
      remove
      {
        if (value == null)
          return;
        this.propertyChange.Remove(new ScopedEventHandler<FormEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
    }

    public event FormEventHandler ControlAdded
    {
      add
      {
        if (value == null)
          return;
        this.controlAdded.Add(new ScopedEventHandler<FormEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
      remove
      {
        if (value == null)
          return;
        this.controlAdded.Remove(new ScopedEventHandler<FormEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
    }

    public event FormEventHandler ControlDeleted
    {
      add
      {
        if (value == null)
          return;
        this.controlDeleted.Add(new ScopedEventHandler<FormEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
      remove
      {
        if (value == null)
          return;
        this.controlDeleted.Remove(new ScopedEventHandler<FormEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
    }

    public event FormEventHandler FieldBindingChanged
    {
      add
      {
        if (value == null)
          return;
        this.fieldBindingChanged.Add(new ScopedEventHandler<FormEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
      remove
      {
        if (value == null)
          return;
        this.fieldBindingChanged.Remove(new ScopedEventHandler<FormEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
    }

    public event ControlIDChangedEventHandler ControlIDChanged
    {
      add
      {
        if (value == null)
          return;
        this.controlIDChanged.Add(new ScopedEventHandler<ControlIDChangedEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
      remove
      {
        if (value == null)
          return;
        this.controlIDChanged.Remove(new ScopedEventHandler<ControlIDChangedEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
    }

    public event EventEditorHandler EventEditor
    {
      add
      {
        if (value == null)
          return;
        this.eventEditor.Add(new ScopedEventHandler<EventCodeEditorArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
      remove
      {
        if (value == null)
          return;
        this.eventEditor.Remove(new ScopedEventHandler<EventCodeEditorArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
    }

    public event KeyEventHandler KeyPress
    {
      add
      {
        if (value == null)
          return;
        this.keyPress.Add(new ScopedEventHandler<KeyEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
      remove
      {
        if (value == null)
          return;
        this.keyPress.Remove(new ScopedEventHandler<KeyEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
    }

    public event EventHandler Load
    {
      add
      {
        if (value == null)
          return;
        this.load.Add(new ScopedEventHandler<EventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
      remove
      {
        if (value == null)
          return;
        this.load.Remove(new ScopedEventHandler<EventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
    }

    public event EventHandler Unload
    {
      add
      {
        if (value == null)
          return;
        this.unload.Add(new ScopedEventHandler<EventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
      remove
      {
        if (value == null)
          return;
        this.unload.Remove(new ScopedEventHandler<EventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
    }

    public Form() => this.controlCache = new ControlCache();

    public void AttachToDocument(HTMLDocument document, FormOptions options)
    {
      using (PerformanceMeter performanceMeter = PerformanceMeter.StartNew("Form.AttachToDocument: " + this.Name, 317, nameof (AttachToDocument), "D:\\ws\\24.3.0.0\\EmLite\\EncompassAutomation\\Forms\\Form.cs"))
      {
        this.options = options;
        if (!Control.IsControl(((DispHTMLDocument) document).body))
        {
          Control.SetControlIDForElement(((DispHTMLDocument) document).body, "Form1");
          Control.SetControlTypeForElement(((DispHTMLDocument) document).body, this.GetType());
        }
        this.AttachToElement(this, ((DispHTMLDocument) document).body);
        performanceMeter.AddCheckpoint("Attached Form to HTML body element", 336, nameof (AttachToDocument), "D:\\ws\\24.3.0.0\\EmLite\\EncompassAutomation\\Forms\\Form.cs");
        this.prepareControlTree();
        performanceMeter.AddCheckpoint("Control tree initialized", 341, nameof (AttachToDocument), "D:\\ws\\24.3.0.0\\EmLite\\EncompassAutomation\\Forms\\Form.cs");
      }
    }

    internal override void AttachToElement(Form form, IHTMLElement controlElement)
    {
      this.baseDocument = !(controlElement.tagName.ToLower() != "body") ? (HTMLDocument) controlElement.document : throw new ArgumentException("The specified control element must be an HTMLBodyElement object");
      base.AttachToElement(form, controlElement);
      if (this.eventHandler != null)
        this.eventHandler.Dispose();
      this.adjustStylesheetReference();
      if (this.IsOptionEnabled(FormOptions.ManageEvents))
        this.eventHandler = new DocumentEvents(this);
      this.selectedControls = new ControlSelection(this);
      this.rolodexGroups = new RolodexGroups(this);
      this.controlEvents = new ControlEvents(this);
      this.codeBase = new FormCodeBase(this);
    }

    internal override IHTMLElement FindContainerElement(IHTMLElement controlElement)
    {
      if (((DispHTMLDocument) this.baseDocument).forms.length != 1)
        throw new Exception("The HTML document must contain exactly one <form> element");
      IHTMLElement controlElement1 = (IHTMLElement) ((DispHTMLDocument) this.baseDocument).forms.item((object) null, (object) 0);
      IHTMLElement containerElement = base.FindContainerElement(controlElement1);
      return containerElement == controlElement ? controlElement1 : containerElement;
    }

    internal override IHTMLElement FindContentElement(IHTMLElement controlElement)
    {
      return controlElement;
    }

    public string ToHTML()
    {
      return RadioButton.PerformGroupNameReplacements(((DispHTMLDocument) this.HTMLElement.document).documentElement.outerHTML);
    }

    [Browsable(false)]
    public ControlSelection SelectedControls => this.selectedControls;

    [Browsable(false)]
    public override bool Visible
    {
      get => true;
      set => throw new InvalidOperationException("The form's Visible property cannot be modified");
    }

    [Browsable(false)]
    public override bool Enabled
    {
      get => base.Enabled;
      set => base.Enabled = value;
    }

    [Browsable(false)]
    public override Point Position
    {
      get => base.Position;
      set => throw new InvalidOperationException("The position of the Form cannot be modified");
    }

    [Browsable(false)]
    public override Size Size
    {
      get => base.Size;
      set => throw new InvalidOperationException("The size of the form cannot be modified");
    }

    [Browsable(false)]
    public override LayoutMethod Layout
    {
      get => LayoutMethod.Flow;
      set
      {
        if (value != LayoutMethod.Flow)
          throw new InvalidOperationException("Top-level document must have Flow layout");
      }
    }

    public override bool AllowCutCopyDelete => false;

    public override Color BorderColor
    {
      get => base.BorderColor;
      set
      {
        if (value.ToArgb() == Color.Black.ToArgb())
          this.HTMLElement.style.borderColor = "";
        else
          base.BorderColor = value;
      }
    }

    [Browsable(false)]
    public Point ScrollOffset
    {
      get => new Point(this.HTMLElement2.scrollLeft, this.HTMLElement2.scrollTop);
    }

    public Point VisibleToAbsolutePosition(Point clientPosition)
    {
      return new Point(clientPosition.X + this.HTMLElement2.scrollLeft, clientPosition.Y + this.HTMLElement2.scrollTop);
    }

    public Point AbsoluteToVisiblePosition(Point position)
    {
      return new Point(position.X - this.HTMLElement2.scrollLeft, position.Y - this.HTMLElement2.scrollTop);
    }

    [Category("Implementation")]
    [Editor(typeof (CodeLanguageEditor), typeof (UITypeEditor))]
    public ScriptLanguage EventLanguage
    {
      get => this.ControlEvents.Language;
      set => this.ControlEvents.Language = value;
    }

    [Category("Implementation")]
    [Editor(typeof (CodeBaseEditor), typeof (UITypeEditor))]
    public CodeBase CodeBase
    {
      get => this.codeBase.GetCodeBase();
      set
      {
        this.EnsureEditing();
        this.codeBase.SetCodeBase(value);
        this.NotifyPropertyChange();
      }
    }

    public override void Delete()
    {
      throw new InvalidOperationException("Cannot delete document control");
    }

    public override void MoveTo(Location location)
    {
    }

    public override void Select() => this.OnSelectionChanged(new FormEventArgs((Control) this));

    public virtual void CreateControls()
    {
    }

    public Control FindControl(string controlId)
    {
      using (PerformanceMeter.Current.BeginOperation("Form.FindControl"))
      {
        if (this.controlCache.Contains(controlId))
          return this.controlCache[controlId];
        IHTMLElement elementById = ((DispHTMLDocument) this.GetHTMLDocument()).getElementById(controlId);
        return elementById == null || !Control.IsControl(elementById) ? (Control) null : this.ElementToControl(elementById);
      }
    }

    public Control[] FindControlsByType(Type baseType)
    {
      ArrayList arrayList = new ArrayList();
      foreach (Control control in this.controlCache)
      {
        if (baseType.IsAssignableFrom(control.GetType()))
          arrayList.Add((object) control);
      }
      return (Control[]) arrayList.ToArray(typeof (Control));
    }

    public FieldControl[] GetAllFieldControls()
    {
      return (FieldControl[]) new ArrayList((ICollection) this.FindControlsByType(typeof (FieldControl))).ToArray(typeof (FieldControl));
    }

    public Control[] GetAllControls()
    {
      return this.Form.EditingEnabled ? this.FindControlsByType(typeof (Control)) : this.FindControlsByType(typeof (RuntimeControl));
    }

    public override void Refresh() => this.Refresh(false);

    public void Refresh(bool refreshAll) => this.formScreen.RefreshAllControls(refreshAll);

    public FieldControl[] GetFieldTabSequence()
    {
      FieldControl[] allFieldControls = this.GetAllFieldControls();
      Array.Sort((Array) allFieldControls, (IComparer) new TabIndexSortComparer());
      return allFieldControls;
    }

    public override ContainerControl GetContainerAtPoint(Point pt)
    {
      return base.GetContainerAtPoint(pt) ?? (ContainerControl) this;
    }

    internal override ContainerControl GetContainerAtPointEx(Point pt, IDictionary exclusionList)
    {
      ContainerControl containerAtPointEx = base.GetContainerAtPointEx(pt, exclusionList);
      if (containerAtPointEx != null)
        return containerAtPointEx;
      return !exclusionList.Contains((object) this.ControlID) ? (ContainerControl) this : (ContainerControl) null;
    }

    [Browsable(false)]
    public string[] SupportedEvents => Form.supportedEvents;

    public void InvokeLoad() => this.OnLoad(EventArgs.Empty);

    protected virtual void OnLoad(EventArgs e) => this.load((object) this, e);

    public void InvokeUnload() => this.OnUnload(EventArgs.Empty);

    protected virtual void OnUnload(EventArgs e) => this.unload((object) this, e);

    internal ControlCache ControlCache => this.controlCache;

    internal void OnControlAdded(Control ctrl)
    {
      this.controlAdded((object) this, new FormEventArgs(ctrl));
    }

    internal void OnControlDeleted(Control ctrl)
    {
      this.controlDeleted((object) this, new FormEventArgs(ctrl));
    }

    internal void OnControlIDChanged(Control ctrl, string priorControlId)
    {
      this.controlIDChanged((object) this, new ControlIDChangedEventArgs(ctrl, priorControlId));
    }

    internal string NewControlID(string prefix) => this.NewControlID(prefix, (IDictionary) null);

    internal string NewControlID(string prefix, IDictionary namesToExclude)
    {
      int num = 1;
      while (this.ControlExists(prefix + (object) num) || namesToExclude != null && namesToExclude.Contains((object) (prefix + (object) num)))
        ++num;
      return prefix + (object) num;
    }

    internal bool ControlExists(string controlId) => this.FindControl(controlId) != null;

    internal Control ElementToControl(IHTMLElement controlElement)
    {
      string controlIdForElement = Control.GetControlIDForElement(controlElement);
      if ((controlIdForElement ?? "") != "")
      {
        Control control = this.ControlCache[controlIdForElement];
        if (control != null)
          return control;
      }
      Type controlTypeForElement = Control.GetControlTypeForElement(controlElement);
      return controlTypeForElement == (Type) null ? (Control) null : this.ElementToControl(controlElement, controlTypeForElement);
    }

    private Control ElementToControl(IHTMLElement controlElement, Type controlType)
    {
      using (PerformanceMeter.Current.BeginOperation("Form.ElementToControl"))
      {
        ConstructorInfo constructor = (ConstructorInfo) Form.controlConstructorCache[(object) controlType];
        if (constructor == (ConstructorInfo) null)
        {
          constructor = controlType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, new Type[2]
          {
            typeof (Form),
            typeof (IHTMLElement)
          }, new ParameterModifier[0]);
          Form.controlConstructorCache.Add((object) controlType, (object) constructor);
        }
        return (Control) constructor.Invoke(new object[2]
        {
          (object) this,
          (object) controlElement
        });
      }
    }

    public Control FindControlForElement(IHTMLElement element)
    {
      while (element != null && !Control.IsControl(element))
        element = element.parentElement;
      return element == null ? (Control) null : this.ElementToControl(element);
    }

    [Browsable(false)]
    public bool EditingEnabled => this.editingEnabled;

    internal override void PrepareForDisplay()
    {
      base.PrepareForDisplay();
      if (!(this.HTMLElement.style.border == ""))
        return;
      this.HTMLElement.style.border = "2px inset";
    }

    public void StartEditing()
    {
      if (!this.IsOptionEnabled(FormOptions.AllowEditing))
        throw new InvalidOperationException("Editing is not supported in the current context");
      ((DispHTMLDocument) this.baseDocument).execCommand("2D-Position", false, (object) true);
      ((DispHTMLDocument) this.baseDocument).execCommand("MultipleSelection", false, (object) true);
      this.editingEnabled = true;
      this.OnStartEditing();
    }

    public void StopEditing()
    {
      this.OnStopEditing();
      if (!this.EditingEnabled)
        return;
      ((IHTMLElement3) this.HTMLElement).contentEditable = "false";
      this.editingEnabled = false;
    }

    internal HTMLDocument GetSafeBaseDocument() => this.baseDocument;

    public static Form Open(HTMLDocument baseDocument)
    {
      try
      {
        Form form = new Form();
        form.AttachToDocument(baseDocument, FormOptions.All);
        return form;
      }
      catch (Exception ex)
      {
        Console.WriteLine((object) ex);
        throw;
      }
    }

    public void Close()
    {
      if (this.eventHandler != null)
      {
        this.eventHandler.Dispose();
        this.eventHandler = (DocumentEvents) null;
      }
      if (this.formScreen != null)
      {
        this.formScreen.Dispose();
        this.formScreen = (IFormScreen) null;
      }
      this.controlEvents.Clear();
      foreach (Control control in this.ControlCache)
        control.Dispose();
      this.ControlCache.Clear();
      this.Dispose();
    }

    public void AttachToFormScreen(IFormScreen formScreen, string formName)
    {
      this.formScreen = formScreen;
      if (formName == null)
        return;
      this.name = formName;
    }

    public string Name => this.name;

    internal IFormScreen FormScreen => this.formScreen;

    [Browsable(false)]
    public ControlEvents ControlEvents => this.controlEvents;

    internal RolodexGroups RolodexGroups => this.rolodexGroups;

    internal override void ReattachToElement()
    {
      this.AttachToElement(this, ((DispHTMLDocument) this.baseDocument).body);
    }

    internal FormOptions FormOptions => this.options;

    internal bool IsOptionEnabled(FormOptions option) => (this.options & option) == option;

    [Browsable(false)]
    public Loan Loan => EncompassApplication.CurrentLoan;

    internal FieldDescriptor GetFieldDescriptor(string fieldId)
    {
      if ((fieldId ?? "") == "")
        return FieldDescriptor.Empty;
      return (!EncompassApplication.Started ? FieldDescriptors.StandardFields[fieldId] : EncompassApplication.Session.Loans.FieldDescriptors[fieldId]) ?? FieldDescriptor.CreateUndefined(fieldId);
    }

    internal void OnContextMenu(FormEventArgs e) => this.contextMenu((object) this, e);

    internal void OnSelectionChanged(FormEventArgs e) => this.selectionChanged((object) this, e);

    internal void OnResize(FormEventArgs e)
    {
      this.resize((object) this, e);
      this.OnPropertyChange(e);
    }

    internal void OnMove(FormEventArgs e)
    {
      this.move((object) this, e);
      this.OnPropertyChange(e);
    }

    internal void OnPropertyChange(FormEventArgs e) => this.propertyChange((object) this, e);

    internal void OnFieldBindingChanged(FormEventArgs e)
    {
      this.fieldBindingChanged((object) this, e);
    }

    public void OnEventEditor(EventCodeEditorArgs e) => this.eventEditor((object) this, e);

    internal void OnKeyPress(KeyEventArgs e) => this.keyPress((object) this, e);

    private void adjustStylesheetReference()
    {
    }

    private void prepareControlTree()
    {
      this.prepareControlsInContainer((ContainerControl) this);
      this.PrepareForDisplay();
    }

    private void prepareControlsInContainer(ContainerControl container)
    {
      foreach (Control control in container.Controls)
      {
        if (control is ContainerControl)
          this.prepareControlsInContainer(control as ContainerControl);
        control.PrepareForDisplay();
      }
    }

    private void rebuildRolodex()
    {
      Control[] byType = this.Controls.FindByType(typeof (TextBox));
      Hashtable hashtable = new Hashtable();
      foreach (TextBox textBox in byType)
      {
        if (textBox.Rolodex == null && textBox.Field.FieldID != "")
          hashtable[(object) textBox.Field.FieldID] = (object) textBox;
      }
      if (hashtable.Count == 0)
        return;
      foreach (RolodexGroup rolodexGroup in (IEnumerable) this.RolodexGroups)
      {
        foreach (RolodexField field in Enum.GetValues(typeof (RolodexField)))
        {
          if (rolodexGroup[field] != "")
          {
            TextBox textBox = (TextBox) hashtable[(object) rolodexGroup[field]];
            if (textBox != null)
            {
              textBox.Rolodex = rolodexGroup;
              textBox.RolodexField = field;
            }
          }
        }
      }
    }

    public virtual void ExecAction(string action) => this.formScreen.ExecAction(action);
  }
}
