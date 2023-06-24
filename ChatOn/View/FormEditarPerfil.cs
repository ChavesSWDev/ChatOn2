using ChatOn.Controller;
using System.IO;
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
using System.Security.Cryptography;

namespace ChatOn.View
{
    public partial class FormEditarPerfil : Form
    {
        private FormMenu parentForm;
        public FormEditarPerfil()
        {
            InitializeComponent();
        }

        private string loginUserLogado;
        private string nomeUserLogado;
        private string emailUserLogado;
        private string senhaUserLogado;
        public FormEditarPerfil(FormMenu parentForm, string userLogin, string userLogado, string emailLogado, string senhaLogado) : this()
        {
            this.parentForm = parentForm;
            loginUserLogado = userLogin;
            nomeUserLogado = userLogado;
            emailUserLogado = emailLogado;
            senhaUserLogado = senhaLogado;

            lblNomeUserLogado.Text = nomeUserLogado;
            lblEmailUserLogado.Text = emailUserLogado;
            lblSenhaUserLogado.Text = "**************";

            byte[] imageData = UsuarioController.GetImagem(loginUserLogado);
            if (imageData != null)
            {
                using (MemoryStream ms = new MemoryStream(imageData))
                {
                    // Defina a imagem do PictureBox a partir dos dados de imagem carregados
                    imgPerfil.Image = Image.FromStream(ms);
                    imgPerfil.BackgroundImageLayout = ImageLayout.Stretch;
                }
            }
        }

        public string userLogged
        {
            get { return loginUserLogado; }
        }

        public Label LblUserNameLogado
        {
            get { return lblNomeUserLogado; }
        }

        public Label LblEmailUserLogado
        {
            get { return lblEmailUserLogado; }
        }

        public Label LblSenhaUserLogado
        {
            get { return lblSenhaUserLogado; }
        }

        public FormEditarPerfil(FormMenu parentForm)
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
            }
        }

        private void UpdatePanelControlsColors(Color selectedColor, Color selectedColor2)
        {
            // Atualize as cores de fundo dos controles no painel
            foreach (Control control in parentForm.SplitContainer1.Panel2.Controls)
            {
                control.BackColor = selectedColor2;
                // Atualize outras propriedades como ForeColor, etc., se necessário
            }
        }
        private void FormEditarPerfil_Load(object sender, EventArgs e)
        {
            if (parentForm != null)
            {
                UpdateColors(parentForm.selectedColor, parentForm.selectedColor2);
            }
            LoadAndResizeUserImage();
        }

        private void LoadAndResizeUserImage()
        {
            // Carregue os dados de imagem do usuário do banco de dados
            byte[] imageData = UsuarioController.GetImagem(loginUserLogado);

            if (imageData != null)
            {
                // Crie um MemoryStream a partir dos dados da imagem
                using (MemoryStream ms = new MemoryStream(imageData))
                {
                    // Carregar a imagem do MemoryStream
                    Image userImage = Image.FromStream(ms);

                    // redimensionar a imagem para caber no PictureBox
                    Image resizedImage = ResizeImage(userImage, imgPerfil.Width, imgPerfil.Height);

                    // Defina a imagem redimensionada como a imagem PictureBox
                    imgPerfil.Image = resizedImage;

                    // Defina a propriedade BackgroundImageLayout como Stretch
                    imgPerfil.BackgroundImageLayout = ImageLayout.Stretch;
                }
            }
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

            //Crie um objeto Graphics a partir do bitmap redimensionado
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (parentForm != null)
            {
                parentForm.AddFormLogado();
            }
        }

        private void btnSalvarNovoNome_Click(object sender, EventArgs e)
        {
            string novoNome = txtNovoNomeUser.Text;

            try
            {
                UsuarioController.AtualizarNome(loginUserLogado, novoNome);
                MessageBox.Show("Nome atualizado com sucesso!");

                lblNomeUserLogado.Text = novoNome;

                // Atualizar o rótulo MainForm
                if (parentForm != null)
                {
                    parentForm.UpdateUserNameLabel(novoNome);
                }

                // Atualize o SplitContainer1.Panel1
                if (parentForm != null && parentForm.SplitContainer1.Panel1.Controls.Count > 0)
                {
                    Control[] controls = parentForm.SplitContainer1.Panel1.Controls.Find("lblUserName", true);
                    if (controls.Length > 0)
                    {
                        Label lblUserName = controls[0] as Label;
                        lblUserName.Text = novoNome;
                    }
                }

                Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSalvarNovaSenha_Click(object sender, EventArgs e)
        {
            string novaSenha = txtNovaSenhaUser.Text;

            try
            {
                string senhaCriptografada = CriptografarSenha(novaSenha); // Criptografar a nova senha

                UsuarioController.AtualizarSenha(loginUserLogado, senhaCriptografada); // Salve a senha criptografada no banco de dados

                MessageBox.Show("Senha atualizada com sucesso!");

                lblSenhaUserLogado.Text = "**************";

                Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

        private void imgPerfil_Click(object sender, EventArgs e)
        {
            // Crie e configure o OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog.Title = "Select an Image";

            //Mostre a caixa de diálogo e verifique se o usuário clicou em OK
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Obtenha o caminho do arquivo de imagem selecionado
                string imagePath = openFileDialog.FileName;

                try
                {
                    // Carregar a imagem
                    Image selectedImage = Image.FromFile(imagePath);

                    // Redimensione a imagem para caber no PictureBox
                    Image resizedImage = new Bitmap(imgPerfil.Width, imgPerfil.Height);
                    using (Graphics graphics = Graphics.FromImage(resizedImage))
                    {
                        graphics.DrawImage(selectedImage, 0, 0, imgPerfil.Width, imgPerfil.Height);
                    }

                    // Defina a imagem redimensionada como a imagem PictureBox
                    imgPerfil.Image = resizedImage;

                    // Defina a propriedade BackgroundImageLayout como Stretch
                    imgPerfil.BackgroundImageLayout = ImageLayout.Stretch;

                    // Convert the image to a byte array
                    byte[] imageData = ImageToByteArray(resizedImage);

                    // Salve a imagem no banco de dados usando o UsuarioController
                    UsuarioController.SalvarImagem(loginUserLogado, imageData);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading the selected image: " + ex.Message);
                }
            }
        }

        private void btnSalvarImagem_Click(object sender, EventArgs e)
        {
            // Verifique se uma imagem está definida no PictureBox
            if (imgPerfil.Image != null)
            {
                // Obtenha a imagem atual do PictureBox
                Image currentImage = imgPerfil.Image;

                // Converter a imagem em uma matriz de bytes
                byte[] imageData = ImageToByteArray(currentImage);

                try
                {
                    // Salve os dados da imagem no banco de dados usando o UsuarioController
                    UsuarioController.SalvarImagem(loginUserLogado, imageData);

                    MessageBox.Show("Imagem salva com sucesso!");
                    UpdateProfileImage(currentImage);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving the image: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Nenhuma imagem selecionada.");
            }
        }

        private void UpdateProfileImage(Image image)
        {
            // Definir a imagem no formulário atual
            imgPerfil.Image = image;

            // Definir a imagem no formulário pai
            parentForm.SetProfileImage(image);
        }

        private byte[] ImageToByteArray(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Especifique o formato da imagem como JPEG
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }
    }
}
