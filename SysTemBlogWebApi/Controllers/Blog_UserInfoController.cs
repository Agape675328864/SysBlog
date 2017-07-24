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
    /// 
    /// </summary>
    [BlogFiter]
    public class Blog_UserInfoController : BaseController
    {
        // GET: GetUserInfo
        /// <summary>
        /// 获取登录用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public UserInfo GetUserInfo()
        {
            UserInfo model = new UserInfo();
            List<B_UserInfo> UserInfoList = B_UserInfo_Provider.GetUserInfoListById(B_UserInfoBase.Id.ToString());
            if (UserInfoList.Count > 0)
            {
                B_UserInfo UserInfoModel = UserInfoList.SingleOrDefault();
                model.Msg = "获取成功";
                model.Result = ResultCode.Ok;
                model.Id = Des.string_Encrypt(UserInfoModel.Id.ToString());
                model.Gender = UserInfoModel.Gender.Equals(1) ? "男" : "女";
                model.BirthDay = UserInfoModel.BirthDay?.ToString("yyyy-MM-dd");
                model.NickName = UserInfoModel.NickName;
                model.Photo = Consts.UserPhotoPath + UserInfoModel.Photo;
            }
            else
            {
                model.Msg = "获取失败";
                model.Result = ResultCode.ServerError;
            }
            return model;
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ResultBase UpdateUserInfo()
        {
            ResultBase model = new ResultBase();
            string Gender = RequestContent["Gender"];
            string BirthDay = RequestContent["BirthDay"];
            string Photo = "";
            //修改用户信息是否有头像
            try
            {
                if (RequestContent.Files.Count > 0)
                {
                    var file = RequestContent.Files[0];
                    int length = file.ContentLength;
                    if (length > 1145728)
                    {
                        model.Msg = "It should be smaller than 1 MB";
                        model.Result = ResultCode.ClientError;
                        return model;
                    }
                    else
                    {
                        string filename = DateTime.Now.ToString("yyyyMMddHHmmsss") + Path.GetExtension(file.FileName);
                        if (SysBlogCommonProvider.PhoneUserSave(file, UpLoadImageType.UserHead, filename))
                        {
                            Photo = filename;
                        }
                    }
                }
                List<B_UserInfo> UserInfoList = B_UserInfo_Provider.GetUserInfoListById(B_UserInfoBase.Id.ToString());
                if (UserInfoList.Count > 0)
                {
                    if (!string.IsNullOrEmpty(Photo))
                    {
                        UserInfoList[0].Photo = Photo;
                    }
                    if (!string.IsNullOrEmpty(Gender))
                    {
                        UserInfoList[0].Gender = int.Parse(Gender);
                    }
                    if (!string.IsNullOrEmpty(BirthDay))
                    {
                        UserInfoList[0].BirthDay = DateTime.Parse(BirthDay);
                    }
                    B_UserInfo B_UserInfoModel = new B_UserInfo() { Id = UserInfoList.SingleOrDefault().Id, Gender = UserInfoList[0].Gender, BirthDay = UserInfoList[0].BirthDay, Photo = UserInfoList[0].Photo };
                    if (B_UserInfo_Provider.UpdateUserInfo(B_UserInfoModel))
                    {
                        model.Msg = "修改成功";
                        model.Result = ResultCode.Ok;
                    }
                    else
                    {
                        model.Msg = "修改失败";
                        model.Result = ResultCode.ServerError;
                    }
                }
                else
                {
                    model.Msg = "It should be smaller than 1 MB";
                    model.Result = ResultCode.ClientError;
                }
            }
            catch (Exception ex)
            {
                model.Msg = "系统错误" + ex.ToString();
                model.Result = ResultCode.ServerError;
            }

            return model;
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ResultBase ChangePwd()
        {
            ResultBase model = new ResultBase();
            string OldPwd = RequestContent["OldPwd"];
            string NewPwd = RequestContent["NewPwd"];
            try
            {
                if (!string.IsNullOrEmpty(OldPwd) && !string.IsNullOrEmpty(NewPwd))
                {
                    int m = B_UserInfo_Provider.ChangerPwd(B_UserInfoBase.Id.ToString(), SysBlogCommonProvider.CalculateMd5Hash(OldPwd), SysBlogCommonProvider.CalculateMd5Hash(NewPwd));
                    if (m.Equals(1))
                    {
                        model.Msg = "修改密码成功";
                        model.Result = ResultCode.Ok;
                    }
                    else if (m.Equals(2))
                    {
                        model.Msg = "修改密码失败";
                        model.Result = ResultCode.ServerError;
                    }
                    else
                    {
                        model.Msg = "旧密码不正确";
                        model.Result = ResultCode.ClientError;
                    }
                }
                else
                {
                    model.Msg = "参数错误";
                    model.Result = ResultCode.ClientError;
                }
            }
            catch (Exception ex)
            {

                model.Msg = "系统错误" + ex.ToString();
                model.Result = ResultCode.ServerError;
            }

            return model;
        }
    }
}
