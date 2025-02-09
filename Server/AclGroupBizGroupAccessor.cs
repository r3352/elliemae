// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.AclGroupBizGroupAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class AclGroupBizGroupAccessor
  {
    private const string className = "AclGroupBizGroupAccessor�";
    private const string tableName_AclGroupPublicBizGroupRef = "[AclGroupPublicBizGroupRef]�";

    public static AclTriState GetBizContactAccessRight(UserInfo userObj, int contactID)
    {
      AclTriState contactAccessRight = AclTriState.False;
      try
      {
        if (userObj.IsSuperAdministrator())
        {
          contactAccessRight = AclTriState.True;
        }
        else
        {
          if (userObj.IsTopLevelAdministrator())
          {
            int[] withoutGroupIdList = BizPartnerContact.GetPublicBizContactWithoutGroupIDList();
            if (withoutGroupIdList != null && withoutGroupIdList.Length != 0)
            {
              foreach (int num in withoutGroupIdList)
              {
                if (num == contactID)
                {
                  contactAccessRight = AclTriState.True;
                  break;
                }
              }
            }
          }
          if (contactAccessRight != AclTriState.True)
          {
            AclGroup[] groupsOfUser = AclGroupAccessor.GetGroupsOfUser(userObj.Userid);
            if (groupsOfUser != null && groupsOfUser.Length != 0)
            {
              string str = string.Concat((object) groupsOfUser[0].ID);
              for (int index = 1; index < groupsOfUser.Length; ++index)
                str = str + ", " + (object) groupsOfUser[index].ID;
              DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
              dbQueryBuilder.Append("select Distinct AGP.* from AclGroups AG inner join AclGroupPublicBizGroupRef AGP on AG.groupID = AGP.aclGroupID inner join ContactGroupPartners CG on AGP.bizGroupID = CG.GroupId Where AG.groupID in (" + str + ") AND CG.ContactId = " + (object) contactID + " and AGP.Access = 1");
              DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
              contactAccessRight = dataRowCollection == null || dataRowCollection.Count == 0 ? AclTriState.False : AclTriState.True;
            }
          }
        }
        return contactAccessRight;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupBizGroupAccessor), ex);
        return contactAccessRight;
      }
    }

    public static AclTriState GetBizContactGroupAccessRight(UserInfo userObj, int contactGroupID)
    {
      AclTriState groupAccessRight = AclTriState.False;
      try
      {
        if (userObj.IsSuperAdministrator())
        {
          groupAccessRight = AclTriState.True;
        }
        else
        {
          AclGroup[] groupsOfUser = AclGroupAccessor.GetGroupsOfUser(userObj.Userid);
          if (groupsOfUser != null && groupsOfUser.Length != 0)
          {
            string str = string.Concat((object) groupsOfUser[0].ID);
            for (int index = 1; index < groupsOfUser.Length; ++index)
              str = str + ", " + (object) groupsOfUser[index].ID;
            DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
            dbQueryBuilder.Append("select Distinct AGP.* from AclGroups AG inner join AclGroupPublicBizGroupRef AGP on AG.groupID = AGP.aclGroupID Where AG.groupID in (" + str + ") AND AGP.bizGroupID = " + (object) contactGroupID);
            DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
            if (dataRowCollection == null || dataRowCollection.Count == 0)
              return groupAccessRight;
            groupAccessRight = !(dataRowCollection[0]["Access"].ToString() == "1") ? AclTriState.False : AclTriState.True;
          }
        }
        return groupAccessRight;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupBizGroupAccessor), ex);
        return groupAccessRight;
      }
    }

    public static BizGroupRef[] GetBizContactGroupRefs(int aclGroupId)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.Append("select * from AclGroupPublicBizGroupRef where aclGroupID = " + (object) aclGroupId);
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection == null)
          return new BizGroupRef[0];
        BizGroupRef[] contactGroupRefs = new BizGroupRef[dataRowCollection.Count];
        for (int index = 0; index < contactGroupRefs.Length; ++index)
          contactGroupRefs[index] = new BizGroupRef((int) dataRowCollection[index]["bizGroupID"], (AclResourceAccess) Enum.Parse(typeof (AclResourceAccess), dataRowCollection[index]["Access"].ToString()));
        return contactGroupRefs;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupBizGroupAccessor), ex);
        return (BizGroupRef[]) null;
      }
    }

    public static void ResetBizContactGroupRefs(
      int aclGroupId,
      BizGroupRef[] bizGroupRefs,
      string loggedInUser)
    {
      try
      {
        if (bizGroupRefs == null)
          return;
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("delete from AclGroupPublicBizGroupRef where aclGroupID = " + (object) aclGroupId);
        for (int index = 0; index < bizGroupRefs.Length; ++index)
          dbQueryBuilder.AppendLine("insert into [AclGroupPublicBizGroupRef] (aclGroupID, bizGroupID, Access) values (" + (object) aclGroupId + ", " + (object) bizGroupRefs[index].BizGroupID + ", " + (object) (int) bizGroupRefs[index].Access + ")");
        dbQueryBuilder.AppendLine(AclGroupAccessor.UpdateUserGroupMetadata(loggedInUser, aclGroupId));
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupBizGroupAccessor), ex);
      }
    }

    public static void UpdateBizContactGroupRef(
      int aclGroupId,
      BizGroupRef bizGroupRef,
      string loggedInUser)
    {
      if (bizGroupRef == (BizGroupRef) null)
        return;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("update AclGroupPublicBizGroupRef set access = " + (object) (int) bizGroupRef.Access + " where aclGroupID = " + (object) aclGroupId + " and bizGroupID = " + (object) bizGroupRef.BizGroupID);
      dbQueryBuilder.AppendLine(AclGroupAccessor.UpdateUserGroupMetadata(loggedInUser, aclGroupId));
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void Clone(int sourceGroupID, int desGroupID)
    {
      try
      {
        DbQueryBuilder sql = new DbQueryBuilder();
        sql.Append("select * from [AclGroupPublicBizGroupRef] where aclGroupID = " + (object) sourceGroupID);
        DataTable sourceTable = sql.ExecuteTableQuery();
        sql.Reset();
        if (sourceTable == null || sourceTable.Rows.Count <= 0)
          return;
        AclGroupBizGroupAccessor.CloneStatementHelper(sourceTable, sql, "[AclGroupPublicBizGroupRef]", "aclGroupID", desGroupID);
        sql.ExecuteNonQuery();
        sql.Reset();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupBizGroupAccessor), ex);
      }
    }

    private static void CloneStatementHelper(
      DataTable sourceTable,
      DbQueryBuilder sql,
      string tableName,
      string keyColumnName,
      int desKeyIDValue)
    {
      AclGroupAccessor.CloneStatementHelper(sourceTable, sql, tableName, keyColumnName, desKeyIDValue);
    }
  }
}
