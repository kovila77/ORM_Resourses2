using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ORM_Resourses
{
    class DataGridViewComboBoxColumnBuildings : DataGridViewComboBoxColumn
    {
        private DataTable _dtBuildings = null;

        public DataGridViewComboBoxColumnBuildings() : base()
        {
            this.Name = MyHelper.strBuildingId;
            this.HeaderText = "Форпост";
            this.DisplayMember = MyHelper.strBuildingName;
            this.ValueMember = MyHelper.strBuildingId;
            this.DataPropertyName = MyHelper.strBuildingId;
            this.FlatStyle = FlatStyle.Flat;
            InitializeDataTableBuildings();
        }

        public void InitializeDataTableBuildings()
        {
            _dtBuildings = new DataTable();
            _dtBuildings.Columns.Add(MyHelper.strBuildingId, typeof(int));
            _dtBuildings.Columns.Add(MyHelper.strBuildingName, typeof(string));
            this.DataSource = _dtBuildings;
        }

        public void Add(int building_id, string building_name, int? outpost_id)
        {
            //_dtOutposts.Rows.Add(outpost_id, outpost_name, outpost_coordinate_x, outpost_coordinate_y, outpost_coordinate_z);
            _dtBuildings.Rows.Add(building_id, building_name + (outpost_id.HasValue ? " — "
                                             + outpost_id.ToString() : ""));
        }

        public void Change(int building_id, string building_name, int? outpost_id = null)
        {
            DataRow forChange = _dtBuildings.AsEnumerable().SingleOrDefault(row => row.Field<int>(MyHelper.strBuildingId) == building_id);
            if (forChange != null)
            {
                forChange[MyHelper.strBuildingName] = building_name + (outpost_id.HasValue ? " — "
                                                                    + outpost_id.ToString() : "");
            }
        }

        public void Remove(int building_id)
        {
            DataRow forDel = _dtBuildings.AsEnumerable().SingleOrDefault(row => row.Field<int>(MyHelper.strBuildingId) == building_id);
            if (forDel != null)
            {
                _dtBuildings.Rows.Remove(forDel);
            }
        }
    }
}
