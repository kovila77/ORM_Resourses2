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
        private void InitializeDGVResources()
        {
            using (var ctx = new OpenDataContext())
            {
                dgvRConsume.CancelEdit();
                dgvResources.Rows.Clear();
                dgvResources.Columns.Clear();
                dgvResources.DefaultCellStyle.NullValue = null;
                cbcResorcesId.InitializeDataTableResources();

                dgvResources.Columns.Add(MyHelper.strResourceName, "Название ресурса");
                dgvResources.Columns.Add(MyHelper.strResourceId, "id");
                dgvResources.Columns.Add(MyHelper.strSource, "");

                dgvResources.Columns[MyHelper.strResourceName].ValueType = typeof(string);
                dgvResources.Columns[MyHelper.strResourceId].ValueType = typeof(int);
                dgvResources.Columns[MyHelper.strSource].ValueType = typeof(resource);

                dgvResources.Columns[MyHelper.strResourceId].Visible = false;
                dgvResources.Columns[MyHelper.strSource].Visible = false;


                foreach (var res in ctx.resources)
                {
                    dgvResources.Rows.Add(res.resources_name, res.resources_id, res);
                    cbcResorcesId.Add(res.resources_id, res.resources_name);
                }
            }
        }

        private void dgvResources_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            var row = dgvResources.Rows[e.RowIndex];
            if (row.IsNewRow || !dgvResources.IsCurrentRowDirty) return;
            //var cell = dgvOutposts[e.ColumnIndex, e.RowIndex];
            //var cellFormatedValue = cell.FormattedValue.ToString().RmvExtrSpaces();

            // Проверка можно ли фиксировать строку
            var cellWithPotentialError = dgvResources[MyHelper.strResourceName, e.RowIndex];
            if (cellWithPotentialError.FormattedValue.ToString().RmvExtrSpaces() == "")
            {
                cellWithPotentialError.ErrorText = MyHelper.strEmptyCell;
                row.ErrorText = MyHelper.strBadRow;
            }
            else
            {
                cellWithPotentialError.ErrorText = "";
                row.ErrorText = "";
            }

            try
            {
                using (var ctx = new OpenDataContext())
                {
                    if (row.HaveSource())
                    {
                        var new_res = (resource)row.Cells[MyHelper.strSource].Value;
                        ctx.resources.Attach(new_res);

                        string new_resources_name = (string)row.Cells[MyHelper.strResourceName].Value;

                        if (ctx.resources.AsEnumerable().FirstOrDefault(res => res != new_res && res.resources_name.ToLower() == new_resources_name.ToLower()) != null)
                        {
                            string eo = $"Ресурс {new_resources_name} уже существует!";
                            MessageBox.Show(eo);
                            row.ErrorText = MyHelper.strBadRow + " " + eo;
                            return;
                        }

                        new_res.resources_name = new_resources_name;

                        ctx.SaveChanges();
                    }
                    else
                    {
                        string new_resources_name = (string)row.Cells[MyHelper.strResourceName].Value;

                        if (ctx.resources.AsEnumerable().FirstOrDefault(res => res.resources_name.ToLower() == new_resources_name.ToLower()) != null)
                        {
                            string eo = $"Ресурс {new_resources_name} уже существует!";
                            MessageBox.Show(eo);
                            row.ErrorText = MyHelper.strBadRow + " " + eo;
                            return;
                        }

                        var new_res = new resource();
                        new_res.resources_name = new_resources_name;
                        ctx.resources.Add(new_res);
                        ctx.SaveChanges();
                        row.Cells[MyHelper.strSource].Value = new_res;
                        row.Cells[MyHelper.strResourceId].Value = new_res.resources_id;
                    }
                }
            }
            catch (Exception err2)
            {
                MessageBox.Show(err2.Message);
            }
        }

        private void dgvResources_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (e.Row.Cells[MyHelper.strSource].Value != null)
            {
                try
                {
                    using (var ctx = new OpenDataContext())
                    {
                        var res = (resource)e.Row.Cells[MyHelper.strSource].Value;
                        ctx.resources.Attach(res);

                        bool cancel = res.buildings_resources_consume.Count > 0;

                        if (cancel)
                        {
                            MessageBox.Show("Невозможно удалить ресурс, который используется!");
                            e.Cancel = true;
                            return;
                        }

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
    }
}
