// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.MultilineTextBox
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using EllieMae.Encompass.ComponentModel;
using mshtml;
using System;
using System.ComponentModel;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  /// <summary>
  /// Represents a text box capable of supporting multiple lines of text input by the user.
  /// </summary>
  [ToolboxControl("Multi-line Text Box")]
  public class MultilineTextBox : TextBox
  {
    /// <summary>Constructor for a new MultilineTextBox control.</summary>
    public MultilineTextBox()
    {
    }

    internal MultilineTextBox(Form form, IHTMLElement controlElement)
      : base(form, controlElement)
    {
    }

    /// <summary>Gets/Sets the value of the field</summary>
    [Browsable(false)]
    public override string Text
    {
      get => ((IHTMLTextAreaElement) this.FieldElement).value ?? "";
      set => this.ApplyValueToControl(value);
    }

    /// <summary>Gets/Sets the maximum length of the text in the field</summary>
    [Browsable(false)]
    public override int MaxLength
    {
      get => -1;
      set => throw new NotSupportedException();
    }

    /// <summary>
    /// Indciates whether this control type supports the MaxLength property.
    /// </summary>
    /// <remarks>This property always returns False for the MultilineTextBox.</remarks>
    public override bool SupportsMaxLength => false;

    internal override bool ReadOnly
    {
      get => ((IHTMLTextAreaElement) this.HTMLElement).readOnly;
      set => ((IHTMLTextAreaElement) this.HTMLElement).readOnly = value;
    }

    internal override void DisplayValue(string value)
    {
      ((IHTMLTextAreaElement) this.FieldElement).value = value;
    }

    internal override string RenderHTML()
    {
      return "<textarea" + this.GetBaseAttributes() + this.GetBaseFieldAttributes() + " style=\"width: 100px; height: 40px\" wrap></textarea>";
    }
  }
}
