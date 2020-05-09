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
        private DataGridViewComboBoxColumnResources cbcResorcesId = new DataGridViewComboBoxColumnResources();
        private DataGridViewComboBoxColumnBuildings cbcBuldingsId = new DataGridViewComboBoxColumnBuildings();

        private delegate void DeletingResourceHandle(resource res, ref bool cancel);
        private event DeletingResourceHandle DeletinResource;

        public fRConsume()
        {
            InitializeComponent();
            menuStrip1.CausesValidation = false;
            dgvResources.DefaultCellStyle.NullValue = null;
            dgvRConsume.DefaultCellStyle.NullValue = null;
            InitializeDGVResources2();
            InitializeDGVRConsume2();
        }

        private void ReloadData(object sender, EventArgs e)
        {
            dgvResources.CancelEdit();
            dgvRConsume.CancelEdit();
            dgvResources.Columns.Clear();
            InitializeDGVResources2();
            InitializeDGVRConsume2();
        }

        private void InitializeDGVRConsume2()
        {
            using (var ctx = new OpenDataContext())
            {
                cbcBuldingsId.InitializeDataTableBuildings();
                dgvRConsume.Columns.Clear();
                dgvRConsume.Rows.Clear();

                foreach (var build in ctx.buildings)
                {
                    cbcBuldingsId.Add(build.building_id, build.building_name, build.outpost_id);
                }

                dgvRConsume.Columns.Add(cbcBuldingsId);
                dgvRConsume.Columns.Add(cbcResorcesId);
                dgvRConsume.Columns.Add(MyHelper.strConsumeSpeed, "Скорость потребления");
                dgvRConsume.Columns.Add(MyHelper.strSource, "");

                foreach (var brc in ctx.buildings_resources_consume)
                {
                    dgvRConsume.Rows.Add(brc.building_id, brc.resources_id, brc.consume_speed, brc);
                }

                dgvRConsume.Columns[MyHelper.strSource].Visible = false;
            }
        }

        private void InitializeDGVResources2()
        {
            using (var ctx = new OpenDataContext())
            {
                cbcResorcesId.InitializeDataTableResources();
                dgvResources.Columns.Clear();
                dgvResources.Rows.Clear();

                dgvResources.Columns.Add(MyHelper.strResourceName, "Название ресурса");
                dgvResources.Columns.Add(MyHelper.strResourceId, "id");
                dgvResources.Columns.Add(MyHelper.strSource, "");

                foreach (var res in ctx.resources)
                {
                    dgvResources.Rows.Add(res.resources_name, res.resources_id, res);
                    cbcResorcesId.Add(res.resources_id, res.resources_name);
                }

                dgvResources.Columns[MyHelper.strResourceId].Visible = false;
                dgvResources.Columns[MyHelper.strSource].Visible = false;
            }
        }

        private void InitializeDGVRConsume()
        {
            using (var ctx = new OpenDataContext())
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("id", typeof(int));
                dt.Columns.Add("name", typeof(string));

                foreach (var build in ctx.buildings)
                {
                    dt.Rows.Add(build.building_id, build.building_name);
                }

                BindingSource bs = new BindingSource(dt, "");
                cbcBuldingsId.DataSource = bs;

                dgvRConsume.Columns.Add(cbcResorcesId);
                dgvRConsume.Columns.Add(cbcBuldingsId);
                dgvRConsume.Columns.Add("consumeSpeed", "Скорость потребления");
                dgvRConsume.Columns.Add("Source", "Source");

                foreach (var brc in ctx.buildings_resources_consume)
                {
                    int i = dgvRConsume.Rows.Add();
                    dgvRConsume.Rows[i].Cells[0].Value = brc.resources_id;
                    dgvRConsume.Rows[i].Cells[1].Value = brc.building_id;
                    dgvRConsume.Rows[i].Cells[2].Value = brc.consume_speed;
                    dgvRConsume.Rows[i].Cells[3].Value = brc;
                }

                dgvRConsume.Columns["Source"].Visible = false;
            }
        }

        private void InitializeDGVResources()
        {
            using (var ctx = new OpenDataContext())
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("id", typeof(int));
                dt.Columns.Add("name", typeof(string));
                dt.Columns.Add("Source", typeof(resource));

                foreach (var res in ctx.resources)
                {
                    dt.Rows.Add(res.resources_id, res.resources_name, res);
                }

                BindingSource bs = new BindingSource(dt, "");
                cbcResorcesId.DataSource = bs;

                dgvResources.DataSource = dt;

                dgvResources.Columns["name"].HeaderText = "Ресурс";
                dgvResources.Columns["id"].Visible = false;
                dgvResources.Columns["Source"].Visible = false;
            }
        }

        private bool IsResourcesExists(string name, int curRow)
        {
            for (int i = 0; i < dgvResources.Rows.Count - 1; i++)
            {
                if (i == curRow) continue;
                if (dgvResources.Rows[i].Cells["name"].Value.ToString() == name) return true;
            }
            return false;
        }

        private bool IsRConsumeExists(DataGridViewRow row, int except)
        {
            DataGridViewRow curRow;
            if (row.Cells["rId"].Value == null || row.Cells["bId"].Value == null) return false;
            for (int i = 0; i < dgvRConsume.Rows.Count - 1; i++)
            {
                if (i == except) continue;
                curRow = dgvRConsume.Rows[i];
                if (!(curRow.Cells["rId"].Value == null || curRow.Cells["bId"].Value == null)
                    && (int)curRow.Cells["rId"].Value == (int)row.Cells["rId"].Value
                    && (int)curRow.Cells["bId"].Value == (int)row.Cells["bId"].Value)
                    return true;
            }
            return false;
        }

        private bool RowHaveSource(DataGridViewRow row)
        {
            return !(row.Cells["Source"].Value == null || row.Cells["Source"].Value == DBNull.Value);
        }

        private void InsertToDB(DataGridView dgv, DataGridViewRow row)
        {
            using (var ctx = new OpenDataContext())
            {
                if (dgv == dgvResources)
                {
                    resource res = new resource { resources_name = row.Cells["name"].Value.ToString() };
                    ctx.resources.Add(res);
                    ctx.SaveChanges();

                    row.Cells["id"].Value = res.resources_id;
                    row.Cells["Source"].Value = res;
                }
                else
                {
                    buildings_resources_consume brc = new buildings_resources_consume
                    {
                        resources_id = (int)row.Cells["rId"].Value,
                        building_id = (int)row.Cells["bId"].Value,
                        consume_speed = int.Parse(row.Cells["consumeSpeed"].Value.ToString())
                    };
                    ctx.buildings_resources_consume.Add(brc);
                    ctx.SaveChanges();

                    row.Cells["Source"].Value = brc;
                }
            }
        }

        private void UpdateDB(DataGridView dgv, DataGridViewRow row)
        {
            using (var ctx = new OpenDataContext())
            {
                if (dgv == dgvResources)
                {
                    resource res = (resource)row.Cells["Source"].Value;
                    ctx.resources.Attach(res);
                    res.resources_name = row.Cells["name"].Value.ToString();
                }
                else
                {
                    buildings_resources_consume bsc = (buildings_resources_consume)row.Cells["Source"].Value;
                    ctx.buildings_resources_consume.Attach(bsc);
                    if (bsc.building_id != (int)row.Cells["bId"].Value
                        || bsc.resources_id != (int)row.Cells["rId"].Value)
                    {
                        ctx.buildings_resources_consume.Remove(bsc);
                        ctx.SaveChanges();
                        bsc = new buildings_resources_consume
                        {
                            building_id = (int)row.Cells["bId"].Value,
                            resources_id = (int)row.Cells["rId"].Value,
                            consume_speed = Convert.ToInt32(row.Cells["consumeSpeed"].Value)
                        };
                        ctx.buildings_resources_consume.Add(bsc);
                    }
                    else
                    {
                        bsc.consume_speed = Convert.ToInt32(row.Cells["consumeSpeed"].Value);
                    }
                }
                ctx.SaveChanges();
            }
        }

        private bool DeleteFromDB(DataGridView dgv, DataGridViewRow row)
        {
            using (var ctx = new OpenDataContext())
            {
                try
                {
                    if (dgv == dgvResources)
                    {
                        resource res = (resource)row.Cells["Source"].Value;
                        ctx.resources.Attach(res);
                        ctx.resources.Remove(res);
                    }
                    else
                    {
                        buildings_resources_consume bsc = (buildings_resources_consume)row.Cells["Source"].Value;
                        ctx.buildings_resources_consume.Attach(bsc);
                        ctx.buildings_resources_consume.Remove(bsc);
                    }
                    ctx.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Ошибка во время удаления, возможно объект связан с чем-то внутри БД: \n{e.Message}");
                }
            }
            return false;
        }

        private void CellValidating(DataGridView dgv, DataGridViewCellValidatingEventArgs e)
        {
            if (e.RowIndex == dgv.Rows.Count - 1) return;
            if (dgv == dgvRConsume && e.ColumnIndex == 2)
            {
                int t;
                if (e.FormattedValue.ToString().Trim() != "")
                {
                    if (!int.TryParse(e.FormattedValue.ToString(), out t) || t < 0)
                    {
                        dgv.CancelEdit();
                        //e.Cancel = true;
                        SystemSounds.Beep.Play();
                    }
                }
            }
            if (string.IsNullOrWhiteSpace(e.FormattedValue.ToString()) && RowHaveSource(dgv.Rows[e.RowIndex]))
                dgv.CancelEdit();
        }

        private void CellEndEdit(DataGridView dgv, DataGridViewCellEventArgs e)
        {
            if (dgv.Rows[e.RowIndex].IsNewRow) return;
            bool canCommit = true;

            for (int i = 0; i < dgv.Columns.Count - 1; i++)
            {
                var cell = dgv[i, e.RowIndex];
                if (cell.OwningColumn.Visible == false) continue;
                if (cell.Value == null
                    || string.IsNullOrWhiteSpace(cell.FormattedValue.ToString()))
                {
                    canCommit = false;
                    cell.ErrorText = cell.ErrorText.Replace("Пустая ячейка! ", "");
                    cell.ErrorText = "Пустая ячейка! ";
                }
                else
                {
                    cell.ErrorText = cell.ErrorText.Replace("Пустая ячейка! ", "");
                }
            }
            if (!canCommit) return;
            if (dgv == dgvResources)
            {
                if (IsResourcesExists(dgv.Rows[e.RowIndex].Cells["name"].Value.ToString(), e.RowIndex))
                {
                    MessageBox.Show("Ресурс уже существует!");
                    if (RowHaveSource(dgv.Rows[e.RowIndex]))
                        dgv.Rows[e.RowIndex].Cells["name"].Value = ((resource)dgv.Rows[e.RowIndex].Cells["Source"].Value).resources_name;
                    else
                        dgv.Rows.RemoveAt(e.RowIndex);
                    return;
                }
            }
            if (dgv == dgvRConsume)
            {
                if (IsRConsumeExists(dgv.Rows[e.RowIndex], e.RowIndex))
                {
                    MessageBox.Show("Такая комбинация уже существует!");
                    if (RowHaveSource(dgv.Rows[e.RowIndex]))
                    {
                        var brc = (buildings_resources_consume)dgv.Rows[e.RowIndex].Cells["Source"].Value;
                        dgv.Rows[e.RowIndex].Cells["bId"].Value = brc.building_id;
                        dgv.Rows[e.RowIndex].Cells["rId"].Value = brc.resources_id;
                        dgv.Rows[e.RowIndex].Cells["consumeSpeed"].Value = brc.consume_speed;
                    }
                    else
                    {
                        dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = null;
                        dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "Пустая ячейка! ";
                    }
                    return;
                }
            }

            if (RowHaveSource(dgv.Rows[e.RowIndex]))
            {
                UpdateDB(dgv, dgv.Rows[e.RowIndex]);
            }
            else
            {
                InsertToDB(dgv, dgv.Rows[e.RowIndex]);
            }
        }

        private bool dgvRConsumeContainRes(int idRes)
        {
            for (int i = 0; i < dgvRConsume.Rows.Count - 1; i++)
            {
                if (dgvRConsume.Rows[i].Cells["rId"].Value != null && (int)dgvRConsume.Rows[i].Cells["rId"].Value == idRes) return true;
            }
            return false;
        }

        private void dgvResources_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (e.Row.Cells[MyHelper.strResourceId].Value != null)
            {
                try
                {
                    var res = (resource)e.Row.Cells[MyHelper.strSource].Value;
                    bool cancel = false;
                    DeletinResource(res, ref cancel);

                    if (cancel)
                    {
                        MessageBox.Show("Невозможно удалить ресурс, который используется!");
                        return;
                    }

                    using (var ctx = new OpenDataContext())
                    {
                        ctx.resources.Attach(res);
                        ctx.resources.Remove(res);
                        ctx.SaveChanges();
                        cbcResorcesId.Remove(res.resources_id);
                    }
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }
            }
        }
        private void dgvRConsume_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (e.Row.Cells[MyHelper.strSource].Value != null)
            {
                try
                {
                    var brc = (buildings_resources_consume)e.Row.Cells[MyHelper.strSource].Value;

                    using (var ctx = new OpenDataContext())
                    {
                        ctx.buildings_resources_consume.Attach(brc);
                        ctx.buildings_resources_consume.Remove(brc);
                        ctx.SaveChanges();
                    }
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }
            }
        }

        private void dgvResources_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvResources.Rows[e.RowIndex].IsNewRow
                || !dgvResources.Columns[e.ColumnIndex].Visible) return;

            //var row = dgvResources.Rows[e.RowIndex];
            var cell = dgvResources[e.ColumnIndex, e.RowIndex];
            var cellFormatedValue = cell.FormattedValue.ToString().RmvExtrSpaces();

            if (cell.ValueType == typeof(String))
            {
                cell.Value = cellFormatedValue;
            }
        }

        private void dgvRConsume_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvRConsume.Rows[e.RowIndex].IsNewRow
                || !dgvRConsume.Columns[e.ColumnIndex].Visible) return;

            //var row = dgvResources.Rows[e.RowIndex];
            var cell = dgvRConsume[e.ColumnIndex, e.RowIndex];
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
            int t;

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
            else
            {
                cell.ErrorText = "";
            }
        }

        private void CancelEdit(object sender, EventArgs e)
        {
            return;

            switch (tabControl.SelectedIndex)
            {
                case 0:
                    dgvResources.CancelEdit();
                    break;
                case 1:
                    dgvRConsume.CancelEdit();
                    break;
            }
        }
    }
}

