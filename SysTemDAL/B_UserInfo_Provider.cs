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
    public class B_UserInfo_Provider
    {
        /// <summary>
        /// 添加博客用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool Add(B_UserInfo model)
        {
            string sql = "INSERT INTO [B_UserInfo] ([NickName],[Password],[Photo], [Gender], [BirthDay]) VALUES (@NickName,@Password,@Photo,@Gender,@BirthDay)";
            SqlParameter[] param = {
                                  new SqlParameter("@NickName",model.NickName),
                                  new SqlParameter("@Password",model.Password),
                                  new SqlParameter("@Photo",model.Photo),
                                  new SqlParameter("@Gender",model.Gender),
                                  new SqlParameter("@BirthDay",model.BirthDay)
                                  };
            return DBHelperDao.ExecuteNonQuery(sql, param) > 0;
        }
        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool UpdateUserInfo(B_UserInfo model)
        {
            string sql = @"UPDATE [dbo].[B_UserInfo]
                           SET [Photo] = @Photo
                              ,[Gender] = @Gender
                              ,[BirthDay] = @BirthDay
                              ,[State]=@State
                           WHERE Id=@Id";
            SqlParameter[] param ={
                                     new SqlParameter("@Photo", model.Photo),
                                     new SqlParameter("@Gender", model.Gender),
                                     new SqlParameter("@BirthDay", model.BirthDay),
                                     new SqlParameter("@Id", model.Id),
                                     new SqlParameter("@State",model.State)
                                   };
            return DBHelperDao.ExecuteNonQuery(sql, param) > 0;
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="OldPwd"></param>
        /// <param name="NewPwd"></param>
        /// <returns></returns>
        public static int ChangerPwd(string Id, string OldPwd, string NewPwd)
        {
            int i = 0;
            if (GetUserInfoListById(Id).SingleOrDefault().Password.Equals(OldPwd))//判断旧密码是否正确
            {
                string sql = @"UPDATE [dbo].[B_UserInfo]
                           SET [Password] = @Password
                           WHERE Id=@Id";
                SqlParameter[] param ={
                                     new SqlParameter("@Password", NewPwd),
                                     new SqlParameter("@Id", Id),
                                   };
                if (DBHelperDao.ExecuteNonQuery(sql, param) > 0)
                {
                    i = 1;//成功
                }
                else
                {
                    i = 2;//修改失败
                }
            }
            else
            {
                i = -1;//旧密码不正确
            }
            return i;
        }

        /// <summary>
        /// 根据用户昵称查询用户信息
        /// </summary>
        /// <param name="NickName"></param>
        /// <returns></returns>
        public static List<B_UserInfo> GetUserInfoList(string NickName)
        {
            string sql = "select * from B_UserInfo where NickName=@NickName";
            return DBHelperDao.GetList<B_UserInfo>(sql, new SqlParameter("@NickName", NickName));
        }
        /// <summary>
        /// 根据用户Id获取用户信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static List<B_UserInfo> GetUserInfoListById(string Id)
        {
            string sql = "select * from B_UserInfo where Id=@Id";
            return DBHelperDao.GetList<B_UserInfo>(sql, new SqlParameter("@Id", Id));
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="NickName"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public static List<B_UserInfo> LoginUserInfo(string NickName, string Password)
        {
            string sql = "select * from B_UserInfo where NickName=@NickName and Password=@Password and State=1";
            return DBHelperDao.GetList<B_UserInfo>(sql, new SqlParameter("@NickName", NickName), new SqlParameter("@Password", Password));
        }



        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static Tuple<List<B_UserInfo>, int> GetUserListPage(int PageIndex, int PageSize, string NickName)
        {

            string sql = ";WITH tab1 AS (SELECT ROW_NUMBER() OVER(ORDER BY CreateTime DESC) AS row_num,t.* FROM (SELECT * FROM dbo.B_UserInfo WHERE 1=1 {0}) AS t) SELECT * FROM tab1 WHERE row_num BETWEEN @PageIndex AND @PageSize";
            List<SqlParameter> ParaList = new List<SqlParameter>();

            ParaList.Add(new SqlParameter("@PageIndex", (PageSize * (PageIndex - 1) + 1)));
            ParaList.Add(new SqlParameter("@PageSize", PageSize * PageIndex));
            string TotalSql = ";WITH tab1 AS (SELECT ROW_NUMBER() OVER(ORDER BY CreateTime DESC) AS row_num,t.* FROM ( SELECT * FROM dbo.B_UserInfo WHERE 1=1 {0}) AS t) SELECT COUNT(1) FROM tab1";
            if (!string.IsNullOrEmpty(NickName))
            {
                NickName = "%" + NickName + "%";
                sql = string.Format(sql, " AND NickName like @NickName");
                TotalSql = string.Format(TotalSql, " AND NickName like @NickName");
                ParaList.Add(new SqlParameter("@NickName", NickName));
            }
            else
            {
                sql = string.Format(sql, "");
                TotalSql = string.Format(TotalSql, "");
            }
            int totalcount = DBHelperDao.ExecuteScalar(TotalSql, ParaList) == null ? 0 : int.Parse(DBHelperDao.ExecuteScalar(TotalSql, ParaList).ToString());
            Tuple<List<B_UserInfo>, int> TupleModel = new Tuple<List<B_UserInfo>, int>(DBHelperDao.GetList<B_UserInfo>(sql, ParaList), totalcount);
            return TupleModel;
        }

    }
}
