﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ORM_Resourses
{
    public static class MyHelper
    {
        public static readonly string strEmptyCell = "Вы ввели пустое значение!";
        public static readonly string strUncorrectIntValueCell = "Вы ввели некорректное число!";
        public static readonly string strExistingMision = "Для данного форпоста существует точная копия ведённой миссии!";
        public static readonly string strBadRow = "Плохая строка!";
        public static readonly string strOutpostIsUsing = "Данный форпост связан с миссией! Измените или удалите миссию!";

        public static readonly string strSource = "Source";

        public static readonly string strOutpostId = "outpost_id";
        public static readonly string strOutpostName = "outpost_name";
        public static readonly string strOutpostEconomicValue = "outpost_economic_value";
        public static readonly string strOutpostCoordinateX = "outpost_coordinate_x";
        public static readonly string strOutpostCoordinateY = "outpost_coordinate_y";
        public static readonly string strOutpostCoordinateZ = "outpost_coordinate_z";

        public static readonly string strMissionId = "mission_id";
        public static readonly string strMissionDescription = "mission_description";
        public static readonly string strDateBegin = "date_begin";
        public static readonly string strDatePlanEnd = "date_plan_end";
        public static readonly string strDateActualEnd = "date_actual_end";

        public static readonly string strBuildingId = "building_id";
        public static readonly string strBuildingName = "building_name";

        public static readonly string strResourceId = "resources_id";
        public static readonly string strResourceName = "resources_name";

        public static readonly string strConsumeSpeed = "consume_speed";

        //public static readonly string strUniqueOutpostConstraintName = "unique_outpost";

        public static string RmvExtrSpaces(this String str)
        {
            if (str == null) return str;
            str = str.Trim();
            str = Regex.Replace(str, @"\s+", " ");
            return str;
        }

        public static bool HaveSource(this DataGridViewRow row)
        {
            return row.Cells[strSource].Value != null && row.Cells[strSource].Value != DBNull.Value;
        }

        public static bool IsEntireRowEmpty(DataGridViewRow row)
        {
            foreach (DataGridViewCell cell in row.Cells)
                if (cell.FormattedValue.ToString().RmvExtrSpaces() != "")
                    return false;
            return true;
        }
    }
}
