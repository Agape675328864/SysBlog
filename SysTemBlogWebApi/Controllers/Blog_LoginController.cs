using Blog.ComComment.Common;
using Jusfoun.YunCompany.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using SysTemBlogWebApi.App_Start;
using SysTemCommHelper;
using SysTemDAL;
using SysTemModel;
using SysTemModel.ResultModel;
using Zane.Common.WebBrowser;

namespace SysTemBlogWebApi.Controllers
{
    /// <summary>
    /// 登录
    /// </summary>
    public class Blog_LoginController : BaseController
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public UserLogin Login()
        {
            UserLogin model = new UserLogin();
            try
            {
                string NickName = RequestContent["NickName"] ?? "";
                string Password = RequestContent["Password"] ?? "";
                if (!string.IsNullOrEmpty(NickName) && !string.IsNullOrEmpty(Password))
                {
                    List<B_UserInfo> UserInfoList = B_UserInfo_Provider.LoginUserInfo(NickName, SysBlogCommonProvider.CalculateMd5Hash(Password));
                    if (UserInfoList.Count > 0)
                    {
                        B_UserInfo UserInfoModel = UserInfoList.SingleOrDefault();
                        CookieHelper.Set(Des.string_Encrypt(UserInfoModel.Id.ToString()), Des.string_Encrypt(UserInfoModel.Id.ToString()), DateTime.Now.AddDays(1));
                        model.Id = Des.string_Encrypt(UserInfoModel.Id.ToString());
                        model.Msg = "登录成功";
                        model.Result = ResultCode.Ok;
                    }
                    else
                    {
                        model.Msg = "用户名或密码不正确";
                        model.Result = ResultCode.ServerError;
                    }
                }
                else
                {
                    model.Msg = "用户名或密码不能为空";
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
        /// 退出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ResultBase Out()
        {
            ResultBase model = new ResultBase();
            string Id = RequestContent["Id"] ?? "";
            if (!string.IsNullOrEmpty(Id))
            {
                try
                {
                    CookieHelper.Delete(Id);
                    model.Result = ResultCode.Ok;
                    model.Msg = "退出成功";
                }
                catch (Exception ex)
                {
                    model.Msg = "系统错误";
                    model.Result = ResultCode.ServerError;
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
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ResultBase SmsMsg()
        {
            string TemplateCode = RequestContent["TemplateCode"];//短信模版编号
            string TemplateParam = RequestContent["TemplateParam"];//短信内容 //"{\"customer\":\"王丽红\"}"
            string content = "{\"customer\":\""+ TemplateParam + "\"}";
            string PhoneNumbers = RequestContent["PhoneNumbers"];//手机号
            ResultBase model = new ResultBase();
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("AccessKeyId", AliYn.GetHttpClientJson("AccessKeyId"));
            sParaTemp.Add("Action", "SendSms");
            sParaTemp.Add("Format", "JSON");
            sParaTemp.Add("TemplateParam", content);
            sParaTemp.Add("PhoneNumbers", PhoneNumbers);
            sParaTemp.Add("SignName", "GoGoTalk青少外教");
            sParaTemp.Add("SignatureMethod", "HMAC-SHA1");
            sParaTemp.Add("SignatureNonce", Guid.NewGuid().ToString());
            sParaTemp.Add("SignatureVersion", "1.0");
            sParaTemp.Add("TemplateCode", TemplateCode);
            sParaTemp.Add("Timestamp", AliYn.FormatISO8601Date());
            sParaTemp.Add("Version", AliYn.GetHttpClientJson("Version"));
            sParaTemp.Add("RegionId", "cn-hangzhou");
            //开始生成Signature
            string[] array = sParaTemp.OrderBy(a => a.Key, StringComparer.Ordinal).Select(a => AliYn.PercentEncode(a.Key) + "=" + AliYn.PercentEncode(a.Value.ToString())).ToArray();
            string dataStr = string.Join("&", array);
            string signStr = "GET" + "&" + AliYn.PercentEncode("/") + "&" + AliYn.PercentEncode(dataStr);
            HMACSHA1 myhmacsha1 = new HMACSHA1(Encoding.UTF8.GetBytes(AliYn.GetHttpClientJson("AccessKeySecret") + "&"));
            byte[] byteArray = Encoding.UTF8.GetBytes(signStr);
            MemoryStream stream = new MemoryStream(byteArray);
            string signature = Convert.ToBase64String(myhmacsha1.ComputeHash(stream));
            signature = signature.Replace("=", "%3D").Replace("+", "%20");
            string StringToSign = "Signature=" + signature + "&" + AliYn.BuildRequest(sParaTemp);
            int nLen = StringToSign.Length;
            StringToSign = StringToSign.Remove(nLen - 1, 1);
            JObject SourcePage = (JObject)JsonConvert.DeserializeObject(model.Msg = AliYn.HttpGetAlidy(StringToSign));
            if (SourcePage["Message"].ToString().Equals("OK") && SourcePage["Code"].ToString().Equals("OK"))
            {
                model.Msg = SourcePage["Message"].ToString();
                model.Result = ResultCode.Ok;
            }
            else
            {
                model.Msg = SourcePage["Message"].ToString();
                model.Result = ResultCode.ServerError;
            }
            return model;
        }



    }
}