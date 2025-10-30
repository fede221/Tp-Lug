namespace SistemaAlquilerAutos.UI.Forms
{
    partial class FormDevolverVehiculo
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

            this.Text = "Devolver Vehículo";
            this.Size = new Size(700, 500);
            this.StartPosition = FormStartPosition.CenterParent;

            Label lblInfo = new Label
            {
                Text = "Funcionalidad de Devolución de Vehículo\n(En desarrollo)",
                AutoSize = true,
                Location = new Point(50, 50),
                Font = new Font("Segoe UI", 12)
            };

            this.Controls.Add(lblInfo);

            this.ResumeLayout(false);
        }
    }
}
