using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ORM_Resourses
{
    public partial class fRConsume : Form
    {
        private void InitializeDGVRConsume()
        {
            dgvRConsume.CancelEdit();
            dgvRConsume.Rows.Clear();
            dgvRConsume.Columns.Clear();
            dgvRConsume.DefaultCellStyle.NullValue = null;
            cbcBuldingsId.InitializeDataTableBuildings();

            dgvRConsume.Columns.Add(cbcBuldingsId);
            dgvRConsume.Columns.Add(cbcResorcesId);
            dgvRConsume.Columns.Add(MyHelper.strConsumeSpeed, "Скорость потребления");
            dgvRConsume.Columns.Add(MyHelper.strSource, "");

            dgvRConsume.Columns[MyHelper.strBuildingId].ValueType = typeof(int);
            dgvRConsume.Columns[MyHelper.strResourceId].ValueType = typeof(int);
            dgvRConsume.Columns[MyHelper.strConsumeSpeed].ValueType = typeof(int);
            dgvRConsume.Columns[MyHelper.strSource].ValueType = typeof(buildings_resources_consume);

            try
            {
                using (var ctx = new OpenDataContext())
                {
                    foreach (var build in ctx.buildings)
                    {
                        cbcBuldingsId.Add(build.building_id, build.building_name, build.outpost_id);
                    }

                    foreach (var brc in ctx.buildings_resources_consume)
                    {
                        dgvRConsume.Rows.Add(brc.building_id, brc.resources_id, brc.consume_speed, brc);
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
#if DEBUG
                throw err;
#endif
            }
            dgvRConsume.Columns[MyHelper.strSource].Visible = false;
        }

        private void dgvRConsume_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            var row = dgvRConsume.Rows[e.RowIndex];
            if (row.IsNewRow || !dgvRConsume.IsCurrentRowDirty) return;
            //var cell = dgvOutposts[e.ColumnIndex, e.RowIndex];
            //var cellFormatedValue = cell.FormattedValue.ToString().RmvExtrSpaces();

            // Проверка можно ли фиксировать строку
            var cellsWithPotentialErrors = new List<DataGridViewCell> {
                                                   row.Cells[MyHelper.strBuildingId],
                                                   row.Cells[MyHelper.strResourceId],
                                                   row.Cells[MyHelper.strConsumeSpeed],
                                                 };
            foreach (var cellWithPotentialError in cellsWithPotentialErrors)
            {
                if (cellWithPotentialError.FormattedValue.ToString().RmvExtrSpaces() == "")
                {
                    cellWithPotentialError.ErrorText = MyHelper.strEmptyCell;
                    row.ErrorText = MyHelper.strBadRow;
                }
                else
                {
                    cellWithPotentialError.ErrorText = "";
                }
            }
            if (cellsWithPotentialErrors.FirstOrDefault(cellWithPotentialError => cellWithPotentialError.ErrorText.Length > 0) == null)
                row.ErrorText = "";
            else
                return;

            try
            {
                using (var ctx = new OpenDataContext())
                {
                    if (row.HaveSource())
                    {
                        var new_brc = (buildings_resources_consume)row.Cells[MyHelper.strSource].Value;
                        ctx.buildings_resources_consume.Attach(new_brc);

                        int new_building_id = (int)row.Cells[MyHelper.strBuildingId].Value;
                        int new_resource_id = (int)row.Cells[MyHelper.strResourceId].Value;
                        int new_consume_speed = (int)row.Cells[MyHelper.strConsumeSpeed].Value;

                        if (ctx.buildings_resources_consume.AsEnumerable().FirstOrDefault(res =>
                                    res != new_brc
                                    && res.building_id == new_building_id
                                    && res.resources_id == new_resource_id) != null)
                        {
                            string eo = $"Для данного здания потребляемый ресурс уже существует!";
                            MessageBox.Show(eo);
                            row.ErrorText = MyHelper.strBadRow + " " + eo;
                            return;
                        }

                        new_brc.resources_id = new_resource_id;
                        new_brc.building_id = new_building_id;
                        new_brc.consume_speed = new_consume_speed;

                        ctx.SaveChanges();
                    }
                    else
                    {
                        int new_building_id = (int)row.Cells[MyHelper.strBuildingId].Value;
                        int new_resource_id = (int)row.Cells[MyHelper.strResourceId].Value;
                        int new_consume_speed = (int)row.Cells[MyHelper.strConsumeSpeed].Value;

                        if (ctx.buildings_resources_consume.AsEnumerable().FirstOrDefault(res =>
                                    res.building_id == new_building_id
                                    && res.resources_id == new_resource_id) != null)
                        {
                            string eo = $"Для данного здания потребляемый ресурс уже существует!";
                            MessageBox.Show(eo);
                            row.ErrorText = MyHelper.strBadRow + " " + eo;
                            return;
                        }

                        var new_brc = new buildings_resources_consume();
                        new_brc.resources_id = new_resource_id;
                        new_brc.building_id = new_building_id;
                        new_brc.consume_speed = new_consume_speed;

                        ctx.buildings_resources_consume.Add(new_brc);

                        ctx.SaveChanges();

                        row.Cells[MyHelper.strSource].Value = new_brc;
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
#if DEBUG
                throw err;
#endif
            }
        }

        private void dgvRConsume_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (e.Row.Cells[MyHelper.strSource].Value != null)
            {
                try
                {
                    using (var ctx = new OpenDataContext())
                    {
                        var brc = (buildings_resources_consume)e.Row.Cells[MyHelper.strSource].Value;
                        ctx.buildings_resources_consume.Attach(brc);
                        ctx.buildings_resources_consume.Remove(brc);
                        ctx.SaveChanges();
                    }
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
#if DEBUG
                    throw err;
#endif
                }
            }
        }
    }
}
