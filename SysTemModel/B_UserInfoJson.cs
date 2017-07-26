using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysTemModel
{
    public class B_UserInfoJson
    {
        public int Id { get; set; }
        /// <summary>
        /// 昵称用户名
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string Photo { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public int Gender { get; set; }
        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime? BirthDay { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 发帖数量
        /// </summary>
        public int PostCount { get; set; }
        /// <summary>
        /// 回帖数量
        /// </summary>
        public int RepliesCount { get; set; }
    }
}
