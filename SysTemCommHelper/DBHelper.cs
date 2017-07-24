using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Configuration;
using System.Collections;

namespace SysTemCommHelper
{

    public static class DBHelperDao
    {
        #region 变量
        private static readonly string CONNECTION_SQLSERVER = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        #endregion
        #region 构造函数
        static DBHelperDao()
        {

        }
        #endregion
        /// <summary>
        /// 返回一个打开状态的SqlConnection连接对象
        /// 单例设计模式
        /// </summary>
        /// <summary>
        /// 增，删，改
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string sql, params SqlParameter[] parameters)
        {

            int count = 0;
            using (SqlConnection con = new SqlConnection(CONNECTION_SQLSERVER))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    if (parameters != null && parameters.Length > 0)
                    {
                        foreach (SqlParameter param in parameters)
                        {
                            if ((param.Direction == ParameterDirection.InputOutput || param.Direction == ParameterDirection.Input) && param.Value == null)
                                param.Value = DBNull.Value;
                            cmd.Parameters.Add(param);
                        }
                    }
                    //cmd.Parameters.AddRange(parameters);
                    count = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
                return count;

            }
        }
        /// <summary>
        /// 增，删，改
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string sql, List<SqlParameter> parameters = null)
        {
            int count = 0;
            try
            {

                using (SqlConnection con = new SqlConnection(CONNECTION_SQLSERVER))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        if (parameters != null && parameters.Count > 0)
                        {
                            foreach (SqlParameter param in parameters)
                            {
                                if ((param.Direction == ParameterDirection.InputOutput || param.Direction == ParameterDirection.Input) && param.Value == null)
                                    param.Value = DBNull.Value;
                                cmd.Parameters.Add(param);
                            }
                        }
                        count = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }


                }
            }
            catch
            {

            }
            return count;
        }
        /// <summary>
        /// 增，删，改 事务
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static bool ExecuteNonQuery(List<string> cmdsql, params SqlParameter[] parameters)
        {
            bool istrue = false;
            using (SqlConnection con = new SqlConnection(CONNECTION_SQLSERVER))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    SqlTransaction trans = con.BeginTransaction();
                    cmd.Transaction = trans;
                    if (parameters != null && parameters.Length > 0)
                    {
                        foreach (SqlParameter param in parameters)
                        {
                            if ((param.Direction == ParameterDirection.InputOutput || param.Direction == ParameterDirection.Input) && param.Value == null)
                                param.Value = DBNull.Value;
                            cmd.Parameters.Add(param);
                        }
                    }
                    // cmd.Parameters.AddRange(parameters);
                    try
                    {
                        foreach (string sql in cmdsql)
                        {
                            cmd.CommandText = sql;
                            cmd.ExecuteNonQuery();
                        }
                        trans.Commit();
                        istrue = true;
                    }
                    catch
                    {

                        trans.Rollback();
                    }
                    cmd.Parameters.Clear();
                }
            }
            return istrue;
        }
        /// <summary>
        /// 增，删，改 事务
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static bool ExecuteNonQuery(List<string> cmdsql, List<SqlParameter> parameters = null)
        {
            bool istrue = false;
            using (SqlConnection con = new SqlConnection(CONNECTION_SQLSERVER))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    SqlTransaction trans = con.BeginTransaction();
                    cmd.Transaction = trans;
                    if (parameters != null && parameters.Count > 0)
                    {
                        foreach (SqlParameter param in parameters)
                        {
                            if ((param.Direction == ParameterDirection.InputOutput || param.Direction == ParameterDirection.Input) && param.Value == null)
                                param.Value = DBNull.Value;
                            cmd.Parameters.Add(param);
                        }
                    }
                    try
                    {
                        foreach (string sql in cmdsql)
                        {
                            cmd.CommandText = sql;
                            cmd.ExecuteNonQuery();
                        }
                        trans.Commit();
                        istrue = true;
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message == "")
                        {

                        }

                        trans.Rollback();
                    }
                    cmd.Parameters.Clear();
                }
            }
            return istrue;
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static DataTable ExecuteData(string sql, params SqlParameter[] parameters)
        {
            DataTable dt = null;
            using (SqlConnection con = new SqlConnection(CONNECTION_SQLSERVER))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    if (parameters != null && parameters.Length > 0)
                    {
                        foreach (SqlParameter param in parameters)
                        {
                            if ((param.Direction == ParameterDirection.InputOutput || param.Direction == ParameterDirection.Input) && param.Value == null)
                                param.Value = DBNull.Value;
                            cmd.Parameters.Add(param);
                        }
                    }
                    cmd.CommandText = sql;
                    // cmd.Parameters.AddRange(parameters);
                    DataSet ds = new DataSet();
                    SqlDataAdapter ad = new SqlDataAdapter(cmd);
                    ad.Fill(ds);
                    dt = ds.Tables[0];
                    cmd.Parameters.Clear();
                }
            }
            return dt;
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static DataTable ExecuteData(string sql, List<SqlParameter> parameters = null)
        {
            DataTable dt = null;

            using (SqlConnection con = new SqlConnection(CONNECTION_SQLSERVER))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = sql;
                    if (parameters != null && parameters.Count > 0)
                    {
                        foreach (SqlParameter param in parameters)
                        {
                            if ((param.Direction == ParameterDirection.InputOutput || param.Direction == ParameterDirection.Input) && param.Value == null)
                                param.Value = DBNull.Value;
                            cmd.Parameters.Add(param);
                        }
                    }
                    DataSet ds = new DataSet();
                    SqlDataAdapter ad = new SqlDataAdapter(cmd);
                    ad.Fill(ds);
                    dt = ds.Tables[0];
                    cmd.Parameters.Clear();
                }
            }
            return dt;
        }
        /// <summary>
        /// 查询 单个数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string sql, params SqlParameter[] parameters)
        {
            object obj = null;
            using (SqlConnection con = new SqlConnection(CONNECTION_SQLSERVER))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = sql;
                    if (parameters != null && parameters.Length > 0)
                    {
                        foreach (SqlParameter param in parameters)
                        {
                            if ((param.Direction == ParameterDirection.InputOutput || param.Direction == ParameterDirection.Input) && param.Value == null)
                                param.Value = DBNull.Value;
                            cmd.Parameters.Add(param);
                        }
                    }
                    // cmd.Parameters.AddRange(parameters);
                    obj = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                }
            }
            return obj;
        }
        /// <summary>
        /// 查询 单个数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string sql, List<SqlParameter> parameters = null)
        {
            object obj = null;
            using (SqlConnection con = new SqlConnection(CONNECTION_SQLSERVER))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = sql;
                    if (parameters != null && parameters.Count > 0)
                    {
                        foreach (SqlParameter param in parameters)
                        {
                            if ((param.Direction == ParameterDirection.InputOutput || param.Direction == ParameterDirection.Input) && param.Value == null)
                                param.Value = DBNull.Value;
                            cmd.Parameters.Add(param);
                        }
                    }

                    obj = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                }
            }
            return obj;
        }
        /// <summary>
        /// 查询 多行数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(string sql, params SqlParameter[] parameters)
        {
            SqlDataReader obj = null;
            SqlConnection con = new SqlConnection(CONNECTION_SQLSERVER);
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = sql;
            if (parameters != null && parameters.Length > 0)
            {
                foreach (SqlParameter param in parameters)
                {
                    if ((param.Direction == ParameterDirection.InputOutput || param.Direction == ParameterDirection.Input) && param.Value == null)
                        param.Value = DBNull.Value;
                    cmd.Parameters.Add(param);
                }
            }
            // cmd.Parameters.AddRange(parameters);
            obj = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            cmd.Parameters.Clear();
            return obj;
        }
        /// <summary>
        /// 将一条查询记录创建为一个实体类对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <returns></returns>
        private static T ExecuteDataReader<T>(SqlDataReader dr)
        {
            string fieldName = string.Empty;
            T obj = default(T);
            try
            {
                //创建对象
                obj = Activator.CreateInstance<T>();
                Type type = typeof(T);
                //获取对象属性列表
                PropertyInfo[] properties = type.GetProperties();
                int fieldCount = dr.FieldCount;
                for (int i = 0; i < fieldCount; i++)
                {
                    object value = dr.GetValue(i);

                    fieldName = dr.GetName(i).ToUpper();
                    //类型不匹配返回single类型 导致异常  数据库中float=C# 中 double
                    //根据数据字段匹配属性列表
                    PropertyInfo pInfo = properties.SingleOrDefault(p => p.Name.ToUpper().Equals(fieldName));
                    if (pInfo == null)
                    {
                        // 如果从此类属性未找到，则搜索旗子类对象
                        SetObj(properties, value, fieldName, obj);
                    }
                    else
                    {
                        if (value == null || value == DBNull.Value)
                        {
                            continue;
                        }
                        pInfo.SetValue(obj, value, null);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message != "")
                {

                }
            }
            return obj;
        }
        /// <summary>
        /// 递归赋值对象
        /// </summary>
        /// <param name="_properties"></param>
        private static object SetObj(PropertyInfo[] properties, object value, string filedName, object obj)
        {
            //子对象
            var _properties = from p in properties where p.Name.IndexOf('_') == 0 select p;
            foreach (PropertyInfo propertyInfo in _properties)
            {
                Type _pType = propertyInfo.PropertyType;
                //检查子对象是否实例化
                object _obj = null;
                _obj = propertyInfo.GetValue(obj, null);
                if (_obj == null)
                {
                    _obj = Activator.CreateInstance(_pType);
                }

                PropertyInfo[] _pProperties = _pType.GetProperties();
                PropertyInfo _ChildPInfo = _pProperties.SingleOrDefault(p => p.Name.ToUpper().Equals(filedName));
                if (_ChildPInfo == null)
                {
                    //取消递归
                    //  propertyInfo.SetValue(obj,SetObj(_pProperties, value, filedName, _obj),null);
                }
                else
                {
                    if (value != null && value != DBNull.Value)
                        _ChildPInfo.SetValue(_obj, value, null);
                    propertyInfo.SetValue(obj, _obj, null);
                    break;
                }

            }
            return obj;
        }
        /// <summary>
        /// 查询 多行数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(string sql, List<SqlParameter> parameters)
        {
            SqlDataReader obj = null;
            SqlConnection con = new SqlConnection(CONNECTION_SQLSERVER);
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = sql;
            try
            {
                if (parameters != null && parameters.Count > 0)
                {
                    foreach (SqlParameter param in parameters)
                    {
                        if ((param.Direction == ParameterDirection.InputOutput || param.Direction == ParameterDirection.Input) && param.Value == null)
                            param.Value = DBNull.Value;
                        cmd.Parameters.Add(param);
                    }
                }
                obj = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
            }
            catch (Exception)
            {

            }
            return obj;
        }
        /// <summary>
        /// 反射实体类集合
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static List<T> GetList<T>(string sql, params SqlParameter[] parameters)
        {
            List<T> lists = new List<T>();
            try
            {
                using (SqlConnection con = new SqlConnection(CONNECTION_SQLSERVER))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        if (parameters != null && parameters.Length > 0)
                        {
                            foreach (SqlParameter param in parameters)
                            {
                                if ((param.Direction == ParameterDirection.InputOutput || param.Direction == ParameterDirection.Input) && param.Value == null)
                                    param.Value = DBNull.Value;
                                cmd.Parameters.Add(param);
                            }
                        }
                        // cmd.Parameters.AddRange(parameters);
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr != null)
                            {
                                while (dr.Read())
                                {
                                    T obj = ExecuteDataReader<T>(dr);
                                    if (obj != null)
                                    {
                                        lists.Add(obj);
                                    }
                                }
                                dr.Close();
                            }
                        }
                    }
                }
            }
            catch
            {

            }

            return lists;
        }


        /// <summary>
        /// 反射实体类集合xls
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        //public static List<SERIALMEMBERliveModel> ReadExcelToTable(string conns)
        //{
        //    List<SERIALMEMBERliveModel> SerialmemberpassList = new List<SERIALMEMBERliveModel>();
        //    DataSet set = new DataSet();
        //    try
        //    {
        //        using (OleDbConnection conn = new OleDbConnection(conns))
        //        {
        //            conn.Open();
        //            DataTable sheetsName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" });  //得到所有sheet的名字    
        //            string firstSheetName = sheetsName.Rows[0][2].ToString();   //得到第一个sheet的名字    
        //            string sql = string.Format("SELECT * FROM [{0}]", firstSheetName);  //查询字符串     
        //            OleDbDataAdapter ada = new OleDbDataAdapter(sql, conns);

        //            ada.Fill(set);
        //            set.Tables[0].Columns[0].ColumnName = "KSN";
        //            set.Tables[0].Columns[1].ColumnName = "MEMBERTBPlatID";
        //            set.Tables[0].Columns[2].ColumnName = "SERAILNO";
        //            set.Tables[0].Columns[3].ColumnName = "SERAILPASS";
        //            SerialmemberpassList = DataSetToIList<SERIALMEMBERliveModel>(set, 0);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        StreamWriter sw;
        //        if (File.Exists("D://smserror.txt"))
        //        {
        //            sw = File.AppendText("D://smserror.txt");//如果文件存在.则追加到文件  
        //        }
        //        else
        //        {
        //            sw = File.CreateText("D://smserror.txt");//如果文件不存在.则新建   
        //        }

        //        sw.WriteLine(ex.Message);
        //        sw.Close();
        //        throw;
        //    }

        //    return SerialmemberpassList;
        //}


        /// <summary>
        /// 反射实体类集合
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static List<T> GetList<T>(string sql, CommandType type = CommandType.Text, params SqlParameter[] parameters)
        {
            List<T> lists = new List<T>();
            try
            {
                using (SqlConnection con = new SqlConnection(CONNECTION_SQLSERVER))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        if (parameters != null && parameters.Length > 0)
                        {
                            foreach (SqlParameter param in parameters)
                            {
                                if ((param.Direction == ParameterDirection.InputOutput || param.Direction == ParameterDirection.Input) && param.Value == null)
                                    param.Value = DBNull.Value;
                                cmd.Parameters.Add(param);
                            }
                        }
                        //cmd.Parameters.AddRange(parameters);
                        cmd.CommandType = type;
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr != null)
                            {
                                while (dr.Read())
                                {
                                    T obj = ExecuteDataReader<T>(dr);
                                    if (obj != null)
                                    {
                                        lists.Add(obj);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {


            }

            return lists;
        }
        /// <summary>
        /// 反射实体类集合
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static List<T> GetList<T>(string sql, List<SqlParameter> parameters, CommandType type = CommandType.Text)
        {
            List<T> lists = new List<T>();
            try
            {
                using (SqlConnection con = new SqlConnection(CONNECTION_SQLSERVER))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.CommandType = type;
                        if (parameters != null && parameters.Count > 0)
                        {
                            foreach (SqlParameter param in parameters)
                            {
                                if ((param.Direction == ParameterDirection.InputOutput || param.Direction == ParameterDirection.Input) && param.Value == null)
                                    param.Value = DBNull.Value;
                                cmd.Parameters.Add(param);
                            }
                        }

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr != null)
                            {
                                while (dr.Read())
                                {
                                    T obj = ExecuteDataReader<T>(dr);
                                    if (obj != null)
                                    {
                                        lists.Add(obj);
                                    }
                                }
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch
            {

            }
            return lists;
        }
        /// <summary>
        /// 反射实体类
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static T GetModel<T>(string sql, List<SqlParameter> parameters)
        {
            T obj = default(T);
            try
            {
                using (SqlConnection con = new SqlConnection(CONNECTION_SQLSERVER))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        if (parameters != null && parameters.Count > 0)
                        {
                            foreach (SqlParameter param in parameters)
                            {
                                if ((param.Direction == ParameterDirection.InputOutput || param.Direction == ParameterDirection.Input) && param.Value == null)
                                    param.Value = DBNull.Value;
                                cmd.Parameters.Add(param);
                            }
                        }
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    obj = ExecuteDataReader<T>(dr);
                                    break;
                                }
                                dr.Close();
                            }
                        }
                    }
                }
            }
            catch
            {

            }
            return obj;
        }
        /// <summary>
        /// 反射实体类
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static T GetModel<T>(string sql, params SqlParameter[] parameters)
        {
            T obj = default(T);
            try
            {
                using (SqlConnection con = new SqlConnection(CONNECTION_SQLSERVER))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        if (parameters != null && parameters.Length > 0)
                        {
                            foreach (SqlParameter param in parameters)
                            {
                                if ((param.Direction == ParameterDirection.InputOutput || param.Direction == ParameterDirection.Input) && param.Value == null)
                                    param.Value = DBNull.Value;
                                cmd.Parameters.Add(param);
                            }
                        }
                        //cmd.Parameters.AddRange(parameters);
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    obj = ExecuteDataReader<T>(dr);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch
            {


            }
            return obj;
        }


        /// <summary>
        /// 批量添加数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="models">实体类集合</param>
        /// <param name="tableName">表名  </param>
        /// <returns></returns>
        public static bool ExecuteSqlBulkCopy<T>(List<T> models, string tableName, params string[] propertyName)
        {
            bool IsSuccess = true;
            try
            {
                DataTable dt = ToDataTable(models, propertyName);
                using (SqlConnection con = new SqlConnection(CONNECTION_SQLSERVER))
                {
                    con.Open();
                    SqlBulkCopy bulk = new SqlBulkCopy(con);
                    bulk.DestinationTableName = tableName;
                    bulk.BatchSize = dt.Rows.Count;
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        bulk.WriteToServer(dt);
                    }
                    bulk.Close();
                }
            }
            catch
            {


                IsSuccess = false;
            }
            return IsSuccess;

        }

        /// <summary>
        /// List集合转DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(IEnumerable<T> collection, params string[] propertyName)
        {
            List<string> propertyNameList = new List<string>();
            if (propertyName != null)
            {
                propertyNameList.AddRange(propertyName);
            }
            var props = typeof(T).GetProperties();
            var dt = new DataTable();
            for (int i = 0; i < props.Length; i++)
            {
                PropertyInfo property = props[i];
                if (property.Name.IndexOf("_") == 0)
                {
                    continue;
                }
                if (propertyNameList.Count == 0)
                {
                    dt.Columns.Add(property.Name, property.PropertyType);
                }
                else
                {
                    if (propertyNameList.Contains(property.Name))
                        dt.Columns.Add(property.Name, property.PropertyType);
                }
            }
            object[] values = new object[dt.Columns.Count];
            if (collection.Count() > 0)
            {
                for (int i = 0; i < collection.Count(); i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in props)
                    {
                        if (pi.Name.IndexOf("_") == 0) { continue; }
                        if (propertyNameList.Count == 0)
                        {
                            object obj = pi.GetValue(collection.ElementAt(i), null);
                            tempList.Add(obj);
                        }
                        else if (propertyNameList.Contains(pi.Name))
                        {
                            object obj = pi.GetValue(collection.ElementAt(i), null);
                            tempList.Add(obj);
                        }

                    }
                    object[] array = tempList.ToArray();
                    dt.LoadDataRow(array, true);
                }
            }
            return dt;
        }


        /// <summary> 
        /// DataSet装换为泛型集合 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="p_DataSet">DataSet</param> 
        /// <param name="p_TableIndex">待转换数据表索引</param> 
        /// <returns></returns> 
        /// 2008-08-01 22:46 HPDV2806 
        public static List<T> DataSetToIList<T>(DataSet p_DataSet, int p_TableIndex)
        {
            if (p_DataSet == null || p_DataSet.Tables.Count < 0)
                return null;
            if (p_TableIndex > p_DataSet.Tables.Count - 1)
                return null;
            if (p_TableIndex < 0)
                p_TableIndex = 0;

            DataTable p_Data = p_DataSet.Tables[p_TableIndex];
            // 返回值初始化 
            List<T> result = new List<T>();
            for (int j = 0; j < p_Data.Rows.Count; j++)
            {
                T _t = (T)Activator.CreateInstance(typeof(T));
                PropertyInfo[] propertys = _t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    for (int i = 0; i < p_Data.Columns.Count; i++)
                    {
                        // 属性与字段名称一致的进行赋值 
                        if (pi.Name.Equals(p_Data.Columns[i].ColumnName))
                        {
                            // 数据库NULL值单独处理 
                            if (p_Data.Rows[j][i] != DBNull.Value)
                                pi.SetValue(_t, p_Data.Rows[j][i].ToString(), null);
                            else
                                pi.SetValue(_t, null, null);
                            break;
                        }
                    }
                }
                result.Add(_t);
            }
            return result;
        }


    }
}
