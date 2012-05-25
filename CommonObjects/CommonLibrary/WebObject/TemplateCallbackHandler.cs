using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace CommonLibrary.WebObject
{
    public class TemplateCallbackHandler : IHttpHandler, IRequiresSessionState, IReadOnlySessionState
    {
        #region Properties

        public HttpRequest Request { get; set; }
        public HttpResponse Response { get; set; }

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

        #region Json Result

        public void JsonResult(bool success, string message, object extraData)
        {
            Response.Write(JsonHelper.GetJsonResult(success, message, extraData));
        }

        public void JsonSuccess()
        {
            JsonResult(true, null, null);
        }

        public void JsonError(string errMsg)
        {
            JsonResult(false, errMsg, null);
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

        #region Util Function
        public void ResponseNoRight()
        {
            Response.Write("您无权访问该页面。");
            Response.End();
        }

        public void CheckRight(bool allowed)
        {
            if (!allowed) ResponseNoRight();
        }

        public static void SetValues<T>(T o, string prefix)
            where T : class, new()
        {
            WebHelper.SetValues(o, prefix);
        }

        public static List<T> GetListFromRequest<T>(string prefix)
            where T : class, new()
        {
            return WebHelper.GetListFromRequest<T>(prefix);
        }

        public static List<T> GetListFromRequestForVal<T>(string prefix)
        {
            return WebHelper.GetListFromRequestForVal<T>(prefix);
        }
        #endregion

        #region Interface IHttpHandler

        public virtual void ProcessRequest(HttpContext context)
        {
            Request = context.Request;
            Response = context.Response;
            context.Response.ContentEncoding = Encoding.UTF8;
            context.Response.ContentType = "text/plain";
        }

        public virtual bool IsReusable
        {
            get
            {
                return false;
            }
        }
        #endregion
    }
}
