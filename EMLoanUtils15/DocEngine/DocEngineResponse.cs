// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DocEngine.DocEngineResponse
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.LoanUtils.DocEngine;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DocEngine
{
  internal class DocEngineResponse
  {
    private XmlDocument responseXml;
    private DocEngineResponseStatus status;

    public DocEngineResponse(string xmlText)
    {
      this.responseXml = new XmlDocument();
      try
      {
        this.responseXml.LoadXml(xmlText);
      }
      catch (Exception ex)
      {
        throw new Exception("Failed to parse DocEngineResponse XML", ex);
      }
      SyncedDataHelper.ReadItemsNeededByEds(this.responseXml);
      string attribute = this.responseXml.DocumentElement.GetAttribute("Status");
      try
      {
        this.status = (DocEngineResponseStatus) Enum.Parse(typeof (DocEngineResponseStatus), attribute, true);
      }
      catch
      {
        this.status = DocEngineResponseStatus.Unknown;
      }
    }

    public XmlDocument ResponseXml => this.responseXml;

    public DocEngineResponseStatus ResponseStatus => this.status;

    public bool IsError() => this.status < DocEngineResponseStatus.Unknown;

    public string GetErrorMessage()
    {
      if (this.ResponseStatus == DocEngineResponseStatus.Exception)
        return this.responseXml.DocumentElement.InnerText;
      if (!this.IsError())
        return (string) null;
      string attribute = this.responseXml.DocumentElement.GetAttribute("Message");
      return !(attribute != "") ? "Unknown error" : attribute;
    }
  }
}
