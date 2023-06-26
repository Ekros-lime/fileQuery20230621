using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fileQuery20230621
{
    //用于存储搜索到的信息并与CheckBoxList绑定
    public class QueryInfo
    {
        private string _info;

        private string _value;

        public string Info
        {
            get
            {
                return this._info;
            }
            set
            {
                this._info = value;
            }
        }

        public string Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
            }
        }

        public QueryInfo(string info)
        {
            this.Info = info;
        }

        public QueryInfo(string info, string value)
        {
            this.Info = info;
            this.Value = value;
        }

        public QueryInfo()
        {

        }
    }
}