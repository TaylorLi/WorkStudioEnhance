/* 
 * 控件基础类
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
using System.IO;
using System.Web.UI;

namespace CommonLibrary.WebObject
{
    public class TemplateControl : System.Web.UI.UserControl
    {
        private string _NomarlCellStyle = "td";
        public string NomarlCellStyle
        {
            get { return _NomarlCellStyle; }
            set { _NomarlCellStyle = value; }
        }

        private string _AlternateCellStyle = "td2";
        public string AlternateCellStyle
        {
            get { return _AlternateCellStyle; }
            set { _AlternateCellStyle = value; }
        }

        private static bool _css = true;
        public string GetCss(bool change)
        {
            if (!change) _css = !_css;
            if (_css)
            {
                _css = false;
                return _NomarlCellStyle;
            }
            else
            {
                _css = true;
                return _AlternateCellStyle;
            }
        }

        public string Html
        {
            get
            {
                StringWriter sw = new StringWriter();
                HtmlTextWriter htmltw = new HtmlTextWriter(sw);
                OnLoad(null);
                base.Render(htmltw);
                System.Text.StringBuilder html = sw.GetStringBuilder();
                return html.ToString();
            }
        }

        public Control GetControl(string id)
        {
            return ControlHelper.GetControl(this.Page.Controls, null, id);
        }

        public T GetControl<T>(string id) where T : class, new()
        {
            return ControlHelper.GetControl<T>(this.Controls, id);
        }

        public static void SetValues<T>(T o, string prefix)
            where T : class, new()
        {
            WebHelper.SetValues(o, prefix);
        }
    }
}
