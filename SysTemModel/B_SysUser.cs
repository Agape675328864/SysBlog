/*----------------------------------------------------------------
 * Copyright (C) 2017 雨林小筑版权所有。 
 * 文件名：B_SysUser
 * 文件功能描述：后台系统用户信息

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

namespace SysTemModel
{
    public class B_SysUser
    {
        public int Id { get; set; }
        public string SysName { get; set; }
        public string Password { get; set; }
    }
}
