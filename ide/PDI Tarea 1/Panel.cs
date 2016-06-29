using System.Windows.Forms;
using System;

namespace PDI_Tarea_1{
    public partial class PrincipalPanel : Form{

        private BMP bmp;

        public PrincipalPanel(){
            InitializeComponent();
            this.comboBox.SelectedIndex = 0;
            bmp = new BMP();
        }

        private void rotacionIzquierda_Click(object sender, EventArgs e){
           if(bmp.ImageIsLoaded()) {
                // Apply acction.
                if(comboBox.SelectedIndex == 1)
                    bmp.Girar180Grados();
                if(comboBox.SelectedIndex == 0)
                    bmp.Girar90GradosIzquierda();
				if(comboBox.SelectedIndex == 2)
					bmp.Girar90GradosDerecha();
                // Get the imagen file and show it on the box.
                this.imagenPrevio.Image = bmp.GetImage();
            } else {
                // Show error message.
                MessageBox.Show("Para aplicar esta accion, primero debe abrir una imagen.", "Error lellendo imagen", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void rotacionDerecha_Click(object sender, EventArgs e){
            if(bmp.ImageIsLoaded()) {
                // Apply acction.
                if(comboBox.SelectedIndex == 1)
                    bmp.Girar180Grados();
				if(comboBox.SelectedIndex == 0)
					bmp.Girar90GradosDerecha();
				if(comboBox.SelectedIndex == 2)
					bmp.Girar90GradosIzquierda();
				// Get the imagen file and show it on the box.
				this.imagenPrevio.Image = bmp.GetImage();
            } else {
                // Show error message.
                MessageBox.Show("Para aplicar esta accion, primero debe abrir una imagen.", "Error lellendo imagen", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void imagenNegativa_Click(object sender, EventArgs e){
            if (bmp.ImageIsLoaded()){
                // Apply acction.
                bmp.InvertirColores();
                // Get the imagen file and show it on the box.
                this.imagenPrevio.Image = bmp.GetImage();
            }else{
                // Show error message.
                MessageBox.Show("Para aplicar esta accion, primero debe abrir una imagen.", "Error lellendo imagen", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void guardarImagen_Click(object sender, EventArgs e){
            if (bmp.ImageIsLoaded()){
                // Save new file.
                bmp.SaveImageFile();
                MessageBox.Show("La imagen se guardo exitosamente!!!", "Archivo guardado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }else{
                // Show error message.
                MessageBox.Show("Para guardar, primero debe abrir una imagen.", "Error guardando archivo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void seleccionarArchivo_Click(object sender, EventArgs e){
            // Create an instance of the open file dialog box.
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Set filter options and filter index.
            openFileDialog.Filter = "Windows Bitmap (.bmp)|*.bmp|All Files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;

            openFileDialog.Multiselect = false;

            // Call the ShowDialog method to show the dialog box.
            bool? userClickedOK = openFileDialog.ShowDialog().ToString().Equals("OK");

            // Process input if the user clicked OK.
            if (userClickedOK == true){
                // Read BMP file to edit.
                bmp.LoadImageFile(openFileDialog.FileName);
                if (bmp.ImageIsLoaded()){
                    // Get the file patch and set this to user viwe.
                    this.ubicacionDelArcgivo.Text = openFileDialog.FileName;

                    // Get the imagen file and show it on the box.
                    this.imagenPrevio.Image = bmp.GetImage();
                }else{
                    // Clean Data.
                    this.imagenPrevio.Image = null;
                    this.ubicacionDelArcgivo.Text = "";

                    // Show message.
                    MessageBox.Show("Esta imagen no cumple con las condiciones necesarias para ser abierta.", "Error abriendo archivo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            GC.Collect();
        }

        private void flipVertical_Click(object sender, EventArgs e){
			if (bmp.ImageIsLoaded()){
                // Apply acction.
                bmp.FlipVertical();
                // Get the imagen file and show it on the box.
                this.imagenPrevio.Image = bmp.GetImage();
            }
            else
            {
                // Show error message.
                MessageBox.Show("Para aplicar esta accion, primero debe abrir una imagen.", "Error de imagen", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
		}

        private void flipHorizontal_Click(object sender, EventArgs e){
            if(bmp.ImageIsLoaded()) {
                // Apply acction.
                bmp.FlipHorizontal();
                // Get the imagen file and show it on the box.
                this.imagenPrevio.Image = bmp.GetImage();
            } else {
                // Show error message.
                MessageBox.Show("Para aplicar esta accion, primero debe abrir una imagen.", "Error de imagen", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
