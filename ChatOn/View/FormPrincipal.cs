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

            // Call the method to update the colors in the child forms
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
                    return null; // or return a default image if desired
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
            // Load the selected color from the stored settings
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

            // Hide or show the appropriate controls based on the selected color
            // You can add more customization based on the selected color if needed
            if (TemaSettings.CorPrimaria == Color.Yellow)
            {
                // Hide the logo and show the user profile controls
                picBoxLogo.Visible = false;
                ShowButtons();
            }
            else
            {
                // Show the logo and hide the user profile controls
                picBoxLogo.Visible = true;
                btnAddAmigos.Visible = false;
                btnPerfil.Visible = false;
                btnDeslogar.Visible = false;
                picBoxUserIMG.Visible = false;
                lblUserName.Visible = false;
            }
        }



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

        private void btnPerfil_Click(object sender, EventArgs e)
        {
            FormEditarPerfil formEditarPerfil = new FormEditarPerfil(this, FormEscolhaCadLog.loggedInUser.Login, FormEscolhaCadLog.loggedInUser.NomeUsuario, FormEscolhaCadLog.loggedInUser.Email, FormEscolhaCadLog.loggedInUser.Senha);//lblUserNameLogado
            openChildForm(formEditarPerfil);
        }

        private void btnAddAmigos_Click(object sender, EventArgs e)
        {
            FormAddAmigos formAddAmigos = new FormAddAmigos(this, FormEscolhaCadLog.loggedInUser.Login, FormEscolhaCadLog.loggedInUser.NomeUsuario, FormEscolhaCadLog.loggedInUser.Email, FormEscolhaCadLog.loggedInUser.Senha);
            openChildForm(formAddAmigos);
        }

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

                // Subscribe to the color change event of the child forms
                if (childForm is FormLogado)
                {
                    FormLogado formLogado = (FormLogado)childForm;
                    formLogado.parentForm = this;
                    formLogado.CorSecundaria = TemaSettings.CorSecundaria;

                    // Hide the buttons in the parent form
                    btnAddAmigos.Visible = true;
                    btnPerfil.Visible = true;
                }
                else
                {
                    // Show the buttons in the parent form
                    btnAddAmigos.Visible = true;
                    btnPerfil.Visible = true;
                }

                splitContainer1.Panel2.Controls.Add(childForm);
                childForm.BringToFront();
                childForm.Show();

                activeForm = childForm;
            }
        }

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
            // Show a confirmation message
            var result = MessageBox.Show("Você tem certeza que quer deslogar?", "Logout Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                MessageBox.Show("Deslogando...");
                // Close the currently active form (FormLogado)
                if (activeForm != null)
                {
                    activeForm.Close();
                    activeForm = null;
                }

                // Show the buttons and clear the panel in splitContainer1.Panel2
                ShowButtons();
                splitContainer1.Panel2.Controls.Clear();

                // Re-add the original controls (panel2 and btnIniciar) to splitContainer1.Panel2
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

                // Update the primary and secondary colors
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

                // Update the background colors in the main form
                splitContainer1.Panel1.BackColor = TemaSettings.CorPrimaria;
                splitContainer1.Panel2.BackColor = TemaSettings.CorSecundaria;

                // Notify the child form (FormEscolhaCadLog) about the color change
                UpdateChildFormColors();
                this.Refresh();
            }
        }
    }
}
