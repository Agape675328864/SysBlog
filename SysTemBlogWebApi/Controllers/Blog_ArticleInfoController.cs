using Jusfoun.YunCompany.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SysTemBlogWebApi.App_Start;
using SysTemBlogWebApi.Filter;
using SysTemCommHelper;
using SysTemDAL;
using SysTemModel;
using SysTemModel.ResultModel;

namespace SysTemBlogWebApi.Controllers
{
    /// <summary>
    /// 发帖文章管理
    /// </summary>

    public class Blog_ArticleInfoController : BaseController
    {

        /// <summary>
        /// 用户发帖
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [BlogFiter]
        public ResultBase BlogArticleAdd()
        {
            ResultBase model = new ResultBase();
            string Title = RequestContent["Title"] ?? "", Content = RequestContent["Content"] ?? "", TypeName = RequestContent["TypeName"] ?? "", TypeId = RequestContent["TypeId"] ?? "0";
            if (string.IsNullOrEmpty(Title) && !string.IsNullOrEmpty(Content) && !string.IsNullOrEmpty(TypeName))
            {
                List<B_UserInfo> ListUserInfo = B_UserInfo_Provider.GetUserInfoListById(B_UserInfoBase.Id.ToString());
                if (ListUserInfo.Count > 0)
                {
                    try
                    {
                        string GuidPath = "";
                        //如果有附件
                        if (RequestContent.Files.Count > 0)
                        {
                            var file = RequestContent.Files[0];
                            int length = file.ContentLength;
                            if (length > 3145728)
                            {
                                model.Msg = "It should be smaller than 3 MB";
                                model.Result = ResultCode.ClientError;
                            }
                            string filename = DateTime.Now.ToString("yyyyMMddHHmmsss") + Path.GetExtension(file.FileName);
                            GuidPath = Guid.NewGuid().ToString("N");
                            if (SysBlogCommonProvider.ArticleImgSave(file, UpLoadImageType.ArticleImg, filename, GuidPath))
                            {
                                GuidPath += "/" + filename;
                            }
                        }

                        B_Article ArticleModel = new B_Article() { UserId = ListUserInfo.SingleOrDefault().Id, Title = Title, Content = Content, Picture = GuidPath };
                        if (B_Article_Provider.Add(ArticleModel))
                        {
                            model.Msg = "发帖成功";
                            model.Result = ResultCode.Ok;
                        }
                        else
                        {
                            model.Msg = "发帖失败";
                            model.Result = ResultCode.ServerError;
                        }

                    }
                    catch (Exception ex)
                    {
                        model.Msg = "系统错误";
                        model.Result = ResultCode.ServerError;
                    }
                }
                else
                {
                    model.Msg = "用户不存在";
                    model.Result = ResultCode.ClientError;
                }
            }
            else
            {
                model.Msg = "参数错误";
                model.Result = ResultCode.ClientError;
            }
            return model;
        }
        /// <summary>
        /// 获取发帖列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ListResultBase GetArticleListInfo()
        {

            ListResultBase ArticleRootModel = new ListResultBase();
            string PageIndex = RequestContent["PageIndex"] ?? "1";
            string PageSize = RequestContent["PageSize"] ?? "10";
            string UserId = RequestContent["Id"] ?? "0";
            string fiter = " State=0";
            ArticleInfo Model = new ArticleInfo();
            try
            {
                int PIndex = (Tool.ToInt(PageSize) * (Tool.ToInt(PageIndex) - 1) + 1);
                int PSize = Tool.ToInt(PageIndex) * Tool.ToInt(PageSize);
                Tuple<List<B_Article>, int> ListArticle = B_Article_Provider.GetArticleList(PIndex, PSize, fiter);
                var VarArticle = from a in ListArticle.Item1
                                 select a.Id;
                string ArticleIds = string.Join(",", VarArticle.ToArray());
                var VarUser = from a in ListArticle.Item1
                              select a.UserId;
                string UserIds = string.Join(",", VarUser.ToArray());
                List<B_RepliesArticle> RList = B_Article_Provider.RepliesArticleListByArticleid(ArticleIds);
                List<B_UserInfo> UserInfoList = B_UserInfo_Provider.GetUserInfoListUserIds(UserIds);
                var result = from a in ListArticle.Item1
                             join b in UserInfoList on a.UserId equals b.Id into bb
                             from c in bb.DefaultIfEmpty()
                             select new
                             {
                                 a.Id,
                                 a.Title,
                                 a.Content,
                                 CreateTime = a.CreateTime?.ToString("yyyy-MM-dd HH:mm:ss"),
                                 a.Check,
                                 c.NickName,
                                 TypeName = a.TypeName,
                                 Photo = Consts.UserPhotoPath + c.Photo,
                                 RepliesCount = RList.FindAll(s => s.ArticleId.Equals(a.Id)) == null ? 0 : RList.FindAll(s => s.ArticleId.Equals(a.Id)).Count,

                             };
                List<ArticleInfo> B_UserInfoJsonList = JsonConvert.DeserializeObject<List<ArticleInfo>>(JsonConvert.SerializeObject(result));
                ArticleRootModel.Count = ListArticle.Item2;
                ArticleRootModel.ListData = B_UserInfoJsonList;
                ArticleRootModel.Result = ResultCode.Ok;
                ArticleRootModel.Msg = "√";
            }
            catch (Exception ex)
            {
                ArticleRootModel.Count = 0;
                ArticleRootModel.ListData = "";
                ArticleRootModel.Result = ResultCode.ServerError;
                ArticleRootModel.Msg = "异常";
            }
            return ArticleRootModel;
        }

        /// <summary>
        /// 根据文章id获取信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ListResultBase GetArticleModelInfo()
        {
            string Id = RequestContent["Id"] ?? "0";
            ListResultBase ArticleRootModel = new ListResultBase();
            try
            {
                List<B_Article> list = B_Article_Provider.ArticleList(Tool.ToInt(Id));
                var VarArticle = from a in list
                                 select a.Id;
                string ArticleIds = string.Join(",", VarArticle.ToArray());
                var VarUser = from a in list
                              select a.UserId;
                string UserIds = string.Join(",", VarUser.ToArray());
                List<B_RepliesArticle> RList = B_Article_Provider.RepliesArticleListByArticleid(ArticleIds);
                List<B_UserInfo> UserInfoList = B_UserInfo_Provider.GetUserInfoListUserIds(UserIds);
                var result = from a in list
                             join b in UserInfoList on a.UserId equals b.Id into bb
                             from c in bb.DefaultIfEmpty()
                             select new
                             {
                                 a.Id,
                                 a.Title,
                                 a.Content,
                                 CreateTime = a.CreateTime?.ToString("yyyy-MM-dd HH:mm:ss"),
                                 a.Check,
                                 c.NickName,
                                 TypeName = a.TypeName,
                                 Photo = Consts.UserPhotoPath + c.Photo,
                                 RepliesCount = RList.FindAll(s => s.ArticleId.Equals(a.Id)) == null ? 0 : RList.FindAll(s => s.ArticleId.Equals(a.Id)).Count
                             };
                List<ArticleInfo> B_UserInfoJsonList = JsonConvert.DeserializeObject<List<ArticleInfo>>(JsonConvert.SerializeObject(result));
                ArticleRootModel.Count = 0;
                ArticleRootModel.ListData = B_UserInfoJsonList.FirstOrDefault();
                ArticleRootModel.Result = ResultCode.Ok;
                ArticleRootModel.Msg = "√";
            }
            catch (Exception ex)
            {
                ArticleRootModel.Count = 0;
                ArticleRootModel.ListData = "";
                ArticleRootModel.Result = ResultCode.ServerError;
                ArticleRootModel.Msg = "异常";
            }
            return ArticleRootModel;
        }
        /// <summary>
        /// 更新主题浏览次数
        /// </summary>
        /// <returns></returns>
        public ArticleBrowseTimes ArticleCheckeds()
        {
            ArticleBrowseTimes ArticleBrowseTimes = new ArticleBrowseTimes();
            string Id = RequestContent["Id"] ?? "0";
            ResultBase model = new ResultBase();
            try
            {
                ArticleBrowseTimes.TotalCount = B_Article_Provider.UpdateAritcleCheckTimes(int.Parse(Id));
                ArticleBrowseTimes.Result = ResultCode.Ok;
                ArticleBrowseTimes.Msg = "成功";
                //json = JsonConvert.SerializeObject(new
                //{
                //    count = B_Article_Provider.UpdateAritcleCheckTimes(int.Parse(Id)),
                //    data = model
                //});
            }
            catch (Exception)
            {
                ArticleBrowseTimes.TotalCount = 0;
                ArticleBrowseTimes.Result = ResultCode.ServerError;
                ArticleBrowseTimes.Msg = "失败";
            }
            return ArticleBrowseTimes;
        }
    }
}
