using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ORM_Resourses
{
    class DataGridViewComboBoxColumnResources : DataGridViewComboBoxColumn
    {

        private DataTable _dtResources = null;

        public DataGridViewComboBoxColumnResources() : base()
        {
            this.Name = MyHelper.strResourceId;
            this.HeaderText = "Ресурс";
            this.DisplayMember = MyHelper.strResourceName;
            this.ValueMember = MyHelper.strResourceId;
            this.DataPropertyName = MyHelper.strResourceId;
            this.FlatStyle = FlatStyle.Flat;
            InitializeDataTableResources();
        }

        public void InitializeDataTableResources()
        {
            _dtResources = new DataTable();
            _dtResources.Columns.Add(MyHelper.strResourceId, typeof(int));
            _dtResources.Columns.Add(MyHelper.strResourceName, typeof(string));
            this.DataSource = _dtResources;
        }

        public void Add(int resource_id, string resource_name)
        {
            //_dtOutposts.Rows.Add(outpost_id, outpost_name, outpost_coordinate_x, outpost_coordinate_y, outpost_coordinate_z);
            _dtResources.Rows.Add(resource_id, resource_name);
        }

        public void Change(int resource_id, string resource_name)
        {
            DataRow forChange = _dtResources.AsEnumerable().SingleOrDefault(row => row.Field<int>(MyHelper.strResourceId) == resource_id);
            if (forChange != null)
            {
                forChange[MyHelper.strResourceName] = resource_name;
            }
        }

        public void Remove(int resource_id)
        {
            DataRow forDel = _dtResources.AsEnumerable().SingleOrDefault(row => row.Field<int>(MyHelper.strResourceId) == resource_id);
            if (forDel != null)
            {
                _dtResources.Rows.Remove(forDel);
            }
        }
    }
}
