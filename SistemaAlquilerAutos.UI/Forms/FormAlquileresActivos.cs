using System;
using System.Windows.Forms;
using SistemaAlquilerAutos.BLL;
using SistemaAlquilerAutos.BLL.Exceptions;

namespace SistemaAlquilerAutos.UI.Forms
{
    public partial class FormAlquileresActivos : Form
    {
        private readonly AlquilerBLL _alquilerBLL;
        private DataGridView dgvAlquileres;

        public FormAlquileresActivos()
        {
            _alquilerBLL = new AlquilerBLL();
            InitializeComponent();
            CargarAlquileresActivos();
        }

        private void InitializeComponent()
        {
            this.Text = "Alquileres Activos";
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

        private void CargarAlquileresActivos()
        {
            try
            {
                var alquileres = _alquilerBLL.GetActivos();
                dgvAlquileres.DataSource = alquileres;
            }
            catch (BusinessException ex)
            {
                MessageBox.Show($"Error al cargar alquileres:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
