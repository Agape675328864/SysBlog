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
        /// <summary>
        /// 回复 0：显示 1：不显示
        /// </summary>
        public int State { get; set; }
    }

    public class RepliesArticleTemp : B_RepliesArticle
    {
        public string Title { get; set; }
        public string NickName { get; set; }
    }
}
