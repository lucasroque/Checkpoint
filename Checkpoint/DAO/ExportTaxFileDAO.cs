using Checkpoint.Model;
using Checkpoint.Tools;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace Checkpoint.DAO
{
    class ExportTaxFileDAO
    {

        public List<String> getAllHeaders(DateTime startDate, DateTime endDate)
        {
            List<String> headers = new List<String>();

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT CNPJ, CEI, COMPANY_NAME FROM COMPANY";
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    String cnpj = result.GetString(0);
                    cnpj = cnpj.Replace(".", "").Replace("/", "").Replace("-", "").Replace(" ", "");
                    String cei = !"".Equals(result.GetString(1)) ? result.GetString(1) : "000000000000";
                    String companyName = result.GetString(2);

                    String header = "11" + cnpj + cei.PadLeft(12, '0') + companyName.PadRight(150) + startDate.ToString("ddMMyyyy") + endDate.ToString("ddMMyyyy") + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("hhmm");

                    headers.Add(header);
                }
            }

            return headers;
        }

        public String getCompanyHeader(int idCompany, DateTime startDate, DateTime endDate)
        {
            String header = "";

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT CNPJ, CEI, COMPANY_NAME FROM COMPANY WHERE ID_COMPANY=?";
            cmd.Parameters.Add("ID_COMPANY", OleDbType.Integer).Value = idCompany;
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                if (result.Read())
                {
                    String cnpj = result.GetString(0);
                    cnpj = cnpj.Replace(".", "").Replace("/", "").Replace("-", "").Replace(" ", "");
                    String cei = !"".Equals(result.GetString(1)) ? result.GetString(1) : "000000000000";
                    String companyName = result.GetString(2);

                    header = "11" + cnpj + cei.PadLeft(12, '0') + companyName.PadRight(150) + startDate.ToString("ddMMyyyy") + endDate.ToString("ddMMyyyy") + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("hhmm");
                }
            }

            return header;
        }

        public List<String> getAFDTDetails(DateTime startDate, DateTime endDate, Company company, Department department)
        {
            List<String> details = new List<String>();

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            string commandText = "SELECT M.MARKING_DAY, M.MARKING_MONTH, M.MARKING_YEAR, M.MARKING_HOUR, M.MARKING_MINUTE, M.PIS_PASEP " +
                 "FROM(((MARKING M INNER JOIN EMPLOYEE E ON M.PIS_PASEP = E.PIS_PASEP) " +
                 "INNER JOIN COMPANY C ON E.ID_COMPANY = C.ID_COMPANY) " +
                 "INNER JOIN DEPARTMENT D ON E.ID_DEPARTMENT = D.ID_DEPARTMENT)";

            Boolean where = false;

            if (company != null && !company.companyName.Equals("Todos"))
            {
                commandText += " WHERE ID_COMPANY=?";
                cmd.Parameters.Add("ID_COMPANY", OleDbType.Integer).Value = company.idCompany;
                where = true;
            }

            if (department != null && !department.description.Equals("Todos"))
            {
                commandText += where ? " AND" : " WHERE";
                commandText += " ID_DEPARTMENT=?";
                cmd.Parameters.Add("ID_DEPARTMENT", OleDbType.Integer).Value = department.idDepartment;
                where = true;
            }

            cmd.CommandText = commandText;
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                }
            }

            return details;
        }

        public List<String> getCJEFDetails(DateTime startDate, DateTime endDate, Company company, Department department)
        {
            List<String> details = new List<String>();

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            string commandText = "SELECT M.MARKING_DAY, M.MARKING_MONTH, M.MARKING_YEAR, M.MARKING_HOUR, M.MARKING_MINUTE, M.PIS_PASEP " +
                 "FROM(((MARKING M INNER JOIN EMPLOYEE E ON M.PIS_PASEP = E.PIS_PASEP) " +
                 "INNER JOIN COMPANY C ON E.ID_COMPANY = C.ID_COMPANY) " +
                 "INNER JOIN DEPARTMENT D ON E.ID_DEPARTMENT = D.ID_DEPARTMENT)";

            Boolean where = false;

            if (company != null && !company.companyName.Equals("Todos"))
            {
                commandText += " WHERE ID_COMPANY=?";
                cmd.Parameters.Add("ID_COMPANY", OleDbType.Integer).Value = company.idCompany;
                where = true;
            }

            if (department != null && !department.description.Equals("Todos"))
            {
                commandText += where ? " AND" : " WHERE";
                commandText += " ID_DEPARTMENT=?";
                cmd.Parameters.Add("ID_DEPARTMENT", OleDbType.Integer).Value = department.idDepartment;
                where = true;
            }

            cmd.CommandText = commandText;
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                }
            }

            return details;
        }

        public List<String> getSchedules()
        {
            List<String> schedules = new List<String>();

            return schedules;
        }
    }
}
