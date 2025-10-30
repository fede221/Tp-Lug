using System;
using System.Windows.Forms;
using SistemaAlquilerAutos.BLL;
using SistemaAlquilerAutos.BLL.Exceptions;

namespace SistemaAlquilerAutos.UI.Forms
{
    public partial class FormHistorialAlquileres : Form
    {
        private readonly AlquilerBLL _alquilerBLL;
        private DataGridView dgvAlquileres;

        public FormHistorialAlquileres()
        {
            _alquilerBLL = new AlquilerBLL();
            InitializeComponent();
            CargarHistorial();
        }

        private void InitializeComponent()
        {
            this.Text = "Historial de Alquileres";
            this.Size = new Size(1000, 600);

            dgvAlquileres = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            this.Controls.Add(dgvAlquileres);
        }

        private void CargarHistorial()
        {
            try
            {
                var alquileres = _alquilerBLL.GetAll();
                dgvAlquileres.DataSource = alquileres;
            }
            catch (BusinessException ex)
            {
                MessageBox.Show($"Error al cargar historial:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
