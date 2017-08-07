/*----------------------------------------------------------------
 * Copyright (C) 2017 雨林小筑版权所有。 
 * 文件名：B_Article
 * 文件功能描述：发帖文章

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
    public class B_Article
    {
        public int Id { get; set; }
        /// <summary>
        /// 发帖人id
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 帖子标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 帖子内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 帖子图片
        /// </summary>
        public string Picture { get; set; }
        /// <summary>
        /// 发帖时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 帖子状态 0：显示 1：不显示
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 查看次数 热度
        /// </summary>
        public int Check { get; set; }
        /// <summary>
        /// 帖子类型名
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// 类型id
        /// </summary>
        public int TypeId { get; set; }
    }
    public class ArticleModel
    {
        public B_Article B_Article { get; set; }
        public ResultBase ResultBase { get; set; }
    }

    /// <summary>
    /// 获取列表
    /// </summary>
    public class ArticleRoot
    {
        public object ArticleInfoList { get; set; }
        public int Count { get; set; }
        public ResultBase ResultBase { get; set; }

    }

    public class ArticleInfo
    {
        public int Id { get; set; }
        /// <summary>
        /// 帖子标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 帖子内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 帖子图片
        /// </summary>
        //public string Picture { get; set; }
        /// <summary>
        /// 发帖时间
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 查看次数 热度（浏览次数）
        /// </summary>
        public int Check { get; set; }
        /// <summary>
        /// 评论数量
        /// </summary>
        public int RepliesCount { get; set; }
        /// <summary>
        /// 发帖用户
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 帖子类型名
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string Photo { get; set; }
    }
}
