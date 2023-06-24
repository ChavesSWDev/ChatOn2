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

namespace ChatOn.View
{
    public partial class FormMenu : Form
    {
        private bool showButtons = true;

        public Color selectedColor = Color.DimGray;
        public Color selectedColor2 = Color.FromArgb(206, 206, 206);

        public Color SelectedColor
        {
            get { return selectedColor; }
            set { selectedColor = value; }
        }

        public Color SelectedColor2
        {
            get { return selectedColor2; }
            set { selectedColor2 = value; }
        }

        public event EventHandler<ColorChangedEventArgs> ColorsChanged;

        private void OnColorsChanged(ColorChangedEventArgs e)
        {
            ColorsChanged?.Invoke(this, e);

            // chama a função pra mudar a cor de tema
            UpdateChildFormColors();
        }

        public class ColorChangedEventArgs : EventArgs
        {
            public Color SelectedColor { get; set; }
            public Color SelectedColor2 { get; set; }
        }
        public byte[] LoginUserLogadoImageData { get; set; }
        public Image LoginUserLogadoImage
        {
            get
            {
                if (LoginUserLogadoImageData != null && LoginUserLogadoImageData.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream(LoginUserLogadoImageData))
                    {
                        return Image.FromStream(ms);
                    }
                }
                else
                {
                    return null; 
                }
            }
            set
            {
                if (value != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        value.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        LoginUserLogadoImageData = ms.ToArray();
                    }
                }
                else
                {
                    LoginUserLogadoImageData = null;
                }
            }
        }

        public void SetProfileImage(Image image)
        {
            picBoxUserIMG.Image = image;
            picBoxUserIMG.SizeMode = PictureBoxSizeMode.StretchImage;
            picBoxUserIMG.Visible = true;
        }

        public FormMenu()
        {
            InitializeComponent();
            // Carrega as cores selecionadas para serem inseridas no menu esquerdo e direito do programa
            splitContainer1.Panel1.BackColor = SelectedColor;
            splitContainer1.Panel2.BackColor = SelectedColor2;

            TemaSettings.CorPrimaria = SelectedColor;
            TemaSettings.CorSecundaria = SelectedColor2;

            btnTemas.FlatAppearance.BorderSize = 0;
            btnPerfil.FlatAppearance.BorderSize = 0;
            btnDeslogar.FlatAppearance.BorderSize = 0;
            btnAddAmigos.FlatAppearance.BorderSize = 0;
            btnTemas.FlatAppearance.BorderSize = 0;
            btnIniciar.BackColor = TemaSettings.CorPrimaria;

            // Oculte ou mostre os controles apropriados com base na cor selecionada
            if (TemaSettings.CorPrimaria == Color.Yellow)
            {
                // Oculte o logotipo e mostre os controles do perfil do usuário
                picBoxLogo.Visible = false;
                ShowButtons();
            }
            else
            {
                // Mostrar o logotipo e ocultar os controles do perfil do usuário
                picBoxLogo.Visible = true;
                btnAddAmigos.Visible = false;
                btnPerfil.Visible = false;
                btnDeslogar.Visible = false;
                picBoxUserIMG.Visible = false;
                lblUserName.Visible = false;
            }
        }


        //isso foi feito para poder acessar dos outros formulários
        //os controles do formulário principal, praticamente todo formulário
        //deve fazer isso caso algum formulário usa dados do anterior
        public SplitContainer SplitContainer1
        {
            get { return splitContainer1; }
        }

        public Label LblUserName
        {
            get { return lblUserName; }
        }

        public PictureBox UserImage
        {
            get { return picBoxUserIMG; }
        }

        public PictureBox PicLogo
        {
            get { return picBoxLogo; }
        }

        public Button BtnConfiguracoes
        {
            get { return btnAddAmigos; }
        }

        public Button BtnPerfil
        {
            get { return btnPerfil; }
        }

        public Button BtnIniciar
        {
            get { return btnIniciar; }
        }

        public Button BtnAddAmigos
        {
            get { return btnAddAmigos; }
        }

        public Button BtnDeslogar
        {
            get { return btnDeslogar; }
        }

        public Panel Panel2
        {
            get { return panel2; }
        }

        public Form activeForm = null;

        //chama form para logar
        private void btnIniciar_Click(object sender, EventArgs e)
        {
            FormEscolhaCadLog formEscolhaCadLog = new FormEscolhaCadLog(this);
            openChildForm(formEscolhaCadLog);
            btnAddAmigos.Visible = false;
            btnPerfil.Visible = false;
            btnDeslogar.Visible = false;
            picBoxUserIMG.Visible = false;
            lblUserName.Visible = false;
        }
        //chama o form editar perfil
        private void btnPerfil_Click(object sender, EventArgs e)
        {
            FormEditarPerfil formEditarPerfil = new FormEditarPerfil(this, FormEscolhaCadLog.loggedInUser.Login, FormEscolhaCadLog.loggedInUser.NomeUsuario, FormEscolhaCadLog.loggedInUser.Email, FormEscolhaCadLog.loggedInUser.Senha);//lblUserNameLogado
            openChildForm(formEditarPerfil);
        }
        //chama form para add amigo
        private void btnAddAmigos_Click(object sender, EventArgs e)
        {
            FormAddAmigos formAddAmigos = new FormAddAmigos(this, FormEscolhaCadLog.loggedInUser.Login, FormEscolhaCadLog.loggedInUser.NomeUsuario, FormEscolhaCadLog.loggedInUser.Email, FormEscolhaCadLog.loggedInUser.Senha);
            openChildForm(formAddAmigos);
        }
        //volta ao menu caso n tiver logado qnd clicar na logo do app
        private void picBoxLogo_Click(object sender, EventArgs e)
        {
            if (activeForm != null)
            {
                activeForm.Close();
                activeForm = null;
            }


            // Reexibe os controles originais em splitContainer1.Panel2
            splitContainer1.Panel2.Controls.Add(panel2);
            splitContainer1.Panel2.Controls.Add(btnIniciar);
        }

        public void ShowButtons()
        {
            btnAddAmigos.Visible = true;
            btnPerfil.Visible = true;
            btnDeslogar.Visible = true;
            picBoxUserIMG.Visible = true;
            picBoxLogo.Visible = false;
            lblUserName.Visible = true;
        }

        private Image ResizeImage(Image image, int width, int height)
        {
            // Calcular a proporção
            double aspectRatio = (double)image.Width / image.Height;

            // Calcule a nova largura e altura com base na proporção
            int newWidth = width;
            int newHeight = height;

            if (width / (double)height > aspectRatio)
            {
                newWidth = (int)(height * aspectRatio);
            }
            else
            {
                newHeight = (int)(width / aspectRatio);
            }

            // Crie um novo Bitmap com a largura e altura especificadas
            Bitmap resizedBitmap = new Bitmap(newWidth, newHeight);

            // Defina a resolução do novo bitmap para corresponder à imagem original
            resizedBitmap.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            // Crie um objeto Graphics a partir do bitmap redimensionado
            using (Graphics graphics = Graphics.FromImage(resizedBitmap))
            {
                // Configure o objeto Graphics para renderização de alta qualidade
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                // Desenhe a imagem original no bitmap redimensionado
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);
            }

            return resizedBitmap;
        }


        //essa função é para chamar todos os formulários no split container1 panel 2
        public void openChildForm(Form childForm)
        {
            if (activeForm != null)
            {
                activeForm.Close();
                activeForm = null;
            }

            if (childForm != null)
            {
                childForm.TopLevel = false;
                childForm.FormBorderStyle = FormBorderStyle.None;
                childForm.Dock = DockStyle.Fill;

                splitContainer1.Panel2.Controls.Clear();

                // Inscreva-se no evento de mudança de cor dos formulários filhos
                if (childForm is FormLogado)
                {
                    FormLogado formLogado = (FormLogado)childForm;
                    formLogado.parentForm = this;
                    formLogado.CorSecundaria = TemaSettings.CorSecundaria;

                    // Ocultar os botões no formulário pai
                    btnAddAmigos.Visible = true;
                    btnPerfil.Visible = true;
                }
                else
                {
                    // Mostrar os botões no formulário pai
                    btnAddAmigos.Visible = true;
                    btnPerfil.Visible = true;
                }

                splitContainer1.Panel2.Controls.Add(childForm);
                childForm.BringToFront();
                childForm.Show();

                activeForm = childForm;
            }
        }

        //essa função é para manter as cores de todos os formulários
        //na cor selecionada do tema
        public void UpdateChildFormColors()
        {
            if (activeForm is FormEscolhaCadLog escolhaCadLogForm)
            {
                escolhaCadLogForm.Refresh();
                escolhaCadLogForm.UpdateColors(SelectedColor, SelectedColor2);
                escolhaCadLogForm.Refresh();
            }
            if (activeForm is FormCadastrar formCadastrar)
            {
                formCadastrar.Refresh();
                formCadastrar.UpdateColors(SelectedColor, SelectedColor2);
                formCadastrar.Refresh();
            }
            if (activeForm is FormEditarPerfil formEditarPerfil)
            {
                formEditarPerfil.Refresh();
                formEditarPerfil.UpdateColors(SelectedColor, SelectedColor2);
                formEditarPerfil.Refresh();
            }
            if (activeForm is FormLogado formLogado)
            {
                formLogado.Refresh();
                formLogado.UpdateColors(SelectedColor, SelectedColor2);
                formLogado.Refresh();
            }
            if (activeForm is FormAddAmigos formAddAmigos)
            {
                formAddAmigos.Refresh();
                formAddAmigos.UpdateColors(SelectedColor, SelectedColor2);
                formAddAmigos.Refresh();
            }
        }

        //atualiza o nome de quem ta logado para aparecer debaixo da foto
        //de perfil no menu da esquerda do programa
        public void UpdateUserNameLabel(string newUserName)
        {
            lblUserName.Text = newUserName;
        }

        private void picBoxLogo_Click_1(object sender, EventArgs e)
        {
            if (activeForm != null)
            {
                activeForm.Close();
                activeForm = null;
            }

            // Limpa os controles do formulário filho, se houver algum
            splitContainer1.Panel2.Controls.Clear();

            // Reexibe os controles originais em splitContainer1.Panel2
            splitContainer1.Panel2.Controls.Add(panel2);
            splitContainer1.Panel2.Controls.Add(btnIniciar);
            btnAddAmigos.Visible = false;
            btnPerfil.Visible = false;
            btnDeslogar.Visible = false;
            picBoxUserIMG.Visible = false;
            lblUserName.Visible = false;
            splitContainer1.Panel2.Controls.SetChildIndex(btnIniciar, 0);
        }

        //adiciona o form cadastrar no panel 2
        public void AddFormCadastrar()
        {
            FormCadastrar formCadastrar = new FormCadastrar(this);
            formCadastrar.TopLevel = false;
            formCadastrar.FormBorderStyle = FormBorderStyle.None;
            formCadastrar.Dock = DockStyle.Fill;

            SplitContainer1.Panel2.Controls.Clear();
            SplitContainer1.Panel2.Controls.Add(formCadastrar);
            formCadastrar.UpdateColors(selectedColor, selectedColor2);
            formCadastrar.BringToFront();
            formCadastrar.Show();

        }

        //adiciona o form login no panel 2
        public void AddFormEscolhaCadLog()
        {
            FormEscolhaCadLog formEscolhaCadLog = new FormEscolhaCadLog(this);
            formEscolhaCadLog.TopLevel = false;
            formEscolhaCadLog.FormBorderStyle = FormBorderStyle.None;
            formEscolhaCadLog.Dock = DockStyle.Fill;

            SplitContainer1.Panel2.Controls.Clear();
            SplitContainer1.Panel2.Controls.Add(formEscolhaCadLog);
            formEscolhaCadLog.UpdateColors(selectedColor, selectedColor2);
            formEscolhaCadLog.BringToFront();
            formEscolhaCadLog.Show();
            formEscolhaCadLog.Refresh();
        }

        //adiciona o form logado no panel 2
        public void AddFormLogado()
        {
            FormLogado formLogado = new FormLogado(this, FormEscolhaCadLog.loggedInUser);
            formLogado.TopLevel = false;
            formLogado.FormBorderStyle = FormBorderStyle.None;
            formLogado.Dock = DockStyle.Fill;

            SplitContainer1.Panel2.Controls.Clear();
            SplitContainer1.Panel2.Controls.Add(formLogado);
            formLogado.UpdateColors(selectedColor, selectedColor2);
            formLogado.BringToFront();
            formLogado.Show();
            formLogado.Refresh();
            formLogado.Show();
        }

        private void btnDeslogar_Click(object sender, EventArgs e)
        {
            // Mostra uma mensagem de confirmação
            var result = MessageBox.Show("Você tem certeza que quer deslogar?", "Logout Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                MessageBox.Show("Deslogando...");
                // Fecha o form ativo (FormLogado)
                if (activeForm != null)
                {
                    activeForm.Close();
                    activeForm = null;
                }

                // Mostrar os botões e limpar o painel em splitContainer1.Panel2
                ShowButtons();
                splitContainer1.Panel2.Controls.Clear();

                // Adicione novamente os controles originais (panel2 e btnIniciar) a splitContainer1.Panel2
                splitContainer1.Panel2.Controls.Add(panel2);
                splitContainer1.Panel2.Controls.Add(btnIniciar);
                splitContainer1.Panel1.Controls.Add(picBoxLogo);
                picBoxLogo.Visible = true;
                btnAddAmigos.Visible = false;
                btnPerfil.Visible = false;
                btnDeslogar.Visible = false;
                picBoxUserIMG.Visible = false;
                lblUserName.Visible = false;
                splitContainer1.Panel2.Controls.SetChildIndex(btnIniciar, 0);
            }
        }

        private void btnTemas_Click(object sender, EventArgs e)
        {
            FormTemas colorSelector = new FormTemas();

            if (colorSelector.ShowDialog() == DialogResult.OK)
            {
                SelectedColor = colorSelector.SelectedColor;
                SelectedColor2 = colorSelector.SelectedColor2;

                // Atualize as cores primárias e secundárias
                TemaSettings.CorPrimaria = SelectedColor;
                TemaSettings.CorSecundaria = SelectedColor2;

                if (
                    colorSelector.SelectedButton == colorSelector.btnTemaEscuro ||
                    colorSelector.SelectedButton == colorSelector.btnTemaAzul ||
                    colorSelector.SelectedButton == colorSelector.btnTemaRoxo)
                {
                    btnIniciar.ForeColor = Color.WhiteSmoke;
                }
                else
                {
                    btnIniciar.ForeColor = Color.Black;
                }
                btnIniciar.BackColor = TemaSettings.CorPrimaria;

                // Atualize as cores de fundo no formulário principal
                splitContainer1.Panel1.BackColor = TemaSettings.CorPrimaria;
                splitContainer1.Panel2.BackColor = TemaSettings.CorSecundaria;

                //Notificar o formulário filho (FormEscolhaCadLog) sobre a mudança de cor
                UpdateChildFormColors();
                this.Refresh();
            }
        }
    }
}
