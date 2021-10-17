namespace Trivial
{
    partial class Consultas
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
            this.Contraseña = new System.Windows.Forms.RadioButton();
            this.duracion = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.Preguntar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Contraseña
            // 
            this.Contraseña.AutoSize = true;
            this.Contraseña.BackColor = System.Drawing.Color.Transparent;
            this.Contraseña.ForeColor = System.Drawing.Color.White;
            this.Contraseña.Location = new System.Drawing.Point(144, 71);
            this.Contraseña.Margin = new System.Windows.Forms.Padding(4);
            this.Contraseña.Name = "Contraseña";
            this.Contraseña.Size = new System.Drawing.Size(185, 21);
            this.Contraseña.TabIndex = 9;
            this.Contraseña.TabStop = true;
            this.Contraseña.Text = "¿Cuál es mi contraseña?";
            this.Contraseña.UseVisualStyleBackColor = false;
            // 
            // duracion
            // 
            this.duracion.AutoSize = true;
            this.duracion.BackColor = System.Drawing.Color.Transparent;
            this.duracion.ForeColor = System.Drawing.Color.White;
            this.duracion.Location = new System.Drawing.Point(144, 118);
            this.duracion.Margin = new System.Windows.Forms.Padding(4);
            this.duracion.Name = "duracion";
            this.duracion.Size = new System.Drawing.Size(221, 21);
            this.duracion.TabIndex = 10;
            this.duracion.TabStop = true;
            this.duracion.Text = "¿Cuál es la partida más larga?";
            this.duracion.UseVisualStyleBackColor = false;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.BackColor = System.Drawing.Color.Transparent;
            this.radioButton2.ForeColor = System.Drawing.Color.White;
            this.radioButton2.Location = new System.Drawing.Point(144, 161);
            this.radioButton2.Margin = new System.Windows.Forms.Padding(4);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(273, 21);
            this.radioButton2.TabIndex = 11;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "¿Quién es el jugador con más puntos?";
            this.radioButton2.UseVisualStyleBackColor = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(61, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(185, 20);
            this.label4.TabIndex = 12;
            this.label4.Text = "¿Qué quieres saber?";
            // 
            // Preguntar
            // 
            this.Preguntar.BackColor = System.Drawing.Color.Black;
            this.Preguntar.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Preguntar.ForeColor = System.Drawing.Color.White;
            this.Preguntar.Location = new System.Drawing.Point(194, 225);
            this.Preguntar.Name = "Preguntar";
            this.Preguntar.Size = new System.Drawing.Size(135, 47);
            this.Preguntar.TabIndex = 13;
            this.Preguntar.Text = "Preguntar";
            this.Preguntar.UseVisualStyleBackColor = false;
            this.Preguntar.Click += new System.EventHandler(this.Registrar_Click);
            // 
            // Consultas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(658, 361);
            this.Controls.Add(this.Preguntar);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.duracion);
            this.Controls.Add(this.Contraseña);
            this.Name = "Consultas";
            this.Text = "Consultas";
            this.Load += new System.EventHandler(this.Consultas_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton Contraseña;
        private System.Windows.Forms.RadioButton duracion;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button Preguntar;
    }
}