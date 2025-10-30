using System;
using System.Windows.Forms;
using SistemaAlquilerAutos.BLL;
using SistemaAlquilerAutos.BLL.Exceptions;

namespace SistemaAlquilerAutos.UI.Forms
{
    public partial class FormCategorias : Form
    {
        private readonly CategoriaBLL _categoriaBLL;
        private DataGridView dgvCategorias;

        public FormCategorias()
        {
            _categoriaBLL = new CategoriaBLL();
            InitializeComponent();
            CargarCategorias();
        }

        private void InitializeComponent()
        {
            this.Text = "Gestión de Categorías";
            this.Size = new Size(800, 500);

            dgvCategorias = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            this.Controls.Add(dgvCategorias);
        }

        private void CargarCategorias()
        {
            try
            {
                var categorias = _categoriaBLL.GetAll();
                dgvCategorias.DataSource = categorias;
            }
            catch (BusinessException ex)
            {
                MessageBox.Show($"Error al cargar categorías:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
