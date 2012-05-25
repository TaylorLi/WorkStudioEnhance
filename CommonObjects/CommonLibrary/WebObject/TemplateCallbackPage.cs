/* 
 * 回调基础页面
 *  
 * 创建：杜吉利   
 * 时间：2011年9月5日
 * 
 * 修改：[姓名]         
 * 时间：[时间]             
 * 说明：[说明]
 * 
*/


using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace CommonLibrary.WebObject
{
    public class TemplateCallbackPage : TemplatePage
    {

        #region Properties

        public int type
        {
            get { return CommonLibrary.Utility.NumberHelper.ToInt(Request["type"], -1); }
        }

        public int PageIndex
        {
            get { return Request["page"] != null ? Convert.ToInt32(Request["page"]) : 1; }
        }

        public int PageSize
        {
            get { return Request["size"] != null ? Convert.ToInt32(Request["size"]) : 20; }
        }
        private string _DefaultSort;

        public string DefaultSort
        {
            get { return _DefaultSort; }
            set { _DefaultSort = value; _Sort = value; }
        }

        private string _Sort;
        public string Sort
        {
            get
            {
                if (!string.IsNullOrEmpty(Request["sort"]))
                {
                    _Sort = Request["sort"];
                    return _Sort;
                }
                return _Sort;
            }
            set
            {
                _Sort = value;
            }
        }

        private bool _DefaultIsAsc = false;
        public bool DefaultIsAsc
        {
            get { return _DefaultIsAsc; }
            set { _DefaultIsAsc = value; _IsAsc = value; }
        }

        private bool _IsAsc;
        public bool IsAsc
        {
            get
            {
                if (!string.IsNullOrEmpty(Request["asc"]))
                {
                    _IsAsc = Request["asc"] == "Y";
                }
                return _IsAsc;
            }
            set
            {
                _IsAsc = value;
            }
        }

        #endregion        

        #region Parameters Util Function

        public int[] GetParameterArrayInt(string parameter, string prefix, char separator)
        {
            List<int> ret = new List<int>();

            string[] array = parameter.Replace(prefix, "").Split(separator);
            foreach (string s in array)
                ret.Add(int.Parse(s));
            return ret.ToArray();
        }
        public string[] GetParameterArrayString(string parameter, string prefix, char separator)
        {
            return parameter.Replace(prefix, "").Split(separator);
        }

        #endregion
    }
}