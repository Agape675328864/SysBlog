using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysTemModel.ResultModel
{
    /// <summary>
    /// 获取列表
    /// </summary>
    public class ListResultBase : ResultBase
    {
        public object ListData { get; set; }
        /// <summary>
        /// 总条数
        /// </summary>
        public int Count { get; set; }
    }
}
