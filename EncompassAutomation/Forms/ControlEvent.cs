// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.ControlEvent
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using mshtml;
using System;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  /// <summary>
  /// Represents the definition of an event handler for a control's event.
  /// </summary>
  /// <remarks>This class is used by the Encompass Form Builder and intended for use
  /// at design time only. Attempts to modify this object properties at runtime will result in
  /// exceptions or unexpected behavior.</remarks>
  public class ControlEvent : IComparable
  {
    internal static ControlEvent Empty = new ControlEvent();
    private ControlEvents parentEvents;
    private IHTMLElement eventElement;

    internal ControlEvent(ControlEvents parentEvents, IHTMLElement eventElement)
    {
      this.parentEvents = parentEvents;
      this.eventElement = eventElement;
      if (this.parentEvents == null || this.eventElement == null)
        throw new ArgumentNullException();
    }

    private ControlEvent()
    {
    }

    /// <summary>
    /// The ControlID of the <see cref="T:EllieMae.Encompass.Forms.Control" /> object with which the event is associated.
    /// </summary>
    public string ControlID
    {
      get
      {
        return this.eventElement == null ? string.Empty : this.eventElement.getAttribute("for", 1).ToString() + string.Empty;
      }
      set
      {
        this.parentEvents.ParentForm.EnsureEditing();
        try
        {
          this.eventElement.setAttribute("for", (object) value);
        }
        catch (Exception ex)
        {
        }
      }
    }

    /// <summary>
    /// The name of event for which this object contains the definition.
    /// </summary>
    public string EventType
    {
      get
      {
        return this.eventElement == null ? string.Empty : this.eventElement.getAttribute("event", 1).ToString() + string.Empty;
      }
      set
      {
        this.parentEvents.ParentForm.EnsureEditing();
        try
        {
          this.eventElement.setAttribute("event", (object) value);
        }
        catch (Exception ex)
        {
        }
      }
    }

    /// <summary>Gets the language for the event handler.</summary>
    public ScriptLanguage Language => this.parentEvents.Language;

    /// <summary>Gets the source code for the event handler.</summary>
    public string EventCode
    {
      get
      {
        if (!(((IHTMLDOMNode) this.EventElement).firstChild is IHTMLCommentElement firstChild))
          return "";
        try
        {
          return firstChild.text.Substring(5 + Environment.NewLine.Length, firstChild.text.Length - 9 - 2 * Environment.NewLine.Length);
        }
        catch
        {
          return "";
        }
      }
      set
      {
        this.parentEvents.ParentForm.EnsureEditing();
        IHTMLDOMNode newChild = ((IHTMLDOMNode) this.EventElement).firstChild;
        if (newChild == null)
        {
          newChild = ((DispHTMLDocument) this.eventElement.document).createComment("");
          ((IHTMLDOMNode) this.EventElement).appendChild(newChild);
        }
        ((IHTMLCommentElement) newChild).text = "<!-- " + Environment.NewLine + value + Environment.NewLine + " -->";
      }
    }

    /// <summary>Deletes the event handler from the Form.</summary>
    public void Delete()
    {
      this.parentEvents.ParentForm.EnsureEditing();
      this.parentEvents.Remove(this.ControlID, this.EventType);
    }

    /// <summary>Compares two ControlEvent objects for equality.</summary>
    /// <param name="obj">The object to which to compare the current object.</param>
    /// <returns>Returns <c>true</c> if the two objects are equivalent, <c>false</c> otherwise.</returns>
    public override bool Equals(object obj)
    {
      ControlEvent controlEvent = obj as ControlEvent;
      return !object.Equals(obj, (object) null) && this.ControlID == controlEvent.ControlID && this.EventType == controlEvent.EventType;
    }

    internal IHTMLElement EventElement => this.eventElement;

    /// <summary>Provides a has code for the class.</summary>
    /// <returns>A integral has code for the object.</returns>
    public override int GetHashCode()
    {
      return this.ControlID.GetHashCode() ^ this.EventType.GetHashCode();
    }

    /// <summary>Provides a string representation of the event.</summary>
    /// <returns>Returns a string indicating if the event is populated.</returns>
    public override string ToString() => this.EventCode == "" ? "" : "(Click to Edit)";

    /// <summary>Provides a comparison operation for the ControlEvent.</summary>
    /// <param name="obj">The ControlEvent to which to compare the current event.</param>
    /// <returns>Compares the objects based first on the control ID and then on the event
    /// name, sorting alphabetically in both cases.</returns>
    public int CompareTo(object obj)
    {
      if (!(obj is ControlEvent controlEvent))
        throw new InvalidOperationException("Cannot sort against specified type");
      return controlEvent.ControlID != this.ControlID ? string.Compare(this.ControlID, controlEvent.ControlID, true) : string.Compare(this.EventType, controlEvent.EventType, true);
    }
  }
}
