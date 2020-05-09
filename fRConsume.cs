using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ORM_Resourses
{
    public partial class fRConsume : Form
    {
        private int currentTab = 0;
        private DataGridViewComboBoxColumnResources cbcResorcesId = new DataGridViewComboBoxColumnResources();
        private DataGridViewComboBoxColumnBuildings cbcBuldingsId = new DataGridViewComboBoxColumnBuildings();

        private delegate void InitializeDGV();

        public fRConsume()
        {
            InitializeComponent();

            tabControl.TabPages[0].Tag = new InitializeDGV(InitializeDGVResources);
            tabControl.TabPages[1].Tag = new InitializeDGV(InitializeDGVRConsume);

            InitializeDGVResources();
            InitializeDGVRConsume();
        }

        private void ReloadData(object sender, EventArgs e)
        {
            ((InitializeDGV)tabControl.TabPages[currentTab].Tag).Invoke();
        }
        private void tabControl_Selecting(object sender, TabControlCancelEventArgs e)
        {
            currentTab = tabControl.SelectedIndex;
        }

        private void dgv_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (sender == null) return;
            DataGridView dgv = null;
            try { dgv = (DataGridView)sender; } catch { return; }
            if (dgv.Rows[e.RowIndex].IsNewRow
                || !dgv.Columns[e.ColumnIndex].Visible) return;

            //var row = dgvResources.Rows[e.RowIndex];
            var cell = dgv[e.ColumnIndex, e.RowIndex];
            var cellFormatedValue = cell.FormattedValue.ToString().RmvExtrSpaces();

            if (cell.ValueType == typeof(String))
            {
                cell.Value = cellFormatedValue;
            }
        }

        private void dgv_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            //var row = dgvOutposts.Rows[e.RowIndex];
            if (sender == null) return;
            DataGridView dgv = null;
            try { dgv = (DataGridView)sender; } catch { return; }
            if (dgv.Rows[e.RowIndex].IsNewRow || !dgv[e.ColumnIndex, e.RowIndex].IsInEditMode) return;

            var cell = dgv[e.ColumnIndex, e.RowIndex];
            var cellFormatedValue = e.FormattedValue.ToString().RmvExtrSpaces();
            int t = -1;

            if (dgv.Columns[e.ColumnIndex].CellType != typeof(DataGridViewComboBoxCell)
                && dgv.Columns[e.ColumnIndex].ValueType == typeof(Int32)
                && !int.TryParse(cellFormatedValue, out t))
            {
                if (cellFormatedValue == "" || MessageBox.Show(MyHelper.strUncorrectIntValueCell + $"\n\"{cellFormatedValue}\"\nОтменить изменения?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    dgv.CancelEdit();
                else
                    e.Cancel = true;
                return;
            }
            else if (cell.OwningColumn.Name == MyHelper.strConsumeSpeed && t < 0)
            {
                if (MessageBox.Show(MyHelper.strUncorrectIntValueZeroCell + $"\n\"{cellFormatedValue}\"\nОтменить изменения?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    dgv.CancelEdit();
                else
                    e.Cancel = true;
                return;
            }
            else
            {
                cell.ErrorText = "";
            }
        }

    }
}

