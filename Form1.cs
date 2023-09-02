using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private Dictionary<string, string> vehicles = new Dictionary<string, string>
        {
            { "trains", "rails" },
            { "planes", "motors" },
            { "cars", "wheels" }
            // Add more vehicles as needed
        };

        public Form1()
        {
            InitializeComponent();
        }

        private void InitializeDataGridView()
        {
            dataGridView1.AutoGenerateColumns = false;

            // Create columns for the DataGridView
            dataGridView1.Columns.Add("VehicleName", "Vehicle Name");
            dataGridView1.Columns.Add("MovementMethod", "Movement Method");

        }


        private void FillGrid()
        {
            try
            {
                var playersList = new List<Players>();

                foreach (var vehicle in vehicles)
                {
                    Players p = new Players
                    {
                        // Assuming you want to use "vehicle.Key" and "vehicle.Value" as values
                        Vehicle = vehicle.Key,
                        Movement = vehicle.Value,
                };

                    playersList.Add(p);
                }

                dataGridView1.DataSource = playersList;

                // Set the value for the Appearances column to display the image
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    var player = (Players)row.DataBoundItem;

                    // Check if the Appearances property is a valid image file path
                    if (File.Exists(player.Image))
                    {
                        // Load the image from the file path
                        using (Image image = Image.FromFile(player.Image))
                        {
                            // Set the cell in the "Appearances" column to display the image
                            var appearancesColumnIndex = dataGridView1.Columns["Image"].Index; // Use the correct column name
                            row.Cells[appearancesColumnIndex].Value = image;
                        }
                    }
                    else
                    {
                        // Handle the case where the image file does not exist
                        // You can set a default image or display an error message.
                        // For example, set a default image:
                        var appearancesColumnIndex = dataGridView1.Columns["Image"].Index; // Use the correct column name
                        var defaultImage = Image.FromFile("Images/car.png"); // Provide the path to your default image
                        row.Cells[appearancesColumnIndex].Value = defaultImage;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public class Players
        {
            public string? Vehicle { set; get; }
            public string? Movement { set; get; }
            public string? Image { set; get; }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FillGrid();
        }

        private int _previousIndex;
        private bool _sortDirection;

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == _previousIndex)
                _sortDirection ^= true; // toggle direction

            dataGridView1.DataSource = SortData((List<Players>)dataGridView1.DataSource, dataGridView1.Columns[e.ColumnIndex].Name, _sortDirection);

            _previousIndex = e.ColumnIndex;
        }

        public List<Players> SortData(List<Players> list, string column, bool ascending)
        {
            return ascending ?
                list.OrderBy(_ => _.GetType().GetProperty(column).GetValue(_)).ToList() :
                list.OrderByDescending(_ => _.GetType().GetProperty(column).GetValue(_)).ToList();
        }
    }
}
