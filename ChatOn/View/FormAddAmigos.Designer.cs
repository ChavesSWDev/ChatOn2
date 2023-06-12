namespace ChatOn.View
{
    partial class FormAddAmigos
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAddAmigos));
            this.listBoxUsersDataBase = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtEnviarPedidoAmigo = new System.Windows.Forms.TextBox();
            this.btnEnviarPedidoAmizade = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.btnNegarAmizade = new System.Windows.Forms.Button();
            this.listBoxPedidosAmizade = new System.Windows.Forms.ListBox();
            this.btnAceitarAmizade = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtAceitarNegarAmizade = new System.Windows.Forms.TextBox();
            this.btnFechar = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBoxUsersDataBase
            // 
            this.listBoxUsersDataBase.ColumnWidth = 1;
            this.listBoxUsersDataBase.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            this.listBoxUsersDataBase.FormattingEnabled = true;
            this.listBoxUsersDataBase.ItemHeight = 15;
            this.listBoxUsersDataBase.Location = new System.Drawing.Point(20, 32);
            this.listBoxUsersDataBase.Name = "listBoxUsersDataBase";
            this.listBoxUsersDataBase.Size = new System.Drawing.Size(230, 319);
            this.listBoxUsersDataBase.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.listBoxUsersDataBase);
            this.panel1.Location = new System.Drawing.Point(27, 22);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(270, 467);
            this.panel1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(42, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(189, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Pessoas que talvez você conheça";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(78, 397);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(164, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Digite o email para adicionar";
            // 
            // txtEnviarPedidoAmigo
            // 
            this.txtEnviarPedidoAmigo.Location = new System.Drawing.Point(47, 415);
            this.txtEnviarPedidoAmigo.Name = "txtEnviarPedidoAmigo";
            this.txtEnviarPedidoAmigo.Size = new System.Drawing.Size(230, 23);
            this.txtEnviarPedidoAmigo.TabIndex = 3;
            // 
            // btnEnviarPedidoAmizade
            // 
            this.btnEnviarPedidoAmizade.BackColor = System.Drawing.Color.LimeGreen;
            this.btnEnviarPedidoAmizade.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnviarPedidoAmizade.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnEnviarPedidoAmizade.Location = new System.Drawing.Point(75, 456);
            this.btnEnviarPedidoAmizade.Name = "btnEnviarPedidoAmizade";
            this.btnEnviarPedidoAmizade.Size = new System.Drawing.Size(167, 23);
            this.btnEnviarPedidoAmizade.TabIndex = 4;
            this.btnEnviarPedidoAmizade.Text = "Enviar pedido de amizade";
            this.btnEnviarPedidoAmizade.UseVisualStyleBackColor = false;
            this.btnEnviarPedidoAmizade.Click += new System.EventHandler(this.btnEnviarPedidoAmizade_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.btnNegarAmizade);
            this.panel2.Controls.Add(this.listBoxPedidosAmizade);
            this.panel2.Controls.Add(this.btnAceitarAmizade);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.txtAceitarNegarAmizade);
            this.panel2.Location = new System.Drawing.Point(335, 22);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(270, 467);
            this.panel2.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(76, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(116, 15);
            this.label3.TabIndex = 1;
            this.label3.Text = "Pedidos de amizade";
            // 
            // btnNegarAmizade
            // 
            this.btnNegarAmizade.BackColor = System.Drawing.Color.Red;
            this.btnNegarAmizade.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNegarAmizade.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnNegarAmizade.Location = new System.Drawing.Point(153, 434);
            this.btnNegarAmizade.Name = "btnNegarAmizade";
            this.btnNegarAmizade.Size = new System.Drawing.Size(97, 23);
            this.btnNegarAmizade.TabIndex = 8;
            this.btnNegarAmizade.Text = "Cancelar";
            this.btnNegarAmizade.UseVisualStyleBackColor = false;
            // 
            // listBoxPedidosAmizade
            // 
            this.listBoxPedidosAmizade.ColumnWidth = 1;
            this.listBoxPedidosAmizade.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            this.listBoxPedidosAmizade.FormattingEnabled = true;
            this.listBoxPedidosAmizade.ItemHeight = 15;
            this.listBoxPedidosAmizade.Location = new System.Drawing.Point(20, 32);
            this.listBoxPedidosAmizade.Name = "listBoxPedidosAmizade";
            this.listBoxPedidosAmizade.Size = new System.Drawing.Size(230, 319);
            this.listBoxPedidosAmizade.TabIndex = 0;
            // 
            // btnAceitarAmizade
            // 
            this.btnAceitarAmizade.BackColor = System.Drawing.Color.LimeGreen;
            this.btnAceitarAmizade.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAceitarAmizade.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnAceitarAmizade.Location = new System.Drawing.Point(20, 434);
            this.btnAceitarAmizade.Name = "btnAceitarAmizade";
            this.btnAceitarAmizade.Size = new System.Drawing.Size(97, 23);
            this.btnAceitarAmizade.TabIndex = 7;
            this.btnAceitarAmizade.Text = "Aceitar";
            this.btnAceitarAmizade.UseVisualStyleBackColor = false;
            this.btnAceitarAmizade.Click += new System.EventHandler(this.btnAceitarAmizade_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.SystemColors.ControlLight;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(94, 375);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 15);
            this.label4.TabIndex = 5;
            this.label4.Text = "Digite o email";
            // 
            // txtAceitarNegarAmizade
            // 
            this.txtAceitarNegarAmizade.Location = new System.Drawing.Point(20, 393);
            this.txtAceitarNegarAmizade.Name = "txtAceitarNegarAmizade";
            this.txtAceitarNegarAmizade.Size = new System.Drawing.Size(230, 23);
            this.txtAceitarNegarAmizade.TabIndex = 6;
            // 
            // btnFechar
            // 
            this.btnFechar.BackColor = System.Drawing.Color.Red;
            this.btnFechar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFechar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnFechar.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnFechar.Location = new System.Drawing.Point(645, 22);
            this.btnFechar.Name = "btnFechar";
            this.btnFechar.Size = new System.Drawing.Size(83, 28);
            this.btnFechar.TabIndex = 9;
            this.btnFechar.Text = "Voltar";
            this.btnFechar.UseVisualStyleBackColor = false;
            this.btnFechar.Click += new System.EventHandler(this.btnFechar_Click);
            // 
            // FormAddAmigos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(757, 501);
            this.Controls.Add(this.btnFechar);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.btnEnviarPedidoAmizade);
            this.Controls.Add(this.txtEnviarPedidoAmigo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormAddAmigos";
            this.Text = "Adicionar amigos";
            this.Load += new System.EventHandler(this.FormAddAmigos_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ListBox listBoxUsersDataBase;
        private Panel panel1;
        private Label label1;
        private Label label2;
        private TextBox txtEnviarPedidoAmigo;
        private Button btnEnviarPedidoAmizade;
        private Panel panel2;
        private Label label3;
        private ListBox listBoxPedidosAmizade;
        private Label label4;
        private TextBox txtAceitarNegarAmizade;
        private Button btnAceitarAmizade;
        private Button btnNegarAmizade;
        private Button btnFechar;
    }
}