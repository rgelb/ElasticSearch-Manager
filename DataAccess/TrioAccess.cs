using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ElasticSearchManager
{
    public class TrioAccess : Database
    {
        #region Constants
        private const string SP_GET_MARKET_ROUTING_SET = "pr_Get_MarketRouting_set";
        private const string SP_GET_INVOICE_LIST = "pr_Get_AttachmentInvoice_set";
        private const string SP_GET_INVOICE_CONTENT = "pr_Get_AttachmentContent_rec";
        private const string SP_GET_COPY_GROUP_LIST = "pr_GetCopyGroupByAdvertiserSfdcAndLengthMdm_set";
        private const string SP_GET_COPY_GROUP_BY_ORDER = "pr_GetCopyGroupByOrder_set";
        private const string SP_INS_COPY_GROUP = "pr_CopyGroup_ins";
        private const string SP_GET_AE_LIST = "pr_Get_AE_List_Recent_set";
        private const string SP_GET_AE_LIST_BY_MARKET = "pr_Get_AE_List_Recent_ByMarket_set";
        private const string SP_GET_SPECIAL_EVENTS_BY_MARKET = "pr_Get_SpecialEvents_Recent_ByMarket_set";
        private const string SP_RPT_ORDER_CONFIRMATION = "ms_rpt_order_confirmation_wrapper";
        private const string SP_GET_ORDER_HEAD_BY_PROPOSAL_ID = "pr_Sf_OrderHead_rec";
        private const string SP_GET_ZYZ_CONST = "dp_GetOption";
        private const string SP_GET_ADV_REV_BY_STATION_DATERANGE = "ms_rpt_AdvertiserRevenueByStationAndRange";
        #endregion

        #region Private Variables

        private readonly string connectionString = string.Empty;

        #endregion

        #region Constructors
        public TrioAccess(string connString)
        {
            connectionString = connString;
        }
        #endregion

        #region Public Methods

        public DataSet GetLayoutCategoriesAndTypes(Int32 inParentLayoutCategoryID, Int32 inRECoID) {

            var parameters = new[]
            {
                new DbParameter("@LayoutCategoryID", inParentLayoutCategoryID),
                new DbParameter("@RECoID", inRECoID)
            };

            return ExecuteDataSet(connectionString, "tsp_getLayoutCategoriesAndTypesByRECo", parameters);
        }

        public DataTable GetRecos() {
            string sql = @"SELECT rc.RECoID as Id, rc.Name 
                            FROM dbo.RECo rc
                            WHERE rc.ActiveFlg = 1 
                            ORDER BY rc.Name";

            return ExecuteDataTableText(connectionString, sql);
        }

        public DataTable GetActiveLayoutById(int layoutID) {
            string sql  = $@"SELECT l.Markup,
                                   l.LayoutVersion,
                                   l.CreateDtm,
                                   l.ModifyDtm,
                                   l.IsActive,
                                   l.Notes,
                                   l.LayoutID,
                                   l.BasedOnLayoutVersion 
                            FROM dbo.Layout l
                            WHERE l.LayoutID = {layoutID}
	                            AND l.IsActive = 1";

            return ExecuteDataTableText(connectionString, sql);
        }


        public void GenerateChangeHistory()
        {
            
        }

        public bool SaveLayout(int layoutId, int version, string notes, string markup, 
                            int recoId, int basedOnVersion, int layoutTypeId)
        {
            var parameters = new[]
            {
                new DbParameter("@LayoutID", layoutId),
                new DbParameter("@Version", version),
                new DbParameter("@Notes", notes),
                new DbParameter("@Markup", markup),
                new DbParameter("@RECoID", recoId),
                new DbParameter("@BasedOnLayoutVersion", basedOnVersion),
                new DbParameter("@LayoutTypeID", layoutTypeId)
            };

            int rowsAffected = ExecuteNonQuery(connectionString, "tsp_updLayoutMarkup", parameters);

            return (rowsAffected > 0);
        }

        #endregion

        #region Examples

        //public int CreateCopyGroup(string AdvertiserID, string SpotLengthID, string Title, string SpotTypeID, DateTime StartDate, DateTime EndDate)
        //{
        //    var parameters = new[] 
        //    { 
        //        new DbParameter("@chvAdvertiserSfdcID", AdvertiserID),
        //        new DbParameter("@chvSpotLengthMdmID", SpotLengthID),
        //        new DbParameter("@chvTitle", Title),
        //        new DbParameter("@chvSpotTypeMdmID", SpotTypeID),
        //        new DbParameter("@StartDate", StartDate),
        //        new DbParameter("@EndDate", EndDate)
        //    };
                
        //    return (int)ExecuteScalar(connectionString, SP_INS_COPY_GROUP, parameters);
        //}        

        #endregion


        public bool PromoteLayoutVersionToActive(int layoutId, int version, int recoId)
        {
            var parameters = new[]
            {
                new DbParameter("@LayoutID", layoutId),
                new DbParameter("@Version", version),
                new DbParameter("@RECoID", recoId)
            };

            int rowsAffected = ExecuteNonQuery(connectionString, "tsp_updLayoutPromoteVersion", parameters);

            return (rowsAffected > 0);            
        }

        public DataTable SearchLayouts(string text, int recoId) {
            string sql  = $@"SELECT l.LayoutID as LayoutId, rcl.RECoID, dlcParent.Name AS ParentCategory, dlcChild.Name AS ChildCategory, dlt.Name AS LayoutName
                            FROM  dbo.Layout l (NOLOCK)
                            JOIN dbo.RECoLayout rcl (NOLOCK) ON rcl.LayoutID = l.LayoutID
                            JOIN dbo.dmnLayoutType dlt (NOLOCK) ON dlt.LayoutTypeID = rcl.LayoutTypeID
                            JOIN dbo.dmnLayoutCategory dlcChild (NOLOCK) ON dlcChild.LayoutCategoryID = dlt.LayoutCategoryID
                            JOIN dbo.dmnLayoutCategory dlcParent (NOLOCK) ON dlcParent.LayoutCategoryID = dlcChild.ParentLayoutCategoryID
                            JOIN dbo.RECo rc ON rc.RECoID = rcl.RECoID
                            WHERE rc.ActiveFlg = 1 AND  l.IsActive = 1  AND l.Markup LIKE '%{text}%' and rc.RECoID = {recoId}		
                            ORDER BY rc.RECoID, dlcParent.Name, dlcChild.Name, dlt.Name";

            return ExecuteDataTableText(connectionString, sql);            
        }
    }
}
