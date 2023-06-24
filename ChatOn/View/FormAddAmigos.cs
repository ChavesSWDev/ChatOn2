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
            // Atualize as cores de fundo dos controles no painel
            foreach (Control control in parentForm.SplitContainer1.Panel2.Controls)
            {
                control.BackColor = selectedColor2;
                //Atualize outras propriedades como ForeColor, etc., se necessário
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
            // encontre o usuário logado
            Usuarios loggedUser = UsuarioController.Context.Usuario
                .Include(u => u.PedidoAmizade) // Incluir propriedade de navegação PedidoAmizade
                .Include(u => u.Amigos) // Incluir propriedade de navegação Amigos
                .FirstOrDefault(u => u.Login == loginUserLogado);

            //Limpar os itens existentes em listBoxPedidosAmizade
            listBoxPedidosAmizade.Items.Clear();

            // Verifique se o usuário conectado tem alguma solicitação de amizade pendente
            if (loggedUser != null && loggedUser.PedidoAmizade.Count > 0)
            {
                foreach (var friendRequestUser in loggedUser.PedidoAmizade)
                {
                    // Verifique se o usuário da solicitação de amizade já não está na lista de amigos
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
            // Supondo que você tenha um objeto de contexto chamado "dbContext" para interagir com seu banco de dados
            using (var dbContext = new DataContext())
            {
                usersList = UsuarioController.Context.Usuario.ToList();
            }
        }

        private void PopulateListBox()
        {
            // Limpe os itens existentes em listBoxUsersDataBase
            listBoxUsersDataBase.Items.Clear();

            //Obtenha o usuário logado
            Usuarios loggedUser = UsuarioController.Context.Usuario
                .Include(u => u.Amigos) //Incluir propriedade de navegação Amigos
                .FirstOrDefault(u => u.Login == loginUserLogado);

            if (loggedUser != null)
            {
                // Adicione os endereços de e-mail a listBoxUsersDataBase, excluindo amigos
                foreach (var user in usersList)
                {
                    // Verifique se o usuário não é o usuário logado e não está na lista de amigos
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

            // Encontre o usuário com o e-mail correspondente
            Usuarios user = usersList.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                MessageBox.Show("Email não encontrado.");
                return;
            }

            // Verifique se o usuário não é o usuário logado
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
                //Envie a solicitação de amizade
                UsuarioController.EnviarPedidoAmizade(emailUserLogado, user.Email);

                //Opcional: Mostrar uma mensagem para indicar que a solicitação de amizade foi enviada
                MessageBox.Show($"Pedido de amizade enviado para {email}.");
                user.PedidoAmizade.Add(user);
                // Limpar a caixa de texto
                txtEnviarPedidoAmigo.Text = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao enviar o pedido de amizade: " + ex.Message);
            }
        }

        private bool IsFriend(Usuarios user)
        {
            // Verifique se o usuário já é amigo do usuário logado
            Usuarios loggedUser = UsuarioController.Context.Usuario.FirstOrDefault(u => u.Login == loginUserLogado);
            return loggedUser.Amigos.Contains(user);
        }

        private void btnNegarAmizade_Click(object sender, EventArgs e)
        {
            string txtEmailNegar = txtAceitarNegarAmizade.Text;

            // Encontre o usuário logado
            Usuarios loggedUser = UsuarioController.Context.Usuario
                .Include(u => u.PedidoAmizade) // Incluir propriedade de navegação PedidoAmizade
                .FirstOrDefault(u => u.Login == loginUserLogado);

            if (loggedUser != null)
            {
                // Encontre o usuário da solicitação de amizade com o e-mail especificado
                Usuarios friendRequestUser = loggedUser.PedidoAmizade.FirstOrDefault(u => u.Email == txtEmailNegar);

                if (friendRequestUser != null)
                {
                    // Remover o usuário de solicitação de amizade da lista PedidoAmizade do usuário logado
                    loggedUser.PedidoAmizade.Remove(friendRequestUser);

                    // salvar alterações no banco de dados
                    UsuarioController.Context.SaveChanges();

                    // Remova o usuário de solicitação de amizade do listBoxUsersDataBase ListBox
                    listBoxUsersDataBase.Items.Remove(txtEmailNegar);
                    //Limpe o texto em txtAceitarNegarAmizade TextBox
                    txtAceitarNegarAmizade.Clear();

                    MessageBox.Show("Convite de amizade negado com sucesso!");

                    Refresh();
                    // Atualize a lista de pedidos de amizade
                    ShowFriendRequests();
                }
                else
                {
                    MessageBox.Show("Usuário não encontrado na lista de pedidos de amizade.");
                }
            }
        }

        private void btnAceitarAmizade_Click(object sender, EventArgs e)
        {
            string emailToAccept = txtAceitarNegarAmizade.Text;

            // encontre o usuário logado
            Usuarios loggedUser = UsuarioController.Context.Usuario
                .Include(u => u.PedidoAmizade) // Incluir propriedade de navegação PedidoAmizade
                .Include(u => u.Amigos) //Incluir propriedade de navegação Amigos
                .FirstOrDefault(u => u.Login == loginUserLogado);

            if (loggedUser != null)
            {
                // Verifique se o usuário a aceitar já é um amigo
                Usuarios friendUser = loggedUser.Amigos.FirstOrDefault(u => u.Email == emailToAccept);

                if (friendUser != null)
                {
                    MessageBox.Show($"{emailToAccept} já é seu amigo.");
                    return;
                }

                // Encontre o usuário da solicitação de amizade com o e-mail especificado
                Usuarios friendRequestUser = loggedUser.PedidoAmizade.FirstOrDefault(u => u.Email == emailToAccept);

                if (friendRequestUser != null)
                {
                    // Adicione o usuário de solicitação de amizade à lista de Amigos do usuário conectado
                    loggedUser.Amigos.Add(friendRequestUser);

                    //Adicione o usuário conectado à lista de Amigos do usuário de solicitação de amizade
                    friendRequestUser.Amigos.Add(loggedUser);

                    // Remover o usuário de solicitação de amizade da lista PedidoAmizade do usuário logado
                    loggedUser.PedidoAmizade.Remove(friendRequestUser);

                    // Remova o usuário logado da lista PedidoAmizade do usuário de pedido de amizade
                    friendRequestUser.PedidoAmizade.Remove(loggedUser);

                    //Salvar alterações no banco de dados
                    UsuarioController.Context.SaveChanges();
                    MessageBox.Show(friendRequestUser.Email + " foi adicionado.");

                    Refresh();
                    //Atualize a lista de pedidos de amizade
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

        

        private void listBoxPedidosAmizade_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedItem = listBoxPedidosAmizade.SelectedItem?.ToString();

            // Atualize o TextBox txtEnviarPedidoAmigo com o texto do item selecionado
            txtAceitarNegarAmizade.Text = selectedItem;
        }

        private void listBoxUsersDataBase_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Obtenha o item selecionado do listBoxUsersDataBase ListBox
            string selectedItem = listBoxUsersDataBase.SelectedItem?.ToString();

            // Atualize o TextBox txtEnviarPedidoAmigo com o texto do item selecionado
            txtEnviarPedidoAmigo.Text = selectedItem;
        }
    }
}