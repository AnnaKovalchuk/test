using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Classes;
using WindowsFormsApp1.Models;
using WindowsFormsApp1.View;

namespace WindowsFormsApp1
{
    public partial class FormAuth : Form
    {
        public FormAuth()
        {
            InitializeComponent();
        }

        int sec = 5;
        bool vis = true;

        private void buttonAuth_Click(object sender, EventArgs e)
        {
            string login = textBoxLogin.Text;
            string password = textBoxPassword.Text;

            using (DBEventEntities DB = new DBEventEntities())
            {
                List<User> users = DB.User.Where(x => x.Login == login && x.Password == password).ToList();
                Models.User user = users.FirstOrDefault();

                if (user != null)
                {
                    Classes.Helper.idRole = user.UserRoleID;
                    List<Role> roles = DB.Role.Where(x => x.RoleID == Helper.idRole).ToList();
                    string role = roles.Select(x => x.RoleName).First();
                    MessageBox.Show($"Your role: {role}", "Welcome");
                    if (captcha == textBoxCaptcha.Text)
                    {
                        Form2 form2 = new Form2();
                        Hide();
                        form2.ShowDialog();
                        Show();
                    }
                    else
                    {
                        labelCaptcha.Text = GenerateCaptcha();
                        MessageBox.Show("Captcha incorrect","Error");
                    }
                }
                else
                {
                    MessageBox.Show("User not found", "Error");
                    labelCaptcha.Visible = true;
                    textBoxCaptcha.Visible = true;
                    labelCaptcha.Visible = true;
                    timerBlock.Enabled = true;
                    timerBlock.Start();
                    textBoxLogin.Enabled = false;
                    textBoxPassword.Enabled = false;
                    labelCaptcha.Text=GenerateCaptcha();
                }
            }
        }

        private void timerBlock_Tick(object sender, EventArgs e)
        {
            if(sec==0)
            {
                timerBlock.Enabled = false;
                timerBlock.Stop();
                textBoxLogin.Enabled = true;
                textBoxPassword.Enabled = true;
            }
            if(sec>0)
            {
                sec--;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (vis)
            {
                textBoxPassword.UseSystemPasswordChar = false;
                vis = false;
            }
            else
            {
                textBoxPassword.UseSystemPasswordChar = true;
                vis = true;
            }
        }

        string captcha="";
        private string GenerateCaptcha()
        {
            Random random = new Random();
            captcha = "";

            int lenght = random.Next(8,8);
            for(int i=0; i < lenght/2; i++)
            {
                captcha += (char)random.Next(65,90);
                captcha += (char)random.Next(48, 57);
            }
            return captcha;
        }
    }
}
