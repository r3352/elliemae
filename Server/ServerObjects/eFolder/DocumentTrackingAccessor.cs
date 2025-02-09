// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.eFolder.DocumentTrackingAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.Security.AntiXss;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.eFolder
{
  public class DocumentTrackingAccessor
  {
    private const string tableName = "DocumentTrackingSetup�";

    private DocumentTrackingAccessor()
    {
    }

    public static IEnumerable<DocumentTemplate> GetDocumentTemplates()
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select * from DocumentTrackingSetup order by [Name] Asc");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection != null && dataRowCollection.Count > 0)
      {
        XmlSerializer serializer = new XmlSerializer();
        foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
          yield return serializer.Deserialize<DocumentTemplate>("<objdata><element name=\"root\"><element name=\"Guid\">" + string.Format("{0}</element><element name=\"Name\">", dataRow["Guid"]) + AntiXssEncoder.XmlEncode(dataRow["Name"].ToString()) + string.Format("</element>{0}</element></objdata>", dataRow["Data"]));
        serializer = (XmlSerializer) null;
      }
    }

    public static void SaveDocumentTrackingSetup(DocumentTrackingSetup docTrackingSetup)
    {
      DocumentTrackingAccessor.MergeValues(DocumentTrackingAccessor.GetValuesToMerge(DocumentTrackingAccessor.GetSetupXml(docTrackingSetup)), true);
    }

    public static void UpsertDocumentTrackingTemplate(
      DocumentTrackingSetup docTrackingSetup,
      string targetGuid)
    {
      DocumentTrackingAccessor.MergeValues(DocumentTrackingAccessor.GetValuesToMerge(DocumentTrackingAccessor.GetSetupXml(docTrackingSetup), targetGuid), false);
    }

    private static XmlDocument GetSetupXml(DocumentTrackingSetup docTrackingSetup)
    {
      XmlDocument setupXml = new XmlDocument();
      using (BinaryObject binaryObject = new BinaryObject((IXmlSerializable) docTrackingSetup))
      {
        string xml = binaryObject.ToString(Encoding.UTF8);
        setupXml.LoadXml(xml);
      }
      return setupXml;
    }

    private static List<List<object>> GetValuesToMerge(XmlDocument docTemp, string targetGuid = null)
    {
      XmlNodeList xmlNodeList = docTemp.SelectNodes("objdata/element/element[element[@name='Guid']]");
      List<List<object>> valuesToMerge = new List<List<object>>();
      foreach (XmlNode xmlNode in xmlNodeList)
      {
        string innerText1 = xmlNode.SelectSingleNode("element[@name='Guid']").InnerText;
        string innerText2 = xmlNode.SelectSingleNode("element[@name='Name']").InnerText;
        xmlNode.RemoveChild(xmlNode.SelectSingleNode("element[@name='Guid']"));
        xmlNode.RemoveChild(xmlNode.SelectSingleNode("element[@name='Name']"));
        string innerXml = xmlNode.InnerXml;
        if (targetGuid == null || innerText1 == targetGuid)
          valuesToMerge.Add(new List<object>()
          {
            (object) innerText1,
            (object) innerText2,
            (object) innerXml
          });
      }
      return valuesToMerge;
    }

    private static void MergeValues(List<List<object>> values, bool useDelete)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      MergeTable table = new MergeTable()
      {
        Name = "DocumentTrackingSetup",
        Columns = new List<MergeColumn>()
        {
          new MergeColumn("Guid", MergeIntent.PrimaryKey, DbColumnType.VarChar, 38),
          new MergeColumn("Name", MergeIntent.Upsert, DbColumnType.VarChar, 512),
          new MergeColumn("Data", MergeIntent.Upsert, DbColumnType.Text, 0)
        },
        Rows = values
      };
      dbQueryBuilder.AppendMsMergeTable(table, useDelete);
      dbQueryBuilder.ExecuteRowQuery();
    }
  }
}
