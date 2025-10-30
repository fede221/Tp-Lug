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
            try
            {
                InitializeComponent();
                _clienteBLL = new ClienteBLL();
                this.Load += FormClientes_Load;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inicializar formulario:\n{ex.Message}\n\n{ex.StackTrace}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormClientes_Load(object? sender, EventArgs e)
        {
            try
            {
                CargarClientes();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos:\n{ex.Message}\n\nInner: {ex.InnerException?.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar clientes:\n{ex.Message}\n\nDetalles:\n{ex.InnerException?.Message}\n\n{ex.StackTrace}",
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
            catch (Exception ex)
            {
                MessageBox.Show($"Error en la búsqueda:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnNuevo_Click(object? sender, EventArgs e)
        {
            try
            {
                var formEditar = new FormEditarCliente();
                if (formEditar.ShowDialog() == DialogResult.OK)
                {
                    CargarClientes();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir formulario:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEditar_Click(object? sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show($"Error al editar:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEliminar_Click(object? sender, EventArgs e)
        {
            try
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
                    _clienteBLL.Delete(cliente.Id);
                    MessageBox.Show("Cliente eliminado correctamente.",
                        "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarClientes();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar cliente:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
