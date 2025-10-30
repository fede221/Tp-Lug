using System;
using System.Windows.Forms;
using SistemaAlquilerAutos.BLL;
using SistemaAlquilerAutos.BLL.Exceptions;
using SistemaAlquilerAutos.Entity;

namespace SistemaAlquilerAutos.UI.Forms
{
    public partial class FormClientes : Form
    {
        private readonly ClienteBLL _clienteBLL;
        private DataGridView dgvClientes;
        private TextBox txtBuscar;
        private Button btnNuevo, btnEditar, btnEliminar, btnBuscar;

        public FormClientes()
        {
            _clienteBLL = new ClienteBLL();
            InitializeComponent();
            CargarClientes();
        }

        private void InitializeComponent()
        {
            this.Text = "Gestión de Clientes";
            this.Size = new Size(900, 600);

            // Panel superior con controles
            Panel panelSuperior = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                Padding = new Padding(10)
            };

            Label lblBuscar = new Label
            {
                Text = "Buscar:",
                Location = new Point(10, 18),
                AutoSize = true
            };

            txtBuscar = new TextBox
            {
                Location = new Point(70, 15),
                Width = 250
            };

            btnBuscar = new Button
            {
                Text = "Buscar",
                Location = new Point(330, 14),
                Width = 80
            };
            btnBuscar.Click += BtnBuscar_Click;

            btnNuevo = new Button
            {
                Text = "Nuevo Cliente",
                Location = new Point(450, 14),
                Width = 120
            };
            btnNuevo.Click += BtnNuevo_Click;

            btnEditar = new Button
            {
                Text = "Editar",
                Location = new Point(580, 14),
                Width = 80
            };
            btnEditar.Click += BtnEditar_Click;

            btnEliminar = new Button
            {
                Text = "Eliminar",
                Location = new Point(670, 14),
                Width = 80
            };
            btnEliminar.Click += BtnEliminar_Click;

            panelSuperior.Controls.Add(lblBuscar);
            panelSuperior.Controls.Add(txtBuscar);
            panelSuperior.Controls.Add(btnBuscar);
            panelSuperior.Controls.Add(btnNuevo);
            panelSuperior.Controls.Add(btnEditar);
            panelSuperior.Controls.Add(btnEliminar);

            // DataGridView
            dgvClientes = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            this.Controls.Add(dgvClientes);
            this.Controls.Add(panelSuperior);
        }

        private void CargarClientes()
        {
            try
            {
                var clientes = _clienteBLL.GetAll();
                dgvClientes.DataSource = clientes;

                // Ocultar columnas no necesarias
                if (dgvClientes.Columns["Activo"] != null)
                    dgvClientes.Columns["Activo"].Visible = false;

                // Configurar headers
                if (dgvClientes.Columns["NombreCompleto"] != null)
                    dgvClientes.Columns["NombreCompleto"].HeaderText = "Nombre Completo";
                if (dgvClientes.Columns["FechaNacimiento"] != null)
                    dgvClientes.Columns["FechaNacimiento"].HeaderText = "Fecha Nacimiento";
            }
            catch (BusinessException ex)
            {
                MessageBox.Show($"Error al cargar clientes:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnBuscar_Click(object? sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtBuscar.Text))
                {
                    CargarClientes();
                }
                else
                {
                    var clientes = _clienteBLL.Search(txtBuscar.Text);
                    dgvClientes.DataSource = clientes;
                }
            }
            catch (BusinessException ex)
            {
                MessageBox.Show($"Error en la búsqueda:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnNuevo_Click(object? sender, EventArgs e)
        {
            var formEditar = new FormEditarCliente();
            if (formEditar.ShowDialog() == DialogResult.OK)
            {
                CargarClientes();
            }
        }

        private void BtnEditar_Click(object? sender, EventArgs e)
        {
            if (dgvClientes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un cliente para editar.",
                    "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var cliente = (Cliente)dgvClientes.SelectedRows[0].DataBoundItem;
            var formEditar = new FormEditarCliente(cliente);
            if (formEditar.ShowDialog() == DialogResult.OK)
            {
                CargarClientes();
            }
        }

        private void BtnEliminar_Click(object? sender, EventArgs e)
        {
            if (dgvClientes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un cliente para eliminar.",
                    "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var cliente = (Cliente)dgvClientes.SelectedRows[0].DataBoundItem;

            var result = MessageBox.Show(
                $"¿Está seguro que desea eliminar al cliente {cliente.NombreCompleto}?",
                "Confirmar Eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    _clienteBLL.Delete(cliente.Id);
                    MessageBox.Show("Cliente eliminado correctamente.",
                        "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarClientes();
                }
                catch (BusinessException ex)
                {
                    MessageBox.Show($"Error al eliminar cliente:\n{ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
