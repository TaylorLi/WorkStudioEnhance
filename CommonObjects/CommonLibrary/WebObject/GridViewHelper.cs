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
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Data;
namespace CommonLibrary.WebObject
{
    public class GridViewHelper
    {
        public static List<DataKey> GetCheckedDataKey(GridView gv, int columnIndex)
        {
            if (gv.DataKeyNames.Length == 0)
            {
                throw new ArgumentNullException("DataKeys", "Not Set GridView DataKeyNames");
            }

            List<DataKey> list = new List<DataKey>();

            int i = 0;
            foreach (GridViewRow gvr in gv.Rows)
            {
                if (gvr.RowType == DataControlRowType.DataRow)
                {
                    foreach (Control c in gvr.Cells[columnIndex].Controls)
                    {
                        if (c is CheckBox && ((CheckBox)c).Checked)
                        {
                            list.Add(gv.DataKeys[i]);
                            break;
                        }
                    }

                    i++;
                }
            }

            return list;
        }

        public static List<DataKey> GetCheckedDataKey(GridView gv, string checkboxId)
        {
            return GetCheckedDataKey(gv, GetColumnIndex(gv, checkboxId));
        }

        public static int GetColumnIndex(GridView gv, string controlId)
        {
            foreach (GridViewRow gvr in gv.Rows)
            {
                for (int i = 0; i < gvr.Cells.Count; i++)
                {
                    foreach (Control c in gvr.Cells[i].Controls)
                    {
                        if (c.ID == controlId)
                        {
                            return i;
                        }
                    }
                }
            }

            return -1;
        }
        /// <summary>
        /// ��ָ�����кϲ�GridView��ͬ���ݵ�������
        /// </summary>
        /// <param name="gv"></param>
        /// <param name="columnIndices">������</param>
        public static void MergeCells(GridView gv, int[] columnIndices)
        {
            int[] aryInt = new int[columnIndices.Length];
            bool[] aryBln = new bool[columnIndices.Length];
            for (int i = 0; i < aryInt.Length; i++)
            {
                aryInt[i] = 0;
            }
            for (int i = 0; i < aryBln.Length; i++)
            {
                aryBln[i] = true;
            }
            for (int i = 1; i < gv.Rows.Count; i++)
            {
                if (gv.Rows[i].RowType == DataControlRowType.DataRow && gv.Rows[i - 1].RowType == DataControlRowType.DataRow)
                {
                    for (int j = 0; j < columnIndices.Length; j++)
                    {
                        if (columnIndices[j] < 0 || columnIndices[j] > gv.Columns.Count - 1) continue;
                        if (gv.Rows[i].Cells[columnIndices[j]].Text == gv.Rows[i - 1].Cells[columnIndices[j]].Text)
                        {
                            if (aryBln[j])
                                aryInt[j] = i - 1;

                            if (gv.Rows[aryInt[j]].Cells[columnIndices[j]].RowSpan == 0)
                                gv.Rows[aryInt[j]].Cells[columnIndices[j]].RowSpan = 1;

                            gv.Rows[aryInt[j]].Cells[columnIndices[j]].RowSpan++;
                            gv.Rows[i].Cells[columnIndices[j]].Visible = false;

                            aryBln[j] = false;
                        }
                        else
                        {
                            aryBln[j] = true;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// ��̬����GridView��Columns
        /// </summary>
        /// <param name="gv">GridView</param>
        /// <param name="dt">�󶨵����ݱ�</param>
        public static void DynamicGenerateColumns(GridView gv, DataTable dt)
        {
            // ��GridView���Զ�����������Ϊfalse,���������ظ���
            gv.AutoGenerateColumns = false;
            gv.Columns.Clear();
            foreach (DataColumn dc in dt.Columns)
            {
                BoundField field = new BoundField();
                field.HeaderText = dc.ColumnName;
                field.DataField = dc.ColumnName;
                gv.Columns.Add(field);
            }
        }
    }
}
