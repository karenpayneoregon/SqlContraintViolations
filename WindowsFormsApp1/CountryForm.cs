using System;
using System.Data;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class CountryForm : Form
    {
        public CountryForm()
        {
            InitializeComponent();
            Shown += CountryForm_Shown;
        }

        private void CountryForm_Shown(object sender, EventArgs e)
        {
            DataOperations ops = new DataOperations();
            dataGridView1.DataSource = ops.ReadCountries();
            // ReSharper disable once PossibleNullReferenceException
            dataGridView1.Columns["id"].ReadOnly = true;
        }

        private void cmdAdd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtCountryName.Text))
            {
                DataOperations ops = new DataOperations();

                int id = 0;
                string message = "";

                if (ops.InsertCountry1(txtCountryName.Text, ref id, ref message))
                {
                    ((DataTable) dataGridView1.DataSource).Rows.Add(id, txtCountryName.Text);
                }
                else
                {
                    MessageBox.Show(message);
                }
            }
            else
            {
                MessageBox.Show("Please add a country name");
            }
        }
    }
}
