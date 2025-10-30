using System;
using System.Windows.Forms;
using SistemaAlquilerAutos.BLL;
using SistemaAlquilerAutos.BLL.Exceptions;

namespace SistemaAlquilerAutos.UI.Forms
{
    public partial class FormVehiculos : Form
    {
        private readonly VehiculoBLL _vehiculoBLL;
        private DataGridView dgvVehiculos;

        public FormVehiculos()
        {
            _vehiculoBLL = new VehiculoBLL();
            InitializeComponent();
            CargarVehiculos();
        }

        private void InitializeComponent()
        {
            this.Text = "Gestión de Vehículos";
            this.Size = new Size(900, 600);

            dgvVehiculos = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            this.Controls.Add(dgvVehiculos);
        }

        private void CargarVehiculos()
        {
            try
            {
                var vehiculos = _vehiculoBLL.GetAll();
                dgvVehiculos.DataSource = vehiculos;
            }
            catch (BusinessException ex)
            {
                MessageBox.Show($"Error al cargar vehículos:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
