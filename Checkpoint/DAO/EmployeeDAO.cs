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
    class EmployeeDAO
    {
        CompanyControl companyControl = new CompanyControl();
        ScheduleControl scheduleControl = new ScheduleControl();
        DepartmentControl departmentControl = new DepartmentControl();
        OfficeControl officeControl = new OfficeControl();
        ResignationReasonControl resignationReasonControl = new ResignationReasonControl();

        public Boolean saveEmployee(Employee employee)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            String pisPasep = employee.pisPasep.Replace(".", "").Replace("-", "").Replace(" ", "");
            String fileName = "employee_" + pisPasep + ".jpg";

            cmd.CommandText = "INSERT INTO EMPLOYEE (EMPLOYEE_NAME, EMPLOYEE_IMAGE, PIS_PASEP, ID_COMPANY, ID_SCHEDULE, " +
                "LEEF_NUMBER, CTPS, ID_DEPARTMENT, ID_OFFICE, ADMISSION, RESIGNATION, ID_RESIGNATION_REASON, PHONE, EMAIL, GENERAL_REGISTRY, " +
                "REGISTRY_ENTITY, FATHER, MOTHER, BIRTH, GENDER, CIVIL_STATUS, NATIONALITY, NATURALNESS, ADDRESS, NEIGHBORHOOD, " +
                "CITY, STATE, ZIP_CODE) VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

            cmd.Parameters.Add("EMPLOYEE_NAME", OleDbType.VarChar).Value = employee.employeeName;
            cmd.Parameters.Add("EMPLOYEE_IMAGE", OleDbType.VarChar).Value = fileName;
            cmd.Parameters.Add("PIS_PASEP", OleDbType.VarChar).Value = pisPasep;
            cmd.Parameters.Add("ID_COMPANY", OleDbType.Integer).Value = employee.company != null ? employee.company.idCompany : 0;
            cmd.Parameters.Add("ID_SCHEDULE", OleDbType.Integer).Value = employee.schedule != null ? employee.schedule.idSchedule : 0;

            cmd.Parameters.Add("LEEF_NUMBER", OleDbType.VarChar).Value = employee.leefNumber;
            cmd.Parameters.Add("CTPS", OleDbType.VarChar).Value = employee.ctps;
            cmd.Parameters.Add("ID_DEPARTMENT", OleDbType.Integer).Value = employee.department != null ? employee.department.idDepartment : 0;
            cmd.Parameters.Add("ID_OFFICE", OleDbType.Integer).Value = employee.office != null ? employee.office.idOffice : 0;
            cmd.Parameters.Add("ADMISSION", OleDbType.Date).Value = employee.admission;
            cmd.Parameters.Add("RESIGNATION", OleDbType.Date).Value = employee.resignation;
            cmd.Parameters.Add("ID_RESIGNATION_REASON", OleDbType.Integer).Value = employee.resignationReason != null ? employee.resignationReason.idResignationReason : 0;

            cmd.Parameters.Add("PHONE", OleDbType.VarChar).Value = employee.phone;
            cmd.Parameters.Add("EMAIL", OleDbType.VarChar).Value = employee.email;
            cmd.Parameters.Add("GENERAL_REGISTRY", OleDbType.VarChar).Value = employee.generalRegistry;
            cmd.Parameters.Add("REGISTRY_ENTITY", OleDbType.VarChar).Value = employee.registryEntity;
            cmd.Parameters.Add("FATHER", OleDbType.VarChar).Value = employee.father;
            cmd.Parameters.Add("MOTHER", OleDbType.VarChar).Value = employee.mother;
            cmd.Parameters.Add("BIRTH", OleDbType.VarChar).Value = employee.birth;
            cmd.Parameters.Add("GENDER", OleDbType.VarChar).Value = employee.gender;
            cmd.Parameters.Add("CIVIL_STATUS", OleDbType.VarChar).Value = employee.civilStatus;
            cmd.Parameters.Add("NATIONALITY", OleDbType.VarChar).Value = employee.nationality;
            cmd.Parameters.Add("NATURALNESS", OleDbType.VarChar).Value = employee.naturalness;
            cmd.Parameters.Add("ADDRESS", OleDbType.VarChar).Value = employee.address;
            cmd.Parameters.Add("NEIGHBORHOOD", OleDbType.VarChar).Value = employee.neighborhood;
            cmd.Parameters.Add("CITY", OleDbType.VarChar).Value = employee.city;
            cmd.Parameters.Add("STATE", OleDbType.VarChar).Value = employee.state;
            cmd.Parameters.Add("ZIP_CODE", OleDbType.VarChar).Value = employee.zipCode;

            try
            {
                cmd.ExecuteNonQuery();
                success = true;

                String origin = employee.employeeImage.UriSource.AbsolutePath.Replace("%20", " ");
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

        public Boolean saveImportEmployee(String name, String pisPasep, int idCompany, int idSchedule, String leefNumber, DateTime admission)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            pisPasep = pisPasep.Replace(".", "").Replace("-", "").Replace(" ", "");

            cmd.CommandText = "INSERT INTO EMPLOYEE (EMPLOYEE_NAME, PIS_PASEP, ID_COMPANY, ID_SCHEDULE, " +
                "LEEF_NUMBER, ADMISSION) VALUES (?,?,?,?,?,?)";

            cmd.Parameters.Add("EMPLOYEE_NAME", OleDbType.VarChar).Value = name;
            cmd.Parameters.Add("PIS_PASEP", OleDbType.VarChar).Value = pisPasep;
            cmd.Parameters.Add("ID_COMPANY", OleDbType.Integer).Value = idCompany;
            cmd.Parameters.Add("ID_SCHEDULE", OleDbType.Integer).Value = idSchedule;
            cmd.Parameters.Add("LEEF_NUMBER", OleDbType.VarChar).Value = leefNumber;
            cmd.Parameters.Add("ADMISSION", OleDbType.Date).Value = admission;
            
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

        public Boolean updateEmployee(Employee employee)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            String pisPasep = employee.pisPasep.Replace(".", "").Replace("-", "").Replace(" ", "");
            String fileName = "employee_" + pisPasep + ".jpg";

            cmd.CommandText = "UPDATE EMPLOYEE SET EMPLOYEE_NAME=?, EMPLOYEE_IMAGE=?, PIS_PASEP=?, ID_COMPANY=?, ID_SCHEDULE=?, " +
                "LEEF_NUMBER=?, CTPS=?, ID_DEPARTMENT=?, ID_OFFICE=?, ADMISSION=?, RESIGNATION=?, ID_RESIGNATION_REASON=?, PHONE=?, EMAIL=?, GENERAL_REGISTRY=?, " +
                "REGISTRY_ENTITY=?, FATHER=?, MOTHER=?, BIRTH=?, GENDER=?, CIVIL_STATUS=?, NATIONALITY=?, NATURALNESS=?, ADDRESS=?, NEIGHBORHOOD=?, " +
                "CITY=?, STATE=?, ZIP_CODE=? WHERE ID_EMPLOYEE=?";

            cmd.Parameters.Add("EMPLOYEE_NAME", OleDbType.VarChar).Value = employee.employeeName;
            cmd.Parameters.Add("EMPLOYEE_IMAGE", OleDbType.VarChar).Value = fileName;
            cmd.Parameters.Add("PIS_PASEP", OleDbType.VarChar).Value = pisPasep;
            cmd.Parameters.Add("ID_COMPANY", OleDbType.Integer).Value = employee.company != null ? employee.company.idCompany : 0;
            cmd.Parameters.Add("ID_SCHEDULE", OleDbType.Integer).Value = employee.schedule != null ? employee.schedule.idSchedule : 0;
            cmd.Parameters.Add("LEEF_NUMBER", OleDbType.VarChar).Value = employee.leefNumber;
            cmd.Parameters.Add("CTPS", OleDbType.VarChar).Value = employee.ctps;
            cmd.Parameters.Add("ID_DEPARTMENT", OleDbType.Integer).Value = employee.department != null ? employee.department.idDepartment : 0;
            cmd.Parameters.Add("ID_OFFICE", OleDbType.Integer).Value = employee.office != null ? employee.office.idOffice : 0;
            cmd.Parameters.Add("ADMISSION", OleDbType.Date).Value = employee.admission;
            cmd.Parameters.Add("RESIGNATION", OleDbType.Date).Value = employee.resignation;
            cmd.Parameters.Add("ID_RESIGNATION_REASON", OleDbType.Integer).Value = employee.resignationReason != null ? employee.resignationReason.idResignationReason : 0;
            cmd.Parameters.Add("PHONE", OleDbType.VarChar).Value = employee.phone;
            cmd.Parameters.Add("EMAIL", OleDbType.VarChar).Value = employee.email;
            cmd.Parameters.Add("GENERAL_REGISTRY", OleDbType.VarChar).Value = employee.generalRegistry;
            cmd.Parameters.Add("REGISTRY_ENTITY", OleDbType.VarChar).Value = employee.registryEntity;
            cmd.Parameters.Add("FATHER", OleDbType.VarChar).Value = employee.father;
            cmd.Parameters.Add("MOTHER", OleDbType.VarChar).Value = employee.mother;
            cmd.Parameters.Add("BIRTH", OleDbType.VarChar).Value = employee.birth;
            cmd.Parameters.Add("GENDER", OleDbType.VarChar).Value = employee.gender;
            cmd.Parameters.Add("CIVIL_STATUS", OleDbType.VarChar).Value = employee.civilStatus;
            cmd.Parameters.Add("NATIONALITY", OleDbType.VarChar).Value = employee.nationality;
            cmd.Parameters.Add("NATURALNESS", OleDbType.VarChar).Value = employee.naturalness;
            cmd.Parameters.Add("ADDRESS", OleDbType.VarChar).Value = employee.address;
            cmd.Parameters.Add("NEIGHBORHOOD", OleDbType.VarChar).Value = employee.neighborhood;
            cmd.Parameters.Add("CITY", OleDbType.VarChar).Value = employee.city;
            cmd.Parameters.Add("STATE", OleDbType.VarChar).Value = employee.state;
            cmd.Parameters.Add("ZIP_CODE", OleDbType.VarChar).Value = employee.zipCode;
            cmd.Parameters.Add("ID_EMPLOYEE", OleDbType.Integer).Value = employee.idEmployee;

            try
            {
                cmd.ExecuteNonQuery();
                success = true;

                String origin = employee.employeeImage.UriSource.AbsolutePath.Replace("%20", " ");
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

        public Boolean deleteEmployee(Employee employee)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            cmd.CommandText = "DELETE FROM EMPLOYEE WHERE ID_EMPLOYEE=?";
            cmd.Parameters.Add("ID_EMPLOYEE", OleDbType.Integer).Value = employee.idEmployee;

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

        public Employee getEmployee(int idEmployee)
        {
            Employee employee = new Employee();

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT * FROM EMPLOYEE WHERE ID_EMPLOYEE=?";
            cmd.Parameters.Add("ID_EMPLOYEE", OleDbType.Integer).Value = idEmployee;
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                if (result.Read())
                {
                    employee.idEmployee = result.GetInt32(0);
                    employee.employeeName = result.GetString(1);
                    String fileName = Convert.ToString(result[2]);
                    employee.pisPasep = Formatter.getInstance.formatPisPasep(result.GetString(3), "");
                    employee.company = companyControl.getCompany(result.GetInt32(4));
                    employee.schedule = scheduleControl.getSchedule(result.GetInt32(5));

                    employee.leefNumber = Convert.ToString(result[6]);
                    employee.ctps = result.GetString(7);
                    employee.department = departmentControl.getDepartment(result.GetInt32(8));
                    employee.office = officeControl.getOffice(result.GetInt32(9));
                    employee.admission = result.GetDateTime(10);
                    employee.resignation = result.GetDateTime(11);
                    employee.resignationReason = resignationReasonControl.getResignationReason(result.GetInt32(12));

                    employee.phone = result.GetString(13);
                    employee.email = result.GetString(14);
                    employee.generalRegistry = result.GetString(15);
                    employee.registryEntity = result.GetString(16);
                    employee.father = result.GetString(17);
                    employee.mother = result.GetString(18);
                    employee.birth = result.GetDateTime(19);
                    employee.gender = result.GetString(20);
                    employee.civilStatus = result.GetString(21);
                    employee.nationality = result.GetString(22);
                    employee.naturalness = result.GetString(23);
                    employee.address = result.GetString(24);
                    employee.neighborhood = result.GetString(25);
                    employee.city = result.GetString(26);
                    employee.state = result.GetString(27);
                    employee.zipCode = result.GetString(28);

                    String imageFile = ConfigControl.Instance.getImageDirectory() + "/" + fileName;

                    if (File.Exists(imageFile))
                    {
                        BitmapImage img = new BitmapImage();
                        img.BeginInit();
                        img.CacheOption = BitmapCacheOption.OnLoad;
                        img.UriSource = new Uri(imageFile);
                        img.EndInit();

                        employee.employeeImage = img;
                    }
                    else
                    {
                        BitmapImage img = new BitmapImage();
                        img.BeginInit();
                        img.CacheOption = BitmapCacheOption.OnLoad;
                        img.UriSource = new Uri(ConfigControl.Instance.getNoImageFile());
                        img.EndInit();

                        employee.employeeImage = img;
                    }
                }
            }

            result.Close();

            return employee;
        }

        public Employee getEmployeeByPisPasep(String pisPasep)
        {
            Employee employee = new Employee();
            String formattedPsiPAsep = Formatter.getInstance.formatPisPasep(pisPasep, null);

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT * FROM EMPLOYEE WHERE PIS_PASEP=?";
            cmd.Parameters.Add("PIS_PASEP", OleDbType.VarChar).Value = formattedPsiPAsep;
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                if (result.Read())
                {
                    employee.idEmployee = result.GetInt32(0);
                    employee.employeeName = result.GetString(1);
                    String fileName = Convert.ToString(result[2]);
                    employee.pisPasep = Formatter.getInstance.formatPisPasep(result.GetString(3), "");
                    employee.company = companyControl.getCompany(result.GetInt32(4));
                    employee.schedule = scheduleControl.getSchedule(result.GetInt32(5));

                    employee.leefNumber = Convert.ToString(result[6]);
                    employee.ctps = result.GetString(7);
                    employee.department = departmentControl.getDepartment(result.GetInt32(8));
                    employee.office = officeControl.getOffice(result.GetInt32(9));
                    employee.admission = result.GetDateTime(10);
                    employee.resignation = result.GetDateTime(11);
                    employee.resignationReason = resignationReasonControl.getResignationReason(result.GetInt32(12));

                    employee.phone = result.GetString(13);
                    employee.email = result.GetString(14);
                    employee.generalRegistry = result.GetString(15);
                    employee.registryEntity = result.GetString(16);
                    employee.father = result.GetString(17);
                    employee.mother = result.GetString(18);
                    employee.birth = result.GetDateTime(19);
                    employee.gender = result.GetString(20);
                    employee.civilStatus = result.GetString(21);
                    employee.nationality = result.GetString(22);
                    employee.naturalness = result.GetString(23);
                    employee.address = result.GetString(24);
                    employee.neighborhood = result.GetString(25);
                    employee.city = result.GetString(26);
                    employee.state = result.GetString(27);
                    employee.zipCode = result.GetString(28);

                    String imageFile = ConfigControl.Instance.getImageDirectory() + "/" + fileName;

                    if (File.Exists(imageFile))
                    {
                        employee.employeeImage = new BitmapImage(new Uri(imageFile));
                    }
                    else
                    {
                        employee.employeeImage = new BitmapImage(new Uri(ConfigControl.Instance.getNoImageFile()));
                    }
                }
            }

            result.Close();

            return employee;
        }

        public List<Employee> getAllEmployees()
        {
            List<Employee> employees = new List<Employee>();

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT * FROM EMPLOYEE";
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    Employee employee = new Employee();
                    employee.idEmployee = result.GetInt32(0);
                    employee.employeeName = result.GetString(1);
                    String fileName = Convert.ToString(result[2]);
                    employee.pisPasep = Formatter.getInstance.formatPisPasep(result.GetString(3), "");
                    employee.company = companyControl.getCompany(result.GetInt32(4));
                    employee.schedule = scheduleControl.getSchedule(result.GetInt32(5));

                    employee.leefNumber = Convert.ToString(result[6]);
                    employee.ctps = result.GetString(7);
                    employee.department = departmentControl.getDepartment(result.GetInt32(8));
                    employee.office = officeControl.getOffice(result.GetInt32(9));
                    employee.admission = result.GetDateTime(10);
                    employee.resignation = result.GetDateTime(11);
                    employee.resignationReason = resignationReasonControl.getResignationReason(result.GetInt32(12));

                    employee.phone = result.GetString(13);
                    employee.email = result.GetString(14);
                    employee.generalRegistry = result.GetString(15);
                    employee.registryEntity = result.GetString(16);
                    employee.father = result.GetString(17);
                    employee.mother = result.GetString(18);
                    employee.birth = result.GetDateTime(19);
                    employee.gender = result.GetString(20);
                    employee.civilStatus = result.GetString(21);
                    employee.nationality = result.GetString(22);
                    employee.naturalness = result.GetString(23);
                    employee.address = result.GetString(24);
                    employee.neighborhood = result.GetString(25);
                    employee.city = result.GetString(26);
                    employee.state = result.GetString(27);
                    employee.zipCode = result.GetString(28);

                    String imageFile = ConfigControl.Instance.getImageDirectory() + "/" + fileName;

                    if (File.Exists(imageFile))
                    {
                        BitmapImage img = new BitmapImage();
                        img.BeginInit();
                        img.CacheOption = BitmapCacheOption.OnLoad;
                        img.UriSource = new Uri(imageFile);
                        img.EndInit();

                        employee.employeeImage = img;
                    }
                    else
                    {
                        BitmapImage img = new BitmapImage();
                        img.BeginInit();
                        img.CacheOption = BitmapCacheOption.OnLoad;
                        img.UriSource = new Uri(ConfigControl.Instance.getNoImageFile());
                        img.EndInit();

                        employee.employeeImage = img;
                    }

                    employees.Add(employee);
                }
            }

            result.Close();

            return employees;
        }

        public List<Employee> getAllEmployeesFromDepartment(int idDepartment, int idOffice)
        {
            List<Employee> employees = new List<Employee>();

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            String commandText = "SELECT * FROM EMPLOYEE";

            if (idDepartment != 0)
            {
                commandText += " WHERE ID_DEPARTMENT=?";
                cmd.Parameters.Add("ID_DEPARTMENT", OleDbType.Integer).Value = idDepartment;
            }

            if (idOffice != 0)
            {
                if (idDepartment != 0)
                {
                    commandText += " AND";
                }
                else
                {
                    commandText += " WHERE";
                }

                commandText += " ID_OFFICE=?";
                cmd.Parameters.Add("ID_OFFICE", OleDbType.Integer).Value = idOffice;
            }

            cmd.CommandText = commandText;
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    Employee employee = new Employee();
                    employee.idEmployee = result.GetInt32(0);
                    employee.employeeName = result.GetString(1);
                    String fileName = Convert.ToString(result[2]);
                    employee.pisPasep = Formatter.getInstance.formatPisPasep(result.GetString(3), "");
                    employee.company = companyControl.getCompany(result.GetInt32(4));
                    employee.schedule = scheduleControl.getSchedule(result.GetInt32(5));

                    employee.leefNumber = Convert.ToString(result[6]);
                    employee.ctps = Convert.ToString(result[7]);
                    employee.department = departmentControl.getDepartment(result.GetInt32(8));
                    employee.office = officeControl.getOffice(result.GetInt32(9));
                    employee.admission = result.GetDateTime(10);
                    employee.resignation = result.GetDateTime(11);
                    employee.resignationReason = resignationReasonControl.getResignationReason(result.GetInt32(12));

                    employee.phone = result.GetString(13);
                    employee.email = result.GetString(14);
                    employee.generalRegistry = result.GetString(15);
                    employee.registryEntity = result.GetString(16);
                    employee.father = result.GetString(17);
                    employee.mother = result.GetString(18);
                    employee.birth = result.GetDateTime(19);
                    employee.gender = result.GetString(20);
                    employee.civilStatus = result.GetString(21);
                    employee.nationality = result.GetString(22);
                    employee.naturalness = result.GetString(23);
                    employee.address = result.GetString(24);
                    employee.neighborhood = result.GetString(25);
                    employee.city = result.GetString(26);
                    employee.state = result.GetString(27);
                    employee.zipCode = result.GetString(28);

                    String imageFile = ConfigControl.Instance.getImageDirectory() + "/" + fileName;

                    if (File.Exists(imageFile))
                    {
                        employee.employeeImage = new BitmapImage(new Uri(imageFile));
                    }
                    else
                    {
                        employee.employeeImage = new BitmapImage(new Uri(ConfigControl.Instance.getNoImageFile()));
                    }

                    employees.Add(employee);
                }
            }

            result.Close();

            return employees;
        }

        public bool validadeLeefNumber(string leefNumber)
        {
            bool valid = true;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT * FROM EMPLOYEE WHERE LEEF_NUMBER=?";
            cmd.Parameters.Add("LEEF_NUMBER", OleDbType.VarChar).Value = leefNumber;
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                valid = false;
            }

            return valid;
        }

        public bool validadePisPasep(string pisPasep)
        {
            bool valid = true;
            pisPasep = pisPasep.Replace(".", "").Replace("-", "").Replace(" ", "");

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT * FROM EMPLOYEE WHERE PIS_PASEP=?";
            cmd.Parameters.Add("PIS_PASEP", OleDbType.VarChar).Value = pisPasep;
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                valid = false;
            }

            return valid;
        }
    }
}
