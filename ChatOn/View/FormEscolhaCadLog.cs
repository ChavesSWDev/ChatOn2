using ChatOn.Controller;
using ChatOn.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Security.Cryptography;
namespace ChatOn.View
{
    public partial class FormEscolhaCadLog : Form
    {
        private FormMenu parentForm;
        private FormLogado loggedForm;

        public FormEscolhaCadLog()
        {
            InitializeComponent();
        }

        public FormEscolhaCadLog(FormMenu parentForm)
        {
            InitializeComponent();
            this.parentForm = parentForm;
        }

        public void UpdateColors(Color selectedColor, Color selectedColor2)
        {
            this.BackColor = selectedColor2;
            if (parentForm != null)
            {
                UpdatePanelControlsColors(selectedColor, selectedColor2);
                UpdateControlColors(lblCadastrar, selectedColor);
                UpdateControlColors(btnLogar, selectedColor);
            }
        }

        private void UpdateControlColors(Control control, Color selectedColor)
        {
            if (control is Label lbl && lbl.Name == "lblCadastrar")
            {
                lblCadastrar.LinkColor = (selectedColor == Color.FromArgb(40, 40, 40) ||
                                 selectedColor == Color.Indigo ||
                                 selectedColor == Color.DarkBlue)
                                ? Color.White
                                : Color.Red;
            }
            else if (control is Button btn && btn.Name == "btnLogar")
            {
                btnLogar.BackColor = selectedColor;
                btnLogar.ForeColor = (selectedColor == Color.FromArgb(40, 40, 40) ||
                                  selectedColor == Color.Indigo ||
                                  selectedColor == Color.DarkBlue)
                                 ? Color.WhiteSmoke
                                 : Color.Black;
            }
        }

        private void UpdatePanelControlsColors(Color selectedColor, Color selectedColor2)
        {
            // Atualizar as cores de fundo dos controles no painel
            foreach (Control control in parentForm.SplitContainer1.Panel2.Controls)
            {
                control.BackColor = selectedColor2;
            }
        }

        private void lblCadastrar_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            if (parentForm != null)
            {
                parentForm.AddFormCadastrar();
            }
        }

        public static Usuarios loggedInUser;
        public Usuarios logado
        {
            get { return loggedInUser; }
        }
        private List<Usuarios> friendList;
        private void btnLogar_Click(object sender, EventArgs e)
        {
            string login = txtLogin.Text;
            string senha = txtSenha.Text;

            // Consultar o banco de dados para verificar se as credenciais de login são válidas
            Usuarios usuario = UsuarioController.Context.Usuario.FirstOrDefault(u => (u.Email == login || u.Login == login) && u.Senha == CriptografarSenha(senha));

            if (usuario != null && VerificarSenha(senha, usuario.Senha))
            {
                loggedInUser = usuario;

                if (parentForm != null)
                {
                    byte[] imageData = usuario.ImagemPerfil;
                    if (imageData != null && imageData.Length > 0)
                    {
                        using (MemoryStream ms = new MemoryStream(imageData))
                        {
                            Image image = Image.FromStream(ms);
                            // Defina a imagem no PictureBox do formulário pai
                            parentForm.SetProfileImage(image);

                        }
                    }

                    parentForm.LoginUserLogadoImageData = imageData;
                    parentForm.LblUserName.Text = usuario.NomeUsuario.ToString();
                    parentForm.ShowButtons();
                    parentForm.AddFormLogado();
                }
            }
            else
            {
                // Dados digitados são inválidos
                MessageBox.Show("Login ou senha incorretos, por favor, tente novamente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtLogin.Text = "";
                txtSenha.Text = "";
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
            string senhaCriptografadaDigitada = CriptografarSenha(senhaDigitada);
            return senhaCriptografadaDigitada == senhaArmazenada;
        }


        public void ShowButtons()
        {
            if (parentForm != null)
            {
                parentForm.BtnConfiguracoes.Visible = true;
                parentForm.BtnPerfil.Visible = true;
            }
        }

        private void FormEscolhaCadLog_Load(object sender, EventArgs e)
        {
            txtLogin.Focus();
            if (parentForm != null)
            {
                UpdateColors(parentForm.selectedColor, parentForm.selectedColor2);
            }
        }

        private void FormEscolhaCadLog_Activated(object sender, EventArgs e)
        {
            parentForm.SplitContainer1.Panel2.BackColor = parentForm.selectedColor2;
        }
    }
}
