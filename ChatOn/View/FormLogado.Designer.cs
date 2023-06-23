namespace ChatOn.View
{
    partial class FormLogado
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLogado));
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtChat = new System.Windows.Forms.RichTextBox();
            this.btnEnviarMsg = new System.Windows.Forms.Button();
            this.txtMsgDigitada = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panelAmigos = new System.Windows.Forms.FlowLayoutPanel();
            this.listBoxAmigos = new System.Windows.Forms.ListBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblChatComQuem = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.Controls.Add(this.txtChat);
            this.panel1.Controls.Add(this.btnEnviarMsg);
            this.panel1.Controls.Add(this.txtMsgDigitada);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(537, 477);
            this.panel1.TabIndex = 0;
            // 
            // txtChat
            // 
            this.txtChat.Location = new System.Drawing.Point(10, 39);
            this.txtChat.Name = "txtChat";
            this.txtChat.ReadOnly = true;
            this.txtChat.Size = new System.Drawing.Size(522, 367);
            this.txtChat.TabIndex = 2;
            this.txtChat.Text = "";
            // 
            // btnEnviarMsg
            // 
            this.btnEnviarMsg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnEnviarMsg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnviarMsg.Image = ((System.Drawing.Image)(resources.GetObject("btnEnviarMsg.Image")));
            this.btnEnviarMsg.Location = new System.Drawing.Point(455, 412);
            this.btnEnviarMsg.Name = "btnEnviarMsg";
            this.btnEnviarMsg.Size = new System.Drawing.Size(77, 58);
            this.btnEnviarMsg.TabIndex = 1;
            this.btnEnviarMsg.UseVisualStyleBackColor = true;
            this.btnEnviarMsg.Click += new System.EventHandler(this.btnEnviarMsg_Click);
            // 
            // txtMsgDigitada
            // 
            this.txtMsgDigitada.Location = new System.Drawing.Point(10, 412);
            this.txtMsgDigitada.Multiline = true;
            this.txtMsgDigitada.Name = "txtMsgDigitada";
            this.txtMsgDigitada.PlaceholderText = "Digite alguma coisa aqui...";
            this.txtMsgDigitada.Size = new System.Drawing.Size(441, 57);
            this.txtMsgDigitada.TabIndex = 0;
            this.txtMsgDigitada.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMsgDigitada_KeyPress);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel2.Controls.Add(this.panelAmigos);
            this.panel2.Controls.Add(this.listBoxAmigos);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Location = new System.Drawing.Point(555, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(212, 477);
            this.panel2.TabIndex = 1;
            // 
            // panelAmigos
            // 
            this.panelAmigos.BackColor = System.Drawing.Color.LightGray;
            this.panelAmigos.Location = new System.Drawing.Point(14, 53);
            this.panelAmigos.Name = "panelAmigos";
            this.panelAmigos.Size = new System.Drawing.Size(181, 407);
            this.panelAmigos.TabIndex = 5;
            // 
            // listBoxAmigos
            // 
            this.listBoxAmigos.FormattingEnabled = true;
            this.listBoxAmigos.ItemHeight = 15;
            this.listBoxAmigos.Location = new System.Drawing.Point(14, 306);
            this.listBoxAmigos.Name = "listBoxAmigos";
            this.listBoxAmigos.Size = new System.Drawing.Size(181, 154);
            this.listBoxAmigos.TabIndex = 4;
            this.listBoxAmigos.Visible = false;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.DarkGray;
            this.panel4.Controls.Add(this.label1);
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(212, 36);
            this.panel4.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(212, 36);
            this.label1.TabIndex = 0;
            this.label1.Text = "Amigos";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.DarkGray;
            this.panel3.Controls.Add(this.lblChatComQuem);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Location = new System.Drawing.Point(12, 12);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(537, 36);
            this.panel3.TabIndex = 2;
            // 
            // lblChatComQuem
            // 
            this.lblChatComQuem.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblChatComQuem.Location = new System.Drawing.Point(132, 0);
            this.lblChatComQuem.Name = "lblChatComQuem";
            this.lblChatComQuem.Size = new System.Drawing.Size(218, 36);
            this.lblChatComQuem.TabIndex = 2;
            this.lblChatComQuem.Text = "nenhum usuário selecionado";
            this.lblChatComQuem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(141, 36);
            this.label2.TabIndex = 1;
            this.label2.Text = "Conversando com:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FormLogado
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(206)))), ((int)(((byte)(206)))));
            this.ClientSize = new System.Drawing.Size(791, 501);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormLogado";
            this.Text = "Logado";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private Button btnEnviarMsg;
        private TextBox txtMsgDigitada;
        private Panel panel4;
        private Panel panel3;
        private Label label1;
        private Label label2;
        private Label lblChatComQuem;
        private ListBox listBoxAmigos;
        private FlowLayoutPanel panelAmigos;
        private RichTextBox txtChat;
    }
}