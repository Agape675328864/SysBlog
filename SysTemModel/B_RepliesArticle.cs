using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysTemModel
{
    public class B_RepliesArticle
    {
        public int Id { get; set; }
        /// <summary>
        /// 回帖文章id
        /// </summary>
        public int ArticleId { get; set; }
        /// <summary>
        /// 回帖人
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 回帖内容
        /// </summary>
        public string Contents { get; set; }
    }
}
