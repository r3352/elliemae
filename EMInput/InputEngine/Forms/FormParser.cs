// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.Forms.FormParser
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Compiler;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.InputEngine.Forms
{
  public class FormParser
  {
    private string html;
    private XmlDocument xhtml = new XmlDocument();

    public FormParser(FormDescriptor descriptor)
      : this(descriptor.FormHTML)
    {
    }

    public FormParser(string html)
    {
      this.html = html;
      this.xhtml.LoadXml(Html2XHtml.Convert(this.html));
    }

    public FormParser(FileInfo fileInfo)
    {
      using (BinaryObject binaryObject = new BinaryObject(fileInfo.FullName))
      {
        this.html = binaryObject.ToString(Encoding.ASCII);
        this.xhtml.LoadXml(Html2XHtml.Convert(this.html));
      }
    }

    public string ToHTML() => this.html;

    public string ToXHTML() => this.xhtml.OuterXml;

    public bool IsEMFRMFormat() => this.xhtml.SelectNodes("//*[@controlType]").Count > 0;

    public CodeBase GetCodeBase()
    {
      try
      {
        XmlElement xmlElement = (XmlElement) this.xhtml.SelectSingleNode("//EMCODEBASE");
        return xmlElement == null ? (CodeBase) null : new CodeBase(xmlElement.GetAttribute("path") ?? "", xmlElement.GetAttribute("assembly") ?? "", xmlElement.GetAttribute("version") ?? "", xmlElement.GetAttribute("typeName") ?? "");
      }
      catch
      {
        return (CodeBase) null;
      }
    }

    public string[] GetReferencedFields()
    {
      XmlNodeList xmlNodeList = this.xhtml.SelectNodes("//*[@fieldId]");
      ArrayList arrayList = new ArrayList();
      foreach (XmlElement parent in xmlNodeList)
      {
        string str = !(parent.Name.ToUpper() == "OBJECT") ? parent.GetAttribute("emid") ?? "" : this.findParamValueWithName(parent, "emid");
        if (str != "" && !arrayList.Contains((object) str))
          arrayList.Add((object) str);
      }
      return (string[]) arrayList.ToArray(typeof (string));
    }

    public CodeLanguage GetCodeLanguageForEvents()
    {
      XmlNodeList elementsByTagName = this.xhtml.GetElementsByTagName("EMEVENTS");
      if (elementsByTagName.Count == 0)
        return CodeLanguage.None;
      switch (((XmlElement) elementsByTagName[0]).GetAttribute("language"))
      {
        case "C#":
          return CodeLanguage.CSharp;
        case "VB":
        case "VB.NET":
          return CodeLanguage.VB;
        default:
          return CodeLanguage.None;
      }
    }

    public bool ContainsEvents() => this.GetEvents().Length != 0;

    public bool RequiresAuthorization() => this.ContainsEvents() || this.GetCodeBase() != null;

    public EventDescriptor[] GetEvents()
    {
      XmlNodeList xmlNodeList = this.xhtml.SelectNodes("//*[@eventId]");
      ArrayList arrayList = new ArrayList();
      foreach (XmlElement xmlElement in xmlNodeList)
      {
        try
        {
          EventDescriptor eventDescriptor = new EventDescriptor(xmlElement.GetAttribute("for") ?? "", xmlElement.GetAttribute("event") ?? "", xmlElement.FirstChild.InnerText.Trim());
          if (eventDescriptor.EventSourceCode != "")
            arrayList.Add((object) eventDescriptor);
        }
        catch
        {
        }
      }
      return (EventDescriptor[]) arrayList.ToArray(typeof (EventDescriptor));
    }

    public ControlDescriptor[] GetControls() => this.GetControls(false);

    public ControlDescriptor[] GetControls(bool runtimeOnly)
    {
      XmlNodeList xmlNodeList = this.xhtml.SelectNodes("//*[@controlType]");
      ArrayList arrayList = new ArrayList();
      foreach (XmlElement xmlElement in xmlNodeList)
      {
        string controlId = xmlElement.GetAttribute("id") ?? "";
        string str = xmlElement.GetAttribute("controlType") ?? "";
        if (controlId != "" && str != "")
        {
          ControlDescriptor controlDescriptor = new ControlDescriptor(controlId, "EllieMae.Encompass.Forms." + str);
          if (!runtimeOnly || typeof (RuntimeControl).IsAssignableFrom(controlDescriptor.GetControlType()))
            arrayList.Add((object) controlDescriptor);
        }
      }
      return (ControlDescriptor[]) arrayList.ToArray(typeof (ControlDescriptor));
    }

    private string findParamValueWithName(XmlElement parent, string paramName)
    {
      foreach (XmlNode childNode in parent.ChildNodes)
      {
        if (childNode is XmlElement && childNode.Name.ToUpper() == "PARAM")
        {
          XmlElement xmlElement = childNode as XmlElement;
          if (xmlElement.GetAttribute("name") == paramName)
            return xmlElement.GetAttribute("value") ?? "";
          if (xmlElement.GetAttribute("NAME") == paramName)
            return xmlElement.GetAttribute("VALUE") ?? "";
        }
      }
      return "";
    }
  }
}
