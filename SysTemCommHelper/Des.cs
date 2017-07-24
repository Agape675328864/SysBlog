using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Jusfoun.YunCompany.Common
{
    /// <summary>
    /// 对称加密，解密类
    /// </summary>
    public class Des
    {
        /// <summary>    
        /// myiv is  iv    
        /// </summary>    
        private static string[] arrmyiv = new string[6] { "JusFouncom222", "JusFouncom222", "JusFouncom222", "JusFouncom222", "JusFouncom222", "JusFouncom222" };

        private static string myiv = "";

        private static void Set_myiv()
        {
            switch (DateTime.Now.Month)
            { 
                case 1:
                    myiv = arrmyiv[0];
                    break;
                case 2:
                    myiv = arrmyiv[1];
                    break;
                case 3:
                    myiv = arrmyiv[2];
                    break;
                case 4:
                    myiv = arrmyiv[3];
                    break;
                case 5:
                    myiv = arrmyiv[4];
                    break;
                case 6:
                    myiv = arrmyiv[5];
                    break;
                case 7:
                    myiv = arrmyiv[0];
                    break;
                case 8:
                    myiv = arrmyiv[1];
                    break;
                case 9:
                    myiv = arrmyiv[2];
                    break;
                case 10:
                    myiv = arrmyiv[3];
                    break;
                case 11:
                    myiv = arrmyiv[4];
                    break;
                case 12:
                    myiv = arrmyiv[5];
                    break;
            }
        }
        /// <summary>    
        /// mykey is key    
        /// </summary>    
        private static string mykey = "gyBankxm";//AppSettingUtility.AuthKey;

        /// <summary>    
        /// DES加密偏移量    
        /// 必须是>=8位长的字符串    
        /// </summary>    
        public string IV
        {
            get { return myiv; }
            set { myiv = value; }
        }

        /// <summary>    
        /// DES加密的私钥    
        /// 必须是8位长的字符串    
        /// </summary>    
        public string Key
        {
            get { return mykey; }
            set { mykey = value; }
        }

        /// <summary>    
        /// 对字符串进行DES加密    
        /// Encrypts the specified sourcestring.    
        /// </summary>    
        /// <param name="sourcestring">The sourcestring.待加密的字符串</param>    
        /// <returns>加密后的BASE64编码的字符串</returns>    
        public static string Encrypt(string sourceString)
        {
            StringBuilder sb = new StringBuilder();
            Set_myiv();
            byte[] btKey = Encoding.Default.GetBytes(mykey);
            byte[] btIV = Encoding.Default.GetBytes(myiv);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] inData = Encoding.Default.GetBytes(sourceString);
                try
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(btKey, btIV), CryptoStreamMode.Write))
                    {
                        cs.Write(inData, 0, inData.Length);
                        cs.FlushFinalBlock();
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
                catch
                {
                    //throw;  
                    return "";
                }
            }
        }

        /// <summary>    
        /// Decrypts the specified encrypted string.    
        /// 对DES加密后的字符串进行解密    
        /// </summary>    
        /// <param name="encryptedString">The encrypted string.待解密的字符串</param>    
        /// <returns>解密后的字符串</returns>    
        public static string Decrypt(string encryptedString)
        {           
            Set_myiv();
            byte[] btKey = Encoding.Default.GetBytes(mykey);
            byte[] btIV = Encoding.Default.GetBytes(myiv);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            using (MemoryStream ms = new MemoryStream())
            {
                byte[] inData = Convert.FromBase64String(encryptedString);
                try
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(btKey, btIV), CryptoStreamMode.Write))
                    {
                        cs.Write(inData, 0, inData.Length);
                        cs.FlushFinalBlock();
                    }
                    return Encoding.Default.GetString(ms.ToArray());
                }
                catch
                {
                    //throw;
                    return "";
                }
            }



        }

        /// <summary>    
        /// Encrypts the file.    
        /// 对文件内容进行DES加密    
        /// </summary>    
        /// <param name="sourceFile">The source file.待加密的文件绝对路径</param>    
        /// <param name="destFile">The dest file.加密后的文件保存的绝对路径</param>    
        public static void EncryptFile(string sourceFile, string destFile)
        {
            Set_myiv();
            if (!File.Exists(sourceFile)) throw new FileNotFoundException("指定的文件路径不存在！", sourceFile);

            byte[] btKey = Encoding.Default.GetBytes(mykey);
            byte[] btIV = Encoding.Default.GetBytes(myiv);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] btFile = File.ReadAllBytes(sourceFile);

            using (FileStream fs = new FileStream(destFile, FileMode.Create, FileAccess.Write))
            {
                try
                {
                    using (CryptoStream cs = new CryptoStream(fs, des.CreateEncryptor(btKey, btIV), CryptoStreamMode.Write))
                    {
                        cs.Write(btFile, 0, btFile.Length);
                        cs.FlushFinalBlock();
                    }
                }
                catch
                {
                    throw;
                }

                finally
                {
                    fs.Close();
                }
            }
        }

        /// <summary>    
        /// Encrypts the file.    
        /// 对文件内容进行DES加密，加密后覆盖掉原来的文件    
        /// </summary>    
        /// <param name="sourceFile">The source file.待加密的文件的绝对路径</param>    
        public static void EncryptFile(string sourceFile)
        {
            EncryptFile(sourceFile, sourceFile);
        }

        /// <summary>    
        /// Decrypts the file.    
        /// 对文件内容进行DES解密    
        /// </summary>    
        /// <param name="sourceFile">The source file.待解密的文件绝对路径</param>    
        /// <param name="destFile">The dest file.解密后的文件保存的绝对路径</param>    
        public static void DecryptFile(string sourceFile, string destFile)
        {
            Set_myiv();
            if (!File.Exists(sourceFile)) throw new FileNotFoundException("指定的文件路径不存在！", sourceFile);
            byte[] btKey = Encoding.Default.GetBytes(mykey);
            byte[] btIV = Encoding.Default.GetBytes(myiv);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] btFile = File.ReadAllBytes(sourceFile);

            using (FileStream fs = new FileStream(destFile, FileMode.Create, FileAccess.Write))
            {
                try
                {
                    using (CryptoStream cs = new CryptoStream(fs, des.CreateDecryptor(btKey, btIV), CryptoStreamMode.Write))
                    {
                        cs.Write(btFile, 0, btFile.Length);
                        cs.FlushFinalBlock();
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    fs.Close();
                }
            }
        }

        /// <summary>    
        /// Decrypts the file.    
        /// 对文件内容进行DES解密，加密后覆盖掉原来的文件.    
        /// </summary>    
        /// <param name="sourceFile">The source file.待解密的文件的绝对路径.</param>    
        public static void DecryptFile(string sourceFile)
        {
            DecryptFile(sourceFile, sourceFile);
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="plainStr">明文字符串</param>
        /// <returns>密文</returns>
        public static string AESEncrypt(string plainStr)
        {
            StringBuilder sb = new StringBuilder();
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
                   // encrypt = Convert.ToBase64String(mStream.ToArray());
                    encrypt = Encode(Convert.ToBase64String(mStream.ToArray()));
                    //byte[] array = mStream.ToArray();
                    //for (int i = 0; i < array.Length; i++)
                    //{
                    //    if (array[i] <10)
                    //    {
                    //        sb.Append("0" + array[i]);
                    //    }
                    //    else
                    //    {
                    //        sb.Append(array[i]);
                    //    }
                    //}
                    //encrypt= sb.ToString();
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
            string decrypt = null;
           
            try
            {
                encryptStr = Decode(encryptStr);
                byte[] bKey = Encoding.UTF8.GetBytes(GetKey());
                byte[] bIV = Encoding.UTF8.GetBytes(GetIV());
                byte[] byteArray = Convert.FromBase64String(encryptStr);

               
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
            }
            catch (Exception)
            {
                
                throw;
            }
            //char[] strs = encryptStr.ToCharArray();

            //byte[] array = new byte[strs.Length / 2];
            //for (int i = 0; i <= array.Length; i++)
            //{
            //    array[i] = Convert.ToByte(encryptStr.Substring(0, 2));
            //}
            //encryptStr = System.Text.Encoding.ASCII.GetString(array);

           
          
            return decrypt;
        }

        public static string Encode(string strEncode)
        {
            string strReturn = "";//  存储转换后的编码
            foreach (short shortx in strEncode.ToCharArray())
            {
                strReturn += shortx.ToString("X");
            }
            return strReturn;
        }

        public static string Decode(string strDecode)
        {
            string sResult = "";
            for (int i = 0; i < strDecode.Length /2; i++)
            {
                sResult += (char)short.Parse(strDecode.Substring(i * 2, 2), global::System.Globalization.NumberStyles.HexNumber);
            }
            return sResult;
        }
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
        /// DES加密
        /// </summary>
        /// <param name="pToEncrypt">加密字符串</param>
        /// <param name="sKey">密钥</param>
        /// <returns></returns>
        public static string string_Encrypt(string pToEncrypt)
        {
            string sKey = "Blog";
            if (pToEncrypt == "") return "";
            if (sKey.Length < 8) sKey = sKey + "xuE29xWp";
            if (sKey.Length > 8) sKey = sKey.Substring(0, 8);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            //把字符串放到byte数组中  
            //原来使用的UTF8编码，我改成Unicode编码了，不行  
            byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
            //建立加密对象的密钥和偏移量  
            //原文使用ASCIIEncoding.ASCII方法的GetBytes方法  
            //使得输入密码必须输入英文文本  
            des.Key = ASCIIEncoding.Default.GetBytes(sKey);
            des.IV = ASCIIEncoding.Default.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            //Write  the  byte  array  into  the  crypto  stream  
            //(It  will  end  up  in  the  memory  stream)  
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            //Get  the  data  back  from  the  memory  stream,  and  into  a  string  
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                //Format  as  hex  
                ret.AppendFormat("{0:X2}", b);
            }
            ret.ToString();
            return ret.ToString();
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="pToDecrypt">解密字符串</param>
        /// <param name="sKey">解密密钥</param>
        /// <param name="outstr">返回值</param>
        /// <returns></returns> 
        public static string string_Decrypt(string pToDecrypt)
        {
            string sKey = "Blog";
            string outstr = "-1";
            if (pToDecrypt == "")
            {
                outstr = "";
            };
            if (sKey.Length < 8) sKey = sKey + "xuE29xWp";
            if (sKey.Length > 8) sKey = sKey.Substring(0, 8);
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                //Put  the  input  string  into  the  byte  array  
                byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
                for (int x = 0; x < pToDecrypt.Length / 2; x++)
                {
                    int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                    inputByteArray[x] = (byte)i;
                }
                //建立加密对象的密钥和偏移量，此值重要，不能修改  
                des.Key = ASCIIEncoding.Default.GetBytes(sKey);
                des.IV = ASCIIEncoding.Default.GetBytes(sKey);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                //Flush  the  data  through  the  crypto  stream  into  the  memory  stream  
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                //Get  the  decrypted  data  back  from  the  memory  stream  
                //建立StringBuild对象，CreateDecrypt使用的是流对象，必须把解密后的文本变成流对象  
                StringBuilder ret = new StringBuilder();
                outstr = System.Text.Encoding.Default.GetString(ms.ToArray());
            }
            catch
            {

            }
            return outstr;
        }
    }
}
