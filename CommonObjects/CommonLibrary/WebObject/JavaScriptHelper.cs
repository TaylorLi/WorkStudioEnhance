/* 
 * 
 *  
 * �������ż���   
 * ʱ�䣺2011��9��5��
 * 
 * �޸ģ�[����]         
 * ʱ�䣺[ʱ��]             
 * ˵����[˵��]
 * 
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

namespace CommonLibrary.WebObject
{
    public class JavaScriptHelper
    {
        public static void RegisterAlertScript(string message, string navigateTo, string key, Page page)
        {
            string script = @"alert('" + message + @"');window.navigate('" + navigateTo + @"');";
            page.ClientScript.RegisterClientScriptBlock(page.GetType(), page.UniqueID + key, script, true);
        }

        public static void RegisterConfirmScript(string message, string YesNavigateTo, string NoNavigateTo, string key, Page page)
        {
            string script = @"if(confirm('" + message + @"'))
                     window.href='" + YesNavigateTo + @"');
                   else
                     window.href='" + NoNavigateTo + @"')";
            page.ClientScript.RegisterClientScriptBlock(page.GetType(), page.UniqueID + key, script, true);
        }

        public static void RegisterAlertAndBackScript(string message, string key, Page page)
        {
            string script = @"alert('" + message + @"');history.back();";
            page.ClientScript.RegisterClientScriptBlock(page.GetType(), page.UniqueID + key, script, true);
        }        
    }
}
