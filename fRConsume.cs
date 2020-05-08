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
        private DataGridViewComboBoxColumn cbResorcesId;
        private DataGridViewComboBoxColumn cbBuldingsId;

        public fRConsume()
        {
            InitializeComponent();
            menuStrip1.CausesValidation = false;
            cbResorcesId = new DataGridViewComboBoxColumn()
            {
                Name = "rId",
                HeaderText = "Ресурс",
                DisplayMember = "name",
                ValueMember = "id"
            };
            cbBuldingsId = new DataGridViewComboBoxColumn()
            {
                Name = "bId",
                HeaderText = "Здание",
                DisplayMember = "name",
                ValueMember = "id"
            };
            InitializeDGVResources();
            InitializeDGVRConsume();
        }

        private void ReloadData(object sender, EventArgs e)
        {
            dgvResources.CancelEdit();
            dgvRConsume.CancelEdit();
            dgvRConsume.Columns.Clear();
            dgvResources.Columns.Clear();
            InitializeDGVResources();
            InitializeDGVRConsume();
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
                cbBuldingsId.DataSource = bs;

                dgvRConsume.Columns.Add(cbResorcesId);
                dgvRConsume.Columns.Add(cbBuldingsId);
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
                cbResorcesId.DataSource = bs;

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
            var id = (int)e.Row.Cells["id"].Value;
            if (!dgvRConsumeContainRes(id))
            {
                e.Cancel = !DeleteFromDB(dgvResources, e.Row);
            }
            else
            {
                if (MessageBox.Show("Данный ресурс связан с существующей таблицей! Удалить все связанные объекты?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    List<DataGridViewRow> rowsForDel = new List<DataGridViewRow>();
                    foreach (DataGridViewRow row in dgvRConsume.Rows)
                    {
                        if (row.Cells["rId"].Value != null && (int)row.Cells["rId"].Value == id)
                        {
                            //if (RowHaveSource(row))
                            //    dgvRConsume_UserDeletingRow(null, new DataGridViewRowCancelEventArgs(row));
                            rowsForDel.Add(row);
                        }
                    }
                    foreach (DataGridViewRow row in rowsForDel) dgvRConsume.Rows.Remove(row);
                    e.Cancel = !DeleteFromDB(dgvResources, e.Row);
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }
        private void dgvRConsume_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (RowHaveSource(e.Row))
                e.Cancel = !DeleteFromDB(dgvRConsume, e.Row);
        }

        private void dgvResources_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            CellEndEdit(dgvResources, e);
        }
        private void dgvRConsume_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            CellEndEdit(dgvRConsume, e);
        }

        private void dgvRConsume_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            CellValidating(dgvRConsume, e);
        }
        private void dgvResources_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            CellValidating(dgvResources, e);
        }

        private void CancelEdit(object sender, EventArgs e)
        {
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

