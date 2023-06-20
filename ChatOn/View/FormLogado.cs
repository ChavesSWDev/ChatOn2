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
            btnEnviarMsg.Enabled = false;
            txtMsgDigitada.Enabled = false;
            txtChat.Visible = false;
            chatRefreshTimer = new Timer();
            chatRefreshTimer.Interval = 1000;
            chatRefreshTimer.Tick += (sender, e) =>
            {
                RefreshChat();
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
            // Check if a table layout already exists for the friend
            TableLayoutPanel existingTableLayout = GetTableLayoutForFriend(friend);
            if (existingTableLayout != null)
            {
                return; // Return early if table layout already exists
            }

            // Create a new TableLayoutPanel for the friend's data
            TableLayoutPanel tableLayout = new TableLayoutPanel();
            tableLayout.AutoSize = true;
            tableLayout.ColumnCount = 2;
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 57));
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            // Add the friend as a tag to the table layout
            tableLayout.Tag = friend;
            tableLayout.BackColor = SystemColors.Control;

            // Add the tableLayout to the panelAmigos
            panelAmigos.Controls.Add(tableLayout);

            // Set the image and username
            byte[] imageData = friend.ImagemPerfil;
            if (imageData != null && imageData.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream(imageData))
                {
                    Image profileImage = Image.FromStream(ms);
                    PictureBox picBoxImgAmigo = new PictureBox();
                    picBoxImgAmigo.SizeMode = PictureBoxSizeMode.StretchImage;
                    picBoxImgAmigo.Image = profileImage;
                    picBoxImgAmigo.Size = new Size(57, 57);
                    tableLayout.Controls.Add(picBoxImgAmigo, 0, 0);
                }
            }

            Label lblNomeAmigo = new Label();
            lblNomeAmigo.Text = friend.NomeUsuario;
            lblNomeAmigo.Size = new Size(85, 23);
            lblNomeAmigo.TextAlign = ContentAlignment.MiddleLeft;
            tableLayout.Controls.Add(lblNomeAmigo, 1, 0);
            tableLayout.Cursor = Cursors.Hand;

            tableLayout.Click += (sender, e) =>
            {
                // Store the selected user
                selectedUser = friend;

                // Open the chat between the logged-in user and the selected user
                currentChat = UsuarioController.GetChat(usuario, selectedUser);

                // Update the txtChat with the new chat
                StringBuilder chatLog = new StringBuilder();
                foreach (Message message in currentChat.Messages)
                {
                    chatLog.AppendLine($"{message.Sender.NomeUsuario}: {message.Content}");
                }
                txtChat.Text = chatLog.ToString();

                // Update the lblChatComQuem text to the selected user's username
                lblChatComQuem.Text = selectedUser.NomeUsuario;
                btnEnviarMsg.Enabled = true;
                txtMsgDigitada.Enabled = true;
                txtChat.Visible = true;
            };
        }

        private void RefreshChat()
        {
            if (selectedUser == null || currentChat == null)
            {
                return;
            }

            // Retrieve the latest chat data from the database
            Chat updatedChat = UsuarioController.GetChat(usuario, selectedUser);

            // Store the previous message count
            int previousMessageCount = currentChat.Messages.Count;

            // Update the current chat with the latest data
            currentChat.Messages = updatedChat.Messages;

            // Store the current scroll position
            int scrollPosition = txtChat.GetPositionFromCharIndex(txtChat.SelectionStart).Y;

            // Update the txtChat with the refreshed chat data
            StringBuilder chatLog = new StringBuilder();
            foreach (Message message in currentChat.Messages)
            {
                chatLog.AppendLine($"{message.Sender.NomeUsuario}: {message.Content} [{message.Timestamp.ToString("HH:mm:ss")}]");
            }
            string newText = chatLog.ToString();

            // Determine if new messages have been added
            bool newMessagesAdded = currentChat.Messages.Count > previousMessageCount;

            // Update the text of the txtChat
            txtChat.Text = newText;

            // Scroll to the bottom if new messages have been added
            if (newMessagesAdded)
            {
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

            // Retrieve the logged-in user
            Usuarios loggedUser = usuario;

            // Get or create the chat between the logged-in user and the selected user
            Chat chat = UsuarioController.GetChat(loggedUser, selectedUser);

            // Create a new message
            Message newMessage = new Message()
            {
                Sender = loggedUser,
                Content = message,
                Timestamp = DateTime.Now
            };

            // Add the message to the chat
            chat.Messages.Add(newMessage);

            // Save the chat in the database
            UsuarioController.SaveChat(chat);

            // Update the txtChat with the new message
            txtChat.AppendText(loggedUser.NomeUsuario + ": " + message + " [" + newMessage.Timestamp.ToString("HH:mm:ss") + "] " + Environment.NewLine);

            // Scroll to the bottom of the chat
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
            // Update the background colors of controls within the panel
            foreach (Control control in parentForm.SplitContainer1.Panel2.Controls)
            {
                control.BackColor = selectedColor2;
                // Update other properties like ForeColor, etc., if needed
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
    }
}