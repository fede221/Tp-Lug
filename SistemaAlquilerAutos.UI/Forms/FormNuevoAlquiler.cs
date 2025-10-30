using System.Windows.Forms;

namespace SistemaAlquilerAutos.UI.Forms
{
    public partial class FormNuevoAlquiler : Form
    {
        public FormNuevoAlquiler()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Nuevo Alquiler";
            this.Size = new Size(800, 600);

            Label lblInfo = new Label
            {
                Text = "Funcionalidad de Nuevo Alquiler\n(En desarrollo)",
                AutoSize = true,
                Location = new Point(50, 50),
                Font = new Font("Segoe UI", 12)
            };

            this.Controls.Add(lblInfo);
        }
    }
}
