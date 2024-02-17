using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Net.Http;


namespace WindowsFormsLab1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            int X = Convert.ToInt32(textBox1.Text);
            int Y = Convert.ToInt32(textBox2.Text);


            using (var client = new HttpClient())
            {
                var parameters = new FormUrlEncodedContent(new[]
                {
                        new KeyValuePair<string, string>("X", X.ToString()),
                        new KeyValuePair<string, string>("Y", Y.ToString())
                });

                var response = await client.PostAsync("http://localhost:5096/PVGSum/", parameters);
               
                
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    label4.Text = result;
                }
                else
                {
                    label4.Text = "Error: " + response.StatusCode;
                }
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!Char.IsDigit(number))
            {
                e.Handled = true;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
