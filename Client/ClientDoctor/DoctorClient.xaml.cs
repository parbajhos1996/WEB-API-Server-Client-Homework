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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClientDoctor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class DoctorClient : Window
    {
        public DoctorClient()
        {
            InitializeComponent();
            GetPatient();
        }

        public void GetPatient()
        {
            Patients.ItemsSource = GetPatientList();
        }

        private void PatientClick(object sender, RoutedEventArgs e)
        {
            var PatientList = GetPatientList();

            if (Patients.SelectedIndex != -1)
            {
                patient currentPatient = PatientList.ElementAt(Patients.SelectedIndex);
                Address.Text = ("Adress: " + currentPatient.Address);
                TajNumber.Text = ("Taj Number: " + currentPatient.TajNumber);
                Complaint.Text = ("Complaint: " + currentPatient.Complaint);
            }
        }

        private void PatientDeleteClick(object sender, RoutedEventArgs e)
        {
            using (var client = new HttpClient())
            {
                patient currentPatient = (patient)Patients.SelectedItem;
                currentPatient.Id = GetPatientList().First(p => p.Equals(currentPatient)).Id;
                var json = JsonConvert.SerializeObject(currentPatient);
                var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");

                var result = client.PostAsync("http://localhost:61111/api/patient", content).Result;
                if (!result.IsSuccessStatusCode)
                {
                    MessageBox.Show("Fail! " + result.StatusCode);
                }
                else
                {
                    PatientDeletedProcess(currentPatient);
                }
            }
        }

        private void PatientDeletedProcess(patient currentPatient)
        {
            var db = new patientEntities();
            patient pat = db.patients.Where(p => p.TajNumber == currentPatient.TajNumber).FirstOrDefault();
            if (pat != null)
            {
                db.patients.Remove(pat);
                db.SaveChanges();
            }
            MessageBox.Show("Patient deleted succesfully.");
            GetPatient();
        }

        private void RefreshClick(object sender, RoutedEventArgs e)
        {
            GetPatient();
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
