namespace PDI_Tarea_1
{
    partial class PrincipalPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrincipalPanel));
            this.seleccionarArchivo = new System.Windows.Forms.Button();
            this.ubicacionDelArcgivo = new System.Windows.Forms.Label();
            this.imagenPrevio = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.guardarImagen = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.imagenNegativa = new System.Windows.Forms.Button();
            this.comboBox = new System.Windows.Forms.ComboBox();
            this.rotacionDerecha = new System.Windows.Forms.Button();
            this.rotacionIzquierda = new System.Windows.Forms.Button();
            this.flipVertical = new System.Windows.Forms.Button();
            this.flipHorizontal = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.imagenPrevio)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // seleccionarArchivo
            // 
            resources.ApplyResources(this.seleccionarArchivo, "seleccionarArchivo");
            this.seleccionarArchivo.Name = "seleccionarArchivo";
            this.seleccionarArchivo.UseVisualStyleBackColor = true;
            this.seleccionarArchivo.Click += new System.EventHandler(this.seleccionarArchivo_Click);
            // 
            // ubicacionDelArcgivo
            // 
            resources.ApplyResources(this.ubicacionDelArcgivo, "ubicacionDelArcgivo");
            this.ubicacionDelArcgivo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ubicacionDelArcgivo.Name = "ubicacionDelArcgivo";
            // 
            // imagenPrevio
            // 
            resources.ApplyResources(this.imagenPrevio, "imagenPrevio");
            this.imagenPrevio.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.imagenPrevio.Name = "imagenPrevio";
            this.imagenPrevio.TabStop = false;
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.guardarImagen);
            this.groupBox1.Controls.Add(this.ubicacionDelArcgivo);
            this.groupBox1.Controls.Add(this.seleccionarArchivo);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // guardarImagen
            // 
            resources.ApplyResources(this.guardarImagen, "guardarImagen");
            this.guardarImagen.Name = "guardarImagen";
            this.guardarImagen.UseVisualStyleBackColor = true;
            this.guardarImagen.Click += new System.EventHandler(this.guardarImagen_Click);
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.imagenPrevio);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.imagenNegativa);
            this.groupBox3.Controls.Add(this.comboBox);
            this.groupBox3.Controls.Add(this.rotacionDerecha);
            this.groupBox3.Controls.Add(this.rotacionIzquierda);
            this.groupBox3.Controls.Add(this.flipVertical);
            this.groupBox3.Controls.Add(this.flipHorizontal);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // imagenNegativa
            // 
            resources.ApplyResources(this.imagenNegativa, "imagenNegativa");
            this.imagenNegativa.Name = "imagenNegativa";
            this.imagenNegativa.UseVisualStyleBackColor = true;
            this.imagenNegativa.Click += new System.EventHandler(this.imagenNegativa_Click);
            // 
            // comboBox
            // 
            resources.ApplyResources(this.comboBox, "comboBox");
            this.comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox.FormattingEnabled = true;
            this.comboBox.Items.AddRange(new object[] {
            resources.GetString("comboBox.Items"),
            resources.GetString("comboBox.Items1"),
            resources.GetString("comboBox.Items2")});
            this.comboBox.Name = "comboBox";
            // 
            // rotacionDerecha
            // 
            resources.ApplyResources(this.rotacionDerecha, "rotacionDerecha");
            this.rotacionDerecha.Name = "rotacionDerecha";
            this.rotacionDerecha.UseVisualStyleBackColor = true;
            this.rotacionDerecha.Click += new System.EventHandler(this.rotacionDerecha_Click);
            // 
            // rotacionIzquierda
            // 
            resources.ApplyResources(this.rotacionIzquierda, "rotacionIzquierda");
            this.rotacionIzquierda.Name = "rotacionIzquierda";
            this.rotacionIzquierda.UseVisualStyleBackColor = true;
            this.rotacionIzquierda.Click += new System.EventHandler(this.rotacionIzquierda_Click);
            // 
            // flipVertical
            // 
            resources.ApplyResources(this.flipVertical, "flipVertical");
            this.flipVertical.Name = "flipVertical";
            this.flipVertical.UseVisualStyleBackColor = true;
            this.flipVertical.Click += new System.EventHandler(this.flipHorizontal_Click);
            // 
            // flipHorizontal
            // 
            resources.ApplyResources(this.flipHorizontal, "flipHorizontal");
            this.flipHorizontal.Name = "flipHorizontal";
            this.flipHorizontal.UseVisualStyleBackColor = true;
            this.flipHorizontal.Click += new System.EventHandler(this.flipVertical_Click);
            // 
            // PrincipalPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.Name = "PrincipalPanel";
            ((System.ComponentModel.ISupportInitialize)(this.imagenPrevio)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button seleccionarArchivo;
        private System.Windows.Forms.Label ubicacionDelArcgivo;
        private System.Windows.Forms.PictureBox imagenPrevio;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button guardarImagen;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox comboBox;
        private System.Windows.Forms.Button rotacionDerecha;
        private System.Windows.Forms.Button rotacionIzquierda;
        private System.Windows.Forms.Button flipVertical;
        private System.Windows.Forms.Button flipHorizontal;
        private System.Windows.Forms.Button imagenNegativa;
    }
}

