using Jusfoun.YunCompany.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using SysTemDAL;
using SysTemModel;

namespace SysTemBlogWebApi.App_Start
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        public B_UserInfo B_UserInfoBase
        {
            get
            {
                return GetUserInfoBase();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public B_UserInfo GetUserInfoBase()
        {
            B_UserInfo obj = new B_UserInfo();
            string token = Request.Headers.Authorization.ToString();
            List<B_UserInfo> list = B_UserInfo_Provider.GetUserInfoListById(Des.string_Decrypt(token));
            if (list.Count > 0)
            {
                obj = list.FirstOrDefault();
            }
            return obj;
        }
        private readonly HttpRequest _requestContent;
        /// <summary>
        /// 
        /// </summary>
        public BaseController()
        {
            _requestContent = HttpContext.Current.Request;
        }
        /// <summary>
        /// 
        /// </summary>
        public HttpRequest RequestContent
        {
            get { return _requestContent; }
        }
    }
}