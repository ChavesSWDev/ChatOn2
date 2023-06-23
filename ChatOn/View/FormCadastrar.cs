using ChatOn.Controller;
using ChatOn.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System;
using System.Security.Cryptography;

namespace ChatOn.View
{
    public partial class FormCadastrar : Form
    {
        private EstadoForm Estado = EstadoForm.INSERCAO;
        private Usuarios UsuariosAtualizacao = null;

        public FormCadastrar()
        {
            InitializeComponent();

        }

        private Panel panel;
        private FormMenu formMenu;
        public FormCadastrar(FormMenu formMenu)
        {
            InitializeComponent();
            this.formMenu = formMenu;
        }
        public FormCadastrar(Panel panel)
        {
            InitializeComponent();
            this.panel = panel;
        }

        public void UpdateColors(Color selectedColor, Color selectedColor2)
        {
            this.BackColor = selectedColor2;
            if (formMenu != null)
            {
                UpdatePanelControlsColors(selectedColor, selectedColor2);
                UpdateControlColors(lblLogar, selectedColor);
                UpdateControlColors(btnCadastrar, selectedColor);
            }
        }

        private void UpdateControlColors(Control control, Color selectedColor)
        {
            if (control is Label lbl && lbl.Name == "lblLogar")
            {
                lblLogar.LinkColor = (selectedColor == Color.FromArgb(40, 40, 40) ||
                                 selectedColor == Color.Indigo ||
                                 selectedColor == Color.DarkBlue)
                                ? Color.White
                                : Color.Red;
            }
            else if (control is Button btn && btn.Name == "btnCadastrar")
            {
                btnCadastrar.BackColor = selectedColor;
                btnCadastrar.ForeColor = (selectedColor == Color.FromArgb(40, 40, 40) ||
                                  selectedColor == Color.Indigo ||
                                  selectedColor == Color.DarkBlue)
                                 ? Color.WhiteSmoke
                                 : Color.Black;
            }
        }

        private void UpdatePanelControlsColors(Color selectedColor, Color selectedColor2)
        {
            // Update the background colors of controls within the panel
            foreach (Control control in formMenu.SplitContainer1.Panel2.Controls)
            {
                control.BackColor = selectedColor2;
                // Update other properties like ForeColor, etc., if needed
            }
        }

        private void lblLogar_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            if (formMenu != null)
            {
                formMenu.AddFormEscolhaCadLog();
            }
        }

        private void FormCadastrar_Load(object sender, EventArgs e)
        {
            txtEmail.Focus();
            UpdateColors(formMenu.selectedColor, formMenu.selectedColor2);
            txtEmail.BackColor = Color.WhiteSmoke;
            txtUsername.BackColor = Color.WhiteSmoke;
            txtSenha.BackColor = Color.WhiteSmoke;
        }

        private bool IsEmailValid(string email)
        {
            try
            {
                MailAddress mailAddress = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            Usuarios novoUser = new Usuarios();
            novoUser.Email = txtEmail.Text.Trim();
            novoUser.Login = txtUsername.Text.Trim();
            novoUser.Senha = CriptografarSenha(txtSenha.Text.Trim());
            novoUser.NomeUsuario = txtUsername.Text.Trim();

            // Retrieve the image from the PictureBox in the MainForm
            Image userImage = formMenu.UserImage.Image;

            if (userImage != null)
            {
                // Resize the image to match the size of the PictureBox with stretch effect
                Image resizedImage = ResizeImage(userImage, formMenu.UserImage.Size);

                // Convert the resized image to a byte array
                using (MemoryStream ms = new MemoryStream())
                {
                    resizedImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    novoUser.ImagemPerfil = ms.ToArray();
                }
            }
            else
            {
                //string defaultImagePath = @"Y:\TomanoNoturnoADS\Dev Sistemas - Tiago\ChatOn2-master\ChatOn\Images\defaultUserImg.png";
                string defaultImagePath = @"C:\Users\Renan\Desktop\projetoTiago\ChatOn2-master\ChatOn\Images\defaultUserImg.png";

                // Get the full path to the default image
                string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, defaultImagePath);

                if (File.Exists(fullPath))
                {
                    using (FileStream fs = new FileStream(fullPath, FileMode.Open))
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            fs.CopyTo(ms);
                            novoUser.ImagemPerfil = ms.ToArray();
                        }
                    }
                }
                else
                {
                    // Default image file not found
                    MessageBox.Show("Default user image not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            errorProvider1.Clear();
            bool erros = false;

            // Validate the user input
            if (novoUser.Login == "")
            {
                errorProvider1.SetError(txtUsername, "Username inválido!");
                erros = true;
            }
            if (novoUser.Email == "")
            {
                errorProvider1.SetError(txtEmail, "Email inválido!");
                erros = true;
            }
            else if (!IsEmailValid(novoUser.Email))
            {
                errorProvider1.SetError(txtEmail, "Email inválido!");
                erros = true;
            }
            if (novoUser.Senha == "")
            {
                errorProvider1.SetError(txtSenha, "Senha inválida!");
                erros = true;
            }

            if (!erros)
            {
                try
                {
                    if (Estado == EstadoForm.INSERCAO)
                    {
                        UsuarioController.SalvarUsuario(novoUser);
                        MessageBox.Show("Usuário cadastrado com sucesso!");
                        if (formMenu != null)
                        {
                            formMenu.AddFormEscolhaCadLog();
                        }
                    }
                    else
                    {
                        UsuariosAtualizacao.Login = novoUser.Login;
                        UsuariosAtualizacao.Email = novoUser.Email;
                        UsuariosAtualizacao.Senha = novoUser.Senha;
                        UsuarioController.Atualizar(UsuariosAtualizacao);
                        MessageBox.Show("Usuário atualizado com sucesso");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }
        private string CriptografarSenha(string senha)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(senha));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        private bool VerificarSenha(string senhaDigitada, string senhaArmazenada)
        {
            string senhaCriptografada = CriptografarSenha(senhaDigitada);
            return senhaCriptografada == senhaArmazenada;
        }

        private Image ResizeImage(Image image, Size size)
        {
            Bitmap resizedImage = new Bitmap(size.Width, size.Height);
            using (Graphics graphics = Graphics.FromImage(resizedImage))
            {
                graphics.DrawImage(image, 0, 0, size.Width, size.Height);
            }
            return resizedImage;
        }
    }

    public enum EstadoForm
    {
        INSERCAO,
        ATUALIZACAO
    }
}
