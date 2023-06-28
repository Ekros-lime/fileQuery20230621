using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fileQuery20230621
{
    public class PathInfo
    {
        private string _showName;
        private string _path;

        public string ShowName
        {
            get 
            {
                return this._showName;
            }
            set
            {
                this._showName = value;
            }
        }
        public string Path
        {
            get
            {
                return this._path;
            }
            set
            {
                this._path = value;
            }
        }

        public PathInfo()
        {

        }
        public PathInfo(string showName, string path)
        {
            this.ShowName = showName;
            this.Path = path;
        }
    }
}