/* 
 *List 基类，包括一些常用方法，方便使用
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
using System.Web.UI.WebControls;
using System.Data;
using CommonLibrary.Utility;
using System.Linq;
using System.Linq.Expressions;
using System.Collections;

namespace CommonLibrary.ObjectBase
{
    public class ListBase<T> : List<T>, IQueryable<T>
        where T : class, new()
    {
        #region Implete IQueryable Interface

        public ListBase()
        {
            _Queryable = this.AsQueryable();
        }

        private IQueryable<T> _Queryable;
        public IQueryable<T> Queryable
        {
            get { return _Queryable; }
            set { _Queryable = value; }
        }

        public Type ElementType
        {
            get { return this.Queryable.ElementType; }
        }

        public System.Linq.Expressions.Expression Expression
        {
            get { return this.Queryable.Expression; }
        }

        public IQueryProvider Provider
        {
            get { return this.Queryable.Provider; }
        }

        #endregion

        #region Convert To DataTable

        public DataTable ToDataTable()
        {
            if (this != null)
                return ListHelper.ListToDataTable(this);
            return null;
        }

        public Table ToTable(params int[] merge_columns)
        {
            if (this != null && this.Count > 0)
                return WebObject.TableHelper.GenListToTable(this, null, null, null, false, merge_columns);
            return null;
        }

        public Table ToTable(string table_css, string header_css, string rows_css, params int[] merge_columns)
        {
            if (this != null && this.Count > 0)
                return WebObject.TableHelper.GenListToTable(this, table_css, header_css, rows_css, false, merge_columns);
            return null;
        }

        public Table ToTable(bool merge_all_columns)
        {
            if (this != null && this.Count > 0)
                return WebObject.TableHelper.GenListToTable(this, null, null, null, merge_all_columns);
            return null;
        }

        #endregion

        #region Sort Functions

        public void SortBy(string column)
        {
            this.Sort(new Utility.Dynamic.DynamicComparer<T>(column));
        }

        public void SortBy(string column, bool asc)
        {
            this.Sort(new Utility.Dynamic.DynamicComparer<T>(column, asc));
        }

        public void SortBy(Dictionary<string, bool> sortList)
        {
            this.Sort(new Utility.Dynamic.DynamicComparer<T>(sortList));
        }

        public void SortBy(object column)
        {
            SortBy(column.ToString());
        }

        public void SortBy(object column, bool asc)
        {
            SortBy(column.ToString(), asc);

        }

        public void SortBy(Dictionary<object, bool> sortList)
        {
            Dictionary<string, bool> d = new Dictionary<string, bool>();
            foreach (KeyValuePair<object, bool> sort in sortList)
            {
                d.Add(sort.Key.ToString(), sort.Value);
            }
            SortBy(d);
        }
        #endregion

        #region Paging Functions

        public ListBase<T> GetPaging(int pageSize, int pageIndex)
        {
            return GetPaging(pageSize, pageIndex, "", true);
        }

        public ListBase<T> GetPaging(int pageSize, int pageIndex, object sortColumn, bool isAsc)
        {
            return GetPaging(pageSize, pageIndex, sortColumn.ToString(), true);
        }

        public ListBase<T> GetPaging(int pageSize, int pageIndex, string sortColumn, bool isAsc)
        {
            ListBase<T> ret = new ListBase<T>();
            if (!string.IsNullOrEmpty(sortColumn))
            {
                this.SortBy(sortColumn, isAsc);
            }
            int index;
            if (this.Count > pageSize)
            {
                for (index = (pageIndex - 1) * pageSize; index < pageSize * pageIndex && index < this.Count; index++)
                {
                    ret.Add(this[index]);
                }
                return ret;
            }
            else
                return this;
        }

        #endregion

    }
}
