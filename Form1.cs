using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;


namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private Dictionary<string, string> vehicles = new Dictionary<string, string>
        {
            { "trains", "rails" },
            { "airplanes", "motors" },
            { "cars", "wheels" }
            // Add more vehicles as needed
        };

        public Form1()
        {
            InitializeComponent();
            InitializeDataGridView();
            FillGrid();

            // Add this line to subscribe to the RowPostPaint event
            dataGridView1.RowPostPaint += DataGridView1_RowPostPaint;
        }

        private void DataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
            if (row.DataBoundItem is Players player)
            {
                // Check if the player object has an image and it's not null
                if (player.Image != null)
                {
                    int imageHeight = player.Image.Height;
                    if (imageHeight > row.Height)
                    {
                        row.Height = imageHeight;
                    }
                }
            }
        }



        private void InitializeDataGridView()
        {
            dataGridView1.AutoGenerateColumns = true;
        }

        public class Players
        {
            public string Vehicle { get; set; }
            public string Method { get; set; }
            public Image Image { get; set; } // Add this property
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
                        Method = vehicle.Value
                    };

                    // Load the image from the file path
                    p.Image = Image.FromFile($"Images\\{vehicle.Key.ToLower()}.png");

                    playersList.Add(p);
                }

                dataGridView1.DataSource = playersList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            // You can implement file opening logic here if needed
            // For now, it calls FillGrid to populate the DataGridView
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
