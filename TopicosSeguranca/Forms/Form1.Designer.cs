namespace TopicosSeguranca
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.lstMensagens = new System.Windows.Forms.ListBox();
            this.lstUsers = new System.Windows.Forms.ListBox();
            this.btnConectar = new System.Windows.Forms.Button();
            this.txtChat = new System.Windows.Forms.TextBox();
            this.btnEnviar = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.txtPesquisa = new System.Windows.Forms.TextBox();
            this.btnPesquisar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lstMensagens
            // 
            this.lstMensagens.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lstMensagens.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstMensagens.Font = new System.Drawing.Font("Arial", 11F);
            this.lstMensagens.ForeColor = System.Drawing.SystemColors.Window;
            this.lstMensagens.FormattingEnabled = true;
            this.lstMensagens.ItemHeight = 21;
            this.lstMensagens.Location = new System.Drawing.Point(343, 79);
            this.lstMensagens.Name = "lstMensagens";
            this.lstMensagens.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.lstMensagens.Size = new System.Drawing.Size(716, 443);
            this.lstMensagens.TabIndex = 0;
            // 
            // lstUsers
            // 
            this.lstUsers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lstUsers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstUsers.Font = new System.Drawing.Font("Arial", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstUsers.ForeColor = System.Drawing.SystemColors.Window;
            this.lstUsers.FormattingEnabled = true;
            this.lstUsers.ItemHeight = 21;
            this.lstUsers.Location = new System.Drawing.Point(31, 169);
            this.lstUsers.Name = "lstUsers";
            this.lstUsers.Size = new System.Drawing.Size(273, 422);
            this.lstUsers.TabIndex = 1;
            // 
            // btnConectar
            // 
            this.btnConectar.ForeColor = System.Drawing.SystemColors.WindowText;
            this.btnConectar.Location = new System.Drawing.Point(32, 79);
            this.btnConectar.Name = "btnConectar";
            this.btnConectar.Size = new System.Drawing.Size(273, 27);
            this.btnConectar.TabIndex = 2;
            this.btnConectar.Text = "Conectar";
            this.btnConectar.UseVisualStyleBackColor = true;
            this.btnConectar.Click += new System.EventHandler(this.btnConectar_Click);
            // 
            // txtChat
            // 
            this.txtChat.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtChat.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtChat.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtChat.Font = new System.Drawing.Font("Arial", 11F);
            this.txtChat.ForeColor = System.Drawing.SystemColors.Window;
            this.txtChat.Location = new System.Drawing.Point(343, 550);
            this.txtChat.Multiline = true;
            this.txtChat.Name = "txtChat";
            this.txtChat.Size = new System.Drawing.Size(650, 41);
            this.txtChat.TabIndex = 3;
            this.txtChat.Text = "Write your message";
            this.txtChat.TextChanged += new System.EventHandler(this.txtChat_TextChanged);
            // 
            // btnEnviar
            // 
            this.btnEnviar.BackColor = System.Drawing.SystemColors.Window;
            this.btnEnviar.Font = new System.Drawing.Font("Microsoft Sans Serif", 17F);
            this.btnEnviar.ForeColor = System.Drawing.SystemColors.WindowText;
            this.btnEnviar.Location = new System.Drawing.Point(1009, 550);
            this.btnEnviar.Name = "btnEnviar";
            this.btnEnviar.Size = new System.Drawing.Size(50, 41);
            this.btnEnviar.TabIndex = 4;
            this.btnEnviar.Text = "➜";
            this.btnEnviar.UseVisualStyleBackColor = false;
            this.btnEnviar.Click += new System.EventHandler(this.btnEnviar_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Arial", 26F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(87, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(230, 51);
            this.label1.TabIndex = 5;
            this.label1.Text = "Messages";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(32, 18);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(50, 50);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // txtPesquisa
            // 
            this.txtPesquisa.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtPesquisa.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPesquisa.ForeColor = System.Drawing.SystemColors.Window;
            this.txtPesquisa.Location = new System.Drawing.Point(32, 123);
            this.txtPesquisa.Multiline = true;
            this.txtPesquisa.Name = "txtPesquisa";
            this.txtPesquisa.Size = new System.Drawing.Size(228, 31);
            this.txtPesquisa.TabIndex = 7;
            // 
            // btnPesquisar
            // 
            this.btnPesquisar.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.btnPesquisar.ForeColor = System.Drawing.Color.Black;
            this.btnPesquisar.Location = new System.Drawing.Point(266, 123);
            this.btnPesquisar.Name = "btnPesquisar";
            this.btnPesquisar.Size = new System.Drawing.Size(38, 31);
            this.btnPesquisar.TabIndex = 8;
            this.btnPesquisar.Text = "⌕";
            this.btnPesquisar.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(1102, 613);
            this.Controls.Add(this.btnPesquisar);
            this.Controls.Add(this.txtPesquisa);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnEnviar);
            this.Controls.Add(this.txtChat);
            this.Controls.Add(this.btnConectar);
            this.Controls.Add(this.lstUsers);
            this.Controls.Add(this.lstMensagens);
            this.ForeColor = System.Drawing.SystemColors.Window;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Messages";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstMensagens;
        private System.Windows.Forms.ListBox lstUsers;
        private System.Windows.Forms.Button btnConectar;
        private System.Windows.Forms.TextBox txtChat;
        private System.Windows.Forms.Button btnEnviar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox txtPesquisa;
        private System.Windows.Forms.Button btnPesquisar;
    }
}

