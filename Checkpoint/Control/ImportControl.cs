using Checkpoint.Model;
using Checkpoint.Tools;
using System;
using System.IO;
using System.Linq;

namespace Checkpoint.Control
{
    class ImportControl
    {
        MarkingControl markingControl = new MarkingControl();
        AdjustmentControl adjustmentControl = new AdjustmentControl();
        EmployeeControl employeeControl = new EmployeeControl();

        public Boolean importAFD(String outcomingFile, DateTime startDate, DateTime endDate, String pisPasep)
        {
            Boolean success = false;

            if (File.Exists(outcomingFile))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(outcomingFile))
                    {
                        String line;
                        Int64 newLastNsr = 0;

                        while ((line = sr.ReadLine()) != null)
                        {
                            string nsrStr = line.Substring(0, 9);

                            if (nsrStr.All(char.IsNumber))
                            {
                                Int64 nsr = Convert.ToInt64(nsrStr);
                                int type = Convert.ToInt16(line.Substring(9, 1));

                                if (type == 3){

                                    Int64 lastNsr = adjustmentControl.getGetLastNsr();
                                    String actualPisPasep = line.Substring(22, 11);

                                    if ((nsr > lastNsr) && (pisPasep.Equals(actualPisPasep) || "".Equals(pisPasep)))
                                    {
                                        DateTime actualDate = new DateTime(Convert.ToInt16(line.Substring(14, 4)), Convert.ToInt16(line.Substring(12, 2)), Convert.ToInt16(line.Substring(10, 2)), Convert.ToInt16(line.Substring(18, 2)), Convert.ToInt16(line.Substring(20, 2)), 0);

                                        if ((actualDate > startDate || startDate == DateTime.MinValue) && (actualDate < endDate || endDate == DateTime.MinValue))
                                        {
                                            if (Inspector.getInstance.validatePis(actualPisPasep))
                                            {
                                                Marking marking = new Marking();

                                                marking.nsr = Convert.ToInt64(line.Substring(0, 9));
                                                marking.cont = Convert.ToInt64(line.Substring(0, 9));
                                                marking.pisPasep = actualPisPasep;
                                                marking.day = Convert.ToInt16(line.Substring(10, 2));
                                                marking.month = Convert.ToInt16(line.Substring(12, 2));
                                                marking.year = Convert.ToInt16(line.Substring(14, 4));
                                                marking.hour = Convert.ToInt16(line.Substring(18, 2));
                                                marking.minute = Convert.ToInt16(line.Substring(20, 2));

                                                markingControl.saveMarking(marking);

                                                newLastNsr = nsr;
                                            }
                                            else
                                            {
                                                RejectedMarking marking = new RejectedMarking();

                                                marking.nsr = Convert.ToInt64(line.Substring(0, 9));
                                                marking.cont = Convert.ToInt64(line.Substring(0, 9));
                                                marking.pisPasep = actualPisPasep;
                                                marking.day = Convert.ToInt16(line.Substring(10, 2));
                                                marking.month = Convert.ToInt16(line.Substring(12, 2));
                                                marking.year = Convert.ToInt16(line.Substring(14, 4));
                                                marking.hour = Convert.ToInt16(line.Substring(18, 2));
                                                marking.minute = Convert.ToInt16(line.Substring(20, 2));
                                                marking.description = "PIS/PASEP Inválido.";

                                                markingControl.saveRejectedMarking(marking);

                                                newLastNsr = nsr;
                                            }
                                        }
                                    }

                                }
                            }
                        }

                        if (newLastNsr > adjustmentControl.getGetLastNsr())
                        {
                            adjustmentControl.updateLastMarkingNsr(newLastNsr);
                        }
                    }

                    success = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return success;
        }

        public Boolean importEmployee(String outcomingFile, int idCompany, int idSchedule)
        {
            Boolean success = false;

            if (File.Exists(outcomingFile))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(outcomingFile))
                    {
                        String line;

                        while ((line = sr.ReadLine()) != null)
                        {
                            String[] fields = line.Split(';');

                            if (employeeControl.validadePisPasep(fields[1]))
                            {
                                employeeControl.saveImportEmployee(fields[0], fields[1], idCompany, idSchedule, fields[2], DateTime.Parse(fields[3]));
                            }
                        }
                    }

                    success = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return success;
        }
    }
}
