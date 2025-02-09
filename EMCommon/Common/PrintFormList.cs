// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.PrintFormList
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.Common.Licensing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class PrintFormList : IEnumerable<PrintForm>, IEnumerable
  {
    private List<PrintForm> formList = new List<PrintForm>();
    private Dictionary<string, PrintForm> idMap = new Dictionary<string, PrintForm>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);

    public PrintFormList(XmlDocument xml, EncompassEdition edition)
    {
      foreach (XmlElement selectNode in xml.SelectNodes("/formList/add"))
      {
        PrintForm printForm = new PrintForm(selectNode);
        if (edition == EncompassEdition.None || printForm.AppliesToEdition(edition))
        {
          this.formList.Add(printForm);
          this.idMap[printForm.FormID] = printForm;
        }
      }
    }

    public PrintFormList(XmlDocument xml)
      : this(xml, EncompassEdition.None)
    {
    }

    public PrintFormList(string path)
      : this(path, EncompassEdition.None)
    {
    }

    public PrintFormList(string path, EncompassEdition edition)
      : this(PrintFormList.openXml(path), edition)
    {
    }

    public int Count => this.formList.Count;

    public PrintForm this[int index] => this.formList[index];

    public PrintForm GetFormByID(string formId)
    {
      PrintForm formById = (PrintForm) null;
      this.idMap.TryGetValue(formId, out formById);
      return formById;
    }

    private static XmlDocument openXml(string path)
    {
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.Load(path);
      return xmlDocument;
    }

    public IEnumerator<PrintForm> GetEnumerator()
    {
      return (IEnumerator<PrintForm>) this.formList.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.formList.GetEnumerator();

    public static PrintFormList Parse(string xml, EncompassEdition edition)
    {
      try
      {
        XmlDocument xml1 = new XmlDocument();
        xml1.XmlResolver = (XmlResolver) null;
        xml1.LoadXml(xml);
        return new PrintFormList(xml1, edition);
      }
      catch (Exception ex)
      {
        Tracing.Log(true, "Error", "PrintFormList.Parse()", ex.Message);
        Tracing.Log(true, "Error", "PrintFormList.Parse()", xml);
        throw ex;
      }
    }

    public static PrintFormList Parse(string xml)
    {
      return PrintFormList.Parse(xml, EncompassEdition.None);
    }
  }
}
