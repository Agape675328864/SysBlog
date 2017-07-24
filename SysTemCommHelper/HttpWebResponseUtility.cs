using Agape.Xml.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Blog.ComComment.Common
{
    public class HttpWebResponseUtility
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetUrl"></param>
        /// <param name="PostData">each keyvalue pair should be splited by "&"</param>
        /// <returns></returns>
        public static string PostDataToServer(string targetUrl, string postData, Encoding inputEncoding = null, Encoding outPutEncoding = null)
        {
            if (string.IsNullOrEmpty(targetUrl))
            {
                throw new ArgumentNullException("targetUrl");
            }

            if (inputEncoding == null)
            {
                inputEncoding = Encoding.UTF8;
            }

            if (outPutEncoding == null)
            {
                outPutEncoding = Encoding.UTF8;
            }

            byte[] postDataBytes = null;
            if (string.IsNullOrEmpty(postData))
            {
                postDataBytes = new byte[] { };
            }
            else
            {
                postDataBytes = inputEncoding.GetBytes(postData);
            }

            WebClient client = new WebClient();
            client.Encoding = inputEncoding;
            client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            byte[] result = client.UploadData(targetUrl, "POST", postDataBytes);
            string outputString = outPutEncoding.GetString(result);
            return outputString;
        }
        /// <summary>  
        /// GET请求与获取结果  
        /// </summary>  
        public static string HttpGet(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

        /// <summary>
        ///  Http同步Post同步请求
        /// </summary>
        /// <param name="url">Url地址</param>
        /// <param name="postStr">请求Url数据</param>
        /// <param name="encode">编码(默认UTF8)</param>
        /// <returns></returns>
        public static string HttpPost(string url, string postStr = "", Encoding encode = null)
        {
            string result;

            try
            {
                var webClient = new WebClient { Encoding = Encoding.UTF8 };

                if (encode != null)
                    webClient.Encoding = encode;

                var sendData = Encoding.GetEncoding("GB2312").GetBytes(postStr);

                webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                webClient.Headers.Add("ContentLength", sendData.Length.ToString(CultureInfo.InvariantCulture));

                var readData = webClient.UploadData(url, "POST", sendData);

                result = Encoding.GetEncoding("GB2312").GetString(readData);

            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 苹果订单查询
        /// </summary>
        /// <param name="JSONData">订单json数据</param>
        /// <param name="Url">地址</param>
        /// <returns></returns>
        public static string PostJsonGetStr(string JSONData, string Url)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(JSONData);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentLength = bytes.Length;
            request.ContentType = "application/json";
            Stream reqstream = request.GetRequestStream();
            reqstream.Write(bytes, 0, bytes.Length);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream streamReceive = response.GetResponseStream();
            Encoding encoding = Encoding.UTF8;

            StreamReader streamReader = new StreamReader(streamReceive, encoding);
            string strResult = streamReader.ReadToEnd();
            streamReceive.Dispose();
            streamReader.Dispose();

            return strResult;
        }


        public static string DataSearchJson(string data, string url)
        {
            using (var client = GetHttpClientJson())
            {
                var list = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("data", data)
                };
                var form = new FormUrlEncodedContent(list);
                var result = client.PostAsync(url, form).Result;

                var content = result.Content.ReadAsStringAsync().Result;
                return content;
            }
        }

        protected static HttpClient GetHttpClientJson()
        {
            string url = XmlHelper.GetSNSTemplate("SmsUrl", string.Format("{0}/Xml/app.xml", System.AppDomain.CurrentDomain.BaseDirectory));
            return new HttpClient { BaseAddress = new Uri(url) };
        }

    }
}
