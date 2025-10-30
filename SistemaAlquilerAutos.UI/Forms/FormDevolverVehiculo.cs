using System.Windows.Forms;

namespace SistemaAlquilerAutos.UI.Forms
{
    public partial class FormDevolverVehiculo : Form
    {
        public FormDevolverVehiculo()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Devolver Vehículo";
            this.Size = new Size(700, 500);

            Label lblInfo = new Label
            {
                Text = "Funcionalidad de Devolución de Vehículo\n(En desarrollo)",
                AutoSize = true,
                Location = new Point(50, 50),
                Font = new Font("Segoe UI", 12)
            };

            this.Controls.Add(lblInfo);
        }
    }
}
