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
    public class B_SysUser_Provider
    {
        public static List<B_SysUser> GetSysUserList(string SysName, string Password)
        {
            string sql = "select * from B_SysUser where SysName=@SysName and Password=@Password";
            return DBHelperDao.GetList<B_SysUser>(sql, new SqlParameter("@SysName", SysName), new SqlParameter("@Password", Password));
        }

        public static B_SysUser GetModel(int Id)
        {
            string sql = "select * from B_SysUser where Id=@Id";
            return DBHelperDao.GetModel<B_SysUser>(sql, new SqlParameter("@Id", Id));
        }
    }
}
