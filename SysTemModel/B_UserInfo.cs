/*----------------------------------------------------------------
 * Copyright (C) 2017 雨林小筑版权所有。 
 * 文件名：B_UserInfo
 * 文件功能描述：用户信息表

 * 创建者：杨红彬 (Administrator)
 * 时间：2017/6/21 16:43:37
 * 修改人：
 * 时间：
 * 修改说明：
 * 版本：V1.0.0
----------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SysTemModel.ResultModel;

namespace SysTemModel
{
    public class B_UserInfo
    {
        public int Id { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string Photo { get; set; }
        /// <summary>
        ///性别 1:男 2:女 0 未知
        /// </summary>
        public int Gender { get; set; }
        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime? BirthDay { get; set; }
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 0:冻结 1:正常
        /// </summary>
        public int State { get; set; }
    }
    /// <summary>
    /// 登录成功返回Id
    /// </summary>
    public class UserLogin : ResultBase
    {
        public string Id { get; set; }
    }
    /// <summary>
    /// 获取用户信息实体
    /// </summary>
    public class UserInfo : ResultBase
    {
        public string Id { get; set; }
        public string NickName { get; set; }
        public string Photo { get; set; }
        public string Gender { get; set; }
        public string BirthDay { get; set; }
    }
}
