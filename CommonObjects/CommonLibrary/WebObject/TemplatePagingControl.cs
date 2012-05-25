/* 
 * 分页控件
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
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Collections.Generic;

namespace CommonLibrary.WebObject
{
    public class TemplatePagingControl : TemplateControl
    {
        #region Attributes

        private string _DefaultSelCss = "select";
        public string DefaultSelCss
        {
            get { return _DefaultSelCss; }
            set { _DefaultSelCss = value; }
        }

        private int _DefaultPagingButtonCount = 5;
        public int DefaultPagingButtonCount
        {
            get { return _DefaultPagingButtonCount; }
            set { _DefaultPagingButtonCount = value; }
        }

        private string _DefaultPageSizeSelectId = "selPageSize";
        public string DefaultPageSizeSelectId
        {
            get { return _DefaultPageSizeSelectId; }
            set { _DefaultPageSizeSelectId = value; }
        }

        private bool _ReponseEndWhenEmpty = true;
        public bool ReponseEndWhenEmpty
        {
            get { return _ReponseEndWhenEmpty; }
            set { _ReponseEndWhenEmpty = value; }
        }

        private int _pageindex = 1;
        public int PageIndex
        {
            get { return _pageindex; }
            set { _pageindex = value; }
        }
        private int _pagesize = 20;
        public int PageSize
        {
            get { return _pagesize; }
            set { _pagesize = value; }
        }
        private bool _IsAsc = false;
        public bool IsAsc
        {
            get { return _IsAsc; }
            set { _IsAsc = value; }
        }
        private string _Sort = "";
        public string Sort
        {
            get { return _Sort; }
            set { _Sort = value; }
        }

        private int _Total;

        public int Total
        {
            get { return _Total; }
            set { _Total = value; }
        }

        private string _JsFunction;

        public string JsFunction
        {
            get { return _JsFunction; }
            set { _JsFunction = value; }
        }

        private object _BindingResult;

        public object BindingResult
        {
            get { return _BindingResult; }
            set { _BindingResult = value; }
        }



        #endregion

        #region Functions

        public string ShowPaging(int buttonCount, string pageUrl, string javascript)
        {
            return CommonLibrary.WebObject.Paging.GetPagingString(buttonCount, PageIndex, PageSize, Total, pageUrl, javascript);
        }

        public string ShowPaging(string pageUrl)
        {
            return CommonLibrary.WebObject.Paging.GetPagingString(DefaultPagingButtonCount, PageIndex, PageSize, Total, pageUrl, "");
        }

        public string ShowPaging(string pageUrl, string javascript)
        {
            return CommonLibrary.WebObject.Paging.GetPagingString(5, PageIndex, PageSize, Total, pageUrl, javascript);
        }

        public string ShowPaging(params string[] extraParams)
        {
            return ShowPaging("", JsFunction + "({0}," + PageSize + ",'" + Sort + "'," + (!IsAsc ? "false" : "true") + (extraParams != null && extraParams.Length > 0 ? string.Concat(",", Utility.StringHelper.ArrayToString(extraParams, ",")) : "") + ")");
        }

        public string GetSortHeader(string title, string sortBy, string javascript)
        {
            return CommonLibrary.WebObject.HtmlHelper.GetSortHeader(javascript, title, sortBy, IsAsc, Sort);
        }

        public string GetSortHeader(string title, string sortBy, params string[] extraParams)
        {
            return GetSortHeader(title, sortBy, JsFunction + "(1," + PageSize + ",'" + sortBy + "'," + (IsAsc ? "false" : "true") + (extraParams != null && extraParams.Length > 0 ? string.Concat(",", Utility.StringHelper.ArrayToString(extraParams, ",")) : "") + ")");
        }

        public string GenPageSize(string id, params string[] extraParams)
        {
            return CommonLibrary.WebObject.HtmlHelper.GenPageSize(id, DefaultSelCss, PageSize, JsFunction + "(1,{0},null,null" + (extraParams != null && extraParams.Length > 0 ? string.Concat(",", Utility.StringHelper.ArrayToString(extraParams, ",")) : "") + ")");
        }

        public string GenPageSize()
        {
            return CommonLibrary.WebObject.HtmlHelper.GenPageSize(DefaultPageSizeSelectId, DefaultSelCss, PageSize, JsFunction + "(1,{0})");
        }

        public void SetData<T, C>(C c)
            where T : class, new()
            where C : ICollection<T>, new()
        {
            this.BindingResult = c;
            this.Total = c.Count;
        }
        public void SetData<T>(IList<T> list)
        {
            this.BindingResult = list;
            this.Total = list.Count;
        }

        public void SetData<T>(IList<T> list, int total)
        {
            this.BindingResult = list;
            this.Total = total;
        }

        protected override void OnLoad(EventArgs e)
        {
            if (ReponseEndWhenEmpty && Total == 0)
            {
                Response.Clear();
                Response.Write(CommonLibrary.WebObject.HtmlHelper.MsgBoxHtml(CommonLibrary.Resources.Resource.RecordNotFound, this.Page));
                Response.End();
            }
            base.OnLoad(e);
        }

        #endregion
    }
}
