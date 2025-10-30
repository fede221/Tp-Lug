namespace SistemaAlquilerAutos.UI.Forms
{
    partial class FormEditarCliente
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

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

            this.ResumeLayout(false);
        }
    }
}
