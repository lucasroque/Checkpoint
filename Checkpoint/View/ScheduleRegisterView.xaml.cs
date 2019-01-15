using MaterialDesignThemes.Wpf;
using Checkpoint.Control;
using Checkpoint.Message;
using Checkpoint.Model;
using Checkpoint.Tools;
using Checkpoint.ViewControl;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;
using System.Windows.Media;

namespace Checkpoint.View
{
    public partial class ScheduleRegisterView : System.Windows.Controls.UserControl
    {
        private ScheduleControl scheduleControl;
        private ScheduleViewControl scheduleViewControl;

        List<Schedule> allSchedules = new List<Schedule>();

        private int idScheduleEditing = 0;

        public ScheduleRegisterView()
        {
            InitializeComponent();
            scheduleControl = new ScheduleControl();
            scheduleViewControl = new ScheduleViewControl();

            DataContext = scheduleViewControl;

            fillCBSchedule();
        }

        private void fillCBSchedule()
        {
            allSchedules.Clear();
            allSchedules = scheduleControl.getAllSchedules();
            CBSchedule.ItemsSource = allSchedules;
        }

        private void upsertSchedule(object sender, RoutedEventArgs e)
        {
            String validation = validateSchedule();

            if ("".Equals(validation))
            {
                if (!"".Equals(TBDescription.Text))
                {
                    upsertSchedule();
                }
                else
                {
                    DialogHost.Show(new SampleMessageDialog("Preencher campos obrigatórios."), "DHMain");
                }
            }
            else
            {
                DialogHost.Show(new SampleMessageDialog(validation), "DHMain");
            }
        }

        private void loadSchedule(object sender, RoutedEventArgs e)
        {
            Schedule schedule = (Schedule)CBSchedule.SelectedItem;
            loadSchedule(schedule);
        }

        private async void deleteSchedule(object sender, RoutedEventArgs e)
        {
            OptionMessageDialog optionMessageDialog = new OptionMessageDialog("Deseja realmente excluir este Horário?");
            Boolean result = (Boolean)await DialogHost.Show(optionMessageDialog, "DHMain");

            if (result == true)
            {
                Schedule schedule = (Schedule)CBSchedule.SelectedItem;
                deleteSchedule(schedule);
            }
        }

        private void repeatFirstScheduleDay(object sender, RoutedEventArgs e)
        {
            ScheduleDay firstSd = (ScheduleDay)GDScheduleDays.Items[0];

            foreach (ScheduleDay sd in GDScheduleDays.ItemsSource)
            {
                sd.EntryOne = firstSd.EntryOne;
                sd.exitOne = firstSd.exitOne;
                sd.entryTwo = firstSd.entryTwo;
                sd.exitTwo = firstSd.exitTwo;
                sd.entryThree = firstSd.entryThree;
                sd.exitThree = firstSd.exitThree;
                sd.entryTolerance = firstSd.entryTolerance;
                sd.exitTolerance = firstSd.exitTolerance;
                sd.workload = firstSd.workload;
                sd.endOfficeHour = firstSd.endOfficeHour;
                sd.neutral = firstSd.neutral;
                sd.compensation = firstSd.compensation;
                sd.automaticDayOff = firstSd.automaticDayOff;
            }

            GDScheduleDays.Items.Refresh();
            updateWeekWorload();
        }

        private void repeatScheduleUp(object sender, RoutedEventArgs e)
        {
            ScheduleDay baseSd = ((FrameworkElement)sender).DataContext as ScheduleDay;
            int index = GDScheduleDays.Items.IndexOf(baseSd);

            for (int i = index - 1; i >= 0; i--)
            {
                ScheduleDay sd = (ScheduleDay)GDScheduleDays.Items[i];

                sd.EntryOne = baseSd.EntryOne;
                sd.exitOne = baseSd.exitOne;
                sd.entryTwo = baseSd.entryTwo;
                sd.exitTwo = baseSd.exitTwo;
                sd.entryThree = baseSd.entryThree;
                sd.exitThree = baseSd.exitThree;
                sd.entryTolerance = baseSd.entryTolerance;
                sd.exitTolerance = baseSd.exitTolerance;
                sd.workload = baseSd.workload;
                sd.endOfficeHour = baseSd.endOfficeHour;
                sd.neutral = baseSd.neutral;
                sd.compensation = baseSd.compensation;
                sd.automaticDayOff = baseSd.automaticDayOff;
            }

            GDScheduleDays.Items.Refresh();
            updateWeekWorload();
        }

        private void repeatScheduleDown(object sender, RoutedEventArgs e)
        {
            ScheduleDay baseSd = ((FrameworkElement)sender).DataContext as ScheduleDay;
            int index = GDScheduleDays.Items.IndexOf(baseSd);

            for (int i = index + 1; i < GDScheduleDays.Items.Count; i++)
            {
                ScheduleDay sd = (ScheduleDay)GDScheduleDays.Items[i];

                sd.EntryOne = baseSd.EntryOne;
                sd.exitOne = baseSd.exitOne;
                sd.entryTwo = baseSd.entryTwo;
                sd.exitTwo = baseSd.exitTwo;
                sd.entryThree = baseSd.entryThree;
                sd.exitThree = baseSd.exitThree;
                sd.entryTolerance = baseSd.entryTolerance;
                sd.exitTolerance = baseSd.exitTolerance;
                sd.workload = baseSd.workload;
                sd.endOfficeHour = baseSd.endOfficeHour;
                sd.neutral = baseSd.neutral;
                sd.compensation = baseSd.compensation;
                sd.automaticDayOff = baseSd.automaticDayOff;
            }

            GDScheduleDays.Items.Refresh();
            updateWeekWorload();
        }

        private void setDayOff(object sender, RoutedEventArgs e)
        {
            ScheduleDay sd = ((FrameworkElement)sender).DataContext as ScheduleDay;

            if (sd.dayOff)
            {
                sd.EntryOne = "";
                sd.exitOne = "";
                sd.entryTwo = "";
                sd.exitTwo = "";
                sd.entryThree = "";
                sd.exitThree = "";
                sd.entryTolerance = "";
                sd.exitTolerance = "";
                sd.workload = "";
                sd.endOfficeHour = "";
                sd.dayOff = false;
            }
            else
            {
                sd.EntryOne = "FOLGA";
                sd.exitOne = "FOLGA";
                sd.entryTwo = "FOLGA";
                sd.exitTwo = "FOLGA";
                sd.entryThree = "FOLGA";
                sd.exitThree = "FOLGA";
                sd.entryTolerance = "FOLGA";
                sd.exitTolerance = "FOLGA";
                sd.workload = "FOLGA";
                sd.endOfficeHour = "FOLGA";
                sd.dayOff = true;
                sd.compensation = false;
                sd.automaticDayOff = false;
                sd.neutral = false;
            }

            GDScheduleDays.Items.Refresh();
        }

        private void cleanControls(object sender, RoutedEventArgs e)
        {
            cleanControls();
        }

        private void upsertSchedule()
        {
            Schedule schedule = getScheduleFromControls();
            Boolean success;

            if (idScheduleEditing != 0)
            {
                schedule.idSchedule = idScheduleEditing;
                success = scheduleControl.updateSchedule(schedule);
                saveModeControls();
            }
            else
            {
                success = scheduleControl.saveSchedule(schedule);
            }

            if (success)
            {
                DialogHost.Show(new SampleMessageDialog("Horário salvo com sucesso."), "DHMain");
                cleanControls();
            }
            else
            {
                DialogHost.Show(new SampleMessageDialog("Erro ao salvar Horário."), "DHMain");
            }

            cleanControls();
        }

        private void loadSchedule(Schedule schedule)
        {
            if (schedule != null)
            {
                TBDescription.Text = schedule.description;
                TBNightly.Text = schedule.nightlyStart;
                TBOvertimePercentage.Text = schedule.overtimePercentage;
                scheduleViewControl.setScheduleList(schedule.scheduleDays);

                idScheduleEditing = schedule.idSchedule;

                editModeControls();
            }
            else
            {
                DialogHost.Show(new SampleMessageDialog("Selecione um Horário."), "DHMain");
            }

        }

        private void deleteSchedule(Schedule schedule)
        {
            Boolean success = scheduleControl.deleteSchedule(schedule);

            if (success)
            {
                DialogHost.Show(new SampleMessageDialog("Horário excluído com sucesso."), "DHMain");
            }
            else
            {
                DialogHost.Show(new SampleMessageDialog("Erro ao excluir Horário."), "DHMain");
            }

            cleanControls();
        }

        private Schedule getScheduleFromControls()
        {
            Schedule schedule = new Schedule();
            schedule.description = TBDescription.Text;
            schedule.nightlyStart = TBNightly.Text;
            schedule.overtimePercentage = TBOvertimePercentage.Text;

            List<ScheduleDay> scheduleDays = new List<ScheduleDay>();
            foreach (ScheduleDay sd in GDScheduleDays.ItemsSource)
            {
                scheduleDays.Add(sd);
            }

            schedule.scheduleDays = scheduleDays;

            return schedule;
        }

        private String validateSchedule()
        {
            Schedule schedule = getScheduleFromControls();

            foreach (ScheduleDay sd in schedule.scheduleDays)
            {

                TimeSpan outDT = new TimeSpan();

                String entryOne = sd.EntryOne == null ? "" : sd.EntryOne.Replace(":", "");
                if (!MarkingUtil.isEmptyMarking(entryOne) && (!entryOne.All(char.IsNumber) || !(entryOne.Length == 4) || !TimeSpan.TryParse(sd.EntryOne, out outDT)))
                {
                    return "Entrada 1 Inválida na " + sd.descriptionDay;
                }

                String exitOne = sd.exitOne == null ? "" : sd.exitOne.Replace(":", "");
                if (!MarkingUtil.isEmptyMarking(exitOne) && (!exitOne.All(char.IsNumber) || !(exitOne.Length == 4) || !TimeSpan.TryParse(sd.exitOne, out outDT)))
                {
                    return "Saída 1 Inválida na " + sd.descriptionDay;
                }

                String entryTwo = sd.entryTwo == null ? "" : sd.entryTwo.Replace(":", "");
                if (!MarkingUtil.isEmptyMarking(entryTwo) && (!entryTwo.All(char.IsNumber) || !(entryTwo.Length == 4) || !TimeSpan.TryParse(sd.entryTwo, out outDT)))
                {
                    return "Entrada 2 Inválida na " + sd.descriptionDay;
                }

                String exitTwo = sd.exitTwo == null ? "" : sd.exitTwo.Replace(":", "");
                if (!MarkingUtil.isEmptyMarking(exitTwo) && (!exitTwo.All(char.IsNumber) || !(exitTwo.Length == 4) || !TimeSpan.TryParse(sd.exitTwo, out outDT)))
                {
                    return "Saída 2 Inválida na " + sd.descriptionDay;
                }

                String entryThree = sd.entryThree == null ? "" : sd.entryThree.Replace(":", "");
                if (!MarkingUtil.isEmptyMarking(entryThree) && (!entryThree.All(char.IsNumber) || !(entryThree.Length == 4) || !TimeSpan.TryParse(sd.entryThree, out outDT)))
                {
                    return "Entrada 3 Inválida na " + sd.descriptionDay;
                }
                String exitThree = sd.exitThree == null ? "" : sd.exitThree.Replace(":", "");
                if (!MarkingUtil.isEmptyMarking(exitThree) && (!exitThree.All(char.IsNumber) || !(exitThree.Length == 4) || !TimeSpan.TryParse(sd.exitThree, out outDT)))
                {
                    return "Saída 3 Inválida na " + sd.descriptionDay;
                }

                if (!sd.dayOff)
                {

                    if (!MarkingUtil.isEmptyMarking(sd.EntryOne) && !MarkingUtil.isEmptyMarking(sd.exitOne))
                    {
                        if (TimeSpan.Parse(sd.exitOne).CompareTo(TimeSpan.Parse(sd.EntryOne)) == -1)
                        {
                            return "Saída 1 menor que Entrada 1 na " + sd.descriptionDay;
                        }
                    }

                    if (!MarkingUtil.isEmptyMarking(sd.entryTwo) && !MarkingUtil.isEmptyMarking(sd.exitOne))
                    {
                        if (TimeSpan.Parse(sd.entryTwo).CompareTo(TimeSpan.Parse(sd.exitOne)) == -1)
                        {
                            return "Entrada 2 menor que Saída 1 na " + sd.descriptionDay;
                        }
                    }

                    if (!MarkingUtil.isEmptyMarking(sd.exitTwo) && !MarkingUtil.isEmptyMarking(sd.entryTwo))
                    {
                        if (TimeSpan.Parse(sd.exitTwo).CompareTo(TimeSpan.Parse(sd.entryTwo)) == -1)
                        {
                            return "Saída 2 menor que Entrada 2 na " + sd.descriptionDay;
                        }
                    }

                    if (!MarkingUtil.isEmptyMarking(sd.entryThree) && !MarkingUtil.isEmptyMarking(sd.exitTwo))
                    {
                        if (TimeSpan.Parse(sd.entryThree).CompareTo(TimeSpan.Parse(sd.exitTwo)) == -1)
                        {
                            return "Entrada 3 menor que Saída 2 na " + sd.descriptionDay;
                        }
                    }

                    if (!MarkingUtil.isEmptyMarking(sd.exitThree) && !MarkingUtil.isEmptyMarking(sd.entryThree))
                    {
                        if (TimeSpan.Parse(sd.exitThree).CompareTo(TimeSpan.Parse(sd.entryThree)) == -1)
                        {
                            return "Saída 3 menor que Entrada 3 na " + sd.descriptionDay;
                        }
                    }
                }
            }

            return "";
        }

        private void cleanControls()
        {
            TBDescription.Text = "";
            TBNightly.Text = "";
            TBOvertimePercentage.Text = "";
            scheduleViewControl.fillScheduleList();

            idScheduleEditing = 0;
            saveModeControls();

            fillCBSchedule();
        }

        private void saveModeControls()
        {
            BTSave.IsEnabled = true;
            BTEdit.IsEnabled = false;
            BTDelete.IsEnabled = false;
        }

        private void editModeControls()
        {
            BTSave.IsEnabled = false;
            BTEdit.IsEnabled = true;
            BTDelete.IsEnabled = true;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void cellEntryOne_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox TBEntryOne = (TextBox)sender;
            ScheduleDay scheduleDay = ((FrameworkElement)sender).DataContext as ScheduleDay;

            if (GDScheduleDays.SelectedIndex > -1)
            {
                var dataGridCellInfo = new DataGridCellInfo(GDScheduleDays.Items[GDScheduleDays.SelectedIndex], GDScheduleDays.Columns[9]);
                var cellContent = dataGridCellInfo.Column.GetCellContent(dataGridCellInfo.Item);
                TextBox TBWorload = (TextBox)VisualTreeHelper.GetChild(cellContent, 0);

                if (scheduleDay.EntryOne.Length == 4 && !scheduleDay.EntryOne.Contains(':'))
                {
                    TimeSpan result;
                    String formated = Formatter.getInstance.formatHour(scheduleDay.EntryOne);
                    formated = scheduleDay.EntryOne = TimeSpan.TryParse(formated, out result) ? formated : "";
                    scheduleDay.workload = getWorkload(scheduleDay);
                    TBEntryOne.Text = formated;
                    TBWorload.Text = scheduleDay.workload;
                    updateWeekWorload();
                    TBEntryOne.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }
                else if (!scheduleDay.EntryOne.Replace(":", "").All(char.IsNumber) && !scheduleDay.EntryOne.Equals("FOLGA"))
                {
                    scheduleDay.EntryOne = "";
                    TBEntryOne.Text = "";
                }
                else
                {
                    scheduleDay.workload = getWorkload(scheduleDay);
                    TBWorload.Text = scheduleDay.workload;
                    updateWeekWorload();
                }
            }
        }

        private void cellExitOne_TextChanged(object sender, TextChangedEventArgs e)
        {

            TextBox TBExitOne = (TextBox)sender;
            ScheduleDay scheduleDay = ((FrameworkElement)sender).DataContext as ScheduleDay;

            if (GDScheduleDays.SelectedIndex > -1)
            {
                var dataGridCellInfo = new DataGridCellInfo(GDScheduleDays.Items[GDScheduleDays.SelectedIndex], GDScheduleDays.Columns[9]);
                var cellContent = dataGridCellInfo.Column.GetCellContent(dataGridCellInfo.Item);
                TextBox TBWorload = (TextBox)VisualTreeHelper.GetChild(cellContent, 0);

                if (scheduleDay.exitOne.Length == 4 && !scheduleDay.exitOne.Contains(':'))
                {
                    TimeSpan result;
                    String formated = Formatter.getInstance.formatHour(scheduleDay.exitOne);
                    formated = scheduleDay.exitOne = TimeSpan.TryParse(formated, out result) ? formated : "";
                    scheduleDay.workload = getWorkload(scheduleDay);
                    TBExitOne.Text = formated;
                    TBWorload.Text = scheduleDay.workload;
                    updateWeekWorload();
                    TBExitOne.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }
                else if (!scheduleDay.exitOne.Replace(":", "").All(char.IsNumber) && !scheduleDay.exitOne.Equals("FOLGA"))
                {
                    scheduleDay.exitOne = "";
                    TBExitOne.Text = "";
                }
                else
                {
                    scheduleDay.workload = getWorkload(scheduleDay);
                    TBWorload.Text = scheduleDay.workload;
                    updateWeekWorload();
                }
            }
        }

        private void cellEntryTwo_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox TBEntryTwo = (TextBox)sender;
            ScheduleDay scheduleDay = ((FrameworkElement)sender).DataContext as ScheduleDay;

            if (GDScheduleDays.SelectedIndex > -1)
            {
                var dataGridCellInfo = new DataGridCellInfo(GDScheduleDays.Items[GDScheduleDays.SelectedIndex], GDScheduleDays.Columns[9]);
                var cellContent = dataGridCellInfo.Column.GetCellContent(dataGridCellInfo.Item);
                TextBox TBWorload = (TextBox)VisualTreeHelper.GetChild(cellContent, 0);

                if (scheduleDay.entryTwo.Replace(":", "").Length == 4 && !scheduleDay.entryTwo.Contains(':'))
                {
                    TimeSpan result;
                    String formated = Formatter.getInstance.formatHour(scheduleDay.entryTwo);
                    formated = scheduleDay.entryTwo = TimeSpan.TryParse(formated, out result) ? formated : "";
                    scheduleDay.workload = getWorkload(scheduleDay);
                    TBEntryTwo.Text = formated;
                    TBWorload.Text = scheduleDay.workload;
                    updateWeekWorload();
                    TBEntryTwo.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }
                else if (!scheduleDay.entryTwo.Replace(":", "").All(char.IsNumber) && !scheduleDay.entryTwo.Equals("FOLGA"))
                {
                    scheduleDay.entryTwo = "";
                    TBEntryTwo.Text = "";
                }
                else
                {
                    scheduleDay.workload = getWorkload(scheduleDay);
                    TBWorload.Text = scheduleDay.workload;
                    updateWeekWorload();
                }
            }
        }

        private void cellExitTwo_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox TBExitTwo = (TextBox)sender;
            ScheduleDay scheduleDay = ((FrameworkElement)sender).DataContext as ScheduleDay;

            if (GDScheduleDays.SelectedIndex > -1)
            {
                var dataGridCellInfo = new DataGridCellInfo(GDScheduleDays.Items[GDScheduleDays.SelectedIndex], GDScheduleDays.Columns[9]);
                var cellContent = dataGridCellInfo.Column.GetCellContent(dataGridCellInfo.Item);
                TextBox TBWorload = (TextBox)VisualTreeHelper.GetChild(cellContent, 0);

                if (scheduleDay.exitTwo.Length == 4 && !scheduleDay.exitTwo.Contains(':'))
                {
                    TimeSpan result;
                    String formated = Formatter.getInstance.formatHour(scheduleDay.exitTwo);
                    formated = scheduleDay.exitTwo = TimeSpan.TryParse(formated, out result) ? formated : "";
                    scheduleDay.workload = getWorkload(scheduleDay);
                    TBExitTwo.Text = formated;
                    TBWorload.Text = scheduleDay.workload;
                    updateWeekWorload();
                    TBExitTwo.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }
                else if (!scheduleDay.exitTwo.Replace(":", "").All(char.IsNumber) && !scheduleDay.exitTwo.Equals("FOLGA"))
                {
                    scheduleDay.exitTwo = "";
                    TBExitTwo.Text = "";
                }
                else
                {
                    scheduleDay.workload = getWorkload(scheduleDay);
                    TBWorload.Text = scheduleDay.workload;
                    updateWeekWorload();
                }
            }
        }

        private void cellEntryThree_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox TBEntryThree = (TextBox)sender;
            ScheduleDay scheduleDay = ((FrameworkElement)sender).DataContext as ScheduleDay;

            if (GDScheduleDays.SelectedIndex > -1)
            {
                var dataGridCellInfo = new DataGridCellInfo(GDScheduleDays.Items[GDScheduleDays.SelectedIndex], GDScheduleDays.Columns[9]);
                var cellContent = dataGridCellInfo.Column.GetCellContent(dataGridCellInfo.Item);
                TextBox TBWorload = (TextBox)VisualTreeHelper.GetChild(cellContent, 0);

                if (scheduleDay.entryThree.Length == 4 && !scheduleDay.entryThree.Contains(':'))
                {
                    TimeSpan result;
                    String formated = Formatter.getInstance.formatHour(scheduleDay.entryThree);
                    formated = scheduleDay.entryThree = TimeSpan.TryParse(formated, out result) ? formated : "";
                    scheduleDay.workload = getWorkload(scheduleDay);
                    TBEntryThree.Text = formated;
                    TBWorload.Text = scheduleDay.workload;
                    updateWeekWorload();
                    TBEntryThree.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }
                else if (!scheduleDay.entryThree.Replace(":", "").All(char.IsNumber) && !scheduleDay.entryThree.Equals("FOLGA"))
                {
                    scheduleDay.entryThree = "";
                    TBEntryThree.Text = "";
                }
                else
                {
                    scheduleDay.workload = getWorkload(scheduleDay);
                    TBWorload.Text = scheduleDay.workload;
                    updateWeekWorload();
                }
            }
        }

        private void cellExitThree_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox TBExitThree = (TextBox)sender;
            ScheduleDay scheduleDay = ((FrameworkElement)sender).DataContext as ScheduleDay;

            if (GDScheduleDays.SelectedIndex > -1)
            {
                var dataGridCellInfo = new DataGridCellInfo(GDScheduleDays.Items[GDScheduleDays.SelectedIndex], GDScheduleDays.Columns[9]);
                var cellContent = dataGridCellInfo.Column.GetCellContent(dataGridCellInfo.Item);
                TextBox TBWorload = (TextBox)VisualTreeHelper.GetChild(cellContent, 0);

                if (scheduleDay.exitThree.Length == 4 && !scheduleDay.exitThree.Contains(':'))
                {
                    TimeSpan result;
                    String formated = Formatter.getInstance.formatHour(scheduleDay.exitThree);
                    formated = scheduleDay.exitThree = TimeSpan.TryParse(formated, out result) ? formated : "";
                    scheduleDay.workload = getWorkload(scheduleDay);
                    TBExitThree.Text = formated;
                    TBWorload.Text = scheduleDay.workload;
                    updateWeekWorload();
                }
                else if (!scheduleDay.exitThree.Replace(":", "").All(char.IsNumber) && !scheduleDay.exitThree.Equals("FOLGA"))
                {
                    scheduleDay.exitThree = "";
                    TBExitThree.Text = "";
                }
                else
                {
                    scheduleDay.workload = getWorkload(scheduleDay);
                    TBWorload.Text = scheduleDay.workload;
                    updateWeekWorload();
                }
            }
        }

        private void cellEntryTolerance_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox TBEntryTolerance = (TextBox)sender;
            TBEntryTolerance.Text = TBEntryTolerance.Text.All(char.IsNumber) ? TBEntryTolerance.Text : "";
        }

        private void cellExitTolerance_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox TBExitTolerance = (TextBox)sender;
            TBExitTolerance.Text = TBExitTolerance.Text.All(char.IsNumber) ? TBExitTolerance.Text : "";
        }

        private void cellEndOfficeHour_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox TBOfficeGour = (TextBox)sender;
            TimeSpan result;
            String formated = Formatter.getInstance.formatHour(TBOfficeGour.Text);
            formated = TimeSpan.TryParse(formated, out result) ? formated : "";
            TBOfficeGour.Text = formated;
        }

        private void tbNightly_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox TBNightly = (TextBox)sender;
            TimeSpan result;
            String formated = Formatter.getInstance.formatHour(TBNightly.Text);
            formated = TimeSpan.TryParse(formated, out result) ? formated : "";
            TBNightly.Text = formated;
        }

        private void tbOvertimePercentage_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox TBOvertimePercentage = (TextBox)sender;
            if (!"".Equals(TBOvertimePercentage.Text))
            {
                string formated = Convert.ToInt16(TBOvertimePercentage.Text) > 100 ? "100" : TBOvertimePercentage.Text;
                TBOvertimePercentage.Text = formated;
            }
        }

        private void DataGridCell_GotFocus(object sender, EventArgs e)
        {
            DataGridCell cell = sender as DataGridCell;
            ContentPresenter  contentPresenter = (ContentPresenter)cell.Content;
            TextBox tb = (TextBox) VisualTreeHelper.GetChild(contentPresenter, 0);
            tb.Focus();
        }

        private int compareHours(String hrOne, String hrTwo)
        {
            TimeSpan hrOneTS = TimeSpan.Parse(hrOne);
            TimeSpan hrTwoTS = TimeSpan.Parse(hrTwo);

            return hrOneTS.CompareTo(hrTwoTS);
        }


        private string getWorkload(ScheduleDay sd)
        {
            TimeSpan workload = TimeSpan.Parse("00:00");

            if ((!"".Equals(sd.EntryOne) && sd.EntryOne != null && sd.EntryOne.Contains(":")) && (!"".Equals(sd.exitOne) && sd.exitOne != null && sd.exitOne.Contains(":")))
            {
                TimeSpan entry;
                TimeSpan exit;
                TimeSpan.TryParse(sd.EntryOne, out entry);
                TimeSpan.TryParse(sd.exitOne, out exit);

                if (entry != null && exit != null)
                {
                    workload += exit - entry;
                }
            }

            if ((!"".Equals(sd.entryTwo) && sd.entryTwo != null && sd.entryTwo.Contains(":")) && (!"".Equals(sd.exitTwo) && sd.exitTwo != null && sd.exitTwo.Contains(":")))
            {
                TimeSpan entry;
                TimeSpan exit;
                TimeSpan.TryParse(sd.entryTwo, out entry);
                TimeSpan.TryParse(sd.exitTwo, out exit);

                if (entry != null && exit != null)
                {
                    workload += exit - entry;
                }
            }

            if ((!"".Equals(sd.entryThree) && sd.entryThree != null && sd.entryThree.Contains(":")) && (!"".Equals(sd.exitThree) && sd.exitThree != null && sd.exitThree.Contains(":")))
            {
                TimeSpan entry;
                TimeSpan exit;
                TimeSpan.TryParse(sd.entryThree, out entry);
                TimeSpan.TryParse(sd.exitThree, out exit);

                if (entry != null && exit != null)
                {
                    workload += exit - entry;
                }
            }

            return workload.ToString(@"hh\:mm");
        }

        private void updateWeekWorload()
        {
            TimeSpan weekWorkload = TimeSpan.Parse("00:00");

            foreach (ScheduleDay sd in GDScheduleDays.ItemsSource)
            {
                if (sd.workload != null && sd.workload.Contains(":"))
                {
                    weekWorkload = weekWorkload.Add(TimeSpan.Parse(sd.workload));
                }
            }

            TBWeekWorkload.Text = string.Format("{0}:{1}", Convert.ToInt16(weekWorkload.TotalHours).ToString().PadLeft(2, '0'), Convert.ToInt16(weekWorkload.Minutes).ToString().PadLeft(2, '0'));
        }
    }
}
