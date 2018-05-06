using System;
using System.Data;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    { 
        private readonly BindingSource _bs = new BindingSource();

        public Form1()
        {
            InitializeComponent();
            Shown += Form1_Shown;
        }
        /// <summary>
        /// Load table from SQL-Server table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Shown(object sender, EventArgs e)
        {
            var ops = new DataOperations();
            _bs.DataSource = ops.Read();
            DataGridView1.DataSource = _bs;
        }
        /// <summary>
        /// Attempt to update current row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdUpdateCurrent_Click(object sender, EventArgs e)
        {
            DataRow row = ((DataRowView)_bs.Current).Row;
            DataOperations ops = new DataOperations();

            if (ops.PersonUpdate(row.Field<string>("FirstName"), row.Field<string>("LastName"), row.Field<int>("id")))
            {
                MessageBox.Show("Update successful");
            }
            else
            {
                if (ops.HasException)
                {
                    if (ops.SqlException.Message.Contains("unique index"))
                    {
                        row.RejectChanges();
                        string fieldPluralize = "field";

                        if (ops.ConstraintColumns.Contains(","))
                        {
                            fieldPluralize += "s";
                        }

                        MessageBox.Show($"The change to {fieldPluralize} {ops.ConstraintColumns} values '{ops.ConstraintValue}' violates a constraint, reverting back to pre-edit value");

                    }
                    else
                    {
                        MessageBox.Show("Update failed, contact support.");
                    }
                }
            }

        }
        /// <summary>
        /// Setup for constraint violation on first/last name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdSetupForFailure_Click(object sender, EventArgs e)
        {
            DataRow row = ((DataRowView)_bs.Current).Row;
            row.SetField<string>("FirstName", "Karen");
            row.SetField<string>("LastName", "Payne");
        }
    }
}
