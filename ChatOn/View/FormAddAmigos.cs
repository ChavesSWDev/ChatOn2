using ChatOn.Controller;
using ChatOn.Data;
using ChatOn.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ChatOn.Model.Usuarios;

namespace ChatOn.View
{
    public partial class FormAddAmigos : Form
    {
        private List<Usuarios> usersList;
        private Usuarios loggedUser;

        public FormMenu parentForm { get; set; }
        public Color CorSecundaria { get; set; }
        public Color SelectedColor { get; set; }
        public Color SelectedColor2 { get; set; }
        public string email;

        private Usuarios usuario;

        private Panel panel;
        public FormAddAmigos()
        {
            InitializeComponent();
        }
        private string loginUserLogado;
        private string emailUserLogado;
        public FormAddAmigos(FormMenu parentForm, string userLogin, string userLogado, string emailLogado, string senhaLogado) : this()
        {
            this.parentForm = parentForm;
            loginUserLogado = userLogin;
            emailUserLogado = emailLogado;
        }

        public void UpdateColors(Color selectedColor, Color selectedColor2)
        {
            this.BackColor = selectedColor2;
            if (parentForm != null)
            {
                UpdatePanelControlsColors(selectedColor, selectedColor2);
            }
        }

        private void UpdatePanelControlsColors(Color selectedColor, Color selectedColor2)
        {
            // Update the background colors of controls within the panel
            foreach (Control control in parentForm.SplitContainer1.Panel2.Controls)
            {
                control.BackColor = selectedColor2;
                // Update other properties like ForeColor, etc., if needed
            }
        }

        private void FormAddAmigos_Load(object sender, EventArgs e)
        {
            if (parentForm != null)
            {
                UpdateColors(parentForm.selectedColor, parentForm.selectedColor2);
            }

            LoadUsersFromDatabase();
            PopulateListBox();
            ShowFriendRequests();
        }

        private void ShowFriendRequests()
        {
            // Find the logged-in user
            Usuarios loggedUser = UsuarioController.Context.Usuario
                .Include(u => u.PedidoAmizade) // Include PedidoAmizade navigation property
                .Include(u => u.Amigos) // Include Amigos navigation property
                .FirstOrDefault(u => u.Login == loginUserLogado);

            // Clear the existing items in listBoxPedidosAmizade
            listBoxPedidosAmizade.Items.Clear();

            // Check if the logged-in user has any pending friend requests
            if (loggedUser != null && loggedUser.PedidoAmizade.Count > 0)
            {
                foreach (var friendRequestUser in loggedUser.PedidoAmizade)
                {
                    // Check if the friend request user is not already in the friend list
                    if (!loggedUser.Amigos.Contains(friendRequestUser))
                    {
                        listBoxPedidosAmizade.Items.Add(friendRequestUser.Email);
                    }
                }
            }
            else
            {
                listBoxPedidosAmizade.Items.Add("Não há pedidos de amizades :(");
            }
        }

        private void LoadUsersFromDatabase()
        {
            // Assuming you have a context object named "dbContext" to interact with your database
            using (var dbContext = new DataContext())
            {
                usersList = UsuarioController.Context.Usuario.ToList();
            }
        }

        private void PopulateListBox()
        {
            // Clear the existing items in listBoxUsersDataBase
            listBoxUsersDataBase.Items.Clear();

            // Get the logged-in user
            Usuarios loggedUser = UsuarioController.Context.Usuario
                .Include(u => u.Amigos) // Include Amigos navigation property
                .FirstOrDefault(u => u.Login == loginUserLogado);

            if (loggedUser != null)
            {
                // Add the email addresses to listBoxUsersDataBase, excluding friends
                foreach (var user in usersList)
                {
                    // Check if the user is not the logged-in user and not already in the friend list
                    if (user.Login != loginUserLogado && !loggedUser.Amigos.Contains(user))
                    {
                        listBoxUsersDataBase.Items.Add(user.Email);
                    }
                }
            }
        }

        private void btnEnviarPedidoAmizade_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtEnviarPedidoAmigo.Text))
            {
                MessageBox.Show("Por favor, digite um email válido.");
                return;
            }

            string email = txtEnviarPedidoAmigo.Text.Trim();

            // Find the user with the matching email
            Usuarios user = usersList.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                MessageBox.Show("Email não encontrado.");
                return;
            }

            // Check if the user is not the logged-in user
            if (user.Login == loginUserLogado)
            {
                MessageBox.Show("Você não pode enviar um pedido de amigo para si mesmo.");
                return;
            }

            if (IsFriend(user))
            {
                MessageBox.Show($"{txtEnviarPedidoAmigo.Text} já é seu amigo.");
                return;
            }

            try
            {
                // Send the friend request
                UsuarioController.EnviarPedidoAmizade(emailUserLogado, user.Email);

                // Optional: Show a message to indicate that the friend request was sent
                MessageBox.Show($"Pedido de amizade enviado para {email}.");

                // Clear the text box
                txtEnviarPedidoAmigo.Text = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao enviar o pedido de amizade: " + ex.Message);
            }
        }

        private bool IsFriend(Usuarios user)
        {
            // Check if the user is already a friend of the logged-in user
            Usuarios loggedUser = UsuarioController.Context.Usuario.FirstOrDefault(u => u.Login == loginUserLogado);
            return loggedUser.Amigos.Contains(user);
        }

        private void btnAceitarAmizade_Click(object sender, EventArgs e)
        {
            string emailToAccept = txtAceitarNegarAmizade.Text;

            // Find the logged-in user
            Usuarios loggedUser = UsuarioController.Context.Usuario
                .Include(u => u.PedidoAmizade) // Include PedidoAmizade navigation property
                .Include(u => u.Amigos) // Include Amigos navigation property
                .FirstOrDefault(u => u.Login == loginUserLogado);

            if (loggedUser != null)
            {
                // Check if the user to accept is already a friend
                Usuarios friendUser = loggedUser.Amigos.FirstOrDefault(u => u.Email == emailToAccept);

                if (friendUser != null)
                {
                    MessageBox.Show($"{emailToAccept} já é seu amigo.");
                    return;
                }

                // Find the friend request user with the specified email
                Usuarios friendRequestUser = loggedUser.PedidoAmizade.FirstOrDefault(u => u.Email == emailToAccept);

                if (friendRequestUser != null)
                {
                    // Add the friend request user to the Amigos list of the logged-in user
                    loggedUser.Amigos.Add(friendRequestUser);

                    // Add the logged-in user to the Amigos list of the friend request user
                    friendRequestUser.Amigos.Add(loggedUser);

                    // Remove the friend request user from the PedidoAmizade list of the logged-in user
                    loggedUser.PedidoAmizade.Remove(friendRequestUser);

                    // Remove the logged-in user from the PedidoAmizade list of the friend request user
                    friendRequestUser.PedidoAmizade.Remove(loggedUser);

                    // Save changes to the database
                    UsuarioController.Context.SaveChanges();
                    MessageBox.Show(friendRequestUser.Email + " foi adicionado.");

                    Refresh();
                    // Refresh the friend requests list
                    ShowFriendRequests();
                }
                else
                {
                    MessageBox.Show("Usuário não encontrado na lista de pedidos de amizade.");
                }
            }
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            if (parentForm != null)
            {
                parentForm.AddFormLogado();
            }
        }
    }
}