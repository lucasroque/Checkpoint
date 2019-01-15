using Checkpoint.Control;
using Checkpoint.Model;
using Checkpoint.Tools;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace Checkpoint.DAO
{
    class StructureDAO
    {
        CompanyControl companyControl = new CompanyControl();
        EmployeeControl employeeControl = new EmployeeControl();

        public Boolean saveStructure(Structure structure)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            cmd.CommandText = "INSERT INTO STRUCTURE (ID_COMPANY, DESCRIPTION, INSIDE, ID_RESPONSIBLE) VALUES (?,?,?,?)";

            cmd.Parameters.Add("ID_COMPANY", OleDbType.Integer).Value = structure.company != null ? structure.company.idCompany : 0;
            cmd.Parameters.Add("DESCRIPTION", OleDbType.VarChar).Value = structure.description;
            cmd.Parameters.Add("INSIDE", OleDbType.VarChar).Value = structure.inside;
            cmd.Parameters.Add("ID_RESPONSIBLE", OleDbType.Integer).Value = structure.responsible != null ? structure.responsible.idEmployee : 0;

            try
            {
                cmd.ExecuteNonQuery();
                success = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro ao salvar!" + e);
                success = false;
            }

            DBConnection.getInstance.closeConnection();

            return success;
        }

        public Boolean updateStructure(Structure structure)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            cmd.CommandText = "UPDATE STRUCTURE SET ID_COMPANY=?, DESCRIPTION=?, INSIDE=?, ID_RESPONSIBLE=? WHERE ID_STRUCTURE=?";

            cmd.Parameters.Add("ID_COMPANY", OleDbType.Integer).Value = structure.company != null ? structure.company.idCompany : 0;
            cmd.Parameters.Add("DESCRIPTION", OleDbType.VarChar).Value = structure.description;
            cmd.Parameters.Add("INSIDE", OleDbType.VarChar).Value = structure.inside;
            cmd.Parameters.Add("RESPONSIBLE", OleDbType.VarChar).Value = structure.responsible;
            cmd.Parameters.Add("ID_RESPONSIBLE", OleDbType.Integer).Value = structure.responsible != null ? structure.responsible.idEmployee : 0;

            try
            {
                cmd.ExecuteNonQuery();
                success = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro ao alterar!" + e);
                success = false;
            }

            DBConnection.getInstance.closeConnection();

            return success;
        }

        public Boolean deleteStructure(Structure structure)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            cmd.CommandText = "DELETE FROM STRUCTURE WHERE ID_STRUCTURE=?";
            cmd.Parameters.Add("ID_STRUCTURE", OleDbType.Integer).Value = structure.idStructure;

            try
            {
                cmd.ExecuteNonQuery();
                success = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro ao excluir!" + e);
                success = false;
            }

            DBConnection.getInstance.closeConnection();

            return success;
        }

        public List<Structure> getAllStructures()
        {
            List<Structure> structures = new List<Structure>();

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT * FROM STRUCTURE";
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    Structure structure = new Structure();
                    structure.idStructure = result.GetInt32(0);
                    structure.company = companyControl.getCompany(result.GetInt32(1));
                    structure.description = result.GetString(2);
                    structure.inside = result.GetString(3);
                    structure.responsible = employeeControl.getEmployee(result.GetInt32(4));

                    structures.Add(structure);
                }
            }

            result.Close();

            return structures;
        }
    }
}
