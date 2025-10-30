using System;
using System.Windows.Forms;
using SistemaAlquilerAutos.BLL;
using SistemaAlquilerAutos.BLL.Exceptions;
using SistemaAlquilerAutos.Entity;

namespace SistemaAlquilerAutos.UI.Forms
{
    public partial class FormEditarCliente : Form
    {
        private readonly ClienteBLL _clienteBLL;
        private readonly Cliente? _cliente;
        private bool _esNuevo;

        private TextBox txtDNI, txtNombre, txtApellido, txtEmail, txtTelefono, txtDireccion, txtCiudad;
        private DateTimePicker dtpFechaNacimiento;
        private Button btnGuardar, btnCancelar;

        public FormEditarCliente(Cliente? cliente = null)
        {
            _clienteBLL = new ClienteBLL();
            _cliente = cliente;
            _esNuevo = cliente == null;

            InitializeComponent();

            if (_cliente != null)
            {
                CargarDatos();
            }
        }

        private void InitializeComponent()
        {
            this.Text = _esNuevo ? "Nuevo Cliente" : "Editar Cliente";
            this.Size = new Size(500, 450);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            int y = 20;
            int labelX = 20;
            int controlX = 150;
            int controlWidth = 300;

            // DNI
            Label lblDNI = new Label { Text = "DNI:", Location = new Point(labelX, y), AutoSize = true };
            txtDNI = new TextBox { Location = new Point(controlX, y - 3), Width = controlWidth };
            this.Controls.Add(lblDNI);
            this.Controls.Add(txtDNI);
            y += 35;

            // Nombre
            Label lblNombre = new Label { Text = "Nombre:", Location = new Point(labelX, y), AutoSize = true };
            txtNombre = new TextBox { Location = new Point(controlX, y - 3), Width = controlWidth };
            this.Controls.Add(lblNombre);
            this.Controls.Add(txtNombre);
            y += 35;

            // Apellido
            Label lblApellido = new Label { Text = "Apellido:", Location = new Point(labelX, y), AutoSize = true };
            txtApellido = new TextBox { Location = new Point(controlX, y - 3), Width = controlWidth };
            this.Controls.Add(lblApellido);
            this.Controls.Add(txtApellido);
            y += 35;

            // Fecha Nacimiento
            Label lblFechaNac = new Label { Text = "Fecha Nacimiento:", Location = new Point(labelX, y), AutoSize = true };
            dtpFechaNacimiento = new DateTimePicker
            {
                Location = new Point(controlX, y - 3),
                Width = controlWidth,
                Format = DateTimePickerFormat.Short
            };
            dtpFechaNacimiento.Value = DateTime.Today.AddYears(-25);
            this.Controls.Add(lblFechaNac);
            this.Controls.Add(dtpFechaNacimiento);
            y += 35;

            // Email
            Label lblEmail = new Label { Text = "Email:", Location = new Point(labelX, y), AutoSize = true };
            txtEmail = new TextBox { Location = new Point(controlX, y - 3), Width = controlWidth };
            this.Controls.Add(lblEmail);
            this.Controls.Add(txtEmail);
            y += 35;

            // Teléfono
            Label lblTelefono = new Label { Text = "Teléfono:", Location = new Point(labelX, y), AutoSize = true };
            txtTelefono = new TextBox { Location = new Point(controlX, y - 3), Width = controlWidth };
            this.Controls.Add(lblTelefono);
            this.Controls.Add(txtTelefono);
            y += 35;

            // Dirección
            Label lblDireccion = new Label { Text = "Dirección:", Location = new Point(labelX, y), AutoSize = true };
            txtDireccion = new TextBox { Location = new Point(controlX, y - 3), Width = controlWidth };
            this.Controls.Add(lblDireccion);
            this.Controls.Add(txtDireccion);
            y += 35;

            // Ciudad
            Label lblCiudad = new Label { Text = "Ciudad:", Location = new Point(labelX, y), AutoSize = true };
            txtCiudad = new TextBox { Location = new Point(controlX, y - 3), Width = controlWidth };
            this.Controls.Add(lblCiudad);
            this.Controls.Add(txtCiudad);
            y += 50;

            // Botones
            btnGuardar = new Button
            {
                Text = "Guardar",
                Location = new Point(controlX, y),
                Width = 100,
                DialogResult = DialogResult.None
            };
            btnGuardar.Click += BtnGuardar_Click;

            btnCancelar = new Button
            {
                Text = "Cancelar",
                Location = new Point(controlX + 110, y),
                Width = 100,
                DialogResult = DialogResult.Cancel
            };

            this.Controls.Add(btnGuardar);
            this.Controls.Add(btnCancelar);
            this.CancelButton = btnCancelar;
        }

        private void CargarDatos()
        {
            if (_cliente == null) return;

            txtDNI.Text = _cliente.DNI;
            txtNombre.Text = _cliente.Nombre;
            txtApellido.Text = _cliente.Apellido;
            txtEmail.Text = _cliente.Email;
            txtTelefono.Text = _cliente.Telefono;
            txtDireccion.Text = _cliente.Direccion;
            txtCiudad.Text = _cliente.Ciudad;
            dtpFechaNacimiento.Value = _cliente.FechaNacimiento;
        }

        private void BtnGuardar_Click(object? sender, EventArgs e)
        {
            try
            {
                var cliente = _cliente ?? new Cliente();

                cliente.DNI = txtDNI.Text.Trim();
                cliente.Nombre = txtNombre.Text.Trim();
                cliente.Apellido = txtApellido.Text.Trim();
                cliente.Email = txtEmail.Text.Trim();
                cliente.Telefono = txtTelefono.Text.Trim();
                cliente.Direccion = txtDireccion.Text.Trim();
                cliente.Ciudad = txtCiudad.Text.Trim();
                cliente.FechaNacimiento = dtpFechaNacimiento.Value;

                if (_esNuevo)
                {
                    _clienteBLL.Insert(cliente);
                    MessageBox.Show("Cliente creado correctamente.",
                        "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    _clienteBLL.Update(cliente);
                    MessageBox.Show("Cliente actualizado correctamente.",
                        "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (BusinessRuleException ex)
            {
                MessageBox.Show($"Error de validación:\n{ex.Message}",
                    "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (BusinessException ex)
            {
                MessageBox.Show($"Error al guardar cliente:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
