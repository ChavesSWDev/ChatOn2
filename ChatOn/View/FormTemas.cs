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
    public partial class FormTemas : Form
    {

        public Color SelectedColor { get; private set; }
        public Color SelectedColor2 { get; private set; }
        public Button SelectedButton { get; private set; }

        private Color defaultBackColor;

        public FormTemas()
        {
            InitializeComponent();
            defaultBackColor = BackColor;
        }

        private void UpdateFormBackground(Color color)
        {
            BackColor = color;
        }

        public void btnTemaPadrao_Click(object sender, EventArgs e)
        {
            SelectedButton = btnTemaPadrao;
            SelectedColor = Color.DimGray;
            SelectedColor2 = Color.FromArgb(206, 206, 206);
            DialogResult = DialogResult.OK;
        }

        private void btnTemaPadrao_MouseEnter(object sender, EventArgs e)
        {
            UpdateFormBackground(btnTemaPadrao.BackColor);
        }

        private void btnTemaPadrao_MouseLeave(object sender, EventArgs e)
        {
            UpdateFormBackground(defaultBackColor);
        }

        private void btnTemaVerde_Click(object sender, EventArgs e)
        {
            SelectedButton = btnTemaVerde;
            SelectedColor = Color.ForestGreen;
            SelectedColor2 = Color.LightGreen;
            DialogResult = DialogResult.OK;
        }

        private void btnTemaVerde_MouseEnter_1(object sender, EventArgs e)
        {
            UpdateFormBackground(btnTemaVerde.BackColor);
        }

        private void btnTemaVerde_MouseLeave_1(object sender, EventArgs e)
        {
            UpdateFormBackground(defaultBackColor);
        }


        private void btnTemaAmarelo_Click(object sender, EventArgs e)
        {
            SelectedButton = btnTemaAmarelo;
            SelectedColor = Color.DarkGoldenrod;
            SelectedColor2 = Color.Khaki;
            DialogResult = DialogResult.OK;
        }

        private void btnTemaAmarelo_MouseEnter(object sender, EventArgs e)
        {
            UpdateFormBackground(btnTemaAmarelo.BackColor);
        }

        private void btnTemaAmarelo_MouseLeave(object sender, EventArgs e)
        {
            UpdateFormBackground(defaultBackColor);
        }

        private void btnTemaAzul_Click(object sender, EventArgs e)
        {
            SelectedButton = btnTemaAzul;
            SelectedColor = Color.DarkBlue;
            SelectedColor2 = Color.RoyalBlue;
            DialogResult = DialogResult.OK;
        }

        private void btnTemaAzul_MouseEnter(object sender, EventArgs e)
        {
            UpdateFormBackground(btnTemaAzul.BackColor);
        }

        private void btnTemaAzul_MouseLeave(object sender, EventArgs e)
        {
            UpdateFormBackground(defaultBackColor);
        }

        private void btnTemaRoxo_Click(object sender, EventArgs e)
        {
            SelectedButton = btnTemaRoxo;
            SelectedColor = Color.Indigo;
            SelectedColor2 = Color.BlueViolet;
            DialogResult = DialogResult.OK;
        }

        private void btnTemaRoxo_MouseEnter(object sender, EventArgs e)
        {
            UpdateFormBackground(btnTemaRoxo.BackColor);
        }

        private void btnTemaRoxo_MouseLeave(object sender, EventArgs e)
        {
            UpdateFormBackground(defaultBackColor);
        }

        public void btnTemaEscuro_Click(object sender, EventArgs e)
        {
            SelectedButton = btnTemaEscuro;
            SelectedColor = Color.FromArgb(40, 40, 40);
            SelectedColor2 = Color.FromArgb(90, 90, 90);
            DialogResult = DialogResult.OK;
        }

        private void btnTemaEscuro_MouseEnter(object sender, EventArgs e)
        {
            UpdateFormBackground(btnTemaEscuro.BackColor);
        }

        private void btnTemaEscuro_MouseLeave(object sender, EventArgs e)
        {
            UpdateFormBackground(defaultBackColor);
        }

    }
}
