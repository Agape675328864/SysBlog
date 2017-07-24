using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysTemModel.ResultModel
{
    public class ResultBase
    {
        /// <summary>
        /// 状态
        /// </summary>
     
        public ResultCode Result { get; set; }

        /// <summary>
        /// 消息语
        /// </summary>
      
        public string Msg { get; set; }
    }
}
