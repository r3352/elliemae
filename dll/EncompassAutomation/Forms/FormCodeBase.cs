// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.FormCodeBase
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using mshtml;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  internal class FormCodeBase
  {
    private Form parentForm;
    private IHTMLElement codeElement;

    internal FormCodeBase(Form parentForm)
    {
      this.parentForm = parentForm;
      this.attachToCodebaseElement();
    }

    private void attachToCodebaseElement()
    {
      IHTMLElementCollection elementsByTagName = ((DispHTMLDocument) this.parentForm.GetHTMLDocument()).getElementsByTagName("EMCODEBASE");
      if (elementsByTagName.length == 0)
        return;
      this.codeElement = (IHTMLElement) elementsByTagName.item((object) null, (object) 0);
    }

    private void ensureAttached()
    {
      if (this.codeElement != null)
        return;
      this.codeElement = ((DispHTMLDocument) this.parentForm.GetHTMLDocument()).createElement("EMCODEBASE");
      ((IHTMLDOMNode) this.parentForm.HTMLElement).appendChild((IHTMLDOMNode) this.codeElement);
    }

    public CodeBase GetCodeBase()
    {
      try
      {
        return this.codeElement == null ? CodeBase.Empty : new CodeBase(string.Concat(this.codeElement.getAttribute("path", 1)), string.Concat(this.codeElement.getAttribute("assembly", 1)), string.Concat(this.codeElement.getAttribute("version", 1)), string.Concat(this.codeElement.getAttribute("typeName", 1)));
      }
      catch
      {
        return CodeBase.Empty;
      }
    }

    public void SetCodeBase(CodeBase codeBase)
    {
      if (codeBase != null && codeBase != CodeBase.Empty)
      {
        this.ensureAttached();
        this.codeElement.setAttribute("path", (object) codeBase.AssemblyPath, 1);
        this.codeElement.setAttribute("assembly", (object) codeBase.AssemblyName, 1);
        this.codeElement.setAttribute("version", (object) codeBase.AssemblyVersion, 1);
        this.codeElement.setAttribute("typeName", (object) codeBase.ClassName, 1);
      }
      else
      {
        if (this.codeElement == null)
          return;
        ((IHTMLDOMNode) this.parentForm.HTMLElement).removeChild((IHTMLDOMNode) this.codeElement);
        this.codeElement = (IHTMLElement) null;
      }
    }
  }
}
