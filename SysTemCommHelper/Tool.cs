using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.IO;

namespace Jusfoun.YunCompany.Common
{
    public class Tool
    {


        /**
        * 生成时间戳，标准北京时间，时区为东八区，自1970年1月1日 0点0分0秒以来的秒数
         * @return 时间戳
        */
        public static string GenerateTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /**
        * 生成随机串，随机串包含字母或数字
        * @return 随机串
        */
        public static string GenerateNonceStr()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        /// <summary>
        /// 获得签名
        /// </summary>
        /// <param name="strTimestamp">时间戳</param>
        /// <param name="strNonce">随机字符串</param>
        /// <param name="strAppToken">Token</param>
        /// <returns></returns>
        public static string ValidateSignatureNoReverse(string strTimestamp, string strNonce, string strAppToken)
        {
            //进行排序
            string[] arrTmp = { strAppToken, strTimestamp, strNonce };
            Array.Sort(arrTmp);
            string tmpStr = string.Join("", arrTmp);

            //MD5 32位加密
            tmpStr = GetMd5Hash(tmpStr);
            tmpStr = tmpStr.ToLower();

            return tmpStr;
        }

        //MD5加密
        public static string GetMd5Hash(string input)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.
            var md5Hasher = System.Security.Cryptography.MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            var data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data and format each one as a hexadecimal string.
            foreach (var item in data)
            {
                sBuilder.Append(item.ToString("X"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }



        private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        /// <summary>
        /// 转换字符串为日期时间类型，错误返回当前时间
        /// </summary>
        /// <param name="objDtValue"></param>
        /// <returns></returns>
        public static DateTime? Convert3DateTimenew(object objDtValue)
        {
            DateTime dtTime = DateTime.Now;
            if (objDtValue != null)
            {
                DateTime.TryParse(objDtValue.ToString(), out dtTime);
                if (dtTime == Convert.ToDateTime("0001/1/1 0:00:00"))
                {
                    dtTime = DateTime.ParseExact(objDtValue.ToString(), "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                }
            }
            if (IsEmpty(objDtValue.ToString()))
                return null;
            return dtTime;
        }


        public static DateTime? Convert3DateTime(object objDtValue)
        {
            DateTime dtTime = DateTime.Now;
            if (objDtValue != null)
            {
                DateTime.TryParse(objDtValue.ToString(), out dtTime);
            }
            if (IsEmpty(objDtValue.ToString()))
                return null;
            return dtTime;
        }

        public static DateTime Convert2DateTime(object objDtValue)
        {
            DateTime dtTime = DateTime.Now;
            if (objDtValue != null)
            {
                DateTime.TryParse(objDtValue.ToString(), out dtTime);
            }

            return dtTime;
        }

        /// <summary>
        /// 转换为数字,出错返回0
        /// </summary>
        /// <param name="objNum"></param>
        /// <returns></returns>
        public static int Convert2Int(object objNum)
        {
            int iValue = 0;
            if (objNum != null && objNum.ToString().Length > 0)
            {
                int.TryParse(objNum.ToString(), out iValue);
            }
            return iValue;
        }
        /// <summary>
        /// 转换为字符串
        /// </summary>
        /// <param name="objStr"></param>
        /// <returns></returns>
        public static string Convert2Str(object objStr)
        {
            string strReturn = "";
            if (objStr != null)
            {
                strReturn = objStr.ToString();
            }
            return strReturn;

        }

        /// <summary>
        /// 转换成int
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ToInt(string str)
        {

            try
            {
                if (string.IsNullOrEmpty(str))
                    return 0;
                else
                {

                    if (Convert.ToInt32(str).GetType().ToString() == "System.Int32")
                        return Convert.ToInt32(str);
                    else
                        return 0;
                }
            }
            catch
            {
                return 0;
            }

        }

        /// <summary>
        /// 转换成double
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static double ToDouble(object objNum)
        {

            try
            {
                double iValue = 0;
                if (objNum != null && objNum.ToString().Length > 0)
                {
                    double.TryParse(objNum.ToString(), out iValue);
                }
                return iValue;
            }
            catch
            {
                return 0;
            }

        }

        public static decimal ToDecimal(string str)
        {

            try
            {
                if (string.IsNullOrEmpty(str))
                    return 0;
                else
                {

                    if (Convert.ToDecimal(str).GetType().ToString() == "System.Decimal")
                        return Convert.ToDecimal(str);
                    else
                        return 0;
                }
            }
            catch
            {
                return 0;
            }

        }
        public static double Convert2Double(object objValue)
        {
            try
            {
                double dValue = 0;
                if (objValue != null)
                {
                    double.TryParse(objValue.ToString(), out dValue);
                }
                return dValue;
            }
            catch (Exception ex)
            {

                return 0;
            }
        }
        public static string InitNumberInt(object strVlue)
        {
            if (String.IsNullOrEmpty(Convert2Str(strVlue)) || Convert2Str(strVlue) == "0")
            {
                return "-";
            }
            string dsm = "-";
            try
            {
                long num = Convert.ToInt64(Convert2Double(strVlue));

                if (num.ToString().Length > 2)
                {
                    dsm = num.ToString().Substring(0, num.ToString().Length - 2) + "00";
                }
                else
                {
                    dsm = num.ToString();
                }
            }
            catch (Exception ex)
            {

                return "0";
            }

            return InitNumberStr(dsm);
        }
        /// <summary>
        /// 不会讲后两位数字 置为0
        /// </summary>
        /// <param name="strVlue"></param>
        /// <returns></returns>
        public static string InitNumberInt1(object strVlue)
        {
            if (String.IsNullOrEmpty(Convert2Str(strVlue)) || Convert2Str(strVlue) == "0")
            {
                return "-";
            }
            string dsm = "-";
            try
            {
                long num = Convert.ToInt64(Convert2Double(strVlue));

                //if (num.ToString().Length > 2)
                //{
                //    dsm = num.ToString().Substring(0, num.ToString().Length - 2) + "00";
                //}
                //else
                //{
                dsm = num.ToString();
                //}
            }
            catch (Exception ex)
            {

                return "0";
            }

            return InitNumberStr(dsm);
        }
        /// <summary>
        /// 初始化数字显示
        /// </summary>
        /// <param name="objIValue"></param>
        /// <returns></returns>
        public static string InitNumberStr(object objIValue)
        {
            try
            {
                double dValue = 0;
                if (objIValue != null)
                {
                    double.TryParse(objIValue.ToString(), out dValue);
                }
                return string.Format("{0:N0}", dValue);
            }
            catch (Exception ex)
            {

                return "";
            }
        }
        public static string GetValueFromWebConfig(string key)
        {
            string ret = "";
            try
            {
                ret = ConfigurationManager.AppSettings[key].ToString();
            }
            catch
            {
                ret = "";
            }
            return ret;
        }
        /// <summary>
        /// 判断字符创是否为空或者NULL
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsEmpty(string str)
        {
            if (string.IsNullOrEmpty(str))
                return true;
            else
                return false;
        }
        public static Dictionary<string, string> GetAttachPath()
        {
            string savePath = Tool.GetValueFromWebConfig("SavePath");
            string saveUrl = Tool.GetValueFromWebConfig("SaveUrl");
            if (String.IsNullOrEmpty(savePath)) savePath = "~/Files/";
            if (String.IsNullOrEmpty(saveUrl))
            {
                saveUrl = savePath.Substring(1);
            }
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("savePath", savePath);
            dic.Add("saveUrl", saveUrl);
            return dic;
        }
        /// <summary>
        /// MD5函数(加密方法)
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <returns>MD5结果</returns>
        public static string MD5(string str)
        {
            byte[] b = Encoding.Default.GetBytes(str);
            b = new MD5CryptoServiceProvider().ComputeHash(b);
            string ret = "";
            for (int i = 0; i < b.Length; i++)
                ret += b[i].ToString("x").PadLeft(2, '0');
            return ret;
        }
        public static string MD5ASCII(string str)
        {
            byte[] b = Encoding.ASCII.GetBytes(str);
            b = new MD5CryptoServiceProvider().ComputeHash(b);
            string ret = "";

            //for (int i = 0; i < b.Length; i++)
            //    ret += b[i].ToString("x").PadLeft(2, '0');
            return ret;
        }
        public static string MD5_Other(string str)
        {
            byte[] b = Encoding.UTF8.GetBytes(str);
            b = new MD5CryptoServiceProvider().ComputeHash(b);
            string ret = "";
            for (int i = 0; i < b.Length; i++)
                ret += b[i].ToString("x2");
            return ret;
        }
        /// <summary>
        /// 当我们想要获得一个唯一的key的时候，通常会想到GUID。这个key非常的长，虽然我们在很多情况下这并不是个问题。但是当我们需要将这个36个字符的字符串放在URL中时，会使的URL非常的丑陋。
        /// 想要缩短GUID的长度而不牺牲它的唯一性是不可能的，但是如果我们能够接受一个16位的字符串的话是可以做出这个牺牲的。
        /// 我们可以将一个标准的GUID 21726045-e8f7-4b09-abd8-4bcc926e9e28  转换成短的字符串 3c4ebc5f5f2c4edc 
        /// 下面的方法会生成一个短的字符串，并且这个字符串是唯一的。重复1亿次都不会出现重复的，它也是依照GUID的唯一性来生成这个字符串的
        /// </summary>
        /// <returns></returns>
        public static string GenerateStringID()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }
        /// <summary>
        /// string型转换为bool型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool StrToBool(object Expression, bool defValue)
        {
            if (Expression != null)
            {
                if (string.Compare(Expression.ToString(), "true", true) == 0)
                {
                    return true;
                }
                else if (string.Compare(Expression.ToString(), "false", true) == 0)
                {
                    return false;
                }
            }
            return defValue;
        }
        /// <summary>
        /// 获得当前绝对路径
        /// </summary>
        /// <param name="strPath">指定的路径</param>
        /// <returns>绝对路径</returns>
        public static string GetMapPath(string strPath)
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(strPath);
            }
            else //非web程序引用
            {
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="stringarray">字符串数组</param>
        /// <returns>判断结果</returns>
        public static bool InArray(string str, string[] stringarray)
        {
            return InArray(str, stringarray, false);
        }
        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="strSearch">字符串</param>
        /// <param name="stringArray">字符串数组</param>
        /// <param name="caseInsensetive">是否不区分大小写, true为不区分, false为区分</param>
        /// <returns>判断结果</returns>
        public static bool InArray(string strSearch, string[] stringArray, bool caseInsensetive)
        {
            return GetInArrayID(strSearch, stringArray, caseInsensetive) >= 0;
        }
        /// <summary>
        /// 判断指定字符串在指定字符串数组中的位置
        /// </summary>
        /// <param name="strSearch">字符串</param>
        /// <param name="stringArray">字符串数组</param>
        /// <param name="caseInsensetive">是否不区分大小写, true为不区分, false为区分</param>
        /// <returns>字符串在指定字符串数组中的位置, 如不存在则返回-1</returns>
        public static int GetInArrayID(string strSearch, string[] stringArray, bool caseInsensetive)
        {
            for (int i = 0; i < stringArray.Length; i++)
            {
                if (caseInsensetive)
                {
                    if (strSearch.ToLower() == stringArray[i].ToLower())
                    {
                        return i;
                    }
                }
                else
                {
                    if (strSearch == stringArray[i])
                    {
                        return i;
                    }
                }

            }
            return -1;
        }
        /// <summary>
        /// 从字符串的指定位置截取指定长度的子字符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="startIndex">子字符串的起始位置</param>
        /// <param name="length">子字符串的长度</param>
        /// <returns>子字符串</returns>
        public static string CutString(string str, int startIndex, int length)
        {
            if (startIndex >= 0)
            {
                if (length < 0)
                {
                    length = length * -1;
                    if (startIndex - length < 0)
                    {
                        length = startIndex;
                        startIndex = 0;
                    }
                    else
                    {
                        startIndex = startIndex - length;
                    }
                }


                if (startIndex > str.Length)
                {
                    return "";
                }


            }
            else
            {
                if (length < 0)
                {
                    return "";
                }
                else
                {
                    if (length + startIndex > 0)
                    {
                        length = length + startIndex;
                        startIndex = 0;
                    }
                    else
                    {
                        return "";
                    }
                }
            }

            if (str.Length - startIndex < length)
            {
                length = str.Length - startIndex;
            }

            return str.Substring(startIndex, length);
        }
        /// <summary>
        /// 返回服务器IP
        /// </summary>
        /// <returns></returns>
        public static string GetServerIp()
        {
            try
            {
                IPAddress[] addressList = Dns.GetHostByName(Dns.GetHostName()).AddressList;

                if (addressList != null)
                    return addressList.Length > 0 ? addressList[0].ToString() : string.Empty;
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }

        }
        /// <summary>
        /// Base64编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToBase64String(string str)
        {
            try
            {
                byte[] bytes = Encoding.Default.GetBytes(str);
                return Convert.ToBase64String(bytes);
            }
            catch
            {
                return "";
            }

        }
        /// <summary>
        /// Base64解码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FromBase64String(string str)
        {
            try
            {
                byte[] outputb = Convert.FromBase64String(str);
                return Encoding.Default.GetString(outputb);
            }
            catch
            {
                return "";
            }

        }
        /// <summary>
        /// 0 转换为空，显示用
        /// 
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string ToZeroEmpty(object num)
        {
            if (num == null)
                return "";
            if (num.ToString() == "0" || num.ToString() == "0.00" || num.ToString() == "0001-01-01")
                return "";
            return num.ToString();
        }
        public static string GetUserType()
        {
            return Des.Decrypt(CookieHelper.Get("UserType"));
        }
        /// <summary>
        /// 判断是否可编辑，false 能保存，true--不能保存
        /// </summary>
        /// <returns></returns>
        public static bool IsSave()
        {
            if (Tool.IsEmpty(CookieHelper.Get("UserE")))
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 判断是否为数字，true是数字
        /// </summary>
        /// <param name="strNumber"></param>
        /// <returns></returns>
        public static bool IsNumber(string strNumber)
        {
            if (strNumber == string.Empty || strNumber == null)
                return false;
            try
            {
                decimal.Parse(strNumber);
            }
            catch
            {
                return false;
            }
            return true;
        }
        public static string EncryptDES(string encryptString, string encryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                return encryptString;
            }
        }
        /// <summary>
        /// 获取密钥
        /// </summary>
        private static string GetKey()
        {
            return @")O[NB]6,YF}+efcaj{+oESb9d8>Z'e9M";
        }

        /// <summary>
        /// 获取向量
        /// </summary>
        private static string GetIV()
        {
            return @"L+\~f4,Ir)b$=pkf";
        }
        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="plainStr">明文字符串</param>
        /// <returns>密文</returns>
        public static string AESEncrypt(string plainStr)
        {
            byte[] bKey = Encoding.UTF8.GetBytes(GetKey());
            byte[] bIV = Encoding.UTF8.GetBytes(GetIV());
            byte[] byteArray = Encoding.UTF8.GetBytes(plainStr);

            string encrypt = null;
            Rijndael aes = Rijndael.Create();
            using (MemoryStream mStream = new MemoryStream())
            {
                using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateEncryptor(bKey, bIV), CryptoStreamMode.Write))
                {
                    cStream.Write(byteArray, 0, byteArray.Length);
                    cStream.FlushFinalBlock();
                    encrypt = Convert.ToBase64String(mStream.ToArray());
                }
            }
            aes.Clear();
            return encrypt;
        }
        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="encryptStr">密文字符串</param>
        /// <returns>明文</returns>
        public static string AESDecrypt(string encryptStr)
        {
            byte[] bKey = Encoding.UTF8.GetBytes(GetKey());
            byte[] bIV = Encoding.UTF8.GetBytes(GetIV());
            byte[] byteArray = Convert.FromBase64String(encryptStr);

            string decrypt = null;
            Rijndael aes = Rijndael.Create();
            using (MemoryStream mStream = new MemoryStream())
            {
                using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateDecryptor(bKey, bIV), CryptoStreamMode.Write))
                {
                    cStream.Write(byteArray, 0, byteArray.Length);
                    cStream.FlushFinalBlock();
                    decrypt = Encoding.UTF8.GetString(mStream.ToArray());
                }
            }
            aes.Clear();
            return decrypt;
        }
        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
        public static string DecryptDES(string decryptString, string decryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return decryptString;
            }
        }
        public static string GetMD5(String str)
        {
            string pwd = "";
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            for (int i = 0; i < s.Length; i++)
                pwd = pwd + s[i].ToString("X2");
            return pwd;
        }
        /// <summary>
        /// 获取客户端IP地址（无视代理）
        /// </summary>
        /// <returns>若失败则返回回送地址</returns>
        public static string GetHostAddress()
        {
            string userHostAddress = HttpContext.Current.Request.UserHostAddress;

            if (string.IsNullOrEmpty(userHostAddress))
            {
                userHostAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            //最后判断获取是否成功，并检查IP地址的格式（检查其格式非常重要）
            if (!string.IsNullOrEmpty(userHostAddress) && IsIP(userHostAddress))
            {
                return userHostAddress;
            }
            return "127.0.0.1";
        }
        /// <summary>
        /// 检查IP地址格式
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
        public static void WriteLogToTxt(string txt, string fileName)
        {
            try
            {
                //string strPath = System.Environment.CurrentDirectory + @"\Log\";
                string strPath = System.AppDomain.CurrentDomain.BaseDirectory + @"\LogPrint\";
                if (!Directory.Exists(strPath))
                {
                    Directory.CreateDirectory(strPath);
                }

                using (FileStream aFile = new FileStream(strPath + fileName + ".txt", FileMode.Append))
                {
                    StreamWriter sw = new StreamWriter(aFile);
                    sw.WriteLine(txt);
                    sw.Close();
                }
            }
            catch (IOException ex)
            {
            }

        }
        /// <summary>
        /// 将日期转换成时间戳
        /// </summary>
        /// <param name="dtime"></param>
        /// <returns></returns>
        public static long DataTimeToTimespan(DateTime dtime)
        {

            DateTime oldTime = new DateTime(1970, 1, 1);
            TimeSpan span = dtime.Subtract(oldTime);
            long milliSecondsTime = (long)span.Ticks;
            return milliSecondsTime;
        }
        /// <summary>
        /// 万级资金单位转换 转换为万 或亿
        /// </summary>
        /// <param name="num"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static string WCapitalNumUnitChange(decimal num, out string unit)
        {
            string number = "";
            unit = "万";
            if (num >= 10000)
            {
                num = num / 10000;
                //number = Math.Round((num / 10000), 2, MidpointRounding.AwayFromZero).ToString();
                if (num >= 10000)
                {
                    num = num / 10000;
                }

                number = Math.Round(num, 2, MidpointRounding.AwayFromZero).ToString("0.##");
                unit = "亿";
            }
            else
            {
                number = Math.Floor((num)).ToString("0");
            }
            return number;
        }


        public static string WCapitalNumUnit(decimal num, out string unit)
        {
            string number = "";
            unit = "万人民币";
            if (num >= 10000)
            {
                num = num / 10000;
                number = Math.Round(num, 2, MidpointRounding.AwayFromZero).ToString("0.##");
                unit = "亿人民币";
            }
            else
            {
                number = Math.Round(num, 2, MidpointRounding.AwayFromZero).ToString("0.##");
            }
            return number;
        }


        /// <summary>
        /// 通过反射 将父类 转换为子类
        /// </summary>
        /// <typeparam name="T">子类类型</typeparam>
        /// <param name="parent">父类对象</param>
        /// <returns></returns>
        public static T ParentAutoToSubclass<T>(object parent) where T : new()
        {
            T child = new T();
            Type ParentType = parent.GetType();
            var Properties = ParentType.GetProperties();
            foreach (var Propertie in Properties)
            {
                if (Propertie.CanRead && Propertie.CanWrite)
                {
                    Propertie.SetValue(child, Propertie.GetValue(parent, null), null);
                }
            }
            return child;
        }

        public static string SubStringEntName(string entname)
        {
            if (entname.Length > 6)
            {
                string strEntName = entname.Substring(0, 6);
                if (strEntName.Contains("（"))
                {
                    strEntName = strEntName.Substring(0, strEntName.IndexOf('（'));
                }
                if (strEntName.Contains('('))
                {
                    strEntName = strEntName.Substring(0, strEntName.IndexOf('('));
                }
                return strEntName;
            }
            else
            {
                return entname;
            }
        }
        public static string ConvertToFromatStandardTime(string inputValue)
        {
            try
            {
                return Convert.ToDateTime(inputValue).ToString("yyyy-MM-dd");
            }
            catch (Exception ex)
            {
                return inputValue;
            }
        }

        public static bool CheckKeyWord(string sWord)
        {
            //过滤关键字
            // string StrKeyWord = @"select|insert|delete|count\(|drop table|update|truncate|asc\(|mid\(|char\(|xp_cmdshell|exec master|net localgroup administrators|net user|or";
            string StrKeyWord = @"insert|delete|drop table|update|truncate|or 1=1";
            //过滤关键字符
            // string StrRegex = @"[-|;|,|/|\(|\)|\[|\]|}|{|%|\@|*|!|']";
            if (Regex.IsMatch(sWord, StrKeyWord, RegexOptions.IgnoreCase)) //|| Regex.IsMatch(sWord, StrRegex)
                return true;
            return false;
        }
        /// <summary>
        /// 时间戳转换为时间。
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime ConvertIntDateTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }
        // DateTime时间格式转换为Unix时间戳格式
        public int DateTimeToStamp(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }


        ///// <summary>
        ///// 日期转换成unix时间戳
        ///// </summary>
        ///// <param name="dateTime"></param>
        ///// <returns></returns>
        //public static long DateTimeToUnixTimestamp(DateTime dateTime)
        //{
        //    var start = new DateTime(1970, 1, 1, 0, 0, 0, dateTime.Kind);
        //    return Convert.ToInt64((dateTime - start).TotalSeconds);
        //}

        /// <summary>
        /// unix时间戳转换成日期
        /// </summary>
        /// <param name="unixTimeStamp">时间戳（秒）</param>
        /// <returns></returns>
        public static DateTime longtimestoDatetime(long timestamp)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, System.DateTime.Now.Kind);
            return start.AddSeconds(timestamp);
        }

        #region 接口加密处理函数

        /// <summary>
        /// 判断是否进行加密处理
        /// </summary>
        /// <returns></returns>
        public static bool IsAouth()
        {
            string isAouth = ConfigurationManager.AppSettings["IsAouth"];
            if ("true".Equals(isAouth))
            {
                return false;
            }
            else
            {
                return true;
                //var request = HttpContext.Current.Request;
                //string versioncode = request.Headers.Get("version");//版本号               
                //if (string.IsNullOrEmpty(versioncode))
                //{
                //    versioncode = request.QueryString["version"]; //如果版本号没有放到header中则从地址栏中找。
                //}
                //if (!string.IsNullOrEmpty(versioncode))
                //{
                //    versioncode = versioncode.Replace(".", "");
                //}
                //string apptype = request.Headers.Get("apptype");
                //if (string.IsNullOrEmpty(apptype))
                //{
                //    apptype = request.QueryString.Get("apptype");
                //}
                //if (string.IsNullOrEmpty(versioncode) || string.IsNullOrEmpty(apptype))
                //{
                //    string t = request.QueryString["t"];
                //    string m = request.QueryString["m"];
                //    if (string.IsNullOrEmpty(t) || string.IsNullOrEmpty(m))
                //    {
                //        return true;
                //    }
                //    else
                //    {
                //        return false;
                //    }
                //}
                //string auth = request.QueryString["Auth"];
                //string configauth = ConfigurationManager.AppSettings["Auth"];
                //int intversioncode = 0;
                //if (string.IsNullOrEmpty(versioncode) || string.IsNullOrEmpty(apptype))
                //{
                //    return false;
                //}
                //else
                //{
                //    bool resbool = false;
                //    int.TryParse(versioncode, out intversioncode);
                //    if (configauth.Equals(auth)) //配置是否过安全验证。
                //    {
                //        resbool = false;
                //    }
                //    else
                //    {
                //        switch (apptype)
                //        {
                //            case "0":
                //                if (intversioncode >= 106)//版本号大的必须过安全验证
                //                {
                //                    resbool = true;
                //                }
                //                else
                //                {
                //                    resbool = false;
                //                }
                //                break;
                //            case "1":
                //                if (intversioncode >= 102)//版本号大的必须过安全验证
                //                {
                //                    resbool = true;
                //                }
                //                else
                //                {
                //                    resbool = false;
                //                }
                //                break;
                //            case "2":
                //                resbool = true;//如果h5的apptype 需要加密                
                //                break;
                //            default: //如果没有apptype，即不是请求新接口，不需要加密。
                //                resbool = false;
                //                break;
                //        }
                //    }
                //    return resbool;
                //}
            }

        }
        /// <summary>
        /// 判断是否在有效时效中
        /// </summary>
        /// <returns></returns>
        public static bool Isintime()
        {
            string intime = ConfigurationManager.AppSettings["intime"];
            string t = HttpContext.Current.Request.Headers["t"] == null ? HttpContext.Current.Request["t"] : HttpContext.Current.Request.Headers["t"];
            if (string.IsNullOrEmpty(t))
            {
                return false;  //t参数空 无效。
            }
            int mins = intime == null ? 2 : int.Parse(intime);
            DateTime time1 = DateTime.Now.AddMinutes(mins);
            DateTime time2 = DateTime.Now.AddMinutes(-mins);
            DateTime time3 = Tool.ConvertIntDateTime(t);
            if (time1 > time3 && time3 > time2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 写入文件
        public static void WriteToFile(string strfile)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            FileStream fs = new FileStream(path + "/Logs/Error/20160303.txt", FileMode.OpenOrCreate);
            //获得字节数组
            byte[] data = System.Text.Encoding.Default.GetBytes(strfile);
            //开始写入
            fs.Write(data, 0, data.Length);
            //清空缓冲区、关闭流
            fs.Flush();
            fs.Close();
        }

        public static void WriteToFile2(string strfile)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path + string.Format(@"/Logs/Error/{0}.txt", DateTime.Now.ToString("yyyyMMddHH")), true))
            {
                //file.Write(line);//直接追加文件末尾，不换行
                file.WriteLine(strfile);// 直接追加文件末尾，换行  
            }
        }
        #endregion


        /// <summary>
        /// object转换为DateTime
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="expression">要转换的表达式</param>
        /// <returns></returns>
        public static string ConvertDateTimeToString(object obj, string expression)
        {
            string strReturn = string.Empty;
            if (obj != null)
            {
                try
                {
                    strReturn = Convert.ToDateTime(obj).ToString(expression);
                }
                catch
                {
                    strReturn = "";
                }
            }
            return strReturn;
        }
        public static DateTime ConvertObjectToDateTime(object objDtValue)
        {
            DateTime dtTime = default(DateTime);
            try
            {
                if (objDtValue != null)
                {
                    DateTime.TryParse(objDtValue.ToString(), out dtTime);
                }
            }
            catch { }
            return dtTime;
        }

        public static int IosAndroid()
        {
            int type = 0;
            string agent = HttpContext.Current.Request.UserAgent.ToLower();
            if (agent.Contains("ios"))
            {
                type = 1;
            }
            else if (agent.Contains("android"))
            {
                type = 2;
            }
            return type;
        }

        public static string GenerateTimeStamp(DateTime dt)
        {
            TimeSpan ts = dt.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /// <summary>
        /// 检索关键字  颜色标注
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="searchkey">关键字</param>
        /// <returns></returns>
        public static string GetFontColor(string color, string searchkey)
        {
            return "<font color='" + color + "'>" + searchkey + "</font>";
        }


        public static Decimal ChangeDataToD(string strData)
        {
            Decimal dData = 0.0M;
            if (strData.Contains("E"))
            {
                dData = Convert.ToDecimal(Decimal.Parse(strData.ToString(), System.Globalization.NumberStyles.Float));
            }
            else
            {
                dData = Tool.ToDecimal(strData);
            }
            return dData;
        }

        /// <summary>
        /// 正则拿过滤非数字
        /// </summary>
        /// <param name="zj"></param>
        /// <returns></returns>
        public static decimal GetRatings(string zj)
        {
            decimal Ratings = 0M;
            Regex r = new Regex(@"([1-9]\d*\.?\d*)|(0\.\d*[1-9])");

            //开始匹配
            Match m = r.Match(zj);

            //匹配成功
            if (m.Success)
                Ratings = Convert.ToDecimal(m.Groups[0].Value);


            //从上一个匹配结束的位置开始下一个匹配
            //m = m.NextMatch();



            return Ratings;

        }


    }
}
