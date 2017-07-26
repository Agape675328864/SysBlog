/*----------------------------------------------------------------
 * Copyright (C) 2017 GoGoTalk青少外教在线英语版权所有。 
 * 文件名：B_Article_Provider
 * 文件功能描述：

 * 创建者：杨红彬 (Administrator)
 * 时间：2017/6/21 17:37:56
 * 修改人：
 * 时间：
 * 修改说明：
 * 版本：V1.0.0
----------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SysTemCommHelper;
using SysTemModel;

namespace SysTemDAL
{
    public class B_Article_Provider
    {
        /// <summary>
        /// 用户发贴
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool Add(B_Article model)
        {
            string sql = @"INSERT INTO [B_Article]
                           ([UserId]
                           ,[Title]
                           ,[Content]
                           ,[Picture])
                     VALUES
                           (@UserId
                           ,@Title
                           ,@Content
                           ,@Picture)";

            SqlParameter[] param = {
                                  new SqlParameter("@UserId",model.UserId),
                                  new SqlParameter("@Title",model.Title),
                                  new SqlParameter("@Content",model.Content),
                                  new SqlParameter("@Picture",model.Picture)
                                  };
            return DBHelperDao.ExecuteNonQuery(sql, param) > 0;
        }
        /// <summary>
        /// 获取发帖列表
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="UserId">用户id -1全部</param>
        /// <param name="State">0:只显示有显示状态的 1：全部</param>
        /// <returns></returns>
        public static Tuple<List<B_Article>, int> GetArticleList(int PageIndex, int PageSize, string fiter = " 1=1")
        {
            string sql = ";WITH tab1 AS (SELECT ROW_NUMBER() OVER(ORDER BY CreateTime DESC) AS row_num,t.* FROM (SELECT * FROM dbo.B_Article WHERE {0}) AS t) SELECT * FROM tab1 WHERE row_num BETWEEN @PageIndex AND @PageSize";
            List<SqlParameter> ParaList = new List<SqlParameter>();

            ParaList.Add(new SqlParameter("@PageIndex", (PageSize * (PageIndex - 1) + 1)));
            ParaList.Add(new SqlParameter("@PageSize", PageSize * PageIndex));
            string TotalSql = ";WITH tab1 AS (SELECT ROW_NUMBER() OVER(ORDER BY CreateTime DESC) AS row_num,t.* FROM ( SELECT * FROM dbo.B_Article WHERE {0}) AS t) SELECT COUNT(1) FROM tab1";
            if (!string.IsNullOrEmpty(fiter))
            {
                sql = string.Format(sql, fiter);
                TotalSql = string.Format(TotalSql, fiter);
            }
            else
            {
                sql = string.Format(sql, "");
                TotalSql = string.Format(TotalSql, "");
            }

            int totalcount = DBHelperDao.ExecuteScalar(TotalSql, ParaList) == null ? 0 : int.Parse(DBHelperDao.ExecuteScalar(TotalSql, ParaList).ToString());
            Tuple<List<B_Article>, int> TupleModel = new Tuple<List<B_Article>, int>(DBHelperDao.GetList<B_Article>(sql, ParaList), totalcount);
            return TupleModel;
        }
        /// <summary>
        /// 文章id获取文章信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static List<B_Article> ArticleList(int Id)
        {
            string sql = "SELECT * FROM dbo.B_Article WHERE Id=@Id";
            return DBHelperDao.GetList<B_Article>(sql, new SqlParameter("@Id", Id));
        }
        /// <summary>
        /// 当前用户id获取所以主题发帖信息
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static List<B_Article> ArticleList(string ItemIds)
        {
            string sql = "SELECT * FROM dbo.B_Article WHERE UserId in (" + ItemIds + ")";
            return DBHelperDao.GetList<B_Article>(sql, null);
        }
        /// <summary>
        /// 获取当前用户回帖信息
        /// </summary>
        /// <param name="ItemIds"></param>
        /// <returns></returns>
        public static List<B_RepliesArticle> RepliesArticleList(string ItemIds)
        {
            string sql = "SELECT * FROM dbo.B_RepliesArticle WHERE UserId in (" + ItemIds + ")";
            return DBHelperDao.GetList<B_RepliesArticle>(sql, null);
        }
        /// <summary>
        /// 修改文章显示状态
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool UpdateAritcleStatus(B_Article model)
        {
            string sql = @"UPDATE [dbo].[B_Article]
                           SET [State] = @State
                           WHERE Id=@Id";
            SqlParameter[] param ={
                                     new SqlParameter("@Id", model.Id),
                                     new SqlParameter("@State",model.State)
                                   };
            return DBHelperDao.ExecuteNonQuery(sql, param) > 0;
        }
    }
}
