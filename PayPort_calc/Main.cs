using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;

namespace PayPort_calc
{
    public partial class MainWindow : Window
    {
        int rate;
        int days;
        double salary;
        DateTime tempDateTime;

        public MainWindow()
        {
            InitializeComponent();
            List<string> beginTime = new List<string>(); 
            for (int i = 0; i < 24; i++) 
            {
                tempDateTime = new DateTime(2023, 3, 1, i, 00, 00);
                beginTime.Add(tempDateTime.ToShortTimeString());
            }

            comboBoxTime1.ItemsSource = beginTime;
            comboBoxTime2.ItemsSource = beginTime;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DateTime tempDate1 = Convert.ToDateTime(comboBoxTime1.SelectedItem);
            DateTime tempDate2 = Convert.ToDateTime(comboBoxTime2.SelectedItem);

            if (tempDate1 > DateTime.Today.AddHours(16)) { tempDate2 = tempDate2.AddDays(1); }

            try
            {
                rate = Convert.ToInt32(textBox2.Text); 
                days = Convert.ToInt32(textBox1.Text); 
            }
            catch
            {
                MessageBox.Show($"Значение '{Label2.Content}' и '{Label3.Content}' должны быть заполнены");
            }

            if (comboBoxTime1.SelectedItem != null && comboBoxTime2.SelectedItem != null)
            {
                TimeSpan intrval = tempDate2 - tempDate1;
                int hoursCount = Convert.ToInt32(intrval.Hours);
                if (hoursCount > 1) { hoursCount--; }

                if (checkBox2.IsChecked == false)
                {
                    days = hoursCount * days; 
                }

                double netSalary = (rate * days) * 0.87; 

                salary = NightHHCounter(netSalary, tempDate2, (days / hoursCount));

                Print(salary);
            }
            else { MessageBox.Show("Укажите часы работы"); }

        }


        /// <summary>
        /// Доплата за ночные часы
        /// </summary>
        /// <param name="temp1">сумма</param>
        /// <param name="tempDate2">время окончания смены</param>
        /// <param name="days">кол-во рабочих дней</param>
        /// <returns></returns>
        private double NightHHCounter(double temp1, DateTime tempDate2, int days)
        {
            int nightHours = Convert.ToInt32(tempDate2.Subtract(DateTime.Today.AddHours(22)).Hours);

            if (nightHours > 0)
            {
                if (nightHours >= 8) { nightHours = 7; }

                double a, b, c;
                a = rate * 0.2;
                b = days * nightHours;
                c = (a * b) * 0.87;

                return temp1 + c;
            }
            return temp1;
        }

        /// <summary>
        /// Вывод полученной суммы на экран
        /// </summary>
        /// <param name="wages">ЗП</param>
        private void Print(double wages)
        {
            Label1.Content = String.Format("{0:0.##}", wages);
        }

        private void checkBox2_Checked(object sender, RoutedEventArgs e)
        {
            Label2.Content = "Количество часов";
        }

        private void checkBox2_Unchecked(object sender, RoutedEventArgs e)
        {
            Label2.Content = "Количество дней";
        }

        private void Grid_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
        }

        private void textBox2_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
        }

        
        /// <summary>
        /// Открытие ссылки 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://github.com/dazai19") { UseShellExecute = true });
        }
    }
}
