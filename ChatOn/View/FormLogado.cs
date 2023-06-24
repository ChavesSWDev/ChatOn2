using ChatOn.Controller;
using ChatOn.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ChatOn.View.FormMenu;
using Message = ChatOn.Model.Message;
using Timer = System.Windows.Forms.Timer;

namespace ChatOn.View
{
    public partial class FormLogado : Form
    {
        public FormMenu parentForm { get; set; }
        public Color CorSecundaria { get; set; }
        public Color SelectedColor { get; set; }
        public Color SelectedColor2 { get; set; }

        public FormLogado()
        {
            InitializeComponent();
            btnEnviarMsg.Enabled = false;
            txtChat.Visible = false;
            txtMsgDigitada.Enabled = false;
        }
        private Usuarios usuario;
        private Usuarios selectedUser;
        private List<Usuarios> friendList;
        private Panel panel;
        private List<Usuarios> usersList;

        public FormLogado(FormMenu parentForm)
        {
            InitializeComponent();
            this.parentForm = parentForm;
            btnEnviarMsg.Enabled = false;
            txtMsgDigitada.Enabled = false;
            txtChat.Visible = false;
        }
        private Timer chatRefreshTimer;

        public FormLogado(FormMenu parentForm, Usuarios usuario)
        {
            InitializeComponent();
            usersList = new List<Usuarios> { usuario };
            btnEnviarMsg.Enabled = false;
            txtMsgDigitada.Enabled = false;
            txtChat.Visible = false;
            chatRefreshTimer = new Timer();
            chatRefreshTimer.Interval = 1000;
            chatRefreshTimer.Tick += (sender, e) =>
            {
                if (!txtChat.Focused)
                {
                    RefreshChat();
                }
            };


            chatRefreshTimer.Start();

            this.usuario = usuario;
            this.parentForm = parentForm;
            friendList = UsuarioController.GetFriendList(usuario.Id);
            if (parentForm != null)
            {

            }
            if (friendList != null && friendList.Count > 0)
            {
                selectedUser = friendList[0];
                currentChat = UsuarioController.GetChat(usuario, selectedUser);


                txtChat.Text = "";


                RefreshChat();

                foreach (var friend in friendList)
                {
                    listBoxAmigos.Items.Add(friend.NomeUsuario);
                }
                panelAmigos.FlowDirection = FlowDirection.TopDown;
                panelAmigos.WrapContents = false;
                panelAmigos.VerticalScroll.Enabled = true;
                panelAmigos.AutoScroll = true;

                foreach (var friend in usuario.Amigos)
                {
                    CreateTableLayoutForFriend(friend);
                }
            }
        }

        private TableLayoutPanel GetTableLayoutForFriend(Usuarios friend)
        {

            foreach (Control control in panelAmigos.Controls)
            {
                if (control is TableLayoutPanel tableLayout && tableLayout.Tag == friend)
                {
                    return tableLayout;
                }
            }

            return null;
        }
        private Chat currentChat;
        private void CreateTableLayoutForFriend(Usuarios friend)
        {
            if (!FriendRequestAccepted(friend))
            {
                return;
            }

            TableLayoutPanel existingTableLayout = GetTableLayoutForFriend(friend);
            if (existingTableLayout != null)
            {
                return;
            }

            // Verifique se o amigo é o usuário logado
            if (friend.Id == usuario.Id)
            {
                return;
            }

            TableLayoutPanel tableLayout = new TableLayoutPanel();
            tableLayout.AutoSize = true;
            tableLayout.ColumnCount = 2;
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 57));
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            // Adicione o amigo como uma tag ao layout da mesa
            tableLayout.Tag = friend;
            tableLayout.BackColor = SystemColors.Control;

            // Adicione o tableLayout ao painelAmigos
            panelAmigos.Controls.Add(tableLayout);
            PictureBox picBoxImgAmigo = new PictureBox();
            // Defina a imagem e o nome de usuário
            byte[] imageData = friend.ImagemPerfil;
            if (imageData != null && imageData.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream(imageData))
                {
                    Image profileImage = Image.FromStream(ms);
                    picBoxImgAmigo.SizeMode = PictureBoxSizeMode.StretchImage;
                    picBoxImgAmigo.Image = profileImage;
                    picBoxImgAmigo.Size = new Size(57, 57);
                    tableLayout.Controls.Add(picBoxImgAmigo, 0, 0);
                }
            }

            Label lblNomeAmigo = new Label();
            lblNomeAmigo.Text = friend.NomeUsuario + (friend.HasUnreadMessages ? " (!)" : "");
            lblNomeAmigo.Size = new Size(85, 23);
            lblNomeAmigo.TextAlign = ContentAlignment.MiddleLeft;
            tableLayout.Controls.Add(lblNomeAmigo, 1, 0);
            tableLayout.Cursor = Cursors.Hand;

            picBoxImgAmigo.Click += (sender, e) =>
            {
                OpenChatWithFriend(friend, lblNomeAmigo);
            };

            lblNomeAmigo.Click += (sender, e) =>
            {
                OpenChatWithFriend(friend, lblNomeAmigo);
            };

            tableLayout.Click += (sender, e) =>
            {
                // Armazenar o usuário selecionado
                selectedUser = friend;
                lblNomeAmigo.Text = friend.NomeUsuario;
                friend.HasUnreadMessages = false;

                // Abra o chat entre o usuário logado e o usuário selecionado
                currentChat = UsuarioController.GetChat(usuario, selectedUser);

                // Atualize o txtChat com o novo chat
                StringBuilder chatLog = new StringBuilder();
                foreach (Message message in currentChat.Messages)
                {
                    chatLog.AppendLine($"{message.Sender.NomeUsuario}: {message.Content}");
                }
                txtChat.Text = chatLog.ToString();

                //Atualize o texto lblChatComQuem para o nome de usuário do usuário selecionado
                lblChatComQuem.Text = selectedUser.NomeUsuario;
                btnEnviarMsg.Enabled = true;
                txtMsgDigitada.Enabled = true;
                txtChat.Visible = true;
                txtMsgDigitada.Focus();
                RefreshFriendList();
            };
        }

        private void OpenChatWithFriend(Usuarios friend, Label lblNomeAmigo)
        {
            // Armazenar o usuário selecionado
            selectedUser = friend;
            lblNomeAmigo.Text = friend.NomeUsuario;
            friend.HasUnreadMessages = false;

            // Abra o chat entre o usuário logado e o usuário selecionado
            currentChat = UsuarioController.GetChat(usuario, selectedUser);

            // Atualize o txtChat com o novo chat
            StringBuilder chatLog = new StringBuilder();
            foreach (Message message in currentChat.Messages)
            {
                chatLog.AppendLine($"{message.Sender.NomeUsuario}: {message.Content}");
            }
            txtChat.Text = chatLog.ToString();

            // Atualize o texto lblChatComQuem para o nome de usuário do usuário selecionado
            lblChatComQuem.Text = selectedUser.NomeUsuario;
            btnEnviarMsg.Enabled = true;
            txtMsgDigitada.Enabled = true;
            txtChat.Visible = true;
            txtMsgDigitada.Focus();
            RefreshFriendList();
        }

        private void RefreshFriendList()
        {
            listBoxAmigos.Items.Clear();

            foreach (var friend in friendList)
            {
                listBoxAmigos.Items.Add(friend.NomeUsuario + (friend.HasUnreadMessages ? " (!)" : ""));
            }
        }


        private bool FriendRequestAccepted(Usuarios friend)
        {
            // Encontre o usuário logado
            Usuarios loggedInUser = usersList.FirstOrDefault(u => u.Login == usuario.Login);

            // Verifique se a solicitação de amizade foi aceita por ambos os usuários
            bool isAccepted = loggedInUser?.Amigos.Any(u => u.Id == friend.Id) ?? false;
            bool isFriend = friend.Amigos.Any(u => u.Id == loggedInUser.Id);

            return isAccepted && isFriend;
        }

        private void RefreshChat()
        {
            if (selectedUser == null || currentChat == null)
            {
                return;
            }

            // Recupere os dados de bate-papo mais recentes do banco de dados
            Chat updatedChat = UsuarioController.GetChat(usuario, selectedUser);

            // armazenar a contagem de mensagens anterior
            int previousMessageCount = currentChat.Messages.Count;

            // Atualize o bate-papo atual com os dados mais recentes
            currentChat.Messages = updatedChat.Messages;

            // Armazena a posição de rolagem atual
            int scrollPosition = txtChat.GetPositionFromCharIndex(txtChat.SelectionStart).Y;

            // Determinar se novas mensagens foram adicionadas
            bool newMessagesAdded = currentChat.Messages.Count > previousMessageCount;

            // Atualize o txtChat com os dados atualizados do chat
            StringBuilder chatLog = new StringBuilder();
            for (int i = 0; i < currentChat.Messages.Count; i++)
            {
                Message message = currentChat.Messages[i];
                string senderName;
                if (message.Sender == usuario)
                {
                    senderName = "Eu";
                }
                else
                {
                    senderName = message.Sender.NomeUsuario;
                }

                chatLog.AppendLine(senderName + ": " + message.Content + " [" + message.Timestamp.ToString("HH:mm:ss") + "]");

                //Adicione uma nova linha se não for a última mensagem
                if (i < currentChat.Messages.Count)
                {
                    chatLog.AppendLine();
                }
            }
            string newText = chatLog.ToString();



            // Atualize o texto do txtChat
            txtChat.Text = newText;

            // Role até o final se novas mensagens foram adicionadas
            if (newMessagesAdded && friendList.Contains(selectedUser))
            {
                selectedUser.HasUnreadMessages = true;
                RefreshFriendList(); // Atualize a exibição da lista de amigos
                txtChat.SelectionStart = txtChat.Text.Length;
                txtChat.ScrollToCaret();
            }
        }

        private void btnEnviarMsg_Click(object sender, EventArgs e)
        {
            string message = txtMsgDigitada.Text.Trim();

            if (string.IsNullOrEmpty(message))
            {
                MessageBox.Show("Please enter a valid message.");
                return;
            }

            if (selectedUser == null)
            {
                MessageBox.Show("Please select a friend to send the message to.");
                return;
            }

            // Recupere o usuário logado
            Usuarios loggedUser = usuario;

            // Obtenha ou crie o chat entre o usuário logado e o usuário selecionado
            Chat chat = UsuarioController.GetChat(loggedUser, selectedUser);

            //Criar uma nova mensagem
            Message newMessage = new Message()
            {
                Sender = loggedUser,
                Content = message,
                Timestamp = DateTime.Now
            };

            // Adicione a mensagem ao chat
            chat.Messages.Add(newMessage);

            // Salve o chat no banco de dados
            UsuarioController.SaveChat(chat);

            // Atualize o txtChat com a nova mensagem
            txtChat.AppendText("Eu" + ": " + message + " [" + newMessage.Timestamp.ToString("HH:mm:ss") + "] ");

            // Role até a parte inferior do bate-papo
            txtChat.SelectionStart = txtChat.Text.Length;
            txtChat.ScrollToCaret();

            txtMsgDigitada.Text = "";
            txtMsgDigitada.Focus();
        }

        public FormLogado(Panel panel)
        {
            InitializeComponent();
            this.panel = panel;
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

        private void FormLogado_Load(object sender, EventArgs e)
        {
            if (parentForm != null)
            {
                UpdateColors(parentForm.selectedColor, parentForm.selectedColor2);
                if (parentForm != null && parentForm.SplitContainer1 != null &&
                    parentForm.BtnConfiguracoes != null && parentForm.BtnPerfil != null)
                {
                    UpdateColors(parentForm.selectedColor, parentForm.selectedColor2);
                    parentForm.LblUserName.Text = usuario.NomeUsuario.ToString();
                    parentForm.BtnConfiguracoes.Visible = true;
                    parentForm.BtnPerfil.Visible = true;
                    parentForm.LblUserName.Visible = true;

                    parentForm.SplitContainer1.Panel2.BackColor = SelectedColor2;
                    parentForm.SplitContainer1.Panel1.Controls.Add(parentForm.BtnConfiguracoes);
                    parentForm.SplitContainer1.Panel1.Controls.Add(parentForm.BtnPerfil);
                    UpdateColors(parentForm.selectedColor, parentForm.selectedColor2);
                }
                UpdateColors(parentForm.selectedColor, parentForm.selectedColor2);
            }


        }

        private void txtMsgDigitada_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                btnEnviarMsg.PerformClick();
            }
        }
    }
}