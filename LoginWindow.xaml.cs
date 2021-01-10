using System;
using System.Collections.Generic;
using System.Linq;
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
using MyModel;

namespace MyApp
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public static Admin loggedAdmin = null;
        public static User loggedUser = null;
        String tempUsername;
        String tempPass;
        public LoginWindow()
        {
            InitializeComponent();
            MyEntitiesModel model = new MyEntitiesModel();
            
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            tempUsername = txtUsername.Text.ToString();
            tempPass = txtPassword.Password.ToString();

            if(tempUsername.Length==0 || tempPass.Length == 0)
            {
                MessageBox.Show("Username/Password fields cannot be empty!", "Error");
                
            }
            else
            {
                
                   
                    using(MyEntitiesModel db = new MyEntitiesModel())
                    {
                        var admin = db.Admins.FirstOrDefault(u => u.Username == tempUsername && u.Password == tempPass);
                        var user = db.Users.FirstOrDefault(u => u.Username == tempUsername && u.Password == tempPass);

                    if (admin != null)
                    {
                        loggedAdmin = admin;
                        MessageBox.Show("Succes", "Welcome Admin");
                        this.Hide();
                        var adminWindow = new AdminWindow();
                        adminWindow.Closed += (s, args) => this.Close();
                        adminWindow.Show();
                        
                    }
                    else if (user != null)
                    {
                        loggedUser = user;
                        MessageBox.Show("Succes", "Succesful login!");
                        this.Hide();
                        var main = new MainWindow();
                        main.Closed += (s, args) => this.Close();
                        main.Show();
                    }

                    else if (user == null && admin == null)
                        {
                        MessageBox.Show("Wrong username/password combination!");
                        txtUsername.Clear();
                        txtPassword.Clear();
                        
                        }
                    }
                    
                
            }
        }
    }
}
