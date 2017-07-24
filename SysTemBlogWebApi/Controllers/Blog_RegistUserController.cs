using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SysTemBlogWebApi.App_Start;
using SysTemModel.ResultModel;
using System.IO;
using SysTemCommHelper;
using SysTemModel;
using SysTemDAL;

namespace SysTemBlogWebApi.Controllers
{
    /// <summary>
    /// 注册接口
    /// </summary>
    public class Blog_RegistUserController : BaseController
    {
        /// <summary>
        /// 博客用户注册
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ResultBase BlogUserReg()
        {
            ResultBase model = new ResultBase();
            string NickName = RequestContent["NickName"] ?? "", Password = RequestContent["Password"] ?? "", Gender = RequestContent["Gender"] ?? "-1", BirthDay = RequestContent["BirthDay"] ?? null;
            if (!string.IsNullOrEmpty(NickName) && !Gender.Equals("-1") && !string.IsNullOrEmpty(BirthDay) && !string.IsNullOrEmpty(Password))
            {
                #region 注册
                try
                {
                    if (B_UserInfo_Provider.GetUserInfoList(NickName).Count <= 0)
                    {
                        //如果有附件
                        if (RequestContent.Files.Count > 0)
                        {
                            var file = RequestContent.Files[0];
                            int length = file.ContentLength;
                            if (length > 1145728)
                            {
                                model.Msg = "It should be smaller than 1 MB";
                                model.Result = ResultCode.ClientError;
                            }
                            else
                            {
                                string filename = DateTime.Now.ToString("yyyyMMddHHmmsss") + Path.GetExtension(file.FileName);
                                if (SysBlogCommonProvider.PhoneUserSave(file, UpLoadImageType.UserHead, filename))
                                {
                                    B_UserInfo UserInfoModel = new B_UserInfo() { BirthDay = DateTime.Parse(BirthDay), Gender = int.Parse(Gender), NickName = NickName, Photo = filename, Password = SysBlogCommonProvider.CalculateMd5Hash(Password) };
                                    if (B_UserInfo_Provider.Add(UserInfoModel))
                                    {
                                        model.Msg = "注册成功";
                                        model.Result = ResultCode.Ok;
                                    }
                                    else
                                    {
                                        model.Msg = "注册失败";
                                        model.Result = ResultCode.ServerError;
                                    }
                                }
                                else
                                {
                                    model.Msg = "图片格式不正确或上传失败";
                                    model.Result = ResultCode.ClientError;
                                }
                            }
                        }
                        else
                        {
                            model.Msg = "请上传用户头像";
                            model.Result = ResultCode.ClientError;
                        }
                    }
                    else
                    {
                        model.Msg = "用户已存在";
                        model.Result = ResultCode.ServerError;
                    }
                }
                catch (Exception ex)
                {
                    model.Msg = "服务器异常" + ex.ToString();
                    model.Result = ResultCode.ServerError;
                }
                #endregion
            }
            else
            {
                model.Msg = "客户参数不正确";
                model.Result = ResultCode.ClientError;
            }
            return model;
        }
    }
}