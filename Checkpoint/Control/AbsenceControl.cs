using Checkpoint.DAO;
using Checkpoint.Model;
using System;
using System.Collections.Generic;

namespace Checkpoint.Control
{
    class AbsenceControl
    {
        AbsenceDAO absenceDAO = new AbsenceDAO();

        public Boolean saveAbsence(Absence absence)
        {
            return absenceDAO.saveAbsence(absence);
        }

        public Boolean updateAbsence(Absence absence)
        {
            return absenceDAO.updateAbsence(absence);
        }

        public Boolean deleteAbsence(Absence absence)
        {
            return absenceDAO.deleteAbsence(absence);
        }

        public List<Absence> getAllAbsence()
        {
            return absenceDAO.getAllAbsence();
        }

        public Boolean getEmployeeInAbsence(Employee employee, DateTime date)
        {
            return absenceDAO.getEmployeeInAbsence(employee, date);
        }

    }
}