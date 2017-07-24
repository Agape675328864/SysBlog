using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysTemModel.ResultModel
{
    public enum ResultCode
    {
        Ok = 1, //ok
        ServerError = 0, //服务器错误 
        ClientError = 2, //客户端非法数据
    }
}
