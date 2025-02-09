// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Form
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

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
  /// <summary>Represents an Encompass input form.</summary>
  /// <remarks>
  /// The Form control provides the framework for every Encompass input form, whether it's a standard
  /// form provided by the Encompass application or a custom form created in the Input Form Builder.
  /// A Form is made up of multiple <see cref="T:EllieMae.Encompass.Forms.Control">Controls</see> which provide the visual appearance
  /// and behaviors of the form.
  /// </remarks>
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

    /// <summary>This event is meant for us within the Encompass application only.</summary>
    /// <exclude />
    public event FormEventHandler ContextMenu
    {
      add
      {
        if (value == null)
          return;
        this.contextMenu.Add(new ScopedEventHandler<FormEventArgs>.EventHandlerT(value.Invoke));
      }
      remove
      {
        if (value == null)
          return;
        this.contextMenu.Remove(new ScopedEventHandler<FormEventArgs>.EventHandlerT(value.Invoke));
      }
    }

    /// <summary>This event is meant for us within the Encompass application only.</summary>
    /// <exclude />
    public event FormEventHandler SelectionChanged
    {
      add
      {
        if (value == null)
          return;
        this.selectionChanged.Add(new ScopedEventHandler<FormEventArgs>.EventHandlerT(value.Invoke));
      }
      remove
      {
        if (value == null)
          return;
        this.selectionChanged.Remove(new ScopedEventHandler<FormEventArgs>.EventHandlerT(value.Invoke));
      }
    }

    /// <summary>This event is meant for us within the Encompass application only.</summary>
    /// <exclude />
    public event FormEventHandler Resize
    {
      add
      {
        if (value == null)
          return;
        this.resize.Add(new ScopedEventHandler<FormEventArgs>.EventHandlerT(value.Invoke));
      }
      remove
      {
        if (value == null)
          return;
        this.resize.Remove(new ScopedEventHandler<FormEventArgs>.EventHandlerT(value.Invoke));
      }
    }

    /// <summary>This event is meant for us within the Encompass application only.</summary>
    /// <exclude />
    public event FormEventHandler Move
    {
      add
      {
        if (value == null)
          return;
        this.move.Add(new ScopedEventHandler<FormEventArgs>.EventHandlerT(value.Invoke));
      }
      remove
      {
        if (value == null)
          return;
        this.move.Remove(new ScopedEventHandler<FormEventArgs>.EventHandlerT(value.Invoke));
      }
    }

    /// <summary>This event is meant for us within the Encompass application only.</summary>
    /// <exclude />
    public event FormEventHandler PropertyChange
    {
      add
      {
        if (value == null)
          return;
        this.propertyChange.Add(new ScopedEventHandler<FormEventArgs>.EventHandlerT(value.Invoke));
      }
      remove
      {
        if (value == null)
          return;
        this.propertyChange.Remove(new ScopedEventHandler<FormEventArgs>.EventHandlerT(value.Invoke));
      }
    }

    /// <summary>This event is meant for us within the Encompass application only.</summary>
    /// <exclude />
    public event FormEventHandler ControlAdded
    {
      add
      {
        if (value == null)
          return;
        this.controlAdded.Add(new ScopedEventHandler<FormEventArgs>.EventHandlerT(value.Invoke));
      }
      remove
      {
        if (value == null)
          return;
        this.controlAdded.Remove(new ScopedEventHandler<FormEventArgs>.EventHandlerT(value.Invoke));
      }
    }

    /// <summary>This event is meant for us within the Encompass application only.</summary>
    /// <exclude />
    public event FormEventHandler ControlDeleted
    {
      add
      {
        if (value == null)
          return;
        this.controlDeleted.Add(new ScopedEventHandler<FormEventArgs>.EventHandlerT(value.Invoke));
      }
      remove
      {
        if (value == null)
          return;
        this.controlDeleted.Remove(new ScopedEventHandler<FormEventArgs>.EventHandlerT(value.Invoke));
      }
    }

    /// <summary>This event is meant for us within the Encompass application only.</summary>
    /// <exclude />
    public event FormEventHandler FieldBindingChanged
    {
      add
      {
        if (value == null)
          return;
        this.fieldBindingChanged.Add(new ScopedEventHandler<FormEventArgs>.EventHandlerT(value.Invoke));
      }
      remove
      {
        if (value == null)
          return;
        this.fieldBindingChanged.Remove(new ScopedEventHandler<FormEventArgs>.EventHandlerT(value.Invoke));
      }
    }

    /// <summary>This event is meant for us within the Encompass application only.</summary>
    /// <exclude />
    public event ControlIDChangedEventHandler ControlIDChanged
    {
      add
      {
        if (value == null)
          return;
        this.controlIDChanged.Add(new ScopedEventHandler<ControlIDChangedEventArgs>.EventHandlerT(value.Invoke));
      }
      remove
      {
        if (value == null)
          return;
        this.controlIDChanged.Remove(new ScopedEventHandler<ControlIDChangedEventArgs>.EventHandlerT(value.Invoke));
      }
    }

    /// <summary>This event is meant for us within the Encompass application only.</summary>
    /// <exclude />
    public event EventEditorHandler EventEditor
    {
      add
      {
        if (value == null)
          return;
        this.eventEditor.Add(new ScopedEventHandler<EventCodeEditorArgs>.EventHandlerT(value.Invoke));
      }
      remove
      {
        if (value == null)
          return;
        this.eventEditor.Remove(new ScopedEventHandler<EventCodeEditorArgs>.EventHandlerT(value.Invoke));
      }
    }

    /// <summary>This event is meant for us within the Encompass application only.</summary>
    /// <exclude />
    public event KeyEventHandler KeyPress
    {
      add
      {
        if (value == null)
          return;
        this.keyPress.Add(new ScopedEventHandler<KeyEventArgs>.EventHandlerT(value.Invoke));
      }
      remove
      {
        if (value == null)
          return;
        this.keyPress.Remove(new ScopedEventHandler<KeyEventArgs>.EventHandlerT(value.Invoke));
      }
    }

    /// <summary>The Load event is fired when the form is intially loaded into the Loan Editor.</summary>
    /// <remarks>The Load event can be used to perform any processing that must occur when the form
    /// is first displayed to the user, e.g. hiding form elements or populating FieldControls which
    /// are not bound to specific Encompass fields.</remarks>
    public event EventHandler Load
    {
      add
      {
        if (value == null)
          return;
        this.load.Add(new ScopedEventHandler<EventArgs>.EventHandlerT(value.Invoke));
      }
      remove
      {
        if (value == null)
          return;
        this.load.Remove(new ScopedEventHandler<EventArgs>.EventHandlerT(value.Invoke));
      }
    }

    /// <summary>The Unload event is fired when the form is unloaded from the Loan Editor.</summary>
    /// <remarks>A form is unloaded whenever the user navigates to a new input form or closes the
    /// loan file. The Unload event occurs prior to the form being closed but cannot be used to
    /// prevent the form from being closed. Use this event to perform any cleanup that is necessary
    /// before the form is disposed.</remarks>
    public event EventHandler Unload
    {
      add
      {
        if (value == null)
          return;
        this.unload.Add(new ScopedEventHandler<EventArgs>.EventHandlerT(value.Invoke));
      }
      remove
      {
        if (value == null)
          return;
        this.unload.Remove(new ScopedEventHandler<EventArgs>.EventHandlerT(value.Invoke));
      }
    }

    /// <summary>Constructor for a Form.</summary>
    /// <remarks>You will never need to instantiate a Form object within your own code. Instead,
    /// you may need to derive from the Form class when creating a codebase assembly for your
    /// custom input form. For more information, see the Encompass Advanced API documentation.</remarks>
    public Form() => this.controlCache = new ControlCache();

    /// <summary>Attaches the Form control to an HTML document object.</summary>
    /// <param name="document">The document object to which to attach the form.</param>
    /// <param name="options">The optons to determine the behavior of the form object.</param>
    /// <remarks>This method is used by the Encompass application and is not intended for use
    /// by external code.</remarks>
    /// <exclude />
    public void AttachToDocument(HTMLDocument document, FormOptions options)
    {
      using (PerformanceMeter performanceMeter = PerformanceMeter.StartNew("Form.AttachToDocument: " + this.Name, 317, nameof (AttachToDocument), "D:\\ws\\24.3.0.0\\EmLite\\EncompassAutomation\\Forms\\Form.cs"))
      {
        this.options = options;
        if (!Control.IsControl(document.body))
        {
          Control.SetControlIDForElement(document.body, "Form1");
          Control.SetControlTypeForElement(document.body, this.GetType());
        }
        this.AttachToElement(this, document.body);
        performanceMeter.AddCheckpoint("Attached Form to HTML body element", 336, nameof (AttachToDocument), "D:\\ws\\24.3.0.0\\EmLite\\EncompassAutomation\\Forms\\Form.cs");
        this.prepareControlTree();
        performanceMeter.AddCheckpoint("Control tree initialized", 341, nameof (AttachToDocument), "D:\\ws\\24.3.0.0\\EmLite\\EncompassAutomation\\Forms\\Form.cs");
      }
    }

    internal override void AttachToElement(Form form, mshtml.IHTMLElement controlElement)
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

    internal override mshtml.IHTMLElement FindContainerElement(mshtml.IHTMLElement controlElement)
    {
      if (this.baseDocument.forms.length != 1)
        throw new Exception("The HTML document must contain exactly one <form> element");
      mshtml.IHTMLElement controlElement1 = (mshtml.IHTMLElement) this.baseDocument.forms.item(index: (object) 0);
      mshtml.IHTMLElement containerElement = base.FindContainerElement(controlElement1);
      return containerElement == controlElement ? controlElement1 : containerElement;
    }

    internal override mshtml.IHTMLElement FindContentElement(mshtml.IHTMLElement controlElement)
    {
      return controlElement;
    }

    /// <summary>
    /// Returns a string containing the HTML content of the form.
    /// </summary>
    /// <returns>A string containing the HTML of the form.</returns>
    /// <remarks>This method is intended for use within the Encompass Form Builder only.</remarks>
    /// <exclude />
    public string ToHTML()
    {
      return RadioButton.PerformGroupNameReplacements(((DispHTMLDocument) this.HTMLElement.document).documentElement.outerHTML);
    }

    /// <summary>Gets the collection of currently selected controls.</summary>
    /// <remarks>This property can only be used when the Form is being designed.</remarks>
    [Browsable(false)]
    public ControlSelection SelectedControls => this.selectedControls;

    /// <summary>Overrides the Visible property for the Form control.</summary>
    /// <remarks>A form's visible property cannot be modified, it will always be <c>true</c>.</remarks>
    [Browsable(false)]
    public override bool Visible
    {
      get => true;
      set => throw new InvalidOperationException("The form's Visible property cannot be modified");
    }

    /// <summary>Overrides the Enabled property for the Form control.</summary>
    [Browsable(false)]
    public override bool Enabled
    {
      get => base.Enabled;
      set => base.Enabled = value;
    }

    /// <summary>Gets or sets the position of the form.</summary>
    /// <remarks>This property cannot be modified.</remarks>
    [Browsable(false)]
    public override Point Position
    {
      get => base.Position;
      set => throw new InvalidOperationException("The position of the Form cannot be modified");
    }

    /// <summary>Gets or sets the size of the form.</summary>
    /// <remarks>This property cannot be modified.</remarks>
    [Browsable(false)]
    public override Size Size
    {
      get => base.Size;
      set => throw new InvalidOperationException("The size of the form cannot be modified");
    }

    /// <summary>
    /// Overrides the behavior of the Layout property for the form.
    /// </summary>
    /// <remarks>The layout for a form is always Flow and cannot be modified.</remarks>
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

    /// <summary>
    /// Indicates that the Form control cannot be cut, copied or deleted.
    /// </summary>
    public override bool AllowCutCopyDelete => false;

    /// <summary>Gets or sets the border color of the Form.</summary>
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

    /// <summary>
    /// Gets the offset of the origin of the form based on the current scroll position.
    /// </summary>
    [Browsable(false)]
    public Point ScrollOffset
    {
      get => new Point(this.HTMLElement2.scrollLeft, this.HTMLElement2.scrollTop);
    }

    /// <summary>
    /// Converts a point which in client coordinates to an absolute position on the form.
    /// </summary>
    /// <param name="clientPosition">The point to be converted, relative to the
    /// top-left visible corner of the form.</param>
    /// <returns>The corresponding absolute position on the form</returns>
    public Point VisibleToAbsolutePosition(Point clientPosition)
    {
      return new Point(clientPosition.X + this.HTMLElement2.scrollLeft, clientPosition.Y + this.HTMLElement2.scrollTop);
    }

    /// <summary>
    /// Converts a point from absolute form coordinates to visible client coordinates.
    /// </summary>
    /// <param name="position">The point to be converted, relative to the
    /// top-left corner of the form.</param>
    /// <returns>The corresponding position, relative to the top-left visible
    /// corner of the form, as determined by scrolling.</returns>
    public Point AbsoluteToVisiblePosition(Point position)
    {
      return new Point(position.X - this.HTMLElement2.scrollLeft, position.Y - this.HTMLElement2.scrollTop);
    }

    /// <summary>Gets or sets the language used for coding of events</summary>
    [Category("Implementation")]
    [Editor(typeof (CodeLanguageEditor), typeof (UITypeEditor))]
    public ScriptLanguage EventLanguage
    {
      get => this.ControlEvents.Language;
      set => this.ControlEvents.Language = value;
    }

    /// <summary>
    /// Gets or sets the class and assembly which act as the code base for this form.
    /// </summary>
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

    /// <summary>Overrides the Delete method for the Form control.</summary>
    /// <remarks>This method throws the InvalidOperationException.</remarks>
    public override void Delete()
    {
      throw new InvalidOperationException("Cannot delete document control");
    }

    /// <summary>Overrides the MoveTo method for the Form control.</summary>
    /// <param name="location">The location to which to move the control.</param>
    /// <remarks>This method has no effect on the control.</remarks>
    public override void MoveTo(Location location)
    {
    }

    /// <summary>Adds the Form control into the current selection set.</summary>
    /// <remarks>This method can only be used within the Encompass Form Builder.</remarks>
    public override void Select() => this.OnSelectionChanged(new FormEventArgs((Control) this));

    /// <summary>
    /// Overridable method intended to allow derived classes to create control instances.
    /// </summary>
    /// <remarks>When creating a derived Form class, use the CreateControls method to
    /// retrieve control instances and save them into instance variables in your derived class.
    /// For example, use the <see cref="M:EllieMae.Encompass.Forms.Form.FindControl(System.String)" /> method to retrieve a control based on
    /// its <see cref="P:EllieMae.Encompass.Forms.Control.ControlID" /> and cast it to the appropriate control type.</remarks>
    public virtual void CreateControls()
    {
    }

    /// <summary>Locates a control in the Form based on Control ID.</summary>
    /// <param name="controlId">The <see cref="P:EllieMae.Encompass.Forms.Control.ControlID" /> of the control to be found.</param>
    /// <returns>Returns the control with the specified ControlID, or <c>null</c> if no such control
    /// exists.</returns>
    public Control FindControl(string controlId)
    {
      using (PerformanceMeter.Current.BeginOperation("Form.FindControl"))
      {
        if (this.controlCache.Contains(controlId))
          return this.controlCache[controlId];
        mshtml.IHTMLElement elementById = this.GetHTMLDocument().getElementById(controlId);
        return elementById == null || !Control.IsControl(elementById) ? (Control) null : this.ElementToControl(elementById);
      }
    }

    /// <summary>
    /// Returns all controls that derive from a particular Type.
    /// </summary>
    /// <param name="baseType">The base control type.</param>
    /// <returns>Returns an array of all controls which derive from the specified
    /// Type.</returns>
    public Control[] FindControlsByType(System.Type baseType)
    {
      ArrayList arrayList = new ArrayList();
      foreach (Control control in this.controlCache)
      {
        if (baseType.IsAssignableFrom(control.GetType()))
          arrayList.Add((object) control);
      }
      return (Control[]) arrayList.ToArray(typeof (Control));
    }

    /// <summary>
    /// Returns all of the <see cref="T:EllieMae.Encompass.Forms.FieldControl">FieldControls</see> in the Form.
    /// </summary>
    /// <returns>Returns an array containing every control which derives from the
    /// <see cref="T:EllieMae.Encompass.Forms.FieldControl" /> base class.</returns>
    public FieldControl[] GetAllFieldControls()
    {
      return (FieldControl[]) new ArrayList((ICollection) this.FindControlsByType(typeof (FieldControl))).ToArray(typeof (FieldControl));
    }

    /// <summary>Returns a list of every control on the form.</summary>
    /// <returns>Returns an array containing every control on the Form.</returns>
    public Control[] GetAllControls()
    {
      return this.Form.EditingEnabled ? this.FindControlsByType(typeof (Control)) : this.FindControlsByType(typeof (RuntimeControl));
    }

    /// <summary>Forces the entire form's state to be refreshed</summary>
    public override void Refresh() => this.Refresh(false);

    /// <summary>Refreshes the controls on the forms.</summary>
    /// <param name="refreshAll">If <c>false</c>, only controls associated with fields whose
    /// values have changed will be refreshed. Otherwise, all fields are refreshed.</param>
    public void Refresh(bool refreshAll) => this.formScreen.RefreshAllControls(refreshAll);

    /// <summary>
    /// Returns the list of field controls in tab-order sequence.
    /// </summary>
    /// <returns>The returned array contains all of the field controls sorted by
    /// their <see cref="P:EllieMae.Encompass.Forms.FieldControl.TabIndex" /> values.</returns>
    public FieldControl[] GetFieldTabSequence()
    {
      FieldControl[] allFieldControls = this.GetAllFieldControls();
      Array.Sort((Array) allFieldControls, (IComparer) new TabIndexSortComparer());
      return allFieldControls;
    }

    /// <summary>
    /// Returns the most-nested <see cref="T:EllieMae.Encompass.Forms.ContainerControl" /> at the specified point.
    /// </summary>
    /// <param name="pt">The point to test at.</param>
    /// <returns>Returns the ContainerControl lowest in the control hierarchy
    /// which is at the specified point. This method always returns a value since the
    /// Form control itself is a ContainerControl and will be returned if no other ContainerControl
    /// is found.</returns>
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

    /// <summary>Gets the list of events supported by the Form class.</summary>
    [Browsable(false)]
    public string[] SupportedEvents => Form.supportedEvents;

    /// <summary>Invokes the Load event on the Form.</summary>
    /// <remarks>This method is intended for internal use within Encompass only.</remarks>
    /// <exclude />
    public void InvokeLoad() => this.OnLoad(EventArgs.Empty);

    /// <summary>Raises the Load event.</summary>
    /// <param name="e">The event arguments passed to the event handlers.</param>
    protected virtual void OnLoad(EventArgs e) => this.load.Invoke((object) this, e);

    /// <summary>Invokes the Unload event on the Form.</summary>
    /// <remarks>This method is intended for internal use within Encompass only.</remarks>
    /// <exclude />
    public void InvokeUnload() => this.OnUnload(EventArgs.Empty);

    /// <summary>Raises the Unload event.</summary>
    /// <param name="e">The event arguments passed to the event handlers.</param>
    protected virtual void OnUnload(EventArgs e) => this.unload.Invoke((object) this, e);

    internal ControlCache ControlCache => this.controlCache;

    internal void OnControlAdded(Control ctrl)
    {
      this.controlAdded.Invoke((object) this, new FormEventArgs(ctrl));
    }

    internal void OnControlDeleted(Control ctrl)
    {
      this.controlDeleted.Invoke((object) this, new FormEventArgs(ctrl));
    }

    internal void OnControlIDChanged(Control ctrl, string priorControlId)
    {
      this.controlIDChanged.Invoke((object) this, new ControlIDChangedEventArgs(ctrl, priorControlId));
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

    /// <summary>
    /// Converts a HTML element to a control based on its properties.
    /// </summary>
    /// <param name="controlElement"></param>
    /// <returns></returns>
    internal Control ElementToControl(mshtml.IHTMLElement controlElement)
    {
      string controlIdForElement = Control.GetControlIDForElement(controlElement);
      if ((controlIdForElement ?? "") != "")
      {
        Control control = this.ControlCache[controlIdForElement];
        if (control != null)
          return control;
      }
      System.Type controlTypeForElement = Control.GetControlTypeForElement(controlElement);
      return controlTypeForElement == (System.Type) null ? (Control) null : this.ElementToControl(controlElement, controlTypeForElement);
    }

    /// <summary>
    /// Converts an HTML element to a specific type of control.
    /// </summary>
    /// <param name="controlElement"></param>
    /// <param name="controlType"></param>
    /// <returns></returns>
    private Control ElementToControl(mshtml.IHTMLElement controlElement, System.Type controlType)
    {
      using (PerformanceMeter.Current.BeginOperation("Form.ElementToControl"))
      {
        ConstructorInfo constructor = (ConstructorInfo) Form.controlConstructorCache[(object) controlType];
        if (constructor == (ConstructorInfo) null)
        {
          constructor = controlType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, new System.Type[2]
          {
            typeof (Form),
            typeof (mshtml.IHTMLElement)
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

    /// <summary>Finds the control in which an HTML element exists.</summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public Control FindControlForElement(mshtml.IHTMLElement element)
    {
      while (element != null && !Control.IsControl(element))
        element = element.parentElement;
      return element == null ? (Control) null : this.ElementToControl(element);
    }

    /// <summary>Indicates if the form is currently in edit mode.</summary>
    [Browsable(false)]
    public bool EditingEnabled => this.editingEnabled;

    internal override void PrepareForDisplay()
    {
      base.PrepareForDisplay();
      if (!(this.HTMLElement.style.border == ""))
        return;
      this.HTMLElement.style.border = "2px inset";
    }

    /// <summary>Places the form in editing mode.</summary>
    /// <remarks>This method can only be invoked from within the Encompass Form Builder.</remarks>
    /// <exclude />
    public void StartEditing()
    {
      if (!this.IsOptionEnabled(FormOptions.AllowEditing))
        throw new InvalidOperationException("Editing is not supported in the current context");
      this.baseDocument.execCommand("2D-Position", value: (object) true);
      this.baseDocument.execCommand("MultipleSelection", value: (object) true);
      this.editingEnabled = true;
      this.OnStartEditing();
    }

    /// <summary>Removes the form from editing mode.</summary>
    /// <remarks>This method can only be invoked from within the Encompass Form Builder.</remarks>
    /// <exclude />
    public void StopEditing()
    {
      this.OnStopEditing();
      if (!this.EditingEnabled)
        return;
      ((mshtml.IHTMLElement3) this.HTMLElement).contentEditable = "false";
      this.editingEnabled = false;
    }

    internal HTMLDocument GetSafeBaseDocument() => this.baseDocument;

    /// <summary>Opens an Form based on an underlying HTMLDocument.</summary>
    /// <param name="baseDocument">The document which backs the Form.</param>
    /// <returns></returns>
    /// <exclude />
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

    /// <summary>Closes the Form, releasing all of its resources.</summary>
    /// <remarks>This method is intended for use by the Encompass application only. Closing the
    /// form manually can cause errors or unexpected behavior.</remarks>
    /// <exclude />
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

    /// <summary>
    /// Attaches the object to the underlying form engine. This method is intended for
    /// internal use by Encompass only.
    /// </summary>
    /// <exclude />
    public void AttachToFormScreen(IFormScreen formScreen, string formName)
    {
      this.formScreen = formScreen;
      if (formName == null)
        return;
      this.name = formName;
    }

    /// <summary>Gets the name of the input form.</summary>
    public string Name => this.name;

    internal IFormScreen FormScreen => this.formScreen;

    /// <summary>Returns the set of Control Event for the form.</summary>
    [Browsable(false)]
    public ControlEvents ControlEvents => this.controlEvents;

    internal RolodexGroups RolodexGroups => this.rolodexGroups;

    internal override void ReattachToElement()
    {
      this.AttachToElement(this, this.baseDocument.body);
    }

    internal FormOptions FormOptions => this.options;

    internal bool IsOptionEnabled(FormOptions option) => (this.options & option) == option;

    /// <summary>
    /// Retrieves the Loan which is currently loaded into the page.
    /// </summary>
    /// <remarks>This property is only valid when the form is loaded in
    /// Encompass during runtime.</remarks>
    [Browsable(false)]
    public Loan Loan => EncompassApplication.CurrentLoan;

    internal FieldDescriptor GetFieldDescriptor(string fieldId)
    {
      if ((fieldId ?? "") == "")
        return FieldDescriptor.Empty;
      return (!EncompassApplication.Started ? FieldDescriptors.StandardFields[fieldId] : EncompassApplication.Session.Loans.FieldDescriptors[fieldId]) ?? FieldDescriptor.CreateUndefined(fieldId);
    }

    internal void OnContextMenu(FormEventArgs e) => this.contextMenu.Invoke((object) this, e);

    internal void OnSelectionChanged(FormEventArgs e)
    {
      this.selectionChanged.Invoke((object) this, e);
    }

    internal void OnResize(FormEventArgs e)
    {
      this.resize.Invoke((object) this, e);
      this.OnPropertyChange(e);
    }

    internal void OnMove(FormEventArgs e)
    {
      this.move.Invoke((object) this, e);
      this.OnPropertyChange(e);
    }

    internal void OnPropertyChange(FormEventArgs e) => this.propertyChange.Invoke((object) this, e);

    internal void OnFieldBindingChanged(FormEventArgs e)
    {
      this.fieldBindingChanged.Invoke((object) this, e);
    }

    /// <summary>Raises the EventEditor event.</summary>
    /// <param name="e">The event parameters to be passed to the event handlers</param>
    /// <exclude />
    public void OnEventEditor(EventCodeEditorArgs e) => this.eventEditor.Invoke((object) this, e);

    /// <summary>Raises the EventEditor event.</summary>
    /// <param name="e">The event parameters to be passed to the event handlers</param>
    /// <exclude />
    internal void OnKeyPress(KeyEventArgs e) => this.keyPress.Invoke((object) this, e);

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

    /// <summary>
    /// Executes a standard action in the Encompass loan editor.
    /// </summary>
    /// <param name="action">The action to be performed</param>
    public virtual void ExecAction(string action) => this.formScreen.ExecAction(action);
  }
}
