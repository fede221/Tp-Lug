using System;
using System.Windows.Forms;
using SistemaAlquilerAutos.BLL;
using SistemaAlquilerAutos.BLL.Exceptions;

namespace SistemaAlquilerAutos.UI.Forms
{
    public partial class FormSucursales : Form
    {
        private readonly SucursalBLL _sucursalBLL;
        private DataGridView dgvSucursales;

        public FormSucursales()
        {
            _sucursalBLL = new SucursalBLL();
            InitializeComponent();
            CargarSucursales();
        }

        private void InitializeComponent()
        {
            this.Text = "Gestión de Sucursales";
            this.Size = new Size(800, 500);

            dgvSucursales = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            this.Controls.Add(dgvSucursales);
        }

        private void CargarSucursales()
        {
            try
            {
                var sucursales = _sucursalBLL.GetAll();
                dgvSucursales.DataSource = sucursales;
            }
            catch (BusinessException ex)
            {
                MessageBox.Show($"Error al cargar sucursales:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
