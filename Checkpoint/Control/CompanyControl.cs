using Checkpoint.DAO;
using Checkpoint.Model;
using System;
using System.Collections.Generic;

namespace Checkpoint.Control
{
    class CompanyControl
    {
        CompanyDAO companyDAO = new CompanyDAO();

        public Boolean saveCompany(Company company)
        {
            return companyDAO.saveCompany(company);
        }

        public Boolean updateCompany(Company company)
        {
            return companyDAO.updateCompany(company);
        }

        public Boolean deleteCompany(Company company)
        {
            return companyDAO.deleteCompany(company);
        }

        public Company getCompany(int idCompany)
        {
            return companyDAO.getCompany(idCompany);
        }

        public Boolean validateCnpj(String cnpj)
        {
            return companyDAO.validateCnpj(cnpj);
        }

        public List<Company> getAllCompanies()
        {
            return companyDAO.getAllCompanies();
        }

        public List<Company> getAllFatherCompanies()
        {
            return companyDAO.getAllFatherCompanies();
        }

    }
}
