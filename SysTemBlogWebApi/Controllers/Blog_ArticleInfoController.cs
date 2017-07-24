using Jusfoun.YunCompany.Common;
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
            string  Title = RequestContent["Title"] ?? "", Content = RequestContent["Content"] ?? "";
            if (string.IsNullOrEmpty(Title) && !string.IsNullOrEmpty(Content))
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
        public ArticleRoot GetArticleListInfo()
        {

            ArticleRoot ArticleRootModel = new ArticleRoot();
            ArticleRootModel.ArticleInfoList = new List<ArticleInfo>();
            ArticleRootModel.ResultBase = new ResultBase();
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
                if (ListArticle.Item1.Count > 0)
                {
                    foreach (var item in ListArticle.Item1)
                    {
                        ArticleInfo ArticleInfoModel = new ArticleInfo();
                        ArticleInfoModel.Id = item.Id;
                        ArticleInfoModel.Title = item.Title;
                        // ArticleInfoModel.Content = item.Content;
                        ArticleInfoModel.Picture = Consts.ArticlePath + item.Picture;
                        ArticleInfoModel.CreateTime = item.CreateTime?.ToString("yyyy-MM-dd HH:mm:ss");
                        ArticleInfoModel.Check = item.Check;
                        ArticleRootModel.ArticleInfoList.Add(ArticleInfoModel);
                    }
                    ArticleRootModel.Count = ListArticle.Item2;
                    ArticleRootModel.ResultBase.Msg = "获取成功";
                    ArticleRootModel.ResultBase.Result = ResultCode.Ok;
                }
                else
                {
                    ArticleInfo ArticleInfoModel = new ArticleInfo();
                    ArticleInfoModel.Id = 0;
                    ArticleInfoModel.Title = "";
                    // ArticleInfoModel.Content = "";
                    ArticleInfoModel.Picture = "";
                    ArticleRootModel.ArticleInfoList.Add(ArticleInfoModel);
                    ArticleRootModel.Count = 0;
                    ArticleRootModel.ResultBase.Msg = "获取成功";
                    ArticleRootModel.ResultBase.Result = ResultCode.Ok;
                }
            }
            catch (Exception ex)
            {
                ArticleRootModel.ResultBase.Msg = "获取失败";
                ArticleRootModel.ResultBase.Result = ResultCode.ServerError;
            }
            return ArticleRootModel;
        }

        /// <summary>
        /// 根据文章id获取信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ArticleModel GetArticleModelInfo()
        {
            string Id = RequestContent["Id"] ?? "0";
            ArticleModel Model = new ArticleModel();
            Model.B_Article = new B_Article();
            Model.ResultBase = new ResultBase();
            try
            {
                List<B_Article> list = B_Article_Provider.ArticleList(Tool.ToInt(Id));
                Model.ResultBase.Msg = "获取成功";
                Model.ResultBase.Result = ResultCode.Ok;
                if (list.Count > 0)
                {
                    Model.B_Article = list.FirstOrDefault();
                    Model.B_Article.Picture = Consts.ArticlePath + Model.B_Article.Picture;
                }
            }
            catch (Exception ex)
            {

                Model.ResultBase.Result = ResultCode.ServerError;
                Model.ResultBase.Msg = "失败异常";
            }
            return Model;
        }
    }
}
