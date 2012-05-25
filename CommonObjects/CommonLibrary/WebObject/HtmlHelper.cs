/* 
 * 常用控件生产器
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
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;

namespace CommonLibrary.WebObject
{
    public class HtmlHelper
    {
        #region Select

        #region Properties
        public static string[] PAGE_SELECTOR = new string[] { "5,5", "10,10", "20,20", "30,30", "50,50", "100,100", "1000,All" };
        public const string STR_SELECT = "<select id=\"{0}\" name=\"{0}\" class=\"{1}\" title=\"{2}\">{3}</select>";
        public const string STR_SELECT_WITH_EVENT = "<select id=\"{0}\" name=\"{0}\" class=\"{1}\" title=\"{2}\" {4}>{3}</select>";
        public const string STR_SELECT_ITEM = "<option value=\"{0}\">{1}</option>";
        public const string STR_SELECT_ITEM_SELECTED = "<option value=\"{0}\" selected=\"selected\">{1}</option>";
        public const string STR_WHITESPACE = "&nbsp;";
        public static string SortHeaderFmt = "<a href=\"Javascript:{0}\" style=\"white-space:nowrap;{1}\">{2}{3}</a>";
        #endregion
        /// <summary>
        /// format:value=="":text,else text(value)
        /// </summary>
        /// <param name="source">dictionary:key->value,text->value</param>
        /// <param name="selected_value"></param>
        /// <param name="id">select id</param>
        /// <param name="css">select css</param>
        /// <param name="title">select title</param>
        /// <param name="jsFunc">extend attribute,"onchange="SetVisible();","style="display:none" eg.</param>
        /// <returns>select control html</returns>
        public static string GenSelect(System.Collections.Generic.Dictionary<string, string> source, string selected_value, string id, string css, string title, string jsFunc)
        {
            StringBuilder sb_ret = new StringBuilder();
            if (string.IsNullOrEmpty(selected_value)) selected_value = "";
            selected_value = selected_value.Trim().ToUpper();
            foreach (string k in source.Keys)
            {
                if (k.Trim().ToUpper().Equals(selected_value))
                {
                    if (k == "")
                        sb_ret.Append(string.Format(STR_SELECT_ITEM_SELECTED, k, string.Format("{0}", source[k])));
                    else
                        sb_ret.Append(string.Format(STR_SELECT_ITEM_SELECTED, k, string.Format("{0}({1})", source[k], k)));
                }
                else
                {
                    if (k == "")
                        sb_ret.Append(string.Format(STR_SELECT_ITEM, k, string.Format("{0}", source[k])));
                    else
                        sb_ret.Append(string.Format(STR_SELECT_ITEM, k, string.Format("{0}({1})", source[k], k)));
                }
            }
            if (jsFunc == "")
                return string.Format(STR_SELECT, id, css, title, sb_ret.ToString());
            else
                return string.Format(STR_SELECT_WITH_EVENT, id, css, title, sb_ret.ToString(), jsFunc);
        }
        /// <summary>
        /// format:empty:text,else:text
        /// </summary>
        /// <param name="source">dictionary:key->value,text->value</param>
        /// <param name="selected_value"></param>
        /// <param name="id">select id</param>
        /// <param name="css">select css</param>
        /// <param name="title">select title</param>
        /// <param name="jsFunc">extend attribute,"onchange="SetVisible();","style="display:none" eg.</param>
        /// <returns>select control html</returns>
        public static string GenSelectWithoutVal(System.Collections.Generic.Dictionary<string, string> source, string selected_value, string id, string css, string title, string jsFunc)
        {
            StringBuilder sb_ret = new StringBuilder();
            if (!string.IsNullOrEmpty(selected_value))
                selected_value = selected_value.Trim().ToUpper();
            else
                selected_value = "";
            foreach (string k in source.Keys)
            {
                if (k.Trim().ToUpper().Equals(selected_value))
                {
                    if (k == "")
                        sb_ret.Append(string.Format(STR_SELECT_ITEM_SELECTED, k, string.Format("{0}", source[k])));
                    else
                        sb_ret.Append(string.Format(STR_SELECT_ITEM_SELECTED, k, string.Format("{0}", source[k])));
                }
                else
                {
                    if (k == "")
                        sb_ret.Append(string.Format(STR_SELECT_ITEM, k, string.Format("{0}", source[k])));
                    else
                        sb_ret.Append(string.Format(STR_SELECT_ITEM, k, string.Format("{0}", source[k])));
                }
            }
            if (jsFunc == "")
                return string.Format(STR_SELECT, id, css, title, sb_ret.ToString());
            else
                return string.Format(STR_SELECT_WITH_EVENT, id, css, title, sb_ret.ToString(), jsFunc);
        }

        public static string GenSelectForEnum(Type t, System.Resources.ResourceManager rm, bool isForValues, string selected_value, string id, string css, string title, string jsFunc, Dictionary<string, string> source, List<string> excludeValues)
        {
            string name = string.Empty;
            if (source == null) source = new Dictionary<string, string>();
            if (isForValues)
            {
                string eName = string.Empty;
                foreach (int v in Enum.GetValues(t))
                {
                    eName = Enum.GetName(t, v);
                    if (rm != null)
                        name = rm.GetString(eName, System.Threading.Thread.CurrentThread.CurrentUICulture);
                    if (string.IsNullOrEmpty(name))
                        name = eName;
                    if (!(excludeValues != null && excludeValues.Contains(v.ToString())))
                        source.Add(v.ToString(), name);
                }
            }
            else
                foreach (string n in Enum.GetNames(t))
                {
                    if (rm != null)
                        name = rm.GetString(n, System.Threading.Thread.CurrentThread.CurrentUICulture);
                    if (string.IsNullOrEmpty(name))
                        name = n;
                    if (!(excludeValues != null && excludeValues.Contains(name)))
                        source.Add(n, name);
                }
            return CommonLibrary.WebObject.HtmlHelper.GenSelectWithoutVal(source, selected_value, id, css, title, jsFunc);
        }

        public static string GenSelectForEnum(Type t, System.Resources.ResourceManager rm, bool isForValues, string selected_value, string id, string css, string title, string jsFunc, Dictionary<string, string> source)
        {
            return CommonLibrary.WebObject.HtmlHelper.GenSelectForEnum(t, rm, isForValues, selected_value, id, css, title, jsFunc, source, null);
        }

        public static string GenSelectForEnum(Type t, System.Resources.ResourceManager rm, bool isForValues, string selected_value, string id, string css, string title, string jsFunc)
        {
            return GenSelectForEnum(t, rm, isForValues, selected_value, id, css, title, jsFunc, null);
        }

        public static string GenSelectForEnum(Type t, System.Resources.ResourceManager rm, bool isForValues, string selected_value, string id, string css, string title, string jsFunc, string appendName, string appendValue)
        {
            Dictionary<string, string> source = new Dictionary<string, string>();
            if (appendValue != null)
                source.Add(appendValue, appendName);
            return GenSelectForEnum(t, rm, isForValues, selected_value, id, css, title, jsFunc, source);
        }

        public static string GenSelectForEnum(Type t, System.Resources.ResourceManager rm, bool isForValues, string selected_value, string id, string css, string title, string jsFunc, string appendName, string appendValue, string excludeValue)
        {
            Dictionary<string, string> source = new Dictionary<string, string>();
            if (appendName != null || appendValue != null)
                source.Add(appendValue, appendName);
            List<string> excludeVals = new List<string>();
            if (excludeValue != null)
                excludeVals.Add(excludeValue);
            return GenSelectForEnum(t, rm, isForValues, selected_value, id, css, title, jsFunc, source, excludeVals);
        }

        public static string GenSelectForEnum(Type t, bool isForValues, string selected_value, string id, string css, string title, string jsFunc, Dictionary<string, string> source)
        {
            if (source == null) source = new Dictionary<string, string>();
            if (isForValues)
            {
                string name = string.Empty;
                foreach (int v in Enum.GetValues(t))
                    source.Add(v.ToString(), Enum.GetName(t, v));
            }
            else
                foreach (string n in Enum.GetNames(t))
                    source.Add(n, n);
            return CommonLibrary.WebObject.HtmlHelper.GenSelectWithoutVal(source, selected_value, id, css, title, jsFunc);
        }

        public static string GenSelectForEnum(Type t, bool isForValues, string selected_value, string id, string css, string title, string jsFunc, string appendName, string appendValue)
        {
            Dictionary<string, string> source = new Dictionary<string, string>();
            if (appendValue != null)
                source.Add(appendValue, appendName);
            return GenSelectForEnum(t, isForValues, selected_value, id, css, title, jsFunc, source);
        }

        public static string GenSelectForNumber(string selected_value, string id, string css, string title, string jsFunc, int start, int end, int spaceNumber)
        {
            Dictionary<string, string> dicNumbers = new Dictionary<string, string>();
            for (int i = start; i <= end; i = i + spaceNumber)
            {
                string s = i.ToString();
                dicNumbers.Add(s, s);
            }
            return GenSelectWithoutVal(dicNumbers, selected_value, id, css, title, jsFunc);
        }

        #endregion

        #region Paging
        public static string GenRecordInfo(int recordCount)
        {
            return string.Format(Resources.Resource.RecordsFoundString, recordCount);
        }
        public static string GenPageSize(string id, string css, decimal pageSize)
        {
            return GenPageSize(id, css, pageSize, null);
        }
        public static string GenPageSize(string id, string css, decimal pageSize, string evalCode)
        {
            StringBuilder sb_r = new StringBuilder();
            StringBuilder sb_ret = new StringBuilder();
            foreach (string s in HtmlHelper.PAGE_SELECTOR)
            {
                string[] info = s.Split(',');
                if (decimal.Parse(info[0]) == pageSize)
                    sb_ret.Append(string.Format(STR_SELECT_ITEM_SELECTED, info[0], info[1]));
                else
                    sb_ret.Append(string.Format(STR_SELECT_ITEM, info[0], info[1]));
            }
            if (evalCode != null & evalCode != "")
                return sb_r.AppendFormat(Resources.Resource.ShowString, string.Format(STR_SELECT_WITH_EVENT, id, css, "", sb_ret.ToString(), "onchange=\"ChangePageSize(this,'" + evalCode + "')\" style='width:50px;'")).ToString();
            else
                return sb_r.AppendFormat(Resources.Resource.ShowString, string.Format(STR_SELECT_WITH_EVENT, id, css, "", sb_ret.ToString(), "onchange='ChangePageSize(this)' style='width:50px;'")).ToString();
        }
        public static string GetSort(string key, string sortColumn, bool isAsc)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(sortColumn)) return string.Empty;
            if (key.ToLower() == sortColumn.ToLower())
            {
                if (isAsc)
                    return "<span class=\"asc\">&nbsp;</span>";
                else
                    return "<span class=\"desc\">&nbsp;</span>";
            }
            return string.Empty;
        }
        public static string GetSortHeader(string js, string title, string sortBy, bool isAsc, string currentSortBy)
        {
            return GetSortHeader(js, title, sortBy, isAsc, currentSortBy, "");
        }
        public static string GetSortHeader(string js, string title, string sortBy, bool isAsc, string currentSortBy, string style)
        {
            return string.Format(SortHeaderFmt, js, style, title, GetSort(currentSortBy, sortBy, isAsc));
        }
        public static string MsgBoxHtml(string text, Page page)
        {
            string MsgBoxFormat = "<table width=\"100%\"><tr><td align=\"center\"><table border=\"0px\" cellpadding=\"0px\" cellspacing=\"0px\" width=\"450px\"><tr><td class=\"ad-lt\"></td><td class=\"ad-mt\"></td><td class=\"ad-rt\"></td></tr><tr><td class=\"ad-l\"></td><td class=\"ad-bg\" align=\"left\" style=\"height: 100px;\"><table width=\"85%\" border=\"0\"><tr><td style=\"padding: 5px 10px 5px 10px; width: 15%\">{0}</td><td align=\"left\" style=\"width: 85%\">{1}</td></tr></table></td><td class=\"ad-r\"></td></tr><tr><td class=\"ad-lb\"></td><td class=\"ad-mb\"></td><td class=\"ad-rb\"></td></tr></table></td></tr></table>";
            string Img = "<img src=\"" + page.ResolveUrl("~") + "img/common/error1.gif\" alt=\"\" />";
            return string.Format(MsgBoxFormat, Img, text);
        }
        public static string MsgBoxHtml(string text, Page page, string style)
        {
            string MsgBoxFormat = "<table width=\"100%\"><tr><td align=\"center\"><table border=\"0px\" cellpadding=\"0px\" cellspacing=\"0px\" style=\"{2}\"><tr><td class=\"ad-lt\" /><td class=\"ad-mt\" /><td class=\"ad-rt\" /></tr><tr><td class=\"ad-l\" /><td class=\"ad-bg\" align=\"left\" style=\"height: 100px;\"><table width=\"85%\" border=\"0\"><tr><td style=\"padding: 5px 10px 5px 10px; width: 15%\">{0}</td><td align=\"left\" style=\"width: 85%\">{1}</td></tr></table></td><td class=\"ad-r\" /></tr><tr><td class=\"ad-lb\" /><td class=\"ad-mb\" /><td class=\"ad-rb\" /></tr></table></td></tr></table>";
            string Img = "<img src=\"" + page.ResolveUrl("~") + "img/common/error1.gif\" alt=\"\" />";
            return string.Format(MsgBoxFormat, Img, text, style);
        }
        #endregion

        #region ReportView

        /// <summary>
        /// 返回导出工具条如果有Button，则selectFunName参数无效，否则，buttonFunName无效。
        /// </summary>
        /// <param name="selectId"></param>
        /// <param name="selectCss"></param>
        /// <param name="selectFunName"></param>
        /// <param name="isWithButton"></param>
        /// <param name="buttonId"></param>
        /// <param name="buttonCss"></param>
        /// <param name="buttonText"></param>
        /// <param name="buttonFunName"></param>
        /// <returns></returns>
        public static string GenReportViewerExportControl(string selectId, string selectCss, string selectFunName, bool isWithButton, string buttonId, string buttonCss, string buttonText, string buttonFunName)
        {
            StringBuilder sb_ret = new StringBuilder();
            StringBuilder sb_item = new StringBuilder();
            string[] EXPORT_SELECTOR = new string[3];
            EXPORT_SELECTOR[0] = "" + "," + Resources.Resource.SelectFormat;
            EXPORT_SELECTOR[1] = "pdf" + "," + Resources.Resource.ToPDF;
            EXPORT_SELECTOR[2] = "excel" + "," + Resources.Resource.ToExcel;
            foreach (string s in EXPORT_SELECTOR)
            {
                string[] info = s.Split(',');
                if (info[0] == string.Empty)
                    sb_item.Append(string.Format(STR_SELECT_ITEM_SELECTED, info[0], info[1]));
                else
                    sb_item.Append(string.Format(STR_SELECT_ITEM, info[0], info[1]));
            }
            string buttonString = string.Empty;
            string selectString = string.Empty;
            if (!isWithButton)
            {
                sb_ret.Append(string.Format(STR_SELECT_WITH_EVENT, selectId, selectCss, "", sb_item.ToString(), string.Format(" onchange=\"{0}\" ", selectFunName)));
                return sb_ret.ToString();
            }
            else
            {
                string selchangeFun = "if(document.getElementById('{0}').value==''){ document.getElementById('{1}').disabled=true;document.getElementById('{1}').style.cursor = 'default';document.getElementById('{1}').style.color = '#BFBFBF';}else{document.getElementById('{1}').disabled=false;document.getElementById('{1}').style.cursor = 'pointer';document.getElementById('{1}').style.color = '';} ";
                string selStr = selchangeFun.Replace("{0}", selectId).Replace("{1}", buttonId);
                sb_ret.Append(string.Format(STR_SELECT_WITH_EVENT, selectId, selectCss, "", sb_item.ToString(), string.Format(" onchange=\"{0}\" ", selStr)));
                sb_ret.Append(HtmlHelper.STR_WHITESPACE);
                sb_ret.Append(HtmlHelper.STR_WHITESPACE);
                sb_ret.Append(string.Format(HtmlHelper.STR_DISABLEBUTTON, buttonId, buttonCss, buttonFunName, buttonText));
                return sb_ret.ToString();
            }


        }

        #endregion

        #region Button

        public const string STR_BUTTON = "<input id=\"{0}\" class=\"{1}\" type=\"button\" onclick=\"{2}\" value=\"{3}\"/>";
        public const string STR_DISABLEBUTTON = "<input type=\"button\" id=\"{0}\" class=\"{1}\" onclick=\"{2}\" value=\"{3}\" style=\"cursor: default; color: #BFBFBF;\" enable=\"false\" disabled=\"\"/>";

        public static string GetButtonHtml(string id, string css, string onClick, string value, bool isDisabled)
        {
            if (isDisabled)
                return string.Format(STR_DISABLEBUTTON, id, css, onClick, value);
            else
                return string.Format(STR_BUTTON, id, css, onClick, value);
        }
        #endregion

        #region Encode Decode

        public static string HtmlEncode(string theString)
        {
            theString = theString.Replace(">", "&gt;");
            theString = theString.Replace("<", "&lt;");
            theString = theString.Replace("  ", " &nbsp;");
            theString = theString.Replace("  ", " &nbsp;");
            theString = theString.Replace("\"", "&quot;");
            theString = theString.Replace("\'", "&#39;");
            theString = theString.Replace("\n", "<br/> ");
            return theString;
        }

        public static string HtmlDiscode(string theString)
        {
            theString = theString.Replace("&gt;", ">");
            theString = theString.Replace("&lt;", "<");
            theString = theString.Replace("&nbsp;", " ");
            theString = theString.Replace(" &nbsp;", "  ");
            theString = theString.Replace("&quot;", "\"");
            theString = theString.Replace("&#39;", "\'");
            theString = theString.Replace("<br/> ", "\n");
            return theString;
        }

        public static string DealHtml(string str)
        {
            str = Regex.Replace(str, @"\<(img)[^>]*>|<\/(img)>", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"\<(table|tbody|tr|td|th|)[^>]*>|<\/(table|tbody|tr|td|th|)>", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"\<(div|blockquote|fieldset|legend)[^>]*>|<\/(div|blockquote|fieldset|legend)>", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"\<(font|i|u|h[1-9]|s)[^>]*>|<\/(font|i|u|h[1-9]|s)>", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"\<(style|strong)[^>]*>|<\/(style|strong)>", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"\<a[^>]*>|<\/a>", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"\<(meta|iframe|frame|span|tbody|layer)[^>]*>|<\/(iframe|frame|meta|span|tbody|layer)>", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"\<a[^>]*", "", RegexOptions.IgnoreCase);
            return str;
        }

        public static string CleanHtml(string str)
        {
            Regex reg = new Regex("<[^>]*>");
            str = reg.Replace(str, "");
            return str;
        }

        #endregion

        #region Html Controls
        internal static string GenText(string id, string css, string value, int maxlength, string extraAttrs, Enums.FieldRight fr)
        {
            string fmtModify = "<input id=\"{0}\" value=\"{1}\" type=\"text\" class=\"{2}\"{3} {4}/>";
            string fmtView = "<input id=\"{0}\" value=\"{1}\" type=\"text\" class=\"readonly {2}\" readonly=\"readonly\" {3}/>"; ;

            if (fr == Enums.FieldRight.Modify)
            {
                return string.Format(fmtModify, id, value, css, maxlength == 0 ? "" : string.Concat(" maxlength=\"", maxlength, "\""), extraAttrs);
            }
            else if (fr == Enums.FieldRight.ViewOnly)
            {
                return string.Format(fmtView, "", value, css, extraAttrs.Replace("required=\"1\"", ""));
            }
            else
            {
                return string.Empty;
            }
        }
        public static string GenText(string id, string css, string value, int maxlength, string extraAttrs)
        {
            return GenText(id, css, value, maxlength, extraAttrs, Enums.FieldRight.Modify);
        }

        internal static string GenTextarea(string id, string css, string value, int maxlength, string extraAttrs, Enums.FieldRight vr)
        {
            string fmtModify = "<textarea id=\"{0}\" class=\"{1}\" cols=\"5\" row=\"3\" {2} {3}>{4}</textarea>";
            string fmtView = "<textarea id=\"{0}\" class=\"readonly {1}\" cols=\"5\" row=\"3\" readonly=\"readonly\" {2}>{3}</textarea>";
            if (vr == Enums.FieldRight.Modify)
            {
                return string.Format(fmtModify, id, css, maxlength == 0 ? "" : string.Concat(" maxlength=\"", maxlength, "\""), extraAttrs, value);
            }
            else if (vr == Enums.FieldRight.ViewOnly)
            {
                return string.Format(fmtView, "", css, extraAttrs.Replace("required=\"1\"", ""), value);
            }
            else
            {
                return string.Empty;
            }
        }
        public static string GenTextarea(string id, string css, string value, int maxlength, string extraAttrs)
        {
            return GenTextarea(id, css, value, maxlength, extraAttrs, Enums.FieldRight.Modify);
        }

        internal static string GenCheckBox(string id, string css, string value, bool isChecked, string text, string extraAttrs, Enums.FieldRight vr)
        {
            string fmtModify = "<input id=\"{0}\" type=\"checkbox\"{1} class=\"{2}\"{5} {3}><label for=\"{0}\">{4}</label>";
            string fmtView = "<input id=\"{0}\" type=\"checkbox\"{1} class=\"{2}\" disabled=\"disabled\"{5} {3}><label>{4}</label>";
            if (vr == Enums.FieldRight.Modify)
            {
                return string.Format(fmtModify, id, string.IsNullOrEmpty(value) ? "" : string.Concat(" value=\"", value, "\""), css, extraAttrs, text, isChecked ? " checked=\"checked\"" : "");
            }
            else if (vr == Enums.FieldRight.ViewOnly)
            {
                return string.Format(fmtView, id, string.IsNullOrEmpty(value) ? "" : string.Concat(" value=\"", value, "\""), css, extraAttrs, text, isChecked ? " checked=\"checked\"" : "");
            }
            else
            {
                return string.Empty;
            }
        }
        public static string GenCheckBox(string id, string css, string value, bool isChecked, string text, string extraAttrs)
        {
            return GenCheckBox(id, css, value, isChecked, text, extraAttrs, Enums.FieldRight.Modify);
        }

        public static string GenRadioButtonWithVal(Dictionary<string, string> valTexts, string name, string selected_value, string css, bool sepreateRow)
        {
            string radio = "<input id=\"{0}\" type=\"radio\" class=\"{1}\" name=\"{2}\" value=\"{3}\" {4}><label for=\"{0}\">{5}</label>";
            string hiddenField = string.Concat("<input type=\"hidden\" id=\"", name, "\" value=\"", selected_value, "\" />");
            string jsEvent = string.Concat("onclick=\"o('", name, "').value=this.value;\"");
            StringBuilder sb = new StringBuilder();
            int i = 0;
            string seperator = sepreateRow ? "<br />" : "&nbsp;&nbsp;&nbsp;";
            foreach (KeyValuePair<string, string> k in valTexts)
            {
                if (i != 0)
                    sb.Append(seperator);
                sb.AppendFormat(radio, string.Concat(name, "_", i), css, name, k.Key, string.Concat(selected_value == k.Key ? "checked=\"checked\"" : "", jsEvent), k.Value);
                i++;
            }
            sb.Append(hiddenField);
            return sb.ToString();
        }

        public static string GenRadioButtonForEnum(Type t, string name, string selected_value, string css)
        {
            Dictionary<string, string> d = new Dictionary<string, string>();
            foreach (int i in Enum.GetValues(t))
            {
                d.Add(i.ToString(), Enum.GetName(t, i));
            }
            return GenRadioButtonWithVal(d, name, selected_value, css, false);
        }

        internal static string GenSuggestBox(string id, string css, string value, int maxlength, string extraAttrs, bool enablePopular, Enums.FieldRight fr)
        {
            extraAttrs = string.Concat("enable_popular=\"", enablePopular ? "1" : "0", "\" autocomplete=\"off\" notfound=\"", CommonLibrary.Resources.Resource.SGNotFound, "\" ", extraAttrs);
            return GenText(id, css, value, maxlength, extraAttrs, fr);
        }
        public static string GenSuggestBox(string id, string css, string value, int maxlength, string extraAttrs, bool enablePopular)
        {
            return GenSuggestBox(id, css, value, maxlength, extraAttrs, enablePopular, Enums.FieldRight.Modify);
        }

        public static string GenHiddenField(string id, string value, string extraAttrs)
        {
            return string.Concat("<input type=\"hidden\" id=\"", id, "\" value=\"", value, "\" ", extraAttrs, "/>");
        }

        #endregion
    }
}
