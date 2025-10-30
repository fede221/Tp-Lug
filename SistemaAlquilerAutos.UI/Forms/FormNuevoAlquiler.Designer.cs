namespace SistemaAlquilerAutos.UI.Forms
{
    partial class FormNuevoAlquiler
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = "Nuevo Alquiler";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterParent;

            Label lblInfo = new Label
            {
                Text = "Funcionalidad de Nuevo Alquiler\n(En desarrollo)\n\nNota: Requiere conexión a base de datos configurada",
                AutoSize = true,
                Location = new Point(50, 50),
                Font = new Font("Segoe UI", 12)
            };

            this.Controls.Add(lblInfo);

            this.ResumeLayout(false);
        }
    }
}
