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
            lblSenhaUserLogado.Text = senhaUserLogado;

            byte[] imageData = UsuarioController.GetImagem(loginUserLogado);
            if (imageData != null)
            {
                using (MemoryStream ms = new MemoryStream(imageData))
                {
                    // Set the PictureBox's image from the loaded image data
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
            // Update the background colors of controls within the panel
            foreach (Control control in parentForm.SplitContainer1.Panel2.Controls)
            {
                control.BackColor = selectedColor2;
                // Update other properties like ForeColor, etc., if needed
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
            // Load the user's image data from the database
            byte[] imageData = UsuarioController.GetImagem(loginUserLogado);

            if (imageData != null)
            {
                // Create a MemoryStream from the image data
                using (MemoryStream ms = new MemoryStream(imageData))
                {
                    // Load the image from the MemoryStream
                    Image userImage = Image.FromStream(ms);

                    // Resize the image to fit the PictureBox
                    Image resizedImage = ResizeImage(userImage, imgPerfil.Width, imgPerfil.Height);

                    // Set the resized image as the PictureBox image
                    imgPerfil.Image = resizedImage;

                    // Set the BackgroundImageLayout property to Stretch
                    imgPerfil.BackgroundImageLayout = ImageLayout.Stretch;
                }
            }
        }

        private Image ResizeImage(Image image, int width, int height)
        {
            // Calculate the aspect ratio
            double aspectRatio = (double)image.Width / image.Height;

            // Calculate the new width and height based on the aspect ratio
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

            // Create a new Bitmap with the specified width and height
            Bitmap resizedBitmap = new Bitmap(newWidth, newHeight);

            // Set the resolution of the new bitmap to match the original image
            resizedBitmap.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            // Create a Graphics object from the resized bitmap
            using (Graphics graphics = Graphics.FromImage(resizedBitmap))
            {
                // Configure the Graphics object for high-quality rendering
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                // Draw the original image onto the resized bitmap
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

                // Refresh the MainForm label
                if (parentForm != null)
                {
                    parentForm.UpdateUserNameLabel(novoNome);
                }

                // Refresh the SplitContainer1.Panel1
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
                UsuarioController.AtualizarSenha(loginUserLogado, novaSenha);
                MessageBox.Show("Senha atualizada com sucesso!");

                lblSenhaUserLogado.Text = novaSenha;

                Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void imgPerfil_Click(object sender, EventArgs e)
        {
            // Create and configure the OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog.Title = "Select an Image";

            // Show the dialog and check if the user clicked OK
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Get the selected image file path
                string imagePath = openFileDialog.FileName;

                try
                {
                    // Load the image
                    Image selectedImage = Image.FromFile(imagePath);

                    // Resize the image to fit the PictureBox
                    Image resizedImage = new Bitmap(imgPerfil.Width, imgPerfil.Height);
                    using (Graphics graphics = Graphics.FromImage(resizedImage))
                    {
                        graphics.DrawImage(selectedImage, 0, 0, imgPerfil.Width, imgPerfil.Height);
                    }

                    // Set the resized image as the PictureBox image
                    imgPerfil.Image = resizedImage;

                    // Set the BackgroundImageLayout property to Stretch
                    imgPerfil.BackgroundImageLayout = ImageLayout.Stretch;

                    // Convert the image to a byte array
                    byte[] imageData = ImageToByteArray(resizedImage);

                    // Save the image in the database using the UsuarioController
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
            // Check if an image is currently set in the PictureBox
            if (imgPerfil.Image != null)
            {
                // Get the current image from the PictureBox
                Image currentImage = imgPerfil.Image;

                // Convert the image to a byte array
                byte[] imageData = ImageToByteArray(currentImage);

                try
                {
                    // Save the image data in the database using the UsuarioController
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
            // Set the image in the current form
            imgPerfil.Image = image;

            // Set the image in the parent form
            parentForm.SetProfileImage(image);
        }

        private byte[] ImageToByteArray(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Specify the image format as JPEG
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }
    }
}
