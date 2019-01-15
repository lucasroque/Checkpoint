using Checkpoint.DAO;
using Checkpoint.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Checkpoint.Control
{
    class ExportTaxFileControl
    {
        ExportTaxFileDAO exportTaxFileDAO = new ExportTaxFileDAO();

        public Boolean exportFDTFile(String folder, DateTime startDate, DateTime endDate, Company company, Department department)
        {
            Boolean success = true;
            String path = folder + "/AFDT_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".txt";

            if (company != null)
            {
                String header = exportTaxFileDAO.getCompanyHeader(company.idCompany, startDate, endDate);
                List<String> details = exportTaxFileDAO.getAFDTDetails(startDate, endDate, company, department);

                success = writeFile(path, header, new List<string>(), details);
            }
            else
            {
                List<String> headers = exportTaxFileDAO.getAllHeaders(startDate, endDate);

                foreach (String header in headers)
                {
                    Company actualCompany = null;

                    List<String> details = exportTaxFileDAO.getAFDTDetails(startDate, endDate, actualCompany, department);

                    success = writeFile(path, header, new List<string>(), details);
                }

            }

            return success;
        }

        public Boolean exportCJEFFile(String folder, DateTime startDate, DateTime endDate, Company company, Department department) 
        {

            Boolean success = true;
            String path = folder + "/CJEF_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".txt";

            if (company != null)
            {
                String header = exportTaxFileDAO.getCompanyHeader(company.idCompany, startDate, endDate);
                List<String> schedules = exportTaxFileDAO.getSchedules();
                List<String> details = exportTaxFileDAO.getCJEFDetails(startDate, endDate, company, department);

                success = writeFile(path, header, schedules, details);
            }
            else
            {
                List<String> headers = exportTaxFileDAO.getAllHeaders(startDate, endDate);

                foreach (String header in headers)
                {
                    Company actualCompany = null;

                    List<String> schedules = exportTaxFileDAO.getSchedules();
                    List<String> details = exportTaxFileDAO.getCJEFDetails(startDate, endDate, actualCompany, department);

                    success = writeFile(path, header, schedules, details);
                }

            }

            return success;
        }

        private Boolean writeFile(String path, String header, List<String> schedules, List<String> details)
        {
            Boolean success = true;
            int seq = 1;

            StreamWriter outWriter = new StreamWriter(path, true, Encoding.Default);

            try
            {
                
                outWriter.WriteLine(Convert.ToString(seq).PadLeft(9, '0') + header);
                seq++;

                foreach (String schedule in schedules)
                {
                    outWriter.WriteLine(Convert.ToString(seq).PadLeft(9, '0') + schedule);
                    seq++;
                }

                foreach (String detail in details)
                {
                    outWriter.WriteLine(Convert.ToString(seq).PadLeft(9, '0') +  detail);
                    seq++;
                }

            }
            catch (Exception e)
            {
                success = false;
            }

            outWriter.Close();

            return success;
        }

    }
}
