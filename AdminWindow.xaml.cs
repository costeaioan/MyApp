using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
using System.Windows.Shapes;
using MyModel;


namespace MyApp
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
   
    enum ActionState
        {
            New,
            Edit,
            Delete,
            Nothing
        }
    
    
    public partial class AdminWindow : Window
    {
        ActionState action = ActionState.Nothing;
        MyDBDataSet myDBDataSet= new MyDBDataSet();
        MyDBDataSetTableAdapters.UsersTableAdapter tblUsersAdapter= new MyDBDataSetTableAdapters.UsersTableAdapter();
        Binding txtUsernameBinding = new Binding();
        Binding txtPasswordBinding = new Binding();
        Admin myAdmin = null;



        public AdminWindow()
        {
            InitializeComponent();
            grdMain.DataContext = myDBDataSet.Users;
            txtUsernameBinding.Path = new PropertyPath("Username");
            txtPasswordBinding.Path = new PropertyPath("Password");
            txtUsername.SetBinding(TextBox.TextProperty, txtUsernameBinding);
            txtPassword.SetBinding(TextBox.TextProperty, txtPasswordBinding);


            if (LoginWindow.loggedAdmin != null)
            {
                myAdmin = LoginWindow.loggedAdmin;
                lblUsername.Content = "Welcome " + myAdmin.Username + " ! Admin Mode ON";
            }

        }

        

        private void lstUsers_Loaded(object sender, RoutedEventArgs e)
        {
            lstPhonesLoad();
        }


        private void lstPhonesLoad()
        {
            tblUsersAdapter.Fill(myDBDataSet.Users);
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.New;
            btnNew.IsEnabled = false;
            btnEdit.IsEnabled = false;
            btnDelete.IsEnabled = false;

            btnSave.IsEnabled = true;
            btnCancel.IsEnabled = true;
            lstUsers.IsEnabled = false;
            btnPrevious.IsEnabled = false;
            btnNext.IsEnabled = true;
            txtUsername.IsEnabled = true;
            txtPassword.IsEnabled = true;

            BindingOperations.ClearBinding(txtUsername, TextBox.TextProperty);
            BindingOperations.ClearBinding(txtPassword, TextBox.TextProperty);
            txtUsername.Text = "";
            txtPassword.Text = "";
            Keyboard.Focus(txtUsername);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Edit;
            string tempUsername = txtUsername.Text.ToString();
            string tempPassword = txtPassword.Text.ToString();

            
            btnSave.IsEnabled = true;
            btnCancel.IsEnabled = true;
            lstUsers.IsEnabled = false;
            btnPrevious.IsEnabled = false;
            btnNext.IsEnabled = false;
            lstUsers.IsEnabled = true;
            txtUsername.IsEnabled = true;
            txtPassword.IsEnabled = true;
            

            BindingOperations.ClearBinding(txtUsername, TextBox.TextProperty);
            BindingOperations.ClearBinding(txtPassword, TextBox.TextProperty);
           
            txtUsername.Text = tempUsername;
            txtPassword.Text = tempPassword;
            Keyboard.Focus(txtUsername);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Delete;
            string tempUsername = txtUsername.Text.ToString();
            string tempPassword = txtPassword.Text.ToString();
            btnNew.IsEnabled = false;
            btnEdit.IsEnabled = false;
            btnDelete.IsEnabled = false;
            btnSave.IsEnabled = true;
            btnCancel.IsEnabled = true;
            lstUsers.IsEnabled = false;
            btnPrevious.IsEnabled = false;
            btnNext.IsEnabled = false;
            BindingOperations.ClearBinding(txtUsername, TextBox.TextProperty);
            BindingOperations.ClearBinding(txtPassword, TextBox.TextProperty);
            txtUsername.Text = tempUsername;
            txtPassword.Text = tempPassword;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)

        {

            action = ActionState.Nothing;
            btnNew.IsEnabled = true;
            btnEdit.IsEnabled = true;
            btnEdit.IsEnabled = true;
            btnSave.IsEnabled = false;
            btnCancel.IsEnabled = false;
            lstUsers.IsEnabled = true;
            btnPrevious.IsEnabled = true;
            btnNext.IsEnabled = true;
            txtUsername.IsEnabled = false;
            txtPassword.IsEnabled = false;
            txtUsername.SetBinding(TextBox.TextProperty, txtUsernameBinding);
            txtPassword.SetBinding(TextBox.TextProperty, txtPasswordBinding);

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            using (MyEntitiesModel db = new MyEntitiesModel())
            {
                if (action == ActionState.New)
                {
                    try
                    {
                        User myUser = new User();
                        myUser.Username= txtUsername.Text.Trim();
                        myUser.Password = txtPassword.Text.Trim();

                        db.Users.Add(myUser);
                        try
                        {
                            Int32.Parse(myUser.Username);
                            MessageBox.Show("Username cannot be a number!", "Error");
                            safety_Cancel();
                        }
                        catch (FormatException)
                        {
                            db.SaveChanges();
                            myUser = db.Users.FirstOrDefault(u => u.Username == myUser.Username);
                            db.Records.Add(new Record { UserId = myUser.UserId });
                            db.SaveChanges();
                            DataRow newRow = myDBDataSet.Users.NewRow();
                            newRow.BeginEdit();
                            newRow["Username"] = txtUsername.Text.Trim();
                            newRow["Password"] = txtPassword.Text.Trim();
                            newRow.EndEdit();
                            myDBDataSet.Users.Rows.Add(newRow);
                            tblUsersAdapter.Update(myDBDataSet.Users);

                            myDBDataSet.AcceptChanges();
                        }
                        
                    }

                    catch (DataException ex)
                    {

                        myDBDataSet.RejectChanges();
                        MessageBox.Show(ex.Message);
                        safety_Cancel();
                    }

                    catch (FormatException dateEx)
                    {
                        myDBDataSet.RejectChanges();
                        MessageBox.Show(dateEx.Message);
                        safety_Cancel();
                    }

                    catch (System.ArgumentException ex)
                    {
                        myDBDataSet.RejectChanges();
                        MessageBox.Show(ex.Message);
                        safety_Cancel();
                    }

                    btnNew.IsEnabled = true;
                    btnEdit.IsEnabled = true;
                    btnSave.IsEnabled = false;
                    btnCancel.IsEnabled = false;
                    lstUsers.IsEnabled = true;
                    btnPrevious.IsEnabled = true;
                    btnNext.IsEnabled = true;
                    txtUsername.IsEnabled = false;
                    txtPassword.IsEnabled = false;
                }

                else
                if (action == ActionState.Edit)
                {
                    try
                    {


                        var myUser= db.Users.FirstOrDefault(u => u.Username == txtUsername.Text);
                        myUser.Username = txtUsername.Text;
                        myUser.Password = txtPassword.Text;

                        try
                        {
                            Int32.Parse(myUser.Username);
                            MessageBox.Show("Username cannot be a number!", "Error");
                            safety_Cancel();
                        }
                        catch (FormatException)
                        {
                            db.SaveChanges();
                            DataRow newRow = myDBDataSet.Users.NewRow();
                            newRow.BeginEdit();
                            newRow["Username"] = txtUsername.Text.Trim();
                            newRow["Password"] = txtPassword.Text.Trim();
                            newRow.EndEdit();
                            myDBDataSet.Users.Rows.Add(newRow);
                            tblUsersAdapter.Update(myDBDataSet.Users);
                            myDBDataSet.AcceptChanges();
                        }

                        
                    }

                    catch (DataException ex)
                    {

                        myDBDataSet.RejectChanges();
                        MessageBox.Show(ex.Message);
                    }

                    catch (FormatException dateEx)
                    {
                        myDBDataSet.RejectChanges();
                        MessageBox.Show(dateEx.Message);
                    }

                    catch (System.ArgumentException ex)
                    {
                        myDBDataSet.RejectChanges();
                        MessageBox.Show(ex.Message);
                    }

                    btnNew.IsEnabled = true;
                    btnEdit.IsEnabled = true;
                    btnDelete.IsEnabled = true;
                    btnSave.IsEnabled = false;
                    btnCancel.IsEnabled = false;
                    lstUsers.IsEnabled = true;
                    btnPrevious.IsEnabled = true;
                    btnNext.IsEnabled = true;
                    txtUsername.IsEnabled = false;
                    txtPassword.IsEnabled = false;
                    txtUsername.SetBinding(TextBox.TextProperty, txtUsernameBinding);
                    txtPassword.SetBinding(TextBox.TextProperty, txtPasswordBinding);
                }

                else
                if (action == ActionState.Delete)
                {
                    try
                    {
                        DataRow deleteRow = myDBDataSet.Users.Rows[lstUsers.SelectedIndex];

                        deleteRow.Delete();

                        var myUser = db.Users.FirstOrDefault(u => u.Username == txtUsername.Text);
                        db.Users.Remove(myUser);
                        db.SaveChanges();

                        tblUsersAdapter.Update(myDBDataSet.Users);
                        myDBDataSet.AcceptChanges();
                    }

                    catch (DataException ex)
                    {
                        myDBDataSet.RejectChanges();
                        MessageBox.Show(ex.Message);
                    }

                    btnNew.IsEnabled = true;
                    btnEdit.IsEnabled = true;
                    btnDelete.IsEnabled = true;
                    btnSave.IsEnabled = false;
                    btnCancel.IsEnabled = false;
                    lstUsers.IsEnabled = true;
                    btnPrevious.IsEnabled = true;
                    btnNext.IsEnabled = true;
                    txtUsername.IsEnabled = false;
                    txtPassword.IsEnabled = false;
                    txtUsername.SetBinding(TextBox.TextProperty, txtUsernameBinding);
                    txtPassword.SetBinding(TextBox.TextProperty, txtPasswordBinding);
                }
            }
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            //using System.ComponentMode1
            ICollectionView navigationView = CollectionViewSource.GetDefaultView(myDBDataSet.Users);
            navigationView.MoveCurrentToPrevious();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView navigationView = CollectionViewSource.GetDefaultView(myDBDataSet.Users);
            navigationView.MoveCurrentToNext();
        }

        private void lstPhones_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void safety_Cancel()
        {

            action = ActionState.Nothing;
            btnNew.IsEnabled = true;
            btnEdit.IsEnabled = true;
            btnEdit.IsEnabled = true;
            btnSave.IsEnabled = false;
            btnCancel.IsEnabled = false;
            lstUsers.IsEnabled = true;
            btnPrevious.IsEnabled = true;
            btnNext.IsEnabled = true;
            txtUsername.IsEnabled = false;
            txtPassword.IsEnabled = false;
            txtUsername.SetBinding(TextBox.TextProperty, txtUsernameBinding);
            txtPassword.SetBinding(TextBox.TextProperty, txtPasswordBinding);


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
