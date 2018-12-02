using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ClientAssistent
{
    /// <summary>
    /// Interaction logic for AssistentClient.xaml
    /// </summary>
    public partial class AssistentClient : Window
    {
        public AssistentClient()
        {
            InitializeComponent();

        }

        private void AddPatientClick(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(FirstName.Text) ||
                String.IsNullOrEmpty(LastName.Text) ||
                String.IsNullOrEmpty(Address.Text) ||
                String.IsNullOrEmpty(TajNumber.Text) ||
                String.IsNullOrEmpty(Complaint.Text))
            {
                MessageBox.Show("Incomplete patient.");
                return;
            }

            if (TajNumber.Text.Length != 9)
            {
                MessageBox.Show("Wrong Taj Number.");
                return;
            }

            using (var client = new HttpClient())
            {
                var currentPatient = new patient
                {
                    FirstName = FirstName.Text,
                    LastName = LastName.Text,
                    Address = Address.Text,
                    TajNumber = TajNumber.Text,
                    Complaint = Complaint.Text
                };

                if (GetPatientList().Contains(currentPatient))
                {
                    MessageBox.Show("Patient already exists.");
                    return;
                }

                var json = JsonConvert.SerializeObject(currentPatient);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var result = client.PostAsync("http://localhost:61111/api/patient", content).Result;
                if (!result.IsSuccessStatusCode)
                {
                    MessageBox.Show("Fail! " + result.StatusCode);
                }
                else
                {
                    PatientAddedProcess(currentPatient);
                }
            }
        }

        private void PatientAddedProcess(patient currentPatient)
        {
            FirstName.Text = string.Empty;
            LastName.Text = string.Empty;
            Address.Text = string.Empty;
            TajNumber.Text = string.Empty;
            Complaint.Text = string.Empty;
            var db = new patientEntities();
            db.patients.Add(currentPatient);
            db.SaveChanges();
            MessageBox.Show("Patient added successfully.");
        }

        private IList<patient> GetPatientList()
        {
            using (var client = new HttpClient())
            {
                var result = client.GetAsync("http://localhost:61111/api/patient").Result;
                if (result.IsSuccessStatusCode)
                {
                    var content = result.Content.ReadAsStringAsync().Result;
                    var PatientList = JsonConvert.DeserializeObject<IList<patient>>(content);
                    return PatientList;
                }
            }
            return null;
        }
    }
}
