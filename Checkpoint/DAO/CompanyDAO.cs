using Checkpoint.Control;
using Checkpoint.Model;
using Checkpoint.Tools;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Windows.Media.Imaging;

namespace Checkpoint.DAO
{
    class CompanyDAO
    {
        public Boolean saveCompany(Company company)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            String cnpj = company.cnpj.Replace(".", "").Replace("/", "").Replace("-", "").Replace(" ", "");
            String fileName = "company_" + cnpj + ".jpg";

            cmd.CommandText = "INSERT INTO COMPANY (COMPANY_NAME, COMPANY_IMAGE, ID_FATHER_COMPANY, CNPJ, INSCRIPTION, RESPONSIBLE_NAME, " +
                "RESPONSIBLE_OFFICE, RESPONSIBLE_EMAIL, ADDRESS, NEIGHBORHOOD, CITY, STATE, ZIP_CODE, PHONE, CEI, LEEF_NUMBER) " +
                "VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

            cmd.Parameters.Add("COMPANY_NAME", OleDbType.VarChar).Value = company.companyName;
            cmd.Parameters.Add("COMPANY_IMAGE", OleDbType.VarChar).Value = fileName;
            cmd.Parameters.Add("ID_FATHER_COMPANY", OleDbType.Integer).Value = company.fatherCompany != null ? company.fatherCompany.idCompany : 0;
            cmd.Parameters.Add("CNPJ", OleDbType.VarChar).Value = company.cnpj;
            cmd.Parameters.Add("INSCRIPTION", OleDbType.VarChar).Value = company.inscription;
            cmd.Parameters.Add("RESPONSIBLE_NAME", OleDbType.VarChar).Value = company.responsibleName;
            cmd.Parameters.Add("RESPONSIBLE_OFFICE", OleDbType.VarChar).Value = company.responsibleOffice;
            cmd.Parameters.Add("RESPONSIBLE_EMAIL", OleDbType.VarChar).Value = company.responsibleEmail;
            cmd.Parameters.Add("ADDRESS", OleDbType.VarChar).Value = company.address;
            cmd.Parameters.Add("NEIGHBORHOOD", OleDbType.VarChar).Value = company.neighborhood;
            cmd.Parameters.Add("CITY", OleDbType.VarChar).Value = company.city;
            cmd.Parameters.Add("STATE", OleDbType.VarChar).Value = company.state;
            cmd.Parameters.Add("ZIP_CODE", OleDbType.VarChar).Value = company.zipCode;
            cmd.Parameters.Add("PHONE", OleDbType.VarChar).Value = company.phone;
            cmd.Parameters.Add("CEI", OleDbType.VarChar).Value = company.cei;
            cmd.Parameters.Add("LEEF_NUMBER", OleDbType.VarChar).Value = company.leefNumber;

            try
            {
                cmd.ExecuteNonQuery();
                success = true;

                String origin = company.companyImage.UriSource.AbsolutePath.Replace("%20", " ");
                String destiny = ConfigControl.Instance.getImageDirectory() + "/" + fileName;
                destiny = destiny.Replace("\\", "/");

                if (!origin.Equals(destiny))
                {
                    File.Copy(origin, destiny, true);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro ao salvar!" + e);
                success = false;
            }

            DBConnection.getInstance.closeConnection();

            return success;
        }

        public Boolean updateCompany(Company company)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            String cnpj = company.cnpj.Replace(".", "").Replace("/", "").Replace("-", "").Replace(" ", "");
            String fileName = "company_" + cnpj + ".jpg";

            cmd.CommandText = "UPDATE COMPANY SET COMPANY_NAME=?, COMPANY_IMAGE=?, ID_FATHER_COMPANY=?, CNPJ=?, INSCRIPTION=?, RESPONSIBLE_NAME=?, RESPONSIBLE_OFFICE=?, " +
                "RESPONSIBLE_EMAIL=?, ADDRESS=?, NEIGHBORHOOD=?, CITY=?, STATE=?, ZIP_CODE=?, PHONE=?, CEI=?, LEEF_NUMBER=? " +
                "WHERE ID_COMPANY=?";

            cmd.Parameters.Add("COMPANY_NAME", OleDbType.VarChar).Value = company.companyName;
            cmd.Parameters.Add("COMPANY_IMAGE", OleDbType.VarChar).Value = fileName;
            cmd.Parameters.Add("ID_FATHER_COMPANY", OleDbType.Integer).Value = company.fatherCompany != null ? company.fatherCompany.idCompany : 0;
            cmd.Parameters.Add("CNPJ", OleDbType.VarChar).Value = company.cnpj;
            cmd.Parameters.Add("INSCRIPTION", OleDbType.VarChar).Value = company.inscription;
            cmd.Parameters.Add("RESPONSIBLE_NAME", OleDbType.VarChar).Value = company.responsibleName;
            cmd.Parameters.Add("RESPONSIBLE_OFFICE", OleDbType.VarChar).Value = company.responsibleOffice;
            cmd.Parameters.Add("RESPONSIBLE_EMAIL", OleDbType.VarChar).Value = company.responsibleEmail;
            cmd.Parameters.Add("ADDRESS", OleDbType.VarChar).Value = company.address;
            cmd.Parameters.Add("NEIGHBORHOOD", OleDbType.VarChar).Value = company.neighborhood;
            cmd.Parameters.Add("CITY", OleDbType.VarChar).Value = company.city;
            cmd.Parameters.Add("STATE", OleDbType.VarChar).Value = company.state;
            cmd.Parameters.Add("ZIP_CODE", OleDbType.VarChar).Value = company.zipCode;
            cmd.Parameters.Add("PHONE", OleDbType.VarChar).Value = company.phone;
            cmd.Parameters.Add("CEI", OleDbType.VarChar).Value = company.cei;
            cmd.Parameters.Add("LEEF_NUMBER", OleDbType.VarChar).Value = company.leefNumber;
            cmd.Parameters.Add("ID_COMPANY", OleDbType.Integer).Value = company.idCompany;

            try
            {
                cmd.ExecuteNonQuery();
                success = true;
              
                String origin = company.companyImage.UriSource.AbsolutePath.Replace("%20", " ");
                String destiny = ConfigControl.Instance.getImageDirectory() + "/" + fileName;
                destiny = destiny.Replace("\\", "/");

                if (!origin.Equals(destiny))
                {
                    File.Copy(origin, destiny, true);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro ao alterar!" + e);
                success = false;
            }

            DBConnection.getInstance.closeConnection();

            return success;
        }

        public Boolean deleteCompany(Company company)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            cmd.CommandText = "DELETE FROM COMPANY WHERE ID_COMPANY=?";
            cmd.Parameters.Add("ID_COMPANY", OleDbType.Integer).Value = company.idCompany;

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

        public Company getCompany(int idCompany)
        {
            Company company = new Company();

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT * FROM COMPANY WHERE ID_COMPANY=?";
            cmd.Parameters.Add("ID_COMPANY", OleDbType.Integer).Value = idCompany;
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                if (result.Read())
                {
                    company.idCompany = result.GetInt32(0);
                    company.companyName = result.GetString(1);
                    String fileName = Convert.ToString(result[2]);
                    company.fatherCompany = getCompany(Convert.ToInt32(result[3]));
                    company.cnpj = result.GetString(4);
                    company.inscription = result.GetString(5);
                    company.responsibleName = result.GetString(6);
                    company.responsibleOffice = result.GetString(7);
                    company.responsibleEmail = result.GetString(8);
                    company.address = result.GetString(9);
                    company.neighborhood = result.GetString(10);
                    company.city = result.GetString(11);
                    company.state = result.GetString(12);
                    company.zipCode = result.GetString(13);
                    company.phone = result.GetString(14);
                    company.cei = result.GetString(15);
                    company.leefNumber = result.GetString(16);
                    
                    String imageFile = ConfigControl.Instance.getImageDirectory() + "/" + fileName;

                    if (File.Exists(imageFile))
                    {
                        BitmapImage img = new BitmapImage();
                        img.BeginInit();
                        img.CacheOption = BitmapCacheOption.OnLoad;
                        img.UriSource = new Uri(imageFile);
                        img.EndInit();

                        company.companyImage = img;
                    }
                    else
                    {
                        BitmapImage img = new BitmapImage();
                        img.BeginInit();
                        img.CacheOption = BitmapCacheOption.OnLoad;
                        img.UriSource = new Uri(ConfigControl.Instance.getNoImageFile());
                        img.EndInit();

                        company.companyImage = img;
                    }
                }
            }

            result.Close();

            return company;
        }

        public Boolean validateCnpj(String cnpj)
        {
            bool valid = true;
            //cnpj = cnpj.Replace(".", "").Replace("/", "").Replace("-", "").Replace(" ", "");

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT * FROM COMPANY WHERE CNPJ=?";
            cmd.Parameters.Add("CNPJ", OleDbType.VarChar).Value = cnpj;
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                valid = false;
            }

            return valid;
        }

        public List<Company> getAllCompanies()
        {
            List<Company> companies = new List<Company>();

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT * FROM COMPANY";
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    Company company = new Company();
                    company.idCompany = result.GetInt32(0);
                    company.companyName = result.GetString(1);
                    String fileName = Convert.ToString(result[2]);
                    company.fatherCompany = getCompany(Convert.ToInt32(result[3]));
                    company.cnpj = result.GetString(4);
                    company.inscription = result.GetString(5);
                    company.responsibleName = result.GetString(6);
                    company.responsibleOffice = result.GetString(7);
                    company.responsibleEmail = result.GetString(8);
                    company.address = result.GetString(9);
                    company.neighborhood = result.GetString(10);
                    company.city = result.GetString(11);
                    company.state = result.GetString(12);
                    company.zipCode = result.GetString(13);
                    company.phone = result.GetString(14);
                    company.cei = result.GetString(15);
                    company.leefNumber = result.GetString(16);
                    
                    String imageFile = ConfigControl.Instance.getImageDirectory() + "/" + fileName;

                    if (File.Exists(imageFile))
                    {
                        BitmapImage img = new BitmapImage();
                        img.BeginInit();
                        img.CacheOption = BitmapCacheOption.OnLoad;
                        img.UriSource = new Uri(imageFile);
                        img.EndInit();

                        company.companyImage = img;
                    }
                    else
                    {
                        BitmapImage img = new BitmapImage();
                        img.BeginInit();
                        img.CacheOption = BitmapCacheOption.OnLoad;
                        img.UriSource = new Uri(ConfigControl.Instance.getNoImageFile());
                        img.EndInit();

                        company.companyImage = img;
                    }

                    companies.Add(company);
                }
            }

            result.Close();

            return companies;
        }

        public List<Company> getAllFatherCompanies()
        {
            List<Company> companies = new List<Company>();

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT * FROM COMPANY WHERE ID_FATHER_COMPANY=0";
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    Company company = new Company();
                    company.idCompany = result.GetInt32(0);
                    company.companyName = result.GetString(1);
                    String fileName = Convert.ToString(result[2]);
                    company.fatherCompany = getCompany(Convert.ToInt32(result[3]));
                    company.cnpj = result.GetString(4);
                    company.inscription = result.GetString(5);
                    company.responsibleName = result.GetString(6);
                    company.responsibleOffice = result.GetString(7);
                    company.responsibleEmail = result.GetString(8);
                    company.address = result.GetString(9);
                    company.neighborhood = result.GetString(10);
                    company.city = result.GetString(11);
                    company.state = result.GetString(12);
                    company.zipCode = result.GetString(13);
                    company.phone = result.GetString(14);
                    company.cei = result.GetString(15);
                    company.leefNumber = result.GetString(16);

                    String imageFile = ConfigControl.Instance.getImageDirectory() + "/" + fileName;

                    if (File.Exists(imageFile))
                    {
                        BitmapImage img = new BitmapImage();
                        img.BeginInit();
                        img.CacheOption = BitmapCacheOption.OnLoad;
                        img.UriSource = new Uri(imageFile);
                        img.EndInit();

                        company.companyImage = img;
                    }
                    else
                    {
                        BitmapImage img = new BitmapImage();
                        img.BeginInit();
                        img.CacheOption = BitmapCacheOption.OnLoad;
                        img.UriSource = new Uri(ConfigControl.Instance.getNoImageFile());
                        img.EndInit();

                        company.companyImage = img;
                    }

                    companies.Add(company);
                }
            }

            result.Close();

            return companies;
        }

    }
}
