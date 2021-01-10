using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>    
    
    public partial class MainWindow : Window
    {
        //Binding txtBicepsCurlsBinding = new Binding();
        //Binding txtChestPressBinding= new Binding();
        //Binding txtRunningBinding = new Binding();
        //Binding txtPullUpsBinding = new Binding();
        int tempBicepsCurls;
        int tempPullUps;
        int tempRunning;
        int tempChestPress;
        //Record myRecord = null;
        MyEntitiesModel model = new MyEntitiesModel();
        //CollectionViewSource recordViewSource;
        public int myId;
        User myUser = null;
        //Binding lblUsernameBinding = new Binding();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;


            if (LoginWindow.loggedUser != null)
            {
                myUser = LoginWindow.loggedUser;
                myId = myUser.UserId;
                
                //lblUsernameBinding.Path = new PropertyPath("Username");
                //lblUsername.SetBinding(Label.ContentProperty,lblUsernameBinding);
                
                    
                using(MyEntitiesModel db = new MyEntitiesModel())
                {
                    var myRecord = db.Records.FirstOrDefault(u => u.UserId == myId);
                    
                    

                    
                    //txtBicepsCurlsBinding.Path = new PropertyPath("BicepsCurls");
                    //txtChestPressBinding.Path = new PropertyPath("ChestPress");
                    //txtRunningBinding.Path = new PropertyPath("Running");
                    //txtPullUpsBinding.Path = new PropertyPath("PullUps");

                    //txtBicepsCurls.SetBinding(TextBox.TextProperty, txtBicepsCurlsBinding);
                    //txtChestPress.SetBinding(TextBox.TextProperty, txtChestPressBinding);
                    //txtRunning.SetBinding(TextBox.TextProperty, txtRunningBinding);
                    //txtPullUps.SetBinding(TextBox.TextProperty, txtPullUpsBinding);

                    
                    txtBicepsCurls.Text = myRecord.BicepsCurls.ToString();
                    txtChestPress.Text = myRecord.ChestPress.ToString();
                    txtRunning.Text = myRecord.RunningKm.ToString();
                    txtPullUps.Text = myRecord.PullUps.ToString();

                    


                    int bicepsCurls = myRecord.BicepsCurls;
                    int chestPress = myRecord.ChestPress;
                    int running = myRecord.RunningKm;
                    int pullUps = myRecord.PullUps;
                    lblUsername.Content = "Welcome " + myUser.Username + "!"; 


                }
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //recordViewSource =
            //    ((System.Windows.Data.CollectionViewSource)(this.FindResource("recordViewSource")));

            //   model.Records.Load();
            //recordViewSource.Source = myUser.Username;

        }

        private void SetValidationBinding()
        {
            Binding usernameBinding = new Binding();
            //usernameBinding.Source = recordViewSource;
            usernameBinding.Path = new PropertyPath("Username");
            usernameBinding.NotifyOnValidationError = true;
            usernameBinding.Mode = BindingMode.TwoWay;
            usernameBinding.UpdateSourceTrigger =
           UpdateSourceTrigger.PropertyChanged;
            
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            using (MyEntitiesModel db = new MyEntitiesModel())
            {

                try
                {
                    tempBicepsCurls = Int32.Parse(txtBicepsCurls.Text);


                }
                catch (FormatException)
                {
                    MessageBox.Show("Please enter a number!", "Error");
                    txtBicepsCurls.Clear();
                }

                try
                {
                    tempPullUps = Int32.Parse(txtPullUps.Text);

                }
                catch (FormatException)
                {
                    MessageBox.Show("Please enter a number!", "Error");
                    txtPullUps.Clear();
                }

                try
                {
                    tempChestPress = Int32.Parse(txtChestPress.Text);

                }
                catch (FormatException)
                {
                    MessageBox.Show("Please enter a number!", "Error");
                    txtChestPress.Clear();
                }

                try
                {
                    tempRunning = Int32.Parse(txtRunning.Text);


                }
                catch (FormatException)
                {
                    MessageBox.Show("Please enter a number!", "Error");
                    txtRunning.Clear();
                }


               var myRecord = db.Records.FirstOrDefault(u => u.UserId == myId);


                myRecord.BicepsCurls = tempBicepsCurls;
                myRecord.PullUps = tempPullUps;
                myRecord.ChestPress = tempChestPress;
                myRecord.RunningKm = tempRunning;

                db.SaveChanges();

                MessageBox.Show("Data updated succesfully!", "Succes");

            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            var login = new LoginWindow();
            login.Closed += (s, args) => this.Close();
            login.Show();
        }
    }
}
